using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu_AbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    public static event Action Action_AbilityButtonIsSelected;

    [Header("Video Parent")]
    [SerializeField] GameObject videoDispalyParent;

    [SerializeField] List<GameObject> abilityTextVersions;


    //--------------------


    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowAbilityDisplay();
        SetActiveAbilityButton();
        Action_AbilityButtonIsSelected_IsSet();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideAbilityDisplay();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        HideAbilityDisplay();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ShowAbilityDisplay();
        SetActiveAbilityButton();
        Action_AbilityButtonIsSelected_IsSet();
    }

    public void OnSelect(BaseEventData eventData)
    {
        ShowAbilityDisplay();
        SetActiveAbilityButton();
        Action_AbilityButtonIsSelected_IsSet();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        HideAbilityDisplay();
    }


    //--------------------


    void SetActiveAbilityButton()
    {
        PauseMenuManager.Instance.activeAbilityButton = gameObject.GetComponent<Button>();
    }


    //--------------------


    void ShowAbilityDisplay()
    {
        videoDispalyParent.SetActive(true);

        if (ControllerState.Instance.activeController == InputType.Keyboard)
            abilityTextVersions[0].SetActive(true);
        else if (ControllerState.Instance.activeController == InputType.PlayStation)
            abilityTextVersions[1].SetActive(true);
        else if (ControllerState.Instance.activeController == InputType.Xbox)
            abilityTextVersions[2].SetActive(true);
    }
    void HideAbilityDisplay()
    {
        videoDispalyParent.SetActive(false);

        for (int i = 0; i < abilityTextVersions.Count; i++)
        {
            abilityTextVersions[i].SetActive(false);
        }
    }


    //--------------------


    void Action_AbilityButtonIsSelected_IsSet()
    {
        Action_AbilityButtonIsSelected?.Invoke();
    }
}
