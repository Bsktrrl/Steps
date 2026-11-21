
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        if (currentMenuCategorySelected == category && IsCategoryOpen(category))
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

        Player_Statistics_Manager.Active?.FlushStatistics();
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

    internal void ShiftTab(EventSystem eventSystem, bool left)
    {
        bool inInfoMenu = IsInfoCategory(currentMenuCategorySelected);
        if (!IsTabInMenu(currentMenuCategorySelected, inInfoMenu))
            return;

        GameObject firstTab = inInfoMenu
            ? (infoManager ? infoManager.FirstTab : null)
            : settingsMenuButton;
        if (!firstTab || !firstTab.activeInHierarchy)
            return;

        Transform tabBar = firstTab.transform.parent;
        GameObject selected = eventSystem.currentSelectedGameObject;
        int startIndex = firstTab.transform.GetSiblingIndex();
        for (int i = 0; i < tabBar.childCount; i++)
        {
            Transform tab = tabBar.GetChild(i);
            if (!tab.TryGetComponent(out OptionCategoryButton category) ||
                !IsTabInMenu(category.optionCategory, inInfoMenu))
                continue;

            if (tab.gameObject == selected)
            {
                startIndex = i;
                break;
            }

            if (category.optionCategory == currentMenuCategorySelected)
                startIndex = i;
        }

        int direction = left ? -1 : 1;
        int tabCount = tabBar.childCount;
        for (int offset = 1; offset < tabCount; offset++)
        {
            Transform tab = tabBar.GetChild((startIndex + direction * offset + tabCount) % tabCount);
            if (tab.TryGetComponent(out OptionCategoryButton category) &&
                IsTabInMenu(category.optionCategory, inInfoMenu) &&
                tab.TryGetComponent(out Button button) && button.IsActive() && button.IsInteractable())
            {
                eventSystem.SetSelectedGameObject(tab.gameObject);
                return;
            }
        }
    }

    static bool IsTabInMenu(MenuCategories category, bool info) => info
        ? IsInfoCategory(category)
        : category is MenuCategories.Settings or MenuCategories.Controls or MenuCategories.Video or MenuCategories.Audio;

    static bool IsInfoCategory(MenuCategories category) =>
        category is MenuCategories.Info_Controls or MenuCategories.Info_Stats or MenuCategories.Info_Tablet;

    bool IsCategoryOpen(MenuCategories category) => category switch
    {
        MenuCategories.Settings => settingsMenuParent.activeInHierarchy,
        MenuCategories.Controls => controlsMenuParent.activeInHierarchy,
        MenuCategories.Video => videoMenuParent.activeInHierarchy,
        MenuCategories.Audio => audioMenuParent.activeInHierarchy,
        _ => IsInfoCategory(category)
    };
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
