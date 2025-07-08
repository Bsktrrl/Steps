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

        #region Hardcoded dialogueSegments

        //#region Segment 1

        //SetupDialogue(SetupDialogueSegment
        //(
        //    SetupLanguageTextList
        //    (
        //        "Velkommen, vandrer! Det er sjelden noen går denne stien. Skogen husker fottrinn som dine...",
        //        "Welcome, traveler! It's rare to see someone take this path. The forest remembers footsteps like yours...",
        //        "German",
        //        "Japanese",
        //        "Chinese"
        //    ),

        //    SetupOptionLanguages
        //    (
        //        SetupOption
        //        (
        //            "Hvem er du?", 2,
        //            "Hva er dette stedet?", 3,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "Who are you?", 2,
        //            "What is this place?", 3,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "German option 1", 2,
        //            "German option 2", 3,
        //            "German option 3", 0,
        //            "German option 4", 0
        //        ),
        //        SetupOption
        //        (
        //            "Japanese option 1", 2,
        //            "Japanese option 2", 3,
        //            "Japanese option 3", 0,
        //            "Japanese option 4", 0
        //        ),
        //        SetupOption
        //        (
        //            "Chinese option 1", 2,
        //            "Chinese option 2", 3,
        //            "Chinese option 3", 0,
        //            "Chinese option 4", 0
        //        )
        //    )

        //));

        //#endregion

        //#region Segment 2

        //SetupDialogue(SetupDialogueSegment
        //(
        //    SetupLanguageTextList
        //    (
        //        "Jeg er Floríel, en gang vokter av denne skogen. Min tid er forbi, men stemmen min henger igjen.",
        //        "I am Floríel, once guardian of these woods. My time has long passed, yet my voice lingers.",
        //        "German",
        //        "Japanese",
        //        "Chinese"
        //    ),

        //    SetupOptionLanguages
        //    (
        //        SetupOption
        //        (
        //            "Hvorfor er du fortsatt her?", 4,
        //            "Kan du hjelpe meg?", 5,
        //            "Ha det.", 10,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "Why are you still here?", 4,
        //            "Can you help me?", 5,
        //            "Goodbye", 10,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "German option 1", 4,
        //            "German option 2", 5,
        //            "German option 3", 10,
        //            "German option 4", 0
        //        ),
        //        SetupOption
        //        (
        //            "Japanese option 1", 4,
        //            "Japanese option 2", 5,
        //            "Japanese option 3", 10,
        //            "Japanese option 4", 0
        //        ),
        //        SetupOption
        //        (
        //            "Chinese option 1", 4,
        //            "Chinese option 2", 5,
        //            "Chinese option 3", 10,
        //            "Chinese option 4", 0
        //        )
        //    )
        //));

        //#endregion

        //#region Segment 3

        //SetupDialogue(SetupDialogueSegment
        //(
        //    SetupLanguageTextList
        //    (
        //        "Dette stedet var en gang et fristed, før ilden kom. Nå er det bare ekko og aske igjen.",
        //        "This place was once a sanctuary, before the fire. Now, only echoes and ash remain.",
        //        "German",
        //        "Japanese",
        //        "Chinese"
        //    ),

        //    SetupOptionLanguages
        //    (
        //        SetupOption
        //        (
        //            "Hvilken ild?", 6,
        //            "Kan det gjenopprettes?", 7,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "What fire?", 6,
        //            "Can it be restored?", 7,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "German option 1", 6,
        //            "German option 2", 7,
        //            "German option 3", 0,
        //            "German option 4", 0
        //        ),
        //        SetupOption
        //        (
        //            "Japanese option 1", 6,
        //            "Japanese option 2", 7,
        //            "Japanese option 3", 0,
        //            "Japanese option 4", 0
        //        ),
        //        SetupOption
        //        (
        //            "Chinese option 1", 6,
        //            "Chinese option 2", 7,
        //            "Chinese option 3", 0,
        //            "Chinese option 4", 0
        //        )
        //    )
        //));

        //#endregion

        //#region Segment 4

        //SetupDialogue(SetupDialogueSegment
        //(
        //    SetupLanguageTextList
        //    (
        //        "Eden min binder meg. Før skogen tilgir, kan jeg ikke hvile.",
        //        "My oath binds me. Until the forest forgives, I cannot rest.",
        //        "German",
        //        "Japanese",
        //        "Chinese"
        //    ),

        //    SetupOptionLanguages
        //    (
        //        SetupOption
        //        (
        //            "Hvordan kan jeg hjelpe?", 5,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "How can I help?", 5,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "German option 1", 5,
        //            "German option 2", 0,
        //            "German option 3", 0,
        //            "German option 4", 0
        //        ),
        //        SetupOption
        //        (
        //            "Japanese option 1", 5,
        //            "Japanese option 2", 0,
        //            "Japanese option 3", 0,
        //            "Japanese option 4", 0
        //        ),
        //        SetupOption
        //        (
        //            "Chinese option 1", 5,
        //            "Chinese option 2", 0,
        //            "Chinese option 3", 0,
        //            "Chinese option 4", 0
        //        )
        //    )
        //));

        //#endregion

        //#region Segment 5

        //SetupDialogue(SetupDialogueSegment
        //(
        //    SetupLanguageTextList
        //    (
        //        "Dypere inne ligger hjertetroten, den slår fortsatt. Rens den, og kanskje skogen kan puste igjen.",
        //        "Deep within lies the heartroot, still beating. Cleanse it, and the forest may breathe again.",
        //        "German",
        //        "Japanese",
        //        "Chinese"
        //    ),

        //    SetupOptionLanguages
        //    (
        //        SetupOption
        //        (
        //            "Hvor finner jeg den?", 8,
        //            "Er det farlig?", 9,
        //            "Jeg skal prøve.", 10,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "Where do I find it?", 8,
        //            "Is it dangerous?", 9,
        //            "I’ll try.", 10,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "German option 1", 8,
        //            "German option 2", 9,
        //            "German option 3", 10,
        //            "German option 4", 0
        //        ),
        //        SetupOption
        //        (
        //            "Japanese option 1", 8,
        //            "Japanese option 2", 9,
        //            "Japanese option 3", 10,
        //            "Japanese option 4", 0
        //        ),
        //        SetupOption
        //        (
        //            "Chinese option 1", 8,
        //            "Chinese option 2", 9,
        //            "Chinese option 3", 10,
        //            "Chinese option 4", 0
        //        )
        //    )
        //));

        //#endregion

        //#region Segment 6

        //SetupDialogue(SetupDialogueSegment
        //(
        //    SetupLanguageTextList
        //    (
        //        "Ilden kom plutselig. Ingen advarsel, ingen nåde. Noen sier den ble påkalt...",
        //        "The fire came suddenly. No warning, no mercy. Some say it was summoned..",
        //        "German",
        //        "Japanese",
        //        "Chinese"
        //    ),

        //    SetupOptionLanguages
        //    (
        //        SetupOption
        //        (
        //            "", 0,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "", 0,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "", 0,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "", 0,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "", 0,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        )
        //    )
        //));

        //#endregion

        //#region Segment 7

        //SetupDialogue(SetupDialogueSegment
        //(
        //    SetupLanguageTextList
        //    (
        //        "Kanskje. Men skogens sår er dype. Det vil kreve mer enn håp.",
        //        "Perhaps. But the forest's wounds run deep. It will take more than hope.",
        //        "German",
        //        "Japanese",
        //        "Chinese"
        //    ),

        //    SetupOptionLanguages
        //    (
        //        SetupOption
        //        (
        //            "Jeg skal gjøre det jeg kan.", 10,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "I'll do what I can.", 10,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "German option 1", 5,
        //            "German option 2", 0,
        //            "German option 3", 0,
        //            "German option 4", 0
        //        ),
        //        SetupOption
        //        (
        //            "Japanese option 1", 5,
        //            "Japanese option 2", 0,
        //            "Japanese option 3", 0,
        //            "Japanese option 4", 0
        //        ),
        //        SetupOption
        //        (
        //            "Chinese option 1", 5,
        //            "Chinese option 2", 0,
        //            "Chinese option 3", 0,
        //            "Chinese option 4", 0
        //        )
        //    )
        //));

        //#endregion

        //#region Segment 8

        //SetupDialogue(SetupDialogueSegment
        //(
        //    SetupLanguageTextList
        //    (
        //        "Under den gamle steinsirkelen, bortenfor tornekrattet. Få vender tilbake uforandret.",
        //        "Beneath the old stone circle, beyond the briars. Few who go there return unchanged.",
        //        "German",
        //        "Japanese",
        //        "Chinese"
        //    ),

        //    SetupOptionLanguages
        //    (
        //        SetupOption
        //        (
        //            "", 0,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "", 0,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "", 0,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "", 0,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "", 0,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        )
        //    )
        //));

        //#endregion

        //#region Segment 9

        //SetupDialogue(SetupDialogueSegment
        //(
        //    SetupLanguageTextList
        //    (
        //        "Ja. Hjerteroten voktes av det som er igjen av skogens raseri.",
        //        "Yes. The heartroot is guarded by what remains of the forest's rage.",
        //        "German",
        //        "Japanese",
        //        "Chinese"
        //    ),

        //    SetupOptionLanguages
        //    (
        //        SetupOption
        //        (
        //            "Jeg vil møte det.", 10,
        //            "Kanskje jeg ikke er klar.", 10,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "I’ll face it.", 10,
        //            "Maybe I’m not ready.", 10,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "German option 1", 10,
        //            "German option 2", 10,
        //            "German option 3", 0,
        //            "German option 4", 0
        //        ),
        //        SetupOption
        //        (
        //            "Japanese option 1", 10,
        //            "Japanese option 2", 10,
        //            "Japanese option 3", 0,
        //            "Japanese option 4", 0
        //        ),
        //        SetupOption
        //        (
        //            "Chinese option 1", 10,
        //            "Chinese option 2", 10,
        //            "Chinese option 3", 0,
        //            "Chinese option 4", 0
        //        )
        //    )
        //));

        //#endregion

        //#region Segment 10

        //SetupDialogue(SetupDialogueSegment
        //(
        //    SetupLanguageTextList
        //    (
        //        "Da gå, og må røttene huske ditt mot. Jeg vil våke.",
        //        "Then go, and may the roots remember your courage. I will be watching.",
        //        "German",
        //        "Japanese",
        //        "Chinese"
        //    ),

        //    SetupOptionLanguages
        //    (
        //        SetupOption
        //        (
        //            "", 0,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "", 0,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "", 0,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "", 0,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        ),
        //        SetupOption
        //        (
        //            "", 0,
        //            "", 0,
        //            "", 0,
        //            "", 0
        //        )
        //    )
        //));

        //#endregion

        #endregion

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


            #region Norwegian

            //Message
            if (excelData[columns * (i + startRow - 1) + 10] != "")
                dialogueInfo.dialogueSegments[i].languageDialogueList[0] = excelData[columns * (i + startRow - 1) + 10].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageDialogueList[0] = "";


            //Option 1
            if (excelData[columns * (i + startRow - 1) + 11] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option1_Text = excelData[columns * (i + startRow - 1) + 11].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option1_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 12] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option1_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 12);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option1_Linked = -1;

            //Option 2
            if (excelData[columns * (i + startRow - 1) + 13] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option2_Text = excelData[columns * (i + startRow - 1) + 13].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option2_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 14] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option2_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 14);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option2_Linked = -1;

            //Option 3
            if (excelData[columns * (i + startRow - 1) + 15] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option3_Text = excelData[columns * (i + startRow - 1) + 15].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option3_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 16] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option3_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 16);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option3_Linked = -1;

            //Option 4
            if (excelData[columns * (i + startRow - 1) + 17] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option4_Text = excelData[columns * (i + startRow - 1) + 17].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option4_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 18] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option4_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 18);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[0].option4_Linked = -1;

            #endregion

            #region English

            //Message
            if (excelData[columns * (i + startRow - 1) + 20] != "")
                dialogueInfo.dialogueSegments[i].languageDialogueList[1] = excelData[columns * (i + startRow - 1) + 20].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageDialogueList[1] = "";


            //Option 1
            if (excelData[columns * (i + startRow - 1) + 21] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[1].option1_Text = excelData[columns * (i + startRow - 1) + 21].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[1].option1_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 22] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[1].option1_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 22);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[1].option1_Linked = -1;

            //Option 2
            if (excelData[columns * (i + startRow - 1) + 23] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[1].option2_Text = excelData[columns * (i + startRow - 1) + 23].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[1].option2_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 24] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[1].option2_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 24);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[1].option2_Linked = -1;

            //Option 3
            if (excelData[columns * (i + startRow - 1) + 25] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[1].option3_Text = excelData[columns * (i + startRow - 1) + 25].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[1].option3_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 26] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[1].option3_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 26);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[1].option3_Linked = -1;

            //Option 4
            if (excelData[columns * (i + startRow - 1) + 27] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[1].option4_Text = excelData[columns * (i + startRow - 1) + 27].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[1].option4_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 28] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[1].option4_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 28);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[1].option4_Linked = -1;

            #endregion

            #region German

            //Message
            if (excelData[columns * (i + startRow - 1) + 30] != "")
                dialogueInfo.dialogueSegments[i].languageDialogueList[2] = excelData[columns * (i + startRow - 1) + 30].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageDialogueList[2] = "";


            //Option 1
            if (excelData[columns * (i + startRow - 1) + 31] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[2].option1_Text = excelData[columns * (i + startRow - 1) + 31].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[2].option1_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 32] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[2].option1_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 32);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[2].option1_Linked = -1;

            //Option 2
            if (excelData[columns * (i + startRow - 1) + 33] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[2].option2_Text = excelData[columns * (i + startRow - 1) + 33].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[2].option2_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 34] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[2].option2_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 34);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[2].option2_Linked = -1;

            //Option 3
            if (excelData[columns * (i + startRow - 1) + 35] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[2].option3_Text = excelData[columns * (i + startRow - 1) + 35].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[2].option3_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 36] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[2].option3_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 36);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[2].option3_Linked = -1;

            //Option 4
            if (excelData[columns * (i + startRow - 1) + 37] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[2].option4_Text = excelData[columns * (i + startRow - 1) + 37].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[2].option4_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 38] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[2].option4_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 38);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[2].option4_Linked = -1;

            #endregion

            #region Language 4

            //Message
            if (excelData[columns * (i + startRow - 1) + 40] != "")
                dialogueInfo.dialogueSegments[i].languageDialogueList[3] = excelData[columns * (i + startRow - 1) + 40].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageDialogueList[3] = "";


            //Option 1
            if (excelData[columns * (i + startRow - 1) + 41] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[3].option1_Text = excelData[columns * (i + startRow - 1) + 41].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[3].option1_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 42] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[3].option1_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 42);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[3].option1_Linked = -1;

            //Option 2
            if (excelData[columns * (i + startRow - 1) + 43] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[3].option2_Text = excelData[columns * (i + startRow - 1) + 43].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[3].option2_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 44] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[3].option2_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 44);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[3].option2_Linked = -1;

            //Option 3
            if (excelData[columns * (i + startRow - 1) + 45] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[3].option3_Text = excelData[columns * (i + startRow - 1) + 45].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[3].option3_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 46] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[3].option3_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 46);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[3].option3_Linked = -1;

            //Option 4
            if (excelData[columns * (i + startRow - 1) + 47] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[3].option4_Text = excelData[columns * (i + startRow - 1) + 47].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[3].option4_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 48] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[3].option4_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 48);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[3].option4_Linked = -1;

            #endregion

            #region Language 5

            //Message
            if (excelData[columns * (i + startRow - 1) + 50] != "")
                dialogueInfo.dialogueSegments[i].languageDialogueList[4] = excelData[columns * (i + startRow - 1) + 50].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageDialogueList[4] = "";


            //Option 1
            if (excelData[columns * (i + startRow - 1) + 51] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[4].option1_Text = excelData[columns * (i + startRow - 1) + 51].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[4].option1_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 52] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[4].option1_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 52);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[4].option1_Linked = -1;

            //Option 2
            if (excelData[columns * (i + startRow - 1) + 53] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[4].option2_Text = excelData[columns * (i + startRow - 1) + 53].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[4].option2_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 54] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[4].option2_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 54);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[4].option2_Linked = -1;

            //Option 3
            if (excelData[columns * (i + startRow - 1) + 55] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[4].option3_Text = excelData[columns * (i + startRow - 1) + 55].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[4].option3_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 56] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[4].option3_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 56);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[4].option3_Linked = -1;

            //Option 4
            if (excelData[columns * (i + startRow - 1) + 57] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[4].option4_Text = excelData[columns * (i + startRow - 1) + 57].Trim();
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[4].option4_Text = "";
            //Option 1 - Link
            if (excelData[columns * (i + startRow - 1) + 58] != "")
                dialogueInfo.dialogueSegments[i].languageOptionList[4].option4_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 58);
            else
                dialogueInfo.dialogueSegments[i].languageOptionList[4].option4_Linked = -1;

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
