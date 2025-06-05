using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Singleton<Movement>
{
    public static event Action Action_StepTaken;
    public static event Action Action_BodyRotated;
    public static event Action Action_isSwitchingBlocks;
    public static event Action Action_LandedFromFalling;

    [Header("States")]
    public bool isMoving;
    public MovementStates movementStates;

    [Header("Stats")]
    public float heightOverBlock = 0.95f;
    [HideInInspector] public float movementSpeed = 0.2f;
    [HideInInspector] public float fallSpeed = 6f;

    [Header("CeilingGrab")]
    [SerializeField] bool isCeilingGrabbing;

    [Header("BlockIsStandingOn")]
    public GameObject blockStandingOn;

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
        UpdateAvailableMovementBlocks();
    }
    private void Update()
    {
        if (GetMovementState() == MovementStates.Moving)
        {
            UpdateBlockStandingOn();
        }
    }


    //--------------------


    private void OnEnable()
    {
        Action_StepTaken += UpdateAvailableMovementBlocks;
        Action_LandedFromFalling += UpdateAvailableMovementBlocks;
        Action_BodyRotated += UpdateAvailableMovementBlocks;
    }
    private void OnDisable()
    {
        Action_StepTaken -= UpdateAvailableMovementBlocks;
        Action_LandedFromFalling -= UpdateAvailableMovementBlocks;
        Action_BodyRotated -= UpdateAvailableMovementBlocks;
    }


    //--------------------


    void UpdateAvailableMovementBlocks()
    {
        ResetDarkenBlocks();

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

        SetDarkenBlocks();
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
        GameObject outObj = null;
        Vector3 playerPos = PlayerManager.Instance.player.transform.position;

        Vector3 rayDir = Vector3.zero;

        if (isCeilingGrabbing)
            rayDir = Vector3.up;
        else
            rayDir = Vector3.down;

        if (PerformMovementRaycast(playerPos, dir, 1, out outObj) == RaycastHitObjects.None
           && PerformMovementRaycast(playerPos + dir, rayDir, 1, out outObj) == RaycastHitObjects.BlockInfo)
        {
            moveOption.canMoveTo = true;
            moveOption.targetBlock = outObj;
        }
        else
        {
            moveOption.canMoveTo = false;
            moveOption.targetBlock = null;
        }
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

    void SetDarkenBlocks()
    {
        if (moveToBlock_Forward.targetBlock)
            moveToBlock_Forward.targetBlock.GetComponent<BlockInfo>().SetDarkenColors();
        if (moveToBlock_Back.targetBlock)
            moveToBlock_Back.targetBlock.GetComponent<BlockInfo>().SetDarkenColors();
        if (moveToBlock_Left.targetBlock)
            moveToBlock_Left.targetBlock.GetComponent<BlockInfo>().SetDarkenColors();
        if (moveToBlock_Right.targetBlock)
            moveToBlock_Right.targetBlock.GetComponent<BlockInfo>().SetDarkenColors();

        if (moveToBlock_Ascend.targetBlock)
            moveToBlock_Ascend.targetBlock.GetComponent<BlockInfo>().SetDarkenColors();
        if (moveToBlock_Descend.targetBlock)
            moveToBlock_Descend.targetBlock.GetComponent<BlockInfo>().SetDarkenColors();

        if (moveToBlock_SwiftSwimUp.targetBlock)
            moveToBlock_SwiftSwimUp.targetBlock.GetComponent<BlockInfo>().SetDarkenColors();
        if (moveToBlock_SwiftSwimDown.targetBlock)
            moveToBlock_SwiftSwimDown.targetBlock.GetComponent<BlockInfo>().SetDarkenColors();

        if (moveToBlock_Dash.targetBlock)
            moveToBlock_Dash.targetBlock.GetComponent<BlockInfo>().SetDarkenColors();
        if (moveToBlock_Jump.targetBlock)
            moveToBlock_Jump.targetBlock.GetComponent<BlockInfo>().SetDarkenColors();
        if (moveToBlock_GrapplingHook.targetBlock)
            moveToBlock_GrapplingHook.targetBlock.GetComponent<BlockInfo>().SetDarkenColors();
        if (moveToCeilingGrabbing.targetBlock)
            moveToCeilingGrabbing.targetBlock.GetComponent<BlockInfo>().SetDarkenColors();
    }
    void ResetDarkenBlocks()
    {
        if (moveToBlock_Forward.targetBlock)
            moveToBlock_Forward.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
        if (moveToBlock_Back.targetBlock)
            moveToBlock_Back.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
        if (moveToBlock_Left.targetBlock)
            moveToBlock_Left.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
        if (moveToBlock_Right.targetBlock)
            moveToBlock_Right.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();

        if (moveToBlock_Ascend.targetBlock)
            moveToBlock_Ascend.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
        if (moveToBlock_Descend.targetBlock)
            moveToBlock_Descend.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();

        if (moveToBlock_SwiftSwimUp.targetBlock)
            moveToBlock_SwiftSwimUp.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
        if (moveToBlock_SwiftSwimDown.targetBlock)
            moveToBlock_SwiftSwimDown.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();

        if (moveToBlock_Dash.targetBlock)
            moveToBlock_Dash.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
        if (moveToBlock_Jump.targetBlock)
            moveToBlock_Jump.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
        if (moveToBlock_GrapplingHook.targetBlock)
            moveToBlock_GrapplingHook.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
        if (moveToCeilingGrabbing.targetBlock)
            moveToCeilingGrabbing.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
    }
    public void SetAvailableBlock(GameObject obj)
    {
        if (obj.GetComponent<BlockInfo>())
        {
            if (!obj.GetComponent<BlockInfo>().blockIsDark)
            {
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

    public void PerformMovement(Vector3 startPos, MoveOptions canMoveBlock, float speed)
    {
        if(canMoveBlock.canMoveTo)
        {
            StartCoroutine(Move(startPos, canMoveBlock.targetBlock.transform.position, speed));
        }
    }

    private IEnumerator Move(Vector3 startPos, Vector3 endPos, float speed)
    {
        movementStates = MovementStates.Moving;

        float elapsed = 0f;

        while (elapsed < speed)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / speed);
            PlayerManager.Instance.player.transform.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        PlayerManager.Instance.player.transform.position = endPos;

        movementStates = MovementStates.Still;
        Action_StepTaken_Invoke();
    }


    //--------------------


    void HoverMovement()
    {
        if (GetMovementState() == MovementStates.Still)
        {
            
        }
    }
    void FallingMovement()
    {
        SetMovementState(MovementStates.Moving);





        SetMovementState(MovementStates.Still);
        Action_LandedFromFalling_Invoke();
    }

    void FollowElevatorBlockMovement()
    {

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


    //--------------------


    void CheckCurrentBlockStandingOn()
    {

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

        PlayerManager.Instance.lookingDirection = lookDir;
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


    public void SetMovementState(MovementStates state)
    {
        movementStates = state;
    }
    public MovementStates GetMovementState()
    { 
        return movementStates; 
    }

    public void Action_StepTaken_Invoke()
    {
        Action_StepTaken?.Invoke();
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