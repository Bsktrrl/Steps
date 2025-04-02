using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RememberCurrentlySelectedUIElement : Singleton<RememberCurrentlySelectedUIElement>
{
    public static event Action Action_ChangedSelectedUIElement;

    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject lastSelectedUIElement;


    //--------------------


    private void Update()
    {
        if (!eventSystem) { return; }

        if (eventSystem.currentSelectedGameObject && lastSelectedUIElement != eventSystem.currentSelectedGameObject)
        {
            lastSelectedUIElement = eventSystem.currentSelectedGameObject;
        }

        if (!eventSystem.currentSelectedGameObject && lastSelectedUIElement)
        {
            eventSystem.SetSelectedGameObject(lastSelectedUIElement);
            Action_ChangedSelectedUIElement?.Invoke();
        }
    }


    //--------------------


    private void Reset()
    {
        eventSystem = FindObjectOfType<EventSystem>();

        if (eventSystem == null)
        {
            Debug.Log("Did not find an EventSystem in the Scene. ", this);
        }

        lastSelectedUIElement = eventSystem.firstSelectedGameObject;
    }
}
