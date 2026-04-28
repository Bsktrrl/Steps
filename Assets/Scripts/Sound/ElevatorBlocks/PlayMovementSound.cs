using System.Collections;
using UnityEngine;

public class PlayMovementSound : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip movementSound;

    [Header("Fade")]
    [SerializeField] private float fadeDuration = 0.2f;
    [SerializeField, Range(0f, 1f)] private float targetVolume = 1f;

    [Header("3D Sound Settings")]
    [SerializeField] private bool force3DSound = true;
    [SerializeField] private float minDistance = 0f;
    [SerializeField] private float maxDistance = 12f;

    private Coroutine fadeCoroutine;
    private bool isPlaying;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("PlayMovementSound needs an AudioSource.", this);
            enabled = false;
            return;
        }

        targetVolume = audioSource.volume;

        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.clip = movementSound;
        audioSource.volume = 0f;

        if (force3DSound)
        {
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;
        }

        StopAudioInstant();
    }

    private void OnDisable()
    {
        StopAudioInstant();
    }

    public void StartAudio()
    {
        if (audioSource == null || movementSound == null)
            return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        audioSource.clip = movementSound;
        audioSource.loop = true;

        if (!audioSource.isPlaying)
            audioSource.Play();

        isPlaying = true;
        fadeCoroutine = StartCoroutine(FadeVolume(targetVolume, false));
    }

    public void StopAudio()
    {
        if (audioSource == null)
            return;

        if (!audioSource.isPlaying && !isPlaying)
            return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeVolume(0f, true));
    }

    private IEnumerator FadeVolume(float target, bool stopAfterFade)
    {
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, target, timer / fadeDuration);
            yield return null;
        }

        audioSource.volume = target;

        if (stopAfterFade)
        {
            audioSource.Stop();
            isPlaying = false;
        }

        fadeCoroutine = null;
    }

    private void StopAudioInstant()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.volume = 0f;
        }

        isPlaying = false;
        fadeCoroutine = null;
    }
}