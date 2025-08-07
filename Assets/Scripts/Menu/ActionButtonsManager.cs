using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ActionButtonsManager : Singleton<ActionButtonsManager>
{
    [Header("Event System")]
    public EventSystem eventSystem;

    [Header("Button Navigation")]
    public InputActionReference cancel_Button;

    [Header("Input System")]
    PlayerControls playerControls;
    MainMenuManager mainMenuManager;


    //--------------------


    private void Reset()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Start()
    {
        playerControls = new PlayerControls();
        mainMenuManager = FindObjectOfType<MainMenuManager>();
    }


    //--------------------


    void OnMenu_Back()
    {
        //print("1. Back Button is Pressed");
    }


    //--------------------


    void OnOptionsMenuShift_Left()
    {
        if (mainMenuManager && mainMenuManager.optionsMenu_Parent.activeInHierarchy)
        {
            switch (OptionsMenuManager.Instance.currentOptionsMenuCategorySelected)
            {
                case OptionsMenuCategories.Settings:
                    EventSystem.current.SetSelectedGameObject(OptionsMenuManager.Instance.controlsMenuButton);
                    OptionsMenuManager.Instance.ChangeOptionCategory(OptionsMenuCategories.Controls);
                    StartCoroutine(OptionsMenuManager.Instance.controlsMenuButton.GetComponent<SettingsCategorySelected>().WatchSelection());
                    break;
                case OptionsMenuCategories.Controls:
                    EventSystem.current.SetSelectedGameObject(OptionsMenuManager.Instance.settingsMenuButton);
                    OptionsMenuManager.Instance.ChangeOptionCategory(OptionsMenuCategories.Settings);
                    StartCoroutine(OptionsMenuManager.Instance.settingsMenuButton.GetComponent<SettingsCategorySelected>().WatchSelection());
                    break;

                default:
                    break;
            }
        }
    }
    void OnOptionsMenuShift_Right()
    {
        if (mainMenuManager && mainMenuManager.optionsMenu_Parent.activeInHierarchy)
        {
            switch (OptionsMenuManager.Instance.currentOptionsMenuCategorySelected)
            {
                case OptionsMenuCategories.Settings:
                    EventSystem.current.SetSelectedGameObject(OptionsMenuManager.Instance.controlsMenuButton);
                    OptionsMenuManager.Instance.ChangeOptionCategory(OptionsMenuCategories.Controls);
                    StartCoroutine(OptionsMenuManager.Instance.controlsMenuButton.GetComponent<SettingsCategorySelected>().WatchSelection());
                    break;
                case OptionsMenuCategories.Controls:
                    EventSystem.current.SetSelectedGameObject(OptionsMenuManager.Instance.settingsMenuButton);
                    OptionsMenuManager.Instance.ChangeOptionCategory(OptionsMenuCategories.Settings);
                    StartCoroutine(OptionsMenuManager.Instance.settingsMenuButton.GetComponent<SettingsCategorySelected>().WatchSelection());
                    break;

                default:
                    break;
            }
        }
    }
}
