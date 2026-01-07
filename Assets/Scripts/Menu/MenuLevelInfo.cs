using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLevelInfo : MonoBehaviour
{
    public static event Action menuLevelInfo_hasLoaded;

    [Header("MapInfo")]
    public Map_SaveInfoList mapInfo_ToSave;


    //--------------------


    private void OnEnable()
    {
        print("1. MenuLevelInfo - OnEnable");
        DataManager.Action_dataHasLoaded += LoadMapInfo;
    }

    private void OnDisable()
    {
        print("2. MenuLevelInfo - OnDisable");
        DataManager.Action_dataHasLoaded -= LoadMapInfo;
    }


    //--------------------


    void LoadMapInfo()
    {
        mapInfo_ToSave = DataManager.Instance.mapInfo_StoreList;

        menuLevelInfo_hasLoaded?.Invoke();
    }
}
