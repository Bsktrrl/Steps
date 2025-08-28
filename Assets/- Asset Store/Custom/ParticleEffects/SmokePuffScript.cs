using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokePuffScript : MonoBehaviour
{
    ParticleSystem ps;
    [SerializeField] Light light;
    bool lightOn;
    float lightSpeed = 10;
    float lightIntensity = 3;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ps.Play();
            StartCoroutine(LightFlash());
        }
        
        if (lightOn)
        {
            light.intensity = Mathf.Lerp(light.intensity, lightIntensity, lightSpeed * Time.deltaTime);
        }
        else
        {
            light.intensity = Mathf.Lerp(light.intensity, 0, lightSpeed * Time.deltaTime);
        }
    }

    IEnumerator LightFlash ()
    {
        yield return new WaitForSeconds(0.1f);
        lightOn = true;
        yield return new WaitForSeconds(0.2f);
        lightOn = false;
    }
}
