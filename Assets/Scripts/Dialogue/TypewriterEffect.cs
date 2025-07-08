using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : Singleton<TypewriterEffect>
{
    public static event Action Action_Typewriting_Finished;

    [SerializeField] TextMeshProUGUI dialogueText;
    float letterDelay = 0.035f;
    float sentenceDelay = 0.32f;
    float commaDelay = 0.18f;

    string fullText;
    private Coroutine typingCoroutine;
    bool skipRequested;

    public bool isTyping;


    //--------------------


    private void OnEnable()
    {
        Player_KeyInputs.Action_dialogueNextButton_isPressed += SkipTypewriter;
        Player_KeyInputs.Action_dialogueButton_isPressed += SkipTypewriter;
    }
    private void OnDisable()
    {
        Player_KeyInputs.Action_dialogueNextButton_isPressed -= SkipTypewriter;
        Player_KeyInputs.Action_dialogueButton_isPressed -= SkipTypewriter;
    }


    //--------------------


    public void ShowText(string text)
    {
        fullText = text;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        skipRequested = false;
        typingCoroutine = StartCoroutine(TypeText());
    }
    void SkipTypewriter()
    {
        if (isTyping)
        {
            if (typingCoroutine != null)
            {
                skipRequested = true;
            }
        }
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        dialogueText.text = "";

        int i = 0;
        while (i <= fullText.Length)
        {
            if (skipRequested)
            {
                dialogueText.text = fullText;
                break;
            }

            if (i < fullText.Length)
            {
                char currentChar = fullText[i];
                dialogueText.text = fullText.Substring(0, i + 1);

                PlayTypingSound(currentChar);

                // Delay rules
                if (IsLongerPauseChar(currentChar))
                {
                    yield return new WaitForSeconds(sentenceDelay); // Delay after every .
                }
                else if (IsSmallerPauseChar(currentChar))
                {
                    yield return new WaitForSeconds(commaDelay);
                }
                else
                {
                    yield return new WaitForSeconds(letterDelay);
                }
            }

            i++;
        }

        OptionBoxes.Instance.ShowHideOptions();

        skipRequested = false;
        typingCoroutine = null;
        isTyping = false;

        Action_Typewriting_Finished?.Invoke();
    }
    void PlayTypingSound(char c)
    {
        if (!char.IsWhiteSpace(c) && DialogueManager.Instance.typingSound != null)
        {
            float pitchRange = UnityEngine.Random.Range(0.9f, 1.1f);
            DialogueManager.Instance.typingSound.pitch = pitchRange;
            DialogueManager.Instance.typingSound.Play();
        }
    }

    bool IsLongerPauseChar(char c)
    {
        return c == '.' || c == '!' || c == '?';
    }
    bool IsSmallerPauseChar(char c)
    {
        return c == ',' || c == ':' || c == ';' || c == '-' || c == '#' || c == '=' || c == '+' || c == '@';
    }
}
