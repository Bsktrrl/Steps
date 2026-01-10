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


    [Header("Texts")]
    [SerializeField] List<TextMeshProUGUI> Tutorial_Footprint_TextList;
    [SerializeField] List<TextMeshProUGUI> Tutorial_Essence_TextList;
    [SerializeField] List<TextMeshProUGUI> Tutorial_Skin_TextList;

    [Header("Image Lists")]
    [SerializeField] List<Image> Tutorial_Footprint_ImageList;
    [SerializeField] List<Image> Tutorial_Essence_ImageList;
    [SerializeField] List<Image> Tutorial_Skin_ImageList;


    //-----


    [Header("Fade Settings")]
    public float fadeDuration = 0.85f;

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


    //--------------------


    IEnumerator FootprintRoutine()
    {
        ShowDisplay(popupManager, popup_Footprint_Parent, Tutorial_Footprint_TextList, Tutorial_Footprint_ImageList);

        yield return new WaitForSeconds(2f);

        HideDisplay(popupManager, popup_Footprint_Parent, Tutorial_Footprint_TextList, Tutorial_Footprint_ImageList);
    }
    IEnumerator EssenceRoutine()
    {
        ShowDisplay(popupManager, popup_Essence_Parent, Tutorial_Essence_TextList, Tutorial_Essence_ImageList);

        yield return new WaitForSeconds(2f);

        HideDisplay(popupManager, popup_Essence_Parent, Tutorial_Essence_TextList, Tutorial_Essence_ImageList);
    }
    IEnumerator SkinRoutine()
    {
        ShowDisplay(popupManager, popup_Skin_Parent, Tutorial_Skin_TextList, Tutorial_Skin_ImageList);

        yield return new WaitForSeconds(3f);

        HideDisplay(popupManager, popup_Skin_Parent, Tutorial_Skin_TextList, Tutorial_Skin_ImageList);
    }


    //--------------------


    public void ShowDisplay(GameObject mainParent, GameObject parent, List<TextMeshProUGUI> textList, List<Image> imageList)
    {
        //Fade in all images from the list from 0 to 1
        if (parent == null) return;

        mainParent.SetActive(true);

        // Stop any previous fade on this parent
        StopFadeIfRunning(parent);

        // Ensure it's active before fading in
        parent.SetActive(true);

        // Start fully transparent (optional but usually desired)
        SetTextsAlpha(textList, 0f);
        SetImagesAlpha(imageList, 0f);

        if (ControllerState.Instance.activeController == InputType.PlayStation)
        {
            textList[1].gameObject.SetActive(true);
        }
        else if (ControllerState.Instance.activeController == InputType.Xbox)
        {
            textList[2].gameObject.SetActive(true);
        }
        else
        {
            textList[0].gameObject.SetActive(true);
        }

        _runningFades[parent] = StartCoroutine(FadeUI(textList, imageList, 0f, 1f, fadeDuration, disableParentAtEnd: false, parentToDisable: null));
    }
    public void HideDisplay(GameObject mainParent, GameObject parent, List<TextMeshProUGUI> textList, List<Image> imageList)
    {
        if (parent == null) return;

        StopFadeIfRunning(parent);

        if (!parent.activeInHierarchy) return;

        _runningFades[parent] = StartCoroutine(FadeUI(textList, imageList, 1f, 0f, fadeDuration, disableParentAtEnd: true, parentToDisable: parent));

        //for (int i = 0; i < textList.Count; i++)
        //{
        //    textList[i].gameObject.SetActive(false);
        //}

        mainParent.SetActive(true);
    }

    void StopFadeIfRunning(GameObject parent)
    {
        if (_runningFades.TryGetValue(parent, out var c) && c != null)
            StopCoroutine(c);

        _runningFades[parent] = null;
    }

    static void SetImagesAlpha(List<Image> images, float a)
    {
        if (images == null) return;

        for (int i = 0; i < images.Count; i++)
        {
            var img = images[i];
            if (img == null) continue;

            var c = img.color;
            c.a = a;
            img.color = c;
        }
    }

    static void SetTextsAlpha(List<TextMeshProUGUI> texts, float a)
    {
        if (texts == null) return;

        for (int i = 0; i < texts.Count; i++)
        {
            var tmp = texts[i];
            if (tmp == null) continue;

            var c = tmp.color;
            c.a = a;
            tmp.color = c;
        }
    }

    static IEnumerator FadeUI(List<TextMeshProUGUI> texts, List<Image> images, float from, float to, float duration, bool disableParentAtEnd, GameObject parentToDisable)
    {
        // Snap if duration is 0
        if (duration <= 0f)
        {
            SetTextsAlpha(texts, to);
            SetImagesAlpha(images, to);

            if (disableParentAtEnd && parentToDisable != null)
                parentToDisable.SetActive(false);

            yield break;
        }

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float eased = Mathf.SmoothStep(0f, 1f, t);
            float a = Mathf.Lerp(from, to, eased);

            SetTextsAlpha(texts, a);
            SetImagesAlpha(images, a);

            yield return null;
        }

        SetTextsAlpha(texts, to);
        SetImagesAlpha(images, to);

        if (disableParentAtEnd && parentToDisable != null)
            parentToDisable.SetActive(false);
    }
}
