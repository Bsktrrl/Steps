using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField] DetectorPoint detectorPoint;

    Vector3 raycastDown = Vector3.down;
    float maxDistance = 0.4f;
    RaycastHit hit;

    public bool stateHasChanged;


    //--------------------


    private void Start()
    {
        PerformRaycast();
    }


    //--------------------


    public void PerformRaycast()
    {
        if (detectorPoint == DetectorPoint.Center) { return; }

        if (Physics.Raycast(gameObject.transform.position, raycastDown, out hit, maxDistance))
        {
            if (hit.collider.gameObject.GetComponent<Platform>().platformType == PlatformTypes.Water)
            {
                if (!MainManager.Instance.keyItems.SwimSuit)
                {
                    RaycastNothing();
                    return;
                }
            }
            else if (hit.collider.gameObject.GetComponent<Platform>().platformType == PlatformTypes.Lava)
            {
                if (!MainManager.Instance.keyItems.LavaSuit)
                {
                    RaycastNothing();
                    return;
                }
            }

            // Raycast hit something
            RaycastSomething();
        }
        else
        {
            // Raycast did not hit anything
            RaycastNothing();
        }
    }
    public void PerformRaycast_Center()
    {
        if (detectorPoint != DetectorPoint.Center) { return; }

        GameObject standingOnPrevious_Check = MainManager.Instance.playerStats.platformObject_StandingOn_Current;

        if (Physics.Raycast(gameObject.transform.position, raycastDown, out hit, maxDistance))
        {
            // Raycast hit something
            Debug.DrawRay(gameObject.transform.position, raycastDown * hit.distance, Color.green);

            MainManager.Instance.playerStats.platformObject_StandingOn_Current = hit.collider.gameObject;
            MainManager.Instance.playerStats.movementSpeed = MainManager.Instance.playerStats.platformObject_StandingOn_Current.GetComponent<Platform>().speed;
        }
        else
        {
            // Raycast did not hit anything
            Debug.DrawRay(gameObject.transform.position, raycastDown * maxDistance, Color.red);

            MainManager.Instance.playerStats.platformObject_StandingOn_Current = null;
        }

        //Get the previous Platform standing on
        if (standingOnPrevious_Check != MainManager.Instance.playerStats.platformObject_StandingOn_Current && standingOnPrevious_Check
            && MainManager.Instance.playerStats.platformObject_StandingOn_Previous != standingOnPrevious_Check)
        {
            MainManager.Instance.playerStats.platformObject_StandingOn_Previous = standingOnPrevious_Check;
        }

        //Change Platform Standing Shadow
        if (MainManager.Instance.playerStats.platformObject_StandingOn_Previous != MainManager.Instance.playerStats.platformObject_StandingOn_Current)
        {
            if (MainManager.Instance.playerStats.platformObject_StandingOn_Previous)
            {
                if (MainManager.Instance.playerStats.platformObject_StandingOn_Previous.GetComponent<Platform>())
                {
                    if (MainManager.Instance.playerStats.platformObject_StandingOn_Previous.GetComponent<Platform>().image_Darkener.gameObject.activeInHierarchy)
                    {
                        MainManager.Instance.playerStats.platformObject_StandingOn_Previous.GetComponent<Platform>().image_Darkener.gameObject.SetActive(false);
                    }
                }
            }

            if (MainManager.Instance.playerStats.platformObject_StandingOn_Current)
            {
                if (MainManager.Instance.playerStats.platformObject_StandingOn_Current.GetComponent<Platform>())
                {
                    if (!MainManager.Instance.playerStats.platformObject_StandingOn_Current.GetComponent<Platform>().image_Darkener.gameObject.activeInHierarchy)
                    {
                        MainManager.Instance.playerStats.platformObject_StandingOn_Current.GetComponent<Platform>().image_Darkener.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    void RaycastSomething()
    {
        Debug.DrawRay(gameObject.transform.position, raycastDown * hit.distance, Color.green);

        if (detectorPoint == DetectorPoint.Front)
        {
            if (MainManager.Instance.playerStats.platformObject_Forward)
            {
                MainManager.Instance.playerStats.platformObject_Forward.GetComponent<Platform>().stepCost_Text.gameObject.SetActive(false);
            }
            MainManager.Instance.playerStats.platformObject_Forward = hit.collider.gameObject;
            MainManager.Instance.playerStats.platformObject_Forward.GetComponent<Platform>().stepCost_Text.gameObject.SetActive(true);
        }
        else if (detectorPoint == DetectorPoint.Back)
        {
            if (MainManager.Instance.playerStats.platformObject_Backward)
            {
                MainManager.Instance.playerStats.platformObject_Backward.GetComponent<Platform>().stepCost_Text.gameObject.SetActive(false);
            }
            MainManager.Instance.playerStats.platformObject_Backward = hit.collider.gameObject;
            MainManager.Instance.playerStats.platformObject_Backward.GetComponent<Platform>().stepCost_Text.gameObject.SetActive(true);
        }
        else if (detectorPoint == DetectorPoint.Right)
        {
            if (MainManager.Instance.playerStats.platformObject_Right)
            {
                MainManager.Instance.playerStats.platformObject_Right.GetComponent<Platform>().stepCost_Text.gameObject.SetActive(false);
            }
            MainManager.Instance.playerStats.platformObject_Right = hit.collider.gameObject;
            MainManager.Instance.playerStats.platformObject_Right.GetComponent<Platform>().stepCost_Text.gameObject.SetActive(true);
        }
        else if (detectorPoint == DetectorPoint.Left)
        {
            if (MainManager.Instance.playerStats.platformObject_Left)
            {
                MainManager.Instance.playerStats.platformObject_Left.GetComponent<Platform>().stepCost_Text.gameObject.SetActive(false);
            }
            MainManager.Instance.playerStats.platformObject_Left = hit.collider.gameObject;
            MainManager.Instance.playerStats.platformObject_Left.GetComponent<Platform>().stepCost_Text.gameObject.SetActive(true);
        }
    }
    void RaycastNothing()
    {
        Debug.DrawRay(gameObject.transform.position, raycastDown * maxDistance, Color.red);

        if (detectorPoint == DetectorPoint.Front)
        {
            if (MainManager.Instance.playerStats.platformObject_Forward)
            {
                MainManager.Instance.playerStats.platformObject_Forward.GetComponent<Platform>().stepCost_Text.gameObject.SetActive(false);
            }
            MainManager.Instance.playerStats.platformObject_Forward = null;
        }
        else if (detectorPoint == DetectorPoint.Back)
        {
            if (MainManager.Instance.playerStats.platformObject_Backward)
            {
                MainManager.Instance.playerStats.platformObject_Backward.GetComponent<Platform>().stepCost_Text.gameObject.SetActive(false);
            }
            MainManager.Instance.playerStats.platformObject_Backward = null;
        }
        else if (detectorPoint == DetectorPoint.Right)
        {
            if (MainManager.Instance.playerStats.platformObject_Right)
            {
                MainManager.Instance.playerStats.platformObject_Right.GetComponent<Platform>().stepCost_Text.gameObject.SetActive(false);
            }
            MainManager.Instance.playerStats.platformObject_Right = null;
        }
        else if (detectorPoint == DetectorPoint.Left)
        {
            if (MainManager.Instance.playerStats.platformObject_Left)
            {
                MainManager.Instance.playerStats.platformObject_Left.GetComponent<Platform>().stepCost_Text.gameObject.SetActive(false);
            }
            MainManager.Instance.playerStats.platformObject_Left = null;
        }
    }
}
