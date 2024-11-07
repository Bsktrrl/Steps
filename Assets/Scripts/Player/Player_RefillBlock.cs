using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_RefillBlock : MonoBehaviour
{
    private void Start()
    {
        Player_Movement.Action_StepTaken += RefillAvailableSteps;
    }


    //--------------------


    void RefillAvailableSteps()
    {
        if (MainManager.Instance.block_StandingOn.blockElement == BlockElement.RefillSteps)
        {
            gameObject.GetComponent<Player_Stats>().stats.steps_Current = gameObject.GetComponent<Player_Stats>().stats.steps_Max;
        }
    }
}
