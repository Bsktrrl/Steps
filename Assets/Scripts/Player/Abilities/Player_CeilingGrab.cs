using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CeilingGrab : Singleton<Player_CeilingGrab>
{
    public static event Action Action_grabCeiling;
    public static event Action Action_releaseCeiling;

    [SerializeField] bool canCeilingGrab;
    public bool isCeilingGrabbing;
    public bool isCeilingRotation;

    float playerRotationDuration_Ceiling = 0.5f;
    public float playerCeilingRotationValue;

    public float playerBodyHeight_Ceiling = 0.4f;

    RaycastHit hit;

    [SerializeField] GameObject ceilingGrabBlock;


    //--------------------


    private void Update()
    {
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.CeilingGrab && !PlayerStats.Instance.stats.abilitiesGot_Permanent.CeilingGrab) {  return; }

        RaycastCeiling();
        //CeilingGrab();

        if (Player_Movement.Instance.movementStates == MovementStates.Still)
        {
            CheckBlockStandingUnder();
        }
    }

    private void OnEnable()
    {
        Player_Movement.Action_StepTakenEarly += ResetDarkenColor;
    }
    private void OnDisable()
    {
        Player_Movement.Action_StepTakenEarly -= ResetDarkenColor;
    }


    //--------------------


    public void CeilingGrab()
    {
        //Don't be able to switch camera angle before the rotation has been done
        if (!canCeilingGrab) { return; }
        if (CameraController.Instance.isRotating) { return; }
        if (CameraController.Instance.isCeilingRotating) { return; }
        if (Player_Interact.Instance.isInteracting) { return; }
        if (Player_Movement.Instance.isIceGliding) { return; }
        if (Player_Movement.Instance.movementStates == MovementStates.Moving) { return; }

        if (/*Input.GetKeyDown(KeyCode.C) &&*/ CameraController.Instance.cameraState == CameraState.GameplayCam)
        {
            isCeilingGrabbing = true;

            if (CameraController.Instance.cameraRotationState == CameraRotationState.Forward)
                StartCoroutine(CameraController.Instance.CeilingCameraRotation(0));
            else if (CameraController.Instance.cameraRotationState == CameraRotationState.Backward)
                StartCoroutine(CameraController.Instance.CeilingCameraRotation(180));
            else if (CameraController.Instance.cameraRotationState == CameraRotationState.Left)
                StartCoroutine(CameraController.Instance.CeilingCameraRotation(90));
            else if (CameraController.Instance.cameraRotationState == CameraRotationState.Right)
                StartCoroutine(CameraController.Instance.CeilingCameraRotation(-90));

            StartCoroutine(RotateToCeiling(180));
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

            StartCoroutine(RotateToCeiling(0));
        }
    }
    void RaycastCeiling()
    {
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 1))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                if (!isCeilingGrabbing)
                {
                    hit.transform.gameObject.GetComponent<BlockInfo>().DarkenColors();
                    ceilingGrabBlock = hit.transform.gameObject;
                }
                
                canCeilingGrab = true;
                return;
            }
        }

        canCeilingGrab = false;
        ceilingGrabBlock = null;
    }


    //--------------------


    IEnumerator RotateToCeiling(float angle)
    {
        isCeilingRotation = true;

        if (CameraController.Instance.cameraState == CameraState.CeilingCam || CameraController.Instance.cameraState == CameraState.GameplayCam)
        {
            Player_Movement.Instance.Action_ResetBlockColorInvoke();
            Player_BlockDetector.Instance.ReycastReset();
            Player_Movement.Instance.ResetCeilingBlockColor();
        }

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

        // Ensure the final rotation is set exactly
        PlayerManager.Instance.playerBody.transform.localPosition = endPosition;
        PlayerManager.Instance.playerBody.transform.rotation = endRotation;

        //Moving back to ground
        if (CameraController.Instance.cameraState == CameraState.GameplayCam)
        {
            print("1. RotateToGround");
            isCeilingGrabbing = false;
            Player_BlockDetector.Instance.RaycastSetup();

            yield return new WaitForSeconds(0.02f);
            Action_releaseCeiling?.Invoke();
        }
        //Moving to ceiling
        else if (CameraController.Instance.cameraState == CameraState.CeilingCam)
        {
            print("2. RotateToCeiling");
            Player_BlockDetector.Instance.RaycastSetup();
            CheckBlockStandingUnder();
            Player_Movement.Instance.DarkenCeilingBlocks();
            Player_BlockDetector.Instance.ReycastReset();
            Action_grabCeiling?.Invoke();
        }

        PlayerManager.Instance.pauseGame = false;
        isCeilingRotation = false;
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

        if (Player_Movement.Instance.movementStates == MovementStates.Still)
        {
            if (Physics.Raycast(transform.position, Vector3.up, out hit, 1))
            {
                if (hit.transform.GetComponent<BlockInfo>())
                {
                    PlayerManager.Instance.block_StandingOn_Current.block = hit.transform.gameObject;
                    PlayerManager.Instance.block_StandingOn_Current.blockPosition = hit.transform.position;
                    PlayerManager.Instance.block_StandingOn_Current.blockType = hit.transform.GetComponent<BlockInfo>().blockType;

                    if (PlayerManager.Instance.block_StandingOn_Previous != PlayerManager.Instance.block_StandingOn_Current.block)
                    {
                        Player_BlockDetector.Instance.Action_isSwitchingBlocks_Invoke();
                        PlayerManager.Instance.block_StandingOn_Previous = PlayerManager.Instance.block_StandingOn_Current.block;
                    }

                    gameObject.transform.position = PlayerManager.Instance.block_StandingOn_Current.block.transform.position + Vector3.down + (Vector3.down * (1 - Player_Movement.Instance.heightOverBlock));
                }
            }
            else
            {
                PlayerManager.Instance.block_StandingOn_Current.block = null;
                PlayerManager.Instance.block_StandingOn_Current.blockPosition = Vector3.zero;
                PlayerManager.Instance.block_StandingOn_Current.blockType = BlockType.None;
            }
        }
        else if (Player_Movement.Instance.movementStates == MovementStates.Moving)
        {
            if (Physics.Raycast(transform.position, Vector3.up, out hit, 1))
            {
                if (hit.transform.GetComponent<BlockInfo>())
                {
                    PlayerManager.Instance.block_StandingOn_Current.block = hit.transform.gameObject;
                    PlayerManager.Instance.block_StandingOn_Current.blockPosition = hit.transform.position;
                    PlayerManager.Instance.block_StandingOn_Current.blockType = hit.transform.GetComponent<BlockInfo>().blockType;

                    if (PlayerManager.Instance.block_StandingOn_Previous != PlayerManager.Instance.block_StandingOn_Current.block)
                    {
                        Player_BlockDetector.Instance.Action_isSwitchingBlocks_Invoke();
                        PlayerManager.Instance.block_StandingOn_Previous = PlayerManager.Instance.block_StandingOn_Current.block;
                    }

                    //IceBlock
                    if (hit.transform.GetComponent<Block_IceGlide>() && Player_Movement.Instance.movementStates == MovementStates.Moving)
                    {
                        switch (Player_Movement.Instance.lastMovementButtonPressed)
                        {
                            case ButtonsToPress.None:
                                break;
                            case ButtonsToPress.W:
                                Player_Movement.Instance.StartCeilingGrabMovement(Vector3.forward);
                                return;
                            case ButtonsToPress.S:
                                Player_Movement.Instance.StartCeilingGrabMovement(Vector3.back);
                                return;
                            case ButtonsToPress.A:
                                Player_Movement.Instance.StartCeilingGrabMovement(Vector3.left);
                                return;
                            case ButtonsToPress.D:
                                Player_Movement.Instance.StartCeilingGrabMovement(Vector3.right);
                                return;
                            default:
                                break;
                        }
                    }
                }
            }
            else
            {
                PlayerManager.Instance.block_StandingOn_Current.block = null;
                PlayerManager.Instance.block_StandingOn_Current.blockPosition = Vector3.zero;
                PlayerManager.Instance.block_StandingOn_Current.blockType = BlockType.None;
            }

            Player_Movement.Instance.movementStates = MovementStates.Still;
            PlayerManager.Instance.pauseGame = false;
            PlayerManager.Instance.isTransportingPlayer = false;

            Player_Movement.Instance.Action_ResetBlockColorInvoke();
            Player_Movement.Instance.Action_StepTaken_Invoke();
        }
    }

    void ResetDarkenColor()
    {
        if (ceilingGrabBlock)
        {
            ceilingGrabBlock.GetComponent<BlockInfo>().ResetDarkenColor();
        }
    }
}
