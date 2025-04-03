using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIBackNavigation : MonoBehaviour
{
    private Stack<GameObject> selectionHistory = new Stack<GameObject>(); // Stores previous selections
    private EventSystem eventSystem;
    public InputActionReference cancelAction; // Assign in Inspector (UI/Cancel)

    void Start()
    {
        eventSystem = EventSystem.current;
    }

    void Update()
    {
        // Check if any new selection has been made
        GameObject currentSelected = eventSystem.currentSelectedGameObject;
        if (currentSelected != null && (selectionHistory.Count == 0 || selectionHistory.Peek() != currentSelected))
        {
            selectionHistory.Push(currentSelected);
        }

        // Detect "Cancel" or "Back" button press (from Input System)
        if (cancelAction.action.WasPressedThisFrame())
        {
            GoBackToPreviousSelection();
        }
    }

    void GoBackToPreviousSelection()
    {
        // Remove current selection if there are at least two elements in the stack
        if (selectionHistory.Count > 1)
        {
            selectionHistory.Pop(); // Remove the current selection
            GameObject previousSelection = selectionHistory.Peek(); // Get the previous one

            if (previousSelection != null)
            {
                eventSystem.SetSelectedGameObject(previousSelection);
            }
        }
    }
}
