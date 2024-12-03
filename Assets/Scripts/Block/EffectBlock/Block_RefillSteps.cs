using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_RefillSteps : MonoBehaviour
{
    private void OnEnable()
    {
        Player_Movement.Action_StepTaken += RefillAvailableSteps;
    }

    private void OnDisable()
    {
        Player_Movement.Action_StepTaken -= RefillAvailableSteps;
    }


    //--------------------


    void RefillAvailableSteps()
    {
        if (PlayerManager.Instance.block_StandingOn_Current.block == gameObject)
        {
            PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Current = PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Max + PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementCost;
        }
    }
}
