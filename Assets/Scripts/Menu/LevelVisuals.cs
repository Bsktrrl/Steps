using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelVisuals : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Display")]
    [SerializeField] GameObject playBlocker;
    [SerializeField] GameObject completedMarker;


    //--------------------

    private void Update()
    {
        SetLevelVisuals();
        CanPlayDisplay();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Map_SaveInfo mapInfo = SaveLoad_MapInfo.Instance.GetMapInfo(gameObject.GetComponent<LoadLevel>().levelToPlay);

        if (mapInfo != null)
        {
            LevelInfoDisplay.Instance.ShowDisplayLevelInfo(mapInfo, gameObject.GetComponent<LoadLevel>());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LevelInfoDisplay.Instance.HideDisplayLevelInfo();
    }


    //--------------------


    void SetLevelVisuals()
    {
        string levelName = gameObject.GetComponent<LoadLevel>().levelToPlay;

        MenuLevelInfo levelInfo = FindObjectOfType<MenuLevelInfo>();

        if (levelInfo)
        {
            if (levelInfo.mapInfo_ToSave != null)
            {
                if (levelInfo.mapInfo_ToSave.map_SaveInfo_List != null)
                {
                    if (levelInfo.mapInfo_ToSave.map_SaveInfo_List.Count > 0)
                    {
                        foreach (Map_SaveInfo map in levelInfo.mapInfo_ToSave.map_SaveInfo_List)
                        {
                            if (map.mapName == levelName)
                            {
                                //Check if level is completed
                                if (map.isCompleted)
                                    completedMarker.SetActive(true);
                                else
                                    completedMarker.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }


    //--------------------


    void CanPlayDisplay()
    {
        if (gameObject.GetComponent<LoadLevel>().CheckIfCanBePlayed())
        {
            playBlocker.SetActive(false);
        }
        else
        {
            playBlocker.SetActive(true);
        }
    }

}
