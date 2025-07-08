using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMessages : Singleton<ButtonMessages>
{
    [Header("General")]
    public ControlTypes controlType;
    public GameObject buttonMessage;
    public Image buttonImage;
    public TextMeshProUGUI buttonText;

    [Header("Sprites")]
    public Sprite keyboard_Left_Button;
    public Sprite keyboard_Down_Button;
    public Sprite keyboard_Right_Button;
    public Sprite keyboard_Up_Button;

    public Sprite xBox_A_Button;
    public Sprite xBox_B_Button;
    public Sprite xBox_Y_Button;
    public Sprite xBox_X_Button;

    public Sprite ps_Circle_Button;
    public Sprite ps_Cross_Button;
    public Sprite ps_Square_Button;
    public Sprite ps_Triangle_Button;


    //--------------------


    private void Start()
    {
        HideButtonMessage();
    }


    //--------------------


    public void ShowButtonMessage(ControlButtons _controlButton, string _buttonText)
    {
        switch (_controlButton)
        {
            case ControlButtons.Left:
                switch (controlType)
                {
                    case ControlTypes.Keyboard:
                        buttonImage.sprite = keyboard_Left_Button;
                        break;
                    case ControlTypes.XBox:
                        buttonImage.sprite = xBox_A_Button;
                        break;
                    case ControlTypes.PlayStation:
                        buttonImage.sprite = ps_Circle_Button;
                        break;

                    default:
                        break;
                }
                break;
            case ControlButtons.Down:
                switch (controlType)
                {
                    case ControlTypes.Keyboard:
                        buttonImage.sprite = keyboard_Down_Button;
                        break;
                    case ControlTypes.XBox:
                        buttonImage.sprite = xBox_B_Button;
                        break;
                    case ControlTypes.PlayStation:
                        buttonImage.sprite = ps_Cross_Button;
                        break;

                    default:
                        break;
                }
                break;
            case ControlButtons.Right:
                switch (controlType)
                {
                    case ControlTypes.Keyboard:
                        buttonImage.sprite = keyboard_Right_Button;
                        break;
                    case ControlTypes.XBox:
                        buttonImage.sprite = xBox_Y_Button;
                        break;
                    case ControlTypes.PlayStation:
                        buttonImage.sprite = ps_Square_Button;
                        break;

                    default:
                        break;
                }
                break;
            case ControlButtons.Up:
                switch (controlType)
                {
                    case ControlTypes.Keyboard:
                        buttonImage.sprite = keyboard_Up_Button;
                        break;
                    case ControlTypes.XBox:
                        buttonImage.sprite = xBox_X_Button;
                        break;
                    case ControlTypes.PlayStation:
                        buttonImage.sprite = ps_Triangle_Button;
                        break;

                    default:
                        break;
                }
                break;

            default:
                break;
        }
        
        
        buttonText.text = _buttonText;

        buttonMessage.SetActive(true);
    }
    public void HideButtonMessage()
    {
        buttonMessage.SetActive(false);
    }
}

public enum ControlTypes
{
    Keyboard,
    XBox,
    PlayStation
}
public enum ControlButtons
{
    Left,
    Down,
    Right,
    Up
}
