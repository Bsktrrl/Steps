using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Mud : MonoBehaviour
{
    private void OnEnable()
    {
        Movement.Action_isSwitchingBlocks += SteppingOnMud;
        Movement.Action_RespawnPlayer += SteppingOnMud;
    }
    private void OnDisable()
    {
        Movement.Action_isSwitchingBlocks -= SteppingOnMud;
        Movement.Action_RespawnPlayer -= SteppingOnMud;
    }


    //--------------------


    void SteppingOnMud()
    {
        if (Movement.Instance.blockStandingOn)
        {
            if (Movement.Instance.blockStandingOn.GetComponent<Block_Mud>())
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
