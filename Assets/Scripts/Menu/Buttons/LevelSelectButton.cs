using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    [Header("Button")]
    [SerializeField] Button obj_Button;

    [Header("States")]
    [SerializeField] GameObject obj_CanBePlayed_Active;
    [SerializeField] GameObject obj_CanBePlayed_Passive;
    [SerializeField] GameObject obj_CannotBePlayed_Active;
    [SerializeField] GameObject obj_CannotBePlayed_Passive;

    [Header("Is First Level")]
    [SerializeField] bool isFirstSelected;

    int coditionsCounter;

    [SerializeField] bool isSelected;

    MenuLevelInfo menuLevelInfo;


    //--------------------


    private void Start()
    {
        menuLevelInfo = FindAnyObjectByType<MenuLevelInfo>();

        if (isFirstSelected)
        {
            SetActive();
        }
        else
        {
            SetPassive();
        }
    }


    //--------------------


    public void OnPointerEnter(PointerEventData eventData)
    {
        SetActive();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetPassive();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetActive();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetActive();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        SetPassive();
    }


    //--------------------


    public bool CheckButtonStatus()
    {
        coditionsCounter = 0;

        //If there isn't any requirements to play the level
        if (GetComponent<LoadLevel>().levelsToBeFinished.Count <= 0 || (menuLevelInfo && menuLevelInfo.mapInfo_ToSave == null))
        {
            return true;
        }

        //If there are requirements to play the level
        for (int i = 0; i < GetComponent<LoadLevel>().levelsToBeFinished.Count; i++)
        {
            for (int j = 0; j < menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List.Count; j++)
            {
                if (GetComponent<LoadLevel>().levelsToBeFinished[i].GetComponent<LoadLevel>().levelToPlay == menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List[j].mapName)
                {
                    if (menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List[j].isCompleted)
                    {
                        coditionsCounter++;
                    }
                }
            }
        }

        if (coditionsCounter >= GetComponent<LoadLevel>().levelsToBeFinished.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //--------------------


    public void SetActive()
    {
        print("1. SetActive: " + gameObject.name);

        //Can be played, and is selected
        if (CheckButtonStatus())
        {
            obj_CanBePlayed_Active.SetActive(true);
            obj_CanBePlayed_Passive.SetActive(false);
            obj_CannotBePlayed_Active.SetActive(false);
            obj_CannotBePlayed_Passive.SetActive(false);
        }

        //Can NOT be played, but is selected
        else
        {
            obj_CanBePlayed_Active.SetActive(false);
            obj_CanBePlayed_Passive.SetActive(false);
            obj_CannotBePlayed_Active.SetActive(true);
            obj_CannotBePlayed_Passive.SetActive(false);
        }

        isSelected = true;
    }
    public void SetPassive()
    {
        print("2. SetPassive: " + gameObject.name);

        //Can be played, but is NOT selected
        if (CheckButtonStatus())
        {
            obj_CanBePlayed_Active.SetActive(false);
            obj_CanBePlayed_Passive.SetActive(true);
            obj_CannotBePlayed_Active.SetActive(false);
            obj_CannotBePlayed_Passive.SetActive(false);
        }

        //Can NOT be played, and is NOT selected
        else
        {
            obj_CanBePlayed_Active.SetActive(false);
            obj_CanBePlayed_Passive.SetActive(false);
            obj_CannotBePlayed_Active.SetActive(false);
            obj_CannotBePlayed_Passive.SetActive(true);
        }

        isSelected = false;
    }
}
