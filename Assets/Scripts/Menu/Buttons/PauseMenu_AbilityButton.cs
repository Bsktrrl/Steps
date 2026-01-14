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


    //--------------------


    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowVideo();
        SetActiveAbilityButton();
        Action_AbilityButtonIsSelected_IsSet();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideVideo();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        HideVideo();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ShowVideo();
        SetActiveAbilityButton();
        Action_AbilityButtonIsSelected_IsSet();
    }

    public void OnSelect(BaseEventData eventData)
    {
        ShowVideo();
        SetActiveAbilityButton();
        Action_AbilityButtonIsSelected_IsSet();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        HideVideo();
    }


    //--------------------


    void SetActiveAbilityButton()
    {
        PauseMenuManager.Instance.activeAbilityButton = gameObject.GetComponent<Button>();
    }

    void ShowVideo()
    {
        videoDispalyParent.SetActive(true);
    }
    void HideVideo()
    {
        videoDispalyParent.SetActive(false);
    }


    //--------------------


    void Action_AbilityButtonIsSelected_IsSet()
    {
        Action_AbilityButtonIsSelected?.Invoke();
    }
}
