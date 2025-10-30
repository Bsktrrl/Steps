using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Sources")]
    public AudioSource audioSource_ButtonSounds;

    [Header("Menu Sounds")]
    public AudioClip sound_highlightedButton;
    public AudioClip sound_buttonSelect;
    public AudioClip sound_buttonCancel;


    //--------------------


    private void OnEnable()
    {
        RememberCurrentlySelectedUIElement.Action_ChangedSelectedUIElement += PlayHighlightedButton_Sound;
        Button_ToPress.Action_ButtonIsPressed += PlayButtonSelect_Sound;
        CancelPauseMenuByButtonPress.Action_ButtonIsCanceled += PlayButtonCancel_Sound;
    }
    private void OnDisable()
    {
        RememberCurrentlySelectedUIElement.Action_ChangedSelectedUIElement -= PlayHighlightedButton_Sound;
        Button_ToPress.Action_ButtonIsPressed -= PlayButtonSelect_Sound;
        CancelPauseMenuByButtonPress.Action_ButtonIsCanceled -= PlayButtonCancel_Sound;
    }


    //--------------------


    void PlayHighlightedButton_Sound()
    {
        if (audioSource_ButtonSounds)
        {
            audioSource_ButtonSounds.clip = sound_highlightedButton;
            audioSource_ButtonSounds.pitch = 1.75f;
            audioSource_ButtonSounds.volume = 0.75f;
            audioSource_ButtonSounds.Play();
        }
    }
    void PlayButtonSelect_Sound()
    {
        if (audioSource_ButtonSounds)
        {
            audioSource_ButtonSounds.clip = sound_buttonSelect;
            audioSource_ButtonSounds.pitch = 1;
            audioSource_ButtonSounds.volume = 1f;
            audioSource_ButtonSounds.Play();
        }
    }
    void PlayButtonCancel_Sound()
    {
        if (audioSource_ButtonSounds)
        {
            audioSource_ButtonSounds.clip = sound_buttonCancel;
            audioSource_ButtonSounds.pitch = 1;
            audioSource_ButtonSounds.volume = 1f;
            audioSource_ButtonSounds.Play();
        }
    }
}
