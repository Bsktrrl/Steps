using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_GetAbility : MonoBehaviour
{
    public List<Abilities> abilityList;


    //--------------------


    public void GetAbility()
    {
        for (int i = 0; i < abilityList.Count; i++)
        {
            switch (abilityList[i])
            {
                case Abilities.None:
                    break;

                case Abilities.FenceSneak:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.FenceSneak = true;
                    break;
                case Abilities.SwimSuit:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.SwimSuit = true;
                    break;
                case Abilities.SwiftSwim:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.SwiftSwim = true;
                    break;
                case Abilities.Flippers:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Flippers = true;
                    break;
                case Abilities.LavaSuit:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.LavaSuit = true;
                    break;
                case Abilities.LavaSwiftSwim:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.LavaSwiftSwim = true;
                    break;
                case Abilities.HikerGear:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.HikerGear = true;
                    break;

                case Abilities.IceSpikes:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.IceSpikes = true;
                    break;
                case Abilities.GrapplingHook:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.GrapplingHook = true;
                    break;
                case Abilities.Hammer:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Hammer = true;
                    break;
                case Abilities.ClimbingGear:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.ClimbingGear = true;
                    break;
                case Abilities.Dash:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Dash = true;
                    break;
                case Abilities.Ascend:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Ascend = true;
                    break;
                case Abilities.Descend:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Descend = true;
                    break;
                case Abilities.ControlStick:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.ControlStick = true;
                    break;

                default:
                    break;
            }
        }
    }
}