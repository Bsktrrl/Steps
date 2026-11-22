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
        tabletUI_Parent.SetActive(false);

        ReadExcelSheet();
    }


    //--------------------


    public void ShowTabletUI(GameObject parentObj)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeUI(parentObj, 1f, true));
    }
    public void HideTabletUI(GameObject parentObj)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeUI(parentObj, 0f, false));
    }
    public void ShowHUDUI(GameObject HUD_Object)
    {
        if (fadeCoroutine_HUD != null)
            StopCoroutine(fadeCoroutine_HUD);

        fadeCoroutine_HUD = StartCoroutine(FadeUI(HUD_Object, 1f, true));
    }
    public void HideHUDUI(GameObject HUD_Object)
    {
        if (fadeCoroutine_HUD != null)
            StopCoroutine(fadeCoroutine_HUD);

        fadeCoroutine_HUD = StartCoroutine(FadeUI(HUD_Object, 0f, false));
    }

    private IEnumerator FadeUI(GameObject parentObj, float targetAlpha, bool show)
    {
        CanvasGroup canvasGroup = parentObj.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = parentObj.AddComponent<CanvasGroup>();
        }

        if (show)
        {
            parentObj.SetActive(true);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                elapsedTime / fadeDuration
            );

            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        if (!show)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            // Keep the HUD active while hidden.
            // Only deactivate the Tablet UI.
            if (parentObj != hud_Parent)
                parentObj.SetActive(false);
        }

        fadeCoroutine = null;
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
