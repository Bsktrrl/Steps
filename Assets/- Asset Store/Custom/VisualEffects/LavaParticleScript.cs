using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaParticleScript : MonoBehaviour
{
    float waitTimeMin = 0f;
    float waitTimeMax = 30f;

    [SerializeField] ParticleSystem PS1;
    [SerializeField] ParticleSystem PS2;

    int randomness;
    void Start()
    {
        StartCoroutine(Lava());
    }

    IEnumerator Lava()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));

            randomness = Random.Range(0, 4);

            if (randomness == 0)
            {
                PS1.Play();
            }
            else if (randomness == 1)
            {
                PS2.Play();
            }
        }
    }
}
