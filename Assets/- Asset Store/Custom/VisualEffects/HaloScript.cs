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
        Camera camera = Camera.main;
        if (!camera) return;

        Vector3 toCamera = camera.transform.position - transform.position;
        float distance = toCamera.magnitude;
        if (!ignoreOpaques && Physics.Raycast(transform.position, toCamera, distance))
            size = 0f;
        else
            CalculateSize(distance);

        transform.localScale = Vector3.one * Mathf.Lerp(transform.localScale.x, size, speed * Time.deltaTime);
    }

    void CalculateSize(float distance)
    {
        size = distance / 2;
        size = Mathf.Max(size, minDistance) / minDistance;
        size *= scale;
        size = Mathf.Max(size, scale);
    }
}
