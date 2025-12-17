using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_AbilityButtonDisplay : MonoBehaviour
{
    [Header("Ability Displays")]
    [SerializeField] GameObject abilitySprite_Swim;
    [SerializeField] GameObject abilitySprite_SwiftSwim;
    [SerializeField] GameObject abilitySprite_FreeSwim;
    [SerializeField] GameObject abilitySprite_Ascend;
    [SerializeField] GameObject abilitySprite_Descend;
    [SerializeField] GameObject abilitySprite_Dash;
    [SerializeField] GameObject abilitySprite_Jump;
    [SerializeField] GameObject abilitySprite_CeilingGrab;
    [SerializeField] GameObject abilitySprite_GrapplingHook;


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += UpdateDisplay;
        Interactable_Pickup.Action_AbilityPickupGot += UpdateDisplay;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= UpdateDisplay;
        Interactable_Pickup.Action_AbilityPickupGot -= UpdateDisplay;
    }


    //--------------------


    void UpdateDisplay()
    {
        //Swim
        if (PlayerStats.Instance.stats.abilitiesGot_Temporary.SwimSuit || PlayerStats.Instance.stats.abilitiesGot_Permanent.SwimSuit)
            abilitySprite_Swim.SetActive(true);
        else
            abilitySprite_Swim.SetActive(false);

        //SwiftSwim
        if (PlayerStats.Instance.stats.abilitiesGot_Temporary.SwiftSwim || PlayerStats.Instance.stats.abilitiesGot_Permanent.SwiftSwim)
            abilitySprite_SwiftSwim.SetActive(true);
        else
            abilitySprite_SwiftSwim.SetActive(false);

        //Flippers
        if (PlayerStats.Instance.stats.abilitiesGot_Temporary.Flippers || PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers)
            abilitySprite_FreeSwim.SetActive(true);
        else
            abilitySprite_FreeSwim.SetActive(false);

        //Ascend
        if (PlayerStats.Instance.stats.abilitiesGot_Temporary.Ascend || PlayerStats.Instance.stats.abilitiesGot_Permanent.Ascend)
            abilitySprite_Ascend.SetActive(true);
        else
            abilitySprite_Ascend.SetActive(false);

        //Descend
        if (PlayerStats.Instance.stats.abilitiesGot_Temporary.Descend || PlayerStats.Instance.stats.abilitiesGot_Permanent.Descend)
            abilitySprite_Descend.SetActive(true);
        else
            abilitySprite_Descend.SetActive(false);

        //Dash
        if (PlayerStats.Instance.stats.abilitiesGot_Temporary.Dash || PlayerStats.Instance.stats.abilitiesGot_Permanent.Dash)
            abilitySprite_Dash.SetActive(true);
        else
            abilitySprite_Dash.SetActive(false);

        //Jumping
        if (PlayerStats.Instance.stats.abilitiesGot_Temporary.Jumping || PlayerStats.Instance.stats.abilitiesGot_Permanent.Jumping)
            abilitySprite_Jump.SetActive(true);
        else
            abilitySprite_Jump.SetActive(false);

        //CeilingGrab
        if (PlayerStats.Instance.stats.abilitiesGot_Temporary.CeilingGrab || PlayerStats.Instance.stats.abilitiesGot_Permanent.CeilingGrab)
            abilitySprite_CeilingGrab.SetActive(true);
        else
            abilitySprite_CeilingGrab.SetActive(false);

        //GrapplingHook
        if (PlayerStats.Instance.stats.abilitiesGot_Temporary.GrapplingHook || PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook)
            abilitySprite_GrapplingHook.SetActive(true);
        else
            abilitySprite_GrapplingHook.SetActive(false);
    }
}
