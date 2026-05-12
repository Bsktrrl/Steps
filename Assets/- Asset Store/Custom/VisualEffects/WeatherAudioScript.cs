using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherAudioScript : MonoBehaviour
{
    [SerializeField] float pitchMin;
    [SerializeField] float pitchMax;
    float pitch;

    AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();

        StartCoroutine(Pitch());
    }

    void Update()
    {
        source.pitch = Mathf.Lerp(source.pitch, pitch, Time.deltaTime);
    }

    IEnumerator Pitch()
    {
        while (true)
        {
            pitch = Random.Range(pitchMin, pitchMax);

            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }
}
