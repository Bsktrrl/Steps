using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cameras : Singleton<Cameras>
{
    [Header("Camera State")]
    public CameraState cameraState;
    [SerializeField] CameraState cameraState_BeforeSwitching;

    public Vector3 directionFacing;

    [Header("Camera Zoom")]
    public CameraZoomState zoomState;

    float cameraZoom_ExtraShort = 35;
    float cameraZoom_short = 40;
    float cameraZoom_mid = 50;
    float cameraZoom_long = 60;
    float cameraZoom_ExtraLong = 65;

    [Header("CinemachineVirtualCameras")]
    public CinemachineVirtualCamera camera_Forward;
    public CinemachineVirtualCamera camera_Back;
    public CinemachineVirtualCamera camera_Left;
    public CinemachineVirtualCamera camera_Right;

    [Header("Rotation Stats")]
    public float rotationSpeed = 12;
    public float transitionTimer = 0.75f;
    public float timerCounter = 0;
    public float rotationDistanceMin = 0.25f;
    public bool isRotating = false;

    [Header("Camera Stats")]
    public CameraStats cameraStats_Forward;
    public CameraStats cameraStats_Back;
    public CameraStats cameraStats_Left;
    public CameraStats cameraStats_Right;

    [Serializable]
    public class CameraStats
    {
        public Vector3 startPos;
        public Vector3 startRot;
        public Vector3 startOffset;
    }


    //--------------------


    void Start()
    {
        //SetCameraParameters();

        cameraState = CameraState.Forward;
        directionFacing = Vector3.forward;

        zoomState = CameraZoomState.Mid;
        camera_Forward.m_Lens.FieldOfView = cameraZoom_mid;
        camera_Back.m_Lens.FieldOfView = cameraZoom_mid;
        camera_Left.m_Lens.FieldOfView = cameraZoom_mid;
        camera_Right.m_Lens.FieldOfView = cameraZoom_mid;

        SetActiveCamera_OLD();

        //Set Active/Unactive cameras
        //camera_Forward.gameObject.transform.parent.gameObject.SetActive(true);
        //camera_Back.gameObject.transform.parent.gameObject.SetActive(false);
        //camera_Left.gameObject.transform.parent.gameObject.SetActive(false);
        //camera_Right.gameObject.transform.parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        KeyInputs_OLD();

        //Rotate_Setup();
        //RotateCamera();

        //CameraZoom_Setup();
    }


    //--------------------


    void SetCameraParameters()
    {
        //Forward
        cameraStats_Forward.startPos = camera_Forward.gameObject.transform.position;
        cameraStats_Forward.startRot = new Vector3(50, 0, 0);
        cameraStats_Forward.startOffset = camera_Forward.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

        //Back
        cameraStats_Back.startPos = camera_Back.gameObject.transform.position;
        cameraStats_Back.startRot = new Vector3(50, 180, 0);
        cameraStats_Back.startOffset = camera_Back.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

        //Left
        cameraStats_Left.startPos = camera_Left.gameObject.transform.position;
        cameraStats_Left.startRot = new Vector3(50, 90, 0);
        cameraStats_Left.startOffset = camera_Left.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

        //Right
        cameraStats_Right.startPos = camera_Right.gameObject.transform.position;
        cameraStats_Right.startRot = new Vector3(50, -90, 0);
        cameraStats_Right.startOffset = camera_Right.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
    }

    void Rotate_Setup()
    {
        //Don't be able to switch camera angle before the rotation has been done
        if (isRotating) { return; }

        //Rotate, based on the CameraState
        switch (cameraState)
        {
            case CameraState.Forward:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    cameraState_BeforeSwitching = CameraState.Forward;
                    cameraState = CameraState.Left;
                    timerCounter = 0;
                    isRotating = true;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    cameraState_BeforeSwitching = CameraState.Forward;
                    cameraState = CameraState.Right;
                    timerCounter = 0;
                    isRotating = true;
                }
                break;
            case CameraState.Backward:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    cameraState_BeforeSwitching = CameraState.Backward;
                    cameraState = CameraState.Right;
                    timerCounter = 0;
                    isRotating = true;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    cameraState_BeforeSwitching = CameraState.Backward;
                    cameraState = CameraState.Left;
                    timerCounter = 0;
                    isRotating = true;
                }
                break;
            case CameraState.Left:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    cameraState_BeforeSwitching = CameraState.Left;
                    cameraState = CameraState.Backward;
                    timerCounter = 0;
                    isRotating = true;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    cameraState_BeforeSwitching = CameraState.Left;
                    cameraState = CameraState.Forward;
                    timerCounter = 0;
                    isRotating = true;
                }
                break;
            case CameraState.Right:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    cameraState_BeforeSwitching = CameraState.Right;
                    cameraState = CameraState.Forward;
                    timerCounter = 0;
                    isRotating = true;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    cameraState_BeforeSwitching = CameraState.Right;
                    cameraState = CameraState.Backward;
                    timerCounter = 0;
                    isRotating = true;
                }
                break;

            default:
                break;
        }
    }

    void RotateCamera()
    {
        //Only rotate during rotation
        if (!isRotating) { return; }

        timerCounter += Time.deltaTime;

        // Interpolate position, rotation, and follow offset over time
        switch (cameraState_BeforeSwitching)
        {
            case CameraState.Forward:
                switch (cameraState)
                {
                    case CameraState.Forward:
                        RotateDirection(camera_Forward, cameraStats_Forward, camera_Forward, cameraStats_Forward);
                        break;
                    case CameraState.Backward:
                        RotateDirection(camera_Forward, cameraStats_Forward, camera_Back, cameraStats_Back);
                        break;
                    case CameraState.Left:
                        RotateDirection(camera_Forward, cameraStats_Forward, camera_Left, cameraStats_Left);
                        break;
                    case CameraState.Right:
                        RotateDirection(camera_Forward, cameraStats_Forward, camera_Right, cameraStats_Right);
                        break;

                    default:
                        break;
                }
                break;
            case CameraState.Backward:
                switch (cameraState)
                {
                    case CameraState.Forward:
                        RotateDirection(camera_Back, cameraStats_Back, camera_Forward, cameraStats_Forward);
                        break;
                    case CameraState.Backward:
                        RotateDirection(camera_Back, cameraStats_Back, camera_Back, cameraStats_Back);
                        break;
                    case CameraState.Left:
                        RotateDirection(camera_Back, cameraStats_Back, camera_Left, cameraStats_Left);
                        break;
                    case CameraState.Right:
                        RotateDirection(camera_Back, cameraStats_Back, camera_Right, cameraStats_Right);
                        break;

                    default:
                        break;
                }
                //if (cameraState == CameraState.Left)
                //{
                //    camera_Back.transform.SetPositionAndRotation(Vector3.Lerp(camera_Back.transform.position, cameraStats_Left.startPos, rotationSpeed * Time.deltaTime), Quaternion.Lerp(camera_Back.transform.localRotation, Quaternion.Euler(cameraStats_Left.startRot), rotationSpeed * Time.deltaTime));
                //    camera_Back.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(camera_Back.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, cameraStats_Left.startOffset, rotationSpeed * Time.deltaTime);

                //    //If close enough to the second camera, reset current Camera to startPos
                //    if (Vector3.Distance(camera_Back.transform.position, cameraStats_Left.startPos) <= rotationDistanceMin)
                //    {
                //        camera_Back.transform.SetPositionAndRotation(cameraStats_Back.startPos, Quaternion.Euler(cameraStats_Back.startRot));
                //        camera_Back.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = cameraStats_Back.startOffset;

                //        camera_Right.transform.parent.gameObject.SetActive(true);
                //        camera_Back.transform.parent.gameObject.SetActive(false);

                //        isRotating = false;
                //    }
                //}
                //else if (cameraState == CameraState.Right)
                //{
                //    camera_Back.transform.SetPositionAndRotation(Vector3.Lerp(camera_Back.transform.position, cameraStats_Right.startPos, rotationSpeed * Time.deltaTime), Quaternion.Lerp(camera_Back.transform.localRotation, Quaternion.Euler(cameraStats_Right.startRot), rotationSpeed * Time.deltaTime));
                //    camera_Back.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(camera_Back.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, cameraStats_Right.startOffset, rotationSpeed * Time.deltaTime);

                //    //If close enough to the second camera, reset current Camera to startPos
                //    if (Vector3.Distance(camera_Back.transform.position, cameraStats_Right.startPos) <= rotationDistanceMin)
                //    {
                //        camera_Back.transform.SetPositionAndRotation(cameraStats_Back.startPos, Quaternion.Euler(cameraStats_Back.startRot));
                //        camera_Back.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = cameraStats_Back.startOffset;

                //        camera_Left.transform.parent.gameObject.SetActive(true);
                //        camera_Back.transform.parent.gameObject.SetActive(false);

                //        isRotating = false;
                //    }
                //}
                break;
            case CameraState.Left:
                switch (cameraState)
                {
                    case CameraState.Forward:
                        RotateDirection(camera_Left, cameraStats_Left, camera_Forward, cameraStats_Forward);
                        break;
                    case CameraState.Backward:
                        RotateDirection(camera_Left, cameraStats_Left, camera_Back, cameraStats_Back);
                        break;
                    case CameraState.Left:
                        RotateDirection(camera_Left, cameraStats_Left, camera_Left, cameraStats_Left);
                        break;
                    case CameraState.Right:
                        RotateDirection(camera_Left, cameraStats_Left, camera_Right, cameraStats_Right);
                        break;

                    default:
                        break;
                }
                //if (cameraState == CameraState.Forward)
                //{
                //    camera_Left.transform.SetPositionAndRotation(Vector3.Lerp(camera_Left.transform.position, cameraStats_Forward.startPos, rotationSpeed * Time.deltaTime), Quaternion.Lerp(camera_Left.transform.localRotation, Quaternion.Euler(cameraStats_Forward.startRot), rotationSpeed * Time.deltaTime));
                //    camera_Left.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(camera_Left.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, cameraStats_Forward.startOffset, rotationSpeed * Time.deltaTime);

                //    //If close enough to the second camera, reset current Camera to startPos
                //    if (Vector3.Distance(camera_Left.transform.position, cameraStats_Forward.startPos) <= rotationDistanceMin)
                //    {
                //        camera_Left.transform.SetPositionAndRotation(cameraStats_Left.startPos, Quaternion.Euler(cameraStats_Left.startRot));
                //        camera_Left.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = cameraStats_Left.startOffset;

                //        camera_Forward.transform.parent.gameObject.SetActive(true);
                //        camera_Left.transform.parent.gameObject.SetActive(false);

                //        isRotating = false;
                //    }
                //}
                //else if (cameraState == CameraState.Backward)
                //{
                //    camera_Left.transform.SetPositionAndRotation(Vector3.Lerp(camera_Left.transform.position, cameraStats_Back.startPos, rotationSpeed * Time.deltaTime), Quaternion.Lerp(camera_Left.transform.localRotation, Quaternion.Euler(cameraStats_Back.startRot), rotationSpeed * Time.deltaTime));
                //    camera_Left.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(camera_Left.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, cameraStats_Back.startOffset, rotationSpeed * Time.deltaTime);

                //    //If close enough to the second camera, reset current Camera to startPos
                //    if (Vector3.Distance(camera_Left.transform.position, cameraStats_Back.startPos) <= rotationDistanceMin)
                //    {
                //        camera_Left.transform.SetPositionAndRotation(cameraStats_Left.startPos, Quaternion.Euler(cameraStats_Left.startRot));
                //        camera_Left.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = cameraStats_Left.startOffset;

                //        camera_Back.transform.parent.gameObject.SetActive(true);
                //        camera_Left.transform.parent.gameObject.SetActive(false);

                //        isRotating = false;
                //    }
                //}
                break;
            case CameraState.Right:
                switch (cameraState)
                {
                    case CameraState.Forward:
                        RotateDirection(camera_Right, cameraStats_Right, camera_Forward, cameraStats_Forward);
                        break;
                    case CameraState.Backward:
                        RotateDirection(camera_Right, cameraStats_Right, camera_Back, cameraStats_Back);
                        break;
                    case CameraState.Left:
                        RotateDirection(camera_Right, cameraStats_Right, camera_Left, cameraStats_Left);
                        break;
                    case CameraState.Right:
                        RotateDirection(camera_Right, cameraStats_Right, camera_Right, cameraStats_Right);
                        break;

                    default:
                        break;
                }
                //if (cameraState == CameraState.Forward)
                //{
                //    camera_Right.transform.SetPositionAndRotation(Vector3.Lerp(camera_Right.transform.position, cameraStats_Forward.startPos, rotationSpeed * Time.deltaTime), Quaternion.Lerp(camera_Right.transform.localRotation, Quaternion.Euler(cameraStats_Forward.startRot), rotationSpeed * Time.deltaTime));
                //    camera_Right.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(camera_Right.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, cameraStats_Forward.startOffset, rotationSpeed * Time.deltaTime);

                //    //If close enough to the second camera, reset current Camera to startPos
                //    if (Vector3.Distance(camera_Right.transform.position, cameraStats_Forward.startPos) <= rotationDistanceMin)
                //    {
                //        camera_Right.transform.SetPositionAndRotation(cameraStats_Right.startPos, Quaternion.Euler(cameraStats_Right.startRot));
                //        camera_Right.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = cameraStats_Right.startOffset;

                //        camera_Forward.transform.parent.gameObject.SetActive(true);
                //        camera_Right.transform.parent.gameObject.SetActive(false);

                //        isRotating = false;
                //    }
                //}
                //else if (cameraState == CameraState.Backward)
                //{
                //    camera_Right.transform.SetPositionAndRotation(Vector3.Lerp(camera_Right.transform.position, cameraStats_Back.startPos, rotationSpeed * Time.deltaTime), Quaternion.Lerp(camera_Right.transform.localRotation, Quaternion.Euler(cameraStats_Back.startRot), rotationSpeed * Time.deltaTime));
                //    camera_Right.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(camera_Right.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, cameraStats_Back.startOffset, rotationSpeed * Time.deltaTime);

                //    //If close enough to the second camera, reset current Camera to startPos
                //    if (Vector3.Distance(camera_Right.transform.position, cameraStats_Back.startPos) <= rotationDistanceMin)
                //    {
                //        camera_Right.transform.SetPositionAndRotation(cameraStats_Right.startPos, Quaternion.Euler(cameraStats_Right.startRot));
                //        camera_Right.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = cameraStats_Right.startOffset;

                //        camera_Back.transform.parent.gameObject.SetActive(true);
                //        camera_Right.transform.parent.gameObject.SetActive(false);

                //        isRotating = false;
                //    }
                //}
                break;

            default:
                break;
        }
    }
    void RotateDirection(CinemachineVirtualCamera currentCamera, CameraStats currentCameraStat, CinemachineVirtualCamera transitioningCamera, CameraStats transitioningCameraStat)
    {
        currentCamera.transform.SetPositionAndRotation(Vector3.Lerp(currentCamera.transform.position, transitioningCameraStat.startPos, rotationSpeed * Time.deltaTime), Quaternion.Lerp(currentCamera.transform.localRotation, Quaternion.Euler(transitioningCameraStat.startRot), rotationSpeed * Time.deltaTime));
        currentCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(currentCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, transitioningCameraStat.startOffset, rotationSpeed * Time.deltaTime);

        //If close enough to the second camera, reset current Camera to startPos
        if (Vector3.Distance(currentCamera.transform.position, transitioningCameraStat.startPos) <= rotationDistanceMin
            || timerCounter >= transitionTimer)
        {
            currentCamera.transform.SetPositionAndRotation(currentCameraStat.startPos, Quaternion.Euler(currentCameraStat.startRot));
            currentCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = currentCameraStat.startOffset;

            transitioningCamera.transform.parent.gameObject.SetActive(true);
            currentCamera.transform.parent.gameObject.SetActive(false);

            SetBlockDetectorDirection();

            isRotating = false;
        }
    }
    void SetBlockDetectorDirection()
    {
        switch (cameraState)
        {
            case CameraState.Forward:
                MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 0, 0));
                break;
            case CameraState.Backward:
                MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 180, 0));
                break;
            case CameraState.Left:
                MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 90, 0));
                break;
            case CameraState.Right:
                MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, -90, 0));
                break;

            default:
                break;
        }
    }


    //--------------------


    void CameraZoom_Setup()
    {
        if (MainManager.Instance.pauseGame) { return; }
        if (isRotating) { return; }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            switch (zoomState)
            {
                case CameraZoomState.ExtraShort:
                    zoomState = CameraZoomState.ExtraShort;
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    break;
                case CameraZoomState.Short:
                    zoomState = CameraZoomState.ExtraShort;
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    break;
                case CameraZoomState.Mid:
                    zoomState = CameraZoomState.Short;
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_short;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_short;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_short;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_short;
                    break;
                case CameraZoomState.Long:
                    zoomState = CameraZoomState.Mid;
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_mid;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_mid;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_mid;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_mid;
                    break;
                case CameraZoomState.ExtraLong:
                    zoomState = CameraZoomState.Long;
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_long;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_long;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_long;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_long;
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
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_short;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_short;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_short;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_short;
                    break;
                case CameraZoomState.Short:
                    zoomState = CameraZoomState.Mid;
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_mid;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_mid;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_mid;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_mid;
                    break;
                case CameraZoomState.Mid:
                    zoomState = CameraZoomState.Long;
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_long;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_long;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_long;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_long;
                    break;
                case CameraZoomState.Long:
                    zoomState = CameraZoomState.ExtraLong;
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    break;
                case CameraZoomState.ExtraLong:
                    zoomState = CameraZoomState.ExtraLong;
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    break;

                default:
                    break;
            }
        }
    }


    //--------------------


    void KeyInputs_OLD()
    {
        if (MainManager.Instance.pauseGame) { return; }

        //Rotate Camera
        if (Input.GetKeyDown(KeyCode.LeftArrow))
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

            SetBlockDetectorDirection_OLD();
            SetActiveCamera_OLD();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
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

            SetBlockDetectorDirection_OLD();
            SetActiveCamera_OLD();
        }

        //Zoom
        SetCameraZoom_OLD();
    }

    void SetActiveCamera_OLD()
    {
        switch (cameraState)
        {
            case CameraState.Forward:
                camera_Forward.transform.parent.gameObject.SetActive(true);
                camera_Back.transform.parent.gameObject.SetActive(false);
                camera_Left.transform.parent.gameObject.SetActive(false);
                camera_Right.transform.parent.gameObject.SetActive(false);
                break;
            case CameraState.Backward:
                camera_Forward.transform.parent.gameObject.SetActive(false);
                camera_Back.transform.parent.gameObject.SetActive(true);
                camera_Left.transform.parent.gameObject.SetActive(false);
                camera_Right.transform.parent.gameObject.SetActive(false);
                break;
            case CameraState.Left:
                camera_Forward.transform.parent.gameObject.SetActive(false);
                camera_Back.transform.parent.gameObject.SetActive(false);
                camera_Left.transform.parent.gameObject.SetActive(true);
                camera_Right.transform.parent.gameObject.SetActive(false);
                break;
            case CameraState.Right:
                camera_Forward.transform.parent.gameObject.SetActive(false);
                camera_Back.transform.parent.gameObject.SetActive(false);
                camera_Left.transform.parent.gameObject.SetActive(false);
                camera_Right.transform.parent.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    void SetCameraZoom_OLD()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            switch (zoomState)
            {
                case CameraZoomState.ExtraShort:
                    zoomState = CameraZoomState.ExtraShort;
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    break;
                case CameraZoomState.Short:
                    zoomState = CameraZoomState.ExtraShort;
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_ExtraShort;
                    break;
                case CameraZoomState.Mid:
                    zoomState = CameraZoomState.Short;
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_short;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_short;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_short;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_short;
                    break;
                case CameraZoomState.Long:
                    zoomState = CameraZoomState.Mid;
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_mid;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_mid;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_mid;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_mid;
                    break;
                case CameraZoomState.ExtraLong:
                    zoomState = CameraZoomState.Long;
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_long;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_long;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_long;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_long;
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
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_short;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_short;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_short;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_short;
                    break;
                case CameraZoomState.Short:
                    zoomState = CameraZoomState.Mid;
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_mid;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_mid;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_mid;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_mid;
                    break;
                case CameraZoomState.Mid:
                    zoomState = CameraZoomState.Long;
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_long;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_long;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_long;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_long;
                    break;
                case CameraZoomState.Long:
                    zoomState = CameraZoomState.ExtraLong;
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    break;
                case CameraZoomState.ExtraLong:
                    zoomState = CameraZoomState.ExtraLong;
                    camera_Forward.m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    camera_Back.m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    camera_Left.m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    camera_Right.m_Lens.FieldOfView = cameraZoom_ExtraLong;
                    break;

                default:
                    break;
            }
        }
    }

    void SetBlockDetectorDirection_OLD()
    {
        switch (cameraState)
        {
            case CameraState.Forward:
                MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 0, 0));
                break;
            case CameraState.Backward:
                MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 180, 0));
                break;
            case CameraState.Left:
                MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 90, 0));
                break;
            case CameraState.Right:
                MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, -90, 0));
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