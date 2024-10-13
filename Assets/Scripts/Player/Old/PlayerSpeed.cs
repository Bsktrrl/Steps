using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeed : Singleton<PlayerSpeed>
{
    private void Update()
    {
        UpdatePlayerSpeed();
    }

    void UpdatePlayerSpeed()
    {
        if (PlayerDetectorController.Instance.platform_Center)
        {
            PlayerStats.Instance.stats.movementSpeed = PlayerDetectorController.Instance.platform_Center.GetComponent<Platform>().speed;
        }
    }
}
