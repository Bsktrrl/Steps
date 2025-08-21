using UnityEngine;
using Cinemachine;

public class CameraArmCinemachine2 : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] CinemachineVirtualCamera vcam;
    [SerializeField] Transform cameraAnchor;   // pivot you rotate

    [Header("Distance Settings")]
    [SerializeField] float desiredDistance = 5f;
    [SerializeField] float minDistance = 0.5f;

    [Header("Offsets")]
    [SerializeField] float camera_ForwardOffset = -0.2f;
    [SerializeField] float camera_NormalHeight = 0.2f;
    [SerializeField] float camera_StairHeight = 0.43f;

    [Header("Rotation")]
    [SerializeField] float rotX_Normal = 29f;
    [SerializeField] float rotX_Close = 17f;
    [SerializeField] float rotOffset = 0f;

    [Header("Smoothing")]
    [SerializeField] float distanceSmoothSpeed = 6f;
    [SerializeField] float maxDistanceChangePerFrame = 0.3f;

    float currentDistance;
    CinemachineFramingTransposer transposer;
    CinemachineCollider colliderExt;

    void Awake()
    {
        transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        colliderExt = vcam.GetComponent<CinemachineCollider>();

        if (colliderExt != null)
        {
            colliderExt.m_AvoidObstacles = true;
            colliderExt.m_CameraRadius = 0.21f;
            colliderExt.m_MinimumDistanceFromTarget = minDistance;
            colliderExt.m_Damping = 0.2f;   // single float in your version
        }

        if (currentDistance <= 0f) currentDistance = desiredDistance;
        if (transposer != null) transposer.m_CameraDistance = currentDistance;
    }


    void LateUpdate()
    {
        if (transposer == null) return;

        // --- pick height (stairs vs normal) ---
        float height = camera_NormalHeight;
        if (Movement.Instance.blockStandingOn != null)
        {
            var b = Movement.Instance.blockStandingOn.GetComponent<BlockInfo>();
            if (b != null && (b.blockType == BlockType.Stair || b.blockType == BlockType.Slope))
                height = camera_StairHeight;
        }

        // --- blend pitch based on distance ---
        float closeBlendStart = 0.8f;
        float t = Mathf.InverseLerp(minDistance, closeBlendStart, currentDistance);
        float blendedRotX = Mathf.Lerp(rotX_Close, rotX_Normal, t);
        float targetRotX = blendedRotX - rotOffset;

        // Rotate the anchor (Cinemachine will follow)
        Vector3 euler = cameraAnchor.localEulerAngles;
        euler.x = Mathf.LerpAngle(euler.x, targetRotX, Time.deltaTime * 10f);
        cameraAnchor.localEulerAngles = euler;

        // --- smooth zoom distance ---
        float idealDistance = desiredDistance;
        float rawSmoothed = Mathf.Lerp(currentDistance, idealDistance, Time.deltaTime * distanceSmoothSpeed);
        currentDistance = Mathf.MoveTowards(currentDistance, rawSmoothed, maxDistanceChangePerFrame);

        transposer.m_CameraDistance = currentDistance;

        // --- apply offsets (y = up, z = forward) ---
        Vector3 followOffset = transposer.m_TrackedObjectOffset;
        followOffset.y = height;
        followOffset.z = camera_ForwardOffset;
        transposer.m_TrackedObjectOffset = followOffset;
    }
}
