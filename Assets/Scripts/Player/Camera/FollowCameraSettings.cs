using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineCamera))]
public class FollowCameraSettings : MonoBehaviour
{
    [Header("CailingGrabCamera")]
    [SerializeField] bool isCeilingGrabCamera;

    [Header("Targets & Layers")]
    public Transform followTarget;                 // Camera Anchor
    public LayerMask collideAgainst;               // Walls / environment layers

    [Tooltip("Small radius = friendly on stairs")]
    public float smallCastRadius = 0.15f;

    [Tooltip("Large radius = prevents wall clipping at max distance")]
    public float largeCastRadius = 0.3f;

    [Tooltip("Extra gap between camera and any hit geometry")]
    public float wallBuffer = 0.2f;

    [Header("Desired Ranges")]
    public float minDistance = 0.4f;
    public float maxDistance = 4f;
    public float minArmLength = 0.9f;
    public float maxArmLength = 2.77f;

    [Header("Smoothing")]
    public float distanceSmoothTime = 0.10f;
    public float armSmoothTime = 0.00f;

    [Header("Wall/Step Discrimination")]
    [Tooltip("If hit.normal.y is below this, treat as a wall (near-vertical). 0.35 ≈ surfaces steeper than ~69°.")]
    [Range(0f, 1f)] public float wallNormalYMax = 0.35f;

    // --- internal ---
    CinemachineThirdPersonFollow tpf;
    CinemachineCamera vcam;
    float distanceVel, armVel;


    //--------------------


    void Awake()
    {
        vcam = GetComponent<CinemachineCamera>();
        tpf = GetComponent<CinemachineThirdPersonFollow>();

        if (!tpf) Debug.LogError("FollowCameraSettings: needs CinemachineThirdPersonFollow on the same object.");
        if (!followTarget) followTarget = vcam.Follow; // fallback
    }


    //--------------------


    void LateUpdate()
    {
        CameraMotion();

        RadiusGizmo();
    }


    //--------------------


    void CameraMotion()
    {
        if (!tpf || !followTarget) return;

        Vector3 origin = followTarget.position;
        Vector3 toCam = (transform.position - origin);
        Vector3 dir = toCam.sqrMagnitude > 1e-6f ? toCam.normalized : -followTarget.forward;

        float targetDistance = maxDistance;

        //Change CastRadius on Staris to prevent lagging
        if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>() &&
            (Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Stair || Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Slope))
        {
            smallCastRadius = 0.15f;
        }
        else
        {
            if (isCeilingGrabCamera)
            {
                smallCastRadius = 0.2f;
            }
            else
            {
                smallCastRadius = 0.4f;
            }
        }

        // --- PASS 1: small radius (stair-friendly) ---
        if (Physics.SphereCast(origin, smallCastRadius, dir, out RaycastHit smallHit, maxDistance, collideAgainst, QueryTriggerInteraction.Ignore))
        {
            float d = Mathf.Max(smallHit.distance - wallBuffer, minDistance);
            targetDistance = Mathf.Clamp(d, minDistance, maxDistance);
        }

        // --- PASS 2: wall-guard with large radius on near-vertical surfaces only ---
        // Only bother if we were going to sit quite far (where small radius can slip into walls)
        float previewDistance = targetDistance;
        if (previewDistance > maxDistance * 0.9f) // only when close to 4.0
        {
            if (Physics.SphereCast(origin, largeCastRadius, dir, out RaycastHit bigHit, maxDistance, collideAgainst, QueryTriggerInteraction.Ignore))
            {
                // Treat as WALL only if surface is near-vertical (ignore stairs/floors)
                if (bigHit.normal.y <= wallNormalYMax)
                {
                    float d = Mathf.Max(bigHit.distance - wallBuffer, minDistance);
                    previewDistance = Mathf.Clamp(d, minDistance, maxDistance);
                }
            }
        }

        // Choose the stricter (closer) of the two distances
        targetDistance = Mathf.Min(targetDistance, previewDistance);

        // Smoothly move the Third Person Follow distance
        float newDist = Mathf.SmoothDamp(tpf.CameraDistance, targetDistance, ref distanceVel, distanceSmoothTime);
        tpf.CameraDistance = newDist;

        // Map arm length to distance (keeps your "angle" feel)
        float t = Mathf.InverseLerp(minDistance, maxDistance, newDist);
        float targetArm = Mathf.Lerp(minArmLength, maxArmLength, t);
        float newArm = (armSmoothTime > 0f)
            ? Mathf.SmoothDamp(tpf.VerticalArmLength, targetArm, ref armVel, armSmoothTime)
            : targetArm;
        tpf.VerticalArmLength = newArm;
    }

    void RadiusGizmo()
    {
        #if UNITY_EDITOR
        // Optional gizmo to visualize the two radii from the anchor
        void OnDrawGizmosSelected()
        {
            if (!followTarget) return;
            Vector3 origin = followTarget.position;
            Vector3 toCam = (transform.position - origin);
            if (toCam.sqrMagnitude < 1e-6f) return;
            Vector3 dir = toCam.normalized;

            Gizmos.color = new Color(0f, 0.8f, 1f, 0.25f);
            Gizmos.DrawWireSphere(origin + dir * Mathf.Min(maxDistance, 0.5f), smallCastRadius);

            Gizmos.color = new Color(1f, 0.4f, 0f, 0.20f);
            Gizmos.DrawWireSphere(origin + dir * Mathf.Min(maxDistance, 0.5f), largeCastRadius);
        }
        #endif
    }
}
