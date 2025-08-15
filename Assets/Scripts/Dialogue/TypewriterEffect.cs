using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : Singleton<TypewriterEffect>
{
    public static event Action Action_Typewriting_Finished;

    [SerializeField] TextMeshProUGUI dialogueText;
    float slowText = 0.055f;
    float mediumText = 0.035f;
    float fastText = 0.02f;

    float slowSentence = 0.5f;
    float mediumSentence = 0.32f;
    float fastSentence = 0.2f;

    float slowComma = 0.35f;
    float mediumComma = 0.18f;
    float fastComma = 0.08f;

    [SerializeField] string fullText;
    private Coroutine typingCoroutine;
    bool skipRequested;

    public bool isTyping;
    [SerializeField] int charCounter = 0;
    [SerializeField] int maxCharCounter = 0;
    [SerializeField] int charSafetyCounter = 0;


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
        maxCharCounter = fullText.Length;

        charCounter = 0;
        charSafetyCounter = 0;
        while (charCounter <= fullText.Length)
        {
            if (skipRequested && charSafetyCounter <= 1)
            {
                skipRequested = false;
            }
            else if (skipRequested && charSafetyCounter > 1)
            {
                print("111. skipRequested");
                dialogueText.text = fullText;
                break;
            }

            if (charCounter < fullText.Length)
            {
                char currentChar = fullText[charCounter];
                dialogueText.text = fullText.Substring(0, charCounter + 1);

                PlayTypingSound(currentChar);

                // Delay rules
                if (IsLongerPauseChar(currentChar))
                {
                    yield return new WaitForSeconds(GetSentenceSpeed()); // Delay after every .
                }
                else if (IsSmallerPauseChar(currentChar))
                {
                    yield return new WaitForSeconds(GetCommaSpeed());
                }
                else
                {
                    yield return new WaitForSeconds(GetTextSpeed());
                }
            }

            charCounter++;
            charSafetyCounter++;
        }

        OptionBoxes.Instance.ShowHideOptions();

        skipRequested = false;
        typingCoroutine = null;
        isTyping = false;

        fullText = "";
        //charCounter = 0;

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

    float GetTextSpeed()
    {
        switch (SettingsManager.Instance.settingsData.currentTextSpeed)
        {
            case TextSpeed.Slow:
                return slowText;
            case TextSpeed.Medium:
                return mediumText;
            case TextSpeed.Fast:
                return fastText;

            default:
                return mediumText;
        }
    }
    float GetSentenceSpeed()
    {
        switch (SettingsManager.Instance.settingsData.currentTextSpeed)
        {
            case TextSpeed.Slow:
                return slowSentence;
            case TextSpeed.Medium:
                return mediumSentence;
            case TextSpeed.Fast:
                return fastSentence;

            default:
                return mediumSentence;
        }
    }
    float GetCommaSpeed()
    {
        switch (SettingsManager.Instance.settingsData.currentTextSpeed)
        {
            case TextSpeed.Slow:
                return slowComma;
            case TextSpeed.Medium:
                return mediumComma;
            case TextSpeed.Fast:
                return fastComma;

            default:
                return mediumComma;
        }
    }
}
