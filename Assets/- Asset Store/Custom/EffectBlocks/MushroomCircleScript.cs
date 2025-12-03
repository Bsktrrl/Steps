using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomCircleScript : MonoBehaviour
{
    Animator anim;
    int animation;
    int prevAnimation;

    float minPuffSpeed = 0.5f;
    float maxPuffSpeed = 1.5f;

    float minIdleSpeed = 0.75f;
    float maxIdleSpeed = 1f;

    float animWaitTime = 0.67f;
    float minWaitTime = 0f;
    float maxWaitTime = 10f;

    [SerializeField] ParticleSystem[] PS;

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(timer());

        //Set the different mushrooms to have different idle animation speeds
        anim.SetFloat("IdleSpeed0", UnityEngine.Random.Range(minIdleSpeed, maxIdleSpeed));
        anim.SetFloat("IdleSpeed1", UnityEngine.Random.Range(minIdleSpeed, maxIdleSpeed));
        anim.SetFloat("IdleSpeed2", UnityEngine.Random.Range(minIdleSpeed, maxIdleSpeed));
        anim.SetFloat("IdleSpeed3", UnityEngine.Random.Range(minIdleSpeed, maxIdleSpeed));
        anim.SetFloat("IdleSpeed4", UnityEngine.Random.Range(minIdleSpeed, maxIdleSpeed));
    }

    //Continuously play an animation and then wait for x seconds
    IEnumerator timer()
    {
        while (true)
        {
            //Pick one of 5 random animations and adjust the animation speed
            animation = UnityEngine.Random.Range(0,5);
            anim.SetFloat("PuffSpeed", UnityEngine.Random.Range(minPuffSpeed, maxPuffSpeed));

            //If the random animation was just played, play the next one in the list
            if (animation == prevAnimation)
            {
                animation = Mathf.RoundToInt(Mathf.Repeat(animation+1, 5));
            }

            anim.SetTrigger("Puff" + animation);
            yield return new WaitForSeconds(animWaitTime * Mathf.Pow(anim.speed, -1));
            PS[animation].Play();

            prevAnimation = animation;

            yield return new WaitForSeconds(UnityEngine.Random.Range(minWaitTime, maxWaitTime));
        }
    }
}
