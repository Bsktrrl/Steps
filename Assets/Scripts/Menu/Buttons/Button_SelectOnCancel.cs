using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CancelPauseMenuByButtonPress : MonoBehaviour
{
    public static event Action Action_ButtonIsCanceled;

    [Header("Select On Cancel")]
    [SerializeField] Button selectOnCancel;

    [Header("Menus SetActive")]
    [SerializeField] GameObject menuToOpen;
    [SerializeField] GameObject menuToClose;

    [Header("MenuStates")]
    public MenuState menuState_ToSelect;
    public LevelState levelState_ToSelect;
    public RegionState regionState_ToSelect;

    [Header("Change Current Menu Category")]
    public MenuCategories currentMenuCategoryToSelect;

    [Header("Other GameObject to be hidden with This")]
    [SerializeField] List<GameObject> gameObjectsToHideWithThis;

    [SerializeField] PauseMenuManager pauseMenuManager;

    [Header("Animator - Closing Animation")]
    [SerializeField] Animator closingMenuAnimator;
    float closingMenuDelay = 0.2f;


    //--------------------


    private void Start()
    {
        pauseMenuManager = FindObjectOfType<PauseMenuManager>();
    }
    private void Reset()
    {
        if (ActionButtonsManager.Instance.eventSystem == null)
        {
            Debug.Log("Did not find an EventSystem in the Scene. ", this);
        }
    }

    void Update()
    {
        if (ActionButtonsManager.Instance.cancel_Button.action.WasPressedThisFrame() && /*RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement == gameObject*/ ActionButtonsManager.Instance.eventSystem.currentSelectedGameObject == gameObject
            /*&& !PauseMenuManager.Instance.pauseMenu_Parent*/)
        {
            SelectCancelTarget();

            print("1. CloseWindow");
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

        if (selectOnCancel == null)
        {
            Debug.Log("No place to jump? ", this);

            return;
        }


        //-----


        if (closingMenuAnimator)
        {
            print("2. CloseWindow");
            closingMenuAnimator.SetTrigger("Close");

            StartCoroutine(CloseMenuDelay(closingMenuDelay));
        }
        else
        {
            CloseMenu();
        }
    }

    IEnumerator CloseMenuDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        CloseMenu();
    }

    void CloseMenu()
    {
        print("3. CloseWindow");

        Action_ButtonIsCanceled?.Invoke();

        //Open/Close menus
        if (menuToOpen)
            menuToOpen.SetActive(true);

        if (menuToClose)
        {
            menuToClose.SetActive(false);

            for (int i = 0; i < gameObjectsToHideWithThis.Count; i++)
            {
                if (gameObjectsToHideWithThis[i]) gameObjectsToHideWithThis[i].SetActive(false);
            }
        }

        MenuManager.Instance.currentMenuCategorySelected = currentMenuCategoryToSelect;

        //Make sure that mainMenu isn't tried accessed during gameplay
        if (menuState_ToSelect != MenuState.Pause_Menu_Main && menuState_ToSelect != MenuState.Pause_Menu_Options)
        {
            MainMenuManager.Instance.menuState = menuState_ToSelect;
            OverWorldManager.Instance.regionState = regionState_ToSelect;
            OverWorldManager.Instance.levelState = levelState_ToSelect;
        }

        MapManager mapManager = FindObjectOfType<MapManager>();

        if (mapManager == null)
            RememberCurrentlySelectedUIElement.Instance.SaveSelectedUIElement(regionState_ToSelect, levelState_ToSelect);


        if (selectOnCancel)
        {
            ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(selectOnCancel.gameObject);
        }
    }
}
