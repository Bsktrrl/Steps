using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{

    public MovementStates movementStates;


    //--------------------


    private void Update()
    {
        KeyInputs();
    }


    //--------------------


    void KeyInputs()
    {
        if (movementStates == MovementStates.Moving) { return; }
        if (MainManager.Instance.pauseGame) { return; }

        if (Input.GetKeyDown(KeyCode.W))
        {
            //if (PlayerStats.Instance.stats.steps_Current <= 0)
            //{
            //    movementStates = MovementStates.Moving;
            //    PlayerStepCost.Instance.RefillSteps();

            //    return;
            //}

            if (MainManager.Instance.canMove_Forward)
            {
                MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.forward * 1, Quaternion.identity);
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //if (PlayerStats.Instance.stats.steps_Current <= 0)
            //{
            //    movementStates = MovementStates.Moving;
            //    PlayerStepCost.Instance.RefillSteps();

            //    return;
            //}

            if (MainManager.Instance.canMove_Back)
            {
                MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.back * 1, Quaternion.identity);
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            //if (PlayerStats.Instance.stats.steps_Current <= 0)
            //{
            //    movementStates = MovementStates.Moving;
            //    PlayerStepCost.Instance.RefillSteps();

            //    return;
            //}

            if (MainManager.Instance.canMove_Left)
            {
                MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.left * 1, Quaternion.identity);
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            //if (PlayerStats.Instance.stats.steps_Current <= 0)
            //{
            //    movementStates = MovementStates.Moving;
            //    PlayerStepCost.Instance.RefillSteps();

            //    return;
            //}

            if (MainManager.Instance.canMove_Right)
            {
                MainManager.Instance.player.transform.SetPositionAndRotation(MainManager.Instance.player.transform.position + Vector3.right * 1, Quaternion.identity);
            }
        }
    }
}

public enum MovementStates
{
    Still,
    Moving
}