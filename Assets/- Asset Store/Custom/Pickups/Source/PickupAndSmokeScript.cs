using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAndSmokeScript : MonoBehaviour
{
    [SerializeField] private GameObject shineHor;
    private Vector3 shineHorSize;
    [SerializeField] private GameObject shineDia;
    private Vector3 shineDiaSize;
    [SerializeField] private GameObject rotationObject;
    [SerializeField] private GameObject scaleObject;
    private float scaleObjectSize;
    [SerializeField] private Light pointLight;

    void Start()
    {
        if(shineHor != null)
        {
            shineHorSize = shineHor.transform.localScale;
        }
        if (shineDia != null)
        {
            shineDiaSize = shineDia.transform.localScale;
        }
        if(scaleObject != null)
        {
            scaleObjectSize = scaleObject.transform.localScale.x;
        }
    }

    void Update()
    {
        if(shineHor != null && shineHor.activeInHierarchy)
        {
            shineHor.transform.LookAt(Camera.main.transform);
            shineHor.transform.localEulerAngles += new Vector3(90, Mathf.Sin(Time.time * 2) * shineHorSize.x * 100, 0);
            shineHor.transform.localScale = shineHorSize + Vector3.one * Mathf.Sin(Time.time * 3) * shineHorSize.x / 10;
        }

        if(shineDia != null && shineDia.activeInHierarchy)
        {
            shineDia.transform.LookAt(Camera.main.transform);
            shineDia.transform.localEulerAngles += new Vector3(90, Mathf.Sin(Time.time * 2 + Mathf.PI) * shineDiaSize.x * 100, 0);
            shineDia.transform.localScale = shineDiaSize + Vector3.one * Mathf.Sin(Time.time * 3 + Mathf.PI) * shineDiaSize.x / 10;
        }

        if(pointLight != null && pointLight.gameObject.activeInHierarchy)
        {
            pointLight.intensity = 0.1f + Mathf.Sin(Time.time * 3f) * 0.02f;
        }

        if(rotationObject != null && rotationObject.activeInHierarchy)
        {
            //rotationObject.transform.eulerAngles += new Vector3(0, 100 * Time.deltaTime, 0);
        }

        if (scaleObject != null && scaleObject.activeInHierarchy)
        {
            scaleObject.transform.localScale = new Vector3(scaleObjectSize, scaleObjectSize, scaleObjectSize) + Vector3.one * Mathf.Sin(Time.time * 3) * scaleObjectSize / 7;
        }
    }
}
