using Unity.Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class CameraController : Singleton<CameraController>
{
    public static event Action Action_RotateCamera_Start;
    public static event Action Action_RotateCamera_End;

    [Header("Camera Objects")]
    public GameObject cameraAnchor;

    [Header("Cameras")]
    public CinemachineBrain CM_Brain;
    public CinemachineCamera CM_Player;
    public CinemachineCamera CM_Player_CeilingGrab;
    public CinemachineCamera CM_UnderCeiling;
    public CinemachineCamera CM_Other;


    [Header("States")]
    public CameraState cameraState;
    public CameraRotationState cameraRotationState;
    public Vector3 directionFacing;

    [Header("Parameters")]
    float rotationDuration_Movement = 0.35f;
    float rotationDegreesPerSecond => 90f / rotationDuration_Movement;
    float waitDelay = 0.02f;
    public bool isRotating;
    public bool isIgnoringObstaclesWhenRotating;
    public bool isCeilingRotating;

    [Header("Positions")]
    [SerializeField] Vector3 cameraAnchor_originalPos;
    [SerializeField] Quaternion cameraAnchor_originalRot;
    float cameraTilt_Ceiling = -17;

    //[HideInInspector] public Vector3 cameraOffset_ceilingGrabPos = new Vector3(0, -1f, -4.2f);
    float cameraTilt_Original;

    [Header("NPC Camera")]
    float npcMovementTimer = 0.85f;

    [Header("Rotation Que")]
    Coroutine rotationCoroutine;

    // The last fully completed 90-degree camera state.
    CameraRotationState snappedRotationState;

    // Direction of current active segment:
    // +1 = +90, -1 = -90
    int activeRotationDirection = 0;

    // Single buffered extra rotation.
    // 0 = none, +1 = queue +90, -1 = queue -90
    const int maxQueuedRotations = 3;
    Queue<int> queuedRotationDirections = new Queue<int>(maxQueuedRotations);

    // The state the current segment is moving toward.
    CameraRotationState activeSegmentTargetState;
    CameraRotationState activeSegmentStartState;


    //--------------------


    private void Start()
    {
        cameraRotationState = CameraRotationState.Forward;
        snappedRotationState = cameraRotationState;
        activeSegmentStartState = cameraRotationState;
        activeSegmentTargetState = cameraRotationState;

        directionFacing = Vector3.forward;

        cameraAnchor_originalPos = cameraAnchor.transform.localPosition;
        cameraAnchor_originalRot = cameraAnchor.transform.rotation;

        AdjustFacingDirection();
    }


    //--------------------


    public void RotateCameraX()
    {
        if (isCeilingRotating) { return; }
        if (Player_Interact.Instance.isInteracting) { return; }
        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return; }

        int rotationDirection = 0;

        if (DataManager.Instance.settingData_StoreList.currentRevertedCameraMotion == RevertedCameraMotion.Normal)
        {
            rotationDirection = +1;
        }
        else
        {
            rotationDirection = -1;
        }

        RequestCameraRotation(rotationDirection);
    }

    public void RotateCameraY()
    {
        if (isCeilingRotating) { return; }
        if (Player_Interact.Instance.isInteracting) { return; }
        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return; }

        int rotationDirection = 0;

        if (DataManager.Instance.settingData_StoreList.currentRevertedCameraMotion == RevertedCameraMotion.Normal)
        {
            rotationDirection = -1;
        }
        else
        {
            rotationDirection = +1;
        }

        RequestCameraRotation(rotationDirection);
    }

    void RequestCameraRotation(int rotationDirection)
    {
        if (rotationDirection == 0) { return; }

        // If not rotating, start immediately.
        if (!isRotating)
        {
            activeRotationDirection = rotationDirection;
            queuedRotationDirections.Clear();

            activeSegmentStartState = snappedRotationState;
            activeSegmentTargetState = GetNextRotationState(snappedRotationState, activeRotationDirection);
            cameraRotationState = activeSegmentTargetState;

            rotationCoroutine = StartCoroutine(RotateCamera());
            return;
        }

        // Same direction while rotating:
        // allow up to 2 queued extra turns.
        if (rotationDirection == activeRotationDirection)
        {
            if (queuedRotationDirections.Count < maxQueuedRotations)
            {
                queuedRotationDirections.Enqueue(rotationDirection);
            }

            return;
        }

        // Opposite direction while rotating:
        // clear the queue and reverse immediately back to the current segment's start state.
        queuedRotationDirections.Clear();
        activeRotationDirection = rotationDirection;

        activeSegmentTargetState = activeSegmentStartState;
        cameraRotationState = activeSegmentTargetState;
    }

    IEnumerator RotateCamera()
    {
        Action_RotateCamera_Start?.Invoke();
        HoleShaderOnOffScript.Instance.HoleShader_On();

        isRotating = true;
        PlayerManager.Instance.pauseGame = true;

        while (true)
        {
            Quaternion targetRotation = GetRotationForState(activeSegmentTargetState);

            while (Quaternion.Angle(cameraAnchor.transform.rotation, targetRotation) > 0.01f)
            {
                float remainingAngle = Quaternion.Angle(cameraAnchor.transform.rotation, targetRotation);

                if (remainingAngle <= 22.5f || remainingAngle >= 67.5f)
                {
                    isIgnoringObstaclesWhenRotating = false;
                }
                else
                {
                    isIgnoringObstaclesWhenRotating = true;
                }

                if (SettingsManager.Instance.settingsData.currentCameraMotion == CameraMotion.Can)
                {
                    float step = rotationDegreesPerSecond * Time.deltaTime;
                    cameraAnchor.transform.rotation = Quaternion.RotateTowards(
                        cameraAnchor.transform.rotation,
                        targetRotation,
                        step
                    );
                }
                else
                {
                    cameraAnchor.transform.rotation = targetRotation;
                }

                // If target changed during rotation (for example reverse input),
                // immediately continue toward the new exact snapped target
                // with the same angular speed.
                targetRotation = GetRotationForState(activeSegmentTargetState);

                yield return null;
            }

            // Snap exactly to the final legal state.
            cameraAnchor.transform.rotation = targetRotation;

            snappedRotationState = activeSegmentTargetState;
            cameraRotationState = snappedRotationState;
            AdjustFacingDirection();

            // If queued extra turns exist, continue immediately with no pause.
            if (queuedRotationDirections.Count > 0)
            {
                activeRotationDirection = queuedRotationDirections.Dequeue();

                activeSegmentStartState = snappedRotationState;
                activeSegmentTargetState = GetNextRotationState(snappedRotationState, activeRotationDirection);
                cameraRotationState = activeSegmentTargetState;

                continue;
            }

            break;
        }

        isIgnoringObstaclesWhenRotating = false;
        HoleShaderOnOffScript.Instance.HoleShader_Off();

        yield return new WaitForSeconds(waitDelay);

        PlayerManager.Instance.pauseGame = false;
        isRotating = false;

        Movement.Instance.previousPosition = transform.position;

        Action_RotateCamera_End?.Invoke();
        rotationCoroutine = null;
    }

    CameraRotationState GetNextRotationState(CameraRotationState currentState, int rotationDirection)
    {
        // +1 = +90
        // -1 = -90

        if (rotationDirection > 0)
        {
            switch (currentState)
            {
                case CameraRotationState.Forward:
                    return CameraRotationState.Left;
                case CameraRotationState.Left:
                    return CameraRotationState.Backward;
                case CameraRotationState.Backward:
                    return CameraRotationState.Right;
                case CameraRotationState.Right:
                    return CameraRotationState.Forward;
            }
        }
        else if (rotationDirection < 0)
        {
            switch (currentState)
            {
                case CameraRotationState.Forward:
                    return CameraRotationState.Right;
                case CameraRotationState.Right:
                    return CameraRotationState.Backward;
                case CameraRotationState.Backward:
                    return CameraRotationState.Left;
                case CameraRotationState.Left:
                    return CameraRotationState.Forward;
            }
        }

        return currentState;
    }

    public IEnumerator CeilingCameraRotation(float angle)
    {
        isCeilingRotating = true;
        PlayerManager.Instance.pauseGame = true;

        //Iterate the states
        if (cameraState == CameraState.GameplayCam)
        {
            if (CM_Player_CeilingGrab)
            {
                StartCoroutine(StartVirtualCameraBlend_In(CM_Player_CeilingGrab));
            }

            cameraState = CameraState.CeilingCam;
        }
        else if (cameraState == CameraState.CeilingCam)
        {
            if (CM_Player_CeilingGrab)
            {
                StartCoroutine(StartVirtualCameraBlend_Out(CM_Player_CeilingGrab));
            }

            cameraState = CameraState.GameplayCam;
        }

        //SetBlockDetectorDirection();
        AdjustFacingDirection();

        yield return new WaitForSeconds(waitDelay); // Wait for the next frame

        PlayerManager.Instance.pauseGame = false;
        isCeilingRotating = false;
    }

    public void ResetCameraRotation()
    {
        ResetCameraPriority();

        cameraRotationState = CameraRotationState.Forward;
        snappedRotationState = cameraRotationState;
        activeSegmentStartState = cameraRotationState;
        activeSegmentTargetState = cameraRotationState;
        cameraState = CameraState.GameplayCam;

        activeRotationDirection = 0;
        queuedRotationDirections.Clear();
        activeSegmentTargetState = cameraRotationState;

        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = null;
        }

        isRotating = false;
        isIgnoringObstaclesWhenRotating = false;

        cameraAnchor.transform.rotation = Quaternion.Euler(cameraAnchor_originalRot.eulerAngles.x, 0, 0);

        cameraAnchor.transform.localPosition = cameraAnchor_originalPos;
        cameraAnchor.transform.rotation = cameraAnchor_originalRot;

        AdjustFacingDirection();
    }

    public void SetRespawnCameraRotation()
    {
        ResetCameraPriority();

        switch (MapManager.Instance.playerStartRot)
        {
            case MovementDirection.None:
                cameraRotationState = CameraRotationState.Forward;
                cameraAnchor.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;

            case MovementDirection.Forward:
                cameraRotationState = CameraRotationState.Forward;
                cameraAnchor.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case MovementDirection.Backward:
                cameraRotationState = CameraRotationState.Backward;
                cameraAnchor.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case MovementDirection.Left:
                cameraRotationState = CameraRotationState.Right;
                cameraAnchor.transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            case MovementDirection.Right:
                cameraRotationState = CameraRotationState.Left;
                cameraAnchor.transform.rotation = Quaternion.Euler(0, 90, 0);
                break;

            default:
                cameraRotationState = CameraRotationState.Forward;
                cameraAnchor.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
        }

        snappedRotationState = cameraRotationState;
        activeRotationDirection = 0;
        queuedRotationDirections.Clear();
        activeSegmentStartState = cameraRotationState;
        activeSegmentTargetState = cameraRotationState;

        cameraState = CameraState.GameplayCam;

        AdjustFacingDirection();
        Movement.Instance.previousPosition = transform.position;
    }

    public Quaternion GetRespawnCameraDirection()
    {
        switch (MapManager.Instance.playerStartRot)
        {
            case MovementDirection.None:
                return Quaternion.Euler(0, 0, 0);

            case MovementDirection.Forward:
                return Quaternion.Euler(0, 0, 0);
            case MovementDirection.Backward:
                return Quaternion.Euler(0, 180, 0);
            case MovementDirection.Left:
                return Quaternion.Euler(0, 90, 0);
            case MovementDirection.Right:
                return Quaternion.Euler(0, -90, 0);

            default:
                return Quaternion.Euler(0, 0, 0);
        }
    }

    void AdjustFacingDirection()
    {
        switch (cameraRotationState)
        {
            case CameraRotationState.Forward:
                if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
                {
                    directionFacing = Vector3.forward;
                }
                else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 180, 0))
                        || PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -180, 0)))
                {
                    directionFacing = Vector3.back;
                }
                else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 90, 0)))
                {
                    directionFacing = Vector3.right;
                }
                else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 270, 0))
                        || PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -90, 0)))
                {
                    directionFacing = Vector3.left;
                }
                else
                {
                    directionFacing = Vector3.forward;
                }
                break;
            case CameraRotationState.Backward:
                if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
                {
                    directionFacing = Vector3.back;
                }
                else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 180, 0))
                        || PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -180, 0)))
                {
                    directionFacing = Vector3.forward;
                }
                else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 90, 0)))
                {
                    directionFacing = Vector3.left;
                }
                else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 270, 0))
                        || PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -90, 0)))
                {
                    directionFacing = Vector3.right;
                }
                else
                {
                    directionFacing = Vector3.back;
                }
                break;
            case CameraRotationState.Left:
                if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
                {
                    directionFacing = Vector3.left;
                }
                else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 180, 0))
                        || PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -180, 0)))
                {
                    directionFacing = Vector3.right;
                }
                else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 90, 0)))
                {
                    directionFacing = Vector3.forward;
                }
                else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 270, 0))
                        || PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -90, 0)))
                {
                    directionFacing = Vector3.back;
                }
                else
                {
                    directionFacing = Vector3.left;
                }
                break;
            case CameraRotationState.Right:
                if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
                {
                    directionFacing = Vector3.right;
                }
                else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 180, 0))
                        || PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -180, 0)))
                {
                    directionFacing = Vector3.left;
                }
                else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 90, 0)))
                {
                    directionFacing = Vector3.back;
                }
                else if (PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, 270, 0))
                        || PlayerManager.Instance.playerBody.transform.rotation == Quaternion.Euler(new Vector3(0, -90, 0)))
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

    Quaternion GetRotationForState(CameraRotationState state)
    {
        switch (state)
        {
            case CameraRotationState.Forward:
                return Quaternion.Euler(0f, 0f, 0f);

            case CameraRotationState.Backward:
                return Quaternion.Euler(0f, 180f, 0f);

            case CameraRotationState.Left:
                return Quaternion.Euler(0f, 90f, 0f);

            case CameraRotationState.Right:
                return Quaternion.Euler(0f, -90f, 0f);

            default:
                return Quaternion.Euler(0f, 0f, 0f);
        }
    }


    //--------------------


    void ResetCameraPriority()
    {
        CM_Player.Priority.Value = 10;
        if (CM_Player_CeilingGrab)
        {
            CM_Player_CeilingGrab.Priority.Value = -10;
        }

        if (CM_Other)
        {
            CM_Other.Priority.Value = -10;
        }
    }


    //--------------------


    public IEnumerator StartVirtualCameraBlend_In(CinemachineCamera blendCamera)
    {
        if (CM_Player)
        {
            CM_Player.Priority.Value = -10;
        }
        if (blendCamera)
        {
            blendCamera.Priority.Value = 10;
        }

        CM_Brain.DefaultBlend.Time = npcMovementTimer;
        yield return new WaitForSeconds(CM_Brain.DefaultBlend.Time + 0.15f);
    }

    public IEnumerator StartVirtualCameraBlend_Out(CinemachineCamera blendCamera)
    {
        if (blendCamera)
        {
            blendCamera.Priority.Value = -10;
        }
        if (CM_Player)
        {
            CM_Player.Priority.Value = 10;
        }

        yield return null;

        CM_Brain.DefaultBlend.Time = npcMovementTimer;
        yield return new WaitForSeconds(CM_Brain.DefaultBlend.Time + 0.15f);
    }
}

public enum CameraState
{
    GameplayCam,

    FreeCam,
    CeilingCam,
    DialogueCam
}
public enum CameraRotationState
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