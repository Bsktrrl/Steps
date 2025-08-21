using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class DynamicCameraHeightByDistance : MonoBehaviour
{
    [Header("Refs")]
    public Transform followAnchor;    // usually CameraPivot

    [Header("Distances (positive, in units)")]
    public float farDistance = 4f;
    public float nearDistance = 2f;

    [Header("Tilt Heights for Transposer (affects camera angle)")]
    public float farY = 2.77f;   // ≈29° at far
    public float nearY = 1.20f;  // ≈17° at near

    [Header("Pivot lift (moves the pivot object itself)")]
    public float farPivotY = 0f;    // baseline pivot height
    public float nearPivotY = 0.3f; // raise pivot when close

    [Header("Smoothing")]
    public float smooth = 15f;

    CinemachineVirtualCamera vcam;
    CinemachineTransposer transposer;
    Camera outputCam;

    void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        if (!followAnchor && vcam.Follow != null)
            followAnchor = vcam.Follow;
        outputCam = Camera.main;
    }

    void LateUpdate()
    {
        if (!transposer || !followAnchor || !outputCam) return;

        // world-space distance between camera and pivot
        float backDist = Vector3.Distance(outputCam.transform.position, followAnchor.position);
        float d = Mathf.Clamp(backDist, nearDistance, farDistance);

        // blend factor
        float t = Mathf.InverseLerp(nearDistance, farDistance, d);

        // ---- CAMERA OFFSET (tilt + distance) ----
        float targetZ = -Mathf.Lerp(nearDistance, farDistance, t);
        float targetY = Mathf.Lerp(nearY, farY, t);

        var off = transposer.m_FollowOffset;
        off.z = Mathf.Lerp(off.z, targetZ, Time.unscaledDeltaTime * smooth);
        off.y = Mathf.Lerp(off.y, targetY, Time.unscaledDeltaTime * smooth);
        transposer.m_FollowOffset = off;

        // ---- PIVOT LIFT (moves the object itself) ----
        float targetPivotY = Mathf.Lerp(nearPivotY, farPivotY, t);
        Vector3 pivotPos = followAnchor.localPosition;
        pivotPos.y = Mathf.Lerp(pivotPos.y, targetPivotY, Time.unscaledDeltaTime * smooth);
        followAnchor.localPosition = pivotPos;
    }

}
