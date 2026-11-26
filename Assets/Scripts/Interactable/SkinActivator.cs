using System.Collections.Generic;
using UnityEngine;

public class SkinActivator : RequirementEffectsActivator
{
    [Header("Skin Material Requirements")]
    [SerializeField] List<EssenceRequirements> skinRequirementsList = new List<EssenceRequirements>();

    [Header("MeshRenderer")]
    [SerializeField] List<MeshRenderer> meshRenderer = new List<MeshRenderer>();

    [Header("Materials")]
    [SerializeField] Material material_Unactive;
    [SerializeField] Material material_Active;

    [Header("Shine Objects")]
    [SerializeField] GameObject obj_ShineHor;
    [SerializeField] GameObject obj_ShineDia;

    [Header("Idle Sound")]
    [SerializeField] AudioSource idleSound;

    protected override IReadOnlyList<EssenceRequirements> Requirements => skinRequirementsList;

    protected override void ApplyEffects(bool active)
    {
        Material material = active ? material_Active : material_Unactive;
        if (material)
        {
            foreach (MeshRenderer renderer in meshRenderer)
            {
                if (renderer)
                    renderer.sharedMaterial = material;
            }
        }

        if (obj_ShineHor) obj_ShineHor.SetActive(active);
        if (obj_ShineDia) obj_ShineDia.SetActive(active);

        if (!idleSound)
            return;
        if (active && !idleSound.isPlaying)
            idleSound.Play();
        else if (!active && idleSound.isPlaying)
            idleSound.Stop();
    }
}
