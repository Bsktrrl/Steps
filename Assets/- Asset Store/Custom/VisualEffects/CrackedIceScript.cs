using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedIceScript : MonoBehaviour
{
    [SerializeField] ParticleSystem PS1;
    [SerializeField] ParticleSystem PS2;
    AudioSource source;

    [SerializeField] AudioClip[] crackSounds;
    [SerializeField] AudioClip[] breakSounds;

    float pitchMin = 0.9f;
    float pitchMax = 1.1f;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Crack();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Break();
        }
    }

    void Crack()
    {
        source.clip = crackSounds[Random.Range(0, crackSounds.Length)];
        source.pitch = Random.Range(pitchMin, pitchMax);

        PS1.Play();
        source.Play();
    }
    void Break()
    {
        source.clip = breakSounds[Random.Range(0, breakSounds.Length)];
        source.pitch = Random.Range(pitchMin, pitchMax);

        PS2.Play();
        source.Play();
    }
}
