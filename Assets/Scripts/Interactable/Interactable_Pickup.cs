using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable_Pickup : MonoBehaviour
{
    public static event Action Action_PickupGot;
    public static event Action Action_EssencePickupGot;
    public static event Action Action_StepsUpPickupGot;
    public static event Action Action_SkinPickupGot;
    public static event Action Action_AbilityPickupGot;

    public static event Action Action_SnorkelGot;
    public static event Action Action_OxygenTankGot;
    public static event Action Action_JumpingGot;


    MapManager mapManager;

    public SkinType skinReceived;
    public Items itemReceived;
    public Abilities abilityReceived;
    public bool goal;

    Vector3 startPos;
    RaycastHit hit;


    //--------------------


    private void Start()
    {
        startPos = transform.position;

        mapManager = FindObjectOfType<MapManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            //PlayerManager.Instance.PauseGame();
            //print("Pickup entered");

            GetItems();
            GetAbility();

            PlayerManager.Instance.SavePlayerStats();
            MapManager.Instance.SaveMapInfo();

            if (goal)
            {
                PlayerManager.Instance.PauseGame();

                StartCoroutine(ExitLevel(0.5f));
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    IEnumerator ExitLevel(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //Update analyticsData
        AnalyticsCalls.OnLevel(mapManager.timeUsedInLevel, mapManager.stepCount, mapManager.respawnCount, mapManager.abilitiesPickedUp, mapManager.cameraRotated, mapManager.swimCounter, mapManager.swiftSwimCounter, mapManager.jumpCounter, mapManager.dashCounter, mapManager.ascendCounter, mapManager.descendCounter, mapManager.grapplingHookCounter, mapManager.ceilingGrabCounter);
        AnalyticsCalls.OnLevelFinishing(mapManager.timeUsedInLevel, mapManager.stepCount, mapManager.respawnCount, mapManager.abilitiesPickedUp, mapManager.cameraRotated, mapManager.swimCounter, mapManager.swiftSwimCounter, mapManager.jumpCounter, mapManager.dashCounter, mapManager.ascendCounter, mapManager.descendCounter, mapManager.grapplingHookCounter, mapManager.ceilingGrabCounter);

        AnalyticsCalls.CompleteLevel();

        PlayerManager.Instance.player.GetComponent<PlayerManager>().QuitLevel();

        gameObject.SetActive(false);
    }


    //--------------------


    public void ShowPickup()
    {
        gameObject.gameObject.SetActive(true);
    }

    public void GetItems()
    {
        if (PlayerManager.Instance.player)
        {
            if (PlayerManager.Instance.player.GetComponent<PlayerStats>())
            {
                if (PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.itemsGot != null)
                {
                    switch (itemReceived)
                    {
                        case Items.None:
                            break;

                        case Items.Essence:
                            PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.itemsGot.essence_Max += 1;
                            PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.itemsGot.essence_Current += 1;

                            //Get coin number
                            for (int i = 0; i < MapManager.Instance.mapInfo_ToSave.essenceList.Count; i++)
                            {
                                if (Vector3.Distance(MapManager.Instance.mapInfo_ToSave.essenceList[i].pos, PlayerManager.Instance.player.transform.position) <= 0.5f)
                                {
                                    AnalyticsCalls.GetEssence(i);
                                    break;
                                }
                            }

                            //Check if all essence are collected
                            bool isTaken = true;
                            for (int i = 0; i < MapManager.Instance.mapInfo_ToSave.essenceList.Count; i++)
                            {
                                if (!MapManager.Instance.mapInfo_ToSave.essenceList[i].isTaken)
                                {
                                    isTaken = false;
                                    break;
                                }
                            }

                            if (isTaken)
                            {
                                AnalyticsCalls.GetAllEssenceInALevel();
                            }
                            break;
                        case Items.Skin:
                            PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.itemsGot.skin += 1 /*itemReceived.amount*/;
                            UpdateSkinTaken();
                            break;
                        case Items.IncreaseMaxSteps:
                            PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Max += 1 /*itemReceived.amount*/;
                            PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Current += 1 /*itemReceived.amount*/;
                            break;

                        default:
                            break;
                    }

                    MarkedAsTaken();
                    Action_PickupGot_isActive();
                }
            }
        }
    }
    void MarkedAsTaken()
    {
        for (int i = 0; i < DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Count; i++)
        {
            if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].mapName == SceneManager.GetActiveScene().name)
            {
                switch (itemReceived)
                {
                    case Items.None:
                        break;

                    case Items.Essence:
                        for (int j = 0; j < DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].essenceList.Count; j++)
                        {
                            if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].essenceList[j].pos.x == gameObject.transform.position.x
                                && DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].essenceList[j].pos.z == gameObject.transform.position.z)
                            {
                                DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].essenceList[j].isTaken = true;
                                Action_EssencePickupGot_isActive();

                                print("0. Pickup got: Essence");

                                return;
                            }
                        }
                        break;

                    case Items.Skin:
                        if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].levelSkin.pos.x == gameObject.transform.position.x
                                && DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].levelSkin.pos.z == gameObject.transform.position.z)
                        {
                            DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].levelSkin.isTaken = true;
                            Action_SkinPickupGot_isActive();

                            print("0. Pickup got: Skin");
                            return;
                        }
                        break;

                    case Items.IncreaseMaxSteps:
                        for (int j = 0; j < DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].maxStepList.Count; j++)
                        {
                            if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].maxStepList[j].pos.x == gameObject.transform.position.x
                                && DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].maxStepList[j].pos.z == gameObject.transform.position.z)
                            {
                                DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].maxStepList[j].isTaken = true;
                                Action_StepUpPickupGot_isActive();

                                print("0. Pickup got: MaxStep");
                                return;
                            }
                        }
                        break;

                    default:
                        break;
                }

                //If it's a goal
                if (goal)
                {
                    DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].isCompleted = true;
                }
            }
        }
    }
    void UpdateSkinTaken()
    {
        SkinType tempType = SkinType.None;

        for (int i = 0; i < DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Count; i++)
        {
            if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].mapName == mapManager.mapInfo_ToSave.mapName)
            {
                tempType = mapManager.mapInfo_ToSave.skintype;
                break;
            }
        }

        switch (tempType)
        {
            case SkinType.None:
                break;

            case SkinType.Rivergreen_Lv1:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level1 = WardrobeSkinState.Available;
                break;
            case SkinType.Rivergreen_Lv2:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level2 = WardrobeSkinState.Available;
                break;
            case SkinType.Rivergreen_Lv3:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level3 = WardrobeSkinState.Available;
                break;
            case SkinType.Rivergreen_Lv4:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level4 = WardrobeSkinState.Available;
                break;
            case SkinType.Rivergreen_Lv5:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level5 = WardrobeSkinState.Available;
                break;

            case SkinType.Firevein_Lv1:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level1 = WardrobeSkinState.Available;
                break;
            case SkinType.Firevein_Lv2:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level2 = WardrobeSkinState.Available;
                break;
            case SkinType.Firevein_Lv3:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level3 = WardrobeSkinState.Available;
                break;
            case SkinType.Firevein_Lv4:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level4 = WardrobeSkinState.Available;
                break;
            case SkinType.Firevein_Lv5:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level5 = WardrobeSkinState.Available;
                break;

            case SkinType.Sandlands_Lv1:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level1 = WardrobeSkinState.Available;
                break;
            case SkinType.Sandlands_Lv2:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level2 = WardrobeSkinState.Available;
                break;
            case SkinType.Sandlands_Lv3:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level3 = WardrobeSkinState.Available;
                break;
            case SkinType.Sandlands_Lv4:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level4 = WardrobeSkinState.Available;
                break;
            case SkinType.Sandlands_Lv5:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level5 = WardrobeSkinState.Available;
                break;

            case SkinType.Frostfield_Lv1:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level1 = WardrobeSkinState.Available;
                break;
            case SkinType.Frostfield_Lv2:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level2 = WardrobeSkinState.Available;
                break;
            case SkinType.Frostfield_Lv3:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level3 = WardrobeSkinState.Available;
                break;
            case SkinType.Frostfield_Lv4:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level4 = WardrobeSkinState.Available;
                break;
            case SkinType.Frostfield_Lv5:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level5 = WardrobeSkinState.Available;
                break;

            case SkinType.Witchmire_Lv1:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level1 = WardrobeSkinState.Available;
                break;
            case SkinType.Witchmire_Lv2:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level2 = WardrobeSkinState.Available;
                break;
            case SkinType.Witchmire_Lv3:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level3 = WardrobeSkinState.Available;
                break;
            case SkinType.Witchmire_Lv4:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level4 = WardrobeSkinState.Available;
                break;
            case SkinType.Witchmire_Lv5:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level5 = WardrobeSkinState.Available;
                break;

            case SkinType.Metalworks_Lv1:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level1 = WardrobeSkinState.Available;
                break;
            case SkinType.Metalworks_Lv2:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level2 = WardrobeSkinState.Available;
                break;
            case SkinType.Metalworks_Lv3:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level3 = WardrobeSkinState.Available;
                break;
            case SkinType.Metalworks_Lv4:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level4 = WardrobeSkinState.Available;
                break;
            case SkinType.Metalworks_Lv5:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level5 = WardrobeSkinState.Available;
                break;

            case SkinType.Default:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default = WardrobeSkinState.Available;
                break;

            default:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default = WardrobeSkinState.Available;
                break;
        }

        SkinsManager.Instance.SaveData();
    }
    
    public void GetAbility()
    {
        switch (abilityReceived)
        {
            case Abilities.None:
                break;

            case Abilities.Snorkel:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Snorkel = true;
                Action_AbilityPickupGot_isActive();
                //MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.SwimSuit = true;
                Action_SnorkelGot?.Invoke();
                break;
            case Abilities.Flippers:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Flippers = true;
                Action_AbilityPickupGot_isActive();
                //MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.SwiftSwim = true;
                break;
            case Abilities.OxygenTank:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.OxygenTank = true;
                Action_AbilityPickupGot_isActive();
                //MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Flippers = true;
                Action_OxygenTankGot?.Invoke();
                break;
            case Abilities.SpringShoes:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.SpringShoes = true;
                Action_AbilityPickupGot_isActive();
                Action_JumpingGot?.Invoke();
                //MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Jumping = true;
                break;
            case Abilities.GrapplingHook:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.GrapplingHook = true;
                Action_AbilityPickupGot_isActive();
                //MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.GrapplingHook = true;
                break;
            case Abilities.ClimingGloves:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.ClimingGloves = true;
                Action_AbilityPickupGot_isActive();
                //MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.CeilingGrab = true;
                break;
            case Abilities.HandDrill:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.HandDrill = true;
                Action_AbilityPickupGot_isActive();
                //MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Dash = true;
                break;
            case Abilities.DrillHelmet:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.DrillHelmet = true;
                Action_AbilityPickupGot_isActive();
                //MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Ascend = true;
                break;
            case Abilities.DrillBoots:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.DrillBoots = true;
                Action_AbilityPickupGot_isActive();
                //MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Descend = true;
                break;

            default:
                break;
        }

        if (abilityReceived != Abilities.None)
        {
            PopUpManager.Instance.ShowAbilityPopup(abilityReceived);

            Action_PickupGot_isActive();

            Movement.Instance.UpdateLookDir();
        }
    }


    //--------------------


    public void Action_PickupGot_isActive()
    {
        Action_PickupGot?.Invoke();
    }
    public void Action_EssencePickupGot_isActive()
    {
        Action_EssencePickupGot?.Invoke();
    }
    public void Action_StepUpPickupGot_isActive()
    {
        Action_StepsUpPickupGot?.Invoke();
    }
    public void Action_SkinPickupGot_isActive()
    {
        Action_SkinPickupGot?.Invoke();
    }
    public void Action_AbilityPickupGot_isActive()
    {
        Action_AbilityPickupGot?.Invoke();
    }
}

[Serializable]
public class ItemStats
{
    public Items item;
    public int amount;
}

public enum Items
{
    None,

    Essence,
    Skin,
    IncreaseMaxSteps
}

public enum Abilities
{
    None,

    Snorkel,
    Flippers,
    OxygenTank,

    DrillHelmet,
    DrillBoots,

    HandDrill,

    SpringShoes,

    ClimingGloves,

    GrapplingHook,
}
