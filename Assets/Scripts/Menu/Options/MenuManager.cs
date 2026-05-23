
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    [Header("Options Categories")]
    public MenuCategories currentMenuCategorySelected;

    [Header("Options Menues")]
    public GameObject settingsMenuParent;
    public GameObject controlsMenuParent;
    public GameObject videoMenuParent;
    public GameObject audioMenuParent;

    [Header("Option Buttons")]
    public GameObject settingsMenuButton;
    public GameObject controlsMenuButton;
    public GameObject video_MenuButton;
    public GameObject audio_MenuButton;

    [Header("Skin Menues")]
    public GameObject wardrobeMenuParent;

    [Header("Skin Buttons")]
    public GameObject wardrobe_StartButton;


    //--------------------


    public void ChangeMenuCategory(MenuCategories category)
    {
        HideAllMenus();

        switch (category)
        {
            case MenuCategories.Settings:
                currentMenuCategorySelected = MenuCategories.Settings;
                settingsMenuParent.SetActive(true);
                break;
            case MenuCategories.Controls:
                currentMenuCategorySelected = MenuCategories.Controls;
                controlsMenuParent.SetActive(true);
                break;
            case MenuCategories.Video:
                currentMenuCategorySelected = MenuCategories.Video;
                videoMenuParent.SetActive(true);
                break;
            case MenuCategories.Audio:
                currentMenuCategorySelected = MenuCategories.Audio;
                audioMenuParent.SetActive(true);
                break;

            case MenuCategories.Wardrobe:
                currentMenuCategorySelected = MenuCategories.Wardrobe;
                wardrobeMenuParent.SetActive(true);
                break;

            default:
                break;
        }
    }
    public void HideAllMenus()
    {
        settingsMenuParent.SetActive(false);
        controlsMenuParent.SetActive(false);
        videoMenuParent.SetActive(false);
        audioMenuParent.SetActive(false);

        wardrobeMenuParent.SetActive(false);

        //MainMenuManager.Instance.newGameWarningMessage_Parent.SetActive(false);
    }
}

public enum MenuCategories
{
    None,

    //Options
    Settings,
    Controls,
    Video,
    Audio,

    //Skins
    Wardrobe
}

public enum CategoryState
{
    Setting,
    Controls,

    Wardrobe,
    Shop,
}
public enum SettingState
{
    None,

    //Settings
    Settings_Language,
    Settings_TextSpeed,

    //Controls
    Controls_Movement,
    Controls_CameraRotation,
    Controls_Respawn,
    Controls_PauseMenu,
    Controls_Ascend,
    Controls_Descend,
    Controls_CeilingGrab,
    Controls_GrapplingHook,

    //Back
    BackButton,

    //Wardrobe
    Wardrobe_Blocks,
    Wardrobe_Hats,

    //Shop
    Shop_Blocks,

    //Settings
    Settings_StepDisplay,
    Settings_CameraMotion,
    Settings_RevertedCameraMotion,
    Settings_SkipLevelIntro,
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
public enum TextSpeed
{
    Medium,
    Fast,
    Slow
}
public enum StepDisplay
{
    Steps,
    Number,
    NumberSteps,
    None
}
public enum CameraMotion
{
    Can,
    Cannot
}
public enum RevertedCameraMotion
{
    Normal,
    Reverted
}
public enum SkipIntro
{
    Yes,
    No
}