
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DarkenBlock : Singleton<Player_DarkenBlock>
{
    bool block_hasBeenDarkened;


    //--------------------


    private void Start()
    {
        UpdateDarkenBlocks();
    }
    private void Update()
    {
        UpdateDarkenBlockWhenButtonIsPressed();
    }


    //--------------------


    public void UpdateDarkenBlockWhenButtonIsPressed()
    {
        if (Player_Movement.Instance.movementStates == MovementStates.Moving)
        {
            if (block_hasBeenDarkened)
            {
                block_hasBeenDarkened = false;
            }
            
            return; 
        }

        if (PlayerManager.Instance.forward_isPressed
            || PlayerManager.Instance.back_isPressed
            || PlayerManager.Instance.left_isPressed
            || PlayerManager.Instance.right_isPressed)
        {
            
        }
        else
        {
            if (!block_hasBeenDarkened)
            {
                UpdateDarkenBlocks();
            }
        }

        if (!PlayerManager.Instance.forward_isPressed
            && !PlayerManager.Instance.back_isPressed
            && !PlayerManager.Instance.left_isPressed
            && !PlayerManager.Instance.right_isPressed)
        {
            if (!block_hasBeenDarkened)
            {
                UpdateDarkenBlocks();
            }
        }
    }

    public void UpdateDarkenBlocks()
    {
        if (Player_Movement.Instance.movementStates == MovementStates.Moving) { return; }

        //Darken block in the front of player
        if (PlayerManager.Instance.block_Horizontal_InFront != null)
        {
            if (PlayerManager.Instance.block_Horizontal_InFront.block == null && PlayerManager.Instance.block_Vertical_InFront.block == null) { }
            else
            {
                if (PlayerManager.Instance.block_Vertical_InFront.block)
                {
                    if (PlayerManager.Instance.canMove_Forward)
                    {
                        if (PlayerManager.Instance.block_Vertical_InFront.block.GetComponent<BlockInfo>())
                            PlayerManager.Instance.block_Vertical_InFront.block.GetComponent<BlockInfo>().DarkenColors();
                    }
                    else
                    {
                        if (PlayerManager.Instance.block_Vertical_InFront.block.GetComponent<BlockInfo>())
                            PlayerManager.Instance.block_Vertical_InFront.block.GetComponent<BlockInfo>().ResetDarkenColor();
                    }
                }
            }
        }


        //Darken block in the back of player
        if (PlayerManager.Instance.block_Horizontal_InBack != null)
        {
            if (PlayerManager.Instance.block_Horizontal_InBack.block == null && PlayerManager.Instance.block_Vertical_InBack.block == null) { }
            else
            {
                if (PlayerManager.Instance.block_Vertical_InBack.block)
                {
                    if (PlayerManager.Instance.canMove_Back)
                    {
                        if (PlayerManager.Instance.block_Vertical_InBack.block.GetComponent<BlockInfo>())
                            PlayerManager.Instance.block_Vertical_InBack.block.GetComponent<BlockInfo>().DarkenColors();
                    }
                    else
                    {
                        if (PlayerManager.Instance.block_Vertical_InBack.block.GetComponent<BlockInfo>())
                            PlayerManager.Instance.block_Vertical_InBack.block.GetComponent<BlockInfo>().ResetDarkenColor();
                    }
                }
            }
        }


        //Darken block to the left of player
        if (PlayerManager.Instance.block_Horizontal_ToTheLeft != null)
        {
            if (PlayerManager.Instance.block_Horizontal_ToTheLeft.block == null && PlayerManager.Instance.block_Vertical_ToTheLeft.block == null) { }
            else
            {
                if (PlayerManager.Instance.block_Vertical_ToTheLeft.block)
                {
                    if (PlayerManager.Instance.canMove_Left)
                    {
                        if (PlayerManager.Instance.block_Vertical_ToTheLeft.block.GetComponent<BlockInfo>())
                            PlayerManager.Instance.block_Vertical_ToTheLeft.block.GetComponent<BlockInfo>().DarkenColors();
                    }
                    else
                    {
                        if (PlayerManager.Instance.block_Vertical_ToTheLeft.block.GetComponent<BlockInfo>())
                            PlayerManager.Instance.block_Vertical_ToTheLeft.block.GetComponent<BlockInfo>().ResetDarkenColor();
                    }
                }
            }
        }


        //Darken block to the right of player
        if (PlayerManager.Instance.block_Horizontal_ToTheRight != null)
        {
            if (PlayerManager.Instance.block_Horizontal_ToTheRight.block == null && PlayerManager.Instance.block_Vertical_ToTheRight.block == null) { }
            else
            {
                if (PlayerManager.Instance.block_Vertical_ToTheRight.block)
                {
                    if (PlayerManager.Instance.canMove_Right)
                    {
                        if (PlayerManager.Instance.block_Vertical_ToTheRight.block.GetComponent<BlockInfo>())
                            PlayerManager.Instance.block_Vertical_ToTheRight.block.GetComponent<BlockInfo>().DarkenColors();
                    }
                    else
                    {
                        if (PlayerManager.Instance.block_Vertical_ToTheRight.block.GetComponent<BlockInfo>())
                            PlayerManager.Instance.block_Vertical_ToTheRight.block.GetComponent<BlockInfo>().ResetDarkenColor();
                    }
                }
            }
        }
        

        //Lighten Block player is standing on
        if (PlayerManager.Instance.block_StandingOn_Current.block == null) { }
        else
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block)
            {
                if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>())
                    PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().ResetDarkenColor();
            }
        }

        block_hasBeenDarkened = true;
    }
}
