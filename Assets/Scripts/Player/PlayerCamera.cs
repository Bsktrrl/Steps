using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera State")]
    public CameraState cameraState;
    public Vector3 directionFacing;


    //--------------------


    private void Start()
    {
        cameraState = CameraState.Forward;
        SetActiveCamera();
    }
    private void Update()
    {
        KeyInputs();
    }


    //--------------------


    void KeyInputs()
    {
        if (MainManager.Instance.pauseGame) { return; }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            switch (cameraState)
            {
                case CameraState.Forward:
                    cameraState = CameraState.Right;
                    break;
                case CameraState.Backward:
                    cameraState = CameraState.Left;
                    break;
                case CameraState.Left:
                    cameraState = CameraState.Forward;
                    break;
                case CameraState.Right:
                    cameraState = CameraState.Backward;
                    break;
                default:
                    break;
            }

            SetActiveCamera();
            //SetPlayerRotation();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            switch (cameraState)
            {
                case CameraState.Forward:
                    cameraState = CameraState.Left;
                    break;
                case CameraState.Backward:
                    cameraState = CameraState.Right;
                    break;
                case CameraState.Left:
                    cameraState = CameraState.Backward;
                    break;
                case CameraState.Right:
                    cameraState = CameraState.Forward;
                    break;
                default:
                    break;
            }

            SetActiveCamera();
            //SetPlayerRotation();
        }
    }

    void SetActiveCamera()
    {
        switch (cameraState)
        {
            case CameraState.Forward:
                MainManager.Instance.camera_Forward.SetActive(true);
                MainManager.Instance.camera_Backward.SetActive(false);
                MainManager.Instance.camera_Left.SetActive(false);
                MainManager.Instance.camera_Right.SetActive(false);
                break;
            case CameraState.Backward:
                MainManager.Instance.camera_Forward.SetActive(false);
                MainManager.Instance.camera_Backward.SetActive(true);
                MainManager.Instance.camera_Left.SetActive(false);
                MainManager.Instance.camera_Right.SetActive(false);
                break;
            case CameraState.Left:
                MainManager.Instance.camera_Forward.SetActive(false);
                MainManager.Instance.camera_Backward.SetActive(false);
                MainManager.Instance.camera_Left.SetActive(true);
                MainManager.Instance.camera_Right.SetActive(false);
                break;
            case CameraState.Right:
                MainManager.Instance.camera_Forward.SetActive(false);
                MainManager.Instance.camera_Backward.SetActive(false);
                MainManager.Instance.camera_Left.SetActive(false);
                MainManager.Instance.camera_Right.SetActive(true);
                break;
            default:
                break;
        }
    }

    void SetPlayerRotation()
    {
        switch (gameObject.GetComponent<PlayerCamera>().cameraState)
        {
            case CameraState.Forward:
                MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, 0, 0));
                break;
            case CameraState.Backward:
                MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, 180, 0));
                break;
            case CameraState.Left:
                MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, -90, 0));
                break;
            case CameraState.Right:
                MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position, Quaternion.Euler(0, 90, 0));
                break;
            default:
                break;
        }
    }
}

public enum CameraState
{
    Forward,
    Backward,
    Left,
    Right
}