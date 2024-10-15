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
    [SerializeField] string directionTranslator;

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
    public float transitionTimer = 0.25f;
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
        SetCameraParameters();

        cameraState = CameraState.Forward;
        directionFacing = Vector3.forward;

        zoomState = CameraZoomState.Mid;
        camera_Forward.m_Lens.FieldOfView = cameraZoom_mid;
        camera_Back.m_Lens.FieldOfView = cameraZoom_mid;
        camera_Left.m_Lens.FieldOfView = cameraZoom_mid;
        camera_Right.m_Lens.FieldOfView = cameraZoom_mid;

        //Set Active/Unactive cameras
        camera_Forward.gameObject.transform.parent.gameObject.SetActive(true);
        camera_Back.gameObject.transform.parent.gameObject.SetActive(false);
        camera_Left.gameObject.transform.parent.gameObject.SetActive(false);
        camera_Right.gameObject.transform.parent.gameObject.SetActive(false);

        AdjustFacingDirection();
    }

    private void Update()
    {
        Rotate_Setup();
        RotateCamera();

        CameraZoom_Setup();

        TranslatingDirection();
    }


    //--------------------


    void SetCameraParameters()
    {
        //Forward
        cameraStats_Forward.startPos = camera_Forward.gameObject.transform.position;
        cameraStats_Forward.startRot = new Vector3(50, 0, 0);
        cameraStats_Forward.startOffset = camera_Forward.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

        camera_Forward.transform.localRotation = Quaternion.Euler(cameraStats_Forward.startRot);

        //Back
        cameraStats_Back.startPos = camera_Back.gameObject.transform.position;
        cameraStats_Back.startRot = new Vector3(50, 180, 0);
        cameraStats_Back.startOffset = camera_Back.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

        camera_Back.transform.localRotation = Quaternion.Euler(cameraStats_Back.startRot);

        //Left
        cameraStats_Left.startPos = camera_Left.gameObject.transform.position;
        cameraStats_Left.startRot = new Vector3(50, 90, 0);
        cameraStats_Left.startOffset = camera_Left.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

        camera_Left.transform.localRotation = Quaternion.Euler(cameraStats_Left.startRot);

        //Right
        cameraStats_Right.startPos = camera_Right.gameObject.transform.position;
        cameraStats_Right.startRot = new Vector3(50, -90, 0);
        cameraStats_Right.startOffset = camera_Right.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

        camera_Right.transform.localRotation = Quaternion.Euler(cameraStats_Right.startRot);
    }

    void Rotate_Setup()
    {
        //Don't be able to switch camera angle before the rotation has been done
        if (isRotating) { return; }
        if (Player_Interact.Instance.isInteracting) { return; }

        //Rotate Camera
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            switch (cameraState)
            {
                case CameraState.Forward:
                    cameraState_BeforeSwitching = CameraState.Forward;
                    cameraState = CameraState.Left;
                    break;
                case CameraState.Backward:
                    cameraState_BeforeSwitching = CameraState.Backward;
                    cameraState = CameraState.Right;
                    break;
                case CameraState.Left:
                    cameraState_BeforeSwitching = CameraState.Left;
                    cameraState = CameraState.Backward;
                    break;
                case CameraState.Right:
                    cameraState_BeforeSwitching = CameraState.Right;
                    cameraState = CameraState.Forward;
                    break;
                default:
                    break;
            }

            timerCounter = 0;
            isRotating = true;

            SetBlockDetectorDirection();

            //Adjust Facing Direction
            AdjustFacingDirection();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            switch (cameraState)
            {
                case CameraState.Forward:
                    cameraState_BeforeSwitching = CameraState.Forward;
                    cameraState = CameraState.Right;
                    break;
                case CameraState.Backward:
                    cameraState_BeforeSwitching = CameraState.Backward;
                    cameraState = CameraState.Left;
                    break;
                case CameraState.Left:
                    cameraState_BeforeSwitching = CameraState.Left;
                    cameraState = CameraState.Forward;
                    break;
                case CameraState.Right:
                    cameraState_BeforeSwitching = CameraState.Right;
                    cameraState = CameraState.Backward;
                    break;
                default:
                    break;
            }

            timerCounter = 0;
            isRotating = true;

            SetBlockDetectorDirection();

            //Adjust Facing Direction
            AdjustFacingDirection();
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
                    case CameraState.Left:
                        //Forward -> Left: 
                        RotateDirection(camera_Forward, cameraStats_Forward, camera_Left, cameraStats_Left);
                        break;
                    case CameraState.Right:
                        //Forward -> Right
                        RotateDirection(camera_Forward, cameraStats_Forward, camera_Right, cameraStats_Right);
                        break;

                    default:
                        break;
                }
                break;
            case CameraState.Backward:
                switch (cameraState)
                {
                    case CameraState.Left:
                        //Back -> Left
                        RotateDirection(camera_Back, cameraStats_Back, camera_Left, cameraStats_Left);
                        break;
                    case CameraState.Right:
                        //Back -> Right
                        RotateDirection(camera_Back, cameraStats_Back, camera_Right, cameraStats_Right);
                        break;

                    default:
                        break;
                }
                break;
            case CameraState.Left:
                switch (cameraState)
                {
                    case CameraState.Forward:
                        //Left -> Forward
                        RotateDirection(camera_Left, cameraStats_Left, camera_Forward, cameraStats_Forward);
                        break;
                    case CameraState.Backward:
                        //Left -> Back
                        RotateDirection(camera_Left, cameraStats_Left, camera_Back, cameraStats_Back);
                        break;

                    default:
                        break;
                }
                break;
            case CameraState.Right:
                switch (cameraState)
                {
                    case CameraState.Forward:
                        //Right -> Forward
                        RotateDirection(camera_Right, cameraStats_Right, camera_Forward, cameraStats_Forward);
                        break;
                    case CameraState.Backward:
                        //Right -> Back
                        RotateDirection(camera_Right, cameraStats_Right, camera_Back, cameraStats_Back);
                        break;

                    default:
                        break;
                }
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
                //if (directionFacing == Vector3.forward)
                //    directionFacing = Vector3.forward;
                //else if (directionFacing == Vector3.back)
                //    directionFacing = Vector3.back;
                //else if (directionFacing == Vector3.left)
                //    directionFacing = Vector3.left;
                //else if (directionFacing == Vector3.right)
                //    directionFacing = Vector3.right;
                break;
            case CameraState.Backward:
                MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 180, 0));
                //if (directionFacing == Vector3.forward)
                //    directionFacing = Vector3.back;
                //else if (directionFacing == Vector3.back)
                //    directionFacing = Vector3.forward;
                //else if (directionFacing == Vector3.left)
                //    directionFacing = Vector3.right;
                //else if (directionFacing == Vector3.right)
                //    directionFacing = Vector3.left;
                break;
            case CameraState.Left:
                MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 90, 0));
                //if (directionFacing == Vector3.forward)
                //    directionFacing = Vector3.left;
                //else if (directionFacing == Vector3.back)
                //    directionFacing = Vector3.right;
                //else if (directionFacing == Vector3.left)
                //    directionFacing = Vector3.forward;
                //else if (directionFacing == Vector3.right)
                //    directionFacing = Vector3.back;
                break;
            case CameraState.Right:
                MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(MainManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, -90, 0));
                //if (directionFacing == Vector3.forward)
                //    directionFacing = Vector3.right;
                //else if (directionFacing == Vector3.back)
                //    directionFacing = Vector3.left;
                //else if (directionFacing == Vector3.left)
                //    directionFacing = Vector3.back;
                //else if (directionFacing == Vector3.right)
                //    directionFacing = Vector3.forward;
                break;

            default:
                break;
        }
    }
    void AdjustFacingDirection()
    {
        switch (cameraState)
        {
            case CameraState.Forward:
                if (MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
                {
                    directionFacing = Vector3.forward;
                }
                else if (MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 180, 0))
                        || MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -180, 0)))
                {
                    directionFacing = Vector3.back;
                }
                else if (MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 90, 0)))
                {
                    directionFacing = Vector3.right;
                }
                else if (MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 270, 0))
                        || MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -90, 0)))
                {
                    directionFacing = Vector3.left;
                }
                else
                {
                    directionFacing = Vector3.forward;
                }
                break;
            case CameraState.Backward:
                if (MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
                {
                    directionFacing = Vector3.back;
                }
                else if (MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 180, 0))
                        || MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -180, 0)))
                {
                    directionFacing = Vector3.forward;
                }
                else if (MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 90, 0)))
                {
                    directionFacing = Vector3.left;
                }
                else if (MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 270, 0))
                        || MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -90, 0)))
                {
                    directionFacing = Vector3.right;
                }
                else
                {
                    directionFacing = Vector3.back;
                }
                break;
            case CameraState.Left:
                if (MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
                {
                    directionFacing = Vector3.left;
                }
                else if (MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 180, 0))
                        || MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -180, 0)))
                {
                    directionFacing = Vector3.right;
                }
                else if (MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 90, 0)))
                {
                    directionFacing = Vector3.forward;
                }
                else if (MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 270, 0))
                        || MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -90, 0)))
                {
                    directionFacing = Vector3.back;
                }
                else
                {
                    directionFacing = Vector3.left;
                }
                break;
            case CameraState.Right:
                if (MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
                {
                    directionFacing = Vector3.right;
                }
                else if (MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 180, 0))
                        || MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -180, 0)))
                {
                    directionFacing = Vector3.left;
                }
                else if (MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 90, 0)))
                {
                    directionFacing = Vector3.back;
                }
                else if (MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 270, 0))
                        || MainManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -90, 0)))
                {
                    directionFacing = Vector3.forward;
                }
                else
                {
                    directionFacing = Vector3.right;
                }
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


    //--------------------


    void TranslatingDirection()
    {
        if (directionFacing == Vector3.forward)
            directionTranslator = "Forward";
        else if (directionFacing == Vector3.back)
            directionTranslator = "Back";
        else if (directionFacing == Vector3.left)
            directionTranslator = "Left";
        else if (directionFacing == Vector3.right)
            directionTranslator = "Right";
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