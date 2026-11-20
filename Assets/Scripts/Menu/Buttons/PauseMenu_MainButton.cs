using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu_MainButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public static event Action Action_ButtonIsSelected;


    //--------------------


    public void OnSelect(BaseEventData eventData)
    {
        SetActiveMainButton();
        Action_ButtonIsSelected_IsSet();
    }

    public void OnDeselect(BaseEventData eventData)
    {

    }


    //--------------------


    void SetActiveMainButton()
    {
        PauseMenuManager.Instance.activeMainButton = gameObject.GetComponent<Button>();
    }


    //--------------------


    void Action_ButtonIsSelected_IsSet()
    {
        Action_ButtonIsSelected?.Invoke();
    }
}
