using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRefillSteps : MonoBehaviour
{
    private void Start()
    {
        PlayerMovement.playerStopped += RefillSteps;
    }

    void RefillSteps()
    {
        if (PlayerDetectorController.Instance.platform_Center)
        {
            if (PlayerDetectorController.Instance.platform_Center.GetComponent<Platform>())
            {
                if (PlayerDetectorController.Instance.platform_Center.GetComponent<Platform>().platformType == PlatformType.RefillSteps)
                {
                    PlayerStats.Instance.stats.steps_Current = PlayerStats.Instance.stats.steps_Max;
                }
            }
        }
    }
}
