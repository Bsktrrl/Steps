using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineCamera))]
public class FollowCameraSettings : MonoBehaviour
{
    [Header("Targets & Layers")]
    public Transform followTarget;                 // Your Camera Anchor
    public LayerMask collideAgainst;               // Walls / environment layers
    public float castRadius = 0.30f;               // Match your camera radius feel
    public float wallBuffer = 0.20f;               // Keep a little gap from walls

    [Header("Desired Ranges")]
    public float minDistance = 2.0f;               // Close
    public float maxDistance = 4.0f;               // Far
    public float minArmLength = 0.45f;             // Your "angle 0.45"
    public float maxArmLength = 2.77f;             // Your "angle 2.77"

    [Header("Smoothing")]
    public float distanceSmoothTime = 0.20f;       // Seconds to ease
    public float armSmoothTime = 0.20f;

    // --- internal ---
    CinemachineThirdPersonFollow tpf;
    CinemachineCamera vcam;
    float distanceVel, armVel;

    void Awake()
    {
        vcam = GetComponent<CinemachineCamera>();
        tpf = GetComponent<CinemachineThirdPersonFollow>();
        if (tpf == null)
            Debug.LogError("AdaptiveThirdPersonFollow: needs CinemachineThirdPersonFollow on the vcam.");
        if (followTarget == null)
            followTarget = vcam.Follow; // fallback if you forgot to assign it
    }

    void LateUpdate()
    {
        if (tpf == null || followTarget == null) return;

        // Direction from target to current camera pos (where we want to stay if clear)
        Vector3 origin = followTarget.position;
        Vector3 toCam = (transform.position - origin);
        Vector3 dir = toCam.sqrMagnitude > 1e-6f ? toCam.normalized : -followTarget.forward;

        // Spherecast to see how much room we have behind the target
        float targetDistance = maxDistance;
        if (Physics.SphereCast(origin, castRadius, dir, out RaycastHit hit, maxDistance, collideAgainst, QueryTriggerInteraction.Ignore))
        {
            // desired distance is "just before the wall", clamped to our range
            float d = Mathf.Max(hit.distance - wallBuffer, minDistance);
            targetDistance = Mathf.Clamp(d, minDistance, maxDistance);
        }

        // Smoothly move the Third Person Follow distance
        float newDist = Mathf.SmoothDamp(tpf.CameraDistance, targetDistance, ref distanceVel, distanceSmoothTime);
        tpf.CameraDistance = newDist;

        // Optionally map arm length to distance (feel free to comment out if you prefer a fixed arm)
        float t = Mathf.InverseLerp(minDistance, maxDistance, newDist);
        float targetArm = Mathf.Lerp(minArmLength, maxArmLength, t);
        float newArm = Mathf.SmoothDamp(tpf.VerticalArmLength, targetArm, ref armVel, armSmoothTime);
        tpf.VerticalArmLength = newArm;
    }
}
