using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInfo : MonoBehaviour
{
    [Header("Stats")]
    //public BlockElement blockElement;
    public BlockType blockType;
    public int movementCost;
    public float movementSpeed;

    [Header("StepCost Color")]
    public Color stepCostText_Color;

    [Header("Step Sound")]
    [SerializeField] float stepSound_Volume;
    public List<AudioClip> stepSound_ClipList;
    AudioSource stepSound_Source;

    [Header("Starting Position")]
    public Vector3 startPos;

    [Header("Material Rendering")]
    public bool hasOtherMaterial;
    Material material;
    List<Renderer> objectRenderers = new List<Renderer>();
    [HideInInspector] public List<MaterialPropertyBlock> propertyBlocks = new List<MaterialPropertyBlock>();

    #region Adjacent Blocks
    [Header("Adjacent Blocks - Upper")]
    [HideInInspector] public GameObject upper_Front_Left;
    [HideInInspector] public GameObject upper_Front;
    [HideInInspector] public GameObject upper_Front_Right;
    [HideInInspector] public GameObject upper_Center_Left;
    [HideInInspector] public GameObject upper_Center;
    [HideInInspector] public GameObject upper_Center_Right;
    [HideInInspector] public GameObject upper_Back_Left;
    [HideInInspector] public GameObject upper_Back;
    [HideInInspector] public GameObject upper_Back_Right;

    [Header("Adjacent Blocks - Middle")]
    [HideInInspector] public GameObject center_Front_Left;
    [HideInInspector] public GameObject center_Front;
    [HideInInspector] public GameObject center_Front_Right;
    [HideInInspector] public GameObject center_Center_Left;
    [HideInInspector] public GameObject center_Center;
    [HideInInspector] public GameObject center_Center_Right;
    [HideInInspector] public GameObject center_Back_Left;
    [HideInInspector] public GameObject center_Back;
    [HideInInspector] public GameObject center_Back_Right;

    [Header("Adjacent Blocks - Lower")]
    [HideInInspector] public GameObject lower_Front_Left;
    [HideInInspector] public GameObject lower_Front;
    [HideInInspector] public GameObject lower_Front_Right;
    [HideInInspector] public GameObject lower_Center_Left;
    [HideInInspector] public GameObject lower_Center;
    [HideInInspector] public GameObject lower_Center_Right;
    [HideInInspector] public GameObject lower_Back_Left;
    [HideInInspector] public GameObject lower_Back;
    [HideInInspector] public GameObject lower_Back_Right;
    #endregion


    //--------------------


    private void Start()
    {
        startPos = transform.position;
        stepSound_Source = gameObject.AddComponent<AudioSource>();

        if (hasOtherMaterial)
        {
            material = gameObject.GetComponentInChildren<MeshRenderer>().material;
            stepCostText_Color = gameObject.GetComponentInChildren<MeshRenderer>().material.GetColor("_BaseColor");
        }
        
        SetObjectRenderer();
        SetPropertyBlock();
        GetAdjacentBlocksInfo();
    }


    //--------------------


    private void OnEnable()
    {
        Player_Movement.Action_resetBlockColor += ResetColor;
        PlayerStats.Action_RespawnToSavePos += ResetColor;
        PlayerStats.Action_RespawnPlayer += ResetBlock;
        Player_Movement.Action_StepTaken += MakeStepSound;
    }

    private void OnDisable()
    {
        Player_Movement.Action_resetBlockColor -= ResetColor;
        PlayerStats.Action_RespawnToSavePos -= ResetColor;
        PlayerStats.Action_RespawnPlayer -= ResetBlock;
        Player_Movement.Action_StepTaken -= MakeStepSound;
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
        if (PlayerManager.Instance.block_StandingOn_Previous == gameObject && !PlayerManager.Instance.block_StandingOn_Previous.GetComponent<Block_Pusher>() && PlayerManager.Instance.player.GetComponent<Player_Pusher>().playerIsPushed)
        {
            return 0;
        }

        //If Dashing with Free cost
        else if (PlayerManager.Instance.player.GetComponent<Player_Dash>().dashBlock_Current == gameObject && PlayerManager.Instance.player.GetComponent<Player_Pusher>().playerIsPushed && PlayerManager.Instance.player.GetComponent<Player_Dash>().playerCanDash)
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
        if (PlayerManager.Instance.player.GetComponent<Player_Dash>().isDashing) { return; }

        //Don't darken Fences
        if (blockType == BlockType.Fence) { return; }

        if (PlayerStats.Instance.stats.steps_Current <= 0 && movementCost > 0 /*|| PlayerStats.Instance.stats.steps_Current < movementCost*/)
        {
            ResetColor();
            return;
        }

        //Darken all materials attached
        for (int i = 0; i < propertyBlocks.Count; i++)
        {
            // Darken the color
            Color darkenedColor = new Color();
            if (hasOtherMaterial)
            {
                darkenedColor = material.color * BlockManager.Instance.materialDarkeningValue;
            }
            else
            {
                darkenedColor = Color.white * BlockManager.Instance.materialDarkeningValue;
            }

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
            Color restoredColor = new Color();
            if (hasOtherMaterial)
            {
                restoredColor = material.color;
            }
            else
            {
                restoredColor = Color.white;
            }
           
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


    void MakeStepSound()
    {
        if (stepSound_ClipList.Count > 0 && PlayerManager.Instance.block_StandingOn_Current.block == gameObject)
        {
            int sound = Random.Range(0, stepSound_ClipList.Count);

            stepSound_Source.clip = stepSound_ClipList[sound];

            float volume = Random.Range(0.75f, 1.25f);

            if (stepSound_Volume > 0)
                stepSound_Source.volume = stepSound_Volume * volume;

            stepSound_Source.Play();
        }
    }


    //--------------------


    void ResetBlock()
    {
        transform.position = startPos;
    }
}