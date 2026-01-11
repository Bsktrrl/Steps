using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : Singleton<PopUpManager>
{
    [Header("Parents")]
    [SerializeField] GameObject popupManager;

    [SerializeField] GameObject popup_Footprint_Parent;
    [SerializeField] GameObject popup_Essence_Parent;
    [SerializeField] GameObject popup_Skin_Parent;
    [SerializeField] GameObject popup_Ability_Parent;


    [Header("Tutorial - Childs")]
    [SerializeField] List<GameObject> tutorial_Footprint;
    [SerializeField] List<GameObject> tutorial_Essence;
    [SerializeField] List<GameObject> tutorial_Skin;


    [Header("Abilities - Parent")]
    [SerializeField] GameObject ability_SwimSuit;
    [SerializeField] GameObject ability_SwiftSwim;
    [SerializeField] GameObject ability_Freeswim;

    [SerializeField] GameObject ability_Ascend;
    [SerializeField] GameObject ability_Descend;

    [SerializeField] GameObject ability_Dash;
    [SerializeField] GameObject ability_Jump;

    [SerializeField] GameObject ability_GrapplingHook;
    [SerializeField] GameObject ability_CeilingGrab;

    [Header("Abilities - Childs")]
    [SerializeField] List<GameObject> abilities_SwimSuit_Childs;
    [SerializeField] List<GameObject> abilities_SwiftSwim_Childs;
    [SerializeField] List<GameObject> abilities_Freeswim_Childs;
    [SerializeField] List<GameObject> abilities_Ascend_Childs;
    [SerializeField] List<GameObject> abilities_Descend_Childs;
    [SerializeField] List<GameObject> abilities_Dash_Childs;
    [SerializeField] List<GameObject> abilities_Jump_Childs;
    [SerializeField] List<GameObject> abilities_GrapplingHook_Childs;
    [SerializeField] List<GameObject> abilities_CeilingGrab_Childs;


    //-----


    [Header("Fade Settings")]
    public float fadeDuration = 0.85f;
    [SerializeField] float pickupMessageDuration = 1.5f;

    // Keep track of running fades so we can stop/replace them per parent
    private readonly Dictionary<GameObject, Coroutine> _runningFades = new();


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
        switch (ability)
        {
            case Abilities.None:
                break;

            case Abilities.SwimSuit:
                //ShowDisplay();
                break;
            case Abilities.SwiftSwim:
                break;
            case Abilities.Flippers:
                break;
            case Abilities.Ascend:
                break;
            case Abilities.Descend:
                break;
            case Abilities.Dash:
                break;
            case Abilities.Jumping:
                break;
            case Abilities.CeilingGrab:
                break;
            case Abilities.GrapplingHook:
                break;

            default:
                break;
        }
    }


    //--------------------


    IEnumerator FootprintRoutine()
    {
        ShowDisplay(popupManager, popup_Footprint_Parent, tutorial_Footprint);

        yield return new WaitForSeconds(pickupMessageDuration);

        HideDisplay(popupManager, popup_Footprint_Parent, tutorial_Footprint);
    }
    IEnumerator EssenceRoutine()
    {
        ShowDisplay(popupManager,popup_Essence_Parent, tutorial_Essence);

        yield return new WaitForSeconds(pickupMessageDuration);

        HideDisplay(popupManager, popup_Essence_Parent, tutorial_Essence);
    }
    IEnumerator SkinRoutine()
    {
        ShowDisplay(popupManager, popup_Skin_Parent, tutorial_Skin);

        yield return new WaitForSeconds(pickupMessageDuration * 1.5f);

        HideDisplay(popupManager, popup_Skin_Parent, tutorial_Skin);
    }


    //--------------------


    public void ShowDisplay(GameObject mainParent, GameObject parent, List<GameObject> textVersions_Child)
    {
        if (parent == null) return;

        mainParent.SetActive(true);
        popupManager.SetActive(true);
        if (ControllerState.Instance.activeController == InputType.Keyboard)
            textVersions_Child[0].SetActive(true);
        else if (ControllerState.Instance.activeController == InputType.PlayStation)
            textVersions_Child[1].SetActive(true);
        else if (ControllerState.Instance.activeController == InputType.Xbox)
            textVersions_Child[2].SetActive(true);

        StopFadeIfRunning(parent);

        parent.SetActive(true);

        var cg = GetOrAddCanvasGroup(parent);
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;

        _runningFades[parent] = StartCoroutine(FadeUI(parent, 0f, 1f, fadeDuration, disableParentAtEnd: false));
    }
    public void HideDisplay(GameObject mainParent, GameObject parent, List<GameObject> textVersions_Child)
    {
        if (parent == null) return;

        StopFadeIfRunning(parent);

        if (!parent.activeInHierarchy) return;

        var cg = GetOrAddCanvasGroup(parent);
        cg.interactable = false;
        cg.blocksRaycasts = false;

        _runningFades[parent] = StartCoroutine(FadeUI(parent, cg.alpha, 0f, fadeDuration, disableParentAtEnd: true));
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
