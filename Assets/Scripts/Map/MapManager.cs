using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public static event Action mapInfo_hasLoaded;

    [Header("Player")]
    [SerializeField] GameObject playerObject;
    [SerializeField] GameObject playerObjectInScene;
    public Vector3 playerStartPos;

    [Header("Sound")]
    public List<AudioTrack> mapAudioList;
    public List<AudioSource> mapAudioSourceList;

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

        PlayAudio();
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


    //--------------------


    void PlayAudio()
    {
        for (int i = 0; i < mapAudioList.Count; i++)
        {
            //Make a new AudioSource
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            mapAudioSourceList.Add(audioSource);

            mapAudioSourceList[mapAudioSourceList.Count - 1].clip = mapAudioList[i].track;

            if (mapAudioList[i].volume > 0)
                mapAudioSourceList[mapAudioSourceList.Count - 1].volume = mapAudioList[i].volume;

            mapAudioSourceList[mapAudioSourceList.Count - 1].loop = true;

            mapAudioSourceList[mapAudioSourceList.Count - 1].Play();
        }
    }
}

[Serializable]
public class AudioTrack
{
    public AudioClip track;
    public float volume;
}
