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
    public int languageAmount;

    [Header("Sound")]
    public AudioSource typingSound;
    public AudioClip typeClip;

    [Header("Button")]
    public int selectedButton = -1;

    [Header("Arrow")]
    public Image arrowImage;
    public Image nameBoxImage;

    [Header("ActiveNPC")]
    public Interactable_NPC npcObject;
    public NPCs activeNPC;
    public int segmentTotal;
    public int currentSegement;


    //--------------------


    private void Awake()
    {
        typingSound.clip = typeClip;

        languageAmount = Enum.GetValues(typeof(Languages)).Length;
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


    public void SetupDialogueSegment_toDisplay(NPCs npc, string dialogueText, LanguageOptions languageSection)
    {
        OptionBoxes.Instance.HideOptions();
        HideArrow();

        SetupNPCNameText_toDisplay(npc.ToString());
        SetupDialogueText_toDisplay(dialogueText);

        OptionBoxes.Instance.SetupOptions(npc, languageSection.option1_Text, languageSection.option2_Text, languageSection.option3_Text, languageSection.option4_Text);

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

    public void StartDialogue()
    {
        npcObject.isInteracting = true;

        PlayerManager.Instance.npcInteraction = true;

        SelectColor(nameBoxImage, ColorVariants.Normal);

        dialogueCanvas.SetActive(true);
    }
    public IEnumerator EndDialogue()
    {
        dialogueCanvas.SetActive(false);
        npcObject.isInteracting = false;
        npcObject.hasTalked = true;

        yield return StartCoroutine(CameraController.Instance.StartVirtualCameraBlend_Out());

        ButtonMessages.Instance.ShowButtonMessage(ControlButtons.Down, MessageManager.Instance.Show_Message(MessageManager.Instance.interact_Talk_Message));

        NPCManager.Instance.SetDialogueFinished(npcObject.characterName, npcObject.levelNumber);

        //Reset Stats
        npcObject = null;
        activeNPC = NPCs.None;
        segmentTotal = 0;
        currentSegement = 0;

        PlayerManager.Instance.npcInteraction = false;
    }

    void SetupArrow()
    {
        if (OptionBoxes.Instance.optionButton_1.gameObject.activeInHierarchy || (currentSegement >= segmentTotal) || npcObject.dialogueInfo.dialogueSegments[currentSegement].lastSegment)
            HideArrow();
        else
            ShowArrow();
    }
    void ShowArrow()
    {
        SelectColor(arrowImage, ColorVariants.Pressed);

        arrowImage.gameObject.SetActive(true);
    }
    void HideArrow()
    {
        arrowImage.gameObject.SetActive(false);
    }

    void SelectColor(Image _image, ColorVariants colorVariants)
    {
        switch (activeNPC)
        {
            case NPCs.None:
                break;

            case NPCs.Floriel:
                switch (colorVariants)
                {
                    case ColorVariants.Normal:
                        _image.color = DialogueColors.Instance.floriel_DialogueBox_Normal;
                        break;
                    case ColorVariants.Highlighted:
                        _image.color = DialogueColors.Instance.floriel_DialogueBox_Highlighted;
                        break;
                    case ColorVariants.Selected:
                        _image.color = DialogueColors.Instance.floriel_DialogueBox_Selected;
                        break;
                    case ColorVariants.Pressed:
                        _image.color = DialogueColors.Instance.floriel_DialogueBox_Pressed;
                        break;

                    default:
                        _image.color = DialogueColors.Instance.floriel_DialogueBox_Normal;
                        break;
                }
                break;
            case NPCs.Granith:
                switch (colorVariants)
                {
                    case ColorVariants.Normal:
                        _image.color = DialogueColors.Instance.granith_DialogueBox_Normal;
                        break;
                    case ColorVariants.Highlighted:
                        _image.color = DialogueColors.Instance.granith_DialogueBox_Highlighted;
                        break;
                    case ColorVariants.Selected:
                        _image.color = DialogueColors.Instance.granith_DialogueBox_Selected;
                        break;
                    case ColorVariants.Pressed:
                        _image.color = DialogueColors.Instance.granith_DialogueBox_Pressed;
                        break;

                    default:
                        _image.color = DialogueColors.Instance.granith_DialogueBox_Normal;
                        break;
                }
                break;
            case NPCs.Archie:
                switch (colorVariants)
                {
                    case ColorVariants.Normal:
                        _image.color = DialogueColors.Instance.archie_DialogueBox_Normal;
                        break;
                    case ColorVariants.Highlighted:
                        _image.color = DialogueColors.Instance.archie_DialogueBox_Highlighted;
                        break;
                    case ColorVariants.Selected:
                        _image.color = DialogueColors.Instance.archie_DialogueBox_Selected;
                        break;
                    case ColorVariants.Pressed:
                        _image.color = DialogueColors.Instance.archie_DialogueBox_Pressed;
                        break;

                    default:
                        _image.color = DialogueColors.Instance.archie_DialogueBox_Normal;
                        break;
                }
                break;
            case NPCs.Aisa:
                switch (colorVariants)
                {
                    case ColorVariants.Normal:
                        _image.color = DialogueColors.Instance.aisa_DialogueBox_Normal;
                        break;
                    case ColorVariants.Highlighted:
                        _image.color = DialogueColors.Instance.aisa_DialogueBox_Highlighted;
                        break;
                    case ColorVariants.Selected:
                        _image.color = DialogueColors.Instance.aisa_DialogueBox_Selected;
                        break;
                    case ColorVariants.Pressed:
                        _image.color = DialogueColors.Instance.aisa_DialogueBox_Pressed;
                        break;

                    default:
                        _image.color = DialogueColors.Instance.aisa_DialogueBox_Normal;
                        break;
                }
                break;
            case NPCs.Mossy:
                switch (colorVariants)
                {
                    case ColorVariants.Normal:
                        _image.color = DialogueColors.Instance.mossy_DialogueBox_Normal;
                        break;
                    case ColorVariants.Highlighted:
                        _image.color = DialogueColors.Instance.mossy_DialogueBox_Highlighted;
                        break;
                    case ColorVariants.Selected:
                        _image.color = DialogueColors.Instance.mossy_DialogueBox_Selected;
                        break;
                    case ColorVariants.Pressed:
                        _image.color = DialogueColors.Instance.mossy_DialogueBox_Pressed;
                        break;

                    default:
                        _image.color = DialogueColors.Instance.mossy_DialogueBox_Normal;
                        break;
                }
                break;
            case NPCs.Larry:
                switch (colorVariants)
                {
                    case ColorVariants.Normal:
                        _image.color = DialogueColors.Instance.larry_DialogueBox_Normal;
                        break;
                    case ColorVariants.Highlighted:
                        _image.color = DialogueColors.Instance.larry_DialogueBox_Highlighted;
                        break;
                    case ColorVariants.Selected:
                        _image.color = DialogueColors.Instance.larry_DialogueBox_Selected;
                        break;
                    case ColorVariants.Pressed:
                        _image.color = DialogueColors.Instance.larry_DialogueBox_Pressed;
                        break;

                    default:
                        _image.color = DialogueColors.Instance.larry_DialogueBox_Normal;
                        break;
                }
                break;
            case NPCs.Stepellier:
                switch (colorVariants)
                {
                    case ColorVariants.Normal:
                        _image.color = DialogueColors.Instance.stepellier_DialogueBox_Normal;
                        break;
                    case ColorVariants.Highlighted:
                        _image.color = DialogueColors.Instance.stepellier_DialogueBox_Highlighted;
                        break;
                    case ColorVariants.Selected:
                        _image.color = DialogueColors.Instance.stepellier_DialogueBox_Selected;
                        break;
                    case ColorVariants.Pressed:
                        _image.color = DialogueColors.Instance.stepellier_DialogueBox_Pressed;
                        break;

                    default:
                        _image.color = DialogueColors.Instance.stepellier_DialogueBox_Normal;
                        break;
                }
                break;

            default:
                break;
        }
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
    public string segmentDescription;

    [Header("Segment Info")]
    public bool firstSegment;
    public bool lastSegment;

    [Header("Stats")]
    public List<DialogueStat> statRequired;
    public List<DialogueStat> statToGet;

    [Header("Animations")]
    public List<int> animation_Player = new List<int>();
    public List<int> animation_NPC = new List<int>();
    public int cutscene;

    [Header("Dialogue")]
    public List<string> languageDialogueList = new List<string>();

    [Header("Options")]
    public List<LanguageOptions> languageOptionList = new List<LanguageOptions>();
}

[Serializable]
public class LanguageOptions
{
    [Header("Option 1")]
    public string option1_Text;
    public int option1_Linked;
    public int option1_AlternativeLinked;
    public int option1_EndingValue;

    [Header("Option 2")]
    public string option2_Text;
    public int option2_Linked;
    public int option2_AlternativeLinked;
    public int option2_EndingValue;

    [Header("Option 3")]
    public string option3_Text;
    public int option3_Linked;
    public int option3_AlternativeLinked;
    public int option3_EndingValue;

    [Header("Option 4")]
    public string option4_Text;
    public int option4_Linked;
    public int option4_AlternativeLinked;
    public int option4_EndingValue;
}
[Serializable]
public class DialogueStat
{
    public NPCs character;
    public int value;
}

public enum NPCs
{
    None,

    Floriel,
    Granith,
    Archie,
    Aisa,
    Mossy,
    Larry,

    Stepellier
}

public enum ColorVariants
{
    Normal,
    Highlighted,
    Selected,
    Pressed
}