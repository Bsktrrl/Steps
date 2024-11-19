using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public static event Action mapInfo_hasLoaded;

    [Header("Player")]
    [SerializeField] GameObject playerObject;
    [SerializeField] Vector3 playerStartPos;
    [SerializeField] GameObject playerObjectInScene;

    [Header("MapManager")]
    public Map_SaveInfo mapInfo_ToSave;


    //--------------------


    private void Awake()
    {
        SpawnPlayerObject();
    }

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
        SaveLoad_MapInfo.Instance.LoadData();

        mapInfo_hasLoaded?.Invoke();
    }
    public void SaveMapInfo()
    {
        SaveLoad_PlayerStats.Instance.SaveData();
    }


    //--------------------


    void SpawnPlayerObject()
    {
        playerObjectInScene = Instantiate(playerObject);
        playerObjectInScene.transform.position = playerStartPos;
    }
}
