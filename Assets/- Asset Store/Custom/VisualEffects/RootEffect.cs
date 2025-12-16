using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootEffect : MonoBehaviour
{
    public ParticleSystem particle;

    void OnEnable()
    {
        StartCoroutine(PerformEffectDelay(0.05f));
    }

    IEnumerator PerformEffectDelay(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);

        particle.Play();
    }
}
