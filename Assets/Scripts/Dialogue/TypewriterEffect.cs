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

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = null;
        skipRequested = false;
        isTyping = false;
    }


    //--------------------


    public void ShowText(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = null;
        skipRequested = false;
        isTyping = true;
        fullText = text ?? string.Empty;
        charCounter = 0;
        charSafetyCounter = 0;

        // Give TMP the complete line so formatting and sprite tags stay intact during the reveal.
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.text = fullText;
        dialogueText.ForceMeshUpdate(ignoreActiveState: true);
        maxCharCounter = dialogueText.textInfo.characterCount;

        if (maxCharCounter == 0)
        {
            FinishTyping();
            return;
        }

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
        double nextCharacterTime = Time.timeAsDouble;
        while (charCounter < maxCharCounter)
        {
            if (skipRequested)
            {
                if (charSafetyCounter >= 2)
                    break;
                skipRequested = false;
            }

            TMP_CharacterInfo element = dialogueText.textInfo.characterInfo[charCounter];
            dialogueText.maxVisibleCharacters = ++charCounter;
            bool isSprite = element.elementType == TMP_TextElementType.Sprite;
            // TMP can turn tags such as <br> into layout characters. Reveal them without a typing beat.
            if (!isSprite && element.stringLength > 1 && element.index >= 0 &&
                element.index < fullText.Length && fullText[element.index] == '<')
                continue;

            char currentChar = element.character;
            if (!isSprite)
                PlayTypingSound(currentChar);

            charSafetyCounter++;
            TextSpeed speed = SettingsManager.Instance.settingsData.currentTextSpeed;
            float delay = !isSprite && IsLongerPauseChar(currentChar) ? GetSentenceSpeed(speed)
                : !isSprite && IsSmallerPauseChar(currentChar) ? GetCommaSpeed(speed) : GetTextSpeed(speed);

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

        // A line made only of layout elements needs to yield before its coroutine can finish.
        if (charSafetyCounter == 0)
            yield return null;

        FinishTyping();
    }

    void FinishTyping()
    {
        dialogueText.maxVisibleCharacters = int.MaxValue;
        OptionBoxes.Instance.ShowHideOptions();

        skipRequested = false;
        typingCoroutine = null;
        isTyping = false;

        fullText = string.Empty;

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
