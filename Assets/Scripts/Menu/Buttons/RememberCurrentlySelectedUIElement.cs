using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RememberCurrentlySelectedUIElement : Singleton<RememberCurrentlySelectedUIElement>
{
    public static event Action Action_ChangedSelectedUIElement;

    public EventSystem eventSystem;
    public GameObject currentSelectedUIElement;

    public OverWorldStates overWorldStates;


    //--------------------


    private void Update()
    {
        if (!eventSystem) { return; }

        //If changing SelectedElement
        if (eventSystem.currentSelectedGameObject && currentSelectedUIElement != eventSystem.currentSelectedGameObject)
        {
            currentSelectedUIElement = eventSystem.currentSelectedGameObject;

            if (currentSelectedUIElement.GetComponent<RegionButton>())
            {
                OverWorldManager.Instance.ChangeStates(currentSelectedUIElement.GetComponent<RegionButton>().regionState, OverWorldManager.Instance.levelState);
            }

            Action_ChangedSelectedUIElement_Invoke();
        }

        //If no SelectedElement
        if (!eventSystem.currentSelectedGameObject && currentSelectedUIElement)
        {
            eventSystem.SetSelectedGameObject(currentSelectedUIElement);
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

        currentSelectedUIElement = eventSystem.firstSelectedGameObject;
    }


    //--------------------


    public void SaveSelectedUIElement(RegionState _regionState, LevelState _levelState)
    {
        OverWorldStates temp = new OverWorldStates();

        temp.regionState = _regionState;
        temp.levelState = _levelState;

        overWorldStates = temp;
        DataManager.Instance.overWorldStates_StoreList = overWorldStates;

        DataPersistanceManager.instance.SaveGame();

        print("1000. SaveSelectedUIElement - R: " + DataManager.Instance.overWorldStates_StoreList.regionState + " | L: " + DataManager.Instance.overWorldStates_StoreList.levelState);
    }


    //--------------------


    public void Action_ChangedSelectedUIElement_Invoke()
    {
        Action_ChangedSelectedUIElement?.Invoke();
    }
}

[Serializable]
public class OverWorldStates
{
    public RegionState regionState;
    public LevelState levelState;
}