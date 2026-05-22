using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsManager : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer soundMixer;

    [Header("Master Volume")]
    private const string volumeParameter_Master = "MasterVolume";

    [Header("Group Volumes")]
    private const string volumeParameter_Group_3DEnviroment = "3D_EnviromentalVolume";
    private const string volumeParameter_Group_Weather = "WeatherVolume";
    private const string volumeParameter_Group_Player = "PlayerVolume";
    private const string volumeParameter_Group_UI = "UIVolume";
    private const string volumeParameter_Group_Dialogue = "DialogueVolume";


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


    void SaveAudioSettings()
    {
        DataPersistanceManager.instance.SaveGame();
    }
    void LoadAudioSettings()
    {
        Set_Master_Volume(DataManager.Instance.settingData_StoreList.volume_Master);
        Set_3DEnviroment_GroupVolume(DataManager.Instance.settingData_StoreList.volume_3DEnviroment);
        Set_Weather_GroupVolume(DataManager.Instance.settingData_StoreList.volume_Weather);
        Set_Player_GroupVolume(DataManager.Instance.settingData_StoreList.volume_Player);
        Set_UI_GroupVolume(DataManager.Instance.settingData_StoreList.volume_UI);
        Set_Dialogue_GroupVolume(DataManager.Instance.settingData_StoreList.volume_Dialogue);
    }


    //--------------------


    public void Set_Master_Volume(float volume)
    {
        SetMixerVolume(volumeParameter_Master, volume);
        DataManager.Instance.settingData_StoreList.volume_Master = volume;

        SaveAudioSettings();
    }


    public void Set_3DEnviroment_GroupVolume(float volume)
    {
        SetMixerVolume(volumeParameter_Group_3DEnviroment, volume);
        DataManager.Instance.settingData_StoreList.volume_3DEnviroment = volume;

        SaveAudioSettings();
    }
    public void Set_Weather_GroupVolume(float volume)
    {
        SetMixerVolume(volumeParameter_Group_Weather, volume);
        DataManager.Instance.settingData_StoreList.volume_Weather = volume;

        SaveAudioSettings();
    }
    public void Set_Player_GroupVolume(float volume)
    {
        SetMixerVolume(volumeParameter_Group_Player, volume);
        DataManager.Instance.settingData_StoreList.volume_Player = volume;

        SaveAudioSettings();
    }
    public void Set_UI_GroupVolume(float volume)
    {
        SetMixerVolume(volumeParameter_Group_UI, volume);
        DataManager.Instance.settingData_StoreList.volume_UI = volume;

        SaveAudioSettings();
    }
    public void Set_Dialogue_GroupVolume(float volume)
    {
        SetMixerVolume(volumeParameter_Group_Dialogue, volume);
        DataManager.Instance.settingData_StoreList.volume_Dialogue = volume;

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
}
