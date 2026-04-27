using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalEffectScript : MonoBehaviour
{
    ParticleSystem PS;
    AudioSource source;

    [SerializeField] AudioClip[] audioClips;

    [SerializeField] float waitTimeMin;
    [SerializeField] float waitTimeMax;
    [SerializeField] float length;

    float pitchMin = 0.9f;
    float pitchMax = 1.1f;
    void Start()
    {
        PS = GetComponent<ParticleSystem>();
        source = GetComponent<AudioSource>();

        StartCoroutine(PlayParticles());
    }

    IEnumerator PlayParticles()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));

            source.clip = audioClips[Random.Range(0, audioClips.Length)];
            source.pitch = Random.Range(pitchMin, pitchMax);
            source.time = Random.Range(0f, 2f);
            source.volume = 1f;

            PS.Play();
            source.Play();

            yield return new WaitForSeconds(length);

            while(source.volume > 0f)
            {
                source.volume -= Time.deltaTime;
                yield return null;
            }

            source.Stop();
        }
    }
}