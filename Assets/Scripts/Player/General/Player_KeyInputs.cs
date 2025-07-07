using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_KeyInputs : Singleton<Player_KeyInputs>
{
    public static event Action Action_dialogueButton_isPressed;

    [Header("Input System")]
    public PlayerControls playerControls;
    MapManager mapManager;

    [Header("KeyPresses")]
    public bool forward_isPressed = false;
    public bool back_isPressed = false;
    public bool left_isPressed = false;
    public bool right_isPressed = false;

    public bool up_isPressed = false;
    public bool down_isPressed = false;

    public bool grapplingHook_isPressed = false;

    public bool cameraX_isPressed = false;
    public bool cameraY_isPressed = false;

    public bool dialogueButton_isPressed = false;


    //--------------------


    private void Start()
    {
        playerControls = new PlayerControls();
        mapManager = FindObjectOfType<MapManager>();
    }


    //--------------------


    void OnForward_Down()
    {
        if (!ButtonChecks_Movement()) { return; }

        forward_isPressed = true;
    }
    void OnForward_Up()
    {
        forward_isPressed = false;
    }
    void OnBackward_Down()
    {
        if (!ButtonChecks_Movement()) { return; }

        back_isPressed = true;
    }
    void OnBackward_Up()
    {
        back_isPressed = false;
    }
    void OnLeft_Down()
    {
        if (!ButtonChecks_Movement()) { return; }

        left_isPressed = true;
    }
    void OnLeft_Up()
    {
        left_isPressed = false;
    }
    void OnRight_Down()
    {
        if (!ButtonChecks_Movement()) { return; }

        right_isPressed = true;
    }
    void OnRight_Up()
    {
        right_isPressed = false;
    }
    void OnAbilityUp_Down()
    {
        if (!ButtonChecks_Movement() || Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        up_isPressed = true;
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
        if (!PlayerManager.Instance.pauseGame) { return; }

        dialogueButton_isPressed = true;
        Action_dialogueButton_isPressed?.Invoke();
    }


    //--------------------


    void OnAbilityLeft()
    {
        if (!ButtonChecks_Other()) { return; }

        Player_CeilingGrab.Instance.CeilingGrab();
    }

    void OnAbilityRight_DownPress()
    {
        if (!ButtonChecks_Other() || Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        grapplingHook_isPressed = true;
    }
    void OnAbilityRight_RelesePress()
    {
        if (!ButtonChecks_Other()) { return; }

        grapplingHook_isPressed = false;
        Movement.Instance.UpdateGrapplingHookMovement_Release();
    }


    //--------------------


    void OnCameraRotateX()
    {
        if (!ButtonChecks_Other()) { return; }
        if (Movement.Instance.isUpdatingDarkenBlocks) { return; }

        MapManager.Instance.cameraRotated++;
        CameraController.Instance.RotateCameraX();
    }
    void OnCameraRotateY()
    {
        if (!ButtonChecks_Other()) { return; }
        if (Movement.Instance.isUpdatingDarkenBlocks) { return; }

        MapManager.Instance.cameraRotated++;
        CameraController.Instance.RotateCameraY();
    }


    //--------------------


    void OnRespawn()
    {
        if (!ButtonChecks_Other()) { return; }

        Key_Respawn();
    }
    void OnQuit()
    {
        if (!ButtonChecks_Other()) { return; }

        Key_Quit();
    }

    bool ButtonChecks_Movement()
    {
        if (Movement.Instance.GetMovementState() == MovementStates.Ability) { return false; }

        if (PlayerManager.Instance.pauseGame) { return false; }
        if (CameraController.Instance.isRotating) { return false; }
        if (Player_Interact.Instance.isInteracting) { return false; }
        if (Player_CeilingGrab.Instance.isCeilingRotation) { return false; }

        return true;
    }
    bool ButtonChecks_Other()
    {
        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return false; }
        if (Movement.Instance.GetMovementState() == MovementStates.Ability) { return false; }

        if (Player_CeilingGrab.Instance.isCeilingRotation) { return false; }
        if (PlayerManager.Instance.pauseGame) { return false; }
        if (CameraController.Instance.isRotating) { return false; }
        if (Player_Interact.Instance.isInteracting) { return false; }

        return true;
    }


    //--------------------


    public void Key_Respawn()
    {
        if (PlayerManager.Instance.pauseGame) { return; }
        if (CameraController.Instance.isRotating) { return; }
        if (Player_Interact.Instance.isInteracting) { return; }
        if (Player_GraplingHook.Instance.isGrapplingHooking) { return; }

        Movement.Instance.RespawnPlayer();
    }
    public void Key_Quit()
    {
        if (!ButtonChecks_Other()) { return; }

        RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement = PauseMenuManager.Instance.pauseMenu_StartButton;
        PauseMenuManager.Instance.OpenPauseMenu();
        PlayerManager.Instance.pauseGame = true;
    }
}
