using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : Singleton<SettingsMenu>
{
    public static event Action Action_SetNewLanguage;

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


    //--------------------


    private void Start()
    {
        UpdateSettingsMenuDisplay();
    }


    //--------------------


    public void LoadData()
    {
        if (DataManager.Instance.settingData_StoreList != null)
        {
            settingsData.currentLanguage = DataManager.Instance.settingData_StoreList.currentLanguage;
        }
    }
    public void SaveData()
    {
        DataManager.Instance.settingData_StoreList.currentLanguage = settingsData.currentLanguage;
        DataPersistanceManager.instance.SaveGame();
    }


    //--------------------


    public void UpdateSettingsMenuDisplay()
    {
        LoadData();
        ChangeFlagImage();
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


    //--------------------


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
}

[Serializable]
public class SettingData
{
    public Languages currentLanguage;
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