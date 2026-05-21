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
    private const string volumeParameter_Group_Player = "PlayerVolume";
    private const string volumeParameter_Group_3DEnviroment = "3D_EnviromentalVolume";
    private const string volumeParameter_Group_UI = "UIVolume";
    private const string volumeParameter_Group_Weather = "WeatherVolume";

    


    //--------------------


    public void Set_Master_Volume(float volume)
    {
        SetMixerVolume(volumeParameter_Master, volume);
    }


    public void Set_Player_GroupVolume(float volume)
    {
        SetMixerVolume(volumeParameter_Group_Player, volume);
    }
    public void Set_3DEnviroment_GroupVolume(float volume)
    {
        SetMixerVolume(volumeParameter_Group_3DEnviroment, volume);
    }
    public void Set_UI_GroupVolume(float volume)
    {
        SetMixerVolume(volumeParameter_Group_UI, volume);
    }
    public void Set_Weather_GroupVolume(float volume)
    {
        SetMixerVolume(volumeParameter_Group_Weather, volume);
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
