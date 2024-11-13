
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DarkenBlock : MonoBehaviour
{

    private void Start()
    {
        UpdateDarkenBlocks();
    }
    private void Update()
    {
        UpdateDarkenBlockWhenButtonIsPressed();
    }


    //--------------------


    void UpdateDarkenBlockWhenButtonIsPressed()
    {
        if (Player_Movement.Instance.movementStates == MovementStates.Moving) { return; }

        //If a key is held down, don't show the new darkening blocks
        if (Input.GetKey(KeyCode.W)
            || Input.GetKey(KeyCode.S)
            || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.D)) { }
        else
        {
            UpdateDarkenBlocks();
        }

        //When a key is pressed up
        if (Input.GetKeyUp(KeyCode.W)
            || Input.GetKeyUp(KeyCode.S)
            || Input.GetKeyUp(KeyCode.A)
            || Input.GetKeyUp(KeyCode.D))
        {
            UpdateDarkenBlocks();
        }
    }

    void UpdateDarkenBlocks()
    {
        if (Player_Movement.Instance.movementStates == MovementStates.Moving) { return; }

        //Darken block in the front of player
        if (MainManager.Instance.block_Horizontal_InFront.block == null && MainManager.Instance.block_Vertical_InFront.block == null) { }
        else
        {
            if (MainManager.Instance.block_Vertical_InFront.block)
            {
                if (MainManager.Instance.canMove_Forward)
                {
                    if (MainManager.Instance.block_Vertical_InFront.block.GetComponent<BlockInfo>())
                        MainManager.Instance.block_Vertical_InFront.block.GetComponent<BlockInfo>().DarkenColors();
                }
                else
                {
                    if (MainManager.Instance.block_Vertical_InFront.block.GetComponent<BlockInfo>())
                        MainManager.Instance.block_Vertical_InFront.block.GetComponent<BlockInfo>().ResetColor();
                }
            }
        }

        //Darken block in the back of player
        if (MainManager.Instance.block_Horizontal_InBack.block == null && MainManager.Instance.block_Vertical_InBack.block == null) { }
        else
        {
            if (MainManager.Instance.block_Vertical_InBack.block)
            {
                if (MainManager.Instance.canMove_Back)
                {
                    if (MainManager.Instance.block_Vertical_InBack.block.GetComponent<BlockInfo>())
                        MainManager.Instance.block_Vertical_InBack.block.GetComponent<BlockInfo>().DarkenColors();
                }
                else
                {
                    if (MainManager.Instance.block_Vertical_InBack.block.GetComponent<BlockInfo>())
                        MainManager.Instance.block_Vertical_InBack.block.GetComponent<BlockInfo>().ResetColor();
                }
            }
        }

        //Darken block to the left of player
        if (MainManager.Instance.block_Horizontal_ToTheLeft.block == null && MainManager.Instance.block_Vertical_ToTheLeft.block == null) { }
        else
        {
            if (MainManager.Instance.block_Vertical_ToTheLeft.block)
            {
                if (MainManager.Instance.canMove_Left)
                {
                    if (MainManager.Instance.block_Vertical_ToTheLeft.block.GetComponent<BlockInfo>())
                        MainManager.Instance.block_Vertical_ToTheLeft.block.GetComponent<BlockInfo>().DarkenColors();
                }
                else
                {
                    if (MainManager.Instance.block_Vertical_ToTheLeft.block.GetComponent<BlockInfo>())
                        MainManager.Instance.block_Vertical_ToTheLeft.block.GetComponent<BlockInfo>().ResetColor();
                }
            }
        }

        //Darken block to the right of player
        if (MainManager.Instance.block_Horizontal_ToTheRight.block == null && MainManager.Instance.block_Vertical_ToTheRight.block == null) { }
        else
        {
            if (MainManager.Instance.block_Vertical_ToTheRight.block)
            {
                if (MainManager.Instance.canMove_Right)
                {
                    if (MainManager.Instance.block_Vertical_ToTheRight.block.GetComponent<BlockInfo>())
                        MainManager.Instance.block_Vertical_ToTheRight.block.GetComponent<BlockInfo>().DarkenColors();
                }
                else
                {
                    if (MainManager.Instance.block_Vertical_ToTheRight.block.GetComponent<BlockInfo>())
                        MainManager.Instance.block_Vertical_ToTheRight.block.GetComponent<BlockInfo>().ResetColor();
                }
            }
        }

        //Lighten Block player is standing on
        if (MainManager.Instance.block_StandingOn_Current.block == null) { }
        else
        {
            if (MainManager.Instance.block_StandingOn_Current.block)
            {
                if (MainManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>())
                    MainManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().ResetColor();
            }
        }
    }
}
