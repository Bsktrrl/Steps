using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{

    public MovementStates movementStates;


    //--------------------


    private void Update()
    {
        KeyInputs();
    }


    //--------------------


    void KeyInputs()
    {
        if (movementStates == MovementStates.Moving) { return; }
        if (MainManager.Instance.pauseGame) { return; }

        if (Input.GetKeyDown(KeyCode.W))
        {
            //if (PlayerStats.Instance.stats.steps_Current <= 0)
            //{
            //    movementStates = MovementStates.Moving;
            //    PlayerStepCost.Instance.RefillSteps();

            //    return;
            //}

            if (MainManager.Instance.canMove_Forward)
            {
                switch (gameObject.GetComponent<PlayerCamera>().cameraState)
                {
                    case CameraState.Forward:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.forward * 1, Quaternion.Euler(0, 0, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.forward;
                        break;
                    case CameraState.Backward:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.back * 1, Quaternion.Euler(0, 180, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.back;
                        break;
                    case CameraState.Left:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.left * 1, Quaternion.Euler(0, -90, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.left;
                        break;
                    case CameraState.Right:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.right * 1, Quaternion.Euler(0, 90, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.right;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (gameObject.GetComponent<PlayerCamera>().cameraState)
                {
                    case CameraState.Forward:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, 0, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.forward;
                        break;
                    case CameraState.Backward:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, 180, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.back;
                        break;
                    case CameraState.Left:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, -90, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.left;
                        break;
                    case CameraState.Right:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, 90, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.right;
                        break;
                    default:
                        break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //if (PlayerStats.Instance.stats.steps_Current <= 0)
            //{
            //    movementStates = MovementStates.Moving;
            //    PlayerStepCost.Instance.RefillSteps();

            //    return;
            //}

            if (MainManager.Instance.canMove_Back)
            {
                switch (gameObject.GetComponent<PlayerCamera>().cameraState)
                {
                    case CameraState.Forward:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.back * 1, Quaternion.Euler(0, 180, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.back;
                        break;
                    case CameraState.Backward:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.forward * 1, Quaternion.Euler(0, 0, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.forward;
                        break;
                    case CameraState.Left:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.right * 1, Quaternion.Euler(0, 90, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.right;
                        break;
                    case CameraState.Right:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.left * 1, Quaternion.Euler(0, -90, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.left;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (gameObject.GetComponent<PlayerCamera>().cameraState)
                {
                    case CameraState.Forward:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, 180, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.back;
                        break;
                    case CameraState.Backward:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, 0, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.forward;
                        break;
                    case CameraState.Left:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, 90, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.right;
                        break;
                    case CameraState.Right:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, -90, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.left;
                        break;
                    default:
                        break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            //if (PlayerStats.Instance.stats.steps_Current <= 0)
            //{
            //    movementStates = MovementStates.Moving;
            //    PlayerStepCost.Instance.RefillSteps();

            //    return;
            //}

            if (MainManager.Instance.canMove_Left)
            {
                switch (gameObject.GetComponent<PlayerCamera>().cameraState)
                {
                    case CameraState.Forward:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.left * 1, Quaternion.Euler(0, -90, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.left;
                        break;
                    case CameraState.Backward:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.right * 1, Quaternion.Euler(0, 90, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.right;
                        break;
                    case CameraState.Left:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.back * 1, Quaternion.Euler(0, 180, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.back;
                        break;
                    case CameraState.Right:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.forward * 1, Quaternion.Euler(0, 0, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.forward;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (gameObject.GetComponent<PlayerCamera>().cameraState)
                {
                    case CameraState.Forward:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, -90, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.left;
                        break;
                    case CameraState.Backward:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, 90, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.right;
                        break;
                    case CameraState.Left:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, 180, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.back;
                        break;
                    case CameraState.Right:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, 0, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.forward;
                        break;
                    default:
                        break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            //if (PlayerStats.Instance.stats.steps_Current <= 0)
            //{
            //    movementStates = MovementStates.Moving;
            //    PlayerStepCost.Instance.RefillSteps();

            //    return;
            //}

            if (MainManager.Instance.canMove_Right)
            {
                switch (gameObject.GetComponent<PlayerCamera>().cameraState)
                {
                    case CameraState.Forward:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.right * 1, Quaternion.Euler(0, 90, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.right;
                        break;
                    case CameraState.Backward:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.left * 1, Quaternion.Euler(0, -90, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.left;
                        break;
                    case CameraState.Left:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.forward * 1, Quaternion.Euler(0, 0, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.forward;
                        break;
                    case CameraState.Right:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.back * 1, Quaternion.Euler(0, 180, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.back;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (gameObject.GetComponent<PlayerCamera>().cameraState)
                {
                    case CameraState.Forward:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, 90, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.right;
                        break;
                    case CameraState.Backward:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, -90, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.left;
                        break;
                    case CameraState.Left:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, 0, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.forward;
                        break;
                    case CameraState.Right:
                        MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, 180, 0));
                        gameObject.GetComponent<PlayerCamera>().directionFacing = Vector3.back;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

public enum MovementStates
{
    Still,
    Moving
}