using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AbilityInfoButton_ImageDisplay : MonoBehaviour
{
    [SerializeField] GameObject passive_Temporary;
    [SerializeField] GameObject passive_Permanent;

    [SerializeField] GameObject active_Temporary;
    [SerializeField] GameObject active_Permanent;


    //--------------------


    private void OnEnable()
    {
        UpdateImage();
    }


    //--------------------


    void UpdateImage()
    {
        HideAllImages();

        switch (gameObject.GetComponent<AbilityVersions>().ability)
        {
            case Abilities.None:
                break;

            case Abilities.Snorkel:
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.Snorkel)
                    ShowImage(passive_Temporary, active_Temporary);
                else if (PlayerStats.Instance.stats.abilitiesGot_Permanent.Snorkel)
                    ShowImage(active_Permanent, passive_Permanent);
                break;
            case Abilities.Flippers:
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.Flippers)
                    ShowImage(passive_Temporary, active_Temporary);
                else if (PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers)
                    ShowImage(active_Permanent, passive_Permanent);
                break;
            case Abilities.OxygenTank:
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.OxygenTank)
                    ShowImage(passive_Temporary, active_Temporary);
                else if (PlayerStats.Instance.stats.abilitiesGot_Permanent.OxygenTank)
                    ShowImage(active_Permanent, passive_Permanent);
                break;
            case Abilities.DrillHelmet:
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.DrillHelmet)
                    ShowImage(passive_Temporary, active_Temporary);
                else if (PlayerStats.Instance.stats.abilitiesGot_Permanent.DrillHelmet)
                    ShowImage(active_Permanent, passive_Permanent);
                break;
            case Abilities.DrillBoots:
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.DrillBoots)
                    ShowImage(passive_Temporary, active_Temporary);
                else if (PlayerStats.Instance.stats.abilitiesGot_Permanent.DrillBoots)
                    ShowImage(active_Permanent, passive_Permanent);
                break;
            case Abilities.HandDrill:
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.HandDrill)
                    ShowImage(passive_Temporary, active_Temporary);
                else if (PlayerStats.Instance.stats.abilitiesGot_Permanent.HandDrill)
                    ShowImage(active_Permanent, passive_Permanent);
                break;
            case Abilities.SpringShoes:
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.SpringShoes)
                    ShowImage(passive_Temporary, active_Temporary);
                else if (PlayerStats.Instance.stats.abilitiesGot_Permanent.SpringShoes)
                    ShowImage(active_Permanent, passive_Permanent);
                break;
            case Abilities.ClimingGloves:
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.ClimingGloves)
                    ShowImage(passive_Temporary, active_Temporary);
                else if (PlayerStats.Instance.stats.abilitiesGot_Permanent.ClimingGloves)
                    ShowImage(active_Permanent, passive_Permanent);
                break;
            case Abilities.GrapplingHook:
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.GrapplingHook)
                    ShowImage(passive_Temporary, active_Temporary);
                else if (PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook)
                    ShowImage(active_Permanent, passive_Permanent);
                break;

            default:
                break;
        }
    }


    //--------------------


    void HideAllImages()
    {
        passive_Temporary.SetActive(false);
        passive_Permanent.SetActive(false);

        active_Temporary.SetActive(false);
        active_Permanent.SetActive(false);
    }
    void ShowImage(GameObject obj_Passive, GameObject obj_Active)
    {
        obj_Passive.SetActive(true);
        obj_Active.SetActive(true);
    }
}
