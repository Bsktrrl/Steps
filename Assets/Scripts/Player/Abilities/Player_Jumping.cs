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
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_Elevator>()
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
                    StartCoroutine(PerformJump(jumpTarget_Forward));
                break;
            case CameraRotationState.Backward:
                if (canJump_Back && jumpTarget_Back)
                    StartCoroutine(PerformJump(jumpTarget_Back));
                break;
            case CameraRotationState.Left:
                if (canJump_Right && jumpTarget_Right)
                    StartCoroutine(PerformJump(jumpTarget_Right));
                break;
            case CameraRotationState.Right:
                if (canJump_Left && jumpTarget_Left)
                    StartCoroutine(PerformJump(jumpTarget_Left));
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
                    StartCoroutine(PerformJump(jumpTarget_Back));
                break;
            case CameraRotationState.Backward:
                if (canJump_Forward && jumpTarget_Forward)
                    StartCoroutine(PerformJump(jumpTarget_Forward));
                break;
            case CameraRotationState.Left:
                if (canJump_Left && jumpTarget_Left)
                    StartCoroutine(PerformJump(jumpTarget_Left));
                break;
            case CameraRotationState.Right:
                if (canJump_Right && jumpTarget_Right)
                    StartCoroutine(PerformJump(jumpTarget_Right));
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
                    StartCoroutine(PerformJump(jumpTarget_Left));
                break;
            case CameraRotationState.Backward:
                if (canJump_Right && jumpTarget_Right)
                    StartCoroutine(PerformJump(jumpTarget_Right));
                break;
            case CameraRotationState.Left:
                if (canJump_Forward && jumpTarget_Forward)
                    StartCoroutine(PerformJump(jumpTarget_Forward));
                break;
            case CameraRotationState.Right:
                if (canJump_Back && jumpTarget_Back)
                    StartCoroutine(PerformJump(jumpTarget_Back));
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
                    StartCoroutine(PerformJump(jumpTarget_Right));
                break;
            case CameraRotationState.Backward:
                if (canJump_Left && jumpTarget_Left)
                    StartCoroutine(PerformJump(jumpTarget_Left));
                break;
            case CameraRotationState.Left:
                if (canJump_Back && jumpTarget_Back)
                    StartCoroutine(PerformJump(jumpTarget_Back));
                break;
            case CameraRotationState.Right:
                if (canJump_Forward && jumpTarget_Forward)
                    StartCoroutine(PerformJump(jumpTarget_Forward));
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

        RaycastHit hit;

        // Step 0: Check if player is standing on a stair/slope and is jumping in the same direction
        if (PlayerManager.Instance.block_StandingOn_Current.blockType == BlockType.Stair || PlayerManager.Instance.block_StandingOn_Current.blockType == BlockType.Slope)
        {
            // Get forward of the block in XZ
            Vector3 stairForward = PlayerManager.Instance.block_StandingOn_Current.block.transform.forward;
            stairForward.y = 0;
            stairForward.Normalize();

            // Get jump direction in XZ
            Vector3 jumpDirXZ = dir;
            jumpDirXZ.y = 0;
            jumpDirXZ.Normalize();

            float dot = Vector3.Dot(stairForward, jumpDirXZ);
            //Debug.Log($"Jump direction dot with stair under player: {dot}");

            if (dot < 0.7f)
            {
                Debug.Log("Player is standing on stair/slope but not jumping in its forward direction. Jump blocked.");
                ResetTargetBlock(ref target);
                target = null;
                return false;
            }
        }


        // Step 1: Raycast forward +2 to see what we are jumping over/into
        if (Physics.Raycast(transform.position, dir, out hit, 2f, MapManager.Instance.pickup_LayerMask))
        {
            var blockInfo = hit.transform.GetComponent<BlockInfo>();

            if (blockInfo != null)
            {
                //Check if hitting a Stair or Slope
                if (blockInfo.blockType == BlockType.Stair || blockInfo.blockType == BlockType.Slope)
                {
                    Transform blockTransform = hit.transform;
                    Transform playerTransform = PlayerManager.Instance.playerBody.transform;

                    // Vector from stair to player, XZ only
                    Vector3 toPlayer = playerTransform.position - blockTransform.position;
                    toPlayer.y = 0;
                    toPlayer.Normalize();

                    // Stair's forward vector in XZ
                    Vector3 stairForward = blockTransform.forward;
                    stairForward.y = 0;
                    stairForward.Normalize();

                    float dot = Vector3.Dot(stairForward, toPlayer);
                    //Debug.Log($"Stair to player dot: {dot}");

                    if (dot < 0.7f) // stair is NOT facing the player
                    {
                        //Debug.Log("Stair is not facing the player. Jump blocked.");
                        ResetTargetBlock(ref target);
                        target = null;
                        return false;
                    }
                }
                else
                {
                    ResetTargetBlock(ref target);
                    target = null;
                    return false;
                }
            }
            else
            {
                ResetTargetBlock(ref target);
                target = null;
                return false;
            }
        }

        // Step 2: Raycast down from forward+2 (landing spot)
        if (Physics.Raycast(transform.position + dir * 2f, Vector3.down, out hit, 1f, MapManager.Instance.pickup_LayerMask))
        {
            var blockInfo = hit.transform.GetComponent<BlockInfo>();

            if (blockInfo == null ||
                blockInfo.blockElement == BlockElement.Lava ||
                (blockInfo.blockElement == BlockElement.Water && !PlayerHasSwimAbility()))
            {
                ResetTargetBlock(ref target);
                target = null;
                return false;
            }
            else if (blockInfo.blockType == BlockType.Stair ||
                     blockInfo.blockType == BlockType.Slope)
            {
                //Allow jumping ONLY when Stair and Slope is on the same hight level as the player
                if ((transform.position.y - blockInfo.gameObject.transform.transform.position.y) > 0.445f && (transform.position.y - blockInfo.gameObject.transform.transform.position.y) < 0.455f)
                {
                    print("1. Stair Jump: " + (transform.position.y - blockInfo.gameObject.transform.transform.position.y));
                    target = hit.transform.gameObject;
                }
                else
                {
                    print("2. Stair Jump: " + (transform.position.y - blockInfo.gameObject.transform.transform.position.y));
                    ResetTargetBlock(ref target);
                    target = null;
                    return false;
                }
            }
            else if (PlayerStats.Instance.stats.steps_Current <= 0 && blockInfo.movementCost > 0)
            {
                ResetTargetBlock(ref target);
                target = null;
                return false;
            }
            else
            {
                target = hit.transform.gameObject;
            }
        }

        // Step 3: Raycast down from forward+1 (block between player and stair)
        if (Physics.Raycast(transform.position + dir, Vector3.down, out hit, 1f, MapManager.Instance.pickup_LayerMask))
        {
            var blockInfo = hit.transform.GetComponent<BlockInfo>();

            if (blockInfo == null)
            {
                ResetTargetBlock(ref target);
                target = null;
                return false;
            }

            else if (blockInfo.blockElement != BlockElement.Lava &&
                !(blockInfo.blockElement == BlockElement.Water && !PlayerHasSwimAbility()))
            {
                ResetTargetBlock(ref target);
                target = null;
                return false;
            }
        }

        // Step 4: Darken target block
        if (target)
        {
            var info = target.GetComponent<BlockInfo>();
            if (info != null && !info.blockIsDark)
            {
                info.SetDarkenColors();
            }
        }

        ResetDarkenColorIfStepsIsGone(ref target);
        return true;
    }

    bool PlayerHasSwimAbility()
    {
        var stats = PlayerStats.Instance.stats;
        return stats.abilitiesGot_Permanent.SwimSuit ||
               stats.abilitiesGot_Permanent.Flippers ||
               stats.abilitiesGot_Permanent.SwiftSwim ||
               stats.abilitiesGot_Temporary.SwimSuit ||
               stats.abilitiesGot_Temporary.Flippers ||
               stats.abilitiesGot_Temporary.SwiftSwim;
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


    private IEnumerator PerformJump(GameObject target)
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
