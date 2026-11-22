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
        //print("1. ShowText");
        fullText = text;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        skipRequested = false;
        typingCoroutine = StartCoroutine(TypeText());
    }
    public void SkipTypewriter()
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
        double nextCharacterTime = Time.timeAsDouble;
        while (charCounter <= fullText.Length)
        {
            if (skipRequested && charSafetyCounter <= 1)
            {
                skipRequested = false;
            }
            else if (skipRequested && charSafetyCounter > 1)
            {
                //print("111. skipRequested");
                dialogueText.text = fullText;
                break;
            }

            if (charCounter < fullText.Length)
            {
                char currentChar = fullText[charCounter];
                dialogueText.text = fullText.Substring(0, charCounter + 1);

                PlayTypingSound(currentChar);

                TextSpeed speed = SettingsManager.Instance.settingsData.currentTextSpeed;
                float delay = IsLongerPauseChar(currentChar) ? GetSentenceSpeed(speed)
                    : IsSmallerPauseChar(currentChar) ? GetCommaSpeed(speed) : GetTextSpeed(speed);

                if (speed is TextSpeed.Faster or TextSpeed.Fastest)
                {
                    // Keep fractional frame time so settings 4 and 5 can exceed one character per frame.
                    nextCharacterTime += delay * GetFastDelayScale(speed);
                    while (Time.timeAsDouble < nextCharacterTime)
                        yield return null;
                }
                else
                {
                    yield return new WaitForSeconds(delay);
                    nextCharacterTime = Time.timeAsDouble;
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

    float GetFastDelayScale(TextSpeed speed)
    {
        float fastRate = 1f / fastText;
        float step = fastRate - 1f / mediumText;
        int extraSteps = speed == TextSpeed.Faster ? 1 : 2;
        return fastRate / (fastRate + step * extraSteps);
    }

    float GetTextSpeed(TextSpeed speed)
    {
        switch (speed)
        {
            case TextSpeed.Slow:
                return slowText;
            case TextSpeed.Medium:
                return mediumText;
            case TextSpeed.Fast:
            case TextSpeed.Faster:
            case TextSpeed.Fastest:
                return fastText;

            default:
                return mediumText;
        }
    }
    float GetSentenceSpeed(TextSpeed speed)
    {
        switch (speed)
        {
            case TextSpeed.Slow:
                return slowSentence;
            case TextSpeed.Medium:
                return mediumSentence;
            case TextSpeed.Fast:
            case TextSpeed.Faster:
            case TextSpeed.Fastest:
                return fastSentence;

            default:
                return mediumSentence;
        }
    }
    float GetCommaSpeed(TextSpeed speed)
    {
        switch (speed)
        {
            case TextSpeed.Slow:
                return slowComma;
            case TextSpeed.Medium:
                return mediumComma;
            case TextSpeed.Fast:
            case TextSpeed.Faster:
            case TextSpeed.Fastest:
                return fastComma;

            default:
                return mediumComma;
        }
    }
}
