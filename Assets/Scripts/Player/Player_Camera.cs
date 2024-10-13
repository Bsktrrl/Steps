using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : Singleton<Player_Camera>
{
    [Header("Camera State")]
    public CameraState cameraState;
    public Vector3 directionFacing;

    public CameraZoomState zoomState;

    float cameraZoom_ExtraShort = 35;
    float cameraZoom_short = 40;
    float cameraZoom_mid = 50;
    float cameraZoom_long = 60;
    float cameraZoom_ExtraLong = 65;


    //--------------------


    private void Start()
    {
        cameraState = CameraState.Forward;
        directionFacing = Vector3.forward;

        zoomState = CameraZoomState.Mid;
        MainManager.Instance.camera_Forward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_mid;
        MainManager.Instance.camera_Backward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_mid;
        MainManager.Instance.camera_Left.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_mid;
        MainManager.Instance.camera_Right.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_mid;

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

        //Rotate Camera
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

            SetBlockDetectorDirection();
            SetActiveCamera();
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
            
            SetBlockDetectorDirection();
            SetActiveCamera();
        }

        //Zoom
        SetCameraZoom();
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

    void SetCameraZoom()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            switch (zoomState)
            {
                case CameraZoomState.ExtraShort:
                    zoomState = CameraZoomState.ExtraShort;
                    MainManager.Instance.camera_Forward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    MainManager.Instance.camera_Backward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    MainManager.Instance.camera_Left.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    MainManager.Instance.camera_Right.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    break;
                case CameraZoomState.Short:
                    zoomState = CameraZoomState.ExtraShort;
                    MainManager.Instance.camera_Forward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    MainManager.Instance.camera_Backward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    MainManager.Instance.camera_Left.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    MainManager.Instance.camera_Right.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    break;
                case CameraZoomState.Mid:
                    zoomState = CameraZoomState.Short;
                    MainManager.Instance.camera_Forward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_short;
                    MainManager.Instance.camera_Backward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_short;
                    MainManager.Instance.camera_Left.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_short;
                    MainManager.Instance.camera_Right.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_short;
                    break;
                case CameraZoomState.Long:
                    zoomState = CameraZoomState.Mid;
                    MainManager.Instance.camera_Forward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_mid;
                    MainManager.Instance.camera_Backward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_mid;
                    MainManager.Instance.camera_Left.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_mid;
                    MainManager.Instance.camera_Right.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_mid;
                    break;
                case CameraZoomState.ExtraLong:
                    zoomState = CameraZoomState.Long;
                    MainManager.Instance.camera_Forward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_long;
                    MainManager.Instance.camera_Backward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_long;
                    MainManager.Instance.camera_Left.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_long;
                    MainManager.Instance.camera_Right.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_long;
                    break;

                default:
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            switch (zoomState)
            {
                case CameraZoomState.ExtraShort:
                    zoomState = CameraZoomState.Short;
                    MainManager.Instance.camera_Forward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_short;
                    MainManager.Instance.camera_Backward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_short;
                    MainManager.Instance.camera_Left.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_short;
                    MainManager.Instance.camera_Right.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_short;
                    break;
                case CameraZoomState.Short:
                    zoomState = CameraZoomState.Mid;
                    MainManager.Instance.camera_Forward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_mid;
                    MainManager.Instance.camera_Backward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_mid;
                    MainManager.Instance.camera_Left.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_mid;
                    MainManager.Instance.camera_Right.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_mid;
                    break;
                case CameraZoomState.Mid:
                    zoomState = CameraZoomState.Long;
                    MainManager.Instance.camera_Forward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_long;
                    MainManager.Instance.camera_Backward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_long;
                    MainManager.Instance.camera_Left.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_long;
                    MainManager.Instance.camera_Right.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_long;
                    break;
                case CameraZoomState.Long:
                    zoomState = CameraZoomState.ExtraLong;
                    MainManager.Instance.camera_Forward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    MainManager.Instance.camera_Backward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    MainManager.Instance.camera_Left.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    MainManager.Instance.camera_Right.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    break;
                case CameraZoomState.ExtraLong:
                    zoomState = CameraZoomState.ExtraLong;
                    MainManager.Instance.camera_Forward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    MainManager.Instance.camera_Backward.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    MainManager.Instance.camera_Left.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    MainManager.Instance.camera_Right.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    break;

                default:
                    break;
            }
        }
    }

    void SetBlockDetectorDirection()
    {
        switch (cameraState)
        {
            case CameraState.Forward:
                gameObject.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(gameObject.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 0, 0));
                break;
            case CameraState.Backward:
                gameObject.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(gameObject.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 180, 0));
                break;
            case CameraState.Left:
                gameObject.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(gameObject.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, -90, 0));
                break;
            case CameraState.Right:
                gameObject.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(gameObject.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 90, 0));
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

public enum CameraZoomState
{
    ExtraShort,
    Short,
    Mid,
    Long,
    ExtraLong
}