using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public static event Action mapInfo_hasLoaded;

    [Header("Player")]
    [SerializeField] GameObject playerObject;
    public Vector3 playerStartPos;
    [SerializeField] GameObject playerObjectInScene;

    [Header("MapManager")]
    public Map_SaveInfo mapInfo_ToSave;

    BlockInfo[] blockInfoList;


    //--------------------


    private void Awake()
    {
        SpawnPlayerObject();
    }
    private void Start()
    {
        blockInfoList = FindObjectsOfType<BlockInfo>();
    }

    private void OnEnable()
    {
        DataManager.datahasLoaded += LoadMapInfo;
        PlayerStats.Action_RespawnPlayer += ShowHiddenObjects;
    }

    private void OnDisable()
    {
        DataManager.datahasLoaded -= LoadMapInfo;
        PlayerStats.Action_RespawnPlayer -= ShowHiddenObjects;
    }


    //--------------------


    void LoadMapInfo()
    {
        SaveLoad_MapInfo.Instance.LoadData();

        mapInfo_hasLoaded?.Invoke();
    }
    public void SaveMapInfo()
    {
        SaveLoad_MapInfo.Instance.SaveData();
    }


    //--------------------


    void SpawnPlayerObject()
    {
        playerObjectInScene = Instantiate(playerObject);
        playerObjectInScene.transform.position = playerStartPos;
    }

    public void ShowHiddenObjects()
    {
        foreach (BlockInfo block in blockInfoList)
        {
            if (!block.gameObject.activeInHierarchy)
            {
                block.gameObject.SetActive(true);
            }

            if (block.gameObject.GetComponent<Block_Falling>())
            {
                block.gameObject.GetComponent<Block_Falling>().ResetBlock();
            }

            if (block.gameObject.GetComponent<Block_Weak>())
            {
                block.gameObject.GetComponent<Block_Weak>().ResetBlock();
            }
        }
    }
}
