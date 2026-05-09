using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInfo : MonoBehaviour
{
    [Header("Stats")]
    public BlockType blockType;
    public BlockElement blockElement;

    [HideInInspector] public int movementCost_Temp;
    [HideInInspector] public int movementCost_Temp_Base;

    [HideInInspector] public bool movementCost_OverrideActive;
    [HideInInspector] public int movementCost_OverrideValue;

    [HideInInspector] public bool movementCost_DisplayOverrideActive;
    [HideInInspector] public int movementCost_DisplayOverrideValue;

    public int movementCost;
    public float movementSpeed;

    public MovementStates movementState;

    [Header("StepCost Color")]
    public Color stepCostText_Color;
    public Color stepCostText_ColorUnder;

    [Header("Starting Position")]
    [HideInInspector] public Vector3 startPos;

    [Header("Material Rendering")]
    List<Renderer> objectRenderers = new List<Renderer>();
    [HideInInspector] public List<MaterialPropertyBlock> propertyBlocks = new List<MaterialPropertyBlock>();

    [HideInInspector] public bool colorTint_isActive;
    bool color_isAboutToBeDarkened;

    [HideInInspector] public float tintValue = 0.95f;

    [HideInInspector] NumberDisplay numberDisplay;

    bool finishedSetup;

    [HideInInspector] public bool blockIsDark;

    public Vector3Int GridPosition => Vector3Int.RoundToInt(transform.position);

    int mushroomCircle_Buff = 0;



    //--------------------


    private void Awake()
    {
        startPos = transform.position;
    }
    private void Start()
    {
        tintValue = 0.92f;
        //tintValue = 0.4f;

        numberDisplay = GetComponentInChildren<NumberDisplay>();

        movementCost_Temp_Base = movementCost;
        movementCost_Temp = movementCost_Temp_Base;

        SetObjectRenderer();
        SetPropertyBlock();

        //ResetDarkenColor();
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
        //CheckDarkeningWhenPlayerIsOnElevator();
    }


    //--------------------


    private void OnEnable()
    {
        Movement.Action_RespawnToSavePos += ResetDarkenColor;
        Movement.Action_RespawnPlayer += ResetBlock;

        Player_MushroomCircle.Action_StartMushroomCircle += ActivateMushroomCircleBuff;
        Player_MushroomCircle.Action_EndMushroomCircle += DeactivateMushroomCircleBuff;
        Player_MushroomCircle.Action_UpdateMushroomCircle += RefreshStepCostDisplay;
    }

    private void OnDisable()
    {
        Movement.Action_RespawnToSavePos -= ResetDarkenColor;
        Movement.Action_RespawnPlayer -= ResetBlock;


        Player_MushroomCircle.Action_StartMushroomCircle -= ActivateMushroomCircleBuff;
        Player_MushroomCircle.Action_EndMushroomCircle -= DeactivateMushroomCircleBuff;
        Player_MushroomCircle.Action_UpdateMushroomCircle -= RefreshStepCostDisplay;
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

    GameObject FindBlock(Vector3 dir1, Vector3 dir2, Vector3 dir3)
    {
        return BlockPosManager.Instance.FindGameObjectAtPosition(transform.position + dir1 + dir2 + dir3, gameObject);
    }


    //--------------------


    void CheckDarkeningWhenPlayerIsOnElevator()
    {
        //if (blockIsDark)
        //{
        //    if (Movement.Instance.blockStandingOn)
        //    {
        //        if (Movement.Instance.blockStandingOn.GetComponent<Block_Elevator>()
        //        || Movement.Instance.blockStandingOn.GetComponent<Block_Elevator_StepOn>())
        //        {
        //            if ((gameObject == PlayerManager.Instance.block_Vertical_InFront.block && PlayerManager.Instance.canMove_Forward)
        //                || (gameObject == PlayerManager.Instance.block_Vertical_InBack.block && PlayerManager.Instance.canMove_Back)
        //                || (gameObject == PlayerManager.Instance.block_Vertical_ToTheLeft.block && PlayerManager.Instance.canMove_Left)
        //                || (gameObject == PlayerManager.Instance.block_Vertical_ToTheRight.block && PlayerManager.Instance.canMove_Right))
        //            {
        //                //SetDarkenColors();
        //            }
        //            else if ((gameObject == Player_Jumping.Instance.jumpTarget_Forward && Player_Jumping.Instance.canJump_Forward)
        //                     || (gameObject == Player_Jumping.Instance.jumpTarget_Back && Player_Jumping.Instance.canJump_Back)
        //                     || (gameObject == Player_Jumping.Instance.jumpTarget_Left && Player_Jumping.Instance.canJump_Left)
        //                     || (gameObject == Player_Jumping.Instance.jumpTarget_Right && Player_Jumping.Instance.canJump_Right))
        //            {
        //                //SetDarkenColors();
        //            }
        //            else
        //            {
        //                ResetDarkenColor();
        //            }
        //        }
        //    }
        //}
    }


    //--------------------


    public void SetDarkenColors()
    {
        if (blockIsDark) return;

        ApplyTemporaryMovementCostModifiers();

        color_isAboutToBeDarkened = true;
        UpdateBlock_Darken();
        color_isAboutToBeDarkened = false;
    }

    public void ResetDarkenColor()
    {
        if (!blockIsDark) return;

        ResetTemporaryMovementCostModifiers();

        for (int i = 0; i < propertyBlocks.Count; i++)
        {
            Color restoredColor = ResetBlockColorTint();
            propertyBlocks[i].SetColor("_BaseColor", restoredColor);
            objectRenderers[i].SetPropertyBlock(propertyBlocks[i]);
        }

        if (numberDisplay)
        {
            numberDisplay.HideNumber();
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
        if (blockIsDark) return;

        for (int i = 0; i < propertyBlocks.Count; i++)
        {
            Color finalColor = GetBlockColorTint();
            propertyBlocks[i].SetColor("_BaseColor", finalColor);
            objectRenderers[i].SetPropertyBlock(propertyBlocks[i]);
        }

        if (GetComponent<Block_Snow>())
        {
            GetComponent<Block_Snow>().ChangeStepCounterPosition();
        }

        if (numberDisplay && finishedSetup)
        {
            numberDisplay.ShowNumber();
        }

        blockIsDark = true;
    }

    public Color GetBlockColorTint()
    {
        if (color_isAboutToBeDarkened)
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


    public Vector3 GetFacingDirection()
    {
        return transform.TransformDirection(Vector3.left); // local -X
    }


    //--------------------


    void ActivateMushroomCircleBuff()
    {
        mushroomCircle_Buff = 1;
        RefreshStepCostDisplay();
    }

    void DeactivateMushroomCircleBuff()
    {
        mushroomCircle_Buff = 0;
        RefreshStepCostDisplay();
    }

    public bool HasMushroomCircleBuff()
    {
        return mushroomCircle_Buff > 0;
    }

    public int GetCurrentStepCostModifier()
    {
        int modifier = 0;

        // Mud increases cost by +1
        if (Player_Mud.Instance != null && Player_Mud.Instance.isInMud)
        {
            modifier += 1;
        }

        // SwampWater reduces cost by -1
        if (Player_SwampWater.Instance != null && Player_SwampWater.Instance.isInSwampWater)
        {
            modifier -= 1;
        }

        // MushroomCircle reduces cost by -1
        if (HasMushroomCircleBuff())
        {
            modifier -= 1;
        }

        return modifier;
    }

    public int GetMovementCost_WithTemporaryEffects()
    {
        if (movementCost_OverrideActive)
        {
            return movementCost_OverrideValue;
        }

        // Quicksand uses the quicksand counter as the actual step cost,
        // not only as the displayed number.
        if (GetComponent<Block_Quicksand>() &&
            Player_Quicksand.Instance != null &&
            Player_Quicksand.Instance.isInQuicksand)
        {
            return Player_Quicksand.Instance.quicksandCounter;
        }

        if (blockElement == BlockElement.Water && PlayerHasFlippers())
        {
            return 0;
        }

        return movementCost_Temp_Base + GetCurrentStepCostModifier();
    }

    public int GetMovementCost_ForPlayerMove()
    {
        ApplyTemporaryMovementCostModifiers();
        return movementCost_Temp;
    }

    public void ApplyTemporaryMovementCostModifiers()
    {
        movementCost_Temp = GetMovementCost_WithTemporaryEffects();
    }

    public void ResetTemporaryMovementCostModifiers()
    {
        if (movementCost_OverrideActive)
        {
            movementCost_Temp = movementCost_OverrideValue;
            return;
        }

        movementCost_Temp = movementCost_Temp_Base;
    }

    public void RefreshStepCostDisplay()
    {
        if (!finishedSetup) return;

        ApplyTemporaryMovementCostModifiers();

        if (blockIsDark && numberDisplay != null)
        {
            numberDisplay.ShowNumber();
        }
    }
    public void SetBaseMovementCost(int newCost)
    {
        movementCost = newCost;
        movementCost_Temp_Base = newCost;
        movementCost_Temp = GetMovementCost_WithTemporaryEffects();
    }

    public void SetTemporaryMovementCostOverride(int newCost)
    {
        movementCost_OverrideActive = true;
        movementCost_OverrideValue = newCost;

        movementCost_Temp = newCost;

        if (blockIsDark && numberDisplay != null)
        {
            numberDisplay.ShowNumber();
        }
    }

    public void ClearTemporaryMovementCostOverride()
    {
        movementCost_OverrideActive = false;
        movementCost_OverrideValue = 0;

        ApplyTemporaryMovementCostModifiers();

        if (blockIsDark && numberDisplay != null)
        {
            numberDisplay.ShowNumber();
        }
    }

    bool PlayerHasFlippers()
    {
        if (PlayerStats.Instance == null || PlayerStats.Instance.stats == null)
            return false;

        return PlayerStats.Instance.stats.abilitiesGot_Temporary.Flippers ||
               PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers;
    }

    public int GetMovementCost_ForDisplay()
    {
        if (movementCost_DisplayOverrideActive)
            return movementCost_DisplayOverrideValue;

        ApplyTemporaryMovementCostModifiers();
        return movementCost_Temp;
    }

    public void SetDisplayMovementCostOverride(int newCost)
    {
        movementCost_DisplayOverrideActive = true;
        movementCost_DisplayOverrideValue = newCost;

        if (blockIsDark && numberDisplay != null)
        {
            numberDisplay.ShowNumber();
        }
    }

    public void ClearDisplayMovementCostOverride()
    {
        movementCost_DisplayOverrideActive = false;
        movementCost_DisplayOverrideValue = 0;

        if (blockIsDark && numberDisplay != null)
        {
            numberDisplay.ShowNumber();
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