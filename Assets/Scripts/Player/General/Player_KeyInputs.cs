using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_KeyInputs : Singleton<Player_KeyInputs>
{
    public static event Action Action_WalkButton_isPressed;
    public static event Action Action_WalkButton_isReleased;

    public static event Action Action_Ascend_isPressed;
    public static event Action Action_Descend_isPressed;
    public static event Action Action_CeilingGrab_isPressed;
    public static event Action Action_GrapplingHook_isPressed;

    public static event Action Action_dialogueButton_isPressed;
    public static event Action Action_dialogueNextButton_isPressed;
    public static event Action Action_InteractButton_isPressed;

    public static event Action Action_RespawnHold;
    public static event Action Action_RespawnCanceled;

    public static event Action Action_FreeCamIsActive;
    public static event Action Action_FreeCamIsPassive;

    public static event Action Action_PauseMenuIsPressed;

    [Header("Input System")]
    public PlayerControls playerControls;
    MapManager mapManager;

    [Header("KeyPresses")]
    public bool forward_isPressed = false;
    public bool back_isPressed = false;
    public bool left_isPressed = false;
    public bool right_isPressed = false;

    public bool forward_isHold = false;
    public bool back_isHold = false;
    public bool left_isHold = false;
    public bool right_isHold = false;

    public bool up_isPressed = false;
    public bool down_isPressed = false;

    public bool grapplingHook_isPressed = false;

    public bool cameraX_isPressed = false;
    public bool cameraY_isPressed = false;

    public bool respawn_isPressed = false;

    public bool freeCam_isPressed = false;

    PauseMenuManager pauseMenuManager;

    private Vector2 _lastMousePos;



    //--------------------


    private void Start()
    {
        pauseMenuManager = FindAnyObjectByType<PauseMenuManager>();

        playerControls = new PlayerControls();
        mapManager = FindObjectOfType<MapManager>();

        if (PlayerManager.Instance.playerBody.transform.GetComponentInChildren<Animator>())
        {
            Player_Animations.Instance.playerAnimator = PlayerManager.Instance.playerBody.GetComponentInChildren<Animator>();
        }
    }
    private void Update()
    {
        //if (!freeCam_isPressed) return;

        //var pad = Gamepad.current;
        //if (pad == null) return;

        //// Only poll sticks if FreeCam is set to controller mode
        //if (FreeCam.Instance.CurrentInputType == InputType.Keyboard)
        //    return;

        //FreeCam.Instance.SetMoveAxis(pad.leftStick.ReadValue());
        //FreeCam.Instance.RotateFromStick(pad.rightStick.ReadValue());
    }


    //--------------------


    void OnForward_Down()
    {
        if (!Run_AbilityDisplayExit()) return;
        if (freeCam_isPressed) return;

        Run_Movement_Tutorial(ref forward_isPressed);

        if (!ButtonChecks_Movement()) { return; }

        forward_isPressed = true;
        Action_WalkButton_isPressed?.Invoke();
    }
    void OnForward_Hold()
    {
        if (!ButtonChecks_Movement()) { return; }

        if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>() && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement != BlockElement.Ice)
            forward_isHold = true;
    }
    void OnForward_Up()
    {
        if (freeCam_isPressed) return;

        forward_isPressed = false;
        forward_isHold = false;

        Action_WalkButton_isReleased?.Invoke();
    }
   
    void OnBackward_Down()
    {
        if (!Run_AbilityDisplayExit()) return;
        if (freeCam_isPressed) return;

        Run_Movement_Tutorial(ref back_isPressed);

        if (!ButtonChecks_Movement()) { return; }

        back_isPressed = true;
        Action_WalkButton_isPressed?.Invoke();
    }
    void OnBackward_Hold()
    {
        if (!ButtonChecks_Movement()) { return; }

        if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>() && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement != BlockElement.Ice)
            back_isHold = true;
    }
    void OnBackward_Up()
    {
        if (freeCam_isPressed) return;

        back_isPressed = false;
        back_isHold = false;

        Action_WalkButton_isReleased?.Invoke();
    }
  
    void OnLeft_Down()
    {
        if (!Run_AbilityDisplayExit()) return;
        if (freeCam_isPressed) return;

        Run_Movement_Tutorial(ref left_isPressed);

        if (!ButtonChecks_Movement()) { return; }

        left_isPressed = true;
        Action_WalkButton_isPressed?.Invoke();
    }
    void OnLeft_Hold()
    {
        if (!ButtonChecks_Movement()) { return; }

        if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>() && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement != BlockElement.Ice)
            left_isHold = true;
    }
    void OnLeft_Up()
    {
        if (freeCam_isPressed) return;

        left_isPressed = false;
        left_isHold = false;

        Action_WalkButton_isReleased?.Invoke();
    }
  
    void OnRight_Down()
    {
        if (!Run_AbilityDisplayExit()) return;
        if (freeCam_isPressed) return;

        Run_Movement_Tutorial(ref right_isPressed);

        if (!ButtonChecks_Movement()) { return; }

        right_isPressed = true;
        Action_WalkButton_isPressed?.Invoke();
    }
    void OnRight_Hold()
    {
        if (!ButtonChecks_Movement()) { return; }

        if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>() && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement != BlockElement.Ice)
            right_isHold = true;
    }
    void OnRight_Up()
    {
        if (freeCam_isPressed) return;

        right_isPressed = false;
        right_isHold = false;

        Action_WalkButton_isReleased?.Invoke();
    }


    //--------------------


    void OnAbilityUp_Down()
    {
        if (!Run_AbilityDisplayExit()) return;
        if (freeCam_isPressed) return;

        if (!ButtonChecks_Movement() || Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (Movement.Instance.isGrapplingHooking) return;
        if (Movement.Instance.isJumping) return;
        if (Movement.Instance.isDashing) return;
        if (Movement.Instance.isSwiftSwim) return;
        if (Movement.Instance.isAscending) return;
        if (Movement.Instance.isDescending) return;
        if (grapplingHook_isPressed) return;

        print("1. OnAbilityUp_Down");

        up_isPressed = true;
        Action_Ascend_isPressed?.Invoke();
    }
    void OnAbilityUp_Up()
    {
        if (!ButtonChecks_Movement()) { return; }
        if (freeCam_isPressed) return;

        up_isPressed = false;
    }

    void OnAbilityDown_Down()
    {
        if (!Run_AbilityDisplayExit()) return;
        if (freeCam_isPressed) return;

        if (!ButtonChecks_Movement() || Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (Movement.Instance.isGrapplingHooking) return;
        if (Movement.Instance.isJumping) return;
        if (Movement.Instance.isDashing) return;
        if (Movement.Instance.isSwiftSwim) return;
        if (Movement.Instance.isAscending) return;
        if (Movement.Instance.isDescending) return;
        if (grapplingHook_isPressed) return;

        print("2. OnAbilityDown_Down");

        down_isPressed = true;
        Action_Descend_isPressed?.Invoke();
    }
    void OnAbilityDown_Up()
    {
        if (!ButtonChecks_Movement()) { return; }
        if (freeCam_isPressed) return;

        down_isPressed = false;
    }

    void OnAbilityLeft()
    {
        if (!Run_AbilityDisplayExit()) return;
        if (freeCam_isPressed) return;

        if (!ButtonChecks_Other()) { return; }
        if (Movement.Instance.movementStates == MovementStates.Falling) { return; }

        if (Movement.Instance.isGrapplingHooking) return;
        if (Movement.Instance.isJumping) return;
        if (Movement.Instance.isDashing) return;
        if (Movement.Instance.isSwiftSwim) return;
        if (Movement.Instance.isAscending) return;
        if (Movement.Instance.isDescending) return;
        if (grapplingHook_isPressed) return;

        print("3. OnAbilityLeft");

        Player_CeilingGrab.Instance.CeilingGrab();

        Action_CeilingGrab_isPressed?.Invoke();
    }

    void OnAbilityRight_DownPress()
    {
        if (!Run_AbilityDisplayExit()) return;
        if (freeCam_isPressed) return;

        if (!ButtonChecks_Other() || Player_CeilingGrab.Instance.isCeilingGrabbing) return;
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.GrapplingHook && !PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook) return;

        if (Movement.Instance.isJumping) return;
        if (Movement.Instance.isDashing) return;
        if (Movement.Instance.isSwiftSwim) return;
        if (Movement.Instance.isAscending) return;
        if (Movement.Instance.isDescending) return;

        print("4. OnAbilityRight_DownPress");

        grapplingHook_isPressed = true;

        Movement.Instance.Action_isGrapplingHooking_Invoke();

        Player_Animations.Instance.Trigger_GrapplingHookAnimation();
    }
    void OnAbilityRight_RelesePress()
    {
        if (!Run_AbilityDisplayExit()) return;
        if (freeCam_isPressed) return;

        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.GrapplingHook && !PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook) return;

        grapplingHook_isPressed = false;

        if (!ButtonChecks_Other_MinusGrapplingHook()) { return; }

        Movement.Instance.UpdateGrapplingHookMovement_Release();

        Movement.Instance.Action_isGrapplingHooking_Finished_Invoke();
    }


    //--------------------


    void OnDialogueSkip_Pressed()
    {
        if (!Run_AbilityDisplayExit()) return;
        if (freeCam_isPressed) return;

        if (!PlayerManager.Instance.npcInteraction) { return; }

        Action_dialogueButton_isPressed?.Invoke();
    }
    void OnDialogueNext_Pressed()
    {
        if (!Run_AbilityDisplayExit()) return;
        if (freeCam_isPressed) return;

        if (!PlayerManager.Instance.npcInteraction) { return; }

        Action_dialogueNextButton_isPressed?.Invoke();
    }
    void OnInteractButton_Pressed()
    {
        if (!Run_AbilityDisplayExit()) return;
        if (freeCam_isPressed) return;

        if (!ButtonChecks_Movement()) { return; }
        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return; }

        Action_InteractButton_isPressed?.Invoke();
    }


    //--------------------


    void OnCameraRotateX()
    {
        if (!Run_AbilityDisplayExit()) return;
        if (freeCam_isPressed) return;

        Run_CameraRotation_Tutorial(true);

        if (!ButtonChecks_Other()) { return; }
        if (Movement.Instance.isUpdatingDarkenBlocks) { return; }

        MapManager.Instance.cameraRotated++;
        CameraController.Instance.RotateCameraX();
    }
    void OnCameraRotateX_Down()
    {
        //FreeCam Movement
        if (freeCam_isPressed)
        {
            FreeCam.Instance.SetMoveDirection(Vector3.down, true);
        }
    }
    void OnCameraRotateX_Up()
    {
        //FreeCam Movement
        if (freeCam_isPressed)
        {
            FreeCam.Instance.SetMoveDirection(Vector3.down, false);
        }
    }
    void OnCameraRotateY()
    {
        if (!Run_AbilityDisplayExit()) return;
        if (freeCam_isPressed) return;

        Run_CameraRotation_Tutorial(false);

        if (!ButtonChecks_Other()) { return; }
        if (Movement.Instance.isUpdatingDarkenBlocks) { return; }

        MapManager.Instance.cameraRotated++;
        CameraController.Instance.RotateCameraY();
    }
    void OnCameraRotateY_Down()
    {
        FreeCam.Instance.SetMoveDirection(Vector3.up, true);
    }
    void OnCameraRotateY_Up()
    {
        FreeCam.Instance.SetMoveDirection(Vector3.up, false);
    }


    //--------------------


    void OnRespawn_In()
    {
        if (!Run_AbilityDisplayExit()) return;
        if (freeCam_isPressed) return;

        Run_Respawn_Tutorial();

        if (!ButtonChecks_Other()) { return; }

        respawn_isPressed = true;
        Action_RespawnHold?.Invoke();
    }
    void OnRespawn_Out()
    {
        if (freeCam_isPressed) return;

        respawn_isPressed = false;
        Action_RespawnCanceled?.Invoke();
    }


    //--------------------


    void OnFreeCam()
    {
        if (!Run_AbilityDisplayExit()) return;
        if (Tutorial.Instance.state_Movement || Tutorial.Instance.state_CameraRotation || Tutorial.Instance.state_Respawn) return;
        if (PopUpManager.Instance.ability_Active || PopUpManager.Instance.ability_CanBeClosed) return;

        if (Movement.Instance.GetMovementState() == MovementStates.Moving) return;
        if (Movement.Instance.GetMovementState() == MovementStates.Ability) return;

        if (Player_CeilingGrab.Instance.isCeilingRotation) return;
        if (PlayerManager.Instance.npcInteraction) return;
        if (CameraController.Instance.isRotating) return;
        if (Player_Interact.Instance.isInteracting) return;
        if (Tutorial.Instance.tutorial_isRunning) return;
        if (PopUpManager.Instance.ability_Active) return;
        if (PopUpManager.Instance.ability_CanBeClosed) return;
        if (MapManager.Instance.introSequence) return;

        if (Player_GraplingHook.Instance.isGrapplingHooking) return;

        if (grapplingHook_isPressed)
        {
            Movement.Instance.moveToBlock_GrapplingHook.canMoveTo = false;

            if (Movement.Instance.moveToBlock_GrapplingHook != null && Movement.Instance.moveToBlock_GrapplingHook.targetBlock.GetComponent<BlockInfo>())
            {
                Movement.Instance.moveToBlock_GrapplingHook.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
            }

            grapplingHook_isPressed = false;
        }

        if (pauseMenuManager && pauseMenuManager.pauseMenu_MainMenu_Parent.activeInHierarchy) return;


        if (freeCam_isPressed)
        {
            if (Tutorial.Instance.tutorial_isRunning && !Tutorial.Instance.state_FreeCam_2) return;

            PlayerManager.Instance.UnpauseGame();
            Run_FreeCam_2_Tutorial_End();

            freeCam_isPressed = false;

            Action_FreeCamIsPassive?.Invoke();
        }
        else
        {
            if (Tutorial.Instance.tutorial_isRunning && !Tutorial.Instance.state_FreeCam_1) return;

            freeCam_isPressed = true;
            PlayerManager.Instance.PauseGame();

            Run_FreeCam_1_Tutorial_End();

            Action_FreeCamIsActive?.Invoke();
        }
    }

    void OnMouse_Down()
    {
        _lastMousePos = Input.mousePosition;
        //FreeCam.Instance.BeginMouseLook();
    }
    void OnMouse_Up()
    {
        //FreeCam.Instance.EndMouseLook();
    }

    void OnFreeCam_Move(InputValue value)
    {
        if (!freeCam_isPressed) return;
        FreeCam.Instance.SetMoveAxis(value.Get<Vector2>());
    }
    void OnFreeCam_Look(InputValue value)
    {
        if (!freeCam_isPressed) return;
        FreeCam.Instance.SetLookAxis(value.Get<Vector2>());
    }

    void OnForward_FreeCam_Down()
    {
        if (!freeCam_isPressed) return;
        FreeCam.Instance.SetMoveDirection(Vector3.forward, true);
    }
    void OnForward_FreeCam_Up()
    {
        if (!freeCam_isPressed) return;
        FreeCam.Instance.SetMoveDirection(Vector3.forward, false);
    }
    void OnBackward_FreeCam_Down()
    {
        if (!freeCam_isPressed) return;
        FreeCam.Instance.SetMoveDirection(Vector3.back, true);
    }
    void OnBackward_FreeCam_Up()
    {
        if (!freeCam_isPressed) return;
        FreeCam.Instance.SetMoveDirection(Vector3.back, false);
    }
    void OnLeft_FreeCam_Down()
    {
        if (!freeCam_isPressed) return;
        FreeCam.Instance.SetMoveDirection(Vector3.left, true);
    }
    void OnLeft_FreeCam_Up()
    {
        if (!freeCam_isPressed) return;
        FreeCam.Instance.SetMoveDirection(Vector3.left, false);
    }
    void OnRight_FreeCam_Down()
    {
        if (!freeCam_isPressed) return;
        FreeCam.Instance.SetMoveDirection(Vector3.right, true);
    }
    void OnRight_FreeCam_Up()
    {
        if (!freeCam_isPressed) return;
        FreeCam.Instance.SetMoveDirection(Vector3.right, false);
    }


    //--------------------


    void OnQuit()
    {
        if (!Run_AbilityDisplayExit()) return;

        if (!ButtonChecks_Other()) { return; }

        Key_Quit();
    }

    bool ButtonChecks_Movement()
    {
        if (Movement.Instance.GetMovementState() == MovementStates.Ability) { return false; }

        if (PlayerManager.Instance.pauseGame) { return false; }
        if (PlayerManager.Instance.npcInteraction) { return false; }
        if (CameraController.Instance.isRotating) { return false; }
        if (Player_Interact.Instance.isInteracting) { return false; }
        if (Player_CeilingGrab.Instance.isCeilingRotation) { return false; }
        if (Tutorial.Instance.tutorial_isRunning) { return false; }
        if (PopUpManager.Instance.ability_Active) { return false; }
        if (PopUpManager.Instance.ability_CanBeClosed) { return false; }
        if (MapManager.Instance.introSequence) { return false; }

        if (grapplingHook_isPressed)
        {
            Movement.Instance.moveToBlock_GrapplingHook.canMoveTo = false;

            if (Movement.Instance.moveToBlock_GrapplingHook != null && Movement.Instance.moveToBlock_GrapplingHook.targetBlock.GetComponent<BlockInfo>())
            {
                Movement.Instance.moveToBlock_GrapplingHook.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
            }

            grapplingHook_isPressed = false;
        }

        return true;
    }
    bool ButtonChecks_Other()
    {
        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return false; }
        if (Movement.Instance.GetMovementState() == MovementStates.Ability) { return false; }

        if (Player_CeilingGrab.Instance.isCeilingRotation) { return false; }
        if (PlayerManager.Instance.pauseGame) { return false; }
        if (PlayerManager.Instance.npcInteraction) { return false; }
        if (CameraController.Instance.isRotating) { return false; }
        if (Player_Interact.Instance.isInteracting) { return false; }
        if (Tutorial.Instance.tutorial_isRunning) { return false; }
        if (PopUpManager.Instance.ability_Active) { return false; }
        if (PopUpManager.Instance.ability_CanBeClosed) { return false; }
        if (MapManager.Instance.introSequence) { return false; }

        if (Player_GraplingHook.Instance.isGrapplingHooking) { return false; }

        if (grapplingHook_isPressed)
        {
            Movement.Instance.moveToBlock_GrapplingHook.canMoveTo = false;

            if (Movement.Instance.moveToBlock_GrapplingHook != null && Movement.Instance.moveToBlock_GrapplingHook.targetBlock.GetComponent<BlockInfo>())
            {
                Movement.Instance.moveToBlock_GrapplingHook.targetBlock.GetComponent<BlockInfo>().ResetDarkenColor();
            }

            grapplingHook_isPressed = false;
        }

        return true;
    }
    bool ButtonChecks_Other_MinusGrapplingHook()
    {
        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return false; }
        if (Movement.Instance.GetMovementState() == MovementStates.Ability) { return false; }

        if (Player_CeilingGrab.Instance.isCeilingRotation) { return false; }
        if (PlayerManager.Instance.pauseGame) { return false; }
        if (PlayerManager.Instance.npcInteraction) { return false; }
        if (CameraController.Instance.isRotating) { return false; }
        if (Player_Interact.Instance.isInteracting) { return false; }
        if (Tutorial.Instance.tutorial_isRunning) { return false; }
        if (PopUpManager.Instance.ability_Active) { return false; }
        if (PopUpManager.Instance.ability_CanBeClosed) { return false; }
        if (MapManager.Instance.introSequence) { return false; }

        return true;
    }
    bool ButtonChecks_Abilities()
    {
        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return false; }
        if (Movement.Instance.GetMovementState() == MovementStates.Ability) { return false; }

        if (Movement.Instance.isJumping) { return false; }
        if (Movement.Instance.isGrapplingHooking) { return false; }
        if (Movement.Instance.isDashing) { return false; }
        if (Movement.Instance.isSwiftSwim) { return false; }
        if (Movement.Instance.isAscending) { return false; }
        if (Movement.Instance.isDescending) { return false; }
        if (MapManager.Instance.introSequence) { return false; }

        return true;
    }


    //--------------------


    public void Key_Quit()
    {
        if (!ButtonChecks_Other()) { return; }

        RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement = PauseMenuManager.Instance.pauseMenu_StartButton;
        PlayerManager.Instance.pauseGame = true;
        PauseMenuManager.Instance.OpenPauseMenu();

        Action_PauseMenuIsPressed?.Invoke();
    }
    
    //--------------------


    //Tutorial
    void Run_Movement_Tutorial(ref bool button_isPressed)
    {
        if (Tutorial.Instance.state_Movement)
        {
            button_isPressed = true;
            Action_WalkButton_isPressed?.Invoke();

            Tutorial.Instance.Tutorial_Movement(false);
        }
    }
    void Run_CameraRotation_Tutorial(bool cameraX)
    {
        if (Tutorial.Instance.state_CameraRotation)
        {
            MapManager.Instance.cameraRotated++;
            
            if (cameraX)
                CameraController.Instance.RotateCameraX();
            else
                CameraController.Instance.RotateCameraY();

            Tutorial.Instance.Tutorial_CameraRotation(false);
        }
    }
    void Run_Respawn_Tutorial()
    {
        if (Tutorial.Instance.state_Respawn)
        {
            respawn_isPressed = true;
            Action_RespawnHold?.Invoke();
        }
    }

    void Run_FreeCam_1_Tutorial_End()
    {
        if (Tutorial.Instance.state_FreeCam_1 && freeCam_isPressed)
        {
            Tutorial.Instance.Tutorial_FreeCam_1(false);
        }
    }
    void Run_FreeCam_2_Tutorial_End()
    {
        if (Tutorial.Instance.state_FreeCam_2 && freeCam_isPressed)
        {
            Tutorial.Instance.Tutorial_FreeCam_2(false);
        }
    }


    //Ability
    bool Run_AbilityDisplayExit()
    {
        if (PopUpManager.Instance.ability_CanBeClosed)
        {
            PopUpManager.Instance.HideAbilityPopup();

            return false;
        }

        return true;
    }
}
