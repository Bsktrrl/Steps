using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class GlueplantCamera : MonoBehaviour
{
    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineCamera CM_Glueplant;
    [SerializeField] private CinemachineCamera CM_Player;

    [Header("Travel Waypoints (world positions)")]
    [Tooltip("Intermediate world positions the camera will pass through (does not need to include start/end).")]
    [SerializeField] private List<Vector3> travelPosList = new List<Vector3>();

    [Header("Travel Settings (Speed)")]
    [Tooltip("Max travel speed along the path in world units per second (peak speed).")]
    [SerializeField] private float travelSpeed = 3f; //2.7f

    [Tooltip("Meters to ramp from start speed to max speed.")]
    [SerializeField] private float accelDistance = 1.5f;

    [Tooltip("Meters to ramp from max speed down to end speed.")]
    [SerializeField] private float decelDistance = 2f; //1.5f

    [Tooltip("Fraction of max speed used by the distance-based ramp at the very start.")]
    [Range(0f, 0.5f)]
    [SerializeField] private float startSpeedFraction = 0.15f;

    [Tooltip("Fraction of max speed used by the distance-based ramp at the very end.")]
    [Range(0f, 0.5f)]
    [SerializeField] private float endSpeedFraction = 0.5f; //0.3f

    [Header("Start smoothing (prevents fast start / jerk)")]
    [Tooltip("Seconds to ease in the speed from 0 at travel start (in addition to accelDistance ramp).")]
    [Range(0f, 2f)]
    [SerializeField] private float startEaseTime = 0.6f;

    [Tooltip("Curve for start ease (0..1 time -> 0..1 multiplier). Default is smooth ease-in.")]
    [SerializeField] private AnimationCurve startEaseCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Spline Quality")]
    [Tooltip("How many samples per segment when generating the smooth path. Higher = smoother but more CPU on start.")]
    [Range(4, 64)]
    [SerializeField] private int samplesPerSegment = 50;

    [Tooltip("0 = straight lines through points, 1 = full Catmull-Rom smoothing.")]
    [Range(0f, 1f)]
    [SerializeField] private float splineSmoothness = 1f;

    [Tooltip("If true, the travel uses unscaled time (ignores Time.timeScale).")]
    [SerializeField] private bool useUnscaledTime = false;

    [Header("Turn Slowdown")]
    [Range(0.2f, 1f)]
    [SerializeField] private float minTurnSpeedMultiplier = 0.5f;

    [Range(10f, 170f)]
    [SerializeField] private float turnAngleForMaxSlowdown = 70f;

    [Range(0.5f, 4f)]
    [SerializeField] private float turnSlowdownPower = 0.75f;

    [Header("End Blends")]
    [SerializeField] private float endPositionBlendDistance = 2f; //4f
    [SerializeField] private float endRotationBlendDistance = 3f; //4f

    [Header("Facing / Rotation")]
    [SerializeField] private float lookRotationSharpness = 3f;
    [SerializeField] private float verticalLookSharpness = 3f;

    [Range(0f, 89f)]
    [SerializeField] private float maxPitchFromHorizontal = 40f; //35f

    [Tooltip("0 = ignore vertical when computing look direction (comfort). 1 = full vertical influence (more literal).")]
    [Range(0f, 1f)]
    [SerializeField] private float verticalInfluenceOnLook = 0.15f;

    [Tooltip("Seconds at start where we blend from initial forward to motion-based backward look (prevents snap).")]
    [Range(0f, 2f)]
    [SerializeField] private float initialLookStabilizeTime = 0.4f; //0.6f

    [Header("Stability")]
    [SerializeField] private float minPointSeparation = 0.01f;

    [Header("Priority Switching")]
    [SerializeField] private int glueplantPriorityDuringTravel = 50;
    [SerializeField] private int playerPriorityDuringTravel = 0;

    [Header("Start Rotation Blend")]
    [Tooltip("Meters from the start where we blend from the initial shot rotation into the path-based look rotation.")]
    [SerializeField] private float startRotationBlendDistance = 4f;

    [Tooltip("Extra meters ahead on the path to sample the tangent/look direction (reduces jitter at very start).")]
    [SerializeField] private float startLookAheadDistance = 2f;

    [Tooltip("If true, camera looks backwards along travel direction (towards where it came from).")]
    [SerializeField] private bool lookBackwardsAlongPath = true;


    [SerializeField] private bool camera_isTraveling;
    private Coroutine travelRoutine;


    //--------------------


    private void OnEnable()
    {
        MapManager.Action_SpawnedPlayerObject += OnPlayerSpawned;

        if (MapManager.SpawnedPlayer != null)
            OnPlayerSpawned(MapManager.SpawnedPlayer);
    }

    private void OnDisable()
    {
        MapManager.Action_SpawnedPlayerObject -= OnPlayerSpawned;
    }


    //--------------------


    private void OnPlayerSpawned(GameObject playerGO)
    {
        if (MapManager.Instance.haveIntroSequence)
        {
            CM_Player = CameraController.Instance.CM_Player;

            SetPriority(CM_Glueplant, glueplantPriorityDuringTravel);
            SetPriority(CM_Player, playerPriorityDuringTravel);
            CM_Glueplant.Prioritize();

            RunCameraTravel();
        }
    }

    public void RunCameraTravel()
    {
        if (camera_isTraveling || CM_Glueplant == null) return;

        if (CM_Player == null)
            CM_Player = CameraController.Instance.CM_Player;

        if (CM_Player == null) return;

        if (travelRoutine != null)
            StopCoroutine(travelRoutine);

        travelRoutine = StartCoroutine(Co_RunCameraTravel(1.5f));
    }


    //--------------------


    private IEnumerator Co_RunCameraTravel(float waitTime)
    {
        camera_isTraveling = true;

        if (waitTime > 0f)
            yield return new WaitForSeconds(waitTime);

        // --- switch priority first
        SetPriority(CM_Glueplant, glueplantPriorityDuringTravel);
        SetPriority(CM_Player, playerPriorityDuringTravel);
        CM_Glueplant.Prioritize();

        // IMPORTANT: wait 1 frame so the Brain actually makes CM_Glueplant live
        yield return null;

        // Get the REAL rendered camera pose (what the player actually sees)
        Vector3 livePos;
        Quaternion liveRot;
        if (!TryGetLiveCameraPose(out livePos, out liveRot))
        {
            // Fallback to vcam transform if no brain/main cam
            livePos = CM_Glueplant.transform.position;
            liveRot = CM_Glueplant.transform.rotation;
        }

        // HARD SNAP the vcam to live pose and reset CM history so no damping “settle” happens
        CM_Glueplant.transform.SetPositionAndRotation(livePos, liveRot);

        // If you're on Cinemachine v2/v3 and this property exists, it's the magic sauce:
        try { CM_Glueplant.PreviousStateIsValid = false; } catch { /* ignore if not available */ }

        // Now start from what is actually visible
        Vector3 startPos = livePos;
        Quaternion startRot = liveRot;

        Vector3 endPos = CM_Player.transform.position;
        Quaternion endRot = CM_Player.transform.rotation;

        List<Vector3> controlPoints = new List<Vector3>(travelPosList.Count + 2);
        controlPoints.Add(startPos);
        controlPoints.AddRange(travelPosList);
        controlPoints.Add(endPos);

        controlPoints = RemoveNearDuplicates(controlPoints, minPointSeparation);

        if (controlPoints.Count < 2 || Vector3.Distance(startPos, endPos) <= minPointSeparation)
        {
            CM_Glueplant.transform.SetPositionAndRotation(endPos, endRot);
            CM_Player.transform.SetPositionAndRotation(endPos, endRot);
            FinishTravel();
            yield break;
        }

        List<Vector3> sampled = BuildSampledPath(controlPoints, samplesPerSegment, splineSmoothness);
        sampled = RemoveNearDuplicates(sampled, minPointSeparation);

        if (sampled.Count < 2)
        {
            sampled.Clear();
            sampled.Add(startPos);
            sampled.Add(endPos);
        }

        List<float> cumulative = new List<float>(sampled.Count);
        cumulative.Add(0f);

        float totalLen = 0f;
        for (int i = 1; i < sampled.Count; i++)
        {
            totalLen += Vector3.Distance(sampled[i - 1], sampled[i]);
            cumulative.Add(totalLen);
        }

        if (totalLen <= 0.0001f)
        {
            CM_Glueplant.transform.SetPositionAndRotation(endPos, endRot);
            CM_Player.transform.SetPositionAndRotation(endPos, endRot);
            FinishTravel();
            yield break;
        }

        float aDist = Mathf.Max(0f, accelDistance);
        float dDist = Mathf.Max(0f, decelDistance);
        float sum = aDist + dDist;
        if (sum > totalLen)
        {
            float k = totalLen / Mathf.Max(0.0001f, sum);
            aDist *= k;
            dDist *= k;
        }

        float traveled = 0f;

        Vector3 prevPos = startPos;                 // instead of CM_Glueplant.transform.position
        Vector3 initialForward = (startRot * Vector3.forward).normalized; // instead of CM_Glueplant.transform.forward
        Vector3 smoothedLookDir = initialForward;

        float travelStartTime = useUnscaledTime ? Time.unscaledTime : Time.time;

        while (traveled < totalLen)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

            // --- Speed with: turn slowdown + distance ramps + time-based start ease
            float turnFactor = EvaluateTurnSpeedFactor(sampled, cumulative, traveled,
                turnAngleForMaxSlowdown, minTurnSpeedMultiplier, turnSlowdownPower);

            float accelFactor = EvaluateRamp(traveled, aDist, startSpeedFraction, 1f);
            float decelFactor = EvaluateRamp(totalLen - traveled, dDist, endSpeedFraction, 1f);

            float currentSpeed = travelSpeed * accelFactor * decelFactor * turnFactor;

            // Additional time-based start easing (ramps from 0 smoothly)
            if (startEaseTime > 0.0001f)
            {
                float now = useUnscaledTime ? Time.unscaledTime : Time.time;
                float tStart = Mathf.Clamp01((now - travelStartTime) / startEaseTime);
                float startMul = Mathf.Clamp01(startEaseCurve.Evaluate(tStart));
                currentSpeed *= startMul;
            }

            // Keep it numerically stable, but allow genuinely slow start
            currentSpeed = Mathf.Max(0.001f, currentSpeed);

            traveled = Mathf.Min(totalLen, traveled + currentSpeed * dt);

            // --- Base position on path
            Vector3 pathPos = EvaluatePolyline(sampled, cumulative, traveled);

            // --- End position blend
            Vector3 pos = pathPos;
            if (endPositionBlendDistance > 0f)
            {
                float remaining = totalLen - traveled;
                float tPos = Mathf.InverseLerp(endPositionBlendDistance, 0f, remaining);
                tPos = Smooth01(tPos);
                pos = Vector3.Lerp(pathPos, endPos, tPos);
            }

            // Movement direction from actual moved position (post-blend)
            Vector3 moveDir = pos - prevPos;
            if (moveDir.sqrMagnitude < 1e-10f)
                moveDir = EvaluateTangent(sampled, cumulative, traveled);

            // Reduce vertical influence to avoid early pitch spikes
            Vector3 moveDirForLook = new Vector3(moveDir.x, moveDir.y * verticalInfluenceOnLook, moveDir.z);

            Vector3 desiredLookDir = (-moveDirForLook);
            if (desiredLookDir.sqrMagnitude < 1e-10f)
                desiredLookDir = smoothedLookDir;

            desiredLookDir.Normalize();

            // Pitch clamp
            desiredLookDir = ClampPitchFromHorizontal(desiredLookDir, maxPitchFromHorizontal, smoothedLookDir);

            // Initial look stabilization: blend from initialForward -> desiredLookDir over time
            if (initialLookStabilizeTime > 0.0001f)
            {
                float now = useUnscaledTime ? Time.unscaledTime : Time.time;
                float tStab = Mathf.Clamp01((now - travelStartTime) / initialLookStabilizeTime);
                float stab = Smooth01(tStab);
                desiredLookDir = Vector3.Slerp(initialForward, desiredLookDir, stab).normalized;
            }

            // Two-stage smoothing: yaw faster, pitch slower
            Vector3 desiredHorizontal = new Vector3(desiredLookDir.x, 0f, desiredLookDir.z);
            if (desiredHorizontal.sqrMagnitude > 1e-10f) desiredHorizontal.Normalize();
            else
            {
                desiredHorizontal = new Vector3(smoothedLookDir.x, 0f, smoothedLookDir.z);
                if (desiredHorizontal.sqrMagnitude > 1e-10f) desiredHorizontal.Normalize();
                else desiredHorizontal = Vector3.forward;
            }

            float yawAlpha = 1f - Mathf.Exp(-lookRotationSharpness * dt);
            Vector3 yawSmoothed = Vector3.Slerp(smoothedLookDir, desiredHorizontal, yawAlpha).normalized;

            float pitchAlpha = 1f - Mathf.Exp(-verticalLookSharpness * dt);
            smoothedLookDir = Vector3.Slerp(yawSmoothed, desiredLookDir, pitchAlpha).normalized;

            Quaternion lookBackRot = Quaternion.LookRotation(smoothedLookDir, Vector3.up);

            // End rotation blend
            Quaternion targetRot = lookBackRot;
            if (endRotationBlendDistance > 0f)
            {
                float remaining = totalLen - traveled;
                float tRot = Mathf.InverseLerp(endRotationBlendDistance, 0f, remaining);
                tRot = Smooth01(tRot);
                targetRot = Quaternion.Slerp(lookBackRot, endRot, tRot);
            }

            CM_Glueplant.transform.SetPositionAndRotation(pos, targetRot);

            prevPos = pos;
            yield return null;
        }

        CM_Glueplant.transform.SetPositionAndRotation(endPos, endRot);
        CM_Player.transform.SetPositionAndRotation(endPos, endRot);

        yield return new WaitForSeconds(0.25f);

        FinishTravel();
    }

    private void FinishTravel()
    {
        camera_isTraveling = false;
        travelRoutine = null;

        SetPriority(CM_Player, glueplantPriorityDuringTravel);
        SetPriority(CM_Glueplant, playerPriorityDuringTravel);
        CM_Player.Prioritize();

        MapManager.Instance.introSequence_Finished = true;
        MapManager.Instance.introSequence = false;

        print("1000000000000. FinishTravel()");

        MapManager.Instance.Action_EndIntroSequence_Invoke();
    }

    private static void SetPriority(CinemachineCamera cam, int value)
    {
        if (cam == null) return;
        cam.Priority = new PrioritySettings { Enabled = true, Value = value };
    }


    // ----------------------------
    // Safety / cleanup
    // ----------------------------


    private static List<Vector3> RemoveNearDuplicates(List<Vector3> points, float minDist)
    {
        if (points == null) return new List<Vector3>();
        if (points.Count <= 1) return new List<Vector3>(points);

        float minDistSqr = minDist * minDist;
        List<Vector3> clean = new List<Vector3>(points.Count);
        clean.Add(points[0]);

        for (int i = 1; i < points.Count; i++)
        {
            if ((points[i] - clean[clean.Count - 1]).sqrMagnitude >= minDistSqr)
                clean.Add(points[i]);
        }

        if (clean.Count >= 1 && (clean[clean.Count - 1] - points[points.Count - 1]).sqrMagnitude > 0f)
            clean.Add(points[points.Count - 1]);

        return clean;
    }


    // ----------------------------
    // Speed helpers
    // ----------------------------


    private bool TryGetLiveCameraPose(out Vector3 pos, out Quaternion rot)
    {
        pos = Vector3.zero;
        rot = Quaternion.identity;

        var mainCam = Camera.main;
        if (mainCam == null) return false;

        var brain = mainCam.GetComponent<CinemachineBrain>();
        if (brain == null || brain.OutputCamera == null) return false;

        pos = brain.OutputCamera.transform.position;
        rot = brain.OutputCamera.transform.rotation;
        return true;
    }


    private static float EvaluateRamp(float distanceIntoRamp, float rampDist, float minFrac, float maxFrac)
    {
        if (rampDist <= 0.0001f)
            return maxFrac;

        float t = Mathf.Clamp01(distanceIntoRamp / rampDist);
        float s = Smooth01(t);
        return Mathf.Lerp(minFrac, maxFrac, s);
    }

    private static float EvaluateTurnSpeedFactor(List<Vector3> pts, List<float> cumulative, float distance, float maxTurnDeg, float minMultiplier, float power)
    {
        if (pts == null || pts.Count < 4 || cumulative == null || cumulative.Count < 4)
            return 1f;

        int idx = FindSegmentIndex(cumulative, distance);
        idx = Mathf.Clamp(idx, 0, pts.Count - 2);

        int i0 = Mathf.Clamp(idx - 1, 0, pts.Count - 1);
        int i1 = Mathf.Clamp(idx, 0, pts.Count - 1);
        int i2 = Mathf.Clamp(idx + 1, 0, pts.Count - 1);
        int i3 = Mathf.Clamp(idx + 2, 0, pts.Count - 1);

        Vector3 d0 = pts[i1] - pts[i0];
        Vector3 d1 = pts[i2] - pts[i1];
        Vector3 d2 = pts[i3] - pts[i2];

        if (d0.sqrMagnitude < 1e-9f || d1.sqrMagnitude < 1e-9f || d2.sqrMagnitude < 1e-9f)
            return 1f;

        d0.Normalize(); d1.Normalize(); d2.Normalize();

        float a01 = Vector3.Angle(d0, d1);
        float a12 = Vector3.Angle(d1, d2);
        float angle = Mathf.Max(a01, a12);

        float t = Mathf.InverseLerp(0f, Mathf.Max(1f, maxTurnDeg), angle);
        t = Mathf.Clamp01(t);
        t = Mathf.Pow(t, Mathf.Max(0.01f, power));

        return Mathf.Lerp(1f, minMultiplier, t);
    }


    // ----------------------------
    // Rotation helpers
    // ----------------------------


    private static Vector3 ClampPitchFromHorizontal(Vector3 dir, float maxPitchDeg, Vector3 fallbackDir)
    {
        Vector3 horiz = new Vector3(dir.x, 0f, dir.z);
        if (horiz.sqrMagnitude < 1e-9f)
        {
            Vector3 fb = new Vector3(fallbackDir.x, 0f, fallbackDir.z);
            if (fb.sqrMagnitude < 1e-9f) fb = Vector3.forward;
            return fb.normalized;
        }

        horiz.Normalize();
        float pitch = Vector3.Angle(dir, horiz);
        if (pitch <= maxPitchDeg) return dir;

        float t = Mathf.InverseLerp(maxPitchDeg, 90f, pitch);
        return Vector3.Slerp(dir, horiz, t).normalized;
    }

    private static float Smooth01(float x)
    {
        x = Mathf.Clamp01(x);
        return x * x * (3f - 2f * x);
    }


    // ----------------------------
    // Path building & evaluation
    // ----------------------------


    private static List<Vector3> BuildSampledPath(List<Vector3> points, int samplesPerSeg, float smoothness01)
    {
        if (points == null || points.Count == 0)
            return new List<Vector3>();

        if (smoothness01 <= 0f || points.Count < 3)
            return DensifyLinear(points, Mathf.Max(2, samplesPerSeg / 2));

        List<Vector3> sampled = new List<Vector3>();
        int n = points.Count;

        Vector3 P(int idx)
        {
            if (idx < 0)
            {
                // Mirror the first segment direction so the curve starts by heading forward
                if (n >= 2) return points[0] + (points[0] - points[1]);
                return points[0];
            }
            if (idx >= n)
            {
                // Mirror the last segment direction so the curve ends cleanly
                if (n >= 2) return points[n - 1] + (points[n - 1] - points[n - 2]);
                return points[n - 1];
            }
            return points[idx];
        }

        for (int i = 0; i < n - 1; i++)
        {
            Vector3 p0 = P(i - 1);
            Vector3 p1 = P(i);
            Vector3 p2 = P(i + 1);
            Vector3 p3 = P(i + 2);

            if (i == 0)
                sampled.Add(p1);

            for (int s = 1; s <= samplesPerSeg; s++)
            {
                float t = s / (float)samplesPerSeg;
                Vector3 catmull = CatmullRom(p0, p1, p2, p3, t);
                Vector3 linear = Vector3.Lerp(p1, p2, t);
                Vector3 blended = Vector3.Lerp(linear, catmull, smoothness01);
                sampled.Add(blended);
            }
        }

        return sampled;
    }

    private static List<Vector3> DensifyLinear(List<Vector3> points, int samplesPerSeg)
    {
        List<Vector3> sampled = new List<Vector3>();
        if (points == null || points.Count == 0) return sampled;

        sampled.Add(points[0]);
        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 a = points[i];
            Vector3 b = points[i + 1];

            for (int s = 1; s <= samplesPerSeg; s++)
            {
                float t = s / (float)samplesPerSeg;
                sampled.Add(Vector3.Lerp(a, b, t));
            }
        }
        return sampled;
    }

    private static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3
        );
    }

    private static Vector3 EvaluatePolyline(List<Vector3> pts, List<float> cumulative, float distance)
    {
        if (pts == null || pts.Count == 0) return Vector3.zero;
        if (pts.Count == 1) return pts[0];
        if (cumulative == null || cumulative.Count != pts.Count) return pts[0];

        float total = cumulative[cumulative.Count - 1];
        distance = Mathf.Clamp(distance, 0f, total);

        int idx = FindSegmentIndex(cumulative, distance);
        idx = Mathf.Clamp(idx, 0, pts.Count - 2);

        int next = idx + 1;

        float d0 = cumulative[idx];
        float d1 = cumulative[next];
        float segLen = Mathf.Max(0.000001f, d1 - d0);

        float t = (distance - d0) / segLen;
        return Vector3.Lerp(pts[idx], pts[next], t);
    }

    private static Vector3 EvaluateTangent(List<Vector3> pts, List<float> cumulative, float distance)
    {
        if (pts == null || pts.Count < 2) return Vector3.forward;
        if (cumulative == null || cumulative.Count != pts.Count) return (pts[1] - pts[0]);

        float total = cumulative[cumulative.Count - 1];
        distance = Mathf.Clamp(distance, 0f, total);

        int idx = FindSegmentIndex(cumulative, distance);
        idx = Mathf.Clamp(idx, 0, pts.Count - 2);

        return (pts[idx + 1] - pts[idx]);
    }

    private static int FindSegmentIndex(List<float> cumulative, float distance)
    {
        for (int i = 0; i < cumulative.Count - 1; i++)
        {
            if (distance <= cumulative[i + 1])
                return i;
        }
        return Mathf.Max(0, cumulative.Count - 2);
    }
}
