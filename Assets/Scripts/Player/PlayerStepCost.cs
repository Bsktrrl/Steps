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
        if(PlayerController.Instance.platform_Center)
        {
            if (PlayerController.Instance.platform_Center.GetComponent<Platform>())
            {
                PlayerStats.Instance.stats.steps_Current -= PlayerController.Instance.platform_Center.GetComponent<Platform>().stepsCost;

                if (PlayerStats.Instance.stats.steps_Current < 0)
                {
                    RefillSteps();
                }

                updateStepCounter?.Invoke();
            }
        }
    }

    void RefillSteps()
    {
        transform.position = PlayerStats.Instance.startPos;

        PlayerStats.Instance.stats.steps_Current = PlayerStats.Instance.stats.steps_Max;
        gameObject.GetComponent<PlayerMovement>().movementStates = MovementStates.Still;
    }
}
