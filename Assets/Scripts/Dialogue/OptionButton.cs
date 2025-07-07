using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionButton : MonoBehaviour, ISelectHandler
{
    

    //--------------------


    public void button_isPressed()
    {
        if (DialogueManager.Instance.typingSound != null)
            DialogueManager.Instance.typingSound.Play();

        print("Button: " + gameObject.name + " is pressed");
    }


    //--------------------


    public void OnSelect(BaseEventData eventData)
    {
        if (DialogueManager.Instance.typingSound != null)
            DialogueManager.Instance.typingSound.Play();
    }
}
