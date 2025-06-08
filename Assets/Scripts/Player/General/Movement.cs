using System;
using System.Collections;
using System.Collections.Generic;
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
    [HideInInspector] public float fallSpeed = 6f;
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

    public MoveOptions moveToBlock_Dash;
    public MoveOptions moveToBlock_Jump;
    public MoveOptions moveToBlock_GrapplingHook;
    public MoveOptions moveToCeilingGrabbing;

    RaycastHit hit;


    //--------------------


    private void Start()
    {
        savePos = transform.position;

        RespawnPlayer();
        //UpdateAvailableMovementBlocks();
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

            EndFalling();
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
        UpdateBlockStandingOn();
        UpdateNormalMovement();
        UpdateAscendMovement();
        UpdateDescendMovement();
        UpdateSwiftSwimUpMovement();
        UpdateSwiftSwimDownMovement();
        UpdateDashMovement();
        UpdateJumpMovement();
        UpdateGrapplingHookMovement();
        UpdateCeilingGrabMovement();
    }
    void UpdateBlockStandingOn()
    {
        GameObject obj = null;

        PerformMovementRaycast(PlayerManager.Instance.player.transform.position, Vector3.down, 1, out obj);

        if (blockStandingOn != obj)
        {
            blockStandingOn = null;
        }

        blockStandingOn = obj;
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
        GameObject outObj1 = null;
        GameObject outObj2 = null;
        Vector3 playerPos = PlayerManager.Instance.player.transform.position;

        Vector3 rayDir = Vector3.zero;

        if (isCeilingGrabbing)
            rayDir = Vector3.up;
        else
            rayDir = Vector3.down;

        //If standing on a Stair
        //if (blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Stair)
        //{
        //    Vector3 stairForward = blockStandingOn.transform.forward;
        //    Vector3 stairBackward = -stairForward;

        //    if (dir != stairForward && dir != stairBackward)
        //    {
        //        Block_IsNot_Target(moveOption);
        //        return;
        //    }

        //    stairForward.y = 0;
        //    stairBackward.y = 0;
        //    stairForward.Normalize();
        //    stairBackward.Normalize();

        //    foreach (Vector3 stairDir in new[] { stairForward, stairBackward })
        //    {
        //        if (PerformMovementRaycast(playerPos, stairDir, 1, out outObj1) == RaycastHitObjects.None &&
        //            PerformMovementRaycast(playerPos + stairDir, stairDir, 1, out outObj2) == RaycastHitObjects.BlockInfo)
        //        {
        //            if (outObj2.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
        //            {
        //                if (PlayerHasSwimAbility())
        //                    Block_Is_Target(moveOption, outObj2);
        //                else
        //                    Block_IsNot_Target(moveOption);
        //            }
        //            else
        //                Block_Is_Target(moveOption, outObj2);

        //            return;
        //        }
        //    }
        //}

        ////If standing on a Slope
        //else if (blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Stair)
        //{

        //}

        //If standing on the Ground
        //else
        //{
        //    if (PerformMovementRaycast(playerPos, dir, 1, out outObj1) == RaycastHitObjects.None
        //    && PerformMovementRaycast(playerPos + dir, rayDir, 1, out outObj2) == RaycastHitObjects.BlockInfo)
        //    {
        //        //Check if it's a Water Block where the player want to move
        //        if (outObj2.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
        //        {
        //            if (PlayerHasSwimAbility())
        //                Block_Is_Target(moveOption, outObj2);
        //            else
        //                Block_IsNot_Target(moveOption);
        //        }
        //        else
        //            Block_Is_Target(moveOption, outObj2);

        //        return;
        //    }
        //    else
        //    {
        //        //Check if it's a Stair or Slope in front of the player
        //        if (outObj1)
        //        {
        //            BlockInfo blockInfo = outObj1.GetComponent<BlockInfo>();

        //            if (blockInfo != null && (blockInfo.blockType == BlockType.Stair || blockInfo.blockType == BlockType.Slope))
        //            {
        //                Vector3 stairForward = outObj1.transform.forward;
        //                Vector3 toPlayer = (transform.position - outObj1.transform.position).normalized;

        //                float dot = Vector3.Dot(stairForward, toPlayer);

        //                if (dot > 0.5f) // Adjust threshold as needed
        //                {
        //                    //Stair is facing the player
        //                    Block_Is_Target(moveOption, outObj1);
        //                }
        //                else
        //                {
        //                    //Stair is facing away from the player
        //                    Block_IsNot_Target(moveOption);
        //                }

        //                return;
        //            }
        //        }
        //    }
        //}

        if (PerformMovementRaycast(playerPos, dir, 1, out outObj1) == RaycastHitObjects.None //Front = None
        && PerformMovementRaycast(playerPos + dir, rayDir, 1, out outObj2) == RaycastHitObjects.BlockInfo) //Front-Down = Block
        {
            //Check if it's a Water Block where the player want to move
            if (outObj2.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
            {
                if (PlayerHasSwimAbility())
                    Block_Is_Target(moveOption, outObj2);
                else
                    Block_IsNot_Target(moveOption);
            }
            else
            {
                Block_Is_Target(moveOption, outObj2);
            }

            return;
        }
        else
        {
            //Check if it's a Stair or Slope in front of the player
            if (outObj1)
            {
                BlockInfo blockInfo = outObj1.GetComponent<BlockInfo>();

                if (blockInfo != null && (blockInfo.blockType == BlockType.Stair || blockInfo.blockType == BlockType.Slope))
                {
                    Vector3 stairForward = outObj1.transform.forward;
                    Vector3 toPlayer = (transform.position - outObj1.transform.position).normalized;

                    float dot = Vector3.Dot(stairForward, toPlayer);

                    if (dot > 0.5f) // Adjust threshold as needed
                    {
                        //Stair is facing the player
                        Block_Is_Target(moveOption, outObj1);
                    }
                    else
                    {
                        //Stair is facing away from the player
                        Block_IsNot_Target(moveOption);
                    }

                    return;
                }
            }
        }

        Block_IsNot_Target(moveOption);
    }

    void UpdateAscendMovement()
    {

    }
    void UpdateDescendMovement()
    {

    }
    void UpdateSwiftSwimUpMovement()
    {

    }
    void UpdateSwiftSwimDownMovement()
    {

    }
    void UpdateDashMovement()
    {

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


    //--------------------


    void SetDarkenBlocks()
    {
        if (Player_KeyInputs.Instance.forward_isPressed || Player_KeyInputs.Instance.back_isPressed || Player_KeyInputs.Instance.left_isPressed || Player_KeyInputs.Instance.right_isPressed) { return; }


        if (moveToBlock_Forward.targetBlock)
        {
            SetAvailableBlock(moveToBlock_Forward.targetBlock);

            print("1. SetDarkenBlock");
        }
        if (moveToBlock_Back.targetBlock)
        {
            SetAvailableBlock(moveToBlock_Back.targetBlock);

            print("2. SetDarkenBlock");
        }
        if (moveToBlock_Left.targetBlock)
        {
            SetAvailableBlock(moveToBlock_Left.targetBlock);

            print("3. SetDarkenBlock");
        }
        if (moveToBlock_Right.targetBlock)
        {
            SetAvailableBlock(moveToBlock_Right.targetBlock);

            print("4. SetDarkenBlock");
        }

        if (moveToBlock_Ascend.targetBlock)
            SetAvailableBlock(moveToBlock_Ascend.targetBlock);
        if (moveToBlock_Descend.targetBlock)
            SetAvailableBlock(moveToBlock_Descend.targetBlock);

        if (moveToBlock_SwiftSwimUp.targetBlock)
            SetAvailableBlock(moveToBlock_SwiftSwimUp.targetBlock);
        if (moveToBlock_SwiftSwimDown.targetBlock)
            SetAvailableBlock(moveToBlock_SwiftSwimDown.targetBlock);

        if (moveToBlock_Dash.targetBlock)
            SetAvailableBlock(moveToBlock_Dash.targetBlock);
        if (moveToBlock_Jump.targetBlock)
            SetAvailableBlock(moveToBlock_Jump.targetBlock);
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

        if (moveToBlock_Dash.targetBlock)
            ResetAvailableBlock(moveToBlock_Dash.targetBlock);
        if (moveToBlock_Jump.targetBlock)
            ResetAvailableBlock(moveToBlock_Jump.targetBlock);
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
            if (obj.GetComponent<BlockInfo>().blockIsDark)
            {
                obj.GetComponent<BlockInfo>().ResetDarkenColor();
            }
        }
    }


    //--------------------


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
            PerformMovement(moveToBlock_Forward);
        else if (Player_KeyInputs.Instance.back_isPressed && moveToBlock_Back.targetBlock && moveToBlock_Back.canMoveTo)
            PerformMovement(moveToBlock_Back);
        else if (Player_KeyInputs.Instance.left_isPressed && moveToBlock_Left.targetBlock && moveToBlock_Left.canMoveTo)
            PerformMovement(moveToBlock_Left);
        else if (Player_KeyInputs.Instance.right_isPressed && moveToBlock_Right.targetBlock && moveToBlock_Right.canMoveTo)
            PerformMovement(moveToBlock_Right);
    }
    public void PerformMovement(MoveOptions canMoveBlock)
    {
        if (PlayerStats.Instance.stats.steps_Current >= canMoveBlock.targetBlock.GetComponent<BlockInfo>().movementCost)
        {
            ResetDarkenBlocks();

            StartCoroutine(Move(canMoveBlock.targetBlock.transform.position));
        }
        else
        {
            RespawnPlayer();
        }
    }

    private IEnumerator Move(Vector3 endPos)
    {
        Vector3 startPos = transform.position;
        Vector3 newEndPos = endPos + (Vector3.up * heightOverBlock);

        movementStates = MovementStates.Moving;

        float elapsed = 0f;
        float distance = Vector3.Distance(startPos, newEndPos);

        while (elapsed < 1f)
        {
            //Get current speed based on the current block
            float currentSpeed = baseTime / blockStandingOn.GetComponent<BlockInfo>().movementSpeed;

            //Invert speed relation: higher speed value = faster movement
            float speedFactor = 1f / Mathf.Max(currentSpeed, 0.01f); // avoid divide by zero
            float moveStep = Time.deltaTime * speedFactor;

            elapsed += moveStep;

            float t = Mathf.Clamp01(elapsed);
            transform.position = Vector3.Lerp(startPos, newEndPos, t);

            yield return null;
        }

        transform.position = newEndPos;

        UpdateBlockLookingAt();

        movementStates = MovementStates.Still;
        Action_StepTaken_Invoke();
    }


    //--------------------


    public void StartFalling()
    {
        if (blockStandingOn.GetComponent<BlockInfo>().movementState == MovementStates.Falling)
        {
            SetMovementState(MovementStates.Falling);

            ResetDarkenBlocks();
        }
    }
    void PlayerIsFalling()
    {
        if (blockStandingOn)
        {
            gameObject.transform.position = blockStandingOn.transform.position + (Vector3.up * heightOverBlock);
        }
        else
        {
            //Just fall untill a block becomes "blockStandingOn"
            gameObject.transform.SetPositionAndRotation(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - (6 * Time.deltaTime), gameObject.transform.position.z), gameObject.transform.rotation);
        }
    }
    void EndFalling()
    {
        if (blockStandingOn.GetComponent<BlockInfo>().movementState != MovementStates.Falling)
        {
            SetMovementState(MovementStates.Still);
            Action_LandedFromFalling_Invoke();
        }
    }

    void IceGlideMovement()
    {

    }

    void SlopeGlideMovement()
    {

    }

    void LadderMovement()
    {

    }

    void FollowElevatorBlockMovement()
    {

    }


    //--------------------


    void CheckCurrentBlockStandingOn()
    {

    }
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
        print("Respawn Player");

        StartCoroutine(Resetplayer(0.01f));
    }
    IEnumerator Resetplayer(float waitTime)
    {
        Player_KeyInputs.Instance.forward_isPressed = false;
        Player_KeyInputs.Instance.back_isPressed = false;
        Player_KeyInputs.Instance.left_isPressed = false;
        Player_KeyInputs.Instance.right_isPressed = false;

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
    Falling
}
public enum MovementDirection
{
    None,

    Forward,
    Backward,
    Left,
    Right
}