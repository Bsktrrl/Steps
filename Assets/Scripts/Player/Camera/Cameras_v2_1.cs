using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cameras_v1 : Singleton<Cameras_v1>
{
    //public static event Action rotateCamera;

    [Header("Camera State")]
    //public CameraState cameraState;
    [SerializeField] CameraState cameraState_BeforeSwitching;

    public Vector3 directionFacing;
    [SerializeField] string directionTranslator;

    [Header("Camera Zoom")]
    [SerializeField] float zoomScrollValue_Base = 60;
    [SerializeField] float zoomScrollValue_Current;
    [SerializeField] float zoomScrollSpeed = 8;
    [SerializeField] float zoomScrollValue_Min = 45;
    [SerializeField] float zoomScrollValue_Max = 75;

    [Header("CinemachineVirtualCameras")]
    public CinemachineVirtualCamera camera_Forward;
    public CinemachineVirtualCamera camera_Back;
    public CinemachineVirtualCamera camera_Left;
    public CinemachineVirtualCamera camera_Right;

    [Header("Rotation Stats")]
    public float rotationSpeed = 12;
    public float transitionTimer = 0.25f;
    public float timerCounter = 0;
    public float rotationDistanceMin = 0.15f;
    float cameraStatRot_X = 27;
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

        public Vector3 parentPos;
        public Vector3 parentRot;
    }


    ////--------------------


    //void Start()
    //{
    //    SetCameraParameters();

    //    cameraState = CameraState.Forward;
    //    directionFacing = Vector3.forward;

    //    zoomScrollValue_Current = zoomScrollValue_Base;
    //    camera_Forward.m_Lens.FieldOfView = zoomScrollValue_Current;
    //    camera_Back.m_Lens.FieldOfView = zoomScrollValue_Current;
    //    camera_Left.m_Lens.FieldOfView = zoomScrollValue_Current;
    //    camera_Right.m_Lens.FieldOfView = zoomScrollValue_Current;

    //    //Set Active/Unactive cameras
    //    camera_Forward.gameObject.transform.parent.gameObject.SetActive(true);
    //    camera_Back.gameObject.transform.parent.gameObject.SetActive(false);
    //    camera_Left.gameObject.transform.parent.gameObject.SetActive(false);
    //    camera_Right.gameObject.transform.parent.gameObject.SetActive(false);

    //    AdjustFacingDirection();
    //}

    //private void Update()
    //{
    //    Rotate_Setup();
    //    RotateCamera();

    //    CameraZoom_Setup();

    //    TranslatingDirection();
    //}


    ////--------------------


    //void SetCameraParameters()
    //{
    //    //Forward
    //    cameraStats_Forward.startPos = camera_Forward.gameObject.transform.position;
    //    cameraStats_Forward.startRot = new Vector3(cameraStatRot_X, 0, 0);
    //    cameraStats_Forward.startOffset = camera_Forward.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

    //    camera_Forward.transform.localRotation = Quaternion.Euler(cameraStats_Forward.startRot);

    //    //Back
    //    cameraStats_Back.startPos = camera_Back.gameObject.transform.position;
    //    cameraStats_Back.startRot = new Vector3(cameraStatRot_X, 180, 0);
    //    cameraStats_Back.startOffset = camera_Back.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

    //    camera_Back.transform.localRotation = Quaternion.Euler(cameraStats_Back.startRot);

    //    //Left
    //    cameraStats_Left.startPos = camera_Left.gameObject.transform.position;
    //    cameraStats_Left.startRot = new Vector3(cameraStatRot_X, 90, 0);
    //    cameraStats_Left.startOffset = camera_Left.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

    //    camera_Left.transform.localRotation = Quaternion.Euler(cameraStats_Left.startRot);

    //    //Right
    //    cameraStats_Right.startPos = camera_Right.gameObject.transform.position;
    //    cameraStats_Right.startRot = new Vector3(cameraStatRot_X, -90, 0);
    //    cameraStats_Right.startOffset = camera_Right.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

    //    camera_Right.transform.localRotation = Quaternion.Euler(cameraStats_Right.startRot);
    //}

    //void Rotate_Setup()
    //{
    //    //Don't be able to switch camera angle before the rotation has been done
    //    if (isRotating) { return; }
    //    if (Player_Interact.Instance.isInteracting) { return; }
    //    if (Player_Movement.Instance.iceGliding) { return; }
    //    if (Player_Movement.Instance.movementStates == MovementStates.Moving) { return; }
    //    if (Player_Movement.Instance.ladderMovement_Top_ToBlock) { return; }

    //    //Rotate Camera
    //    if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        switch (cameraState)
    //        {
    //            case CameraState.Forward:
    //                cameraState_BeforeSwitching = CameraState.Forward;
    //                cameraState = CameraState.Left;
    //                break;
    //            case CameraState.Backward:
    //                cameraState_BeforeSwitching = CameraState.Backward;
    //                cameraState = CameraState.Right;
    //                break;
    //            case CameraState.Left:
    //                cameraState_BeforeSwitching = CameraState.Left;
    //                cameraState = CameraState.Backward;
    //                break;
    //            case CameraState.Right:
    //                cameraState_BeforeSwitching = CameraState.Right;
    //                cameraState = CameraState.Forward;
    //                break;
    //            default:
    //                break;
    //        }

    //        timerCounter = 0;
    //        isRotating = true;

    //        SetBlockDetectorDirection();

    //        //Adjust Facing Direction
    //        AdjustFacingDirection();

    //        rotateCamera?.Invoke();
    //    }
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        switch (cameraState)
    //        {
    //            case CameraState.Forward:
    //                cameraState_BeforeSwitching = CameraState.Forward;
    //                cameraState = CameraState.Right;
    //                break;
    //            case CameraState.Backward:
    //                cameraState_BeforeSwitching = CameraState.Backward;
    //                cameraState = CameraState.Left;
    //                break;
    //            case CameraState.Left:
    //                cameraState_BeforeSwitching = CameraState.Left;
    //                cameraState = CameraState.Forward;
    //                break;
    //            case CameraState.Right:
    //                cameraState_BeforeSwitching = CameraState.Right;
    //                cameraState = CameraState.Backward;
    //                break;
    //            default:
    //                break;
    //        }

    //        timerCounter = 0;
    //        isRotating = true;

    //        SetBlockDetectorDirection();

    //        //Adjust Facing Direction
    //        AdjustFacingDirection();

    //        rotateCamera?.Invoke();
    //    }
    //}
    //void RotateCamera()
    //{
    //    //Only rotate during rotation
    //    if (!isRotating) { return; }

    //    timerCounter += Time.deltaTime;

    //    // Interpolate position, rotation, and follow offset over time
    //    switch (cameraState_BeforeSwitching)
    //    {
    //        case CameraState.Forward:
    //            switch (cameraState)
    //            {
    //                case CameraState.Left:
    //                    //Forward -> Left: 
    //                    RotateDirection(camera_Forward, cameraStats_Forward, camera_Left, cameraStats_Left);
    //                    break;
    //                case CameraState.Right:
    //                    //Forward -> Right
    //                    RotateDirection(camera_Forward, cameraStats_Forward, camera_Right, cameraStats_Right);
    //                    break;

    //                default:
    //                    break;
    //            }
    //            break;
    //        case CameraState.Backward:
    //            switch (cameraState)
    //            {
    //                case CameraState.Left:
    //                    //Back -> Left
    //                    RotateDirection(camera_Back, cameraStats_Back, camera_Left, cameraStats_Left);
    //                    break;
    //                case CameraState.Right:
    //                    //Back -> Right
    //                    RotateDirection(camera_Back, cameraStats_Back, camera_Right, cameraStats_Right);
    //                    break;

    //                default:
    //                    break;
    //            }
    //            break;
    //        case CameraState.Left:
    //            switch (cameraState)
    //            {
    //                case CameraState.Forward:
    //                    //Left -> Forward
    //                    RotateDirection(camera_Left, cameraStats_Left, camera_Forward, cameraStats_Forward);
    //                    break;
    //                case CameraState.Backward:
    //                    //Left -> Back
    //                    RotateDirection(camera_Left, cameraStats_Left, camera_Back, cameraStats_Back);
    //                    break;

    //                default:
    //                    break;
    //            }
    //            break;
    //        case CameraState.Right:
    //            switch (cameraState)
    //            {
    //                case CameraState.Forward:
    //                    //Right -> Forward
    //                    RotateDirection(camera_Right, cameraStats_Right, camera_Forward, cameraStats_Forward);
    //                    break;
    //                case CameraState.Backward:
    //                    //Right -> Back
    //                    RotateDirection(camera_Right, cameraStats_Right, camera_Back, cameraStats_Back);
    //                    break;

    //                default:
    //                    break;
    //            }
    //            break;

    //        default:
    //            break;
    //    }
    //}
    //void RotateDirection(CinemachineVirtualCamera currentCamera, CameraStats currentCameraStat, CinemachineVirtualCamera transitioningCamera, CameraStats transitioningCameraStat)
    //{
    //    currentCamera.transform.SetPositionAndRotation(Vector3.Lerp(currentCamera.transform.position, transitioningCameraStat.startPos, rotationSpeed * Time.deltaTime), Quaternion.Lerp(currentCamera.transform.localRotation, Quaternion.Euler(transitioningCameraStat.startRot), rotationSpeed * Time.deltaTime));
    //    currentCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(currentCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, transitioningCameraStat.startOffset, rotationSpeed * Time.deltaTime);

    //    //If close enough to the second camera, reset current Camera to startPos
    //    if (Vector3.Distance(currentCamera.transform.position, transitioningCameraStat.startPos) <= rotationDistanceMin
    //        || timerCounter >= transitionTimer)
    //    {
    //        currentCamera.transform.SetPositionAndRotation(currentCameraStat.startPos, Quaternion.Euler(currentCameraStat.startRot));
    //        currentCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = currentCameraStat.startOffset;

    //        transitioningCamera.transform.parent.gameObject.SetActive(true);
    //        currentCamera.transform.parent.gameObject.SetActive(false);

    //        SetBlockDetectorDirection();

    //        isRotating = false;
    //    }
    //}
    //void SetBlockDetectorDirection()
    //{
    //    switch (cameraState)
    //    {
    //        case CameraState.Forward:
    //            PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 0, 0));
    //            break;
    //        case CameraState.Backward:
    //            PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 180, 0));
    //            break;
    //        case CameraState.Left:
    //            PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 90, 0));
    //            break;
    //        case CameraState.Right:
    //            PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, -90, 0));
    //            break;

    //        default:
    //            break;
    //    }
    //}
    //void AdjustFacingDirection()
    //{
    //    switch (cameraState)
    //    {
    //        case CameraState.Forward:
    //            if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
    //            {
    //                directionFacing = Vector3.forward;
    //            }
    //            else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 180, 0))
    //                    || PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -180, 0)))
    //            {
    //                directionFacing = Vector3.back;
    //            }
    //            else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 90, 0)))
    //            {
    //                directionFacing = Vector3.right;
    //            }
    //            else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 270, 0))
    //                    || PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -90, 0)))
    //            {
    //                directionFacing = Vector3.left;
    //            }
    //            else
    //            {
    //                directionFacing = Vector3.forward;
    //            }
    //            break;
    //        case CameraState.Backward:
    //            if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
    //            {
    //                directionFacing = Vector3.back;
    //            }
    //            else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 180, 0))
    //                    || PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -180, 0)))
    //            {
    //                directionFacing = Vector3.forward;
    //            }
    //            else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 90, 0)))
    //            {
    //                directionFacing = Vector3.left;
    //            }
    //            else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 270, 0))
    //                    || PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -90, 0)))
    //            {
    //                directionFacing = Vector3.right;
    //            }
    //            else
    //            {
    //                directionFacing = Vector3.back;
    //            }
    //            break;
    //        case CameraState.Left:
    //            if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
    //            {
    //                directionFacing = Vector3.left;
    //            }
    //            else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 180, 0))
    //                    || PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -180, 0)))
    //            {
    //                directionFacing = Vector3.right;
    //            }
    //            else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 90, 0)))
    //            {
    //                directionFacing = Vector3.forward;
    //            }
    //            else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 270, 0))
    //                    || PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -90, 0)))
    //            {
    //                directionFacing = Vector3.back;
    //            }
    //            else
    //            {
    //                directionFacing = Vector3.left;
    //            }
    //            break;
    //        case CameraState.Right:
    //            if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
    //            {
    //                directionFacing = Vector3.right;
    //            }
    //            else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 180, 0))
    //                    || PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -180, 0)))
    //            {
    //                directionFacing = Vector3.left;
    //            }
    //            else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 90, 0)))
    //            {
    //                directionFacing = Vector3.back;
    //            }
    //            else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 270, 0))
    //                    || PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -90, 0)))
    //            {
    //                directionFacing = Vector3.forward;
    //            }
    //            else
    //            {
    //                directionFacing = Vector3.right;
    //            }
    //            break;

    //        default:
    //            break;
    //    }
    //}


    ////--------------------


    //void CameraZoom_Setup()
    //{
    //    if (PlayerManager.Instance.pauseGame) { return; }
    //    if (isRotating) { return; }

    //    //Get the scroll input value
    //    float scrollInput = Input.GetAxis("Mouse ScrollWheel");

    //    if (scrollInput < 0f)
    //    {
    //        //Get ZoomScroll Value
    //        zoomScrollValue_Current = zoomScrollValue_Current + zoomScrollSpeed;

    //        //Set ZoomScroll Max Value
    //        if ((zoomScrollValue_Current) >= zoomScrollValue_Max)
    //        {
    //            zoomScrollValue_Current = zoomScrollValue_Max;
    //        }

    //        //Set the zoom of all cameras
    //        camera_Forward.m_Lens.FieldOfView = zoomScrollValue_Current;
    //        camera_Back.m_Lens.FieldOfView = zoomScrollValue_Current;
    //        camera_Left.m_Lens.FieldOfView = zoomScrollValue_Current;
    //        camera_Right.m_Lens.FieldOfView = zoomScrollValue_Current;
    //    }
    //    else if (scrollInput > 0f)
    //    {
    //        //Get ZoomScroll Value
    //        zoomScrollValue_Current = zoomScrollValue_Current - zoomScrollSpeed;

    //        //Set ZoomScroll Max Value
    //        if ((zoomScrollValue_Current) <= zoomScrollValue_Min)
    //        {
    //            zoomScrollValue_Current = zoomScrollValue_Min;
    //        }

    //        //Set the zoom of all cameras
    //        camera_Forward.m_Lens.FieldOfView = zoomScrollValue_Current;
    //        camera_Back.m_Lens.FieldOfView = zoomScrollValue_Current;
    //        camera_Left.m_Lens.FieldOfView = zoomScrollValue_Current;
    //        camera_Right.m_Lens.FieldOfView = zoomScrollValue_Current;
    //    }

    //    #region Old Zoom
    //    //if (Input.GetKeyDown(KeyCode.Q))
    //    //{
    //    //    switch (zoomState)
    //    //    {
    //    //        case CameraZoomState.ExtraShort:
    //    //            zoomState = CameraZoomState.ExtraShort;
    //    //            camera_Forward.m_Lens.FieldOfView = cameraZoom_ExtraShort;
    //    //            camera_Back.m_Lens.FieldOfView = cameraZoom_ExtraShort;
    //    //            camera_Left.m_Lens.FieldOfView = cameraZoom_ExtraShort;
    //    //            camera_Right.m_Lens.FieldOfView = cameraZoom_ExtraShort;
    //    //            break;
    //    //        case CameraZoomState.Short:
    //    //            zoomState = CameraZoomState.ExtraShort;
    //    //            camera_Forward.m_Lens.FieldOfView = cameraZoom_ExtraShort;
    //    //            camera_Back.m_Lens.FieldOfView = cameraZoom_ExtraShort;
    //    //            camera_Left.m_Lens.FieldOfView = cameraZoom_ExtraShort;
    //    //            camera_Right.m_Lens.FieldOfView = cameraZoom_ExtraShort;
    //    //            break;
    //    //        case CameraZoomState.Mid:
    //    //            zoomState = CameraZoomState.Short;
    //    //            camera_Forward.m_Lens.FieldOfView = cameraZoom_short;
    //    //            camera_Back.m_Lens.FieldOfView = cameraZoom_short;
    //    //            camera_Left.m_Lens.FieldOfView = cameraZoom_short;
    //    //            camera_Right.m_Lens.FieldOfView = cameraZoom_short;
    //    //            break;
    //    //        case CameraZoomState.Long:
    //    //            zoomState = CameraZoomState.Mid;
    //    //            camera_Forward.m_Lens.FieldOfView = cameraZoom_mid;
    //    //            camera_Back.m_Lens.FieldOfView = cameraZoom_mid;
    //    //            camera_Left.m_Lens.FieldOfView = cameraZoom_mid;
    //    //            camera_Right.m_Lens.FieldOfView = cameraZoom_mid;
    //    //            break;
    //    //        case CameraZoomState.ExtraLong:
    //    //            zoomState = CameraZoomState.Long;
    //    //            camera_Forward.m_Lens.FieldOfView = cameraZoom_long;
    //    //            camera_Back.m_Lens.FieldOfView = cameraZoom_long;
    //    //            camera_Left.m_Lens.FieldOfView = cameraZoom_long;
    //    //            camera_Right.m_Lens.FieldOfView = cameraZoom_long;
    //    //            break;

    //    //        default:
    //    //            break;
    //    //    }
    //    //}
    //    //if (Input.GetKeyDown(KeyCode.E))
    //    //{
    //    //    switch (zoomState)
    //    //    {
    //    //        case CameraZoomState.ExtraShort:
    //    //            zoomState = CameraZoomState.Short;
    //    //            camera_Forward.m_Lens.FieldOfView = cameraZoom_short;
    //    //            camera_Back.m_Lens.FieldOfView = cameraZoom_short;
    //    //            camera_Left.m_Lens.FieldOfView = cameraZoom_short;
    //    //            camera_Right.m_Lens.FieldOfView = cameraZoom_short;
    //    //            break;
    //    //        case CameraZoomState.Short:
    //    //            zoomState = CameraZoomState.Mid;
    //    //            camera_Forward.m_Lens.FieldOfView = cameraZoom_mid;
    //    //            camera_Back.m_Lens.FieldOfView = cameraZoom_mid;
    //    //            camera_Left.m_Lens.FieldOfView = cameraZoom_mid;
    //    //            camera_Right.m_Lens.FieldOfView = cameraZoom_mid;
    //    //            break;
    //    //        case CameraZoomState.Mid:
    //    //            zoomState = CameraZoomState.Long;
    //    //            camera_Forward.m_Lens.FieldOfView = cameraZoom_long;
    //    //            camera_Back.m_Lens.FieldOfView = cameraZoom_long;
    //    //            camera_Left.m_Lens.FieldOfView = cameraZoom_long;
    //    //            camera_Right.m_Lens.FieldOfView = cameraZoom_long;
    //    //            break;
    //    //        case CameraZoomState.Long:
    //    //            zoomState = CameraZoomState.ExtraLong;
    //    //            camera_Forward.m_Lens.FieldOfView = cameraZoom_ExtraLong;
    //    //            camera_Back.m_Lens.FieldOfView = cameraZoom_ExtraLong;
    //    //            camera_Left.m_Lens.FieldOfView = cameraZoom_ExtraLong;
    //    //            camera_Right.m_Lens.FieldOfView = cameraZoom_ExtraLong;
    //    //            break;
    //    //        case CameraZoomState.ExtraLong:
    //    //            zoomState = CameraZoomState.ExtraLong;
    //    //            camera_Forward.m_Lens.FieldOfView = cameraZoom_ExtraLong;
    //    //            camera_Back.m_Lens.FieldOfView = cameraZoom_ExtraLong;
    //    //            camera_Left.m_Lens.FieldOfView = cameraZoom_ExtraLong;
    //    //            camera_Right.m_Lens.FieldOfView = cameraZoom_ExtraLong;
    //    //            break;

    //    //        default:
    //    //            break;
    //    //    }
    //    //}
    //    #endregion
    //}


    ////--------------------


    //void TranslatingDirection()
    //{
    //    if (directionFacing == Vector3.forward)
    //        directionTranslator = "Forward";
    //    else if (directionFacing == Vector3.back)
    //        directionTranslator = "Back";
    //    else if (directionFacing == Vector3.left)
    //        directionTranslator = "Left";
    //    else if (directionFacing == Vector3.right)
    //        directionTranslator = "Right";
    //}
}