using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Mud : MonoBehaviour
{
    private void OnEnable()
    {
        Player_BlockDetector.Action_isSwitchingBlocks += SteppingOnMud;
        PlayerStats.Action_RespawnPlayer += SteppingOnMud;
    }
    private void OnDisable()
    {
        Player_BlockDetector.Action_isSwitchingBlocks -= SteppingOnMud;
        PlayerStats.Action_RespawnPlayer -= SteppingOnMud;
    }


    //--------------------


    void SteppingOnMud()
    {
        if (PlayerManager.Instance.block_StandingOn_Current.block)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_Mud>())
            {
                Player_Mud.Instance.isInMud = true;
            }
            else
            {
                Player_Mud.Instance.isInMud = false;
            }
        }
    }
}
