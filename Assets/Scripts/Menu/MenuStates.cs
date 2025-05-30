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
            case MenuState.Overworld_Menu:
                MainMenuManager.Instance.menuState = MenuState.Overworld_Menu;
                break;
            case MenuState.Info_Menu:
                MainMenuManager.Instance.menuState = MenuState.Info_Menu;
                break;
            case MenuState.Settings_Menu:
                MainMenuManager.Instance.menuState = MenuState.Settings_Menu;
                break;

            case MenuState.Biome_Menu:
                MainMenuManager.Instance.menuState = MenuState.Biome_Menu;
                break;

            case MenuState.Pause_Menu_Main:
                MainMenuManager.Instance.menuState = MenuState.Pause_Menu_Main;
                break;

            case MenuState.Pause_Menu_Settings:
                MainMenuManager.Instance.menuState = MenuState.Pause_Menu_Settings;
                break;

            case MenuState.Pause_Menu_Info:
                MainMenuManager.Instance.menuState = MenuState.Pause_Menu_Info;
                break;

            default:
                break;
        }

        SaveMenuState(state);
        MenuState_isChanged_Invoke();
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
    Info_Menu,
    Settings_Menu,

    Biome_Menu,

    NewGame_Menu,
    Pause_Menu_Main,
    Pause_Menu_Settings,
    Pause_Menu_Info
}
