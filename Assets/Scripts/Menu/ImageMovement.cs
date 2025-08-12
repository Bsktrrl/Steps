using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float verticalAmplitude;
    [SerializeField] float verticalFrequency;

    [SerializeField] float horizontalAmplitude;
    [SerializeField] float horizontalFrequency;

    RectTransform rectTransform;
    Vector2 originalPosition;
    float time;
    private float randomPhaseOffset;


    //--------------------

    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;

        // Random offset between 0 and 2?
        randomPhaseOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        time += Time.deltaTime;

        // Add the random offset to time-based oscillation
        float verticalOffset = verticalAmplitude != 0f
            ? Mathf.Sin((time * verticalFrequency * Mathf.PI * 2f) + randomPhaseOffset) * verticalAmplitude
            : 0f;

        float horizontalOffset = horizontalAmplitude != 0f
            ? Mathf.Cos((time * horizontalFrequency * Mathf.PI * 2f) + randomPhaseOffset) * horizontalAmplitude
            : 0f;

        rectTransform.anchoredPosition = originalPosition + new Vector2(horizontalOffset, verticalOffset);
    }
}
