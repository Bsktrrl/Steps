using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueplantAnimScript : MonoBehaviour
{
    Animator anim;

    [SerializeField] ParticleSystem PS;

    [SerializeField] Region region;
    enum Region
    {
        Firevein,
        Frostfields,
        Metalworks,
        Rivergreens,
        Sandlands,
        Witchmire
    }

    float animWaitTime = 1.75f;
    float minWaitTime = 5;
    float maxWaitTime = 15;

    void Start()
    {
        anim = GetComponent<Animator>();
        switch (region)
        {
            case Region.Firevein:
                anim.SetInteger("Region", 0);
                break;
            case Region.Frostfields:
                anim.SetInteger("Region", 1);
                break;
            case Region.Metalworks:
                anim.SetInteger("Region", 2);
                break;
            case Region.Rivergreens:
                anim.SetInteger("Region", 3);
                break;
            case Region.Sandlands:
                anim.SetInteger("Region", 4);
                break;
            case Region.Witchmire:
                anim.SetInteger("Region", 5);
                break;
        }
        StartCoroutine(Puff());
    }

    IEnumerator Puff()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

            anim.SetInteger("PuffType", Random.Range(0,3));
            anim.SetTrigger("Puff");

            yield return new WaitForSeconds(animWaitTime);

            PS.Play();
        }
    }
}
