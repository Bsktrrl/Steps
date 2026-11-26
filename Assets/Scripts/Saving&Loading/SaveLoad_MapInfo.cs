using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad_MapInfo : Singleton<SaveLoad_MapInfo>
{
    public void LoadData()
    {
        var map = MapManager.Instance;
        if (!map || map.mapInfo_ToSave == null)
            return;

        var list = DataManager.Instance.mapInfo_StoreList?.map_SaveInfo_List;
        if (list != null)
        {
            string sceneName = map.gameObject.scene.name;
            foreach (var mapInfo in list)
            {
                if (mapInfo.mapName == sceneName)
                {
                    map.mapInfo_ToSave = mapInfo;
                    map.mapInfo_ToSave.CorrectingMapObjects();
                    return; // Loaded, no need to save here
                }
            }
        }

        map.mapInfo_ToSave.SetupMap();
    }
    public void SaveGame()
    {
        var map = MapManager.Instance;
        if (!map || map.mapInfo_ToSave == null)
            return;

        Map_SaveInfo currentMap = map.mapInfo_ToSave;
        var savedMaps = DataManager.Instance.mapInfo_StoreList?.map_SaveInfo_List;
        if (savedMaps != null && savedMaps.Count > 0)
        {
            for (int i = 0; i < savedMaps.Count; i++)
            {
                if (savedMaps[i].mapName == currentMap.mapName)
                {
                    savedMaps[i] = currentMap;
                    DataPersistanceManager.instance.SaveGame();

                    return;
                }
            }
        }

        SaveNewLevel(currentMap);
    }
    void SaveNewLevel(Map_SaveInfo currentMap)
    {
        if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Count <= 0)
        {
            print("1. Add new Level");
            List<Map_SaveInfo> Map_SaveInfoList_Temp = new List<Map_SaveInfo>();
            DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List = Map_SaveInfoList_Temp;
        }

        print("2. Add new Level");
        DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Add(currentMap);
        DataPersistanceManager.instance.SaveGame();
    }


    //--------------------


    public Map_SaveInfo GetMapInfo(string mapName)
    {
        var savedMaps = DataManager.Instance.mapInfo_StoreList?.map_SaveInfo_List;
        if (savedMaps != null)
        {
            foreach (Map_SaveInfo savedMap in savedMaps)
            {
                if (savedMap.mapName == mapName)
                    return savedMap;
            }
        }

        return null;
    }
}
