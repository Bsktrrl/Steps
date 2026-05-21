using UnityEngine;

public class Player_IceGlideSound : Singleton<Player_IceGlideSound>
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip _clip;

    [Header("Volume")]
    [SerializeField, Range(0f, 1f)] private float targetVolume = 1f;
    [SerializeField] private float fadeInTime = 0.02f;
    [SerializeField] private float fadeOutTime = 0.08f;

    [Header("Timing")]
    [SerializeField] private float stopDelay = 0.05f;
    [SerializeField] private float enterIceDelay = 0.45f;

    [Header("Variation")]
    [SerializeField] private bool randomizePitch = true;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.98f, 1.02f);

    [Header("Track Position")]
    [SerializeField] private float endOfClipResetThreshold = 0.03f;

    private bool _isPlaying;
    private bool _isFadingOut;
    private float _lastTimeSoundWasWanted;
    private float _currentVolume;

    private GameObject _delayedTargetBlock;
    private float _delayedStartTime;

    private int _savedPlaybackSample;


    private void Awake()
    {
        if (audioSource != null)
        {
            audioSource.volume = 0f;
            audioSource.loop = true;
        }
    }


    private void Update()
    {
        bool shouldPlay = ShouldPlayIceGlidingSound();

        if (shouldPlay)
        {
            _lastTimeSoundWasWanted = Time.time;
            Start_PlayIceGlidingSound();
            FadeTowards(targetVolume, fadeInTime);
        }
        else if (_isPlaying && Time.time - _lastTimeSoundWasWanted > stopDelay)
        {
            _isFadingOut = true;
            FadeTowards(0f, fadeOutTime);

            if (_currentVolume <= 0.001f)
            {
                Stop_PlayIceGlidingSound_Immediate();
            }
        }
    }


    private bool ShouldPlayIceGlidingSound()
    {
        Movement movement = Movement.Instance;

        if (movement == null)
            return false;

        // The player is not moving, so the glide sound should stop.
        if (!movement.isMoving && movement.GetMovementState() != MovementStates.Moving)
        {
            ClearDelayedIceStart();
            return false;
        }

        bool standingOnIce = IsIceBlock(movement.blockStandingOn);
        bool movingToIce = IsIceBlock(movement.currentMoveTargetBlock);

        // Already on ice and gliding:
        // Start immediately. No enter delay.
        if (movement.isIceGliding && standingOnIce)
        {
            ClearDelayedIceStart();
            return true;
        }

        // Moving from ice to ice:
        // Also start/continue immediately.
        if (standingOnIce && movingToIce)
        {
            ClearDelayedIceStart();
            return true;
        }

        // Moving from non-ice to ice:
        // Delay the sound so it begins closer to when the player reaches the ice block.
        if (!standingOnIce && movingToIce)
        {
            return HasDelayedEnterIceTimePassed(movement.currentMoveTargetBlock);
        }

        ClearDelayedIceStart();
        return false;
    }


    private bool HasDelayedEnterIceTimePassed(GameObject targetBlock)
    {
        if (targetBlock == null)
            return false;

        if (_delayedTargetBlock != targetBlock)
        {
            _delayedTargetBlock = targetBlock;
            _delayedStartTime = Time.time;
        }

        return Time.time - _delayedStartTime >= enterIceDelay;
    }


    private void ClearDelayedIceStart()
    {
        _delayedTargetBlock = null;
        _delayedStartTime = 0f;
    }


    private bool IsIceBlock(GameObject block)
    {
        if (block == null)
            return false;

        if (!block.TryGetComponent(out BlockInfo blockInfo))
            return false;

        return blockInfo.blockElement == BlockElement.Ice;
    }


    public void Start_PlayIceGlidingSound()
    {
        if (audioSource == null || _clip == null)
            return;

        _isFadingOut = false;

        if (_isPlaying && audioSource.isPlaying)
            return;

        audioSource.clip = _clip;
        audioSource.loop = true;

        if (randomizePitch)
            audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        else
            audioSource.pitch = 1f;

        SetSavedPlaybackPosition();

        _currentVolume = 0f;
        audioSource.volume = 0f;

        audioSource.Play();

        _isPlaying = true;
    }


    private void SetSavedPlaybackPosition()
    {
        if (audioSource == null || _clip == null)
            return;

        if (_clip.samples <= 0)
        {
            audioSource.timeSamples = 0;
            return;
        }

        _savedPlaybackSample = Mathf.Clamp(_savedPlaybackSample, 0, _clip.samples - 1);
        audioSource.timeSamples = _savedPlaybackSample;
    }


    public void Stop_PlayIceGlidingSound()
    {
        // Keep this public method for compatibility with existing Movement calls.
        // Do not stop instantly. Let Update handle the fade-out.
        _lastTimeSoundWasWanted = Time.time - stopDelay;
    }


    private void FadeTowards(float target, float fadeTime)
    {
        if (audioSource == null)
            return;

        if (fadeTime <= 0f)
        {
            _currentVolume = target;
        }
        else
        {
            float speed = 1f / fadeTime;
            _currentVolume = Mathf.MoveTowards(
                _currentVolume,
                target,
                speed * Time.deltaTime
            );
        }

        audioSource.volume = _currentVolume;
    }


    private void SavePlaybackPosition()
    {
        if (audioSource == null || _clip == null || _clip.samples <= 0)
        {
            _savedPlaybackSample = 0;
            return;
        }

        int currentSample = audioSource.timeSamples;

        int resetThresholdSamples = Mathf.RoundToInt(endOfClipResetThreshold * _clip.frequency);
        int resetFromSample = Mathf.Max(0, _clip.samples - resetThresholdSamples);

        // If the sound stopped right at the end of the clip,
        // start from the beginning next time.
        if (currentSample >= resetFromSample)
        {
            _savedPlaybackSample = 0;
            return;
        }

        _savedPlaybackSample = Mathf.Clamp(currentSample, 0, _clip.samples - 1);
    }


    private void Stop_PlayIceGlidingSound_Immediate()
    {
        SavePlaybackPosition();

        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.volume = 0f;
        }

        _currentVolume = 0f;
        _isPlaying = false;
        _isFadingOut = false;

        ClearDelayedIceStart();
    }


    private void OnDisable()
    {
        Stop_PlayIceGlidingSound_Immediate();
    }
}