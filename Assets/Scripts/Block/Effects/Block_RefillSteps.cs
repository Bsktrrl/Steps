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
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken -= RefillAvailableSteps;
    }


    //--------------------


    void RefillAvailableSteps()
    {
        if (Movement.Instance.blockStandingOn == gameObject)
        {
            PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Current = PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Max;
            StartCoroutine(ResetSteps(0.01f));
        }
    }
    IEnumerator ResetSteps(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Current = PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Max;

        Action_RefillStepsEntered?.Invoke();
    }
}
