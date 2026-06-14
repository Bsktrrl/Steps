using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class TabletManager : Singleton<TabletManager>
{
    [Header("Tablet Text Document")]
    public TextAsset tabletTextDocument;
    public TabletData tabletData = new TabletData();

    int startRow = 2;
    int columns = 4; //Size + 1

    [Header("Tablet UI")]
    public GameObject tabletUI_Parent;
    public TextMeshProUGUI tabletUI_Text;

    [Header("Fade Settings")]
    public float fadeDuration = 0.25f;

    private Coroutine fadeCoroutine;

    int currentLanguageAmount = 3;


    //--------------------


    private void Start()
    {
        tabletUI_Parent.SetActive(false);

        BuildTabletTextDatabase();
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
            parentObj.SetActive(false);
        }

        fadeCoroutine = null;
    }


    //--------------------


    void BuildTabletTextDatabase()
    {
        ReadExcelSheet();
    }
    public void ReadExcelSheet()
    {
        if (tabletTextDocument == null) return;

        //Separate Excel Sheet into a string[] by its ";"
        string[] excelData = tabletTextDocument.text.Split(new string[] { ";", "\n" }, StringSplitOptions.None);

        // Calculate the size of the Excel table
        int excelTableSize = (excelData.Length / columns - 1) - 0;

        // Initialize the list
        tabletData.tabletDataSegment = new List<TabletDataSegment>();

        // Populate the list with default DataObject instances
        for (int i = 0; i < excelTableSize; i++)
        {
            TabletDataSegment tabletSegment = new TabletDataSegment();

            for (int j = 0; j < currentLanguageAmount; j++)
            {
                tabletSegment.languageDialogueList.Add(null);
            }

            tabletData.tabletDataSegment.Add(tabletSegment);
        }

        //Fill the new element with data
        for (int i = 0; i < excelTableSize; i++)
        {
            #region Description

            //Segment Description
            if (excelData[columns * (i + startRow - 1) + 0] != "")
                tabletData.tabletDataSegment[i].segmentDescription = excelData[columns * (i + startRow - 1) + 0].Trim();
            else
                tabletData.tabletDataSegment[i].segmentDescription = "";

            #endregion

            #region Languages

            for (int j = 0; j < currentLanguageAmount; j++)
            {
                if (excelData[columns * (i + startRow - 1) + 1 + j] != "")
                    tabletData.tabletDataSegment[i].languageDialogueList[j] = excelData[columns * (i + startRow - 1) + 1 + j].Trim();
                else
                    tabletData.tabletDataSegment[i].languageDialogueList[j] = "";
            }

            #endregion

            CleanTheTextDialogue(i);
        }

        //Remove elements that doesn't have a name
        tabletData.tabletDataSegment = tabletData.tabletDataSegment.Where(obj => obj != null && !string.IsNullOrEmpty(obj.segmentDescription)).ToList();
    }
    void CleanTheTextDialogue(int i)
    {
        tabletData.tabletDataSegment[i].segmentDescription = CleanQuotes(tabletData.tabletDataSegment[i].segmentDescription);

        for (int j = 0; j < tabletData.tabletDataSegment[i].languageDialogueList.Count; j++)
        {
            tabletData.tabletDataSegment[i].languageDialogueList[j] = CleanQuotes(tabletData.tabletDataSegment[i].languageDialogueList[j]);
        }
    }
    string CleanQuotes(string input)
    {
        // Remove enclosing quotes, and replace double double-quotes with a single one
        if (input == "")
        {
            return "";
        }
        else if (input == null)
        {
            return "";
        }
        else if (input.StartsWith("\"") && input.EndsWith("\""))
        {
            input = input.Substring(1, input.Length - 2);
        }

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