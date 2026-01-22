using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoFScript : MonoBehaviour
{
    [SerializeField] FullScreenPassRendererFeature feature;
    [SerializeField] Material baseMat;
    Material mat;
    float baseValue;
    float blurDistance;
    float conversionValue = 2.13f;
    Vector3 direction;

    void Start()
    {
        mat = new Material(baseMat);
        feature.passMaterial = mat;
        baseValue = mat.GetFloat("_BlurDistance");
    }

    void Update()
    {
        RaycastHit hit;

        direction = Camera.main.transform.forward;
        direction = Vector3.RotateTowards(direction, -Camera.main.transform.up, 0.1f, 0);

        if (Physics.Raycast(Camera.main.transform.position, direction, out hit))
        {
            blurDistance = Mathf.Lerp(mat.GetFloat("_BlurDistance"), Vector3.Distance(Camera.main.transform.position, hit.point) / conversionValue, 10 * Time.deltaTime);
        }

        blurDistance = Mathf.Clamp(blurDistance, 1, 100);
        mat.SetFloat("_BlurDistance", blurDistance);
    }

    void OnDestroy()
    {
        if (feature != null && baseMat != null)
        {
            feature.passMaterial = baseMat;
        }

        if (mat != null)
        {
            Destroy(mat);
        }
    }
}