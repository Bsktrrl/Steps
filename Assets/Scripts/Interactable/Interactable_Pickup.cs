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

    MapManager mapManager;

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
            print("Pickup entered");

            GetItems();
            GetAbility();

            gameObject.SetActive(false);

            PlayerManager.Instance.SavePlayerStats();
            MapManager.Instance.SaveMapInfo();

            if (goal)
            {
                //Update analyticsData
                AnalyticsCalls.OnLevel(mapManager.timeUsedInLevel, mapManager.stepCount, mapManager.respawnCount, mapManager.abilitiesPickedUp, mapManager.cameraRotated, mapManager.swimCounter, mapManager.swiftSwimCounter, mapManager.jumpCounter, mapManager.dashCounter, mapManager.ascendCounter, mapManager.descendCounter, mapManager.grapplingHookCounter, mapManager.ceilingGrabCounter);
                AnalyticsCalls.OnLevelFinishing(mapManager.timeUsedInLevel, mapManager.stepCount, mapManager.respawnCount, mapManager.abilitiesPickedUp, mapManager.cameraRotated, mapManager.swimCounter, mapManager.swiftSwimCounter, mapManager.jumpCounter, mapManager.dashCounter, mapManager.ascendCounter, mapManager.descendCounter, mapManager.grapplingHookCounter, mapManager.ceilingGrabCounter);

                AnalyticsCalls.CompleteLevel();

                PlayerManager.Instance.player.GetComponent<PlayerManager>().QuitLevel();
            }
        }
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
                            PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.itemsGot.essence += 1 /*itemReceived.amount*/;

                            //Get coin number
                            for (int i = 0; i < MapManager.Instance.mapInfo_ToSave.essenceList.Count; i++)
                            {
                                if (Vector3.Distance(MapManager.Instance.mapInfo_ToSave.essenceList[i].pos, PlayerManager.Instance.player.transform.position) <= 0.5f)
                                {
                                    AnalyticsCalls.GetEssence(i);
                                    break;
                                }
                            }

                            //Check if all coins are collected
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

                            Action_EssencePickupGot_isActive();
                            break;
                        case Items.Skin:
                            Action_SkinPickupGot_isActive();
                            PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.itemsGot.skin += 1 /*itemReceived.amount*/;
                            break;
                        case Items.IncreaseMaxSteps:
                            Action_StepUpPickupGot_isActive();
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

                                return;
                            }
                        }
                        break;
                    case Items.Skin:
                        if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].levelSkin.pos.x == gameObject.transform.position.x
                                && DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].levelSkin.pos.z == gameObject.transform.position.z)
                        {
                            DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].levelSkin.isTaken = true;

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
