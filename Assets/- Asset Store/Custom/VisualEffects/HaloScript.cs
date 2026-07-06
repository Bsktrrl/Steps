using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class HaloScript : MonoBehaviour
{
    [SerializeField] float scale;
    [SerializeField] bool ignoreOpaques;
    float minDistance = 3f;
    float speed = 10f;
    float size;

    void Update()
    {
        if (ignoreOpaques)
        {
            CalculateSize();
        }
        else
        {
            if (Physics.Raycast(transform.position, Camera.main.transform.position - transform.position, Vector3.Distance(transform.position, Camera.main.transform.position)))
            {
                size = 0f;
            }
            else
            {
                CalculateSize();
            }
        }

        transform.localScale = Vector3.one * Mathf.Lerp(transform.localScale.x, size, speed * Time.deltaTime);
    }

    void CalculateSize()
    {
        size = Vector3.Distance(transform.position, Camera.main.transform.position) / 2;
        size = Mathf.Max(size, minDistance) / minDistance;
        size *= scale;
        size = Mathf.Max(size, scale);
    }
}
