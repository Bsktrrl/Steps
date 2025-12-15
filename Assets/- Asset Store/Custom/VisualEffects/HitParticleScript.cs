using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticleScript : Singleton<HitParticleScript>
{
    public ParticleSystem particle;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }
}
