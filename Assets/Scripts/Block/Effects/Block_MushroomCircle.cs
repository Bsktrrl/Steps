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
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken -= Refill_1_Step;
    }


    //--------------------


    void Refill_1_Step()
    {
        if (PlayerManager.Instance.block_StandingOn_Current.block == gameObject)
        {
            Action_MushroomCircleEntered?.Invoke();
        }
    }
}
