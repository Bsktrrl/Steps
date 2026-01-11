using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] Button obj_Button;
    [SerializeField] GameObject obj_Passive;
    [SerializeField] GameObject obj_Active;


    [SerializeField] bool isFirstSelected;

    PauseMenuManager pausedMenuManager;


    //--------------------


    private void Awake()
    {
        SetPassive();
    }
    private void Start()
    {
        pausedMenuManager = FindAnyObjectByType<PauseMenuManager>();

        if (isFirstSelected)
        {
            SetActive();
        }
    }


    //--------------------


    private void OnEnable()
    {
        if (pausedMenuManager)
        {
            PauseMenuManager.Action_closePauseMenu += SetPassive;
            
            if (isFirstSelected)
                Player_KeyInputs.Action_PauseMenuIsPressed += SetActive;
        }
    }
    private void OnDisable()
    {
        if (pausedMenuManager)
        {
            PauseMenuManager.Action_closePauseMenu -= SetPassive;

            if (isFirstSelected)
                Player_KeyInputs.Action_PauseMenuIsPressed -= SetActive;
        }
    }


    //--------------------


    public void OnPointerEnter(PointerEventData eventData)
    {
        // Highlighted
        SetActive();
        //Debug.Log("Highlighted");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Normal (mouse left)
        SetPassive();
        //Debug.Log("Normal");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Pressed

        //Debug.Log("Pressed");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Released (back to Highlighted if still hovered)
        SetActive();
        //Debug.Log("Pointer Up");
    }

    public void OnSelect(BaseEventData eventData)
    {
        // Selected (keyboard/controller)
        SetActive();
        //Debug.Log("Selected");
    }

    public void OnDeselect(BaseEventData eventData)
    {
        SetPassive();
        //Debug.Log("Deselected");
    }


    //--------------------


    public void SetActive()
    {
        obj_Passive.SetActive(false);
        obj_Active.SetActive(true);
    }
    public void SetPassive()
    {
        obj_Passive.SetActive(true);
        obj_Active.SetActive(false);
    }
}
