using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class HaloScript : MonoBehaviour
{
    [SerializeField] float scale;
    float minDistance = 3f;
    float speed = 10f;
    float size;

    void Update()
    {
        if (Physics.Raycast(transform.position, Camera.main.transform.position - transform.position, Vector3.Distance(transform.position, Camera.main.transform.position)))
        {
            size = 0.1f;
        }
        else
        {
            size = Vector3.Distance(transform.position, Camera.main.transform.position) / 2;
            size = Mathf.Max(size, minDistance) / minDistance;
            size *= scale;
            size = Mathf.Max(size, scale);
        }

        transform.localScale = Vector3.one * Mathf.Lerp(transform.localScale.x, size, speed * Time.deltaTime);
    }
}
