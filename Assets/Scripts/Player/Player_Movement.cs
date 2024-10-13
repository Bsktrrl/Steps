using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : Singleton<Player_Movement>
{
    public static Action updateStepDisplay;
    public static Action resetBlockColor;

    public MovementStates movementStates;

    float heightOverBlock = 0.85f;

    Vector3 endDestination;
    [SerializeField] float playerSpeed = 5;


    //--------------------


    private void Start()
    {
        updateStepDisplay?.Invoke();
    }
    private void Update()
    {
        NewKeyInputs();

        if (movementStates == MovementStates.Moving && endDestination != (Vector3.zero + (Vector3.up * heightOverBlock)))
        {
            MovePlayer();
        }
        else
        {
            movementStates = MovementStates.Still;
        }
    }


    //--------------------


    void NewKeyInputs()
    {
        if (movementStates == MovementStates.Moving) { return; }
        if (MainManager.Instance.pauseGame) { return; }

        //If pressing UP
        if (Input.GetKey(KeyCode.W))
        {
            if (MainManager.Instance.canMove_Forward)
            {
                //Set new Position - Based on the Block to enter
                endDestination = MainManager.Instance.block_Vertical_InFront.blockPosition + (Vector3.up * heightOverBlock);
                SetPlayerBodyRotation(0);
                movementStates = MovementStates.Moving;

                resetBlockColor?.Invoke();
            }
            else
            {
                SetPlayerBodyRotation(0);
            }
        }

        //If pressing DOWN
        else if (Input.GetKey(KeyCode.S))
        {
            if (MainManager.Instance.canMove_Back)
            {
                endDestination = MainManager.Instance.block_Vertical_InBack.blockPosition + (Vector3.up * heightOverBlock);
                SetPlayerBodyRotation(180);
                movementStates = MovementStates.Moving;

                resetBlockColor?.Invoke();
            }
            else
            {
                SetPlayerBodyRotation(180);
            }
        }

        //If pressing LEFT
        else if (Input.GetKey(KeyCode.A))
        {
            if (MainManager.Instance.canMove_Left)
            {
                endDestination = MainManager.Instance.block_Vertical_ToTheLeft.blockPosition + (Vector3.up * heightOverBlock);
                SetPlayerBodyRotation(-90);
                movementStates = MovementStates.Moving;

                resetBlockColor?.Invoke();
            }
            else
            {
                SetPlayerBodyRotation(-90);
            }
        }

        //If pressing RIGHT
        else if (Input.GetKey(KeyCode.D))
        {
            if (MainManager.Instance.canMove_Right)
            {
                endDestination = MainManager.Instance.block_Vertical_ToTheRight.blockPosition + (Vector3.up * heightOverBlock);
                SetPlayerBodyRotation(90);
                movementStates = MovementStates.Moving;

                resetBlockColor?.Invoke();
            }
            else
            {
                SetPlayerBodyRotation(90);
            }
        }
    }
    void SetPlayerBodyRotation(int rotationValue)
    {
        //Set new Rotation - Based on the key input
        switch (gameObject.GetComponent<Player_Camera>().cameraState)
        {
            case CameraState.Forward:
                MainManager.Instance.playerBody.transform.SetPositionAndRotation(MainManager.Instance.playerBody.transform.position, Quaternion.Euler(0, 0 + rotationValue, 0));
                break;
            case CameraState.Backward:
                MainManager.Instance.playerBody.transform.SetPositionAndRotation(MainManager.Instance.playerBody.transform.position, Quaternion.Euler(0, 180 + rotationValue, 0));
                break;
            case CameraState.Left:
                MainManager.Instance.playerBody.transform.SetPositionAndRotation(MainManager.Instance.playerBody.transform.position, Quaternion.Euler(0, -90 + rotationValue, 0));
                break;
            case CameraState.Right:
                MainManager.Instance.playerBody.transform.SetPositionAndRotation(MainManager.Instance.playerBody.transform.position, Quaternion.Euler(0, 90 + rotationValue, 0));
                break;

            default:
                break;
        }
    }

    void MovePlayer()
    {
        MainManager.Instance.player.transform.position = Vector3.MoveTowards(MainManager.Instance.player.transform.position, endDestination, playerSpeed * Time.deltaTime);

        if (Vector3.Distance(MainManager.Instance.player.transform.position, endDestination) <= 0.05f)
        {
            MainManager.Instance.player.transform.position = endDestination;
            movementStates = MovementStates.Still;

            updateStepDisplay?.Invoke();
        }
    }
}

public enum MovementStates
{
    Still,
    Moving
}