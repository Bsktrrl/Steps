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
        Player_Mud playerMud = Player_Mud.ExistingInstance;
        Movement movement = Movement.ExistingInstance;
        if (playerMud && movement && movement.blockStandingOn)
            playerMud.isInMud = movement.blockStandingOn.TryGetComponent<Block_Mud>(out _);
    }
}
