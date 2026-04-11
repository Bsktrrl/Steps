using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Sources (Pool)")]
    public List<AudioSource> audioSource_MenuTransition_List = new List<AudioSource>();

    [Header("Menu Sounds")]
    public AudioClip ui_MenuTransition;
    public AudioClip ui_ButtonNavigation;
    public AudioClip ui_ButtonPressed;
    public AudioClip ui_ButtonBackward;
    public AudioClip ui_ButtonCannot;
    public AudioClip ui_Wardrobe_Buy;
    public AudioClip ui_Wardrobe_Equip;
    public AudioClip ui_Options_Select;

    [Header("Menus")]
    public GameObject pauseMenu;

    private int nextAudioSourceIndex = 0;


    //--------------------


    private void OnEnable()
    {
        ButtonSound.Action_Button_MenuTransition_Forward += Play_Ui_MenuTransition_Forward;
        ButtonSound.Action_Button_MenuTransition_Back += Play_Ui_MenuTransition_Back;
        ButtonSound.Action_Button_Navigate += Play_Ui_ButtonNavigation;
        ButtonSound.Action_Button_PressSound += Play_Ui_ButtonPressed;
        ButtonSound.Action_Button_BackSound += Play_Ui_ButtonBackward;
        ButtonSound.Action_Button_Cannot += Play_Ui_ButtonCannot;
        ButtonSound.Action_Button_Buy += Play_Ui_Wardrobe_Buy;
        ButtonSound.Action_Button_Equip_On += Play_Ui_Wardrobe_Equip_On;
        ButtonSound.Action_Button_Equip_Off += Play_Ui_Wardrobe_Equip_Off;

        OptionSelectAnimation.Action_ButtonIsPressed += Play_Ui_Options_Select;

        PauseMenuManager.Action_openPauseMenu += Play_Ui_MenuTransition_Forward;
        MoveOutFromPauseMenu.Action_MoveOutFromPauseMenu += Play_Ui_MenuTransition_Back;
        PauseMenu_BackToGame_Button.Action_ClosePauseMenu += Play_Ui_MenuTransition_Back;

        PauseMenu_ExitLevel_Button.Action_ExitLevel += Play_Ui_ButtonPressed;
        QuitGame.Action_QuitGame += Play_Ui_ButtonPressed;

        Interactable_Pickup.Action_goalReached += FadeOutALLSound;
    }

    private void OnDisable()
    {
        ButtonSound.Action_Button_MenuTransition_Forward -= Play_Ui_MenuTransition_Forward;
        ButtonSound.Action_Button_MenuTransition_Back -= Play_Ui_MenuTransition_Back;
        ButtonSound.Action_Button_Navigate -= Play_Ui_ButtonNavigation;
        ButtonSound.Action_Button_PressSound -= Play_Ui_ButtonPressed;
        ButtonSound.Action_Button_BackSound -= Play_Ui_ButtonBackward;
        ButtonSound.Action_Button_Cannot -= Play_Ui_ButtonCannot;
        ButtonSound.Action_Button_Buy -= Play_Ui_Wardrobe_Buy;
        ButtonSound.Action_Button_Equip_On -= Play_Ui_Wardrobe_Equip_On;
        ButtonSound.Action_Button_Equip_Off -= Play_Ui_Wardrobe_Equip_Off;

        OptionSelectAnimation.Action_ButtonIsPressed -= Play_Ui_Options_Select;

        PauseMenuManager.Action_openPauseMenu -= Play_Ui_MenuTransition_Forward;
        MoveOutFromPauseMenu.Action_MoveOutFromPauseMenu -= Play_Ui_MenuTransition_Back;
        PauseMenu_BackToGame_Button.Action_ClosePauseMenu -= Play_Ui_MenuTransition_Back;

        PauseMenu_ExitLevel_Button.Action_ExitLevel -= Play_Ui_ButtonPressed;
        QuitGame.Action_QuitGame -= Play_Ui_ButtonPressed;

        Interactable_Pickup.Action_goalReached -= FadeOutALLSound;
    }


    //--------------------


    void PlaySound(List<AudioSource> audioSourcePool, AudioClip clip, float pitch, float volume)
    {
        if (clip == null || audioSourcePool == null || audioSourcePool.Count == 0)
            return;

        //if (pauseMenu != null && !pauseMenu.activeInHierarchy)
        //    return;

        AudioSource selectedSource = null;
        int poolCount = audioSourcePool.Count;

        // Start searching from the next index, so usage rotates through the pool
        for (int i = 0; i < poolCount; i++)
        {
            int index = (nextAudioSourceIndex + i) % poolCount;
            AudioSource source = audioSourcePool[index];

            if (source == null)
                continue;

            if (!source.isPlaying)
            {
                selectedSource = source;
                nextAudioSourceIndex = (index + 1) % poolCount;
                break;
            }
        }

        // If all sources are busy, fall back to the next valid one in the pool
        if (selectedSource == null)
        {
            for (int i = 0; i < poolCount; i++)
            {
                int index = (nextAudioSourceIndex + i) % poolCount;
                AudioSource source = audioSourcePool[index];

                if (source == null)
                    continue;

                selectedSource = source;
                nextAudioSourceIndex = (index + 1) % poolCount;
                break;
            }
        }

        if (selectedSource == null)
            return;

        selectedSource.pitch = pitch;
        selectedSource.volume = volume;

        // PlayOneShot does not permanently assign the clip to the AudioSource,
        // so the source remains "empty" and ready for reuse.
        selectedSource.PlayOneShot(clip);
    }


    //--------------------


    void Play_Ui_MenuTransition_Forward()
    {
        PlaySound(audioSource_MenuTransition_List, ui_MenuTransition, 1f, 1f);
        //print("1.1. Play_Ui_MenuTransition_Forward");
    }
    void Play_Ui_MenuTransition_Back()
    {
        PlaySound(audioSource_MenuTransition_List, ui_MenuTransition, 0.8f, 1f);
        //print("1.2 Play_Ui_MenuTransition_Back");
    }

    void Play_Ui_ButtonNavigation()
    {
        PlaySound(audioSource_MenuTransition_List, ui_ButtonNavigation, Random.Range(0.99f, 1.01f), 0.5f);
        //print("2. Play_Ui_ButtonNavigation");
    }

    void Play_Ui_ButtonPressed()
    {
        PlaySound(audioSource_MenuTransition_List, ui_ButtonPressed, 1f, 1f);
        //print("3. Play_Ui_ButtonPressed");
    }

    void Play_Ui_ButtonBackward()
    {
        PlaySound(audioSource_MenuTransition_List, ui_ButtonBackward, 1f, 1f);
        //print("4. Play_Ui_ButtonBackward");
    }

    void Play_Ui_ButtonCannot()
    {
        PlaySound(audioSource_MenuTransition_List, ui_ButtonCannot, 1f, 1f);
        //print("5. Play_Ui_ButtonCannot");
    }

    void Play_Ui_Wardrobe_Buy()
    {
        PlaySound(audioSource_MenuTransition_List, ui_Wardrobe_Buy, 1f, 1f);
        //print("6. Play_Ui_Wardrobe_Buy");
    }

    void Play_Ui_Wardrobe_Equip_On()
    {
        PlaySound(audioSource_MenuTransition_List, ui_Wardrobe_Equip, 1f, 1f);
        //print("7. Play_Ui_Wardrobe_Equip");
    }

    void Play_Ui_Wardrobe_Equip_Off()
    {
        PlaySound(audioSource_MenuTransition_List, ui_Wardrobe_Equip, 0.8f, 1f);
        //print("8. Play_Ui_Wardrobe_Equip_Off");
    }
    void Play_Ui_Options_Select()
    {
        PlaySound(audioSource_MenuTransition_List, ui_Options_Select, 1f, 0.5f);
        //print("9. Play_Ui_Options_Select");
    }


    //--------------------


    void FadeOutALLSound()
    {
        //Make functionalty to fade out all sounds, for exiting levels

    }
}