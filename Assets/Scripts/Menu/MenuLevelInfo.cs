using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLevelInfo : Singleton<MenuLevelInfo>
{
    public static event Action menuLevelInfo_hasLoaded;

    [Header("MapInfo")]
    public Map_SaveInfoList mapInfo_ToSave;


    //--------------------


    private void OnEnable()
    {
        DataManager.datahasLoaded += LoadMapInfo;
    }

    private void OnDisable()
    {
        DataManager.datahasLoaded -= LoadMapInfo;
    }


    //--------------------


    void LoadMapInfo()
    {
        mapInfo_ToSave = DataManager.Instance.mapInfo_StoreList;

        menuLevelInfo_hasLoaded?.Invoke();
    }
}
