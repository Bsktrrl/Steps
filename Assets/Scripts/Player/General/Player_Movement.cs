using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Movement : Singleton<Player_Movement>
{
    public static event Action Action_StepTakenEarly;
    public static event Action Action_StepTaken;
    public static event Action Action_StepCostTaken;
    public static event Action Action_BodyRotated;
    public static event Action Action_resetBlockColor;
    public static event Action Action_PressMoveBlockButton;
    public static event Action Action_LandedFromFalling;

    [Header("Current Movement Cost")]
    public int currentMovementCost;

    [Header("Movement State")]
    public MovementStates movementStates;
    public ButtonsToPress lastMovementButtonPressed;

    [Header("Player Movement over Blocks")]
    [HideInInspector] public float movementSpeed = 0.2f;
    [HideInInspector] public float heightOverBlock = 0.95f;
    [HideInInspector] public float fallSpeed = 6f;

    [Header("Gliding")]
    public bool isIceGliding;
    public bool isSlopeGliding;
    [HideInInspector] Vector3 endDestination;

    [Header("CeilingGrab")]
    RaycastHit hit;

    [Header("Ladder")]
    public Vector3 ladderEndPos_Up;
    public Vector3 ladderEndPos_Down;
    public bool isMovingOnLadder_Up;
    public bool isMovingOnLadder_Down;

    Vector3 ladderClimbPos_Start;
    Vector3 ladderClimbPos_End;
    public int ladderPartsToClimb;
    [SerializeField] Quaternion ladderToEnterRot;


    //--------------------


    private void Start()
    {
        FindLadderExitBlock();
    }
    private void Update()
    {
        if (PlayerManager.Instance.forward_isPressed)
            Key_MoveForward();
        else if (PlayerManager.Instance.back_isPressed)
            Key_MoveBackward();
        else if(PlayerManager.Instance.left_isPressed)
            Key_MoveLeft();
        else if(PlayerManager.Instance.right_isPressed)
            Key_MoveRight();

        if (Player_GraplingHook.Instance.isGrapplingHooking) { return; }
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (movementStates == MovementStates.Moving
            && !Player_SwiftSwim.Instance.isSwiftSwimming_Up && !Player_SwiftSwim.Instance.isSwiftSwimming_Down
            && !Player_Ascend.Instance.isAscending && !Player_Descend.Instance.isDescending
            && !Player_Dash.Instance.isDashing
            && !isSlopeGliding)
        {
            Action_StepTakenEarly_Invoke();

            MovePlayer();
            PlayerHover();
        }
        else if (movementStates == MovementStates.Falling
            && !Player_SwiftSwim.Instance.isSwiftSwimming_Up && !Player_SwiftSwim.Instance.isSwiftSwimming_Down
            && !Player_Ascend.Instance.isAscending && !Player_Descend.Instance.isDescending
            && !Player_Dash.Instance.isDashing
            && !isSlopeGliding)
        {
            PlayerHover();
        }
        else if (Player_SwiftSwim.Instance.isSwiftSwimming_Up || Player_SwiftSwim.Instance.isSwiftSwimming_Down)
        {

        }
        else if (Player_Ascend.Instance.isAscending || Player_Descend.Instance.isDescending)
        {

        }
        else
        {
            movementStates = MovementStates.Still;
            PlayerHover();
        }
    }


    //--------------------


    private void OnEnable()
    {
        Action_StepTaken += IceGlide;
        Action_StepTaken += SlopeGlide;

        Action_StepTaken += DarkenCeilingBlocks;

        Action_StepTaken += FindLadderExitBlock;
        PlayerStats.Action_RespawnPlayer += FindLadderExitBlock;
    }

    private void OnDisable()
    {
        Action_StepTaken -= IceGlide;
        Action_StepTaken -= SlopeGlide;

        Action_StepTaken -= DarkenCeilingBlocks;

        Action_StepTaken -= FindLadderExitBlock;
        PlayerStats.Action_RespawnPlayer -= FindLadderExitBlock;
    }


    //--------------------


    bool KeyInputsChecks()
    {
        if (movementStates == MovementStates.Moving) { return false; }

        if (PlayerManager.Instance.pauseGame) { return false; }
        if (PlayerManager.Instance.isTransportingPlayer) { return false; }
        if (CameraController.Instance.isRotating) { return false; }
        if (Player_Interact.Instance.isInteracting) { return false; }
        if (Player_GraplingHook.Instance.isGrapplingHooking) { return false; }

        return true;
    }
    public void Key_MoveForward()
    {
        if (!KeyInputsChecks()) { return; }

        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            lastMovementButtonPressed = ButtonsToPress.W;
            StartCeilingGrabMovement(Vector3.forward);
        }
        else if (CheckLaddersToEnter_Up(DirectionCalculator(Vector3.forward)))
        {
            StartCoroutine(PerformLadderMovement_Up(DirectionCalculator(Vector3.forward), GetLadderExitPart_Up(DirectionCalculator(Vector3.forward))));
        }
        else if (CheckLaddersToEnter_Down(DirectionCalculator(Vector3.forward)))
        {
            StartCoroutine(PerformLadderMovement_Down(DirectionCalculator(Vector3.forward), GetLadderExitPart_Down(DirectionCalculator(Vector3.forward))));
        }
        else
        {
            PrepareMovement(ButtonsToPress.W, PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
        }
    }
    public void Key_MoveBackward()
    {
        if (!KeyInputsChecks()) { return; }

        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            lastMovementButtonPressed = ButtonsToPress.S;
            StartCeilingGrabMovement(Vector3.back);
        }
        else if (CheckLaddersToEnter_Up(DirectionCalculator(Vector3.back)))
        {
            StartCoroutine(PerformLadderMovement_Up(DirectionCalculator(Vector3.back), GetLadderExitPart_Up(DirectionCalculator(Vector3.back))));
        }
        else if (CheckLaddersToEnter_Down(DirectionCalculator(Vector3.back)))
        {
            StartCoroutine(PerformLadderMovement_Down(DirectionCalculator(Vector3.back), GetLadderExitPart_Down(DirectionCalculator(Vector3.back))));
        }
        else
        {
            PrepareMovement(ButtonsToPress.S, PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);
        }
    }
    public void Key_MoveLeft()
    {
        if (!KeyInputsChecks()) { return; }

        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            lastMovementButtonPressed = ButtonsToPress.A;
            StartCeilingGrabMovement(Vector3.left);
        }
        else if (CheckLaddersToEnter_Up(DirectionCalculator(Vector3.left)))
        {
            StartCoroutine(PerformLadderMovement_Up(DirectionCalculator(Vector3.left), GetLadderExitPart_Up(DirectionCalculator(Vector3.left))));
        }
        else if (CheckLaddersToEnter_Down(DirectionCalculator(Vector3.left)))
        {
            StartCoroutine(PerformLadderMovement_Down(DirectionCalculator(Vector3.left), GetLadderExitPart_Down(DirectionCalculator(Vector3.left))));
        }
        else
        {
            PrepareMovement(ButtonsToPress.A, PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
        }
    }
    public void Key_MoveRight()
    {
        if (!KeyInputsChecks()) { return; }

        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            lastMovementButtonPressed = ButtonsToPress.D;
            StartCeilingGrabMovement(Vector3.right);
        }
        else if (CheckLaddersToEnter_Up(DirectionCalculator(Vector3.right)))
        {
            StartCoroutine(PerformLadderMovement_Up(DirectionCalculator(Vector3.right), GetLadderExitPart_Up(DirectionCalculator(Vector3.right))));
        }
        else if (CheckLaddersToEnter_Down(DirectionCalculator(Vector3.right)))
        {
            StartCoroutine(PerformLadderMovement_Down(DirectionCalculator(Vector3.right), GetLadderExitPart_Down(DirectionCalculator(Vector3.right))));
        }
        else
        {
            PrepareMovement(ButtonsToPress.D, PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
        }
    }


    public void Key_SwiftSwimUp()
    {
        if (!KeyInputsChecks()) { return; }

        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (gameObject.GetComponent<Player_SwiftSwim>().canSwiftSwim_Up)
        {
            gameObject.GetComponent<Player_SwiftSwim>().SwiftSwim_Up();
        }
    }
    public void Key_SwiftSwimDown()
    {
        if (!KeyInputsChecks()) { return; }

        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (gameObject.GetComponent<Player_SwiftSwim>().canSwiftSwim_Down)
        {
            gameObject.GetComponent<Player_SwiftSwim>().SwiftSwim_Down();
        }
    }

    public void Key_Respawn()
    {
        if (!KeyInputsChecks()) { return; }

        PlayerStats.Instance.RespawnPlayer();

        Action_StepTaken_Invoke();
    }
    public void Key_Quit()
    {
        if (!KeyInputsChecks()) { return; }

        QuitLevel();
    }


    //--------------------


    void PrepareMovement(ButtonsToPress button, bool canMove, DetectedBlockInfo detectedBlock, int rotation)
    {
        lastMovementButtonPressed = button;
        MovementKeyIsPressed(canMove, detectedBlock, rotation);
    }
    void MovementKeyIsPressed(bool canMove, DetectedBlockInfo block_Vertical, int rotation)
    {
        if (block_Vertical.block)
        {
            if (PlayerStats.Instance.stats.steps_Current <= 0 && block_Vertical.block.GetComponent<BlockInfo>().movementCost > 0)
            {
                PlayerStats.Instance.RespawnPlayer();
                return;
            }
        }
        
        if (canMove)
        {
            if (block_Vertical != null)
            {
                if (block_Vertical.block != null)
                {
                    if (block_Vertical.block.GetComponent<BlockInfo>())
                    {
                        SetPlayerBodyRotation(rotation);

                        if (block_Vertical.block.GetComponent<BlockInfo>().movementCost > PlayerStats.Instance.stats.steps_Current)
                        {
                            return;
                        }

                        PlayerManager.Instance.block_MovingTowards = block_Vertical;

                        endDestination = block_Vertical.blockPosition + (Vector3.up * heightOverBlock);
                        movementStates = MovementStates.Moving;

                        Action_resetBlockColor?.Invoke();

                        return;
                    }
                }
            }
        }

        movementStates = MovementStates.Still;
        SetPlayerBodyRotation(rotation);
    }
    public void SetPlayerBodyRotation(float rotationValue)
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        //Ladder Rotation - Rotate together with the ladder
        if (isMovingOnLadder_Up || isMovingOnLadder_Down)
        {
            if (rotationValue == int.MinValue)
            {
                PlayerManager.Instance.playerBody.transform.SetPositionAndRotation(PlayerManager.Instance.playerBody.transform.position, ladderToEnterRot);
            }
            else
            {
                PlayerManager.Instance.playerBody.transform.SetPositionAndRotation(PlayerManager.Instance.playerBody.transform.position, ladderToEnterRot * Quaternion.Euler(0, 180, 0));
            }
        }

        //Normal Rotation
        else
        {
            //Set new Rotation - Based on the key input
            switch (CameraController.Instance.cameraRotationState)
            {
                case CameraRotationState.Forward:
                    PlayerManager.Instance.playerBody.transform.SetPositionAndRotation(PlayerManager.Instance.playerBody.transform.position, Quaternion.Euler(0, 0 + rotationValue, 0));

                    if (rotationValue == 0 || rotationValue == 360)
                        CameraController.Instance.directionFacing = Vector3.forward;
                    else if (rotationValue == 180)
                        CameraController.Instance.directionFacing = Vector3.back;
                    else if (rotationValue == 90)
                        CameraController.Instance.directionFacing = Vector3.right;
                    else if (rotationValue == -90 || rotationValue == 270)
                        CameraController.Instance.directionFacing = Vector3.left;
                    break;
                case CameraRotationState.Backward:
                    PlayerManager.Instance.playerBody.transform.SetPositionAndRotation(PlayerManager.Instance.playerBody.transform.position, Quaternion.Euler(0, 180 + rotationValue, 0));

                    if (180 + rotationValue == 0 || 180 + rotationValue == 360)
                        CameraController.Instance.directionFacing = Vector3.back;
                    else if (180 + rotationValue == 180)
                        CameraController.Instance.directionFacing = Vector3.forward;
                    else if (180 + rotationValue == 90)
                        CameraController.Instance.directionFacing = Vector3.left;
                    else if (180 + rotationValue == -90 || 180 + rotationValue == 270)
                        CameraController.Instance.directionFacing = Vector3.right;
                    break;
                case CameraRotationState.Left:
                    PlayerManager.Instance.playerBody.transform.SetPositionAndRotation(PlayerManager.Instance.playerBody.transform.position, Quaternion.Euler(0, 90 + rotationValue, 0));

                    if (90 + rotationValue == 0 || 90 + rotationValue == 360)
                        CameraController.Instance.directionFacing = Vector3.left;
                    else if (90 + rotationValue == 180)
                        CameraController.Instance.directionFacing = Vector3.right;
                    else if (90 + rotationValue == 90)
                        CameraController.Instance.directionFacing = Vector3.forward;
                    else if (90 + rotationValue == -90 || 90 + rotationValue == 270)
                        CameraController.Instance.directionFacing = Vector3.back;
                    break;
                case CameraRotationState.Right:
                    PlayerManager.Instance.playerBody.transform.SetPositionAndRotation(PlayerManager.Instance.playerBody.transform.position, Quaternion.Euler(0, -90 + rotationValue, 0));

                    if (-90 + rotationValue == 0 || -90 + rotationValue == 360)
                        CameraController.Instance.directionFacing = Vector3.right;
                    else if (-90 + rotationValue == 180 || -90 + rotationValue == -180)
                        CameraController.Instance.directionFacing = Vector3.left;
                    else if (-90 + rotationValue == 90)
                        CameraController.Instance.directionFacing = Vector3.back;
                    else if (-90 + rotationValue == -90 || -90 + rotationValue == 270)
                        CameraController.Instance.directionFacing = Vector3.forward;
                    break;

                default:
                    break;
            }
        }

        Action_BodyRotated?.Invoke();
    }
    
    void MovePlayer()
    {
        //Move with a set speed
        if (PlayerManager.Instance.block_MovingTowards != null)
        {
            if (PlayerManager.Instance.block_MovingTowards.block != null)
            {
                //Check if the block standing on is different from the one entering, to move with what the player stands on
                if (PlayerManager.Instance.block_StandingOn_Current.block != PlayerManager.Instance.block_MovingTowards.block && PlayerManager.Instance.block_StandingOn_Current.block)
                {
                    if (PlayerManager.Instance.block_MovingTowards.block.GetComponent<BlockInfo>().movementSpeed <= 0)
                        PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, endDestination, 5 * Time.deltaTime);
                    else
                    {
                        if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_IceGlide>() && (PlayerStats.Instance.stats.abilitiesGot_Permanent.IceSpikes || PlayerStats.Instance.stats.abilitiesGot_Temporary.IceSpikes))
                            PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, endDestination, (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementSpeed / 2) * Time.deltaTime);
                        else
                            PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, endDestination, PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    if (PlayerManager.Instance.block_MovingTowards.block.GetComponent<BlockInfo>().movementSpeed <= 0)
                        PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, endDestination, 5 * Time.deltaTime);
                    else
                    {
                        if (PlayerManager.Instance.block_MovingTowards.block.GetComponent<Block_IceGlide>() && (PlayerStats.Instance.stats.abilitiesGot_Permanent.IceSpikes || PlayerStats.Instance.stats.abilitiesGot_Temporary.IceSpikes))
                            PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, endDestination, (PlayerManager.Instance.block_MovingTowards.block.GetComponent<BlockInfo>().movementSpeed / 2) * Time.deltaTime);
                        else
                            PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, endDestination, PlayerManager.Instance.block_MovingTowards.block.GetComponent<BlockInfo>().movementSpeed * Time.deltaTime);
                    }
                }
            }
            else
            {
                PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, endDestination, 5 * Time.deltaTime);
            }
        }
        else
        {
            PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, endDestination, 5 * Time.deltaTime);
        }

        //Snap into place when close enough
        if (Vector3.Distance(PlayerManager.Instance.player.transform.position, endDestination) <= 0.03f)
        {
            PlayerManager.Instance.player.transform.position = endDestination;
            movementStates = MovementStates.Still;

            Player_BlockDetector.Instance.RaycastSetup();

            Action_StepTaken_Invoke();
        }
    }
    void PlayerHover()
    {
        //Don't hover if teleporting
        if (PlayerManager.Instance.isTransportingPlayer) { return; }

        //Don't fall if moving
        if (movementStates == MovementStates.Moving) { return; }

        //Make a safety Raycast
        Player_BlockDetector.Instance.Update_BlockStandingOn();

        //Get distance to blockStandingOn
        float distance = 1;
        if (PlayerManager.Instance.block_StandingOn_Current.block)
        {
            distance = Vector3.Distance(transform.position, PlayerManager.Instance.block_StandingOn_Current.block.transform.position + (Vector3.up * heightOverBlock));
        }

        //Start Fall if standing still and no block is under the player
        if (movementStates == MovementStates.Still && !PlayerManager.Instance.block_StandingOn_Current.block)
        {
            movementStates = MovementStates.Falling;
            gameObject.transform.position = gameObject.transform.position + (Vector3.down * fallSpeed * Time.deltaTime);
        }
        //Land the fall
        else if (movementStates == MovementStates.Falling && distance <= 0.1f /*0.15*/)
        {
            gameObject.transform.position = PlayerManager.Instance.block_StandingOn_Current.block.transform.position + (Vector3.up * heightOverBlock);
            movementStates = MovementStates.Still;
            Action_LandedFromFalling?.Invoke();
            Action_StepTaken_Invoke();
        }
        //Continue fall
        else if (movementStates == MovementStates.Falling)
        {
            FallingRaycast();
            gameObject.transform.position = gameObject.transform.position + (Vector3.down * fallSpeed * Time.deltaTime);
            movementStates = MovementStates.Falling;
        }

        //Hover over blocks you're standing on
        else if (movementStates == MovementStates.Still && PlayerManager.Instance.block_StandingOn_Current.block)
        {
            Player_BlockDetector.Instance.Update_BlockStandingOn();
            gameObject.transform.position = PlayerManager.Instance.block_StandingOn_Current.block.transform.position + (Vector3.up * heightOverBlock);
        }
    }


    //--------------------


    #region CeilingGrab

    public void StartCeilingGrabMovement(Vector3 direction)
    {
        GameObject targetObject = CeilingGrabMovementCheck(DirectionCalculator(direction));

        if (targetObject)
        {
            print("10. IceGlide");
            StartCoroutine(CeilingGrabMoveRoutine(targetObject, direction));
        }
        else
        {
            movementStates = MovementStates.Still;
            PlayerManager.Instance.pauseGame = false;
            PlayerManager.Instance.isTransportingPlayer = false;

            Action_ResetBlockColorInvoke();

            RotateCeilingPlayerBody(direction);
        }
    }
    GameObject CeilingGrabMovementCheck(Vector3 direction)
    {
        //Check if direction is blocked by a block
        if (Physics.Raycast(transform.position, direction, out hit, 1))
        {
            return null;
        }

        //Check if there is a block to move into
        if (Physics.Raycast(transform.position + direction, Vector3.up, out hit, 1))
        {
            return hit.transform.gameObject;
        }
        else
        {
            return null;
        }
    }
    private IEnumerator CeilingGrabMoveRoutine(GameObject target, Vector3 rotationDirection)
    {
        movementStates = MovementStates.Moving;
        PlayerManager.Instance.pauseGame = true;
        PlayerManager.Instance.isTransportingPlayer = true;

        RotateCeilingPlayerBody(rotationDirection);

        Vector3 startPosition = transform.position;
        Vector3 endPosition;
        if (target)
            endPosition = target.transform.position + Vector3.down + (Vector3.down * (1 - heightOverBlock));
        else
            endPosition = startPosition;

        float elapsedTime = 0f;

        float speed = movementSpeed;
        if (PlayerManager.Instance.block_StandingOn_Current.block)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_IceGlide>())
                speed = 0.1f;
            else
                speed = movementSpeed;
        }
        
        while (elapsedTime < speed)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the progress (0 to 1) of the jump
            float progress = elapsedTime / movementSpeed;

            // Interpolate the player position
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, progress);

            // Update the player's position
            transform.position = currentPosition;

            yield return null;
        }

        // Ensure the player lands exactly at the end position
        transform.position = endPosition;

        Player_CeilingGrab.Instance.CheckBlockStandingUnder();
    }
    void RotateCeilingPlayerBody(Vector3 direction)
    {
        ResetCeilingBlockColor();

        Vector3 tempDir = DirectionCalculator(direction);

        if (tempDir == Vector3.forward)
            PlayerManager.Instance.playerBody.transform.rotation = Quaternion.Euler(PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.x, 0, PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.z);
        else if (tempDir == Vector3.back)
            PlayerManager.Instance.playerBody.transform.rotation = Quaternion.Euler(PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.x, 180, PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.z);
        else if (tempDir == Vector3.left)
            PlayerManager.Instance.playerBody.transform.rotation = Quaternion.Euler(PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.x, -90, PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.z);
        else if (tempDir == Vector3.right)
            PlayerManager.Instance.playerBody.transform.rotation = Quaternion.Euler(PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.x, 90, PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.z);

        Action_BodyRotated?.Invoke();

        DarkenCeilingBlocks();
    }

    public void DarkenCeilingBlocks()
    {
        if (!Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        GameObject targetObject_forward = CeilingGrabMovementCheck(DirectionCalculator(Vector3.forward));
        if (targetObject_forward)
        {
            if (targetObject_forward.GetComponent<BlockInfo>())
            {
                targetObject_forward.GetComponent<BlockInfo>().DarkenColors();
            }
        }

        GameObject targetObject_back = CeilingGrabMovementCheck(DirectionCalculator(Vector3.back));
        if (targetObject_back)
        {
            if (targetObject_back.GetComponent<BlockInfo>())
            {
                targetObject_back.GetComponent<BlockInfo>().DarkenColors();
            }
        }

        GameObject targetObject_left = CeilingGrabMovementCheck(DirectionCalculator(Vector3.left));
        if (targetObject_left)
        {
            if (targetObject_left.GetComponent<BlockInfo>())
            {
                targetObject_left.GetComponent<BlockInfo>().DarkenColors();
            }
        }

        GameObject targetObject_right = CeilingGrabMovementCheck(DirectionCalculator(Vector3.right));
        if (targetObject_right)
        {
            if (targetObject_right.GetComponent<BlockInfo>())
            {
                targetObject_right.GetComponent<BlockInfo>().DarkenColors();
            }
        }
    }
    public void ResetCeilingBlockColor()
    {
        if (!Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        GameObject targetObject_forward = CeilingGrabMovementCheck(DirectionCalculator(Vector3.forward));
        if (targetObject_forward)
        {
            if (targetObject_forward.GetComponent<BlockInfo>())
            {
                targetObject_forward.GetComponent<BlockInfo>().ResetDarkenColor();
            }
        }
           
        GameObject targetObject_back = CeilingGrabMovementCheck(DirectionCalculator(Vector3.back));
        if (targetObject_back)
        {
            if (targetObject_back.GetComponent<BlockInfo>())
            {
                targetObject_back.GetComponent<BlockInfo>().ResetDarkenColor();
            }
        }

        GameObject targetObject_left = CeilingGrabMovementCheck(DirectionCalculator(Vector3.left));
        if (targetObject_left)
        {
            if (targetObject_left.GetComponent<BlockInfo>())
            {
                targetObject_left.GetComponent<BlockInfo>().ResetDarkenColor();
            }
        }

        GameObject targetObject_right = CeilingGrabMovementCheck(DirectionCalculator(Vector3.right));
        if (targetObject_right)
        {
            if (targetObject_right.GetComponent<BlockInfo>())
            {
                targetObject_right.GetComponent<BlockInfo>().ResetDarkenColor();
            }
        }
    }

    #endregion


    //--------------------


    void FallingRaycast()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                PlayerManager.Instance.block_StandingOn_Current.block = hit.transform.gameObject;
                PlayerManager.Instance.block_StandingOn_Current.blockPosition = hit.transform.position;
                PlayerManager.Instance.block_StandingOn_Current.blockType = hit.transform.GetComponent<BlockInfo>().blockType;
            }
        }
    }


    //--------------------


    #region Ladder
    
    void FindLadderExitBlock()
    {
        CheckAvailableLadderExitBlocks(Vector3.forward);
        CheckAvailableLadderExitBlocks(Vector3.back);
        CheckAvailableLadderExitBlocks(Vector3.left);
        CheckAvailableLadderExitBlocks(Vector3.right);
    }
    void CheckAvailableLadderExitBlocks(Vector3 dir)
    {
        //Check from the bottom and up
        if (Physics.Raycast(transform.position, dir, out hit, 1))
        {
            if (hit.transform.gameObject.GetComponent<Block_Ladder>())
            {
                hit.transform.gameObject.GetComponent<Block_Ladder>().DarkenExitBlock_Up(dir);
            }
        }

        //Check from the top and down
        if (Physics.Raycast(transform.position + (dir * 0.65f), Vector3.down, out hit, 1))
        {
            if (hit.transform.gameObject.GetComponent<Block_Ladder>())
            {
                hit.transform.gameObject.GetComponent<Block_Ladder>().DarkenExitBlock_Down();
            }
        }
    }

    bool CheckLaddersToEnter_Up(Vector3 dir)
    {
        //Check from the bottom and up
        if (Physics.Raycast(transform.position, dir, out hit, 1))
        {
            if (hit.transform.gameObject.GetComponent<Block_Ladder>())
            {
                return true;
            }
        }

        //If no ladder is found
        return false;
    }
    bool CheckLaddersToEnter_Down(Vector3 dir)
    {
        //Check from the top and down
        if (Physics.Raycast(transform.position + (dir * 0.65f), Vector3.down, out hit, 1))
        {
            if (hit.transform.gameObject.GetComponent<Block_Ladder>())
            {
                return true;
            }
        }

        //If no ladder is found
        return false;
    }

    GameObject GetLadderExitPart_Up(Vector3 dir)
    {
        //Check from the bottom and up
        if (Physics.Raycast(transform.position, dir, out hit, 1))
        {
            if (hit.transform.gameObject.GetComponent<Block_Ladder>())
            {
                ladderToEnterRot = hit.transform.rotation;
                return hit.transform.gameObject.GetComponent<Block_Ladder>().lastLadderPart_Up;
            }
        }

        return null;
    }
    GameObject GetLadderExitPart_Down(Vector3 dir)
    {
        //Check from the top and down
        if (Physics.Raycast(transform.position + (dir * 0.65f), Vector3.down, out hit, 1))
        {
            if (hit.transform.gameObject.GetComponent<Block_Ladder>())
            {
                ladderToEnterRot = hit.transform.rotation;
                return hit.transform.gameObject.GetComponent<Block_Ladder>().lastLadderPart_Down;
            }
        }

        return null;
    }

    IEnumerator PerformLadderMovement_Up(Vector3 dir, GameObject targetPosObj)
    {
        Action_ResetBlockColorInvoke();

        #region Setup Movement Parameters

        isMovingOnLadder_Up = true;
        ladderClimbPos_Start = transform.position;

        movementStates = MovementStates.Moving;
        PlayerManager.Instance.pauseGame = true;
        PlayerManager.Instance.isTransportingPlayer = true;

        Vector3 startPosition;
        Vector3 endPosition;
        float ladderClimbDuration = 0;
        float elapsedTime = 0;

        #endregion

        SetPlayerBodyRotation(int.MinValue);

        #region Move To Top LadderPart

        startPosition = transform.position;
        endPosition = targetPosObj.transform.position + (Vector3.up * heightOverBlock);

        ladderClimbDuration = Vector3.Distance(startPosition, endPosition) * 0.4f;
        elapsedTime = 0f;

        //Move to the top ladder
        while (elapsedTime < ladderClimbDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the progress of the ladderMovement
            float progress = elapsedTime / ladderClimbDuration;

            // Interpolate the up/down position
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, progress);

            // Update the player's position
            transform.position = currentPosition;

            Action_ResetBlockColorInvoke();

            yield return null;
        }

        // Ensure the player lands exactly at the end position
        transform.position = endPosition;

        #endregion

        #region Move To ExitBlock

        endPosition = startPosition + dir;
        if (Physics.Raycast(transform.position + dir, Vector3.down, out hit, 1))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                endPosition = hit.transform.gameObject.transform.position + (Vector3.up * heightOverBlock);
            }
        }

        startPosition = transform.position;

        ladderClimbDuration = 0.4f;
        elapsedTime = 0f;

        //Move to the top ladder
        while (elapsedTime < ladderClimbDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the progress of the ladderMovement
            float progress = elapsedTime / ladderClimbDuration;

            // Interpolate the up/down position
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, progress);

            // Update the player's position
            transform.position = currentPosition;

            Action_ResetBlockColorInvoke();

            yield return null;
        }

        // Ensure the player lands exactly at the end position
        transform.position = endPosition;

        #endregion


        #region Setup StopMovement Parameters

        Player_BlockDetector.Instance.RaycastSetup();

        isMovingOnLadder_Up = false;

        movementStates = MovementStates.Still;
        PlayerManager.Instance.pauseGame = false;
        PlayerManager.Instance.isTransportingPlayer = false;

        Player_BlockDetector.Instance.Update_BlockStandingOn();

        FindLadderExitBlock();
        Action_ResetBlockColorInvoke();
        Action_StepTaken_Invoke();

        #endregion
    }
    IEnumerator PerformLadderMovement_Down(Vector3 dir, GameObject targetPosObj)
    {
        Action_ResetBlockColorInvoke();

        #region Setup Movement Parameters

        isMovingOnLadder_Down = true;
        ladderClimbPos_Start = transform.position;

        movementStates = MovementStates.Moving;
        PlayerManager.Instance.pauseGame = true;
        PlayerManager.Instance.isTransportingPlayer = true;

        Vector3 startPosition;
        Vector3 endPosition;
        float ladderClimbDuration = 0;
        float elapsedTime = 0;

        #endregion

        SetPlayerBodyRotation(0);

        #region Move From ExitBlock

        startPosition = transform.position;
        endPosition = startPosition + dir;

        ladderClimbDuration = 0.4f;
        elapsedTime = 0f;

        //Move to the top ladder
        while (elapsedTime < ladderClimbDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the progress of the ladderMovement
            float progress = elapsedTime / ladderClimbDuration;

            // Interpolate the up/down position
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, progress);

            // Update the player's position
            transform.position = currentPosition;

            Action_ResetBlockColorInvoke();

            yield return null;
        }

        // Ensure the player lands exactly at the end position
        transform.position = endPosition;

        #endregion

        SetPlayerBodyRotation(int.MinValue);

        #region Move To Bottom LadderPart

        startPosition = transform.position;
        endPosition = targetPosObj.transform.position/* + (Vector3.up * heightOverBlock)*/;

        ladderClimbDuration = Vector3.Distance(startPosition, endPosition) * 0.4f;
        elapsedTime = 0f;

        //Move to the bottom ladder
        while (elapsedTime < ladderClimbDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the progress of the ladderMovement
            float progress = elapsedTime / ladderClimbDuration;

            // Interpolate the up/down position
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, progress);

            // Update the player's position
            transform.position = currentPosition;

            Action_ResetBlockColorInvoke();

            yield return null;
        }

        // Ensure the player lands exactly at the end position
        transform.position = endPosition;

        #endregion

        SetPlayerBodyRotation(0);

        #region Setup StopMovement Parameters

        Player_BlockDetector.Instance.RaycastSetup();

        isMovingOnLadder_Down = false;

        movementStates = MovementStates.Still;
        PlayerManager.Instance.pauseGame = false;
        PlayerManager.Instance.isTransportingPlayer = false;

        Player_BlockDetector.Instance.Update_BlockStandingOn();

        FindLadderExitBlock();
        Action_ResetBlockColorInvoke();
        Action_StepTaken_Invoke();

        #endregion
    }

    #endregion


    //--------------------


    public void IceGlide()
    {
        Player_BlockDetector.Instance.RaycastSetup();

        if (PlayerManager.Instance.block_StandingOn_Current.blockType == BlockType.Slope) { isIceGliding = false; return; }

        if (PlayerManager.Instance.block_StandingOn_Current.block)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_IceGlide>() && !PlayerStats.Instance.stats.abilitiesGot_Permanent.IceSpikes && !PlayerStats.Instance.stats.abilitiesGot_Temporary.IceSpikes)
            {
                isIceGliding = true;
                PlayerStats.Instance.stats.steps_Current += PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementCost;

                switch (lastMovementButtonPressed)
                {
                    case ButtonsToPress.W:
                        if (PlayerManager.Instance.canMove_Forward)
                        {
                            MovementKeyIsPressed(PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
                        }
                        break;
                    case ButtonsToPress.S:
                        if (PlayerManager.Instance.canMove_Back)
                        {
                            MovementKeyIsPressed(PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);
                        }
                        break;
                    case ButtonsToPress.A:
                        if (PlayerManager.Instance.canMove_Left)
                        {
                            MovementKeyIsPressed(PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
                        }
                        break;
                    case ButtonsToPress.D:
                        if (PlayerManager.Instance.canMove_Right)
                        {
                            MovementKeyIsPressed(PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        isIceGliding = false;
    }

    void SlopeGlide()
    {
        if (PlayerStats.Instance.stats.abilitiesGot_Temporary.HikerGear || PlayerStats.Instance.stats.abilitiesGot_Permanent.HikerGear) { return; }

        Player_BlockDetector.Instance.RaycastSetup();
        Player_BlockDetector.Instance.Update_BlockStandingOn();

        if (PlayerManager.Instance.block_StandingOn_Current.blockType == BlockType.Slope)
        {
            //If IceGlide is attached to SlopeBock, ignore the slope if player has IceSpikes
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_IceGlide>())
            {
                if (PlayerStats.Instance.stats.abilitiesGot_Temporary.IceSpikes || PlayerStats.Instance.stats.abilitiesGot_Permanent.IceSpikes) { return; }
            }

            isSlopeGliding = true;

            PlayerStats.Instance.stats.steps_Current += PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementCost;

            //Forward - Slope is rotated 0
            if (PlayerManager.Instance.block_StandingOn_Current.block.transform.rotation == Quaternion.Euler(0, 0, 0))
            {
                if (CameraController.Instance.cameraRotationState == CameraRotationState.Forward)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.forward)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
                        lastMovementButtonPressed = ButtonsToPress.W;
                    }
                    else if (PlayerManager.Instance.lookingDirection == Vector3.back)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InFront, 0);
                        lastMovementButtonPressed = ButtonsToPress.W;
                    }
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Backward)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.forward)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);
                        lastMovementButtonPressed = ButtonsToPress.S;
                    }
                    else if (PlayerManager.Instance.lookingDirection == Vector3.back)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InBack, 180);
                        lastMovementButtonPressed = ButtonsToPress.S;
                    }
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Left)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.forward)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
                        lastMovementButtonPressed = ButtonsToPress.A;
                    }
                    else if (PlayerManager.Instance.lookingDirection == Vector3.back)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
                        lastMovementButtonPressed = ButtonsToPress.A;
                    }
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Right)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.forward)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
                        lastMovementButtonPressed = ButtonsToPress.D;
                    }
                    else if (PlayerManager.Instance.lookingDirection == Vector3.back)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
                        lastMovementButtonPressed = ButtonsToPress.D;
                    }
                }
            }

            //Back - Slope is rotated 180
            else if (PlayerManager.Instance.block_StandingOn_Current.block.transform.rotation == Quaternion.Euler(0, 180, 0))
            {
                if (CameraController.Instance.cameraRotationState == CameraRotationState.Forward)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.forward)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InBack, 180);
                        lastMovementButtonPressed = ButtonsToPress.S;
                    }
                    else if (PlayerManager.Instance.lookingDirection == Vector3.back)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);
                        lastMovementButtonPressed = ButtonsToPress.S;
                    }
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Backward)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.forward)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InFront, 0);
                        lastMovementButtonPressed = ButtonsToPress.W;
                    }
                    else if (PlayerManager.Instance.lookingDirection == Vector3.back)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
                        lastMovementButtonPressed = ButtonsToPress.W;
                    }
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Left)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.forward)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
                        lastMovementButtonPressed = ButtonsToPress.D;
                    }
                    else if (PlayerManager.Instance.lookingDirection == Vector3.back)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
                        lastMovementButtonPressed = ButtonsToPress.D;
                    }
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Right)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.forward)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
                        lastMovementButtonPressed = ButtonsToPress.A;
                    }
                    else if (PlayerManager.Instance.lookingDirection == Vector3.back)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
                        lastMovementButtonPressed = ButtonsToPress.A;
                    }
                }
            }

            //Left - Slope is rotated -90
            else if (PlayerManager.Instance.block_StandingOn_Current.block.transform.rotation == Quaternion.Euler(0, -90, 0))
            {
                if (CameraController.Instance.cameraRotationState == CameraRotationState.Forward)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.left)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
                        lastMovementButtonPressed = ButtonsToPress.A;
                    }
                    else if (PlayerManager.Instance.lookingDirection == Vector3.right)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
                        lastMovementButtonPressed = ButtonsToPress.A;
                    }
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Backward)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.left)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
                        lastMovementButtonPressed = ButtonsToPress.D;
                    }
                    else if (PlayerManager.Instance.lookingDirection == Vector3.right)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
                        lastMovementButtonPressed = ButtonsToPress.D;
                    }
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Left)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.left)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);
                        lastMovementButtonPressed = ButtonsToPress.S;
                    }
                    else if (PlayerManager.Instance.lookingDirection == Vector3.right)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InBack, 180);
                        lastMovementButtonPressed = ButtonsToPress.S;
                    }
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Right)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.left)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
                        lastMovementButtonPressed = ButtonsToPress.W;
                    }
                    else if (PlayerManager.Instance.lookingDirection == Vector3.right)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InFront, 0);
                        lastMovementButtonPressed = ButtonsToPress.W;
                    }
                }
            }

            //Right - Slope is rotated 90
            else if (PlayerManager.Instance.block_StandingOn_Current.block.transform.rotation == Quaternion.Euler(0, 90, 0))
            {
                if (CameraController.Instance.cameraRotationState == CameraRotationState.Forward)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.left)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheRight, -90);
                        lastMovementButtonPressed = ButtonsToPress.D;
                    }
                    else if (PlayerManager.Instance.lookingDirection == Vector3.right)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, -90);
                        lastMovementButtonPressed = ButtonsToPress.D;
                    }
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Backward)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.left)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheLeft, 90);
                        lastMovementButtonPressed = ButtonsToPress.A;
                    }
                    else if (PlayerManager.Instance.lookingDirection == Vector3.right)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, 90);
                        lastMovementButtonPressed = ButtonsToPress.A;
                    }
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Left)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.left)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InFront, 180);
                        lastMovementButtonPressed = ButtonsToPress.W;
                    }
                    else if (PlayerManager.Instance.lookingDirection == Vector3.right)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 180);
                        lastMovementButtonPressed = ButtonsToPress.W;
                    }
                }
                else if (CameraController.Instance.cameraRotationState == CameraRotationState.Right)
                {
                    if (PlayerManager.Instance.lookingDirection == Vector3.left)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InBack, 0);
                        lastMovementButtonPressed = ButtonsToPress.S;
                    }
                    else if (PlayerManager.Instance.lookingDirection == Vector3.right)
                    {
                        MovementKeyIsPressed(PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 0);
                        lastMovementButtonPressed = ButtonsToPress.S;
                    }
                }
            }
        }

        isSlopeGliding = false;
    }


    //--------------------


    public void Action_StepTakenEarly_Invoke()
    {
        Action_StepTakenEarly?.Invoke();
    }
    public void Action_StepTaken_Invoke()
    {
        Action_StepTaken?.Invoke();
    }
    public void Action_StepCostTakenInvoke()
    {
        Action_StepCostTaken?.Invoke();
    }
    public void Action_ResetBlockColorInvoke()
    {
        Action_resetBlockColor?.Invoke();
    }
    public void Action_PressMoveBlockButtonInvoke()
    {
        Action_PressMoveBlockButton?.Invoke();
    }


    //--------------------


    public Vector3 DirectionCalculator(Vector3 direction)
    {
        switch (CameraController.Instance.cameraRotationState)
        {
            case CameraRotationState.Forward:
                if (direction == Vector3.forward)
                    return Vector3.forward;
                else if (direction == Vector3.back)
                    return Vector3.back;
                else if (direction == Vector3.left)
                    return Vector3.left;
                else if (direction == Vector3.right)
                    return Vector3.right;
                break;
            case CameraRotationState.Backward:
                if (direction == Vector3.back)
                    return Vector3.forward;
                else if (direction == Vector3.forward)
                    return Vector3.back;
                else if (direction == Vector3.right)
                    return Vector3.left;
                else if (direction == Vector3.left)
                    return Vector3.right;
                break;
            case CameraRotationState.Left:
                if (direction == Vector3.left)
                    return Vector3.forward;
                else if (direction == Vector3.right)
                    return Vector3.back;
                else if (direction == Vector3.back)
                    return Vector3.left;
                else if (direction == Vector3.forward)
                    return Vector3.right;
                break;
            case CameraRotationState.Right:
                if (direction == Vector3.right)
                    return Vector3.forward;
                else if (direction == Vector3.left)
                    return Vector3.back;
                else if (direction == Vector3.forward)
                    return Vector3.left;
                else if (direction == Vector3.back)
                    return Vector3.right;
                break;

            default:
                return Vector3.forward;
        }

        return Vector3.forward;
    }


    //--------------------


    public void QuitLevel()
    {
        if (!string.IsNullOrEmpty("MainMenu"))
        {
            StartCoroutine(LoadSceneCoroutine("MainMenu"));
        }
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            Debug.Log($"Loading progress: {operation.progress * 100}%");
            yield return null;
        }
    }
}
public enum MovementStates
{
    Still,
    Moving,
    Falling
}
public enum ButtonsToPress
{
    None,

    W,
    S,
    A,
    D,

    Arrow_Left,
    ArrowRight,

    Space,
    X,

}

public enum MovementDirection
{
    None,

    Forward,
    Backward,
    Left,
    Right
}

public enum DetectorPoint
{
    Center,

    Front,
    Back,
    Right,
    Left
}