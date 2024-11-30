using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [Header("MenusOnMenuState")]
    [SerializeField] GameObject mainMenu_Parent;
    [SerializeField] GameObject overworldMenu_Parent;
    [SerializeField] GameObject overworldMenu_BiomesSelected_Parent;
    [SerializeField] GameObject overworldMenu_BiomesBig_Parent;
    [SerializeField] GameObject infoMenu_Parent;

    [SerializeField] GameObject Biome_0_Parent;
    [SerializeField] GameObject Biome_1_Parent;
    [SerializeField] GameObject Biome_2_Parent;
    [SerializeField] GameObject Biome_3_Parent;
    [SerializeField] GameObject Biome_4_Parent;
    [SerializeField] GameObject Biome_5_Parent;
    [SerializeField] GameObject Biome_6_Parent;
    [SerializeField] GameObject Biome_7_Parent;
    [SerializeField] GameObject Biome_8_Parent;

    [Header("Menu State")]
    public MenuState menuState;


    //--------------------


    private void OnEnable()
    {
        MenuStates.menuState_isChanged += MenusOnMenuState;
        DataManager.Action_dataHasLoaded += LoadPlayerStats;
        DataManager.Action_dataHasLoaded += SetMenu;
        OverworldMenu.OverworldButton_isPressed += MenusOnMenuState;
    }

    private void OnDisable()
    {
        MenuStates.menuState_isChanged -= MenusOnMenuState;
        DataManager.Action_dataHasLoaded -= LoadPlayerStats;
        DataManager.Action_dataHasLoaded -= SetMenu;
        OverworldMenu.OverworldButton_isPressed -= MenusOnMenuState;
    }


    //--------------------


    void LoadPlayerStats()
    {
        SaveLoad_PlayerStats.Instance.LoadData();
    }

    void SetMenu()
    {
        if (DataManager.Instance.menuState_Store == MenuState.None)
            menuState = MenuState.Main_Menu;
        else
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

            case MenuState.Biome_0:
                Menu_Biome_0();
                break;
            case MenuState.Biome_1:
                Menu_Biome_1();
                break;
            case MenuState.Biome_2:
                Menu_Biome_2();
                break;
            case MenuState.Biome_3:
                Menu_Biome_3();
                break;
            case MenuState.Biome_4:
                Menu_Biome_4();
                break;
            case MenuState.Biome_5:
                Menu_Biome_5();
                break;
            case MenuState.Biome_6:
                Menu_Biome_6();
                break;
            case MenuState.Biome_7:
                Menu_Biome_7();
                break;
            case MenuState.Biome_8:
                Menu_Biome_8();
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

    void Menu_Biome_0()
    {
        //Close any other menu
        HideAllMenus();

        //Open the InfoMenu
        overworldMenu_Parent.SetActive(true);
        overworldMenu_BiomesSelected_Parent.SetActive(false);
        overworldMenu_BiomesBig_Parent.SetActive(true);
        Biome_0_Parent.SetActive(true);
    }
    void Menu_Biome_1()
    {
        //Close any other menu
        HideAllMenus();

        //Open the InfoMenu
        overworldMenu_Parent.SetActive(true);
        overworldMenu_BiomesSelected_Parent.SetActive(false);
        overworldMenu_BiomesBig_Parent.SetActive(true);
        Biome_1_Parent.SetActive(true);
    }
    void Menu_Biome_2()
    {
        //Close any other menu
        HideAllMenus();

        //Open the InfoMenu
        overworldMenu_Parent.SetActive(true);
        overworldMenu_BiomesSelected_Parent.SetActive(false);
        overworldMenu_BiomesBig_Parent.SetActive(true);
        Biome_2_Parent.SetActive(true);
    }
    void Menu_Biome_3()
    {
        //Close any other menu
        HideAllMenus();

        //Open the InfoMenu
        overworldMenu_Parent.SetActive(true);
        overworldMenu_BiomesSelected_Parent.SetActive(false);
        overworldMenu_BiomesBig_Parent.SetActive(true);
        Biome_3_Parent.SetActive(true);
    }
    void Menu_Biome_4()
    {
        //Close any other menu
        HideAllMenus();

        //Open the InfoMenu
        overworldMenu_Parent.SetActive(true);
        overworldMenu_BiomesSelected_Parent.SetActive(false);
        overworldMenu_BiomesBig_Parent.SetActive(true);
        Biome_4_Parent.SetActive(true);
    }
    void Menu_Biome_5()
    {
        //Close any other menu
        HideAllMenus();

        //Open the InfoMenu
        overworldMenu_Parent.SetActive(true);
        overworldMenu_BiomesSelected_Parent.SetActive(false);
        overworldMenu_BiomesBig_Parent.SetActive(true);
        Biome_5_Parent.SetActive(true);
    }
    void Menu_Biome_6()
    {
        //Close any other menu
        HideAllMenus();

        //Open the InfoMenu
        overworldMenu_Parent.SetActive(true);
        overworldMenu_BiomesSelected_Parent.SetActive(false);
        overworldMenu_BiomesBig_Parent.SetActive(true);
        Biome_6_Parent.SetActive(true);
    }
    void Menu_Biome_7()
    {
        //Close any other menu
        HideAllMenus();

        //Open the InfoMenu
        overworldMenu_Parent.SetActive(true);
        overworldMenu_BiomesSelected_Parent.SetActive(false);
        overworldMenu_BiomesBig_Parent.SetActive(true);
        Biome_7_Parent.SetActive(true);
    }
    void Menu_Biome_8()
    {
        //Close any other menu
        HideAllMenus();

        //Open the InfoMenu
        overworldMenu_Parent.SetActive(true);
        overworldMenu_BiomesSelected_Parent.SetActive(false);
        overworldMenu_BiomesBig_Parent.SetActive(true);
        Biome_8_Parent.SetActive(true);
    }


    //--------------------


    void HideAllMenus()
    {
        mainMenu_Parent.SetActive(false);
        infoMenu_Parent.SetActive(false);
        overworldMenu_Parent.SetActive(false);

        Biome_1_Parent.SetActive(false);
        Biome_2_Parent.SetActive(false);
        Biome_3_Parent.SetActive(false);
        Biome_4_Parent.SetActive(false);
        Biome_5_Parent.SetActive(false);
        Biome_6_Parent.SetActive(false);
        Biome_7_Parent.SetActive(false);
        Biome_8_Parent.SetActive(false);
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
        Application.Quit();
    }

    #endregion
}