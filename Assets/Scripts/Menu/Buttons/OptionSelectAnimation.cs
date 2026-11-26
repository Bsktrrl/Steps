using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionSelectAnimation : MonoBehaviour, IPointerDownHandler
{
    public static event Action Action_ButtonIsPressed;

    [Header("Button Type")]
    [SerializeField] bool isLeftArrow;
    [SerializeField] bool isRightArrow;

    [Header("Active Parent")]
    [SerializeField] GameObject parent;

    public Button SelectionButton => parent ? parent.GetComponent<Button>() : null;

    [Header("Pulsating")]
    [SerializeField] Vector3 maxScale = new Vector3(0.2f, 0.2f, 0.2f);
    [SerializeField] Vector3 startScale = new Vector3(1f, 1f, 1f);
    [SerializeField] float pulseSpeed = 3f;
    [SerializeField, Range(0.2f, 3f)] float edgeTightness = 0.9f;

    RectTransform rt;
    UI_PulsatingMotion_WhenSelected parentSelection;
    Coroutine pressAnimation;


    //--------------------


    private void Awake()
    {
        rt = (RectTransform)transform;
        if (parent)
            parent.TryGetComponent(out parentSelection);
    }


    //--------------------


    private void OnEnable()
    {
        rt.localScale = startScale;
        Menu_KeyInputs.Action_MenuSettingsNavigationLeft_isPressed += ButtonPressAnimation_Left;
        Menu_KeyInputs.Action_MenuSettingsNavigationRight_isPressed += ButtonPressAnimation_Right;
    }
    private void OnDisable()
    {
        Menu_KeyInputs.Action_MenuSettingsNavigationLeft_isPressed -= ButtonPressAnimation_Left;
        Menu_KeyInputs.Action_MenuSettingsNavigationRight_isPressed -= ButtonPressAnimation_Right;
        StopPressAnimation();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left || !GetComponent<Button>().IsInteractable()) return;

        Button row = SelectionButton;
        if (!ActionButtonsManager.CanSelectWithMouse(row)) return;

        if (row.TryGetComponent(out ButtonSound sound))
            sound.SelectWithoutNavigationSound(eventData);
        else
            eventData.selectedObject = row.gameObject;
    }

    public void Click()
    {
        if (!parent || !parent.TryGetComponent(out MenuCategorySelected row)) return;

        Menu_KeyInputs.PressSettingsArrow(row.settingState, isLeftArrow);
    }


    //--------------------


    void ButtonPressAnimation_Left()
    {
        if (isLeftArrow && parentSelection && parentSelection.isActive)
            StartPressAnimation();
    }
    void ButtonPressAnimation_Right()
    {
        if (isRightArrow && parentSelection && parentSelection.isActive)
            StartPressAnimation();
    }

    private void StartPressAnimation()
    {
        StopPressAnimation();
        rt.localScale = startScale;
        pressAnimation = StartCoroutine(AnimatePress());
        Action_ButtonIsPressed?.Invoke();
    }

    private void StopPressAnimation()
    {
        if (pressAnimation == null)
            return;

        StopCoroutine(pressAnimation);
        pressAnimation = null;
    }

    private IEnumerator AnimatePress()
    {
        float elapsed = 0f;

        while (true)
        {
            yield return null;

            elapsed += Time.unscaledDeltaTime;
            float duration = 1f / (2f * pulseSpeed);
            if (elapsed >= duration)
                break;

            float t = Mathf.Sin(Mathf.Clamp01(elapsed / duration) * Mathf.PI);
            t = TightenEdges(t, edgeTightness);
            rt.localScale = Vector3.LerpUnclamped(startScale, startScale + maxScale, t);
        }

        rt.localScale = startScale;
        pressAnimation = null;
    }
    static float TightenEdges(float t, float k)
    {
        float a = Mathf.Pow(t, k);
        float b = Mathf.Pow(1f - t, k);
        return a / (a + b);
    }
}
