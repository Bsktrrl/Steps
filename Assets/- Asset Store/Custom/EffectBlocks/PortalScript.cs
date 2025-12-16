using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    [SerializeField] Light light;
    [SerializeField] ParticleSystem teleportParticle;
    float lightIntensity;

    float lightSpeed = 5f;

    float lowerIntensity = 0.2f;
    float higherIntensity = 0.3f;
    float teleportIntensity = 2f;

    float lowerWaitTime = 0.25f;
    float higherWaitTime = 0.5f;

    bool teleporting;
    float teleportLength = 0.5f;

    private void Start()
    {
        StartCoroutine(timer());
    }

    void Update()
    {
        //Light flickers when not teleporting and lerps to teleportIntensity when teleporting
        if (!teleporting)
        {
            light.intensity = Mathf.Lerp(light.intensity, lightIntensity, lightSpeed * Time.deltaTime);
        }
        else
        {
            light.intensity = Mathf.Lerp(light.intensity, teleportIntensity, lightSpeed * Time.deltaTime);
        }
    }

    public void ActivatePortalEffect()
    {
        StartCoroutine(teleport());
        //teleportParticle.Play();
    }

    //Light flicker intensity
    IEnumerator timer()
    {
        while (true)
        {
            lightIntensity = Random.Range(lowerIntensity, higherIntensity);

            yield return new WaitForSeconds(Random.Range(lowerWaitTime, higherWaitTime));
        }
    }

    //Teleport timer
    IEnumerator teleport()
    {
        teleporting = true;

        yield return new WaitForSeconds(teleportLength);

        teleporting = false;
    }
}
