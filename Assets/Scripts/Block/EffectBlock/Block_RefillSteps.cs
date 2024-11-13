using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_RefillSteps : MonoBehaviour
{
    private void Start()
    {
        Player_Movement.Action_StepTaken += RefillAvailableSteps;
    }


    //--------------------


    void RefillAvailableSteps()
    {
        if (MainManager.Instance.block_StandingOn_Current.block == gameObject)
        {
            MainManager.Instance.player.GetComponent<Player_Stats>().stats.steps_Current = MainManager.Instance.player.GetComponent<Player_Stats>().stats.steps_Max;
        }
    }
}
