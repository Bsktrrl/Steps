using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CancelPauseMenuByButtonPress : MonoBehaviour
{
    [Header("Select On Cancel")]
    [SerializeField] Button selectOnCancel;

    [Header("Menus SetActive")]
    [SerializeField] GameObject menuToOpen;
    [SerializeField] GameObject menuToClose;

    [Header("MenuStates")]
    [SerializeField] public MenuState menuState_ToSelect;
    [SerializeField] public LevelState levelState_ToSelect;
    [SerializeField] public RegionState regionState_ToSelect;

    [Header("Change Current Menu Category")]
    public MenuCategories currentMenuCategoryToSelect;

    [Header("Other GameObject to be hidden with This")]
    [SerializeField] List<GameObject> gameObjectsToHideWithThis;


    //--------------------


    private void Reset()
    {
        if (ActionButtonsManager.Instance.eventSystem == null)
        {
            Debug.Log("Did not find an EventSystem in the Scene. ", this);
        }
    }

    void Update()
    {
        if (ActionButtonsManager.Instance.cancel_Button.action.WasPressedThisFrame() && /*RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement == gameObject*/ ActionButtonsManager.Instance.eventSystem.currentSelectedGameObject == gameObject)
        {
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

        if (selectOnCancel == null)
        {
            Debug.Log("This should jump where? ", this);
            return;
        }


        //-----

        if (selectOnCancel)
        {
            ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(selectOnCancel.gameObject);
        }
        
        //Open/Close menus
        if (menuToOpen)
            menuToOpen.SetActive(true);

        if (menuToClose)
        {
            menuToClose.SetActive(false);

            for (int i = 0; i < gameObjectsToHideWithThis.Count; i++)
            {
                gameObjectsToHideWithThis[i].SetActive(false);
            }

            //OverWorldManager.Instance.panelBackground.SetActive(false);
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
    }
}
