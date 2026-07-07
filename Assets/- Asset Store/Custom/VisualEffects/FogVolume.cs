using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;

public class FogVolume : MonoBehaviour
{
    Camera cam;

    bool inside;
    bool runOnce;

    float depth;
    float lastDepth;

    float edgePadding = 0.25f;
    float depthMin = 0f;
    float depthMax = 0.75f;

    void Start()
    {
        Shader.SetGlobalFloat("_FogStrength", 0);

        cam = Camera.main;
    }

    void Update()
    {
        Vector3 point = transform.InverseTransformPoint(cam.transform.position);
        Vector3 halfSize = Vector3.one * 0.5f + Vector3.one * edgePadding;

        Vector3 distance = new Vector3(Mathf.Abs(point.x), Mathf.Abs(point.y), Mathf.Abs(point.z));

        depth = Mathf.Min(halfSize.x - distance.x, Mathf.Min(halfSize.y - distance.y, halfSize.z - distance.z));

        inside = depth > 0;

        depth = Mathf.Clamp(depth, depthMin, depthMax);

        if (inside)
        {
            if (depth != lastDepth)
            {
                Shader.SetGlobalFloat("_FogStrength", depth);

                lastDepth = depth;
            }
            runOnce = true;
        }
        else
        {
            if (runOnce)
            {
                Shader.SetGlobalFloat("_FogStrength", 0);

                runOnce = false;
            }
        }
    }

    void OnDisable()
    {
        Shader.SetGlobalFloat("_FogStrength", 0);
    }
}
