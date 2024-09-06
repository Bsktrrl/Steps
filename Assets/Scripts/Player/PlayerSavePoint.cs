using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSavePoint : Singleton<PlayerSavePoint>
{
    private void Start()
    {
        PlayerMovement.playerStopped += SavePlayerPosition;
    }

    void SavePlayerPosition()
    {
        if (PlayerDetectorController.Instance.platform_Center)
        {
            if (PlayerDetectorController.Instance.platform_Center.GetComponent<Platform>())
            {
                if (PlayerDetectorController.Instance.platform_Center.GetComponent<Platform>().platformType == PlatformType.SavePoint)
                {
                    PlayerStats.Instance.startPos = transform.position;
                }
            }
        }
    }
}
