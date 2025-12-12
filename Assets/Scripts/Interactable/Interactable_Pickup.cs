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

    public static event Action Action_FlippersGot;
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

            case SkinType.Water_Grass:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level1 = WardrobeSkinState.Available;
                break;
            case SkinType.Water_Water:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level2 = WardrobeSkinState.Available;
                break;
            case SkinType.Water_Wood:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level3 = WardrobeSkinState.Available;
                break;
            case SkinType.Water_4:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level4 = WardrobeSkinState.Available;
                break;
            case SkinType.Water_5:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level5 = WardrobeSkinState.Available;
                break;
            case SkinType.Water_6:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level6 = WardrobeSkinState.Available;
                break;

            case SkinType.Cave_Stone:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level1 = WardrobeSkinState.Available;
                break;
            case SkinType.Cave_Stone_Brick:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level2 = WardrobeSkinState.Available;
                break;
            case SkinType.Cave_Lava:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level3 = WardrobeSkinState.Available;
                break;
            case SkinType.Cave_Rock:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level4 = WardrobeSkinState.Available;
                break;
            case SkinType.Cave_Brick_Brown:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level5 = WardrobeSkinState.Available;
                break;
            case SkinType.Cave_Brick_Black:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level6 = WardrobeSkinState.Available;
                break;

            case SkinType.Desert_Sand:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level1 = WardrobeSkinState.Available;
                break;
            case SkinType.Desert_Clay:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level2 = WardrobeSkinState.Available;
                break;
            case SkinType.Desert_Clay_Tiles:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level3 = WardrobeSkinState.Available;
                break;
            case SkinType.Desert_Sandstone:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level4 = WardrobeSkinState.Available;
                break;
            case SkinType.Desert_Sandstone_Swirl:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level5 = WardrobeSkinState.Available;
                break;
            case SkinType.Desert_Quicksand:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level6 = WardrobeSkinState.Available;
                break;

            case SkinType.Winter_Snow:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level1 = WardrobeSkinState.Available;
                break;
            case SkinType.Winter_Ice:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level2 = WardrobeSkinState.Available;
                break;
            case SkinType.Winter_ColdWood:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level3 = WardrobeSkinState.Available;
                break;
            case SkinType.Winter_FrozenGrass:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level4 = WardrobeSkinState.Available;
                break;
            case SkinType.Winter_CrackedIce:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level5 = WardrobeSkinState.Available;
                break;
            case SkinType.Winter_Crocked:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level6 = WardrobeSkinState.Available;
                break;

            case SkinType.Swamp_SwampWater:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level1 = WardrobeSkinState.Available;
                break;
            case SkinType.Swamp_Mud:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level2 = WardrobeSkinState.Available;
                break;
            case SkinType.Swamp_SwampGrass:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level3 = WardrobeSkinState.Available;
                break;
            case SkinType.Swamp_JungleWood:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level4 = WardrobeSkinState.Available;
                break;
            case SkinType.Swamp_SwampWood:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level5 = WardrobeSkinState.Available;
                break;
            case SkinType.Swamp_TempleBlock:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level6 = WardrobeSkinState.Available;
                break;

            case SkinType.Industrial_Metal:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level1 = WardrobeSkinState.Available;
                break;
            case SkinType.Industrial_Brass:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level2 = WardrobeSkinState.Available;
                break;
            case SkinType.Industrial_Gold:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level3 = WardrobeSkinState.Available;
                break;
            case SkinType.Industrial_Casing_Metal:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level4 = WardrobeSkinState.Available;
                break;
            case SkinType.Industria_Casingl_Brass:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level5 = WardrobeSkinState.Available;
                break;
            case SkinType.Industrial_Casing_Gold:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level6 = WardrobeSkinState.Available;
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

            case Abilities.SwimSuit:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.SwimSuit = true;
                Action_AbilityPickupGot_isActive();
                //MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.SwimSuit = true;
                break;
            case Abilities.SwiftSwim:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.SwiftSwim = true;
                Action_AbilityPickupGot_isActive();
                //MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.SwiftSwim = true;
                break;
            case Abilities.Flippers:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Flippers = true;
                Action_AbilityPickupGot_isActive();
                //MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Flippers = true;
                Action_FlippersGot?.Invoke();
                break;
            case Abilities.Jumping:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Jumping = true;
                Action_AbilityPickupGot_isActive();
                Action_JumpingGot?.Invoke();
                //MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Jumping = true;
                break;
            case Abilities.GrapplingHook:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.GrapplingHook = true;
                Action_AbilityPickupGot_isActive();
                //MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.GrapplingHook = true;
                break;
            case Abilities.CeilingGrab:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.CeilingGrab = true;
                Action_AbilityPickupGot_isActive();
                //MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.CeilingGrab = true;
                break;
            case Abilities.Dash:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Dash = true;
                Action_AbilityPickupGot_isActive();
                //MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Dash = true;
                break;
            case Abilities.Ascend:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Ascend = true;
                Action_AbilityPickupGot_isActive();
                //MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Ascend = true;
                break;
            case Abilities.Descend:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Descend = true;
                Action_AbilityPickupGot_isActive();
                //MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Descend = true;
                break;

            default:
                break;
        }

        Action_PickupGot_isActive();

        Movement.Instance.UpdateLookDir();
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

    SwimSuit,
    SwiftSwim,
    Flippers,

    Ascend,
    Descend,

    Dash,

    Jumping,

    CeilingGrab,

    GrapplingHook,
}
