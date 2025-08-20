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
    float desiredDistance = 3f;   // shorter ray
    float minDistance = 0.5f;     // fixed nearest point

    [Header("Ray Tilt")]
    float rayTiltAngle = 15f; // upward tilt to avoid stairs

    void Update()
    {
        // Determine target camera position
        Vector3 targetPos = transform.position + transform.TransformDirection(cameraController.cameraOffset_originalPos);

        if (CameraController.Instance.cameraState == CameraState.CeilingCam || CameraController.Instance.isCeilingRotating)
        {
            targetPos = transform.position + transform.TransformDirection(cameraController.cameraOffset_ceilingGrabPos);
        }

        Vector3 direction = (targetPos - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPos);

        // SphereCast along forward direction
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, sphereRadius, direction, distance);

        RaycastHit? closestBlockHit = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            BlockInfo blockInfo = hit.collider.GetComponent<BlockInfo>();
            if (blockInfo != null)
            {
                // Only accept hits roughly in front using sphere radius instead of angle
                Vector3 hitDir = (hit.point - transform.position);
                float lateralDistance = Vector3.Cross(direction, hitDir).magnitude;
                if (lateralDistance <= sphereRadius)
                {
                    float d = hitDir.magnitude;
                    if (d < closestDistance)
                    {
                        closestDistance = d;
                        closestBlockHit = hit;
                    }
                }
            }
        }

        // Determine final camera position
        Vector3 finalPosition = targetPos;

        if (closestBlockHit.HasValue)
        {
            float finalDistance = Mathf.Max(minDistance, closestDistance - sphereRadius);
            finalPosition = transform.position + direction * finalDistance;

            // Apply height offsets based on the blocking object itself
            var blockInfo = closestBlockHit.Value.collider.GetComponent<BlockInfo>();
            float heightOffset = camera_NormalHeight;
            if (blockInfo.blockType == BlockType.Stair || blockInfo.blockType == BlockType.Slope)
                heightOffset = camera_StairHeight;

            finalPosition.y += heightOffset;
        }

        // Smooth camera movement
        cameraOffset.transform.position = Vector3.Lerp(cameraOffset.transform.position, finalPosition, transitionSpeed * Time.deltaTime);

        // Smooth camera rotation
        float targetRotX = camera_RotationX_Offset_Normal - camera_RotationX_Offset_NormalOffset;
        cameraOffset.transform.localEulerAngles = Vector3.Lerp(cameraOffset.transform.localEulerAngles, new Vector3(targetRotX, 0, 0), transitionSpeed * Time.deltaTime);

        // Debug: cyan = full path, red = up to blocking object
        Debug.DrawLine(transform.position, transform.position + direction * distance, Color.cyan);
        if (closestBlockHit.HasValue)
        {
            Debug.DrawLine(transform.position, transform.position + direction * closestDistance, Color.red);
        }
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
