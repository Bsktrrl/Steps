using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectScript : MonoBehaviour
{
    ParticleSystem PS;
    AudioSource source;

    [SerializeField] float waitTimeMin;
    [SerializeField] float waitTimeMax;

    float pitchMin = 0.9f;
    float pitchMax = 1.1f;

    bool flying;
    void Start()
    {
        PS = GetComponent<ParticleSystem>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (flying == false)
        {
            StartCoroutine(Fly());
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            source.Play();
        }
    }

    IEnumerator Fly()
    {
        flying = true;
        PS.Play();
        source.pitch = Random.Range(pitchMin, pitchMax);
        source.Play();

        yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));

        flying = false;
    }
}
