using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [Header("MenusOnMenuState")]
    public GameObject mainMenu_Parent;
    public GameObject overworldMenu_Parent;
    [SerializeField] GameObject infoMenu_Parent;
    [SerializeField] GameObject settingsMenu_Parent;

    [Header("Menu State")]
    public MenuState menuState;


    //--------------------


    //private void Awake()
    //{
    //    Menu_Main();
    //    menuState = MenuState.Main_Menu;
    //}
    private void Start()
    {
        if (DataManager.Instance.menuState_Store == MenuState.None)
        {
            menuState = MenuState.Main_Menu;

            Menu_Main();
        }
    }


    //--------------------


    private void OnEnable()
    {
        MenuStates.menuState_isChanged += MenusOnMenuState;
        DataManager.Action_dataHasLoaded += LoadPlayerStats;
        DataManager.Action_dataHasLoaded += SetMenu;
    }

    private void OnDisable()
    {
        MenuStates.menuState_isChanged -= MenusOnMenuState;
        DataManager.Action_dataHasLoaded -= LoadPlayerStats;
        DataManager.Action_dataHasLoaded -= SetMenu;
    }


    //--------------------


    void LoadPlayerStats()
    {
        SaveLoad_PlayerStats.Instance.LoadData();
    }

    void SetMenu()
    {
        menuState = DataManager.Instance.menuState_Store;

        MenusOnMenuState();
    }


    //--------------------


    void MenusOnMenuState()
    {
        switch (menuState)
        {
            case MenuState.None:
                Menu_Main();
                break;

            case MenuState.Main_Menu:
                Menu_Main();
                break;
            case MenuState.Overworld_Menu:
                Menu_Overworld();
                break;
            case MenuState.Info_Menu:
                Menu_Info();
                break;
            case MenuState.Settings_Menu:
                Menu_Settings();
                break;
            case MenuState.Biome_Menu:
                Menu_Biome();
                break;

            default:
                Menu_Main();
                break;
        }
    }


    //--------------------

    #region Open Menus

    void Menu_Main()
    {
        //Close any other menu
        HideAllMenus();

        //Open the MainMenu
        mainMenu_Parent.SetActive(true);
    }
    void Menu_Overworld()
    {
        //Close any other menu
        HideAllMenus();

        ////Open the OverworldMenu
        //overworldMenu_BiomesSelected_Parent.SetActive(true);
        //overworldMenu_BiomesBig_Parent.SetActive(false);
        overworldMenu_Parent.SetActive(true);
    }
    void Menu_Info()
    {
        //Close any other menu
        HideAllMenus();

        //Open the InfoMenu
        infoMenu_Parent.SetActive(true);
    }
    void Menu_Settings()
    {
        //Close any other menu
        HideAllMenus();

        //Open the InfoMenu
        settingsMenu_Parent.SetActive(true);
    }

    void Menu_Biome()
    {
        overworldMenu_Parent.SetActive(true);
    }


    //--------------------


    void HideAllMenus()
    {
        mainMenu_Parent.SetActive(false);
        infoMenu_Parent.SetActive(false);
        overworldMenu_Parent.SetActive(false);
        settingsMenu_Parent.SetActive(false);
    }

    #endregion

    #region Buttons

    public void To_MainMenu_Button_isPressed()
    {
        MenuStates.Instance.ChangeMenuState(MenuState.Main_Menu);
    }
    public void To_Overworld_Button_isPressed()
    {
        MenuStates.Instance.ChangeMenuState(MenuState.Overworld_Menu);
    }
    public void To_Info_Button_isPressed()
    {
        MenuStates.Instance.ChangeMenuState(MenuState.Info_Menu);
    }
    public void QuitButton_isPressed()
    {
        print("Quit Game");

        RememberCurrentlySelectedUIElement.Instance.SaveSelectedUIElement(RegionState.None, LevelState.None);

        menuState = MenuState.Main_Menu;

        Application.Quit();
    }

    #endregion
}