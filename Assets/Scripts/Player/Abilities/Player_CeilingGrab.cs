using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_CeilingGrab : Singleton<Player_CeilingGrab>
{
    public static event Action Action_grabCeiling;
    public static event Action Action_releaseCeiling;
    public static event Action Action_raycastCeiling;

    [SerializeField] bool canCeilingGrab;
    public bool isCeilingGrabbing;
    public bool isCeilingRotation;
    public bool isCeilingRotation_OFF;

    float playerRotationDuration_Ceiling = 0.5f;
    public float playerCeilingRotationValue;

    public float playerBodyHeight_Ceiling = 0.4f;

    RaycastHit hit;

    public GameObject ceilingGrabBlock;


    //--------------------


    private void OnEnable()
    {
        Movement.Action_StepTaken += UpdateRaycastCeiling;
        DataManager.Action_dataHasLoaded += UpdateRaycastCeiling;
        Movement.Action_RespawnPlayerEarly += ResetStats;
    }
    private void OnDisable()
    {
        Movement.Action_StepTaken -= UpdateRaycastCeiling;
        DataManager.Action_dataHasLoaded -= UpdateRaycastCeiling;
        Movement.Action_RespawnPlayerEarly -= ResetStats;
    }


    //--------------------

    void UpdateRaycastCeiling()
    {
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.CeilingGrab && !PlayerStats.Instance.stats.abilitiesGot_Permanent.CeilingGrab) { return; }

        RaycastCeiling();
        //CeilingGrab();

        if (Movement.Instance.GetMovementState() == MovementStates.Still)
        {
            CheckBlockStandingUnder();
        }
    }
    public void CeilingGrab()
    {
        //Don't be able to switch camera angle before the rotation has been done
        if (!canCeilingGrab) { return; }
        if (CameraController.Instance.isRotating) { return; }
        if (CameraController.Instance.isCeilingRotating) { return; }
        if (Player_Interact.Instance.isInteracting) { return; }
        //if (Player_Movement.Instance.isIceGliding) { return; }
        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return; }

        if (/*Input.GetKeyDown(KeyCode.C) &&*/ CameraController.Instance.cameraState == CameraState.GameplayCam)
        {
            isCeilingGrabbing = true;
            Movement.Instance.heightOverBlock = Movement.Instance.heightOverBlock;

            if (CameraController.Instance.cameraRotationState == CameraRotationState.Forward)
                StartCoroutine(CameraController.Instance.CeilingCameraRotation(0));
            else if (CameraController.Instance.cameraRotationState == CameraRotationState.Backward)
                StartCoroutine(CameraController.Instance.CeilingCameraRotation(180));
            else if (CameraController.Instance.cameraRotationState == CameraRotationState.Left)
                StartCoroutine(CameraController.Instance.CeilingCameraRotation(90));
            else if (CameraController.Instance.cameraRotationState == CameraRotationState.Right)
                StartCoroutine(CameraController.Instance.CeilingCameraRotation(-90));

            StartCoroutine(RotateToOrFromCeiling(180));
        }
        else if (/*Input.GetKeyDown(KeyCode.C) &&*/ CameraController.Instance.cameraState == CameraState.CeilingCam)
        {
            if (CameraController.Instance.cameraRotationState == CameraRotationState.Forward)
                StartCoroutine(CameraController.Instance.CeilingCameraRotation(0));
            else if (CameraController.Instance.cameraRotationState == CameraRotationState.Backward)
                StartCoroutine(CameraController.Instance.CeilingCameraRotation(180));
            else if (CameraController.Instance.cameraRotationState == CameraRotationState.Left)
                StartCoroutine(CameraController.Instance.CeilingCameraRotation(90));
            else if (CameraController.Instance.cameraRotationState == CameraRotationState.Right)
                StartCoroutine(CameraController.Instance.CeilingCameraRotation(-90));

            StartCoroutine(RotateToOrFromCeiling(0));
        }
    }
    public void RaycastCeiling()
    {
        GameObject outObject1;

        if (Movement.Instance.PerformMovementRaycast(transform.position, Vector3.up, 1, out outObject1) == RaycastHitObjects.BlockInfo)
        {
            if (!isCeilingGrabbing)
            {
                outObject1.GetComponent<BlockInfo>().SetDarkenColors();
                ceilingGrabBlock = outObject1;

                Action_raycastCeiling?.Invoke();
            }

            outObject1.GetComponent<BlockInfo>().ResetDarkenColor();

            canCeilingGrab = true;
            return;
        }

        canCeilingGrab = false;
        ceilingGrabBlock = null;
    }


    //--------------------


    IEnumerator RotateToOrFromCeiling(float angle)
    {
        isCeilingRotation = true;

        // Record the starting rotation
        Vector3 startPosition = PlayerManager.Instance.playerBody.transform.localPosition;
        Quaternion startRotation = PlayerManager.Instance.playerBody.transform.rotation;

        // Calculate the target rotation
        Vector3 endPosition = new Vector3();
        Quaternion endRotation = new Quaternion();

        //Set endPosition for playerBody
        if (CameraController.Instance.cameraState == CameraState.GameplayCam)
        {
            endPosition = new Vector3(0, Player_BodyHeight.Instance.height_Normal, 0);
            endRotation = Quaternion.Euler(0, PlayerManager.Instance.playerBody.transform.localRotation.eulerAngles.y, angle);
        }
        else if (CameraController.Instance.cameraState == CameraState.CeilingCam)
        {
            endPosition = new Vector3(0, playerBodyHeight_Ceiling, 0);
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

        if (CameraController.Instance.cameraState == CameraState.GameplayCam)
            isCeilingRotation_OFF = true;
        else if (CameraController.Instance.cameraState == CameraState.CeilingCam)
            isCeilingRotation_OFF = false;

        // Ensure the final rotation is set exactly
        PlayerManager.Instance.playerBody.transform.localPosition = endPosition;
        PlayerManager.Instance.playerBody.transform.rotation = endRotation;

        //Moving back to ground
        if (CameraController.Instance.cameraState == CameraState.GameplayCam)
        {
            //print("1. RotateToGround");
            isCeilingGrabbing = false;

            yield return new WaitForSeconds(0.02f);
            Action_releaseCeiling?.Invoke();
        }
        //Moving to ceiling
        else if (CameraController.Instance.cameraState == CameraState.CeilingCam)
        {
            //print("2. RotateToCeiling");
            CheckBlockStandingUnder();
            Action_grabCeiling?.Invoke();
        }

        PlayerManager.Instance.pauseGame = false;
        isCeilingRotation = false;

        Action_raycastCeiling?.Invoke();
        Movement.Instance.Action_BodyRotated_Invoke();
        Movement.Instance.UpdateAvailableMovementBlocks();
        RaycastCeiling();

        if (CameraController.Instance.cameraState == CameraState.GameplayCam)
            isCeilingRotation_OFF = false;
    }

    public void ResetCeilingGrab()
    {
        if (isCeilingGrabbing)
        {
            playerCeilingRotationValue = 0;
            PlayerManager.Instance.playerBody.transform.rotation = Quaternion.Euler(0, 0, 0);

            isCeilingGrabbing = false;
        }
    }

    public void CheckBlockStandingUnder()
    {
        if (!isCeilingGrabbing) { return; }

        if (Movement.Instance.GetMovementState() == MovementStates.Still)
        {
            if (Physics.Raycast(transform.position, Vector3.up, out hit, 1, MapManager.Instance.pickup_LayerMask))
            {
                if (hit.transform.GetComponent<BlockInfo>())
                {
                    Movement.Instance.blockStandingOn = hit.transform.gameObject;
                    Movement.Instance.blockStandingOn.transform.position = hit.transform.position;
                    Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockType = hit.transform.GetComponent<BlockInfo>().blockType;

                    if (Movement.Instance.blockStandingOn_Previous != Movement.Instance.blockStandingOn)
                    {
                        Movement.Instance.Action_isSwitchingBlocks_Invoke();
                        Movement.Instance.blockStandingOn_Previous = Movement.Instance.blockStandingOn;
                    }

                    gameObject.transform.position = Movement.Instance.blockStandingOn.transform.position + Vector3.down + (Vector3.down * (1 - Movement.Instance.heightOverBlock));
                }
            }
            else
            {
                Movement.Instance.blockStandingOn = null;
                Movement.Instance.blockStandingOn.transform.position = Vector3.zero;
                Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockType = BlockType.None;
            }
        }
        else if (Movement.Instance.GetMovementState() == MovementStates.Moving)
        {
            if (Physics.Raycast(transform.position, Vector3.up, out hit, 1, MapManager.Instance.pickup_LayerMask))
            {
                if (hit.transform.GetComponent<BlockInfo>())
                {
                    Movement.Instance.blockStandingOn = hit.transform.gameObject;
                    Movement.Instance.blockStandingOn.transform.position = hit.transform.position;
                    Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockType = hit.transform.GetComponent<BlockInfo>().blockType;

                    if (Movement.Instance.blockStandingOn_Previous != Movement.Instance.blockStandingOn)
                    {
                        Movement.Instance.Action_isSwitchingBlocks_Invoke();
                        Movement.Instance.blockStandingOn_Previous = Movement.Instance.blockStandingOn;
                    }

                    //IceBlock
                    //if (hit.transform.GetComponent<Block_IceGlide>() && Movement.Instance.GetMovementState() == MovementStates.Moving)
                    //{
                    //    switch (Player_Movement.Instance.lastMovementButtonPressed)
                    //    {
                    //        case ButtonsToPress.None:
                    //            break;
                    //        case ButtonsToPress.W:
                    //            Player_Movement.Instance.StartCeilingGrabMovement(Vector3.forward);
                    //            return;
                    //        case ButtonsToPress.S:
                    //            Player_Movement.Instance.StartCeilingGrabMovement(Vector3.back);
                    //            return;
                    //        case ButtonsToPress.A:
                    //            Player_Movement.Instance.StartCeilingGrabMovement(Vector3.left);
                    //            return;
                    //        case ButtonsToPress.D:
                    //            Player_Movement.Instance.StartCeilingGrabMovement(Vector3.right);
                    //            return;
                    //        default:
                    //            break;
                    //    }
                    //}
                }
            }
            else
            {
                Movement.Instance.blockStandingOn = null;
                Movement.Instance.blockStandingOn.transform.position = Vector3.zero;
                Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockType = BlockType.None;
            }

            Movement.Instance.SetMovementState(MovementStates.Still);
            PlayerManager.Instance.pauseGame = false;

            Movement.Instance.Action_StepTaken_Invoke();
        }
    }

    void ResetDarkenColor()
    {
        if (ceilingGrabBlock)
        {
            ceilingGrabBlock.GetComponent<BlockInfo>().ResetDarkenColor();
        }
    }


    //--------------------


    void ResetStats()
    {
        ResetCeilingGrab();

        canCeilingGrab = false;
        isCeilingGrabbing = false;
        isCeilingRotation = false;
        ceilingGrabBlock = null;
    }
}
