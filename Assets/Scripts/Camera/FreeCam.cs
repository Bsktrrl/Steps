using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FreeCam : Singleton<FreeCam>
{
    [Header("References")]
    [SerializeField] public CinemachineCamera CM_FreeCam;

    [Header("Switching")]
    [Tooltip("If true, the FreeCam GameObject will be enabled/disabled when entering/exiting.")]
    [SerializeField] private bool toggleFreeCamGameObject = false;

    [Tooltip("Extra priority added to FreeCam while active (higher wins).")]
    [SerializeField] private int freeCamPriorityBoost = 10;

    [Header("Movement")]
    [Tooltip("Units per second.")]
    [SerializeField] private float moveSpeed = 8f;

    [Tooltip("How quickly the camera accelerates towards target speed (bigger = snappier).")]
    [SerializeField] private float acceleration = 12f;

    [Tooltip("How quickly the camera slows down when input stops (bigger = snappier).")]
    [SerializeField] private float deceleration = 18f;

    [Tooltip("If true, uses unscaled time (recommended if you pause with Time.timeScale = 0).")]
    [SerializeField] private bool useUnscaledTime = true;

    [Header("Collision")]
    [Tooltip("How far ahead to probe for obstacles when moving.")]
    [SerializeField] private float collisionProbeDistance = 0.5f;

    [Tooltip("Radius for the sphere cast used to stop at walls.")]
    [SerializeField] private float collisionProbeRadius = 0.18f;

    [Tooltip("Small offset so we stop slightly before the hit surface.")]
    [SerializeField] private float collisionStopOffset = 0.02f;

    [Tooltip("Ignore trigger colliders.")]
    [SerializeField] private bool ignoreTriggers = true;

    [Header("Collision")]
    [Tooltip("Prefab assets the FreeCam is allowed to pass through (any instance in the scene).")]
    [SerializeField] private List<GameObject> passThroughPrefabs = new List<GameObject>();

    [Header("Return Pan (EndFreeCam)")]
    [Tooltip("Duration of the smooth pan back to the player camera pose.")]
    [SerializeField] private float returnDuration = 0.35f;

    [Tooltip("Easing curve for the return pan (0..1).")]
    [SerializeField] private AnimationCurve returnEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("FreeCam Enter Cue")]
    [SerializeField] private float enterFadeDistance = 1f;
    [SerializeField] private float enterFadeDuration = 0.03f;
    [SerializeField] private AnimationCurve enterFadeEase = AnimationCurve.EaseInOut(0, 0, 1, 1);


    //--------------------


    private CinemachineCamera _cmPlayer;

    // Cinemachine 3.0.1: Priority is PrioritySettings, not int
    private PrioritySettings _playerPriorityBefore;
    private PrioritySettings _freePriorityBefore;

    private bool _isActive;
    private Coroutine _returnRoutine;

    // movement input (sum of pressed directions, in camera-local axes)
    // x = right/left, y = up/down, z = forward/back
    private Vector3 _inputSum;
    private Vector3 _currentVelocity; // world-space velocity (smoothed)

    private Coroutine _enterCueRoutine;


    //--------------------


    private void OnEnable()
    {
        Player_KeyInputs.Action_FreeCamIsActive += StartFreeCam;
        Player_KeyInputs.Action_FreeCamIsPassive += EndFreeCam;
    }

    private void OnDisable()
    {
        Player_KeyInputs.Action_FreeCamIsActive -= StartFreeCam;
        Player_KeyInputs.Action_FreeCamIsPassive -= EndFreeCam;
    }


    //--------------------


    private void Awake()
    {
       
    }

    private void Update()
    {
        if (!_isActive || CM_FreeCam == null) return;

        float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        if (dt <= 0f) return;

        TickMovement(dt);
    }

    // --------------------
    // Public API (assign these yourself to your button logic)
    // --------------------

    /// <summary>
    /// Call on "button down" to add a direction, and on "button up" to remove it.
    /// Direction should be in camera-local axes:
    /// Forward (0,0,1), Back (0,0,-1), Right (1,0,0), Left (-1,0,0), Up (0,1,0), Down (0,-1,0).
    /// Diagonal movement happens automatically by summing.
    /// </summary>
    public void SetMoveDirection(Vector3 localAxisDirection, bool pressed)
    {
        localAxisDirection = new Vector3(
            Mathf.Clamp(localAxisDirection.x, -1f, 1f),
            Mathf.Clamp(localAxisDirection.y, -1f, 1f),
            Mathf.Clamp(localAxisDirection.z, -1f, 1f)
        );

        if (pressed) _inputSum += localAxisDirection;
        else _inputSum -= localAxisDirection;

        _inputSum = new Vector3(
            Mathf.Clamp(_inputSum.x, -1f, 1f),
            Mathf.Clamp(_inputSum.y, -1f, 1f),
            Mathf.Clamp(_inputSum.z, -1f, 1f)
        );
    }

    // --------------------
    // Your requested functions
    // --------------------

    private void StartFreeCam()
    {
        if (CM_FreeCam == null) return;

        _cmPlayer = CameraController.Instance != null ? CameraController.Instance.CM_Player : null;
        if (_cmPlayer == null) return;

        if (_returnRoutine != null)
        {
            StopCoroutine(_returnRoutine);
            _returnRoutine = null;
        }

        // snap FreeCam to PlayerCam pose
        CM_FreeCam.transform.SetPositionAndRotation(_cmPlayer.transform.position, _cmPlayer.transform.rotation);

        // store current priorities (PrioritySettings)
        _playerPriorityBefore = _cmPlayer.Priority;
        _freePriorityBefore = CM_FreeCam.Priority;

        if (toggleFreeCamGameObject)
            CM_FreeCam.gameObject.SetActive(true);

        // make FreeCam win (PrioritySettings.Value in CM 3.x)
        int playerVal = GetPriorityValue(_cmPlayer);
        SetPriorityValue(CM_FreeCam, playerVal + freeCamPriorityBoost);

        _isActive = true;
        _inputSum = Vector3.zero;
        _currentVelocity = Vector3.zero;

        if (_enterCueRoutine != null)
            StopCoroutine(_enterCueRoutine);

        _enterCueRoutine = StartCoroutine(EnterFreeCamCue());
    }

    private void EndFreeCam()
    {
        if (!_isActive || CM_FreeCam == null) return;

        _cmPlayer = CameraController.Instance != null ? CameraController.Instance.CM_Player : _cmPlayer;
        if (_cmPlayer == null)
        {
            _isActive = false;
            return;
        }

        if (_returnRoutine != null) StopCoroutine(_returnRoutine);
        _returnRoutine = StartCoroutine(ReturnToPlayerThenSwitch());
    }

    // --------------------
    // Priority helpers (Cinemachine 3.0.1)
    // --------------------

    private static int GetPriorityValue(CinemachineCamera cam)
    {
        // Most CM 3.x builds:
        return cam.Priority.Value;

        // If your PrioritySettings uses a different field name, use one of these instead:
        // return cam.Priority.m_Value;
        // return cam.Priority.Value.Value; // (unlikely, but some APIs nest)
    }

    private static void SetPriorityValue(CinemachineCamera cam, int value)
    {
        var p = cam.Priority;
        p.Value = value;      // change to p.m_Value = value; if needed
        cam.Priority = p;     // IMPORTANT: write the struct back
    }

    // --------------------
    // Internals
    // --------------------

    private void TickMovement(float dt)
    {
        Vector3 local = _inputSum;

        Vector3 desiredWorldVelocity = Vector3.zero;
        if (local.sqrMagnitude > 0.0001f)
        {
            local = local.normalized;

            Transform t = CM_FreeCam.transform;
            Vector3 worldDir =
                (t.right * local.x) +
                (t.up * local.y) +
                (t.forward * local.z);

            desiredWorldVelocity = worldDir.normalized * moveSpeed;
        }

        float rate = (desiredWorldVelocity.sqrMagnitude > 0.0001f) ? acceleration : deceleration;
        _currentVelocity = Vector3.Lerp(_currentVelocity, desiredWorldVelocity, 1f - Mathf.Exp(-rate * dt));

        Vector3 delta = _currentVelocity * dt;
        if (delta.sqrMagnitude < 0.0000001f) return;

        Vector3 newPos = ComputeBlockedMovement(CM_FreeCam.transform.position, delta);
        CM_FreeCam.transform.position = newPos;
    }

    private Vector3 ComputeBlockedMovement(Vector3 currentPos, Vector3 delta)
    {
        float maxProbe = Mathf.Max(0f, collisionProbeDistance);
        if (maxProbe <= 0f || collisionProbeRadius <= 0f)
            return currentPos + delta;

        float moveDist = delta.magnitude;
        Vector3 dir = delta / moveDist;

        float probeDist = Mathf.Min(maxProbe, moveDist);

        RaycastHit[] hits = _hitsCache;
        int hitCount = Physics.SphereCastNonAlloc(
            currentPos,
            collisionProbeRadius,
            dir,
            hits,
            probeDist,
            Physics.DefaultRaycastLayers,
            ignoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide
        );

        if (hitCount <= 0)
            return currentPos + delta;

        float closest = float.PositiveInfinity;
        bool foundBlocking = false;

        for (int i = 0; i < hitCount; i++)
        {
            Collider col = hits[i].collider;
            if (!col) continue;

            if (col.GetComponentInParent<FreeCamPassThrough>() != null)
                continue;

            if (col.transform.IsChildOf(transform))
                continue;

            float d = hits[i].distance;
            if (d < closest)
            {
                closest = d;
                foundBlocking = true;
            }
        }

        if (!foundBlocking)
            return currentPos + delta;

        float allowed = Mathf.Max(0f, closest - collisionStopOffset);
        float finalDist = Mathf.Min(moveDist, allowed);

        return currentPos + dir * finalDist;
    }

    private bool IsPassThroughCollider(Collider col)
    {
#if UNITY_EDITOR
        if (col == null) return false;

        GameObject source =
            PrefabUtility.GetCorrespondingObjectFromSource(col.gameObject);

        if (source == null)
            return false;

        for (int i = 0; i < passThroughPrefabs.Count; i++)
        {
            if (passThroughPrefabs[i] == source)
                return true;
        }
#endif
        return false;
    }


    private static readonly RaycastHit[] _hitsCache = new RaycastHit[16];

    private IEnumerator ReturnToPlayerThenSwitch()
    {
        _isActive = false;
        _inputSum = Vector3.zero;
        _currentVelocity = Vector3.zero;

        // Ensure FreeCam is winning during the pan
        int playerVal = GetPriorityValue(_cmPlayer);
        SetPriorityValue(CM_FreeCam, playerVal + freeCamPriorityBoost);

        Vector3 startPos = CM_FreeCam.transform.position;
        Quaternion startRot = CM_FreeCam.transform.rotation;

        Vector3 targetPos = _cmPlayer.transform.position;
        Quaternion targetRot = _cmPlayer.transform.rotation;

        float dur = Mathf.Max(0.0001f, returnDuration);
        float t = 0f;

        while (t < dur)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            t += dt;

            float u = Mathf.Clamp01(t / dur);
            float eased = (returnEase != null) ? returnEase.Evaluate(u) : u;

            CM_FreeCam.transform.SetPositionAndRotation(
                Vector3.LerpUnclamped(startPos, targetPos, eased),
                Quaternion.SlerpUnclamped(startRot, targetRot, eased)
            );

            yield return null;
        }

        CM_FreeCam.transform.SetPositionAndRotation(targetPos, targetRot);

        // Restore exact previous PrioritySettings structs (no int math here)
        CM_FreeCam.Priority = _freePriorityBefore;
        _cmPlayer.Priority = _playerPriorityBefore;

        if (toggleFreeCamGameObject)
            CM_FreeCam.gameObject.SetActive(false);

        _returnRoutine = null;
    }

    private IEnumerator EnterFreeCamCue()
    {
        Vector3 startPos = CM_FreeCam.transform.position;
        Quaternion rot = CM_FreeCam.transform.rotation;

        // move backwards along camera forward
        Vector3 targetPos = startPos - (rot * Vector3.forward * enterFadeDistance);

        float t = 0f;
        float dur = Mathf.Max(0.0001f, enterFadeDuration);

        while (t < dur)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            t += dt;

            float u = Mathf.Clamp01(t / dur);
            float eased = enterFadeEase != null ? enterFadeEase.Evaluate(u) : u;

            CM_FreeCam.transform.position =
                Vector3.LerpUnclamped(startPos, targetPos, eased);

            yield return null;
        }

        CM_FreeCam.transform.position = targetPos;
        _enterCueRoutine = null;
    }
}
