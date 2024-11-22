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
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesTempGot.FenceSneak = true;
                    break;
                case Abilities.SwimSuit:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesTempGot.SwimSuit = true;
                    break;
                case Abilities.SwiftSwim:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesTempGot.SwiftSwim = true;
                    break;
                case Abilities.Flippers:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesTempGot.Flippers = true;
                    break;
                case Abilities.LavaSuit:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesTempGot.LavaSuit = true;
                    break;
                case Abilities.LavaSwiftSwim:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesTempGot.LavaSwiftSwim = true;
                    break;
                case Abilities.HikersKit:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesTempGot.HikerGear = true;
                    break;

                case Abilities.IceSpikes:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesTempGot.IceSpikes = true;
                    break;
                case Abilities.GrapplingHook:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesTempGot.GrapplingHook = true;
                    break;
                case Abilities.Hammer:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesTempGot.Hammer = true;
                    break;
                case Abilities.ClimbingGear:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesTempGot.ClimbingGear = true;
                    break;
                case Abilities.Dash:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesTempGot.Dash = true;
                    break;
                case Abilities.Ascend:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesTempGot.Ascend = true;
                    break;
                case Abilities.Descend:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesTempGot.Descend = true;
                    break;
                case Abilities.ControlStick:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesTempGot.ControlStick = true;
                    break;

                default:
                    break;
            }
        }
    }
}