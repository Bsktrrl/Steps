using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInfo : MonoBehaviour
{
    [Header("Block Stats")]
    public BlockElement blockElement;
    public BlockType blockType;

    public Material material;
    Color originalColor;


    //--------------------


    private void Start()
    {
        //NewPlayerMovement.darkenBlockColor += ChangeMaterialDarkness;
        //NewPlayerMovement.resetBlockColor += ResetMaterialDarkness;

        Color originalColor = material.color;
    }


    //--------------------


    public void ChangeMaterialDarkness()
    {
        // Darken the color by multiplying the RGB values
        Color darkenedColor = originalColor * BlockManager.Instance.materialDarkenAmount;

        // Apply the darkened color to the material
        material.color = darkenedColor;
    }
    void ResetMaterialDarkness()
    {
        if (material.color != originalColor)
        {
            material.color = originalColor;
        }
    }
}