using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMessageManager : Singleton<ButtonMessageManager>
{
    public ButtonMessageList buttonMessages;

    public GameObject buttonMessage_Parent;


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
        buttonMessages.buttonMessage_SwiftSwim_Up.button_MessageText = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)SettingsManager.Instance.settingsData.currentLanguage].interractableButton_Message_FlippersUP_Keyboard;
        buttonMessages.buttonMessage_SwiftSwim_Down.button_MessageText = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)SettingsManager.Instance.settingsData.currentLanguage].interractableButton_Message_FlippersDOWN_Keyboard;
        buttonMessages.buttonMessage_Ascend.button_MessageText = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)SettingsManager.Instance.settingsData.currentLanguage].interractableButton_Message_DrillHelmet_Keyboard;
        buttonMessages.buttonMessage_Descend.button_MessageText = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)SettingsManager.Instance.settingsData.currentLanguage].interractableButton_Message_DrillShoes_Keyboard;
        buttonMessages.buttonMessage_Dash.button_MessageText = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)SettingsManager.Instance.settingsData.currentLanguage].ability_Message_HandDrill_Keyboard;
        buttonMessages.buttonMessage_GrapplingHook.button_MessageText = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)SettingsManager.Instance.settingsData.currentLanguage].ability_Message_GrapplingHook_Keyboard;
        buttonMessages.buttonMessage_Jump.button_MessageText = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)SettingsManager.Instance.settingsData.currentLanguage].ability_Message_SpringShoes_Keyboard;
        buttonMessages.buttonMessage_CeilingClimb.button_MessageText = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)SettingsManager.Instance.settingsData.currentLanguage].interractableButton_Message_ClimbingGloves_Keyboard;

        buttonMessages.buttonMessage_Talk.button_MessageText = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)SettingsManager.Instance.settingsData.currentLanguage].interractableButton_Message_Talk_Keyboard;
        buttonMessages.buttonMessage_Respawn.button_MessageText = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)SettingsManager.Instance.settingsData.currentLanguage].interractableButton_Message_Respawn_Keyboard;
        buttonMessages.buttonMessage_PlaceGlueplant.button_MessageText = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)SettingsManager.Instance.settingsData.currentLanguage].interractableButton_Message_PlaceGlueplant_Keyboard;
    }


    //--------------------


    public string GetButtonMessageText(ButtonMessage _buttonMessage)
    {
        return _buttonMessage.button_MessageText;
    }
    public Sprite GetButtonMessageSprite(ControlTypes _controlTypes, ButtonMessage _buttonMessage)
    {
        switch (_controlTypes)
        {
            case ControlTypes.Keyboard:
                return _buttonMessage.button_MessageSprite_Keyboard;
            case ControlTypes.XBox:
                return _buttonMessage.button_MessageSprite_PlayStation;
            case ControlTypes.PlayStation:
                return _buttonMessage.button_MessageSprite_xBox;

            default:
                return null;
        }
    }


    //--------------------


    public void ShowButtonMessage()
    {
        buttonMessage_Parent.SetActive(true);
    }
    public void HideButtonMessage()
    {
        buttonMessage_Parent.SetActive(false);
    }
}


[Serializable]
public class ButtonMessageList
{
    public ButtonMessage buttonMessage_Talk;
    public ButtonMessage buttonMessage_SwiftSwim_Up;
    public ButtonMessage buttonMessage_SwiftSwim_Down;
    public ButtonMessage buttonMessage_Ascend;
    public ButtonMessage buttonMessage_Descend;
    public ButtonMessage buttonMessage_Dash;
    public ButtonMessage buttonMessage_GrapplingHook;
    public ButtonMessage buttonMessage_Jump;
    public ButtonMessage buttonMessage_CeilingClimb;
    public ButtonMessage buttonMessage_Respawn;
    public ButtonMessage buttonMessage_PlaceGlueplant;
}
[Serializable]
public class ButtonMessage
{
    [Header("Message Text List in languages")]
    [HideInInspector] public string button_MessageText;

    [Header("Button Sprite")]
    public Sprite button_MessageSprite_Keyboard;
    public Sprite button_MessageSprite_PlayStation;
    public Sprite button_MessageSprite_xBox;
}

public enum ControlTypes
{
    Keyboard,
    XBox,
    PlayStation
}