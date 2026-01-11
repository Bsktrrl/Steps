using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : Singleton<PopUpManager>
{
    [Header("Parents")]
    [SerializeField] GameObject popupManager;


    //-----


    [Header("Popup - Parents")]
    [SerializeField] GameObject popup_Footprint_Parent;
    [SerializeField] GameObject popup_Essence_Parent;
    [SerializeField] GameObject popup_Skin_Parent;

    [Header("Popup - Children")]
    [SerializeField] List<GameObject> popup_Footprint_Children;
    [SerializeField] List<GameObject> popup_Essence_Children;
    [SerializeField] List<GameObject> popup_Skin_Children;


    //-----


    [Header("Abilities - Parents")]
    [SerializeField] GameObject ability_SwimSuit_Parent;
    [SerializeField] GameObject ability_SwiftSwim_Parent;
    [SerializeField] GameObject ability_Freeswim_Parent;

    [SerializeField] GameObject ability_Ascend_Parent;
    [SerializeField] GameObject ability_Descend_Parent;

    [SerializeField] GameObject ability_Dash_Parent;
    [SerializeField] GameObject ability_Jump_Parent;

    [SerializeField] GameObject ability_GrapplingHook_Parent;
    [SerializeField] GameObject ability_CeilingGrab_Parent;

    [Header("Abilities - Children")]
    [SerializeField] List<GameObject> abilities_SwimSuit_Children;
    [SerializeField] List<GameObject> abilities_SwiftSwim_Children;
    [SerializeField] List<GameObject> abilities_Freeswim_Children;
    [SerializeField] List<GameObject> abilities_Ascend_Children;
    [SerializeField] List<GameObject> abilities_Descend_Children;
    [SerializeField] List<GameObject> abilities_Dash_Children;
    [SerializeField] List<GameObject> abilities_Jump_Children;
    [SerializeField] List<GameObject> abilities_GrapplingHook_Children;
    [SerializeField] List<GameObject> abilities_CeilingGrab_Children;


    //-----


    [Header("Fade Settings")]
    public float fadeDuration = 0.85f;
    [SerializeField] float pickupMessageDuration = 1.5f;

    // Keep track of running fades so we can stop/replace them per parent
    private readonly Dictionary<GameObject, Coroutine> _runningFades = new();


    //-----


    [Header("Ability Active State")]
    public bool ability_Active;


    //--------------------


    public void OnEnable()
    {
        Interactable_Pickup.Action_StepsUpPickupGot += ShowFootprintPopup;
        Interactable_Pickup.Action_EssencePickupGot += ShowEssencePopup;
        Interactable_Pickup.Action_SkinPickupGot += ShowSkinPopup;
    }
    private void OnDisable()
    {
        Interactable_Pickup.Action_StepsUpPickupGot -= ShowFootprintPopup;
        Interactable_Pickup.Action_EssencePickupGot -= ShowEssencePopup;
        Interactable_Pickup.Action_SkinPickupGot -= ShowSkinPopup;
    }


    //--------------------


    void ShowFootprintPopup()
    {
        StartCoroutine(FootprintRoutine());
    }
    void ShowEssencePopup()
    {
        StartCoroutine(EssenceRoutine());
    }
    void ShowSkinPopup()
    {
        StartCoroutine(SkinRoutine());
    }

    public void ShowAbilityPopup(Abilities ability)
    {
        StartCoroutine(ShowAbilityPopup_Routine(ability, 0.5f));
    }
    IEnumerator ShowAbilityPopup_Routine(Abilities ability, float waitTime)
    {
        PlayerManager.Instance.PauseGame();

        yield return new WaitForSeconds(waitTime);

        switch (ability)
        {
            case Abilities.None:
                break;

            case Abilities.SwimSuit:
                ShowDisplay(popupManager, ability_SwimSuit_Parent, abilities_SwimSuit_Children);
                break;
            case Abilities.SwiftSwim:
                ShowDisplay(popupManager, ability_SwiftSwim_Parent, abilities_SwiftSwim_Children);
                break;
            case Abilities.Flippers:
                ShowDisplay(popupManager, ability_Freeswim_Parent, abilities_Freeswim_Children);
                break;
            case Abilities.Ascend:
                ShowDisplay(popupManager, ability_Ascend_Parent, abilities_Ascend_Children);
                break;
            case Abilities.Descend:
                ShowDisplay(popupManager, ability_Descend_Parent, abilities_Descend_Children);
                break;
            case Abilities.Dash:
                ShowDisplay(popupManager, ability_Dash_Parent, abilities_Dash_Children);
                break;
            case Abilities.Jumping:
                ShowDisplay(popupManager, ability_Jump_Parent, abilities_Jump_Children);
                break;
            case Abilities.CeilingGrab:
                ShowDisplay(popupManager, ability_CeilingGrab_Parent, abilities_CeilingGrab_Children);
                break;
            case Abilities.GrapplingHook:
                ShowDisplay(popupManager, ability_GrapplingHook_Parent, abilities_GrapplingHook_Children);
                break;

            default:
                break;
        }

        ability_Active = true;
    }
    public void HideAbilityPopup()
    {
        ability_Active = false;
        PlayerManager.Instance.UnpauseGame();

        HideDisplay(popupManager, ability_SwimSuit_Parent, abilities_SwimSuit_Children);
        HideDisplay(popupManager, ability_SwiftSwim_Parent, abilities_SwiftSwim_Children);
        HideDisplay(popupManager, ability_Freeswim_Parent, abilities_Freeswim_Children);

        HideDisplay(popupManager, ability_Ascend_Parent, abilities_Ascend_Children);
        HideDisplay(popupManager, ability_Descend_Parent, abilities_Descend_Children);

        HideDisplay(popupManager, ability_Dash_Parent, abilities_Dash_Children);
        HideDisplay(popupManager, ability_Jump_Parent, abilities_Jump_Children);

        HideDisplay(popupManager, ability_CeilingGrab_Parent, abilities_CeilingGrab_Children);
        HideDisplay(popupManager, ability_GrapplingHook_Parent, abilities_GrapplingHook_Children);
    }


    //--------------------


    IEnumerator FootprintRoutine()
    {
        ShowDisplay(popupManager, popup_Footprint_Parent, popup_Footprint_Children);

        yield return new WaitForSeconds(pickupMessageDuration);

        HideDisplay(popupManager, popup_Footprint_Parent, popup_Footprint_Children);
    }
    IEnumerator EssenceRoutine()
    {
        ShowDisplay(popupManager,popup_Essence_Parent, popup_Essence_Children);

        yield return new WaitForSeconds(pickupMessageDuration);

        HideDisplay(popupManager, popup_Essence_Parent, popup_Essence_Children);
    }
    IEnumerator SkinRoutine()
    {
        ShowDisplay(popupManager, popup_Skin_Parent, popup_Skin_Children);

        yield return new WaitForSeconds(pickupMessageDuration * 1.5f);

        HideDisplay(popupManager, popup_Skin_Parent, popup_Skin_Children);
    }


    //--------------------


    public void ShowDisplay(GameObject popupParent, GameObject categoryParent, List<GameObject> controllerVersions_Child)
    {
        if (categoryParent == null) return;

        popupParent.SetActive(true);
        popupManager.SetActive(true);
        if (ControllerState.Instance.activeController == InputType.Keyboard)
            controllerVersions_Child[0].SetActive(true);
        else if (ControllerState.Instance.activeController == InputType.PlayStation)
            controllerVersions_Child[1].SetActive(true);
        else if (ControllerState.Instance.activeController == InputType.Xbox)
            controllerVersions_Child[2].SetActive(true);

        StopFadeIfRunning(categoryParent);

        categoryParent.SetActive(true);

        var cg = GetOrAddCanvasGroup(categoryParent);
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;

        _runningFades[categoryParent] = StartCoroutine(FadeUI(categoryParent, 0f, 1f, fadeDuration, disableParentAtEnd: false));
    }
    public void HideDisplay(GameObject popupParent, GameObject categoryParent, List<GameObject> controllerVersions_Child)
    {
        if (categoryParent == null) return;

        StopFadeIfRunning(categoryParent);

        if (!categoryParent.activeInHierarchy) return;

        var cg = GetOrAddCanvasGroup(categoryParent);
        cg.interactable = false;
        cg.blocksRaycasts = false;

        _runningFades[categoryParent] = StartCoroutine(FadeUI(categoryParent, cg.alpha, 0f, fadeDuration, disableParentAtEnd: true));
    }

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
}
