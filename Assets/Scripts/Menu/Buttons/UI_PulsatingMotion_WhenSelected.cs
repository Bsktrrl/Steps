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
    [SerializeField] Vector3 maxScale = new Vector3(0.15f, 0.15f, 0.15f);
    [SerializeField] Vector3 startScale = new Vector3(1.1f, 1.1f, 1.1f);
    [SerializeField] float pulseSpeed = 0.3f;
    [SerializeField, Range(0.2f, 3f)] float edgeTightness = 0.9f;

    [Header("Exta ImageObjects")]
    [SerializeField] List<GameObject> extraImages = new List<GameObject>();

    List<RectTransform> rt = new List<RectTransform>();

    [Header("Button States")]
    [SerializeField] bool isBackButton;
    [SerializeField] bool isStartButton;

    public bool isActive;
    bool performed_SetPassive;


    //--------------------


    private void Awake()
    {
        rt.Add((RectTransform)transform);

        for (int i = 0; i < extraImages.Count; i++)
        {
            rt.Add(extraImages[i].GetComponent<RectTransform>());
        }
    }
    private void Update()
    {
        if (isActive)
        {
            SetActive();
            performed_SetPassive = false;
        }
        else if (!performed_SetPassive)
        {
            SetPassive();
            performed_SetPassive = true;
        }
    }


    //--------------------


    private void OnEnable()
    {
        if (isStartButton)
        {
            isActive = true;
        }
        else
        {
            isActive = false;
        }
    }
    private void OnDisable()
    {
        if (isBackButton)
            isActive = false;
    }


    //--------------------


    public void EnableStartButton()
    {
        if (isStartButton)
        {
            isActive = true;
        }
    }


    //--------------------


    public void OnPointerEnter(PointerEventData eventData)
    {
        // Highlighted
        isActive = true;
        //print("1. OnPointerEnter - true: " + gameObject.name);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // Normal (mouse left)
        isActive = false;
        //print("2. OnPointerExit - false: " + gameObject.name);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // Pressed
        isActive = true;
        //print("3. OnPointerDown - true: " + gameObject.name);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // Released (back to Highlighted if still hovered)
        isActive = false;
        //print("4. OnPointerUp - false: " + gameObject.name);
    }
    public void OnSelect(BaseEventData eventData)
    {
        // Selected (keyboard/controller)
        isActive = true;
        //print("5. OnSelect - true: " + gameObject.name);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        isActive = false;
        //print("6. OnDeselect - false: " + gameObject.name);
    }


    //--------------------


    public void SetActive()
    {
        float t = 0.5f + 0.5f * Mathf.Sin(Time.unscaledTime * pulseSpeed * Mathf.PI * 2f);
        t = TightenEdges(t, edgeTightness);

        for (int i = 0; i < rt.Count; i++)
        {
            rt[i].localScale = Vector3.LerpUnclamped(startScale, startScale + maxScale, t);
            rt[i].gameObject.SetActive(true);
        }
        
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
        for (int i = 1; i < rt.Count; i++)
        {
            rt[i].localScale = Vector2.one;
            rt[i].gameObject.SetActive(false);
        }

        rt[0].localScale = Vector2.one;

        //rt[0].gameObject.SetActive(true);

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
