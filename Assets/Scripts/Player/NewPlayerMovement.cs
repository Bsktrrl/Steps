using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    public MovementStates movementStates;

    float waitTime = 0.15f;

    Vector3 endDestination;
    [SerializeField] float playerSpeed = 7;


    //--------------------


    private void Update()
    {
        NewKeyInputs();

        if (movementStates == MovementStates.Moving)
        {
            MovePlayer();
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
                endDestination = MainManager.Instance.block_Vertical_InFront.blockPosition + (Vector3.up * 0.85f);
                //MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.block_Vertical_InFront.blockPosition + (Vector3.up * 0.85f), MainManager.Instance.player.transform.rotation);
                SetPlayerBodyRotation(0);
                movementStates = MovementStates.Moving;
            }
            else
            {
                SetPlayerBodyRotation(0);
            }

            //StartCoroutine(MovementWaitTime(waitTime));
        }

        //If pressing DOWN
        else if (Input.GetKey(KeyCode.S))
        {
            if (MainManager.Instance.canMove_Back)
            {
                endDestination = MainManager.Instance.block_Vertical_InBack.blockPosition + (Vector3.up * 0.85f);
                //MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.block_Vertical_InBack.blockPosition + (Vector3.up * 0.85f), MainManager.Instance.player.transform.rotation);
                SetPlayerBodyRotation(180);
                movementStates = MovementStates.Moving;
            }
            else
            {
                SetPlayerBodyRotation(180);
            }

            //StartCoroutine(MovementWaitTime(waitTime));
        }

        //If pressing LEFT
        else if (Input.GetKey(KeyCode.A))
        {
            if (MainManager.Instance.canMove_Left)
            {
                endDestination = MainManager.Instance.block_Vertical_ToTheLeft.blockPosition + (Vector3.up * 0.85f);
                //MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.block_Vertical_ToTheLeft.blockPosition + (Vector3.up * 0.85f), MainManager.Instance.player.transform.rotation);
                SetPlayerBodyRotation(-90);
                movementStates = MovementStates.Moving;
            }
            else
            {
                SetPlayerBodyRotation(-90);
            }

            //StartCoroutine(MovementWaitTime(waitTime));
        }

        //If pressing RIGHT
        else if (Input.GetKey(KeyCode.D))
        {
            if (MainManager.Instance.canMove_Right)
            {
                endDestination = MainManager.Instance.block_Vertical_ToTheRight.blockPosition + (Vector3.up * 0.85f);
                //MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.block_Vertical_ToTheRight.blockPosition + (Vector3.up * 0.85f), MainManager.Instance.player.transform.rotation);
                SetPlayerBodyRotation(90);
                movementStates = MovementStates.Moving;
            }
            else
            {
                SetPlayerBodyRotation(90);
            }

            //StartCoroutine(MovementWaitTime(waitTime));
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
        }
    }
}

public enum MovementStates
{
    Still,
    Moving
}