using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIBackNavigation : Singleton<UIBackNavigation>
{
    [SerializeField] List<GameObject> selectionHistory = new List<GameObject>(); // Stores previous selections
    public InputActionReference cancelAction; // Assign in Inspector (UI/Cancel)

    public List<GameObject> regionList = new List<GameObject>();
    public List<GameObject> mainMenuList = new List<GameObject>();

    GameObject currentSelected;


    //--------------------


    void Update()
    {
        //If navigating in the menus, out of gameplay
        if (!FindObjectOfType<MainMenuManager>()) { return; }


        if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            BackToPreviousMenu();
            //GoBackToPreviousSelection_LevelPanel();
        }
    }


    //--------------------


    void BackToPreviousMenu()
    {
        //GameObject currentSelected = RememberCurrentlySelectedUIElement.Instance.eventSystem.currentSelectedGameObject;
        //if (currentSelected != null && (selectionHistory.Count == 0 || selectionHistory[selectionHistory.Count - 1] != currentSelected))
        //{
        //    selectionHistory.Add(currentSelected);

        //    //Return back to previous menu
        //    if (MainMenuManager.Instance.menuState == MenuState.Info_Menu)
        //    {
        //        MainMenuManager.Instance.menuState = MenuState.Main_Menu;
        //    }
        //    else if (MainMenuManager.Instance.menuState == MenuState.Settings_Menu)
        //    {
        //        MainMenuManager.Instance.menuState = MenuState.Main_Menu;
        //    }
        //    else if (MainMenuManager.Instance.menuState == MenuState.Overworld_Menu)
        //    {
        //        MainMenuManager.Instance.menuState = MenuState.Main_Menu;
        //    }
        //    else if (MainMenuManager.Instance.menuState == MenuState.Biome_Menu)
        //    {
        //        OverWorldManager.Instance.regionState = RememberCurrentlySelectedUIElement.Instance.lastRegionSelected;
        //        MainMenuManager.Instance.menuState = MenuState.Overworld_Menu;

        //        RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement = RememberCurrentlySelectedUIElement.Instance.lastRegionSelectedObj;
        //        RememberCurrentlySelectedUIElement.Instance.eventSystem.SetSelectedGameObject(RememberCurrentlySelectedUIElement.Instance.lastRegionSelectedObj);

        //        OverWorldManager.Instance.ChangeStates(OverWorldManager.Instance.regionState, LevelState.None);
        //        OverWorldManager.Instance.levelPanel.SetActive(false);
        //    }

        //    RememberCurrentlySelectedUIElement.Instance.Action_ChangedSelectedUIElement_Invoke();
        //}
    }


    //--------------------


    //void CheckRegionSelections()
    //{
    //    // Check if any new selection has been made
    //    currentSelected = RememberCurrentlySelectedUIElement.Instance.eventSystem.currentSelectedGameObject;
    //    if (currentSelected != null && (selectionHistory.Count == 0 || selectionHistory[selectionHistory.Count - 1] != currentSelected))
    //    {
    //        selectionHistory.Add(currentSelected);

    //        if (currentSelected == regionList[0])
    //            OverWorldManager.Instance.regionState = RegionState.Ice;
    //        else if (currentSelected == regionList[1])
    //            OverWorldManager.Instance.regionState = RegionState.Stone;
    //        else if (currentSelected == regionList[2])
    //            OverWorldManager.Instance.regionState = RegionState.Grass;
    //        else if (currentSelected == regionList[3])
    //            OverWorldManager.Instance.regionState = RegionState.Desert;
    //        else if (currentSelected == regionList[4])
    //            OverWorldManager.Instance.regionState = RegionState.Swamp;
    //        else if (currentSelected == regionList[5])
    //            OverWorldManager.Instance.regionState = RegionState.Industrial;
    //        else
    //            OverWorldManager.Instance.regionState = RegionState.None;

    //        RememberCurrentlySelectedUIElement.Instance.Action_ChangedSelectedUIElement_Invoke();
    //    }
    //}
    //void CheckMenuSelections()
    //{
    //    // Check if any new selection has been made
    //    GameObject currentSelected = RememberCurrentlySelectedUIElement.Instance.eventSystem.currentSelectedGameObject;
    //    if (currentSelected != null && (selectionHistory.Count == 0 || selectionHistory[selectionHistory.Count - 1] != currentSelected))
    //    {
    //        selectionHistory.Add(currentSelected);

    //        if (currentSelected == mainMenuList[0])
    //            OverWorldManager.Instance.regionState = RegionState.None;
    //        else if (currentSelected == mainMenuList[1])
    //            OverWorldManager.Instance.regionState = RegionState.None;
    //        else if (currentSelected == mainMenuList[2])
    //            OverWorldManager.Instance.regionState = RegionState.None;

    //        RememberCurrentlySelectedUIElement.Instance.Action_ChangedSelectedUIElement_Invoke();
    //    }
    //}

    //void GoBackToPreviousSelection_LevelPanel()
    //{
    //    // Remove current selection if there are at least two elements in the stack
    //    if (selectionHistory.Count > 1)
    //    {
    //        GameObject newRegionSelection = new GameObject();

    //        //Move back to RegionsMenu
    //        if (OverWorldManager.Instance.levelState != LevelState.None)
    //        {
    //            newRegionSelection = CheckBackState_LevelPanel();
    //        }
            
    //        if (newRegionSelection != null)
    //        {
    //            RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement = newRegionSelection;
    //            RememberCurrentlySelectedUIElement.Instance.eventSystem.SetSelectedGameObject(newRegionSelection);
    //            OverWorldManager.Instance.ChangeStates(OverWorldManager.Instance.regionState, LevelState.None);
    //            //OverWorldManager.Instance.levelPanel.SetActive(false);

    //            RememberCurrentlySelectedUIElement.Instance.Action_ChangedSelectedUIElement_Invoke();
    //        }
    //    }
    //}

    //GameObject CheckBackState_LevelPanel()
    //{
    //    GameObject tempObj = null;

    //    for (int i = selectionHistory.Count - 1; i >= 0; i--)
    //    {
    //        if (selectionHistory[i] == regionList[0]
    //                || selectionHistory[i] == regionList[1]
    //                || selectionHistory[i] == regionList[2]
    //                || selectionHistory[i] == regionList[3]
    //                || selectionHistory[i] == regionList[4]
    //                || selectionHistory[i] == regionList[5])
    //        {
    //            tempObj = selectionHistory[i];
    //            return tempObj;
    //        }
    //    }

    //    return tempObj;
    //}
}
