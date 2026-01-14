using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu_AbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    public static event Action Action_AbilityButtonIsSelected;


    //--------------------


    public void OnPointerEnter(PointerEventData eventData)
    {
        SetActiveAbilityButton();
        Action_AbilityButtonIsSelected_IsSet();
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetActiveAbilityButton();
        Action_AbilityButtonIsSelected_IsSet();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetActiveAbilityButton();
        Action_AbilityButtonIsSelected_IsSet();
    }

    public void OnDeselect(BaseEventData eventData)
    {

    }


    //--------------------


    void SetActiveAbilityButton()
    {
        PauseMenuManager.Instance.activeAbilityButton = gameObject.GetComponent<Button>();
    }


    //--------------------


    void Action_AbilityButtonIsSelected_IsSet()
    {
        Action_AbilityButtonIsSelected?.Invoke();
    }
}
