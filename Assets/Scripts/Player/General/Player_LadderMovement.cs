using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_LadderMovement : Singleton<Player_LadderMovement>
{
    public static event Action Action_resetBlockColor;

    RaycastHit ladderHit;

    public Vector3 ladderEndPos_Up;
    public Vector3 ladderEndPos_Down;
    public bool isMovingOnLadder_Up;
    public bool isMovingOnLadder_Down;

    Vector3 ladderClimbPos_Start;
    Vector3 ladderClimbPos_End;
    public int ladderPartsToClimb;
    public Quaternion ladderToEnterRot;


    //--------------------


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
        if (Physics.Raycast(transform.position, dir, out ladderHit, 1, MapManager.Instance.pickup_LayerMask))
        {
            if (ladderHit.transform.gameObject.GetComponent<Block_Ladder>())
            {
                ladderHit.transform.gameObject.GetComponent<Block_Ladder>().DarkenExitBlock_Up(dir);
            }
        }

        //Check from the top and down
        if (Physics.Raycast(transform.position + (dir * 0.65f), Vector3.down, out ladderHit, 1, MapManager.Instance.pickup_LayerMask))
        {
            if (ladderHit.transform.gameObject.GetComponent<Block_Ladder>())
            {
                ladderHit.transform.gameObject.GetComponent<Block_Ladder>().DarkenExitBlock_Down();
            }
        }
    }

    public bool CheckLaddersToEnter_Up(Vector3 dir)
    {
        //Check from the bottom and up
        if (Physics.Raycast(transform.position, dir, out ladderHit, 1, MapManager.Instance.pickup_LayerMask))
        {
            if (ladderHit.transform.gameObject.GetComponent<Block_Ladder>())
            {
                return true;
            }
        }

        //If no ladder is found
        return false;
    }
    public bool CheckLaddersToEnter_Down(Vector3 dir)
    {
        //Check from the top and down
        if (Physics.Raycast(transform.position + (dir * 0.65f), Vector3.down, out ladderHit, 1, MapManager.Instance.pickup_LayerMask))
        {
            if (ladderHit.transform.gameObject.GetComponent<Block_Ladder>())
            {
                return true;
            }
        }

        //If no ladder is found
        return false;
    }

    public GameObject GetLadderExitPart_Up(Vector3 dir)
    {
        //Check from the bottom and up
        if (Physics.Raycast(transform.position, dir, out ladderHit, 1, MapManager.Instance.pickup_LayerMask))
        {
            if (ladderHit.transform.gameObject.GetComponent<Block_Ladder>())
            {
                ladderToEnterRot = ladderHit.transform.rotation;
                return ladderHit.transform.gameObject.GetComponent<Block_Ladder>().lastLadderPart_Up;
            }
        }

        return null;
    }
    public GameObject GetLadderExitPart_Down(Vector3 dir)
    {
        //Check from the top and down
        if (Physics.Raycast(transform.position + (dir * 0.65f), Vector3.down, out ladderHit, 1, MapManager.Instance.pickup_LayerMask))
        {
            if (ladderHit.transform.gameObject.GetComponent<Block_Ladder>())
            {
                ladderToEnterRot = ladderHit.transform.rotation;
                return ladderHit.transform.gameObject.GetComponent<Block_Ladder>().lastLadderPart_Down;
            }
        }

        return null;
    }

    public IEnumerator PerformLadderMovement_Up(Vector3 dir, GameObject targetPosObj)
    {
        Action_ResetBlockColorInvoke();

        #region Setup Movement Parameters

        isMovingOnLadder_Up = true;
        ladderClimbPos_Start = transform.position;

        Movement.Instance.SetMovementState(MovementStates.Moving);
        PlayerManager.Instance.PauseGame();

        Vector3 startPosition;
        Vector3 endPosition;
        float ladderClimbDuration = 0;
        float elapsedTime = 0;

        #endregion

        Movement.Instance.RotatePlayerBody(int.MinValue);

        #region Move To Top LadderPart

        startPosition = transform.position;
        endPosition = targetPosObj.transform.position + (Vector3.up * Movement.Instance.heightOverBlock);

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
        if (Physics.Raycast(transform.position + dir, Vector3.down, out ladderHit, 1, MapManager.Instance.pickup_LayerMask))
        {
            if (ladderHit.transform.gameObject.GetComponent<BlockInfo>())
            {
                endPosition = ladderHit.transform.gameObject.transform.position + (Vector3.up * Movement.Instance.heightOverBlock);
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

        isMovingOnLadder_Up = false;

        Movement.Instance.SetMovementState(MovementStates.Still);

        PlayerManager.Instance.UnpauseGame();

        FindLadderExitBlock();
        Action_ResetBlockColorInvoke();
        Movement.Instance.Action_StepTaken_Invoke();

        #endregion
    }
    public IEnumerator PerformLadderMovement_Down(Vector3 dir, GameObject targetPosObj)
    {
        Action_ResetBlockColorInvoke();

        #region Setup Movement Parameters

        isMovingOnLadder_Down = true;
        ladderClimbPos_Start = transform.position;

        Movement.Instance.SetMovementState(MovementStates.Moving);
        PlayerManager.Instance.PauseGame();

        Vector3 startPosition;
        Vector3 endPosition;
        float ladderClimbDuration = 0;
        float elapsedTime = 0;

        #endregion

        Movement.Instance.RotatePlayerBody(0);

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

        Movement.Instance.RotatePlayerBody(int.MinValue);

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

        Movement.Instance.RotatePlayerBody(0);

        #region Setup StopMovement Parameters

        isMovingOnLadder_Down = false;

        Movement.Instance.SetMovementState(MovementStates.Still);
        PlayerManager.Instance.PauseGame();

        FindLadderExitBlock();
        Action_ResetBlockColorInvoke();
        Movement.Instance.Action_StepTaken_Invoke();

        #endregion
    }

    public void Action_ResetBlockColorInvoke()
    {
        Action_resetBlockColor?.Invoke();
    }
}