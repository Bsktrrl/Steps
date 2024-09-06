using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private void Start()
    {
        PlayerMovement.playerStopped += Teleport;
    }

    void Teleport()
    {
        if (PlayerDetectorController.Instance.platform_Center)
        {
            if (PlayerDetectorController.Instance.platform_Center.GetComponent<Platform>())
            {
                if (PlayerDetectorController.Instance.platform_Center.GetComponent<Platform>().platformType == PlatformType.Teleporter)
                {
                    if (PlayerDetectorController.Instance.platform_Center.GetComponent<Platform_Teleporter>())
                    {
                        if (transform.position.x == PlayerDetectorController.Instance.platform_Center.GetComponent<Platform_Teleporter>().teleporter_1.transform.position.x
                            && transform.position.z == PlayerDetectorController.Instance.platform_Center.GetComponent<Platform_Teleporter>().teleporter_1.transform.position.z)
                        {
                            print("6. Teleporter");
                            transform.position = PlayerDetectorController.Instance.platform_Center.GetComponent<Platform_Teleporter>().teleporter_2.transform.position + new Vector3(0, 0.35f, 0);
                        }
                        else if (transform.position.x == PlayerDetectorController.Instance.platform_Center.GetComponent<Platform_Teleporter>().teleporter_2.transform.position.x
                            && transform.position.z == PlayerDetectorController.Instance.platform_Center.GetComponent<Platform_Teleporter>().teleporter_2.transform.position.z)
                        {
                            print("7. Teleporter");
                            transform.position = PlayerDetectorController.Instance.platform_Center.GetComponent<Platform_Teleporter>().teleporter_1.transform.position + new Vector3(0, 0.35f, 0);
                        }
                    }
                }
            }
        }
    }
}
