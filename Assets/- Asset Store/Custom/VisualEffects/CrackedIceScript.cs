using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedIceScript : MonoBehaviour
{
    [SerializeField] ParticleSystem PS1;
    [SerializeField] ParticleSystem PS2;
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip[] crackSounds;
    [SerializeField] AudioClip[] breakSounds;

    float pitchMin = 0.9f;
    float pitchMax = 1.1f;


    //--------------------

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


    //--------------------


    public void Crack()
    {
        if (crackSounds.Length <= 0) return;

        audioSource.clip = crackSounds[Random.Range(0, crackSounds.Length)];
        audioSource.pitch = Random.Range(pitchMin, pitchMax);

        PS1.Play();
        audioSource.PlayOneShot(audioSource.clip);
    }
    public void Break()
    {
        if (breakSounds.Length <= 0) return;

        audioSource.clip = breakSounds[Random.Range(0, breakSounds.Length)];
        audioSource.pitch = Random.Range(pitchMin, pitchMax);

        PS2.Play();
        audioSource.PlayOneShot(audioSource.clip);
    }
}
