using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleCalmScript : MonoBehaviour
{
    Animator anim;

    [SerializeField] float newSpeed;
    [SerializeField] float maxOffset;
    [SerializeField] float minWaitTime;
    [SerializeField] float maxWaitTime;
    [SerializeField] float minBubbleTime;
    [SerializeField] float maxBubbleTime;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = newSpeed + Random.Range(-0.1f, 0.1f);

        StartCoroutine(Bubbling());
    }

    IEnumerator Bubbling()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

            anim.SetFloat("CycleOffset", Random.Range(0f, maxOffset));
            anim.SetBool("Bubbling", true);

            yield return new WaitForSeconds(Random.Range(minBubbleTime, maxBubbleTime));

            anim.SetBool("Bubbling", false);
        }
    }
}