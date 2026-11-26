using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    [Header("Level Name Display in different Languages")]
    public string norwegian_MapNameDisplay;
    public string english_MapNameDisplay;
    public string german_MapNameDisplay;
    //public string chinese_MapNameDisplay;
    //public string japanese_MapNameDisplay;
    //public string korean_MapNameDisplay;


    //--------------------


    private void Start()
    {
        if (DataManager.Instance.dataHasLoaded_Check)
            SaveLevelInfoData();
    }

    private void OnEnable() => DataManager.Action_dataHasLoaded += SaveLevelInfoData;
    private void OnDisable() => DataManager.Action_dataHasLoaded -= SaveLevelInfoData;

    //--------------------


    void SaveLevelInfoData()
    {
        string sceneName = GetComponent<LoadLevel>().levelToPlay;
        foreach (Map_SaveInfo map in DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List)
        {
            if (map?.mapName != sceneName)
                continue;

            MapNameDisplay display = map.mapNameDisplay ??= new MapNameDisplay();
            if (display.mapNameDisplay_norwegian == norwegian_MapNameDisplay &&
                display.mapNameDisplay_english == english_MapNameDisplay &&
                display.mapNameDisplay_german == german_MapNameDisplay)
                return;

            display.mapNameDisplay_norwegian = norwegian_MapNameDisplay;
            display.mapNameDisplay_english = english_MapNameDisplay;
            display.mapNameDisplay_german = german_MapNameDisplay;
            DataPersistanceManager.instance.SaveGame();
            return;
        }
    }

    internal void StoreNameDisplay()
    {
        MapNameDisplay mapNameDisplay = new MapNameDisplay();
        mapNameDisplay.sceneName = GetComponent<LoadLevel>().levelToPlay;

        mapNameDisplay.mapNameDisplay_norwegian = norwegian_MapNameDisplay;
        mapNameDisplay.mapNameDisplay_english = english_MapNameDisplay;
        mapNameDisplay.mapNameDisplay_german = german_MapNameDisplay;
        //mapNameDisplay.mapNameDisplay_chinese = chinese_MapNameDisplay;
        //mapNameDisplay.mapNameDisplay_japanese = japanese_MapNameDisplay;
        //mapNameDisplay.mapNameDisplay_korean = korean_MapNameDisplay;
        
        DataManager.Instance.mapNameDisplay_Store = mapNameDisplay;
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
            //case Languages.Japanese:
            //    return chinese_MapNameDisplay;
            //case Languages.Chinese:
            //    return japanese_MapNameDisplay;
            //case Languages.Korean:
            //    return korean_MapNameDisplay;

            default:
                return norwegian_MapNameDisplay;
        }
    }
}
