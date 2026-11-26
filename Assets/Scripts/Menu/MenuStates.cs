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
        MainMenuManager menu = MainMenuManager.Instance;
        if (!menu || MapManager.Instance)
        {
            return;
        }

        switch (state)
        {
            case MenuState.None:
                menu.menuState = MenuState.None;
                break;

            case MenuState.Main_Menu:
                menu.menuState = MenuState.Main_Menu;
                break;
            case MenuState.Skin_Menu:
                menu.menuState = MenuState.Skin_Menu;
                break;
            case MenuState.Options_Menu:
                menu.menuState = MenuState.Options_Menu;
                break;
            case MenuState.Info_Menu:
                menu.menuState = MenuState.Info_Menu;
                break;

            case MenuState.Overworld_Menu:
                menu.menuState = MenuState.Overworld_Menu;
                break;
            case MenuState.Biome_Menu:
                menu.menuState = MenuState.Biome_Menu;
                break;

            case MenuState.Pause_Menu_Main:
                menu.menuState = MenuState.Pause_Menu_Main;
                break;
            case MenuState.Pause_Menu_Skins:
                menu.menuState = MenuState.Pause_Menu_Skins;
                break;
            case MenuState.Pause_Menu_Options:
                menu.menuState = MenuState.Pause_Menu_Options;
                break;

            case MenuState.NewGameWarningMessage:
                //MainMenuManager.Instance.menuState = MenuState.Main_Menu;
                break;
            case MenuState.NewGameWarningMessage_No:
                //MainMenuManager.Instance.menuState = MenuState.Main_Menu;
                break;


            case MenuState.RegionMenu_HUB:
                menu.menuState = MenuState.RegionMenu_HUB;
                break;
            case MenuState.RegionMenu_Water:
                menu.menuState = MenuState.RegionMenu_Water;
                break;
            case MenuState.RegionMenu_Sand:
                menu.menuState = MenuState.RegionMenu_Sand;
                break;
            case MenuState.RegionMenu_Ice:
                menu.menuState = MenuState.RegionMenu_Ice;
                break;
            case MenuState.RegionMenu_Lava:
                menu.menuState = MenuState.RegionMenu_Lava;
                break;
            case MenuState.RegionMenu_Swamp:
                menu.menuState = MenuState.RegionMenu_Swamp;
                break;
            case MenuState.RegionMenu_Metal:
                menu.menuState = MenuState.RegionMenu_Metal;
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

    RegionMenu_Water,
    RegionMenu_Sand,
    RegionMenu_Ice,
    RegionMenu_Lava,
    RegionMenu_Swamp,
    RegionMenu_Metal,
    RegionMenu_HUB,

    Info_Menu,
    Pause_Menu_Info,
}
