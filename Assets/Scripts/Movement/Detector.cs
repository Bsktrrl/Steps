using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField] DetectorPoint detectorPoint;

    Vector3 raycastDown = Vector3.down;
    float maxDistance = 0.4f;
    RaycastHit hit;

    public bool canMoveFurther;
    bool canMoveFurtherCheck;
    public bool stateHasChanged;


    //--------------------


    private void Start()
    {
        canMoveFurther = false;

        PerformRaycast();
    }

    private void Update()
    {
        //Raycast for Borders
        if ((detectorPoint == DetectorPoint.Front && gameObject.transform.parent.GetComponent<PlayerMovement>().movementDirection == MovementDirection.Forward)
            || (detectorPoint == DetectorPoint.Back && gameObject.transform.parent.GetComponent<PlayerMovement>().movementDirection == MovementDirection.Backward)
            || (detectorPoint == DetectorPoint.Right && gameObject.transform.parent.GetComponent<PlayerMovement>().movementDirection == MovementDirection.Right)
            || (detectorPoint == DetectorPoint.Left && gameObject.transform.parent.GetComponent<PlayerMovement>().movementDirection == MovementDirection.Left))
        {
            canMoveFurtherCheck = canMoveFurther;

            PerformRaycast();

            if (canMoveFurtherCheck != canMoveFurther && canMoveFurther == false)
            {
                stateHasChanged = true;
            }
        }

        //Raycast Other
        else
        {
            PerformRaycast();
        }

        PerformRaycast_Center();
    }


    //--------------------


    public void PerformRaycast()
    {
        if (Physics.Raycast(gameObject.transform.position, raycastDown, out hit, maxDistance))
        {
            // Raycast hit something
            Debug.DrawRay(gameObject.transform.position, raycastDown * hit.distance, Color.green);

            if (hit.collider.CompareTag("Platform_Ice"))
            {
                canMoveFurther = true;
            }
            else if (hit.collider.CompareTag("Platform_Grass"))
            {
                canMoveFurther = true;
            }
            else
            {
                canMoveFurther = false;
            }

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

            canMoveFurther = false;

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
    void PerformRaycast_Center()
    {
        if (Physics.Raycast(gameObject.transform.position, raycastDown, out hit, maxDistance))
        {
            // Raycast hit something
            Debug.DrawRay(gameObject.transform.position, raycastDown * hit.distance, Color.green);

            if (hit.collider.CompareTag("Platform_Ice"))
            {
                MainManager.Instance.playerStats.platformType_StandingOn = PlatformTypes.Ice;
                MainManager.Instance.playerStats.platformObject_StandingOn = hit.collider.gameObject;
                MainManager.Instance.playerStats.movementSpeed = MainManager.Instance.playerStats.platformObject_StandingOn.GetComponent<Platform>().speed;
            }
            else if (hit.collider.CompareTag("Platform_Grass"))
            {
                MainManager.Instance.playerStats.platformType_StandingOn = PlatformTypes.Grass;
                MainManager.Instance.playerStats.platformObject_StandingOn = hit.collider.gameObject;
                MainManager.Instance.playerStats.movementSpeed = MainManager.Instance.playerStats.platformObject_StandingOn.GetComponent<Platform>().speed;
            }
            else
            {
                MainManager.Instance.playerStats.platformType_StandingOn = PlatformTypes.None;
                MainManager.Instance.playerStats.platformObject_StandingOn = null;
            }
        }
        else
        {
            // Raycast did not hit anything
            Debug.DrawRay(gameObject.transform.position, raycastDown * maxDistance, Color.red);

            MainManager.Instance.playerStats.platformType_StandingOn = PlatformTypes.None;
            MainManager.Instance.playerStats.platformObject_StandingOn = null;
        }
    }
}
