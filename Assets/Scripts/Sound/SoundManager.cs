using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Sources")]
    public AudioSource audioSource_HighlightedButton;
    public AudioSource audioSource_ButtonSelect;
    public AudioSource audioSource_ButtonCancel;

    [Header("Menu Sounds")]
    public AudioClip sound_highlightedButton;
    public AudioClip sound_buttonSelect;
    public AudioClip sound_buttonCancel;


    [Header("Menus")]
    public GameObject pauseMenu;


    //--------------------


    private void OnEnable()
    {
        Button_ToPress.Action_ButtonIsPressed += PlayButtonSelect_Sound;
        CancelPauseMenuByButtonPress.Action_ButtonIsCanceled += PlayButtonCancel_Sound;

        Menu_KeyInputs.Action_MenuNavigationUp_isPressed += PlayHighlightedButton_Sound;
        Menu_KeyInputs.Action_MenuNavigationDown_isPressed += PlayHighlightedButton_Sound;
        Menu_KeyInputs.Action_MenuNavigationLeft_isPressed += PlayHighlightedButton_Sound;
        Menu_KeyInputs.Action_MenuNavigationRight_isPressed += PlayHighlightedButton_Sound;
    }
    private void OnDisable()
    {
        Button_ToPress.Action_ButtonIsPressed -= PlayButtonSelect_Sound;
        CancelPauseMenuByButtonPress.Action_ButtonIsCanceled -= PlayButtonCancel_Sound;

        Menu_KeyInputs.Action_MenuNavigationUp_isPressed -= PlayHighlightedButton_Sound;
        Menu_KeyInputs.Action_MenuNavigationDown_isPressed -= PlayHighlightedButton_Sound;
        Menu_KeyInputs.Action_MenuNavigationLeft_isPressed -= PlayHighlightedButton_Sound;
        Menu_KeyInputs.Action_MenuNavigationRight_isPressed -= PlayHighlightedButton_Sound;
    }


    //--------------------


    void PlaySound(AudioSource audioSource, AudioClip clip, float pitch, float volume)
    {
        if (audioSource && ((pauseMenu && pauseMenu.activeInHierarchy) || !pauseMenu))
        {
            audioSource.clip = clip;
            audioSource.pitch = pitch;
            audioSource.volume = volume;

            audioSource.Play();
        }
    }


    //--------------------


    void PlayHighlightedButton_Sound()
    {
        PlaySound(audioSource_HighlightedButton, sound_highlightedButton, 1.75f, 0.75f);
    }
    void PlayButtonSelect_Sound()
    {
        PlaySound(audioSource_ButtonSelect, sound_buttonSelect, 1, 1);
    }
    void PlayButtonCancel_Sound()
    {
        PlaySound(audioSource_ButtonCancel, sound_buttonCancel, 1, 1);
    }
}
