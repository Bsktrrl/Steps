using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField] DetectorPoint detectorPoint;

    Vector3 raycastDown = Vector3.down;
    float maxDistance = 0.4f;
    RaycastHit hit;

    bool canMoveFurtherCheck;
    public bool stateHasChanged;


    //--------------------


    private void Start()
    {
        //canMoveFurther = false;

        PerformRaycast();
    }


    //--------------------


    public void PerformRaycast()
    {
        if (detectorPoint == DetectorPoint.Center) { return; }

        if (Physics.Raycast(gameObject.transform.position, raycastDown, out hit, maxDistance))
        {
            // Raycast hit something
            Debug.DrawRay(gameObject.transform.position, raycastDown * hit.distance, Color.green);

            if (detectorPoint == DetectorPoint.Front)
            {
                MainManager.Instance.playerStats.platformObject_Forward = hit.collider.gameObject;
            }
            else if (detectorPoint == DetectorPoint.Back)
            {
                MainManager.Instance.playerStats.platformObject_Backward = hit.collider.gameObject;
            }
            else if (detectorPoint == DetectorPoint.Right)
            {
                MainManager.Instance.playerStats.platformObject_Right = hit.collider.gameObject;
            }
            else if (detectorPoint == DetectorPoint.Left)
            {
                MainManager.Instance.playerStats.platformObject_Left = hit.collider.gameObject;
            }
        }
        else
        {
            // Raycast did not hit anything
            Debug.DrawRay(gameObject.transform.position, raycastDown * maxDistance, Color.red);

            if (detectorPoint == DetectorPoint.Front)
            {
                MainManager.Instance.playerStats.platformObject_Forward = null;
            }
            else if (detectorPoint == DetectorPoint.Back)
            {
                MainManager.Instance.playerStats.platformObject_Backward = null;
            }
            else if (detectorPoint == DetectorPoint.Right)
            {
                MainManager.Instance.playerStats.platformObject_Right = null;
            }
            else if (detectorPoint == DetectorPoint.Left)
            {
                MainManager.Instance.playerStats.platformObject_Left = null;
            }
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

            //if (hit.collider.CompareTag("Platform_Ice"))
            //{
            //    MainManager.Instance.playerStats.platformObject_StandingOn_Current = hit.collider.gameObject;
            //    MainManager.Instance.playerStats.movementSpeed = MainManager.Instance.playerStats.platformObject_StandingOn_Current.GetComponent<Platform>().speed;
            //}
            //else if (hit.collider.CompareTag("Platform_Grass"))
            //{
            //    MainManager.Instance.playerStats.platformObject_StandingOn_Current = hit.collider.gameObject;
            //    MainManager.Instance.playerStats.movementSpeed = MainManager.Instance.playerStats.platformObject_StandingOn_Current.GetComponent<Platform>().speed;
            //}
            //else
            //{
            //    MainManager.Instance.playerStats.platformObject_StandingOn_Current = null;
            //}
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
                    if (MainManager.Instance.playerStats.platformObject_StandingOn_Previous.GetComponent<Platform>().image_Darkener.activeInHierarchy)
                    {
                        MainManager.Instance.playerStats.platformObject_StandingOn_Previous.GetComponent<Platform>().image_Darkener.SetActive(false);
                    }
                }
            }

            if (MainManager.Instance.playerStats.platformObject_StandingOn_Current)
            {
                if (MainManager.Instance.playerStats.platformObject_StandingOn_Current.GetComponent<Platform>())
                {
                    if (!MainManager.Instance.playerStats.platformObject_StandingOn_Current.GetComponent<Platform>().image_Darkener.activeInHierarchy)
                    {
                        MainManager.Instance.playerStats.platformObject_StandingOn_Current.GetComponent<Platform>().image_Darkener.SetActive(true);
                    }
                }
            }
        }
    }
}
