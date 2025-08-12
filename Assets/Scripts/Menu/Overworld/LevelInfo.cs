using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    [Header("Level Name Display in different Languages")]
    public string norwegian_MapNameDisplay;
    public string english_MapNameDisplay;
    public string german_MapNameDisplay;
    public string chinese_MapNameDisplay;
    public string japanese_MapNameDisplay;
    public string korean_MapNameDisplay;


    //--------------------


    private void Start()
    {
        SaveLevelInfoData();
    }


    //--------------------


    void SaveLevelInfoData()
    {
        for (int i = 0; i < DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Count; i++)
        {
            if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].mapName == GetComponent<LoadLevel>().levelToPlay)
            {
                DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].mapNameDisplay.mapNameDisplay_norwegian = norwegian_MapNameDisplay;
                DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].mapNameDisplay.mapNameDisplay_english = english_MapNameDisplay;
                DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].mapNameDisplay.mapNameDisplay_german = german_MapNameDisplay;
                DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].mapNameDisplay.mapNameDisplay_chinese = chinese_MapNameDisplay;
                DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].mapNameDisplay.mapNameDisplay_japanese = japanese_MapNameDisplay;
                DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].mapNameDisplay.mapNameDisplay_korean = korean_MapNameDisplay;

                break;
            }
        }

        DataPersistanceManager.instance.SaveGame();
    }

    public void SaveNameDisplay()
    {
        print("1. SaveNameDisplay");
        MapNameDisplay mapNameDisplay = new MapNameDisplay();

        mapNameDisplay.mapNameDisplay_norwegian = norwegian_MapNameDisplay;
        mapNameDisplay.mapNameDisplay_english = english_MapNameDisplay;
        mapNameDisplay.mapNameDisplay_german = german_MapNameDisplay;
        mapNameDisplay.mapNameDisplay_chinese = chinese_MapNameDisplay;
        mapNameDisplay.mapNameDisplay_japanese = japanese_MapNameDisplay;
        mapNameDisplay.mapNameDisplay_korean = korean_MapNameDisplay;

        DataManager.Instance.mapNameDisplay_Store = mapNameDisplay;

        DataPersistanceManager.instance.SaveGame();
    }


    //--------------------


    public string GetName()
    {
        switch (DataManager.Instance.settingData_StoreList.currentLanguage)
        {
            case Languages.Norwegian:
                return norwegian_MapNameDisplay;
            case Languages.English:
                return english_MapNameDisplay;
            case Languages.German:
                return german_MapNameDisplay;
            case Languages.Japanese:
                return chinese_MapNameDisplay;
            case Languages.Chinese:
                return japanese_MapNameDisplay;
            case Languages.Korean:
                return korean_MapNameDisplay;

            default:
                return norwegian_MapNameDisplay;
        }
    }
}
