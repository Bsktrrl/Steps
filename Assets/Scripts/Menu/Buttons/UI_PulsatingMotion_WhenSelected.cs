using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_PulsatingMotion_WhenSelected : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    [Header("Sprites (Optional)")]
    public Image backgroundImage;
    public Sprite background_Default;
    public Sprite background_Active;

    [Header("Pulsating")]
    [SerializeField] Vector3 maxScale;
    [SerializeField] Vector3 startScale = Vector3.one;
    [SerializeField] float pulseSpeed = 1f;
    [SerializeField, Range(0.2f, 3f)] float edgeTightness = 1f;

    RectTransform rt;

    [SerializeField] bool isBackButton;
    [SerializeField] bool isStartButton;

    bool isActive;


    //--------------------


    private void Awake()
    {
        rt = (RectTransform)transform;
    }
    private void Update()
    {
        if (isActive)
        {
            SetActive();
        }
        else
        {
            SetPassive();
        }
    }


    //--------------------


    private void OnEnable()
    {
        if (isStartButton)
            isActive = true;
    }
    private void OnDisable()
    {
        if (isBackButton)
            isActive = false;
    }


    //--------------------


    public void OnPointerEnter(PointerEventData eventData)
    {
        // Highlighted
        isActive = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // Normal (mouse left)
        isActive = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // Pressed
        isActive = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // Released (back to Highlighted if still hovered)
        isActive = false;
    }
    public void OnSelect(BaseEventData eventData)
    {
        // Selected (keyboard/controller)
        isActive = true;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        isActive = false;
    }


    //--------------------


    public void SetActive()
    {
        float t = 0.5f + 0.5f * Mathf.Sin(Time.unscaledTime * pulseSpeed * Mathf.PI * 2f);
        t = TightenEdges(t, edgeTightness);

        rt.localScale = Vector3.LerpUnclamped(startScale, startScale + maxScale, t);

        SetBackgroundImageActive();
    }
    static float TightenEdges(float t, float k)
    {
        float a = Mathf.Pow(t, k);
        float b = Mathf.Pow(1f - t, k);
        return a / (a + b);
    }

    public void SetPassive()
    {
        rt.localScale = Vector2.one;

        SetBackgroundImageDefault();
    }

    void SetBackgroundImageActive()
    {
        if (backgroundImage && background_Active)
        {
            //print("1. SetBackgroundImageActive");
            backgroundImage.sprite = background_Active;
        }
    }
    void SetBackgroundImageDefault()
    {
        if (backgroundImage && background_Default)
        {
            //print("2. SetBackgroundImageDefault");
            backgroundImage.sprite = background_Default;
        }
    }
}
