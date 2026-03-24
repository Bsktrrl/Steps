using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSandScript : MonoBehaviour
{
    Animator anim;
    [SerializeField] ParticleSystem PS1;
    [SerializeField] ParticleSystem PS2;
    [SerializeField] ParticleSystem PS3;
    [SerializeField] ParticleSystem PS4;
    float minWaitTime = 1f;
    float maxWaitTime = 7f;
    float minShakeTime = 0f;
    float maxShakeTime = 0.67f;

    void Start()
    {
        anim = GetComponent<Animator>();

        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

            anim.SetBool("Shaking", true);

            yield return new WaitForSeconds(0.33f);
            PS1.Play();
            PS2.Play();
            PS3.Play();
            PS4.Play();

            yield return new WaitForSeconds(Random.Range(minShakeTime, maxShakeTime));

            anim.SetBool("Shaking", false);
        }
    }
}
