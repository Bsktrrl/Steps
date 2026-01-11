using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PulsatingMotion : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] Vector2 farthestPos;
    Vector2 startPos;

    [Header("Scale (optional)")]
    [SerializeField] Vector3 maxScale;
    Vector3 startScale;

    [Header("Speed")]
    [SerializeField] float pulseSpeed = 1f;
    [SerializeField, Range(0.2f, 3f)] float edgeTightness = 1f;


    RectTransform rt;



    //--------------------


    void Awake()
    {
        rt = (RectTransform)transform;
        startPos = rt.anchoredPosition;

        rt.anchoredPosition = startPos;
        startScale = rt.localScale;
    }

    void Update()
    {
        float t = 0.5f + 0.5f * Mathf.Sin(Time.unscaledTime * pulseSpeed * Mathf.PI * 2f);
        t = TightenEdges(t, edgeTightness);

        rt.anchoredPosition = Vector2.LerpUnclamped(startPos, startPos + farthestPos, t);

        rt.localScale = Vector3.LerpUnclamped(startScale, startScale + maxScale, t);
    }


    //--------------------


    static float TightenEdges(float t, float k)
    {
        float a = Mathf.Pow(t, k);
        float b = Mathf.Pow(1f - t, k);
        return a / (a + b);
    }
}
