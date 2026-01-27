using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuStates : Singleton<MenuStates>
{
    public static event Action menuState_isChanged;


    //--------------------


    public void ChangeMenuState(MenuState state)
    {
        if (FindObjectOfType<MapManager>())
        {
            return;
        }

        switch (state)
        {
            case MenuState.None:
                MainMenuManager.Instance.menuState = MenuState.None;
                break;

            case MenuState.Main_Menu:
                MainMenuManager.Instance.menuState = MenuState.Main_Menu;
                break;
            case MenuState.Skin_Menu:
                MainMenuManager.Instance.menuState = MenuState.Skin_Menu;
                break;
            case MenuState.Options_Menu:
                MainMenuManager.Instance.menuState = MenuState.Options_Menu;
                break;

            case MenuState.Overworld_Menu:
                MainMenuManager.Instance.menuState = MenuState.Overworld_Menu;
                break;
            case MenuState.Biome_Menu:
                MainMenuManager.Instance.menuState = MenuState.Biome_Menu;
                break;

            case MenuState.Pause_Menu_Main:
                MainMenuManager.Instance.menuState = MenuState.Pause_Menu_Main;
                break;
            case MenuState.Pause_Menu_Skins:
                MainMenuManager.Instance.menuState = MenuState.Pause_Menu_Skins;
                break;
            case MenuState.Pause_Menu_Options:
                MainMenuManager.Instance.menuState = MenuState.Pause_Menu_Options;
                break;

            case MenuState.NewGameWarningMessage:
                //MainMenuManager.Instance.menuState = MenuState.Main_Menu;
                break;
            case MenuState.NewGameWarningMessage_No:
                //MainMenuManager.Instance.menuState = MenuState.Main_Menu;
                break;

            default:
                break;
        }

        if (state != MenuState.NewGameWarningMessage && state != MenuState.NewGameWarningMessage_No)
        {
            SaveMenuState(state);
            MenuState_isChanged_Invoke();
        }
    }

    public void MenuState_isChanged_Invoke()
    {
        menuState_isChanged?.Invoke();
    }

    public void SaveMenuState(MenuState state)
    {
        DataManager.Instance.menuState_Store = state;
        DataPersistanceManager.instance.SaveGame();
    }
}

public enum MenuState
{
    None,

    Main_Menu,
    Overworld_Menu,

    Biome_Menu,

    NewGame_Menu,
    Pause_Menu_Main,

    Skin_Menu,
    Options_Menu,
    Pause_Menu_Options,
    Pause_Menu_Skins,

    NewGameWarningMessage,
    NewGameWarningMessage_No,
}
