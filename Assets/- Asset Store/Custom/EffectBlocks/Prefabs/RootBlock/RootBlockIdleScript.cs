using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBlockIdleScript : MonoBehaviour
{
    float waitTimeMin = 3f;
    float waitTimeMax = 10f;

    float pitchMin = 0.9f;
    float pitchMax = 1.1f;

    Animator anim;
    AudioSource source;

    void Start()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        StartCoroutine(Idle());
    }

    IEnumerator Idle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));

            anim.SetInteger("IdleAnimation", Random.Range(0,3));
            anim.SetTrigger("Idle");
        }
    }

    void PlaySound()
    {
        source.pitch = Random.Range(pitchMin, pitchMax);
        source.Play();
    }
}