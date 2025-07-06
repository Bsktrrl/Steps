using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : Singleton<TypewriterEffect>
{
    [SerializeField] TextMeshProUGUI dialogueText;
    float letterDelay = 0.04f;
    float sentenceDelay = 0.2f;

    string fullText;
    private Coroutine typingCoroutine;
    bool skipRequested;

    public bool isTyping;


    //--------------------


    
    void Update()
    {
        if (Player_KeyInputs.Instance.dialogueSkip_isPressed)
        {
            Player_KeyInputs.Instance.dialogueSkip_isPressed = false;

            if (typingCoroutine != null)
            {
                skipRequested = true;
            }
        }
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

    IEnumerator TypeText()
    {
        isTyping = true;
        dialogueText.text = "";

        for (int i = 0; i <= fullText.Length; i++)
        {
            if (skipRequested)
            {
                dialogueText.text = fullText;
                break;
            }

            dialogueText.text = fullText.Substring(0, i);

            if (i > 0 && !char.IsWhiteSpace(fullText[i - 1]) && DialogueManager.Instance.typingSound != null)
            {
                float pitchRange = Random.Range(0.9f, 1.1f);
                DialogueManager.Instance.typingSound.pitch = pitchRange;
                DialogueManager.Instance.typingSound.Play();
            }

            if (i > 0 && fullText[i - 1] == '.')
            {
                yield return new WaitForSeconds(sentenceDelay);
            }
            else
            {
                yield return new WaitForSeconds(letterDelay);
            }
        }

        // Typing finished or skipped
        OptionBoxes.Instance.ShowHideOptions();

        isTyping = false;
        typingCoroutine = null;
        skipRequested = false;
    }
}
