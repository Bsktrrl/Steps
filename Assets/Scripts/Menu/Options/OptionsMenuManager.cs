
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuManager : Singleton<OptionsMenuManager>
{
    [Header("Options Categories")]
    public OptionsMenuCategories currentOptionsMenuCategorySelected;

    [Header("Options Menues")]
    public GameObject settingsMenuParent;
    public GameObject controlsMenuParent;


    //--------------------


    public void ChangeOptionCategory(OptionsMenuCategories category)
    {
        HideAllMenus();

        switch (category)
        {
            case OptionsMenuCategories.Settings:
                currentOptionsMenuCategorySelected = OptionsMenuCategories.Settings;
                settingsMenuParent.SetActive(true);
                break;
            case OptionsMenuCategories.Controls:
                currentOptionsMenuCategorySelected = OptionsMenuCategories.Controls;
                controlsMenuParent.SetActive(true);
                break;

            default:
                currentOptionsMenuCategorySelected = OptionsMenuCategories.Settings;
                settingsMenuParent.SetActive(true);
                break;
        }
    }
    void HideAllMenus()
    {
        settingsMenuParent.SetActive(false);
        controlsMenuParent.SetActive(false);
    }
}

public enum OptionsMenuCategories
{
    Settings,
    Controls,
}

public enum CategoryState
{
    Setting,
    Controls,

}
public enum SettingState
{
    None,

    Settings_Language,
    Settings_TextSpeed,

    Controls_Movement,
    Controls_CameraRotation,
    Controls_Respawn,
    Controls_PauseMenu,
    Controls_Ascend,
    Controls_Descend,
    Controls_CeilingGrab,
    Controls_GrapplingHook,

    BackButton,
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