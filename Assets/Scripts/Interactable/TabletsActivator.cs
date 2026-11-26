using System.Collections.Generic;
using UnityEngine;

public class TabletsActivator : RequirementEffectsActivator
{
    [Header("Tablet Material and Text Material Requirements")]
    [SerializeField] List<EssenceRequirements> tabletRequirementsList = new List<EssenceRequirements>();

    [Header("MeshRenderer")]
    [SerializeField] List<MeshRenderer> meshRenderer = new List<MeshRenderer>();

    [Header("Materials")]
    [SerializeField] Material material_Unactive;
    [SerializeField] Material material_Active;
    [SerializeField] Material material_Text_Unactive;
    [SerializeField] Material material_Text_Active;

    [Header("Shine Objects")]
    [SerializeField] GameObject obj_ShineHor;
    [SerializeField] GameObject obj_ShineDia;

    private readonly List<Material> materials = new List<Material>(2);

    protected override IReadOnlyList<EssenceRequirements> Requirements => tabletRequirementsList;

    protected override void ApplyEffects(bool active)
    {
        Material baseMaterial = active ? material_Active : material_Unactive;
        Material tracksMaterial = active ? material_Text_Active : material_Text_Unactive;

        foreach (MeshRenderer renderer in meshRenderer)
        {
            if (!renderer)
                continue;

            renderer.GetSharedMaterials(materials);
            if (baseMaterial && materials.Count > 0)
                materials[0] = baseMaterial;
            if (tracksMaterial && materials.Count > 1)
                materials[1] = tracksMaterial;
            renderer.SetSharedMaterials(materials);
        }

        if (obj_ShineHor) obj_ShineHor.SetActive(active);
        if (obj_ShineDia) obj_ShineDia.SetActive(active);
    }
}
