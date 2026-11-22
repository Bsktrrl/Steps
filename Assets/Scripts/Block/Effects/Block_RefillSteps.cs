using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_RefillSteps : MonoBehaviour
{
    public static event Action Action_RefillStepsEntered;

    private void OnEnable()
    {
        Movement.Action_StepTaken += RefillAvailableSteps;
        Movement.Action_LandedFromFalling += RefillAvailableSteps;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken -= RefillAvailableSteps;
        Movement.Action_LandedFromFalling -= RefillAvailableSteps;
    }


    //--------------------


    void RefillAvailableSteps()
    {
        if (Movement.Instance.blockStandingOn == gameObject)
        {
            PlayerStats.Instance.RestoreMissingSteps();
            StartCoroutine(ResetSteps(0.01f));
        }
    }
    IEnumerator ResetSteps(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        PlayerStats.Instance.RestoreMissingSteps();

        Action_RefillStepsEntered?.Invoke();
    }
}
