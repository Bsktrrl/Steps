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
        Player_SwampWater playerSwampWater = Player_SwampWater.ExistingInstance;
        Movement movement = Movement.ExistingInstance;
        if (playerSwampWater && movement && movement.blockStandingOn)
            playerSwampWater.isInSwampWater = movement.blockStandingOn.TryGetComponent<Block_SwampWater>(out _);
    }
}
