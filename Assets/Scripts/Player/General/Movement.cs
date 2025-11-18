using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : Singleton<Movement>
{
    public static event Action Action_RespawnToSavePos;
    public static event Action Action_RespawnPlayerEarly;
    public static event Action Action_RespawnPlayer;
    public static event Action Action_RespawnPlayerLate;

    public static event Action Action_UpdatedBlocks;

    public static event Action Action_StepTaken_Early;
    public static event Action Action_StepTaken;
    public static event Action Action_StepTaken_Late;
    public static event Action Action_BodyRotated;
    public static event Action Action_isSwitchingBlocks;
    public static event Action Action_LandedFromFalling;

    public static event Action Action_PickupAnimation_Complete;

    #region Variables

    [Header("States")]
    public bool isMoving;
    public MovementStates movementStates = MovementStates.Still;

    [Header("Stats")]
    public float heightOverBlock = 0.95f;
    float baseTime = 1;
    public float fallSpeed = 8f;
    float abilitySpeed = 8;
    float grapplingLength = 5;
    public Vector3 savePos;

    [Header("BlockIsStandingOn")]
    public Vector3 lookingDirection;
    [SerializeField] string lookingDirectionDescription;
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
    public Vector3 tempGrapplingTaregtPos;

    public MoveOptions moveToLadder_Forward;
    public MoveOptions moveToLadder_Back;
    public MoveOptions moveToLadder_Left;
    public MoveOptions moveToLadder_Right;

    public bool isUpdatingDarkenBlocks;

    public bool performGrapplingHooking;
    public bool grapplingTargetHasBeenSet;
    public bool grapplingTowardsStair;
    public List<GameObject> grapplingObjects = new List<GameObject>();

    public Vector3 previousPosition;
    public Vector3 teleportMovementDir;

    [Header("Ladder")]
    public Vector3 ladderEndPos_Up;
    public Vector3 ladderEndPos_Down;
    public bool isMovingOnLadder_Up;
    public bool isMovingOnLadder_Down;

    Vector3 ladderClimbPos_Start;
    Vector3 ladderClimbPos_End;
    public int ladderPartsToClimb;
    [SerializeField] Quaternion ladderToEnterRot;

    public Vector3 elevatorPos_Previous;
    private Transform elevatorBeingFollowed = null;
    private Vector3 elevatorOffset = Vector3.zero;

    RaycastHit hit;

    public bool isJumping;
    public bool isGrapplingHooking;
    public bool isDashing;
    public bool isIceGliding;
    public bool isAscending;
    public bool isDecending;

    [Header("Animations")]
    public Animator anim;
    [SerializeField] bool blink;
    [SerializeField] bool secondaryIdle;
    [SerializeField] bool walkAnimationCheck;

    [Header("Temp Movement Cost for Slope Gliding")]
    [SerializeField] bool hasSlopeGlided;
    [SerializeField] bool isSlopeGliding;
    #endregion


    //--------------------


    private void Start()
    {
        if (PlayerManager.Instance.playerBody.transform.GetComponent<Animator>())
        {
            anim = PlayerManager.Instance.playerBody.GetComponent<Animator>();
        }

        savePos = transform.position;
        previousPosition = transform.position;
        elevatorPos_Previous = transform.position;

        RespawnPlayer();
    }
    private void Update()
    {
        //Movement
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
            if (PlayerManager.Instance.pauseGame) return;
            MovementSetup();
        }

        if (Player_KeyInputs.Instance.grapplingHook_isPressed && !grapplingTargetHasBeenSet && !Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            UpdateGrapplingHookMovement(moveToBlock_GrapplingHook, lookDir);
            grapplingTargetHasBeenSet = true;
        }

        CancelSlopeIfFalling();
    }


    //--------------------


    private void OnEnable()
    {
        Action_StepTaken += UpdateAvailableMovementBlocks;
        Action_RespawnPlayerLate += UpdateAvailableMovementBlocks;
        Action_LandedFromFalling += UpdateAvailableMovementBlocks;

        Action_RespawnPlayerEarly += RespawnUnderGrappling;

        Action_BodyRotated += UpdateLookDir;

        Action_RespawnPlayerEarly += ResetDarkenBlocks;
        Action_StepTaken += TakeAStep;

        Action_isSwitchingBlocks += UpdateStepsAmonutWhenGrapplingMoving;
        Action_StepTaken_Late += RunIceGliding;
        Action_StepTaken_Late += CheckIfSwimming;

        CameraController.Action_RotateCamera_End += UpdateBlocks;

        Player_KeyInputs.Action_WalkButton_isReleased += WalkButtonIsReleased;

        SFX_Respawn.Action_RespawnPlayer += RespawnPlayer;

        Interactable_Pickup.Action_AbilityPickupGot += UpdateLookDir;

        Interactable_Pickup.Action_EssencePickupGot += Temp_EssencePickupGot_Animation;
        Interactable_Pickup.Action_StepsUpPickupGot += Temp_StepsUpPickupGot_Animation;
        Interactable_Pickup.Action_SkinPickupGot += Temp_SkinPickupGot_Animation;
        Interactable_Pickup.Action_AbilityPickupGot += Temp_AbilityPickupGot_Animation;
    }
    private void OnDisable()
    {
        Action_StepTaken -= UpdateAvailableMovementBlocks;
        Action_RespawnPlayerLate -= UpdateAvailableMovementBlocks;
        Action_LandedFromFalling -= UpdateAvailableMovementBlocks;

        Action_RespawnPlayerEarly -= RespawnUnderGrappling;

        Action_BodyRotated -= UpdateLookDir;

        Action_RespawnPlayerEarly -= ResetDarkenBlocks;
        Action_StepTaken -= TakeAStep;

        Action_isSwitchingBlocks -= UpdateStepsAmonutWhenGrapplingMoving;
        Action_StepTaken_Late -= RunIceGliding;
        Action_StepTaken_Late -= CheckIfSwimming;

        CameraController.Action_RotateCamera_End -= UpdateBlocks;

        Player_KeyInputs.Action_WalkButton_isReleased -= WalkButtonIsReleased;

        SFX_Respawn.Action_RespawnPlayer -= RespawnPlayer;

        Interactable_Pickup.Action_AbilityPickupGot -= UpdateLookDir;

        Interactable_Pickup.Action_EssencePickupGot -= Temp_EssencePickupGot_Animation;
        Interactable_Pickup.Action_StepsUpPickupGot -= Temp_StepsUpPickupGot_Animation;
        Interactable_Pickup.Action_SkinPickupGot -= Temp_SkinPickupGot_Animation;
        Interactable_Pickup.Action_AbilityPickupGot -= Temp_AbilityPickupGot_Animation;
    }


    //--------------------


    #region Movement Functions

    public void UpdateAvailableMovementBlocks()
    {
        ResetDarkenBlocks();

        UpdateBlocks();

        SetDarkenBlocks();

        Action_UpdatedBlocks?.Invoke();
    }
    public void UpdateBlocks()
    {
        isUpdatingDarkenBlocks = true;

        UpdateBlockStandingOn();

        if (blockStandingOn /*&& movementStates == MovementStates.Still*/)
        {
            UpdateNormalMovement();

            UpdateSwiftSwimMovement(moveToBlock_SwiftSwimUp, Vector3.up); //Must come before Ascend
            UpdateSwiftSwimMovement(moveToBlock_SwiftSwimDown, Vector3.down); //Must come before Descend

            UpdateAscendMovement();
            UpdateDescendMovement();

            UpdateDashMovement();

            UpdateJumpMovement();

            FindLadderExitBlock();
        }
        else
        {
            StartFallingWithNoBlock();
        }

        isUpdatingDarkenBlocks = false;

        CameraController.Instance.isRotating = false;
    }

    //public void UpdateBlockStandingOn()
    //{
    //    GameObject obj = null;
    //    GameObject objTemp = blockStandingOn;
    //    Vector3 playerPos = PlayerManager.Instance.player.transform.position;

    //    if (blockStandingOn_Previous != blockStandingOn && !Player_CeilingGrab.Instance.isCeilingGrabbing)
    //    {
    //        blockStandingOn_Previous = blockStandingOn;
    //    }

    //    Vector3 rayDir = Vector3.zero;
    //    if (Player_CeilingGrab.Instance.isCeilingGrabbing)
    //    {
    //        rayDir = Vector3.up;
    //    }
    //    else
    //    {
    //        rayDir = Vector3.down;
    //    }

    //    PerformMovementRaycast(playerPos, rayDir, 1, out obj);

    //    if (blockStandingOn != obj)
    //    {
    //        blockStandingOn = null;
    //    }

    //    blockStandingOn = obj;

    //    //Check if the player has moved over to a new block
    //    if (objTemp != blockStandingOn)
    //    {
    //        Action_isSwitchingBlocks_Invoke();
    //    }
    //}

    public void UpdateBlockStandingOn()
    {
        GameObject obj = null;
        GameObject prev = blockStandingOn;

        Vector3 playerPos = PlayerManager.Instance.player.transform.position;
        Vector3 rayDir = Player_CeilingGrab.Instance.isCeilingGrabbing ? Vector3.up : Vector3.down;

        // Use a blocks-only mask here (see Step 2)
        if (TryGetBlockUnder(playerPos, rayDir, 1.25f, out obj))
        {
            blockStandingOn = obj;
        }
        else
        {
            blockStandingOn = null;
        }

        if (blockStandingOn_Previous != blockStandingOn && !Player_CeilingGrab.Instance.isCeilingGrabbing)
            blockStandingOn_Previous = prev;

        if (prev != blockStandingOn)
            Action_isSwitchingBlocks_Invoke();
    }
    bool TryGetBlockUnder(Vector3 origin, Vector3 dir, float distance, out GameObject block)
    {
        // Start a hair above the feet to avoid self-hit/overlap issues
        origin += dir * -0.05f; // if dir is down, this nudges up a bit

        if (Physics.Raycast(origin, dir, out var hit, distance, MapManager.Instance.player_LayerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.TryGetComponent<BlockInfo>(out _))
            {
                block = hit.transform.gameObject;
                return true;
            }
        }

        block = null;
        return false;
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

        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
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
                    else if (outObj2.GetComponent<BlockInfo>().blockElement == BlockElement.Lava)
                    {
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
                    else if (outObj2.GetComponent<BlockInfo>().blockElement == BlockElement.Lava)
                    {
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
                if (PerformMovementRaycast(playerPos, dir, 1, out outObj1) == RaycastHitObjects.None && PerformMovementRaycast(playerPos + dir, rayDir, 1, out outObj2) == RaycastHitObjects.BlockInfo)
                {
                    BlockInfo targetInfo = outObj2.GetComponent<BlockInfo>();

                    if (targetInfo.blockType == BlockType.Stair || targetInfo.blockType == BlockType.Slope)
                    {
                        // Compare forward directions flattened to XZ
                        Vector3 forwardCurrent = blockStandingOn.transform.forward;
                        Vector3 forwardTarget = outObj2.transform.forward;

                        forwardCurrent.y = 0;
                        forwardTarget.y = 0;

                        forwardCurrent.Normalize();
                        forwardTarget.Normalize();

                        float dot = Vector3.Dot(forwardCurrent, forwardTarget);

                        if (dot > 0.9f) // Check for same direction
                            Block_Is_Target(moveOption, outObj2);
                        else
                            Block_IsNot_Target(moveOption);
                    }
                    else
                    {
                        Block_IsNot_Target(moveOption);
                    }
                }
                else
                {
                    Block_IsNot_Target(moveOption);
                }
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
                else if (outObj2.GetComponent<BlockInfo>().blockElement == BlockElement.Lava)
                {
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

            //If the first hit is a Block
            else
            {
                if (outObj1)
                {
                    if (outObj1.GetComponent<BlockInfo>())
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
                                if (PlayerHasSwiftSwimAbility() /*PlayerHasSwimAbility() PlayerStats.Instance.stats.abilitiesGot_Temporary.SwiftSwim && PlayerStats.Instance.stats.abilitiesGot_Permanent.SwiftSwim*/)
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
        }

        Block_IsNot_Target(moveOption);
    }

    void UpdateSwiftSwimMovement(MoveOptions swiftSwimOption, Vector3 dir)
    {
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.SwiftSwim && !PlayerStats.Instance.stats.abilitiesGot_Permanent.SwiftSwim)
        {
            Block_IsNot_Target(swiftSwimOption);
            return;
        }

        GameObject outObj1 = null;
        GameObject outObj2 = null;
        Vector3 playerPos = PlayerManager.Instance.player.transform.position;

        if (PerformMovementRaycast(blockStandingOn.transform.position, dir, 1, out outObj1) == RaycastHitObjects.BlockInfo)
        {
            BlockInfo hitBlock = outObj1.GetComponent<BlockInfo>();

            print("100. SwiftSwimBlock Detected Above: " + outObj1.name);

            if (hitBlock.blockElement == BlockElement.Water)
            {
                Block_Is_Target(swiftSwimOption, outObj1);
            }
            else
                Block_IsNot_Target(swiftSwimOption);
        }
        else
        {
            Block_IsNot_Target(swiftSwimOption);
        }
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
                    else if (outObj1.GetComponent<BlockInfo>().blockElement == BlockElement.Lava)
                    {
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
                    else if (outObj1.GetComponent<BlockInfo>().blockElement == BlockElement.Lava)
                    {
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
                    else if (outObj2.GetComponent<BlockInfo>().blockElement == BlockElement.Lava)
                    {
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
                    else if (outObj1.GetComponent<BlockInfo>().blockElement == BlockElement.Lava)
                    {
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
                    else if (outObj1.GetComponent<BlockInfo>().blockElement == BlockElement.Lava)
                    {
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

        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
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
            else if (outObj3.GetComponent<BlockInfo>().blockElement == BlockElement.Lava)
            {
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
        //Forward
        UpdateJumpMovements(moveToBlock_Jump_Forward, UpdatedDir(Vector3.forward));

        //Back
        UpdateJumpMovements(moveToBlock_Jump_Back, UpdatedDir(Vector3.back));

        //Left
        UpdateJumpMovements(moveToBlock_Jump_Left, UpdatedDir(Vector3.left));

        //Right
        UpdateJumpMovements(moveToBlock_Jump_Right, UpdatedDir(Vector3.right));
    }
    void UpdateJumpMovements(MoveOptions moveOption, Vector3 dir)
    {
        if (!PlayerHasJumpAbility())
        {
            Block_IsNot_Target(moveOption);
            return;
        }

        GameObject finalTarget = null;
        Vector3 playerPos = PlayerManager.Instance.player.transform.position;
        Vector3 rayDir = Player_CeilingGrab.Instance.isCeilingGrabbing ? Vector3.up : Vector3.down;

        //Try raycasts with two different height offsets (normal and stair)
        bool success =
            TryPerformJumpWithCorrection(playerPos, dir, -0.25f, out finalTarget) ||
            TryPerformJumpWithCorrection(playerPos, dir, 0.25f, out finalTarget);

        if (success)
        {
            BlockInfo info = finalTarget.GetComponent<BlockInfo>();

            if (info.blockElement == BlockElement.Water)
            {
                if (PlayerHasSwimAbility())
                    Block_Is_Target(moveOption, finalTarget);
                else
                    Block_IsNot_Target(moveOption);
            }
            else if (info.blockElement == BlockElement.Lava)
            {
                Block_IsNot_Target(moveOption);
            }
            else if (info.blockType == BlockType.Stair || info.blockType == BlockType.Slope)
            {
                Vector3 toPlayerFlat = -dir.normalized;
                Vector3 blockForwardFlat = finalTarget.transform.forward;
                blockForwardFlat.y = 0;
                blockForwardFlat.Normalize();

                float dot = Vector3.Dot(blockForwardFlat, toPlayerFlat);

                if (dot < -0.9f) // Only jump to stair/slope if it faces away from player
                    Block_Is_Target(moveOption, finalTarget);
                else
                    Block_IsNot_Target(moveOption);
            }
            else if (finalTarget != blockStandingOn)
            {
                Block_Is_Target(moveOption, finalTarget);
            }
            else
            {
                Block_IsNot_Target(moveOption);
            }
        }

        else if (PerformMovementRaycast(playerPos, dir, 1, out GameObject outObj_1) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir, rayDir, 1, out GameObject outObj_2) == RaycastHitObjects.BlockInfo &&
            PerformMovementRaycast(playerPos + dir, dir, 1, out GameObject outObj_3) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir + dir, rayDir, 1, out GameObject outObj_4) == RaycastHitObjects.BlockInfo &&
            (outObj_2.GetComponent<BlockInfo>().blockType == BlockType.Stair || outObj_2.GetComponent<BlockInfo>().blockType == BlockType.Slope))
        {
            Vector3 stairForwardFlat = outObj_2.transform.forward;
            stairForwardFlat.y = 0;
            stairForwardFlat.Normalize();

            Vector3 dirFlat = dir;
            dirFlat.y = 0;
            dirFlat.Normalize();

            float dot = Vector3.Dot(stairForwardFlat, dirFlat);

            // Only block the jump if stair is facing directly AWAY from player
            if (dot > 0.9f)
            {
                Block_IsNot_Target(moveOption);
            }
            else
            {
                Block_Is_Target(moveOption, outObj_4);
            }
        }

        //If jumping over a water block
        else if (PerformMovementRaycast(playerPos, dir, 1, out GameObject outObj_5) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir, rayDir, 1, out GameObject outObj_6) == RaycastHitObjects.BlockInfo &&
            PerformMovementRaycast(playerPos + dir, dir, 1, out GameObject outObj_7) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir + dir, rayDir, 1, out GameObject outObj_8) == RaycastHitObjects.BlockInfo)
        {
            if (outObj_6.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
            {
                if (PlayerHasSwimAbility())
                    Block_IsNot_Target(moveOption);
                else
                    Block_Is_Target(moveOption, outObj_8);
            }
            else if (outObj_6.GetComponent<BlockInfo>().blockElement == BlockElement.Lava)
            {
                Block_IsNot_Target(moveOption);
            }
            else
                Block_IsNot_Target(moveOption);
        }

        //Jumping directly onto a stair/slope facing the player
        else if (PerformMovementRaycast(playerPos, dir, 1, out GameObject outObj1) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir, rayDir, 1, out GameObject outObj2) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir, dir, 1, out GameObject outObj3) == RaycastHitObjects.BlockInfo)
        {
            BlockInfo info = outObj3.GetComponent<BlockInfo>();

            if (info.blockType == BlockType.Stair || info.blockType == BlockType.Slope)
            {
                Vector3 toPlayerFlat = -dir.normalized;
                Vector3 stairForwardFlat = outObj3.transform.forward;
                stairForwardFlat.y = 0;
                stairForwardFlat.Normalize();

                float dot = Vector3.Dot(stairForwardFlat, toPlayerFlat);

                if (dot > 0.9f) // Only jump to stair/slope if it faces the player
                    Block_Is_Target(moveOption, outObj3);
                else
                    Block_IsNot_Target(moveOption);
            }
            else
            {
                Block_IsNot_Target(moveOption);
            }
        }

        //Jumping directly onto a stair/slope facing the player, if the block in between is a waterblock
        else if (PerformMovementRaycast(playerPos, dir, 1, out GameObject outObj4) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir, rayDir, 1, out GameObject outObj5) == RaycastHitObjects.BlockInfo &&
            PerformMovementRaycast(playerPos + dir, dir, 1, out GameObject outObj6) == RaycastHitObjects.BlockInfo)
        {
            if (outObj6.GetComponent<BlockInfo>().blockType == BlockType.Stair || outObj6.GetComponent<BlockInfo>().blockType == BlockType.Slope)
            {
                if (outObj5.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
                {
                    if (PlayerHasSwimAbility())
                    {
                        Block_IsNot_Target(moveOption);
                    }
                    else
                    {
                        Vector3 toPlayerFlat = -dir.normalized;
                        Vector3 stairForwardFlat = outObj6.transform.forward;
                        stairForwardFlat.y = 0;
                        stairForwardFlat.Normalize();

                        float dot = Vector3.Dot(stairForwardFlat, toPlayerFlat);

                        if (dot > 0.9f) // Only jump to stair/slope if it faces the player
                            Block_Is_Target(moveOption, outObj6);
                        else
                            Block_IsNot_Target(moveOption);
                    }
                }
                else if (outObj5.GetComponent<BlockInfo>().blockElement == BlockElement.Lava)
                {
                    Block_IsNot_Target(moveOption);
                }
            }
            else
            {
                Block_IsNot_Target(moveOption);
            }
        }
        else
        {
            Block_IsNot_Target(moveOption);
        }
    }
    bool TryPerformJumpWithCorrection(Vector3 playerPos, Vector3 dir, float correction, out GameObject targetBlock)
    {
        GameObject o1, o2, o3, o4;
        Vector3 rayDir = Player_CeilingGrab.Instance.isCeilingGrabbing ? Vector3.up : Vector3.down;

        targetBlock = null;

        //Ordinary jump
        if (PerformMovementRaycast(playerPos + (-rayDir * correction), dir, 1, out o1) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir + (-rayDir * correction), rayDir, 1, out o2) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir + (-rayDir * correction), dir, 1, out o3) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir + dir + (-rayDir * correction), rayDir, 1, out o4) == RaycastHitObjects.BlockInfo)
        {
            targetBlock = o4;
            return true;
        }

        //If there is a waterBlock in-between
        else if (PerformMovementRaycast(playerPos + (-rayDir * correction), dir, 1, out o1) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir + (-rayDir * correction), rayDir, 1, out o2) == RaycastHitObjects.BlockInfo &&
            PerformMovementRaycast(playerPos + dir + (-rayDir * correction), dir, 1, out o3) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir + dir + (-rayDir * correction), rayDir, 1, out o4) == RaycastHitObjects.BlockInfo)
        {
            if (o2.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
            {
                if (PlayerHasSwimAbility())
                {
                    return false;
                }
                else
                {
                    targetBlock = o4;
                    return true;
                }
            }
        }

        return false;
    }

    public void UpdateGrapplingHookMovement(MoveOptions moveOption, Vector3 dir)
    {
        if (!PlayerHasGrapplingHookAbility())
        {
            Block_IsNot_Target(moveOption);
            return;
        }

        Player_GraplingHook.Instance.isGrapplingHooking = true;
        Player_GraplingHook.Instance.EndLineRenderer();

        GameObject outObj1 = null;
        Vector3 playerPos = transform.position;

        if (PerformMovementRaycast(playerPos, dir, grapplingLength, out outObj1) == RaycastHitObjects.BlockInfo)
        {
            Collider objCollider = outObj1.GetComponent<Collider>();
            Vector3 contactPoint = objCollider != null
                ? objCollider.ClosestPoint(playerPos + dir * (grapplingLength + 1))
                : outObj1.transform.position + (Vector3.forward * (grapplingLength + 1));

            Player_GraplingHook.Instance.endPoint = contactPoint + (-dir * 0.05f);

            Block_Is_Target(moveOption, outObj1);

            if (moveOption.targetBlock.GetComponent<BlockInfo>().blockType == BlockType.Stair || moveOption.targetBlock.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                grapplingTowardsStair = true;
            else
                grapplingTowardsStair = false;

            // Check if Stari/Slope is facing the player
            Vector3 toPlayer = (transform.position - moveToBlock_GrapplingHook.targetBlock.transform.position).normalized;
            bool StairIsFacingPlayer = (moveToBlock_GrapplingHook.targetBlock.GetComponent<BlockInfo>().blockType == BlockType.Stair || moveToBlock_GrapplingHook.targetBlock.GetComponent<BlockInfo>().blockType == BlockType.Slope) && Vector3.Dot(moveToBlock_GrapplingHook.targetBlock.transform.forward, toPlayer) > 0.5f;

            if (StairIsFacingPlayer)
                Player_GraplingHook.Instance.redDotSceneObject.transform.SetPositionAndRotation(Player_GraplingHook.Instance.endPoint - (dir * 0.5f), Quaternion.LookRotation(dir));
            else
                Player_GraplingHook.Instance.redDotSceneObject.transform.SetPositionAndRotation(Player_GraplingHook.Instance.endPoint - dir, Quaternion.LookRotation(dir));

            Player_GraplingHook.Instance.redDotSceneObject.SetActive(true);
            Player_GraplingHook.Instance.RunLineReader();

            UpdateBlocksOnTheGrapplingWay(moveOption);

            return;
        }
        else
        {
            Player_GraplingHook.Instance.endPoint = transform.position + (dir * grapplingLength);

            Player_GraplingHook.Instance.redDotSceneObject.SetActive(false);
            Player_GraplingHook.Instance.RunLineReader();

            Block_IsNot_Target(moveOption);
            return;
        }  
    }
    public void UpdateGrapplingHookMovement_Release()
    {
        if (moveToBlock_GrapplingHook.targetBlock && moveToBlock_GrapplingHook.canMoveTo)
        {
            //Prevent Grappling when standing against a wall
            if (Vector3.Distance(transform.position, moveToBlock_GrapplingHook.targetBlock.transform.position) > 1.1f)
            {
                tempGrapplingTaregtPos = moveToBlock_GrapplingHook.targetBlock.transform.position;
                performGrapplingHooking = true;
                RunGrapplingHook();

                Player_GraplingHook.Instance.isGrapplingHooking = false;
                grapplingTargetHasBeenSet = false;

                ResetBlocksOnTheGrapplingWay();

                return;
            }
        }

        if (moveToBlock_GrapplingHook != null)
            Block_IsNot_Target(moveToBlock_GrapplingHook);

        Player_GraplingHook.Instance.redDotSceneObject.SetActive(false);
        Player_GraplingHook.Instance.EndLineRenderer();
        Player_GraplingHook.Instance.isGrapplingHooking = false;
        grapplingTargetHasBeenSet = false;
    }
    void UpdateBlocksOnTheGrapplingWay(MoveOptions moveOption)
    {
        Vector3 playerPos = transform.position; // or wherever your player is
        Vector3 dir = (moveOption.targetBlock.transform.position - playerPos).normalized; // direction from player to hit
        float totalDistance = Vector3.Distance(playerPos, moveOption.targetBlock.transform.position);
        int steps = Mathf.FloorToInt(totalDistance); // one check per unit

        GameObject outObj1 = null;

        for (int i = 1; i <= steps - 1; i++)
        {
            Vector3 samplePos = playerPos + dir * i; // sample point along the line

            // Cast downward from slightly above to ensure hit
            if (PerformMovementRaycast(samplePos, Vector3.down, 1, out outObj1) == RaycastHitObjects.BlockInfo)
            {
                grapplingObjects.Add(outObj1);

                grapplingObjects[grapplingObjects.Count - 1].GetComponent<BlockInfo>().SetDarkenColors();
            }
        }

        if (grapplingTowardsStair)
        {
            if (PerformMovementRaycast(playerPos + dir * (steps - 1), dir, 1, out outObj1) == RaycastHitObjects.BlockInfo)
            {
                grapplingObjects.Add(outObj1);

                grapplingObjects[grapplingObjects.Count - 1].GetComponent<BlockInfo>().SetDarkenColors();
            }
        }
    }
    void ResetBlocksOnTheGrapplingWay()
    {
        for (int i = grapplingObjects.Count - 1; i >= 0; i--)
        {
            grapplingObjects[i].GetComponent<BlockInfo>().ResetDarkenColor();
            grapplingObjects.RemoveAt(i);
        }
    }
    void UpdateStepsAmonutWhenGrapplingMoving()
    {
        if (performGrapplingHooking && blockStandingOn
            && ((grapplingTowardsStair && Vector3.Distance(transform.position, tempGrapplingTaregtPos) > 1.5f) || (!grapplingTowardsStair && Vector3.Distance(blockStandingOn.transform.position, tempGrapplingTaregtPos) > 1.5f)))
        {
            if (PlayerStats.Instance.stats.steps_Current < 0)
            {
                RespawnPlayer();
                return;
            }

            PlayerStats.Instance.stats.steps_Current -= blockStandingOn.GetComponent<BlockInfo>().movementCost;
            StepsHUD.Instance.UpdateStepsDisplay();
        }
    }
    void RespawnUnderGrappling()
    {
        grapplingTowardsStair = false;
        performGrapplingHooking = false;
        grapplingTargetHasBeenSet = false;
        ResetBlocksOnTheGrapplingWay();
    }

    void Block_Is_Target(MoveOptions moveOption, GameObject obj)
    {
        if (moveOption.targetBlock && moveOption.targetBlock != obj && moveOption.targetBlock.GetComponent<BlockInfo>() && moveOption.targetBlock.GetComponent<BlockInfo>().blockIsDark && !CameraController.Instance.isRotating && !CameraController.Instance.isCeilingRotating)
        {
            moveOption.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
        }

        moveOption.canMoveTo = true;
        moveOption.targetBlock = obj;
    }
    void Block_IsNot_Target(MoveOptions moveOption)
    {
        if (moveOption.targetBlock && moveOption.targetBlock.GetComponent<BlockInfo>() && moveOption.targetBlock.GetComponent<BlockInfo>().blockIsDark && !CameraController.Instance.isRotating && !CameraController.Instance.isCeilingRotating)
        {
            moveOption.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
        }

        moveOption.canMoveTo = false;
        moveOption.targetBlock = null;
    }


    //--------------------


    #region PlayerHasAbility
    public bool PlayerHasSwimAbility()
    {
        var stats = PlayerStats.Instance.stats;
        return stats.abilitiesGot_Permanent.SwimSuit ||
               stats.abilitiesGot_Permanent.Flippers ||
               stats.abilitiesGot_Permanent.SwiftSwim ||
               stats.abilitiesGot_Temporary.SwimSuit ||
               stats.abilitiesGot_Temporary.Flippers ||
               stats.abilitiesGot_Temporary.SwiftSwim;
    }
    public bool PlayerHasSwiftSwimAbility()
    {
        var stats = PlayerStats.Instance.stats;
        return stats.abilitiesGot_Permanent.SwiftSwim ||
               stats.abilitiesGot_Temporary.SwiftSwim;
    }
    bool PlayerHasDashAbility()
    {
        var stats = PlayerStats.Instance.stats;
        return stats.abilitiesGot_Permanent.Dash ||
               stats.abilitiesGot_Temporary.Dash;
    }
    bool PlayerHasJumpAbility()
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


    #region SetBlocks

    public void SetDarkenBlocks()
    {
        if (Player_KeyInputs.Instance.forward_isPressed || Player_KeyInputs.Instance.back_isPressed || Player_KeyInputs.Instance.left_isPressed || Player_KeyInputs.Instance.right_isPressed || Player_KeyInputs.Instance.up_isPressed || Player_KeyInputs.Instance.down_isPressed || Player_KeyInputs.Instance.grapplingHook_isPressed) { return; }
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

        if (moveToLadder_Forward.targetBlock)
            ResetAvailableBlock(moveToLadder_Forward.targetBlock);
        if (moveToLadder_Back.targetBlock)
            ResetAvailableBlock(moveToLadder_Back.targetBlock);
        if (moveToLadder_Left.targetBlock)
            ResetAvailableBlock(moveToLadder_Left.targetBlock);
        if (moveToLadder_Right.targetBlock)
            ResetAvailableBlock(moveToLadder_Right.targetBlock);
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
            obj.GetComponent<BlockInfo>().ResetDarkenColor();
        }
    }

    #endregion


    //--------------------


    #region Run Abilities

    bool RunSwiftSwimUp()
    {
        if (moveToBlock_SwiftSwimUp.canMoveTo)
        {
            MapManager.Instance.swiftSwimCounter++;
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
            MapManager.Instance.swiftSwimCounter++;
            PerformMovement(moveToBlock_SwiftSwimDown, MovementStates.Moving, 2);
            return true;
        }
        else
            return false;
    }
    void RunGrapplingHook()
    {
        if (moveToBlock_GrapplingHook.canMoveTo)
        {
            isGrapplingHooking = true;

            if (moveToBlock_GrapplingHook.targetBlock.GetComponent<BlockInfo>().blockType == BlockType.Stair || moveToBlock_GrapplingHook.targetBlock.GetComponent<BlockInfo>().blockType == BlockType.Slope)
            {
                // Check if Stari/Slope is facing the player
                Vector3 toPlayer = (transform.position - moveToBlock_GrapplingHook.targetBlock.transform.position).normalized;
                bool isFacingPlayer = Vector3.Dot(moveToBlock_GrapplingHook.targetBlock.transform.forward, toPlayer) > 0.5f;

                if (isFacingPlayer)
                    PerformMovement(moveToBlock_GrapplingHook.targetBlock.transform.position - lookDir.normalized + Vector3.down + (Vector3.up * 0.95f) + lookDir, abilitySpeed + grapplingLength);
                else
                    PerformMovement(moveToBlock_GrapplingHook.targetBlock.transform.position - lookDir.normalized + Vector3.down + (Vector3.up * 0.5f), abilitySpeed + grapplingLength);
            }
            else
                PerformMovement(moveToBlock_GrapplingHook.targetBlock.transform.position - lookDir.normalized + Vector3.down, abilitySpeed + grapplingLength);

            MapManager.Instance.grapplingHookCounter++;

            moveToBlock_GrapplingHook.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
            Block_IsNot_Target(moveToBlock_GrapplingHook);
            Player_GraplingHook.Instance.EndLineRenderer();
        }
    }
    void CheckAscend()
    {
        if (!RunSwiftSwimUp())
            RunAscend();
    }
    void CheckDescend()
    {
        if (!RunSwiftSwimDown())
            RunDescend();
    }
    bool RunAscend()
    {
        if (moveToBlock_Ascend.canMoveTo)
        {
            isAscending = true;
            PlayerCameraOcclusionController.Instance.CameraZoom(true);

            MapManager.Instance.ascendCounter++;
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
            isDecending = true;
            PlayerCameraOcclusionController.Instance.CameraZoom(true);

            MapManager.Instance.descendCounter++;
            PerformMovement(moveToBlock_Descend, MovementStates.Moving, abilitySpeed);
            return true;
        }
        else
            return false;
    }

    #endregion


    //--------------------


    #region Movement

    public RaycastHitObjects PerformMovementRaycast(Vector3 objPos, Vector3 dir, float distance, out GameObject obj)
    {
        int combinedMask = MapManager.Instance.pickup_LayerMask;

        if (Physics.Raycast(objPos, dir, out hit, distance, combinedMask))
        {
            if (hit.transform.GetComponent<BlockInfo>())
            {
                obj = hit.transform.gameObject;
                return RaycastHitObjects.BlockInfo;
            }
            else if (hit.transform.GetComponentInParent<Fence>())
            {
                //print("1. Fence");
                obj = null;
                return RaycastHitObjects.Fence;
            }
            else if (hit.transform.GetComponentInParent<Block_Ladder>() && hit.transform.GetComponent<LadderColliderBlocker>())
            {
                //print("2. LadderColliderBlocker");
                obj = null;
                return RaycastHitObjects.LadderBlocker;
            }
            else if (hit.transform.GetComponentInParent<Block_Ladder>() && hit.transform.GetComponent<LadderCollider>())
            {
                //print("3. LadderCollider");
                obj = hit.transform.parent.gameObject;

                return RaycastHitObjects.Ladder;
            }
            else
            {
                obj = hit.transform.gameObject;
                return RaycastHitObjects.Other;
            }
        }
        else
        {
            obj = null;

            return RaycastHitObjects.None;
        }
    }

    void MovementSetup()
    {
        //Rotate Player
        RotatePlayerBody_Setup();

        //Perform LadderMovement, if possible
        if (Player_KeyInputs.Instance.forward_isPressed && CheckLaddersToEnter_Up(UpdatedDir(Vector3.forward)) && moveToLadder_Forward.targetBlock && moveToLadder_Forward.canMoveTo)
            StartCoroutine(PerformLadderMovement_Up(UpdatedDir(Vector3.forward), GetLadderExitPart_Up(UpdatedDir(Vector3.forward))));
        else if (Player_KeyInputs.Instance.back_isPressed && CheckLaddersToEnter_Up(UpdatedDir(Vector3.back)) && moveToLadder_Back.targetBlock && moveToLadder_Back.canMoveTo)
            StartCoroutine(PerformLadderMovement_Up(UpdatedDir(Vector3.back), GetLadderExitPart_Up(UpdatedDir(Vector3.back))));
        else if (Player_KeyInputs.Instance.left_isPressed && CheckLaddersToEnter_Up(UpdatedDir(Vector3.left)) && moveToLadder_Left.targetBlock && moveToLadder_Left.canMoveTo)
            StartCoroutine(PerformLadderMovement_Up(UpdatedDir(Vector3.left), GetLadderExitPart_Up(UpdatedDir(Vector3.left))));
        else if (Player_KeyInputs.Instance.right_isPressed && CheckLaddersToEnter_Up(UpdatedDir(Vector3.right)) && moveToLadder_Right.targetBlock && moveToLadder_Right.canMoveTo)
            StartCoroutine(PerformLadderMovement_Up(UpdatedDir(Vector3.right), GetLadderExitPart_Up(UpdatedDir(Vector3.right))));

        else if (Player_KeyInputs.Instance.forward_isPressed && CheckLaddersToEnter_Down(UpdatedDir(Vector3.forward)) && moveToLadder_Forward.targetBlock && moveToLadder_Forward.canMoveTo)
            StartCoroutine(PerformLadderMovement_Down(UpdatedDir(Vector3.forward), GetLadderExitPart_Down(UpdatedDir(Vector3.forward))));
        else if (Player_KeyInputs.Instance.back_isPressed && CheckLaddersToEnter_Down(UpdatedDir(Vector3.back)) && moveToLadder_Back.targetBlock && moveToLadder_Back.canMoveTo)
            StartCoroutine(PerformLadderMovement_Down(UpdatedDir(Vector3.back), GetLadderExitPart_Down(UpdatedDir(Vector3.back))));
        else if (Player_KeyInputs.Instance.left_isPressed && CheckLaddersToEnter_Down(UpdatedDir(Vector3.left)) && moveToLadder_Left.targetBlock && moveToLadder_Left.canMoveTo)
            StartCoroutine(PerformLadderMovement_Down(UpdatedDir(Vector3.left), GetLadderExitPart_Down(UpdatedDir(Vector3.left))));
        else if (Player_KeyInputs.Instance.right_isPressed && CheckLaddersToEnter_Down(UpdatedDir(Vector3.right)) && moveToLadder_Right.targetBlock && moveToLadder_Right.canMoveTo)
            StartCoroutine(PerformLadderMovement_Down(UpdatedDir(Vector3.right), GetLadderExitPart_Down(UpdatedDir(Vector3.right))));

        //Perform Normal Movement, if possible
        else if(Player_KeyInputs.Instance.forward_isPressed && moveToBlock_Forward.targetBlock && moveToBlock_Forward.canMoveTo && blockStandingOn && blockStandingOn.GetComponent<BlockInfo>())
            PerformMovement(moveToBlock_Forward, MovementStates.Moving, blockStandingOn.GetComponent<BlockInfo>().movementSpeed);
        else if (Player_KeyInputs.Instance.back_isPressed && moveToBlock_Back.targetBlock && moveToBlock_Back.canMoveTo && blockStandingOn && blockStandingOn.GetComponent<BlockInfo>())
            PerformMovement(moveToBlock_Back, MovementStates.Moving, blockStandingOn.GetComponent<BlockInfo>().movementSpeed);
        else if (Player_KeyInputs.Instance.left_isPressed && moveToBlock_Left.targetBlock && moveToBlock_Left.canMoveTo && blockStandingOn && blockStandingOn.GetComponent<BlockInfo>())
            PerformMovement(moveToBlock_Left, MovementStates.Moving, blockStandingOn.GetComponent<BlockInfo>().movementSpeed);
        else if (Player_KeyInputs.Instance.right_isPressed && moveToBlock_Right.targetBlock && moveToBlock_Right.canMoveTo && blockStandingOn && blockStandingOn.GetComponent<BlockInfo>())
            PerformMovement(moveToBlock_Right, MovementStates.Moving, blockStandingOn.GetComponent<BlockInfo>().movementSpeed);

        //Perform Dash Movement, if possible
        else if (Player_KeyInputs.Instance.forward_isPressed && moveToBlock_Dash_Forward.targetBlock && moveToBlock_Dash_Forward.canMoveTo)
        {
            MapManager.Instance.dashCounter++;
            PerformMovement(moveToBlock_Dash_Forward, MovementStates.Moving, abilitySpeed, ref isDashing);
        }
        else if (Player_KeyInputs.Instance.back_isPressed && moveToBlock_Dash_Back.targetBlock && moveToBlock_Dash_Back.canMoveTo)
        {
            MapManager.Instance.dashCounter++;
            PerformMovement(moveToBlock_Dash_Back, MovementStates.Moving, abilitySpeed, ref isDashing);
        }
        else if (Player_KeyInputs.Instance.left_isPressed && moveToBlock_Dash_Left.targetBlock && moveToBlock_Dash_Left.canMoveTo)
        {
            MapManager.Instance.dashCounter++;
            PerformMovement(moveToBlock_Dash_Left, MovementStates.Moving, abilitySpeed, ref isDashing);
        }
        else if (Player_KeyInputs.Instance.right_isPressed && moveToBlock_Dash_Right.targetBlock && moveToBlock_Dash_Right.canMoveTo)
        {
            MapManager.Instance.dashCounter++;
            PerformMovement(moveToBlock_Dash_Right, MovementStates.Moving, abilitySpeed, ref isDashing);
        }

        //Perform Jump Movement, if possible
        else if (Player_KeyInputs.Instance.forward_isPressed && moveToBlock_Jump_Forward.targetBlock && moveToBlock_Jump_Forward.canMoveTo)
        {
            MapManager.Instance.jumpCounter++;
            PerformMovement(moveToBlock_Jump_Forward, MovementStates.Moving, abilitySpeed, ref isJumping);
        }
        else if (Player_KeyInputs.Instance.back_isPressed && moveToBlock_Jump_Back.targetBlock && moveToBlock_Jump_Back.canMoveTo)
        {
            MapManager.Instance.jumpCounter++;
            PerformMovement(moveToBlock_Jump_Back, MovementStates.Moving, abilitySpeed, ref isJumping);
        }
        else if (Player_KeyInputs.Instance.left_isPressed && moveToBlock_Jump_Left.targetBlock && moveToBlock_Jump_Left.canMoveTo)
        {
            MapManager.Instance.jumpCounter++;
            PerformMovement(moveToBlock_Jump_Left, MovementStates.Moving, abilitySpeed, ref isJumping);
        }
        else if (Player_KeyInputs.Instance.right_isPressed && moveToBlock_Jump_Right.targetBlock && moveToBlock_Jump_Right.canMoveTo)
        {
            MapManager.Instance.jumpCounter++;
            PerformMovement(moveToBlock_Jump_Right, MovementStates.Moving, abilitySpeed, ref isJumping);
        }

        //Perform SwiftSwim Movement, if possible
        else if (Player_KeyInputs.Instance.up_isPressed && moveToBlock_SwiftSwimUp.targetBlock && moveToBlock_SwiftSwimUp.canMoveTo)
            CheckAscend();
        else if (Player_KeyInputs.Instance.down_isPressed && moveToBlock_SwiftSwimDown.targetBlock && moveToBlock_SwiftSwimDown.canMoveTo)
            CheckDescend();

        //Perform Ascend/Descend Movement, if possible
        else if (Player_KeyInputs.Instance.up_isPressed && moveToBlock_Ascend.targetBlock && moveToBlock_Ascend.canMoveTo)
            CheckAscend();
        else if (Player_KeyInputs.Instance.down_isPressed && moveToBlock_Descend.targetBlock && moveToBlock_Descend.canMoveTo)
            CheckDescend();
    }
    public void PerformMovement(MoveOptions canMoveBlock, MovementStates moveState, float movementSpeed, ref bool isMoving)
    {
        if (canMoveBlock == null) { return; }
        if (canMoveBlock.targetBlock == null) { return; }
        if (!canMoveBlock.targetBlock.GetComponent<BlockInfo>()) { return; }
        if (PlayerStats.Instance.stats == null) { return; }

        if (PlayerStats.Instance.stats.steps_Current >= canMoveBlock.targetBlock.GetComponent<BlockInfo>().movementCost || Player_Pusher.Instance.playerIsPushed)
        {
            MovingAnimation(canMoveBlock);

            isMoving = true;

            ResetDarkenBlocks();

            StartCoroutine(Move(canMoveBlock.targetBlock.transform.position, moveState, movementSpeed, canMoveBlock));
        }
        else
        {
            RespawnPlayer();
        }
    }
    public void PerformMovement(MoveOptions canMoveBlock, MovementStates moveState, float movementSpeed)
    {
        if (canMoveBlock == null) { return; }
        if (canMoveBlock.targetBlock == null) { return; }
        if (!canMoveBlock.targetBlock.GetComponent<BlockInfo>()) { return; }
        if (PlayerStats.Instance.stats == null) { return; }

        if (PlayerStats.Instance.stats.steps_Current >= canMoveBlock.targetBlock.GetComponent<BlockInfo>().movementCost || Player_Pusher.Instance.playerIsPushed)
        {
            MovingAnimation(canMoveBlock);

            isMoving = true;

            ResetDarkenBlocks();

            StartCoroutine(Move(canMoveBlock.targetBlock.transform.position, moveState, movementSpeed, canMoveBlock));
        }
        else
        {
            RespawnPlayer();
        }
    }
    public void PerformMovement(Vector3 targetPos)
    {
        isMoving = true;

        ResetDarkenBlocks();

        StartCoroutine(Move(targetPos, MovementStates.Moving, blockStandingOn.GetComponent<BlockInfo>().movementSpeed, null));
    }
    public void PerformMovement(Vector3 targetPos, float movementSpeed)
    {
        isMoving = true;

        ResetDarkenBlocks();

        StartCoroutine(Move(targetPos, MovementStates.Moving, movementSpeed, null));
    }

    private IEnumerator Move(Vector3 endPos, MovementStates moveState, float movementSpeed, MoveOptions moveOptions)
    {
        Action_StepTaken_Early_Invoke();

        //Safety check for slope gliding
        if (blockStandingOn != null && blockStandingOn.GetComponent<BlockInfo>() && blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Slope)
        {
            print("1. hasSlopeGlided");
            hasSlopeGlided = true;
        }

        if (moveOptions != null && moveOptions.targetBlock)
        {
            //Move onto a moving block
            if (moveOptions.targetBlock.GetComponent<Block_Elevator>())
            {
                yield return ElevatorMovement(moveState, movementSpeed, moveOptions);
            }

            //Move onto a block
            else
            {
                yield return NormalMovement(endPos, moveState, movementSpeed);
            }
        }

        //Move to a position, not a block
        else
        {
            yield return NormalMovement(endPos, moveState, movementSpeed);
        }

        isMoving = false;
        isJumping = false;
        isGrapplingHooking = false;
        isDashing = false;
        isIceGliding = false;

        isAscending = false;
        isDecending = false;
        PlayerCameraOcclusionController.Instance.CameraZoom(false);

        //StartCoroutine(DelayAscendDescendCamera(0.2f));

        Action_StepTaken_Invoke();
    }

    IEnumerator NormalMovement(Vector3 endPos, MovementStates moveState, float movementSpeed)
    {
        Vector3 rayDir = Vector3.zero;
        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
            rayDir = Vector3.down;
        else
            rayDir = Vector3.up;

        float counter = 0;
        previousPosition = transform.position;

        Vector3 startPos = transform.position;

        Vector3 newEndPos = endPos + (rayDir * heightOverBlock);
        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
            newEndPos = endPos + (rayDir * (heightOverBlock - (Player_BodyHeight.Instance.height_Normal) / 2)); //Change HeightOverBlock sligtly when ceilinggrab (it moves some up before snapping in place

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

        UpdateLookDir();

        movementStates = MovementStates.Still;
        performGrapplingHooking = false;

        isAscending = false;
        isDecending = false;
        PlayerCameraOcclusionController.Instance.CameraZoom(false);
    }
    IEnumerator ElevatorMovement(MovementStates moveState, float movementSpeed, MoveOptions moveOptions)
    {
        Vector3 rayDir = Vector3.zero;
        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
            rayDir = Vector3.down;
        else
            rayDir = Vector3.up;

        float counter = 0f;
        previousPosition = transform.position;

        Transform targetBlockTransform = moveOptions.targetBlock.transform;
        Vector3 startPos = transform.position;
        Vector3 targetOffset = new Vector3(0f, (rayDir * (heightOverBlock - (Player_BodyHeight.Instance.height_Normal) / 2)).y, 0f);
        Vector3 endPos = targetBlockTransform.position + targetOffset;

        movementStates = moveState;

        float elapsed = 0f;
        float distance = Vector3.Distance(startPos, endPos);

        float currentSpeed = baseTime / movementSpeed;
        float speedFactor = 1f / Mathf.Max(currentSpeed, 0.01f);
        float duration = distance / speedFactor;

        while (elapsed < duration)
        {
            counter += Time.deltaTime;
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            Vector3 targetPosition = Vector3.Lerp(startPos, endPos, t);
            transform.position = targetPosition;

            yield return null;
        }

        // Snap exactly to final position atop the elevator
        transform.position = targetBlockTransform.position + targetOffset;

        // Store reference to elevator and relative offset for syncing elsewhere
        elevatorBeingFollowed = targetBlockTransform;
        elevatorOffset = targetOffset;

        UpdateLookDir();

        movementStates = MovementStates.Still;
        performGrapplingHooking = false;

        isAscending = false;
        isDecending = false;
        PlayerCameraOcclusionController.Instance.CameraZoom(false);
    }

    IEnumerator DelayAscendDescendCamera(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        isAscending = false;
        isDecending = false;
        PlayerCameraOcclusionController.Instance.CameraZoom(false);
    }

    void MovingAnimation(MoveOptions canMoveBlock)
    {
        //Perform walking animation when entering a Stair or Slope
        if (blockStandingOn && blockStandingOn.GetComponent<BlockInfo>() && blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Slope
            && canMoveBlock.targetBlock.GetComponent<BlockInfo>().blockType == BlockType.Slope)
        {
            
        }
        else if (blockStandingOn && blockStandingOn.GetComponent<BlockInfo>() && blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Stair)
        {
            Player_Animations.Instance.Perform_StairSlopeWalkingAnimation();
        }
        else if (canMoveBlock.targetBlock.GetComponent<BlockInfo>().blockType == BlockType.Stair || canMoveBlock.targetBlock.GetComponent<BlockInfo>().blockType == BlockType.Slope)
        {
            Player_Animations.Instance.Perform_StairSlopeWalkingAnimation();
        }
        
        //Perform animation only the first time when pressing down a movmentButton
        else if (canMoveBlock.targetBlock != blockStandingOn && !walkAnimationCheck && !isIceGliding
            && blockStandingOn && blockStandingOn.GetComponent<BlockInfo>() && blockStandingOn.GetComponent<BlockInfo>().blockType != BlockType.Slope)
        {
            //if (Player_KeyInputs.Instance.forward_isPressed || Player_KeyInputs.Instance.back_isPressed || Player_KeyInputs.Instance.left_isPressed || Player_KeyInputs.Instance.right_isPressed)
            //{ return; }

            Player_Animations.Instance.Perform_StairSlopeWalkingAnimation();
            walkAnimationCheck = true;
        }
    }

    void WalkButtonIsReleased()
    {
        walkAnimationCheck = false;
    }

    #endregion


    //--------------------


    #region Falling

    public void StartFallingWithBlock()
    {
        if (blockStandingOn.GetComponent<BlockInfo>().movementState == MovementStates.Falling && !Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            SetMovementState(MovementStates.Falling);

            ResetDarkenBlocks();
        }
    }
    void StartFallingWithNoBlock()
    {
        if (!Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            SetMovementState(MovementStates.Falling);

            ResetDarkenBlocks();
        }
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
        if (blockStandingOn && blockStandingOn.GetComponent<BlockInfo>())
        {
            if (blockStandingOn.GetComponent<BlockInfo>().movementState != MovementStates.Falling)
            {
                SetMovementState(MovementStates.Still);
                Action_LandedFromFalling_Invoke();

                //StartCoroutine(LateBlockDetection());
            }
        }
    }
    IEnumerator LateBlockDetection()
    {
        yield return new WaitForSeconds(0.02f);

        UpdateAvailableMovementBlocks();

        print("1. LateBlockDetection");
    }

    #endregion

    #region IceGliding

    void RunIceGliding()
    {
        IceGlideMovement(false);
    }
    public void IceGlideMovement(bool canIceGlide)
    {
        if (!blockStandingOn) return;
        if (!blockStandingOn.GetComponent<BlockInfo>()) return;

        if (blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Ice
            && ((blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Stair || blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Slope) || (blockStandingOn.GetComponent<EffectBlockInfo>() && !blockStandingOn.GetComponent<EffectBlockInfo>().effectBlock_Teleporter_isAdded) || canIceGlide))
        {
            MoveOptions moveOption = new MoveOptions();

            isIceGliding = true;

            Vector3 movementDir = Vector3.zero;
            if (!canIceGlide)
            {
                Vector3 movementDelta = transform.position - previousPosition;
                Vector3 horizontalDirection = new Vector3(movementDelta.x, 0, movementDelta.z);

                movementDir = GetMovingDirection(horizontalDirection);
            }
            else
            {
                movementDir = teleportMovementDir;
            }

            if (movementDir == Vector3.forward && moveToBlock_Forward.canMoveTo && moveToBlock_Forward.targetBlock != blockStandingOn)
                moveOption = moveToBlock_Forward;
            else if (movementDir == Vector3.back && moveToBlock_Back.canMoveTo && moveToBlock_Forward.targetBlock != blockStandingOn)
                moveOption = moveToBlock_Back;
            else if (movementDir == Vector3.left && moveToBlock_Left.canMoveTo && moveToBlock_Forward.targetBlock != blockStandingOn)
                moveOption = moveToBlock_Left;
            else if (movementDir == Vector3.right && moveToBlock_Right.canMoveTo && moveToBlock_Forward.targetBlock != blockStandingOn)
                moveOption = moveToBlock_Right;
            else
                return;

            PerformMovement(moveOption, MovementStates.Moving, blockStandingOn.GetComponent<BlockInfo>().movementSpeed);

            //Update previous position for next frame
            previousPosition = transform.position;
        }
    }
    public Vector3 GetMovingDirection(Vector3 direction)
    {
        direction.y = 0;
        direction.Normalize();

        float forwardDot = Vector3.Dot(direction, UpdatedDir(Vector3.forward));
        float backDot = Vector3.Dot(direction, UpdatedDir(Vector3.back));
        float leftDot = Vector3.Dot(direction, UpdatedDir(Vector3.left));
        float rightDot = Vector3.Dot(direction, UpdatedDir(Vector3.right));

        float maxDot = Mathf.Max(forwardDot, backDot, leftDot, rightDot);

        if (maxDot == forwardDot) return Vector3.forward;
        if (maxDot == backDot) return Vector3.back;
        if (maxDot == leftDot) return Vector3.left;
        if (maxDot == rightDot) return Vector3.right;

        return Vector3.zero; // fallback
    }

    #endregion

    #region Ladder

    void FindLadderExitBlock()
    {
        CheckAvailableLadderExitBlocks(UpdatedDir(Vector3.forward), moveToLadder_Forward);
        CheckAvailableLadderExitBlocks(UpdatedDir(Vector3.back), moveToLadder_Back);
        CheckAvailableLadderExitBlocks(UpdatedDir(Vector3.left), moveToLadder_Left);
        CheckAvailableLadderExitBlocks(UpdatedDir(Vector3.right), moveToLadder_Right);
    }
    void CheckAvailableLadderExitBlocks(Vector3 dir, MoveOptions moveOptions)
    {
        GameObject outObj1 = null;

        if (PerformMovementRaycast(transform.position, dir, 1, out outObj1) == RaycastHitObjects.LadderBlocker
            || PerformMovementRaycast(transform.position, dir, 1, out outObj1) == RaycastHitObjects.Fence)
        {
            Block_IsNot_Target(moveOptions);
            return;
        }

        //Check from the bottom and up
        if (PerformMovementRaycast(transform.position, dir, 1, out outObj1) == RaycastHitObjects.Ladder)
        {
            Block_Is_Target(moveOptions, outObj1.GetComponent<Block_Ladder>().exitBlock_Up);
            outObj1.GetComponent<Block_Ladder>().DarkenExitBlock_Up(dir);

            return;
        }

        //Check from the top and down
        if (PerformMovementRaycast(transform.position + (dir * 0.65f), Vector3.down, 1, out outObj1) == RaycastHitObjects.Ladder)
        {
            Block_Is_Target(moveOptions, outObj1.GetComponent<Block_Ladder>().exitBlock_Down);
            outObj1.GetComponent<Block_Ladder>().DarkenExitBlock_Down();

            return;
        }

        Block_IsNot_Target(moveOptions);
    }

    bool CheckLaddersToEnter_Up(Vector3 dir)
    {
        GameObject outObj1 = null;

        //Check from the bottom and up
        if (PerformMovementRaycast(transform.position, dir, 1, out outObj1) == RaycastHitObjects.Ladder)
        {
            return true;
        }

        //If no ladder is found
        return false;
    }
    bool CheckLaddersToEnter_Down(Vector3 dir)
    {
        GameObject outObj1 = null;

        //Check from the top and down
        if (PerformMovementRaycast(transform.position + (dir * 0.65f), Vector3.down, 1, out outObj1) == RaycastHitObjects.Ladder)
        {
            return true;
        }

        //If no ladder is found
        return false;
    }

    GameObject GetLadderExitPart_Up(Vector3 dir /*GameObject ladderObj*/)
    {
        //return ladderObj.GetComponent<Block_Ladder>().lastLadderPart_Up;

        GameObject outObj1 = null;

        //Check from the bottom and up
        if (PerformMovementRaycast(transform.position, dir, 1, out outObj1) == RaycastHitObjects.Ladder)
        {
            ladderToEnterRot = outObj1.transform.rotation;
            return outObj1.GetComponent<Block_Ladder>().lastLadderPart_Up;
        }

        return null;
    }
    GameObject GetLadderExitPart_Down(Vector3 dir /*GameObject ladderObj*/)
    {
        //return ladderObj.GetComponent<Block_Ladder>().exitBlock_Down;

        GameObject outObj1 = null;

        //Check from the top and down
        if (PerformMovementRaycast(transform.position + (dir * 0.65f), Vector3.down, 1, out outObj1) == RaycastHitObjects.Ladder)
        {
            ladderToEnterRot = outObj1.transform.rotation;
            return outObj1.GetComponent<Block_Ladder>().lastLadderPart_Down;
        }

        return null;
    }

    IEnumerator PerformLadderMovement_Up(Vector3 dir, GameObject targetPosObj)
    {
        #region Setup Movement Parameters

        ResetDarkenBlocks();

        GameObject outObj1 = null;

        isMovingOnLadder_Up = true;
        ladderClimbPos_Start = transform.position;

        movementStates = MovementStates.Moving;
        PlayerManager.Instance.pauseGame = true;
        //PlayerManager.Instance.isTransportingPlayer = true;

        Vector3 startPosition;
        Vector3 endPosition;
        float ladderClimbDuration = 0;
        float elapsedTime = 0;

        #endregion

        //RotatePlayerBody(dir.y);

        #region Move To Top LadderPart

        startPosition = transform.position;
        endPosition = targetPosObj.transform.position + (Vector3.up * heightOverBlock);

        ladderClimbDuration = Vector3.Distance(startPosition, endPosition) * 0.4f;
        elapsedTime = 0f;

        //Move to the top ladder
        while (elapsedTime < ladderClimbDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the progress of the ladderMovement
            float progress = elapsedTime / ladderClimbDuration;

            // Interpolate the up/down position
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, progress);

            // Update the player's position
            transform.position = currentPosition;

            yield return null;
        }

        // Ensure the player lands exactly at the end position
        transform.position = endPosition;

        #endregion

        #region Move To ExitBlock

        endPosition = startPosition + dir;
        if (PerformMovementRaycast(transform.position + dir, Vector3.down, 1, out outObj1) == RaycastHitObjects.BlockInfo)
        {
            endPosition = outObj1.transform.position + (Vector3.up * heightOverBlock);
        }

        startPosition = transform.position;

        ladderClimbDuration = 0.4f;
        elapsedTime = 0f;

        //Move to the top ladder
        while (elapsedTime < ladderClimbDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the progress of the ladderMovement
            float progress = elapsedTime / ladderClimbDuration;

            // Interpolate the up/down position
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, progress);

            // Update the player's position
            transform.position = currentPosition;

            yield return null;
        }

        // Ensure the player lands exactly at the end position
        transform.position = endPosition;

        #endregion


        #region Setup StopMovement Parameters

        if (moveToLadder_Forward.targetBlock)
            moveToLadder_Forward.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
        if (moveToLadder_Back.targetBlock)
            moveToLadder_Back.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
        if (moveToLadder_Left.targetBlock)
            moveToLadder_Left.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
        if (moveToLadder_Right.targetBlock)
            moveToLadder_Right.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();

        Block_IsNot_Target(moveToLadder_Forward);
        Block_IsNot_Target(moveToLadder_Back);
        Block_IsNot_Target(moveToLadder_Left);
        Block_IsNot_Target(moveToLadder_Right);

        UpdateAvailableMovementBlocks();

        isMovingOnLadder_Up = false;

        movementStates = MovementStates.Still;
        PlayerManager.Instance.pauseGame = false;

        FindLadderExitBlock();
        Action_StepTaken_Invoke();

        #endregion
    }
    IEnumerator PerformLadderMovement_Down(Vector3 dir, GameObject targetPosObj)
    {
        #region Setup Movement Parameters

        ResetDarkenBlocks();

        isMovingOnLadder_Down = true;
        ladderClimbPos_Start = transform.position;

        movementStates = MovementStates.Moving;
        PlayerManager.Instance.pauseGame = true;
        //PlayerManager.Instance.isTransportingPlayer = true;

        Vector3 startPosition;
        Vector3 endPosition;
        float ladderClimbDuration = 0;
        float elapsedTime = 0;

        #endregion
        float targetY = 0;
        targetY = targetPosObj.transform.eulerAngles.y;
        PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(PlayerManager.Instance.playerBody.transform.localPosition, Quaternion.Euler(0, targetY, 0));

        #region Move From ExitBlock

        startPosition = transform.position;
        endPosition = startPosition + dir;

        ladderClimbDuration = 0.4f;
        elapsedTime = 0f;

        //Move to the top ladder
        while (elapsedTime < ladderClimbDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the progress of the ladderMovement
            float progress = elapsedTime / ladderClimbDuration;

            // Interpolate the up/down position
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, progress);

            // Update the player's position
            transform.position = currentPosition;

            yield return null;
        }

        // Ensure the player lands exactly at the end position
        transform.position = endPosition;

        #endregion

        //RotatePlayerBody(-targetPosObj.transform.eulerAngles.y);

        #region Move To Bottom LadderPart

        startPosition = transform.position;
        endPosition = targetPosObj.transform.position/* + (Vector3.up * heightOverBlock)*/;

        ladderClimbDuration = Vector3.Distance(startPosition, endPosition) * 0.4f;
        elapsedTime = 0f;

        //Move to the bottom ladder
        while (elapsedTime < ladderClimbDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the progress of the ladderMovement
            float progress = elapsedTime / ladderClimbDuration;

            // Interpolate the up/down position
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, progress);

            // Update the player's position
            transform.position = currentPosition;

            yield return null;
        }

        // Ensure the player lands exactly at the end position
        transform.position = endPosition;

        #endregion

        //RotatePlayerBody(0);

        #region Setup StopMovement Parameters

        if (moveToLadder_Forward.targetBlock)
            moveToLadder_Forward.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
        if (moveToLadder_Back.targetBlock)
            moveToLadder_Back.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
        if (moveToLadder_Left.targetBlock)
            moveToLadder_Left.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
        if (moveToLadder_Right.targetBlock)
            moveToLadder_Right.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();

        Block_IsNot_Target(moveToLadder_Forward);
        Block_IsNot_Target(moveToLadder_Back);
        Block_IsNot_Target(moveToLadder_Left);
        Block_IsNot_Target(moveToLadder_Right);

        UpdateAvailableMovementBlocks();

        targetY = targetPosObj.transform.eulerAngles.y + 180;
        PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(PlayerManager.Instance.playerBody.transform.localPosition, Quaternion.Euler(0, targetY, 0));

        isMovingOnLadder_Down = false;

        movementStates = MovementStates.Still;
        PlayerManager.Instance.pauseGame = false;

        FindLadderExitBlock();
        Action_StepTaken_Invoke();

        #endregion
    }

    #endregion


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

        float rotZ = 0;

        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            print("2. Get isCeilingGrabbing");
            rotZ = -180;
        }
       
        float baseRotation = GetBaseCameraRotation(CameraController.Instance.cameraRotationState);
        float finalYRotation = NormalizeAngle(baseRotation + rotationValue);
        Quaternion newRotation = Quaternion.Euler(0, finalYRotation, rotZ);
        playerBody.SetPositionAndRotation(playerBody.position, newRotation);

        CameraController.Instance.directionFacing = GetFacingDirection(finalYRotation);

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

    public void UpdateLookDir()
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

        lookingDirection = lookDir;
        Player_Pusher.Instance.DisplayPushDirection(lookingDirection, lookingDirectionDescription);
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
            if (blockStandingOn.GetComponent<BlockInfo>() /*&& !PlayerManager.Instance.isTransportingPlayer*/)
            {
                //Don't remove steps if gliding from a slope
                if (hasSlopeGlided && blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                {
                    //print("1. Slope");
                    isSlopeGliding = true;
                }
                if (hasSlopeGlided && blockStandingOn.GetComponent<BlockInfo>().blockType != BlockType.Slope)
                {
                    //print("2. Slope");
                    hasSlopeGlided = false;
                }
                //else if (hasSlopeGlided && blockStandingOn.GetComponent<BlockInfo>().blockType != BlockType.Slope && !Player_Pusher.Instance.playerIsPushed)
                //{
                //    print("3. Slope");
                //    hasSlopeGlided = false;

                //    //PlayerStats.Instance.stats.steps_Current -= blockStandingOn.GetComponent<BlockInfo>().movementCost;
                //}
                else if (Player_Pusher.Instance.playerIsPushed)
                {
                    //print("4. Slope");
                    hasSlopeGlided = false;
                }
                else if (!hasSlopeGlided && blockStandingOn.GetComponent<BlockInfo>().blockType != BlockType.Slope)
                {
                    if (isSlopeGliding)
                    {
                        //print("5. Slope");
                    }
                    else
                    {
                        //print("6. Slope");
                        PlayerStats.Instance.stats.steps_Current -= blockStandingOn.GetComponent<BlockInfo>().movementCost;
                    }

                    isSlopeGliding = false;
                }
                else
                {
                    //print("7. Slope");
                    //PlayerStats.Instance.stats.steps_Current -= blockStandingOn.GetComponent<BlockInfo>().movementCost;
                    hasSlopeGlided = false;
                }
            }
        }

        //If steps is < 0
        if (PlayerStats.Instance.stats.steps_Current < 0 && !Player_Pusher.Instance.playerIsPushed)
        {
            PlayerStats.Instance.stats.steps_Current = 0;
            RespawnPlayer();
        }

        Action_StepTaken_Late_Invoke();
    }
    void CancelSlopeIfFalling()
    {
        if (movementStates == MovementStates.Falling && (isSlopeGliding || hasSlopeGlided))
        {
            //print("0. Slope");
            isSlopeGliding = false;
            hasSlopeGlided = false;
        }
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
        Player_KeyInputs.Instance.grapplingHook_isPressed = false;

        Player_KeyInputs.Instance.cameraX_isPressed = false;
        Player_KeyInputs.Instance.cameraY_isPressed = false;

        isAscending = false;
        isDecending = false;
        PlayerCameraOcclusionController.Instance.CameraZoom(false);

        SetMovementState(MovementStates.Moving);

        RespawnPlayerEarly_Action();

        yield return new WaitForSeconds(waitTime);

        //Move player
        transform.position = MapManager.Instance.playerStartPos;
        PlayerManager.Instance.playerBody.transform.SetPositionAndRotation(MapManager.Instance.playerStartPos /*new Vector3(MapManager.Instance.playerStartPos.x + MapManager.Instance.playerStartPos.y + Player_BodyHeight.Instance.height_Normal, MapManager.Instance.playerStartPos.z)*/, GetRespawnPlayerDirection(0, 180, 0));
        PlayerManager.Instance.playerBody.transform.SetLocalPositionAndRotation(new Vector3(PlayerManager.Instance.playerBody.transform.localPosition.x, Player_BodyHeight.Instance.height_Normal, PlayerManager.Instance.playerBody.transform.localPosition.z), PlayerManager.Instance.playerBody.transform.localRotation);
        //PlayerManager.Instance.playerBody.transform.position = new Vector3(PlayerManager.Instance.playerBody.transform.position.x, -Player_BodyHeight.Instance.height_Normal, PlayerManager.Instance.playerBody.transform.position.z);

        //Reset for CeilingAbility
        Player_CeilingGrab.Instance.ResetCeilingGrab();

        //Player_DarkenBlock.Instance.block_hasBeenDarkened = false;

        yield return new WaitForSeconds(waitTime);

        //Refill Steps to max + stepPickups gotten
        PlayerStats.Instance.RefillStepsToMax();

        //CameraController.Instance.ResetCameraRotation();
        CameraController.Instance.SetRespawnCameraRotation();

        //RotatePlayerBody(GetRespawnPlayerDirection(0, 180, 0).y /*180*/);

        RespawnPlayer_Action();

        yield return new WaitForSeconds(waitTime * 30);
        previousPosition = transform.position;

        SetMovementState(MovementStates.Still);
        RespawnPlayerLate_Action();

        StopAllCoroutines();
    }
    public Quaternion GetRespawnPlayerDirection(int corr_X, int corr_Y, int corr_Z)
    {
        switch (MapManager.Instance.playerStartRot)
        {
            case MovementDirection.None:
                return Quaternion.Euler(0 + corr_X, 0 + corr_Y, 0 + corr_Z);

            case MovementDirection.Forward:
                return Quaternion.Euler(0 + corr_X, 0 + corr_Y, 0 + corr_Z);
            case MovementDirection.Backward:
                return Quaternion.Euler(0 + corr_X, 180 + corr_Y, 0 + corr_Z);
            case MovementDirection.Left:
                return Quaternion.Euler(0 + corr_X, -90 + corr_Y, 0 + corr_Z);
            case MovementDirection.Right:
                return Quaternion.Euler(0 + corr_X, 90 + corr_Y, 0 + corr_Z);

            default:
                return Quaternion.Euler(0 + corr_X, 0 + corr_Y, 0 + corr_Z);
        }
    }

    void CheckIfSwimming()
    {
        if (blockStandingOn && blockStandingOn.GetComponent<BlockInfo>() && blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Water && PlayerHasSwimAbility())
        {
            MapManager.Instance.swiftSwimCounter++;
        }
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


    void Temp_EssencePickupGot_Animation()
    {
        PlayerManager.Instance.PauseGame();
        StartCoroutine(JumpSpin(PlayerManager.Instance.playerBody.transform, 0.3f, 0.5f, 1, -Vector3.right));
    }
    void Temp_StepsUpPickupGot_Animation()
    {
        PlayerManager.Instance.PauseGame();
        StartCoroutine(JumpSpin(PlayerManager.Instance.playerBody.transform, 0.45f, 0.5f, 1, Vector3.up));
    }
    void Temp_SkinPickupGot_Animation()
    {
        PlayerManager.Instance.PauseGame();
        StartCoroutine(JumpSpin(PlayerManager.Instance.playerBody.transform, 0.45f, 0.5f, 2, Vector3.up));
    }
    void Temp_AbilityPickupGot_Animation()
    {
        PlayerManager.Instance.PauseGame();
        StartCoroutine(JumpSpin(PlayerManager.Instance.playerBody.transform, 0.45f, 0.5f, 1, -Vector3.right));
    }


    public IEnumerator JumpSpin(Transform target, float totalTime, float jumpHeight, int spinCount, Vector3 rotationAxis)
    {
        if (target == null) yield break;
        if (totalTime <= 0f) totalTime = 0.0001f; // avoid division by zero

        if (movementStates != MovementStates.Falling)
        {
            movementStates = MovementStates.Moving;
        }
        
        if (blockStandingOn && blockStandingOn.GetComponent<BlockInfo>().movementSpeed >= 5)
        {
            print("1000. Animation Speed >= 5");

            yield return new WaitForSeconds(0.2f);
        }
        else
        {
            print("2000. Animation Speed < 5");

            yield return new WaitForSeconds(0.45f);
        }

        Vector3 startPos = target.position;
        Quaternion startRot = target.rotation;

        float elapsed = 0f;
        float totalRotation = 360f * spinCount; // positive = clockwise around local X

        while (elapsed < totalTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / totalTime);

            // Parabolic jump: 0 -> jumpHeight -> 0 over [0,1]
            float yOffset = 4f * jumpHeight * t * (1f - t);
            target.position = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);

            // Spin ONLY around local X-axis (no Y rotation introduced)
            float angle = Mathf.Lerp(0f, totalRotation, t);
            target.rotation = startRot * Quaternion.AngleAxis(angle, rotationAxis);

            yield return null;
        }

        // Snap to exact end state
        target.position = startPos;
        target.rotation = startRot;

        PlayerManager.Instance.UnpauseGame();

        if (movementStates != MovementStates.Falling)
        {
            movementStates = MovementStates.Still;
        }

        Action_PickupAnimation_Complete?.Invoke();

        Movement.Instance.UpdateLookDir();
    }


    #region Actions

    public void Action_StepTaken_Early_Invoke()
    {
        Action_StepTaken_Early?.Invoke();
    }
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

    #endregion
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

    Ladder,
    LadderBlocker,

    Fence,
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