using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArm : MonoBehaviour
{
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject cameraOffset;

    [Header("Camera Heights")]
    float camera_NormalHeight = 0.2f;   // more influential
    float camera_StairHeight = 0.7f;    // more influential

    [Header("Rotation Settings")]
    float camera_RotationX_Offset_Normal = 29;
    float camera_RotationX_Offset_NormalOffset = 0;
    float camera_RotationX_Offset_CeilingGrab = -17;
    float transitionSpeed = 10f;

    [Header("Camera Distance")]
    float sphereRadius = 0.35f;
    float desiredDistance = 5f;   // shorter ray
    float minDistance = 0.5f;     // fixed nearest point

    [Header("Ray Tilt")]
    float rayTiltAngle = 15f; // upward tilt to avoid stairs

    void Update()
    {
        // Base target pos (no zoom)
        Vector3 targetPos = transform.position + transform.TransformDirection(cameraController.cameraOffset_originalPos);

        if (CameraController.Instance.cameraState == CameraState.CeilingCam || CameraController.Instance.isCeilingRotating)
        {
            targetPos = transform.position + transform.TransformDirection(cameraController.cameraOffset_ceilingGrabPos);
        }

        Vector3 direction = (targetPos - transform.position).normalized;

        // Sphere cast along direction
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, sphereRadius, direction, desiredDistance);

        RaycastHit? closestRelevantHit = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            BlockInfo blockInfo = hit.collider.GetComponent<BlockInfo>();
            if (blockInfo == null) continue;

            // Confirm that the collider actually blocks the *line of sight* to the camera
            if (hit.collider.Raycast(new Ray(transform.position, direction), out RaycastHit centerHit, desiredDistance))
            {
                float d = centerHit.distance;
                if (d < closestDistance)
                {
                    closestDistance = d;
                    closestRelevantHit = centerHit;
                }
            }
        }

        // Final camera position
        Vector3 finalPosition;

        if (closestRelevantHit.HasValue)
        {
            float finalDistance = Mathf.Max(minDistance, closestRelevantHit.Value.distance - sphereRadius);
            finalPosition = transform.position + direction * finalDistance;

            // Apply height offset based on block type under player
            float heightOffset = camera_NormalHeight;
            if (Movement.Instance.blockStandingOn != null && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>())
            {
                var blockType = Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockType;
                if (blockType == BlockType.Stair || blockType == BlockType.Slope)
                    heightOffset = camera_StairHeight;
            }
            finalPosition.y += heightOffset;
        }
        else
        {
            // Nothing in the way → stay at full distance
            finalPosition = targetPos;
        }

        // Smooth movement
        cameraOffset.transform.position = Vector3.Lerp(cameraOffset.transform.position, finalPosition, transitionSpeed * Time.deltaTime);

        // Rotation
        float targetRotX = camera_RotationX_Offset_Normal - camera_RotationX_Offset_NormalOffset;
        cameraOffset.transform.localEulerAngles = Vector3.Lerp(
            cameraOffset.transform.localEulerAngles,
            new Vector3(targetRotX, 0, 0),
            transitionSpeed * Time.deltaTime
        );

        // Debug
        DrawSphereCast(transform.position, sphereRadius, direction, desiredDistance, Color.cyan, closestRelevantHit);
    }








    //--------------------

    // Debug visualization
    void DrawSphereCast(Vector3 origin, float radius, Vector3 direction, float maxDistance, Color color, RaycastHit? closestHit = null)
    {
        Debug.DrawLine(origin, origin + direction * maxDistance, color);
        DebugDrawWireSphere(origin, radius, color);

        if (closestHit.HasValue)
        {
            Debug.DrawLine(origin, closestHit.Value.point, Color.red);
            DebugDrawWireSphere(closestHit.Value.point, radius * 0.5f, Color.red);
        }
    }

    void DebugDrawWireSphere(Vector3 center, float radius, Color color)
    {
        int segments = 16;
        float angle = 360f / segments;

        Vector3 lastPoint = center + new Vector3(radius, 0, 0);
        Vector3 nextPoint;

        // XZ circle
        for (int i = 1; i <= segments; i++)
        {
            float rad = Mathf.Deg2Rad * angle * i;
            nextPoint = center + new Vector3(Mathf.Cos(rad) * radius, 0, Mathf.Sin(rad) * radius);
            Debug.DrawLine(lastPoint, nextPoint, color);
            lastPoint = nextPoint;
        }

        // XY circle
        lastPoint = center + new Vector3(radius, 0, 0);
        for (int i = 1; i <= segments; i++)
        {
            float rad = Mathf.Deg2Rad * angle * i;
            nextPoint = center + new Vector3(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius, 0);
            Debug.DrawLine(lastPoint, nextPoint, color);
            lastPoint = nextPoint;
        }

        // YZ circle
        lastPoint = center + new Vector3(0, radius, 0);
        for (int i = 1; i <= segments; i++)
        {
            float rad = Mathf.Deg2Rad * angle * i;
            nextPoint = center + new Vector3(0, Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius);
            Debug.DrawLine(lastPoint, nextPoint, color);
            lastPoint = nextPoint;
        }
    }
}
