using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInfo : MonoBehaviour
{
    [Header("Block Stats")]
    public BlockElement blockElement;
    public BlockType blockType;

    [Header("Material Rendering")]
    public List<Renderer> objectRenderers = new List<Renderer>();
    List<MaterialPropertyBlock> propertyBlocks = new List<MaterialPropertyBlock>();


    //--------------------


    private void Start()
    {
        NewPlayerMovement.resetBlockColor += RestoreColors;

        // Initialize property blocks and get original colors
        for (int i = 0; i < objectRenderers.Count; i++)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            objectRenderers[i].GetPropertyBlock(block);
            propertyBlocks.Add(block);
        }
    }


    //--------------------


    public void DarkenColors()
    {
        for (int i = 0; i < propertyBlocks.Count; i++)
        {
            // Darken the color
            Color darkenedColor = Color.white * BlockManager.Instance.materialDarkenAmount;

            // Set the new color in the MaterialPropertyBlock
            propertyBlocks[i].SetColor("_Color", darkenedColor);

            // Apply the MaterialPropertyBlock to the renderer
            objectRenderers[i].SetPropertyBlock(propertyBlocks[i]);
        }
    }

    public void RestoreColors()
    {
        for (int i = 0; i < propertyBlocks.Count; i++)
        {
            // Restore the color to full brightness
            Color restoredColor = Color.white;

            // Set the original color in the MaterialPropertyBlock
            propertyBlocks[i].SetColor("_Color", restoredColor);

            // Apply the MaterialPropertyBlock to the renderer
            objectRenderers[i].SetPropertyBlock(propertyBlocks[i]);
        }
    }
}