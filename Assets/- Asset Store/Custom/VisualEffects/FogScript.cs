using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;

public class FogScript : MonoBehaviour
{
    bool inside;
    bool runOnce;

    float edgePadding = 0.1f;
    float depthMin = 0.5f;
    float depthMax = 0.75f;

    void Start()
    {
        Shader.SetGlobalFloat("_FogStrength", 0);
    }

    void Update()
    {
        Vector3 point = transform.InverseTransformPoint(Camera.main.transform.position);
        Vector3 halfSize = Vector3.one * 0.5f + Vector3.one * edgePadding;

        Vector3 distance = new Vector3(Mathf.Abs(point.x), Mathf.Abs(point.y), Mathf.Abs(point.z));

        float depth = Mathf.Min(halfSize.x - distance.x, Mathf.Min(halfSize.y - distance.y, halfSize.z - distance.z));

        print(depth);

        inside = depth > 0 ? true : false;

        depth = Mathf.Clamp(depth, depthMin, depthMax);

        if (inside)
        {
            Shader.SetGlobalFloat("_FogStrength", depth);

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
