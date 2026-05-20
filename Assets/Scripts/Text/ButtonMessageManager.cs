using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMessageManager : Singleton<ButtonMessageManager>
{

    [Header("Button Messages")]
    public ButtonMessageList buttonMessages;



    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += UpdateButtonMessages;
        SettingsManager.Action_SetNewLanguage += UpdateButtonMessages;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= UpdateButtonMessages;
        SettingsManager.Action_SetNewLanguage -= UpdateButtonMessages;
    }


    //--------------------


    void UpdateButtonMessages()
    {
        buttonMessages.buttonMessage_Talk.button_MessageText = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)SettingsManager.Instance.settingsData.currentLanguage].interractableButton_Message_Talk_Keyboard;
        buttonMessages.buttonMessage_PlaceGlueplant.button_MessageText = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)SettingsManager.Instance.settingsData.currentLanguage].interractableButton_Message_PlaceGlueplant_Keyboard;
        buttonMessages.buttonMessage_Push.button_MessageText = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)SettingsManager.Instance.settingsData.currentLanguage].interractableButton_Message_Push_Keyboard;

        SetAllButtonMessages();
    }


    //--------------------


    void SetAllButtonMessages()
    {
        SetButtonMessage(buttonMessages.buttonMessage_Talk);
        SetButtonMessage(buttonMessages.buttonMessage_PlaceGlueplant);
        SetButtonMessage(buttonMessages.buttonMessage_Push);
    }

    public void SetButtonMessage(ButtonMessage _buttonMessage)
    {
        _buttonMessage.buttonMessage_Text.text = SetButtonMessageText(_buttonMessage);
        _buttonMessage.buttonMessage_Image.sprite = SetButtonMessageSprite(_buttonMessage);
    }
    string SetButtonMessageText(ButtonMessage _buttonMessage)
    {
        return _buttonMessage.button_MessageText;
    }
    Sprite SetButtonMessageSprite(ButtonMessage _buttonMessage)
    {
        switch (ControllerState.Instance.activeController)
        {
            case InputType.Keyboard:
                return _buttonMessage.button_MessageSprite_Keyboard;
            case InputType.Xbox:
                return _buttonMessage.button_MessageSprite_PlayStation;
            case InputType.PlayStation:
                return _buttonMessage.button_MessageSprite_xBox;

            default:
                return null;
        }
    }


    //--------------------


    public void ShowButtonMessage(ButtonMessage _buttonMessage)
    {
        _buttonMessage.buttonMessage_Parent.SetActive(true);
    }
    public void HideButtonMessage(ButtonMessage _buttonMessage)
    {
        _buttonMessage.buttonMessage_Parent.SetActive(false);
    }
}


[Serializable]
public class ButtonMessageList
{
    public ButtonMessage buttonMessage_Talk;
    public ButtonMessage buttonMessage_PlaceGlueplant;
    public ButtonMessage buttonMessage_Push;
}
[Serializable]
public class ButtonMessage
{
    [Header("Parents")]
    public GameObject buttonMessage_Parent;
    public TextMeshProUGUI buttonMessage_Text;
    public Image buttonMessage_Image;

    [Header("Message Text List in languages")]
    [HideInInspector] public string button_MessageText;

    [Header("Button Sprite")]
    public Sprite button_MessageSprite_Keyboard;
    public Sprite button_MessageSprite_PlayStation;
    public Sprite button_MessageSprite_xBox;
}
