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

                case Abilities.Snorkel:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Snorkel = true;
                    break;
                case Abilities.Flippers:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Flippers = true;
                    break;
                case Abilities.OxygenTank:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.OxygenTank = true;
                    break;
                case Abilities.SpringShoes:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.SpringShoes = true;
                    break;

                case Abilities.GrapplingHook:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.GrapplingHook = true;
                    break;
                case Abilities.ClimingGloves:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.ClimingGloves = true;
                    break;
                case Abilities.HandDrill:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.HandDrill = true;
                    break;
                case Abilities.DrillHelmet:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.DrillHelmet = true;
                    break;
                case Abilities.DrillBoots:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.DrillBoots = true;
                    break;

                default:
                    break;
            }
        }
    }
}