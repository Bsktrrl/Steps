using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Quicksand : MonoBehaviour
{
    private void OnEnable()
    {
        Movement.Action_isSwitchingBlocks += SteppingOnQuicksand;
        Movement.Action_RespawnPlayer += SteppingOnQuicksand;
    }
    private void OnDisable()
    {
        Movement.Action_isSwitchingBlocks -= SteppingOnQuicksand;
        Movement.Action_RespawnPlayer -= SteppingOnQuicksand;
    }


    //--------------------


    void SteppingOnQuicksand()
    {
        if (Movement.Instance.blockStandingOn)
        {
            if (Movement.Instance.blockStandingOn.GetComponent<Block_Quicksand>())
            {
                Player_Quicksand.Instance.isInQuicksand = true;

                if (Movement.Instance.blockStandingOn == gameObject)
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
