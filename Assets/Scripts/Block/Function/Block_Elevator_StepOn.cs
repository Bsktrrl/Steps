using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Elevator_StepOn : MonoBehaviour
{
    public bool isStandingOnBlock;


    //--------------------


    private void Update()
    {
        CheckIfPlayerIsOn();
    }


    //--------------------


    void CheckIfPlayerIsOn()
    {
        if (PlayerManager.Instance.block_StandingOn_Current.block != gameObject && isStandingOnBlock)
        {
            isStandingOnBlock = false;
        }
        else if (PlayerManager.Instance.block_StandingOn_Current.block == gameObject && !isStandingOnBlock)
        {
            isStandingOnBlock = true;
        }
    }
}