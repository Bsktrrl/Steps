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
        if (!ButtonChecks()) { return; }

        forward_isPressed = true;
    }
    void OnForward_Up()
    {
        forward_isPressed = false;
    }
    void OnBackward_Down()
    {
        if (!ButtonChecks()) { return; }

        back_isPressed = true;
    }
    void OnBackward_Up()
    {
        back_isPressed = false;
    }
    void OnLeft_Down()
    {
        if (!ButtonChecks()) { return; }

        left_isPressed = true;
    }
    void OnLeft_Up()
    {
        left_isPressed = false;
    }
    void OnRight_Down()
    {
        if (!ButtonChecks()) { return; }

        right_isPressed = true;
    }
    void OnRight_Up()
    {
        right_isPressed = false;
    }


    //--------------------


    void OnAbilityUp()
    {
        if (!ButtonChecks()) { return; }

        Movement.Instance.RunAscend();
    }
    void OnAbilityDown()
    {
        if (!ButtonChecks()) { return; }

        //Player_Interact.Instance.InteractWithObject();
        Player_Descend.Instance.RunDescend();
    }
    void OnAbilityLeft()
    {
        if (!ButtonChecks()) { return; }

        Player_CeilingGrab.Instance.CeilingGrab();
    }
    void OnAbilityRight_DownPress()
    {
        if (!ButtonChecks()) { return; }

        Player_GraplingHook.Instance.StartGrappling();
    }
    void OnAbilityRight_RelesePress()
    {
        if (!ButtonChecks()) { return; }

        Player_GraplingHook.Instance.StopGrappling();
    }


    //--------------------


    void OnCameraRotateX()
    {
        if (!ButtonChecks()) { return; }

        CameraController.Instance.RotateCameraX();
    }
    void OnCameraRotateY()
    {
        if (!ButtonChecks()) { return; }

        CameraController.Instance.RotateCameraY();
    }


    //--------------------


    void OnRespawn()
    {
        if (!ButtonChecks()) { return; }

        Player_KeyInputs.Instance.Key_Respawn();
    }
    void OnQuit()
    {
        if (!ButtonChecks()) { return; }

        Player_KeyInputs.Instance.Key_Quit();
    }

    bool ButtonChecks()
    {
        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return false; }
        if (Movement.Instance.GetMovementState() == MovementStates.Ability) { return false; }

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
        if (!ButtonChecks()) { return; }

        RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement = PauseMenuManager.Instance.pauseMenu_StartButton;
        PauseMenuManager.Instance.OpenPauseMenu();
        PlayerManager.Instance.pauseGame = true;
    }
}
