using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_ToPress : MonoBehaviour
{
    [Header("MenuState")]
    [SerializeField] MenuState newMenuState;

    [Header("Setup")]
    [SerializeField] Selectable uiElementToSelect;

    [Header("Visualization")]
    [SerializeField] bool showVisualization;
    [SerializeField] Color navigationColor = Color.cyan;


    //--------------------


    private void OnDrawGizmos()
    {
        if (!showVisualization) { return; }
        if (uiElementToSelect == null) { return; }

        Gizmos.color = navigationColor;
        Gizmos.DrawLine(gameObject.transform.position, uiElementToSelect.gameObject.transform.position);
    }

    private void Reset()
    {
        if (ActionButtonsManager.Instance.eventSystem == null)
        {
            Debug.Log("Did not find an EventSystem in the Scene. ", this);
        }
    }

    void JumpToElement()
    {
        //PauseMenu
        if (ActionButtonsManager.Instance.eventSystem == null)
        {
            Debug.Log("This item has no EventSystem referenced yet.");
        }

        if (uiElementToSelect == null)
        {
            Debug.Log("This should jump where? ", this);
        }

        if (uiElementToSelect.gameObject)
        {
            ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(uiElementToSelect.gameObject);
        }

        switch (newMenuState)
        {
            case MenuState.None:
                break;

            case MenuState.Main_Menu:
                MenuManager.Instance.HideAllMenus();
                MainMenuManager.Instance.mainMenu_Parent.SetActive(true);
                break;
            case MenuState.Skin_Menu:
                EventSystem.current.SetSelectedGameObject(MenuManager.Instance.wardrobeMenuButton);
                MenuManager.Instance.currentMenuCategorySelected = MenuCategories.Wardrobe;
                MenuManager.Instance.ChangeMenuCategory(MenuCategories.Wardrobe);
                break;
            case MenuState.Options_Menu:
                EventSystem.current.SetSelectedGameObject(MenuManager.Instance.settingsMenuButton);
                MenuManager.Instance.currentMenuCategorySelected = MenuCategories.Settings;
                MenuManager.Instance.ChangeMenuCategory(MenuCategories.Settings);
                break;

            case MenuState.Pause_Menu_Main:
                HideAllPauseMenus();
                PauseMenuManager.Instance.pauseMenu_MainMenu_Parent.SetActive(true);
                break;
            case MenuState.Pause_Menu_Skins:
                HideAllPauseMenus();
                PauseMenuManager.Instance.pauseMenu_Skins_Parent.SetActive(true);
                EventSystem.current.SetSelectedGameObject(MenuManager.Instance.wardrobeMenuButton);
                MenuManager.Instance.currentMenuCategorySelected = MenuCategories.Wardrobe;
                MenuManager.Instance.ChangeMenuCategory(MenuCategories.Wardrobe);
                break;
            case MenuState.Pause_Menu_Options:
                HideAllPauseMenus();
                PauseMenuManager.Instance.pauseMenu_Options_Parent.SetActive(true);
                EventSystem.current.SetSelectedGameObject(MenuManager.Instance.settingsMenuButton);
                MenuManager.Instance.currentMenuCategorySelected = MenuCategories.Settings;
                MenuManager.Instance.ChangeMenuCategory(MenuCategories.Settings);
                break;

            default:
                break;
        }
    }

    void HideAllPauseMenus()
    {
        PauseMenuManager.Instance.pauseMenu_MainMenu_Parent.SetActive(false);
        PauseMenuManager.Instance.pauseMenu_Skins_Parent.SetActive(false);
        PauseMenuManager.Instance.pauseMenu_Options_Parent.SetActive(false);
    }


    //--------------------


    public void Button_isPressed()
    {
        MenuStates.Instance.ChangeMenuState(newMenuState);

        JumpToElement();
    }
}
