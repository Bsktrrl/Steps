using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class Cameras_v2 : Singleton<Cameras_v2>
{
    public static event Action rotateCamera;

    [SerializeField] GameObject cameraAnchor;
    public CameraState cameraState;
    public Vector3 directionFacing;

    [SerializeField] float rotationDuration = 0.5f;
    [SerializeField] float waitDelay = 0.1f;
    public bool isRotating;


    //--------------------


    private void Start()
    {
        cameraState = CameraState.Forward;
        directionFacing = Vector3.forward;

        AdjustFacingDirection();
    }
    private void Update()
    {
        RotateCameraSetup();
    }


    //--------------------


    void RotateCameraSetup()
    {
        //Don't be able to switch camera angle before the rotation has been done
        if (isRotating) { return; }
        if (Player_Interact.Instance.isInteracting) { return; }
        if (Player_Movement.Instance.iceGliding) { return; }
        if (Player_Movement.Instance.movementStates == MovementStates.Moving) { return; }
        if (Player_Movement.Instance.ladderMovement_Top_ToBlock) { return; }
        
        //Rotate Camera
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKey(KeyCode.Q))
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

            StartCoroutine(RotateCamera(90));
        }
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKey(KeyCode.E))
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

            StartCoroutine(RotateCamera(-90));
        }
    }
    IEnumerator RotateCamera(float angle)
    {
        rotateCamera?.Invoke();

        isRotating = true;
        PlayerManager.Instance.pauseGame = true;

        // Record the starting rotation
        Quaternion startRotation = cameraAnchor.transform.rotation;

        // Calculate the target rotation
        Quaternion endRotation = startRotation * Quaternion.Euler(0, angle, 0);

        float elapsed = 0f;

        // Smoothly interpolate the rotation
        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotationDuration); // Normalize the time
            cameraAnchor.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null; // Wait for the next frame
        }

        // Ensure the final rotation is set exactly
        cameraAnchor.transform.rotation = endRotation;

        SetBlockDetectorDirection();

        yield return new WaitForSeconds(waitDelay); // Wait for the next frame

        PlayerManager.Instance.pauseGame = false;
        isRotating = false;
    }


    void SetBlockDetectorDirection()
    {
        switch (cameraState)
        {
            case CameraState.Forward:
                PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 0, 0));
                break;
            case CameraState.Backward:
                PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 180, 0));
                break;
            case CameraState.Left:
                PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, 90, 0));
                break;
            case CameraState.Right:
                PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.SetPositionAndRotation(PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().blockDetector_Parent.transform.position, Quaternion.Euler(0, -90, 0));
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
            case CameraState.Backward:
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
            case CameraState.Left:
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
            case CameraState.Right:
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
