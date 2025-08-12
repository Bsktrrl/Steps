using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionButton : MonoBehaviour, ISelectHandler
{
    public static event Action Action_OptionButtonIsPressed;

    [SerializeField] int optionNumber;


    //--------------------


    private void Start()
    {
        if (gameObject == OptionBoxes.Instance.optionButton_1.gameObject)
            optionNumber = 1;
        else if (gameObject == OptionBoxes.Instance.optionButton_2.gameObject)
            optionNumber = 2;
        else if (gameObject == OptionBoxes.Instance.optionButton_3.gameObject)
            optionNumber = 3;
        else if (gameObject == OptionBoxes.Instance.optionButton_4.gameObject)
            optionNumber = 4;
    }


    //--------------------


    public void button_isPressed()
    {
        if (DialogueManager.Instance.typingSound != null)
            DialogueManager.Instance.typingSound.Play();

        //print("Button: " + gameObject.name + " is pressed");

        DialogueManager.Instance.selectedButton = optionNumber;

        Action_OptionButtonIsPressed?.Invoke();
    }


    //--------------------


    public void OnSelect(BaseEventData eventData)
    {
        if (DialogueManager.Instance.typingSound != null)
            DialogueManager.Instance.typingSound.Play();

        DialogueManager.Instance.selectedButton = optionNumber;
    }
}
