using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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


    //--------------------


    private void Start()
    {
        typingSound.clip = typeClip;
    }


    //--------------------


    public void SetupDialogueSegment_toDisplay(int segmentIndex, NPCs npc, string dialogueText, List<Options> options)
    {
        OptionBoxes.Instance.HideOptions();
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
}

[Serializable]
public class DialogueInfo
{
    public string npcName;
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