using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Burning : Singleton<Player_Burning>
{
    public bool isBurning;
    public int flameableStepCounter;

    [SerializeField] GameObject flameEffectObject;

    bool firstTimeCheck;


    //--------------------


    private void OnEnable()
    {
        Movement.Action_isSwitchingBlocks += BecomeFlameable;
        Movement.Action_StepTaken += BecomeFlameable;
        Movement.Action_StepTaken += CheckFlameableCounter;
        PlayerStats.Action_RespawnPlayer += RemoveFlameable;
    }
    private void OnDisable()
    {
        Movement.Action_isSwitchingBlocks -= BecomeFlameable;
        Movement.Action_StepTaken -= BecomeFlameable;
        Movement.Action_StepTaken -= CheckFlameableCounter;
        PlayerStats.Action_RespawnPlayerEarly -= RemoveFlameable;
    }


    //--------------------


    void BecomeFlameable()
    {
        if (PlayerManager.Instance.block_StandingOn_Current.block && !firstTimeCheck)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_BurnTransforming>())
            {
                PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_BurnTransforming>().isSteppedOn = true;

                firstTimeCheck = true;
            }
        }

        if (PlayerManager.Instance.block_Vertical_InFront.block)
        {
            if (PlayerManager.Instance.block_Vertical_InFront.block.GetComponent<Block_Lava>())
            {
                AddFlameable();
            }
        }
        if (PlayerManager.Instance.block_Horizontal_InFront.block)
        {
            if (PlayerManager.Instance.block_Horizontal_InFront.block.GetComponent<Block_Lava>())
            {
                AddFlameable();
            }
        }

        if (PlayerManager.Instance.block_Vertical_InBack.block)
        {
            if (PlayerManager.Instance.block_Vertical_InBack.block.GetComponent<Block_Lava>())
            {
                AddFlameable();
            }
        }
        if (PlayerManager.Instance.block_Horizontal_InBack.block)
        {
            if (PlayerManager.Instance.block_Horizontal_InBack.block.GetComponent<Block_Lava>())
            {
                AddFlameable();
            }
        }

        if (PlayerManager.Instance.block_Vertical_ToTheLeft.block)
        {
            if (PlayerManager.Instance.block_Vertical_ToTheLeft.block.GetComponent<Block_Lava>())
            {
                AddFlameable();
            }
        }
        if (PlayerManager.Instance.block_Horizontal_ToTheLeft.block)
        {
            if (PlayerManager.Instance.block_Horizontal_ToTheLeft.block.GetComponent<Block_Lava>())
            {
                AddFlameable();
            }
        }

        if (PlayerManager.Instance.block_Vertical_ToTheRight.block)
        {
            if (PlayerManager.Instance.block_Vertical_ToTheRight.block.GetComponent<Block_Lava>())
            {
                AddFlameable();
            }
        }
        if (PlayerManager.Instance.block_Horizontal_ToTheRight.block)
        {
            if (PlayerManager.Instance.block_Horizontal_ToTheRight.block.GetComponent<Block_Lava>())
            {
                AddFlameable();
            }
        }
    }

    void CheckFlameableCounter()
    {
        if (!isBurning) { return; }

        flameableStepCounter += 1;

        //Remove Flameable after 5 steps
        if (flameableStepCounter > 5)
        {
            RemoveFlameable();
        }

        //Remove Flameable in water
        if (PlayerManager.Instance.block_StandingOn_Current.block)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_Water>())
            {
                RemoveFlameable();
            }
        }
    }
    void AddFlameable()
    {
        isBurning = true;
        flameableStepCounter = 0;

        flameEffectObject.SetActive(true);
    }
    void RemoveFlameable()
    {
        if (isBurning)
        {
            isBurning = false;
            flameableStepCounter = 0;

            flameEffectObject.SetActive(false);

            firstTimeCheck = false;
        }
    }
}
