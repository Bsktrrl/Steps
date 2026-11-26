using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RequirementEffectsActivator : MonoBehaviour
{
    protected abstract IReadOnlyList<EssenceRequirements> Requirements { get; }
    protected abstract void ApplyEffects(bool active);

    private Coroutine loadRefresh;

    protected void OnEnable()
    {
        DataManager.Action_dataHasLoaded += RefreshAfterLoad;
        Interactable_Pickup.Action_AbilityPickupGot += RefreshEffects;
        Interactable_Pickup.Action_FootprintPickupGot += RefreshEffects;

        if (DataManager.ExistingInstance is { dataHasLoaded_Check: true })
            RefreshAfterLoad();
    }

    protected void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= RefreshAfterLoad;
        Interactable_Pickup.Action_AbilityPickupGot -= RefreshEffects;
        Interactable_Pickup.Action_FootprintPickupGot -= RefreshEffects;

        if (loadRefresh != null)
        {
            StopCoroutine(loadRefresh);
            loadRefresh = null;
        }
    }

    private void RefreshAfterLoad()
    {
        if (isActiveAndEnabled && loadRefresh == null)
            loadRefresh = StartCoroutine(RefreshAfterPlayerSetup());
    }

    private IEnumerator RefreshAfterPlayerSetup()
    {
        // Give the other load listeners time to restore the level's maximum steps and abilities.
        yield return null;
        loadRefresh = null;
        RefreshEffects();
    }

    private void RefreshEffects()
    {
        if (!isActiveAndEnabled)
            return;

        GameObject player = MapManager.SpawnedPlayer;
        Stats stats = player && player.scene == gameObject.scene && player.TryGetComponent(out PlayerStats playerStats)
            ? playerStats.stats
            : null;
        ApplyEffects(ShouldShowActiveEffects(stats));
    }

    private bool ShouldShowActiveEffects(Stats stats)
    {
        var requirements = Requirements;
        if (requirements == null || requirements.Count == 0)
            return true;
        if (stats == null)
            return false;

        foreach (EssenceRequirements entry in requirements)
        {
            if (entry != null && RequirementsAreMet(entry, stats))
                return true;
        }

        return false;
    }

    private static bool RequirementsAreMet(EssenceRequirements requirements, Stats stats)
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

    private static bool HasAbility(Stats stats, Abilities ability)
    {
        return stats.abilitiesGot_Temporary?.HasAbility(ability) == true ||
               stats.abilitiesGot_Permanent?.HasAbility(ability) == true;
    }
}
