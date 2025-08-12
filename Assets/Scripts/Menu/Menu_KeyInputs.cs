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

    MenuManager optionsManager;


    //--------------------


    private void Start()
    {
        playerControls = new PlayerControls();

        optionsManager = FindObjectOfType<MenuManager>();
    }


    //--------------------


    void OnMenuNavigation_Up()
    {
        if (optionsManager && (optionsManager.currentMenuCategorySelected == MenuCategories.Settings || optionsManager.currentMenuCategorySelected == MenuCategories.Controls))
        {
            //print("1. MainMenuManager: OnMenuNavigation_Up");
            Action_MenuNavigationUp_isPressed?.Invoke();
        }
        //else if (optionsManager && (optionsManager.currentMenuCategorySelected == MenuCategories.Wardrobe || optionsManager.currentMenuCategorySelected == MenuCategories.Shop))
        //{
        //    //print("1. MainMenuManager: OnMenuNavigation_Up");
        //    Action_MenuNavigationUp_isPressed?.Invoke();
        //}
    }
    void OnMenuNavigation_Down()
    {
        if (optionsManager && (optionsManager.currentMenuCategorySelected == MenuCategories.Settings || optionsManager.currentMenuCategorySelected == MenuCategories.Controls))
        {
            //print("1. MainMenuManager: OnMenuNavigation_Down");
            Action_MenuNavigationDown_isPressed?.Invoke();
        }
        //else if (optionsManager && (optionsManager.currentMenuCategorySelected == MenuCategories.Wardrobe || optionsManager.currentMenuCategorySelected == MenuCategories.Shop))
        //{
        //    //print("1. MainMenuManager: OnMenuNavigation_Up");
        //    Action_MenuNavigationDown_isPressed?.Invoke();
        //}
    }
    void OnMenuNavigation_Left()
    {
        if (optionsManager && (optionsManager.currentMenuCategorySelected == MenuCategories.Settings || optionsManager.currentMenuCategorySelected == MenuCategories.Controls))
        {
            //print("1. MainMenuManager: OnMenuNavigation_Left");
            Action_MenuNavigationLeft_isPressed?.Invoke();
        }
        //else if (optionsManager && (optionsManager.currentMenuCategorySelected == MenuCategories.Wardrobe || optionsManager.currentMenuCategorySelected == MenuCategories.Shop))
        //{
        //    //print("1. MainMenuManager: OnMenuNavigation_Up");
        //    Action_MenuNavigationLeft_isPressed?.Invoke();
        //}
    }
    void OnMenuNavigation_Right()
    {
        if (optionsManager && (optionsManager.currentMenuCategorySelected == MenuCategories.Settings || optionsManager.currentMenuCategorySelected == MenuCategories.Controls))
        {
            //print("1. MainMenuManager: OnMenuNavigation_Right");
            Action_MenuNavigationRight_isPressed?.Invoke();
        }
        //else if (optionsManager && (optionsManager.currentMenuCategorySelected == MenuCategories.Wardrobe || optionsManager.currentMenuCategorySelected == MenuCategories.Shop))
        //{
        //    //print("1. MainMenuManager: OnMenuNavigation_Up");
        //    Action_MenuNavigationRight_isPressed?.Invoke();
        //}
    }
}
