using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_SwampWater : MonoBehaviour
{
    private void OnEnable()
    {
        Movement.Action_isSwitchingBlocks += SteppingOnSwampWater;
        Movement.Action_RespawnPlayer += SteppingOnSwampWater;
    }
    private void OnDisable()
    {
        Movement.Action_isSwitchingBlocks -= SteppingOnSwampWater;
        Movement.Action_RespawnPlayer -= SteppingOnSwampWater;
    }


    //--------------------


    void SteppingOnSwampWater()
    {
        if (Movement.Instance.blockStandingOn)
        {
            if (Movement.Instance.blockStandingOn.GetComponent<Block_SwampWater>())
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
