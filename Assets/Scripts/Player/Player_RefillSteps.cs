using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_RefillSteps : MonoBehaviour
{
    private void Start()
    {
        Player_Movement.Action_StepTaken += RefillAvailableSteps;
    }


    //--------------------


    void RefillAvailableSteps()
    {
        if (MainManager.Instance.block_StandingOn.block.GetComponent<Block_RefillSteps>())
        {
            gameObject.GetComponent<Player_Stats>().stats.steps_Current = gameObject.GetComponent<Player_Stats>().stats.steps_Max;
        }
    }
}
