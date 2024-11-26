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
            if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List != null)
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
            }
        }

        MapManager.Instance.mapInfo_ToSave.SetupMap();
        SaveData();
    }
    public void SaveData()
    {
        if (DataManager.Instance.mapInfo_StoreList != null)
        {
            if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List != null)
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
            }
            else
            {
                List<Map_SaveInfo> Map_SaveInfoList_Temp = new List<Map_SaveInfo>();
                DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List = Map_SaveInfoList_Temp;
                DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Add(MapManager.Instance.mapInfo_ToSave);
                DataPersistanceManager.instance.SaveGame();
            }
        }
    }
}
