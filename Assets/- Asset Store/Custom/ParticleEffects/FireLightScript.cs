using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLightScript : MonoBehaviour
{
    private Light light;
    private float lightIntensity;

    void Start()
    {
        light = gameObject.GetComponent<Light>();
        StartCoroutine(timer());
    }

    void Update()
    {
        light.intensity = Mathf.Lerp(light.intensity, lightIntensity, 10 * Time.deltaTime);
    }

    IEnumerator timer()
    {
        while (true)
        {
            lightIntensity = Random.Range(0.5f, 0.9f);

            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
        }
    }
}
