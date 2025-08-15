using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOutFromPauseMenu : MonoBehaviour
{


    //--------------------


    void Update()
    {
        if (PauseMenuManager.Instance.pauseMenu_Parent.activeInHierarchy && ActionButtonsManager.Instance.eventSystem != null 
            && ActionButtonsManager.Instance.cancel_Button.action.WasPressedThisFrame() && ActionButtonsManager.Instance.eventSystem.currentSelectedGameObject == gameObject)
        {
            SelectCancelTarget();
        }
    }


    //--------------------


    private void SelectCancelTarget()
    {
        Debug.Log("1. SelectCancelTarget Pressed");

        if (ActionButtonsManager.Instance.eventSystem == null)
        {
            Debug.Log("This item has no EventSystem referenced yet.");
            return;
        }

        if (!PauseMenuManager.Instance.pauseMenu_Parent.activeInHierarchy) { return; }
        if (!PauseMenuManager.Instance.isVisible) { return; }
        if (!PlayerManager.Instance.pauseGame) { return; }

        Debug.Log("2. SelectCancelTarget Pressed");

        //-----


        PauseMenuManager.Instance.ClosePauseMenu();
        PlayerManager.Instance.pauseGame = false;
    }
}
