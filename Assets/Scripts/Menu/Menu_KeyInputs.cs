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

    OptionsMenuManager optionsManager;


    //--------------------


    private void Start()
    {
        playerControls = new PlayerControls();

        optionsManager = FindObjectOfType<OptionsMenuManager>();
    }


    //--------------------


    void OnMenuNavigation_Up()
    {
        if (optionsManager && (optionsManager.currentOptionsMenuCategorySelected == OptionsMenuCategories.Settings || optionsManager.currentOptionsMenuCategorySelected == OptionsMenuCategories.Controls))
        {
            //print("1. MainMenuManager: OnMenuNavigation_Up");
            Action_MenuNavigationUp_isPressed?.Invoke();
        }
        //else if (GetComponent<PlayerManager>() && optionsManager.settingsMenuParent.activeInHierarchy)
        //{
        //    //print("2. PauseMenuManager: OnMenuNavigation_Up");
        //    Action_MenuNavigationUp_isPressed?.Invoke();
        //}
    }
    void OnMenuNavigation_Down()
    {
        if (optionsManager && (optionsManager.currentOptionsMenuCategorySelected == OptionsMenuCategories.Settings || optionsManager.currentOptionsMenuCategorySelected == OptionsMenuCategories.Controls))
        {
            //print("1. MainMenuManager: OnMenuNavigation_Down");
            Action_MenuNavigationDown_isPressed?.Invoke();
        }
        //else if (GetComponent<PlayerManager>() && optionsManager.settingsMenuParent.activeInHierarchy)
        //{
        //    //print("2. PauseMenuManager: OnMenuNavigation_Down");
        //    Action_MenuNavigationDown_isPressed?.Invoke();
        //}
    }
    void OnMenuNavigation_Left()
    {
        if (optionsManager && (optionsManager.currentOptionsMenuCategorySelected == OptionsMenuCategories.Settings || optionsManager.currentOptionsMenuCategorySelected == OptionsMenuCategories.Controls))
        {
            //print("1. MainMenuManager: OnMenuNavigation_Left");
            Action_MenuNavigationLeft_isPressed?.Invoke();
        }
        //else if (GetComponent<PlayerManager>() && optionsManager.settingsMenuParent.activeInHierarchy)
        //{
        //    print("12. PerformButtonAction_Left");

        //    //print("2. PauseMenuManager: OnMenuNavigation_Left");
        //    Action_MenuNavigationLeft_isPressed?.Invoke();
        //}
    }
    void OnMenuNavigation_Right()
    {
        if (optionsManager && (optionsManager.currentOptionsMenuCategorySelected == OptionsMenuCategories.Settings || optionsManager.currentOptionsMenuCategorySelected == OptionsMenuCategories.Controls))
        {
            //print("1. MainMenuManager: OnMenuNavigation_Right");
            Action_MenuNavigationRight_isPressed?.Invoke();
        }
        //else if (GetComponent<PlayerManager>() && optionsManager.settingsMenuParent.activeInHierarchy)
        //{
        //    print("22. PerformButtonAction_Right");

        //    //print("2. PauseMenuManager: OnMenuNavigation_Right");
        //    Action_MenuNavigationRight_isPressed?.Invoke();
        //}
    }
}
