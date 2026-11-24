using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TabletManager : Singleton<TabletManager>
{
    [Header("Tablet Text Document")]
    public TextAsset tabletTextDocument;
    public TabletData tabletData = new TabletData();

    [Header("HUD Parent")]
    public GameObject hud_Parent;

    [Header("Tablet UI")]
    public GameObject tabletUI_Parent;
    public TextMeshProUGUI tabletUI_Text;

    [Header("Fade Settings")]
    public float fadeDuration = 0.25f;

    private Coroutine fadeCoroutine;
    private Coroutine fadeCoroutine_HUD;

    //--------------------


    private void Start()
    {
        if (tabletUI_Parent)
            tabletUI_Parent.SetActive(false);

        ReadExcelSheet();
    }


    //--------------------


    public void ShowTabletUI(GameObject parentObj) => StartFade(parentObj, show: true, isHud: false);
    public void HideTabletUI(GameObject parentObj) => StartFade(parentObj, show: false, isHud: false);
    public void ShowHUDUI(GameObject parentObj) => StartFade(parentObj, show: true, isHud: true);
    public void HideHUDUI(GameObject parentObj) => StartFade(parentObj, show: false, isHud: true);

    private void StartFade(GameObject parentObj, bool show, bool isHud)
    {
        ref Coroutine routine = ref (isHud ? ref fadeCoroutine_HUD : ref fadeCoroutine);
        StopFade(ref routine);
        if (!parentObj)
            return;

        CanvasGroup canvasGroup = GetCanvasGroup(parentObj);
        if (fadeDuration <= 0f)
            SetFadeEndpoint(canvasGroup, show, isHud);
        else
            routine = StartCoroutine(FadeUI(canvasGroup, show, isHud));
    }

    private void StopFade(ref Coroutine routine)
    {
        if (routine != null)
            StopCoroutine(routine);
        routine = null;
    }

    private IEnumerator FadeUI(CanvasGroup canvasGroup, bool show, bool isHud)
    {
        float targetAlpha = show ? 1f : 0f;

        if (show)
        {
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (canvasGroup && elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                elapsedTime / fadeDuration
            );

            yield return null;
        }

        if (canvasGroup)
            SetFadeEndpoint(canvasGroup, show, isHud);
        if (isHud)
            fadeCoroutine_HUD = null;
        else
            fadeCoroutine = null;
    }

    internal void RestoreGameplayUI()
    {
        StopFade(ref fadeCoroutine);
        StopFade(ref fadeCoroutine_HUD);
        if (tabletUI_Parent)
            SetFadeEndpoint(GetCanvasGroup(tabletUI_Parent), show: false, isHud: false);
        if (hud_Parent)
            SetFadeEndpoint(GetCanvasGroup(hud_Parent), show: true, isHud: true);
    }

    private static CanvasGroup GetCanvasGroup(GameObject parentObj)
    {
        return parentObj.TryGetComponent(out CanvasGroup group) ? group : parentObj.AddComponent<CanvasGroup>();
    }

    private static void SetFadeEndpoint(CanvasGroup group, bool show, bool isHud)
    {
        group.alpha = show ? 1f : 0f;
        group.interactable = show;
        group.blocksRaycasts = show;
        if (show || !isHud)
            group.gameObject.SetActive(show);
    }

    internal bool TryGetReadText(int entry, out string text)
    {
        text = null;
        var segments = tabletData?.tabletDataSegment;
        if (segments == null || segments.Count == 0)
            return false;

        var translations = segments[entry >= 0 && entry < segments.Count ? entry : 0]?.languageDialogueList;
        if (translations == null || translations.Count == 0)
            return false;

        int english = (int)Languages.English;
        int language = (int)(DataManager.Instance.settingData_StoreList?.currentLanguage ?? Languages.English);
        if (language >= 0 && language < translations.Count)
            text = translations[language];
        if (string.IsNullOrWhiteSpace(text) && english < translations.Count)
            text = translations[english];
        return true;
    }


    //--------------------


    public void ReadExcelSheet()
    {
        if (tabletTextDocument)
            tabletData = ReadTabletData(tabletTextDocument);
    }

    internal static TabletData ReadTabletData(TextAsset document)
    {
        var data = new TabletData();
        if (!document)
            return data;

        const int languageCount = 3;
        string[] rows = document.text.Split('\n');
        for (int i = 1; i < rows.Length; i++)
        {
            string[] cells = rows[i].Split(';');
            string description = CleanQuotes(cells[0]);
            if (description.Length == 0)
                continue;

            var entry = new TabletDataSegment { segmentDescription = description };
            for (int language = 1; language <= languageCount; language++)
                entry.languageDialogueList.Add(language < cells.Length ? CleanQuotes(cells[language]) : string.Empty);
            data.tabletDataSegment.Add(entry);
        }

        return data;
    }

    private static string CleanQuotes(string input)
    {
        input = input.Trim();
        if (input.Length >= 2 && input[0] == '"' && input[input.Length - 1] == '"')
            input = input.Substring(1, input.Length - 2);
        return input.Replace("\"\"", "\"").Trim();
    }
}

[Serializable]
public class TabletData
{
    public List<TabletDataSegment> tabletDataSegment = new List<TabletDataSegment>();
}
[Serializable]
public class TabletDataSegment
{
    public string segmentDescription;

    [Header("Languages")]
    public List<string> languageDialogueList = new List<string>();
}
