using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentAbility : MonoBehaviour
{
    [SerializeField] List<GameObject> levelsToComplete = new List<GameObject>();
    [SerializeField] List<Abilities> permanentAbilityList = new List<Abilities>();

    private void OnEnable()
    {
        int levelsCompletedCounter = 0;

        for (int i = 0; i < levelsToComplete.Count; i++)
        {
            for (int j = 0; j < MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List.Count; j++)
            {
                if (levelsToComplete[i].GetComponent<LoadLevel>().levelToPlay == MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List[j].mapName)
                {
                    levelsCompletedCounter++;
                }
            }
        }

        if (levelsCompletedCounter >= levelsToComplete.Count)
        {
            print("1. Completed Biome and get an Ability");

            for (int i = 0; i < permanentAbilityList.Count; i++)
            {
                switch (permanentAbilityList[i])
                {
                    case Abilities.None:
                        break;

                    case Abilities.FenceSneak:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.FenceSneak = true;
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
                    case Abilities.LavaSuit:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.LavaSuit = true;
                        break;
                    case Abilities.LavaSwiftSwim:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.LavaSwiftSwim = true;
                        break;
                    case Abilities.HikerGear:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.HikerGear = true;
                        break;
                    case Abilities.ClimbingGear:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.ClimbingGear = true;
                        break;
                    case Abilities.IceSpikes:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.IceSpikes = true;
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
                    case Abilities.Hammer:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.Hammer = true;
                        break;
                    case Abilities.GrapplingHook:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook = true;
                        break;
                    case Abilities.ControlStick:
                        PlayerStats.Instance.stats.abilitiesGot_Permanent.ControlStick = true;
                        break;

                    default:
                        break;
                }
            }
        }

        PlayerStats_SaveLoad.Instance.SaveGame();
    }
}
