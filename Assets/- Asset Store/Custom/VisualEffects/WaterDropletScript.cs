using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDropletScript : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] AudioClip waterSound;
    [SerializeField] AudioClip lavaSound;
    [SerializeField] AudioClip softSound;
    [SerializeField] AudioClip hardSound;

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
        AudioClip[] audioClips = new AudioClip[] {waterSound, lavaSound, softSound, hardSound};

        source = GetComponent<AudioSource>();
        source.clip = audioClips[(int)surfaceType];
        print("AudioClip Changed");
    }

    private void OnParticleCollision()
    {
        source.Play();
    }
}
