using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentAbility : MonoBehaviour
{
    [SerializeField] List<Abilities> permanentAbilityList = new List<Abilities>();
    [SerializeField] List<GameObject> levelsToComplete = new List<GameObject>();

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

        for (int i = 0; i < levelsToComplete.Count; i++)
        {
            for (int j = 0; j < MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List.Count; j++)
            {
                if (levelsToComplete[i].GetComponent<LoadLevel>().levelToPlay == MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List[j].mapName)
                {
                    if (MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List[j].isCompleted)
                    {
                        levelsCompletedCounter++;
                        break;
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

                    case Abilities.SwimSuit:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.SwimSuit = true;
                        break;
                    case Abilities.SwiftSwim:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.SwiftSwim = true;
                        break;
                    case Abilities.Flippers:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers = true;
                        break;
                    case Abilities.Jumping:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.Jumping = true;
                        break;
                    case Abilities.CeilingGrab:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.CeilingGrab = true;
                        break;
                    case Abilities.Dash:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.Dash = true;
                        break;
                    case Abilities.Ascend:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.Ascend = true;
                        break;
                    case Abilities.Descend:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.Descend = true;
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
