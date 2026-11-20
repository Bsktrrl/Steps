using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public List<HintSetup> HintSetups_List = new List<HintSetup>();

    [Min(0f)] public float hintDuration = 4f;
    [Min(0.01f)] public float hintFadeDuration = 0.25f;

    static readonly int SmokeOpacity = Shader.PropertyToID("_SmokeOpacity");
    static readonly int LightOpacity = Shader.PropertyToID("_LightOpacity");
    readonly List<HintFade> activeFades = new List<HintFade>();
    HintSetup activeSetup;
    GameObject activeEffect;
    Coroutine hintRoutine;
    float currentOpacity;
    bool fadeOutRequested;
    bool restartRequested;

    public bool CheckIfHint() => FindHintEffect(FindMatchingSetup()) != null;

    public void PerformHint()
    {
        if (!isActiveAndEnabled) return;

        print("4. OnHintButton");

        if (hintRoutine != null)
        {
            restartRequested = true;
            fadeOutRequested = true;
            return;
        }

        activeSetup = FindMatchingSetup();
        activeEffect = FindHintEffect(activeSetup);
        if (!activeEffect) return;

        foreach (var renderer in activeEffect.GetComponentsInChildren<Renderer>(true))
        {
            print("5. OnHintButton");

            var materials = renderer.sharedMaterials;
            for (int i = 0; i < materials.Length; i++)
            {
                var material = materials[i];
                if (!material) continue;

                if (material.HasProperty(SmokeOpacity))
                    activeFades.Add(new HintFade(renderer, i, material, SmokeOpacity));
                else if (material.HasProperty(LightOpacity))
                    activeFades.Add(new HintFade(renderer, i, material, LightOpacity));
            }
        }

        SetOpacity(0f);
        activeEffect.SetActive(true);
        hintRoutine = StartCoroutine(PlayHint());
    }

    HintSetup FindMatchingSetup()
    {
        if (!TryGetComponent<MapManager>(out _) || HintSetups_List == null)
            return null;

        var player = MapManager.SpawnedPlayer;
        if (!player || player.scene != gameObject.scene || !player.TryGetComponent(out PlayerStats playerStats) ||
            playerStats.stats == null)
            return null;

        Stats stats = playerStats.stats;
        var ownedAbilities = new HashSet<Abilities>();
        // Only abilities used by this level's hint rows affect matching.
        foreach (var setup in HintSetups_List)
        {
            if (setup?.abilitiesGot == null) continue;
            foreach (var ability in setup.abilitiesGot)
            {
                if (ability != Abilities.None && (stats.abilitiesGot_Temporary?.HasAbility(ability) == true ||
                    stats.abilitiesGot_Permanent?.HasAbility(ability) == true))
                    ownedAbilities.Add(ability);
            }
        }

        // The first matching row can intentionally leave its checkpoint unassigned.
        foreach (var setup in HintSetups_List)
            if (setup != null && setup.maxStepsGot == stats.steps_Max && MatchesAbilities(setup, ownedAbilities))
                return setup;

        return null;
    }

    GameObject FindHintEffect(HintSetup setup)
    {
        var checkpoint = setup?.checkpointToLighten;
        if (!checkpoint || checkpoint.scene != gameObject.scene)
            return null;

        foreach (var hint in checkpoint.GetComponentsInChildren<HintObject>(true))
            if (hint.hintObject)
                return hint.hintObject;
        return null;
    }

    static bool MatchesAbilities(HintSetup setup, HashSet<Abilities> ownedAbilities)
    {
        if (setup.abilitiesGot == null) return ownedAbilities.Count == 0;
        foreach (var ability in setup.abilitiesGot)
            if (ability != Abilities.None && !ownedAbilities.Contains(ability))
                return false;
        foreach (var ability in ownedAbilities)
            if (!setup.abilitiesGot.Contains(ability))
                return false;
        return true;
    }

    IEnumerator PlayHint()
    {
        print("6. OnHintButton");

        yield return FadeHint(0f, 1f);
        for (float elapsed = 0f; !fadeOutRequested && elapsed < hintDuration; elapsed += Time.deltaTime)
            yield return null;

        fadeOutRequested = true;
        yield return FadeHint(currentOpacity, 0f);
        ClearHint();
        hintRoutine = null;

        if (restartRequested)
        {
            restartRequested = false;
            PerformHint();
        }
    }

    IEnumerator FadeHint(float from, float to)
    {
        print("7. OnHintButton");

        float duration = Mathf.Max(0.01f, hintFadeDuration);
        bool fadingIn = to > from;
        // Keep intermediate frames visible even when a pickup causes a long frame.
        for (float elapsed = 0f; activeEffect && elapsed < duration;
            elapsed += Mathf.Min(Time.unscaledDeltaTime, duration / 4f))
        {
            if (fadingIn && fadeOutRequested)
                yield break;

            SetOpacity(Mathf.SmoothStep(from, to, elapsed / duration));
            yield return null;
        }

        if (!fadingIn || !fadeOutRequested)
            SetOpacity(to);
    }

    void SetOpacity(float opacity)
    {
        currentOpacity = opacity;
        foreach (var fade in activeFades)
            fade.SetOpacity(opacity);
    }

    void OnEnable()
    {
        Interactable_Pickup.Action_AbilityPickupGot += OnProgressPickup;
        Interactable_Pickup.Action_FootprintPickupGot += OnProgressPickup;
    }

    void OnDisable()
    {
        Interactable_Pickup.Action_AbilityPickupGot -= OnProgressPickup;
        Interactable_Pickup.Action_FootprintPickupGot -= OnProgressPickup;
        StopHint();
    }

    void OnProgressPickup()
    {
        if (activeEffect && !fadeOutRequested && FindMatchingSetup() != activeSetup)
            fadeOutRequested = true;
    }

    void StopHint()
    {
        restartRequested = false;
        if (hintRoutine != null)
        {
            StopCoroutine(hintRoutine);
            hintRoutine = null;
        }
        ClearHint();
    }

    void ClearHint()
    {
        if (activeEffect)
            activeEffect.SetActive(false);
        activeEffect = null;
        activeSetup = null;
        fadeOutRequested = false;

        foreach (var fade in activeFades)
            fade.Restore();
        activeFades.Clear();
    }

    sealed class HintFade
    {
        readonly Renderer renderer;
        readonly int materialIndex;
        readonly int property;
        readonly float fullOpacity;
        readonly MaterialPropertyBlock original = new MaterialPropertyBlock();
        readonly MaterialPropertyBlock faded = new MaterialPropertyBlock();

        public HintFade(Renderer renderer, int materialIndex, Material material, int property)
        {
            this.renderer = renderer;
            this.materialIndex = materialIndex;
            this.property = property;
            renderer.GetPropertyBlock(original, materialIndex);
            if (original.isEmpty)
                renderer.GetPropertyBlock(faded);
            else
                renderer.GetPropertyBlock(faded, materialIndex);

            fullOpacity = faded.HasFloat(property) ? faded.GetFloat(property) : material.GetFloat(property);
        }

        public void SetOpacity(float opacity)
        {
            if (!renderer) return;
            faded.SetFloat(property, fullOpacity * opacity);
            renderer.SetPropertyBlock(faded, materialIndex);
        }

        public void Restore()
        {
            if (renderer)
                renderer.SetPropertyBlock(original.isEmpty ? null : original, materialIndex);
        }
    }
}

[Serializable]
public class HintSetup
{
    [Tooltip("Exact maximum step count shown in the HUD, including the starting steps.")]
    [Min(0)] public int maxStepsGot;
    [Tooltip("Exact set of owned abilities from this level's hint rows. Both temporary and permanent abilities count.")]
    public List<Abilities> abilitiesGot = new List<Abilities>();

    [Tooltip("Assign a checkpoint with a HintObject on it or on one of its children.")]
    public GameObject checkpointToLighten;
}
