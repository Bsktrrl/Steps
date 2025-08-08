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
            switch (MenuManager.Instance.currentMenuCategorySelected)
            {
                case MenuCategories.None:
                    EventSystem.current.SetSelectedGameObject(MenuManager.Instance.settingsMenuButton);
                    MenuManager.Instance.currentMenuCategorySelected = MenuCategories.Settings;
                    MenuManager.Instance.ChangeMenuCategory(MenuCategories.Settings);
                    StartCoroutine(MenuManager.Instance.settingsMenuButton.GetComponent<MenuCategorySelected>().WatchSelection());
                    break;

                case MenuCategories.Settings:
                    EventSystem.current.SetSelectedGameObject(MenuManager.Instance.controlsMenuButton);
                    MenuManager.Instance.ChangeMenuCategory(MenuCategories.Controls);
                    StartCoroutine(MenuManager.Instance.controlsMenuButton.GetComponent<MenuCategorySelected>().WatchSelection());
                    break;
                case MenuCategories.Controls:
                    EventSystem.current.SetSelectedGameObject(MenuManager.Instance.settingsMenuButton);
                    MenuManager.Instance.ChangeMenuCategory(MenuCategories.Settings);
                    StartCoroutine(MenuManager.Instance.settingsMenuButton.GetComponent<MenuCategorySelected>().WatchSelection());
                    break;

                default:
                    break;
            }
        }
        else if (mainMenuManager && mainMenuManager.skinsMenu_Parent.activeInHierarchy)
        {
            switch (MenuManager.Instance.currentMenuCategorySelected)
            {
                case MenuCategories.None:
                    EventSystem.current.SetSelectedGameObject(MenuManager.Instance.wardrobeMenuButton);
                    MenuManager.Instance.currentMenuCategorySelected = MenuCategories.Wardrobe;
                    MenuManager.Instance.ChangeMenuCategory(MenuCategories.Wardrobe);
                    StartCoroutine(MenuManager.Instance.wardrobeMenuButton.GetComponent<MenuCategorySelected>().WatchSelection());
                    break;

                case MenuCategories.Wardrobe:
                    EventSystem.current.SetSelectedGameObject(MenuManager.Instance.shopMenuButton);
                    MenuManager.Instance.ChangeMenuCategory(MenuCategories.Shop);
                    StartCoroutine(MenuManager.Instance.shopMenuButton.GetComponent<MenuCategorySelected>().WatchSelection());
                    break;
                case MenuCategories.Shop:
                    EventSystem.current.SetSelectedGameObject(MenuManager.Instance.wardrobeMenuButton);
                    MenuManager.Instance.ChangeMenuCategory(MenuCategories.Wardrobe);
                    StartCoroutine(MenuManager.Instance.wardrobeMenuButton.GetComponent<MenuCategorySelected>().WatchSelection());
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
            switch (MenuManager.Instance.currentMenuCategorySelected)
            {
                case MenuCategories.None:
                    EventSystem.current.SetSelectedGameObject(MenuManager.Instance.settingsMenuButton);
                    MenuManager.Instance.currentMenuCategorySelected = MenuCategories.Settings;
                    MenuManager.Instance.ChangeMenuCategory(MenuCategories.Settings);
                    StartCoroutine(MenuManager.Instance.settingsMenuButton.GetComponent<MenuCategorySelected>().WatchSelection());
                    break;

                case MenuCategories.Settings:
                    EventSystem.current.SetSelectedGameObject(MenuManager.Instance.controlsMenuButton);
                    MenuManager.Instance.ChangeMenuCategory(MenuCategories.Controls);
                    StartCoroutine(MenuManager.Instance.controlsMenuButton.GetComponent<MenuCategorySelected>().WatchSelection());
                    break;
                case MenuCategories.Controls:
                    EventSystem.current.SetSelectedGameObject(MenuManager.Instance.settingsMenuButton);
                    MenuManager.Instance.ChangeMenuCategory(MenuCategories.Settings);
                    StartCoroutine(MenuManager.Instance.settingsMenuButton.GetComponent<MenuCategorySelected>().WatchSelection());
                    break;

                default:
                    break;
            }
        }
        else if (mainMenuManager && mainMenuManager.skinsMenu_Parent.activeInHierarchy)
        {
            switch (MenuManager.Instance.currentMenuCategorySelected)
            {
                case MenuCategories.None:
                    EventSystem.current.SetSelectedGameObject(MenuManager.Instance.wardrobeMenuButton);
                    MenuManager.Instance.currentMenuCategorySelected = MenuCategories.Wardrobe;
                    MenuManager.Instance.ChangeMenuCategory(MenuCategories.Wardrobe);
                    StartCoroutine(MenuManager.Instance.wardrobeMenuButton.GetComponent<MenuCategorySelected>().WatchSelection());
                    break;

                case MenuCategories.Wardrobe:
                    EventSystem.current.SetSelectedGameObject(MenuManager.Instance.shopMenuButton);
                    MenuManager.Instance.ChangeMenuCategory(MenuCategories.Shop);
                    StartCoroutine(MenuManager.Instance.shopMenuButton.GetComponent<MenuCategorySelected>().WatchSelection());
                    break;
                case MenuCategories.Shop:
                    EventSystem.current.SetSelectedGameObject(MenuManager.Instance.wardrobeMenuButton);
                    MenuManager.Instance.ChangeMenuCategory(MenuCategories.Wardrobe);
                    StartCoroutine(MenuManager.Instance.wardrobeMenuButton.GetComponent<MenuCategorySelected>().WatchSelection());
                    break;

                default:
                    break;
            }
        }
    }
}
