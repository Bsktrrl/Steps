using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable_NPC : MonoBehaviour
{
    [SerializeField] Text dialogueDocument;

    [SerializeField] DialogueInfo dialogueInfo = new DialogueInfo();

    [SerializeField] int segmentIndex = 0;


    //--------------------


    private void Start()
    {
        dialogueInfo.npcName = NPCs.Floriel.ToString();

        SetupDialogue(SetupDialogueSegment
        (
            SetupLanguageTextList
            (
                    "Hei du der, jeg heter Floríel. Jeg er en blokk på søken etter en oransj blomst. Jeg ønsker å finne den, så jeg kan fullføre blomsterkransen min. Vet du hvor jeg kan finne en?",
                    "Hello there, I am Floríel, a block that seeks the one and only orange flower. I seek it so that I can finish my flower wreath. Do you know where it is?",
                    "German",
                    "Japanese",
                    "Chinese"
            ),

            SetupOptionLanguages
            (
                setupOptionList
                (
                    SetupOption("Nei, beklager", 2),
                    SetupOption("Nei, men jeg skal si ifra hvis jeg ser en", 2),
                    SetupOption("Jeg har viktigere ting å tenke på", 3),
                    SetupOption("Jeg ville ikke gitt deg den om jeg fant den engang!", 3)
                ),
                setupOptionList
                (
                    SetupOption("No, sorry", 2),
                    SetupOption("No, but I will tell you if I see one", 2),
                    SetupOption("I got more important stuff to do", 3),
                    SetupOption("I wouldn't have given you the flower if I found it!", 3)
                ),
                setupOptionList
                (
                    SetupOption("German option 1", 2),
                    SetupOption("German option 2", 2),
                    SetupOption("German option 3", 3),
                    SetupOption("German option 4", 3)
                ),
                setupOptionList
                (
                    SetupOption("Japanese option 1", 2),
                    SetupOption("Japanese option 2", 2),
                    SetupOption("Japanese option 3", 3),
                    SetupOption("Japanese option 4", 3)
                ),
                setupOptionList
                (
                    SetupOption("Chinese option 1", 2),
                    SetupOption("Chinese option 2", 2),
                    SetupOption("Chinese option 3", 3),
                    SetupOption("Chinese option 4", 3)
                )
            )
        ));

        SetupDialogueDisplay(segmentIndex, NPCs.Floriel);
    }


    //--------------------


    void SetupDialogueDisplay(int index, NPCs npc)
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
