using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class OptionSelectAnimation : MonoBehaviour
{
    [Header("Button Type")]
    [SerializeField] bool isLeftArrow;
    [SerializeField] bool isRightArrow;

    [Header("Active Parent")]
    [SerializeField] GameObject parent;

    [Header("Pulsating")]
    [SerializeField] Vector3 maxScale = new Vector3(0.2f, 0.2f, 0.2f);
    [SerializeField] Vector3 startScale = new Vector3(1f, 1f, 1f);
    [SerializeField] float pulseSpeed = 3f;
    [SerializeField, Range(0.2f, 3f)] float edgeTightness = 0.9f;

    RectTransform rt;

    bool buttonPressAnimation;
    float counter = 0;


    //--------------------


    private void Awake()
    {
        rt = (RectTransform)transform;
    }

    private void Update()
    {
        if (buttonPressAnimation)
        {
            counter += Time.unscaledDeltaTime;

            float duration = 1f / (2f * pulseSpeed);

            if (counter >= duration)
            {
                buttonPressAnimation = false;
                counter = 0f;
                rt.localScale = startScale;
            }
            else
            {
                ButtonPressAnimation(duration);
            }
        }
        else
        {
            rt.localScale = startScale;
        }
    }


    //--------------------


    private void OnEnable()
    {
        Menu_KeyInputs.Action_MenuSettingsNavigationLeft_isPressed += ButtonPressAnimation_Left;
        Menu_KeyInputs.Action_MenuSettingsNavigationRight_isPressed += ButtonPressAnimation_Right;
    }
    private void OnDisable()
    {
        Menu_KeyInputs.Action_MenuSettingsNavigationLeft_isPressed -= ButtonPressAnimation_Left;
        Menu_KeyInputs.Action_MenuSettingsNavigationRight_isPressed -= ButtonPressAnimation_Right;
    }


    //--------------------


    void ButtonPressAnimation_Left()
    {
        if (isLeftArrow && parent.GetComponent<UI_PulsatingMotion_WhenSelected>() && parent.GetComponent<UI_PulsatingMotion_WhenSelected>().isActive)
        {
            counter = 0;
            rt.localScale = Vector2.one;

            buttonPressAnimation = true;
        }
    }
    void ButtonPressAnimation_Right()
    {
        if (isRightArrow && parent.GetComponent<UI_PulsatingMotion_WhenSelected>() && parent.GetComponent<UI_PulsatingMotion_WhenSelected>().isActive)
        {
            counter = 0;
            rt.localScale = Vector2.one;

            buttonPressAnimation = true;
        }
    }

    void ButtonPressAnimation(float duration)
    {
        float u = Mathf.Clamp01(counter / duration);   // 0..1
        float t = Mathf.Sin(u * Mathf.PI);             // 0..1..0 (single pulse)

        t = TightenEdges(t, edgeTightness);            // optional “less dwell”

        rt.localScale = Vector3.LerpUnclamped(startScale, startScale + maxScale, t);
    }
    static float TightenEdges(float t, float k)
    {
        float a = Mathf.Pow(t, k);
        float b = Mathf.Pow(1f - t, k);
        return a / (a + b);
    }
}
