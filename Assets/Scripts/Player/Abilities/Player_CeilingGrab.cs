using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CeilingGrab : Singleton<Player_CeilingGrab>
{
    [SerializeField] bool canCeilingGrab;
    public bool isCeilingGrabbing;
    public bool isCeilingRotation;

    float playerRotationDuration_Ceiling = 0.5f;
    public float playerCeilingRotationValue;

    RaycastHit hit;


    //--------------------


    private void Update()
    {
        RaycastCeiling();
        CeilingCameraSetup();
    }


    //--------------------


    void CeilingCameraSetup()
    {
        //Don't be able to switch camera angle before the rotation has been done
        if (!canCeilingGrab) { return; }
        if (Cameras_v2.Instance.isRotating) { return; }
        if (Cameras_v2.Instance.isCeilingRotating) { return; }
        if (Player_Interact.Instance.isInteracting) { return; }
        if (Player_Movement.Instance.iceGliding) { return; }
        if (Player_Movement.Instance.movementStates == MovementStates.Moving) { return; }
        if (Player_Movement.Instance.ladderMovement_Top_ToBlock) { return; }

        if (Input.GetKeyDown(KeyCode.C) && Cameras_v2.Instance.cameraState == CameraState.GameplayCam)
        {
            isCeilingGrabbing = true;

            if (Cameras_v2.Instance.cameraRotationState == CameraRotationState.Forward)
                StartCoroutine(Cameras_v2.Instance.CeilingCameraRotation(0));
            else if (Cameras_v2.Instance.cameraRotationState == CameraRotationState.Backward)
                StartCoroutine(Cameras_v2.Instance.CeilingCameraRotation(180));
            else if (Cameras_v2.Instance.cameraRotationState == CameraRotationState.Left)
                StartCoroutine(Cameras_v2.Instance.CeilingCameraRotation(90));
            else if (Cameras_v2.Instance.cameraRotationState == CameraRotationState.Right)
                StartCoroutine(Cameras_v2.Instance.CeilingCameraRotation(-90));

            StartCoroutine(RotateToCeiling(180));
        }
        else if (Input.GetKeyDown(KeyCode.C) && Cameras_v2.Instance.cameraState == CameraState.CeilingCam)
        {
            if (Cameras_v2.Instance.cameraRotationState == CameraRotationState.Forward)
                StartCoroutine(Cameras_v2.Instance.CeilingCameraRotation(0));
            else if (Cameras_v2.Instance.cameraRotationState == CameraRotationState.Backward)
                StartCoroutine(Cameras_v2.Instance.CeilingCameraRotation(180));
            else if (Cameras_v2.Instance.cameraRotationState == CameraRotationState.Left)
                StartCoroutine(Cameras_v2.Instance.CeilingCameraRotation(90));
            else if (Cameras_v2.Instance.cameraRotationState == CameraRotationState.Right)
                StartCoroutine(Cameras_v2.Instance.CeilingCameraRotation(-90));

            StartCoroutine(RotateToCeiling(0));
        }
    }
    void RaycastCeiling()
    {
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 1))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                canCeilingGrab = true;
                return;
            }
        }

        canCeilingGrab = false;
    }


    //--------------------


    IEnumerator RotateToCeiling(float angle)
    {
        isCeilingRotation = true;

        // Record the starting rotation
        Vector3 startPosition = PlayerManager.Instance.playerBody.transform.localPosition;
        Quaternion startRotation = PlayerManager.Instance.playerBody.transform.rotation;

        // Calculate the target rotation
        Vector3 endPosition = new Vector3();
        Quaternion endRotation = new Quaternion();

        //Set endPosition for playerBody
        if (Cameras_v2.Instance.cameraState == CameraState.GameplayCam)
        {
            endPosition = new Vector3(0, -0.2f, 0);
            endRotation = Quaternion.Euler(0, PlayerManager.Instance.playerBody.transform.localRotation.eulerAngles.y, angle);
        }
        else if (Cameras_v2.Instance.cameraState == CameraState.CeilingCam)
        {
            endPosition = new Vector3(0, 0.2f, 0);
            endRotation = Quaternion.Euler(0, PlayerManager.Instance.playerBody.transform.localRotation.eulerAngles.y, angle);
        }

        float elapsed = 0f;

        // Smoothly interpolate the rotation
        while (elapsed < playerRotationDuration_Ceiling)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / playerRotationDuration_Ceiling); // Normalize the time

            PlayerManager.Instance.playerBody.transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            PlayerManager.Instance.playerBody.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);

            playerCeilingRotationValue = PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.z;

            yield return null; // Wait for the next frame
        }

        // Ensure the final rotation is set exactly
        PlayerManager.Instance.playerBody.transform.localPosition = endPosition;
        PlayerManager.Instance.playerBody.transform.rotation = endRotation;

        Player_BlockDetector.Instance.RaycastSetup();

        PlayerManager.Instance.pauseGame = false;
        isCeilingRotation = false;

        if (Cameras_v2.Instance.cameraState == CameraState.GameplayCam)
            isCeilingGrabbing = false;
    }

    public void ResetCeilingGrab()
    {
        if (isCeilingGrabbing)
        {
            playerCeilingRotationValue = 0;
            PlayerManager.Instance.playerBody.transform.rotation = Quaternion.Euler(0, 0, 0);

            Cameras_v2.Instance.cameraState = CameraState.GameplayCam;

            isCeilingGrabbing = false;
        }
    }
}
