using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLightScript : MonoBehaviour
{
    private Light mainLight;
    private float lightIntensity;

    [SerializeField] float lightIntensityMin;
    [SerializeField] float lightIntensityMax;

    void Start()
    {
        mainLight = gameObject.GetComponent<Light>();
        StartCoroutine(timer());
    }

    void Update()
    {
        mainLight.intensity = Mathf.Lerp(mainLight.intensity, lightIntensity, 10 * Time.deltaTime);
    }

    IEnumerator timer()
    {
        while (true)
        {
            lightIntensity = Random.Range(lightIntensityMin, lightIntensityMax);

            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
        }
    }
}
