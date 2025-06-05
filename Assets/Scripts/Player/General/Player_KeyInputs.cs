using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_KeyInputs : Singleton<Player_KeyInputs>
{
    bool KeyInputsChecks()
    {
        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return false; }

        if (PlayerManager.Instance.pauseGame) { return false; }
        //if (PlayerManager.Instance.isTransportingPlayer) { return false; }
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
            //lastMovementButtonPressed = ButtonsToPress.W;
            //StartCeilingGrabMovement(Vector3.forward);
        }
        else if (Player_LadderMovement.Instance.CheckLaddersToEnter_Up(Movement.Instance.UpdatedDir(Vector3.forward)))
        {
            StartCoroutine(Player_LadderMovement.Instance.PerformLadderMovement_Up(Movement.Instance.UpdatedDir(Vector3.forward), Player_LadderMovement.Instance.GetLadderExitPart_Up(Movement.Instance.UpdatedDir(Vector3.forward))));
        }
        else if (Player_LadderMovement.Instance.CheckLaddersToEnter_Down(Movement.Instance.UpdatedDir(Vector3.forward)))
        {
            StartCoroutine(Player_LadderMovement.Instance.PerformLadderMovement_Down(Movement.Instance.UpdatedDir(Vector3.forward), Player_LadderMovement.Instance.GetLadderExitPart_Down(Movement.Instance.UpdatedDir(Vector3.forward))));
        }
        else
        {
            //PrepareMovement(ButtonsToPress.W, PlayerManager.Instance.canMove_Forward, PlayerManager.Instance.block_Vertical_InFront, 0);
        }
    }
    public void Key_MoveBackward()
    {
        if (!KeyInputsChecks()) { return; }

        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            //lastMovementButtonPressed = ButtonsToPress.S;
            //StartCeilingGrabMovement(Vector3.back);
        }
        else if (Player_LadderMovement.Instance.CheckLaddersToEnter_Up(Movement.Instance.UpdatedDir(Vector3.back)))
        {
            StartCoroutine(Player_LadderMovement.Instance.PerformLadderMovement_Up(Movement.Instance.UpdatedDir(Vector3.back), Player_LadderMovement.Instance.GetLadderExitPart_Up(Movement.Instance.UpdatedDir(Vector3.back))));
        }
        else if (Player_LadderMovement.Instance.CheckLaddersToEnter_Down(Movement.Instance.UpdatedDir(Vector3.back)))
        {
            StartCoroutine(Player_LadderMovement.Instance.PerformLadderMovement_Down(Movement.Instance.UpdatedDir(Vector3.back), Player_LadderMovement.Instance.GetLadderExitPart_Down(Movement.Instance.UpdatedDir(Vector3.back))));
        }
        else
        {
            //PrepareMovement(ButtonsToPress.S, PlayerManager.Instance.canMove_Back, PlayerManager.Instance.block_Vertical_InBack, 180);
        }
    }
    public void Key_MoveLeft()
    {
        if (!KeyInputsChecks()) { return; }

        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            //lastMovementButtonPressed = ButtonsToPress.A;
            //StartCeilingGrabMovement(Vector3.left);
        }
        else if (Player_LadderMovement.Instance.CheckLaddersToEnter_Up(Movement.Instance.UpdatedDir(Vector3.left)))
        {
            StartCoroutine(Player_LadderMovement.Instance.PerformLadderMovement_Up(Movement.Instance.UpdatedDir(Vector3.left), Player_LadderMovement.Instance.GetLadderExitPart_Up(Movement.Instance.UpdatedDir(Vector3.left))));
        }
        else if (Player_LadderMovement.Instance.CheckLaddersToEnter_Down(Movement.Instance.UpdatedDir(Vector3.left)))
        {
            StartCoroutine(Player_LadderMovement.Instance.PerformLadderMovement_Down(Movement.Instance.UpdatedDir(Vector3.left), Player_LadderMovement.Instance.GetLadderExitPart_Down(Movement.Instance.UpdatedDir(Vector3.left))));
        }
        else
        {
            //PrepareMovement(ButtonsToPress.A, PlayerManager.Instance.canMove_Left, PlayerManager.Instance.block_Vertical_ToTheLeft, -90);
        }
    }
    public void Key_MoveRight()
    {
        if (!KeyInputsChecks()) { return; }

        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            //lastMovementButtonPressed = ButtonsToPress.D;
            //StartCeilingGrabMovement(Vector3.right);
        }
        else if (Player_LadderMovement.Instance.CheckLaddersToEnter_Up(Movement.Instance.UpdatedDir(Vector3.right)))
        {
            StartCoroutine(Player_LadderMovement.Instance.PerformLadderMovement_Up(Movement.Instance.UpdatedDir(Vector3.right), Player_LadderMovement.Instance.GetLadderExitPart_Up(Movement.Instance.UpdatedDir(Vector3.right))));
        }
        else if (Player_LadderMovement.Instance.CheckLaddersToEnter_Down(Movement.Instance.UpdatedDir(Vector3.right)))
        {
            StartCoroutine(Player_LadderMovement.Instance.PerformLadderMovement_Down(Movement.Instance.UpdatedDir(Vector3.right), Player_LadderMovement.Instance.GetLadderExitPart_Down(Movement.Instance.UpdatedDir(Vector3.right))));
        }
        else
        {
            //PrepareMovement(ButtonsToPress.D, PlayerManager.Instance.canMove_Right, PlayerManager.Instance.block_Vertical_ToTheRight, 90);
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
        if (PlayerManager.Instance.pauseGame) { return; }
        if (CameraController.Instance.isRotating) { return; }
        if (Player_Interact.Instance.isInteracting) { return; }
        if (Player_GraplingHook.Instance.isGrapplingHooking) { return; }

        PlayerStats.Instance.RespawnPlayer();

        Movement.Instance.Action_StepTaken_Invoke();
    }
    public void Key_Quit()
    {
        if (!KeyInputsChecks()) { return; }

        RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement = PauseMenuManager.Instance.pauseMenu_StartButton;
        PauseMenuManager.Instance.OpenPauseMenu();
        PlayerManager.Instance.pauseGame = true;
    }
}
