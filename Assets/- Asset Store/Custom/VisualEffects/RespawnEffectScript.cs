using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnEffectScript : MonoBehaviour
{
    ParticleSystem particle;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            particle.Play();
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            particle.Stop();
        }
    }
}
