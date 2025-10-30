using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : Singleton<SettingsManager>
{
    public static event Action Action_SetNewLanguage;
    public static event Action Action_SetNewTextSpeed;
    public static event Action Action_SetNewStepDisplay;
    public static event Action Action_SetNewCameraMotion;

    public GameObject settingsMenuParent;
    public SettingData settingsData;

    [Header("SettingState")]
    public SettingState settingState;

    [Header("Color")]
    public Color activeSettingSegmentColor;

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

    [Header("StepsDisplay")]
    [SerializeField] Image stepsDisplayImage;
    [SerializeField] Sprite stepsDisplay_Icon_Sprite;
    [SerializeField] Sprite stepsDisplay_Number_Sprite;
    [SerializeField] Sprite stepsDisplay_NumberIcon_Sprite;
    [SerializeField] Sprite stepsDisplay_None_Sprite;

    [Header("Camera Motion")]
    [SerializeField] Image cameraMotionImage;
    [SerializeField] Sprite cameraMotion_Can_Sprite;
    [SerializeField] Sprite cameraMotion_Cannot_Sprite;


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += LoadData;

        Menu_KeyInputs.Action_MenuSettingsNavigationLeft_isPressed += PerformButtonAction_Left;
        Menu_KeyInputs.Action_MenuSettingsNavigationRight_isPressed += PerformButtonAction_Right;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= LoadData;

        Menu_KeyInputs.Action_MenuSettingsNavigationLeft_isPressed -= PerformButtonAction_Left;
        Menu_KeyInputs.Action_MenuSettingsNavigationRight_isPressed -= PerformButtonAction_Right;
    }


    //--------------------


    public void LoadData()
    {
        if (DataManager.Instance.settingData_StoreList != null)
        {
            settingsData.currentLanguage = DataManager.Instance.settingData_StoreList.currentLanguage;
            settingsData.currentTextSpeed = DataManager.Instance.settingData_StoreList.currentTextSpeed;
            settingsData.currentStepDisplay = DataManager.Instance.settingData_StoreList.currentStepDisplay;
            settingsData.currentCameraMotion = DataManager.Instance.settingData_StoreList.currentCameraMotion;
        }

        ChangeFlagImage();
        ChangeTextSpeedImage();
        ChangeStepDisplayImage();
        ChangeCameraMotionImage();
    }
    public void SaveData()
    {
        DataManager.Instance.settingData_StoreList.currentLanguage = settingsData.currentLanguage;
        DataManager.Instance.settingData_StoreList.currentTextSpeed = settingsData.currentTextSpeed;
        DataManager.Instance.settingData_StoreList.currentStepDisplay = settingsData.currentStepDisplay;
        DataManager.Instance.settingData_StoreList.currentCameraMotion = settingsData.currentCameraMotion;

        DataPersistanceManager.instance.SaveGame();
    }


    //--------------------


    public void UpdateSettingsMenuDisplay()
    {
        LoadData();

        ChangeFlagImage();
        ChangeTextSpeedImage();
        ChangeStepDisplayImage();
        ChangeCameraMotionImage();
    }
    
    
    //--------------------


    void PerformButtonAction_Left()
    {
        if (settingState == SettingState.Settings_Language)
            Flag_LeftButton_isPressed();
        else if (settingState == SettingState.Settings_TextSpeed)
            TextSpeed_LeftButton_isPressed();
        else if (settingState == SettingState.Settings_StepDisplay)
            StepDisplay_LeftButton_isPressed();
        else if (settingState == SettingState.Settings_CameraMotion)
            CameraMotion_LeftButton_isPressed();
    }
    void PerformButtonAction_Right()
    {
        if (settingState == SettingState.Settings_Language)
            Flag_RightButton_isPressed();
        else if (settingState == SettingState.Settings_TextSpeed)
            TextSpeed_RightButton_isPressed();
        else if (settingState == SettingState.Settings_StepDisplay)
            StepDisplay_RightButton_isPressed();
        else if (settingState == SettingState.Settings_CameraMotion)
            CameraMotion_RightButton_isPressed();
    }


    //--------------------


    public void Flag_RightButton_isPressed()
    {
        switch (settingsData.currentLanguage)
        {
            case Languages.Norwegian:
                settingsData.currentLanguage = Languages.English;
                break;
            case Languages.English:
                settingsData.currentLanguage = Languages.German;
                break;
            case Languages.German:
                settingsData.currentLanguage = Languages.Norwegian;
                break;
            //case Languages.Japanese:
            //    settingsData.currentLanguage = Languages.Chinese;
            //    break;
            //case Languages.Chinese:
            //    settingsData.currentLanguage = Languages.Korean;
            //    break;
            //case Languages.Korean:
            //    settingsData.currentLanguage = Languages.Norwegian;
            //    break;
            default:
                break;
        }

        ChangeFlagImage();
        SaveData();

        Action_SetNewLanguage?.Invoke();
    }
    public void Flag_LeftButton_isPressed()
    {
        switch (settingsData.currentLanguage)
        {
            case Languages.Norwegian:
                settingsData.currentLanguage = Languages.German;
                break;
            case Languages.English:
                settingsData.currentLanguage = Languages.Norwegian;
                break;
            case Languages.German:
                settingsData.currentLanguage = Languages.English;
                break;
            //case Languages.Japanese:
            //    settingsData.currentLanguage = Languages.German;
            //    break;
            //case Languages.Chinese:
            //    settingsData.currentLanguage = Languages.Japanese;
            //    break;
            //case Languages.Korean:
            //    settingsData.currentLanguage = Languages.Chinese;
            //    break;
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

    public void StepDisplay_RightButton_isPressed()
    {
        switch (settingsData.currentStepDisplay)
        {
            case StepDisplay.Icon:
                settingsData.currentStepDisplay = StepDisplay.Number;
                break;
            case StepDisplay.Number:
                settingsData.currentStepDisplay = StepDisplay.NumberIcon;
                break;
            case StepDisplay.NumberIcon:
                settingsData.currentStepDisplay = StepDisplay.None;
                break;
            case StepDisplay.None:
                settingsData.currentStepDisplay = StepDisplay.Icon;
                break;

            default:
                break;
        }

        ChangeStepDisplayImage();
        SaveData();

        Action_SetNewStepDisplay?.Invoke();
    }
    public void StepDisplay_LeftButton_isPressed()
    {
        switch (settingsData.currentStepDisplay)
        {
            case StepDisplay.Icon:
                settingsData.currentStepDisplay = StepDisplay.None;
                break;
            case StepDisplay.Number:
                settingsData.currentStepDisplay = StepDisplay.Icon;
                break;
            case StepDisplay.NumberIcon:
                settingsData.currentStepDisplay = StepDisplay.Number;
                break;
            case StepDisplay.None:
                settingsData.currentStepDisplay = StepDisplay.NumberIcon;
                break;

            default:
                break;
        }

        ChangeStepDisplayImage();
        SaveData();

        Action_SetNewStepDisplay?.Invoke();
    }
    void ChangeStepDisplayImage()
    {
        switch (settingsData.currentStepDisplay)
        {
            case StepDisplay.Icon:
                stepsDisplayImage.sprite = stepsDisplay_Icon_Sprite;
                break;
            case StepDisplay.Number:
                stepsDisplayImage.sprite = stepsDisplay_Number_Sprite;
                break;
            case StepDisplay.NumberIcon:
                stepsDisplayImage.sprite = stepsDisplay_NumberIcon_Sprite;
                break;
            case StepDisplay.None:
                stepsDisplayImage.sprite = stepsDisplay_None_Sprite;
                break;

            default:
                break;
        }
    }

    public void CameraMotion_RightButton_isPressed()
    {
        switch (settingsData.currentCameraMotion)
        {
            case CameraMotion.Can:
                settingsData.currentCameraMotion = CameraMotion.Cannot;
                break;
            case CameraMotion.Cannot:
                settingsData.currentCameraMotion = CameraMotion.Can;
                break;

            default:
                break;
        }

        ChangeCameraMotionImage();
        SaveData();

        Action_SetNewCameraMotion?.Invoke();
    }
    public void CameraMotion_LeftButton_isPressed()
    {
        switch (settingsData.currentCameraMotion)
        {
            case CameraMotion.Can:
                settingsData.currentCameraMotion = CameraMotion.Cannot;
                break;
            case CameraMotion.Cannot:
                settingsData.currentCameraMotion = CameraMotion.Can;
                break;

            default:
                break;
        }

        ChangeCameraMotionImage();
        SaveData();

        Action_SetNewCameraMotion?.Invoke();
    }
    void ChangeCameraMotionImage()
    {
        switch (settingsData.currentCameraMotion)
        {
            case CameraMotion.Can:
                cameraMotionImage.sprite = cameraMotion_Can_Sprite;
                break;
            case CameraMotion.Cannot:
                cameraMotionImage.sprite = cameraMotion_Cannot_Sprite;
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
    public StepDisplay currentStepDisplay;
    public CameraMotion currentCameraMotion;
}
