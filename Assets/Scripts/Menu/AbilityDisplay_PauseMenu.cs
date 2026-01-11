using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDisplay_PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject ability_Swim;
    [SerializeField] GameObject ability_SwiftSwim;
    [SerializeField] GameObject ability_FreeSwim;

    [SerializeField] GameObject ability_Ascend;
    [SerializeField] GameObject ability_Descend;

    [SerializeField] GameObject ability_Dash;
    [SerializeField] GameObject ability_Jump;

    [SerializeField] GameObject ability_GrapplingHook;
    [SerializeField] GameObject ability_CeilingGrab;


    //--------------------


    private void OnEnable()
    {
        ShowAbilities();
    }


    //--------------------


    void ShowAbilities()
    {
        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.Snorkel || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.Snorkel)
            ability_Swim.SetActive(true);
        else
            ability_Swim.SetActive(false);

        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.Flippers || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.Flippers)
            ability_SwiftSwim.SetActive(true);
        else
            ability_SwiftSwim.SetActive(false);

        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.OxygenTank || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.OxygenTank)
            ability_FreeSwim.SetActive(true);
        else
            ability_FreeSwim.SetActive(false);


        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.DrillHelmet || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.DrillHelmet)
            ability_Ascend.SetActive(true);
        else
            ability_Ascend.SetActive(false);

        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.DrillBoots || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.DrillBoots)
            ability_Descend.SetActive(true);
        else
            ability_Descend.SetActive(false);


        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.HandDrill || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.HandDrill)
            ability_Dash.SetActive(true);
        else
            ability_Dash.SetActive(false);

        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.SpringShoes || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.SpringShoes)
            ability_Jump.SetActive(true);
        else
            ability_Jump.SetActive(false);


        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.GrapplingHook || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.GrapplingHook)
            ability_GrapplingHook.SetActive(true);
        else
            ability_GrapplingHook.SetActive(false);

        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.ClimingGloves || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.ClimingGloves)
            ability_CeilingGrab.SetActive(true);
        else
            ability_CeilingGrab.SetActive(false);
    }
}
