using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [Header("MenusOnMenuState")]
    public GameObject mainMenu_Parent;
    public GameObject overworldMenu_Parent;
    public GameObject optionsMenu_Parent;
    public GameObject skinsMenu_Parent;

    [Header("Menu State")]
    public MenuState menuState;

    [Header("BlackScreen")]
    public GameObject blackScreen;
    float fadeDuration_In = 0.75f;
    float fadeDuration_Out = 0.25f;


    //--------------------


    private void Awake()
    {
        blackScreen.SetActive(true);

        HideAllMenus();
    }
    private void Start()
    {
        // Lock the cursor to the center of the screen, and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (DataManager.Instance.menuState_Store == MenuState.None)
        {
            menuState = MenuState.Main_Menu;
            HideAllMenus();
            Menu_Main();
        }
    }


    //--------------------


    private void OnEnable()
    {
        MenuStates.menuState_isChanged += MenusOnMenuState;
        DataManager.Action_dataHasLoaded += LoadPlayerStats;
        DataManager.Action_dataHasLoaded += SetMenu;
        DataManager.Action_dataHasLoaded += FadeOutBlackScreen;
    }

    private void OnDisable()
    {
        MenuStates.menuState_isChanged -= MenusOnMenuState;
        DataManager.Action_dataHasLoaded -= LoadPlayerStats;
        DataManager.Action_dataHasLoaded -= SetMenu;
        DataManager.Action_dataHasLoaded -= FadeOutBlackScreen;
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
            case MenuState.Skin_Menu:
                Menu_Skins();
                break;
            case MenuState.Options_Menu:
                Menu_Options();
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

        //Open the OverworldMenu
        overworldMenu_Parent.SetActive(true);
    }
    void Menu_Skins()
    {
        //Close any other menu
        HideAllMenus();

        //Open the InfoMenu
        skinsMenu_Parent.SetActive(true);
    }
    void Menu_Options()
    {
        //Close any other menu
        HideAllMenus();

        //Open the InfoMenu
        optionsMenu_Parent.SetActive(true);
    }

    void Menu_Biome()
    {
        overworldMenu_Parent.SetActive(true);
    }


    //--------------------


    void HideAllMenus()
    {
        if (mainMenu_Parent)
            mainMenu_Parent.SetActive(false);
        if (overworldMenu_Parent)
            overworldMenu_Parent.SetActive(false);
        if (skinsMenu_Parent)
            skinsMenu_Parent.SetActive(false);
        if (optionsMenu_Parent)
            optionsMenu_Parent.SetActive(false);
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
    public void To_Skins_Button_isPressed()
    {
        MenuStates.Instance.ChangeMenuState(MenuState.Skin_Menu);
    }
    public void To_Options_Button_isPressed()
    {
        MenuStates.Instance.ChangeMenuState(MenuState.Options_Menu);
    }
    public void QuitButton_isPressed()
    {
        print("Quit Game");

        RememberCurrentlySelectedUIElement.Instance.SaveSelectedUIElement(RegionState.None, LevelState.None);

        menuState = MenuState.Main_Menu;

        Application.Quit();
    }

    #endregion


    //--------------------


    #region Fade BlackScreen

    public void FadeOutBlackScreen()
    {
        StartCoroutine(FadeOutCoroutine());
    }
    private IEnumerator FadeOutCoroutine()
    {
        Image blackScreenImage = blackScreen.GetComponent<Image>();

        Color color = blackScreenImage.color;
        float startAlpha = color.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration_In)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration_In);
            color.a = alpha;
            blackScreenImage.color = color;
            yield return null;
        }

        // Ensure it's fully transparent at the end
        color.a = 0f;
        blackScreenImage.color = color;
        blackScreen.SetActive(false);
    }

    public void FadeInBlackScreen()
    {
        StartCoroutine(FadeInBlackScreenCoroutine());
    }
    public IEnumerator FadeInBlackScreenCoroutine()
    {
        blackScreen.SetActive(true);
        Image blackScreenImage = blackScreen.GetComponent<Image>();

        Color color = blackScreenImage.color;
        float startAlpha = color.a; // should be 0 if transparent
        float elapsed = 0f;

        while (elapsed < fadeDuration_Out)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 1f, elapsed / fadeDuration_Out); // fade to opaque
            color.a = alpha;
            blackScreenImage.color = color;
            yield return null;
        }

        // Ensure it's fully opaque at the end
        color.a = 1f;
        blackScreenImage.color = color;
    }

    #endregion
}