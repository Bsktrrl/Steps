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
        DataManager.Action_dataHasLoaded += LoadAudioSettings;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= LoadAudioSettings;
    }


    //--------------------


    void LoadAudioSettings()
    {
        DataManager data = DataManager.Instance;
        SettingData settings = data.settingData_StoreList;
        if (!data.oneTimeRunData_Store.startVolum_Values)
        {
            settings.volume_Master = AudioVolumStates._100_percent;
            settings.volume_3DEnviroment = AudioVolumStates._50_percent;
            settings.volume_Weather = AudioVolumStates._50_percent;
            settings.volume_Player = AudioVolumStates._50_percent;
            settings.volume_UI = AudioVolumStates._50_percent;
            settings.volume_Dialogue = AudioVolumStates._50_percent;
            data.oneTimeRunData_Store.startVolum_Values = true;
            DataPersistanceManager.instance.SaveGame();
        }

        SettingsManager menu = SettingsManager.Instance;
        if (!masterVolume_FadeValueStored)
            SetMasteVolum_Fade(masterVolume_ShouldFadeUpAfterSceneLoad ? 0f : SetVolumeFromSettings(settings.volume_Master));
        menu.SetVolumeMarker(menu.marker_MasterVolume, settings.volume_Master);

        ApplyVolume(volumeParameter_Group_3DEnviroment, settings.volume_3DEnviroment, menu.marker_EnviromentVolume);
        ApplyVolume(volumeParameter_Group_Weather, settings.volume_Weather, menu.marker_WeatherVolume);
        ApplyVolume(volumeParameter_Group_Player, settings.volume_Player, menu.marker_PlayerVolume);
        ApplyVolume(volumeParameter_Group_UI, settings.volume_UI, menu.marker_UIVolume);
        ApplyVolume(volumeParameter_Group_Dialogue, settings.volume_Dialogue, menu.marker_DialogueVolume);
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
        ChangeVolume(ref DataManager.Instance.settingData_StoreList.volume_Master, volumeSettings,
            volumeParameter_Master, SettingsManager.Instance.marker_MasterVolume);
    }


    public void Set_Enviroment_GroupVolume(AudioVolumStates volumeSettings)
    {
        ChangeVolume(ref DataManager.Instance.settingData_StoreList.volume_3DEnviroment, volumeSettings,
            volumeParameter_Group_3DEnviroment, SettingsManager.Instance.marker_EnviromentVolume);
    }


    public void Set_Weather_GroupVolume(AudioVolumStates volumeSettings)
    {
        ChangeVolume(ref DataManager.Instance.settingData_StoreList.volume_Weather, volumeSettings,
            volumeParameter_Group_Weather, SettingsManager.Instance.marker_WeatherVolume);
    }


    public void Set_Player_GroupVolume(AudioVolumStates volumeSettings)
    {
        ChangeVolume(ref DataManager.Instance.settingData_StoreList.volume_Player, volumeSettings,
            volumeParameter_Group_Player, SettingsManager.Instance.marker_PlayerVolume);
    }


    public void Set_UI_GroupVolume(AudioVolumStates volumeSettings)
    {
        ChangeVolume(ref DataManager.Instance.settingData_StoreList.volume_UI, volumeSettings,
            volumeParameter_Group_UI, SettingsManager.Instance.marker_UIVolume);
    }


    public void Set_Dialogue_GroupVolume(AudioVolumStates volumeSettings)
    {
        ChangeVolume(ref DataManager.Instance.settingData_StoreList.volume_Dialogue, volumeSettings,
            volumeParameter_Group_Dialogue, SettingsManager.Instance.marker_DialogueVolume);
    }

    void ChangeVolume(ref AudioVolumStates storedVolume, AudioVolumStates volume, string parameter, GameObject marker)
    {
        if (!DataManager.Instance.oneTimeRunData_Store.startVolum_Values || storedVolume == volume)
            return;

        storedVolume = volume;
        ApplyVolume(parameter, volume, marker);
        DataPersistanceManager.instance.SaveSettings();
    }

    void ApplyVolume(string parameter, AudioVolumStates volume, GameObject marker)
    {
        SetMixerVolume(parameter, SetVolumeFromSettings(volume));
        SettingsManager.Instance.SetVolumeMarker(marker, volume);
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
