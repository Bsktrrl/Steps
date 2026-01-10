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

    PauseMenuManager pauseMenuManager;



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


    //--------------------


    void OnForward_Down()
    {
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
        forward_isPressed = false;
        forward_isHold = false;

        Action_WalkButton_isReleased?.Invoke();
    }
    void OnBackward_Down()
    {
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
        back_isPressed = false;
        back_isHold = false;

        Action_WalkButton_isReleased?.Invoke();
    }
    void OnLeft_Down()
    {
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
        left_isPressed = false;
        left_isHold = false;

        Action_WalkButton_isReleased?.Invoke();
    }
    void OnRight_Down()
    {
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
        right_isPressed = false;
        right_isHold = false;

        Action_WalkButton_isReleased?.Invoke();
    }

    void OnAbilityUp_Down()
    {
        if (!ButtonChecks_Movement() || Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        up_isPressed = true;
        Action_Ascend_isPressed?.Invoke();
    }
    void OnAbilityUp_Up()
    {
        if (!ButtonChecks_Movement()) { return; }

        up_isPressed = false;
    }
    void OnAbilityDown_Down()
    {
        if (!ButtonChecks_Movement() || Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }
        
        down_isPressed = true;
        Action_Descend_isPressed?.Invoke();

        //Player_Interact.Instance.InteractWithObject();
    }
    void OnAbilityDown_Up()
    {
        if (!ButtonChecks_Movement()) { return; }

        down_isPressed = false;

        //Player_Interact.Instance.InteractWithObject();
    }

    void OnDialogueSkip_Pressed()
    {
        if (!PlayerManager.Instance.npcInteraction) { return; }

        Action_dialogueButton_isPressed?.Invoke();
    }
    void OnDialogueNext_Pressed()
    {
        if (!PlayerManager.Instance.npcInteraction) { return; }

        Action_dialogueNextButton_isPressed?.Invoke();
    }
    void OnInteractButton_Pressed()
    {
        if (!ButtonChecks_Movement()) { return; }
        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return; }

        Action_InteractButton_isPressed?.Invoke();
    }



    //--------------------


    void OnAbilityLeft()
    {
        if (!ButtonChecks_Other()) { return; }
        if (Movement.Instance.movementStates == MovementStates.Falling) { return; }

        Player_CeilingGrab.Instance.CeilingGrab();

        Action_CeilingGrab_isPressed?.Invoke();
    }

    void OnAbilityRight_DownPress()
    {
        if (!ButtonChecks_Other() || Player_CeilingGrab.Instance.isCeilingGrabbing) return;
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.GrapplingHook && !PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook) return;
        
        grapplingHook_isPressed = true;

        Movement.Instance.Action_isGrapplingHooking_Invoke();

        Player_Animations.Instance.Trigger_GrapplingHookAnimation();
    }
    void OnAbilityRight_RelesePress()
    {
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.GrapplingHook && !PlayerStats.Instance.stats.abilitiesGot_Permanent.GrapplingHook) return;

        grapplingHook_isPressed = false;

        if (!ButtonChecks_Other_MinusGrapplingHook()) { return; }

        Movement.Instance.UpdateGrapplingHookMovement_Release();

        Movement.Instance.Action_isGrapplingHooking_Finished_Invoke();
        //Action_GrapplingHook_isPressed?.Invoke();
    }


    //--------------------


    void OnCameraRotateX()
    {
        Run_CameraRotation_Tutorial(true);

        if (!ButtonChecks_Other()) { return; }
        if (Movement.Instance.isUpdatingDarkenBlocks) { return; }

        MapManager.Instance.cameraRotated++;
        CameraController.Instance.RotateCameraX();
    }
    void OnCameraRotateY()
    {
        Run_CameraRotation_Tutorial(false);

        if (!ButtonChecks_Other()) { return; }
        if (Movement.Instance.isUpdatingDarkenBlocks) { return; }

        MapManager.Instance.cameraRotated++;
        CameraController.Instance.RotateCameraY();
    }


    //--------------------


    void OnRespawn_In()
    {
        Run_Respawn_Tutorial();

        if (!ButtonChecks_Other()) { return; }

        respawn_isPressed = true;
        Action_RespawnHold?.Invoke();
    }
    void OnRespawn_Out()
    {
        respawn_isPressed = false;
        Action_RespawnCanceled?.Invoke();
    }


    //--------------------


    void OnQuit()
    {
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
}
