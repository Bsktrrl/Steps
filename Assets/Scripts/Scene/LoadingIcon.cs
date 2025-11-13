using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingIcon : MonoBehaviour
{
    [SerializeField] GameObject loadingIcon;
    public float amplitude = 20f;  // How far up/down (in pixels)
    public float frequency = 3f;   // Speed of the movement

    Vector3 startPos;


    //--------------------


    void Start()
    {
        // Save the original position
        startPos = loadingIcon.GetComponent<RectTransform>().anchoredPosition;
    }

    void Update()
    {
        // Move up and down using a sine wave
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        loadingIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}
