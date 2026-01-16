using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityVersions : MonoBehaviour
{
    [Header("Ability Type")]
    public Abilities ability;

    [Header("Sprites")]
    [SerializeField] Sprite temporary_Sprite;
    [SerializeField] Sprite permanent_Sprite;


    //--------------------


    private void OnEnable()
    {
        UpdateSprite();
    }


    //--------------------


    void UpdateSprite()
    {
        switch (ability)
        {
            case Abilities.None:
                GetComponent<Image>().sprite = null;
                break;

            case Abilities.Snorkel:
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.Snorkel)
                    GetComponent<Image>().sprite = permanent_Sprite;
                else
                    GetComponent<Image>().sprite = temporary_Sprite;
                break;
            case Abilities.Flippers:
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers)
                    GetComponent<Image>().sprite = permanent_Sprite;
                else
                    GetComponent<Image>().sprite = temporary_Sprite;
                break;
            case Abilities.OxygenTank:
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.OxygenTank)
                    GetComponent<Image>().sprite = permanent_Sprite;
                else
                    GetComponent<Image>().sprite = temporary_Sprite;
                break;
            case Abilities.DrillHelmet:
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.DrillHelmet)
                    GetComponent<Image>().sprite = permanent_Sprite;
                else
                    GetComponent<Image>().sprite = temporary_Sprite;
                break;
            case Abilities.DrillBoots:
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.DrillBoots)
                    GetComponent<Image>().sprite = permanent_Sprite;
                else
                    GetComponent<Image>().sprite = temporary_Sprite;
                break;
            case Abilities.HandDrill:
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.HandDrill)
                    GetComponent<Image>().sprite = permanent_Sprite;
                else
                    GetComponent<Image>().sprite = temporary_Sprite;
                break;
            case Abilities.SpringShoes:
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.SpringShoes)
                    GetComponent<Image>().sprite = permanent_Sprite;
                else
                    GetComponent<Image>().sprite = temporary_Sprite;
                break;
            case Abilities.ClimingGloves:
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.ClimingGloves)
                    GetComponent<Image>().sprite = permanent_Sprite;
                else
                    GetComponent<Image>().sprite = temporary_Sprite;
                break;
            case Abilities.GrapplingHook:
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook)
                    GetComponent<Image>().sprite = permanent_Sprite;
                else
                    GetComponent<Image>().sprite = temporary_Sprite;
                break;

            default:
                GetComponent<Image>().sprite = null;
                break;
        }
    }
}
