using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Movement : Singleton<Movement>
{
    public static event Action Action_RespawnToSavePos;
    public static event Action Action_RespawnPlayerEarly;
    public static event Action Action_RespawnPlayer;
    public static event Action Action_RespawnPlayerLate;

    public static event Action Action_StepTaken;
    public static event Action Action_StepTaken_Late;
    public static event Action Action_BodyRotated;
    public static event Action Action_isSwitchingBlocks;
    public static event Action Action_LandedFromFalling;

    [Header("States")]
    public bool isMoving;
    public MovementStates movementStates = MovementStates.Still;

    [Header("Stats")]
    public float heightOverBlock = 0.95f;
    [HideInInspector] public float baseTime = 1;
    [HideInInspector] public float movementSpeed = 0.2f;
    [HideInInspector] public float fallSpeed = 8f;
    [HideInInspector] public float abilitySpeed = 8f;
    public Vector3 savePos;

    [Header("CeilingGrab")]
    [SerializeField] bool isCeilingGrabbing;

    [Header("BlockIsStandingOn")]
    public Vector3 lookingDirection;
    public GameObject blockStandingOn;
    public GameObject blockStandingOn_Previous;

    [Header("LookDirection")]
    [HideInInspector] public Vector3 lookDir;
    [HideInInspector] public float lookDir_Temp;

    [Header("CanMoveBlocks")]
    public MoveOptions moveToBlock_Forward;
    public MoveOptions moveToBlock_Back;
    public MoveOptions moveToBlock_Left;
    public MoveOptions moveToBlock_Right;

    public MoveOptions moveToBlock_Ascend;
    public MoveOptions moveToBlock_Descend;

    public MoveOptions moveToBlock_SwiftSwimUp;
    public MoveOptions moveToBlock_SwiftSwimDown;

    public MoveOptions moveToBlock_Dash_Forward;
    public MoveOptions moveToBlock_Dash_Back;
    public MoveOptions moveToBlock_Dash_Left;
    public MoveOptions moveToBlock_Dash_Right;

    public MoveOptions moveToBlock_Jump_Forward;
    public MoveOptions moveToBlock_Jump_Back;
    public MoveOptions moveToBlock_Jump_Left;
    public MoveOptions moveToBlock_Jump_Right;

    public MoveOptions moveToBlock_GrapplingHook;
    public MoveOptions moveToCeilingGrabbing;

    public bool isUpdatingDarkenBlocks;

    RaycastHit hit;


    //--------------------


    private void Start()
    {
        savePos = transform.position;

        RespawnPlayer();
    }
    private void Update()
    {
        if (GetMovementState() == MovementStates.Moving)
        {
            UpdateBlockStandingOn();
        }
        else if (GetMovementState() == MovementStates.Falling)
        {
            UpdateBlockStandingOn();
            PlayerIsFalling();
        }
        else
        {
            MovementSetup();
        }
    }


    //--------------------


    private void OnEnable()
    {
        Action_StepTaken_Late += UpdateAvailableMovementBlocks;
        Action_RespawnPlayerLate += UpdateAvailableMovementBlocks;
        Action_LandedFromFalling += UpdateAvailableMovementBlocks;

        Action_RespawnPlayerEarly += ResetDarkenBlocks;
        Action_StepTaken += TakeAStep;

        CameraController.Action_RotateCamera_End += UpdateBlocks;
    }
    private void OnDisable()
    {
        Action_StepTaken_Late -= UpdateAvailableMovementBlocks;
        Action_RespawnPlayerLate -= UpdateAvailableMovementBlocks;
        Action_LandedFromFalling -= UpdateAvailableMovementBlocks;

        Action_RespawnPlayerEarly -= ResetDarkenBlocks;
        Action_StepTaken -= TakeAStep;

        CameraController.Action_RotateCamera_End -= UpdateBlocks;
    }


    //--------------------


    #region Movement Functions

    void UpdateAvailableMovementBlocks()
    {
        ResetDarkenBlocks();

        UpdateBlocks();

        SetDarkenBlocks();
    }
    void UpdateBlocks()
    {
        isUpdatingDarkenBlocks = true;

        UpdateBlockStandingOn();

        if (blockStandingOn /*&& movementStates == MovementStates.Still*/)
        {
            UpdateNormalMovement();

            UpdateSwiftSwimUpMovement(); //Must come before Ascend
            UpdateSwiftSwimDownMovement();//Must come before Descend

            UpdateAscendMovement();
            UpdateDescendMovement();

            UpdateDashMovement();

            UpdateJumpMovement();
            UpdateGrapplingHookMovement();
            UpdateCeilingGrabMovement();
        }
        else
        {
            StartFallingWithNoBlock();
        }

        isUpdatingDarkenBlocks = false;
    }
    void UpdateBlockStandingOn()
    {
        GameObject obj = null;
        GameObject objTemp = blockStandingOn;

        PerformMovementRaycast(PlayerManager.Instance.player.transform.position, Vector3.down, 1, out obj);

        if (blockStandingOn != obj)
        {
            blockStandingOn = null;
        }

        blockStandingOn = obj;

        //Check if the player has moved over to a new block
        if (objTemp != blockStandingOn)
        {
            Action_isSwitchingBlocks_Invoke();
        }
    }

    void UpdateNormalMovement()
    {
        //Forward
        UpdateNormalMovements(moveToBlock_Forward, UpdatedDir(Vector3.forward));

        //Back
        UpdateNormalMovements(moveToBlock_Back, UpdatedDir(Vector3.back));

        //Left
        UpdateNormalMovements(moveToBlock_Left, UpdatedDir(Vector3.left));

        //Right
        UpdateNormalMovements(moveToBlock_Right, UpdatedDir(Vector3.right));
    }
    void UpdateNormalMovements(MoveOptions moveOption, Vector3 dir)
    {
        if (!blockStandingOn) { return; }
        if (!blockStandingOn.GetComponent<BlockInfo>()) { return; }

        GameObject outObj1 = null;
        GameObject outObj2 = null;
        Vector3 playerPos = PlayerManager.Instance.player.transform.position;

        Vector3 rayDir = Vector3.zero;

        if (isCeilingGrabbing)
            rayDir = Vector3.up;
        else
            rayDir = Vector3.down;

        //If standing on a Stair
        if (blockStandingOn != null && blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Stair)
        {
            Vector3 stairForward = blockStandingOn.transform.forward;
            Vector3 stairBackward = -stairForward;

            //stairForward.y = 0;
            //stairBackward.y = 0;
            stairForward.Normalize();
            stairBackward.Normalize();

            //Down the Stair
            if (dir == stairForward)
            {
                if (PerformMovementRaycast(playerPos, stairForward, 1, out outObj1) == RaycastHitObjects.None
                    && PerformMovementRaycast(playerPos + (stairForward / 1.5f), rayDir, 1, out outObj2) == RaycastHitObjects.BlockInfo)
                {
                    if (outObj2.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
                    {
                        if (PlayerHasSwimAbility())
                            Block_Is_Target(moveOption, outObj2);
                        else
                            Block_IsNot_Target(moveOption);
                    }
                    else if (outObj2 != blockStandingOn)
                    {
                        Block_Is_Target(moveOption, outObj2);
                    }
                    else
                        Block_IsNot_Target(moveOption);
                }
                else
                    Block_IsNot_Target(moveOption);
            }

            //Up the Stair
            else if (dir == stairBackward)
            {
                if (PerformMovementRaycast(playerPos + Vector3.up, stairBackward, 1, out outObj1) == RaycastHitObjects.None
                    && PerformMovementRaycast(playerPos + Vector3.up + stairBackward, rayDir, 1, out outObj2) == RaycastHitObjects.BlockInfo)
                {
                    if (outObj2.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
                    {
                        if (PlayerHasSwimAbility())
                            Block_Is_Target(moveOption, outObj2);
                        else
                            Block_IsNot_Target(moveOption);
                    }
                    else if (outObj2 != blockStandingOn)
                    {
                        Block_Is_Target(moveOption, outObj2);
                    }
                    else
                        Block_IsNot_Target(moveOption);
                }
                else
                    Block_IsNot_Target(moveOption);
            }

            //If another Stair or Slope is connected at the sides
            else
            {
                if (PerformMovementRaycast(playerPos, dir, 1, out outObj1) == RaycastHitObjects.None &&
                        PerformMovementRaycast(playerPos + dir, rayDir, 1, out outObj2) == RaycastHitObjects.BlockInfo)
                {
                    if (outObj2.GetComponent<BlockInfo>().blockType == BlockType.Stair || outObj2.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                    {
                        Block_Is_Target(moveOption, outObj2);
                    }
                    else
                        Block_IsNot_Target(moveOption);
                }
                else
                    Block_IsNot_Target(moveOption);
            }

            return;
        }

        //If standing on a Slope
        else if (blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Slope)
        {
            Vector3 slopeForward = blockStandingOn.transform.forward;
            slopeForward.Normalize();

            if (dir == slopeForward)
            {
                if (PerformMovementRaycast(playerPos, slopeForward, 1, out outObj1) == RaycastHitObjects.None
                && PerformMovementRaycast(playerPos + (slopeForward / 1.5f), rayDir, 1, out outObj2) == RaycastHitObjects.BlockInfo)
                {
                    if (outObj2 != blockStandingOn)
                    {
                        Block_Is_Target(moveOption, outObj2);
                    }
                    else
                        Block_IsNot_Target(moveOption);
                }
                else
                    Block_IsNot_Target(moveOption);

                if (moveOption.canMoveTo)
                {
                    //RotatePlayerBody(blockStandingOn.transform.forward.y);
                    PerformMovement(moveOption, MovementStates.Moving, blockStandingOn.GetComponent<BlockInfo>().movementSpeed);
                }
                else
                {
                    //If there isn't any block to stand on
                    PerformMovement(blockStandingOn.transform.position + slopeForward + (Vector3.down * 0.5f));
                }
            }
        }

        //If standing on a Cube/Slab
        else
        {
            if (PerformMovementRaycast(playerPos, dir, 1, out outObj1) == RaycastHitObjects.None
            && PerformMovementRaycast(playerPos + dir, rayDir, 1, out outObj2) == RaycastHitObjects.BlockInfo)
            {
                //If there is a Water Block where the player want to move
                if (outObj2.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
                {
                    if (PlayerHasSwimAbility())
                        Block_Is_Target(moveOption, outObj2);
                    else
                        Block_IsNot_Target(moveOption);
                }
                else if (outObj2.GetComponent<BlockInfo>().blockType == BlockType.Stair || outObj2.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                {
                    if (transform.position.y > outObj2.transform.position.y + 0.5f && Vector3.Dot(outObj2.transform.forward, dir.normalized) > 0.5f)
                    {
                        Block_Is_Target(moveOption, outObj2);
                    }
                    else
                        Block_IsNot_Target(moveOption);
                }
                else
                    Block_Is_Target(moveOption, outObj2);

                return;
            }

            //IF the first hit is a Block
            else
            {
                if (outObj1)
                {
                    BlockInfo blockInfo1 = outObj1.GetComponent<BlockInfo>();

                    if (blockInfo1 != null && (blockInfo1.blockType == BlockType.Stair || blockInfo1.blockType == BlockType.Slope))
                    {
                        Vector3 stairForward = outObj1.transform.forward;
                        Vector3 toPlayer = (transform.position - outObj1.transform.position).normalized;

                        float dot = Vector3.Dot(stairForward, toPlayer);

                        // CASE 1: Stair is facing the player
                        if (dot > 0.5f)
                        {
                            Block_Is_Target(moveOption, outObj1);
                        }
                        // CASE 2: Player is above the stair AND moving down in its forward direction
                        else if (transform.position.y > outObj1.transform.position.y + 0.5f && Vector3.Dot(stairForward, dir.normalized) > 0.5f)
                            Block_Is_Target(moveOption, outObj1);
                        else
                            Block_IsNot_Target(moveOption);

                        return;
                    }

                    //If looking at a Water block with another Water block under it
                    else if (PerformMovementRaycast(playerPos + dir, rayDir, 1, out outObj2) == RaycastHitObjects.BlockInfo)
                    {
                        if (outObj1.GetComponent<BlockInfo>().blockElement == BlockElement.Water && outObj2.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
                        {
                            if (PlayerHasSwimAbility())
                                Block_Is_Target(moveOption, outObj2);
                            else
                                Block_IsNot_Target(moveOption);
                        }
                        else
                            Block_IsNot_Target(moveOption);

                        return;
                    }
                    else
                        Block_IsNot_Target(moveOption);
                }
            }
        }

        Block_IsNot_Target(moveOption);
    }

    void UpdateSwiftSwimUpMovement()
    {
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.SwiftSwim && !PlayerStats.Instance.stats.abilitiesGot_Permanent.SwiftSwim)
        {
            Block_IsNot_Target(moveToBlock_SwiftSwimUp);
            return;
        }

        GameObject outObj1 = null;
        Vector3 playerPos = PlayerManager.Instance.player.transform.position;

        if (PerformMovementRaycast(blockStandingOn.transform.position, Vector3.up, 1, out outObj1) == RaycastHitObjects.BlockInfo)
        {
            BlockInfo hitBlock= outObj1.GetComponent<BlockInfo>();

            if (hitBlock.blockElement == BlockElement.Water)
                Block_Is_Target(moveToBlock_SwiftSwimUp, outObj1);
            else
                Block_IsNot_Target(moveToBlock_SwiftSwimUp);
        }
        else
            Block_IsNot_Target(moveToBlock_SwiftSwimUp);
    }
    void UpdateSwiftSwimDownMovement()
    {
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.SwiftSwim && !PlayerStats.Instance.stats.abilitiesGot_Permanent.SwiftSwim)
        {
            Block_IsNot_Target(moveToBlock_SwiftSwimDown);
            return;
        }

        GameObject outObj1 = null;
        Vector3 playerPos = PlayerManager.Instance.player.transform.position;

        if (PerformMovementRaycast(blockStandingOn.transform.position, Vector3.down, 1, out outObj1) == RaycastHitObjects.BlockInfo)
        {
            BlockInfo hitBlock = outObj1.GetComponent<BlockInfo>();

            if (hitBlock.blockElement == BlockElement.Water)
                Block_Is_Target(moveToBlock_SwiftSwimDown, outObj1);
            else
                Block_IsNot_Target(moveToBlock_SwiftSwimDown);
        }
        else
            Block_IsNot_Target(moveToBlock_SwiftSwimDown);
    }
    void UpdateAscendMovement()
    {
        if (moveToBlock_SwiftSwimUp.canMoveTo) { Block_IsNot_Target(moveToBlock_Ascend); return; }

        if (!PlayerHasAscendAbility())
        {
            Block_IsNot_Target(moveToBlock_Ascend);
            return;
        }

        GameObject outObj1 = null;
        GameObject outObj2 = null;
        Vector3 playerPos = PlayerManager.Instance.player.transform.position;
        Vector3 adjustments;

        if (PerformMovementRaycast(playerPos, Vector3.up, 2, out outObj1) == RaycastHitObjects.BlockInfo)
        {
            if (outObj1.GetComponent<BlockInfo>().blockType == BlockType.Stair || outObj1.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                adjustments = Vector3.up * 0.5f;
            else
                adjustments = Vector3.up * 0;

            //If hit is a Slab
            if (outObj1.GetComponent<BlockInfo>().blockType == BlockType.Slab)
            {
                //If second hit is nothing
                if (PerformMovementRaycast(outObj1.transform.position + adjustments + (Vector3.up * 0.25f), Vector3.up, 1, out outObj2) == RaycastHitObjects.None)
                {
                    if (outObj1.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
                    {
                        if (PlayerHasSwimAbility())
                            Block_Is_Target(moveToBlock_Ascend, outObj1);
                        else
                            Block_IsNot_Target(moveToBlock_Ascend);
                    }
                    else if (outObj1 != blockStandingOn)
                    {
                        Block_Is_Target(moveToBlock_Ascend, outObj1);
                    }
                    else
                    {
                        Block_IsNot_Target(moveToBlock_Ascend);
                    }
                }

                //If second hit is a block
                else if (PerformMovementRaycast(outObj1.transform.position + adjustments + (Vector3.up * 0.25f), Vector3.up, 1, out outObj2) == RaycastHitObjects.BlockInfo)
                {
                    if (outObj2.GetComponent<BlockInfo>().blockType == BlockType.Slab)
                    {
                        Block_Is_Target(moveToBlock_Ascend, outObj1);
                    }
                    //else if (outObj2.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
                    //{
                    //    if (PlayerHasSwimAbility())
                    //        Block_Is_Target(moveToBlock_Ascend, outObj2);
                    //    else
                    //        Block_IsNot_Target(moveToBlock_Ascend);
                    //}
                    else
                        Block_IsNot_Target(moveToBlock_Ascend);
                }

                //If hitting something else
                else
                    Block_IsNot_Target(moveToBlock_Ascend);
            }

            //If first hit isn't a slab
            else
            {
                if (PerformMovementRaycast(outObj1.transform.position + adjustments, Vector3.up, 1, out outObj2) == RaycastHitObjects.None)
                {
                    //if (blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Water && outObj1.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
                    //{
                    //    if (PlayerStats.Instance.stats.abilitiesGot_Temporary.SwiftSwim || PlayerStats.Instance.stats.abilitiesGot_Permanent.SwiftSwim)
                    //        Block_Is_Target(moveToBlock_Ascend, outObj1);
                    //    else
                    //        Block_IsNot_Target(moveToBlock_Ascend);
                    //}
                    //else
                    if (outObj1.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
                    {
                        if (PlayerHasSwimAbility())
                            Block_Is_Target(moveToBlock_Ascend, outObj1);
                        else
                            Block_IsNot_Target(moveToBlock_Ascend);
                    }
                    else if(outObj1 != blockStandingOn)
                    {
                        Block_Is_Target(moveToBlock_Ascend, outObj1);
                    }
                    else
                    {
                        Block_IsNot_Target(moveToBlock_Ascend);
                    }
                }
                else if (PerformMovementRaycast(outObj1.transform.position + adjustments, Vector3.up, 1, out outObj2) == RaycastHitObjects.BlockInfo)
                {
                    //if (blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Water && (outObj2.GetComponent<BlockInfo>().blockElement == BlockElement.Water || outObj1.GetComponent<BlockInfo>().blockElement == BlockElement.Water))
                    //{
                    //    if (PlayerStats.Instance.stats.abilitiesGot_Temporary.SwiftSwim || PlayerStats.Instance.stats.abilitiesGot_Permanent.SwiftSwim)
                    //        Block_Is_Target(moveToBlock_Ascend, outObj1);
                    //    else
                    //        Block_IsNot_Target(moveToBlock_Ascend);
                    //}
                    //else 
                    if(outObj2.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
                    {
                        if (PlayerHasSwimAbility())
                            Block_Is_Target(moveToBlock_Ascend, outObj1);
                        else
                            Block_IsNot_Target(moveToBlock_Ascend);
                    }
                    else
                        Block_IsNot_Target(moveToBlock_Ascend);
                }
                else
                    Block_IsNot_Target(moveToBlock_Ascend);
            }
        }
        else
        {
            Block_IsNot_Target(moveToBlock_Ascend);
        }
    }
    void UpdateDescendMovement()
    {
        if (moveToBlock_SwiftSwimDown.canMoveTo) { Block_IsNot_Target(moveToBlock_Descend); return; }

        if (!PlayerHasDescendAbility())
        {
            Block_IsNot_Target(moveToBlock_Descend);
            return;
        }

        GameObject outObj1 = null;
        GameObject outObj2 = null;
        Vector3 playerPos = PlayerManager.Instance.player.transform.position;

        if (PerformMovementRaycast(playerPos + Vector3.down, Vector3.down, 2.5f, out outObj1) == RaycastHitObjects.BlockInfo)
        {
            //If hit is a Slab
            if (outObj1.GetComponent<BlockInfo>().blockType == BlockType.Slab)
            {
                //If second hit is nothing
                if (PerformMovementRaycast(outObj1.transform.position + (Vector3.up * 0.25f), Vector3.up, 1, out outObj2) == RaycastHitObjects.None)
                {
                    if (outObj1.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
                    {
                        if (PlayerHasSwimAbility())
                            Block_Is_Target(moveToBlock_Descend, outObj1);
                        else
                            Block_IsNot_Target(moveToBlock_Descend);
                    }
                    else if (outObj1 != blockStandingOn)
                    {
                        Block_Is_Target(moveToBlock_Descend, outObj1);
                    }
                    else
                        Block_IsNot_Target(moveToBlock_Descend);
                }

                //If second hit is a block
                else if (PerformMovementRaycast(outObj1.transform.position + (Vector3.up * 0.25f), Vector3.up, 1, out outObj2) == RaycastHitObjects.BlockInfo)
                {
                    if (outObj2.GetComponent<BlockInfo>().blockType == BlockType.Slab)
                    {
                        Block_Is_Target(moveToBlock_Descend, outObj1);
                    }
                    else
                        Block_IsNot_Target(moveToBlock_Descend);
                }
                
                //If hitting something else
                else
                    Block_IsNot_Target(moveToBlock_Descend);
            }
            else
            {
                //If second hit is nothing
                if (PerformMovementRaycast(outObj1.transform.position, Vector3.up, 1, out outObj2) == RaycastHitObjects.None)
                {
                    if (outObj1.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
                    {
                        if (PlayerHasSwimAbility())
                            Block_Is_Target(moveToBlock_Descend, outObj1);
                        else
                            Block_IsNot_Target(moveToBlock_Descend);
                    }
                    else if (outObj1 != blockStandingOn)
                    {
                        Block_Is_Target(moveToBlock_Descend, outObj1);
                    }
                    else
                        Block_IsNot_Target(moveToBlock_Descend);
                }
                else
                    Block_IsNot_Target(moveToBlock_Descend);
            }
        }
        else
            Block_IsNot_Target(moveToBlock_Descend);
    }

    void UpdateDashMovement()
    {
        //Forward
        UpdateDashMovements(moveToBlock_Dash_Forward, UpdatedDir(Vector3.forward));

        //Back
        UpdateDashMovements(moveToBlock_Dash_Back, UpdatedDir(Vector3.back));

        //Left
        UpdateDashMovements(moveToBlock_Dash_Left, UpdatedDir(Vector3.left));

        //Right
        UpdateDashMovements(moveToBlock_Dash_Right, UpdatedDir(Vector3.right));
    }
    void UpdateDashMovements(MoveOptions moveOption, Vector3 dir)
    {
        if (!PlayerHasDashAbility())
        {
            Block_IsNot_Target(moveOption);
            return;
        }

        GameObject outObj1 = null;
        GameObject outObj2 = null;
        GameObject outObj3 = null;
        Vector3 playerPos = PlayerManager.Instance.player.transform.position;

        Vector3 rayDir = Vector3.zero;

        if (isCeilingGrabbing)
            rayDir = Vector3.up;
        else
            rayDir = Vector3.down;

        float correction = 0;
        if (blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Stair)
            correction = 0.25f;

        if (PerformMovementRaycast(playerPos + (Vector3.up * correction), dir, 1, out outObj1) == RaycastHitObjects.BlockInfo
            && PerformMovementRaycast(playerPos + dir + (Vector3.up * correction), dir, 1, out outObj2) == RaycastHitObjects.None
            && PerformMovementRaycast(playerPos + dir + dir + (Vector3.up * correction), rayDir, 1, out outObj3) == RaycastHitObjects.BlockInfo)
        {
            if (outObj3.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
            {
                if (PlayerHasSwimAbility())
                    Block_Is_Target(moveOption, outObj3);
                else
                    Block_IsNot_Target(moveOption);
            }
            else if (outObj3 != blockStandingOn)
            {
                Block_Is_Target(moveOption, outObj3);
            }
            else
                Block_IsNot_Target(moveOption);
        }
        else
            Block_IsNot_Target(moveOption);
    }
    void UpdateJumpMovement()
    {

    }
    void UpdateGrapplingHookMovement()
    {

    }
    void UpdateCeilingGrabMovement()
    {

    }

    void Block_Is_Target(MoveOptions moveOption, GameObject obj)
    {
        moveOption.canMoveTo = true;
        moveOption.targetBlock = obj;
    }
    void Block_IsNot_Target(MoveOptions moveOption)
    {
        moveOption.canMoveTo = false;
        moveOption.targetBlock = null;
    }


    //--------------------


    #region PlayerHasAbility
    bool PlayerHasSwimAbility()
    {
        var stats = PlayerStats.Instance.stats;
        return stats.abilitiesGot_Permanent.SwimSuit ||
               stats.abilitiesGot_Permanent.Flippers ||
               stats.abilitiesGot_Permanent.SwiftSwim ||
               stats.abilitiesGot_Temporary.SwimSuit ||
               stats.abilitiesGot_Temporary.Flippers ||
               stats.abilitiesGot_Temporary.SwiftSwim;
    }
    bool PlayerHasDashAbility()
    {
        var stats = PlayerStats.Instance.stats;
        return stats.abilitiesGot_Permanent.Dash ||
               stats.abilitiesGot_Temporary.Dash;
    }
    bool PlayerHasJumpingAbility()
    {
        var stats = PlayerStats.Instance.stats;
        return stats.abilitiesGot_Permanent.Jumping ||
               stats.abilitiesGot_Temporary.Jumping;
    }
    bool PlayerHasAscendAbility()
    {
        var stats = PlayerStats.Instance.stats;
        return stats.abilitiesGot_Permanent.Ascend ||
               stats.abilitiesGot_Temporary.Ascend;
    }
    bool PlayerHasDescendAbility()
    {
        var stats = PlayerStats.Instance.stats;
        return stats.abilitiesGot_Permanent.Descend ||
               stats.abilitiesGot_Temporary.Descend;
    }
    bool PlayerHasGrapplingHookAbility()
    {
        var stats = PlayerStats.Instance.stats;
        return stats.abilitiesGot_Permanent.GrapplingHook ||
               stats.abilitiesGot_Temporary.GrapplingHook;
    }
    bool PlayerHasCeilingGrabAbility()
    {
        var stats = PlayerStats.Instance.stats;
        return stats.abilitiesGot_Permanent.CeilingGrab ||
               stats.abilitiesGot_Temporary.CeilingGrab;
    }

    #endregion
    
    #endregion


    //--------------------


    void SetDarkenBlocks()
    {
        if (Player_KeyInputs.Instance.forward_isPressed || Player_KeyInputs.Instance.back_isPressed || Player_KeyInputs.Instance.left_isPressed || Player_KeyInputs.Instance.right_isPressed || Player_KeyInputs.Instance.down_isPressed || Player_KeyInputs.Instance.up_isPressed) { return; }
        if (movementStates == MovementStates.Moving || movementStates == MovementStates.Falling) { return; }

        if (moveToBlock_Forward.targetBlock)
            SetAvailableBlock(moveToBlock_Forward.targetBlock);
        if (moveToBlock_Back.targetBlock)
            SetAvailableBlock(moveToBlock_Back.targetBlock);
        if (moveToBlock_Left.targetBlock)
            SetAvailableBlock(moveToBlock_Left.targetBlock);
        if (moveToBlock_Right.targetBlock)
            SetAvailableBlock(moveToBlock_Right.targetBlock);

        if (moveToBlock_Ascend.targetBlock)
            SetAvailableBlock(moveToBlock_Ascend.targetBlock);
        if (moveToBlock_Descend.targetBlock)
            SetAvailableBlock(moveToBlock_Descend.targetBlock);

        if (moveToBlock_SwiftSwimUp.targetBlock)
            SetAvailableBlock(moveToBlock_SwiftSwimUp.targetBlock);
        if (moveToBlock_SwiftSwimDown.targetBlock)
            SetAvailableBlock(moveToBlock_SwiftSwimDown.targetBlock);

        if (moveToBlock_Dash_Forward.targetBlock)
            SetAvailableBlock(moveToBlock_Dash_Forward.targetBlock);
        if (moveToBlock_Dash_Back.targetBlock)
            SetAvailableBlock(moveToBlock_Dash_Back.targetBlock);
        if (moveToBlock_Dash_Left.targetBlock)
            SetAvailableBlock(moveToBlock_Dash_Left.targetBlock);
        if (moveToBlock_Dash_Right.targetBlock)
            SetAvailableBlock(moveToBlock_Dash_Right.targetBlock);

        if (moveToBlock_Jump_Forward.targetBlock)
            SetAvailableBlock(moveToBlock_Jump_Forward.targetBlock);
        if (moveToBlock_Jump_Back.targetBlock)
            SetAvailableBlock(moveToBlock_Jump_Back.targetBlock);
        if (moveToBlock_Jump_Left.targetBlock)
            SetAvailableBlock(moveToBlock_Jump_Left.targetBlock);
        if (moveToBlock_Jump_Right.targetBlock)
            SetAvailableBlock(moveToBlock_Jump_Right.targetBlock);

        if (moveToBlock_GrapplingHook.targetBlock)
            SetAvailableBlock(moveToBlock_GrapplingHook.targetBlock);
        if (moveToCeilingGrabbing.targetBlock)
            SetAvailableBlock(moveToCeilingGrabbing.targetBlock);
    }
    void ResetDarkenBlocks()
    {
        if (moveToBlock_Forward.targetBlock)
            ResetAvailableBlock(moveToBlock_Forward.targetBlock);
        if (moveToBlock_Back.targetBlock)
            ResetAvailableBlock(moveToBlock_Back.targetBlock);
        if (moveToBlock_Left.targetBlock)
            ResetAvailableBlock(moveToBlock_Left.targetBlock);
        if (moveToBlock_Right.targetBlock)
            ResetAvailableBlock(moveToBlock_Right.targetBlock);

        if (moveToBlock_Ascend.targetBlock)
            ResetAvailableBlock(moveToBlock_Ascend.targetBlock);
        if (moveToBlock_Descend.targetBlock)
            ResetAvailableBlock(moveToBlock_Descend.targetBlock);

        if (moveToBlock_SwiftSwimUp.targetBlock)
            ResetAvailableBlock(moveToBlock_SwiftSwimUp.targetBlock);
        if (moveToBlock_SwiftSwimDown.targetBlock)
            ResetAvailableBlock(moveToBlock_SwiftSwimDown.targetBlock);

        if (moveToBlock_Dash_Forward.targetBlock)
            ResetAvailableBlock(moveToBlock_Dash_Forward.targetBlock);
        if (moveToBlock_Dash_Back.targetBlock)
            ResetAvailableBlock(moveToBlock_Dash_Back.targetBlock);
        if (moveToBlock_Dash_Left.targetBlock)
            ResetAvailableBlock(moveToBlock_Dash_Left.targetBlock);
        if (moveToBlock_Dash_Right.targetBlock)
            ResetAvailableBlock(moveToBlock_Dash_Right.targetBlock);

        if (moveToBlock_Jump_Forward.targetBlock)
            ResetAvailableBlock(moveToBlock_Jump_Forward.targetBlock);
        if (moveToBlock_Jump_Back.targetBlock)
            ResetAvailableBlock(moveToBlock_Jump_Back.targetBlock);
        if (moveToBlock_Jump_Left.targetBlock)
            ResetAvailableBlock(moveToBlock_Jump_Left.targetBlock);
        if (moveToBlock_Jump_Right.targetBlock)
            ResetAvailableBlock(moveToBlock_Jump_Right.targetBlock);

        if (moveToBlock_GrapplingHook.targetBlock)
            ResetAvailableBlock(moveToBlock_GrapplingHook.targetBlock);
        if (moveToCeilingGrabbing.targetBlock)
            ResetAvailableBlock(moveToCeilingGrabbing.targetBlock);
    }
    public void SetAvailableBlock(GameObject obj)
    {
        if (obj.GetComponent<BlockInfo>())
        {
            if (!obj.GetComponent<BlockInfo>().blockIsDark)
            {
                if (PlayerStats.Instance.stats.steps_Current <= 0 && obj.GetComponent<BlockInfo>().movementCost <= 0)
                    obj.GetComponent<BlockInfo>().SetDarkenColors();
                else if (PlayerStats.Instance.stats.steps_Current - obj.GetComponent<BlockInfo>().movementCost < 0)
                    ResetAvailableBlock(obj);
                else if (PlayerStats.Instance.stats.steps_Current <= 0)
                    ResetAvailableBlock(obj);
                else
                    obj.GetComponent<BlockInfo>().SetDarkenColors();
            }
        }
    }
    public void ResetAvailableBlock(GameObject obj)
    {
        if (obj.GetComponent<BlockInfo>())
        {
            //if (obj.GetComponent<BlockInfo>().blockIsDark)
            //{
            //    obj.GetComponent<BlockInfo>().ResetDarkenColor();
            //}

            obj.GetComponent<BlockInfo>().ResetDarkenColor();
        }
    }


    //--------------------


    void RunUpButton()
    {
        if (!RunSwiftSwimUp())
            RunAscend();
    }
    void RunDownButton()
    {
        if (!RunSwiftSwimDown())
            RunDescend();
    }
    bool RunSwiftSwimUp()
    {
        if (moveToBlock_SwiftSwimUp.canMoveTo)
        {
            PerformMovement(moveToBlock_SwiftSwimUp, MovementStates.Moving, 2);
            return true;
        }
        else
            return false;
    }
    bool RunSwiftSwimDown()
    {
        if (moveToBlock_SwiftSwimDown.canMoveTo)
        {
            PerformMovement(moveToBlock_SwiftSwimDown, MovementStates.Moving, 2);
            return true;
        }
        else
            return false;
    }
    bool RunAscend()
    {
        if (moveToBlock_Ascend.canMoveTo)
        {
            PerformMovement(moveToBlock_Ascend, MovementStates.Moving, abilitySpeed);
            return true;
        }
        else
            return false;
    }
    bool RunDescend()
    {
        if (moveToBlock_Descend.canMoveTo)
        {
            PerformMovement(moveToBlock_Descend, MovementStates.Moving, abilitySpeed);
            return true;
        }
        else
            return false;
    }


    //--------------------


    #region Movement

    public RaycastHitObjects PerformMovementRaycast(Vector3 objPos, Vector3 dir, float distance, out GameObject obj)
    {
        if (Physics.Raycast(objPos, dir, out hit, distance, MapManager.Instance.pickup_LayerMask))
        {
            if (hit.transform.GetComponent<BlockInfo>())
            {
                obj = hit.transform.gameObject;

                return RaycastHitObjects.BlockInfo;
            }
            else
            {
                obj = hit.transform.gameObject;

                return RaycastHitObjects.Other;
            }
        }

        obj = null;
        return RaycastHitObjects.None;
    }

    void MovementSetup()
    {
        //Rotate Player
        RotatePlayerBody_Setup();

        //Perform Movement, if possible
        if (Player_KeyInputs.Instance.forward_isPressed && moveToBlock_Forward.targetBlock && moveToBlock_Forward.canMoveTo)
            PerformMovement(moveToBlock_Forward, MovementStates.Moving, blockStandingOn.GetComponent<BlockInfo>().movementSpeed);
        else if (Player_KeyInputs.Instance.back_isPressed && moveToBlock_Back.targetBlock && moveToBlock_Back.canMoveTo)
            PerformMovement(moveToBlock_Back, MovementStates.Moving, blockStandingOn.GetComponent<BlockInfo>().movementSpeed);
        else if (Player_KeyInputs.Instance.left_isPressed && moveToBlock_Left.targetBlock && moveToBlock_Left.canMoveTo)
            PerformMovement(moveToBlock_Left, MovementStates.Moving, blockStandingOn.GetComponent<BlockInfo>().movementSpeed);
        else if (Player_KeyInputs.Instance.right_isPressed && moveToBlock_Right.targetBlock && moveToBlock_Right.canMoveTo)
            PerformMovement(moveToBlock_Right, MovementStates.Moving, blockStandingOn.GetComponent<BlockInfo>().movementSpeed);

        else if(Player_KeyInputs.Instance.forward_isPressed && moveToBlock_Dash_Forward.targetBlock && moveToBlock_Dash_Forward.canMoveTo)
            PerformMovement(moveToBlock_Dash_Forward, MovementStates.Moving, abilitySpeed);
        else if (Player_KeyInputs.Instance.back_isPressed && moveToBlock_Dash_Back.targetBlock && moveToBlock_Dash_Back.canMoveTo)
            PerformMovement(moveToBlock_Dash_Back, MovementStates.Moving, abilitySpeed);
        else if (Player_KeyInputs.Instance.left_isPressed && moveToBlock_Dash_Left.targetBlock && moveToBlock_Dash_Left.canMoveTo)
            PerformMovement(moveToBlock_Dash_Left, MovementStates.Moving, abilitySpeed);
        else if (Player_KeyInputs.Instance.right_isPressed && moveToBlock_Dash_Right.targetBlock && moveToBlock_Dash_Right.canMoveTo)
            PerformMovement(moveToBlock_Dash_Right, MovementStates.Moving, abilitySpeed);

        else if (Player_KeyInputs.Instance.forward_isPressed && moveToBlock_Jump_Forward.targetBlock && moveToBlock_Jump_Forward.canMoveTo)
            PerformMovement(moveToBlock_Jump_Forward, MovementStates.Moving, abilitySpeed);
        else if (Player_KeyInputs.Instance.back_isPressed && moveToBlock_Jump_Back.targetBlock && moveToBlock_Jump_Back.canMoveTo)
            PerformMovement(moveToBlock_Jump_Back, MovementStates.Moving, abilitySpeed);
        else if (Player_KeyInputs.Instance.left_isPressed && moveToBlock_Jump_Left.targetBlock && moveToBlock_Jump_Left.canMoveTo)
            PerformMovement(moveToBlock_Jump_Left, MovementStates.Moving, abilitySpeed);
        else if (Player_KeyInputs.Instance.right_isPressed && moveToBlock_Jump_Right.targetBlock && moveToBlock_Jump_Right.canMoveTo)
            PerformMovement(moveToBlock_Jump_Right, MovementStates.Moving, abilitySpeed);

        else if (Player_KeyInputs.Instance.up_isPressed && moveToBlock_SwiftSwimUp.targetBlock && moveToBlock_SwiftSwimUp.canMoveTo)
            RunUpButton();
        else if (Player_KeyInputs.Instance.down_isPressed && moveToBlock_SwiftSwimDown.targetBlock && moveToBlock_SwiftSwimDown.canMoveTo)
            RunDownButton();
        else if (Player_KeyInputs.Instance.up_isPressed && moveToBlock_Ascend.targetBlock && moveToBlock_Ascend.canMoveTo)
            RunUpButton();
        else if (Player_KeyInputs.Instance.down_isPressed && moveToBlock_Descend.targetBlock && moveToBlock_Descend.canMoveTo)
            RunDownButton();
    }
    public void PerformMovement(MoveOptions canMoveBlock, MovementStates moveState, float movementSpeed)
    {
        if (PlayerStats.Instance.stats.steps_Current >= canMoveBlock.targetBlock.GetComponent<BlockInfo>().movementCost)
        {
            ResetDarkenBlocks();

            StartCoroutine(Move(canMoveBlock.targetBlock.transform.position, moveState, movementSpeed));
        }
        else
        {
            RespawnPlayer();
        }
    }
    public void PerformMovement(Vector3 targetPos)
    {
        ResetDarkenBlocks();

        StartCoroutine(Move(targetPos, MovementStates.Moving, blockStandingOn.GetComponent<BlockInfo>().movementSpeed));
    }

    private IEnumerator Move(Vector3 endPos, MovementStates moveState, float movementSpeed)
    {
        print("1. Move: speed: " + movementSpeed);
        float counter = 0;

        Vector3 startPos = transform.position;
        Vector3 newEndPos = endPos + (Vector3.up * heightOverBlock);

        movementStates = moveState;

        float elapsed = 0f;
        float distance = Vector3.Distance(startPos, newEndPos);

        float currentSpeed;
        if (blockStandingOn && blockStandingOn.GetComponent<BlockInfo>())
            currentSpeed = baseTime / movementSpeed;
        else
            currentSpeed = baseTime / fallSpeed;

        float speedFactor = 1f / Mathf.Max(currentSpeed, 0.01f);
        float duration = distance / speedFactor; //movement time scales with distance and speed

        while (elapsed < duration)
        {
            counter += Time.deltaTime;
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(startPos, newEndPos, t);

            yield return null;
        }

        transform.position = newEndPos;

        UpdateBlockLookingAt();

        movementStates = MovementStates.Still;
        Action_StepTaken_Invoke();

        print("2. Move: speed: " + movementSpeed + " | Counter: " + counter);
    }


    #endregion


    //--------------------


    #region Falling

    public void StartFallingWithBlock()
    {
        if (blockStandingOn.GetComponent<BlockInfo>().movementState == MovementStates.Falling)
        {
            SetMovementState(MovementStates.Falling);

            ResetDarkenBlocks();
        }
    }
    void StartFallingWithNoBlock()
    {
        SetMovementState(MovementStates.Falling);

        ResetDarkenBlocks();
    }
    void PlayerIsFalling()
    {
        if (blockStandingOn)
        {
            if (Vector3.Distance(blockStandingOn.transform.position, gameObject.transform.position) < heightOverBlock + 0.1f)
            {
                gameObject.transform.position = blockStandingOn.transform.position + (Vector3.up * heightOverBlock);

                EndFalling();
                UpdateAvailableMovementBlocks();
            }
            else
            {
                gameObject.transform.SetPositionAndRotation(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - (fallSpeed * Time.deltaTime), gameObject.transform.position.z), gameObject.transform.rotation);
            }
        }
        else
        {
            //Just fall untill a block becomes "blockStandingOn"
            gameObject.transform.SetPositionAndRotation(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - (fallSpeed * Time.deltaTime), gameObject.transform.position.z), gameObject.transform.rotation);
        }
    }
    void EndFalling()
    {
        if (blockStandingOn)
        {
            if (blockStandingOn.GetComponent<BlockInfo>().movementState != MovementStates.Falling)
            {
                SetMovementState(MovementStates.Still);
                Action_LandedFromFalling_Invoke();
            }
        }
    }
    
    #endregion

    void IceGlideMovement()
    {

    }

    void LadderMovement()
    {

    }

    void FollowElevatorBlockMovement()
    {

    }


    //--------------------


    #region Rotating Player

    void RotatePlayerBody_Setup()
    {
        if (Player_KeyInputs.Instance.forward_isPressed)
            RotatePlayerBody(0);
        else if (Player_KeyInputs.Instance.back_isPressed)
            RotatePlayerBody(180);
        else if (Player_KeyInputs.Instance.left_isPressed)
            RotatePlayerBody(-90);
        else if (Player_KeyInputs.Instance.right_isPressed)
            RotatePlayerBody(90);
    }
    public void RotatePlayerBody(float rotationValue)
    {
        Transform playerBody = PlayerManager.Instance.playerBody.transform;

        // Ladder rotation
        if (Player_LadderMovement.Instance.isMovingOnLadder_Up || Player_LadderMovement.Instance.isMovingOnLadder_Down)
        {
            Quaternion ladderRot = Player_LadderMovement.Instance.ladderToEnterRot;
            if (rotationValue != int.MinValue)
                ladderRot *= Quaternion.Euler(0, 180, 0);

            playerBody.SetPositionAndRotation(playerBody.position, ladderRot);
        }
        // Normal rotation
        else
        {
            float baseRotation = GetBaseCameraRotation(CameraController.Instance.cameraRotationState);
            float finalYRotation = NormalizeAngle(baseRotation + rotationValue);
            Quaternion newRotation = Quaternion.Euler(0, finalYRotation, 0);
            playerBody.SetPositionAndRotation(playerBody.position, newRotation);

            CameraController.Instance.directionFacing = GetFacingDirection(finalYRotation);
        }

        Action_BodyRotated_Invoke();
    }
    float GetBaseCameraRotation(CameraRotationState state)
    {
        switch (state)
        {
            case CameraRotationState.Forward: return 0;
            case CameraRotationState.Backward: return 180;
            case CameraRotationState.Left: return 90;
            case CameraRotationState.Right: return -90;
            default: return 0;
        }
    }
    Vector3 GetFacingDirection(float yRotation)
    {
        int angle = Mathf.RoundToInt(NormalizeAngle(yRotation));

        switch (angle)
        {
            case 0:
            case 360:
                return Vector3.forward;
            case 90:
                return Vector3.right;
            case 180:
                return Vector3.back;
            case 270:
                return Vector3.left;
            default:
                return Vector3.zero; // Optional: fallback if angle is not expected
        }
    }
    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0) angle += 360f;
        return angle;
    }

    #endregion

    public void UpdateBlockLookingAt()
    {
        float yRotation = Mathf.Round(PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.y) % 360;

        //Normalize angle to range [0, 360)
        if (yRotation < 0)
            yRotation += 360;

        switch ((int)yRotation)
        {
            case 0:
            case 360:
                lookDir = Vector3.forward;
                break;
            case 90:
                lookDir = Vector3.right;
                break;
            case 180:
                lookDir = Vector3.back;
                break;
            case 270:
                lookDir = Vector3.left;
                break;

            default:
                lookDir = Vector3.forward;
                break;
        }

        Movement.Instance.lookingDirection = lookDir;
    }


    //--------------------


    public Vector3 UpdatedDir(Vector3 direction)
    {
        //Direction Converter
        switch (CameraController.Instance.cameraRotationState)
        {
            case CameraRotationState.Forward:
                if (direction == Vector3.forward)
                    return Vector3.forward;
                else if (direction == Vector3.back)
                    return Vector3.back;
                else if (direction == Vector3.left)
                    return Vector3.left;
                else if (direction == Vector3.right)
                    return Vector3.right;
                break;
            case CameraRotationState.Backward:
                if (direction == Vector3.back)
                    return Vector3.forward;
                else if (direction == Vector3.forward)
                    return Vector3.back;
                else if (direction == Vector3.right)
                    return Vector3.left;
                else if (direction == Vector3.left)
                    return Vector3.right;
                break;
            case CameraRotationState.Left:
                if (direction == Vector3.left)
                    return Vector3.forward;
                else if (direction == Vector3.right)
                    return Vector3.back;
                else if (direction == Vector3.back)
                    return Vector3.left;
                else if (direction == Vector3.forward)
                    return Vector3.right;
                break;
            case CameraRotationState.Right:
                if (direction == Vector3.right)
                    return Vector3.forward;
                else if (direction == Vector3.left)
                    return Vector3.back;
                else if (direction == Vector3.forward)
                    return Vector3.left;
                else if (direction == Vector3.back)
                    return Vector3.right;
                break;

            default:
                return Vector3.forward;
        }

        return Vector3.forward;
    }


    //--------------------


    public void TakeAStep()
    {
        //Reduce available steps
        if (blockStandingOn)
        {
            if (blockStandingOn.GetComponent<BlockInfo>() /*&& !PlayerManager.Instance.isTransportingPlayer*/ && !Player_Pusher.Instance.playerIsPushed)
            {
                PlayerStats.Instance.stats.steps_Current -= blockStandingOn.GetComponent<BlockInfo>().movementCost;
            }
        }

        //If steps is < 0
        if (PlayerStats.Instance.stats.steps_Current < 0)
        {
            PlayerStats.Instance.stats.steps_Current = 0;
            RespawnPlayer();
        }

        Action_StepTaken_Late_Invoke();
    }
    public void RespawnPlayer()
    {
        StartCoroutine(Resetplayer(0.01f));
    }
    IEnumerator Resetplayer(float waitTime)
    {
        Player_KeyInputs.Instance.forward_isPressed = false;
        Player_KeyInputs.Instance.back_isPressed = false;
        Player_KeyInputs.Instance.left_isPressed = false;
        Player_KeyInputs.Instance.right_isPressed = false;
        Player_KeyInputs.Instance.up_isPressed = false;
        Player_KeyInputs.Instance.down_isPressed = false;

        SetMovementState(MovementStates.Moving);

        RespawnPlayerEarly_Action();

        yield return new WaitForSeconds(waitTime);

        //Move player
        transform.position = MapManager.Instance.playerStartPos;
        transform.SetPositionAndRotation(MapManager.Instance.playerStartPos, Quaternion.identity);

        //Reset for CeilingAbility
        Player_CeilingGrab.Instance.ResetCeilingGrab();

        //Player_DarkenBlock.Instance.block_hasBeenDarkened = false;

        yield return new WaitForSeconds(waitTime);

        //Refill Steps to max + stepPickups gotten
        PlayerStats.Instance.RefillStepsToMax();

        CameraController.Instance.ResetCameraRotation();
        RotatePlayerBody(180);

        RespawnPlayer_Action();

        yield return new WaitForSeconds(waitTime * 30);

        SetMovementState(MovementStates.Still);
        RespawnPlayerLate_Action();
    }


    //--------------------


    public void SetMovementState(MovementStates state)
    {
        movementStates = state;
    }
    public MovementStates GetMovementState()
    {
        return movementStates;
    }


    //--------------------


    public void Action_StepTaken_Invoke()
    {
        Action_StepTaken?.Invoke();
    }
    public void Action_StepTaken_Late_Invoke()
    {
        Action_StepTaken_Late?.Invoke();
    }
    public void Action_BodyRotated_Invoke()
    {
        Action_BodyRotated?.Invoke();
    }
    public void Action_isSwitchingBlocks_Invoke()
    {
        Action_isSwitchingBlocks?.Invoke();
    }
    public void Action_LandedFromFalling_Invoke()
    {
        Action_LandedFromFalling?.Invoke();
    }


    public void RespawnPlayerEarly_Action()
    {
        Action_RespawnPlayerEarly?.Invoke();
    }
    public void RespawnPlayer_Action()
    {
        Action_RespawnPlayer?.Invoke();
    }
    public void RespawnPlayerLate_Action()
    {
        Action_RespawnPlayerLate?.Invoke();
    }
    public void RespawnToSavePos_Action()
    {
        Action_RespawnToSavePos?.Invoke();
    }
}

[Serializable]
public class MoveOptions
{
    public bool canMoveTo;
    public GameObject targetBlock;
}

public enum RaycastHitObjects
{
    None,

    BlockInfo,
    Other,
}
public enum MovementStates
{
    Still,
    Moving,
    Falling,

    Ability
}
public enum MovementDirection
{
    None,

    Forward,
    Backward,
    Left,
    Right
}