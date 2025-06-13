using Cinemachine;
using System;
using System.Collections;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    public static event Action Action_RotateCamera_Start;
    public static event Action Action_RotateCamera_End;

    [Header("Camera Objects")]
    public GameObject cameraAnchor;
    [SerializeField] GameObject cameraOffset;

    [Header("States")]
    public CameraState cameraState;
    public CameraRotationState cameraRotationState;
    public Vector3 directionFacing;

    [Header("Parameters")]
    float rotationDuration_Movement = 0.35f;
    float rotationDuration_Ceiling = 0.5f;
    float waitDelay = 0.05f;
    public bool isRotating;
    public bool isCeilingRotating;

    [Header("Positions")]
    [SerializeField] Vector3 cameraAnchor_originalPos;
    [SerializeField] Quaternion cameraAnchor_originalRot;
    [SerializeField] Vector3 cameraOffset_originalPos /*= new Vector3(0, 4.3f, -9.2f)*/;
    [SerializeField] Quaternion cameraOffset_originalRot;
    Vector3 cameraOffset_ceilingGrabPos = new Vector3(0, -2f, -9.2f);
    float cameraTilt_Original = 27;
    float cameraTilt_Ceiling = -17;


    //--------------------


    private void Start()
    {
        cameraRotationState = CameraRotationState.Forward;
        directionFacing = Vector3.forward;

        cameraAnchor_originalPos = cameraAnchor.transform.localPosition;
        cameraAnchor_originalRot = cameraAnchor.transform.rotation;

        cameraOffset_originalPos = cameraOffset.transform.localPosition;
        cameraOffset_originalRot = cameraOffset.transform.rotation;

        //SetBlockDetectorDirection();
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

        float elapsed = 0f;

        // Smoothly interpolate the rotation
        while (elapsed < rotationDuration_Movement)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotationDuration_Movement); // Normalize the time
            cameraAnchor.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null; // Wait for the next frame
        }

        // Ensure the final rotation is set exactly
        cameraAnchor.transform.rotation = endRotation;

        //SetBlockDetectorDirection();

        yield return new WaitForSeconds(waitDelay); // Wait for the next frame

        PlayerManager.Instance.pauseGame = false;
        isRotating = false;

        Movement.Instance.previousPosition = transform.position;

        Action_RotateCamera_End?.Invoke();
    }

    public IEnumerator CeilingCameraRotation(float angle)
    {
        isCeilingRotating = true;
        PlayerManager.Instance.pauseGame = true;

        //Iterate the states
        if (cameraState == CameraState.GameplayCam)
            cameraState = CameraState.CeilingCam;
        else if (cameraState == CameraState.CeilingCam)
            cameraState = CameraState.GameplayCam;

        // Record the starting rotation
        Vector3 startPosition = cameraOffset.transform.localPosition;
        Quaternion startRotation = cameraOffset.transform.rotation;

        // Calculate the target rotation
        Vector3 endPosition = new Vector3();
        Quaternion endRotation = new Quaternion();

        if (cameraState == CameraState.GameplayCam)
        {
            endPosition = cameraOffset_originalPos;
            endRotation = Quaternion.Euler(cameraTilt_Original, angle, 0);
        }
        else if (cameraState == CameraState.CeilingCam)
        {
            endPosition = cameraOffset_ceilingGrabPos;
            endRotation = Quaternion.Euler(cameraTilt_Ceiling, angle, 0);
        }

        float elapsed = 0f;

        // Smoothly interpolate the rotation
        while (elapsed < rotationDuration_Ceiling)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotationDuration_Ceiling); // Normalize the time
            cameraOffset.transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            cameraOffset.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null; // Wait for the next frame
        }

        // Ensure the final rotation is set exactly
        cameraOffset.transform.localPosition = endPosition;
        cameraOffset.transform.rotation = endRotation;

        //SetBlockDetectorDirection();
        AdjustFacingDirection();

        yield return new WaitForSeconds(waitDelay); // Wait for the next frame

        PlayerManager.Instance.pauseGame = false;
        isCeilingRotating = false;
    }

    public void ResetCameraRotation()
    {
        cameraRotationState = CameraRotationState.Forward;
        cameraState = CameraState.GameplayCam;

        cameraAnchor.transform.rotation = Quaternion.Euler(cameraAnchor_originalRot.eulerAngles.x, 0, 0);

        cameraOffset.transform.localPosition = cameraOffset_originalPos;
        cameraOffset.transform.rotation = cameraOffset_originalRot;

        cameraAnchor.transform.localPosition = cameraAnchor_originalPos;
        cameraAnchor.transform.rotation = cameraAnchor_originalRot;


        //SetBlockDetectorDirection();
    }

    //void SetBlockDetectorDirection()
    //{
    //    switch (cameraRotationState)
    //    {
    //        case CameraRotationState.Forward:
    //            PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 0, 0));
    //            break;
    //        case CameraRotationState.Backward:
    //            PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 180, 0));
    //            break;
    //        case CameraRotationState.Left:
    //            PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 90, 0));
    //            break;
    //        case CameraRotationState.Right:
    //            PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, -90, 0));
    //            break;

    //        default:
    //            break;
    //    }
    //}
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
