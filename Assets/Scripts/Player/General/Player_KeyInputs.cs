using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_KeyInputs : Singleton<Player_KeyInputs>
{
    [Header("Input System")]
    public PlayerControls playerControls;

    [Header("KeyPresses")]
    public bool forward_isPressed = false;
    public bool back_isPressed = false;
    public bool left_isPressed = false;
    public bool right_isPressed = false;

    public bool cameraX_isPressed = false;
    public bool cameraY_isPressed = false;


    //--------------------


    private void Start()
    {
        playerControls = new PlayerControls();
    }


    //--------------------


    void OnForward_Down()
    {
        if (!CameraController.Instance.isRotating && !PlayerManager.Instance.pauseGame)
            forward_isPressed = true;
    }
    void OnForward_Up()
    {
        forward_isPressed = false;
    }
    void OnBackward_Down()
    {
        if (!CameraController.Instance.isRotating && !PlayerManager.Instance.pauseGame)
            back_isPressed = true;
    }
    void OnBackward_Up()
    {
        back_isPressed = false;
    }
    void OnLeft_Down()
    {
        if (!CameraController.Instance.isRotating && !PlayerManager.Instance.pauseGame)
            left_isPressed = true;
    }
    void OnLeft_Up()
    {
        left_isPressed = false;
    }
    void OnRight_Down()
    {
        if (!CameraController.Instance.isRotating && !PlayerManager.Instance.pauseGame)
            right_isPressed = true;
    }
    void OnRight_Up()
    {
        right_isPressed = false;
    }

    void OnAbilityUp()
    {
        Player_Ascend.Instance.RunAscend();
    }
    void OnAbilityDown()
    {
        Player_Interact.Instance.InteractWithObject();
        Player_Descend.Instance.RunDescend();
    }
    void OnAbilityLeft()
    {
        Player_CeilingGrab.Instance.CeilingGrab();
    }
    void OnAbilityRight_DownPress()
    {
        Player_GraplingHook.Instance.StartGrappling();
    }
    void OnAbilityRight_RelesePress()
    {
        Player_GraplingHook.Instance.StopGrappling();
    }

    void OnCameraRotateX()
    {
        CameraController.Instance.RotateCameraX();
    }
    void OnCameraRotateY()
    {
        CameraController.Instance.RotateCameraY();
    }

    void OnRespawn()
    {
        Player_KeyInputs.Instance.Key_Respawn();
    }
    void OnQuit()
    {
        Player_KeyInputs.Instance.Key_Quit();
    }


    //--------------------


    bool KeyInputsChecks()
    {
        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return false; }

        if (PlayerManager.Instance.pauseGame) { return false; }
        if (CameraController.Instance.isRotating) { return false; }
        if (Player_Interact.Instance.isInteracting) { return false; }
        if (Player_GraplingHook.Instance.isGrapplingHooking) { return false; }

        return true;
    }

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
        if (!KeyInputsChecks()) { return; }

        RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement = PauseMenuManager.Instance.pauseMenu_StartButton;
        PauseMenuManager.Instance.OpenPauseMenu();
        PlayerManager.Instance.pauseGame = true;
    }
}
