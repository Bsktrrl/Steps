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
        if (Movement.Instance.blockStandingOn != gameObject && isStandingOnBlock)
        {
            isStandingOnBlock = false;
        }
        else if (Movement.Instance.blockStandingOn == gameObject && !isStandingOnBlock)
        {
            isStandingOnBlock = true;
        }
    }
}