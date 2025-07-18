using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Interactable_NPC : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] NPCs characterName;

    [Header("Stats from Excel")]
    [SerializeField] TextAsset dialogueSheet;
    int startRow = 2;
    int columns = 72;

    [Header("Dialogue Info")]
    public DialogueInfo dialogueInfo = new DialogueInfo();

    int segmentIndex = 0;

    [HideInInspector] public bool canInteract;
    [HideInInspector] public bool isInteracting;
    [HideInInspector] public string interact_Talk_Message = "Talk"; //Temp before language integration on UI elements

    [Header("To be saved in database")]
    [HideInInspector] public bool hasTalked;
    public int lastSegment;

    [Header("Animations")]
    [SerializeField] Animator anim;
    bool blink;
    int animationCount;


    //--------------------


    private void Start()
    {
        interact_Talk_Message = "Talk";

        BuildDialogue();

        dialogueInfo.npcName = characterName;
    }
    private void Update()
    {
        if (!blink)
        {
            StartCoroutine(RandomBlink());
        }

        TalkAnimation();
    }


    //--------------------


    private void OnEnable()
    {
        Player_KeyInputs.Action_dialogueButton_isPressed += StartNewDialogueSegment;
        Player_KeyInputs.Action_dialogueNextButton_isPressed += StartNewDialogueSegment;
        Player_KeyInputs.Action_InteractButton_isPressed += CanInteract;

        OptionButton.Action_OptionButtonIsPressed += StartNewDialogueSegment_OptionButton;
    }
    private void OnDisable()
    {
        Player_KeyInputs.Action_dialogueButton_isPressed -= StartNewDialogueSegment;
        Player_KeyInputs.Action_dialogueNextButton_isPressed -= StartNewDialogueSegment;
        Player_KeyInputs.Action_InteractButton_isPressed -= CanInteract;

        OptionButton.Action_OptionButtonIsPressed -= StartNewDialogueSegment_OptionButton;
    }


    //--------------------


    void CanInteract()
    {
        if (canInteract && !isInteracting)
        {
            canInteract = false;
            StartDialogue();
        }
    }
    void StartDialogue()
    {
        ButtonMessages.Instance.HideButtonMessage();

        DialogueManager.Instance.npcObject = this;
        DialogueManager.Instance.activeNPC = characterName;
        DialogueManager.Instance.segmentTotal = dialogueInfo.dialogueSegments.Count - 1;

        if (hasTalked)
        {
            DialogueManager.Instance.currentSegement = lastSegment;
            segmentIndex = lastSegment;
        }
        else
        {
            DialogueManager.Instance.currentSegement = 0;
            segmentIndex = 0;
        }
        
        SetupDialogueDisplay(segmentIndex, dialogueInfo.npcName);
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
            #region Segment Description

            //SegmentName
            if (excelData[columns * (i + startRow - 1) + 2] != "")
                dialogueInfo.dialogueSegments[i].segmentDescription = excelData[columns * (i + startRow - 1) + 2].Trim();
            else
                dialogueInfo.dialogueSegments[i].segmentDescription = "";

            #endregion

            #region Is last segment

            //SegmentName
            if (excelData[columns * (i + startRow - 1) + 4] != "")
                dialogueInfo.dialogueSegments[i].lastSegment = excelData[columns * (i + startRow - 1) + 4].Trim();
            else
                dialogueInfo.dialogueSegments[i].lastSegment = "";

            #endregion

            #region Animations

            //Player Animation number
            if (excelData[columns * (i + startRow - 1) + 6] != "")
                dialogueInfo.dialogueSegments[i].animation_Player = AnimationDataSplicer(excelData[columns * (i + startRow - 1) + 6].Trim());
            else
                dialogueInfo.dialogueSegments[i].animation_Player = null;

            //NPC Animation number
            if (excelData[columns * (i + startRow - 1) + 7] != "")
                dialogueInfo.dialogueSegments[i].animation_NPC = AnimationDataSplicer(excelData[columns * (i + startRow - 1) + 7].Trim());
            else
                dialogueInfo.dialogueSegments[i].animation_NPC = null;

            //Cutscene
            if (excelData[columns * (i + startRow - 1) + 8] != "")
                dialogueInfo.dialogueSegments[i].cutscene = ParseIntSafe(excelData, columns * (i + startRow - 1) + 8);
            else
                dialogueInfo.dialogueSegments[i].cutscene = -1;

            #endregion

            #region Stats

            //Stats
            if (excelData[columns * (i + startRow - 1) + 10] != "")
                dialogueInfo.dialogueSegments[i].startingStat = StatsDataSplicer(excelData[columns * (i + startRow - 1) + 10].Trim());
            else
                dialogueInfo.dialogueSegments[i].startingStat = null;

            if (excelData[columns * (i + startRow - 1) + 11] != "")
                dialogueInfo.dialogueSegments[i].statToGet = StatsDataSplicer(excelData[columns * (i + startRow - 1) + 11].Trim());
            else
                dialogueInfo.dialogueSegments[i].statToGet = null;

            #endregion

            #region Languages

            //Setup all languages at once
            for (int j = 0; j < DialogueManager.Instance.languageAmount; j++)
            {
                //Message
                if (excelData[columns * (i + startRow - 1) + 13 + (10 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageDialogueList[j] = excelData[columns * (i + startRow - 1) + 13 + (10 * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageDialogueList[j] = "";

                //Option 1
                if (excelData[columns * (i + startRow - 1) + 14 + (10 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_Text = excelData[columns * (i + startRow - 1) + 14 + (10 * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_Text = "";
                //Option 1 - Link
                if (excelData[columns * (i + startRow - 1) + 15 + (10 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 15 + (10 * j));
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_Linked = -1;

                //Option 2
                if (excelData[columns * (i + startRow - 1) + 16 + (10 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_Text = excelData[columns * (i + startRow - 1) + 16 + (10 * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_Text = "";
                //Option 1 - Link
                if (excelData[columns * (i + startRow - 1) + 17 + (10 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 17 + (10 * j));
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_Linked = -1;

                //Option 3
                if (excelData[columns * (i + startRow - 1) + 18 + (10 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_Text = excelData[columns * (i + startRow - 1) + 18 + (10 * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_Text = "";
                //Option 1 - Link
                if (excelData[columns * (i + startRow - 1) + 19 + (10 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 19 + (10 * j));
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_Linked = -1;

                //Option 4
                if (excelData[columns * (i + startRow - 1) + 20 + (10 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_Text = excelData[columns * (i + startRow - 1) + 20 + (10 * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_Text = "";
                //Option 1 - Link
                if (excelData[columns * (i + startRow - 1) + 21 + (10 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 21 + (10 * j));
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_Linked = -1;
            }

            #endregion

            CleanTheTextDialogue(i);
        }

        //Remove elements that doesn't have a name
        dialogueInfo.dialogueSegments = dialogueInfo.dialogueSegments.Where(obj => obj != null && !string.IsNullOrEmpty(obj.segmentDescription)).ToList();
    }
    List<int> AnimationDataSplicer(string text)
    {
        List<int> animationSplizer = new List<int>();

        if (string.IsNullOrWhiteSpace(text))
            return animationSplizer;

        string[] parts = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (string part in parts)
        {
            if (int.TryParse(part, out int number))
            {
                animationSplizer.Add(number);
            }
            else
            {
                Debug.LogWarning($"Invalid number in animation data: '{part}'");
            }
        }

        return animationSplizer;
    }
    DialogueStat StatsDataSplicer(string text)
    {
        DialogueStat statsSplizer = new DialogueStat();

        if (string.IsNullOrWhiteSpace(text))
            return statsSplizer;

        string[] parts = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (string part in parts)
        {
            if (int.TryParse(part, out int number))
            {
                statsSplizer.statNumber = number;
            }
            else if (!string.IsNullOrEmpty(part))
            {
                if (Enum.TryParse(part, out NPCs result))
                {
                    statsSplizer.characterName = result;
                }
                else
                {
                    Debug.LogWarning($"'{part}' is not a valid NPCs enum value.");
                }
            }
            else
            {
                Debug.LogWarning($"Invalid number in animation data: '{part}'");
            }
        }

        return statsSplizer;
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
        dialogueInfo.dialogueSegments[i].segmentDescription = CleanQuotes(dialogueInfo.dialogueSegments[i].segmentDescription);

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
        if (!isInteracting) return;
        if (TypewriterEffect.Instance.isTyping) return;

        //If current segment is a "lastSegment", end the dialogue
        if (dialogueInfo.dialogueSegments[segmentIndex].lastSegment != "")
        {
            lastSegment = segmentIndex;
            DialogueManager.Instance.EndDialogue();
        }

        //If the first element of the norwegian messageText is nothing, run it
        else if (dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option1_Text == "")
        {
            if (!TypewriterEffect.Instance.isTyping)
            {
                segmentIndex++;

                SetupDialogueDisplay(segmentIndex, dialogueInfo.npcName);
            }
        }
    }
    public void StartNewDialogueSegment_OptionButton()
    {
        if (!isInteracting) return;

        if (!TypewriterEffect.Instance.isTyping)
        {
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
        //Text
        if (dialogueInfo.dialogueSegments.Count > index)
        {
            switch (SettingsMenu.Instance.settingsData.currentLanguage)
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
            return;
        }

        //Animation
        if (dialogueInfo.dialogueSegments[index] != null && dialogueInfo.dialogueSegments[index].animation_NPC.Count > 0)
        {
            animationCount = 0;
            StartCoroutine(RunAnimations(index));

            //for (int i = 0; i < dialogueInfo.dialogueSegments[index].animation_NPC.Count; i++)
            //{
            //    PerformAnimation(dialogueInfo.dialogueSegments[index].animation_NPC[i]);
            //}
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


    //--------------------


    IEnumerator RunAnimations(int index)
    {
        int animationNumber = dialogueInfo.dialogueSegments[index].animation_NPC[animationCount];

        PerformAnimation(animationNumber);

        // Wait until the animator enters the state
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName(AnimationManager.Instance.animationList[animationNumber]))
        {
            yield return null;
        }

        // Wait until the animation finishes
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        // Start the next coroutine
        if (animationCount < dialogueInfo.dialogueSegments[index].animation_NPC.Count - 1)
        {
            animationCount++;
            StartCoroutine(RunAnimations(index));
        }
    }
    void PerformAnimation(int animNumber)
    {
        anim.SetTrigger(AnimationManager.Instance.animationList[animNumber]);
        anim.SetTrigger(AnimationManager.Instance.blink);
    }
    IEnumerator RandomBlink()
    {
        blink = true;

        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 10f));

        anim.SetTrigger(AnimationManager.Instance.blink);

        blink = false;
    }
    void TalkAnimation()
    {
        if (TypewriterEffect.Instance.isTyping)
            anim.SetBool("Talking", true);
        else
            anim.SetBool("Talking", false);
    }
}
