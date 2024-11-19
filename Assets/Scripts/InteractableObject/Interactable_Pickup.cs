using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable_Pickup : MonoBehaviour
{
    public Items itemReceived;
    public Abilities abilityReceived;


    //--------------------


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            print("Pickup entered");

            GetItems();
            GetAbility();

            //Hide Pickup
            gameObject.SetActive(false);
        }
    }


    //--------------------


    public void GetItems()
    {
        switch (itemReceived)
        {
            case Items.None:
                break;

            case Items.Coin:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.itemsGot.coin += 1 /*itemReceived.amount*/;
                PlayerStats.Instance.UpdateCoins();
                break;
            case Items.Collectable:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.itemsGot.collectable += 1 /*itemReceived.amount*/;
                PlayerStats.Instance.UpdateCollectable();
                break;
            case Items.IncreaseMaxSteps:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Max += 1 /*itemReceived.amount*/;
                PlayerStats.Instance.UpdateStepsMax();
                break;

            default:
                break;
        }

        MarkedAsTaken();

        PlayerManager.Instance.SavePlayerStats();
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
                        break;

                    default:
                        break;
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

            case Abilities.FenceSneak:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.FenceSneak = true;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().UpdateFenceSneak();
                break;
            case Abilities.SwimSuit:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.SwimSuit = true;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().UpdateSwimsuit();
                break;
            case Abilities.SwiftSwim:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.SwiftSwim = true;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().UpdateSwiftSwim();
                break;
            case Abilities.Flippers:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.Flippers = true;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().UpdateFlippers();
                break;
            case Abilities.LavaSuit:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.LavaSuit = true;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().UpdateLavaSuit();
                break;
            case Abilities.LavaSwiftSwim:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.LavaSwiftSwim = true;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().UpdateLavaSwiftSwim();
                break;
            case Abilities.HikersKit:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.HikerGear = true;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().UpdateHikerGear();
                break;

            case Abilities.IceSpikes:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.IceSpikes = true;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().UpdateIceSpikes();
                break;
            case Abilities.GrapplingHook:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.GrapplingHook = true;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().UpdateGrapplingHook();
                break;
            case Abilities.Hammer:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.Hammer = true;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().UpdateHammer();
                break;
            case Abilities.ClimbingGear:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.ClimbingGear = true;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().UpdateClimbingGear();
                break;
            case Abilities.Dash:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.Dash = true;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().UpdateDash();
                break;
            case Abilities.Ascend:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.Ascend = true;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().UpdateAscend();
                break;
            case Abilities.Descend:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.Descend = true;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().UpdateDescend();
                break;
            case Abilities.ControlStick:
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.ControlStick = true;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().UpdateControlStick();
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
