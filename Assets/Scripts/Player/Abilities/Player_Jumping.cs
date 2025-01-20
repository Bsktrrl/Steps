using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Jumping : Singleton<Player_Jumping>
{
    [SerializeField] bool canJump;

    RaycastHit hit;

    float jumpHeight = 0.5f; // Maximum height of the jump
    float jumpDuration = 0.2f; // Duration of the jump

    bool isJumping = false;

    [SerializeField] GameObject jumpTarget;


    //--------------------


    private void Update()
    {
        if (canJump)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                //Player_Movement.Instance.Action_ResetBlockColorInvoke();

                if (Cameras.Instance.cameraState == CameraState.Forward)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.forward)
                        Player_Movement.Instance.lastMovementButtonPressed = ButtonsToPress.W;
                    else if (PlayerManager.Instance.lookingDirection == Vector3.back)
                        Player_Movement.Instance.lastMovementButtonPressed = ButtonsToPress.S;
                    else if (PlayerManager.Instance.lookingDirection == Vector3.left)
                        Player_Movement.Instance.lastMovementButtonPressed = ButtonsToPress.A;
                    else if (PlayerManager.Instance.lookingDirection == Vector3.right)
                        Player_Movement.Instance.lastMovementButtonPressed = ButtonsToPress.D;
                }
                else if (Cameras.Instance.cameraState == CameraState.Backward)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.back)
                        Player_Movement.Instance.lastMovementButtonPressed = ButtonsToPress.W;
                    else if (PlayerManager.Instance.lookingDirection == Vector3.forward)
                        Player_Movement.Instance.lastMovementButtonPressed = ButtonsToPress.S;
                    else if (PlayerManager.Instance.lookingDirection == Vector3.right)
                        Player_Movement.Instance.lastMovementButtonPressed = ButtonsToPress.A;
                    else if (PlayerManager.Instance.lookingDirection == Vector3.left)
                        Player_Movement.Instance.lastMovementButtonPressed = ButtonsToPress.D;
                }
                else if (Cameras.Instance.cameraState == CameraState.Left)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.right)
                        Player_Movement.Instance.lastMovementButtonPressed = ButtonsToPress.W;
                    else if (PlayerManager.Instance.lookingDirection == Vector3.left)
                        Player_Movement.Instance.lastMovementButtonPressed = ButtonsToPress.S;
                    else if (PlayerManager.Instance.lookingDirection == Vector3.forward)
                        Player_Movement.Instance.lastMovementButtonPressed = ButtonsToPress.A;
                    else if (PlayerManager.Instance.lookingDirection == Vector3.back)
                        Player_Movement.Instance.lastMovementButtonPressed = ButtonsToPress.D;
                }
                else if (Cameras.Instance.cameraState == CameraState.Right)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.left)
                        Player_Movement.Instance.lastMovementButtonPressed = ButtonsToPress.W;
                    else if (PlayerManager.Instance.lookingDirection == Vector3.right)
                        Player_Movement.Instance.lastMovementButtonPressed = ButtonsToPress.S;
                    else if (PlayerManager.Instance.lookingDirection == Vector3.back)
                        Player_Movement.Instance.lastMovementButtonPressed = ButtonsToPress.A;
                    else if (PlayerManager.Instance.lookingDirection == Vector3.forward)
                        Player_Movement.Instance.lastMovementButtonPressed = ButtonsToPress.D;
                }

                StartCoroutine(JumpRoutine());
            }
        }
    }


    //--------------------


    private void OnEnable()
    {
        Player_Movement.Action_StepTaken += CheckIfCanJump;
        Player_Movement.Action_BodyRotated += CheckIfCanJump;
    }
    private void OnDisable()
    {
        Player_Movement.Action_StepTaken -= CheckIfCanJump;
        Player_Movement.Action_BodyRotated -= CheckIfCanJump;
    }


    //--------------------


    void CheckIfCanJump()
    {
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.Jumping && !PlayerStats.Instance.stats.abilitiesGot_Permanent.Jumping) { return; }

        //Raycast forward +2
        if (Physics.Raycast(gameObject.transform.position, PlayerManager.Instance.lookingDirection, out hit, 2))
        {
            canJump = false;
            jumpTarget = null;
            return;
        }

        //Raycast down from forward +2
        if (Physics.Raycast(gameObject.transform.position + (PlayerManager.Instance.lookingDirection * 2), Vector3.down, out hit, 1))
        {
            canJump = true;
            jumpTarget = hit.transform.gameObject;
        }
        else
        {
            canJump = false;
        }

        //Raycast down from forward +1 to see if there is a block adjacent
        if (Physics.Raycast(gameObject.transform.position + (PlayerManager.Instance.lookingDirection * 1), Vector3.down, out hit, 1))
        {
            canJump = false;
            jumpTarget = null;
        }

        if (jumpTarget)
        {
            if (jumpTarget.GetComponent<BlockInfo>())
            {
                jumpTarget.GetComponent<BlockInfo>().DarkenColors();
            }
        }

        //Cannot Jump if having 0 movement and targetBlock has a MovementCost
        if (PlayerStats.Instance.stats.steps_Current <= 0)
        {
            if (jumpTarget)
            {
                if (jumpTarget.GetComponent<BlockInfo>().movementCost > 0)
                {
                    jumpTarget.GetComponent<BlockInfo>().ResetColor();
                    canJump = false;
                }
            }
        }
    }


    //--------------------


    private IEnumerator JumpRoutine()
    {
        isJumping = true;
        Player_Movement.Instance.movementStates = MovementStates.Moving;
        PlayerManager.Instance.pauseGame = true;
        PlayerManager.Instance.isTransportingPlayer = true;

        Vector3 startPosition = PlayerManager.Instance.block_StandingOn_Current.block.transform.position + (Vector3.up * Player_Movement.Instance.heightOverBlock);
        Vector3 endPosition = jumpTarget.transform.position + (Vector3.up * (Player_Movement.Instance.heightOverBlock + 0.2f));

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
        transform.position = endPosition;

        Player_BlockDetector.Instance.RaycastSetup();
        //CheckIfCanJump();

        transform.position = PlayerManager.Instance.block_StandingOn_Current.blockPosition + (Vector3.up * Player_Movement.Instance.heightOverBlock);

        isJumping = false;
        Player_Movement.Instance.movementStates = MovementStates.Still;
        PlayerManager.Instance.pauseGame = false;
        PlayerManager.Instance.isTransportingPlayer = false;

        Player_BlockDetector.Instance.Update_BlockStandingOn();

        Player_Movement.Instance.Action_ResetBlockColorInvoke();
        Player_Movement.Instance.Action_StepTaken_Invoke();
    }
}
