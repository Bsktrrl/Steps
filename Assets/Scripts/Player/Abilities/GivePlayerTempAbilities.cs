using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePlayerTempAbilities : MonoBehaviour
{
    MapManager mapManager;
    PlayerStats playerStats;
    Interactable_Pickup pickupTemp;

    [SerializeField] List<Abilities> abilitiesToGive_List;


    //--------------------


    private void Start()
    {
        pickupTemp = FindObjectOfType<Interactable_Pickup>();
        mapManager = FindObjectOfType<MapManager>();
    }


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += GiveAbilitiesTemporarly;
        MapManager.Action_PlayerObject_IsSpawned += FindPlayerStats;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= GiveAbilitiesTemporarly;
        MapManager.Action_PlayerObject_IsSpawned -= FindPlayerStats;
    }


    //--------------------


    void FindPlayerStats()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    void GiveAbilitiesTemporarly()
    {
        for (int i = 0; i < abilitiesToGive_List.Count; i++)
        {
            switch (abilitiesToGive_List[i])
            {
                case Abilities.None:
                    break;

                case Abilities.Snorkel:
                    playerStats.stats.abilitiesGot_Temporary.Snorkel = true;

                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Snorkel = true;
                    mapManager.mapInfo_ToSave.abilitiesGotInLevel.Snorkel = true;
                    break;
                case Abilities.OxygenTank:
                    playerStats.stats.abilitiesGot_Temporary.OxygenTank = true;

                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.OxygenTank = true;
                    mapManager.mapInfo_ToSave.abilitiesGotInLevel.OxygenTank = true;
                    break;
                case Abilities.Flippers:
                    playerStats.stats.abilitiesGot_Temporary.Flippers = true;

                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Flippers = true;
                    mapManager.mapInfo_ToSave.abilitiesGotInLevel.Flippers = true;
                    break;
                case Abilities.DrillHelmet:
                    playerStats.stats.abilitiesGot_Temporary.DrillHelmet = true;

                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.DrillHelmet = true;
                    mapManager.mapInfo_ToSave.abilitiesGotInLevel.DrillHelmet = true;
                    break;
                case Abilities.DrillBoots:
                    playerStats.stats.abilitiesGot_Temporary.DrillBoots = true;

                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.DrillBoots = true;
                    mapManager.mapInfo_ToSave.abilitiesGotInLevel.DrillBoots = true;
                    break;
                case Abilities.HandDrill:
                    playerStats.stats.abilitiesGot_Temporary.HandDrill = true;

                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.HandDrill = true;
                    mapManager.mapInfo_ToSave.abilitiesGotInLevel.HandDrill = true;
                    break;
                case Abilities.SpringShoes:
                    playerStats.stats.abilitiesGot_Temporary.SpringShoes = true;

                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.SpringShoes = true;
                    mapManager.mapInfo_ToSave.abilitiesGotInLevel.SpringShoes = true;
                    break;
                case Abilities.ClimingGloves:
                    playerStats.stats.abilitiesGot_Temporary.ClimingGloves = true;

                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.ClimingGloves = true;
                    mapManager.mapInfo_ToSave.abilitiesGotInLevel.ClimingGloves = true;
                    break;
                case Abilities.GrapplingHook:
                    playerStats.stats.abilitiesGot_Temporary.GrapplingHook = true;

                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.GrapplingHook = true;
                    mapManager.mapInfo_ToSave.abilitiesGotInLevel.GrapplingHook = true;
                    break;

                default:
                    break;
            }
        }

        if (pickupTemp)
        {
            pickupTemp.Action_AbilityPickupGot_isActive();
        }

        mapManager.SaveMapInfo();
    }
}
