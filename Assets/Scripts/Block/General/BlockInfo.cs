using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInfo : MonoBehaviour
{
    [Header("Stats")]
    //public BlockElement blockElement;
    public BlockType blockType;
    public BlockElement blockElement;
    [HideInInspector] public int movementCost_Temp;
    public int movementCost;
    public float movementSpeed;

    [Header("StepCost Color")]
    public Color stepCostText_Color;
    public Color stepCostText_ColorUnder;

    [Header("Step Sound")]
    public float stepSound_Volume;
    public List<AudioClip> stepSound_ClipList;
    [HideInInspector] public AudioSource stepSound_Source;

    [Header("Starting Position")]
    [HideInInspector] public Vector3 startPos;

    [Header("Material Rendering")]
    List<Renderer> objectRenderers = new List<Renderer>();
    [HideInInspector] public List<MaterialPropertyBlock> propertyBlocks = new List<MaterialPropertyBlock>();

    public bool colorTint_isActive;
    bool color_isDarkened;

    public float tintValue = 0.95f;

    NumberDisplay numberDisplay;

    bool finishedSetup;

    public bool blockIsDark;
    
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
        tintValue = 0.92f;
        //tintValue = 0.4f;

        numberDisplay = GetComponentInChildren<NumberDisplay>();

        movementCost_Temp = movementCost;

        startPos = transform.position;
        stepSound_Source = gameObject.AddComponent<AudioSource>();
        
        SetObjectRenderer();
        SetPropertyBlock();

        GetAdjacentBlocksInfo();

        ResetDarkenColor();
        TintBlock_CheckerPattern();

        //Show StepCost
        if (numberDisplay)
        {
            //GetMovementCost();
            numberDisplay.HideNumber();
        }

        finishedSetup = true;

        blockIsDark = false;
    }
    private void Update()
    {
        CheckDarkeningWhenPlayerIsOnElevator();
    }


    //--------------------


    private void OnEnable()
    {
        Player_Movement.Action_resetBlockColor += ResetDarkenColor;
        PlayerStats.Action_RespawnToSavePos += ResetDarkenColor;
        PlayerStats.Action_RespawnPlayer += ResetBlock;
        Player_Movement.Action_LandedFromFalling += ResetDarkenColor;
    }

    private void OnDisable()
    {
        Player_Movement.Action_resetBlockColor -= ResetDarkenColor;
        PlayerStats.Action_RespawnToSavePos -= ResetDarkenColor;
        PlayerStats.Action_RespawnPlayer -= ResetBlock;
        Player_Movement.Action_LandedFromFalling -= ResetDarkenColor;
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


    //--------------------


    void CheckDarkeningWhenPlayerIsOnElevator()
    {
        if (blockIsDark)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block)
            {
                if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_Elevator_Normal>()
                || PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_Elevator_StepOn>())
                {
                    if ((gameObject == PlayerManager.Instance.block_Vertical_InFront.block && PlayerManager.Instance.canMove_Forward)
                        || (gameObject == PlayerManager.Instance.block_Vertical_InBack.block && PlayerManager.Instance.canMove_Back)
                        || (gameObject == PlayerManager.Instance.block_Vertical_ToTheLeft.block && PlayerManager.Instance.canMove_Left)
                        || (gameObject == PlayerManager.Instance.block_Vertical_ToTheRight.block && PlayerManager.Instance.canMove_Right))
                    {
                        //SetDarkenColors();
                    }
                    else if ((gameObject == Player_Jumping.Instance.jumpTarget_Forward && Player_Jumping.Instance.canJump_Forward)
                             || (gameObject == Player_Jumping.Instance.jumpTarget_Back && Player_Jumping.Instance.canJump_Back)
                             || (gameObject == Player_Jumping.Instance.jumpTarget_Left && Player_Jumping.Instance.canJump_Left)
                             || (gameObject == Player_Jumping.Instance.jumpTarget_Right && Player_Jumping.Instance.canJump_Right))
                    {
                        //SetDarkenColors();
                    }
                    else
                    {
                        ResetDarkenColor();
                    }
                }
            }
        }
    }


    //--------------------


    public void SetDarkenColors()
    {
        if (blockIsDark) { return; }

        if (PlayerManager.Instance.player.GetComponent<Player_Dash>().isDashing) { return; }

        if (PlayerStats.Instance.stats.steps_Current <= 0 && movementCost > 0)
        {
            ResetDarkenColor();
            return;
        }

        color_isDarkened = true;

        UpdateBlock_Darken();

        color_isDarkened = false;
    }

    public void ResetDarkenColor()
    {
        if (numberDisplay)
        {
            for (int i = 0; i < propertyBlocks.Count; i++)
            {
                // Restore the color to full brightness
                Color restoredColor = ResetBlockColorTint();

                // Set the original color in the MaterialPropertyBlock
                propertyBlocks[i].SetColor("_BaseColor", restoredColor);

                // Apply the MaterialPropertyBlock to the renderer
                objectRenderers[i].SetPropertyBlock(propertyBlocks[i]);

                //Hide StepCost
                numberDisplay.HideNumber();
            }
        }

        blockIsDark = false;
    }


    //--------------------


    void TintBlock_CheckerPattern()
    {
        // Get the whole number position (rounding to nearest int)
        int x = Mathf.RoundToInt(transform.position.x);
        int z = Mathf.RoundToInt(transform.position.z);

        // Determine state based on checkerboard pattern
        if ((x + z) % 2 == 0)
        {
            colorTint_isActive = true;
        }
        else
        {
            colorTint_isActive = false;
        }

        UpdateBlock_Darken();
    }
    void UpdateBlock_Darken()
    {
        if (blockIsDark) { return; }

        //Darken all materials attached
        for (int i = 0; i < propertyBlocks.Count; i++)
        {
            if (numberDisplay)
            {
                //Ensure final value is exactly the target
                Color finalColor = GetBlockColorTint();
                propertyBlocks[i].SetColor("_BaseColor", finalColor);
                objectRenderers[i].SetPropertyBlock(propertyBlocks[i]);
            }
        }

        //Change StepCost 3D Asset Pos on Snow
        if (GetComponent<Block_Snow>())
        {
            GetComponent<Block_Snow>().ChangeStepCounterPosition();
        }

        //Show StepCost
        if (numberDisplay && finishedSetup)
        {
            numberDisplay.ShowNumber();
        }

        blockIsDark = true;
    }

    public Color GetBlockColorTint()
    {
        if (color_isDarkened)
        {
            if (colorTint_isActive)
            {
                return Color.white * tintValue * BlockManager.Instance.materialDarkeningValue;
            }
            else
            {
                return Color.white * BlockManager.Instance.materialDarkeningValue;
            }
        }
        else
        {
            if (colorTint_isActive)
            {
                return Color.white * tintValue;
            }
            else
            {
                return Color.white;
            }
        }
    }
    Color ResetBlockColorTint()
    {
        if (colorTint_isActive)
        {
            return Color.white * tintValue;
        }
        else
        {
            return Color.white;
        }
    }


    //--------------------


    public void MakeStepSound()
    {
        if (stepSound_ClipList.Count > 0 && PlayerManager.Instance.block_StandingOn_Current.block == gameObject)
        {
            int sound = UnityEngine.Random.Range(0, stepSound_ClipList.Count);

            stepSound_Source.clip = stepSound_ClipList[sound];

            float volume = UnityEngine.Random.Range(0.75f, 1.25f);

            if (stepSound_Volume > 0)
                stepSound_Source.volume = stepSound_Volume * volume;

            stepSound_Source.Play();
        }
    }


    //--------------------


    void ResetBlock()
    {
        if (transform.position != startPos)
        {
            transform.position = startPos;
        }
    }
}