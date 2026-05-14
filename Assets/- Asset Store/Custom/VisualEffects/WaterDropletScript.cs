using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDropletScript : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] AudioClip[] waterSound;
    [SerializeField] AudioClip[] lavaSound;
    [SerializeField] AudioClip[] softSound;
    [SerializeField] AudioClip[] hardSound;

    float waterPitch = 0.1f;
    float lavaPitch = 0.5f;
    float softPitch = -0.2f;
    float hardPitch = -0.2f;
    float extraPitch;

    float pitchMin = 0.9f;
    float pitchMax = 1.1f;

    enum AudioSurface
    {
        Water = 0,
        Lava = 1,
        Soft = 2,
        Hard = 3
    }

    [Header("Impact Surface")]
    [SerializeField] AudioSurface surfaceType;

    AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();

        switch (surfaceType)
        {
            case AudioSurface.Water:
                extraPitch = waterPitch;
                break;
            case AudioSurface.Lava:
                extraPitch = lavaPitch;
                break;
            case AudioSurface.Soft:
                extraPitch = softPitch;
                break;
            case AudioSurface.Hard:
                extraPitch = hardPitch;
                break;
        }
    }

    private void OnParticleCollision()
    {
        switch (surfaceType)
        {
            case AudioSurface.Water:
                source.clip = waterSound[Random.Range(0, waterSound.Length)];
                break;
            case AudioSurface.Lava:
                source.clip = lavaSound[Random.Range(0, lavaSound.Length)];
                break;
            case AudioSurface.Soft:
                source.clip = softSound[Random.Range(0, softSound.Length)];
                break;
            case AudioSurface.Hard:
                source.clip = hardSound[Random.Range(0, hardSound.Length)];
                break;
        }

        source.pitch = Random.Range(pitchMin, pitchMax) + extraPitch;
        source.Play();
    }
}