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
}