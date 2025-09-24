using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_MushroomCircle : MonoBehaviour
{
    public static event Action Action_MushroomCircleEntered;

    private void OnEnable()
    {
        Movement.Action_StepTaken += Refill_1_Step;
        Movement.Action_LandedFromFalling += Refill_1_StepAfterFalling;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken -= Refill_1_Step;
        Movement.Action_LandedFromFalling -= Refill_1_StepAfterFalling;
    }


    //--------------------


    void Refill_1_Step()
    {
        if (Movement.Instance.blockStandingOn == gameObject)
        {
            Action_MushroomCircleEntered?.Invoke();
        }
    }
    void Refill_1_StepAfterFalling()
    {
        if (Movement.Instance.blockStandingOn == gameObject)
        {
            PlayerStats.Instance.stats.steps_Current += 1;
            Action_MushroomCircleEntered?.Invoke();
        }
    }
}
