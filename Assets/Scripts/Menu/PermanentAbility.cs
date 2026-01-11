using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentAbility : MonoBehaviour
{
    [SerializeField] List<Abilities> permanentAbilityList = new List<Abilities>();
    [SerializeField] List<GameObject> levelsToComplete = new List<GameObject>();

    MenuLevelInfo menuLevelInfo;


    //--------------------


    private void Start()
    {
        menuLevelInfo = FindAnyObjectByType<MenuLevelInfo>();
    }


    //--------------------


    private void OnEnable()
    {
        levelsToComplete = new List<GameObject>();

        // Find all child GameObjects that have the LoadLevel script
        foreach (Transform child in gameObject.transform)
        {
            if (child.GetComponent<LoadLevel>() != null)
            {
                levelsToComplete.Add(child.gameObject);
            }
        }

        int levelsCompletedCounter = 0;

        if (menuLevelInfo)
        {
            for (int i = 0; i < levelsToComplete.Count; i++)
            {
                for (int j = 0; j < menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List.Count; j++)
                {
                    if (levelsToComplete[i].GetComponent<LoadLevel>().levelToPlay == menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List[j].mapName)
                    {
                        if (menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List[j].isCompleted)
                        {
                            levelsCompletedCounter++;
                            break;
                        }
                    }
                }
            }
        }

        

        if (levelsCompletedCounter >= levelsToComplete.Count && levelsToComplete.Count > 0)
        {
            print("1. Completed Biome and get an Ability | Counter: " + levelsCompletedCounter + " | CompleteCount: " + levelsToComplete.Count);

            for (int i = 0; i < permanentAbilityList.Count; i++)
            {
                switch (permanentAbilityList[i])
                {
                    case Abilities.None:
                        break;

                    case Abilities.Snorkel:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.Snorkel = true;
                        break;
                    case Abilities.Flippers:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers = true;
                        break;
                    case Abilities.OxygenTank:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.OxygenTank = true;
                        break;
                    case Abilities.SpringShoes:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.SpringShoes = true;
                        break;
                    case Abilities.ClimingGloves:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.ClimingGloves = true;
                        break;
                    case Abilities.HandDrill:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.HandDrill = true;
                        break;
                    case Abilities.DrillHelmet:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.DrillHelmet = true;
                        break;
                    case Abilities.DrillBoots:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.DrillBoots = true;
                        break;
                    case Abilities.GrapplingHook:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook = true;
                        break;

                    default:
                        break;
                }
            }
        }

        PlayerStats_SaveLoad.Instance.SaveGame();
    }
}
