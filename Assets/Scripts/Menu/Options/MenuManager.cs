
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

    [Header("Option Buttons")]
    public GameObject settingsMenuButton;
    public GameObject controlsMenuButton;
    public GameObject third_MenuButton;
    public GameObject forth_MenuButton;

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

        wardrobeMenuParent.SetActive(false);
    }
}

public enum MenuCategories
{
    None,

    //Options
    Settings,
    Controls,
    nr03,
    nr04,

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
    Icon,
    Number,
    NumberIcon,
    None
}
public enum CameraMotion
{
    Can,
    Cannot
}