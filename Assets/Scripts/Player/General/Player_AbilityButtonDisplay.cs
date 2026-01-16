using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_AbilityButtonDisplay : MonoBehaviour
{
    [Header("Ability Parent")]
    [SerializeField] GameObject abilityParent;

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

    bool swim_FirstTime;
    bool freeSwim_FirstTime;


    //--------------------


    private void OnEnable()
    {
        if (MapManager.Instance.haveIntroSequence)
            MapManager.Action_EndIntroSequence += UpdateDisplay;
        else
            DataManager.Action_dataHasLoaded += UpdateDisplay;

        Interactable_Pickup.Action_AbilityPickupGot += UpdateDisplay;

        Movement.Action_StepTaken += CheckIfStandingInWater;

        Movement.Action_isDashing += Ability_Dash_Activation;
        Movement.Action_isDashing_Finished += Ability_Dash_Deactivation;
        Movement.Action_isJumping += Ability_Jump_Activation;
        Movement.Action_isJumping_Finished += Ability_Jump_Deactivation;

        Movement.Action_isSwiftSwim += Ability_SwiftSwim_Activation;
        Movement.Action_isSwiftSwim_Finished += Ability_SwiftSwim_Deactivation;

        Movement.Action_isAscending += Ability_Ascend_Activation;
        Movement.Action_isAscending_Finished += Ability_Ascend_Deactivation;
        Movement.Action_isDescending += Ability_Descend_Activation;
        Movement.Action_isDescending_Finished += Ability_Descend_Deactivation;

        Movement.Action_isGrapplingHooking += Ability_GrapplingHook_Activation;
        Movement.Action_isGrapplingHooking_Finished += Ability_GrapplingHook_Deactivation;

        Player_CeilingGrab.Action_isCeilingGrabbing += Ability_CeilingGrab_Activation;
        Player_CeilingGrab.Action_isCeilingGrabbing_Finished += Ability_CeilingGrab_Deactivation;
    }
    private void OnDisable()
    {
        if (MapManager.Instance.haveIntroSequence)
            MapManager.Action_EndIntroSequence -= UpdateDisplay;
        else
            DataManager.Action_dataHasLoaded -= UpdateDisplay;

        Interactable_Pickup.Action_AbilityPickupGot -= UpdateDisplay;

        Movement.Action_StepTaken -= CheckIfStandingInWater;

        Movement.Action_isDashing -= Ability_Dash_Activation;
        Movement.Action_isDashing_Finished -= Ability_Dash_Deactivation;
        Movement.Action_isJumping -= Ability_Jump_Activation;
        Movement.Action_isJumping_Finished -= Ability_Jump_Deactivation;

        Movement.Action_isSwiftSwim -= Ability_SwiftSwim_Activation;
        Movement.Action_isSwiftSwim_Finished -= Ability_SwiftSwim_Deactivation;

        Movement.Action_isAscending -= Ability_Ascend_Activation;
        Movement.Action_isAscending_Finished -= Ability_Ascend_Deactivation;
        Movement.Action_isDescending -= Ability_Descend_Activation;
        Movement.Action_isDescending_Finished -= Ability_Descend_Deactivation;

        Movement.Action_isGrapplingHooking -= Ability_GrapplingHook_Activation;
        Movement.Action_isGrapplingHooking_Finished -= Ability_GrapplingHook_Deactivation;

        Player_CeilingGrab.Action_isCeilingGrabbing -= Ability_CeilingGrab_Activation;
        Player_CeilingGrab.Action_isCeilingGrabbing_Finished -= Ability_CeilingGrab_Deactivation;
    }


    //--------------------


    void CheckIfStandingInWater()
    {
        if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
        {
            if (!swim_FirstTime && (PlayerStats.Instance.stats.abilitiesGot_Temporary.Snorkel || PlayerStats.Instance.stats.abilitiesGot_Permanent.Snorkel))
            {
                Ability_Swim_Activation();
                swim_FirstTime = true;
            }

            if (!freeSwim_FirstTime && (PlayerStats.Instance.stats.abilitiesGot_Temporary.OxygenTank || PlayerStats.Instance.stats.abilitiesGot_Permanent.OxygenTank))
            {
                Ability_FreeSwim_Activation();
                freeSwim_FirstTime = true;
            }
        }
        else
        {
            if (swim_FirstTime && (PlayerStats.Instance.stats.abilitiesGot_Temporary.Snorkel || PlayerStats.Instance.stats.abilitiesGot_Permanent.Snorkel))
            {
                Ability_Swim_Deactivation();
                swim_FirstTime = false;
            }

            if (freeSwim_FirstTime && (PlayerStats.Instance.stats.abilitiesGot_Temporary.OxygenTank || PlayerStats.Instance.stats.abilitiesGot_Permanent.OxygenTank))
            {
                Ability_FreeSwim_Deactivation();
                freeSwim_FirstTime = false;
            }
        }
    }


    //--------------------


    void UpdateDisplay()
    {
        abilityParent.SetActive(true);

        Ability_Appearance(abilitySprite_Swim, PlayerStats.Instance.stats.abilitiesGot_Temporary.Snorkel, PlayerStats.Instance.stats.abilitiesGot_Permanent.Snorkel);
        Ability_Appearance(abilitySprite_SwiftSwim, PlayerStats.Instance.stats.abilitiesGot_Temporary.Flippers, PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers);
        Ability_Appearance(abilitySprite_FreeSwim, PlayerStats.Instance.stats.abilitiesGot_Temporary.OxygenTank, PlayerStats.Instance.stats.abilitiesGot_Permanent.OxygenTank);

        Ability_Appearance(abilitySprite_Ascend, PlayerStats.Instance.stats.abilitiesGot_Temporary.DrillHelmet, PlayerStats.Instance.stats.abilitiesGot_Permanent.DrillHelmet);
        Ability_Appearance(abilitySprite_Descend, PlayerStats.Instance.stats.abilitiesGot_Temporary.DrillBoots, PlayerStats.Instance.stats.abilitiesGot_Permanent.DrillBoots);

        Ability_Appearance(abilitySprite_Dash, PlayerStats.Instance.stats.abilitiesGot_Temporary.HandDrill, PlayerStats.Instance.stats.abilitiesGot_Permanent.HandDrill);
        Ability_Appearance(abilitySprite_Jump, PlayerStats.Instance.stats.abilitiesGot_Temporary.SpringShoes, PlayerStats.Instance.stats.abilitiesGot_Permanent.SpringShoes);
        Ability_Appearance(abilitySprite_CeilingGrab, PlayerStats.Instance.stats.abilitiesGot_Temporary.ClimingGloves, PlayerStats.Instance.stats.abilitiesGot_Permanent.ClimingGloves);
        Ability_Appearance(abilitySprite_GrapplingHook, PlayerStats.Instance.stats.abilitiesGot_Temporary.GrapplingHook, PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook);
    }
    void Ability_Appearance(GameObject abilityObject, bool tempAbility, bool permAbility)
    {
        if (tempAbility || permAbility)
        {
            if (!abilityObject.GetComponent<AbilityUIAnimator>().isActive)
                abilityObject.GetComponent<RectTransform>().localScale = Vector3.zero;

            abilityObject.SetActive(true);

            if (!abilityObject.GetComponent<AbilityUIAnimator>().isActive)
                abilityObject.GetComponent<AbilityUIAnimator>().Appearing();
        }
        else
        {
            abilityObject.SetActive(false);
        }
    }


    //--------------------


    void Ability_Swim_Activation()
    {
        abilitySprite_Swim.GetComponent<AbilityUIAnimator>().Activating();
    }
    void Ability_Swim_Deactivation()
    {
        abilitySprite_Swim.GetComponent<AbilityUIAnimator>().Deactivating();
    }

    void Ability_SwiftSwim_Activation()
    {
        abilitySprite_SwiftSwim.GetComponent<AbilityUIAnimator>().Activating();
    }
    void Ability_SwiftSwim_Deactivation()
    {
        abilitySprite_SwiftSwim.GetComponent<AbilityUIAnimator>().Deactivating();
    }

    void Ability_FreeSwim_Activation()
    {
        abilitySprite_FreeSwim.GetComponent<AbilityUIAnimator>().Activating();
    }
    void Ability_FreeSwim_Deactivation()
    {
        abilitySprite_FreeSwim.GetComponent<AbilityUIAnimator>().Deactivating();
    }


    void Ability_Ascend_Activation()
    {
        abilitySprite_Ascend.GetComponent<AbilityUIAnimator>().Activating();
    }
    void Ability_Ascend_Deactivation()
    {
        abilitySprite_Ascend.GetComponent<AbilityUIAnimator>().Deactivating();
    }
    void Ability_Descend_Activation()
    {
        abilitySprite_Descend.GetComponent<AbilityUIAnimator>().Activating();
    }
    void Ability_Descend_Deactivation()
    {
        abilitySprite_Descend.GetComponent<AbilityUIAnimator>().Deactivating();
    }


    void Ability_Dash_Activation()
    {
        abilitySprite_Dash.GetComponent<AbilityUIAnimator>().Activating();
    }
    void Ability_Dash_Deactivation()
    {
        abilitySprite_Dash.GetComponent<AbilityUIAnimator>().Deactivating();
    }
    void Ability_Jump_Activation()
    {
        abilitySprite_Jump.GetComponent<AbilityUIAnimator>().Activating();
    }
    void Ability_Jump_Deactivation()
    {
        abilitySprite_Jump.GetComponent<AbilityUIAnimator>().Deactivating();
    }
    void Ability_CeilingGrab_Activation()
    {
        abilitySprite_CeilingGrab.GetComponent<AbilityUIAnimator>().Activating();
    }
    void Ability_CeilingGrab_Deactivation()
    {
        abilitySprite_CeilingGrab.GetComponent<AbilityUIAnimator>().Deactivating();
    }
    void Ability_GrapplingHook_Activation()
    {
        abilitySprite_GrapplingHook.GetComponent<AbilityUIAnimator>().Activating();
    }
    void Ability_GrapplingHook_Deactivation()
    {
        abilitySprite_GrapplingHook.GetComponent<AbilityUIAnimator>().Deactivating();
    }
}
