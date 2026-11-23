using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceActivator : MonoBehaviour
{
    [Header("Essence Glow and Idle Sound Requirements")]
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
        DataManager.Action_dataHasLoaded += RefreshAfterLoad;
        Interactable_Pickup.Action_AbilityPickupGot += SetMaterial;
        Interactable_Pickup.Action_FootprintPickupGot += SetMaterial;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= RefreshAfterLoad;
        Interactable_Pickup.Action_AbilityPickupGot -= SetMaterial;
        Interactable_Pickup.Action_FootprintPickupGot -= SetMaterial;
    }


    //--------------------



    void RefreshAfterLoad()
    {
        StartCoroutine(RefreshAfterPlayerSetup());
    }

    IEnumerator RefreshAfterPlayerSetup()
    {
        // The other load listeners restore the current level's maximum steps.
        yield return null;
        SetMaterial();
    }

    void SetMaterial()
    {
        GameObject player = MapManager.SpawnedPlayer;
        Stats stats = player && player.scene == gameObject.scene && player.TryGetComponent(out PlayerStats playerStats)
            ? playerStats.stats
            : null;
        bool showActiveEffects = ShouldShowActiveEffects(stats);

        if (meshRenderer)
            meshRenderer.material = showActiveEffects ? material_Active : material_Unactive;
        if (obj_ShineHor)
            obj_ShineHor.SetActive(showActiveEffects);
        if (obj_ShineDia)
            obj_ShineDia.SetActive(showActiveEffects);

        if (!idleSound)
            return;
        if (showActiveEffects && !idleSound.isPlaying)
            idleSound.Play();
        else if (!showActiveEffects && idleSound.isPlaying)
            idleSound.Stop();
    }

    bool ShouldShowActiveEffects(Stats stats)
    {
        // An unconfigured cube always shows its active effects.
        if (essenceRequirementsList == null || essenceRequirementsList.Count == 0)
            return true;
        if (stats == null)
            return false;

        foreach (EssenceRequirements requirements in essenceRequirementsList)
        {
            if (requirements != null && RequirementsAreMet(requirements, stats))
                return true;
        }

        return false;
    }

    static bool RequirementsAreMet(EssenceRequirements requirements, Stats stats)
    {
        return stats.steps_Max >= requirements.stepsRequirement &&
               (!requirements.needAbility_Snorkel || HasAbility(stats, Abilities.Snorkel)) &&
               (!requirements.needAbility_OxygenTank || HasAbility(stats, Abilities.OxygenTank)) &&
               (!requirements.needAbility_Flipper || HasAbility(stats, Abilities.Flippers)) &&
               (!requirements.needAbility_DrillHelmet || HasAbility(stats, Abilities.DrillHelmet)) &&
               (!requirements.needAbility_DrillBoots || HasAbility(stats, Abilities.DrillBoots)) &&
               (!requirements.needAbility_HandDrill || HasAbility(stats, Abilities.HandDrill)) &&
               (!requirements.needAbility_GrapplingHook || HasAbility(stats, Abilities.GrapplingHook)) &&
               (!requirements.needAbility_SpringShoes || HasAbility(stats, Abilities.SpringShoes)) &&
               (!requirements.needAbility_ClimbingGloves || HasAbility(stats, Abilities.ClimingGloves));
    }

    static bool HasAbility(Stats stats, Abilities ability)
    {
        return stats.abilitiesGot_Temporary?.HasAbility(ability) == true ||
               stats.abilitiesGot_Permanent?.HasAbility(ability) == true;
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
