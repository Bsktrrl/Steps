using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : Singleton<SettingsMenu>
{
    public static event Action Action_SetNewLanguage;
    public static event Action Action_SetNewTextSpeed;

    public GameObject settingsMenuParent;
    public SettingData settingsData;

    [Header("Flag")]
    [SerializeField] Image flagImage;
    [SerializeField] Sprite flag_Norway_Sprite;
    [SerializeField] Sprite flag_England_Sprite;
    [SerializeField] Sprite flag_Germany_Sprite;
    [SerializeField] Sprite flag_China_Sprite;
    [SerializeField] Sprite flag_Japan_Sprite;
    [SerializeField] Sprite flag_Korea_Sprite;

    [Header("TextSpeed")]
    [SerializeField] Image textSpeedImage;
    [SerializeField] Sprite textSpeed_Slow_Sprite;
    [SerializeField] Sprite textSpeed_Medium_Sprite;
    [SerializeField] Sprite textSpeed_Fast_Sprite;


    //--------------------


    private void Start()
    {
        UpdateSettingsMenuDisplay();
    }
    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += LoadData;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= LoadData;
    }


    //--------------------


    public void LoadData()
    {
        if (DataManager.Instance.settingData_StoreList != null)
        {
            settingsData.currentLanguage = DataManager.Instance.settingData_StoreList.currentLanguage;
            settingsData.currentTextSpeed = DataManager.Instance.settingData_StoreList.currentTextSpeed;
        }
    }
    public void SaveData()
    {
        DataManager.Instance.settingData_StoreList.currentLanguage = settingsData.currentLanguage;
        DataManager.Instance.settingData_StoreList.currentTextSpeed = settingsData.currentTextSpeed;

        DataPersistanceManager.instance.SaveGame();
    }


    //--------------------


    public void UpdateSettingsMenuDisplay()
    {
        LoadData();
        ChangeFlagImage();
        ChangeTextSpeedImage();
    }


    //--------------------


    public void Flag_RightButton_isPressed()
    {
        print("1. Flag_RightButton_isPressed");
        switch (settingsData.currentLanguage)
        {
            case Languages.Norwegian:
                settingsData.currentLanguage = Languages.English;
                break;
            case Languages.English:
                settingsData.currentLanguage = Languages.German;
                break;
            case Languages.German:
                settingsData.currentLanguage = Languages.Japanese;
                break;
            case Languages.Japanese:
                settingsData.currentLanguage = Languages.Chinese;
                break;
            case Languages.Chinese:
                settingsData.currentLanguage = Languages.Korean;
                break;
            case Languages.Korean:
                settingsData.currentLanguage = Languages.Norwegian;
                break;
            default:
                break;
        }

        ChangeFlagImage();
        SaveData();

        Action_SetNewLanguage?.Invoke();
    }
    public void Flag_LeftButton_isPressed()
    {
        print("2. Flag_LeftButton_isPressed");

        switch (settingsData.currentLanguage)
        {
            case Languages.Norwegian:
                settingsData.currentLanguage = Languages.Korean;
                break;
            case Languages.English:
                settingsData.currentLanguage = Languages.Norwegian;
                break;
            case Languages.German:
                settingsData.currentLanguage = Languages.English;
                break;
            case Languages.Japanese:
                settingsData.currentLanguage = Languages.German;
                break;
            case Languages.Chinese:
                settingsData.currentLanguage = Languages.Japanese;
                break;
            case Languages.Korean:
                settingsData.currentLanguage = Languages.Chinese;
                break;
            default:
                break;
        }

        ChangeFlagImage();
        SaveData();

        Action_SetNewLanguage?.Invoke();
    }
    void ChangeFlagImage()
    {
        switch (settingsData.currentLanguage)
        {
            case Languages.Norwegian:
                flagImage.sprite = flag_Norway_Sprite;
                break;
            case Languages.English:
                flagImage.sprite = flag_England_Sprite;
                break;
            case Languages.German:
                flagImage.sprite = flag_Germany_Sprite;
                break;
            case Languages.Japanese:
                flagImage.sprite = flag_Japan_Sprite;
                break;
            case Languages.Chinese:
                flagImage.sprite = flag_China_Sprite;
                break;
            case Languages.Korean:
                flagImage.sprite = flag_Korea_Sprite;
                break;
            default:
                break;
        }
    }

    public void TextSpeed_RightButton_isPressed()
    {
        print("1. TextSpeed_RightButton_isPressed");
        switch (settingsData.currentTextSpeed)
        {
            case TextSpeed.Slow:
                settingsData.currentTextSpeed = TextSpeed.Medium;
                break;
            case TextSpeed.Medium:
                settingsData.currentTextSpeed = TextSpeed.Fast;
                break;
            case TextSpeed.Fast:
                settingsData.currentTextSpeed = TextSpeed.Slow;
                break;

            default:
                break;
        }

        ChangeTextSpeedImage();
        SaveData();

        Action_SetNewTextSpeed?.Invoke();
    }
    public void TextSpeed_LeftButton_isPressed()
    {
        print("1. TextSpeed_RightButton_isPressed");
        switch (settingsData.currentTextSpeed)
        {
            case TextSpeed.Slow:
                settingsData.currentTextSpeed = TextSpeed.Fast;
                break;
            case TextSpeed.Medium:
                settingsData.currentTextSpeed = TextSpeed.Slow;
                break;
            case TextSpeed.Fast:
                settingsData.currentTextSpeed = TextSpeed.Medium;
                break;

            default:
                break;
        }

        ChangeTextSpeedImage();
        SaveData();

        Action_SetNewTextSpeed?.Invoke();
    }
    void ChangeTextSpeedImage()
    {
        switch (settingsData.currentTextSpeed)
        {
            case TextSpeed.Slow:
                textSpeedImage.sprite = textSpeed_Slow_Sprite;
                break;
            case TextSpeed.Medium:
                textSpeedImage.sprite = textSpeed_Medium_Sprite;
                break;
            case TextSpeed.Fast:
                textSpeedImage.sprite = textSpeed_Fast_Sprite;
                break;

            default:
                break;
        }
    }
}

[Serializable]
public class SettingData
{
    public Languages currentLanguage;
    public TextSpeed currentTextSpeed;
}

public enum Languages
{
    Norwegian,
    English,
    German,
    Japanese,
    Chinese,
    Korean
}
public enum TextSpeed
{
    Medium,
    Fast,
    Slow
}