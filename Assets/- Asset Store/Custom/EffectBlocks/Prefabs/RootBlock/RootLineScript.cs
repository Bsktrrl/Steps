using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootLineScript : MonoBehaviour
{
    AudioSource source;

    [SerializeField] AudioClip[] clips;
    float pitchMin = 0.9f;
    float pitchMax = 1.1f;

    void OnEnable()
    {
        source = GetComponent<AudioSource>();

        source.clip = clips[Random.Range(0, clips.Length)];
        source.pitch = Random.Range(pitchMin, pitchMax);
        source.Play();
    }
}
