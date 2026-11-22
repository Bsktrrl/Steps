using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelectButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Button")]
    [SerializeField] Button obj_Button;

    [Header("States")]
    [SerializeField] GameObject obj_CanBePlayed_Active;
    [SerializeField] GameObject obj_CanBePlayed_Passive;
    [SerializeField] GameObject obj_CannotBePlayed_Active;
    [SerializeField] GameObject obj_CannotBePlayed_Passive;

    [Header("Is First Level")]
    // Retain the existing Inspector setting, but do not use it as a visual selection state.
    [SerializeField] bool isFirstSelected;

    int coditionsCounter;

    [SerializeField] bool isSelected;

    MenuLevelInfo menuLevelInfo;


    //--------------------


    private void Start()
    {
        menuLevelInfo = FindAnyObjectByType<MenuLevelInfo>();

        RefreshSelectionVisual();
    }

    private void OnEnable()
    {
        RefreshSelectionVisual();
    }

    bool IsCurrentlySelected()
    {
        return EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject;
    }

    void RefreshSelectionVisual()
    {
        if (IsCurrentlySelected())
            SetActive();
        else
            SetPassive();
    }


    //--------------------


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
        TryGetComponent(out LoadLevel loadLevel);

        //If there isn't any requirements to play the level
        if ((loadLevel && loadLevel.levelsToBeFinished.Count <= 0) || (menuLevelInfo && menuLevelInfo.mapInfo_ToSave == null)) return true;
        if (menuLevelInfo == null || menuLevelInfo.mapInfo_ToSave == null || menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List == null) return true;

        //If there are requirements to play the level
        if (loadLevel)
        {
            var savedMaps = menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List;
            if (savedMaps.Count == 0)
                return false;

            var requiredLevels = loadLevel.levelsToBeFinished;
            for (int i = 0; i < requiredLevels.Count; i++)
            {
                string requiredLevelName = requiredLevels[i].GetComponent<LoadLevel>().levelToPlay;
                for (int j = 0; j < savedMaps.Count; j++)
                {
                    Map_SaveInfo savedMap = savedMaps[j];
                    if (requiredLevelName == savedMap.mapName)
                    {
                        if (savedMap.isCompleted)
                        {
                            coditionsCounter++;
                        }
                    }
                }
            }


            if (coditionsCounter >= requiredLevels.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (TryGetComponent(out RegionLocked regionLocked))
        {
            if (regionLocked.CheckIfCanBePlayed())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }


    //--------------------


    public void SetActive()
    {
        // A button may only display the active state when it actually has UI focus.
        if (!IsCurrentlySelected())
        {
            SetPassive();
            return;
        }

        //print("1. SetActive: " + gameObject.name);

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
        //print("2. SetPassive: " + gameObject.name);

        //Can be played, but is NOT selected
        if (CheckButtonStatus())
        {
            obj_CanBePlayed_Active.SetActive(false);
            obj_CanBePlayed_Passive.SetActive(true);
            obj_CannotBePlayed_Active.SetActive(false);
            obj_CannotBePlayed_Passive.SetActive(false);
        }

        //Can NOT be played, but is NOT selected
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
