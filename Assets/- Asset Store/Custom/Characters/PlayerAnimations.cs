using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Animator anim;

    bool blink;
    bool secondaryIdle;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Play walking animation when pressing WASD, or play swimming animation if shift is held while pressing WASD
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                anim.SetTrigger("Walk");
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                anim.SetTrigger("Swim");
            }
        }

        //Start Blink Coroutine if it's not active
        if (!blink)
        {
            StartCoroutine("Blink");
        }

        //Start Secondary Idle Animaiton Coroutine if it's not active
        if (!secondaryIdle)
        {
            StartCoroutine("SecondaryIdle");
        }
    }

    //Blink every 0-10 seconds
    IEnumerator Blink()
    {
        blink = true;

        yield return new WaitForSeconds(Random.Range(0f, 10f));

        anim.SetTrigger("Blink");

        blink = false;
    }

    //Play one of two secondary idle animations every 15-30 seconds
    IEnumerator SecondaryIdle()
    {
        secondaryIdle = true;

        yield return new WaitForSeconds(Random.Range(15f, 30f));

        if (Random.Range(0f, 1f) > 0.5f)
        {
            anim.SetTrigger("Confused");
        }
        else
        {
            anim.SetTrigger("Confident");
        }

        secondaryIdle = false;
    }
}
