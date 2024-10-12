using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : Singleton<NewPlayerMovement>
{
    public static Action updateStepDisplay;
    public static Action resetBlockColor;

    public MovementStates movementStates;

    float hightOverBlock = 0.85f;

    Vector3 endDestination;
    [SerializeField] float playerSpeed = 5;


    //--------------------


    private void Start()
    {
        updateStepDisplay?.Invoke();

        UpdateDarkenBlocks();
    }
    private void Update()
    {
        NewKeyInputs();

        if (movementStates == MovementStates.Moving && endDestination != (Vector3.zero + (Vector3.up * hightOverBlock)))
        {
            MovePlayer();
        }
        else
        {
            movementStates = MovementStates.Still;
        }

        UpdateDarkenBlockWhenButtonIsPressed();
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
                endDestination = MainManager.Instance.block_Vertical_InFront.blockPosition + (Vector3.up * hightOverBlock);
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
                endDestination = MainManager.Instance.block_Vertical_InBack.blockPosition + (Vector3.up * hightOverBlock);
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
                endDestination = MainManager.Instance.block_Vertical_ToTheLeft.blockPosition + (Vector3.up * hightOverBlock);
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
                endDestination = MainManager.Instance.block_Vertical_ToTheRight.blockPosition + (Vector3.up * hightOverBlock);
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
        switch (gameObject.GetComponent<PlayerCamera>().cameraState)
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

    void UpdateDarkenBlockWhenButtonIsPressed()
    {
        if (movementStates == MovementStates.Still)
        {
            //If a key is held down, don't show the new darkening blocks
            if (Input.GetKey(KeyCode.W)
                || Input.GetKey(KeyCode.S)
                || Input.GetKey(KeyCode.A)
                || Input.GetKey(KeyCode.D)) { }
            else
            {
                UpdateDarkenBlocks();
            }

            //When a key is pressed up
            if (Input.GetKeyUp(KeyCode.W)
                || Input.GetKeyUp(KeyCode.S)
                || Input.GetKeyUp(KeyCode.A)
                || Input.GetKeyUp(KeyCode.D))
            {
                UpdateDarkenBlocks();
            }
        }
    }

    void UpdateDarkenBlocks()
    {
        //Darken block in front of player
        if (MainManager.Instance.canMove_Forward)
        {
            if (MainManager.Instance.block_Horizontal_InFront.block == null && MainManager.Instance.block_Vertical_InFront.block == null) { }
            else
            {
                if (MainManager.Instance.block_Vertical_InFront.block.GetComponent<BlockInfo>())
                    MainManager.Instance.block_Vertical_InFront.block.GetComponent<BlockInfo>().DarkenColors();
            }
        }

        //Darken block in back of player
        if (MainManager.Instance.canMove_Back)
        {
            if (MainManager.Instance.block_Horizontal_InBack.block == null && MainManager.Instance.block_Vertical_InBack.block == null) { }
            else
            {
                if (MainManager.Instance.block_Vertical_InBack.block.GetComponent<BlockInfo>())
                    MainManager.Instance.block_Vertical_InBack.block.GetComponent<BlockInfo>().DarkenColors();
            }
        }

        //Darken block to the left of player
        if (MainManager.Instance.canMove_Left)
        {
            if (MainManager.Instance.block_Horizontal_ToTheLeft.block == null && MainManager.Instance.block_Vertical_ToTheLeft.block == null) { }
            else
            {
                if (MainManager.Instance.block_Vertical_ToTheLeft.block.GetComponent<BlockInfo>())
                    MainManager.Instance.block_Vertical_ToTheLeft.block.GetComponent<BlockInfo>().DarkenColors();
            }

        }

        //Darken block to the right of player
        if (MainManager.Instance.canMove_Right)
        {
            if (MainManager.Instance.block_Horizontal_ToTheRight.block == null && MainManager.Instance.block_Vertical_ToTheRight.block == null) { }
            else
            {
                if (MainManager.Instance.block_Vertical_ToTheRight.block.GetComponent<BlockInfo>())
                    MainManager.Instance.block_Vertical_ToTheRight.block.GetComponent<BlockInfo>().DarkenColors();
            }
        }

        //Lighten Block standing on
        if (MainManager.Instance.block_StandingOn.block == null) { }
        else
        {
            if (MainManager.Instance.block_StandingOn.block.GetComponent<BlockInfo>())
                MainManager.Instance.block_StandingOn.block.GetComponent<BlockInfo>().RestoreColors();
        }
    }
}

public enum MovementStates
{
    Still,
    Moving
}