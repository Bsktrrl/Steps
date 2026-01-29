using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class PopUpManager_MainMenu : Singleton<PopUpManager_MainMenu>
{
    public InputSystemUIInputModule inputModule;

    [Header("Main Popup Parent")]
    [SerializeField] GameObject popup_Parent;

    [Header("Popup - PermanentAbilities")]
    [SerializeField] GameObject popup_PermanentAbilities_Parent;
    [SerializeField] TextMeshProUGUI popup_PermanentAbilities_Text;


    //-----


    [Header("Ability Active State")]
    public bool ability_Active;
    public bool ability_CanBeClosed;
    public float ability_CanBeClosed_Timer = 0.5f;


    [Header("Fade Settings")]
    public float fadeDuration_In = 0.35f;
    public float fadeDuration_Out = 0.75f;
    [SerializeField] float waitBeforeMessage_Duration = 0.75f;
    [SerializeField] float message_Duration = 4f;


    //Keep track of running fades so we can stop/replace them per parent
    private readonly Dictionary<GameObject, Coroutine> _runningFades = new();


    //--------------------


    public void ShowPermanentAbilityPopup(string _text)
    {
        inputModule.enabled = false;
        StartCoroutine(PermanentAbilityRoutine(_text));
    }


    //--------------------


    public void ShowPermanentAbilityPopup(Abilities ability)
    {
        StartCoroutine(ShowPermanentAbilityPopup_Routine(ability, 0.5f));
    }
    IEnumerator ShowPermanentAbilityPopup_Routine(Abilities ability, float waitTime)
    {
        ability_Active = true;
        PlayerManager.Instance.PauseGame();

        yield return new WaitForSeconds(waitTime);

        ShowDisplay(popup_Parent, popup_PermanentAbilities_Parent);

        yield return new WaitForSeconds(ability_CanBeClosed_Timer);

        ability_CanBeClosed = true;
    }
    public void HidePermanentAbilityPopup()
    {
        ability_Active = false;
        StartCoroutine(ButtonCanBePressedDuringPermanantAbilityMenuFadeIut_Delay(0.4f));
        PlayerManager.Instance.UnpauseGame();

        HideDisplay(popup_Parent, popup_PermanentAbilities_Parent);
    }
    IEnumerator ButtonCanBePressedDuringPermanantAbilityMenuFadeIut_Delay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        ability_CanBeClosed = false;
    }


    //--------------------


    IEnumerator PermanentAbilityRoutine(string _text)
    {
        yield return null;
        ShowDisplay(popup_Parent, popup_PermanentAbilities_Parent);

        popup_PermanentAbilities_Text.text = _text;

        yield return new WaitForSeconds(message_Duration);

        HideDisplay(popup_Parent, popup_PermanentAbilities_Parent);
    }


    //--------------------


    public void ShowDisplay(GameObject popupParent, GameObject categoryParent)
    {
        if (categoryParent == null) return;

        popupParent.SetActive(true);
        popup_Parent.SetActive(true);

        StopFadeIfRunning(categoryParent);

        categoryParent.SetActive(true);

        var cg = GetOrAddCanvasGroup(categoryParent);
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;

        _runningFades[categoryParent] = StartCoroutine(FadeUI(categoryParent, 0f, 1f, fadeDuration_In, disableParentAtEnd: false));
    }
    public void HideDisplay(GameObject popupParent, GameObject categoryParent)
    {
        if (categoryParent == null) return;

        StopFadeIfRunning(categoryParent);

        if (!categoryParent.activeInHierarchy) return;

        var cg = GetOrAddCanvasGroup(categoryParent);
        cg.interactable = false;
        cg.blocksRaycasts = false;

        _runningFades[categoryParent] = StartCoroutine(FadeUI(categoryParent, cg.alpha, 0f, fadeDuration_Out, disableParentAtEnd: true));

        StartCoroutine(InputModule_Delay(fadeDuration_Out + 0.2f));
    }


    //--------------------


    void StopFadeIfRunning(GameObject parent)
    {
        if (_runningFades.TryGetValue(parent, out var c) && c != null)
            StopCoroutine(c);

        _runningFades[parent] = null;
    }
    CanvasGroup GetOrAddCanvasGroup(GameObject parent)
    {
        if (!parent.TryGetComponent(out CanvasGroup cg))
            cg = parent.AddComponent<CanvasGroup>();

        return cg;
    }
    IEnumerator FadeUI(GameObject parent, float from, float to, float duration, bool disableParentAtEnd)
    {
        yield return new WaitForSeconds(waitBeforeMessage_Duration);

        var cg = GetOrAddCanvasGroup(parent);

        if (duration <= 0f)
        {
            cg.alpha = to;

            bool visible = to >= 1f;
            cg.interactable = visible;
            cg.blocksRaycasts = visible;

            if (disableParentAtEnd && !visible)
                parent.SetActive(false);

            yield break;
        }

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float eased = Mathf.SmoothStep(0f, 1f, t);
            cg.alpha = Mathf.Lerp(from, to, eased);
            yield return null;
        }

        cg.alpha = to;

        bool nowVisible = to >= 1f;
        cg.interactable = nowVisible;
        cg.blocksRaycasts = nowVisible;

        if (disableParentAtEnd && !nowVisible)
            parent.SetActive(false);
    }
    IEnumerator InputModule_Delay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        inputModule.enabled = true;
    }
}
