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
                    MainManager.Instance.player.GetComponent<Player_Stats>().stats.abilities.FenceSneak = true;
                    Player_Stats.Instance.UpdateFenceSneak();
                    break;
                case Abilities.SwimSuit:
                    MainManager.Instance.player.GetComponent<Player_Stats>().stats.abilities.SwimSuit = true;
                    Player_Stats.Instance.UpdateSwimsuit();
                    break;
                case Abilities.Flippers:
                    MainManager.Instance.player.GetComponent<Player_Stats>().stats.abilities.Flippers = true;
                    Player_Stats.Instance.UpdateFlippers();
                    break;
                case Abilities.LavaSuit:
                    MainManager.Instance.player.GetComponent<Player_Stats>().stats.abilities.LavaSuit = true;
                    Player_Stats.Instance.UpdateLavaSuit();
                    break;
                case Abilities.HikersKit:
                    MainManager.Instance.player.GetComponent<Player_Stats>().stats.abilities.HikerGear = true;
                    Player_Stats.Instance.UpdateHikerGear();
                    break;

                case Abilities.IceSpikes:
                    MainManager.Instance.player.GetComponent<Player_Stats>().stats.abilities.IceSpikes = true;
                    Player_Stats.Instance.UpdateIceSpikes();
                    break;
                case Abilities.GrapplingHook:
                    MainManager.Instance.player.GetComponent<Player_Stats>().stats.abilities.GrapplingHook = true;
                    Player_Stats.Instance.UpdateGrapplingHook();
                    break;
                case Abilities.Hammer:
                    MainManager.Instance.player.GetComponent<Player_Stats>().stats.abilities.Hammer = true;
                    Player_Stats.Instance.UpdateHammer();
                    break;
                case Abilities.ClimbingGear:
                    MainManager.Instance.player.GetComponent<Player_Stats>().stats.abilities.ClimbingGear = true;
                    Player_Stats.Instance.UpdateClimbingGear();
                    break;
                case Abilities.Dash:
                    MainManager.Instance.player.GetComponent<Player_Stats>().stats.abilities.Dash = true;
                    Player_Stats.Instance.UpdateDash();
                    break;
                case Abilities.Ascend:
                    MainManager.Instance.player.GetComponent<Player_Stats>().stats.abilities.Ascend = true;
                    Player_Stats.Instance.UpdateAscend();
                    break;
                case Abilities.Descend:
                    MainManager.Instance.player.GetComponent<Player_Stats>().stats.abilities.Descend = true;
                    Player_Stats.Instance.UpdateDescend();
                    break;
                case Abilities.ControlStick:
                    MainManager.Instance.player.GetComponent<Player_Stats>().stats.abilities.ControlStick = true;
                    Player_Stats.Instance.UpdateControlStick();
                    break;

                default:
                    break;
            }
        }
    }
}

public enum Abilities
{
    None,

    FenceSneak,
    SwimSuit,
    Flippers,
    LavaSuit,
    HikersKit,

    IceSpikes,
    GrapplingHook,
    Hammer,
    ClimbingGear,
    Dash,
    Ascend,
    Descend,
    ControlStick,
}