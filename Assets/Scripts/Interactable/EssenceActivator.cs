using System;
using System.Collections.Generic;
using UnityEngine;

public class EssenceActivator : RequirementEffectsActivator
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


    protected override IReadOnlyList<EssenceRequirements> Requirements => essenceRequirementsList;

    protected override void ApplyEffects(bool showActiveEffects)
    {
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
