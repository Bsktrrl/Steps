using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Quicksand : MonoBehaviour
{
    private void OnEnable()
    {
        Player_BlockDetector.Action_isSwitchingBlocks += SteppingOnQuicksand;
        PlayerStats.Action_RespawnPlayer += SteppingOnQuicksand;
    }
    private void OnDisable()
    {
        Player_BlockDetector.Action_isSwitchingBlocks -= SteppingOnQuicksand;
        PlayerStats.Action_RespawnPlayer -= SteppingOnQuicksand;
    }


    //--------------------


    void SteppingOnQuicksand()
    {
        if (PlayerManager.Instance.block_StandingOn_Current.block)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_Quicksand>())
            {
                Player_Quicksand.Instance.isInQuicksand = true;

                if (PlayerManager.Instance.block_StandingOn_Current.block == gameObject)
                {
                    Player_Quicksand.Instance.quicksandCounter += 1;
                }
            }
            else
            {
                Player_Quicksand.Instance.isInQuicksand = false;
                Player_Quicksand.Instance.quicksandCounter = 0;
            }
        }
    }
}
