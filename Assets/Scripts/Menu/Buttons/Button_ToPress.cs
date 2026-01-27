using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_ToPress : MonoBehaviour
{
    public static event Action Action_ButtonIsPressed;

    [Header("MenuState")]
    [SerializeField] MenuState newMenuState;

    [Header("Setup")]
    [SerializeField] Selectable uiElementToSelect;

    [Header("Visualization")]
    [SerializeField] bool showVisualization;
    [SerializeField] Color navigationColor = Color.cyan;

    [Header("Animator - Closing Animation")]
    [SerializeField] List<Animator> closingMenuAnimatorList = new List<Animator>();
    float closingMenuDelay = 0.2f;


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


    //--------------------


    public void Button_isPressed()
    {
        JumpToElement();
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


        //-----


        if (closingMenuAnimatorList.Count > 0)
        {
            for (int i = 0; i < closingMenuAnimatorList.Count; i++)
            {
                closingMenuAnimatorList[i].SetTrigger("Close");
            }

            StartCoroutine(CloseMenuDelay(closingMenuDelay));
        }
        else
        {
            CloseOpenMenu();
        }
    }

    IEnumerator CloseMenuDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        CloseOpenMenu();
    }
    void CloseOpenMenu()
    {
        Action_ButtonIsPressed?.Invoke();

        MenuStates.Instance.ChangeMenuState(newMenuState);

        switch (newMenuState)
        {
            case MenuState.None:
                break;

            case MenuState.Main_Menu:
                MenuManager.Instance.HideAllMenus();
                MainMenuManager.Instance.mainMenu_Parent.SetActive(true);
                break;
            case MenuState.Skin_Menu:
                EventSystem.current.SetSelectedGameObject(MenuManager.Instance.wardrobe_StartButton);
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
                EventSystem.current.SetSelectedGameObject(MenuManager.Instance.wardrobe_StartButton);
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

            case MenuState.NewGameWarningMessage:
                MainMenuManager.Instance.newGameWarningMessage_Parent.SetActive(true);
                break;
            case MenuState.NewGameWarningMessage_No:
                MainMenuManager.Instance.newGameWarningMessage_Parent.SetActive(false);
                break;

            default:
                break;
        }

        if (uiElementToSelect.gameObject)
        {
            ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(uiElementToSelect.gameObject);
        }
    }

    void HideAllPauseMenus()
    {
        PauseMenuManager.Instance.pauseMenu_MainMenu_Parent.SetActive(false);
        PauseMenuManager.Instance.pauseMenu_Skins_Parent.SetActive(false);
        PauseMenuManager.Instance.pauseMenu_Options_Parent.SetActive(false);
    }
}
