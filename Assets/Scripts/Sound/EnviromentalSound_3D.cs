using System;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentalSound_3D : Singleton<EnviromentalSound_3D>
{
    [Header("Listener")]
    [Tooltip("The transform used as the center for distance checks. Usually the player root. If left empty, this object's transform is used.")]
    [SerializeField] private Transform listenerTarget;

    [Tooltip("If enabled, stereo panning uses the camera anchor yaw so left/right matches what the player sees.")]
    [SerializeField] private bool useCameraAnchorForPan = true;

    [Header("Detection")]
    [Tooltip("Only colliders on these layers will be considered environmental sound objects.")]
    [SerializeField] private LayerMask layer_EnviromentalSound;

    [Tooltip("How often nearby sound objects are scanned. Lower values are more responsive but cost more CPU.")]
    [Min(0.02f)]
    [SerializeField] private float scanInterval = 0.08f;

    [Tooltip("Maximum number of colliders the non-alloc overlap scan can detect at once.")]
    [Min(16)]
    [SerializeField] private int overlapBufferSize = 256;

    [Header("Listener Smoothing")]
    [Tooltip("Smooths the listener position used for environmental audio.")]
    [Min(0.01f)]
    [SerializeField] private float listenerPositionSmoothTime = 0.12f;

    [Tooltip("Smooths the listener yaw used for stereo panning.")]
    [Min(0.01f)]
    [SerializeField] private float listenerYawSmoothTime = 0.18f;

    [Header("Per-Type Sound Channels")]
    [Tooltip("Each entry represents one looping environmental sound category.")]
    [SerializeField] private List<SoundChannelSettings> channels = new List<SoundChannelSettings>();

    [Header("Debug")]
    [SerializeField] private bool debugLogging = false;

    private readonly Dictionary<SoundObjectType, SoundChannelSettings> channelLookup = new Dictionary<SoundObjectType, SoundChannelSettings>();
    private readonly Dictionary<SoundObjectType, ChannelRuntime> runtimeLookup = new Dictionary<SoundObjectType, ChannelRuntime>();

    private Collider[] overlapBuffer;
    private float maxSearchRadius = 0f;
    private float scanTimer = 0f;

    private Vector3 smoothedListenerPosition;
    private Vector3 listenerPositionVelocity;

    private float smoothedListenerYaw;
    private float listenerYawVelocity;

    [Serializable]
    public class SoundChannelSettings
    {
        [Header("Identity")]
        [Tooltip("The SoundObjectType this channel reacts to.")]
        public SoundObjectType soundType = SoundObjectType.None;

        [Tooltip("The looping AudioSource used for this sound type. Assign a source with its loop clip already set in the Inspector.")]
        public AudioSource audioSource;

        [Header("Distance / Loudness")]
        [Tooltip("How far away this sound type can be detected.")]
        [Min(0.1f)]
        public float searchRadius = 5f;

        [Tooltip("Maximum volume this channel can reach when very close.")]
        [Range(0f, 1f)]
        public float maxVolume = 1f;

        [Tooltip("Controls how volume falls off over normalized distance. X=0 is very close. X=1 is at the search radius edge.")]
        public AnimationCurve distanceVolumeCurve = DefaultDistanceCurve();

        [Tooltip("Invert the evaluated curve result. Enable this if a saved inspector curve is upside down.")]
        public bool invertDistanceCurve = false;

        [Header("Smoothing")]
        [Tooltip("How quickly this channel changes volume.")]
        [Min(0.01f)]
        public float volumeSmoothTime = 0.2f;

        [Tooltip("How quickly this channel changes stereo pan.")]
        [Min(0.01f)]
        public float panSmoothTime = 0.2f;

        [Header("Stereo")]
        [Tooltip("How strongly left/right positioning affects this sound.")]
        [Range(0f, 1f)]
        public float panStrength = 0.4f;

        [Header("Playback Thresholds")]
        [Tooltip("If target volume rises above this, the source starts or resumes.")]
        [Range(0f, 0.25f)]
        public float playThreshold = 0.02f;

        [Tooltip("If both target volume and actual volume are below this, the source pauses.")]
        [Range(0f, 0.25f)]
        public float pauseThreshold = 0.005f;

        public static AnimationCurve DefaultDistanceCurve()
        {
            return new AnimationCurve(
                new Keyframe(0f, 1f),
                new Keyframe(1f, 0f)
            );
        }
    }

    private class ChannelRuntime
    {
        public float targetVolume;
        public float targetPan;

        public float currentVolumeVelocity;
        public float currentPanVelocity;

        public float lastNearestDistance = -1f;
    }

    private struct ScanAggregate
    {
        public bool found;
        public float nearestDistance;
        public float weightedPanSum;
        public float weightSum;
    }

    private void Start()
    {
        if (listenerTarget == null)
            listenerTarget = transform;

        RebuildRuntimeData();

        smoothedListenerPosition = listenerTarget.position;
        smoothedListenerYaw = GetTargetYaw();
    }

    private void Update()
    {
        if (listenerTarget == null)
            listenerTarget = transform;

        UpdateSmoothedListener();

        scanTimer -= Time.deltaTime;
        if (scanTimer <= 0f)
        {
            scanTimer = scanInterval;
            ScanEnvironment();
        }

        ApplyAudioSmoothing();
    }

    private void OnValidate()
    {
        if (scanInterval < 0.02f)
            scanInterval = 0.02f;

        if (overlapBufferSize < 16)
            overlapBufferSize = 16;

        if (listenerPositionSmoothTime < 0.01f)
            listenerPositionSmoothTime = 0.01f;

        if (listenerYawSmoothTime < 0.01f)
            listenerYawSmoothTime = 0.01f;

        if (channels == null)
            return;

        foreach (var channel in channels)
        {
            if (channel == null)
                continue;

            if (channel.searchRadius < 0.1f)
                channel.searchRadius = 0.1f;

            if (channel.volumeSmoothTime < 0.01f)
                channel.volumeSmoothTime = 0.01f;

            if (channel.panSmoothTime < 0.01f)
                channel.panSmoothTime = 0.01f;

            if (channel.playThreshold < channel.pauseThreshold)
                channel.playThreshold = channel.pauseThreshold;
        }
    }

    [ContextMenu("Rebuild Runtime Data")]
    private void RebuildRuntimeData()
    {
        BuildLookup();
        PrepareAudioSources();
        overlapBuffer = new Collider[Mathf.Max(16, overlapBufferSize)];
    }

    private void BuildLookup()
    {
        channelLookup.Clear();
        runtimeLookup.Clear();
        maxSearchRadius = 0f;

        for (int i = 0; i < channels.Count; i++)
        {
            SoundChannelSettings channel = channels[i];
            if (channel == null || channel.soundType == SoundObjectType.None || channel.audioSource == null)
                continue;

            if (channelLookup.ContainsKey(channel.soundType))
            {
                Debug.LogWarning($"Duplicate environmental sound channel found for type '{channel.soundType}'. Only the last one will be used.", this);
            }

            channelLookup[channel.soundType] = channel;
            runtimeLookup[channel.soundType] = new ChannelRuntime();

            if (channel.searchRadius > maxSearchRadius)
                maxSearchRadius = channel.searchRadius;
        }
    }

    private void PrepareAudioSources()
    {
        for (int i = 0; i < channels.Count; i++)
        {
            SoundChannelSettings channel = channels[i];
            if (channel == null || channel.audioSource == null)
                continue;

            AudioSource source = channel.audioSource;
            source.playOnAwake = false;
            source.loop = true;
            source.spatialBlend = 0f;
            source.dopplerLevel = 0f;
        }
    }

    private void UpdateSmoothedListener()
    {
        smoothedListenerPosition = Vector3.SmoothDamp(
            smoothedListenerPosition,
            listenerTarget.position,
            ref listenerPositionVelocity,
            listenerPositionSmoothTime
        );

        float targetYaw = GetTargetYaw();

        smoothedListenerYaw = Mathf.SmoothDampAngle(
            smoothedListenerYaw,
            targetYaw,
            ref listenerYawVelocity,
            listenerYawSmoothTime
        );
    }

    private float GetTargetYaw()
    {
        if (useCameraAnchorForPan)
        {
            Transform camT = CameraController.Instance?.cameraAnchor?.transform;
            if (camT != null)
                return camT.eulerAngles.y;
        }

        return listenerTarget != null ? listenerTarget.eulerAngles.y : transform.eulerAngles.y;
    }

    private float EvaluateDistanceCurve(SoundChannelSettings channel, float normalizedDistance)
    {
        float value = channel.distanceVolumeCurve != null
            ? channel.distanceVolumeCurve.Evaluate(normalizedDistance)
            : 1f - normalizedDistance;

        if (channel.invertDistanceCurve)
            value = 1f - value;

        return Mathf.Clamp01(value);
    }

    private void ScanEnvironment()
    {
        if (maxSearchRadius <= 0f || overlapBuffer == null || overlapBuffer.Length == 0)
        {
            ResetTargetsToSilence();
            return;
        }

        Dictionary<SoundObjectType, ScanAggregate> aggregates = new Dictionary<SoundObjectType, ScanAggregate>();

        int hitCount = Physics.OverlapSphereNonAlloc(
            smoothedListenerPosition,
            maxSearchRadius,
            overlapBuffer,
            layer_EnviromentalSound
        );

        Vector3 listenerRight = Quaternion.Euler(0f, smoothedListenerYaw, 0f) * Vector3.right;

        for (int i = 0; i < hitCount; i++)
        {
            Collider col = overlapBuffer[i];
            if (col == null)
                continue;

            if (!col.TryGetComponent<SoundObject>(out SoundObject soundObject))
                continue;

            if (!channelLookup.TryGetValue(soundObject.soundObjectType, out SoundChannelSettings channel))
                continue;

            Vector3 toObj = col.transform.position - smoothedListenerPosition;
            float distance = toObj.magnitude;

            if (distance > channel.searchRadius)
                continue;

            float normalizedDistance = Mathf.Clamp01(distance / Mathf.Max(0.001f, channel.searchRadius));
            float weight = EvaluateDistanceCurve(channel, normalizedDistance);

            ScanAggregate aggregate;
            if (!aggregates.TryGetValue(soundObject.soundObjectType, out aggregate))
            {
                aggregate = new ScanAggregate
                {
                    found = true,
                    nearestDistance = distance,
                    weightedPanSum = 0f,
                    weightSum = 0f
                };
            }
            else
            {
                if (distance < aggregate.nearestDistance)
                    aggregate.nearestDistance = distance;
            }

            Vector3 flat = Vector3.ProjectOnPlane(toObj, Vector3.up);
            if (flat.sqrMagnitude > 0.000001f && weight > 0f)
            {
                float pan = Vector3.Dot(listenerRight, flat.normalized);
                aggregate.weightedPanSum += pan * weight;
                aggregate.weightSum += weight;
            }

            aggregate.found = true;
            aggregates[soundObject.soundObjectType] = aggregate;
        }

        foreach (var pair in channelLookup)
        {
            SoundObjectType type = pair.Key;
            SoundChannelSettings channel = pair.Value;
            ChannelRuntime runtime = runtimeLookup[type];

            if (aggregates.TryGetValue(type, out ScanAggregate aggregate) && aggregate.found)
            {
                float normalizedNearestDistance = Mathf.Clamp01(
                    aggregate.nearestDistance / Mathf.Max(0.001f, channel.searchRadius)
                );

                float volumeMultiplier = EvaluateDistanceCurve(channel, normalizedNearestDistance);
                runtime.targetVolume = channel.maxVolume * volumeMultiplier;

                if (aggregate.weightSum > 0f)
                    runtime.targetPan = Mathf.Clamp((aggregate.weightedPanSum / aggregate.weightSum) * channel.panStrength, -1f, 1f);
                else
                    runtime.targetPan = 0f;

                runtime.lastNearestDistance = aggregate.nearestDistance;

                if (debugLogging)
                {
                    Debug.Log($"{type}: nearest={aggregate.nearestDistance:F2}, normalized={normalizedNearestDistance:F2}, targetVolume={runtime.targetVolume:F2}, targetPan={runtime.targetPan:F2}", this);
                }
            }
            else
            {
                runtime.targetVolume = 0f;
                runtime.targetPan = 0f;
                runtime.lastNearestDistance = -1f;
            }
        }

        if (hitCount == overlapBuffer.Length)
        {
            Debug.LogWarning("EnviromentalSound_3D overlap buffer is full. Increase Overlap Buffer Size.", this);
        }
    }

    private void ApplyAudioSmoothing()
    {
        foreach (var pair in channelLookup)
        {
            SoundChannelSettings channel = pair.Value;
            ChannelRuntime runtime = runtimeLookup[pair.Key];
            AudioSource source = channel.audioSource;

            if (source == null)
                continue;

            source.volume = Mathf.SmoothDamp(
                source.volume,
                runtime.targetVolume,
                ref runtime.currentVolumeVelocity,
                Mathf.Max(0.001f, channel.volumeSmoothTime)
            );

            source.panStereo = Mathf.SmoothDamp(
                source.panStereo,
                runtime.targetPan,
                ref runtime.currentPanVelocity,
                Mathf.Max(0.001f, channel.panSmoothTime)
            );

            if (runtime.targetVolume > channel.playThreshold)
            {
                if (!source.isPlaying)
                    source.Play();
            }
            else if (runtime.targetVolume <= channel.pauseThreshold && source.volume <= channel.pauseThreshold)
            {
                if (source.isPlaying)
                    source.Pause();
            }
        }
    }

    private void ResetTargetsToSilence()
    {
        foreach (var pair in runtimeLookup)
        {
            pair.Value.targetVolume = 0f;
            pair.Value.targetPan = 0f;
            pair.Value.lastNearestDistance = -1f;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 center = listenerTarget != null ? listenerTarget.position : transform.position;

        if (channels == null)
            return;

        Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.15f);

        for (int i = 0; i < channels.Count; i++)
        {
            var channel = channels[i];
            if (channel == null || channel.soundType == SoundObjectType.None)
                continue;

            Gizmos.DrawWireSphere(center, channel.searchRadius);
        }
    }
#endif
}

public enum SoundObjectType
{
    None,

    Water,
    Waterfall,

    Swampwater,
    Swampwaterfall,

    Mud,
    Mudfall,

    Lava,
    Lavafall,

    Quicksand,
    Quicksandfall,

    IceCrack,

    Checkpoint,
    RefillSteps,
    Teleporter,
    MoveableObject,
    MushroomCircle
}