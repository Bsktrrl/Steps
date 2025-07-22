using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_KeyInputs : Singleton<Menu_KeyInputs>
{
    public static event Action Action_MenuNavigationUp_isPressed;
    public static event Action Action_MenuNavigationDown_isPressed;
    public static event Action Action_MenuNavigationLeft_isPressed;
    public static event Action Action_MenuNavigationRight_isPressed;

    [Header("Input System")]
    public PlayerControls playerControls;

    [SerializeField] SettingsManager settingsManager;


    //--------------------


    private void Start()
    {
        playerControls = new PlayerControls();

        settingsManager = FindObjectOfType<SettingsManager>();
    }


    //--------------------


    void OnMenuNavigation_Up()
    {
        if (GetComponent<MainMenuManager>() && (GetComponent<MainMenuManager>().menuState == MenuState.Settings_Menu || GetComponent<MainMenuManager>().menuState == MenuState.Info_Menu))
        {
            //print("1. MainMenuManager: OnMenuNavigation_Up");
            Action_MenuNavigationUp_isPressed?.Invoke();
        }
        else if (GetComponent<PlayerManager>() && settingsManager.settingsMenuParent.activeInHierarchy)
        {
            //print("2. PauseMenuManager: OnMenuNavigation_Up");
            Action_MenuNavigationUp_isPressed?.Invoke();
        }
    }
    void OnMenuNavigation_Down()
    {
        if (GetComponent<MainMenuManager>() && (GetComponent<MainMenuManager>().menuState == MenuState.Settings_Menu || GetComponent<MainMenuManager>().menuState == MenuState.Info_Menu))
        {
            //print("1. MainMenuManager: OnMenuNavigation_Down");
            Action_MenuNavigationDown_isPressed?.Invoke();
        }
        else if (GetComponent<PlayerManager>() && settingsManager.settingsMenuParent.activeInHierarchy)
        {
            //print("2. PauseMenuManager: OnMenuNavigation_Down");
            Action_MenuNavigationDown_isPressed?.Invoke();
        }
    }
    void OnMenuNavigation_Left()
    {
        if (GetComponent<MainMenuManager>() && (GetComponent<MainMenuManager>().menuState == MenuState.Settings_Menu || GetComponent<MainMenuManager>().menuState == MenuState.Info_Menu))
        {
            //print("1. MainMenuManager: OnMenuNavigation_Left");
            Action_MenuNavigationLeft_isPressed?.Invoke();
        }
        else if (GetComponent<PlayerManager>() && settingsManager.settingsMenuParent.activeInHierarchy)
        {
            //print("2. PauseMenuManager: OnMenuNavigation_Left");
            Action_MenuNavigationLeft_isPressed?.Invoke();
        }
    }
    void OnMenuNavigation_Right()
    {
        if (GetComponent<MainMenuManager>() && (GetComponent<MainMenuManager>().menuState == MenuState.Settings_Menu || GetComponent<MainMenuManager>().menuState == MenuState.Info_Menu))
        {
            //print("1. MainMenuManager: OnMenuNavigation_Right");
            Action_MenuNavigationRight_isPressed?.Invoke();
        }
        else if (GetComponent<PlayerManager>() && settingsManager.settingsMenuParent.activeInHierarchy)
        {
            //print("2. PauseMenuManager: OnMenuNavigation_Right");
            Action_MenuNavigationRight_isPressed?.Invoke();
        }
    }
}
