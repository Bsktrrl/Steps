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
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.FenceSneak = true;
                    PlayerStats.Instance.UpdateFenceSneak();
                    break;
                case Abilities.SwimSuit:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.SwimSuit = true;
                    PlayerStats.Instance.UpdateSwimsuit();
                    break;
                case Abilities.SwiftSwim:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.SwiftSwim = true;
                    PlayerStats.Instance.UpdateSwiftSwim();
                    break;
                case Abilities.Flippers:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.Flippers = true;
                    PlayerStats.Instance.UpdateFlippers();
                    break;
                case Abilities.LavaSuit:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.LavaSuit = true;
                    PlayerStats.Instance.UpdateLavaSuit();
                    break;
                case Abilities.LavaSwiftSwim:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.LavaSwiftSwim = true;
                    PlayerStats.Instance.UpdateLavaSwiftSwim();
                    break;
                case Abilities.HikersKit:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.HikerGear = true;
                    PlayerStats.Instance.UpdateHikerGear();
                    break;

                case Abilities.IceSpikes:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.IceSpikes = true;
                    PlayerStats.Instance.UpdateIceSpikes();
                    break;
                case Abilities.GrapplingHook:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.GrapplingHook = true;
                    PlayerStats.Instance.UpdateGrapplingHook();
                    break;
                case Abilities.Hammer:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.Hammer = true;
                    PlayerStats.Instance.UpdateHammer();
                    break;
                case Abilities.ClimbingGear:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.ClimbingGear = true;
                    PlayerStats.Instance.UpdateClimbingGear();
                    break;
                case Abilities.Dash:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.Dash = true;
                    PlayerStats.Instance.UpdateDash();
                    break;
                case Abilities.Ascend:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.Ascend = true;
                    PlayerStats.Instance.UpdateAscend();
                    break;
                case Abilities.Descend:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.Descend = true;
                    PlayerStats.Instance.UpdateDescend();
                    break;
                case Abilities.ControlStick:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.ControlStick = true;
                    PlayerStats.Instance.UpdateControlStick();
                    break;

                default:
                    break;
            }
        }
    }
}