using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : Singleton<Player_Movement>
{
    public static event Action Action_takeAStep;
    public static event Action Action_resetBlockColor;

    [Header("Current Movement Cost")]
    public int currentMovementCost;

    [Header("Movement State")]
    public MovementStates movementStates;

    [Header("Player Movement over Blocks")]
    float heightOverBlock = 0.85f;

    //Other
    Vector3 endDestination;


    //--------------------


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
                MainManager.Instance.block_MovingTowards = MainManager.Instance.block_Vertical_InFront;

                currentMovementCost = MainManager.Instance.block_Vertical_InFront.block.GetComponent<BlockInfo>().movementCost;

                endDestination = MainManager.Instance.block_Vertical_InFront.blockPosition + (Vector3.up * heightOverBlock);
                SetPlayerBodyRotation(0);
                movementStates = MovementStates.Moving;

                Action_resetBlockColor?.Invoke();
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
                MainManager.Instance.block_MovingTowards = MainManager.Instance.block_Vertical_InBack;

                currentMovementCost = MainManager.Instance.block_Vertical_InBack.block.GetComponent<BlockInfo>().movementCost;

                endDestination = MainManager.Instance.block_Vertical_InBack.blockPosition + (Vector3.up * heightOverBlock);
                SetPlayerBodyRotation(180);
                movementStates = MovementStates.Moving;

                Action_resetBlockColor?.Invoke();
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
                MainManager.Instance.block_MovingTowards = MainManager.Instance.block_Vertical_ToTheLeft;

                currentMovementCost = MainManager.Instance.block_Vertical_ToTheLeft.block.GetComponent<BlockInfo>().movementCost;

                endDestination = MainManager.Instance.block_Vertical_ToTheLeft.blockPosition + (Vector3.up * heightOverBlock);
                SetPlayerBodyRotation(-90);
                movementStates = MovementStates.Moving;

                Action_resetBlockColor?.Invoke();
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
                MainManager.Instance.block_MovingTowards = MainManager.Instance.block_Vertical_ToTheRight;

                currentMovementCost = MainManager.Instance.block_Vertical_ToTheRight.block.GetComponent<BlockInfo>().movementCost;

                endDestination = MainManager.Instance.block_Vertical_ToTheRight.blockPosition + (Vector3.up * heightOverBlock);
                SetPlayerBodyRotation(90);
                movementStates = MovementStates.Moving;

                Action_resetBlockColor?.Invoke();
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
        //Move with a set speed
        if (MainManager.Instance.block_MovingTowards != null)
        {
            if (MainManager.Instance.block_MovingTowards.block != null)
            {
                if (MainManager.Instance.block_MovingTowards.block.GetComponent<BlockInfo>().movementSpeed <= 0)
                    MainManager.Instance.player.transform.position = Vector3.MoveTowards(MainManager.Instance.player.transform.position, endDestination, 5 * Time.deltaTime);
                else
                    MainManager.Instance.player.transform.position = Vector3.MoveTowards(MainManager.Instance.player.transform.position, endDestination, MainManager.Instance.block_MovingTowards.block.GetComponent<BlockInfo>().movementSpeed * Time.deltaTime);
            }
            else
                MainManager.Instance.player.transform.position = Vector3.MoveTowards(MainManager.Instance.player.transform.position, endDestination, 5 * Time.deltaTime);
        }
        else
            MainManager.Instance.player.transform.position = Vector3.MoveTowards(MainManager.Instance.player.transform.position, endDestination, 5 * Time.deltaTime);

        //Snap into place when close enough
        if (Vector3.Distance(MainManager.Instance.player.transform.position, endDestination) <= 0.05f)
        {
            MainManager.Instance.player.transform.position = endDestination;
            movementStates = MovementStates.Still;

            Action_takeAStep?.Invoke();
        }
    }
}

public enum MovementStates
{
    Still,
    Moving
}