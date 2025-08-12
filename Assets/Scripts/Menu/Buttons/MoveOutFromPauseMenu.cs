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
            Debug.Log("Button Pressed");
            SelectCancelTarget();
        }
    }


    //--------------------


    private void SelectCancelTarget()
    {
        if (ActionButtonsManager.Instance.eventSystem == null)
        {
            Debug.Log("This item has no EventSystem referenced yet.");
            return;
        }


        //-----


        PauseMenuManager.Instance.ClosePauseMenu();
        PlayerManager.Instance.pauseGame = false;
    }
}
