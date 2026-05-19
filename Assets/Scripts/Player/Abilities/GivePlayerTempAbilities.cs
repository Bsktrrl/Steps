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
                    print("0. None");
                    break;

                case Abilities.Snorkel:
                    playerStats.stats.abilitiesGot_Temporary.Snorkel = true;
                    print("1. Snorkel");

                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Snorkel = true;
                    mapManager.mapInfo_ToSave.abilitiesGotInLevel.Snorkel = true;
                    break;
                case Abilities.OxygenTank:
                    playerStats.stats.abilitiesGot_Temporary.OxygenTank = true;

                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.OxygenTank = true;
                    mapManager.mapInfo_ToSave.abilitiesGotInLevel.OxygenTank = true;
                    print("2. OxygenTank");
                    break;
                case Abilities.Flippers:
                    playerStats.stats.abilitiesGot_Temporary.Flippers = true;

                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Flippers = true;
                    mapManager.mapInfo_ToSave.abilitiesGotInLevel.Flippers = true;
                    print("3. Flippers");
                    break;
                case Abilities.DrillHelmet:
                    playerStats.stats.abilitiesGot_Temporary.DrillHelmet = true;
                    break;
                case Abilities.DrillBoots:
                    playerStats.stats.abilitiesGot_Temporary.DrillBoots = true;
                    break;
                case Abilities.HandDrill:
                    playerStats.stats.abilitiesGot_Temporary.HandDrill = true;
                    break;
                case Abilities.SpringShoes:
                    playerStats.stats.abilitiesGot_Temporary.SpringShoes = true;
                    break;
                case Abilities.ClimingGloves:
                    playerStats.stats.abilitiesGot_Temporary.ClimingGloves = true;
                    break;
                case Abilities.GrapplingHook:
                    playerStats.stats.abilitiesGot_Temporary.GrapplingHook = true;
                    break;

                default:
                    print("00. None");
                    break;
            }
        }

        if (pickupTemp)
        {
            pickupTemp.Action_AbilityPickupGot_isActive();

            print("00000. pickupTemp - True");
        }
        else
        {
            print("00000. pickupTemp - False");
        }

        mapManager.SaveMapInfo();
    }
}
