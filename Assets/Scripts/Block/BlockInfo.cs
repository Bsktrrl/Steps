using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInfo : MonoBehaviour
{
    [Header("Block Stats")]
    public BlockElement blockElement;
    public BlockType blockType;
    public BlockAbility blockAbility;
    public int movementCost;
    public float movementSpeed;
    public Color stepCostText_Color;

    [Header("Material Rendering")]
    List<Renderer> objectRenderers = new List<Renderer>();
    public List<MaterialPropertyBlock> propertyBlocks = new List<MaterialPropertyBlock>();


    //--------------------


    private void Start()
    {
        Player_Movement.Action_resetBlockColor += ResetColor;
        Player_Stats.Action_RespawnToSavePos += ResetColor;

        //Set objectRenderers
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<MeshRenderer>())
            {
                objectRenderers.Add(transform.GetChild(i).GetComponent<MeshRenderer>());
            }
        }

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
        //Don't darken Fences
        if (blockType == BlockType.Fence) { return; }

        //Darken all materials attached
        for (int i = 0; i < propertyBlocks.Count; i++)
        {
            // Darken the color
            Color darkenedColor = Color.white * BlockManager.Instance.materialDarkeningValue;

            // Set the new color in the MaterialPropertyBlock
            propertyBlocks[i].SetColor("_BaseColor", darkenedColor);

            // Apply the MaterialPropertyBlock to the renderer
            objectRenderers[i].SetPropertyBlock(propertyBlocks[i]);

            //Show StepCost
            if (gameObject.GetComponent<BlockStepCostDisplay>())
            {
                gameObject.GetComponent<BlockStepCostDisplay>().ShowDisplay();
            }
        }
    }

    public void ResetColor()
    {
        for (int i = 0; i < propertyBlocks.Count; i++)
        {
            // Restore the color to full brightness
            Color restoredColor = Color.white;

            // Set the original color in the MaterialPropertyBlock
            propertyBlocks[i].SetColor("_BaseColor", restoredColor);

            // Apply the MaterialPropertyBlock to the renderer
            objectRenderers[i].SetPropertyBlock(propertyBlocks[i]);

            //Hide StepCost
            if (gameObject.GetComponent<BlockStepCostDisplay>())
            {
                gameObject.GetComponent<BlockStepCostDisplay>().HideDisplay();
            }
        }
    }
}