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

    private void Start()
    {
        canMoveFurther = false;

        PerformRaycast();
    }

    private void Update()
    {
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
        else
        {
            PerformRaycast();
        }
    }

    void PerformRaycast()
    {
        if (Physics.Raycast(gameObject.transform.position, raycastDown, out hit, maxDistance))
        {
            // Raycast hit something
            Debug.DrawRay(gameObject.transform.position, raycastDown * hit.distance, Color.green);

            if (hit.collider.CompareTag("Platform"))
            {
                if (gameObject.transform.parent.GetComponent<PlayerMovement>())
                {
                    canMoveFurther = true;
                }
            }
            else
            {
                if (gameObject.transform.parent.GetComponent<PlayerMovement>())
                {
                    canMoveFurther = false;
                }
            }
        }
        else
        {
            // Raycast did not hit anything
            Debug.DrawRay(gameObject.transform.position, raycastDown * maxDistance, Color.red);

            if (gameObject.transform.parent.GetComponent<PlayerMovement>())
            {
                canMoveFurther = false;
            }
        }
    }
}
