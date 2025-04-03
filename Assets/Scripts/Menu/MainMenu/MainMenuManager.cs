using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [Header("MenusOnMenuState")]
    [SerializeField] GameObject mainMenu_Parent;
    [SerializeField] GameObject overworldMenu_Parent;
    [SerializeField] GameObject overworldMenu_BiomesSelected_Parent;
    [SerializeField] GameObject overworldMenu_BiomesBig_Parent;
    [SerializeField] GameObject infoMenu_Parent;
    [SerializeField] GameObject settingsMenu_Parent;

    [SerializeField] GameObject Biome_0_Parent;
    [SerializeField] GameObject Biome_1_Parent;
    [SerializeField] GameObject Biome_2_Parent;
    [SerializeField] GameObject Biome_3_Parent;
    [SerializeField] GameObject Biome_4_Parent;
    [SerializeField] GameObject Biome_5_Parent;
    [SerializeField] GameObject Biome_6_Parent;

    [Header("Menu State")]
    public MenuState menuState;


    //--------------------


    private void Start()
    {
        menuState = MenuState.Main_Menu;
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

        //Open the OverworldMenu
        overworldMenu_BiomesSelected_Parent.SetActive(true);
        overworldMenu_BiomesBig_Parent.SetActive(false);
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

        Biome_0_Parent.SetActive(false);
        Biome_1_Parent.SetActive(false);
        Biome_2_Parent.SetActive(false);
        Biome_3_Parent.SetActive(false);
        Biome_4_Parent.SetActive(false);
        Biome_5_Parent.SetActive(false);
        Biome_6_Parent.SetActive(false);
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