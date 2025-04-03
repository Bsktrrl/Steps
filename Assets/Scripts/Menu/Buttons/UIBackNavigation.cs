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


    void Update()
    {
        if (!FindObjectOfType<MainMenuManager>()) { return; }

        // Check if any new selection has been made
        GameObject currentSelected = RememberCurrentlySelectedUIElement.Instance.eventSystem.currentSelectedGameObject;
        if (currentSelected != null && (selectionHistory.Count == 0 || selectionHistory[selectionHistory.Count - 1] != currentSelected))
        {
            selectionHistory.Add(currentSelected);

            if (currentSelected == regionList[0])
                OverWorldManager.Instance.regionState = RegionState.Ice;
            else if (currentSelected == regionList[1])
                OverWorldManager.Instance.regionState = RegionState.Stone;
            else if (currentSelected == regionList[2])
                OverWorldManager.Instance.regionState = RegionState.Grass;
            else if (currentSelected == regionList[3])
                OverWorldManager.Instance.regionState = RegionState.Desert;
            else if (currentSelected == regionList[4])
                OverWorldManager.Instance.regionState = RegionState.Swamp;
            else if (currentSelected == regionList[5])
                OverWorldManager.Instance.regionState = RegionState.Industrial;

            RememberCurrentlySelectedUIElement.Instance.Action_ChangedSelectedUIElement_Invoke();
        }

        if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            GoBackToPreviousSelection();
        }
    }

    void GoBackToPreviousSelection()
    {
        // Remove current selection if there are at least two elements in the stack
        if (selectionHistory.Count > 1)
        {
            GameObject newRegionSelection = CheckBackState_LevelPanel();

            if (newRegionSelection != null)
            {
                print("1. Back was pressed");

                RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement = newRegionSelection;
                RememberCurrentlySelectedUIElement.Instance.eventSystem.SetSelectedGameObject(newRegionSelection);
                OverWorldManager.Instance.ChangeStates(OverWorldManager.Instance.regionState, LevelState.None);
                OverWorldManager.Instance.levelPanel.SetActive(false);

                RememberCurrentlySelectedUIElement.Instance.Action_ChangedSelectedUIElement_Invoke();
            }
        }
    }

    GameObject CheckBackState_LevelPanel()
    {
        GameObject tempObj = null;

        for (int i = selectionHistory.Count - 1; i >= 0; i--)
        {
            if (selectionHistory[i] == regionList[0]
                    || selectionHistory[i] == regionList[1]
                    || selectionHistory[i] == regionList[2]
                    || selectionHistory[i] == regionList[3]
                    || selectionHistory[i] == regionList[4]
                    || selectionHistory[i] == regionList[5])
            {
                tempObj = selectionHistory[i];
                return tempObj;
            }
        }

        return tempObj;
    }
}
