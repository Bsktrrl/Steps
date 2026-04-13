using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBlockIdleScript : MonoBehaviour
{
    float waitTimeMin = 3f;
    float waitTimeMax = 10f;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        StartCoroutine(Idle());
    }

    IEnumerator Idle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));

            anim.SetInteger("IdleAnimation", Random.Range(0,3));
            anim.SetTrigger("Idle");
            print(anim.GetInteger("IdleAnimation"));
        }
    }
}
