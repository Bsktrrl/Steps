using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArm : MonoBehaviour
{
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject cameraOffset;

    [Header("Camera Heights")]
    [SerializeField] float camera_NormalHeight = 0.2f;
    [SerializeField] float camera_StairHeight = 0.43f;

    [Header("Rotation Settings")]
    [SerializeField] float camera_RotationX_Offset_Normal = 29f;
    [SerializeField] float camera_RotationX_Offset_Close = 17f; // when camera is pushed very close
    [SerializeField] float camera_RotationX_Offset_NormalOffset = 0f;
    [SerializeField] float camera_RotationX_Offset_CeilingGrab = -17f;

    [Header("Smoothing")]
    [SerializeField] float transitionSpeed = 10f;       // smoothing for camera position & rotation
    [SerializeField] float distanceSmoothSpeed = 6f;    // smoothing just for distance changes
    [SerializeField] float maxDistanceChangePerFrame = 0.3f; // clamp distance changes

    [Header("Camera Distance")]
    [SerializeField] float sphereRadius = 0.21f;
    [SerializeField] float desiredDistance = 5f;
    [SerializeField] float minDistance = 0.5f;

    [Header("Offsets")]
    [SerializeField] float camera_ForwardOffset = -0.2f;
    [SerializeField] float camera_UpOffset = 0.05f;

    [Header("Ray Tilt")]
    [SerializeField] float rayTiltAngle_Normal = 5f;
    [SerializeField] float rayTiltAngle_Stair = 35f;

    // runtime state
    float currentDistance;

    void Update()
    {
        Vector3 playerPos = transform.position;

        // Target camera position at full distance
        Vector3 targetLocal = cameraController.cameraOffset_originalPos;
        if (CameraController.Instance.cameraState == CameraState.CeilingCam || CameraController.Instance.isCeilingRotating)
            targetLocal = cameraController.cameraOffset_ceilingGrabPos;

        Vector3 targetPos = playerPos + transform.TransformDirection(targetLocal);

        // Direction (line of sight)
        Vector3 dir = (targetPos - playerPos).normalized;
        float fullDistance = Vector3.Distance(playerPos, targetPos);

        // Pick ray tilt angle depending on ground type
        float chosenTiltAngle = rayTiltAngle_Normal;
        if (Movement.Instance.blockStandingOn != null)
        {
            var b = Movement.Instance.blockStandingOn.GetComponent<BlockInfo>();
            if (b != null && (b.blockType == BlockType.Stair || b.blockType == BlockType.Slope))
                chosenTiltAngle = rayTiltAngle_Stair;
        }

        // Tilted cast direction
        Vector3 castDir = Quaternion.AngleAxis(chosenTiltAngle, transform.right) * dir;

        // Spherecast
        RaycastHit hit;
        bool hasHit = Physics.SphereCast(
            playerPos,
            sphereRadius,
            castDir,
            out hit,
            fullDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore
        );

        float safetyOffset = 0.15f;
        float targetDistance = fullDistance;

        if (hasHit)
        {
            BlockInfo blockInfo = hit.collider.GetComponent<BlockInfo>();
            if (blockInfo != null)
            {
                float distanceAlongDir = Mathf.Max(0f, Vector3.Dot(hit.point - playerPos, dir));
                targetDistance = Mathf.Max(minDistance, distanceAlongDir - sphereRadius - safetyOffset);
            }
        }

        // --- Smooth the distance separately with a clamp ---
        if (currentDistance <= 0f) currentDistance = fullDistance; // init

        float rawSmoothed = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * distanceSmoothSpeed);

        // Clamp max change per frame
        float maxStep = maxDistanceChangePerFrame;
        currentDistance = Mathf.MoveTowards(currentDistance, rawSmoothed, maxStep);

        // Compute final camera position
        Vector3 finalPos = playerPos + dir * currentDistance;

        // Apply height offset (stairs or normal)
        float heightOffset = camera_NormalHeight;
        if (Movement.Instance.blockStandingOn != null)
        {
            var b = Movement.Instance.blockStandingOn.GetComponent<BlockInfo>();
            if (b != null && (b.blockType == BlockType.Stair || b.blockType == BlockType.Slope))
                heightOffset = camera_StairHeight;
        }
        finalPos.y += heightOffset;

        // Apply tweakable offsets
        finalPos += dir * camera_ForwardOffset;
        finalPos += Vector3.up * camera_UpOffset;

        // Smooth movement of the cameraOffset
        cameraOffset.transform.position = Vector3.Lerp(
            cameraOffset.transform.position,
            finalPos,
            transitionSpeed * Time.deltaTime
        );

        // -------- Rotation with "close camera" blending --------
        float closeBlendStart = 0.8f;
        float t = Mathf.InverseLerp(minDistance, closeBlendStart, currentDistance);
        float blendedRotX = Mathf.Lerp(camera_RotationX_Offset_Close, camera_RotationX_Offset_Normal, t);
        float targetRotX = blendedRotX - camera_RotationX_Offset_NormalOffset;

        cameraOffset.transform.localEulerAngles = Vector3.Lerp(
            cameraOffset.transform.localEulerAngles,
            new Vector3(targetRotX, 0, 0),
            transitionSpeed * Time.deltaTime
        );

        // Debug
        DrawSphereCast(playerPos, sphereRadius, castDir, fullDistance, Color.cyan, hasHit ? hit : (RaycastHit?)null);
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
