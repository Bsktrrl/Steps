using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Movement : Singleton<Player_Movement>
{
    public static event Action Action_StepTaken;
    public static event Action Action_StepCostTaken;
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
    [HideInInspector] public float fallSpeed = 6f;

    //Other
    [Header("Gliding")]
    public bool iceGliding;
    public bool slopeGliding;
    [HideInInspector] Vector3 endDestination;

    [Header("Ladder Movement parameters")]
    [HideInInspector] public bool isOnLadder;
    [HideInInspector] public GameObject ladderSteppedOn;
    [HideInInspector] public bool ladderMovement_Up;
    [HideInInspector] public bool ladderMovement_Down;
    [HideInInspector] public bool ladderMovement_Top;
    [HideInInspector] public bool ladderMovement_Top_ToBlock;
    [HideInInspector] public bool ladderMovement_Down_ToBlockFromTop;
    [HideInInspector] public bool ladderMovement_Down_ToBottom;
    [HideInInspector] public Vector3 ladderTop_EndPos;
    [HideInInspector] public GameObject ladderToApproach_Current;
    GameObject ladder_Top;
    bool ladderAndIsOnTheGround;
    RaycastHit hit;


    //--------------------


    private void Update()
    {
        if (Player_GraplingHook.Instance.isGrapplingHooking) { return; }
        //if (Player_Ascend.Instance.isAscending) { return; }
        //if (Player_Descend.Instance.isDescending) { return; }
        //if (Player_Dash.Instance.isDashing) { return; }

        if (PlayerManager.Instance.block_StandingOn_Current.block)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.blockType != BlockType.Ladder)
            {
                //ladderMovement_Top_ToBlock = false;
            }
        }

        KeyInputs();

        if (movementStates == MovementStates.Moving /*&& endDestination != (Vector3.zero + (Vector3.up * heightOverBlock))*/
            && !Player_SwiftSwim.Instance.isSwiftSwimming_Up && !Player_SwiftSwim.Instance.isSwiftSwimming_Down
            && !Player_Ascend.Instance.isAscending && !Player_Descend.Instance.isDescending
            && !Player_Dash.Instance.isDashing
            && !slopeGliding && !ladderMovement_Up && !ladderMovement_Down && !ladderMovement_Down_ToBottom)
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
        
        else if (ladderMovement_Top_ToBlock)
        {
            //if (PlayerStats.Instance.stats.steps_Current <= 0)
            //{
            //    RespawnBasedOnStepsCurrent();
            //}
            //else
            //{
            //    Ladder_PlayerRotation_Into();
            //    LadderMovement_Top_ToBlock();
            //}

            Ladder_PlayerRotation_Into();
            LadderMovement_Top_ToBlock();
        }
        else if (isOnLadder && ladderMovement_Top)
        {
            //if (PlayerStats.Instance.stats.steps_Current <= 0)
            //{
            //    RespawnBasedOnStepsCurrent();
            //}
            //else
            //{
            //    Ladder_PlayerRotation_Into();
            //    LadderMovement_Top();
            //    Ladder_PlayerRotation_Into();
            //}

            Ladder_PlayerRotation_Into();
            LadderMovement_Top();
            Ladder_PlayerRotation_Into();
        }
        else if (isOnLadder && ladderMovement_Up)
        {
            //if (PlayerStats.Instance.stats.steps_Current <= 0)
            //{
            //    RespawnBasedOnStepsCurrent();
            //}
            //else
            //{
            //    Ladder_PlayerRotation_Into();
            //    LadderMovement_UP(ref ladderToApproach_Current);
            //    Ladder_PlayerRotation_Into();
            //}

            Ladder_PlayerRotation_Into();
            LadderMovement_UP(ref ladderToApproach_Current);
            Ladder_PlayerRotation_Into();
        }
        else if (ladderMovement_Down_ToBottom)
        {
            //if (PlayerStats.Instance.stats.steps_Current <= 0)
            //{
            //    RespawnBasedOnStepsCurrent();
            //}
            //else
            //{
            //    Ladder_PlayerRotation_Into();
            //    LadderMovement_Down_ToBottom();
            //}

            Ladder_PlayerRotation_Into();
            LadderMovement_Down_ToBottom();
        }
        else if (ladderMovement_Down_ToBlockFromTop)
        {
            //if (PlayerStats.Instance.stats.steps_Current <= 0)
            //{
            //    RespawnBasedOnStepsCurrent();
            //}
            //else
            //{
            //    Ladder_PlayerRotation_Into();
            //    LadderMovement_Down_ToBlockFromTop();
            //}

            Ladder_PlayerRotation_Into();
            LadderMovement_Down_ToBlockFromTop();
        }
        else if (isOnLadder && ladderMovement_Down)
        {
            //if (PlayerStats.Instance.stats.steps_Current <= 0)
            //{
            //    RespawnBasedOnStepsCurrent();
            //}
            //else
            //{
            //    Ladder_PlayerRotation_Into();
            //    LadderMovement_DOWN(ref ladderToApproach_Current);
            //    Ladder_PlayerRotation_Into();
            //}

            Ladder_PlayerRotation_Into();
            LadderMovement_DOWN(ref ladderToApproach_Current);
            Ladder_PlayerRotation_Into();
        }
        else if (isOnLadder && !ladderAndIsOnTheGround)
        {
            //if (PlayerStats.Instance.stats.steps_Current <= 0)
            //{
            //    RespawnBasedOnStepsCurrent();
            //}
            //else
            //{
            //    Ladder_PlayerRotation_Into();
            //}

            Ladder_PlayerRotation_Into();
        }
        else if (isOnLadder )
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

        if (ladderMovement_Up) { return; }
        if (ladderMovement_Down) { return; }
        if (ladderMovement_Top) { return; }
        if (ladderMovement_Top_ToBlock) { return; }
        if (ladderMovement_Down_ToBlockFromTop) { return; }
        if (ladderMovement_Down_ToBottom) { return; }


        //--------------------


        //If pressing Forward - Movement
        if (Input.GetKey(KeyCode.W))
        {
            RaycastUnderPlayer();

            //W-press on a ladder
            if (isOnLadder && ladderSteppedOn.transform.rotation.eulerAngles.y == 0)
            {
                if (PlayerStats.Instance.stats.steps_Current <= 0)
                {
                    RespawnBasedOnStepsCurrent();
                }

                else if (Cameras.Instance.cameraState == CameraState.Forward && !PlayerManager.Instance.canMove_Forward)
                {
                    MovePlayerOnLadder_UP();
                }
                else if (Cameras.Instance.cameraState == CameraState.Backward && !PlayerManager.Instance.canMove_Back)
                {
                    if (!ladderAndIsOnTheGround)
                    {
                        MovePlayerOnLadder_DOWN();
                    }
                    else
                    {
                        BypassIsOnLadder(ButtonsToPress.W, PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
                    }
                }
                else if (Ladder_Down_MoveAway())
                {
                    BypassIsOnLadder(ButtonsToPress.W, PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
                }
            }
            else if (isOnLadder && ladderSteppedOn.transform.rotation.eulerAngles.y == 180)
            {
                if (PlayerStats.Instance.stats.steps_Current <= 0)
                {
                    RespawnBasedOnStepsCurrent();
                }

                else if (Cameras.Instance.cameraState == CameraState.Backward && !PlayerManager.Instance.canMove_Forward)
                    MovePlayerOnLadder_UP();
                else if (Cameras.Instance.cameraState == CameraState.Forward && !PlayerManager.Instance.canMove_Back)
                {
                    if (!ladderAndIsOnTheGround)
                        MovePlayerOnLadder_DOWN();
                    else
                        BypassIsOnLadder(ButtonsToPress.W, PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
                }
                else if (Ladder_Down_MoveAway())
                    BypassIsOnLadder(ButtonsToPress.W, PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
            }
            else if (isOnLadder && ladderSteppedOn.transform.rotation.eulerAngles.y == 90)
            {
                if (PlayerStats.Instance.stats.steps_Current <= 0)
                {
                    RespawnBasedOnStepsCurrent();
                }

                else if (Cameras.Instance.cameraState == CameraState.Left && !PlayerManager.Instance.canMove_Forward)
                    MovePlayerOnLadder_UP();
                else if (Cameras.Instance.cameraState == CameraState.Right && !PlayerManager.Instance.canMove_Back)
                {
                    if (!ladderAndIsOnTheGround)
                        MovePlayerOnLadder_DOWN();
                    else
                        BypassIsOnLadder(ButtonsToPress.W, PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
                }
                else if (Ladder_Down_MoveAway())
                    BypassIsOnLadder(ButtonsToPress.W, PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
            }
            else if (isOnLadder && (ladderSteppedOn.transform.rotation.eulerAngles.y == -90 || ladderSteppedOn.transform.rotation.eulerAngles.y == 270))
            {
                if (PlayerStats.Instance.stats.steps_Current <= 0)
                {
                    RespawnBasedOnStepsCurrent();
                }

                else if (Cameras.Instance.cameraState == CameraState.Right && !PlayerManager.Instance.canMove_Forward)
                    MovePlayerOnLadder_UP();
                else if (Cameras.Instance.cameraState == CameraState.Left && !PlayerManager.Instance.canMove_Back)
                {
                    if (!ladderAndIsOnTheGround)
                        MovePlayerOnLadder_DOWN();
                    else
                        BypassIsOnLadder(ButtonsToPress.W, PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
                }
                else if (Ladder_Down_MoveAway())
                    BypassIsOnLadder(ButtonsToPress.W, PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
            }

            //Normal Forward Press
            else
            {
                BypassIsOnLadder(ButtonsToPress.W, PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
            }
        }

        //If pressing Backward - Movement
        else if (Input.GetKey(KeyCode.S))
        {
            RaycastUnderPlayer();

            //S-press on a ladder
            if (isOnLadder && ladderSteppedOn.transform.rotation.eulerAngles.y == 0)
            {
                if (PlayerStats.Instance.stats.steps_Current <= 0)
                {
                    RespawnBasedOnStepsCurrent();
                }

                else if (Cameras.Instance.cameraState == CameraState.Forward && !PlayerManager.Instance.canMove_Forward)
                {
                    if (!ladderAndIsOnTheGround)
                        MovePlayerOnLadder_DOWN();
                    else
                        BypassIsOnLadder(ButtonsToPress.S, PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);
                }
                else if (Cameras.Instance.cameraState == CameraState.Backward && !PlayerManager.Instance.canMove_Back)
                    MovePlayerOnLadder_UP();
                else if (Ladder_Down_MoveAway())
                    BypassIsOnLadder(ButtonsToPress.S, PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);

            }
            else if (isOnLadder && ladderSteppedOn.transform.rotation.eulerAngles.y == 180)
            {
                if (PlayerStats.Instance.stats.steps_Current <= 0)
                {
                    RespawnBasedOnStepsCurrent();
                }

                else if (Cameras.Instance.cameraState == CameraState.Backward && !PlayerManager.Instance.canMove_Forward)
                {
                    if (!ladderAndIsOnTheGround)
                        MovePlayerOnLadder_DOWN();
                    else
                        BypassIsOnLadder(ButtonsToPress.S, PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);
                }
                else if (Cameras.Instance.cameraState == CameraState.Forward && !PlayerManager.Instance.canMove_Back)
                    MovePlayerOnLadder_UP();
                else if (Ladder_Down_MoveAway())
                    BypassIsOnLadder(ButtonsToPress.S, PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);
            }
            else if (isOnLadder && ladderSteppedOn.transform.rotation.eulerAngles.y == 90)
            {
                if (PlayerStats.Instance.stats.steps_Current <= 0)
                {
                    RespawnBasedOnStepsCurrent();
                }

                else if (Cameras.Instance.cameraState == CameraState.Left && !PlayerManager.Instance.canMove_Forward)
                {
                    if (!ladderAndIsOnTheGround)
                        MovePlayerOnLadder_DOWN();
                    else
                        BypassIsOnLadder(ButtonsToPress.S, PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);
                }
                else if (Cameras.Instance.cameraState == CameraState.Right && !PlayerManager.Instance.canMove_Back)
                    MovePlayerOnLadder_UP();
                else if (Ladder_Down_MoveAway())
                    BypassIsOnLadder(ButtonsToPress.S, PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);
            }
            else if (isOnLadder && (ladderSteppedOn.transform.rotation.eulerAngles.y == -90 || ladderSteppedOn.transform.rotation.eulerAngles.y == 270))
            {
                if (PlayerStats.Instance.stats.steps_Current <= 0)
                {
                    RespawnBasedOnStepsCurrent();
                }

                else if (Cameras.Instance.cameraState == CameraState.Right && !PlayerManager.Instance.canMove_Forward)
                {
                    if (!ladderAndIsOnTheGround)
                        MovePlayerOnLadder_DOWN();
                    else
                        BypassIsOnLadder(ButtonsToPress.S, PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);
                }
                else if (Cameras.Instance.cameraState == CameraState.Left && !PlayerManager.Instance.canMove_Back)
                    MovePlayerOnLadder_UP();
                else if (Ladder_Down_MoveAway())
                    BypassIsOnLadder(ButtonsToPress.S, PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);
            }

            //Normal Backward Press
            else
            {
                BypassIsOnLadder(ButtonsToPress.S, PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);
            }
        }

        //If pressing Left - Movement
        else if (Input.GetKey(KeyCode.A))
        {
            RaycastUnderPlayer();

            //A-press on a ladder
            if (isOnLadder && ladderSteppedOn.transform.rotation.eulerAngles.y == 0)
            {
                if (PlayerStats.Instance.stats.steps_Current <= 0)
                {
                    RespawnBasedOnStepsCurrent();
                }

                else if (Cameras.Instance.cameraState == CameraState.Left && !PlayerManager.Instance.canMove_Left)
                    MovePlayerOnLadder_UP();
                else if (Cameras.Instance.cameraState == CameraState.Right && !PlayerManager.Instance.canMove_Right)
                {
                    if (!ladderAndIsOnTheGround)
                        MovePlayerOnLadder_DOWN();
                    else
                        BypassIsOnLadder(ButtonsToPress.A, PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
                }
                else if (Ladder_Down_MoveAway())
                    BypassIsOnLadder(ButtonsToPress.A, PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
            }
            else if (isOnLadder && ladderSteppedOn.transform.rotation.eulerAngles.y == 180)
            {
                if (PlayerStats.Instance.stats.steps_Current <= 0)
                {
                    RespawnBasedOnStepsCurrent();
                }

                else if (Cameras.Instance.cameraState == CameraState.Right && !PlayerManager.Instance.canMove_Left)
                    MovePlayerOnLadder_UP();
                else if (Cameras.Instance.cameraState == CameraState.Left && !PlayerManager.Instance.canMove_Right)
                {
                    if (!ladderAndIsOnTheGround)
                        MovePlayerOnLadder_DOWN();
                    else
                        BypassIsOnLadder(ButtonsToPress.A, PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
                }
                else if (Ladder_Down_MoveAway())
                    BypassIsOnLadder(ButtonsToPress.A, PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
            }
            else if (isOnLadder && ladderSteppedOn.transform.rotation.eulerAngles.y == 90)
            {
                if (PlayerStats.Instance.stats.steps_Current <= 0)
                {
                    RespawnBasedOnStepsCurrent();
                }

                else if (Cameras.Instance.cameraState == CameraState.Backward && !PlayerManager.Instance.canMove_Left)
                    MovePlayerOnLadder_UP();
                else if (Cameras.Instance.cameraState == CameraState.Forward && !PlayerManager.Instance.canMove_Right)
                {
                    if (!ladderAndIsOnTheGround)
                        MovePlayerOnLadder_DOWN();
                    else
                        BypassIsOnLadder(ButtonsToPress.A, PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
                }
                else if (Ladder_Down_MoveAway())
                    BypassIsOnLadder(ButtonsToPress.A, PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
            }
            else if (isOnLadder && (ladderSteppedOn.transform.rotation.eulerAngles.y == -90 || ladderSteppedOn.transform.rotation.eulerAngles.y == 270))
            {
                if (PlayerStats.Instance.stats.steps_Current <= 0)
                {
                    RespawnBasedOnStepsCurrent();
                }

                else if (Cameras.Instance.cameraState == CameraState.Forward && !PlayerManager.Instance.canMove_Left)
                    MovePlayerOnLadder_UP();
                else if (Cameras.Instance.cameraState == CameraState.Backward && !PlayerManager.Instance.canMove_Right)
                {
                    if (!ladderAndIsOnTheGround)
                        MovePlayerOnLadder_DOWN();
                    else
                        BypassIsOnLadder(ButtonsToPress.A, PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
                }
                else if (Ladder_Down_MoveAway())
                    BypassIsOnLadder(ButtonsToPress.A, PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
            }

            else
            {
                BypassIsOnLadder(ButtonsToPress.A, PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
            }
        }

        //If pressing RIGHT - Movement
        else if (Input.GetKey(KeyCode.D))
        {
            RaycastUnderPlayer();

            //D-press on a ladder
            if (isOnLadder && ladderSteppedOn.transform.rotation.eulerAngles.y == 0)
            {
                if (PlayerStats.Instance.stats.steps_Current <= 0)
                {
                    RespawnBasedOnStepsCurrent();
                }

                else if (Cameras.Instance.cameraState == CameraState.Left && !PlayerManager.Instance.canMove_Left)
                {
                    if (!ladderAndIsOnTheGround)
                        MovePlayerOnLadder_DOWN();
                    else
                        BypassIsOnLadder(ButtonsToPress.D, PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
                }
                else if (Cameras.Instance.cameraState == CameraState.Right && !PlayerManager.Instance.canMove_Right)
                    MovePlayerOnLadder_UP();
                else if (Ladder_Down_MoveAway())
                    BypassIsOnLadder(ButtonsToPress.D, PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
            }
            else if (isOnLadder && ladderSteppedOn.transform.rotation.eulerAngles.y == 180)
            {
                if (PlayerStats.Instance.stats.steps_Current <= 0)
                {
                    RespawnBasedOnStepsCurrent();
                }

                else if (Cameras.Instance.cameraState == CameraState.Right && !PlayerManager.Instance.canMove_Left)
                {
                    if (!ladderAndIsOnTheGround)
                        MovePlayerOnLadder_DOWN();
                    else
                        BypassIsOnLadder(ButtonsToPress.D, PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
                }
                else if (Cameras.Instance.cameraState == CameraState.Left && !PlayerManager.Instance.canMove_Right)
                    MovePlayerOnLadder_UP();
                else if (Ladder_Down_MoveAway())
                    BypassIsOnLadder(ButtonsToPress.D, PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
            }
            else if (isOnLadder && ladderSteppedOn.transform.rotation.eulerAngles.y == 90)
            {
                if (PlayerStats.Instance.stats.steps_Current <= 0)
                {
                    RespawnBasedOnStepsCurrent();
                }

                else if (Cameras.Instance.cameraState == CameraState.Backward && !PlayerManager.Instance.canMove_Left)
                {
                    if (!ladderAndIsOnTheGround)
                        MovePlayerOnLadder_DOWN();
                    else
                        BypassIsOnLadder(ButtonsToPress.D, PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
                }
                else if (Cameras.Instance.cameraState == CameraState.Forward && !PlayerManager.Instance.canMove_Right)
                    MovePlayerOnLadder_UP();
                else if (Ladder_Down_MoveAway())
                    BypassIsOnLadder(ButtonsToPress.D, PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
            }
            else if (isOnLadder && (ladderSteppedOn.transform.rotation.eulerAngles.y == -90 || ladderSteppedOn.transform.rotation.eulerAngles.y == 270))
            {
                if (PlayerStats.Instance.stats.steps_Current <= 0)
                {
                    RespawnBasedOnStepsCurrent();
                }

                else if (Cameras.Instance.cameraState == CameraState.Forward && !PlayerManager.Instance.canMove_Left)
                {
                    if (!ladderAndIsOnTheGround)
                        MovePlayerOnLadder_DOWN();
                    else
                        BypassIsOnLadder(ButtonsToPress.D, PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
                }
                else if (Cameras.Instance.cameraState == CameraState.Backward && !PlayerManager.Instance.canMove_Right)
                    MovePlayerOnLadder_UP();
                else if (Ladder_Down_MoveAway())
                    BypassIsOnLadder(ButtonsToPress.D, PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
            }

            else
            {
                BypassIsOnLadder(ButtonsToPress.D, PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
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

        //If pressing - Hammer - C
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
    void RaycastUnderPlayer()
    {
        if (Physics.Raycast(PlayerManager.Instance.player.transform.position, Vector3.down, out hit, 1))
        {
            if (hit.transform.gameObject)
            {
                if (hit.transform.gameObject.GetComponent<BlockInfo>())
                {
                    if (hit.transform.gameObject.GetComponent<BlockInfo>().blockType == BlockType.Cube)
                        ladderAndIsOnTheGround = true;
                    else
                        ladderAndIsOnTheGround = false;
                }
                else
                    ladderAndIsOnTheGround = false;
            }
            else
                ladderAndIsOnTheGround = false;
        }
        else
            ladderAndIsOnTheGround = false;
    }
    void BypassIsOnLadder(ButtonsToPress button, bool canMove, DetectedBlockInfo detectedBlock, int rotation)
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
    public void SetPlayerBodyRotation(int rotationValue)
    {
        //if (isOnLadder && !ladderAndIsOnTheGround /*Ladder_Down_MoveAway()*/) { return; }

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


    //--------------------


    #region Ladder
    void MovePlayerOnLadder_UP()
    {
        ResetLadderMovementParameters();

        if (ladderMovement_Up) { return; }
        if (ladderMovement_Down) { return; }

        Action_resetBlockColor();

        if (ladderSteppedOn)
        {
            //If ladder isn't the top one
            if (ladderSteppedOn.GetComponent<Block_Ladder>().ladder_Over)
            {
                ladderToApproach_Current = ladderSteppedOn.GetComponent<Block_Ladder>().ladder_Over;
                ladderMovement_Up = true;
            }

            //If ladder is the top one
            else
            {
                ladder_Top = ladderSteppedOn;
                ladderMovement_Top = true;
                ladderMovement_Down_ToBlockFromTop = false;
                LadderMovement_Top();
            }
        }
    }
    void LadderMovement_UP(ref GameObject ladder)
    {
        if (ladder == null) { return; }

        if (MovementTransition(ladder.transform.position + Vector3.down, 3))
        {
            PlayerStats.Instance.stats.steps_Current -= ladder.GetComponent<BlockInfo>().movementCost;

            ladder = null;

            RaycastDarkenBlockOverLadder();

            ladderMovement_Down_ToBlockFromTop = false;
            ladderMovement_Up = false;

            Player_BlockDetector.Instance.Update_BlockStandingOn();

            Action_StepTakenInvoke();
        }
    }
    void LadderMovement_Top()
    {
        if (MovementTransition(ladder_Top.transform.position, ladder_Top.GetComponent<BlockInfo>().movementSpeed))
        {
            if (ladderSteppedOn.transform.rotation.eulerAngles.y == 0)
                ladderTop_EndPos = ladderSteppedOn.transform.position + Vector3.forward;
            else if (ladderSteppedOn.transform.rotation.eulerAngles.y == 180)
                ladderTop_EndPos = ladderSteppedOn.transform.position + Vector3.back;
            else if (ladderSteppedOn.transform.rotation.eulerAngles.y == 90)
                ladderTop_EndPos = ladderSteppedOn.transform.position + Vector3.right;
            else if (ladderSteppedOn.transform.rotation.eulerAngles.y == -90 || ladderSteppedOn.transform.rotation.eulerAngles.y == 270)
                ladderTop_EndPos = ladderSteppedOn.transform.position + Vector3.left;

            ladderSteppedOn = null;
            isOnLadder = false;
            ladderMovement_Down_ToBlockFromTop = false;
            ladderMovement_Top_ToBlock = true;
            ladderMovement_Top = false;
        }
    }
    void LadderMovement_Top_ToBlock()
    {
        if (MovementTransition(ladderTop_EndPos, 3))
        {
            Ladder_PlayerRotation_Away();

            ladderMovement_Down_ToBlockFromTop = false;
            ladderSteppedOn = null;
            ladderMovement_Top_ToBlock = false;
            isOnLadder = false;
            Action_StepTakenInvoke();

            if (PlayerStats.Instance.stats.steps_Current < 0)
            {
                RespawnBasedOnStepsCurrent();
            }
            else
            {
                PlayerStats.Instance.stats.steps_Current -= PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementCost;

                IceGlide();
            }
        }
    }

    void MovePlayerOnLadder_DOWN()
    {
        Action_resetBlockColor();
        ResetLadderMovementParameters();

        if (ladderMovement_Down) { return; }
        if (ladderMovement_Up) { return; }

        if (ladderSteppedOn)
        {
            if (ladderSteppedOn.GetComponent<Block_Ladder>().ladder_Over == null)
            {
                ladderToApproach_Current = ladderSteppedOn.GetComponent<Block_Ladder>().ladder_Under;
                ladderMovement_Down = true;
            }
            else if (ladderSteppedOn.GetComponent<Block_Ladder>().ladder_Under)
            {
                ladderToApproach_Current = ladderSteppedOn.GetComponent<Block_Ladder>().ladder_Under;
                ladderMovement_Down = true;
            }
            else
            {
                ladderMovement_Top_ToBlock = false;
                isOnLadder = false;
                ladderMovement_Down = false;

                Action_StepTakenInvoke();
            }
        }
    }
    void LadderMovement_DOWN(ref GameObject ladder)
    {
        if (ladder == null) { ladderMovement_Down = false; return; }

        if (MovementTransition((ladder.transform.position + Vector3.down), 3))
        {
            PlayerStats.Instance.stats.steps_Current -= ladder.GetComponent<BlockInfo>().movementCost;

            ladder = null;

            Action_StepTakenInvoke();

            //If the lowest ladder-part
            if (ladderSteppedOn.GetComponent<Block_Ladder>().block_Under)
            {
                ladderToApproach_Current = ladderSteppedOn;
                ladderMovement_Down_ToBottom = true;
            }

            ladderMovement_Down = false;
        }
    }
    void LadderMovement_Down_ToBottom()
    {
        if (ladderToApproach_Current.GetComponent<Block_Ladder>())
        {
            if (ladderToApproach_Current.GetComponent<Block_Ladder>().block_Under)
            {
                if (MovementTransition(ladderToApproach_Current.GetComponent<Block_Ladder>().block_Under.transform.position, 3))
                {
                    ladderToApproach_Current = null;

                    ladderMovement_Down_ToBlockFromTop = false;
                    isOnLadder = true;
                    Action_StepTakenInvoke();

                    if (PlayerStats.Instance.stats.steps_Current <= 0)
                    {
                        RespawnBasedOnStepsCurrent();
                    }
                    else
                    {
                        ladderMovement_Down_ToBottom = false;

                        RaycastUnderPlayer();
                        Ladder_PlayerRotation_Away();
                    }
                }
            }
        }
    }
    void LadderMovement_Down_ToBlockFromTop()
    {
        if (ladderToApproach_Current)
        {
            if (ladderToApproach_Current.GetComponent<Block_Ladder>().ladder_Under)
            {
                if (MovementTransition(ladderToApproach_Current.GetComponent<Block_Ladder>().ladder_Under.transform.position, 3))
                {
                    PlayerStats.Instance.stats.steps_Current -= ladderToApproach_Current.GetComponent<BlockInfo>().movementCost;

                    RaycastDarkenBlockOverLadder();

                    ladderToApproach_Current = null;

                    ladderMovement_Down_ToBlockFromTop = false;
                    isOnLadder = true;
                    Action_StepTakenInvoke();
                }
            }
            else
            {
                //If the lowest ladder-part
                if (ladderSteppedOn.GetComponent<Block_Ladder>().block_Under)
                {
                    ladderToApproach_Current = ladderSteppedOn;
                    ladderMovement_Down_ToBottom = true;
                }

                ladderMovement_Down = false;
            }
        }
    }

    void ResetLadderMovementParameters()
    {
        //ladderSteppedOn = null;

        ladderMovement_Up = false;
        ladderMovement_Down = false;

        ladderMovement_Top = false;

        ladderMovement_Top_ToBlock = false;
        ladderMovement_Down_ToBlockFromTop = false;

        ladderMovement_Down_ToBottom = false;

        ladderToApproach_Current = null;
        ladder_Top = null;
}
    void RaycastDarkenBlockOverLadder()
    {
        RaycastHit hit;

        if (PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.y == 0)
        {
            if (Physics.Raycast(transform.position + Vector3.up + Vector3.forward, Vector3.down, out hit, 1))
            {
                if (hit.transform.gameObject.GetComponent<BlockInfo>())
                {
                    hit.transform.gameObject.GetComponent<BlockInfo>().DarkenColors();
                }
            }
        }
        else if (PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.y == 180)
        {
            if (Physics.Raycast(transform.position + Vector3.up + Vector3.back, Vector3.down, out hit, 1))
            {
                if (hit.transform.gameObject.GetComponent<BlockInfo>())
                {
                    hit.transform.gameObject.GetComponent<BlockInfo>().DarkenColors();
                }
            }
        }
        else if (PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.y == 90)
        {
            if (Physics.Raycast(transform.position + Vector3.up + Vector3.right, Vector3.down, out hit, 1))
            {
                if (hit.transform.gameObject.GetComponent<BlockInfo>())
                {
                    hit.transform.gameObject.GetComponent<BlockInfo>().DarkenColors();
                }
            }
        }
        else if (PlayerManager.Instance.playerBody.transform.rotation.eulerAngles.y == -90)
        {
            print("111. True");
            if (Physics.Raycast(transform.position + Vector3.up + Vector3.left, Vector3.down, out hit, 1))
            {
                print("222. True");
                if (hit.transform.gameObject.GetComponent<BlockInfo>())
                {
                    print("333. Name: " + hit.transform.gameObject.name + " | Pos: " + hit.transform.gameObject.transform.position);
                    hit.transform.gameObject.GetComponent<BlockInfo>().DarkenColors();
                }
            }
        }
    }

    bool Ladder_Down_MoveAway()
    {
        if (isOnLadder && ladderSteppedOn)
        {
            if (ladderSteppedOn.GetComponent<Block_Ladder>())
            {
                if (ladderSteppedOn.GetComponent<Block_Ladder>().block_Under)
                {
                    Ladder_PlayerRotation_Away();

                    return true;
                }
            }
        }

        return false;
    }

    public void Ladder_PlayerRotation_Into()
    {
        if (!ladderSteppedOn) { return; }

        switch (Cameras.Instance.cameraState)
        {
            case CameraState.Forward:
                if (ladderSteppedOn.transform.rotation.eulerAngles.y == 0)
                    SetPlayerBodyRotation(0);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == 180)
                    SetPlayerBodyRotation(180);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == 90)
                    SetPlayerBodyRotation(90);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == -90 || ladderSteppedOn.transform.rotation.eulerAngles.y == 270)
                    SetPlayerBodyRotation(-90);
                break;
            case CameraState.Backward:
                if (ladderSteppedOn.transform.rotation.eulerAngles.y == 0)
                    SetPlayerBodyRotation(180);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == 180)
                    SetPlayerBodyRotation(0);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == 90)
                    SetPlayerBodyRotation(-90);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == -90 || ladderSteppedOn.transform.rotation.eulerAngles.y == 270)
                    SetPlayerBodyRotation(90);
                break;
            case CameraState.Left:
                if (ladderSteppedOn.transform.rotation.eulerAngles.y == 0)
                    SetPlayerBodyRotation(-90);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == 180)
                    SetPlayerBodyRotation(90);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == 90)
                    SetPlayerBodyRotation(0);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == -90 || ladderSteppedOn.transform.rotation.eulerAngles.y == 270)
                    SetPlayerBodyRotation(180);
                break;
            case CameraState.Right:
                if (ladderSteppedOn.transform.rotation.eulerAngles.y == 0)
                    SetPlayerBodyRotation(90);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == 180)
                    SetPlayerBodyRotation(-90);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == 90)
                    SetPlayerBodyRotation(180);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == -90 || ladderSteppedOn.transform.rotation.eulerAngles.y == 270)
                    SetPlayerBodyRotation(0);
                break;

            default:
                break;
        }
    }
    public void Ladder_PlayerRotation_Away()
    {
        if (!ladderSteppedOn) { return; }

        switch (Cameras.Instance.cameraState)
        {
            case CameraState.Forward:
                if (ladderSteppedOn.transform.rotation.eulerAngles.y == 0)
                    SetPlayerBodyRotation(180);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == 180)
                    SetPlayerBodyRotation(0);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == 90)
                    SetPlayerBodyRotation(-90);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == -90 || ladderSteppedOn.transform.rotation.eulerAngles.y == 270)
                    SetPlayerBodyRotation(90);
                break;
            case CameraState.Backward:
                if (ladderSteppedOn.transform.rotation.eulerAngles.y == 0)
                    SetPlayerBodyRotation(0);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == 180)
                    SetPlayerBodyRotation(180);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == 90)
                    SetPlayerBodyRotation(90);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == -90 || ladderSteppedOn.transform.rotation.eulerAngles.y == 270)
                    SetPlayerBodyRotation(-90);
                break;
            case CameraState.Left:
                if (ladderSteppedOn.transform.rotation.eulerAngles.y == 0)
                    SetPlayerBodyRotation(90);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == 180)
                    SetPlayerBodyRotation(-90);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == 90)
                    SetPlayerBodyRotation(180);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == -90 || ladderSteppedOn.transform.rotation.eulerAngles.y == 270)
                    SetPlayerBodyRotation(0);
                break;
            case CameraState.Right:
                if (ladderSteppedOn.transform.rotation.eulerAngles.y == 0)
                    SetPlayerBodyRotation(-90);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == 180)
                    SetPlayerBodyRotation(90);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == 90)
                    SetPlayerBodyRotation(0);
                else if (ladderSteppedOn.transform.rotation.eulerAngles.y == -90 || ladderSteppedOn.transform.rotation.eulerAngles.y == 270)
                    SetPlayerBodyRotation(180);
                break;

            default:
                break;
        }
    }

    void RespawnBasedOnStepsCurrent()
    {
        if (PlayerStats.Instance.stats.steps_Current <= 0)
        {
            PlayerStats.Instance.RespawnPlayer();

            isOnLadder = false;
            ladderSteppedOn = null;
            ladderMovement_Up = false;
            ladderMovement_Down = false;
            ladderMovement_Top = false;
            ladderMovement_Top_ToBlock = false;
            ladderMovement_Down_ToBlockFromTop = false;
            ladderMovement_Down_ToBottom = false;

            ladderTop_EndPos = Vector3.zero;
            ladderToApproach_Current = null;
            ladder_Top = null;
            ladderAndIsOnTheGround = false;

            PlayerStats.Instance.RespawnPlayer();
        }
    }
    #endregion


    //--------------------


    bool MovementTransition(Vector3 endPos, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, endPos + (Vector3.up * heightOverBlock), speed * Time.deltaTime);

        //Snap into place when close enough
        if (Vector3.Distance(transform.position, endPos + (Vector3.up * heightOverBlock)) <= 0.03f)
        {
            transform.position = endPos + (Vector3.up * heightOverBlock);

            return true;
        }

        return false;
    }


    //--------------------


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
    public void Action_StepCostTakenInvoke()
    {
        Action_StepCostTaken?.Invoke();
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