using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInfo : MonoBehaviour
{
    [Header("Block Stats")]
    //public BlockElement blockElement;
    public BlockType blockType;
    public int movementCost;
    public float movementSpeed;
    public Color stepCostText_Color;

    [Header("Adjacent Blocks - Upper")]
    public GameObject upper_Front_Left;
    public GameObject upper_Front;
    public GameObject upper_Front_Right;
    public GameObject upper_Center_Left;
    public GameObject upper_Center;
    public GameObject upper_Center_Right;
    public GameObject upper_Back_Left;
    public GameObject upper_Back;
    public GameObject upper_Back_Right;

    [Header("Adjacent Blocks - Middle")]
    public GameObject center_Front_Left;
    public GameObject center_Front;
    public GameObject center_Front_Right;
    public GameObject center_Center_Left;
    public GameObject center_Center;
    public GameObject center_Center_Right;
    public GameObject center_Back_Left;
    public GameObject center_Back;
    public GameObject center_Back_Right;

    [Header("Adjacent Blocks - Lower")]
    public GameObject lower_Front_Left;
    public GameObject lower_Front;
    public GameObject lower_Front_Right;
    public GameObject lower_Center_Left;
    public GameObject lower_Center;
    public GameObject lower_Center_Right;
    public GameObject lower_Back_Left;
    public GameObject lower_Back;
    public GameObject lower_Back_Right;

    [Header("Material Rendering")]
    List<Renderer> objectRenderers = new List<Renderer>();
    public List<MaterialPropertyBlock> propertyBlocks = new List<MaterialPropertyBlock>();


    //--------------------


    private void Start()
    {
        Player_Movement.Action_resetBlockColor += ResetColor;
        Player_Stats.Action_RespawnToSavePos += ResetColor;

        SetObjectRenderer();
        SetPropertyBlock();
        GetAdjacentBlocksInfo();
    }


    //--------------------


    void SetObjectRenderer()
    {
        //Set objectRenderers
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<MeshRenderer>())
            {
                objectRenderers.Add(transform.GetChild(i).GetComponent<MeshRenderer>());
            }
        }
    }
    void SetPropertyBlock()
    {
        // Initialize property blocks and get original colors
        for (int i = 0; i < objectRenderers.Count; i++)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            objectRenderers[i].GetPropertyBlock(block);
            propertyBlocks.Add(block);
        }
    }
    
    void GetAdjacentBlocksInfo()
    {
        //Upper Layer
        upper_Front_Left = FindBlock(Vector3.up, Vector3.forward, Vector3.left);
        upper_Front = FindBlock(Vector3.up, Vector3.forward, Vector3.zero);
        upper_Front_Right = FindBlock(Vector3.up, Vector3.forward, Vector3.right);
        upper_Center_Left = FindBlock(Vector3.up, Vector3.zero, Vector3.left);
        upper_Center = FindBlock(Vector3.up, Vector3.zero, Vector3.zero);
        upper_Center_Right = FindBlock(Vector3.up, Vector3.zero, Vector3.right);
        upper_Back_Left = FindBlock(Vector3.up, Vector3.back, Vector3.left);
        upper_Back = FindBlock(Vector3.up, Vector3.back, Vector3.zero);
        upper_Back_Right = FindBlock(Vector3.up, Vector3.back, Vector3.right);

        //Middle Layer
        center_Front_Left = FindBlock(Vector3.zero, Vector3.forward, Vector3.left);
        center_Front = FindBlock(Vector3.zero, Vector3.forward, Vector3.zero);
        center_Front_Right = FindBlock(Vector3.zero, Vector3.forward, Vector3.right);
        center_Center_Left = FindBlock(Vector3.zero, Vector3.zero, Vector3.left);
        center_Center = FindBlock(Vector3.zero, Vector3.zero, Vector3.zero);
        center_Center_Right = FindBlock(Vector3.zero, Vector3.zero, Vector3.right);
        center_Back_Left = FindBlock(Vector3.zero, Vector3.back, Vector3.left);
        center_Back = FindBlock(Vector3.zero, Vector3.back, Vector3.zero);
        center_Back_Right = FindBlock(Vector3.zero, Vector3.back, Vector3.right);

        //Lower Layer
        lower_Front_Left = FindBlock(Vector3.down, Vector3.forward, Vector3.left);
        lower_Front = FindBlock(Vector3.down, Vector3.forward, Vector3.zero);
        lower_Front_Right = FindBlock(Vector3.down, Vector3.forward, Vector3.right);
        lower_Center_Left = FindBlock(Vector3.down, Vector3.zero, Vector3.left);
        lower_Center = FindBlock(Vector3.down, Vector3.zero, Vector3.zero);
        lower_Center_Right = FindBlock(Vector3.down, Vector3.zero, Vector3.right);
        lower_Back_Left = FindBlock(Vector3.down, Vector3.back, Vector3.left);
        lower_Back = FindBlock(Vector3.down, Vector3.back, Vector3.zero);
        lower_Back_Right = FindBlock(Vector3.down, Vector3.back, Vector3.right);
    }
    GameObject FindBlock(Vector3 dir1, Vector3 dir2, Vector3 dir3)
    {
        return BlockPosManager.Instance.FindGameObjectAtPosition(transform.position + dir1 + dir2 + dir3, gameObject);
    }


    public int GetMovementCost()
    {
        //If Moving with Free Cost
        if (MainManager.Instance.block_StandingOn_Previous == gameObject && !MainManager.Instance.block_StandingOn_Previous.GetComponent<Block_Pusher>() && MainManager.Instance.player.GetComponent<Player_Pusher>().playerIsPushed)
        {
            return 0;
        }

        //If Dashing with Free cost
        else if (MainManager.Instance.player.GetComponent<Player_Dash>().dashBlock_Current == gameObject && MainManager.Instance.player.GetComponent<Player_Pusher>().playerIsPushed && MainManager.Instance.player.GetComponent<Player_Dash>().playerCanDash)
        {
            return 0;
        }

        //If Moving with Normal Cost
        else
        {
            return movementCost;
        }
    }

    //--------------------


    public void DarkenColors()
    {
        if (MainManager.Instance.player.GetComponent<Player_Dash>().isDashing) { return; }

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


    //--------------------


    public void DestroyBlockInfo()
    {
        Player_Movement.Action_resetBlockColor -= ResetColor;
        Player_Stats.Action_RespawnToSavePos -= ResetColor;

        Destroy(this);
    }
}