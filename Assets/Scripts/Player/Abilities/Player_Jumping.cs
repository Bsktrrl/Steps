using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Jumping : Singleton<Player_Jumping>
{
    [Header("Jumping")]
    [SerializeField] Vector3 jumpStartPos;

    public bool canJump_Forward;
    public bool canJump_Back;
    public bool canJump_Left;
    public bool canJump_Right;

    public GameObject jumpTarget_Forward;
    public GameObject jumpTarget_Back;
    public GameObject jumpTarget_Left;
    public GameObject jumpTarget_Right;

    public bool isJumping = false;

    [Header("Other")]
    RaycastHit hit;

    float jumpHeight = 0.5f;
    float jumpDuration = 0.2f;


    //--------------------


    private void Update()
    {
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.Jumping && !PlayerStats.Instance.stats.abilitiesGot_Permanent.Jumping) { return; }

        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (isJumping) { return; }

        if (Player_Movement.Instance.movementStates == MovementStates.Moving) { return; }
        if (PlayerManager.Instance.pauseGame) { return; }
        if (PlayerManager.Instance.isTransportingPlayer) { return; }

        //CheckIfCanJump_OnAction();
        CheckIfICanJump_Update();

        if (PlayerManager.Instance.forward_isPressed /*&& !PlayerManager.Instance.canMove_Forward*/)
        {
            Jump_Forward();
        }
        else if (PlayerManager.Instance.back_isPressed /*&& !PlayerManager.Instance.canMove_Back*/)
        {
            Jump_Backward();
        }
        else if (PlayerManager.Instance.left_isPressed /*&& !PlayerManager.Instance.canMove_Left*/)
        {
            Jump_Left();
        }
        else if (PlayerManager.Instance.right_isPressed /*&& !PlayerManager.Instance.canMove_Right*/)
        {
            Jump_Right();
        }

        OnElevator();
    }


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += CheckIfCanJump_OnAction;
        Player_Movement.Action_StepTaken += CheckIfCanJump_OnAction;
        Player_Movement.Action_BodyRotated += CheckIfCanJump_OnAction;

        PlayerStats.Action_RespawnPlayer += ResetDarkenOnRespawn;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= CheckIfCanJump_OnAction;
        Player_Movement.Action_StepTaken -= CheckIfCanJump_OnAction;
        Player_Movement.Action_BodyRotated -= CheckIfCanJump_OnAction;

        PlayerStats.Action_RespawnPlayer -= ResetDarkenOnRespawn;
    }


    //--------------------

    void OnElevator()
    {
        if (PlayerManager.Instance.block_StandingOn_Current.block)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_Elevator_Normal>()
            || PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_Elevator_StepOn>())
            {
                CheckIfCanJump_OnAction();
            }
        }
    }
    bool JumpCheck()
    {
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.Jumping && !PlayerStats.Instance.stats.abilitiesGot_Permanent.Jumping) { return false; }

        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return false; }

        if (isJumping) { return false; }

        if (Player_Movement.Instance.movementStates == MovementStates.Moving) { return false; }
        if (PlayerManager.Instance.pauseGame) { return false; }
        if (PlayerManager.Instance.isTransportingPlayer) { return false; }

        return true;
    }
    public void Jump_Forward()
    {
        if (!JumpCheck()) { return; }

        switch (CameraController.Instance.cameraRotationState)
        {
            case CameraRotationState.Forward:
                if (canJump_Forward && jumpTarget_Forward)
                    StartCoroutine(JumpRoutine(jumpTarget_Forward));
                break;
            case CameraRotationState.Backward:
                if (canJump_Back && jumpTarget_Back)
                    StartCoroutine(JumpRoutine(jumpTarget_Back));
                break;
            case CameraRotationState.Left:
                if (canJump_Right && jumpTarget_Right)
                    StartCoroutine(JumpRoutine(jumpTarget_Right));
                break;
            case CameraRotationState.Right:
                if (canJump_Left && jumpTarget_Left)
                    StartCoroutine(JumpRoutine(jumpTarget_Left));
                break;
            default:
                break;
        }
    }
    public void Jump_Backward()
    {
        if (!JumpCheck()) { return; }

        switch (CameraController.Instance.cameraRotationState)
        {
            case CameraRotationState.Forward:
                if (canJump_Back && jumpTarget_Back)
                    StartCoroutine(JumpRoutine(jumpTarget_Back));
                break;
            case CameraRotationState.Backward:
                if (canJump_Forward && jumpTarget_Forward)
                    StartCoroutine(JumpRoutine(jumpTarget_Forward));
                break;
            case CameraRotationState.Left:
                if (canJump_Left && jumpTarget_Left)
                    StartCoroutine(JumpRoutine(jumpTarget_Left));
                break;
            case CameraRotationState.Right:
                if (canJump_Right && jumpTarget_Right)
                    StartCoroutine(JumpRoutine(jumpTarget_Right));
                break;
            default:
                break;
        }
    }
    public void Jump_Left()
    {
        if (!JumpCheck()) { return; }

        switch (CameraController.Instance.cameraRotationState)
        {
            case CameraRotationState.Forward:
                if (canJump_Left && jumpTarget_Left)
                    StartCoroutine(JumpRoutine(jumpTarget_Left));
                break;
            case CameraRotationState.Backward:
                if (canJump_Right && jumpTarget_Right)
                    StartCoroutine(JumpRoutine(jumpTarget_Right));
                break;
            case CameraRotationState.Left:
                if (canJump_Forward && jumpTarget_Forward)
                    StartCoroutine(JumpRoutine(jumpTarget_Forward));
                break;
            case CameraRotationState.Right:
                if (canJump_Back && jumpTarget_Back)
                    StartCoroutine(JumpRoutine(jumpTarget_Back));
                break;
            default:
                break;
        }
    }
    public void Jump_Right()
    {
        if (!JumpCheck()) { return; }

        switch (CameraController.Instance.cameraRotationState)
        {
            case CameraRotationState.Forward:
                if (canJump_Right && jumpTarget_Right)
                    StartCoroutine(JumpRoutine(jumpTarget_Right));
                break;
            case CameraRotationState.Backward:
                if (canJump_Left && jumpTarget_Left)
                    StartCoroutine(JumpRoutine(jumpTarget_Left));
                break;
            case CameraRotationState.Left:
                if (canJump_Back && jumpTarget_Back)
                    StartCoroutine(JumpRoutine(jumpTarget_Back));
                break;
            case CameraRotationState.Right:
                if (canJump_Forward && jumpTarget_Forward)
                    StartCoroutine(JumpRoutine(jumpTarget_Forward));
                break;
            default:
                break;
        }
    }

    void CheckIfCanJump_OnAction()
    {
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.Jumping && !PlayerStats.Instance.stats.abilitiesGot_Permanent.Jumping) { return; }

        ResetTargetBlock(ref jumpTarget_Forward);
        ResetTargetBlock(ref jumpTarget_Back);
        ResetTargetBlock(ref jumpTarget_Left);
        ResetTargetBlock(ref jumpTarget_Right);

        CheckIfICanJump_Update();
    }
    void CheckIfICanJump_Update()
    {
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.Jumping && !PlayerStats.Instance.stats.abilitiesGot_Permanent.Jumping) { return; }

        //Check if can Jump and get JumpTarget
        canJump_Forward = CheckIfCanJump(ref jumpTarget_Forward, Vector3.forward);
        canJump_Back = CheckIfCanJump(ref jumpTarget_Back, Vector3.back);
        canJump_Left = CheckIfCanJump(ref jumpTarget_Left, Vector3.left);
        canJump_Right = CheckIfCanJump(ref jumpTarget_Right, Vector3.right);
    }

    bool CheckIfCanJump(ref GameObject target, Vector3 dir)
    {
        ResetDarkenColorIfStepsIsGone(ref target);

        //Raycast forward +2
        if (Physics.Raycast(gameObject.transform.position, dir, out hit, 2))
        {
            ResetTargetBlock(ref target);
            target = null;
            return false;
        }

        //Raycast down from forward +2
        if (Physics.Raycast(gameObject.transform.position + (dir * 2), Vector3.down, out hit, 1))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                target = hit.transform.gameObject;
            }
        }

        //Raycast down from forward +1 to see if there is a block adjacent
        if (Physics.Raycast(gameObject.transform.position + (dir * 1), Vector3.down, out hit, 1))
        {
            ResetTargetBlock(ref target);
            target = null;
            return false;
        }

        //Darken color in target block
        if (target)
        {
            if (target.GetComponent<BlockInfo>())
            {
                if (!target.GetComponent<BlockInfo>().blockIsDark)
                {
                    target.GetComponent<BlockInfo>().SetDarkenColors();
                }
            }
        }

        ResetDarkenColorIfStepsIsGone(ref target);

        return true;
    }
    void ResetDarkenColorIfStepsIsGone(ref GameObject target)
    {
        //Cannot Jump if having 0 movement and targetBlock has a MovementCost
        if (PlayerStats.Instance.stats.steps_Current <= 0)
        {
            if (target)
            {
                if (target.GetComponent<BlockInfo>().movementCost > 0)
                {
                    target.GetComponent<BlockInfo>().ResetDarkenColor();
                }
            }
        }
    }
    void ResetTargetBlock(ref GameObject target)
    {
        //Reset Darken Color
        if (target)
        {
            target.GetComponent<BlockInfo>().ResetDarkenColor();
            target = null;
        }
    }

    void ResetDarkenOnRespawn()
    {
        ResetTargetBlock(ref jumpTarget_Forward);
        ResetTargetBlock(ref jumpTarget_Back);
        ResetTargetBlock(ref jumpTarget_Left);
        ResetTargetBlock(ref jumpTarget_Right);
    }


    //--------------------


    private IEnumerator JumpRoutine(GameObject target)
    {
        isJumping = true;
        jumpStartPos = gameObject.transform.position;

        Player_Movement.Instance.movementStates = MovementStates.Moving;
        PlayerManager.Instance.pauseGame = true;
        PlayerManager.Instance.isTransportingPlayer = true;

        Vector3 startPosition = PlayerManager.Instance.block_StandingOn_Current.block.transform.position + (Vector3.up * Player_Movement.Instance.heightOverBlock);
        Vector3 endPosition;
        if (target)
            endPosition = target.transform.position + (Vector3.up * (Player_Movement.Instance.heightOverBlock + 0.1f));
        else
            endPosition = jumpStartPos;

        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the progress (0 to 1) of the jump
            float progress = elapsedTime / jumpDuration;

            // Interpolate the forward position
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, progress);

            // Calculate the vertical position to create a parabolic motion
            float height = 4 * jumpHeight * progress * (1 - progress); // Parabola equation
            currentPosition.y += height;

            // Update the player's position
            transform.position = currentPosition;

            yield return null;
        }

        // Ensure the player lands exactly at the end position
        transform.position = endPosition + (Vector3.down * 0.1f);

        //Player_BlockDetector.Instance.RaycastSetup();

        isJumping = false;
        Player_Movement.Instance.movementStates = MovementStates.Still;
        PlayerManager.Instance.pauseGame = false;
        PlayerManager.Instance.isTransportingPlayer = false;

        Player_BlockDetector.Instance.Update_BlockStandingOn();

        Player_Movement.Instance.Action_ResetBlockColorInvoke();
        Player_Movement.Instance.Action_StepTaken_Invoke();
    }
}
