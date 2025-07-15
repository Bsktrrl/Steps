using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animstions : Singleton<Player_Animstions>
{
    public Animator anim;


    //--------------------


    private void Start()
    {
        StartCoroutine(RandomBlink());
        StartCoroutine(RandomIdle());
    }


    //--------------------


    IEnumerator RandomBlink()
    {
        float time = UnityEngine.Random.Range(0, 10);
        print("1.1. RandomBlink: " + time);

        yield return new WaitForSeconds(time);

        anim.SetTrigger(AnimationManager.Instance.blink);

        print("1.2. RandomBlink");

        StartCoroutine(RandomBlink());
    }
    IEnumerator RandomIdle()
    {
        float time = UnityEngine.Random.Range(15, 30);
        print("2.1. SecondaryIdle: " + time);

        yield return new WaitForSeconds(time);

        if (UnityEngine.Random.Range(0, 1) > 0.5f)
            anim.SetTrigger("Confused");
        else
            anim.SetTrigger("Confident");

        print("2.2. SecondaryIdle");

        Invoke(nameof(StartSecondaryIdle), 0);
    }

    void StartSecondaryIdle()
    {
        StartCoroutine(RandomIdle());
    }
}
