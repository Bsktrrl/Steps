using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player_Movement_v2 : Singleton<Player_Movement_v2>
{
    public static event Action Action_MovePlayer_Before;
    public static event Action Action_MovePlayer_After;
    public static event Action Action_RotatePlayer_Before;
    public static event Action Action_RotatePlayer_After;

    [Header("Movement Options")]
    public bool canMove_Forward;
    public bool canMove_Back;
    public bool canMove_Left;
    public bool canMove_Right;

    [Header("Movement State")]
    public MovementStates movementState;
    public bool isSlopeGliding;
    public bool isIceGliding;

    [Header("Stats")]
    float movementSpeed = 0.2f;
    [HideInInspector] public float fallSpeed = 6f;
    [SerializeField] GameObject block_MovingInto;


    //--------------------


    private void Update()
    {
        Key_MovementInput();
    }


    //--------------------


    void Key_MovementInput()
    {
        if (!PreventButtonsOfTrigger()) { return; }

        if (Input.GetKey(KeyCode.W))
        {
            RotatePlayerBody(DirectionCalculator(Vector3.forward));

            canMove_Forward = Player_BlockDetector_v2.Instance.Raycast_IfDirectionIsAllowedToMove(transform.position, DirectionCalculator(Vector3.forward), 1);

            if (canMove_Forward)
            {
                block_MovingInto = Player_BlockDetector_v2.Instance.Raycast_MoveToBlock(transform.position + Vector3.down, DirectionCalculator(Vector3.forward), 1);
                StartCoroutine(MovePlayer());
            }

        }
        if (Input.GetKey(KeyCode.S))
        {
            RotatePlayerBody(DirectionCalculator(Vector3.back));

            canMove_Back = Player_BlockDetector_v2.Instance.Raycast_IfDirectionIsAllowedToMove(transform.position, DirectionCalculator(Vector3.back), 1);

            if (canMove_Back)
            {
                block_MovingInto = Player_BlockDetector_v2.Instance.Raycast_MoveToBlock(transform.position + Vector3.down, DirectionCalculator(Vector3.back), 1);
                StartCoroutine(MovePlayer());
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            RotatePlayerBody(DirectionCalculator(Vector3.left));

            canMove_Left = Player_BlockDetector_v2.Instance.Raycast_IfDirectionIsAllowedToMove(transform.position, DirectionCalculator(Vector3.left), 1);

            if (canMove_Left)
            {
                block_MovingInto = Player_BlockDetector_v2.Instance.Raycast_MoveToBlock(transform.position + Vector3.down, DirectionCalculator(Vector3.left), 1);
                StartCoroutine(MovePlayer());
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            RotatePlayerBody(DirectionCalculator(Vector3.right));

            canMove_Right = Player_BlockDetector_v2.Instance.Raycast_IfDirectionIsAllowedToMove(transform.position, DirectionCalculator(Vector3.right), 1);

            if (canMove_Right)
            {
                block_MovingInto = Player_BlockDetector_v2.Instance.Raycast_MoveToBlock(transform.position + Vector3.down, DirectionCalculator(Vector3.right), 1);
                StartCoroutine(MovePlayer());
            }
        }
    }


    //--------------------


    void RotatePlayerBody(Vector3 direction)
    {
        Action_RotatePlayer_Before?.Invoke();

        if (direction == Vector3.forward)
            PlayerManager_v2.Instance.playerBodyObject.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, 0));
        else if (direction == Vector3.back)
            PlayerManager_v2.Instance.playerBodyObject.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 180, 0));
        else if (direction == Vector3.left)
            PlayerManager_v2.Instance.playerBodyObject.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, -90, 0));
        else if (direction == Vector3.right)
            PlayerManager_v2.Instance.playerBodyObject.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 90, 0));

        Action_RotatePlayer_After?.Invoke();
    }
    IEnumerator MovePlayer()
    {
        Action_MovePlayer_Before?.Invoke();

        movementState = MovementStates.Moving;

        Vector3 startPosition = transform.position;
        Vector3 endPosition;

        if (block_MovingInto)
            endPosition = block_MovingInto.transform.position + Vector3.up;
        else
            endPosition = transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < movementSpeed)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the progress (0 to 1) of the jump
            float progress = elapsedTime / movementSpeed;

            // Interpolate the forward position
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, progress);

            // Update the player's position
            transform.position = currentPosition;

            yield return null;
        }

        // Ensure the player lands exactly at the end position
        transform.position = endPosition;

        movementState = MovementStates.Still;

        //Player_BlockDetector.Instance.RaycastSetup();

        //Player_BlockDetector.Instance.Update_BlockStandingOn();

        //Player_Movement.Instance.Action_ResetBlockColorInvoke();
        //Player_Movement.Instance.Action_StepTaken_Invoke();

        Action_MovePlayer_After?.Invoke();
    }

    public Vector3 DirectionCalculator(Vector3 direction)
    {
        switch (Cameras_v2.Instance.cameraRotationState)
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

    public bool PreventButtonsOfTrigger()
    {
        if (movementState == MovementStates.Moving) { return false; }

        if (isSlopeGliding) { return false; }
        if (isIceGliding) { return false; }

        if (PlayerManager_v2.Instance.pauseGame) { return false; }
        if (Cameras_v2.Instance.isRotating) { return false; }
        if (Player_Interact.Instance.isInteracting) { return false; }
        if (Player_GraplingHook.Instance.isGrapplingHooking) { return false; }
        if (Player_Dash.Instance.isDashing) { return false; }
        if (Player_Ascend.Instance.isAscending) { return false; }
        if (Player_Descend.Instance.isDescending) { return false; }
        if (Player_Jumping.Instance.isJumping) { return false; }
        if (Player_SwiftSwim.Instance.isSwiftSwimming_Down) { return false; }
        if (Player_SwiftSwim.Instance.isSwiftSwimming_Up) { return false; }
        if (Player_CeilingGrab.Instance.isCeilingRotation) { return false; }

        return true;
    }
}