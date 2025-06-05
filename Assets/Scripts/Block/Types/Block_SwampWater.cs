using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_SwampWater : MonoBehaviour
{
    private void OnEnable()
    {
        Movement.Action_isSwitchingBlocks += SteppingOnSwampWater;
        PlayerStats.Action_RespawnPlayer += SteppingOnSwampWater;
    }
    private void OnDisable()
    {
        Movement.Action_isSwitchingBlocks -= SteppingOnSwampWater;
        PlayerStats.Action_RespawnPlayer -= SteppingOnSwampWater;
    }


    //--------------------


    void SteppingOnSwampWater()
    {
        if (PlayerManager.Instance.block_StandingOn_Current.block)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_SwampWater>())
            {
                Player_SwampWater.Instance.isInSwampWater = true;
            }
            else
            {
                Player_SwampWater.Instance.isInSwampWater = false;
            }
        }
    }
}
