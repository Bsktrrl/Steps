using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAndSmokeScript : MonoBehaviour
{
    [SerializeField] private GameObject shineHor;
    private float shineHorSize;
    [SerializeField] private GameObject shineDia;
    private float shineDiaSize;
    [SerializeField] private GameObject rotationObject;
    [SerializeField] private GameObject scaleObject;
    private float scaleObjectSize;
    [SerializeField] private Light pointLight;

    void Start()
    {
        if(shineHor != null)
        {
            shineHorSize = shineHor.transform.localScale.x;
        }
        if (shineDia != null)
        {
            shineDiaSize = shineDia.transform.localScale.x;
        }
        if(scaleObject != null)
        {
            scaleObjectSize = scaleObject.transform.localScale.x;
        }
    }

    void Update()
    {
        if(shineHor != null)
        {
            shineHor.transform.LookAt(Camera.main.transform);
            shineHor.transform.localEulerAngles += new Vector3(90, Mathf.Sin(Time.time * 2) * shineHorSize*100, 0);
            shineHor.transform.localScale = new Vector3(shineHorSize, shineHorSize, shineHorSize) + Vector3.one * Mathf.Sin(Time.time * 3) * shineHorSize/10;
        }

        if(shineDia != null)
        {
            shineDia.transform.LookAt(Camera.main.transform);
            shineDia.transform.localEulerAngles += new Vector3(90, Mathf.Sin(Time.time * 2 + Mathf.PI) * shineDiaSize*100, 0);
            shineDia.transform.localScale = new Vector3(shineDiaSize, shineDiaSize, shineDiaSize) + Vector3.one * Mathf.Sin(Time.time * 3 + Mathf.PI) * shineDiaSize / 10;
        }

        if(pointLight != null)
        {
            pointLight.intensity = 0.1f + Mathf.Sin(Time.time * 3f) * 0.02f;
        }

        if(rotationObject != null)
        {
            rotationObject.transform.eulerAngles += new Vector3(0, 100 * Time.deltaTime, 0);
        }

        if(scaleObject != null)
        {
            scaleObject.transform.localScale = new Vector3(scaleObjectSize, scaleObjectSize, scaleObjectSize) + Vector3.one * Mathf.Sin(Time.time * 3) * scaleObjectSize / 7;
        }
    }
}
