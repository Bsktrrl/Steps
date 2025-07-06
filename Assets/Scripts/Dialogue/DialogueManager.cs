using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    public GameObject dialogueCanvas;
    public TextMeshProUGUI nameText;

    public AudioSource typingSound;
    public AudioClip typeClip;

    private void Start()
    {
        string dialogueText = "Hello there, I am Floríel, a block that seeks the one and only orange flower. I seek it so that I can finish my flower wreath. Do you know where it is?";
        
        List<string> dialogueOptions = new List<string>();
        dialogueOptions.Add("\"Oh, I can totaly do that, no problem!!\"");
        dialogueOptions.Add("\"I would rather Settle than go with you on this one!!\"");
        dialogueOptions.Add("\"I guess you have a point!!!\"");
        dialogueOptions.Add("\"This whole conversation is ridiculous!\"");

        SetupDialogueSegment(NPCs.Floriel, dialogueText, dialogueOptions);

        typingSound.clip = typeClip;
    }


    public void SetupDialogueSegment(NPCs npc, string dialogueText, List<string> options)
    {
        OptionBoxes.Instance.HideOptions();
        SetupNPCNameText(npc.ToString());
        SetupDialogueText(dialogueText);

        OptionBoxes.Instance.SetupOptions(npc, options[0], options[1], options[2], options[3]);

        //Set dialogueBox active, if not
        if (!dialogueCanvas.activeInHierarchy)
        {
            PlayerManager.Instance.pauseGame = true;
            dialogueCanvas.SetActive(true);
        }
    }
    void SetupNPCNameText(string _name)
    {
        nameText.text = _name;
    }
    void SetupDialogueText(string _text)
    {
        TypewriterEffect.Instance.ShowText(_text);
    }
}
