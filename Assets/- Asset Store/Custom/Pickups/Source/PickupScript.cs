using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    [SerializeField] private GameObject shineHor;
    [SerializeField] private GameObject shineDia;
    [SerializeField] private GameObject rotationObject;
    [SerializeField] private Light pointLight;
    [SerializeField] private int size;
    private float shineSize;

    void Start()
    {
        if (size == 0)
        {
            shineSize = 0.05f;
        }
        else if (size == 1)
        {
            shineSize = 0.075f;
        }
        else
        {
            shineSize = 0.1f;
        }
    }

    void Update()
    {
        if(shineHor != null)
        {
            shineHor.transform.LookAt(Camera.main.transform);
            shineHor.transform.localEulerAngles += new Vector3(90, Mathf.Sin(Time.time * 2) * shineSize*100, 0);
            shineHor.transform.localScale = new Vector3(shineSize, shineSize, shineSize) + Vector3.one * Mathf.Sin(Time.time * 3) * shineSize/10;
        }

        if(shineDia != null)
        {
            shineDia.transform.LookAt(Camera.main.transform);
            shineDia.transform.localEulerAngles += new Vector3(90, Mathf.Sin(Time.time * 2 + Mathf.PI) * shineSize*100, 0);
            shineDia.transform.localScale = new Vector3(shineSize, shineSize, shineSize) + Vector3.one * Mathf.Sin(Time.time * 3 + Mathf.PI) * shineSize/10;
        }

        if(pointLight != null)
        {
            pointLight.intensity = 0.1f + Mathf.Sin(Time.time * 3f) * 0.02f;
        }

        if(rotationObject != null)
        {
            rotationObject.transform.eulerAngles += new Vector3(0, 100 * Time.deltaTime, 0);
        }
    }
}
