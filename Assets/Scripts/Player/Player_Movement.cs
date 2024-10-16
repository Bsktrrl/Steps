using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : Singleton<Player_Movement>
{
    public static event Action Action_StepTaken;
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
        KeyInputs();

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


    void KeyInputs()
    {
        if (movementStates == MovementStates.Moving) { return; }
        if (MainManager.Instance.pauseGame) { return; }
        if (Cameras.Instance.isRotating) { return; }
        if (Player_Interact.Instance.isInteracting) { return; }

        //If pressing UP
        if (Input.GetKey(KeyCode.W))
        {
            if (MainManager.Instance.canMove_Forward)
            {
                if (MainManager.Instance.block_Vertical_InFront != null)
                    if (MainManager.Instance.block_Vertical_InFront.block != null)
                        if (MainManager.Instance.block_Vertical_InFront.block.GetComponent<BlockInfo>())
                        {
                            MainManager.Instance.block_MovingTowards = MainManager.Instance.block_Vertical_InFront;

                            currentMovementCost = MainManager.Instance.block_Vertical_InFront.block.GetComponent<BlockInfo>().movementCost;

                            endDestination = MainManager.Instance.block_Vertical_InFront.blockPosition + (Vector3.up * heightOverBlock);
                            SetPlayerBodyRotation(0);
                            movementStates = MovementStates.Moving;

                            Action_resetBlockColor?.Invoke();
                        }
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
                if (MainManager.Instance.block_Vertical_InBack != null)
                    if (MainManager.Instance.block_Vertical_InBack.block != null)
                        if (MainManager.Instance.block_Vertical_InBack.block.GetComponent<BlockInfo>())
                        {
                            MainManager.Instance.block_MovingTowards = MainManager.Instance.block_Vertical_InBack;

                            currentMovementCost = MainManager.Instance.block_Vertical_InBack.block.GetComponent<BlockInfo>().movementCost;

                            endDestination = MainManager.Instance.block_Vertical_InBack.blockPosition + (Vector3.up * heightOverBlock);
                            SetPlayerBodyRotation(180);
                            movementStates = MovementStates.Moving;

                            Action_resetBlockColor?.Invoke();
                        }
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
                if (MainManager.Instance.block_Vertical_ToTheLeft != null)
                    if (MainManager.Instance.block_Vertical_ToTheLeft.block != null)
                        if (MainManager.Instance.block_Vertical_ToTheLeft.block.GetComponent<BlockInfo>())
                        {
                            MainManager.Instance.block_MovingTowards = MainManager.Instance.block_Vertical_ToTheLeft;


                            currentMovementCost = MainManager.Instance.block_Vertical_ToTheLeft.block.GetComponent<BlockInfo>().movementCost;

                            endDestination = MainManager.Instance.block_Vertical_ToTheLeft.blockPosition + (Vector3.up * heightOverBlock);
                            SetPlayerBodyRotation(-90);
                            movementStates = MovementStates.Moving;

                            Action_resetBlockColor?.Invoke();
                        }
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
                if (MainManager.Instance.block_Vertical_ToTheRight != null)
                    if (MainManager.Instance.block_Vertical_ToTheRight.block != null)
                        if (MainManager.Instance.block_Vertical_ToTheRight.block.GetComponent<BlockInfo>())
                        {
                            MainManager.Instance.block_MovingTowards = MainManager.Instance.block_Vertical_ToTheRight;

                            currentMovementCost = MainManager.Instance.block_Vertical_ToTheRight.block.GetComponent<BlockInfo>().movementCost;

                            endDestination = MainManager.Instance.block_Vertical_ToTheRight.blockPosition + (Vector3.up * heightOverBlock);
                            SetPlayerBodyRotation(90);
                            movementStates = MovementStates.Moving;

                            Action_resetBlockColor?.Invoke();
                        }
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
        switch (Cameras.Instance.cameraState)
        {
            case CameraState.Forward:
                MainManager.Instance.playerBody.transform.SetPositionAndRotation(MainManager.Instance.playerBody.transform.position, Quaternion.Euler(0, 0 + rotationValue, 0));
                if (rotationValue == 0 || rotationValue == 360)
                    Cameras.Instance.directionFacing = Vector3.forward;
                else if (rotationValue == 180)
                    Cameras.Instance.directionFacing = Vector3.back;
                else if (rotationValue == 90)
                    Cameras.Instance.directionFacing = Vector3.right;
                else if (rotationValue == -90 || rotationValue == 270)
                    Cameras.Instance.directionFacing = Vector3.left;
                break;
            case CameraState.Backward:
                MainManager.Instance.playerBody.transform.SetPositionAndRotation(MainManager.Instance.playerBody.transform.position, Quaternion.Euler(0, 180 + rotationValue, 0));
                if (180 + rotationValue == 0 || 180 + rotationValue == 360)
                    Cameras.Instance.directionFacing = Vector3.back;
                else if (180 + rotationValue == 180)
                    Cameras.Instance.directionFacing = Vector3.forward;
                else if (180 + rotationValue == 90)
                    Cameras.Instance.directionFacing = Vector3.left;
                else if (180 + rotationValue == -90 || 180 + rotationValue == 270)
                    Cameras.Instance.directionFacing = Vector3.right;
                break;
            case CameraState.Left:
                MainManager.Instance.playerBody.transform.SetPositionAndRotation(MainManager.Instance.playerBody.transform.position, Quaternion.Euler(0, 90 + rotationValue, 0));
                if (90 + rotationValue == 0 || 90 + rotationValue == 360)
                    Cameras.Instance.directionFacing = Vector3.left;
                else if (90 + rotationValue == 180)
                    Cameras.Instance.directionFacing = Vector3.right;
                else if (90 + rotationValue == 90)
                    Cameras.Instance.directionFacing = Vector3.forward;
                else if (90 + rotationValue == -90 || 90 + rotationValue == 270)
                    Cameras.Instance.directionFacing = Vector3.back;
                break;
            case CameraState.Right:
                MainManager.Instance.playerBody.transform.SetPositionAndRotation(MainManager.Instance.playerBody.transform.position, Quaternion.Euler(0, -90 + rotationValue, 0));
                if (-90 + rotationValue == 0 || -90 + rotationValue == 360)
                    Cameras.Instance.directionFacing = Vector3.right;
                else if (-90 + rotationValue == 180 || -90 + rotationValue == -180)
                    Cameras.Instance.directionFacing = Vector3.left;
                else if (-90 + rotationValue == 90)
                    Cameras.Instance.directionFacing = Vector3.back;
                else if (-90 + rotationValue == -90 || -90 + rotationValue == 270)
                    Cameras.Instance.directionFacing = Vector3.forward;
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
                //Check if the block standing on is different from the one entering, to move with what the player stands on
                if (MainManager.Instance.block_StandingOn.blockElement != MainManager.Instance.block_MovingTowards.blockElement && MainManager.Instance.block_StandingOn.block)
                {
                    if (MainManager.Instance.block_MovingTowards.block.GetComponent<BlockInfo>().movementSpeed <= 0)
                        MainManager.Instance.player.transform.position = Vector3.MoveTowards(MainManager.Instance.player.transform.position, endDestination, 5 * Time.deltaTime);
                    else
                        MainManager.Instance.player.transform.position = Vector3.MoveTowards(MainManager.Instance.player.transform.position, endDestination, MainManager.Instance.block_StandingOn.block.GetComponent<BlockInfo>().movementSpeed * Time.deltaTime);
                }
                else
                {
                    if (MainManager.Instance.block_MovingTowards.block.GetComponent<BlockInfo>().movementSpeed <= 0)
                        MainManager.Instance.player.transform.position = Vector3.MoveTowards(MainManager.Instance.player.transform.position, endDestination, 5 * Time.deltaTime);
                    else
                        MainManager.Instance.player.transform.position = Vector3.MoveTowards(MainManager.Instance.player.transform.position, endDestination, MainManager.Instance.block_MovingTowards.block.GetComponent<BlockInfo>().movementSpeed * Time.deltaTime);
                }
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

            Action_StepTaken?.Invoke();
        }
    }
}

public enum MovementStates
{
    Still,
    Moving
}