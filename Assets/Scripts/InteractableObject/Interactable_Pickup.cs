using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable_Pickup : MonoBehaviour
{
    public Items itemReceived;
    public Abilities abilityReceived;
    public bool goal;

    Vector3 startPos;
    RaycastHit hit;


    //--------------------


    private void Start()
    {
        startPos = transform.position;
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
                PlayerManager.Instance.player.GetComponent<Player_Movement>().QuitLevel();
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

                        case Items.Coin:
                            PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.itemsGot.coin += 1 /*itemReceived.amount*/;
                            break;
                        case Items.Collectable:
                            PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.itemsGot.collectable += 1 /*itemReceived.amount*/;
                            break;
                        case Items.IncreaseMaxSteps:
                            PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Max += 1 /*itemReceived.amount*/;
                            break;

                        default:
                            break;
                    }

                    MarkedAsTaken();
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

                    case Items.Coin:
                        for (int j = 0; j < DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].coinList.Count; j++)
                        {
                            if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].coinList[j].pos.x == gameObject.transform.position.x
                                && DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].coinList[j].pos.z == gameObject.transform.position.z)
                            {
                                DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].coinList[j].isTaken = true;

                                return;
                            }
                        }
                        break;
                    case Items.Collectable:
                        for (int j = 0; j < DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].collectableList.Count; j++)
                        {
                            if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].collectableList[j].pos.x == gameObject.transform.position.x
                                && DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].collectableList[j].pos.z == gameObject.transform.position.z)
                            {
                                DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].collectableList[j].isTaken = true;

                                return;
                            }
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
        Player_BlockDetector.Instance.Update_BlockStandingOn();

        switch (abilityReceived)
        {
            case Abilities.None:
                break;

            case Abilities.FenceSneak:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.FenceSneak = true;
                MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.FenceSneak = true;
                break;
            case Abilities.SwimSuit:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.SwimSuit = true;
                MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.SwimSuit = true;
                break;
            case Abilities.SwiftSwim:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.SwiftSwim = true;
                MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.SwiftSwim = true;
                break;
            case Abilities.Flippers:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Flippers = true;
                MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Flippers = true;
                break;
            case Abilities.LavaSuit:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.LavaSuit = true;
                MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.LavaSuit = true;
                break;
            case Abilities.LavaSwiftSwim:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.LavaSwiftSwim = true;
                MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.LavaSwiftSwim = true;
                break;
            case Abilities.HikersKit:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.HikerGear = true;
                MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.HikerGear = true;
                break;

            case Abilities.IceSpikes:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.IceSpikes = true;
                MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.IceSpikes = true;
                break;
            case Abilities.GrapplingHook:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.GrapplingHook = true;
                MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.GrapplingHook = true;
                break;
            case Abilities.Hammer:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Hammer = true;
                MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Hammer = true;
                break;
            case Abilities.ClimbingGear:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.ClimbingGear = true;
                MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.ClimbingGear = true;
                break;
            case Abilities.Dash:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Dash = true;
                MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Dash = true;
                break;
            case Abilities.Ascend:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Ascend = true;
                MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Ascend = true;
                break;
            case Abilities.Descend:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Descend = true;
                MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Descend = true;
                break;
            case Abilities.ControlStick:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.ControlStick = true;
                MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.ControlStick = true;
                break;

            default:
                break;
        }
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

    Coin,
    Collectable,
    IncreaseMaxSteps
}

public enum Abilities
{
    None,

    FenceSneak,

    SwimSuit,
    SwiftSwim,
    Flippers,

    LavaSuit,
    LavaSwiftSwim,

    HikersKit,
    ClimbingGear,

    IceSpikes,

    Dash,
    Ascend,
    Descend,

    Hammer,

    GrapplingHook,

    ControlStick,
}
