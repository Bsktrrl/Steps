using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    [SerializeField] private GameObject shineHor;
    [SerializeField] private GameObject shineVer;
    [SerializeField] private GameObject rotationObject;
    [SerializeField] private Color shineColor;

    void Update()
    {
        shineHor.transform.LookAt(Camera.main.transform);
        shineHor.transform.localEulerAngles += new Vector3(90, Mathf.Sin(Time.time * 2) * 10, 0);

        shineVer.transform.LookAt(Camera.main.transform);
        shineVer.transform.localEulerAngles += new Vector3(90, Mathf.Sin(Time.time * 2 + Mathf.PI) * 10, 0);

        shineHor.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) + Vector3.one * Mathf.Sin(Time.time * 3) * 0.01f;
        shineVer.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) + Vector3.one * Mathf.Sin(Time.time * 3 + Mathf.PI) * 0.01f;

        rotationObject.transform.eulerAngles += new Vector3(0, 100 * Time.deltaTime, 0);
    }
}
