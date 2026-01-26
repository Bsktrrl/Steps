using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOutFromPauseMenu : MonoBehaviour
{
    [Header("Animator - Closing Animation")]
    [SerializeField] List<Animator> closingPauseMenuAnimatorList = new List<Animator>();
    float closingMenuDelay = 0.1f;

    bool firstTimeEnter;


    //--------------------


    void Update()
    {
        if (PauseMenuManager.Instance.pauseMenu_Parent.activeInHierarchy && ActionButtonsManager.Instance.eventSystem != null 
            && ActionButtonsManager.Instance.cancel_Button.action.WasPressedThisFrame() && ActionButtonsManager.Instance.eventSystem.currentSelectedGameObject == gameObject
            && !firstTimeEnter)
        {
            //SelectCancelTarget();

            StartCoroutine(CloseMenuDelay(closingMenuDelay));
        }
        else
        {
            firstTimeEnter = false;
        }
    }

    private void OnEnable()
    {
        firstTimeEnter = true;
    }


    //--------------------


    IEnumerator CloseMenuDelay(float waitTime)
    {
        for (int i = 0; i < closingPauseMenuAnimatorList.Count; i++)
        {
            closingPauseMenuAnimatorList[i].SetTrigger("Close");
        }

        yield return new WaitForSeconds(waitTime);

        SelectCancelTarget();
    }

    void SelectCancelTarget()
    {
        //Debug.Log("1. SelectCancelTarget Pressed");

        if (ActionButtonsManager.Instance.eventSystem == null)
        {
            Debug.Log("This item has no EventSystem referenced yet.");
            return;
        }

        if (!PauseMenuManager.Instance.pauseMenu_Parent.activeInHierarchy) { return; }
        if (!PauseMenuManager.Instance.isVisible) { return; }
        if (!PlayerManager.Instance.pauseGame) { return; }

        //Debug.Log("2. SelectCancelTarget Pressed");

        //-----


        PauseMenuManager.Instance.ClosePauseMenu();
        PlayerManager.Instance.pauseGame = false;
    }
}
