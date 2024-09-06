using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDetector : MonoBehaviour
{
    public static event Action finishMovement;

    public DetectorTypes detectorType;

    float maxDistance_Vertical = 0.4f;
    float maxDistance_Horizontal = 0.3f;

    RaycastHit hit;

    Vector3 direction = Vector3.down;
    float maxDistance = 0;
    Color color = Color.white;

    public Vector3 detectedPlatformNormal;


    //--------------------


    private void Update()
    {
        SetupRaycast();
    }


    //--------------------


    #region Setup Raycast

    public void SetupRaycast()
    {
        SetParameters();

        PerformRaycast();
    }

    void SetParameters()
    {
        switch (detectorType)
        {
            case DetectorTypes.None:
                break;

            case DetectorTypes.Center:
                direction = Vector3.down;
                maxDistance = maxDistance_Vertical;
                color = Color.cyan;
                break;

            case DetectorTypes.Vertical_Front:
                direction = Vector3.down;
                maxDistance = maxDistance_Vertical;
                color = Color.blue;
                break;
            case DetectorTypes.Vertical_Back:
                direction = Vector3.down;
                maxDistance = maxDistance_Vertical;
                color = Color.blue;
                break;
            case DetectorTypes.Vertical_Right:
                direction = Vector3.down;
                maxDistance = maxDistance_Vertical;
                color = Color.blue;
                break;
            case DetectorTypes.Vertical_Left:
                direction = Vector3.down;
                maxDistance = maxDistance_Vertical;
                color = Color.blue;
                break;

            case DetectorTypes.Horizontal_Front:
                direction = Vector3.forward;
                maxDistance = maxDistance_Horizontal;
                color = Color.yellow;
                break;
            case DetectorTypes.Horizontal_Back:
                direction = Vector3.back;
                maxDistance = maxDistance_Horizontal;
                color = Color.yellow;
                break;
            case DetectorTypes.Horizontal_Right:
                direction = Vector3.right;
                maxDistance = maxDistance_Horizontal;
                color = Color.yellow;
                break;
            case DetectorTypes.Horizontal_Left:
                direction = Vector3.left;
                maxDistance = maxDistance_Horizontal;
                color = Color.yellow;
                break;

            default:
                break;
        }
    }
    void PerformRaycast()
    {
        if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
        {
            Debug.DrawRay(transform.position, direction * hit.distance, Color.green);

            detectedPlatformNormal = hit.normal;

            UpdatePlatform_Detected(hit.collider.gameObject);
        }
        else
        {
            Debug.DrawRay(transform.position, direction * maxDistance, color);

            detectedPlatformNormal = Vector3.zero;

            UpdatePlatform_NotDetected();
        }

        finishMovement?.Invoke();
    }
    void UpdatePlatform_Detected(GameObject detectedPlatform)
    {
        switch (detectorType)
        {
            case DetectorTypes.None:
                break;

            case DetectorTypes.Center:
                transform.GetComponentInParent<PlayerController>().Platform_Center_Previous = transform.GetComponentInParent<PlayerController>().Platform_Center;
                transform.GetComponentInParent<PlayerController>().Platform_Center = hit.collider.gameObject;
                break;
            case DetectorTypes.Vertical_Front:
                transform.GetComponentInParent<PlayerController>().platform_Vertical_Forward = hit.collider.gameObject;
                break;
            case DetectorTypes.Vertical_Back:
                transform.GetComponentInParent<PlayerController>().platform_Vertical_Backward = hit.collider.gameObject;
                break;
            case DetectorTypes.Vertical_Right:
                transform.GetComponentInParent<PlayerController>().platform_Vertical_Right = hit.collider.gameObject;
                break;
            case DetectorTypes.Vertical_Left:
                transform.GetComponentInParent<PlayerController>().platform_Vertical_Left = hit.collider.gameObject;
                break;
            case DetectorTypes.Horizontal_Front:
                transform.GetComponentInParent<PlayerController>().platform_Horizontal_Forward = hit.collider.gameObject;
                break;
            case DetectorTypes.Horizontal_Back:
                transform.GetComponentInParent<PlayerController>().platform_Horizontal_Backward = hit.collider.gameObject;
                break;
            case DetectorTypes.Horizontal_Right:
                transform.GetComponentInParent<PlayerController>().platform_Horizontal_Right = hit.collider.gameObject;
                break;
            case DetectorTypes.Horizontal_Left:
                transform.GetComponentInParent<PlayerController>().platform_Horizontal_Left = hit.collider.gameObject;
                break;

            default:
                break;
        }
    }
    void UpdatePlatform_NotDetected()
    {
        switch (detectorType)
        {
            case DetectorTypes.None:
                break;

            case DetectorTypes.Center:
                transform.GetComponentInParent<PlayerController>().Platform_Center = null;
                break;
            case DetectorTypes.Vertical_Front:
                transform.GetComponentInParent<PlayerController>().platform_Vertical_Forward = null;
                break;
            case DetectorTypes.Vertical_Back:
                transform.GetComponentInParent<PlayerController>().platform_Vertical_Backward = null;
                break;
            case DetectorTypes.Vertical_Right:
                transform.GetComponentInParent<PlayerController>().platform_Vertical_Right = null;
                break;
            case DetectorTypes.Vertical_Left:
                transform.GetComponentInParent<PlayerController>().platform_Vertical_Left = null;
                break;
            case DetectorTypes.Horizontal_Front:
                transform.GetComponentInParent<PlayerController>().platform_Horizontal_Forward = null;
                break;
            case DetectorTypes.Horizontal_Back:
                transform.GetComponentInParent<PlayerController>().platform_Horizontal_Backward = null;
                break;
            case DetectorTypes.Horizontal_Right:
                transform.GetComponentInParent<PlayerController>().platform_Horizontal_Right = null;
                break;
            case DetectorTypes.Horizontal_Left:
                transform.GetComponentInParent<PlayerController>().platform_Horizontal_Left = null;
                break;

            default:
                break;
        }
    }

    #endregion
}

public enum DetectorTypes
{
    None,

    Center,

    Vertical_Front,
    Vertical_Back,
    Vertical_Right,
    Vertical_Left,

    Horizontal_Front,
    Horizontal_Back,
    Horizontal_Right,
    Horizontal_Left,
}