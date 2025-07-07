using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable_NPC : MonoBehaviour
{
    [SerializeField] Text dialogueDocument;

    [SerializeField] DialogueInfo dialogueInfo = new DialogueInfo();

    public int segmentIndex = 0;


    //--------------------


    private void Start()
    {
        dialogueInfo.npcName = NPCs.Floriel.ToString();

        //Fill 10 segments with ChatGPT
        #region Hardcoded dialogueSegments

        #region Segment 1

        SetupDialogue(SetupDialogueSegment
        (
            SetupLanguageTextList
            (
                "Velkommen, vandrer. Det er sjelden noen går denne stien. Skogen husker fottrinn som dine.",
                "Welcome, traveler. It's rare to see someone take this path. The forest remembers footsteps like yours.",
                "German",
                "Japanese",
                "Chinese"
            ),

            SetupOptionLanguages
            (
                setupOptionList
                (
                    SetupOption("Hvem er du?", 2),
                    SetupOption("Hva er dette stedet?", 3),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("Who are you?", 2),
                    SetupOption("What is this place?", 3),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("German option 1", 2),
                    SetupOption("German option 2", 3),
                    SetupOption("German option 3", 0),
                    SetupOption("German option 4", 0)
                ),
                setupOptionList
                (
                    SetupOption("Japanese option 1", 2),
                    SetupOption("Japanese option 2", 3),
                    SetupOption("Japanese option 3", 0),
                    SetupOption("Japanese option 4", 0)
                ),
                setupOptionList
                (
                    SetupOption("Chinese option 1", 2),
                    SetupOption("Chinese option 2", 3),
                    SetupOption("Chinese option 3", 0),
                    SetupOption("Chinese option 4", 0)
                )
            )
        ));

        #endregion

        #region Segment 2

        SetupDialogue(SetupDialogueSegment
        (
            SetupLanguageTextList
            (
                "Jeg er Floríel, en gang vokter av denne skogen. Min tid er forbi, men stemmen min henger igjen.",
                "I am Floríel, once guardian of these woods. My time has long passed, yet my voice lingers.",
                "German",
                "Japanese",
                "Chinese"
            ),

            SetupOptionLanguages
            (
                setupOptionList
                (
                    SetupOption("Hvorfor er du fortsatt her?", 4),
                    SetupOption("Kan du hjelpe meg?", 5),
                    SetupOption("Ha det.", 10),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("Why are you still here?", 4),
                    SetupOption("Can you help me?", 5),
                    SetupOption("Goodbye", 10),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("German option 1", 4),
                    SetupOption("German option 2", 5),
                    SetupOption("German option 3", 10),
                    SetupOption("German option 4", 0)
                ),
                setupOptionList
                (
                    SetupOption("Japanese option 1", 4),
                    SetupOption("Japanese option 2", 5),
                    SetupOption("Japanese option 3", 10),
                    SetupOption("Japanese option 4", 0)
                ),
                setupOptionList
                (
                    SetupOption("Chinese option 1", 4),
                    SetupOption("Chinese option 2", 5),
                    SetupOption("Chinese option 3", 10),
                    SetupOption("Chinese option 4", 0)
                )
            )
        ));

        #endregion

        #region Segment 3

        SetupDialogue(SetupDialogueSegment
        (
            SetupLanguageTextList
            (
                "Dette stedet var en gang et fristed, før ilden kom. Nå er det bare ekko og aske igjen.",
                "This place was once a sanctuary, before the fire. Now, only echoes and ash remain.",
                "German",
                "Japanese",
                "Chinese"
            ),

            SetupOptionLanguages
            (
                setupOptionList
                (
                    SetupOption("Hvilken ild?", 6),
                    SetupOption("Kan det gjenopprettes?", 7),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("What fire?", 6),
                    SetupOption("Can it be restored?", 7),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("German option 1", 6),
                    SetupOption("German option 2", 7),
                    SetupOption("German option 3", 0),
                    SetupOption("German option 4", 0)
                ),
                setupOptionList
                (
                    SetupOption("Japanese option 1", 6),
                    SetupOption("Japanese option 2", 7),
                    SetupOption("Japanese option 3", 0),
                    SetupOption("Japanese option 4", 0)
                ),
                setupOptionList
                (
                    SetupOption("Chinese option 1", 6),
                    SetupOption("Chinese option 2", 7),
                    SetupOption("Chinese option 3", 0),
                    SetupOption("Chinese option 4", 0)
                )
            )
        ));

        #endregion

        #region Segment 4

        SetupDialogue(SetupDialogueSegment
        (
            SetupLanguageTextList
            (
                "Eden min binder meg. Før skogen tilgir, kan jeg ikke hvile.",
                "My oath binds me. Until the forest forgives, I cannot rest.",
                "German",
                "Japanese",
                "Chinese"
            ),

            SetupOptionLanguages
            (
                setupOptionList
                (
                    SetupOption("Hvordan kan jeg hjelpe?", 5),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("How can I help?", 5),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("German option 1", 5),
                    SetupOption("German option 2", 0),
                    SetupOption("German option 3", 0),
                    SetupOption("German option 4", 0)
                ),
                setupOptionList
                (
                    SetupOption("Japanese option 1", 5),
                    SetupOption("Japanese option 2", 0),
                    SetupOption("Japanese option 3", 0),
                    SetupOption("Japanese option 4", 0)
                ),
                setupOptionList
                (
                    SetupOption("Chinese option 1", 5),
                    SetupOption("Chinese option 2", 0),
                    SetupOption("Chinese option 3", 0),
                    SetupOption("Chinese option 4", 0)
                )
            )
        ));

        #endregion

        #region Segment 5

        SetupDialogue(SetupDialogueSegment
        (
            SetupLanguageTextList
            (
                "Dypere inne ligger hjertetroten, den slår fortsatt. Rens den, og kanskje skogen kan puste igjen.",
                "Deep within lies the heartroot, still beating. Cleanse it, and the forest may breathe again.",
                "German",
                "Japanese",
                "Chinese"
            ),

            SetupOptionLanguages
            (
                setupOptionList
                (
                    SetupOption("Hvor finner jeg den?", 8),
                    SetupOption("Er det farlig?", 9),
                    SetupOption("Jeg skal prøve.", 10),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("Where do I find it?", 8),
                    SetupOption("Is it dangerous?", 9),
                    SetupOption("I’ll try.", 10),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("German option 1", 8),
                    SetupOption("German option 2", 9),
                    SetupOption("German option 3", 10),
                    SetupOption("German option 4", 0)
                ),
                setupOptionList
                (
                    SetupOption("Japanese option 1", 8),
                    SetupOption("Japanese option 2", 9),
                    SetupOption("Japanese option 3", 10),
                    SetupOption("Japanese option 4", 0)
                ),
                setupOptionList
                (
                    SetupOption("Chinese option 1", 8),
                    SetupOption("Chinese option 2", 9),
                    SetupOption("Chinese option 3", 10),
                    SetupOption("Chinese option 4", 0)
                )
            )
        ));

        #endregion

        #region Segment 6

        SetupDialogue(SetupDialogueSegment
        (
            SetupLanguageTextList
            (
                "Ilden kom plutselig. Ingen advarsel, ingen nåde. Noen sier den ble påkalt...",
                "The fire came suddenly. No warning, no mercy. Some say it was summoned..",
                "German",
                "Japanese",
                "Chinese"
            ),

            SetupOptionLanguages
            (
                setupOptionList
                (
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                )
            )
        ));

        #endregion

        #region Segment 7

        SetupDialogue(SetupDialogueSegment
        (
            SetupLanguageTextList
            (
                "Kanskje. Men skogens sår er dype. Det vil kreve mer enn håp.",
                "Perhaps. But the forest's wounds run deep. It will take more than hope.",
                "German",
                "Japanese",
                "Chinese"
            ),

            SetupOptionLanguages
            (
                setupOptionList
                (
                    SetupOption("I'll do what I can.", 10),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("Jeg skal gjøre det jeg kan.", 10),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("German option 1", 5),
                    SetupOption("German option 2", 0),
                    SetupOption("German option 3", 0),
                    SetupOption("German option 4", 0)
                ),
                setupOptionList
                (
                    SetupOption("Japanese option 1", 5),
                    SetupOption("Japanese option 2", 0),
                    SetupOption("Japanese option 3", 0),
                    SetupOption("Japanese option 4", 0)
                ),
                setupOptionList
                (
                    SetupOption("Chinese option 1", 5),
                    SetupOption("Chinese option 2", 0),
                    SetupOption("Chinese option 3", 0),
                    SetupOption("Chinese option 4", 0)
                )
            )
        ));

        #endregion

        #region Segment 8

        SetupDialogue(SetupDialogueSegment
        (
            SetupLanguageTextList
            (
                "Under den gamle steinsirkelen, bortenfor tornekrattet. Få vender tilbake uforandret.",
                "Beneath the old stone circle, beyond the briars. Few who go there return unchanged.",
                "German",
                "Japanese",
                "Chinese"
            ),

            SetupOptionLanguages
            (
                setupOptionList
                (
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                )
            )
        ));

        #endregion

        #region Segment 9

        SetupDialogue(SetupDialogueSegment
        (
            SetupLanguageTextList
            (
                "Ja. Hjerteroten voktes av det som er igjen av skogens raseri.",
                "Yes. The heartroot is guarded by what remains of the forest's rage.",
                "German",
                "Japanese",
                "Chinese"
            ),

            SetupOptionLanguages
            (
                setupOptionList
                (
                    SetupOption("Jeg vil møte det.", 10),
                    SetupOption("Kanskje jeg ikke er klar.", 10),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("I’ll face it.", 10),
                    SetupOption("Maybe I’m not ready.", 10),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("German option 1", 10),
                    SetupOption("German option 2", 10),
                    SetupOption("German option 3", 0),
                    SetupOption("German option 4", 0)
                ),
                setupOptionList
                (
                    SetupOption("Japanese option 1", 10),
                    SetupOption("Japanese option 2", 10),
                    SetupOption("Japanese option 3", 0),
                    SetupOption("Japanese option 4", 0)
                ),
                setupOptionList
                (
                    SetupOption("Chinese option 1", 10),
                    SetupOption("Chinese option 2", 10),
                    SetupOption("Chinese option 3", 0),
                    SetupOption("Chinese option 4", 0)
                )
            )
        ));

        #endregion

        #region Segment 10

        SetupDialogue(SetupDialogueSegment
        (
            SetupLanguageTextList
            (
                "Da gå, og må røttene huske ditt mot. Jeg vil våke.",
                "Then go, and may the roots remember your courage. I will be watching.",
                "German",
                "Japanese",
                "Chinese"
            ),

            SetupOptionLanguages
            (
                setupOptionList
                (
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                ),
                setupOptionList
                (
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0),
                    SetupOption("", 0)
                )
            )
        ));

        #endregion

        #endregion

        SetupDialogueDisplay(segmentIndex, NPCs.Floriel);
    }

    private void OnEnable()
    {
        Player_KeyInputs.Action_dialogueButton_isPressed += StartNewDialogueSegment;
    }
    private void OnDisable()
    {
        Player_KeyInputs.Action_dialogueButton_isPressed -= StartNewDialogueSegment;
    }


    //--------------------


    void StartNewDialogueSegment()
    {
        if (!TypewriterEffect.Instance.isTyping)
        {
            Player_KeyInputs.Instance.dialogueButton_isPressed = false;
            SetupDialogueDisplay(segmentIndex, NPCs.Floriel);
        }
    }

    void SetupDialogueDisplay(int index, NPCs npc)
    {
        if (dialogueInfo.dialogueSegments.Count > index)
        {
            switch (DialogueManager.Instance.currentLanguage)
            {
                case Languages.Norwegian:
                    DialogueManager.Instance.SetupDialogueSegment_toDisplay(index, npc, dialogueInfo.dialogueSegments[index].languageDialogueList[0], dialogueInfo.dialogueSegments[index].languageOptionList[0]);
                    break;
                case Languages.English:
                    DialogueManager.Instance.SetupDialogueSegment_toDisplay(index, npc, dialogueInfo.dialogueSegments[index].languageDialogueList[1], dialogueInfo.dialogueSegments[index].languageOptionList[1]);
                    break;
                case Languages.German:
                    DialogueManager.Instance.SetupDialogueSegment_toDisplay(index, npc, dialogueInfo.dialogueSegments[index].languageDialogueList[2], dialogueInfo.dialogueSegments[index].languageOptionList[2]);
                    break;
                case Languages.Japanese:
                    DialogueManager.Instance.SetupDialogueSegment_toDisplay(index, npc, dialogueInfo.dialogueSegments[index].languageDialogueList[3], dialogueInfo.dialogueSegments[index].languageOptionList[3]);
                    break;
                case Languages.Chinese:
                    DialogueManager.Instance.SetupDialogueSegment_toDisplay(index, npc, dialogueInfo.dialogueSegments[index].languageDialogueList[4], dialogueInfo.dialogueSegments[index].languageOptionList[4]);
                    break;

                default:
                    break;
            }

            segmentIndex++;
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
    DialogueSegment SetupDialogueSegment(List<string> languageDialogueList, List<List<Options>> languageOptionList)
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
    List<List<Options>> SetupOptionLanguages(List<Options> language1, List<Options> language2, List<Options> language3, List<Options> language4, List<Options> language5)
    {
        List<List<Options>> languageOptions = new List<List<Options>>();
        //List<List<Options>> optionsList = new List<List<Options>>();

        languageOptions.Add(language1);
        languageOptions.Add(language2);
        languageOptions.Add(language3);
        languageOptions.Add(language4);
        languageOptions.Add(language5);

        //languageOptions.language_options = optionsList;

        return languageOptions;
    }
    List<Options> setupOptionList(Options option1, Options option2, Options option3, Options option4)
    {
        List<Options> optionList = new List<Options>();

        optionList.Add(option1);
        optionList.Add(option2);
        optionList.Add(option3);
        optionList.Add(option4);

        return optionList;
    }
    Options SetupOption(string potionText, int segmentLink)
    {
        Options option = new Options();
        option.optionText = potionText;
        option.linkedDialogueSegment = segmentLink;

        return option;
    }
}
