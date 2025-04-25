
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DarkenBlock : Singleton<Player_DarkenBlock>
{
    public bool block_hasBeenDarkened;

    public bool canMove_Forward;
    public bool canMove_Back;
    public bool canMove_Left;
    public bool canMove_Right;

    public GameObject moveTarget_Forward;
    public GameObject moveTarget_Back;
    public GameObject moveTarget_Left;
    public GameObject moveTarget_Right;

    [Header("Other")]
    RaycastHit hit;

    bool movementButtonIsPressed;


    //--------------------


    private void Start()
    {
        block_hasBeenDarkened = false;

        SetMoveDirections();
    }
    private void Update()
    {
        RemoveDarkenBlockWhenKeyPressed();
        SetMoveDirections();
    }


    //--------------------


    void SetMoveDirections()
    {
        canMove_Forward = false;
        canMove_Back = false;
        canMove_Left = false;
        canMove_Right = false;

        if (moveTarget_Forward)
        {
            if (!moveTarget_Forward.GetComponent<BlockInfo>().blockIsDark)
                ResetTargetBlock(ref moveTarget_Forward);
        }
        if (moveTarget_Back)
        {
            if (!moveTarget_Back.GetComponent<BlockInfo>().blockIsDark)
                ResetTargetBlock(ref moveTarget_Back);
        }
        if (moveTarget_Left)
        {
            if (!moveTarget_Left.GetComponent<BlockInfo>().blockIsDark)
                ResetTargetBlock(ref moveTarget_Left);
        }
        if (moveTarget_Right)
        {
            if (!moveTarget_Right.GetComponent<BlockInfo>().blockIsDark)
                ResetTargetBlock(ref moveTarget_Right);
        }

        //ResetTargetBlock(ref moveTarget_Forward);
        //ResetTargetBlock(ref moveTarget_Back);
        //ResetTargetBlock(ref moveTarget_Left);
        //ResetTargetBlock(ref moveTarget_Right);

        if (Player_Movement.Instance.movementStates == MovementStates.Moving) { return; }
        if (movementButtonIsPressed) { return; }

        //Check if can Move and get MoveTarget
        canMove_Forward = CheckIfCanMove(ref moveTarget_Forward, Vector3.forward, PlayerManager.Instance.canMove_Forward);
        canMove_Back = CheckIfCanMove(ref moveTarget_Back, Vector3.back, PlayerManager.Instance.canMove_Back);
        canMove_Left = CheckIfCanMove(ref moveTarget_Left, Vector3.left, PlayerManager.Instance.canMove_Left);
        canMove_Right = CheckIfCanMove(ref moveTarget_Right, Vector3.right, PlayerManager.Instance.canMove_Right);
    }
    bool CheckIfCanMove(ref GameObject target, Vector3 dir, bool canMove)
    {
        if (!canMove)
        {
            ResetTargetBlock(ref target);
            target = null;
            return false;
        }

        ResetDarkenColorIfStepsIsGone(ref target);

        //If standing on a Stair or Slope
        if (PlayerManager.Instance.block_StandingOn_Current.blockType == BlockType.Stair || PlayerManager.Instance.block_StandingOn_Current.blockType == BlockType.Slope)
        {
            return RaycastFromTarget(ref target, Vector3.up + dir, 2);
        }

        //If standing on a Cube
        else
        {
            return RaycastFromTarget(ref target, dir, 1);
        }
    }
    bool RaycastFromTarget(ref GameObject target, Vector3 dir, float length)
    {
        //Raycast down from target
        if (Physics.Raycast(gameObject.transform.position + dir, Vector3.down, out hit, length))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                target = hit.transform.gameObject;

                //Darken color in target block
                if (target)
                {
                    if (target.GetComponent<BlockInfo>())
                    {
                        target.GetComponent<BlockInfo>().SetDarkenColors();
                    }
                }

                ResetDarkenColorIfStepsIsGone(ref target);

                return true;
            }
            else
            {
                ResetTargetBlock(ref target);
                target = null;
                return false;
            }
        }
        else
        {
            ResetTargetBlock(ref target);
            target = null;
            return false;
        }
    }
    void ResetDarkenColorIfStepsIsGone(ref GameObject target)
    {
        //Cannot Move if having 0 movement and targetBlock has a MovementCost
        if (PlayerStats.Instance.stats.steps_Current <= 0)
        {
            if (target)
            {
                if (target.GetComponent<BlockInfo>().movementCost > 0)
                {
                    target.GetComponent<BlockInfo>().ResetDarkenColor();
                }
            }
        }
    }
    void ResetTargetBlock(ref GameObject target)
    {
        //Reset Darken Color
        if (target)
        {
            target.GetComponent<BlockInfo>().ResetDarkenColor();
            target = null;
        }
    }


    //--------------------

    void RemoveDarkenBlockWhenKeyPressed()
    {
        if (PlayerManager.Instance.forward_isPressed
            || PlayerManager.Instance.back_isPressed
            || PlayerManager.Instance.left_isPressed
            || PlayerManager.Instance.right_isPressed)
        {
            if (Player_Movement.Instance.movementStates == MovementStates.Moving)
            {
                movementButtonIsPressed = true;
            }
        }

        if (!PlayerManager.Instance.forward_isPressed
            && !PlayerManager.Instance.back_isPressed
            && !PlayerManager.Instance.left_isPressed
            && !PlayerManager.Instance.right_isPressed)
        {
            if (Player_Movement.Instance.movementStates == MovementStates.Still)
            {
                movementButtonIsPressed = false;
            }
        }
    }
}
