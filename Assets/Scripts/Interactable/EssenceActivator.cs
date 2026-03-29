using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceActivator : MonoBehaviour
{
    [Header("Abilities Needed to Aquire Essence")]
    [SerializeField] bool needAbility_Snorkel;
    [SerializeField] bool needAbility_OxygenTank;
    [SerializeField] bool needAbility_Flipper;
    [SerializeField] bool needAbility_DrillHelmet;
    [SerializeField] bool needAbility_DrillBoots;
    [SerializeField] bool needAbility_HandDrill;
    [SerializeField] bool needAbility_GrapplingHook;
    [SerializeField] bool needAbility_SpringShoes;
    [SerializeField] bool needAbility_ClimbingGloves;

    [Header("MeshRenderer")]
    [SerializeField] MeshRenderer meshRenderer;

    [Header("Materials")]
    [SerializeField] Material material_Unactive;
    [SerializeField] Material material_Active;

    [Header("Shine Objects")]
    [SerializeField] GameObject obj_ShineHor;
    [SerializeField] GameObject obj_ShineDia;


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += SetMaterial;
        Interactable_Pickup.Action_AbilityPickupGot += SetMaterial;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= SetMaterial;
        Interactable_Pickup.Action_AbilityPickupGot -= SetMaterial;
    }


    //--------------------


    void SetMaterial()
    {
        if (CheckIfActive())
        {
            meshRenderer.material = material_Active;
            obj_ShineHor.SetActive(true);
            obj_ShineDia.SetActive(true);
        }
        else
        {
            meshRenderer.material = material_Unactive;
            obj_ShineHor.SetActive(false);
            obj_ShineDia.SetActive(false);
        }
    }

    bool CheckIfActive()
    {
        if (needAbility_Snorkel && !PlayerStats.Instance.stats.abilitiesGot_Temporary.Snorkel && !PlayerStats.Instance.stats.abilitiesGot_Permanent.Snorkel) return false;
        if (needAbility_OxygenTank && !PlayerStats.Instance.stats.abilitiesGot_Temporary.OxygenTank && !PlayerStats.Instance.stats.abilitiesGot_Permanent.OxygenTank) return false;
        if (needAbility_Flipper && !PlayerStats.Instance.stats.abilitiesGot_Temporary.Flippers && !PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers) return false;

        if (needAbility_DrillHelmet && !PlayerStats.Instance.stats.abilitiesGot_Temporary.DrillHelmet && !PlayerStats.Instance.stats.abilitiesGot_Permanent.DrillHelmet) return false;
        if (needAbility_DrillBoots && !PlayerStats.Instance.stats.abilitiesGot_Temporary.DrillBoots && !PlayerStats.Instance.stats.abilitiesGot_Permanent.DrillBoots) return false;

        if (needAbility_HandDrill && !PlayerStats.Instance.stats.abilitiesGot_Temporary.HandDrill && !PlayerStats.Instance.stats.abilitiesGot_Permanent.HandDrill) return false;
        if (needAbility_GrapplingHook && !PlayerStats.Instance.stats.abilitiesGot_Temporary.GrapplingHook && !PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook) return false;
        if (needAbility_SpringShoes && !PlayerStats.Instance.stats.abilitiesGot_Temporary.SpringShoes && !PlayerStats.Instance.stats.abilitiesGot_Permanent.SpringShoes) return false;
        if (needAbility_ClimbingGloves && !PlayerStats.Instance.stats.abilitiesGot_Temporary.ClimingGloves && !PlayerStats.Instance.stats.abilitiesGot_Permanent.ClimingGloves) return false;

        print("1. Return True");
        return true;
    }
}
