using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticleScript : MonoBehaviour
{
    ParticleSystem particle;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            particle.Play();
        }
    }
}
