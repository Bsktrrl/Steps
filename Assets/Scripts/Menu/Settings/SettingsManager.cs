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
    public static event Action Action_SetNewRevertedCameraMotion;
    public static event Action Action_SetNewSkipIntro;

    public GameObject settingsMenuParent;
    public SettingData settingsData;

    [Header("SettingState")]
    public SettingState settingState;

    [Header("Color")]
    public Color activeSettingSegmentColor;

    #region General Settings
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

    [Header("Reverted Camera Motion")]
    [SerializeField] Image revertedCameraMotionImage;
    [SerializeField] Sprite revertedCameraMotion_Normal_Sprite;
    [SerializeField] Sprite revertedCameraMotion_Reverted_Sprite;

    [Header("SkipIntro")]
    [SerializeField] Image skipIntroImage;
    [SerializeField] Sprite skipIntro_YES_Sprite;
    [SerializeField] Sprite skipIntro_NO_Sprite;

    #endregion

    #region Controls Settings

    #endregion

    #region Video Settings

    #endregion

    #region Audio Settings
    [Header("Volume Parameters")]
    public GameObject marker_MasterVolume;
    public GameObject marker_EnviromentVolume;
    public GameObject marker_WeatherVolume;
    public GameObject marker_PlayerVolume;
    public GameObject marker_UIVolume;
    public GameObject marker_DialogueVolume;

    #endregion


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
            settingsData.currentRevertedCameraMotion = DataManager.Instance.settingData_StoreList.currentRevertedCameraMotion;
            settingsData.currentSkipIntro = DataManager.Instance.settingData_StoreList.currentSkipIntro;
        }

        ChangeFlagImage();
        ChangeTextSpeedImage();
        ChangeStepDisplayImage();
        ChangeCameraMotionImage();
        ChangeRevertedCameraMotionImage();
        ChangeSkipIntroImage();
    }
    public void SaveData()
    {
        DataManager.Instance.settingData_StoreList.currentLanguage = settingsData.currentLanguage;
        DataManager.Instance.settingData_StoreList.currentTextSpeed = settingsData.currentTextSpeed;
        DataManager.Instance.settingData_StoreList.currentStepDisplay = settingsData.currentStepDisplay;
        DataManager.Instance.settingData_StoreList.currentCameraMotion = settingsData.currentCameraMotion;
        DataManager.Instance.settingData_StoreList.currentRevertedCameraMotion = settingsData.currentRevertedCameraMotion;
        DataManager.Instance.settingData_StoreList.currentSkipIntro = settingsData.currentSkipIntro;

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
        ChangeRevertedCameraMotionImage();
        ChangeSkipIntroImage();
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
        else if (settingState == SettingState.Settings_RevertedCameraMotion)
            RevertedCameraMotion_LeftButton_isPressed();
        else if (settingState == SettingState.Settings_SkipLevelIntro)
            SkipIntro_LeftButton_isPressed();

        else if (settingState == SettingState.Audio_Master)
            Audio_Master_LeftButton_isPressed();
        else if (settingState == SettingState.Audio_Enviroment)
            Audio_Enviroment_LeftButton_isPressed();
        else if (settingState == SettingState.Audio_Weather)
            Audio_Weather_LeftButton_isPressed();
        else if (settingState == SettingState.Audio_Player)
            Audio_Player_LeftButton_isPressed();
        else if (settingState == SettingState.Audio_UI)
            Audio_UI_LeftButton_isPressed();
        else if (settingState == SettingState.Audio_Dialogue)
            Audio_Dialogue_LeftButton_isPressed();
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
        else if (settingState == SettingState.Settings_RevertedCameraMotion)
            RevertedCameraMotion_RightButton_isPressed();
        else if (settingState == SettingState.Settings_SkipLevelIntro)
            SkipIntro_RightButton_isPressed();

        else if (settingState == SettingState.Audio_Master)
            Audio_Master_RightButton_isPressed();
        else if (settingState == SettingState.Audio_Enviroment)
            Audio_Enviroment_RightButton_isPressed();
        else if (settingState == SettingState.Audio_Weather)
            Audio_Weather_RightButton_isPressed();
        else if (settingState == SettingState.Audio_Player)
            Audio_Player_RightButton_isPressed();
        else if (settingState == SettingState.Audio_UI)
            Audio_UI_RightButton_isPressed();
        else if (settingState == SettingState.Audio_Dialogue)
            Audio_Dialogue_RightButton_isPressed();
    }


    //--------------------


    #region General Settings ButtonPress
    public void Flag_RightButton_isPressed()
    {
        switch (settingsData.currentLanguage)
        {
            case Languages.Norwegian:
                settingsData.currentLanguage = Languages.English;
                break;
            case Languages.English:
                settingsData.currentLanguage = Languages.Norwegian;
                break;
            //case Languages.German:
            //    settingsData.currentLanguage = Languages.Norwegian;
            //    break;
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
                settingsData.currentLanguage = Languages.English;
                break;
            case Languages.English:
                settingsData.currentLanguage = Languages.Norwegian;
                break;
            //case Languages.German:
            //    settingsData.currentLanguage = Languages.English;
            //    break;
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
            case StepDisplay.Steps:
                settingsData.currentStepDisplay = StepDisplay.Number;
                break;
            case StepDisplay.Number:
                settingsData.currentStepDisplay = StepDisplay.NumberSteps;
                break;
            case StepDisplay.NumberSteps:
                settingsData.currentStepDisplay = StepDisplay.None;
                break;
            case StepDisplay.None:
                settingsData.currentStepDisplay = StepDisplay.Steps;
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
            case StepDisplay.Steps:
                settingsData.currentStepDisplay = StepDisplay.None;
                break;
            case StepDisplay.Number:
                settingsData.currentStepDisplay = StepDisplay.Steps;
                break;
            case StepDisplay.NumberSteps:
                settingsData.currentStepDisplay = StepDisplay.Number;
                break;
            case StepDisplay.None:
                settingsData.currentStepDisplay = StepDisplay.NumberSteps;
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
            case StepDisplay.Steps:
                stepsDisplayImage.sprite = stepsDisplay_Icon_Sprite;
                break;
            case StepDisplay.Number:
                stepsDisplayImage.sprite = stepsDisplay_Number_Sprite;
                break;
            case StepDisplay.NumberSteps:
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

    public void RevertedCameraMotion_RightButton_isPressed()
    {
        switch (settingsData.currentRevertedCameraMotion)
        {
            case RevertedCameraMotion.Normal:
                settingsData.currentRevertedCameraMotion = RevertedCameraMotion.Reverted;
                break;
            case RevertedCameraMotion.Reverted:
                settingsData.currentRevertedCameraMotion = RevertedCameraMotion.Normal;
                break;

            default:
                break;
        }

        ChangeRevertedCameraMotionImage();
        SaveData();

        Action_SetNewRevertedCameraMotion?.Invoke();
    }
    public void RevertedCameraMotion_LeftButton_isPressed()
    {
        switch (settingsData.currentRevertedCameraMotion)
        {
            case RevertedCameraMotion.Normal:
                settingsData.currentRevertedCameraMotion = RevertedCameraMotion.Reverted;
                break;
            case RevertedCameraMotion.Reverted:
                settingsData.currentRevertedCameraMotion = RevertedCameraMotion.Normal;
                break;

            default:
                break;
        }

        ChangeRevertedCameraMotionImage();
        SaveData();

        Action_SetNewRevertedCameraMotion?.Invoke();
    }
    void ChangeRevertedCameraMotionImage()
    {
        switch (settingsData.currentRevertedCameraMotion)
        {
            case RevertedCameraMotion.Normal:
                revertedCameraMotionImage.sprite = revertedCameraMotion_Normal_Sprite;
                break;
            case RevertedCameraMotion.Reverted:
                revertedCameraMotionImage.sprite = revertedCameraMotion_Reverted_Sprite;
                break;

            default:
                break;
        }
    }

    public void SkipIntro_RightButton_isPressed()
    {
        switch (settingsData.currentSkipIntro)
        {
            case SkipIntro.Yes:
                settingsData.currentSkipIntro = SkipIntro.No;
                break;
            case SkipIntro.No:
                settingsData.currentSkipIntro = SkipIntro.Yes;
                break;

            default:
                break;
        }

        ChangeSkipIntroImage();
        SaveData();

        Action_SetNewSkipIntro?.Invoke();
    }
    public void SkipIntro_LeftButton_isPressed()
    {
        switch (settingsData.currentSkipIntro)
        {
            case SkipIntro.Yes:
                settingsData.currentSkipIntro = SkipIntro.No;
                break;
            case SkipIntro.No:
                settingsData.currentSkipIntro = SkipIntro.Yes;
                break;

            default:
                break;
        }

        ChangeSkipIntroImage();
        SaveData();

        Action_SetNewSkipIntro?.Invoke();
    }
    void ChangeSkipIntroImage()
    {
        switch (settingsData.currentSkipIntro)
        {
            case SkipIntro.Yes:
                skipIntroImage.sprite = skipIntro_YES_Sprite;
                break;
            case SkipIntro.No:
                skipIntroImage.sprite = skipIntro_NO_Sprite;
                break;

            default:
                break;
        }
    }

    public void Action_SetNewLanguage_isActive()
    {
        Action_SetNewLanguage?.Invoke();
    }

    #endregion

    #region Audio SettingsButtonPress

    AudioVolumStates SetPercentageRightButtonClick(AudioVolumStates audioVolumeState)
    {
        switch (audioVolumeState)
        {
            case AudioVolumStates._0_percent:
                return AudioVolumStates._10_percent;
            case AudioVolumStates._10_percent:
                return AudioVolumStates._20_percent;
            case AudioVolumStates._20_percent:
                return AudioVolumStates._30_percent;
            case AudioVolumStates._30_percent:
                return AudioVolumStates._40_percent;
            case AudioVolumStates._40_percent:
                return AudioVolumStates._50_percent;
            case AudioVolumStates._50_percent:
                return AudioVolumStates._60_percent;
            case AudioVolumStates._60_percent:
                return AudioVolumStates._70_percent;
            case AudioVolumStates._70_percent:
                return AudioVolumStates._80_percent;
            case AudioVolumStates._80_percent:
                return AudioVolumStates._90_percent;
            case AudioVolumStates._90_percent:
                return AudioVolumStates._100_percent;
            case AudioVolumStates._100_percent:
                return AudioVolumStates._100_percent;

            default:
                return AudioVolumStates._0_percent;
        }
    }
    AudioVolumStates SetPercentageLeftButtonClick(AudioVolumStates audioVolumeState)
    {
        switch (audioVolumeState)
        {
            case AudioVolumStates._0_percent:
                return AudioVolumStates._0_percent;
            case AudioVolumStates._10_percent:
                return AudioVolumStates._0_percent;
            case AudioVolumStates._20_percent:
                return AudioVolumStates._10_percent;
            case AudioVolumStates._30_percent:
                return AudioVolumStates._20_percent;
            case AudioVolumStates._40_percent:
                return AudioVolumStates._30_percent;
            case AudioVolumStates._50_percent:
                return AudioVolumStates._40_percent;
            case AudioVolumStates._60_percent:
                return AudioVolumStates._50_percent;
            case AudioVolumStates._70_percent:
                return AudioVolumStates._60_percent;
            case AudioVolumStates._80_percent:
                return AudioVolumStates._70_percent;
            case AudioVolumStates._90_percent:
                return AudioVolumStates._80_percent;
            case AudioVolumStates._100_percent:
                return AudioVolumStates._90_percent;

            default:
                return AudioVolumStates._0_percent;
        }
    }

    public void Audio_Master_RightButton_isPressed()
    {
        AudioSettingsManager.Instance.Set_Master_Volume(SetPercentageRightButtonClick(DataManager.Instance.settingData_StoreList.volume_Master));
    }
    public void Audio_Master_LeftButton_isPressed()
    {
        AudioSettingsManager.Instance.Set_Master_Volume(SetPercentageLeftButtonClick(DataManager.Instance.settingData_StoreList.volume_Master));
    }

    public void Audio_Enviroment_RightButton_isPressed()
    {
        AudioSettingsManager.Instance.Set_Enviroment_GroupVolume(SetPercentageRightButtonClick(DataManager.Instance.settingData_StoreList.volume_3DEnviroment));
    }
    public void Audio_Enviroment_LeftButton_isPressed()
    {
        AudioSettingsManager.Instance.Set_Enviroment_GroupVolume(SetPercentageLeftButtonClick(DataManager.Instance.settingData_StoreList.volume_3DEnviroment));
    }

    public void Audio_Weather_RightButton_isPressed()
    {
        AudioSettingsManager.Instance.Set_Weather_GroupVolume(SetPercentageRightButtonClick(DataManager.Instance.settingData_StoreList.volume_Weather));
    }
    public void Audio_Weather_LeftButton_isPressed()
    {
        AudioSettingsManager.Instance.Set_Weather_GroupVolume(SetPercentageLeftButtonClick(DataManager.Instance.settingData_StoreList.volume_Weather));
    }

    public void Audio_Player_RightButton_isPressed()
    {
        AudioSettingsManager.Instance.Set_Player_GroupVolume(SetPercentageRightButtonClick(DataManager.Instance.settingData_StoreList.volume_Player));
    }
    public void Audio_Player_LeftButton_isPressed()
    {
        AudioSettingsManager.Instance.Set_Player_GroupVolume(SetPercentageLeftButtonClick(DataManager.Instance.settingData_StoreList.volume_Player));
    }

    public void Audio_UI_RightButton_isPressed()
    {
        AudioSettingsManager.Instance.Set_UI_GroupVolume(SetPercentageRightButtonClick(DataManager.Instance.settingData_StoreList.volume_UI));
    }
    public void Audio_UI_LeftButton_isPressed()
    {
        AudioSettingsManager.Instance.Set_UI_GroupVolume(SetPercentageLeftButtonClick(DataManager.Instance.settingData_StoreList.volume_UI));
    }

    public void Audio_Dialogue_RightButton_isPressed()
    {
        AudioSettingsManager.Instance.Set_Dialogue_GroupVolume(SetPercentageRightButtonClick(DataManager.Instance.settingData_StoreList.volume_Dialogue));
    }
    public void Audio_Dialogue_LeftButton_isPressed()
    {
        AudioSettingsManager.Instance.Set_Dialogue_GroupVolume(SetPercentageLeftButtonClick(DataManager.Instance.settingData_StoreList.volume_Dialogue));
    }

    public void SetVolumeMarker(GameObject volumeMarker, AudioVolumStates volume)
    {
        RectTransform markerRect = volumeMarker.GetComponent<RectTransform>();

        float step = 51.5f;
        float x = 0f;

        switch (volume)
        {
            case AudioVolumStates._0_percent:
                x = -5f * step;
                break;
            case AudioVolumStates._10_percent:
                x = -4f * step;
                break;
            case AudioVolumStates._20_percent:
                x = -3f * step;
                break;
            case AudioVolumStates._30_percent:
                x = -2f * step;
                break;
            case AudioVolumStates._40_percent:
                x = -1f * step;
                break;
            case AudioVolumStates._50_percent:
                x = 0f;
                break;
            case AudioVolumStates._60_percent:
                x = 1f * step;
                break;
            case AudioVolumStates._70_percent:
                x = 2f * step;
                break;
            case AudioVolumStates._80_percent:
                x = 3f * step;
                break;
            case AudioVolumStates._90_percent:
                x = 4f * step;
                break;
            case AudioVolumStates._100_percent:
                x = 5f * step;
                break;
        }

        markerRect.anchoredPosition = new Vector2(x, markerRect.anchoredPosition.y);
    }

    #endregion
}

[Serializable]
public class SettingData
{
    //General
    [Header("General")]
    public Languages currentLanguage;
    public TextSpeed currentTextSpeed;
    public StepDisplay currentStepDisplay;
    public CameraMotion currentCameraMotion;
    public RevertedCameraMotion currentRevertedCameraMotion;
    public SkipIntro currentSkipIntro;

    //Audio
    [Header("Volume Parameters")]
    public AudioVolumStates volume_Master;

    public AudioVolumStates volume_3DEnviroment;
    public AudioVolumStates volume_Weather;
    public AudioVolumStates volume_Player;
    public AudioVolumStates volume_UI;
    public AudioVolumStates volume_Dialogue;
}
