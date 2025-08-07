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

    PauseMenuManager pauseMenuManager;


    //--------------------


    private void Start()
    {
        pauseMenuManager = FindAnyObjectByType<PauseMenuManager>();
    }


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

            case MenuState.Pause_Menu_Main:
                PauseMenuManager.Instance.pauseMenu_MainMenu_Parent.SetActive(true);
                PauseMenuManager.Instance.pauseMenu_Options_Parent.SetActive(false);
                break;
            case MenuState.Pause_Menu_Options:
                PauseMenuManager.Instance.pauseMenu_MainMenu_Parent.SetActive(false);
                PauseMenuManager.Instance.pauseMenu_Options_Parent.SetActive(true);
                break;

            default:
                break;
        }
    }


    //--------------------


    public void Button_isPressed()
    {
        MenuStates.Instance.ChangeMenuState(newMenuState);

        JumpToElement();
    }
}
