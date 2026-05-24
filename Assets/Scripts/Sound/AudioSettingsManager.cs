using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsManager : Singleton<AudioSettingsManager>
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer soundMixer;

    [Header("Master Volume")]
    private const string volumeParameter_Master = "MasterVolume";

    private float masterVolume_BeforeFade = 1f;
    private bool masterVolume_FadeValueStored;

    [Header("Group Volumes")]
    private const string volumeParameter_Group_3DEnviroment = "3D_EnviromentalVolume";
    private const string volumeParameter_Group_Weather = "WeatherVolume";
    private const string volumeParameter_Group_Player = "PlayerVolume";
    private const string volumeParameter_Group_UI = "UIVolume";
    private const string volumeParameter_Group_Dialogue = "DialogueVolume";

    private static bool masterVolume_ShouldFadeUpAfterSceneLoad;


    //--------------------


    private void Awake()
    {
        SetMasteVolum_Fade(0f);
    }


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += SetVolume_FirstTime;
        DataManager.Action_dataHasLoaded += LoadAudioSettings;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= SetVolume_FirstTime;
        DataManager.Action_dataHasLoaded -= LoadAudioSettings;
    }


    //--------------------


    void SaveAudioSettings()
    {
        DataPersistanceManager.instance.SaveGame();
    }
    void LoadAudioSettings()
    {
        AudioVolumStates savedMasterVolume = DataManager.Instance.settingData_StoreList.volume_Master;

        if (!masterVolume_ShouldFadeUpAfterSceneLoad)
        {
            SetMixerVolume(volumeParameter_Master, SetVolumeFromSettings(savedMasterVolume));
        }
        else
        {
            SetMasteVolum_Fade(0f);
        }

        SettingsManager.Instance.SetVolumeMarker(
            SettingsManager.Instance.marker_MasterVolume,
            savedMasterVolume
        );

        Set_Enviroment_GroupVolume(DataManager.Instance.settingData_StoreList.volume_3DEnviroment);
        Set_Weather_GroupVolume(DataManager.Instance.settingData_StoreList.volume_Weather);
        Set_Player_GroupVolume(DataManager.Instance.settingData_StoreList.volume_Player);
        Set_UI_GroupVolume(DataManager.Instance.settingData_StoreList.volume_UI);
        Set_Dialogue_GroupVolume(DataManager.Instance.settingData_StoreList.volume_Dialogue);
    }

    //--------------------


    void SetVolume_FirstTime()
    {
        if (!DataManager.Instance.oneTimeRunData_Store.startVolum_Values)
        {
            SetMasterVolume_Function(AudioVolumStates._100_percent);

            SetEnviromentGroupVolume_Function(AudioVolumStates._50_percent);
            SetWeatherGroupVolume_Function(AudioVolumStates._50_percent);
            SetPlayerGroupVolume_Function(AudioVolumStates._50_percent);
            SetUIGroupVolume_Function(AudioVolumStates._50_percent);
            SetDialogueGroupVolume_Function(AudioVolumStates._50_percent);

            DataManager.Instance.oneTimeRunData_Store.startVolum_Values = true;
            SaveAudioSettings();
        }
    }


    //--------------------


    float SetVolumeFromSettings(AudioVolumStates volumeSettings)
    {
        switch (volumeSettings)
        {
            case AudioVolumStates._0_percent:
                return 0f;
            case AudioVolumStates._10_percent:
                return 0.25f;
            case AudioVolumStates._20_percent:
                return 0.5f;
            case AudioVolumStates._30_percent:
                return 0.75f;
            case AudioVolumStates._40_percent:
                return 1f;
            case AudioVolumStates._50_percent:
                return 1.25f;
            case AudioVolumStates._60_percent:
                return 1.5f;
            case AudioVolumStates._70_percent:
                return 1.75f;
            case AudioVolumStates._80_percent:
                return 2f;
            case AudioVolumStates._90_percent:
                return 2.25f;
            case AudioVolumStates._100_percent:
                return 2.5f;

            default:
                return 0f;
        }
    }


    public void Set_Master_Volume(AudioVolumStates volumeSettings)
    {
        if (DataManager.Instance.oneTimeRunData_Store.startVolum_Values)
            SetMasterVolume_Function(volumeSettings);
    }
    void SetMasterVolume_Function(AudioVolumStates volumeSettings)
    {
        SetMixerVolume(volumeParameter_Master, SetVolumeFromSettings(volumeSettings));
        DataManager.Instance.settingData_StoreList.volume_Master = volumeSettings;
        SettingsManager.Instance.SetVolumeMarker(SettingsManager.Instance.marker_MasterVolume, DataManager.Instance.settingData_StoreList.volume_Master);

        SaveAudioSettings();
    }


    public void Set_Enviroment_GroupVolume(AudioVolumStates volumeSettings)
    {
        if (DataManager.Instance.oneTimeRunData_Store.startVolum_Values)
            SetEnviromentGroupVolume_Function(volumeSettings);
    }
    void SetEnviromentGroupVolume_Function(AudioVolumStates volumeSettings)
    {
        SetMixerVolume(volumeParameter_Group_3DEnviroment, SetVolumeFromSettings(volumeSettings));
        DataManager.Instance.settingData_StoreList.volume_3DEnviroment = volumeSettings;
        SettingsManager.Instance.SetVolumeMarker(SettingsManager.Instance.marker_EnviromentVolume, DataManager.Instance.settingData_StoreList.volume_3DEnviroment);

        SaveAudioSettings();
    }


    public void Set_Weather_GroupVolume(AudioVolumStates volumeSettings)
    {
        if (DataManager.Instance.oneTimeRunData_Store.startVolum_Values)
            SetWeatherGroupVolume_Function(volumeSettings);
    }
    void SetWeatherGroupVolume_Function(AudioVolumStates volumeSettings)
    {
        SetMixerVolume(volumeParameter_Group_Weather, SetVolumeFromSettings(volumeSettings));
        DataManager.Instance.settingData_StoreList.volume_Weather = volumeSettings;
        SettingsManager.Instance.SetVolumeMarker(SettingsManager.Instance.marker_WeatherVolume, DataManager.Instance.settingData_StoreList.volume_Weather);

        SaveAudioSettings();
    }


    public void Set_Player_GroupVolume(AudioVolumStates volumeSettings)
    {
        if (DataManager.Instance.oneTimeRunData_Store.startVolum_Values)
            SetPlayerGroupVolume_Function(volumeSettings);
    }
    void SetPlayerGroupVolume_Function(AudioVolumStates volumeSettings)
    {
        SetMixerVolume(volumeParameter_Group_Player, SetVolumeFromSettings(volumeSettings));
        DataManager.Instance.settingData_StoreList.volume_Player = volumeSettings;
        SettingsManager.Instance.SetVolumeMarker(SettingsManager.Instance.marker_PlayerVolume, DataManager.Instance.settingData_StoreList.volume_Player);

        SaveAudioSettings();
    }


    public void Set_UI_GroupVolume(AudioVolumStates volumeSettings)
    {
        if (DataManager.Instance.oneTimeRunData_Store.startVolum_Values)
            SetUIGroupVolume_Function(volumeSettings);
    }
    void SetUIGroupVolume_Function(AudioVolumStates volumeSettings)
    {
        SetMixerVolume(volumeParameter_Group_UI, SetVolumeFromSettings(volumeSettings));
        DataManager.Instance.settingData_StoreList.volume_UI = volumeSettings;
        SettingsManager.Instance.SetVolumeMarker(SettingsManager.Instance.marker_UIVolume, DataManager.Instance.settingData_StoreList.volume_UI);

        SaveAudioSettings();
    }


    public void Set_Dialogue_GroupVolume(AudioVolumStates volumeSettings)
    {
        if (DataManager.Instance.oneTimeRunData_Store.startVolum_Values)
            SetDialogueGroupVolume_Function(volumeSettings);
    }
    void SetDialogueGroupVolume_Function(AudioVolumStates volumeSettings)
    {
        SetMixerVolume(volumeParameter_Group_Dialogue, SetVolumeFromSettings(volumeSettings));
        DataManager.Instance.settingData_StoreList.volume_Dialogue = volumeSettings;
        SettingsManager.Instance.SetVolumeMarker(SettingsManager.Instance.marker_DialogueVolume, DataManager.Instance.settingData_StoreList.volume_Dialogue);

        SaveAudioSettings();
    }


    //--------------------


    private void SetMixerVolume(string parameterName, float volume)
    {
        if (soundMixer == null)
        {
            Debug.LogWarning("AudioSettingsManager: soundMixer is missing.");
            return;
        }

        float volumeInDecibels;

        if (volume <= 0.0001f)
            volumeInDecibels = -80f;
        else
            volumeInDecibels = Mathf.Log10(volume) * 20f;

        bool success = soundMixer.SetFloat(parameterName, volumeInDecibels);

        if (!success)
        {
            Debug.LogWarning("AudioSettingsManager: Could not find exposed mixer parameter: " + parameterName);
        }
    }


    //--------------------


    #region SoundFade

    public void MarkMasterVolumeShouldFadeUpAfterSceneLoad()
    {
        masterVolume_ShouldFadeUpAfterSceneLoad = true;
    }

    public bool ConsumeMasterVolumeShouldFadeUpAfterSceneLoad()
    {
        bool shouldFade = masterVolume_ShouldFadeUpAfterSceneLoad;
        masterVolume_ShouldFadeUpAfterSceneLoad = false;
        return shouldFade;
    }

    public void PrepareMasterVolumeFadeDown()
    {
        masterVolume_BeforeFade = GetCurrentMasterVolumeFromSettings();
        masterVolume_FadeValueStored = true;
    }

    public void PrepareMasterVolumeFadeUp()
    {
        masterVolume_BeforeFade = GetCurrentMasterVolumeFromSettings();
        masterVolume_FadeValueStored = true;

        SetMasteVolum_Fade(0f);
    }

    public void FadeMasterVolumeDown(float t)
    {
        t = Mathf.Clamp01(t);
        float volume = Mathf.Lerp(masterVolume_BeforeFade, 0f, t);

        SetMasteVolum_Fade(volume);
    }

    public void FadeMasterVolumeUp(float t)
    {
        t = Mathf.Clamp01(t);
        float volume = Mathf.Lerp(0f, masterVolume_BeforeFade, t);

        SetMasteVolum_Fade(volume);
    }

    public void FinishMasterVolumeFadeDown()
    {
        SetMasteVolum_Fade(0f);
        MarkMasterVolumeShouldFadeUpAfterSceneLoad();
    }

    public void FinishMasterVolumeFadeUp()
    {
        SetMasteVolum_Fade(masterVolume_BeforeFade);
        masterVolume_FadeValueStored = false;
        masterVolume_ShouldFadeUpAfterSceneLoad = false;
    }

    public void SetMasteVolum_Fade(float _volum)
    {
        SetMixerVolume(volumeParameter_Master, _volum);
    }

    private float GetCurrentMasterVolumeFromSettings()
    {
        if (DataManager.Instance == null)
            return 1f;

        return SetVolumeFromSettings(DataManager.Instance.settingData_StoreList.volume_Master);
    }

    #endregion
}
