using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player_Movement : Singleton<Player_Movement>
{
    public static event Action Action_StepTaken;
    public static event Action Action_BodyRotated;
    public static event Action Action_resetBlockColor;
    public static event Action Action_PressMoveBlockButton;

    [Header("Current Movement Cost")]
    public int currentMovementCost;

    [Header("Movement State")]
    public MovementStates movementStates;
    public ButtonsToPress lastMovementButtonPressed;

    [Header("Player Movement over Blocks")]
    [HideInInspector] public float heightOverBlock = 0.95f;
    public float fallSpeed = 6f;

    //Other
    Vector3 endDestination;
    public bool iceGliding;
    public bool slopeGliding;
    public bool isOnLadder;

    [Header("Ladder Movement parameters")]
    public GameObject ladderSteppedOn;
    public bool ladderMovement;
    public bool ladderMovement_Top;
    public bool ladderMovement_ToBlock;
    public Vector3 ladderTop_EndPos;
    [SerializeField] GameObject ladderToApproach_Current;
    GameObject ladder_Top;


    //--------------------


    private void Update()
    {
        if (Player_GraplingHook.Instance.isGrapplingHooking) { return; }
        //if (Player_Ascend.Instance.isAscending) { return; }
        //if (Player_Descend.Instance.isDescending) { return; }
        //if (Player_Dash.Instance.isDashing) { return; }

        KeyInputs();

        if (movementStates == MovementStates.Moving /*&& endDestination != (Vector3.zero + (Vector3.up * heightOverBlock))*/
            && !Player_SwiftSwim.Instance.isSwiftSwimming_Up && !Player_SwiftSwim.Instance.isSwiftSwimming_Down
            && !Player_Ascend.Instance.isAscending && !Player_Descend.Instance.isDescending
            && !Player_Dash.Instance.isDashing
            && !slopeGliding && !ladderMovement)
        {
            MovePlayer();
            PlayerHover();
        }
        else if (Player_SwiftSwim.Instance.isSwiftSwimming_Up || Player_SwiftSwim.Instance.isSwiftSwimming_Down)
        {

        }
        else if (Player_Ascend.Instance.isAscending || Player_Descend.Instance.isDescending)
        {

        }
        else if (ladderMovement_ToBlock)
        {
            LadderMovement_ToBlock();
        }
        else if (isOnLadder && ladderMovement_Top)
        {
            LadderMovement_Top();
        }
        else if (isOnLadder && ladderMovement)
        {
            LadderMovement(ladderToApproach_Current);
        }
        else if (isOnLadder)
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
    }

    private void OnDisable()
    {
        Action_StepTaken -= IceGlide;
        Action_StepTaken -= SlopeGlide;
    }


    //--------------------


    void KeyInputs()
    {
        if (movementStates == MovementStates.Moving) { return; }
        if (PlayerManager.Instance.pauseGame) { return; }
        if (PlayerManager.Instance.isTransportingPlayer) { return; }
        if (Cameras.Instance.isRotating) { return; }
        if (Player_Interact.Instance.isInteracting) { return; }
        if (Player_GraplingHook.Instance.isGrapplingHooking) { return; }
        if (ladderMovement) { return; }
        if (ladderMovement_Top) { return; }


        //If pressing UP - Movement
        if (Input.GetKey(KeyCode.W))
        {
            if (isOnLadder && Cameras.Instance.cameraState == CameraState.Forward && !PlayerManager.Instance.canMove_Forward)
            {
                MovePlayerOnLadder_UP();
            }
            else if (isOnLadder && Cameras.Instance.cameraState == CameraState.Backward && !PlayerManager.Instance.canMove_Back)
            {
                MovePlayerOnLadder_DOWN();
            }
            else
            {
                lastMovementButtonPressed = ButtonsToPress.W;
                MovementKeyIsPressed(PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
            }
        }

        //If pressing DOWN - Movement
        else if (Input.GetKey(KeyCode.S))
        {
            if (isOnLadder && Cameras.Instance.cameraState == CameraState.Backward && !PlayerManager.Instance.canMove_Back)
            {
                MovePlayerOnLadder_UP();
            }
            else if (isOnLadder && Cameras.Instance.cameraState == CameraState.Forward && !PlayerManager.Instance.canMove_Forward)
            {
                MovePlayerOnLadder_DOWN();
            }
            else
            {
                lastMovementButtonPressed = ButtonsToPress.S;
                MovementKeyIsPressed(PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);
            }
        }

        //If pressing LEFT - Movement
        else if (Input.GetKey(KeyCode.A))
        {
            if (isOnLadder && Cameras.Instance.cameraState == CameraState.Left && !PlayerManager.Instance.canMove_Left)
            {
                MovePlayerOnLadder_UP();
            }
            else if (isOnLadder && Cameras.Instance.cameraState == CameraState.Right && !PlayerManager.Instance.canMove_Right)
            {
                MovePlayerOnLadder_DOWN();
            }
            else
            {
                lastMovementButtonPressed = ButtonsToPress.A;
                MovementKeyIsPressed(PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
            }
        }

        //If pressing RIGHT - Movement
        else if (Input.GetKey(KeyCode.D))
        {
            if (isOnLadder && Cameras.Instance.cameraState == CameraState.Right && !PlayerManager.Instance.canMove_Right)
            {
                MovePlayerOnLadder_UP();
            }
            else if (isOnLadder && Cameras.Instance.cameraState == CameraState.Left && !PlayerManager.Instance.canMove_Left)
            {
                MovePlayerOnLadder_DOWN();
            }
            else
            {
                lastMovementButtonPressed = ButtonsToPress.D;
                MovementKeyIsPressed(PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
            }
        }


        //--------------------


        //Grappling Hook
        else if (Input.GetKey(KeyCode.F) && Input.GetKeyDown(KeyCode.C))
        {
            if (Player_GraplingHook.Instance.CheckIfCanGrapple())
            {
                Player_GraplingHook.Instance.PerformGrapplingMovement();
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Player_GraplingHook.Instance.StartRaycastGrappling();
        }
        else if (Input.GetKey(KeyCode.F))
        {
            Player_GraplingHook.Instance.UngoingRaycastGrappling();
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            Player_GraplingHook.Instance.StopRaycastGrappling();
        }

        //If pressing - UP - ASCEND
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gameObject.GetComponent<Player_SwiftSwim>().canSwiftSwim_Up)
            {
                gameObject.GetComponent<Player_SwiftSwim>().SwiftSwim_Up();
            }
            else if (gameObject.GetComponent<Player_Ascend>().playerCanAscend)
            {
                gameObject.GetComponent<Player_Ascend>().Ascend();
            }
        }
        //If pressing - DOWN - DESCEND
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gameObject.GetComponent<Player_SwiftSwim>().canSwiftSwim_Down)
            {
                gameObject.GetComponent<Player_SwiftSwim>().SwiftSwim_Down();
            }
            else if (gameObject.GetComponent<Player_Descend>().playerCanDescend)
            {
                gameObject.GetComponent<Player_Descend>().Descend();
            }
        }

        //If pressing - Dash - Space
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameObject.GetComponent<Player_Dash>().playerCanDash)
            {
                gameObject.GetComponent<Player_Dash>().Dash();
            }
        }

        //If pressing - Hammer - Enter
        else if (Input.GetKeyDown(KeyCode.C))
        {
            if (gameObject.GetComponent<Player_Hammer>().playerCanHammer)
            {
                gameObject.GetComponent<Player_Hammer>().Hammer();
            }
            else
            {
                Action_PressMoveBlockButton?.Invoke();
            }
        }

        //If pressing - Respawn
        else if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerStats.Instance.RespawnPlayer();
        }

        //If pressing - Quit
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitLevel();
        }
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
    public void SetPlayerBodyRotation(int rotationValue)
    {
        //Set new Rotation - Based on the key input
        switch (Cameras.Instance.cameraState)
        {
            case CameraState.Forward:
                PlayerManager.Instance.playerBody.transform.SetPositionAndRotation(PlayerManager.Instance.playerBody.transform.position, Quaternion.Euler(0, 0 + rotationValue, 0));
                
                if (rotationValue == 0 || rotationValue == 360)
                    Cameras.Instance.directionFacing = Vector3.forward;
                else if (rotationValue == 180)
                    Cameras.Instance.directionFacing = Vector3.back;
                else if (rotationValue == 90)
                    Cameras.Instance.directionFacing = Vector3.right;
                else if (rotationValue == -90 || rotationValue == 270)
                    Cameras.Instance.directionFacing = Vector3.left;
                break;
            case CameraState.Backward:
                PlayerManager.Instance.playerBody.transform.SetPositionAndRotation(PlayerManager.Instance.playerBody.transform.position, Quaternion.Euler(0, 180 + rotationValue, 0));
                
                if (180 + rotationValue == 0 || 180 + rotationValue == 360)
                    Cameras.Instance.directionFacing = Vector3.back;
                else if (180 + rotationValue == 180)
                    Cameras.Instance.directionFacing = Vector3.forward;
                else if (180 + rotationValue == 90)
                    Cameras.Instance.directionFacing = Vector3.left;
                else if (180 + rotationValue == -90 || 180 + rotationValue == 270)
                    Cameras.Instance.directionFacing = Vector3.right;
                break;
            case CameraState.Left:
                PlayerManager.Instance.playerBody.transform.SetPositionAndRotation(PlayerManager.Instance.playerBody.transform.position, Quaternion.Euler(0, 90 + rotationValue, 0));
                
                if (90 + rotationValue == 0 || 90 + rotationValue == 360)
                    Cameras.Instance.directionFacing = Vector3.left;
                else if (90 + rotationValue == 180)
                    Cameras.Instance.directionFacing = Vector3.right;
                else if (90 + rotationValue == 90)
                    Cameras.Instance.directionFacing = Vector3.forward;
                else if (90 + rotationValue == -90 || 90 + rotationValue == 270)
                    Cameras.Instance.directionFacing = Vector3.back;
                break;
            case CameraState.Right:
                PlayerManager.Instance.playerBody.transform.SetPositionAndRotation(PlayerManager.Instance.playerBody.transform.position, Quaternion.Euler(0, -90 + rotationValue, 0));
                
                if (-90 + rotationValue == 0 || -90 + rotationValue == 360)
                    Cameras.Instance.directionFacing = Vector3.right;
                else if (-90 + rotationValue == 180 || -90 + rotationValue == -180)
                    Cameras.Instance.directionFacing = Vector3.left;
                else if (-90 + rotationValue == 90)
                    Cameras.Instance.directionFacing = Vector3.back;
                else if (-90 + rotationValue == -90 || -90 + rotationValue == 270)
                    Cameras.Instance.directionFacing = Vector3.forward;
                break;

            default:
                break;
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

            Action_StepTakenInvoke();
        }
    }
    void MovePlayerOnLadder_UP()
    {
        if (ladderMovement) { return; }

        print("1. MovePlayerOnLadder_UP");

        if (ladderSteppedOn)
        {
            if (ladderSteppedOn.GetComponent<Block_Ladder>().ladder_Over)
            {
                print("2. MovePlayerOnLadder_UP");
                ladderToApproach_Current = ladderSteppedOn.GetComponent<Block_Ladder>().ladder_Over;
                ladderMovement = true;
            }
            else
            {
                print("3. MovePlayerOnLadder_UP");
                ladder_Top = ladderSteppedOn;
                ladderMovement_Top = true;
                LadderMovement_Top();
            }
        }
    }
    void MovePlayerOnLadder_DOWN()
    {
        if (ladderMovement) { return; }

        print("1. MovePlayerOnLadder_UP");

        if (ladderSteppedOn.GetComponent<Block_Ladder>().ladder_Under)
        {
            print("1. MovePlayerOnLadder_UP");
            ladderToApproach_Current = ladderSteppedOn.GetComponent<Block_Ladder>().ladder_Under;
            ladderMovement = true;
        }
    }
    void LadderMovement(GameObject ladder)
    {
        if (ladder == null) { return; }

        print("1. LadderMovement");

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, ladder.transform.position + (Vector3.up * heightOverBlock), ladder.GetComponent<BlockInfo>().movementSpeed * Time.deltaTime);

        //Snap into place when close enough
        if (Vector3.Distance(gameObject.transform.position, ladder.transform.position) <= 0.03f)
        {
            print("2. LadderMovement");

            PlayerManager.Instance.player.transform.position = ladder.transform.position;

            ladderMovement = false;

            Action_StepTakenInvoke();
        }
    }
    void LadderMovement_Top()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, ladder_Top.transform.position + (Vector3.up * heightOverBlock), ladder_Top.GetComponent<BlockInfo>().movementSpeed * Time.deltaTime);

        //Snap into place when close enough
        if (Vector3.Distance(gameObject.transform.position, ladder_Top.transform.position + (Vector3.up * heightOverBlock)) <= 0.03f)
        {
            print("2. LadderMovement");

            PlayerManager.Instance.player.transform.position = ladder_Top.transform.position + (Vector3.up * heightOverBlock);

            ladderTop_EndPos = ladder_Top.transform.position + (Vector3.up * heightOverBlock) + Vector3.forward;

            ladderMovement_Top = false;
            ladderMovement_ToBlock = true;
        }
    }
    void LadderMovement_ToBlock()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, ladderTop_EndPos, 5 * Time.deltaTime);

        //Snap into place when close enough
        if (Vector3.Distance(gameObject.transform.position, ladderTop_EndPos) <= 0.03f)
        {
            print("3. LadderMovement");

            PlayerManager.Instance.player.transform.position = ladderTop_EndPos;

            ladderMovement_ToBlock = false;
            isOnLadder = false;
            Action_StepTakenInvoke();
        }
    }
    void PlayerHover()
    {
        //Don't hover if teleporting
        if (PlayerManager.Instance.isTransportingPlayer) { return; }

        //Don't fall if moving
        if (movementStates == MovementStates.Moving) { return; }

        //make a safty Raycast
        Player_BlockDetector.Instance.Update_BlockStandingOn();

        //Fall if standing still and no block is under the player
        if (movementStates == MovementStates.Still && !PlayerManager.Instance.block_StandingOn_Current.block)
        {
            gameObject.transform.position = gameObject.transform.position + (Vector3.down * fallSpeed * Time.deltaTime);
        }

        //Hover over blocks you're standing on
        else if (movementStates == MovementStates.Still && PlayerManager.Instance.block_StandingOn_Current.block)
        {
            gameObject.transform.position = PlayerManager.Instance.block_StandingOn_Current.block.transform.position + (Vector3.up * heightOverBlock);
        }
    }

    //Begin Ice Gliding
    public void IceGlide()
    {
        Player_BlockDetector.Instance.RaycastSetup();

        if (PlayerManager.Instance.block_StandingOn_Current.blockType == BlockType.Slope) { iceGliding = false; return; }


        //Player_BlockDetector.Instance.Update_BlockStandingOn();

        if (PlayerManager.Instance.block_StandingOn_Current.block)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_IceGlide>() && !PlayerStats.Instance.stats.abilitiesGot_Permanent.IceSpikes && !PlayerStats.Instance.stats.abilitiesGot_Temporary.IceSpikes)
            {
                iceGliding = true;
                PlayerStats.Instance.stats.steps_Current += PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementCost;

                switch (lastMovementButtonPressed)
                {
                    case ButtonsToPress.W:
                        if (PlayerManager.Instance.canMove_Forward)
                            MovementKeyIsPressed(PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
                        break;
                    case ButtonsToPress.S:
                        if (PlayerManager.Instance.canMove_Back)
                            MovementKeyIsPressed(PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);
                        break;
                    case ButtonsToPress.A:
                        if (PlayerManager.Instance.canMove_Left)
                            MovementKeyIsPressed(PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
                        break;
                    case ButtonsToPress.D:
                        if (PlayerManager.Instance.canMove_Right)
                            MovementKeyIsPressed(PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
                        break;

                    default:
                        break;
                }
            }
        }

        iceGliding = false;
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

            slopeGliding = true;

            PlayerStats.Instance.stats.steps_Current += PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementCost;

            //Forward - Ladder is rotated 0
            if (PlayerManager.Instance.block_StandingOn_Current.block.transform.rotation == Quaternion.Euler(0, 0, 0))
            {
                if (Cameras.Instance.cameraState == CameraState.Forward)
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
                else if (Cameras.Instance.cameraState == CameraState.Backward)
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
                else if (Cameras.Instance.cameraState == CameraState.Left)
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
                else if (Cameras.Instance.cameraState == CameraState.Right)
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

            //Back - Ladder is rotated 180
            else if (PlayerManager.Instance.block_StandingOn_Current.block.transform.rotation == Quaternion.Euler(0, 180, 0))
            {
                if (Cameras.Instance.cameraState == CameraState.Forward)
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
                else if (Cameras.Instance.cameraState == CameraState.Backward)
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
                else if (Cameras.Instance.cameraState == CameraState.Left)
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
                else if (Cameras.Instance.cameraState == CameraState.Right)
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

            //Left - Ladder is rotated -90
            else if (PlayerManager.Instance.block_StandingOn_Current.block.transform.rotation == Quaternion.Euler(0, -90, 0))
            {
                if (Cameras.Instance.cameraState == CameraState.Forward)
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
                else if (Cameras.Instance.cameraState == CameraState.Backward)
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
                else if (Cameras.Instance.cameraState == CameraState.Left)
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
                else if (Cameras.Instance.cameraState == CameraState.Right)
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

            //Right - Ladder is rotated 90
            else if (PlayerManager.Instance.block_StandingOn_Current.block.transform.rotation == Quaternion.Euler(0, 90, 0))
            {
                if (Cameras.Instance.cameraState == CameraState.Forward)
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
                else if (Cameras.Instance.cameraState == CameraState.Backward)
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
                else if (Cameras.Instance.cameraState == CameraState.Left)
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
                else if (Cameras.Instance.cameraState == CameraState.Right)
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

        slopeGliding = false;
    }


    //--------------------


    public void Action_StepTakenInvoke()
    {
        Action_StepTaken?.Invoke();
    }
    public void Action_ResetBlockColorInvoke()
    {
        Action_resetBlockColor?.Invoke();
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
    Moving
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
public enum MovementState
{
    Still,

    Moving
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