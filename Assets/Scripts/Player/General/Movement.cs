using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Singleton<Movement>
{
    #region Actions

    public static event Action Action_RespawnToSavePos;
    public static event Action Action_RespawnPlayerEarly;
    public static event Action Action_RespawnPlayer;
    public static event Action Action_RespawnPlayerLate;
    public static event Action Action_RespawnPlayerByHolding;

    public static event Action Action_UpdatedBlocks;

    public static event Action Action_StepTaken_Early;
    public static event Action Action_StepTaken;
    public static event Action Action_StepTaken_Late;
    public static event Action Action_BodyRotated;
    public static event Action Action_isSwitchingBlocks;
    public static event Action Action_LandedFromFalling;

    public static event Action Action_PickupAnimation_Complete;

    public static event Action Action_isSwiftSwim;
    public static event Action Action_isSwiftSwim_Finished;

    public static event Action Action_isDashing;
    public static event Action Action_isDashing_Finished;
    public static event Action Action_isJumping;
    public static event Action Action_isJumping_Finished;
    public static event Action Action_isAscending;
    public static event Action Action_isAscending_Finished;
    public static event Action Action_isDescending;
    public static event Action Action_isDescending_Finished;
    public static event Action Action_isGrapplingHooking;
    public static event Action Action_isGrapplingHooking_Finished;

    #endregion

    #region Variables

    [Header("States")]
    public bool isMoving;
    public MovementStates movementStates = MovementStates.Still;

    [Header("Stats")]
    public float heightOverBlock = 0.95f;
    float baseTime = 1f;
    public float fallSpeed = 10f;
    float abilitySpeed = 8f;
    public float ascendDescend_Distance = 2f;
    float grapplingLength = 5f;
    public Vector3 savePos;

    [Header("BlockIsStandingOn")]
    public Vector3 lookingDirection;
    public Vector3 lookingDirection_Old;
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
    private Vector3 lastFollowedElevatorPosition;
    [SerializeField] private float elevatorRefreshDistance = 0.05f;
    private float elevatorRefreshAccumulatedDistance = 0f;

    RaycastHit hit;

    public bool isJumping;
    public bool isGrapplingHooking;
    public bool isDashing;
    public bool isIceGliding;
    public bool isSwiftSwim;
    public bool isAscending;
    public bool isDescending;

    [Header("Animations")]
    public Animator anim;
    [SerializeField] bool blink;
    [SerializeField] bool secondaryIdle;
    [SerializeField] bool walkAnimationCheck;

    [Header("Temp Movement Cost for Slope Gliding")]
    [SerializeField] bool hasSlopeGlided;
    [SerializeField] bool isSlopeGliding;
    [SerializeField] private bool slopeLandingIsFree;

    [Header("SwiftSwim")]
    [SerializeField] GameObject swiftSwimObject_StandingOn;
    [SerializeField] GameObject swiftSwimObject_Up;
    [SerializeField] GameObject swiftSwimObject_Down;
    [SerializeField] LayerMask swiftSwimLayersToIgnore;

    public bool isRespawning;

    private readonly HashSet<GameObject> currentlyDarkenedBlocks = new HashSet<GameObject>();

    [SerializeField] private float turnBeforeMoveDelay = 0.12f;
    private float lastTurnTime = -999f;
    private bool turnedThisFrame = false;
    private Vector3 lastTurnedToDir = Vector3.zero;

    private static readonly Vector3[] LocalDirections =
    {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
    };

    [SerializeField] private Vector3 lastIceGlideDirection = Vector3.zero;

    [SerializeField] private bool slopeAutoExitInProgress;
    [SerializeField] private GameObject slopeAutoExitSourceBlock;
    [SerializeField] private Vector3 slopeAutoExitTargetPos;

    [SerializeField] private bool suppressDarkeningWhileChaining;
    [SerializeField] private bool pendingDarkeningRefreshAfterChain;

    [Header("Drowning")]
    [SerializeField] bool isDrowning;

    [Header("Slope X")]
    [SerializeField] private bool isSlopeFalling;
    private readonly Dictionary<GameObject, int> slopeDisplayTempOverrides = new Dictionary<GameObject, int>();
    [SerializeField] private bool pendingSlopeFallAfterUphillAttempt;
    [SerializeField] private bool isPlayingSlopeFallAnimation;

    [Header("Falling With Block")]
    [SerializeField] private bool isFallingWithCarrierBlock;
    [SerializeField] private GameObject fallingCarrierBlock;

    #endregion

    #region Cached Accessors

    private PlayerManager PM => PlayerManager.Instance;
    private Player_KeyInputs Inputs => Player_KeyInputs.Instance;
    private CameraController Cam => CameraController.Instance;
    private Player_CeilingGrab CeilingGrab => Player_CeilingGrab.Instance;
    private PlayerStats StatsRoot => PlayerStats.Instance;
    private MapManager Map => MapManager.Instance;
    private Player_Animations Anims => Player_Animations.Instance;

    #endregion

    #region Unity

    private void Start()
    {
        if (PM.playerBody != null && PM.playerBody.TryGetComponent(out Animator playerAnim))
            anim = playerAnim;

        savePos = transform.position;
        previousPosition = transform.position;
        elevatorPos_Previous = transform.position;

        lastFollowedElevatorPosition = transform.position;
        elevatorRefreshAccumulatedDistance = 0f;

        RespawnPlayer();
    }

    private void Update()
    {
        if (Tutorial.Instance.tutorial_isRunning && Inputs.tutorialMovementBlocker)
            return;

        //If standing in water and cannot swim, force Dorwning and respawn player
        if (!isDrowning && blockStandingOn && blockStandingOn.GetComponent<BlockInfo>() && blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Water && !PlayerHasSwimAbility())
        {
            StartCoroutine(StartDrowning());
        }

        switch (GetMovementState())
        {
            case MovementStates.Moving:
                UpdateBlockStandingOn();
                break;

            case MovementStates.Falling:
                UpdateBlockStandingOn();
                PlayerIsFalling();

                if (IsFallingWithCarrierBlockActive())
                {
                    UpdateBlocks();
                    MovementSetup_FallingWithCarrierBlock();
                }
                break;

            default:
                if (!Tutorial.Instance.tutorial_isRunning && PM.pauseGame)
                    return;

                MovementSetup();
                break;
        }

        if (!Block_Moveable.AnyBlockMoving &&
            Inputs.grapplingHook_isPressed &&
            !grapplingTargetHasBeenSet &&
            !CeilingGrab.isCeilingGrabbing)
        {
            UpdateGrapplingHookMovement(moveToBlock_GrapplingHook, lookDir);
            grapplingTargetHasBeenSet = true;
        }

        CancelSlopeIfFalling();

        if (elevatorBeingFollowed != null && blockStandingOn != null && blockStandingOn.transform == elevatorBeingFollowed)
        {
            transform.position = elevatorBeingFollowed.position + elevatorOffset;
            RefreshBlocksWhileStandingOnMovingElevator();
        }
        else
        {
            elevatorRefreshAccumulatedDistance = 0f;

            if (elevatorBeingFollowed != null)
                lastFollowedElevatorPosition = elevatorBeingFollowed.position;
        }
    }

    private void OnEnable()
    {
        Action_StepTaken += UpdateAvailableMovementBlocks;
        Action_RespawnPlayerLate += UpdateAvailableMovementBlocks;
        Action_LandedFromFalling += UpdateAvailableMovementBlocks;

        Action_RespawnPlayerEarly += RespawnUnderGrappling;

        Action_BodyRotated += UpdateLookDir;
        Action_BodyRotated += RefreshAvailableMovementBlocksSmooth;

        Action_RespawnPlayerEarly += ResetDarkenBlocks;
        Action_StepTaken += TakeAStep;

        Action_isSwitchingBlocks += UpdateStepsAmonutWhenGrapplingMoving;
        Action_StepTaken_Late += RunIceGliding;
        Action_StepTaken_Late += CheckIfSwimming;

        CameraController.Action_RotateCamera_End += UpdateBlocks;

        Player_KeyInputs.Action_WalkButton_isReleased += WalkButtonIsReleased;

        SFX_Respawn.Action_RespawnPlayer += RespawnPlayer;

        Interactable_Pickup.Action_AbilityPickupGot += UpdateLookDir;
    }

    private void OnDisable()
    {
        Action_StepTaken -= UpdateAvailableMovementBlocks;
        Action_RespawnPlayerLate -= UpdateAvailableMovementBlocks;
        Action_LandedFromFalling -= UpdateAvailableMovementBlocks;

        Action_RespawnPlayerEarly -= RespawnUnderGrappling;

        Action_BodyRotated -= UpdateLookDir;
        Action_BodyRotated -= RefreshAvailableMovementBlocksSmooth;

        Action_RespawnPlayerEarly -= ResetDarkenBlocks;
        Action_StepTaken -= TakeAStep;

        Action_isSwitchingBlocks -= UpdateStepsAmonutWhenGrapplingMoving;
        Action_StepTaken_Late -= RunIceGliding;
        Action_StepTaken_Late -= CheckIfSwimming;

        CameraController.Action_RotateCamera_End -= UpdateBlocks;

        Player_KeyInputs.Action_WalkButton_isReleased -= WalkButtonIsReleased;

        SFX_Respawn.Action_RespawnPlayer -= RespawnPlayer;

        Interactable_Pickup.Action_AbilityPickupGot -= UpdateLookDir;
    }

    #endregion

    #region Helpers

    private bool TryGetBlockInfo(GameObject obj, out BlockInfo info)
    {
        info = null;
        return obj != null && obj.TryGetComponent(out info);
    }

    private bool TryGetStandingInfo(out BlockInfo info)
    {
        return TryGetBlockInfo(blockStandingOn, out info);
    }

    private Vector3 GroundCheckDir()
    {
        return CeilingGrab.isCeilingGrabbing ? Vector3.up : Vector3.down;
    }

    private Vector3 StandingOffsetDir()
    {
        return CeilingGrab.isCeilingGrabbing ? Vector3.down : Vector3.up;
    }

    private bool IsWater(GameObject obj)
    {
        return TryGetBlockInfo(obj, out BlockInfo info) && info.blockElement == BlockElement.Water;
    }

    private bool IsLava(GameObject obj)
    {
        return TryGetBlockInfo(obj, out BlockInfo info) && info.blockElement == BlockElement.Lava;
    }

    private bool IsIce(GameObject obj)
    {
        return TryGetBlockInfo(obj, out BlockInfo info) && info.blockElement == BlockElement.Ice;
    }

    private bool IsStairOrSlope(GameObject obj)
    {
        return TryGetBlockInfo(obj, out BlockInfo info) &&
               (info.blockType == BlockType.Stair || info.blockType == BlockType.Slope);
    }

    private bool CanAfford(GameObject obj)
    {
        if (StatsRoot.stats == null)
            return false;

        if (obj && obj.GetComponent<BlockInfo>() && obj.GetComponent<BlockInfo>().blockElement == BlockElement.Water && !PlayerHasSwimAbility())
        {
            return true;
        }
        else
        {
            return StatsRoot.stats.steps_Current >= GetRequiredCost(obj);
        }
    }

    private float MovementDuration(Vector3 startPos, Vector3 endPos, float movementSpeed)
    {
        float distance = Vector3.Distance(startPos, endPos);
        float currentSpeed = baseTime / Mathf.Max(movementSpeed, 0.01f);
        float speedFactor = 1f / Mathf.Max(currentSpeed, 0.01f);
        return distance / speedFactor;
    }

    private bool HasValidTarget(MoveOptions option)
    {
        return option != null && option.canMoveTo && option.targetBlock != null;
    }

    private void SetMoveTarget(MoveOptions moveOption, GameObject obj)
    {
        if (moveOption == null) return;
        moveOption.canMoveTo = true;
        moveOption.targetBlock = obj;
    }

    private void ClearMoveTarget(MoveOptions moveOption)
    {
        if (moveOption == null) return;
        moveOption.canMoveTo = false;
        moveOption.targetBlock = null;
    }

    private void ResetMoveVisual(MoveOptions moveOption)
    {
        if (moveOption != null && moveOption.targetBlock != null)
            ResetAvailableBlock(moveOption.targetBlock);
    }

    private void SetMoveVisual(MoveOptions moveOption)
    {
        if (moveOption != null && moveOption.targetBlock != null)
            SetAvailableBlock(moveOption.targetBlock);
    }

    private void ForEachAllMoveOptions(Action<MoveOptions> action)
    {
        action?.Invoke(moveToBlock_Forward);
        action?.Invoke(moveToBlock_Back);
        action?.Invoke(moveToBlock_Left);
        action?.Invoke(moveToBlock_Right);

        action?.Invoke(moveToBlock_Ascend);
        action?.Invoke(moveToBlock_Descend);

        action?.Invoke(moveToBlock_SwiftSwimUp);
        action?.Invoke(moveToBlock_SwiftSwimDown);

        action?.Invoke(moveToBlock_Dash_Forward);
        action?.Invoke(moveToBlock_Dash_Back);
        action?.Invoke(moveToBlock_Dash_Left);
        action?.Invoke(moveToBlock_Dash_Right);

        action?.Invoke(moveToBlock_Jump_Forward);
        action?.Invoke(moveToBlock_Jump_Back);
        action?.Invoke(moveToBlock_Jump_Left);
        action?.Invoke(moveToBlock_Jump_Right);

        action?.Invoke(moveToBlock_GrapplingHook);
        action?.Invoke(moveToCeilingGrabbing);

        action?.Invoke(moveToLadder_Forward);
        action?.Invoke(moveToLadder_Back);
        action?.Invoke(moveToLadder_Left);
        action?.Invoke(moveToLadder_Right);
    }

    private void ForEachDarkenMoveOptions(Action<MoveOptions> action)
    {
        action?.Invoke(moveToBlock_Forward);
        action?.Invoke(moveToBlock_Back);
        action?.Invoke(moveToBlock_Left);
        action?.Invoke(moveToBlock_Right);

        action?.Invoke(moveToBlock_Ascend);
        action?.Invoke(moveToBlock_Descend);

        action?.Invoke(moveToBlock_SwiftSwimUp);
        action?.Invoke(moveToBlock_SwiftSwimDown);

        action?.Invoke(moveToBlock_Dash_Forward);
        action?.Invoke(moveToBlock_Dash_Back);
        action?.Invoke(moveToBlock_Dash_Left);
        action?.Invoke(moveToBlock_Dash_Right);

        action?.Invoke(moveToBlock_Jump_Forward);
        action?.Invoke(moveToBlock_Jump_Back);
        action?.Invoke(moveToBlock_Jump_Left);
        action?.Invoke(moveToBlock_Jump_Right);

        action?.Invoke(moveToBlock_GrapplingHook);
        action?.Invoke(moveToCeilingGrabbing);
    }

    private MoveOptions GetMoveOptionForDirection(Vector3 localDir)
    {
        if (localDir == Vector3.forward) return moveToBlock_Forward;
        if (localDir == Vector3.back) return moveToBlock_Back;
        if (localDir == Vector3.left) return moveToBlock_Left;
        if (localDir == Vector3.right) return moveToBlock_Right;
        return null;
    }

    private MoveOptions GetDashOptionForDirection(Vector3 localDir)
    {
        if (localDir == Vector3.forward) return moveToBlock_Dash_Forward;
        if (localDir == Vector3.back) return moveToBlock_Dash_Back;
        if (localDir == Vector3.left) return moveToBlock_Dash_Left;
        if (localDir == Vector3.right) return moveToBlock_Dash_Right;
        return null;
    }

    private MoveOptions GetJumpOptionForDirection(Vector3 localDir)
    {
        if (localDir == Vector3.forward) return moveToBlock_Jump_Forward;
        if (localDir == Vector3.back) return moveToBlock_Jump_Back;
        if (localDir == Vector3.left) return moveToBlock_Jump_Left;
        if (localDir == Vector3.right) return moveToBlock_Jump_Right;
        return null;
    }

    private MoveOptions GetLadderOptionForDirection(Vector3 localDir)
    {
        if (localDir == Vector3.forward) return moveToLadder_Forward;
        if (localDir == Vector3.back) return moveToLadder_Back;
        if (localDir == Vector3.left) return moveToLadder_Left;
        if (localDir == Vector3.right) return moveToLadder_Right;
        return null;
    }

    private bool InputPressedForDirection(Vector3 localDir)
    {
        if (localDir == Vector3.forward) return Inputs.forward_isPressed;
        if (localDir == Vector3.back) return Inputs.back_isPressed;
        if (localDir == Vector3.left) return Inputs.left_isPressed;
        if (localDir == Vector3.right) return Inputs.right_isPressed;
        return false;
    }

    private bool TryRunAbilityMove(MoveOptions option, ref bool stateFlag, Action animationTrigger, Action startEvent, Action statIncrement)
    {
        if (!HasValidTarget(option))
            return false;

        if (!CanAfford(option.targetBlock))
        {
            RespawnPlayer();
            return true;
        }

        statIncrement?.Invoke();
        animationTrigger?.Invoke();
        PerformMovement(option, MovementStates.Moving, abilitySpeed, ref stateFlag);
        startEvent?.Invoke();
        return true;
    }

    private bool IsNearPosition(Vector3 a, Vector3 b, float tolerance = 0.05f)
    {
        return Vector3.Distance(a, b) <= tolerance;
    }

    private bool ShouldSuppressDarkeningForHeldMovement()
    {
        if (movementStates == MovementStates.Moving || movementStates == MovementStates.Falling)
            return true;

        // Only suppress darkening when the player is HOLDING a movement key
        // and there is a valid next normal move in that same held direction.
        if (Inputs.forward_isHold && HasValidTarget(moveToBlock_Forward))
            return true;

        if (Inputs.back_isHold && HasValidTarget(moveToBlock_Back))
            return true;

        if (Inputs.left_isHold && HasValidTarget(moveToBlock_Left))
            return true;

        if (Inputs.right_isHold && HasValidTarget(moveToBlock_Right))
            return true;

        return false;
    }

    private bool ShouldSkipDarkeningBecauseNextHeldMoveWillStart()
    {
        if (movementStates == MovementStates.Moving || movementStates == MovementStates.Falling)
            return true;

        // If a direction is still pressed and that direction already has a valid target,
        // the player is about to continue moving immediately.
        if (Inputs.forward_isPressed && HasValidTarget(moveToBlock_Forward))
            return true;

        if (Inputs.back_isPressed && HasValidTarget(moveToBlock_Back))
            return true;

        if (Inputs.left_isPressed && HasValidTarget(moveToBlock_Left))
            return true;

        if (Inputs.right_isPressed && HasValidTarget(moveToBlock_Right))
            return true;

        return false;
    }

    private bool ShouldBypassWalkDarkeningForHeldChain()
    {
        if (movementStates == MovementStates.Moving || movementStates == MovementStates.Falling)
            return false;

        // Only suppress the brief flash for chained horizontal walking.
        // Do not affect vertical abilities or other move types.
        if (Inputs.forward_isPressed && HasValidTarget(moveToBlock_Forward))
            return true;

        if (Inputs.back_isPressed && HasValidTarget(moveToBlock_Back))
            return true;

        if (Inputs.left_isPressed && HasValidTarget(moveToBlock_Left))
            return true;

        if (Inputs.right_isPressed && HasValidTarget(moveToBlock_Right))
            return true;

        return false;
    }

    private bool IsAnyWalkButtonStillHeldOrPressed()
    {
        return Inputs.forward_isPressed ||
               Inputs.back_isPressed ||
               Inputs.left_isPressed ||
               Inputs.right_isPressed ||
               Inputs.forward_isHold ||
               Inputs.back_isHold ||
               Inputs.left_isHold ||
               Inputs.right_isHold;
    }

    private bool HasAnyImmediateGroundMoveFromCurrentState()
    {
        return HasValidTarget(moveToBlock_Forward) ||
               HasValidTarget(moveToBlock_Back) ||
               HasValidTarget(moveToBlock_Left) ||
               HasValidTarget(moveToBlock_Right);
    }

    private bool ShouldChainImmediatelyAfterStep()
    {
        // Only suppress visuals if player is actually continuing with held walking
        // AND there is actually somewhere to continue moving.
        return IsAnyWalkButtonStillHeldOrPressed() && HasAnyImmediateGroundMoveFromCurrentState();
    }

    private void RefreshDarkeningNow()
    {
        if (movementStates == MovementStates.Moving || movementStates == MovementStates.Falling)
            return;

        RestoreAllSlopeDisplayOverrides();
        ResetDarkenBlocks();
        UpdateBlocks();
        ApplySlopeDisplayOverridesToCurrentTargets();
        SetDarkenBlocks();

        currentlyDarkenedBlocks.Clear();
        foreach (var block in BuildCurrentTargetSet())
        {
            if (block != null)
                currentlyDarkenedBlocks.Add(block);
        }

        Action_UpdatedBlocks?.Invoke();
    }

    private int GetRequiredCost(GameObject obj)
    {
        if (!TryGetBlockInfo(obj, out BlockInfo info))
            return int.MaxValue;

        return Mathf.Max(info.movementCost, info.movementCost_Temp);
    }

    private bool ShouldShowSlopeAsX(GameObject obj)
    {
        if (!TryGetBlockInfo(obj, out BlockInfo targetInfo))
            return false;

        if (targetInfo.blockType != BlockType.Slope)
            return false;

        if (!TryGetStandingInfo(out BlockInfo standingInfo))
            return false;

        if (blockStandingOn == null)
            return false;

        // Player must be standing on a lower block than the slope.
        return blockStandingOn.transform.position.y < obj.transform.position.y;
    }

    private void ApplySlopeDisplayOverride(GameObject obj)
    {
        if (!TryGetBlockInfo(obj, out BlockInfo info))
            return;

        if (ShouldShowSlopeAsX(obj))
        {
            if (!slopeDisplayTempOverrides.ContainsKey(obj))
                slopeDisplayTempOverrides[obj] = info.movementCost_Temp;

            info.movementCost_Temp = -3;
        }
        else
        {
            RestoreSlopeDisplayOverride(obj);
        }
    }

    private void RestoreSlopeDisplayOverride(GameObject obj)
    {
        if (obj == null)
            return;

        if (!TryGetBlockInfo(obj, out BlockInfo info))
            return;

        if (slopeDisplayTempOverrides.TryGetValue(obj, out int originalTempValue))
        {
            info.movementCost_Temp = originalTempValue;
            slopeDisplayTempOverrides.Remove(obj);
        }
    }

    private void RestoreAllSlopeDisplayOverrides()
    {
        foreach (var kvp in slopeDisplayTempOverrides)
        {
            if (kvp.Key != null && TryGetBlockInfo(kvp.Key, out BlockInfo info))
                info.movementCost_Temp = kvp.Value;
        }

        slopeDisplayTempOverrides.Clear();
    }

    private void ApplySlopeDisplayOverridesToCurrentTargets()
    {
        ApplySlopeDisplayOverride(moveToBlock_Forward?.targetBlock);
        ApplySlopeDisplayOverride(moveToBlock_Back?.targetBlock);
        ApplySlopeDisplayOverride(moveToBlock_Left?.targetBlock);
        ApplySlopeDisplayOverride(moveToBlock_Right?.targetBlock);

        ApplySlopeDisplayOverride(moveToBlock_Ascend?.targetBlock);
        ApplySlopeDisplayOverride(moveToBlock_Descend?.targetBlock);

        ApplySlopeDisplayOverride(moveToBlock_SwiftSwimUp?.targetBlock);
        ApplySlopeDisplayOverride(moveToBlock_SwiftSwimDown?.targetBlock);

        ApplySlopeDisplayOverride(moveToBlock_Dash_Forward?.targetBlock);
        ApplySlopeDisplayOverride(moveToBlock_Dash_Back?.targetBlock);
        ApplySlopeDisplayOverride(moveToBlock_Dash_Left?.targetBlock);
        ApplySlopeDisplayOverride(moveToBlock_Dash_Right?.targetBlock);

        ApplySlopeDisplayOverride(moveToBlock_Jump_Forward?.targetBlock);
        ApplySlopeDisplayOverride(moveToBlock_Jump_Back?.targetBlock);
        ApplySlopeDisplayOverride(moveToBlock_Jump_Left?.targetBlock);
        ApplySlopeDisplayOverride(moveToBlock_Jump_Right?.targetBlock);

        ApplySlopeDisplayOverride(moveToBlock_GrapplingHook?.targetBlock);
        ApplySlopeDisplayOverride(moveToCeilingGrabbing?.targetBlock);

        ApplySlopeDisplayOverride(moveToLadder_Forward?.targetBlock);
        ApplySlopeDisplayOverride(moveToLadder_Back?.targetBlock);
        ApplySlopeDisplayOverride(moveToLadder_Left?.targetBlock);
        ApplySlopeDisplayOverride(moveToLadder_Right?.targetBlock);
    }

    private bool IsBlockedDeepWater(GameObject obj)
    {
        if (!TryGetBlockInfo(obj, out BlockInfo info))
            return false;

        if (info.blockElement != BlockElement.Water)
            return false;

        if (PlayerCanEnterDeepWater())
            return false;

        // Check for another water block directly above this one.
        return PerformMovementRaycast(obj.transform.position, Vector3.up, 1f, out GameObject aboveObj) == RaycastHitObjects.BlockInfo &&
               TryGetBlockInfo(aboveObj, out BlockInfo aboveInfo) &&
               aboveInfo.blockElement == BlockElement.Water;
    }

    private bool IsFallingWithCarrierBlockActive()
    {
        if (!isFallingWithCarrierBlock || fallingCarrierBlock == null)
            return false;

        if (!fallingCarrierBlock.activeInHierarchy)
            return false;

        if (!TryGetBlockInfo(fallingCarrierBlock, out BlockInfo info))
            return false;

        return movementStates == MovementStates.Falling &&
               info.movementState == MovementStates.Falling;
    }

    private void SetFallingCarrierBlock(GameObject carrierBlock)
    {
        if (carrierBlock == null)
            return;

        isFallingWithCarrierBlock = true;
        fallingCarrierBlock = carrierBlock;
        blockStandingOn = carrierBlock;
    }

    private void ClearFallingCarrierBlock()
    {
        isFallingWithCarrierBlock = false;
        fallingCarrierBlock = null;

        foreach (var oldBlock in currentlyDarkenedBlocks)
        {
            if (oldBlock != null)
                ResetAvailableBlock(oldBlock);
        }

        currentlyDarkenedBlocks.Clear();
    }

    private void MovementSetup_FallingWithCarrierBlock()
    {
        if (!IsFallingWithCarrierBlockActive())
            return;

        if (isMoving) return;
        if (Block_Moveable.AnyBlockMoving) return;

        RotatePlayerBody_Setup();

        if (TryHandleNormalMovement())
            return;

        if (TryHandleJumpMovement())
            return;
    }

    bool PlayerCanEnterDeepWater()
    {
        var stats = StatsRoot.stats;
        return stats.abilitiesGot_Permanent.OxygenTank ||
               stats.abilitiesGot_Permanent.Flippers ||
               stats.abilitiesGot_Temporary.OxygenTank ||
               stats.abilitiesGot_Temporary.Flippers;
    }

    void RefreshBlocksWhileStandingOnMovingElevator()
    {
        if (movementStates != MovementStates.Still)
            return;

        if (elevatorBeingFollowed == null || blockStandingOn == null)
            return;

        if (blockStandingOn.transform != elevatorBeingFollowed)
            return;

        Vector3 elevatorDelta = elevatorBeingFollowed.position - lastFollowedElevatorPosition;
        float movedDistance = elevatorDelta.magnitude;

        if (movedDistance > 0f)
        {
            elevatorRefreshAccumulatedDistance += movedDistance;
            lastFollowedElevatorPosition = elevatorBeingFollowed.position;
        }

        if (elevatorRefreshAccumulatedDistance < elevatorRefreshDistance)
            return;

        elevatorRefreshAccumulatedDistance = 0f;

        RefreshAvailableMovementBlocksSmooth();
    }

    void ResetWalkAnimationCheck()
    {
        walkAnimationCheck = false;
    }

    public void StopFollowingElevator()
    {
        elevatorBeingFollowed = null;
        elevatorOffset = Vector3.zero;
        lastFollowedElevatorPosition = transform.position;
        elevatorRefreshAccumulatedDistance = 0f;
    }

    #endregion

    #region Movement Functions

    public void UpdateAvailableMovementBlocks()
    {
        RestoreAllSlopeDisplayOverrides();
        ResetDarkenBlocks();
        UpdateBlocks();
        ApplySlopeDisplayOverridesToCurrentTargets();

        if (ShouldChainImmediatelyAfterStep())
        {
            suppressDarkeningWhileChaining = true;
            pendingDarkeningRefreshAfterChain = true;

            currentlyDarkenedBlocks.Clear();
            foreach (var block in BuildCurrentTargetSet())
            {
                if (block != null)
                    currentlyDarkenedBlocks.Add(block);
            }

            Action_UpdatedBlocks?.Invoke();
            return;
        }

        suppressDarkeningWhileChaining = false;
        pendingDarkeningRefreshAfterChain = false;

        SetDarkenBlocks();

        currentlyDarkenedBlocks.Clear();
        foreach (var block in BuildCurrentTargetSet())
        {
            if (block != null)
                currentlyDarkenedBlocks.Add(block);
        }

        Action_UpdatedBlocks?.Invoke();
    }

    public void UpdateBlocks()
    {
        isUpdatingDarkenBlocks = true;

        UpdateBlockStandingOn();

        if (blockStandingOn != null)
        {
            UpdateNormalMovement();

            UpdateSwiftSwimMovement(moveToBlock_SwiftSwimUp, Vector3.up);
            UpdateSwiftSwimMovement(moveToBlock_SwiftSwimDown, Vector3.down);

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
        Cam.isRotating = false;
    }

    public void UpdateBlockStandingOn()
    {
        GameObject prev = blockStandingOn;
        Vector3 playerPos = PM.player.transform.position;
        Vector3 rayDir = CeilingGrab.isCeilingGrabbing ? Vector3.up : Vector3.down;

        //If falling with block
        if (IsFallingWithCarrierBlockActive())
        {
            blockStandingOn = fallingCarrierBlock;

            if (blockStandingOn_Previous != blockStandingOn && !CeilingGrab.isCeilingGrabbing)
                blockStandingOn_Previous = prev;

            if (prev != blockStandingOn)
                Action_isSwitchingBlocks_Invoke();

            return;
        }
        else if (isFallingWithCarrierBlock)
        {
            ClearFallingCarrierBlock();
        }

        if (TryGetBlockUnder(playerPos, rayDir, 1.25f, out GameObject obj))
            blockStandingOn = obj;
        else
            blockStandingOn = null;

        if (blockStandingOn_Previous != blockStandingOn && !CeilingGrab.isCeilingGrabbing)
            blockStandingOn_Previous = prev;

        if (prev != blockStandingOn)
            Action_isSwitchingBlocks_Invoke();

        if (slopeAutoExitInProgress)
        {
            bool leftSourceSlope = blockStandingOn != slopeAutoExitSourceBlock;
            bool reachedTarget = IsNearPosition(transform.position, slopeAutoExitTargetPos + (StandingOffsetDir() * heightOverBlock), 0.15f);

            if (leftSourceSlope || reachedTarget)
            {
                slopeAutoExitInProgress = false;
                slopeAutoExitSourceBlock = null;
            }
        }
    }

    bool TryGetBlockUnder(Vector3 origin, Vector3 dir, float distance, out GameObject block)
    {
        origin += dir * -0.05f;

        if (Physics.Raycast(origin, dir, out RaycastHit localHit, distance, Map.player_LayerMask, QueryTriggerInteraction.Ignore))
        {
            if (localHit.transform.TryGetComponent(out BlockInfo _))
            {
                block = localHit.transform.gameObject;
                return true;
            }
        }

        block = null;
        return false;
    }

    void UpdateNormalMovement()
    {
        UpdateNormalMovements(moveToBlock_Forward, UpdatedDir(Vector3.forward));
        UpdateNormalMovements(moveToBlock_Back, UpdatedDir(Vector3.back));
        UpdateNormalMovements(moveToBlock_Left, UpdatedDir(Vector3.left));
        UpdateNormalMovements(moveToBlock_Right, UpdatedDir(Vector3.right));
    }

    void UpdateNormalMovements(MoveOptions moveOption, Vector3 dir)
    {
        if (!TryGetStandingInfo(out BlockInfo standingInfo))
        {
            ClearMoveTarget(moveOption);
            return;
        }

        GameObject outObj1 = null;
        GameObject outObj2 = null;
        Vector3 playerPos = PM.player.transform.position;
        Vector3 rayDir = GroundCheckDir();

        if (standingInfo.blockType == BlockType.Stair)
        {
            Vector3 stairForward = blockStandingOn.transform.forward.normalized;
            Vector3 stairBackward = -stairForward;

            if (dir == stairForward)
            {
                if (PerformMovementRaycast(playerPos, stairForward, 1, out outObj1) == RaycastHitObjects.None &&
                    PerformMovementRaycast(playerPos + (stairForward / 1.5f), rayDir, 1, out outObj2) == RaycastHitObjects.BlockInfo)
                {
                    EvaluateStandardMovementTarget(moveOption, outObj2, blockStandingOn);
                }
                else
                {
                    ClearMoveTarget(moveOption);
                }
            }
            else if (dir == stairBackward)
            {
                if (PerformMovementRaycast(playerPos + Vector3.up, stairBackward, 1, out outObj1) == RaycastHitObjects.None &&
                    PerformMovementRaycast(playerPos + Vector3.up + stairBackward, rayDir, 1, out outObj2) == RaycastHitObjects.BlockInfo)
                {
                    EvaluateStandardMovementTarget(moveOption, outObj2, blockStandingOn);
                }
                else
                {
                    ClearMoveTarget(moveOption);
                }
            }
            else
            {
                if (PerformMovementRaycast(playerPos, dir, 1, out outObj1) == RaycastHitObjects.None &&
                    PerformMovementRaycast(playerPos + dir, rayDir, 1, out outObj2) == RaycastHitObjects.BlockInfo &&
                    TryGetBlockInfo(outObj2, out BlockInfo targetInfo))
                {
                    if (targetInfo.blockType == BlockType.Stair || targetInfo.blockType == BlockType.Slope)
                    {
                        Vector3 forwardCurrent = blockStandingOn.transform.forward;
                        Vector3 forwardTarget = outObj2.transform.forward;

                        forwardCurrent.y = 0;
                        forwardTarget.y = 0;

                        forwardCurrent.Normalize();
                        forwardTarget.Normalize();

                        float dot = Vector3.Dot(forwardCurrent, forwardTarget);

                        if (dot > 0.9f)
                            SetMoveTarget(moveOption, outObj2);
                        else
                            ClearMoveTarget(moveOption);
                    }
                    else
                    {
                        ClearMoveTarget(moveOption);
                    }
                }
                else
                {
                    ClearMoveTarget(moveOption);
                }
            }

            return;
        }

        if (standingInfo.blockType == BlockType.Slope)
        {
            // If this slope already started its automatic downhill exit,
            // don't let it start the same transition again until we've left it.
            if (slopeAutoExitInProgress && blockStandingOn == slopeAutoExitSourceBlock)
                return;

            Vector3 slopeForward = blockStandingOn.transform.forward.normalized;

            if (dir == slopeForward)
            {
                if (PerformMovementRaycast(playerPos, slopeForward, 1, out outObj1) == RaycastHitObjects.None &&
                    PerformMovementRaycast(playerPos + (slopeForward / 1.5f), rayDir, 1, out outObj2) == RaycastHitObjects.BlockInfo)
                {
                    if (outObj2 != blockStandingOn)
                        SetMoveTarget(moveOption, outObj2);
                    else
                        ClearMoveTarget(moveOption);
                }
                else
                {
                    ClearMoveTarget(moveOption);
                }

                if (moveOption.canMoveTo)
                {
                    if (standingInfo.blockElement == BlockElement.Ice)
                        lastIceGlideDirection = GetMovingDirection(moveOption.targetBlock.transform.position - transform.position);

                    slopeAutoExitInProgress = true;
                    slopeAutoExitSourceBlock = blockStandingOn;
                    slopeAutoExitTargetPos = moveOption.targetBlock.transform.position;

                    slopeLandingIsFree = true;

                    if (pendingSlopeFallAfterUphillAttempt && !isPlayingSlopeFallAnimation)
                    {
                        StartCoroutine(StartSlopeFalling_MoveOption(moveOption, standingInfo.movementSpeed));
                    }
                    else if (!isPlayingSlopeFallAnimation)
                    {
                        PerformMovement(moveOption, MovementStates.Moving, standingInfo.movementSpeed);
                    }
                }
                else
                {
                    Vector3 fallbackTarget = blockStandingOn.transform.position + slopeForward + (Vector3.down * 0.5f);

                    if (standingInfo.blockElement == BlockElement.Ice)
                        lastIceGlideDirection = GetMovingDirection(slopeForward);

                    slopeAutoExitInProgress = true;
                    slopeAutoExitSourceBlock = blockStandingOn;
                    slopeAutoExitTargetPos = fallbackTarget;

                    slopeLandingIsFree = true;

                    if (pendingSlopeFallAfterUphillAttempt && !isPlayingSlopeFallAnimation)
                    {
                        StartCoroutine(StartSlopeFalling_Position(fallbackTarget, standingInfo.movementSpeed));
                    }
                    else if (!isPlayingSlopeFallAnimation)
                    {
                        PerformMovement(fallbackTarget);
                    }
                }
            }

            return;
        }

        if (PerformMovementRaycast(playerPos, dir, 1, out outObj1) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir, rayDir, 1, out outObj2) == RaycastHitObjects.BlockInfo &&
            TryGetBlockInfo(outObj2, out BlockInfo targetInfoCube))
        {
            if (targetInfoCube.blockElement == BlockElement.Water)
            {
                if (IsBlockedDeepWater(outObj2))
                    ClearMoveTarget(moveOption);
                else
                    SetMoveTarget(moveOption, outObj2);
            }
            else if (targetInfoCube.blockElement == BlockElement.Lava)
            {
                ClearMoveTarget(moveOption);
            }
            else if (targetInfoCube.blockType == BlockType.Stair || targetInfoCube.blockType == BlockType.Slope)
            {
                if (transform.position.y > outObj2.transform.position.y + 0.5f &&
                    Vector3.Dot(outObj2.transform.forward, dir.normalized) > 0.5f)
                {
                    SetMoveTarget(moveOption, outObj2);
                }
                else
                {
                    ClearMoveTarget(moveOption);
                }
            }
            else
            {
                SetMoveTarget(moveOption, outObj2);
            }

            return;
        }

        if (outObj1 != null && TryGetBlockInfo(outObj1, out BlockInfo blockInfo1))
        {
            if (blockInfo1.blockType == BlockType.Stair || blockInfo1.blockType == BlockType.Slope)
            {
                Vector3 stairForward = outObj1.transform.forward;
                Vector3 toPlayer = (transform.position - outObj1.transform.position).normalized;
                float dot = Vector3.Dot(stairForward, toPlayer);

                if (dot > 0.5f)
                    SetMoveTarget(moveOption, outObj1);
                else if (transform.position.y > outObj1.transform.position.y + 0.5f &&
                         Vector3.Dot(stairForward, dir.normalized) > 0.5f)
                    SetMoveTarget(moveOption, outObj1);
                else
                    ClearMoveTarget(moveOption);

                return;
            }

            if (PerformMovementRaycast(playerPos + dir, rayDir, 1, out outObj2) == RaycastHitObjects.BlockInfo &&
                TryGetBlockInfo(outObj2, out BlockInfo blockInfo2))
            {
                if (blockInfo1.blockElement == BlockElement.Water && blockInfo2.blockElement == BlockElement.Water)
                {
                    if (IsBlockedDeepWater(outObj2))
                        ClearMoveTarget(moveOption);
                    else
                        SetMoveTarget(moveOption, outObj2);
                }
                else
                {
                    ClearMoveTarget(moveOption);
                }

                return;
            }
        }

        ClearMoveTarget(moveOption);
    }

    private void EvaluateStandardMovementTarget(MoveOptions moveOption, GameObject target, GameObject currentStandingBlock)
    {
        if (target == null || !TryGetBlockInfo(target, out BlockInfo info))
        {
            ClearMoveTarget(moveOption);
            return;
        }

        if (info.blockElement == BlockElement.Water)
        {
            if (IsBlockedDeepWater(target))
                ClearMoveTarget(moveOption);
            else
                SetMoveTarget(moveOption, target);
        }
        else if (info.blockElement == BlockElement.Lava)
        {
            ClearMoveTarget(moveOption);
        }
        else if (target != currentStandingBlock)
        {
            SetMoveTarget(moveOption, target);
        }
        else
        {
            ClearMoveTarget(moveOption);
        }
    }

    void UpdateWaterBlocksForSwiftSwim()
    {
        int swiftSwimCost = 2;
        if (StatsRoot.stats.abilitiesGot_Temporary.Flippers || StatsRoot.stats.abilitiesGot_Permanent.Flippers)
            swiftSwimCost = 1;
        else
            swiftSwimCost = 2;


        if (StatsRoot.stats.abilitiesGot_Temporary.OxygenTank || StatsRoot.stats.abilitiesGot_Permanent.OxygenTank)
        {
            ResetSwiftSwimMovementCost(swiftSwimObject_StandingOn);
            ResetSwiftSwimMovementCost(swiftSwimObject_Up);
            ResetSwiftSwimMovementCost(swiftSwimObject_Down);

            if (isSwiftSwim &&
                TryGetStandingInfo(out BlockInfo standingInfo) &&
                standingInfo.blockElement == BlockElement.Water)
            {
                standingInfo.movementCost = swiftSwimCost;
                standingInfo.movementCost_Temp = swiftSwimCost;
                swiftSwimObject_StandingOn = blockStandingOn;
            }

            if (Physics.Raycast(transform.position + Vector3.down, Vector3.up, out hit, 1, ~swiftSwimLayersToIgnore))
            {
                if (hit.collider != null &&
                    hit.collider.gameObject.TryGetComponent(out BlockInfo upInfo) &&
                    upInfo.blockElement == BlockElement.Water)
                {
                    upInfo.movementCost = swiftSwimCost;
                    upInfo.movementCost_Temp = swiftSwimCost;
                    upInfo.ResetDarkenColor();
                    upInfo.SetDarkenColors();
                    swiftSwimObject_Up = hit.collider.gameObject;
                }
            }

            if (Physics.Raycast(transform.position + Vector3.down, Vector3.down, out hit, 1, ~swiftSwimLayersToIgnore))
            {
                if (hit.collider != null &&
                    hit.collider.gameObject.TryGetComponent(out BlockInfo downInfo) &&
                    downInfo.blockElement == BlockElement.Water &&
                    TryGetStandingInfo(out BlockInfo standingWaterInfo) &&
                    standingWaterInfo.blockElement == BlockElement.Water)
                {
                    downInfo.movementCost = swiftSwimCost;
                    downInfo.movementCost_Temp = swiftSwimCost;
                    downInfo.ResetDarkenColor();
                    downInfo.SetDarkenColors();
                    swiftSwimObject_Down = hit.collider.gameObject;
                }
            }
        }

        if (isSwiftSwim)
        {
            Action_isSwiftSwim_Finished?.Invoke();
            isSwiftSwim = false;
        }
    }

    private void ResetSwiftSwimMovementCost(GameObject obj)
    {
        if (TryGetBlockInfo(obj, out BlockInfo info))
        {
            info.movementCost = 0;
            info.movementCost_Temp = 0;
        }
    }

    void UpdateSwiftSwimMovement(MoveOptions swiftSwimOption, Vector3 dir)
    {
        if (!StatsRoot.stats.abilitiesGot_Temporary.OxygenTank && !StatsRoot.stats.abilitiesGot_Permanent.OxygenTank)
        {
            ClearMoveTarget(swiftSwimOption);
            return;
        }

        if (blockStandingOn == null)
        {
            ClearMoveTarget(swiftSwimOption);
            return;
        }

        if (PerformMovementRaycast(blockStandingOn.transform.position, dir, 1, out GameObject outObj1) == RaycastHitObjects.BlockInfo &&
            TryGetBlockInfo(outObj1, out BlockInfo hitBlock))
        {
            if (hitBlock.blockElement == BlockElement.Water)
            {
                if (dir == Vector3.down &&
                    TryGetStandingInfo(out BlockInfo standingInfo) &&
                    standingInfo.blockElement != BlockElement.Water)
                {
                    ClearMoveTarget(swiftSwimOption);
                }
                else
                {
                    SetMoveTarget(swiftSwimOption, outObj1);
                }
            }
            else
            {
                ClearMoveTarget(swiftSwimOption);
            }
        }
        else
        {
            ClearMoveTarget(swiftSwimOption);
        }
    }

    void UpdateAscendMovement()
    {
        if (moveToBlock_SwiftSwimUp.canMoveTo)
        {
            ClearMoveTarget(moveToBlock_Ascend);
            return;
        }

        if (!PlayerHasAscendAbility())
        {
            ClearMoveTarget(moveToBlock_Ascend);
            return;
        }

        GameObject outObj1 = null;
        GameObject outObj2 = null;
        Vector3 playerPos = PM.player.transform.position;
        Vector3 adjustments;

        if (PerformMovementRaycast(playerPos, Vector3.up, ascendDescend_Distance, out outObj1) == RaycastHitObjects.BlockInfo &&
            TryGetBlockInfo(outObj1, out BlockInfo firstInfo))
        {
            // Block Ascend through stairs and slopes
            if (firstInfo.blockType == BlockType.Stair || firstInfo.blockType == BlockType.Slope)
            {
                ClearMoveTarget(moveToBlock_Ascend);
                return;
            }

            adjustments = Vector3.zero;

            if (firstInfo.blockType == BlockType.Slab)
            {
                RaycastHitObjects secondHit = PerformMovementRaycast(outObj1.transform.position + adjustments, Vector3.up, 1, out outObj2);

                if (secondHit == RaycastHitObjects.None)
                {
                    EvaluateStandardMovementTarget(moveToBlock_Ascend, outObj1, blockStandingOn);
                }
                else if (secondHit == RaycastHitObjects.BlockInfo && TryGetBlockInfo(outObj2, out BlockInfo secondInfo))
                {
                    if (secondInfo.blockType == BlockType.Slab)
                    {
                        SetMoveTarget(moveToBlock_Ascend, outObj1);
                    }
                    else
                        ClearMoveTarget(moveToBlock_Ascend);
                }
                else
                {
                    ClearMoveTarget(moveToBlock_Ascend);
                }
            }
            else
            {
                RaycastHitObjects secondHit = PerformMovementRaycast(outObj1.transform.position + adjustments, Vector3.up, 1, out outObj2);

                if (secondHit == RaycastHitObjects.None)
                {
                    EvaluateStandardMovementTarget(moveToBlock_Ascend, outObj1, blockStandingOn);
                }
                else if (secondHit == RaycastHitObjects.BlockInfo && TryGetBlockInfo(outObj2, out BlockInfo secondInfo))
                {
                    if (secondInfo.blockElement == BlockElement.Water)
                    {
                        if (IsBlockedDeepWater(outObj1))
                            ClearMoveTarget(moveToBlock_Ascend);
                        else
                            SetMoveTarget(moveToBlock_Ascend, outObj1);
                    }
                    else if (secondInfo.blockElement == BlockElement.Lava)
                    {
                        ClearMoveTarget(moveToBlock_Ascend);
                    }
                    else
                    {
                        ClearMoveTarget(moveToBlock_Ascend);
                    }
                }
                else
                {
                    ClearMoveTarget(moveToBlock_Ascend);
                }
            }
        }
        else
        {
            ClearMoveTarget(moveToBlock_Ascend);
        }
    }

    void UpdateDescendMovement()
    {
        if (moveToBlock_SwiftSwimDown.canMoveTo)
        {
            ClearMoveTarget(moveToBlock_Descend);
            return;
        }

        if (!PlayerHasDescendAbility())
        {
            ClearMoveTarget(moveToBlock_Descend);
            return;
        }

        GameObject outObj1 = null;
        GameObject outObj2 = null;
        Vector3 playerPos = PM.player.transform.position;

        if (PerformMovementRaycast(playerPos + Vector3.down, Vector3.down, ascendDescend_Distance + 0.5f, out outObj1) == RaycastHitObjects.BlockInfo &&
            TryGetBlockInfo(outObj1, out BlockInfo firstInfo))
        {
            // Block Descend through stairs and slopes
            if (firstInfo.blockType == BlockType.Stair || firstInfo.blockType == BlockType.Slope)
            {
                ClearMoveTarget(moveToBlock_Descend);
                return;
            }

            if (firstInfo.blockType == BlockType.Slab)
            {
                RaycastHitObjects secondHit = PerformMovementRaycast(outObj1.transform.position + (Vector3.up * 0.25f), Vector3.up, 1, out outObj2);

                if (secondHit == RaycastHitObjects.None)
                {
                    EvaluateStandardMovementTarget(moveToBlock_Descend, outObj1, blockStandingOn);
                }
                else if (secondHit == RaycastHitObjects.BlockInfo && TryGetBlockInfo(outObj2, out BlockInfo secondInfo))
                {
                    if (secondInfo.blockType == BlockType.Slab)
                        SetMoveTarget(moveToBlock_Descend, outObj1);
                    else
                        ClearMoveTarget(moveToBlock_Descend);
                }
                else
                {
                    ClearMoveTarget(moveToBlock_Descend);
                }
            }
            else
            {
                if (PerformMovementRaycast(outObj1.transform.position, Vector3.up, 1, out outObj2) == RaycastHitObjects.None)
                {
                    EvaluateStandardMovementTarget(moveToBlock_Descend, outObj1, blockStandingOn);
                }
                else
                {
                    ClearMoveTarget(moveToBlock_Descend);
                }
            }
        }
        else
        {
            ClearMoveTarget(moveToBlock_Descend);
        }
    }

    void UpdateDashMovement()
    {
        UpdateDashMovements(moveToBlock_Dash_Forward, UpdatedDir(Vector3.forward));
        UpdateDashMovements(moveToBlock_Dash_Back, UpdatedDir(Vector3.back));
        UpdateDashMovements(moveToBlock_Dash_Left, UpdatedDir(Vector3.left));
        UpdateDashMovements(moveToBlock_Dash_Right, UpdatedDir(Vector3.right));
    }

    void UpdateDashMovements(MoveOptions moveOption, Vector3 dir)
    {
        if (!PlayerHasDashAbility() || lookDir != dir || !TryGetStandingInfo(out BlockInfo standingInfo))
        {
            ClearMoveTarget(moveOption);
            return;
        }

        Vector3 playerPos = PM.player.transform.position;
        Vector3 rayDir = GroundCheckDir();

        float correction = standingInfo.blockType == BlockType.Stair ? 0.25f : 0f;

        if (PerformMovementRaycast(playerPos + (Vector3.up * correction), dir, 1, out GameObject outObj1) == RaycastHitObjects.BlockInfo &&
            PerformMovementRaycast(playerPos + dir + (Vector3.up * correction), dir, 1, out GameObject outObj2) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir + dir + (Vector3.up * correction), rayDir, 1, out GameObject outObj3) == RaycastHitObjects.BlockInfo &&
            TryGetBlockInfo(outObj3, out BlockInfo targetInfo))
        {
            if (targetInfo.blockElement == BlockElement.Water)
            {
                if (IsBlockedDeepWater(outObj3))
                    ClearMoveTarget(moveOption);
                else
                    SetMoveTarget(moveOption, outObj3);
            }
            else if (targetInfo.blockElement == BlockElement.Lava)
            {
                ClearMoveTarget(moveOption);
            }
            else if (outObj3 != blockStandingOn)
            {
                SetMoveTarget(moveOption, outObj3);
            }
            else
            {
                ClearMoveTarget(moveOption);
            }
        }
        else
        {
            ClearMoveTarget(moveOption);
        }
    }

    void UpdateJumpMovement()
    {
        UpdateJumpMovements(moveToBlock_Jump_Forward, UpdatedDir(Vector3.forward));
        UpdateJumpMovements(moveToBlock_Jump_Back, UpdatedDir(Vector3.back));
        UpdateJumpMovements(moveToBlock_Jump_Left, UpdatedDir(Vector3.left));
        UpdateJumpMovements(moveToBlock_Jump_Right, UpdatedDir(Vector3.right));
    }

    void UpdateJumpMovements(MoveOptions moveOption, Vector3 dir)
    {
        if (!PlayerHasJumpAbility())
        {
            ClearMoveTarget(moveOption);
            return;
        }

        GameObject finalTarget = null;
        Vector3 playerPos = PM.player.transform.position;
        Vector3 rayDir = GroundCheckDir();

        bool success =
            TryPerformJumpWithCorrection(playerPos, dir, -0.25f, out finalTarget) ||
            TryPerformJumpWithCorrection(playerPos, dir, 0.25f, out finalTarget);

        if (success && TryGetBlockInfo(finalTarget, out BlockInfo info))
        {
            if (info.blockElement == BlockElement.Water)
            {
                if (IsBlockedDeepWater(finalTarget))
                    ClearMoveTarget(moveOption);
                else
                    SetMoveTarget(moveOption, finalTarget);
            }
            else if (info.blockElement == BlockElement.Lava)
            {
                ClearMoveTarget(moveOption);
            }
            else if (info.blockType == BlockType.Stair || info.blockType == BlockType.Slope)
            {
                Vector3 toPlayerFlat = -dir.normalized;
                Vector3 blockForwardFlat = finalTarget.transform.forward;
                blockForwardFlat.y = 0;
                blockForwardFlat.Normalize();

                float dot = Vector3.Dot(blockForwardFlat, toPlayerFlat);

                if (dot < -0.9f)
                    SetMoveTarget(moveOption, finalTarget);
                else
                    ClearMoveTarget(moveOption);
            }
            else if (finalTarget != blockStandingOn)
            {
                SetMoveTarget(moveOption, finalTarget);
            }
            else
            {
                ClearMoveTarget(moveOption);
            }

            return;
        }

        if (PerformMovementRaycast(playerPos, dir, 1, out GameObject outObj_1) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir, rayDir, 1, out GameObject outObj_2) == RaycastHitObjects.BlockInfo &&
            PerformMovementRaycast(playerPos + dir, dir, 1, out GameObject outObj_3) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir + dir, rayDir, 1, out GameObject outObj_4) == RaycastHitObjects.BlockInfo &&
            TryGetBlockInfo(outObj_2, out BlockInfo middleInfo) &&
            (middleInfo.blockType == BlockType.Stair || middleInfo.blockType == BlockType.Slope))
        {
            Vector3 stairForwardFlat = outObj_2.transform.forward;
            stairForwardFlat.y = 0;
            stairForwardFlat.Normalize();

            Vector3 dirFlat = dir;
            dirFlat.y = 0;
            dirFlat.Normalize();

            float dot = Vector3.Dot(stairForwardFlat, dirFlat);

            if (dot > 0.9f)
                ClearMoveTarget(moveOption);
            else
                SetMoveTarget(moveOption, outObj_4);

            return;
        }

        if (PerformMovementRaycast(playerPos, dir, 1, out GameObject outObj_5) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir, rayDir, 1, out GameObject outObj_6) == RaycastHitObjects.BlockInfo &&
            PerformMovementRaycast(playerPos + dir, dir, 1, out GameObject outObj_7) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir + dir, rayDir, 1, out GameObject outObj_8) == RaycastHitObjects.BlockInfo &&
            TryGetBlockInfo(outObj_6, out BlockInfo jumpMiddleInfo))
        {
            if (jumpMiddleInfo.blockElement == BlockElement.Water)
            {
                if (PlayerHasSwimAbility())
                    ClearMoveTarget(moveOption);
                else
                    SetMoveTarget(moveOption, outObj_8);
            }
            else if (jumpMiddleInfo.blockElement == BlockElement.Lava)
            {
                ClearMoveTarget(moveOption);
            }
            else
            {
                ClearMoveTarget(moveOption);
            }

            return;
        }

        if (PerformMovementRaycast(playerPos, dir, 1, out GameObject outObj1) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir, rayDir, 1, out GameObject outObj2) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir, dir, 1, out GameObject outObj3) == RaycastHitObjects.BlockInfo &&
            TryGetBlockInfo(outObj3, out BlockInfo stairInfo))
        {
            if (stairInfo.blockType == BlockType.Stair || stairInfo.blockType == BlockType.Slope)
            {
                Vector3 toPlayerFlat = -dir.normalized;
                Vector3 stairForwardFlat = outObj3.transform.forward;
                stairForwardFlat.y = 0;
                stairForwardFlat.Normalize();

                float dot = Vector3.Dot(stairForwardFlat, toPlayerFlat);

                if (dot > 0.9f)
                    SetMoveTarget(moveOption, outObj3);
                else
                    ClearMoveTarget(moveOption);
            }
            else
            {
                ClearMoveTarget(moveOption);
            }

            return;
        }

        if (PerformMovementRaycast(playerPos, dir, 1, out GameObject outObj4) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir, rayDir, 1, out GameObject outObj5) == RaycastHitObjects.BlockInfo &&
            PerformMovementRaycast(playerPos + dir, dir, 1, out GameObject outObj6) == RaycastHitObjects.BlockInfo &&
            TryGetBlockInfo(outObj6, out BlockInfo stairWaterInfo) &&
            TryGetBlockInfo(outObj5, out BlockInfo betweenInfo))
        {
            if (stairWaterInfo.blockType == BlockType.Stair || stairWaterInfo.blockType == BlockType.Slope)
            {
                if (betweenInfo.blockElement == BlockElement.Water)
                {
                    if (PlayerHasSwimAbility())
                    {
                        ClearMoveTarget(moveOption);
                    }
                    else
                    {
                        Vector3 toPlayerFlat = -dir.normalized;
                        Vector3 stairForwardFlat = outObj6.transform.forward;
                        stairForwardFlat.y = 0;
                        stairForwardFlat.Normalize();

                        float dot = Vector3.Dot(stairForwardFlat, toPlayerFlat);

                        if (dot > 0.9f)
                            SetMoveTarget(moveOption, outObj6);
                        else
                            ClearMoveTarget(moveOption);
                    }
                }
                else if (betweenInfo.blockElement == BlockElement.Lava)
                {
                    ClearMoveTarget(moveOption);
                }
            }
            else
            {
                ClearMoveTarget(moveOption);
            }

            return;
        }

        ClearMoveTarget(moveOption);
    }

    bool TryPerformJumpWithCorrection(Vector3 playerPos, Vector3 dir, float correction, out GameObject targetBlock)
    {
        GameObject o1, o2, o3, o4;
        Vector3 rayDir = GroundCheckDir();

        targetBlock = null;

        if (PerformMovementRaycast(playerPos + (-rayDir * correction), dir, 1, out o1) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir + (-rayDir * correction), rayDir, 1, out o2) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir + (-rayDir * correction), dir, 1, out o3) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir + dir + (-rayDir * correction), rayDir, 1, out o4) == RaycastHitObjects.BlockInfo)
        {
            targetBlock = o4;
            return true;
        }

        if (PerformMovementRaycast(playerPos + (-rayDir * correction), dir, 1, out o1) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir + (-rayDir * correction), rayDir, 1, out o2) == RaycastHitObjects.BlockInfo &&
            PerformMovementRaycast(playerPos + dir + (-rayDir * correction), dir, 1, out o3) == RaycastHitObjects.None &&
            PerformMovementRaycast(playerPos + dir + dir + (-rayDir * correction), rayDir, 1, out o4) == RaycastHitObjects.BlockInfo &&
            TryGetBlockInfo(o2, out BlockInfo middleInfo))
        {
            if (middleInfo.blockElement == BlockElement.Water)
            {
                //if (PlayerHasSwimAbility())
                //    return false;

                targetBlock = o4;
                return true;
            }
        }

        return false;
    }

    public void UpdateGrapplingHookMovement(MoveOptions moveOption, Vector3 dir)
    {
        if (!PlayerHasGrapplingHookAbility())
        {
            ClearMoveTarget(moveOption);
            return;
        }

        StartCoroutine(Delay_UpdateGrapplingHookMovement(moveOption, dir));
    }

    IEnumerator Delay_UpdateGrapplingHookMovement(MoveOptions moveOption, Vector3 dir)
    {
        yield return new WaitForSeconds(Anims.abilityChargeTime_GrapplingHook);

        if (!Inputs.grapplingHook_isPressed)
            yield break;

        Player_GraplingHook.Instance.isGrapplingHooking = true;
        Player_GraplingHook.Instance.EndLineRenderer();

        Vector3 playerPos = transform.position;

        if (PerformMovementRaycast(playerPos, dir, grapplingLength, out GameObject outObj1) == RaycastHitObjects.BlockInfo)
        {
            Collider objCollider = outObj1.GetComponent<Collider>();
            Vector3 contactPoint = objCollider != null
                ? objCollider.ClosestPoint(playerPos + dir * (grapplingLength + 1))
                : outObj1.transform.position + (Vector3.forward * (grapplingLength + 1));

            Player_GraplingHook.Instance.endPoint = contactPoint + (-dir * 0.05f);

            SetMoveTarget(moveOption, outObj1);

            if (TryGetBlockInfo(moveOption.targetBlock, out BlockInfo targetInfo) &&
                (targetInfo.blockType == BlockType.Stair || targetInfo.blockType == BlockType.Slope))
            {
                grapplingTowardsStair = true;
            }
            else
            {
                grapplingTowardsStair = false;
            }

            Vector3 toPlayer = (transform.position - moveToBlock_GrapplingHook.targetBlock.transform.position).normalized;
            bool stairIsFacingPlayer =
                TryGetBlockInfo(moveToBlock_GrapplingHook.targetBlock, out BlockInfo grapplingInfo) &&
                (grapplingInfo.blockType == BlockType.Stair || grapplingInfo.blockType == BlockType.Slope) &&
                Vector3.Dot(moveToBlock_GrapplingHook.targetBlock.transform.forward, toPlayer) > 0.5f;

            if (stairIsFacingPlayer)
            {
                Player_GraplingHook.Instance.redDotSceneObject.transform.SetPositionAndRotation(
                    Player_GraplingHook.Instance.endPoint - (dir * 0.5f),
                    Quaternion.LookRotation(dir));
            }
            else
            {
                Player_GraplingHook.Instance.redDotSceneObject.transform.SetPositionAndRotation(
                    Player_GraplingHook.Instance.endPoint - dir,
                    Quaternion.LookRotation(dir));
            }

            Player_GraplingHook.Instance.hitEffect.transform.SetPositionAndRotation(
                Player_GraplingHook.Instance.redDotSceneObject.transform.position,
                Quaternion.Euler(-90, 0, 0));

            Player_GraplingHook.Instance.hitEffect.GetComponentInChildren<HitParticleScript>().particle.Play();

            Player_GraplingHook.Instance.redDotSceneObject.SetActive(true);
            Player_GraplingHook.Instance.RunLineReader();

            UpdateBlocksOnTheGrapplingWay(moveOption);
        }
        else
        {
            Player_GraplingHook.Instance.endPoint = transform.position + (dir * grapplingLength);
            Player_GraplingHook.Instance.redDotSceneObject.SetActive(false);
            Player_GraplingHook.Instance.RunLineReader();
            ClearMoveTarget(moveOption);
        }
    }

    public void UpdateGrapplingHookMovement_Release()
    {
        if (moveToBlock_GrapplingHook.targetBlock != null && moveToBlock_GrapplingHook.canMoveTo)
        {
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

        ClearMoveTarget(moveToBlock_GrapplingHook);

        Player_GraplingHook.Instance.redDotSceneObject.SetActive(false);
        Player_GraplingHook.Instance.EndLineRenderer();
        Player_GraplingHook.Instance.isGrapplingHooking = false;
        grapplingTargetHasBeenSet = false;
    }

    void UpdateBlocksOnTheGrapplingWay(MoveOptions moveOption)
    {
        if (moveOption == null || moveOption.targetBlock == null)
            return;

        Vector3 playerPos = transform.position;
        Vector3 dir = (moveOption.targetBlock.transform.position - playerPos).normalized;
        float totalDistance = Vector3.Distance(playerPos, moveOption.targetBlock.transform.position);
        int steps = Mathf.FloorToInt(totalDistance);

        for (int i = 1; i <= steps - 1; i++)
        {
            Vector3 samplePos = playerPos + dir * i;

            if (PerformMovementRaycast(samplePos, Vector3.down, 1, out GameObject outObj1) == RaycastHitObjects.BlockInfo)
            {
                grapplingObjects.Add(outObj1);

                if (TryGetBlockInfo(outObj1, out BlockInfo info))
                    info.SetDarkenColors();
            }
        }

        if (grapplingTowardsStair)
        {
            if (PerformMovementRaycast(playerPos + dir * (steps - 1), dir, 1, out GameObject outObj1) == RaycastHitObjects.BlockInfo)
            {
                grapplingObjects.Add(outObj1);

                if (TryGetBlockInfo(outObj1, out BlockInfo info))
                    info.SetDarkenColors();
            }
        }
    }

    void ResetBlocksOnTheGrapplingWay()
    {
        for (int i = grapplingObjects.Count - 1; i >= 0; i--)
        {
            if (TryGetBlockInfo(grapplingObjects[i], out BlockInfo info))
                info.ResetDarkenColor();

            grapplingObjects.RemoveAt(i);
        }
    }

    void UpdateStepsAmonutWhenGrapplingMoving()
    {
        if (performGrapplingHooking &&
            blockStandingOn != null &&
            ((grapplingTowardsStair && Vector3.Distance(transform.position, tempGrapplingTaregtPos) > 1.5f) ||
             (!grapplingTowardsStair && Vector3.Distance(blockStandingOn.transform.position, tempGrapplingTaregtPos) > 1.5f)) &&
            TryGetStandingInfo(out BlockInfo standingInfo))
        {
            if (StatsRoot.stats.steps_Current < 0)
            {
                RespawnPlayer();
                return;
            }

            StatsRoot.stats.steps_Current -= standingInfo.movementCost;
            StepsHUD.Instance.UpdateStepsDisplay_Walking();
        }
    }

    void RespawnUnderGrappling()
    {
        grapplingTowardsStair = false;
        performGrapplingHooking = false;
        grapplingTargetHasBeenSet = false;
        ResetBlocksOnTheGrapplingWay();
    }

    #endregion

    #region PlayerHasAbility

    public bool PlayerHasSwimAbility()
    {
        var stats = StatsRoot.stats;
        return stats.abilitiesGot_Permanent.Snorkel ||
               stats.abilitiesGot_Permanent.OxygenTank ||
               stats.abilitiesGot_Permanent.Flippers ||
               stats.abilitiesGot_Temporary.Snorkel ||
               stats.abilitiesGot_Temporary.OxygenTank ||
               stats.abilitiesGot_Temporary.Flippers;
    }

    public bool PlayerHasSwiftSwimAbility()
    {
        var stats = StatsRoot.stats;
        return stats.abilitiesGot_Permanent.OxygenTank ||
               stats.abilitiesGot_Temporary.OxygenTank;
    }

    bool PlayerHasDashAbility()
    {
        var stats = StatsRoot.stats;
        return stats.abilitiesGot_Permanent.HandDrill ||
               stats.abilitiesGot_Temporary.HandDrill;
    }

    bool PlayerHasJumpAbility()
    {
        var stats = StatsRoot.stats;
        return stats.abilitiesGot_Permanent.SpringShoes ||
               stats.abilitiesGot_Temporary.SpringShoes;
    }

    bool PlayerHasAscendAbility()
    {
        var stats = StatsRoot.stats;
        return stats.abilitiesGot_Permanent.DrillHelmet ||
               stats.abilitiesGot_Temporary.DrillHelmet;
    }

    bool PlayerHasDescendAbility()
    {
        var stats = StatsRoot.stats;
        return stats.abilitiesGot_Permanent.DrillBoots ||
               stats.abilitiesGot_Temporary.DrillBoots;
    }

    bool PlayerHasGrapplingHookAbility()
    {
        var stats = StatsRoot.stats;
        return stats.abilitiesGot_Permanent.GrapplingHook ||
               stats.abilitiesGot_Temporary.GrapplingHook;
    }

    bool PlayerHasCeilingGrabAbility()
    {
        var stats = StatsRoot.stats;
        return stats.abilitiesGot_Permanent.ClimingGloves ||
               stats.abilitiesGot_Temporary.ClimingGloves;
    }

    bool ShouldDelayAbilityMove(Vector3 desiredDir)
    {
        if (desiredDir != lastTurnedToDir)
            return false;

        return Time.time - lastTurnTime < turnBeforeMoveDelay;
    }

    #endregion

    #region SetBlocks

    public void SetDarkenBlocks()
    {
        if (isMoving || movementStates == MovementStates.Moving || movementStates == MovementStates.Falling)
            return;

        if (suppressDarkeningWhileChaining)
            return;

        ForEachDarkenMoveOptions(SetMoveVisual);
    }

    void ResetDarkenBlocks()
    {
        ForEachAllMoveOptions(ResetMoveVisual);
    }
    public void ResetDarkenBlocks_External()
    {
        ResetDarkenBlocks();
    }

    public void SetAvailableBlock(GameObject obj)
    {
        if (TryGetBlockInfo(obj, out BlockInfo info))
        {
            if (!info.blockIsDark)
            {
                info.SetDarkenColors();
            }
        }
    }

    public void ResetAvailableBlock(GameObject obj)
    {
        if (TryGetBlockInfo(obj, out BlockInfo info))
            info.ResetDarkenColor();
    }

    public void RefreshAvailableMovementBlocksSmooth()
    {
        if (movementStates == MovementStates.Moving || movementStates == MovementStates.Falling)
            return;

        RestoreAllSlopeDisplayOverrides();
        UpdateBlocks();
        ApplySlopeDisplayOverridesToCurrentTargets();
        SyncDarkenedBlocksToCurrentTargets();
        Action_UpdatedBlocks?.Invoke();
    }

    void SyncDarkenedBlocksToCurrentTargets()
    {
        HashSet<GameObject> newTargets = BuildCurrentTargetSet();

        foreach (var oldBlock in currentlyDarkenedBlocks)
        {
            if (oldBlock != null && !newTargets.Contains(oldBlock))
                ResetAvailableBlock(oldBlock);
        }

        foreach (var newBlock in newTargets)
        {
            if (newBlock != null && !currentlyDarkenedBlocks.Contains(newBlock))
                SetAvailableBlock(newBlock);
        }

        currentlyDarkenedBlocks.Clear();
        foreach (var block in newTargets)
        {
            if (block != null)
                currentlyDarkenedBlocks.Add(block);
        }
    }

    HashSet<GameObject> BuildCurrentTargetSet()
    {
        HashSet<GameObject> set = new HashSet<GameObject>();

        AddMoveOptionTarget(set, moveToBlock_Forward);
        AddMoveOptionTarget(set, moveToBlock_Back);
        AddMoveOptionTarget(set, moveToBlock_Left);
        AddMoveOptionTarget(set, moveToBlock_Right);

        AddMoveOptionTarget(set, moveToBlock_Ascend);
        AddMoveOptionTarget(set, moveToBlock_Descend);

        AddMoveOptionTarget(set, moveToBlock_SwiftSwimUp);
        AddMoveOptionTarget(set, moveToBlock_SwiftSwimDown);

        AddMoveOptionTarget(set, moveToBlock_Dash_Forward);
        AddMoveOptionTarget(set, moveToBlock_Dash_Back);
        AddMoveOptionTarget(set, moveToBlock_Dash_Left);
        AddMoveOptionTarget(set, moveToBlock_Dash_Right);

        AddMoveOptionTarget(set, moveToBlock_Jump_Forward);
        AddMoveOptionTarget(set, moveToBlock_Jump_Back);
        AddMoveOptionTarget(set, moveToBlock_Jump_Left);
        AddMoveOptionTarget(set, moveToBlock_Jump_Right);

        AddMoveOptionTarget(set, moveToBlock_GrapplingHook);
        AddMoveOptionTarget(set, moveToCeilingGrabbing);

        AddMoveOptionTarget(set, moveToLadder_Forward);
        AddMoveOptionTarget(set, moveToLadder_Back);
        AddMoveOptionTarget(set, moveToLadder_Left);
        AddMoveOptionTarget(set, moveToLadder_Right);

        return set;
    }

    void AddMoveOptionTarget(HashSet<GameObject> set, MoveOptions moveOption)
    {
        if (moveOption != null && moveOption.canMoveTo && moveOption.targetBlock != null)
            set.Add(moveOption.targetBlock);
    }

    #endregion

    #region Run Abilities

    bool RunSwiftSwimUp()
    {
        if (moveToBlock_SwiftSwimUp.canMoveTo)
        {
            isSwiftSwim = true;
            PerformMovement(moveToBlock_SwiftSwimUp, MovementStates.Moving, 2f);
            Action_isSwiftSwim?.Invoke();
            return true;
        }

        return false;
    }

    bool RunSwiftSwimDown()
    {
        if (moveToBlock_SwiftSwimDown.canMoveTo)
        {
            isSwiftSwim = true;
            PerformMovement(moveToBlock_SwiftSwimDown, MovementStates.Moving, 2f);
            Action_isSwiftSwim?.Invoke();
            return true;
        }

        return false;
    }

    void RunGrapplingHook()
    {
        if (!moveToBlock_GrapplingHook.canMoveTo || moveToBlock_GrapplingHook.targetBlock == null)
            return;

        if (!CanAfford(moveToBlock_GrapplingHook.targetBlock))
        {
            RespawnPlayer();
            return;
        }

        isGrapplingHooking = true;

        if (Inputs.grapplingHook_isPressed)
            Anims.Trigger_GrapplingHookAnimation();
        else
            Anims.Trigger_GrapplingHookDraggingAnimation();

        if (TryGetBlockInfo(moveToBlock_GrapplingHook.targetBlock, out BlockInfo info) &&
            (info.blockType == BlockType.Stair || info.blockType == BlockType.Slope))
        {
            Vector3 toPlayer = (transform.position - moveToBlock_GrapplingHook.targetBlock.transform.position).normalized;
            bool isFacingPlayer = Vector3.Dot(moveToBlock_GrapplingHook.targetBlock.transform.forward, toPlayer) > 0.5f;

            if (isFacingPlayer)
            {
                PerformMovement(
                    moveToBlock_GrapplingHook.targetBlock.transform.position - lookDir.normalized + Vector3.down + (Vector3.up * 0.95f) + lookDir,
                    abilitySpeed + grapplingLength);
            }
            else
            {
                PerformMovement(
                    moveToBlock_GrapplingHook.targetBlock.transform.position - lookDir.normalized + Vector3.down + (Vector3.up * 0.5f),
                    abilitySpeed + grapplingLength);
            }
        }
        else
        {
            PerformMovement(moveToBlock_GrapplingHook.targetBlock.transform.position - lookDir.normalized + Vector3.down, abilitySpeed + grapplingLength);
        }

        MapStatsGathered.Instance.levelStats.ability_GrapplingHook++;

        if (TryGetBlockInfo(moveToBlock_GrapplingHook.targetBlock, out BlockInfo targetInfo))
            targetInfo.ResetDarkenColor();

        ClearMoveTarget(moveToBlock_GrapplingHook);
        Player_GraplingHook.Instance.EndLineRenderer();
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
        if (!HasValidTarget(moveToBlock_Ascend))
            return false;

        if (!CanAfford(moveToBlock_Ascend.targetBlock))
        {
            RespawnPlayer();
            return false;
        }

        ResetWalkAnimationCheck();

        isAscending = true;
        PlayerCameraOcclusionController.Instance.CameraZoom(true);

        MapStatsGathered.Instance.levelStats.ability_Ascend++;
        Anims.Trigger_AscendAnimation();
        PerformMovement(moveToBlock_Ascend, MovementStates.Moving, abilitySpeed);
        Action_isAscending?.Invoke();
        return true;
    }

    bool RunDescend()
    {
        if (!HasValidTarget(moveToBlock_Descend))
            return false;

        if (!CanAfford(moveToBlock_Descend.targetBlock))
        {
            RespawnPlayer();
            return false;
        }

        ResetWalkAnimationCheck();

        isDescending = true;
        PlayerCameraOcclusionController.Instance.CameraZoom(true);

        MapStatsGathered.Instance.levelStats.ability_Descend++;
        Anims.Trigger_DescendAnimation();
        PerformMovement(moveToBlock_Descend, MovementStates.Moving, abilitySpeed);
        Action_isDescending?.Invoke();
        return true;
    }

    #endregion

    #region Movement

    public RaycastHitObjects PerformMovementRaycast(Vector3 objPos, Vector3 dir, float distance, out GameObject obj)
    {
        int combinedMask = Map.pickup_LayerMask;

        if (Physics.Raycast(objPos, dir, out hit, distance, combinedMask))
        {
            if (hit.transform.GetComponent<BlockInfo>())
            {
                obj = hit.transform.gameObject;
                return RaycastHitObjects.BlockInfo;
            }

            if (hit.transform.GetComponentInParent<Fence>())
            {
                obj = null;
                return RaycastHitObjects.Fence;
            }

            if (hit.transform.GetComponentInParent<Block_Ladder>() && hit.transform.GetComponent<LadderColliderBlocker>())
            {
                obj = null;
                return RaycastHitObjects.LadderBlocker;
            }

            if (hit.transform.GetComponentInParent<Block_Ladder>() && hit.transform.GetComponent<LadderCollider>())
            {
                obj = hit.transform.parent.gameObject;
                return RaycastHitObjects.Ladder;
            }

            obj = hit.transform.gameObject;
            return RaycastHitObjects.Other;
        }

        obj = null;
        return RaycastHitObjects.None;
    }

    void MovementSetup()
    {
        if (isMoving) return;
        if (Block_Moveable.AnyBlockMoving) return;

        RotatePlayerBody_Setup();

        if (TryHandleLadderMovement())
            return;

        if (TryHandleNormalMovement())
            return;

        if (TryHandleDashMovement())
            return;

        if (TryHandleJumpMovement())
            return;

        if (TryHandleVerticalMovement())
            return;
    }

    private bool TryHandleLadderMovement()
    {
        foreach (var localDir in LocalDirections)
        {
            if (!InputPressedForDirection(localDir))
                continue;

            Vector3 worldDir = UpdatedDir(localDir);
            MoveOptions ladderOption = GetLadderOptionForDirection(localDir);

            if (CheckLaddersToEnter_Up(worldDir) && ladderOption != null && ladderOption.targetBlock && ladderOption.canMoveTo)
            {
                StartCoroutine(PerformLadderMovement_Up(worldDir, GetLadderExitPart_Up(worldDir)));
                return true;
            }

            if (CheckLaddersToEnter_Down(worldDir) && ladderOption != null && ladderOption.targetBlock && ladderOption.canMoveTo)
            {
                StartCoroutine(PerformLadderMovement_Down(worldDir, GetLadderExitPart_Down(worldDir)));
                return true;
            }
        }

        return false;
    }

    private bool TryHandleNormalMovement()
    {
        if (!TryGetStandingInfo(out BlockInfo standingInfo))
            return false;

        foreach (var localDir in LocalDirections)
        {
            if (!InputPressedForDirection(localDir))
                continue;

            MoveOptions moveOption = GetMoveOptionForDirection(localDir);
            if (!HasValidTarget(moveOption))
                continue;

            bool tryingToMoveUpIntoSlope =
                TryGetBlockInfo(moveOption.targetBlock, out BlockInfo targetInfo) &&
                targetInfo.blockType == BlockType.Slope &&
                blockStandingOn != null &&
                blockStandingOn.transform.position.y < moveOption.targetBlock.transform.position.y;

            if (tryingToMoveUpIntoSlope)
            {
                if (!isSlopeFalling)
                    StartCoroutine(StartSlopeFalling());

                return true;
            }

            PerformMovement(moveOption, MovementStates.Moving, standingInfo.movementSpeed);
            return true;
        }

        return false;
    }

    private bool TryHandleDashMovement()
    {
        foreach (var localDir in LocalDirections)
        {
            if (!InputPressedForDirection(localDir))
                continue;

            MoveOptions dashOption = GetDashOptionForDirection(localDir);
            Vector3 worldDir = UpdatedDir(localDir);

            if (HasValidTarget(dashOption) && !ShouldDelayAbilityMove(worldDir))
            {
                return TryRunAbilityMove(
                    dashOption,
                    ref isDashing,
                    () => Anims.Trigger_DashAnimation(),
                    () => Action_isDashing?.Invoke(),
                    () => MapStatsGathered.Instance.levelStats.ability_Dash++);
            }
        }

        return false;
    }

    private bool TryHandleJumpMovement()
    {
        foreach (var localDir in LocalDirections)
        {
            if (!InputPressedForDirection(localDir))
                continue;

            MoveOptions jumpOption = GetJumpOptionForDirection(localDir);
            if (HasValidTarget(jumpOption))
            {
                return TryRunAbilityMove(
                    jumpOption,
                    ref isJumping,
                    () => Anims.Trigger_JumpAnimation(),
                    () => Action_isJumping?.Invoke(),
                    () => MapStatsGathered.Instance.levelStats.ability_Jump++);
            }
        }

        return false;
    }

    private bool TryHandleVerticalMovement()
    {
        if (Inputs.up_isPressed && HasValidTarget(moveToBlock_SwiftSwimUp))
        {
            CheckAscend();
            return true;
        }

        if (Inputs.down_isPressed && HasValidTarget(moveToBlock_SwiftSwimDown))
        {
            CheckDescend();
            return true;
        }

        if (Inputs.up_isPressed && HasValidTarget(moveToBlock_Ascend))
        {
            CheckAscend();
            return true;
        }

        if (Inputs.down_isPressed && HasValidTarget(moveToBlock_Descend))
        {
            CheckDescend();
            return true;
        }

        return false;
    }

    public void PerformMovement(MoveOptions canMoveBlock, MovementStates moveState, float movementSpeed, ref bool isMovingFlag)
    {
        if (canMoveBlock == null || canMoveBlock.targetBlock == null || !TryGetBlockInfo(canMoveBlock.targetBlock, out BlockInfo _) || StatsRoot.stats == null)
            return;

        if (isMovingFlag)
            return;

        if (CanAfford(canMoveBlock.targetBlock) || Player_Pusher.Instance.playerIsPushed)
        {
            isMovingFlag = true;

            ClearFallingCarrierBlock();
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
        if (canMoveBlock == null || canMoveBlock.targetBlock == null || !TryGetBlockInfo(canMoveBlock.targetBlock, out BlockInfo _) || StatsRoot.stats == null)
            return;

        bool allowSlopeMove = TryGetStandingInfo(out BlockInfo standingInfo) && standingInfo.blockType == BlockType.Slope;

        if (CanAfford(canMoveBlock.targetBlock) || allowSlopeMove)
        {
            MovingAnimation(canMoveBlock);

            isMoving = true;

            ClearFallingCarrierBlock();
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
        if (!TryGetStandingInfo(out BlockInfo standingInfo))
            return;

        isMoving = true;

        ClearFallingCarrierBlock();
        ResetDarkenBlocks();
        StartCoroutine(Move(targetPos, MovementStates.Moving, standingInfo.movementSpeed, null));
    }

    public void PerformMovement(Vector3 targetPos, float movementSpeed)
    {
        isMoving = true;

        ClearFallingCarrierBlock();
        ResetDarkenBlocks();
        StartCoroutine(Move(targetPos, MovementStates.Moving, movementSpeed, null));
    }

    private IEnumerator Move(Vector3 endPos, MovementStates moveState, float movementSpeed, MoveOptions moveOptions)
    {
        isMoving = true;

        Action_StepTaken_Early_Invoke();

        if (TryGetStandingInfo(out BlockInfo standingInfo) && standingInfo.blockType == BlockType.Slope)
            hasSlopeGlided = true;

        if (isAscending)
            yield return new WaitForSeconds(Anims.abilityChargeTime_Ascend);
        else if (isDescending)
            yield return new WaitForSeconds(Anims.abilityChargeTime_Descend);
        else if (isDashing)
            yield return new WaitForSeconds(Anims.abilityChargeTime_Dash);
        else if (isJumping)
            yield return new WaitForSeconds(Anims.abilityChargeTime_Jump);

        if (moveOptions != null && moveOptions.targetBlock != null && moveOptions.targetBlock.GetComponent<Block_Elevator>())
            yield return ElevatorMovement(moveState, movementSpeed, moveOptions);
        else
            yield return NormalMovement(endPos, moveState, movementSpeed);

        isMoving = false;
        isDashing = false;
        isJumping = false;
        isGrapplingHooking = false;
        isIceGliding = false;

        isAscending = false;
        isDescending = false;

        //ResetWalkAnimationCheck();

        PlayerCameraOcclusionController.Instance.CameraZoom(false);

        Action_StepTaken_Invoke();

        if (pendingDarkeningRefreshAfterChain &&
            !ShouldChainImmediatelyAfterStep() &&
            movementStates == MovementStates.Still)
        {
            suppressDarkeningWhileChaining = false;
            pendingDarkeningRefreshAfterChain = false;
            RefreshDarkeningNow();
        }
    }

    IEnumerator NormalMovement(Vector3 endPos, MovementStates moveState, float movementSpeed)
    {
        Vector3 rayDir = StandingOffsetDir();
        previousPosition = transform.position;

        Vector3 startPos = transform.position;
        Vector3 newEndPos = endPos + (rayDir * heightOverBlock);

        if (CeilingGrab.isCeilingGrabbing)
            newEndPos = endPos + (rayDir * (heightOverBlock - (Player_BodyHeight.Instance.height_Normal) / 2f));

        movementStates = moveState;

        float elapsed = 0f;
        float duration = MovementDuration(startPos, newEndPos, movementSpeed);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(startPos, newEndPos, t);
            yield return null;
        }

        transform.position = newEndPos;

        movementStates = MovementStates.Still;
        performGrapplingHooking = false;

        isAscending = false;
        isDescending = false;
        PlayerCameraOcclusionController.Instance.CameraZoom(false);
    }

    IEnumerator ElevatorMovement(MovementStates moveState, float movementSpeed, MoveOptions moveOptions)
    {
        Vector3 rayDir = StandingOffsetDir();
        previousPosition = transform.position;

        Transform targetBlockTransform = moveOptions.targetBlock.transform;
        Vector3 startPos = transform.position;
        Vector3 targetOffset = new Vector3(0f, (rayDir * heightOverBlock).y, 0f);
        Vector3 endPos = targetBlockTransform.position + targetOffset;

        movementStates = moveState;

        float elapsed = 0f;
        float duration = MovementDuration(startPos, endPos, movementSpeed);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        transform.position = targetBlockTransform.position + targetOffset;

        elevatorBeingFollowed = targetBlockTransform;
        elevatorOffset = targetOffset;

        lastFollowedElevatorPosition = targetBlockTransform.position;
        elevatorRefreshAccumulatedDistance = 0f;

        movementStates = MovementStates.Still;
        performGrapplingHooking = false;

        isAscending = false;
        isDescending = false;
        PlayerCameraOcclusionController.Instance.CameraZoom(false);
    }

    IEnumerator DelayAscendDescendCamera(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        isAscending = false;
        isDescending = false;
        PlayerCameraOcclusionController.Instance.CameraZoom(false);
    }

    void MovingAnimation(MoveOptions canMoveBlock)
    {
        if (canMoveBlock == null || canMoveBlock.targetBlock == null || !TryGetBlockInfo(canMoveBlock.targetBlock, out BlockInfo targetInfo))
            return;

        if (TryGetStandingInfo(out BlockInfo standingInfo) &&
            standingInfo.blockType == BlockType.Slope &&
            targetInfo.blockType == BlockType.Slope)
        {
            Anims.Trigger_SlopeDownAnimation();
        }
        else if (TryGetStandingInfo(out standingInfo) && standingInfo.blockType == BlockType.Stair)
        {
            Anims.Trigger_StairSlopeWalkingAnimation();
        }
        else if (targetInfo.blockType == BlockType.Stair || targetInfo.blockType == BlockType.Slope)
        {
            Anims.Trigger_StairSlopeWalkingAnimation();
        }
        else if (canMoveBlock.targetBlock != blockStandingOn &&
                 !walkAnimationCheck &&
                 !isIceGliding &&
                 TryGetStandingInfo(out standingInfo) &&
                 standingInfo.blockType != BlockType.Slope)
        {
            Anims.Trigger_WalkingAnimation();
            walkAnimationCheck = true;
        }
    }

    void WalkButtonIsReleased()
    {
        walkAnimationCheck = false;

        suppressDarkeningWhileChaining = false;

        if (pendingDarkeningRefreshAfterChain && movementStates == MovementStates.Still)
        {
            pendingDarkeningRefreshAfterChain = false;
            RefreshDarkeningNow();
        }
    }

    #endregion

    #region Falling

    public void StartFallingWithBlock()
    {
        if (TryGetStandingInfo(out BlockInfo info) &&
            info.movementState == MovementStates.Falling &&
            !CeilingGrab.isCeilingGrabbing)
        {
            SetFallingCarrierBlock(blockStandingOn);
            SetMovementState(MovementStates.Falling);
            ResetDarkenBlocks();
        }
    }

    void StartFallingWithNoBlock()
    {
        if (!CeilingGrab.isCeilingGrabbing)
        {
            ClearFallingCarrierBlock();
            SetMovementState(MovementStates.Falling);
            ResetDarkenBlocks();
        }
    }

    void PlayerIsFalling()
    {
        if (IsFallingWithCarrierBlockActive())
        {
            transform.position = fallingCarrierBlock.transform.position + (Vector3.up * heightOverBlock);
            return;
        }

        if (blockStandingOn != null)
        {
            if (Vector3.Distance(blockStandingOn.transform.position, transform.position) < heightOverBlock + 0.1f)
            {
                transform.position = blockStandingOn.transform.position + (Vector3.up * heightOverBlock);
                EndFalling();
                UpdateAvailableMovementBlocks();
            }
            else
            {
                transform.SetPositionAndRotation(
                    new Vector3(transform.position.x, transform.position.y - (fallSpeed * Time.deltaTime), transform.position.z),
                    transform.rotation);
            }
        }
        else
        {
            transform.SetPositionAndRotation(
                new Vector3(transform.position.x, transform.position.y - (fallSpeed * Time.deltaTime), transform.position.z),
                transform.rotation);
        }
    }

    void EndFalling()
    {
        if (TryGetStandingInfo(out BlockInfo info) && info.movementState != MovementStates.Falling)
        {
            ClearFallingCarrierBlock();
            SetMovementState(MovementStates.Still);
            Action_LandedFromFalling_Invoke();
        }
    }

    IEnumerator LateBlockDetection()
    {
        yield return new WaitForSeconds(0.02f);
        UpdateAvailableMovementBlocks();
    }

    #endregion

    #region IceGliding

    void RunIceGliding()
    {
        IceGlideMovement(false);
    }

    public void IceGlideMovement(bool canIceGlide)
    {
        if (!TryGetStandingInfo(out BlockInfo standingInfo))
            return;

        bool canGlide =
            standingInfo.blockElement == BlockElement.Ice &&
            ((standingInfo.blockType == BlockType.Stair || standingInfo.blockType == BlockType.Slope) ||
             (blockStandingOn.GetComponent<EffectBlockInfo>() && !blockStandingOn.GetComponent<EffectBlockInfo>().effectBlock_Teleporter_isAdded) ||
             canIceGlide);

        if (!canGlide)
        {
            lastIceGlideDirection = Vector3.zero;
            return;
        }

        MoveOptions moveOption = null;
        isIceGliding = true;

        Vector3 movementDir;

        if (canIceGlide)
        {
            movementDir = teleportMovementDir;
        }
        else if (lastIceGlideDirection != Vector3.zero)
        {
            movementDir = lastIceGlideDirection;
        }
        else
        {
            Vector3 movementDelta = transform.position - previousPosition;
            Vector3 horizontalDirection = new Vector3(movementDelta.x, 0, movementDelta.z);
            movementDir = GetMovingDirection(horizontalDirection);
        }

        if (movementDir == Vector3.zero)
        {
            isIceGliding = false;
            lastIceGlideDirection = Vector3.zero;
            return;
        }

        MoveOptions forwardOption = moveToBlock_Forward;
        MoveOptions backOption = moveToBlock_Back;
        MoveOptions leftOption = moveToBlock_Left;
        MoveOptions rightOption = moveToBlock_Right;

        if (movementDir == Vector3.forward && HasValidTarget(forwardOption) && forwardOption.targetBlock != blockStandingOn && IsIce(forwardOption.targetBlock))
            moveOption = forwardOption;
        else if (movementDir == Vector3.back && HasValidTarget(backOption) && backOption.targetBlock != blockStandingOn && IsIce(backOption.targetBlock))
            moveOption = backOption;
        else if (movementDir == Vector3.left && HasValidTarget(leftOption) && leftOption.targetBlock != blockStandingOn && IsIce(leftOption.targetBlock))
            moveOption = leftOption;
        else if (movementDir == Vector3.right && HasValidTarget(rightOption) && rightOption.targetBlock != blockStandingOn && IsIce(rightOption.targetBlock))
            moveOption = rightOption;
        else
        {
            isIceGliding = false;
            lastIceGlideDirection = Vector3.zero;
            return;
        }

        lastIceGlideDirection = movementDir;
        PerformMovement(moveOption, MovementStates.Moving, standingInfo.movementSpeed);
        previousPosition = transform.position;
    }

    public Vector3 GetMovingDirection(Vector3 direction)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
            return Vector3.zero;

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

        return Vector3.zero;
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
        RaycastHitObjects directHit = PerformMovementRaycast(transform.position, dir, 1, out GameObject outObj1);

        if (directHit == RaycastHitObjects.LadderBlocker || directHit == RaycastHitObjects.Fence)
        {
            ClearMoveTarget(moveOptions);
            return;
        }

        if (directHit == RaycastHitObjects.Ladder)
        {
            SetMoveTarget(moveOptions, outObj1.GetComponent<Block_Ladder>().exitBlock_Up);
            outObj1.GetComponent<Block_Ladder>().DarkenExitBlock_Up(dir);
            return;
        }

        if (PerformMovementRaycast(transform.position + (dir * 0.65f), Vector3.down, 1, out outObj1) == RaycastHitObjects.Ladder)
        {
            SetMoveTarget(moveOptions, outObj1.GetComponent<Block_Ladder>().exitBlock_Down);
            outObj1.GetComponent<Block_Ladder>().DarkenExitBlock_Down();
            return;
        }

        ClearMoveTarget(moveOptions);
    }

    bool CheckLaddersToEnter_Up(Vector3 dir)
    {
        return PerformMovementRaycast(transform.position, dir, 1, out GameObject outObj1) == RaycastHitObjects.Ladder;
    }

    bool CheckLaddersToEnter_Down(Vector3 dir)
    {
        return PerformMovementRaycast(transform.position + (dir * 0.65f), Vector3.down, 1, out GameObject outObj1) == RaycastHitObjects.Ladder;
    }

    GameObject GetLadderExitPart_Up(Vector3 dir)
    {
        if (PerformMovementRaycast(transform.position, dir, 1, out GameObject outObj1) == RaycastHitObjects.Ladder)
        {
            ladderToEnterRot = outObj1.transform.rotation;
            return outObj1.GetComponent<Block_Ladder>().lastLadderPart_Up;
        }

        return null;
    }

    GameObject GetLadderExitPart_Down(Vector3 dir)
    {
        if (PerformMovementRaycast(transform.position + (dir * 0.65f), Vector3.down, 1, out GameObject outObj1) == RaycastHitObjects.Ladder)
        {
            ladderToEnterRot = outObj1.transform.rotation;
            return outObj1.GetComponent<Block_Ladder>().lastLadderPart_Down;
        }

        return null;
    }

    IEnumerator PerformLadderMovement_Up(Vector3 dir, GameObject targetPosObj)
    {
        if (targetPosObj == null)
            yield break;

        if (targetPosObj.GetComponent<Block_Ladder>() &&
            targetPosObj.GetComponent<Block_Ladder>().exitBlock_Up &&
            (StatsRoot.stats.steps_Current < targetPosObj.GetComponent<Block_Ladder>().exitBlock_Up.GetComponent<BlockInfo>().movementCost ||
             StatsRoot.stats.steps_Current < targetPosObj.GetComponent<Block_Ladder>().exitBlock_Up.GetComponent<BlockInfo>().movementCost_Temp))
        {
            RespawnPlayer();
            yield break;
        }

        ResetDarkenBlocks();

        isMovingOnLadder_Up = true;
        ladderClimbPos_Start = transform.position;

        movementStates = MovementStates.Moving;
        PM.pauseGame = true;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = targetPosObj.transform.position + (Vector3.up * heightOverBlock);

        float ladderClimbDuration = Vector3.Distance(startPosition, endPosition) * 0.4f;
        float elapsedTime = 0f;

        while (elapsedTime < ladderClimbDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / ladderClimbDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            yield return null;
        }

        transform.position = endPosition;

        if (PerformMovementRaycast(transform.position + dir, Vector3.down, 1, out GameObject outObj1) == RaycastHitObjects.BlockInfo)
            endPosition = outObj1.transform.position + (Vector3.up * heightOverBlock);
        else
            endPosition = startPosition + dir;

        startPosition = transform.position;
        ladderClimbDuration = 0.4f;
        elapsedTime = 0f;

        while (elapsedTime < ladderClimbDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / ladderClimbDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            yield return null;
        }

        transform.position = endPosition;

        ResetLadderTargets();
        UpdateAvailableMovementBlocks();

        isMovingOnLadder_Up = false;
        movementStates = MovementStates.Still;
        PM.pauseGame = false;

        FindLadderExitBlock();
        Action_StepTaken_Invoke();
    }

    IEnumerator PerformLadderMovement_Down(Vector3 dir, GameObject targetPosObj)
    {
        if (targetPosObj == null)
            yield break;

        if (targetPosObj.GetComponent<Block_Ladder>() &&
            targetPosObj.GetComponent<Block_Ladder>().exitBlock_Down &&
            (StatsRoot.stats.steps_Current < targetPosObj.GetComponent<Block_Ladder>().exitBlock_Down.GetComponent<BlockInfo>().movementCost ||
             StatsRoot.stats.steps_Current < targetPosObj.GetComponent<Block_Ladder>().exitBlock_Down.GetComponent<BlockInfo>().movementCost_Temp))
        {
            RespawnPlayer();
            yield break;
        }

        ResetDarkenBlocks();

        isMovingOnLadder_Down = true;
        ladderClimbPos_Start = transform.position;

        movementStates = MovementStates.Moving;
        PM.pauseGame = true;

        float targetY = targetPosObj.transform.eulerAngles.y;
        PM.playerBody.transform.SetLocalPositionAndRotation(
            PM.playerBody.transform.localPosition,
            Quaternion.Euler(0, targetY, 0));

        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + dir;

        float ladderClimbDuration = 0.4f;
        float elapsedTime = 0f;

        while (elapsedTime < ladderClimbDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / ladderClimbDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            yield return null;
        }

        transform.position = endPosition;

        startPosition = transform.position;
        endPosition = targetPosObj.transform.position;

        ladderClimbDuration = Vector3.Distance(startPosition, endPosition) * 0.4f;
        elapsedTime = 0f;

        while (elapsedTime < ladderClimbDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / ladderClimbDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            yield return null;
        }

        transform.position = endPosition;

        ResetLadderTargets();

        if (targetPosObj &&
            targetPosObj.GetComponent<Block_Ladder>() &&
            targetPosObj.GetComponent<Block_Ladder>().exitBlock_Down &&
            targetPosObj.GetComponent<Block_Ladder>().exitBlock_Down.GetComponent<BlockInfo>() &&
            targetPosObj.GetComponent<Block_Ladder>().exitBlock_Down.GetComponent<BlockInfo>().blockElement == BlockElement.Root &&
            targetPosObj.GetComponent<Block_Ladder>().exitBlock_Down.GetComponentInChildren<Block_Root>())
        {
            targetPosObj.GetComponent<Block_Ladder>().exitBlock_Down.GetComponentInChildren<Block_Root>().ActivateRoots();
        }

        UpdateAvailableMovementBlocks();

        targetY = targetPosObj.transform.eulerAngles.y + 180f;
        PM.playerBody.transform.SetLocalPositionAndRotation(
            PM.playerBody.transform.localPosition,
            Quaternion.Euler(0, targetY, 0));

        isMovingOnLadder_Down = false;
        movementStates = MovementStates.Still;
        PM.pauseGame = false;

        FindLadderExitBlock();
        Action_StepTaken_Invoke();
    }

    void ResetLadderTargets()
    {
        ResetMoveVisual(moveToLadder_Forward);
        ResetMoveVisual(moveToLadder_Back);
        ResetMoveVisual(moveToLadder_Left);
        ResetMoveVisual(moveToLadder_Right);

        ClearMoveTarget(moveToLadder_Forward);
        ClearMoveTarget(moveToLadder_Back);
        ClearMoveTarget(moveToLadder_Left);
        ClearMoveTarget(moveToLadder_Right);
    }

    #endregion

    #region Rotating Player

    bool RotatePlayerBody_Setup()
    {
        turnedThisFrame = false;

        if (Inputs.forward_isPressed)
            turnedThisFrame = RotatePlayerBody(0);
        else if (Inputs.back_isPressed)
            turnedThisFrame = RotatePlayerBody(180);
        else if (Inputs.left_isPressed)
            turnedThisFrame = RotatePlayerBody(-90);
        else if (Inputs.right_isPressed)
            turnedThisFrame = RotatePlayerBody(90);

        if (turnedThisFrame)
            lastTurnTime = Time.time;

        return turnedThisFrame;
    }

    public bool RotatePlayerBody(float rotationValue)
    {
        Transform playerBody = PM.playerBody.transform;

        float rotZ = CeilingGrab.isCeilingGrabbing ? -180f : 0f;

        float baseRotation = GetBaseCameraRotation(Cam.cameraRotationState);
        float finalYRotation = NormalizeAngle(baseRotation + rotationValue);
        Quaternion newRotation = Quaternion.Euler(0, finalYRotation, rotZ);

        if (Quaternion.Angle(playerBody.rotation, newRotation) < 0.01f)
            return false;

        playerBody.SetPositionAndRotation(playerBody.position, newRotation);

        Vector3 newFacingDir = GetFacingDirection(finalYRotation);
        Cam.directionFacing = newFacingDir;
        lastTurnedToDir = newFacingDir;

        Action_BodyRotated_Invoke();
        return true;
    }

    float GetBaseCameraRotation(CameraRotationState state)
    {
        switch (state)
        {
            case CameraRotationState.Forward: return 0f;
            case CameraRotationState.Backward: return 180f;
            case CameraRotationState.Left: return 90f;
            case CameraRotationState.Right: return -90f;
            default: return 0f;
        }
    }

    Vector3 GetFacingDirection(float yRotation)
    {
        int angle = Mathf.RoundToInt(NormalizeAngle(yRotation));

        switch (angle)
        {
            case 0:
            case 360: return Vector3.forward;
            case 90: return Vector3.right;
            case 180: return Vector3.back;
            case 270: return Vector3.left;
            default: return Vector3.zero;
        }
    }

    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0f) angle += 360f;
        return angle;
    }

    public bool JustTurnedToward(Vector3 dir)
    {
        return turnedThisFrame && lookDir == dir;
    }

    public bool IsInTurnAbilityDelay(Vector3 dir)
    {
        return lookDir == dir && Time.time - lastTurnTime < turnBeforeMoveDelay;
    }

    #endregion

    #region LookDir
    public void UpdateLookDir()
    {
        float yRotation = Mathf.Round(PM.playerBody.transform.rotation.eulerAngles.y) % 360f;

        if (yRotation < 0f)
            yRotation += 360f;

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
        lookingDirection_Old = lookingDirection;
    }

    public Vector3 UpdatedDir(Vector3 direction)
    {
        switch (Cam.cameraRotationState)
        {
            case CameraRotationState.Forward:
                if (direction == Vector3.forward) return Vector3.forward;
                if (direction == Vector3.back) return Vector3.back;
                if (direction == Vector3.left) return Vector3.left;
                if (direction == Vector3.right) return Vector3.right;
                break;

            case CameraRotationState.Backward:
                if (direction == Vector3.back) return Vector3.forward;
                if (direction == Vector3.forward) return Vector3.back;
                if (direction == Vector3.right) return Vector3.left;
                if (direction == Vector3.left) return Vector3.right;
                break;

            case CameraRotationState.Left:
                if (direction == Vector3.left) return Vector3.forward;
                if (direction == Vector3.right) return Vector3.back;
                if (direction == Vector3.back) return Vector3.left;
                if (direction == Vector3.forward) return Vector3.right;
                break;

            case CameraRotationState.Right:
                if (direction == Vector3.right) return Vector3.forward;
                if (direction == Vector3.left) return Vector3.back;
                if (direction == Vector3.forward) return Vector3.left;
                if (direction == Vector3.back) return Vector3.right;
                break;
        }

        return Vector3.forward;
    }

    #endregion

    #region Take A Step

    public void TakeAStep()
    {
        if (TryGetStandingInfo(out BlockInfo standingInfo))
        {
            UpdateWaterBlocksForSwiftSwim();

            if ((hasSlopeGlided && standingInfo.blockType == BlockType.Slope) || standingInfo.blockType == BlockType.Slope)
                isSlopeGliding = true;

            // Free landing after slope slide
            if (slopeLandingIsFree && standingInfo.blockType != BlockType.Slope)
            {
                slopeLandingIsFree = false;
                hasSlopeGlided = false;
                isSlopeGliding = false;
            }
            else if (hasSlopeGlided && standingInfo.blockType != BlockType.Slope)
            {
                hasSlopeGlided = false;
            }
            else if (!hasSlopeGlided && standingInfo.blockType != BlockType.Slope)
            {
                if (!isSlopeGliding)
                {
                    if (blockStandingOn && blockStandingOn.GetComponent<BlockInfo>() && blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Water && !PlayerHasSwimAbility())
                    {
                        //Don't take away any steps if in water and cannot swim
                    }
                    else
                    {
                        StatsRoot.stats.steps_Current -= standingInfo.movementCost;
                    }

                    if (CeilingGrab.isCeilingGrabbing)
                        MapStatsGathered.Instance.levelStats.ability_CeilingGrab++;
                }

                isSlopeGliding = false;
            }
            else if (isSlopeGliding && standingInfo.blockType != BlockType.Slope)
            {
                isSlopeGliding = false;
            }
            else
            {
                hasSlopeGlided = false;
            }
        }

        if (!slopeLandingIsFree &&
            StatsRoot.stats.steps_Current < 0 &&
            TryGetStandingInfo(out BlockInfo slopeCheckInfo) &&
            slopeCheckInfo.blockType != BlockType.Slope)
        {
            StatsRoot.stats.steps_Current = 0;
            RespawnPlayer();
        }

        Action_StepTaken_Late_Invoke();

        if (isSlopeGliding)
            isSlopeGliding = false;
    }

    #endregion

    #region Drowning

    IEnumerator StartDrowning()
    {
        isDrowning = true;
        PlayerManager.Instance.PauseGame();

        yield return new WaitForSeconds(0.15f);

        Player_Animations.Instance.Trigger_DrowningAnimation();

        yield return new WaitForSeconds(2.35f);

        RespawnPlayer();
    }
    IEnumerator StartSlopeFalling()
    {
        if (isSlopeFalling)
            yield break;

        isSlopeFalling = true;
        PlayerManager.Instance.PauseGame();

        //yield return new WaitForSeconds(0.1f);

        Player_Animations.Instance.Trigger_SlopeFallingAnimation();

        yield return new WaitForSeconds(2f);

        PlayerManager.Instance.UnpauseGame();
        isSlopeFalling = false;
    }
    IEnumerator StartSlopeFalling_MoveOption(MoveOptions moveOption, float movementSpeed)
    {
        if (isPlayingSlopeFallAnimation)
            yield break;

        isPlayingSlopeFallAnimation = true;
        PlayerManager.Instance.PauseGame();

        //yield return new WaitForSeconds(0.1f);

        Player_Animations.Instance.Trigger_SlopeFallingAnimation();

        yield return new WaitForSeconds(2f);

        PlayerManager.Instance.UnpauseGame();

        pendingSlopeFallAfterUphillAttempt = false;
        isPlayingSlopeFallAnimation = false;

        if (moveOption != null && moveOption.targetBlock != null)
            PerformMovement(moveOption, MovementStates.Moving, movementSpeed);
    }

    IEnumerator StartSlopeFalling_Position(Vector3 targetPos, float movementSpeed)
    {
        if (isPlayingSlopeFallAnimation)
            yield break;

        isPlayingSlopeFallAnimation = true;
        PlayerManager.Instance.PauseGame();

        //yield return new WaitForSeconds(0.1f);

        Player_Animations.Instance.Trigger_SlopeFallingAnimation();

        yield return new WaitForSeconds(2f);

        PlayerManager.Instance.UnpauseGame();

        pendingSlopeFallAfterUphillAttempt = false;
        isPlayingSlopeFallAnimation = false;

        PerformMovement(targetPos, movementSpeed);
    }

    #endregion

    #region Other
    void CancelSlopeIfFalling()
    {
        if (movementStates == MovementStates.Falling && (isSlopeGliding || hasSlopeGlided))
        {
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
        isRespawning = true;

        Inputs.forward_isPressed = false;
        Inputs.back_isPressed = false;
        Inputs.left_isPressed = false;
        Inputs.right_isPressed = false;
        Inputs.up_isPressed = false;
        Inputs.down_isPressed = false;
        Inputs.grapplingHook_isPressed = false;

        Inputs.forward_isHold = false;
        Inputs.back_isHold = false;
        Inputs.left_isHold = false;
        Inputs.right_isHold = false;

        Inputs.cameraX_isPressed = false;
        Inputs.cameraY_isPressed = false;

        isSlopeGliding = false;
        hasSlopeGlided = false;
        slopeLandingIsFree = false;
        lastIceGlideDirection = Vector3.zero;

        slopeAutoExitInProgress = false;
        slopeAutoExitSourceBlock = null;
        slopeAutoExitTargetPos = Vector3.zero;

        isAscending = false;
        isDescending = false;
        PlayerCameraOcclusionController.Instance.CameraZoom(false);

        ClearFallingCarrierBlock();
        SetMovementState(MovementStates.Moving);

        if (!SFX_Respawn.Instance.isRespawning)
            RespawnPlayerByHolding_Action();

        RespawnPlayerEarly_Action();

        yield return new WaitForSeconds(waitTime);

        transform.position = Map.playerStartPos;
        PM.playerBody.transform.SetPositionAndRotation(Map.playerStartPos, GetRespawnPlayerDirection(0, 180, 0));
        PM.playerBody.transform.SetLocalPositionAndRotation(
            new Vector3(PM.playerBody.transform.localPosition.x, Player_BodyHeight.Instance.height_Normal, PM.playerBody.transform.localPosition.z),
            PM.playerBody.transform.localRotation);

        CeilingGrab.ResetCeilingGrab();

        yield return new WaitForSeconds(waitTime);

        StatsRoot.RefillStepsToMax();
        Cam.SetRespawnCameraRotation();

        RespawnPlayer_Action();

        yield return new WaitForSeconds(waitTime * 30f);

        previousPosition = transform.position;

        elevatorBeingFollowed = null;
        elevatorOffset = Vector3.zero;
        lastFollowedElevatorPosition = transform.position;
        elevatorRefreshAccumulatedDistance = 0f;

        SetMovementState(MovementStates.Still);

        RespawnPlayerLate_Action();

        StopAllCoroutines();

        isRespawning = false;
        isDrowning = false;
        isSlopeFalling = false;
        pendingSlopeFallAfterUphillAttempt = false;
        isPlayingSlopeFallAnimation = false;

        PlayerManager.Instance.UnpauseGame();
    }

    public Quaternion GetRespawnPlayerDirection(int corr_X, int corr_Y, int corr_Z)
    {
        switch (Map.playerStartRot)
        {
            case MovementDirection.None:
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
        if (TryGetStandingInfo(out BlockInfo info) &&
            info.blockElement == BlockElement.Water &&
            PlayerHasSwimAbility())
        {
            MapStatsGathered.Instance.levelStats.ability_Swim++;
        }
    }

    public void SetMovementState(MovementStates state)
    {
        movementStates = state;
    }

    public MovementStates GetMovementState()
    {
        return movementStates;
    }

    #endregion

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

    public void RespawnPlayerByHolding_Action()
    {
        Action_RespawnPlayerByHolding?.Invoke();
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

    public void Action_isDashing_Finished_Invoke()
    {
        Action_isDashing_Finished?.Invoke();
    }

    public void Action_isJumping_Finished_Invoke()
    {
        Action_isJumping_Finished?.Invoke();
    }

    public void Action_isAscending_Finished_Invoke()
    {
        Action_isAscending_Finished?.Invoke();
    }

    public void Action_isDescending_Finished_Invoke()
    {
        Action_isDescending_Finished?.Invoke();
    }

    public void Action_isGrapplingHooking_Invoke()
    {
        Action_isGrapplingHooking?.Invoke();
    }

    public void Action_isGrapplingHooking_Finished_Invoke()
    {
        Action_isGrapplingHooking_Finished?.Invoke();
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