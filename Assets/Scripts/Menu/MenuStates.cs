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

            case MenuState.Biome_0:
                MainMenuManager.Instance.menuState = MenuState.Biome_0;
                break;
            case MenuState.Biome_1:
                MainMenuManager.Instance.menuState = MenuState.Biome_1;
                break;
            case MenuState.Biome_2:
                MainMenuManager.Instance.menuState = MenuState.Biome_2;
                break;
            case MenuState.Biome_3:
                MainMenuManager.Instance.menuState = MenuState.Biome_3;
                break;
            case MenuState.Biome_4:
                MainMenuManager.Instance.menuState = MenuState.Biome_4;
                break;
            case MenuState.Biome_5:
                MainMenuManager.Instance.menuState = MenuState.Biome_5;
                break;
            case MenuState.Biome_6:
                MainMenuManager.Instance.menuState = MenuState.Biome_6;
                break;
            case MenuState.Biome_7:
                MainMenuManager.Instance.menuState = MenuState.Biome_7;
                break;
            case MenuState.Biome_8:
                MainMenuManager.Instance.menuState = MenuState.Biome_8;
                break;
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

    Biome_0,
    Biome_1,
    Biome_2,
    Biome_3,
    Biome_4,
    Biome_5,
    Biome_6,
    Biome_7,
    Biome_8,
}
