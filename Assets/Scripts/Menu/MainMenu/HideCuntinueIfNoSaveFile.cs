using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HideCuntinueIfNoSaveFile : Singleton<HideCuntinueIfNoSaveFile>
{
    [SerializeField] GameObject continueButton;

    [SerializeField] Button newGameButton;
    [SerializeField] Button exitButton;

    EventSystem eventSystem;


    //--------------------


    private void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        CheckForSaveFile();
    }

    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += CheckForSaveFile;
        NewGameButton.Action_PressNewGameButton += ShowContinueButton;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= CheckForSaveFile;
        NewGameButton.Action_PressNewGameButton -= ShowContinueButton;
    }


    //--------------------


    void CheckForSaveFile()
    {
        if (DataManager.Instance.haveStartedNewGame_Store)
            SaveFile_NO();
        else
            SaveFile_YES();
    }


    //--------------------


    void ShowContinueButton_Start()
    {
        SetNavigation(newGameButton, continueButton.GetComponent<Button>(), null);
        SetNavigation(exitButton, null, continueButton.GetComponent<Button>());

        eventSystem.SetSelectedGameObject(continueButton);
        SetFirstSelected_NewGame(continueButton.GetComponent<MainMenuButton>(), newGameButton.gameObject.GetComponent<MainMenuButton>());

        continueButton.SetActive(true);
    }
    void ShowContinueButton()
    {
        DataManager.Instance.haveStartedNewGame_Store = true;
        DataPersistanceManager.instance.SaveGame();

        StartCoroutine(ShowContinueButton_Delay(0.2f));
    }
    IEnumerator ShowContinueButton_Delay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        SetNavigation(newGameButton, continueButton.GetComponent<Button>(), null);
        SetNavigation(exitButton, null, continueButton.GetComponent<Button>());

        SetFirstSelected_NewGame(continueButton.GetComponent<MainMenuButton>(), newGameButton.gameObject.GetComponent<MainMenuButton>());

        continueButton.SetActive(true);
    }
    void HideContinueButton()
    {
        SetNavigation(newGameButton, exitButton, null);
        SetNavigation(exitButton, null, newGameButton);

        newGameButton.GetComponent<MainMenuButton>().isFirstSelected = true;

        SetFirstSelected(newGameButton.gameObject.GetComponent<MainMenuButton>(), continueButton.GetComponent<MainMenuButton>());

        continueButton.SetActive(false);
    }

    public static void SetNavigation(Button current, Button up, Button down)
    {
        var nav = current.navigation;   // Navigation is a struct
        nav.mode = Navigation.Mode.Explicit;

        if (up != null)
            nav.selectOnUp = up;
        if (down != null)
            nav.selectOnDown = down;

        current.navigation = nav;
    }


    //--------------------


    void UpdateFirstSelected()
    {
        if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Count > 0)
            SaveFile_NO();
        else
            SaveFile_YES();
    }
    void SaveFile_NO()
    {
        ShowContinueButton_Start();
        //SetFirstSelected(continueButton.GetComponent<MainMenuButton>(), newGameButton.gameObject.GetComponent<MainMenuButton>());
    }
    void SaveFile_YES()
    {
        HideContinueButton();
        //SetFirstSelected(newGameButton.gameObject.GetComponent<MainMenuButton>(), continueButton.GetComponent<MainMenuButton>());
    }

    void SetFirstSelected(MainMenuButton setTrue, MainMenuButton setFalse)
    {
        eventSystem.SetSelectedGameObject(setTrue.gameObject);
        eventSystem.firstSelectedGameObject = setTrue.gameObject;

        setTrue.isFirstSelected = true;
        setFalse.isFirstSelected = false;
    }
    void SetFirstSelected_NewGame(MainMenuButton setTrue, MainMenuButton setFalse)
    {
        //eventSystem.SetSelectedGameObject(setTrue.gameObject);
        eventSystem.firstSelectedGameObject = setTrue.gameObject;

        setTrue.isFirstSelected = true;
        setFalse.isFirstSelected = false;
    }
}
