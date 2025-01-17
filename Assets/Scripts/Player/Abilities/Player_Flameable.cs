using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Flameable : Singleton<Player_Flameable>
{
    public bool isFlameable;
    public int flameableStepCounter;


    //--------------------


    private void OnEnable()
    {
        Player_BlockDetector.Action_isSwitchingBlocks += BecomeFlameable;
        Player_Movement.Action_StepTaken += BecomeFlameable;
        Player_Movement.Action_StepTaken += CheckFlameableCounter;
        PlayerStats.Action_RespawnPlayer += RemoveFlameable;
    }
    private void OnDisable()
    {
        Player_BlockDetector.Action_isSwitchingBlocks -= BecomeFlameable;
        Player_Movement.Action_StepTaken -= BecomeFlameable;
        Player_Movement.Action_StepTaken -= CheckFlameableCounter;
        PlayerStats.Action_RespawnPlayer -= RemoveFlameable;
    }


    //--------------------


    void BecomeFlameable()
    {
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.Flameable && !PlayerStats.Instance.stats.abilitiesGot_Permanent.Flameable) { return; }

        if (PlayerManager.Instance.block_Vertical_InFront.block)
        {
            if (PlayerManager.Instance.block_Vertical_InFront.block.GetComponent<Block_Lava>())
            {
                isFlameable = true;
                flameableStepCounter = 0;
            }
        }
        
        if (PlayerManager.Instance.block_Vertical_InBack.block)
        {
            if (PlayerManager.Instance.block_Vertical_InBack.block.GetComponent<Block_Lava>())
            {
                isFlameable = true;
                flameableStepCounter = 0;
            }
        }
        
        if (PlayerManager.Instance.block_Vertical_ToTheLeft.block)
        {
            if (PlayerManager.Instance.block_Vertical_ToTheLeft.block.GetComponent<Block_Lava>())
            {
                isFlameable = true;
                flameableStepCounter = 0;
            }
        }
        
        if (PlayerManager.Instance.block_Vertical_ToTheRight.block)
        {
            if (PlayerManager.Instance.block_Vertical_ToTheRight.block.GetComponent<Block_Lava>())
            {
                isFlameable = true;
                flameableStepCounter = 0;
            }
        }
    }

    void CheckFlameableCounter()
    {
        if (!PlayerStats.Instance.stats.abilitiesGot_Temporary.Flameable && !PlayerStats.Instance.stats.abilitiesGot_Permanent.Flameable) { return; }
        if (!isFlameable) { return; }

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
    void RemoveFlameable()
    {
        isFlameable = false;
        flameableStepCounter = 0;
    }

}
