using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStepCost : Singleton<PlayerStepCost>
{
    public static event Action updateStepCounter;

    private void Start()
    {
        PlayerMovement.playerStopped += DecreaseStepCounter;
    }

    void DecreaseStepCounter()
    {
        if(PlayerDetectorController.Instance.platform_Center)
        {
            if (PlayerDetectorController.Instance.platform_Center.GetComponent<Platform>())
            {
                PlayerStats.Instance.stats.steps_Current -= PlayerDetectorController.Instance.platform_Center.GetComponent<Platform>().stepsCost;

                //if (PlayerStats.Instance.stats.steps_Current < 0)
                //{
                //    RefillSteps();
                //}

                updateStepCounter?.Invoke();
            }
        }
    }

    public void RefillSteps()
    {
        transform.position = PlayerStats.Instance.startPos;

        StartCoroutine(WaitAfterReset(0.5f));
    }

    IEnumerator WaitAfterReset(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        PlayerStats.Instance.stats.steps_Current = PlayerStats.Instance.stats.steps_Max;
        updateStepCounter?.Invoke();

        yield return new WaitForSeconds(waitTime);

        gameObject.GetComponent<PlayerMovement>().movementStates = MovementStates.Still;
    }
}
