using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [Header("MenusOnMenuState")]
    [SerializeField] GameObject mainMenu_Parent;
    [SerializeField] GameObject overworldMenu_Parent;
    [SerializeField] GameObject infoMenu_Parent;

    [Header("Menu State")]
    public MenuState menuState;


    //--------------------


    private void Start()
    {
        MenuStates.Instance.ChangeMenuState(MenuState.Main_Menu);
    }


    //--------------------


    private void OnEnable()
    {
        MenuStates.menuState_isChanged += MenusOnMenuState;
        DataManager.datahasLoaded += LoadPlayerStats;
    }

    private void OnDisable()
    {
        MenuStates.menuState_isChanged -= MenusOnMenuState;
        DataManager.datahasLoaded -= LoadPlayerStats;
    }


    //--------------------


    void LoadPlayerStats()
    {
        SaveLoad_PlayerStats.Instance.LoadData();
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

            default:
                break;
        }
    }
    void Menu_Main()
    {
        //Close any other menu
        infoMenu_Parent.SetActive(false);
        overworldMenu_Parent.SetActive(false);

        //Open the MainMenu
        mainMenu_Parent.SetActive(true);
    }
    void Menu_Overworld()
    {
        //Close any other menus
        infoMenu_Parent.SetActive(false);
        mainMenu_Parent.SetActive(false);

        //Open the OverworldMenu
        overworldMenu_Parent.SetActive(true);
    }
    void Menu_Info()
    {
        //Close any other menus
        mainMenu_Parent.SetActive(false);
        overworldMenu_Parent.SetActive(false);

        //Open the InfoMenu
        infoMenu_Parent.SetActive(true);
    }

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
        Application.Quit();
    }

    #endregion
}