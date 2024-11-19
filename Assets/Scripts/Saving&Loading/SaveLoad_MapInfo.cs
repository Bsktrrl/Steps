using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoad_MapInfo : Singleton<SaveLoad_MapInfo>
{
    public void LoadData()
    {
        if (DataManager.Instance.mapInfo_StoreList != null)
        {
            for (int i = 0; i < DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Count; i++)
            {
                print("MapName: " + DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].mapName + " | Saved Name: " + SceneManager.GetActiveScene().name);

                if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].mapName == SceneManager.GetActiveScene().name)
                {
                    print("MapInfo is already in the system");

                    MapManager.Instance.mapInfo_ToSave = DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i];
                    MapManager.Instance.mapInfo_ToSave.CorrectingMapObjects();
                    SaveData();

                    return;
                }
            }

            print("Make new MapInfo");

            MapManager.Instance.mapInfo_ToSave.SetupMap();
            SaveData();
        }
    }
    public void SaveData()
    {
        for (int i = 0; i < DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Count; i++)
        {
            if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].mapName == MapManager.Instance.mapInfo_ToSave.mapName)
            {
                DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i] = MapManager.Instance.mapInfo_ToSave;
                DataPersistanceManager.instance.SaveGame();

                return;
            }
        }

        DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Add(MapManager.Instance.mapInfo_ToSave);
        DataPersistanceManager.instance.SaveGame();
    }
}
