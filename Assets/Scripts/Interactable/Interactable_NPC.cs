using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Interactable_NPC : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] NPCs characterName;

    [Header("Stats from Excel")]
    [SerializeField] TextAsset dialogueSheet;
    int startRow = 2;
    int columns = 59;

    [Header("Dialogue Info")]
    [SerializeField] DialogueInfo dialogueInfo = new DialogueInfo();

    int segmentIndex = 0;


    //--------------------


    private void Start()
    {
        BuildDialogue();

        dialogueInfo.npcName = characterName;
        DialogueManager.Instance.activeNPC = characterName;

        DialogueManager.Instance.segmentTotal = dialogueInfo.dialogueSegments.Count - 1;
        DialogueManager.Instance.currentSegement = segmentIndex;

        SetupDialogueDisplay(segmentIndex, dialogueInfo.npcName);
    }

    private void OnEnable()
    {
        Player_KeyInputs.Action_dialogueButton_isPressed += StartNewDialogueSegment;
        Player_KeyInputs.Action_dialogueNextButton_isPressed += StartNewDialogueSegment;
        OptionButton.Action_OptionButtonIsPressed += StartNewDialogueSegment_OptionButton;
    }
    private void OnDisable()
    {
        Player_KeyInputs.Action_dialogueButton_isPressed -= StartNewDialogueSegment;
        Player_KeyInputs.Action_dialogueNextButton_isPressed -= StartNewDialogueSegment;
        OptionButton.Action_OptionButtonIsPressed -= StartNewDialogueSegment_OptionButton;
    }


    //--------------------


    void BuildDialogue()
    {
        ReadExcelSheet();
    }
    public void ReadExcelSheet()
    {
        //Separate Excel Sheet into a string[] by its ";"
        string[] excelData = dialogueSheet.text.Split(new string[] { ";", "\n" }, StringSplitOptions.None);

        // Calculate the size of the Excel table
        int excelTableSize = (excelData.Length / columns - 1) - 0;

        // Initialize the list
        dialogueInfo.dialogueSegments = new List<DialogueSegment>();

        // Populate the list with default DataObject instances
        for (int i = 0; i < excelTableSize; i++)
        {
            DialogueSegment dialogueSegment = new DialogueSegment();

            for (int j = 0; j < DialogueManager.Instance.languageAmount; j++)
            {
                dialogueSegment.languageDialogueList.Add(null);
                dialogueSegment.languageOptionList.Add(new LanguageOptions());
            }

            dialogueInfo.dialogueSegments.Add(dialogueSegment);
        }

        //Fill the new element with data
        for (int i = 0; i < excelTableSize; i++)
        {
            #region Segment Name

            //SegmentName
            if (excelData[columns * (i + startRow - 1) + 2] != "")
                dialogueInfo.dialogueSegments[i].segmentName = excelData[columns * (i + startRow - 1) + 2].Trim();
            else
                dialogueInfo.dialogueSegments[i].segmentName = "";

            #endregion

            #region Animations

            //Player Animation number
            if (excelData[columns * (i + startRow - 1) + 4] != "")
                dialogueInfo.dialogueSegments[i].animation_Player = ParseIntSafe(excelData, columns * (i + startRow - 1) + 4);
            else
                dialogueInfo.dialogueSegments[i].animation_Player = -1;

            //NPC Animation number
            if (excelData[columns * (i + startRow - 1) + 5] != "")
                dialogueInfo.dialogueSegments[i].animation_NPC = ParseIntSafe(excelData, columns * (i + startRow - 1) + 5);
            else
                dialogueInfo.dialogueSegments[i].animation_NPC = -1;

            //Cutscene
            if (excelData[columns * (i + startRow - 1) + 6] != "")
                dialogueInfo.dialogueSegments[i].cutscene = ParseIntSafe(excelData, columns * (i + startRow - 1) + 6);
            else
                dialogueInfo.dialogueSegments[i].cutscene = -1;

            #endregion

            #region Stats

            //Stats
            if (excelData[columns * (i + startRow - 1) + 8] != "")
                dialogueInfo.dialogueSegments[i].dialogueStats = ParseIntSafe(excelData, columns * (i + startRow - 1) + 8);
            else
                dialogueInfo.dialogueSegments[i].dialogueStats = -1;

            #endregion

            #region Languages

            //Setup all languages at once
            for (int j = 0; j < DialogueManager.Instance.languageAmount; j++)
            {
                //Message
                if (excelData[columns * (i + startRow - 1) + 10 + (10 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageDialogueList[j] = excelData[columns * (i + startRow - 1) + 10 + (10 * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageDialogueList[j] = "";

                //Option 1
                if (excelData[columns * (i + startRow - 1) + 11 + (10 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_Text = excelData[columns * (i + startRow - 1) + 11 + (10 * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_Text = "";
                //Option 1 - Link
                if (excelData[columns * (i + startRow - 1) + 12 + (10 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 12 + (10 * j));
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_Linked = -1;

                //Option 2
                if (excelData[columns * (i + startRow - 1) + 13 + (10 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_Text = excelData[columns * (i + startRow - 1) + 13 + (10 * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_Text = "";
                //Option 1 - Link
                if (excelData[columns * (i + startRow - 1) + 14 + (10 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 14 + (10 * j));
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_Linked = -1;

                //Option 3
                if (excelData[columns * (i + startRow - 1) + 15 + (10 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_Text = excelData[columns * (i + startRow - 1) + 15 + (10 * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_Text = "";
                //Option 1 - Link
                if (excelData[columns * (i + startRow - 1) + 16 + (10 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 16 + (10 * j));
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_Linked = -1;

                //Option 4
                if (excelData[columns * (i + startRow - 1) + 17 + (10 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_Text = excelData[columns * (i + startRow - 1) + 17 + (10 * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_Text = "";
                //Option 1 - Link
                if (excelData[columns * (i + startRow - 1) + 18 + (10 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 18 + (10 * j));
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_Linked = -1;
            }

            #endregion

            CleanTheTextDialogue(i);
        }

        //Remove elements that doesn't have a name
        dialogueInfo.dialogueSegments = dialogueInfo.dialogueSegments.Where(obj => obj != null && !string.IsNullOrEmpty(obj.segmentName)).ToList();
    }
    int ParseIntSafe(string[] data, int index)
    {
        if (index < 0 || index >= data.Length) return -1;
        string cleaned = new string(data[index].Where(char.IsDigit).ToArray());

        if (int.TryParse(cleaned, out int result)) return result;

        return -1;
    }
    void CleanTheTextDialogue(int i)
    {
        dialogueInfo.dialogueSegments[i].segmentName = CleanQuotes(dialogueInfo.dialogueSegments[i].segmentName);

        for (int j = 0; j < DialogueManager.Instance.languageAmount; j++)
        {
            dialogueInfo.dialogueSegments[i].languageDialogueList[j] = CleanQuotes(dialogueInfo.dialogueSegments[i].languageDialogueList[j]);
            dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_Text = CleanQuotes(dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_Text);
            dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_Text = CleanQuotes(dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_Text);
            dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_Text = CleanQuotes(dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_Text);
            dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_Text = CleanQuotes(dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_Text);
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




    //--------------------


    void StartNewDialogueSegment()
    {
        //If the first element of the norwegian messageText is nothing, run it
        if (dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option1_Text == "")
        {
            if (!TypewriterEffect.Instance.isTyping)
            {
                segmentIndex++;

                Player_KeyInputs.Instance.dialogueButton_isPressed = false;
                SetupDialogueDisplay(segmentIndex, dialogueInfo.npcName);
            }
        }
    }
    public void StartNewDialogueSegment_OptionButton()
    {
        if (!TypewriterEffect.Instance.isTyping)
        {
            Player_KeyInputs.Instance.dialogueButton_isPressed = false;

            int segment = -1;
            if (DialogueManager.Instance.selectedButton == 1)
                segment = dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option1_Linked - 1;
            else if (DialogueManager.Instance.selectedButton == 2)
                segment = dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option2_Linked - 1;
            else if (DialogueManager.Instance.selectedButton == 3)
                segment = dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option3_Linked - 1;
            else if (DialogueManager.Instance.selectedButton == 4)
                segment = dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option4_Linked - 1;

            segmentIndex = segment;
            DialogueManager.Instance.currentSegement = segmentIndex;

            print("DialogueSegment: Button: " + (DialogueManager.Instance.selectedButton - 1) + " | Index: " + segmentIndex + " | Segment: " + segment);

            SetupDialogueDisplay(segment, dialogueInfo.npcName); 
        }
    } 


    //--------------------


    void SetupDialogueDisplay(int index, NPCs npc)
    {
        if (dialogueInfo.dialogueSegments.Count > index)
        {
            switch (DialogueManager.Instance.currentLanguage)
            {
                case Languages.Norwegian:
                    DialogueManager.Instance.SetupDialogueSegment_toDisplay(npc, dialogueInfo.dialogueSegments[index].languageDialogueList[0], dialogueInfo.dialogueSegments[index].languageOptionList[0]);
                    break;
                case Languages.English:
                    DialogueManager.Instance.SetupDialogueSegment_toDisplay(npc, dialogueInfo.dialogueSegments[index].languageDialogueList[1], dialogueInfo.dialogueSegments[index].languageOptionList[1]);
                    break;
                case Languages.German:
                    DialogueManager.Instance.SetupDialogueSegment_toDisplay(npc, dialogueInfo.dialogueSegments[index].languageDialogueList[2], dialogueInfo.dialogueSegments[index].languageOptionList[2]);
                    break;
                case Languages.Japanese:
                    DialogueManager.Instance.SetupDialogueSegment_toDisplay(npc, dialogueInfo.dialogueSegments[index].languageDialogueList[3], dialogueInfo.dialogueSegments[index].languageOptionList[3]);
                    break;
                case Languages.Chinese:
                    DialogueManager.Instance.SetupDialogueSegment_toDisplay(npc, dialogueInfo.dialogueSegments[index].languageDialogueList[4], dialogueInfo.dialogueSegments[index].languageOptionList[4]);
                    break;

                default:
                    break;
            }
        }
        else
        {
            DialogueManager.Instance.EndDialogue();
        }
    }
    void SetupDialogue(DialogueSegment dialogueSegment)
    {
        dialogueInfo.dialogueSegments.Add(dialogueSegment);
    }
    DialogueSegment SetupDialogueSegment(List<string> languageDialogueList, List<LanguageOptions> languageOptionList)
    {
        DialogueSegment segment = new DialogueSegment();

        segment.languageDialogueList = languageDialogueList;
        segment.languageOptionList = languageOptionList;

        return segment;
    }

    List<string> SetupLanguageTextList(string language_1, string language_2, string language_3, string language_4, string language_5)
    {
        List<string> languageList = new List<string>();

        languageList.Add(language_1);
        languageList.Add(language_2);
        languageList.Add(language_3);
        languageList.Add(language_4);
        languageList.Add(language_5);

        return languageList;
    }
    List<LanguageOptions> SetupOptionLanguages(LanguageOptions language1, LanguageOptions language2, LanguageOptions language3, LanguageOptions language4, LanguageOptions language5)
    {
        List<LanguageOptions> languageOptions = new List<LanguageOptions>();

        languageOptions.Add(language1);
        languageOptions.Add(language2);
        languageOptions.Add(language3);
        languageOptions.Add(language4);
        languageOptions.Add(language5);

        return languageOptions;
    }
    LanguageOptions SetupOption(string _option1_Text, int _segment1_Linked, string _option2_Text, int _segment2_Linked, string _option3_Text, int _segment3_Linked, string _option4_Text, int _segment4_Linked)
    {
        LanguageOptions option = new LanguageOptions();
        option.option1_Text = _option1_Text;
        option.option1_Linked = _segment1_Linked;
        option.option2_Text = _option2_Text;
        option.option2_Linked = _segment2_Linked;
        option.option3_Text = _option3_Text;
        option.option3_Linked = _segment3_Linked;
        option.option4_Text = _option4_Text;
        option.option4_Linked = _segment4_Linked;

        return option;
    }
}
