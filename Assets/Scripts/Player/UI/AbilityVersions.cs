using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityVersions : MonoBehaviour
{
    [Header("Ability Type")]
    [SerializeField] Abilities ability;

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

            case Abilities.SwimSuit:
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.SwimSuit)
                    GetComponent<Image>().sprite = permanent_Sprite;
                else
                    GetComponent<Image>().sprite = temporary_Sprite;
                break;
            case Abilities.SwiftSwim:
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.SwiftSwim)
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
            case Abilities.Ascend:
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.Ascend)
                    GetComponent<Image>().sprite = permanent_Sprite;
                else
                    GetComponent<Image>().sprite = temporary_Sprite;
                break;
            case Abilities.Descend:
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.Descend)
                    GetComponent<Image>().sprite = permanent_Sprite;
                else
                    GetComponent<Image>().sprite = temporary_Sprite;
                break;
            case Abilities.Dash:
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.Dash)
                    GetComponent<Image>().sprite = permanent_Sprite;
                else
                    GetComponent<Image>().sprite = temporary_Sprite;
                break;
            case Abilities.Jumping:
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.Jumping)
                    GetComponent<Image>().sprite = permanent_Sprite;
                else
                    GetComponent<Image>().sprite = temporary_Sprite;
                break;
            case Abilities.CeilingGrab:
                if (PlayerStats.Instance.stats.abilitiesGot_Permanent.CeilingGrab)
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
