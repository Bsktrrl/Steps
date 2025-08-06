using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Interactable_NPC : MonoBehaviour
{
    [Header("Character")]
    public NPCs characterName;

    [Header("Stats from Excel")]
    [SerializeField] TextAsset dialogueSheet;
    public int levelNumber;

    int startRow = 2;
    int columns = 96;

    [Header("Dialogue Info")]
    public DialogueInfo dialogueInfo = new DialogueInfo();

    int segmentIndex = 0;

    [HideInInspector] public bool canInteract;
    [HideInInspector] public bool isInteracting;

    [Header("To be saved in database")]
    [HideInInspector] public bool hasTalked;
    public int lastSegment;

    [Header("Animations")]
    [SerializeField] Animator anim;
    bool blink;
    int animationCount;

    [Header("Camera")]
    public CinemachineVirtualCamera NPCVirtualCamera;

    [Header("DialogueSetup")]
    List<DialogueStat> this_TempDataInfo_StartingStat_List = new List<DialogueStat>();
    List<int> tempIndexList = new List<int>();


    //--------------------


    private void Start()
    {
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

        DataManager.Action_dataHasLoaded += SetupNPC;
    }
    private void OnDisable()
    {
        Player_KeyInputs.Action_dialogueButton_isPressed -= StartNewDialogueSegment;
        Player_KeyInputs.Action_dialogueNextButton_isPressed -= StartNewDialogueSegment;
        Player_KeyInputs.Action_InteractButton_isPressed -= CanInteract;

        OptionButton.Action_OptionButtonIsPressed -= StartNewDialogueSegment_OptionButton;

        DataManager.Action_dataHasLoaded -= SetupNPC;
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
            int tempIndex = SetCorrectSegmentFromTheStart(characterName);

            DialogueManager.Instance.currentSegement = tempIndex;
            segmentIndex = tempIndex;
        }
        
        SetupDialogueDisplay(segmentIndex, dialogueInfo.npcName);
    }

    int SetCorrectSegmentFromTheStart(NPCs npc)
    {
        //Make a list containing all StartStats aquired
        if (dialogueInfo != null)
        {
            for (int i = 0; i < dialogueInfo.dialogueSegments.Count; i++)
            {
                DialogueStat tempDialogueStat = new DialogueStat();

                if (dialogueInfo.dialogueSegments[i].startingStat != null && dialogueInfo.dialogueSegments[i].startingStat.value > 0)
                {
                    tempDialogueStat.character = dialogueInfo.dialogueSegments[i].startingStat.character;
                    tempDialogueStat.value = dialogueInfo.dialogueSegments[i].startingStat.value;

                    this_TempDataInfo_StartingStat_List.Add(tempDialogueStat);
                }
                else
                {
                    tempDialogueStat.character = NPCs.None;
                    tempDialogueStat.value = 0;

                    this_TempDataInfo_StartingStat_List.Add(tempDialogueStat);
                }
            }
        }
        
        //Make a list conataining the segmentIndexes of possible startingSegments 
        if (DataManager.Instance.charatersData_Store != null)
        {
            switch (npc)
            {
                case NPCs.None:
                    break;

                case NPCs.Floriel:
                    for (int i = 0; i < DataManager.Instance.charatersData_Store.floriel_Data.dialogueStartStatList.Count; i++)
                    {
                        for (int j = 0; j < this_TempDataInfo_StartingStat_List.Count; j++)
                        {
                            if (DataManager.Instance.charatersData_Store.floriel_Data.dialogueStartStatList[i] != null
                                && this_TempDataInfo_StartingStat_List[j].value > 0
                                && DataManager.Instance.charatersData_Store.floriel_Data.dialogueStartStatList[i].character == this_TempDataInfo_StartingStat_List[j].character
                                && DataManager.Instance.charatersData_Store.floriel_Data.dialogueStartStatList[i].value == this_TempDataInfo_StartingStat_List[j].value)
                            {
                                tempIndexList.Add(j);
                            }
                        }
                    }
                    break;
                case NPCs.Granith:
                    for (int i = 0; i < DataManager.Instance.charatersData_Store.granith_Data.dialogueStartStatList.Count; i++)
                    {
                        for (int j = 0; j < this_TempDataInfo_StartingStat_List.Count; j++)
                        {
                            if (DataManager.Instance.charatersData_Store.granith_Data.dialogueStartStatList[i] != null
                                && this_TempDataInfo_StartingStat_List[j].value > 0
                                && DataManager.Instance.charatersData_Store.granith_Data.dialogueStartStatList[i].character == this_TempDataInfo_StartingStat_List[j].character
                                && DataManager.Instance.charatersData_Store.granith_Data.dialogueStartStatList[i].value == this_TempDataInfo_StartingStat_List[j].value)
                            {
                                tempIndexList.Add(j);
                            }
                        }
                    }
                    break;
                case NPCs.Archie:
                    for (int i = 0; i < DataManager.Instance.charatersData_Store.archie_Data.dialogueStartStatList.Count; i++)
                    {
                        for (int j = 0; j < this_TempDataInfo_StartingStat_List.Count; j++)
                        {
                            if (DataManager.Instance.charatersData_Store.archie_Data.dialogueStartStatList[i] != null
                                && this_TempDataInfo_StartingStat_List[j].value > 0
                                && DataManager.Instance.charatersData_Store.archie_Data.dialogueStartStatList[i].character == this_TempDataInfo_StartingStat_List[j].character
                                && DataManager.Instance.charatersData_Store.archie_Data.dialogueStartStatList[i].value == this_TempDataInfo_StartingStat_List[j].value)
                            {
                                tempIndexList.Add(j);
                            }
                        }
                    }
                    break;
                case NPCs.Aisa:
                    for (int i = 0; i < DataManager.Instance.charatersData_Store.aisa_Data.dialogueStartStatList.Count; i++)
                    {
                        for (int j = 0; j < this_TempDataInfo_StartingStat_List.Count; j++)
                        {
                            if (DataManager.Instance.charatersData_Store.aisa_Data.dialogueStartStatList[i] != null
                                && this_TempDataInfo_StartingStat_List[j].value > 0
                                && DataManager.Instance.charatersData_Store.aisa_Data.dialogueStartStatList[i].character == this_TempDataInfo_StartingStat_List[j].character
                                && DataManager.Instance.charatersData_Store.aisa_Data.dialogueStartStatList[i].value == this_TempDataInfo_StartingStat_List[j].value)
                            {
                                tempIndexList.Add(j);
                            }
                        }
                    }
                    break;
                case NPCs.Mossy:
                    for (int i = 0; i < DataManager.Instance.charatersData_Store.mossy_Data.dialogueStartStatList.Count; i++)
                    {
                        for (int j = 0; j < this_TempDataInfo_StartingStat_List.Count; j++)
                        {
                            if (DataManager.Instance.charatersData_Store.mossy_Data.dialogueStartStatList[i] != null
                                && this_TempDataInfo_StartingStat_List[j].value > 0
                                && DataManager.Instance.charatersData_Store.mossy_Data.dialogueStartStatList[i].character == this_TempDataInfo_StartingStat_List[j].character
                                && DataManager.Instance.charatersData_Store.mossy_Data.dialogueStartStatList[i].value == this_TempDataInfo_StartingStat_List[j].value)
                            {
                                tempIndexList.Add(j);
                            }
                        }
                    }
                    break;
                case NPCs.Larry:
                    for (int i = 0; i < DataManager.Instance.charatersData_Store.larry_Data.dialogueStartStatList.Count; i++)
                    {
                        for (int j = 0; j < this_TempDataInfo_StartingStat_List.Count; j++)
                        {
                            if (DataManager.Instance.charatersData_Store.floriel_Data.dialogueStartStatList[i] != null
                                && this_TempDataInfo_StartingStat_List[j].value > 0
                                && DataManager.Instance.charatersData_Store.larry_Data.dialogueStartStatList[i].character == this_TempDataInfo_StartingStat_List[j].character
                                && DataManager.Instance.charatersData_Store.larry_Data.dialogueStartStatList[i].value == this_TempDataInfo_StartingStat_List[j].value)
                            {
                                tempIndexList.Add(j);
                            }
                        }
                    }
                    break;
                case NPCs.Stepellier:
                    for (int i = 0; i < DataManager.Instance.charatersData_Store.stepellier_Data.dialogueStartStatList.Count; i++)
                    {
                        for (int j = 0; j < this_TempDataInfo_StartingStat_List.Count; j++)
                        {
                            if (DataManager.Instance.charatersData_Store.stepellier_Data.dialogueStartStatList[i] != null
                                && this_TempDataInfo_StartingStat_List[j].value > 0
                                && DataManager.Instance.charatersData_Store.stepellier_Data.dialogueStartStatList[i].character == this_TempDataInfo_StartingStat_List[j].character
                                && DataManager.Instance.charatersData_Store.stepellier_Data.dialogueStartStatList[i].value == this_TempDataInfo_StartingStat_List[j].value)
                            {
                                tempIndexList.Add(j);
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        //Choose among the possible startingSegemntIndexes
        if (tempIndexList.Count > 0)
        {
            return tempIndexList[UnityEngine.Random.Range(0, tempIndexList.Count)];
        }
        else
        {
            return 0;
        }
    }


    //--------------------


    void SetupNPC()
    {
        switch (characterName)
        {
            case NPCs.None:
                break;

            case NPCs.Floriel:
                switch (levelNumber)
                {
                    case 1:
                        if (DataManager.Instance.charatersData_Store.floriel_Data.level_1_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 2:
                        if (DataManager.Instance.charatersData_Store.floriel_Data.level_2_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 3:
                        if (DataManager.Instance.charatersData_Store.floriel_Data.level_3_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 4:
                        if (DataManager.Instance.charatersData_Store.floriel_Data.level_4_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 5:
                        if (DataManager.Instance.charatersData_Store.floriel_Data.level_5_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 6:
                        if (DataManager.Instance.charatersData_Store.floriel_Data.level_6_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;

                    default:
                        break;
                }
                break;
            case NPCs.Granith:
                switch (levelNumber)
                {
                    case 1:
                        if (DataManager.Instance.charatersData_Store.granith_Data.level_1_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 2:
                        if (DataManager.Instance.charatersData_Store.granith_Data.level_2_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 3:
                        if (DataManager.Instance.charatersData_Store.granith_Data.level_3_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 4:
                        if (DataManager.Instance.charatersData_Store.granith_Data.level_4_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 5:
                        if (DataManager.Instance.charatersData_Store.granith_Data.level_5_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 6:
                        if (DataManager.Instance.charatersData_Store.granith_Data.level_6_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;

                    default:
                        break;
                }
                break;
            case NPCs.Archie:
                switch (levelNumber)
                {
                    case 1:
                        if (DataManager.Instance.charatersData_Store.archie_Data.level_1_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 2:
                        if (DataManager.Instance.charatersData_Store.archie_Data.level_2_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 3:
                        if (DataManager.Instance.charatersData_Store.archie_Data.level_3_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 4:
                        if (DataManager.Instance.charatersData_Store.archie_Data.level_4_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 5:
                        if (DataManager.Instance.charatersData_Store.archie_Data.level_5_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 6:
                        if (DataManager.Instance.charatersData_Store.archie_Data.level_6_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;

                    default:
                        break;
                }
                break;
            case NPCs.Aisa:
                switch (levelNumber)
                {
                    case 1:
                        if (DataManager.Instance.charatersData_Store.aisa_Data.level_1_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 2:
                        if (DataManager.Instance.charatersData_Store.aisa_Data.level_2_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 3:
                        if (DataManager.Instance.charatersData_Store.aisa_Data.level_3_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 4:
                        if (DataManager.Instance.charatersData_Store.aisa_Data.level_4_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 5:
                        if (DataManager.Instance.charatersData_Store.aisa_Data.level_5_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 6:
                        if (DataManager.Instance.charatersData_Store.aisa_Data.level_6_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;

                    default:
                        break;
                }
                break;
            case NPCs.Mossy:
                switch (levelNumber)
                {
                    case 1:
                        if (DataManager.Instance.charatersData_Store.mossy_Data.level_1_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 2:
                        if (DataManager.Instance.charatersData_Store.mossy_Data.level_2_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 3:
                        if (DataManager.Instance.charatersData_Store.mossy_Data.level_3_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 4:
                        if (DataManager.Instance.charatersData_Store.mossy_Data.level_4_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 5:
                        if (DataManager.Instance.charatersData_Store.mossy_Data.level_5_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 6:
                        if (DataManager.Instance.charatersData_Store.mossy_Data.level_6_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;

                    default:
                        break;
                }
                break;
            case NPCs.Larry:
                switch (levelNumber)
                {
                    case 1:
                        if (DataManager.Instance.charatersData_Store.larry_Data.level_1_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 2:
                        if (DataManager.Instance.charatersData_Store.larry_Data.level_2_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 3:
                        if (DataManager.Instance.charatersData_Store.larry_Data.level_3_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 4:
                        if (DataManager.Instance.charatersData_Store.larry_Data.level_4_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 5:
                        if (DataManager.Instance.charatersData_Store.larry_Data.level_5_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 6:
                        if (DataManager.Instance.charatersData_Store.larry_Data.level_6_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;

                    default:
                        break;
                }
                break;
            case NPCs.Stepellier:
                switch (levelNumber)
                {
                    case 1:
                        if (DataManager.Instance.charatersData_Store.stepellier_Data.level_1_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 2:
                        if (DataManager.Instance.charatersData_Store.stepellier_Data.level_2_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 3:
                        if (DataManager.Instance.charatersData_Store.stepellier_Data.level_3_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 4:
                        if (DataManager.Instance.charatersData_Store.stepellier_Data.level_4_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 5:
                        if (DataManager.Instance.charatersData_Store.stepellier_Data.level_5_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;
                    case 6:
                        if (DataManager.Instance.charatersData_Store.stepellier_Data.level_6_DialogueFinished)
                            HideNPC();
                        else
                            ShowNPC();
                        break;

                    default:
                        break;
                }
                break;

            default:
                break;
        }
    }
    void ShowNPC()
    {
        gameObject.SetActive(true);
    }
    void HideNPC()
    {
        gameObject.SetActive(false);
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
                if (excelData[columns * (i + startRow - 1) + 13 + (14 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageDialogueList[j] = excelData[columns * (i + startRow - 1) + 13 + (14 * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageDialogueList[j] = "";

                //Option 1
                if (excelData[columns * (i + startRow - 1) + 14 + (14 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_Text = excelData[columns * (i + startRow - 1) + 14 + (14 * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_Text = "";
                //Option 1 - Link
                if (excelData[columns * (i + startRow - 1) + 15 + (14 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 15 + (14 * j));
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_Linked = -1;
                //Option 1 - EndingValue
                if (excelData[columns * (i + startRow - 1) + 16 + (14 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_EndingValue = ParseIntSafe(excelData, columns * (i + startRow - 1) + 16 + (14 * j));
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option1_EndingValue = 0;

                //Option 2
                if (excelData[columns * (i + startRow - 1) + 17 + (14 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_Text = excelData[columns * (i + startRow - 1) + 17 + (14 * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_Text = "";
                //Option 2 - Link
                if (excelData[columns * (i + startRow - 1) + 18 + (14 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 18 + (14 * j));
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_Linked = -1;
                //Option 2 - EndingValue
                if (excelData[columns * (i + startRow - 1) + 19 + (14 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_EndingValue = ParseIntSafe(excelData, columns * (i + startRow - 1) + 19 + (14 * j));
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option2_EndingValue = 0;

                //Option 3
                if (excelData[columns * (i + startRow - 1) + 20 + (14 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_Text = excelData[columns * (i + startRow - 1) + 20 + (14 * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_Text = "";
                //Option 3 - Link
                if (excelData[columns * (i + startRow - 1) + 21 + (14 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 21 + (14 * j));
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_Linked = -1;
                //Option 3 - EndingValue
                if (excelData[columns * (i + startRow - 1) + 22 + (14 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_EndingValue = ParseIntSafe(excelData, columns * (i + startRow - 1) + 22 + (14 * j));
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option3_EndingValue = 0;

                //Option 4
                if (excelData[columns * (i + startRow - 1) + 23 + (14 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_Text = excelData[columns * (i + startRow - 1) + 23 + (14 * j)].Trim();
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_Text = "";
                //Option 4 - Link
                if (excelData[columns * (i + startRow - 1) + 24 + (14 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_Linked = ParseIntSafe(excelData, columns * (i + startRow - 1) + 24 + (14 * j));
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_Linked = -1;
                //Option 4 - EndingValue
                if (excelData[columns * (i + startRow - 1) + 25 + (14 * j)] != "")
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_EndingValue = ParseIntSafe(excelData, columns * (i + startRow - 1) + 25 + (14 * j));
                else
                    dialogueInfo.dialogueSegments[i].languageOptionList[j].option4_EndingValue = 0;
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
                statsSplizer.value = number;
            }
            else if (!string.IsNullOrEmpty(part))
            {
                if (Enum.TryParse(part, out NPCs result))
                {
                    statsSplizer.character = result;
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
    int ParseEndValue(string[] data, int index)
    {
        int tempValue = ParseIntSafe(data, index);

        if (tempValue == 1)
            return -1;
        else if (tempValue == 2)
            return 1;

        return 0;
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
            {
                segment = dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option1_Linked - 1;
                UpdateEndingValue(NPCManager.Instance.charatersData.stepellier_Data.endingValue, characterName, dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option1_EndingValue);
            }
            else if (DialogueManager.Instance.selectedButton == 2)
            {
                segment = dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option2_Linked - 1;
                UpdateEndingValue(NPCManager.Instance.charatersData.stepellier_Data.endingValue, characterName, dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option2_EndingValue);
            }
            else if (DialogueManager.Instance.selectedButton == 3)
            {
                segment = dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option3_Linked - 1;
                UpdateEndingValue(NPCManager.Instance.charatersData.stepellier_Data.endingValue, characterName, dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option3_EndingValue);
            }
            else if (DialogueManager.Instance.selectedButton == 4)
            {
                segment = dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option4_Linked - 1;
                UpdateEndingValue(NPCManager.Instance.charatersData.stepellier_Data.endingValue, characterName, dialogueInfo.dialogueSegments[segmentIndex].languageOptionList[0].option4_EndingValue);
            }

            segmentIndex = segment;
            DialogueManager.Instance.currentSegement = segmentIndex;

            //print("DialogueSegment: Button: " + (DialogueManager.Instance.selectedButton - 1) + " | Index: " + segmentIndex + " | Segment: " + segment);

            SetupDialogueDisplay(segment, dialogueInfo.npcName); 
        }
    }

    void UpdateEndingValue(int valueToChange, NPCs npc, int tempEndingValue)
    {
        switch (npc)
        {
            case NPCs.None:
                break;

            case NPCs.Floriel:
                valueToChange += tempEndingValue;
                break;
            case NPCs.Granith:
                valueToChange += tempEndingValue;
                break;
            case NPCs.Archie:
                valueToChange += tempEndingValue;
                break;
            case NPCs.Aisa:
                valueToChange += tempEndingValue;
                break;
            case NPCs.Mossy:
                valueToChange += tempEndingValue;
                break;
            case NPCs.Larry:
                valueToChange += tempEndingValue;
                break;

            case NPCs.Stepellier:
                valueToChange += tempEndingValue;
                break;

            default:
                break;
        }

        NPCManager.Instance.SaveData();
    }


    //--------------------


    void SetupDialogueDisplay(int index, NPCs npc)
    {
        NPCManager.Instance.UpdateStatsGathered(index, dialogueInfo, npc);

        //Text
        if (dialogueInfo.dialogueSegments.Count > index)
        {
            switch (SettingsManager.Instance.settingsData.currentLanguage)
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
