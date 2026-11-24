using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreeCam : Singleton<FreeCam>
{
    [Header("References")]
    [SerializeField] public CinemachineCamera CM_FreeCam;
    [SerializeField] private CanvasGroup freeCamUI;

    private const float UIFadeDuration = 0.25f;
    private static readonly float[] SpeedMultipliers = { 0.5f, 0.75f, 1f, 1.5f, 2f };
    private static readonly float[] LookMultipliers = { 0.5f, 0.75f, 1f, 1.25f, 1.5f };

    [Header("Switching")]
    [Tooltip("If true, the FreeCam GameObject will be enabled/disabled when entering/exiting.")]
    [SerializeField] private bool toggleFreeCamGameObject = false;

    [Tooltip("Extra priority added to FreeCam while active (higher wins).")]
    [SerializeField] private int freeCamPriorityBoost = 10;

    [Header("Flight")]
    [Min(0.1f)] public float moveSpeed = 6f;
    public bool moveInLookDirection = true;
    [Tooltip("Higher values make the camera reach its desired velocity sooner.")]
    [Min(0.1f)] public float acceleration = 16f;
    [Tooltip("Higher values make the camera stop sooner when input is released.")]
    [Min(0.1f)] public float deceleration = 28f;
    [Min(1f)] public float boostMultiplier = 2.5f;
    [SerializeField] private bool useUnscaledTime = true;

    [Header("Scroll Speed")]
    public bool allowScrollSpeedAdjust = true;
    [Min(0f)] public float scrollSpeedStep = 0.75f;
    [Min(0.1f)] public float minMoveSpeed = 1f;
    [Min(0.1f)] public float maxMoveSpeed = 20f;

    [Header("Flight Collision")]
    [Min(0.01f)] public float collisionProbeRadius = 0.2f;
    [Min(0f)] public float collisionStopOffset = 0.03f;
    [SerializeField] private LayerMask obstacleLayers = Physics.DefaultRaycastLayers;
    [SerializeField] private bool ignoreTriggers = true;
    [Tooltip("Allow flight through objects marked with FreeCamPassThrough.")]
    [SerializeField] private bool usePassThroughMarker = true;

    [Header("Return Pan (EndFreeCam)")]
    [Tooltip("Duration of the smooth pan back to the player camera pose.")]
    [SerializeField] private float returnDuration = 0.35f;

    [Tooltip("Easing curve for the return pan (0..1).")]
    [SerializeField] private AnimationCurve returnEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("FreeCam Enter Cue")]
    [Min(0f)] [SerializeField] private float enterFadeDistance = 1f;
    [Min(0.0001f)] [SerializeField] private float enterFadeDuration = 0.4f;

    [Header("Flight Look")]
    [Tooltip("Degrees per pixel of mouse movement.")]
    [Min(0f)] public float mouseSensitivity = 0.15f;
    [Tooltip("Degrees per second at full stick deflection.")]
    [Min(0f)] public float stickSensitivity = 120f;
    [Min(0.1f)] public float stickLookResponse = 22f;
    [Tooltip("Higher values give finer control near the center of the look stick.")]
    [Min(1f)] public float stickLookExponent = 1.6f;
    public bool invertY;
    [Range(0f, 0.9f)] public float lookStickDeadzone = 0.15f;
    [Range(0f, 0.9f)] public float moveStickDeadzone = 0.15f;

    private PrioritySettings _freeBasePriority;
    private bool _cameraDetached;
    private TransformHome _cameraHome;
    private TransformHome _listenerHome;
    private Transform _listener;
    private Pose _listenerEntryStart;
    private float _listenerEntryProgress;

    private CinemachineCamera _cmPlayer;
    private CinemachineBrain _brain;
    public bool _isActive;
    internal bool IsReturningToPlayer => _returnRoutine != null;
    private Coroutine _returnRoutine;
    private Coroutine _enterCueRoutine;
    private Vector3? _entryCueTarget;

    private float EntryCueSpeed => Mathf.Max(MinimumFlightDistance, enterFadeDistance) /
        Mathf.Max(0.0001f, enterFadeDuration);

    private const float PitchLimit = 85f;
    private const float ContactTolerance = 0.001f;
    private const float MinimumFlightDistance = 0.00001f;
    private const float EntryRecoveryDistanceReduction = 0.3f;
    private const int SlideIterations = 4;
    private Vector3 _currentVelocity;
    private Vector2 _stickLookRate;
    private int _flightSpeedLevel;
    private float _flightSpeed;
    private float _lookMultiplier = 1f;
    private float _yaw;
    private float _pitch;

    // Cached hits
    private static readonly RaycastHit[] _hitsCache = new RaycastHit[16];
    private static readonly Collider[] _overlapCache = new Collider[16];
    private static readonly Vector3[] PlayerClearanceDirections =
    {
        Vector3.up, Vector3.down, Vector3.right, Vector3.left, Vector3.forward, Vector3.back
    };

    private void OnEnable()
    {
        Player_KeyInputs.Action_FreeCamIsActive += StartFreeCam;
        Player_KeyInputs.Action_FreeCamIsPassive += EndFreeCam;
        Movement.Action_RespawnPlayerEarly += ResetForRespawn;
        SettingsManager.Action_SetNewFreeCamSettings += ApplyFlightSettings;
    }

    private void OnDisable()
    {
        Player_KeyInputs.Action_FreeCamIsActive -= StartFreeCam;
        Player_KeyInputs.Action_FreeCamIsPassive -= EndFreeCam;
        Movement.Action_RespawnPlayerEarly -= ResetForRespawn;
        SettingsManager.Action_SetNewFreeCamSettings -= ApplyFlightSettings;

        StopAndRestoreFreeCam();

        if (freeCamUI)
        {
            freeCamUI.alpha = 0f;
            freeCamUI.gameObject.SetActive(false);
        }
    }

    private void ResetForRespawn()
    {
        if (!_isActive && !_cameraDetached) return;

        Player_KeyInputs.Instance.freeCam_isPressed = false;
        var playerCamera = CameraController.Instance.CM_Player;
        if (playerCamera)
            _cmPlayer = playerCamera;

        _entryCueTarget = null;
        EndFreeCam();
    }

    internal void PrepareForLevelReload()
    {
        if (!_isActive && !_cameraDetached) return;

        StopAndRestoreFreeCam();
        Player_KeyInputs inputs = GetComponentInParent<Player_KeyInputs>();
        if (inputs)
            inputs.freeCam_isPressed = false;
    }

    private void StopAndRestoreFreeCam()
    {
        StopAllCoroutines();
        _enterCueRoutine = null;
        _returnRoutine = null;
        _entryCueTarget = null;
        _isActive = false;
        RestoreCameraHierarchy();
    }

    private void Update()
    {
        UpdateUIFade();
        if (!_isActive || CM_FreeCam == null) return;

        if (PauseMenuManager.Instance.isActive)
        {
            StopEnterCue();
            ResetFlight();
            return;
        }

        float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        if (dt <= 0f) return;

        if (!Application.isFocused)
            ResetFlight();
        else if (_enterCueRoutine == null)
            TickFlight(dt);

        if (_listener && _listenerEntryProgress < 1f)
        {
            _listenerEntryProgress = Mathf.Clamp01(_listenerEntryProgress + dt / Mathf.Max(0.0001f, returnDuration));
            Transform cameraTransform = CM_FreeCam.transform;
            BlendListener(
                _listenerEntryStart,
                new Pose(cameraTransform.position, cameraTransform.rotation),
                Mathf.SmoothStep(0f, 1f, _listenerEntryProgress));
        }
    }

    private void UpdateUIFade()
    {
        if (!freeCamUI || !freeCamUI.gameObject.activeSelf) return;

        float targetAlpha = _isActive ? 1f : 0f;
        if (freeCamUI.alpha != targetAlpha)
            freeCamUI.alpha = Mathf.MoveTowards(freeCamUI.alpha, targetAlpha, Time.unscaledDeltaTime / UIFadeDuration);

        if (!_isActive && freeCamUI.alpha == 0f)
            freeCamUI.gameObject.SetActive(false);
    }

    // ====================
    // MODE SWITCH
    // ====================

    private void StartFreeCam()
    {
        if (_isActive) return;
        if (CM_FreeCam == null) return;

        CameraController controller = CameraController.Instance;
        if (!controller) return;

        _cmPlayer = controller.cameraState == CameraState.CeilingCam && controller.CM_Player_CeilingGrab
            ? controller.CM_Player_CeilingGrab
            : controller.CM_Player;
        if (_cmPlayer == null) return;
        _brain = controller.CM_Brain;
        ApplyFlightSettings();

        bool resuming = _returnRoutine != null;
        if (_returnRoutine != null)
        {
            StopCoroutine(_returnRoutine);
            _returnRoutine = null;
        }

        StopEnterCue();
        DetachCamera();
        _listenerEntryProgress = 0f;
        if (_listener)
            _listenerEntryStart = new Pose(_listener.position, _listener.rotation);

        CM_FreeCam.Follow = null;
        CM_FreeCam.LookAt = null;

        if (!resuming)
        {
            _entryCueTarget = null;
            MatchPlayerCamera();
        }

        Vector3 e = CM_FreeCam.transform.rotation.eulerAngles;
        _yaw = e.y;
        _pitch = Mathf.Clamp(Mathf.DeltaAngle(0f, e.x), -PitchLimit, PitchLimit);

        if (toggleFreeCamGameObject)
            CM_FreeCam.gameObject.SetActive(true);

        int playerVal = GetPriorityValue(_cmPlayer);
        SetPriorityValue(CM_FreeCam, playerVal + freeCamPriorityBoost);

        _isActive = true;
        if (freeCamUI) freeCamUI.gameObject.SetActive(true);
        
        ResetFlight();
        if (!resuming || _entryCueTarget.HasValue || HasBlockingOverlap(CM_FreeCam.transform.position))
            _enterCueRoutine = StartCoroutine(EnterFreeCamCue());

        if (_listener)
            _listener.SetPositionAndRotation(_listenerEntryStart.position, _listenerEntryStart.rotation);

        RenderHiderOnContact.Instance.FreeCamOn();
        HoleShaderOnOffScript.Instance.HoleShader_Off();
    }

    private void EndFreeCam()
    {
        if (!_isActive) return;

        StopEnterCue();
        RenderHiderOnContact.Instance.FreeCamOff();
        HoleShaderOnOffScript.Instance.HoleShader_On();

        _isActive = false;
        if (CM_FreeCam == null || _cmPlayer == null)
        {
            RestoreCameraHierarchy();
            return;
        }

        _returnRoutine = StartCoroutine(ReturnToPlayerThenSwitch());
    }

    private void StopEnterCue()
    {
        if (_enterCueRoutine == null) return;
        StopCoroutine(_enterCueRoutine);
        _enterCueRoutine = null;
    }

    // ====================
    // FLIGHT
    // ====================

    private void ResetFlight()
    {
        _currentVelocity = Vector3.zero;
        _stickLookRate = Vector2.zero;
    }

    private void TickFlight(float dt)
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;
        var gamepad = Gamepad.current;

        if (allowScrollSpeedAdjust && mouse != null)
        {
            float scroll = mouse.scroll.ReadValue().y;
            if (scroll != 0f)
                _flightSpeed = Mathf.Clamp(_flightSpeed + Mathf.Sign(scroll) * scrollSpeedStep,
                    minMoveSpeed, Mathf.Max(minMoveSpeed, maxMoveSpeed));
        }

        Transform cameraTransform = CM_FreeCam.transform;
        TickLook(cameraTransform, mouse, gamepad, dt);

        Vector2 planar = keyboard == null ? Vector2.zero : new Vector2(
            Axis(keyboard.aKey.isPressed, keyboard.dKey.isPressed),
            Axis(keyboard.sKey.isPressed, keyboard.wKey.isPressed));
        bool down = keyboard != null && keyboard.qKey.isPressed;
        bool up = keyboard != null && keyboard.eKey.isPressed;
        bool boost = keyboard != null && keyboard.shiftKey.isPressed;

        if (gamepad != null)
        {
            Vector2 padMove = gamepad.dpad.ReadValue();
            Vector2 stickMove = ApplyStickDeadzone(gamepad.leftStick.ReadUnprocessedValue(), moveStickDeadzone);
            if (planar == Vector2.zero)
                planar = padMove != Vector2.zero ? padMove : stickMove;

            down |= gamepad.leftShoulder.isPressed;
            up |= gamepad.rightShoulder.isPressed;
            boost |= gamepad.leftStickButton.isPressed;
        }

        planar = Vector2.ClampMagnitude(planar, 1f);
        Quaternion heading = Quaternion.Euler(0f, _yaw, 0f);
        Vector3 forward = moveInLookDirection ? cameraTransform.forward : heading * Vector3.forward;
        Vector3 input = (heading * Vector3.right) * planar.x + forward * planar.y + Vector3.up * Axis(down, up);
        float speed = Mathf.Max(0f, _flightSpeed) * (boost ? Mathf.Max(1f, boostMultiplier) : 1f);
        Vector3 desiredVelocity = Vector3.ClampMagnitude(input, 1f) * speed;
        bool braking = desiredVelocity == Vector3.zero;
        float response = Mathf.Max(0.1f, braking ? deceleration : acceleration);
        float blend = 1f - Mathf.Exp(-response * dt);
        Vector3 velocityDifference = _currentVelocity - desiredVelocity;

        // Integrate the acceleration curve over this frame so distance is consistent across frame rates.
        Vector3 displacement = desiredVelocity * dt + velocityDifference * (blend / response);
        _currentVelocity -= velocityDifference * blend;
        if (braking && _currentVelocity.sqrMagnitude < 0.0001f)
            _currentVelocity = Vector3.zero;

        if (displacement.sqrMagnitude >= MinimumFlightDistance * MinimumFlightDistance)
        {
            Vector3 position = cameraTransform.position;
            cameraTransform.position = SweepFlight(position, displacement);
            if (cameraTransform.position != position)
                _entryCueTarget = null;
        }
    }

    private void TickLook(Transform cameraTransform, Mouse mouse, Gamepad gamepad, float dt)
    {
        Vector2 mouseAngle = mouse == null ? Vector2.zero : mouse.delta.ReadValue() * (mouseSensitivity * _lookMultiplier);
        Vector2 stick = gamepad == null ? Vector2.zero :
            ApplyStickDeadzone(gamepad.rightStick.ReadUnprocessedValue(), lookStickDeadzone);
        float magnitude = stick.magnitude;
        Vector2 desiredRate = magnitude > 0f
            ? stick * (Mathf.Pow(magnitude, Mathf.Max(1f, stickLookExponent) - 1f) * stickSensitivity * _lookMultiplier)
            : Vector2.zero;

        float response = Mathf.Max(0.1f, stickLookResponse);
        float blend = 1f - Mathf.Exp(-response * dt);
        Vector2 rateDifference = _stickLookRate - desiredRate;
        Vector2 angle = mouseAngle + desiredRate * dt + rateDifference * (blend / response);
        _stickLookRate -= rateDifference * blend;
        if (desiredRate == Vector2.zero && _stickLookRate.sqrMagnitude < 0.01f)
            _stickLookRate = Vector2.zero;

        if (angle == Vector2.zero) return;

        _entryCueTarget = null;
        _yaw = Mathf.Repeat(_yaw + angle.x, 360f);
        _pitch = Mathf.Clamp(_pitch + (invertY ? angle.y : -angle.y), -PitchLimit, PitchLimit);
        cameraTransform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
    }

    private void ApplyFlightSettings()
    {
        SettingData settings = DataManager.Instance.settingData_StoreList;
        int speedLevel = SettingData.NormalizeFreeCamLevel(settings?.currentFreeCamSpeed ?? 0, SettingData.DefaultFreeCamSpeed);
        if (_flightSpeedLevel != speedLevel)
        {
            _flightSpeedLevel = speedLevel;
            _flightSpeed = moveSpeed * SpeedMultipliers[speedLevel - 1];
        }

        int lookLevel = SettingData.NormalizeFreeCamLevel(settings?.currentFreeCamSensitivity ?? 0, SettingData.DefaultFreeCamSensitivity);
        _lookMultiplier = LookMultipliers[lookLevel - 1];
    }

    private static float Axis(bool negative, bool positive) => (positive ? 1f : 0f) - (negative ? 1f : 0f);

    private static Vector2 ApplyStickDeadzone(Vector2 input, float deadzone)
    {
        float magnitude = input.magnitude;
        deadzone = Mathf.Clamp(deadzone, 0f, 0.99f);
        if (magnitude <= deadzone) return Vector2.zero;

        return (input / magnitude) * Mathf.InverseLerp(deadzone, 1f, magnitude);
    }

    private Vector3 SweepFlight(Vector3 position, Vector3 remaining)
    {
        for (int slide = 0; slide < SlideIterations; slide++)
        {
            float distance = remaining.magnitude;
            if (distance < MinimumFlightDistance) break;

            Vector3 direction = remaining / distance;
            if (!FindFlightHit(position, direction, distance, out RaycastHit hit))
                return position + remaining;

            float travel = Mathf.Max(0f, hit.distance - ContactTolerance);
            position += direction * travel;
            remaining -= direction * travel;

            remaining = Vector3.ProjectOnPlane(remaining, hit.normal);
            float inwardSpeed = Vector3.Dot(_currentVelocity, hit.normal);
            if (inwardSpeed < 0f)
                _currentVelocity -= hit.normal * inwardSpeed;
        }

        return position;
    }

    private bool FindFlightHit(Vector3 position, Vector3 direction, float distance, out RaycastHit nearest)
    {
        RaycastHit[] hits = _hitsCache;
        int count = Physics.SphereCastNonAlloc(position, FlightRadius, direction, hits, distance, obstacleLayers, FlightTriggers);

        // NonAlloc results are unordered. A full buffer may omit the closest blocking hit.
        if (count == hits.Length)
        {
            hits = Physics.SphereCastAll(position, FlightRadius, direction, distance, obstacleLayers, FlightTriggers);
            count = hits.Length;
        }

        nearest = default;
        float closest = float.PositiveInfinity;
        for (int i = 0; i < count; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.distance >= closest || !IsBlockingCollider(hit.collider) ||
                Vector3.Dot(direction, hit.normal) >= -0.0001f)
                continue;

            // A sphere cast that starts overlapped reports a zero-distance hit with no usable surface normal.
            if (hit.distance == 0f && TryGetFlightPenetration(position, hit.collider, out Vector3 away, out _) &&
                Vector3.Dot(direction, away) > 0.0001f)
                continue;

            closest = hit.distance;
            nearest = hit;
        }

        return nearest.collider != null;
    }

    private float FlightRadius => Mathf.Max(0.01f, collisionProbeRadius) + Mathf.Max(0f, collisionStopOffset);
    private QueryTriggerInteraction FlightTriggers => ignoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide;

    private bool IsBlockingCollider(Collider collider)
    {
        return collider && !collider.transform.IsChildOf(transform) &&
               (!usePassThroughMarker || !collider.GetComponentInParent<FreeCamPassThrough>());
    }

    private bool TryGetFlightPenetration(Vector3 position, Collider obstacle, out Vector3 away, out float depth)
    {
        // Only a convex obstacle has a separating plane we can safely move away from.
        if (obstacle is MeshCollider { convex: false })
        {
            away = default;
            depth = 0f;
            return false;
        }

        SphereCollider probe = RuntimeUtility.GetScratchCollider();
        probe.radius = FlightRadius;
        return Physics.ComputePenetration(probe, position, Quaternion.identity,
            obstacle, obstacle.transform.position, obstacle.transform.rotation, out away, out depth);
    }

    private bool HasBlockingOverlap(Vector3 position)
    {
        Collider[] overlaps = _overlapCache;
        int count = Physics.OverlapSphereNonAlloc(position, FlightRadius, overlaps, obstacleLayers, FlightTriggers);
        if (count == overlaps.Length)
        {
            overlaps = Physics.OverlapSphere(position, FlightRadius, obstacleLayers, FlightTriggers);
            count = overlaps.Length;
        }

        for (int i = 0; i < count; i++)
            if (IsBlockingCollider(overlaps[i]))
                return true;
        return false;
    }

    private Vector3 GetClearEntryPosition(Vector3 position)
    {
        // Start on the player's side. The nearest surface normal from inside the wall
        // can point away from the player, and a cast starting there can miss the wall.
        Transform followTarget = _cmPlayer.Follow;
        if (followTarget && TryGetPlayerSideEntryPosition(followTarget.position, position, out Vector3 target))
            return target;

        PlayerManager player = PlayerManager.Instance;
        GameObject body = player.playerBody;
        if (body && TryGetPlayerSideEntryPosition(body.transform.position, position, out target))
            return target;

        Vector3 playerPosition = player.transform.position;
        if (TryGetPlayerSideEntryPosition(playerPosition, position, out target))
            return target;

        // The pivot can be close to the floor or ceiling even when there is room beside it.
        float offset = FlightRadius + ContactTolerance;
        foreach (Vector3 direction in PlayerClearanceDirections)
        {
            if (TryGetPlayerSideEntryPosition(playerPosition + direction * offset, position, out target))
                return target;
        }

        return position;
    }

    private bool TryGetPlayerSideEntryPosition(Vector3 anchor, Vector3 cameraPosition, out Vector3 target)
    {
        target = anchor;
        if (HasBlockingOverlap(anchor))
            return false;

        Vector3 offset = cameraPosition - anchor;
        float distance = offset.magnitude;
        if (distance > MinimumFlightDistance)
        {
            Vector3 direction = offset / distance;
            if (FindFlightHit(anchor, direction, distance, out RaycastHit hit))
            {
                Vector3 candidate = anchor + direction * Mathf.Max(0f, hit.distance - ContactTolerance);
                if (!HasBlockingOverlap(candidate))
                    target = candidate;
            }
        }

        return true;
    }

    // ====================
    // COROUTINES
    // ====================

    private IEnumerator EnterFreeCamCue()
    {
        Transform cameraTransform = CM_FreeCam.transform;
        Vector3 pos = cameraTransform.position;
        Vector3 targetPos;

        if (HasBlockingOverlap(pos))
        {
            _entryCueTarget = null;
            targetPos = Vector3.MoveTowards(GetClearEntryPosition(pos), pos, EntryRecoveryDistanceReduction);
        }
        else
        {
            // Reuse the original destination if the player interrupted the short return.
            targetPos = _entryCueTarget ?? pos - cameraTransform.forward * Mathf.Max(0f, enterFadeDistance);
            Vector3 offset = targetPos - pos;
            float distance = offset.magnitude;
            // Start manual flight on the clear side of any wall behind the player camera.
            if (distance > MinimumFlightDistance && FindFlightHit(pos, offset / distance, distance, out RaycastHit hit))
                targetPos = pos + (offset / distance) * Mathf.Max(0f, hit.distance - ContactTolerance);

            _entryCueTarget = targetPos;
        }

        float speed = EntryCueSpeed;
        yield return null;

        while (cameraTransform.position != targetPos)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, targetPos, speed * dt);
            yield return null;
        }

        CM_FreeCam.transform.position = targetPos;
        _enterCueRoutine = null;
    }

    private void DetachCamera()
    {
        if (_cameraDetached) return;

        Transform cameraTransform = CM_FreeCam.transform;
        _cameraHome = new TransformHome(cameraTransform);
        _freeBasePriority = CM_FreeCam.Priority;
        _cameraDetached = true;
        cameraTransform.SetParent(null, true);

        AudioListener listener = PlayerManager.Instance.GetComponentInChildren<AudioListener>();
        if (listener)
        {
            _listener = listener.transform;
            _listenerHome = new TransformHome(_listener);
            _listener.SetParent(cameraTransform, true);
        }
    }

    private void RestoreCameraHierarchy()
    {
        if (!_cameraDetached) return;
        _cameraDetached = false;
        _entryCueTarget = null;

        _listenerHome.Restore();
        _listener = null;
        _cameraHome.Restore();
        _listenerHome = default;
        _cameraHome = default;

        if (CM_FreeCam)
        {
            CM_FreeCam.Priority = _freeBasePriority;
            if (toggleFreeCamGameObject)
                CM_FreeCam.gameObject.SetActive(false);
        }
    }

    private void MatchPlayerCamera()
    {
        var state = _cmPlayer.State;
        CM_FreeCam.transform.SetPositionAndRotation(state.GetFinalPosition(), state.GetFinalOrientation());
    }

    private void BlendListener(Pose start, Pose target, float progress)
    {
        _listener.SetPositionAndRotation(
            Vector3.Lerp(start.position, target.position, progress),
            Quaternion.Slerp(start.rotation, target.rotation, progress));
    }

    private IEnumerator ReturnToPlayerThenSwitch()
    {
        ResetFlight();

        Vector3 startPos = CM_FreeCam.transform.position;
        Quaternion startRot = CM_FreeCam.transform.rotation;
        Pose listenerStart = _listener ? new Pose(_listener.position, _listener.rotation) : default;

        float distance = Vector3.Distance(startPos, _cmPlayer.State.GetFinalPosition());
        bool reverseEntryCue = _entryCueTarget.HasValue && distance <= enterFadeDistance + ContactTolerance;
        if (!reverseEntryCue)
            _entryCueTarget = null;

        float dur = reverseEntryCue ? distance / EntryCueSpeed : Mathf.Max(0.0001f, returnDuration);
        float t = 0f;

        if (reverseEntryCue)
            yield return null;

        while (t < dur && CM_FreeCam && _cmPlayer)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            t += dt;

            float u = Mathf.Clamp01(t / dur);
            float eased = (returnEase != null) ? returnEase.Evaluate(u) : u;

            var playerState = _cmPlayer.State;
            CM_FreeCam.transform.SetPositionAndRotation(
                Vector3.LerpUnclamped(startPos, playerState.GetFinalPosition(), reverseEntryCue ? u : eased),
                Quaternion.SlerpUnclamped(startRot, playerState.GetFinalOrientation(), eased)
            );
            if (_listener)
                BlendListener(listenerStart, _listenerHome.GetWorldPose(), eased);

            yield return null;
        }

        if (CM_FreeCam && _cmPlayer)
        {
            MatchPlayerCamera();
            if (_listener)
                BlendListener(listenerStart, _listenerHome.GetWorldPose(), 1f);
            CM_FreeCam.Priority = _freeBasePriority;
            _cmPlayer.Prioritize();

            // Keep the outgoing camera at the player until Cinemachine stops sampling it.
            yield return null;
            while (CM_FreeCam && _cmPlayer && _brain && _brain.isActiveAndEnabled && _brain.IsLiveInBlend(CM_FreeCam))
            {
                MatchPlayerCamera();
                if (_listener)
                    BlendListener(listenerStart, _listenerHome.GetWorldPose(), 1f);
                yield return null;
            }
        }

        RestoreCameraHierarchy();
        _returnRoutine = null;
    }

    private readonly struct TransformHome
    {
        private readonly Transform _transform;
        private readonly Transform _parent;
        private readonly int _siblingIndex;
        private readonly Vector3 _localPosition;
        private readonly Quaternion _localRotation;
        private readonly Vector3 _localScale;

        public TransformHome(Transform target)
        {
            _transform = target;
            _parent = target.parent;
            _siblingIndex = target.GetSiblingIndex();
            _localPosition = target.localPosition;
            _localRotation = target.localRotation;
            _localScale = target.localScale;
        }

        public void Restore()
        {
            if (!_transform) return;
            _transform.SetParent(_parent, false);
            _transform.SetSiblingIndex(_siblingIndex);
            _transform.SetLocalPositionAndRotation(_localPosition, _localRotation);
            _transform.localScale = _localScale;
        }

        public Pose GetWorldPose()
        {
            return _parent
                ? new Pose(_parent.TransformPoint(_localPosition), _parent.rotation * _localRotation)
                : new Pose(_localPosition, _localRotation);
        }
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
