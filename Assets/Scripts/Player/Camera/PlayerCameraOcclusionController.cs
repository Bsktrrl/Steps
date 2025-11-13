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

    [Header("Distance Limits (both modes)")]
    public float minDistance = 0.01f;
    public float maxDistance = 4f;

    [Header("Camera Collision Resolve (both modes)")]
    [Tooltip("Radius used when checking if the camera would clip into walls on the SIDES.\nThink of this as how 'fat' the camera is.")]
    public float cameraCollisionRadius = 0.01f;
    [Tooltip("How much to push camera forward from the hit point so it doesn't sit exactly on the wall.")]
    public float cameraSurfaceOffset = 0.05f;

    [Header("DEBUG")]
    public bool debugDraw = true;

    [SerializeField] bool effect_isDisabled;

    CinemachineCamera _cmCam;
    CinemachineThirdPersonFollow _tpf;
    float _currentDistance;

    [SerializeField, Range(0f, 1f)]
    float _modeBlend = 0f;

    public float modeSwitchLerpSpeed = 3f;

    [Header("NORMAL CAMERA SETTINGS")]
    public RigSettings normalRig = new RigSettings();

    [Header("CEILING GRAB CAMERA SETTINGS")]
    public RigSettings ceilingGrabRig = new RigSettings();


    //--------------------


    void Awake()
    {
        _cmCam = GetComponent<CinemachineCamera>();
        _tpf = GetComponent<CinemachineThirdPersonFollow>();

        if (followTarget != null)
            _cmCam.Follow = followTarget;

        if (lookAtTarget != null)
            _cmCam.LookAt = lookAtTarget;

        // start using the normal far distance so there's no pop on frame 1
        _currentDistance = normalRig.farCameraDistance;

        SetupNormalCameraValues();
        SetupCeilingGrabCameraValues();

        ApplyRigInstant(normalRig);
    }

    void LateUpdate()
    {
        if (followTarget == null || _tpf == null || effect_isDisabled || CameraController.Instance.isIgnoringObstaclesWhenRotating)
            return;

        bool ceilingGrabActive = Player_CeilingGrab.Instance != null && Player_CeilingGrab.Instance.isCeilingGrabbing;

        // 1. Smooth the mode blend (0 normal → 1 ceilingGrab)
        float targetBlend = ceilingGrabActive ? 1f : 0f;

        _modeBlend = Mathf.Lerp(_modeBlend, targetBlend, 1f - Mathf.Exp(-modeSwitchLerpSpeed * Time.deltaTime));

        RigSettings blendedRig = BuildBlendedRigForFrame(_modeBlend);

        bool useCeilingGrabRules = (_modeBlend >= 0.5f);

        EvaluateRigAndApply(blendedRig, useCeilingGrabRules);
    }



    //-------------------------


    void SetupNormalCameraValues()
    {
        normalRig.farCameraDistance = 4f;
        normalRig.farShoulderOffset = new Vector3(0f, 0f, -0.5f);
        normalRig.farVerticalArmLength = 2.77f;

        normalRig.nearCameraDistance = 1.5f;
        normalRig.nearShoulderOffset = new Vector3(0f, 1.65f, -0.2f);
        normalRig.nearVerticalArmLength = -1.2f;

        normalRig.ceilingCheckDistance = 0.5f;
        normalRig.wallCheckDistance = 0.55f;

        normalRig.nearShoulderY_Ceiling = 1.5f; 
        normalRig.nearShoulderY_Clear = 1.45f; 

        normalRig.nearShoulderY_Wall = 1.5f; //1.17f
        normalRig.nearShoulderY_NoWall = 1.65f;

        normalRig.nearShoulderZ_Wall = -0.18f; //-0.24f
        normalRig.nearShoulderZ_NoWall = -0.2f;
}
    void SetupCeilingGrabCameraValues()
    {
        ceilingGrabRig.farCameraDistance = 4f;
        ceilingGrabRig.farShoulderOffset = new Vector3(0f, 0f, -0.5f);
        ceilingGrabRig.farVerticalArmLength = -1.35f;

        ceilingGrabRig.nearCameraDistance = 1.5f;
        ceilingGrabRig.nearShoulderOffset = new Vector3(0f, 1.65f, -0.2f);
        ceilingGrabRig.nearVerticalArmLength = -1.2f;

        ceilingGrabRig.ceilingCheckDistance = 0.5f;
        ceilingGrabRig.wallCheckDistance = 0.55f;

        ceilingGrabRig.nearShoulderY_Ceiling = 1.5f;
        ceilingGrabRig.nearShoulderY_Clear = 1.05f; //0.85f

        ceilingGrabRig.nearShoulderY_Wall = 1.17f; //1f
        ceilingGrabRig.nearShoulderY_NoWall = 1.65f;

        ceilingGrabRig.nearShoulderZ_Wall = -0.22f; //-0.05f //-0.35 //-0.45
        ceilingGrabRig.nearShoulderZ_NoWall = -0.2f;
    }


    //-------------------------


    void EvaluateRigAndApply(RigSettings rig, bool ceilingGrabActive)
    {
        // 1. Snapshot base near shoulder for this frame (pre-adjust)
        Vector3 baseNearShoulder = rig.nearShoulderOffset;

        // 2. Predict where near camera would be using base values
        Vector3 predictedNearCamPosWorld = GetCameraWorldPosFromRig(followTarget, baseNearShoulder, rig.nearVerticalArmLength, rig.nearCameraDistance);

        // 3. Sense environment for this rig
        bool hasCeilingLikeSurface = CheckCeilingLikeSurface(rig, ceilingGrabActive);
        bool hasSideWall = HasSideWall(predictedNearCamPosWorld, rig);

        // 4. Pick final Y/Z offsets for "near"
        Vector3 adjustedNearShoulder = ChooseShoulderForThisFrame(rig, baseNearShoulder, hasCeilingLikeSurface, hasSideWall);

        // Debug lines
        if (debugDraw)
        {
            Vector3 upOrDown = ceilingGrabActive ? Vector3.down : Vector3.up;
            Debug.DrawRay(followTarget.position, upOrDown * rig.ceilingCheckDistance, hasCeilingLikeSurface ? Color.red : Color.green);
        }

        // 5. Find allowed camera distance (line-of-sight between player and far camera)
        float allowedDistanceLOS = ComputeAllowedDistance(rig);

        // 6. Smooth distance we are trying to sit at
        _currentDistance = Mathf.Lerp(_currentDistance, allowedDistanceLOS, 1f - Mathf.Exp(-lerpSpeed * Time.deltaTime));
        _currentDistance = Mathf.Clamp(_currentDistance, minDistance, maxDistance);

        // 7. Blend from near rig to far rig based on how far we made it
        float t = Mathf.InverseLerp(rig.nearCameraDistance, rig.farCameraDistance, _currentDistance);
        t = Mathf.Clamp01(t);

        float blendedDistance = Mathf.Lerp(rig.nearCameraDistance, rig.farCameraDistance, t);
        Vector3 blendedShoulder = Vector3.Lerp(adjustedNearShoulder, rig.farShoulderOffset, t);
        float blendedArm = Mathf.Lerp(rig.nearVerticalArmLength, rig.farVerticalArmLength, t);

        // 8. Resolve side collision using final blended shoulder/arm
        blendedDistance = ResolveSideCollision(blendedDistance, blendedShoulder, blendedArm);

        // 9. Push to Cinemachine with smoothing
        _tpf.CameraDistance = Mathf.Lerp(_tpf.CameraDistance, blendedDistance, 1f - Mathf.Exp(-lerpSpeed * Time.deltaTime));
        _tpf.ShoulderOffset = Vector3.Lerp(_tpf.ShoulderOffset, blendedShoulder, 1f - Mathf.Exp(-lerpSpeed * Time.deltaTime));
        _tpf.VerticalArmLength = Mathf.Lerp(_tpf.VerticalArmLength, blendedArm, 1f - Mathf.Exp(-lerpSpeed * Time.deltaTime));
    }


    bool CheckCeilingLikeSurface(RigSettings rig, bool ceilingGrabActive)
    {
        // Normal mode: "ceiling" is above; ceilingGrab: "ceiling" is below world-up
        Vector3 dir = ceilingGrabActive ? Vector3.down : Vector3.up;

        bool hit = RaycastSkippingBlockInfo(followTarget.position, dir, rig.ceilingCheckDistance, obstructionLayers, out _);

        return hit;
    }
    bool HasSideWall(Vector3 desiredCamPos, RigSettings rig)
    {
        Vector3 origin = followTarget.position;
        Vector3 dir = desiredCamPos - origin;

        // Only check horizontally
        dir.y = 0f;
        float dist = dir.magnitude;
        if (dist < 0.0001f)
            return false;

        dir /= dist;
        float checkDist = Mathf.Min(dist, rig.wallCheckDistance);

        bool hit = RaycastSkippingBlockInfo(origin, dir, checkDist, obstructionLayers, out _);

        if (debugDraw)
        {
            Debug.DrawRay(origin, dir * checkDist, hit ? Color.magenta : Color.cyan);
        }

        return hit;
    }
    static bool RaycastSkippingBlockInfo(Vector3 origin, Vector3 dir, float maxDistance, int layerMask, out RaycastHit validHit)
    {
        float remaining = maxDistance;

        while (remaining > 0f)
        {
            if (!Physics.Raycast(origin, dir, out RaycastHit hit, remaining, layerMask, QueryTriggerInteraction.Ignore))
            {
                validHit = default;
                return false;
            }

            // Skip this hit if the collider's GameObject has BlockInfo
            if (hit.collider && hit.collider.GetComponent<BlockInfo>()
                && (hit.collider.GetComponent<BlockInfo>().blockElement == BlockElement.Water
                || hit.collider.GetComponent<BlockInfo>().blockElement == BlockElement.Mud
                || hit.collider.GetComponent<BlockInfo>().blockElement == BlockElement.SwampWater))
            {
                float step = hit.distance + 0.001f;
                origin += dir * step;
                remaining -= step;
                continue;
            }

            validHit = hit;
            return true;
        }

        validHit = default;
        return false;
    }


    Vector3 ChooseShoulderForThisFrame(RigSettings rig, Vector3 baseNearShoulder, bool hasCeilingLikeSurface, bool hasSideWall)
    {
        float finalY;
        float finalZ;

        if (hasCeilingLikeSurface && hasSideWall)
        {
            float yFromCeiling = rig.nearShoulderY_Ceiling;
            float yFromWall = rig.nearShoulderY_Wall;

            finalY = (yFromCeiling < yFromWall) ? yFromCeiling : yFromWall;
            finalZ = rig.nearShoulderZ_Wall;
        }
        else if (hasCeilingLikeSurface)
        {
            finalY = rig.nearShoulderY_Ceiling;
            finalZ = rig.nearShoulderZ_NoWall;
        }
        else if (hasSideWall)
        {
            finalY = rig.nearShoulderY_Wall;
            finalZ = rig.nearShoulderZ_Wall;
        }
        else
        {
            finalY = rig.nearShoulderY_Clear;
            finalZ = rig.nearShoulderZ_NoWall;
        }

        Vector3 adjusted = baseNearShoulder;
        adjusted.y = finalY;
        adjusted.z = finalZ;
        return adjusted;
    }
    Vector3 GetCameraWorldPosFromRig(Transform pivot, Vector3 shoulderOffset, float armLen, float camDistance)
    {
        Vector3 targetPos = pivot.position;
        Quaternion targetRot = pivot.rotation;

        Vector3 worldShoulderPos = targetPos + targetRot * shoulderOffset;
        Vector3 worldHandPos = worldShoulderPos + Vector3.up * armLen;
        Vector3 camPos = worldHandPos - (pivot.forward * camDistance);

        return camPos;
    }
    RigSettings BuildBlendedRigForFrame(float blend01)
    {
        // We create a temporary RigSettings with values interpolated
        // between normalRig (0) and ceilingGrabRig (1).

        RigSettings outRig = new RigSettings();

        // Far rig
        outRig.farCameraDistance = Mathf.Lerp(normalRig.farCameraDistance, ceilingGrabRig.farCameraDistance, blend01);
        outRig.farShoulderOffset = Vector3.Lerp(normalRig.farShoulderOffset, ceilingGrabRig.farShoulderOffset, blend01);
        outRig.farVerticalArmLength = Mathf.Lerp(normalRig.farVerticalArmLength, ceilingGrabRig.farVerticalArmLength, blend01);

        // Near rig
        outRig.nearCameraDistance = Mathf.Lerp(normalRig.nearCameraDistance, ceilingGrabRig.nearCameraDistance, blend01);
        outRig.nearShoulderOffset = Vector3.Lerp(normalRig.nearShoulderOffset, ceilingGrabRig.nearShoulderOffset, blend01);
        outRig.nearVerticalArmLength = Mathf.Lerp(normalRig.nearVerticalArmLength, ceilingGrabRig.nearVerticalArmLength, blend01);

        // Probe distances
        outRig.ceilingCheckDistance = Mathf.Lerp(normalRig.ceilingCheckDistance, ceilingGrabRig.ceilingCheckDistance, blend01);
        outRig.wallCheckDistance = Mathf.Lerp(normalRig.wallCheckDistance, ceilingGrabRig.wallCheckDistance, blend01);

        // Ceiling adjustment
        outRig.nearShoulderY_Ceiling = Mathf.Lerp(normalRig.nearShoulderY_Ceiling, ceilingGrabRig.nearShoulderY_Ceiling, blend01);
        outRig.nearShoulderY_Clear = Mathf.Lerp(normalRig.nearShoulderY_Clear, ceilingGrabRig.nearShoulderY_Clear, blend01);

        // Side wall adjustment (Y)
        outRig.nearShoulderY_Wall = Mathf.Lerp(normalRig.nearShoulderY_Wall, ceilingGrabRig.nearShoulderY_Wall, blend01);
        outRig.nearShoulderY_NoWall = Mathf.Lerp(normalRig.nearShoulderY_NoWall, ceilingGrabRig.nearShoulderY_NoWall, blend01);

        // Side wall adjustment (Z)
        outRig.nearShoulderZ_Wall = Mathf.Lerp(normalRig.nearShoulderZ_Wall, ceilingGrabRig.nearShoulderZ_Wall, blend01);
        outRig.nearShoulderZ_NoWall = Mathf.Lerp(normalRig.nearShoulderZ_NoWall, ceilingGrabRig.nearShoulderZ_NoWall, blend01);

        return outRig;
    }


    float ComputeAllowedDistance(RigSettings rig)
    {
        Transform pivot = followTarget;

        Vector3 targetPos = pivot.position;
        Quaternion targetRot = pivot.rotation;

        // approximate "far" camera position
        Vector3 worldShoulderPos = targetPos + targetRot * rig.farShoulderOffset;
        Vector3 worldHandPos = worldShoulderPos + Vector3.up * rig.farVerticalArmLength;
        Vector3 desiredCamPos = worldHandPos - (pivot.forward * rig.farCameraDistance);

        Vector3 rayDir = desiredCamPos - targetPos;
        float rayLen = rayDir.magnitude;
        if (rayLen < 0.0001f)
            return rig.nearCameraDistance;

        rayDir /= rayLen;

        if (RaycastSkippingBlockInfo(targetPos, rayDir, rayLen, obstructionLayers, out RaycastHit hit))
        {
            // Where the obstruction is, but keep a small buffer off the wall
            Vector3 point = targetPos + rayDir * (hit.distance - wallBuffer);

            // Project along -pivot.forward from the near hand position
            Vector3 worldShoulderPosNear = targetPos + targetRot * rig.nearShoulderOffset;
            Vector3 worldHandPosNear = worldShoulderPosNear + Vector3.up * rig.nearVerticalArmLength;

            float usableDist = Vector3.Dot(point - worldHandPosNear, -pivot.forward);
            usableDist = Mathf.Abs(usableDist);

            return Mathf.Clamp(usableDist, minDistance, rig.farCameraDistance);
        }

        // Nothing blocking within rayLen
        return rig.farCameraDistance;
    }

    float ResolveSideCollision(float desiredDistance, Vector3 shoulderOffset, float armLen)
    {
        Transform pivot = followTarget;

        Vector3 targetPos = pivot.position;
        Quaternion targetRot = pivot.rotation;

        Vector3 worldShoulderPos = targetPos + targetRot * shoulderOffset;
        Vector3 worldHandPos = worldShoulderPos + Vector3.up * armLen;
        Vector3 desiredCamPos = worldHandPos - (pivot.forward * desiredDistance);

        Vector3 dir = desiredCamPos - targetPos;
        float dist = dir.magnitude;
        if (dist < 0.0001f)
            return desiredDistance;

        dir /= dist;

        if (SphereCastSkippingBlockInfo(targetPos, cameraCollisionRadius, dir, dist, obstructionLayers, out RaycastHit hit))
        {
            float allowed = hit.distance - cameraSurfaceOffset;
            allowed = Mathf.Clamp(allowed, minDistance, desiredDistance);
            return allowed;
        }

        return desiredDistance;
    }
    static bool SphereCastSkippingBlockInfo(Vector3 origin, float radius, Vector3 dir, float maxDistance, int layerMask, out RaycastHit validHit)
    {
        float remaining = maxDistance;

        while (remaining > 0f)
        {
            if (!Physics.SphereCast(
                    origin,
                    radius,
                    dir,
                    out RaycastHit hit,
                    remaining,
                    layerMask,
                    QueryTriggerInteraction.Ignore))
            {
                validHit = default;
                return false;
            }

            // Skip any hit that belongs to a BlockInfo object
            if (hit.collider && hit.collider.GetComponent<BlockInfo>()
                && (hit.collider.GetComponent<BlockInfo>().blockElement == BlockElement.Water
                || hit.collider.GetComponent<BlockInfo>().blockElement == BlockElement.Mud
                || hit.collider.GetComponent<BlockInfo>().blockElement == BlockElement.SwampWater))
            {
                float step = hit.distance + 0.001f;
                origin += dir * step;
                remaining -= step;
                continue;
            }

            validHit = hit;
            return true;
        }

        validHit = default;
        return false;
    }

    void ApplyRigInstant(RigSettings rig)
    {
        _tpf.CameraDistance = rig.farCameraDistance;
        _tpf.ShoulderOffset = rig.farShoulderOffset;
        _tpf.VerticalArmLength = rig.farVerticalArmLength;
    }


    //-------------------------


    void OnDrawGizmosSelected()
    {
        if (!debugDraw || followTarget == null)
            return;

        // We'll visualize using NORMAL rig (editor preview).
        RigSettings rig = normalRig;

        Transform pivot = followTarget;
        Vector3 targetPos = pivot.position;
        Quaternion targetRot = pivot.rotation;

        Vector3 worldShoulderPos = targetPos + targetRot * rig.farShoulderOffset;
        Vector3 worldHandPos = worldShoulderPos + Vector3.up * rig.farVerticalArmLength;
        Vector3 desiredCamPos = worldHandPos - (pivot.forward * rig.farCameraDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(targetPos, desiredCamPos);
        Gizmos.DrawSphere(desiredCamPos, 0.05f);

        if (Application.isPlaying && _tpf != null)
        {
            Vector3 curShoulder = targetPos + targetRot * _tpf.ShoulderOffset;
            Vector3 curHand = curShoulder + Vector3.up * _tpf.VerticalArmLength;
            Vector3 curCamPos = curHand - (pivot.forward * _tpf.CameraDistance);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(targetPos, curCamPos);
            Gizmos.DrawSphere(curCamPos, 0.05f);

            Gizmos.color = new Color(1f, 1f, 0f, 0.25f);
            Gizmos.DrawWireSphere(curCamPos, cameraCollisionRadius);
        }
    }

    public void CameraZoom(bool value)
    {
        effect_isDisabled = value;
    }
}
[System.Serializable]
public class RigSettings
{
    [Header("Far Rig (unobstructed)")]
    public float farCameraDistance = 4f;
    public Vector3 farShoulderOffset = new Vector3(0f, 0f, -0.5f);
    public float farVerticalArmLength = 2.77f;

    [Header("Near Rig (pushed in by walls)")]
    public float nearCameraDistance = 1.5f;
    public Vector3 nearShoulderOffset = new Vector3(0f, 1.65f, -0.2f);
    public float nearVerticalArmLength = -1.2f;

    [Header("Environment Probes")]
    [Tooltip("How far to check vertically for a 'ceiling'. For normal mode, we check UP.")]
    public float ceilingCheckDistance = 0.5f;

    [Tooltip("How far forward/horizontal to check for wall hugging the camera.")]
    public float wallCheckDistance = 0.55f;

    [Header("Ceiling Adjustment")]
    [Tooltip("nearShoulder Y when ceiling is close.")]
    public float nearShoulderY_Ceiling = 1.5f;
    [Tooltip("nearShoulder Y when NO ceiling.")]
    public float nearShoulderY_Clear = 1.65f;

    [Header("Side Wall Adjustment")]
    [Tooltip("nearShoulder Y when we're hugging a wall in camera direction.")]
    public float nearShoulderY_Wall = 1.45f;
    [Tooltip("nearShoulder Y when we're not against a wall.")]
    public float nearShoulderY_NoWall = 1.65f;

    [Tooltip("nearShoulder Z (depth) when we're hugging a wall.")]
    public float nearShoulderZ_Wall = -0.03f;
    [Tooltip("nearShoulder Z (depth) when we're not against a wall.")]
    public float nearShoulderZ_NoWall = -0.2f;
}
