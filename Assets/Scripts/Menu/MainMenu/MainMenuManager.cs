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

    public GameObject newGameWarningMessage_Parent;

    public GameObject regionSelectMenu;
    public GameObject levelSelectMenu;
    public GameObject regionMenu_Water;
    public GameObject regionMenu_Sand;
    public GameObject regionMenu_Ice;
    public GameObject regionMenu_Lava;
    public GameObject regionMenu_Swamp;
    public GameObject regionMenu_Metal;

    [Header("Menu State")]
    public MenuState menuState;

    [Header("BlackScreen")]
    public GameObject blackScreen;
    float fadeDuration_In = 1f;
    float fadeDuration_Out = 0.35f;
    [SerializeField] private Image blackScreenImage;
    private Coroutine blackScreenFadeRoutine;


    //--------------------


    private void Awake()
    {
        if (blackScreen != null)
        {
            blackScreenImage = blackScreen.GetComponent<Image>();
            blackScreen.SetActive(true);
        }

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
        DataManager.Action_dataHasLoaded += SetStartingLanguage;
    }

    private void OnDisable()
    {
        MenuStates.menuState_isChanged -= MenusOnMenuState;
        DataManager.Action_dataHasLoaded -= LoadPlayerStats;
        DataManager.Action_dataHasLoaded -= SetMenu;
        DataManager.Action_dataHasLoaded -= FadeOutBlackScreen;
        DataManager.Action_dataHasLoaded -= SetStartingLanguage;
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


            case MenuState.NewGameWarningMessage:
                Menu_NewGameWarningMessage();
                break;
            case MenuState.NewGameWarningMessage_No:
                break;


            case MenuState.RegionMenu_Water:
                Menu_Region_Water();
                break;
            case MenuState.RegionMenu_Sand:
                Menu_Region_Sand();
                break;
            case MenuState.RegionMenu_Ice:
                Menu_Region_Ice();
                break;
            case MenuState.RegionMenu_Lava:
                Menu_Region_Lava();
                break;
            case MenuState.RegionMenu_Swamp:
                Menu_Region_Swamp();
                break;
            case MenuState.RegionMenu_Metal:
                Menu_Region_Metal();
                break;


            default:
                Menu_Main();
                break;
        }
    }


    //--------------------


    void SetStartingLanguage()
    {
        if (!DataManager.Instance.oneTimeRunData_Store.startLanguage_English)
        {
            DataManager.Instance.settingData_StoreList.currentLanguage = Languages.English;
            SettingsManager.Instance.settingsData.currentLanguage = Languages.English;
            SettingsManager.Instance.settingState = SettingState.Settings_Language;

            DataManager.Instance.oneTimeRunData_Store.startLanguage_English = true;

            DataPersistanceManager.instance.SaveGame();

            SettingsManager.Instance.Action_SetNewLanguage_isActive();
        }
    }


    //--------------------


    #region Helpers

    private void StopBlackScreenFadeRoutine()
    {
        if (blackScreenFadeRoutine != null)
        {
            StopCoroutine(blackScreenFadeRoutine);
            blackScreenFadeRoutine = null;
        }
    }

    public void SetBlackScreenImmediate(bool visible, float alpha = 1f)
    {
        if (blackScreen == null)
            return;

        if (blackScreenImage == null)
            blackScreenImage = blackScreen.GetComponent<Image>();

        if (blackScreenImage == null)
            return;

        blackScreen.SetActive(visible);

        Color color = blackScreenImage.color;
        color.a = Mathf.Clamp01(alpha);
        blackScreenImage.color = color;

        Canvas.ForceUpdateCanvases();
    }

    #endregion


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

        if (DataManager.Instance.menuState_Store == MenuState.RegionMenu_Water
            || DataManager.Instance.menuState_Store == MenuState.RegionMenu_Sand
            || DataManager.Instance.menuState_Store == MenuState.RegionMenu_Ice
            || DataManager.Instance.menuState_Store == MenuState.RegionMenu_Lava
            || DataManager.Instance.menuState_Store == MenuState.RegionMenu_Swamp
            || DataManager.Instance.menuState_Store == MenuState.RegionMenu_Metal)
        {
            levelSelectMenu.SetActive(true);
        }
        else
        {
            regionSelectMenu.SetActive(true);
        }
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

    void Menu_NewGameWarningMessage()
    {
        newGameWarningMessage_Parent.SetActive(true);
    }


    void Menu_Region_Water()
    {
        HideAllMenus();

        overworldMenu_Parent.SetActive(true);
        levelSelectMenu.SetActive(true);
        regionMenu_Water.SetActive(true);
    }
    void Menu_Region_Sand()
    {
        HideAllMenus();

        overworldMenu_Parent.SetActive(true);
        levelSelectMenu.SetActive(true);
        regionMenu_Sand.SetActive(true);
    }
    void Menu_Region_Ice()
    {
        HideAllMenus();

        overworldMenu_Parent.SetActive(true);
        levelSelectMenu.SetActive(true);
        regionMenu_Ice.SetActive(true);
    }
    void Menu_Region_Lava()
    {
        HideAllMenus();

        overworldMenu_Parent.SetActive(true);
        levelSelectMenu.SetActive(true);
        regionMenu_Lava.SetActive(true);
    }
    void Menu_Region_Swamp()
    {
        HideAllMenus();

        overworldMenu_Parent.SetActive(true);
        levelSelectMenu.SetActive(true);
        regionMenu_Swamp.SetActive(true);
    }
    void Menu_Region_Metal()
    {
        HideAllMenus();

        overworldMenu_Parent.SetActive(true);
        levelSelectMenu.SetActive(true);
        regionMenu_Metal.SetActive(true);
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
        if (newGameWarningMessage_Parent)
            newGameWarningMessage_Parent.SetActive(false);

        if (regionSelectMenu)
            regionSelectMenu.SetActive(false);
        if (levelSelectMenu)
            levelSelectMenu.SetActive(false);

        if (regionMenu_Water)
            regionMenu_Water.SetActive(false);
        if (regionMenu_Sand)
            regionMenu_Sand.SetActive(false);
        if (regionMenu_Ice)
            regionMenu_Ice.SetActive(false);
        if (regionMenu_Lava)
            regionMenu_Lava.SetActive(false);
        if (regionMenu_Swamp)
            regionMenu_Swamp.SetActive(false);
        if (regionMenu_Metal)
            regionMenu_Metal.SetActive(false);
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
        StopBlackScreenFadeRoutine();
        blackScreenFadeRoutine = StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        if (blackScreen == null || blackScreenImage == null)
            yield break;

        blackScreen.SetActive(true);

        Color color = blackScreenImage.color;
        float startAlpha = color.a;
        float duration = Mathf.Max(0.0001f, fadeDuration_In);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            color.a = Mathf.Lerp(startAlpha, 0f, t);
            blackScreenImage.color = color;
            yield return null;
        }

        color.a = 0f;
        blackScreenImage.color = color;
        blackScreen.SetActive(false);

        blackScreenFadeRoutine = null;
    }

    public void FadeInBlackScreen()
    {
        StopBlackScreenFadeRoutine();
        blackScreenFadeRoutine = StartCoroutine(FadeInBlackScreenCoroutine());
    }

    public IEnumerator FadeInBlackScreenCoroutine(float coverThreshold = 0.92f)
    {
        if (blackScreen == null)
            yield break;

        if (blackScreenImage == null)
            blackScreenImage = blackScreen.GetComponent<Image>();

        if (blackScreenImage == null)
            yield break;

        blackScreen.SetActive(true);

        Color color = blackScreenImage.color;
        float startAlpha = color.a;
        float duration = Mathf.Max(0.0001f, fadeDuration_Out);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float alpha = Mathf.Lerp(startAlpha, 1f, t);

            color.a = alpha;
            blackScreenImage.color = color;

            if (alpha >= coverThreshold)
                break;

            yield return null;
        }

        color.a = 1f;
        blackScreenImage.color = color;

        Canvas.ForceUpdateCanvases();
        yield return new WaitForEndOfFrame();
    }
    public IEnumerator PlayFadeInAndWait(float coverThreshold = 0.92f)
    {
        StopBlackScreenFadeRoutine();
        blackScreenFadeRoutine = StartCoroutine(FadeInBlackScreenCoroutine(coverThreshold));
        yield return blackScreenFadeRoutine;
        blackScreenFadeRoutine = null;
    }
    public IEnumerator PlayFadeOutAndWait()
{
    StopBlackScreenFadeRoutine();
    blackScreenFadeRoutine = StartCoroutine(FadeOutCoroutine());
    yield return blackScreenFadeRoutine;
    blackScreenFadeRoutine = null;
}

    #endregion
}