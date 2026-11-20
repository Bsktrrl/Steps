
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

    [Header("Info Menu")]
    [SerializeField] InfoManager infoManager;


    //--------------------


    public void ChangeMenuCategory(MenuCategories category)
    {
        if (IsInfoCategory(category) && currentMenuCategorySelected == category)
            return;

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

            case MenuCategories.Info_Controls:
            case MenuCategories.Info_Stats:
            case MenuCategories.Info_Tablet:
                if (infoManager)
                {
                    currentMenuCategorySelected = category;
                    infoManager.ShowPanel(category);
                }
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

        CloseInfoMenu();

        //MainMenuManager.Instance.newGameWarningMessage_Parent.SetActive(false);
    }

    internal void OpenInfoMenu()
    {
        if (!infoManager) return;

        ChangeMenuCategory(MenuCategories.Info_Controls);
        infoManager.SelectFirstTab();
    }

    internal void CloseInfoMenu()
    {
        if (!IsInfoCategory(currentMenuCategorySelected)) return;

        currentMenuCategorySelected = MenuCategories.None;
        if (infoManager)
            infoManager.ShowPanel(MenuCategories.None);
    }

    static bool IsInfoCategory(MenuCategories category) =>
        category is MenuCategories.Info_Controls or MenuCategories.Info_Stats or MenuCategories.Info_Tablet;
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
    Wardrobe,

    //Info
    Info_Controls,
    Info_Stats,
    Info_Tablet
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

    //Audio
    Audio_Master,
    Audio_Enviroment,
    Audio_Weather,
    Audio_Player,
    Audio_UI,
    Audio_Dialogue,

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

public enum AudioVolumStates
{
    _0_percent,
    _10_percent,
    _20_percent,
    _30_percent,
    _40_percent,
    _50_percent,
    _60_percent,
    _70_percent,
    _80_percent,
    _90_percent,
    _100_percent,
}
