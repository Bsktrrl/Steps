using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FallingBlock : MonoBehaviour
{
    private void Start()
    {
        Player_Movement.Action_StepTaken += StepsOnFallableBlock;
    }


    //--------------------


    void StepsOnFallableBlock()
    {
        if (MainManager.Instance.block_StandingOn.block)
        {
            if (MainManager.Instance.block_StandingOn.block.GetComponent<Block_Falling>())
            {
                MainManager.Instance.block_StandingOn.block.GetComponent<Block_Falling>().isSteppedOn = true;
            }
        }
    }
}
