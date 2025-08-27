using Unity.Cinemachine;
using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    float waitDelay = 0.05f;
    public bool isRotating;
    public bool isCeilingRotating;

    [Header("Positions")]
    [SerializeField] Vector3 cameraAnchor_originalPos;
    [SerializeField] Quaternion cameraAnchor_originalRot;
    float cameraTilt_Ceiling = -17;

    //[HideInInspector] public Vector3 cameraOffset_ceilingGrabPos = new Vector3(0, -1f, -4.2f);
    float cameraTilt_Original;

    [Header("NPC Camera")]
    float npcMovementTimer = 0.85f;


    //--------------------


    private void Start()
    {
        cameraRotationState = CameraRotationState.Forward;
        directionFacing = Vector3.forward;

        cameraAnchor_originalPos = cameraAnchor.transform.localPosition;
        cameraAnchor_originalRot = cameraAnchor.transform.rotation;

        AdjustFacingDirection();
    }


    //--------------------


    public void RotateCameraX()
    {
        //Don't be able to switch camera angle before the rotation has been done
        if (isRotating) { return; }
        if (isCeilingRotating) { return; }
        if (Player_Interact.Instance.isInteracting) { return; }
        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return; }
        //if (PlayerManager.Instance.isTransportingPlayer) { return; }

        //Rotate Camera
        switch (cameraRotationState)
        {
            case CameraRotationState.Forward:
                cameraRotationState = CameraRotationState.Left;
                break;
            case CameraRotationState.Backward:
                cameraRotationState = CameraRotationState.Right;
                break;
            case CameraRotationState.Left:
                cameraRotationState = CameraRotationState.Backward;
                break;
            case CameraRotationState.Right:
                cameraRotationState = CameraRotationState.Forward;
                break;
            default:
                break;
        }

        StartCoroutine(RotateCamera(90));
    }
    public void RotateCameraY()
    {
        //Don't be able to switch camera angle before the rotation has been done
        if (isRotating) { return; }
        if (isCeilingRotating) { return; }
        if (Player_Interact.Instance.isInteracting) { return; }
        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return; }
        //if (PlayerManager.Instance.isTransportingPlayer) { return; }

        //Rotate Camera
        switch (cameraRotationState)
        {
            case CameraRotationState.Forward:
                cameraRotationState = CameraRotationState.Right;
                break;
            case CameraRotationState.Backward:
                cameraRotationState = CameraRotationState.Left;
                break;
            case CameraRotationState.Left:
                cameraRotationState = CameraRotationState.Forward;
                break;
            case CameraRotationState.Right:
                cameraRotationState = CameraRotationState.Backward;
                break;
            default:
                break;
        }

        StartCoroutine(RotateCamera(-90));
    }
    
    IEnumerator RotateCamera(float angle)
    {
        Action_RotateCamera_Start?.Invoke();

        isRotating = true;
        PlayerManager.Instance.pauseGame = true;

        // Record the starting rotation
        Quaternion startRotation = cameraAnchor.transform.rotation;

        // Calculate the target rotation
        Quaternion endRotation = startRotation * Quaternion.Euler(0, angle, 0);

        if (SettingsManager.Instance.settingsData.currentCameraMotion == CameraMotion.Can)
        {
            float elapsed = 0f;

            // Smoothly interpolate the rotation
            while (elapsed < rotationDuration_Movement)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / rotationDuration_Movement); // Normalize the time
                cameraAnchor.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
                yield return null; // Wait for the next frame
            }
        }
        
        // Ensure the final rotation is set exactly
        cameraAnchor.transform.rotation = endRotation;

        //SetBlockDetectorDirection();

        yield return new WaitForSeconds(waitDelay); // Wait for the next frame

        PlayerManager.Instance.pauseGame = false;

        Movement.Instance.previousPosition = transform.position;

        Action_RotateCamera_End?.Invoke();
    }

    public IEnumerator CeilingCameraRotation(float angle)
    {
        isCeilingRotating = true;
        PlayerManager.Instance.pauseGame = true;

        //Iterate the states
        if (cameraState == CameraState.GameplayCam)
        {
            StartCoroutine(StartVirtualCameraBlend_In(CM_Player_CeilingGrab));

            cameraState = CameraState.CeilingCam;
        }
        else if (cameraState == CameraState.CeilingCam)
        {
            StartCoroutine(StartVirtualCameraBlend_Out(CM_Player_CeilingGrab));

            cameraState = CameraState.GameplayCam;
        }

        //// Record the starting rotation
        //Vector3 startPosition = cameraAnchor.transform.localPosition;
        //Quaternion startRotation = cameraAnchor.transform.rotation;

        //// Calculate the target rotation
        //Vector3 endPosition = new Vector3();
        //Quaternion endRotation = new Quaternion();

        //if (cameraState == CameraState.GameplayCam)
        //{
        //    endRotation = Quaternion.Euler(cameraTilt_Original, angle, 0);
        //}
        //else if (cameraState == CameraState.CeilingCam)
        //{
        //    //endPosition = cameraOffset_ceilingGrabPos;
        //    endRotation = Quaternion.Euler(cameraTilt_Ceiling, angle, 0);
        //}

        //if (SettingsManager.Instance.settingsData.currentCameraMotion == CameraMotion.Can)
        //{
        //    float elapsed = 0f;

        //    // Smoothly interpolate the rotation
        //    while (elapsed < rotationDuration_Ceiling)
        //    {
        //        elapsed += Time.deltaTime;
        //        float t = Mathf.Clamp01(elapsed / rotationDuration_Ceiling); // Normalize the time
        //        cameraAnchor.transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
        //        cameraAnchor.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
        //        yield return null; // Wait for the next frame
        //    }
        //}

        //// Ensure the final rotation is set exactly
        //cameraAnchor.transform.localPosition = endPosition;
        //cameraAnchor.transform.rotation = endRotation;

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
        cameraState = CameraState.GameplayCam;

        cameraAnchor.transform.rotation = Quaternion.Euler(cameraAnchor_originalRot.eulerAngles.x, 0, 0);

        cameraAnchor.transform.localPosition = cameraAnchor_originalPos;
        cameraAnchor.transform.rotation = cameraAnchor_originalRot;
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

        cameraState = CameraState.GameplayCam;

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


    //--------------------


    void ResetCameraPriority()
    {
        CM_Player.Priority.Value = 10;
        CM_Player_CeilingGrab.Priority.Value = -10;

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

        if (SettingsManager.Instance.settingsData.currentCameraMotion == CameraMotion.Can)
        {
            CM_Brain.DefaultBlend.Time = npcMovementTimer;
            yield return new WaitForSeconds(CM_Brain.DefaultBlend.Time + 0.15f);
        }
        else if (SettingsManager.Instance.settingsData.currentCameraMotion == CameraMotion.Cannot)
        {
            MotionSicknessToggle.Instance.SetReduceMotion(true);

            CM_Brain.DefaultBlend.Time = 0;
            yield return new WaitForSeconds(0 + 0.35f);

            MotionSicknessToggle.Instance.SetReduceMotion(false);
        }
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

        if (SettingsManager.Instance.settingsData.currentCameraMotion == CameraMotion.Can)
        {
            CM_Brain.DefaultBlend.Time = npcMovementTimer;

            yield return new WaitForSeconds(CM_Brain.DefaultBlend.Time + 0.15f);
            //yield return new WaitUntil(() => CM_Brain.IsBlending == false);
        }
        else if (SettingsManager.Instance.settingsData.currentCameraMotion == CameraMotion.Cannot)
        {
            MotionSicknessToggle.Instance.SetReduceMotion(true);

            CM_Brain.DefaultBlend.Time = 0;

            yield return new WaitForSeconds(0 + 0.35f);
            //yield return new WaitUntil(() => CM_Brain.IsBlending == false);

            MotionSicknessToggle.Instance.SetReduceMotion(false);
        }
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
