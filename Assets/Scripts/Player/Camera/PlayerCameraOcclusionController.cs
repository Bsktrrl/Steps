using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineCamera))]
[RequireComponent(typeof(CinemachineThirdPersonFollow))]
public class PlayerCameraOcclusionController : Singleton<PlayerCameraOcclusionController>
{
    [Header("Targets")]
    public Transform followTarget;
    public Transform lookAtTarget;

    [Header("Obstruction Check")]
    public LayerMask obstructionLayers = ~0;

    public float wallBuffer = 0.25f;
    public float lerpSpeed = 10f;

    [Header("Far Rig (unobstructed)")]
    public float farCameraDistance = 4f;
    public Vector3 farShoulderOffset = new Vector3(0f, 0f, -0.5f);
    public float farVerticalArmLength = 2.77f;

    [Header("Near Rig (pushed in by walls)")]
    public float nearCameraDistance = 1.5f;
    public Vector3 nearShoulderOffset = new Vector3(0f, 1.65f, -0.2f);
    public float nearVerticalArmLength = -1.2f;

    [Header("Distance Limits")]
    public float minDistance = 0.01f;
    public float maxDistance = 4f;

    [Header("Camera Collision Resolve")]
    [Tooltip("Radius used when checking if the camera would clip into walls on the SIDES.\n" +
             "Think of this as how 'fat' the camera is.")]
    public float cameraCollisionRadius = 0.01f;

    [Tooltip("How much to push camera forward from the hit point so it doesn't sit exactly on the wall.")]
    public float cameraSurfaceOffset = 0.05f;

    [Header("Ceiling Adjustment")]
    [Tooltip("Distance above player to check for ceiling collisions.")]
    public float ceilingCheckDistance = 0.5f;
    [Tooltip("Y offset for near shoulder when ceiling detected.")]
    public float nearShoulderY_Ceiling = 1.5f;
    [Tooltip("Y offset for near shoulder when no ceiling above.")]
    public float nearShoulderY_Clear = 1.65f;

    [Header("Side Wall Adjustment")]
    [Tooltip("How far to check sideways for a wall next to the camera/player.")]
    public float wallCheckDistance = 0.55f; // e.g. half a block
    [Tooltip("Y offset to use (near) when a wall is hugging the camera side.")]
    public float nearShoulderY_Wall = 1.45f;
    [Tooltip("Y offset to use (near) when no side wall is there.")]
    public float nearShoulderY_NoWall = 1.65f;
    [Tooltip("Z offset (depth) for near shoulder when wall beside camera.")]
    public float nearShoulderZ_Wall = -0.03f;
    [Tooltip("Z offset (depth) for near shoulder when no wall beside camera.")]
    public float nearShoulderZ_NoWall = -0.2f;

    [Header("Debug")]
    public bool debugDraw = true;

    CinemachineCamera _cmCam;
    CinemachineThirdPersonFollow _tpf;

    float _currentDistance;

    [SerializeField] bool effect_isDisabled;


    //--------------------


    void Awake()
    {
        _cmCam = GetComponent<CinemachineCamera>();
        _tpf = GetComponent<CinemachineThirdPersonFollow>();

        if (followTarget != null)
        {
            _cmCam.Follow = followTarget;
        }

        if (lookAtTarget != null)
        {
            _cmCam.LookAt = lookAtTarget;
        }

        _currentDistance = farCameraDistance;
        ApplyRigInstant(_currentDistance);
    }

    void LateUpdate()
    {
        if (followTarget == null || _tpf == null || effect_isDisabled)
            return;

        // Cache the "design-time" near shoulder for this frame
        // BEFORE we start mutating it
        Vector3 baseNearShoulder = nearShoulderOffset;

        // Predict where the near camera would sit using the BASE values,
        // so our wall check isn't polluted by last frame's adjustments.
        Vector3 predictedNearCamPosWorld = GetCameraWorldPosFromRig(
            followTarget,
            baseNearShoulder,
            nearVerticalArmLength,
            nearCameraDistance
        );

        // --- Environment awareness ---

        bool hasCeiling = Physics.Raycast(
            followTarget.position,
            Vector3.up,
            ceilingCheckDistance,
            obstructionLayers,
            QueryTriggerInteraction.Ignore
        );

        bool hasSideWall = HasSideWall(predictedNearCamPosWorld);

        // --- Choose shoulder offset for this frame ---

        // We'll compute the final y and z we want for nearShoulderOffset.
        float finalY;
        float finalZ;

        if (hasCeiling && hasSideWall)
        {
            // Both tight: pick the lowest Y of the two,
            // but always use wall Z because wall is what causes the sideways clip.
            float yFromCeiling = nearShoulderY_Ceiling;
            float yFromWall = nearShoulderY_Wall;

            finalY = (yFromCeiling < yFromWall) ? yFromCeiling : yFromWall;
            finalZ = nearShoulderZ_Wall;
        }
        else if (hasCeiling)
        {
            // Only ceiling overhead
            finalY = nearShoulderY_Ceiling;
            // When it's just ceiling, use the "no wall" depth,
            // so Z doesn't suddenly pop.
            finalZ = nearShoulderZ_NoWall;
        }
        else if (hasSideWall)
        {
            // Only wall alongside/behind camera
            finalY = nearShoulderY_Wall;
            finalZ = nearShoulderZ_Wall;
        }
        else
        {
            // Nothing nearby, open space
            finalY = nearShoulderY_Clear;
            finalZ = nearShoulderZ_NoWall;
        }

        // Apply those results to a working copy that we'll blend with far rig
        Vector3 adjustedNearShoulder = baseNearShoulder;
        adjustedNearShoulder.y = finalY;
        adjustedNearShoulder.z = finalZ;

        if (debugDraw)
        {
            // Red = ceiling detected, Green = clear above
            Debug.DrawRay(
                followTarget.position,
                Vector3.up * ceilingCheckDistance,
                hasCeiling ? Color.red : Color.green
            );
        }

        // --- Push through distance / blend pipeline ---

        // 1. How far back are we allowed (blocked from target to camera)?
        float allowedDistanceLOS = ComputeAllowedDistance();

        // 2. Smooth toward that LOS distance
        _currentDistance = Mathf.Lerp(
            _currentDistance,
            allowedDistanceLOS,
            1f - Mathf.Exp(-lerpSpeed * Time.deltaTime)
        );

        _currentDistance = Mathf.Clamp(_currentDistance, minDistance, maxDistance);

        // 3. Blend near/far rig values based on that current distance
        float t = Mathf.InverseLerp(nearCameraDistance, farCameraDistance, _currentDistance);
        t = Mathf.Clamp01(t);

        float blendedDistance = Mathf.Lerp(nearCameraDistance, farCameraDistance, t);
        Vector3 blendedShoulder = Vector3.Lerp(adjustedNearShoulder, farShoulderOffset, t);
        float blendedArm = Mathf.Lerp(nearVerticalArmLength, farVerticalArmLength, t);

        // 4. Resolve collision against actual world (keeps camera from being halfway inside walls)
        blendedDistance = ResolveSideCollision(
            blendedDistance,
            blendedShoulder,
            blendedArm
        );

        // 5. Push final values to Cinemachine, with smoothing
        _tpf.CameraDistance = Mathf.Lerp(
            _tpf.CameraDistance,
            blendedDistance,
            1f - Mathf.Exp(-lerpSpeed * Time.deltaTime)
        );

        _tpf.ShoulderOffset = Vector3.Lerp(
            _tpf.ShoulderOffset,
            blendedShoulder,
            1f - Mathf.Exp(-lerpSpeed * Time.deltaTime)
        );

        _tpf.VerticalArmLength = Mathf.Lerp(
            _tpf.VerticalArmLength,
            blendedArm,
            1f - Mathf.Exp(-lerpSpeed * Time.deltaTime)
        );
    }



    //--------------------

    bool HasSideWall(Vector3 desiredCamPos)
    {
        // Cast from player toward the camera direction, horizontal only,
        // to detect a wall hugging the camera side.
        Vector3 origin = followTarget.position /*+ Vector3.up * 1.0f*/;

        Vector3 dir = desiredCamPos - origin;

        // We only care about horizontal blocking walls here,
        // not floor/ceiling, so zero out Y.
        dir.y = 0f;

        float dist = dir.magnitude;
        if (dist < 0.0001f)
            return false;

        dir /= dist;

        float checkDist = Mathf.Min(dist, wallCheckDistance);

        bool hit = Physics.Raycast(origin, dir, checkDist, obstructionLayers, QueryTriggerInteraction.Ignore);

        if (debugDraw)
        {
            Debug.DrawRay(origin, dir * checkDist, hit ? Color.magenta : Color.cyan);
        }

        return hit;
    }

    Vector3 GetCameraWorldPosFromRig(Transform pivot, Vector3 shoulderOffset, float armLen, float camDistance)
    {
        Vector3 targetPos = pivot.position;
        Quaternion targetRot = pivot.rotation;

        // Shoulder point relative to player
        Vector3 worldShoulderPos = targetPos + targetRot * shoulderOffset;

        // Hand point above shoulder
        Vector3 worldHandPos = worldShoulderPos + Vector3.up * armLen;

        // Camera sits "behind" the player along -pivot.forward
        Vector3 camPos = worldHandPos - (pivot.forward * camDistance);

        return camPos;
    }

    float ComputeAllowedDistance()
    {
        Transform pivot = followTarget;

        Vector3 targetPos = pivot.position;
        Quaternion targetRot = pivot.rotation;

        // Rebuild the FAR rig position guess
        Vector3 worldShoulderPos = targetPos + targetRot * farShoulderOffset;
        Vector3 worldHandPos = worldShoulderPos + Vector3.up * farVerticalArmLength;
        Vector3 desiredCamPos = worldHandPos - (pivot.forward * farCameraDistance);

        // Cast from targetPos to desiredCamPos
        Vector3 rayDir = desiredCamPos - targetPos;
        float rayLen = rayDir.magnitude;
        if (rayLen < 0.0001f)
            return nearCameraDistance;

        rayDir /= rayLen;

        if (Physics.Raycast(
            targetPos,
            rayDir,
            out RaycastHit hit,
            rayLen,
            obstructionLayers,
            QueryTriggerInteraction.Ignore))
        {
            // found something between player and cam
            float blockedDist = hit.distance - wallBuffer;
            blockedDist = Mathf.Clamp(blockedDist, minDistance, farCameraDistance);

            // Project along -pivot.forward to approximate what that means
            Vector3 point = targetPos + rayDir * (hit.distance - wallBuffer);
            // We want how far this point sits "behind the hand" along -forward
            Vector3 worldShoulderPosNear = targetPos + targetRot * nearShoulderOffset;
            Vector3 worldHandPosNear = worldShoulderPosNear + Vector3.up * nearVerticalArmLength;

            float usableDist = Vector3.Dot(point - worldHandPosNear, -pivot.forward);
            usableDist = Mathf.Abs(usableDist);

            return Mathf.Clamp(usableDist, minDistance, farCameraDistance);
        }

        return farCameraDistance;
    }

    float ResolveSideCollision(float desiredDistance, Vector3 shoulderOffset, float armLen)
    {
        Transform pivot = followTarget;

        Vector3 targetPos = pivot.position;
        Quaternion targetRot = pivot.rotation;

        // Reconstruct the rig in world space using the *blended* values
        Vector3 worldShoulderPos = targetPos + targetRot * shoulderOffset;
        Vector3 worldHandPos = worldShoulderPos + Vector3.up * armLen;
        Vector3 desiredCamPos = worldHandPos - (pivot.forward * desiredDistance);

        // Direction from target toward camera
        Vector3 dir = desiredCamPos - targetPos;
        float dist = dir.magnitude;
        if (dist < 0.0001f)
            return desiredDistance;

        dir /= dist;

        // SphereCast to catch side walls
        if (Physics.SphereCast(
            targetPos,
            cameraCollisionRadius,
            dir,
            out RaycastHit hit,
            dist,
            obstructionLayers,
            QueryTriggerInteraction.Ignore))
        {
            // We hit something before we got all the way to desiredCamPos.
            // We "stop" the camera a bit in front of that hit point.
            float allowed = hit.distance - cameraSurfaceOffset;
            allowed = Mathf.Clamp(allowed, minDistance, desiredDistance);
            return allowed;
        }

        // No side collision, keep desired distance
        return desiredDistance;
    }

    void ApplyRigInstant(float dist)
    {
        _tpf.CameraDistance = dist;
        _tpf.ShoulderOffset = farShoulderOffset;
        _tpf.VerticalArmLength = farVerticalArmLength;
    }

    void OnDrawGizmosSelected()
    {
        if (!debugDraw || followTarget == null) return;

        Transform pivot = followTarget;
        Vector3 targetPos = pivot.position;
        Quaternion targetRot = pivot.rotation;

        // visualize FAR guess
        Vector3 worldShoulderPos = targetPos + targetRot * farShoulderOffset;
        Vector3 worldHandPos = worldShoulderPos + Vector3.up * farVerticalArmLength;
        Vector3 desiredCamPos = worldHandPos - (pivot.forward * farCameraDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(targetPos, desiredCamPos);
        Gizmos.DrawSphere(desiredCamPos, 0.05f);

        // visualize CURRENT runtime position guess if playing
        if (Application.isPlaying && _tpf != null)
        {
            Vector3 curShoulder = targetPos + targetRot * _tpf.ShoulderOffset;
            Vector3 curHand = curShoulder + Vector3.up * _tpf.VerticalArmLength;
            Vector3 curCamPos = curHand - (pivot.forward * _tpf.CameraDistance);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(targetPos, curCamPos);
            Gizmos.DrawSphere(curCamPos, 0.05f);

            // draw collision radius at cam pos
            Gizmos.color = new Color(1f, 1f, 0f, 0.25f);
            Gizmos.DrawWireSphere(curCamPos, cameraCollisionRadius);
        }
    }



    //--------------------


    public void CameraZoom(bool value)
    {
        effect_isDisabled = value;
    }
}
