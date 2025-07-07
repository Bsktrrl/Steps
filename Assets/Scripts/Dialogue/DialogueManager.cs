using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("Attachments")]
    public GameObject dialogueCanvas;
    public TextMeshProUGUI nameText;

    [Header("Language")]
    public Languages currentLanguage;

    [Header("Sound")]
    public AudioSource typingSound;
    public AudioClip typeClip;

    [Header("Button")]
    public int selectedButton = -1;

    [Header("Arrow")]
    public Image arrowImage;

    [Header("ActiveNPC")]
    public NPCs activeNPC;
    public int segmentTotal;
    public int currentSegement;


    //--------------------


    private void Start()
    {
        typingSound.clip = typeClip;
    }

    private void OnEnable()
    {
        TypewriterEffect.Action_Typewriting_Finished += SetupArrow;
    }
    private void OnDisable()
    {
        TypewriterEffect.Action_Typewriting_Finished -= SetupArrow;
    }


    //--------------------


    public void SetupDialogueSegment_toDisplay(NPCs npc, string dialogueText, List<Options> options)
    {
        OptionBoxes.Instance.HideOptions();
        HideArrow();

        SetupNPCNameText_toDisplay(npc.ToString());
        SetupDialogueText_toDisplay(dialogueText);

        OptionBoxes.Instance.SetupOptions(npc, options[0].optionText, options[1].optionText, options[2].optionText, options[3].optionText);

        //Set dialogueBox active, if not
        if (!dialogueCanvas.activeInHierarchy)
        {
            StartDialogue();
        }
    }
    void SetupNPCNameText_toDisplay(string _name)
    {
        nameText.text = _name;
    }
    void SetupDialogueText_toDisplay(string _text)
    {
        TypewriterEffect.Instance.ShowText(_text);
    }

    void StartDialogue()
    {
        PlayerManager.Instance.pauseGame = true;
        dialogueCanvas.SetActive(true);
    }
    public void EndDialogue()
    {
        dialogueCanvas.SetActive(false);
        PlayerManager.Instance.pauseGame = false;
    }

    void SetupArrow()
    {
        if (OptionBoxes.Instance.optionButton_1.gameObject.activeInHierarchy || (currentSegement >= segmentTotal))
            HideArrow();
        else
            ShowArrow();
    }
    void ShowArrow()
    {
        switch (activeNPC)
        {
            case NPCs.None:
                break;

            case NPCs.Floriel:
                arrowImage.color = DialogueColors.Instance.floriel_DialogueBox_Pressed;
                break;
            case NPCs.Granith:
                arrowImage.color = DialogueColors.Instance.granith_DialogueBox_Pressed;
                break;
            case NPCs.Archie:
                arrowImage.color = DialogueColors.Instance.archie_DialogueBox_Pressed;
                break;
            case NPCs.Aisa:
                arrowImage.color = DialogueColors.Instance.aisa_DialogueBox_Pressed;
                break;
            case NPCs.Mossy:
                arrowImage.color = DialogueColors.Instance.mossy_DialogueBox_Pressed;
                break;
            case NPCs.Larry:
                arrowImage.color = DialogueColors.Instance.larry_DialogueBox_Pressed;
                break;
            case NPCs.Stepellier:
                arrowImage.color = DialogueColors.Instance.stepellier_DialogueBox_Pressed;
                break;

            default:
                break;
        }

        arrowImage.gameObject.SetActive(true);
    }
    void HideArrow()
    {
        arrowImage.gameObject.SetActive(false);
    }
}

[Serializable]
public class DialogueInfo
{
    public NPCs npcName;
    public List<DialogueSegment> dialogueSegments = new List<DialogueSegment>();

}
[Serializable]
public class DialogueSegment
{
    public List<string> languageDialogueList;
    public List<List<Options>> languageOptionList = new List<List<Options>>();

    public int animation_Player;
    public int animation_NPC;

    public DialogueStats dialogueStats = new DialogueStats();
}
[Serializable]
public class Options
{
    public string optionText;
    public int linkedDialogueSegment;
}
[Serializable]
public class DialogueStats
{
    //Add stats that will result in other variables to be saved, based on the context of the dialogueSegmentText
}

public enum Languages
{
    Norwegian,
    English,
    German,
    Japanese,
    Chinese
}