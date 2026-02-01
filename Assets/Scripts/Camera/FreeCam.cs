using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private float moveSpeed = 6f;

    [Tooltip("If true, forward/back/strafe move in the camera's look direction (flycam style). If false, movement is flattened to world up.")]
    [SerializeField] private bool moveInLookDirection = true;

    [Tooltip("How quickly the camera accelerates towards target speed (bigger = snappier).")]
    [SerializeField] private float acceleration = 12f;

    [Tooltip("How quickly the camera slows down when input stops (bigger = snappier).")]
    [SerializeField] private float deceleration = 18f;

    [Tooltip("If true, uses unscaled time (recommended if you pause with Time.timeScale = 0).")]
    [SerializeField] private bool useUnscaledTime = true;

    [Header("Collision")]
    [Tooltip("How far ahead to probe for obstacles when moving.")]
    [SerializeField] private float collisionProbeDistance = 2f;

    [Tooltip("Radius for the sphere cast used to stop at walls.")]
    [SerializeField] private float collisionProbeRadius = 0.2f;

    [Tooltip("Small offset so we stop slightly before the hit surface.")]
    [SerializeField] private float collisionStopOffset = 0.02f;

    [Tooltip("Ignore trigger colliders.")]
    [SerializeField] private bool ignoreTriggers = true;

    [Header("Pass-through")]
    [Tooltip("Add FreeCamPassThrough to prefab roots you want to allow passing through.")]
    [SerializeField] private bool usePassThroughMarker = true;

    [Header("Return Pan (EndFreeCam)")]
    [Tooltip("Duration of the smooth pan back to the player camera pose.")]
    [SerializeField] private float returnDuration = 0.35f;

    [Tooltip("Easing curve for the return pan (0..1).")]
    [SerializeField] private AnimationCurve returnEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("FreeCam Enter Cue")]
    [SerializeField] private float enterFadeDistance = 1f;
    [SerializeField] private float enterFadeDuration = 0.03f;
    [SerializeField] private AnimationCurve enterFadeEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Rotation")]
    [SerializeField] private InputType inputType = InputType.Keyboard;
    public InputType CurrentInputType => inputType;

    [Tooltip("Degrees per pixel (mouse).")]
    [SerializeField] private float mouseSensitivity = 0.15f;

    [Tooltip("Degrees per second at full stick deflection.")]
    [SerializeField] private float stickSensitivity = 120f;

    [SerializeField] private bool invertY = false;

    [Tooltip("Clamp vertical rotation (pitch).")]
    [SerializeField] private float pitchMin = -85f;
    [SerializeField] private float pitchMax = 85f;

    [Header("Deadzones")]
    [Tooltip("Right stick deadzone.")]
    [SerializeField] private float lookStickDeadzone = 0.12f;

    [Tooltip("Left stick deadzone.")]
    [SerializeField] private float moveStickDeadzone = 0.18f;

    [Header("Collision Sliding (Glide)")]
    [SerializeField] private bool slideAlongSurfaces = true;

    [Tooltip("How many slide attempts per frame. 2–3 is usually enough.")]
    [SerializeField] private int slideIterations = 100;

    [Tooltip("Small distance kept from surfaces to avoid sticking/jitter.")]
    [SerializeField] private float skinWidth = 0.01f;

    [Tooltip("Extra distance added to the sweep so we detect contact reliably at high speed.")]
    [SerializeField] private float sweepPadding = 0.02f;

    [Tooltip("If we are closer than this to a surface, treat as 'touching' and slide without nudging.")]
    [SerializeField] private float touchEpsilon = 1f;

    [Tooltip("Tiny step along the slide direction when touching a surface. Helps avoid 'glued' feeling.")]
    [SerializeField] private float touchSlideStep = 1f;

    [Header("FreeCamForwardPush")]
    [SerializeField] private float freeCamForwardPush = 0.4f;

    private PrioritySettings _playerBasePriority;
    private PrioritySettings _freeBasePriority;
    private bool _basePrioritiesCached;

    private CinemachineCamera _cmPlayer;
    private bool _isActive;
    private Coroutine _returnRoutine;
    private Coroutine _enterCueRoutine;

    // Movement state
    private Vector3 _currentVelocity;     // world-space velocity (smoothed)
    private Vector3 _digitalInputSum;     // keyboard button movement (WASD-like)
    private Vector2 _moveAxis;            // controller move axis (left stick)

    // Rotation state
    private Vector2 _lookAxis;            // controller look axis (right stick)
    private float _yaw;
    private float _pitch;

    // Cached hits
    private static readonly RaycastHit[] _hitsCache = new RaycastHit[16];
    private static readonly Collider[] _overlapCache = new Collider[16];

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

    private void Update()
    {
        if (!_isActive || CM_FreeCam == null) return;

        float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        if (dt <= 0f) return;

        TickMovement(dt);
        TickRotation(dt);
    }

    // ====================
    // INPUT API (called by your input system)
    // ====================

    /// <summary>
    /// Digital movement (keyboard-style). Call on press/release.
    /// Use: Forward(0,0,1) Back(0,0,-1) Right(1,0,0) Left(-1,0,0) Up(0,1,0) Down(0,-1,0)
    /// </summary>
    public void SetMoveDirection(Vector3 localAxisDirection, bool pressed)
    {
        localAxisDirection = new Vector3(
            Mathf.Clamp(localAxisDirection.x, -1f, 1f),
            Mathf.Clamp(localAxisDirection.y, -1f, 1f),
            Mathf.Clamp(localAxisDirection.z, -1f, 1f)
        );

        if (pressed) _digitalInputSum += localAxisDirection;
        else _digitalInputSum -= localAxisDirection;

        _digitalInputSum = new Vector3(
            Mathf.Clamp(_digitalInputSum.x, -1f, 1f),
            Mathf.Clamp(_digitalInputSum.y, -1f, 1f),
            Mathf.Clamp(_digitalInputSum.z, -1f, 1f)
        );
    }

    /// <summary>
    /// Analog movement axis (left stick). Provide (-1..1, -1..1).
    /// IMPORTANT: Call on performed AND canceled so it returns to zero.
    /// </summary>
    public void SetMoveAxis(Vector2 axis)
    {
        if (!_isActive) return;

        if (axis.magnitude < moveStickDeadzone) axis = Vector2.zero;
        _moveAxis = Vector2.ClampMagnitude(axis, 1f);
    }

    /// <summary>
    /// Analog look axis (right stick). Provide (-1..1, -1..1).
    /// IMPORTANT: Call on performed AND canceled so it returns to zero.
    /// </summary>
    public void SetLookAxis(Vector2 axis)
    {
        if (!_isActive) return;

        if (axis.magnitude < lookStickDeadzone) axis = Vector2.zero;
        _lookAxis = Vector2.ClampMagnitude(axis, 1f);
    }

    // ====================
    // MODE SWITCH
    // ====================

    private void StartFreeCam()
    {
        if (_isActive) return;
        if (CM_FreeCam == null) return;

        _cmPlayer = CameraController.Instance != null ? CameraController.Instance.CM_Player : null;

        CacheBasePrioritiesOnce();

        // stop routines (BUT restore if we cancel a return)
        bool cancelledReturn = false;

        if (_returnRoutine != null)
        {
            StopCoroutine(_returnRoutine);
            _returnRoutine = null;
            cancelledReturn = true;
        }

        if (_enterCueRoutine != null)
        {
            StopCoroutine(_enterCueRoutine);
            _enterCueRoutine = null;
        }

        if (cancelledReturn)
        {
            // If we interrupted the exit pan, put priorities back in a known-good state
            RestoreBasePriorities();
        }

        if (_cmPlayer == null) return;

        // IMPORTANT: prevent Cinemachine from overriding your rotation
        CM_FreeCam.Follow = null;
        CM_FreeCam.LookAt = null;

        // snap FreeCam to PlayerCam pose
        CM_FreeCam.transform.SetPositionAndRotation(_cmPlayer.transform.position, _cmPlayer.transform.rotation);

        // init yaw/pitch from current rotation
        Vector3 e = CM_FreeCam.transform.rotation.eulerAngles;
        _yaw = e.y;
        float ex = e.x; if (ex > 180f) ex -= 360f;
        _pitch = Mathf.Clamp(ex, pitchMin, pitchMax);

        if (toggleFreeCamGameObject)
            CM_FreeCam.gameObject.SetActive(true);

        // make FreeCam win
        int playerVal = GetPriorityValue(_cmPlayer);
        SetPriorityValue(CM_FreeCam, playerVal + freeCamPriorityBoost);

        // reset inputs/state
        _isActive = true;
        _digitalInputSum = Vector3.zero;
        _moveAxis = Vector2.zero;
        _lookAxis = Vector2.zero;
        _currentVelocity = Vector3.zero;

        // enter cue
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

    // ====================
    // TICK: MOVEMENT / ROTATION
    // ====================

    private void TickMovement(float dt)
    {
        // combine digital + analog (analog uses x=strafe, y=forward)
        Vector3 analogLocal = new Vector3(_moveAxis.x, 0f, _moveAxis.y);
        Vector3 local = _digitalInputSum + analogLocal;
        local = Vector3.ClampMagnitude(local, 1f);

        Vector3 desiredWorldVelocity = Vector3.zero;

        if (local.sqrMagnitude > 0.0001f)
        {
            local = local.normalized;

            Transform t = CM_FreeCam.transform;

            Vector3 forward;
            Vector3 right;

            if (moveInLookDirection)
            {
                // Flycam: move in the actual look direction (includes pitch)
                forward = t.forward;
                right = t.right;
            }
            else
            {
                // Old: flatten forward/right to horizontal plane
                forward = Vector3.ProjectOnPlane(t.forward, Vector3.up).normalized;
                right = Vector3.ProjectOnPlane(t.right, Vector3.up).normalized;

                if (forward.sqrMagnitude < 0.0001f)
                {
                    Quaternion yawOnly = Quaternion.Euler(0f, _yaw, 0f);
                    forward = yawOnly * Vector3.forward;
                    right = yawOnly * Vector3.right;
                }
            }

            // Up/down stays world-up (still useful)
            Vector3 worldDir =
                (right * local.x) +
                (Vector3.up * local.y) +
                (forward * local.z);

            if (worldDir.sqrMagnitude > 0.0001f)
                desiredWorldVelocity = worldDir.normalized * moveSpeed;
        }

        float rate = (desiredWorldVelocity.sqrMagnitude > 0.0001f) ? acceleration : deceleration;
        _currentVelocity = Vector3.Lerp(_currentVelocity, desiredWorldVelocity, 1f - Mathf.Exp(-rate * dt));

        Vector3 delta = _currentVelocity * dt;
        if (delta.sqrMagnitude < 0.0000001f) return;

        Vector3 newPos = ComputeGlideMovement(CM_FreeCam.transform.position, delta);
        CM_FreeCam.transform.position = newPos;
    }

    private void TickRotation(float dt)
    {
        // Mouse look (Keyboard mode)
        if (inputType == InputType.Keyboard)
        {
            Vector2 mouseDelta = Mouse.current?.delta.ReadValue() ?? Vector2.zero;

            if (mouseDelta != Vector2.zero)
            {
                float ySign = invertY ? 1f : -1f;

                _yaw += mouseDelta.x * mouseSensitivity;
                _pitch += mouseDelta.y * mouseSensitivity * ySign;

                ApplyFreeCamRotation();
            }
        }

        // Controller look
        if (_lookAxis != Vector2.zero)
        {
            float ySign = invertY ? 1f : -1f;

            _yaw += _lookAxis.x * stickSensitivity * dt;
            _pitch += _lookAxis.y * stickSensitivity * dt * ySign;

            ApplyFreeCamRotation();
        }
    }

    private void ApplyFreeCamRotation()
    {
        _pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax);

        Quaternion yawRot = Quaternion.AngleAxis(_yaw, Vector3.up);
        Quaternion pitchRot = Quaternion.AngleAxis(_pitch, Vector3.right);
        CM_FreeCam.transform.rotation = yawRot * pitchRot;
    }

    // ====================
    // COLLISION (GLIDE / SLIDE)
    // ====================

    private Vector3 ComputeGlideMovement(Vector3 startPos, Vector3 delta)
    {
        if (!slideAlongSurfaces)
            return ComputeBlockedMovement_StopOnly(startPos, delta);

        if (collisionProbeRadius <= 0f)
            return startPos + delta;

        Vector3 pos = startPos;

        float remainingDist = delta.magnitude;
        if (remainingDist < 0.00001f) return pos;

        Vector3 dir = delta / remainingDist;

        int iters = Mathf.Clamp(slideIterations, 1, 6);

        float stopOffset = Mathf.Max(skinWidth, collisionStopOffset);
        float stepWhenTouching = Mathf.Max(0.001f, touchSlideStep);

        for (int iter = 0; iter < iters; iter++)
        {
            if (remainingDist < 0.00001f) break;

            float sweep = remainingDist + stopOffset + sweepPadding;

            int hitCount = Physics.SphereCastNonAlloc(
                pos,
                collisionProbeRadius,
                dir,
                _hitsCache,
                sweep,
                Physics.DefaultRaycastLayers,
                ignoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide
            );

            if (hitCount <= 0)
            {
                pos += dir * remainingDist;
                break;
            }

            int best = -1;
            float closest = float.PositiveInfinity;

            for (int i = 0; i < hitCount; i++)
            {
                Collider col = _hitsCache[i].collider;
                if (!col) continue;

                if (usePassThroughMarker && col.GetComponentInParent<FreeCamPassThrough>() != null)
                    continue;

                if (col.transform.IsChildOf(transform))
                    continue;

                float d = _hitsCache[i].distance;
                if (d < closest)
                {
                    closest = d;
                    best = i;
                }
            }

            if (best < 0)
            {
                pos += dir * remainingDist;
                break;
            }

            RaycastHit hit = _hitsCache[best];

            float moveTo = Mathf.Clamp(closest - stopOffset, 0f, remainingDist);

            Vector3 slideDir = Vector3.ProjectOnPlane(dir, hit.normal);
            if (slideDir.sqrMagnitude < 0.0000001f)
                break;

            slideDir.Normalize();

            if (moveTo <= 0.0005f)
            {
                float step = Mathf.Min(remainingDist, stepWhenTouching);

                int microHits = Physics.SphereCastNonAlloc(
                    pos,
                    collisionProbeRadius,
                    slideDir,
                    _hitsCache,
                    step + stopOffset,
                    Physics.DefaultRaycastLayers,
                    ignoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide
                );

                if (microHits > 0)
                    break;

                pos += slideDir * step;
                remainingDist -= step;
                dir = slideDir;
                continue;
            }

            pos += dir * moveTo;
            remainingDist -= moveTo;
            dir = slideDir;
        }

        return pos;
    }

    private Vector3 ComputeBlockedMovement_StopOnly(Vector3 currentPos, Vector3 delta)
    {
        float maxProbe = Mathf.Max(0f, collisionProbeDistance);
        if (maxProbe <= 0f || collisionProbeRadius <= 0f)
            return currentPos + delta;

        float moveDist = delta.magnitude;
        if (moveDist < 0.00001f) return currentPos;

        Vector3 dir = delta / moveDist;

        float probeDist = Mathf.Min(maxProbe, moveDist);

        int hitCount = Physics.SphereCastNonAlloc(
            currentPos,
            collisionProbeRadius,
            dir,
            _hitsCache,
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
            Collider col = _hitsCache[i].collider;
            if (!col) continue;

            if (usePassThroughMarker && col.GetComponentInParent<FreeCamPassThrough>() != null)
                continue;

            if (col.transform.IsChildOf(transform))
                continue;

            float d = _hitsCache[i].distance;
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

    // ====================
    // COROUTINES
    // ====================

    private IEnumerator EnterFreeCamCue()
    {
        Vector3 pos = CM_FreeCam.transform.position;
        Vector3 fwd = CM_FreeCam.transform.forward;

        int overlapCount = Physics.OverlapSphereNonAlloc(
            pos,
            Mathf.Max(0.001f, collisionProbeRadius),
            _overlapCache,
            Physics.DefaultRaycastLayers,
            ignoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide
        );

        bool overlappingBlocking = false;

        for (int i = 0; i < overlapCount; i++)
        {
            Collider col = _overlapCache[i];
            if (!col) continue;

            if (usePassThroughMarker && col.GetComponentInParent<FreeCamPassThrough>() != null)
                continue;

            if (col.transform.IsChildOf(transform))
                continue;

            overlappingBlocking = true;
            break;
        }

        if (overlappingBlocking)
        {
            CM_FreeCam.transform.position += fwd * freeCamForwardPush;
            _enterCueRoutine = null;
            yield break;
        }

        Vector3 startPos = CM_FreeCam.transform.position;
        Quaternion rot = CM_FreeCam.transform.rotation;
        Vector3 targetPos = startPos - (rot * Vector3.forward * enterFadeDistance);

        float t = 0f;
        float dur = Mathf.Max(0.0001f, enterFadeDuration);

        while (t < dur)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            t += dt;

            float u = Mathf.Clamp01(t / dur);
            float eased = enterFadeEase != null ? enterFadeEase.Evaluate(u) : u;

            CM_FreeCam.transform.position = Vector3.LerpUnclamped(startPos, targetPos, eased);
            yield return null;
        }

        CM_FreeCam.transform.position = targetPos;
        _enterCueRoutine = null;
    }

    private void CacheBasePrioritiesOnce()
    {
        if (_basePrioritiesCached) return;
        if (CM_FreeCam == null) return;

        _cmPlayer = CameraController.Instance != null ? CameraController.Instance.CM_Player : _cmPlayer;
        if (_cmPlayer == null) return;

        _playerBasePriority = _cmPlayer.Priority;
        _freeBasePriority = CM_FreeCam.Priority;
        _basePrioritiesCached = true;
    }

    private void RestoreBasePriorities()
    {
        if (!_basePrioritiesCached) return;
        if (CM_FreeCam != null) CM_FreeCam.Priority = _freeBasePriority;
        if (_cmPlayer != null) _cmPlayer.Priority = _playerBasePriority;
    }

    private IEnumerator ReturnToPlayerThenSwitch()
    {
        _isActive = false;

        _digitalInputSum = Vector3.zero;
        _moveAxis = Vector2.zero;
        _lookAxis = Vector2.zero;
        _currentVelocity = Vector3.zero;

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

        RestoreBasePriorities();

        if (toggleFreeCamGameObject)
            CM_FreeCam.gameObject.SetActive(false);

        _returnRoutine = null;
    }

    // ====================
    // PRIORITY HELPERS (CM 3.0.1)
    // ====================

    private static int GetPriorityValue(CinemachineCamera cam) => cam.Priority.Value;

    private static void SetPriorityValue(CinemachineCamera cam, int value)
    {
        var p = cam.Priority;
        p.Value = value;
        cam.Priority = p;
    }
}
