using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceActivator : MonoBehaviour
{
    [Header("Abilities Needed to Aquire Essence")]
    [SerializeField] List<EssenceRequirements> essenceRequirementsList = new List<EssenceRequirements>();

    [Header("MeshRenderer")]
    [SerializeField] MeshRenderer meshRenderer;

    [Header("Materials")]
    [SerializeField] Material material_Unactive;
    [SerializeField] Material material_Active;

    [Header("Shine Objects")]
    [SerializeField] GameObject obj_ShineHor;
    [SerializeField] GameObject obj_ShineDia;

    [Header("Idle Sound")]
    [SerializeField] AudioSource idleSound;


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += SetMaterial;
        Interactable_Pickup.Action_AbilityPickupGot += SetMaterial;
        Interactable_Pickup.Action_FootprintPickupGot += SetMaterial;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= SetMaterial;
        Interactable_Pickup.Action_AbilityPickupGot -= SetMaterial;
        Interactable_Pickup.Action_FootprintPickupGot -= SetMaterial;
    }


    //--------------------


    void SetMaterial()
    {
        if (CheckIfActive())
        {
            meshRenderer.material = material_Active;
            obj_ShineHor.SetActive(true);
            obj_ShineDia.SetActive(true);

            idleSound.Play();
        }
        else
        {
            meshRenderer.material = material_Unactive;
            obj_ShineHor.SetActive(false);
            obj_ShineDia.SetActive(false);

            idleSound.Stop();
        }
    }


    //--------------------


    bool CheckIfActive()
    {
        if (essenceRequirementsList.Count <= 0) return true;

        foreach (EssenceRequirements requirements in essenceRequirementsList)
        {
            if (RequirementsAreMet(requirements))
                return true;
        }

        return false;
    }

    bool RequirementsAreMet(EssenceRequirements requirements)
    {
        //Check Steps Requirement
        if (PlayerStats.Instance.stats.steps_Max < requirements.stepsRequirement) return false;

        //Check Abilities Requirement
        if (requirements.needAbility_Snorkel && !HasAbility(PlayerStats.Instance.stats.abilitiesGot_Temporary.Snorkel, PlayerStats.Instance.stats.abilitiesGot_Permanent.Snorkel)) return false;

        if (requirements.needAbility_OxygenTank && !HasAbility(PlayerStats.Instance.stats.abilitiesGot_Temporary.OxygenTank, PlayerStats.Instance.stats.abilitiesGot_Permanent.OxygenTank)) return false;

        if (requirements.needAbility_Flipper && !HasAbility(PlayerStats.Instance.stats.abilitiesGot_Temporary.Flippers, PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers)) return false;

        if (requirements.needAbility_DrillHelmet && !HasAbility(PlayerStats.Instance.stats.abilitiesGot_Temporary.DrillHelmet, PlayerStats.Instance.stats.abilitiesGot_Permanent.DrillHelmet)) return false;

        if (requirements.needAbility_DrillBoots && !HasAbility(PlayerStats.Instance.stats.abilitiesGot_Temporary.DrillBoots, PlayerStats.Instance.stats.abilitiesGot_Permanent.DrillBoots)) return false;

        if (requirements.needAbility_HandDrill && !HasAbility(PlayerStats.Instance.stats.abilitiesGot_Temporary.HandDrill, PlayerStats.Instance.stats.abilitiesGot_Permanent.HandDrill)) return false;

        if (requirements.needAbility_GrapplingHook && !HasAbility(PlayerStats.Instance.stats.abilitiesGot_Temporary.GrapplingHook, PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook)) return false;

        if (requirements.needAbility_SpringShoes && !HasAbility(PlayerStats.Instance.stats.abilitiesGot_Temporary.SpringShoes, PlayerStats.Instance.stats.abilitiesGot_Permanent.SpringShoes)) return false;

        if (requirements.needAbility_ClimbingGloves && !HasAbility(PlayerStats.Instance.stats.abilitiesGot_Temporary.ClimingGloves, PlayerStats.Instance.stats.abilitiesGot_Permanent.ClimingGloves)) return false;

        return true;
    }

    bool HasAbility(bool temporaryAbility, bool permanentAbility)
    {
        return temporaryAbility || permanentAbility;
    }
}

[Serializable]
public class EssenceRequirements
{
    [Header("Steps")]
    public int stepsRequirement;

    [Header("Abilities")]
    public bool needAbility_Snorkel;
    public bool needAbility_OxygenTank;
    public bool needAbility_Flipper;
    public bool needAbility_DrillHelmet;
    public bool needAbility_DrillBoots;
    public bool needAbility_HandDrill;
    public bool needAbility_GrapplingHook;
    public bool needAbility_SpringShoes;
    public bool needAbility_ClimbingGloves;
}