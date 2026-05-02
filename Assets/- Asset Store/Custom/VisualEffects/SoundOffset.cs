using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOffset : MonoBehaviour
{
    AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();

        source.time = Random.Range(0f, 5f);
    }
}
