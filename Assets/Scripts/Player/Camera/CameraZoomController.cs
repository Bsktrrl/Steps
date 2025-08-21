using UnityEngine;
using Cinemachine;

public class CameraZoomController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vcam;
    [SerializeField] Transform cameraAnchor;

    [Header("Zoom Settings")]
    public float maxDistance = -5f;      // furthest (default = -5 in offset)
    public float minDistance = -2.0f;    // closest
    public float zoomSmooth = 6f;

    [Header("Pitch Settings")]
    public float maxPitch = 29f;         // when far
    public float minPitch = 17f;         // when close
    public float pitchSmooth = 6f;

    private CinemachineTransposer transposer;
    private float currentZ;

    void Start()
    {
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        currentZ = maxDistance;
    }

    void LateUpdate()
    {
        // Example: here you’d put your spherecast/raycast check for obstacles.
        // For now I’ll fake it with a simple line-of-sight distance.
        float targetZ = maxDistance;

        RaycastHit hit;
        Vector3 origin = cameraAnchor.position;
        Vector3 back = -cameraAnchor.forward; // looking backwards from anchor
        float checkDist = Mathf.Abs(maxDistance);

        if (Physics.SphereCast(origin, 0.2f, back, out hit, checkDist))
        {
            targetZ = Mathf.Max(minDistance, -hit.distance + 0.2f);
        }

        // Smooth Z distance
        currentZ = Mathf.Lerp(currentZ, targetZ, Time.unscaledDeltaTime * zoomSmooth);

        // Apply to transposer offset
        Vector3 offset = transposer.m_FollowOffset;
        offset.z = currentZ;
        transposer.m_FollowOffset = offset;

        // Smooth pitch blend
        float t = Mathf.InverseLerp(minDistance, maxDistance, currentZ);
        float targetPitch = Mathf.Lerp(minPitch, maxPitch, t);

        Vector3 euler = cameraAnchor.localEulerAngles;
        euler.x = Mathf.LerpAngle(euler.x, targetPitch, Time.unscaledDeltaTime * pitchSmooth);
        cameraAnchor.localEulerAngles = euler;
    }
}
