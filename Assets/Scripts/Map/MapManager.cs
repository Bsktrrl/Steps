using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [Header("Player")]
    [SerializeField] GameObject playerObject;
    GameObject playerObjectInScene;
    public Vector3 playerStartPos;

    [Header("LayerMask for Raycasting")]
    public LayerMask pickup_LayerMask;

    [Header("Sound")]
    public List<AudioTrack> mapAudioList;
    [SerializeField] List<AudioSource> mapAudioSourceList;

    [Header("MapInfo")]
    public Map_SaveInfo mapInfo_ToSave = new Map_SaveInfo();

    BlockInfo[] blockInfoList;
    Interactable_Pickup[] pickupInfoList;


    //--------------------


    private void Awake()
    {
        SpawnPlayerObject();

        pickup_LayerMask = ~pickup_LayerMask; //Corrects the error that resulted in all raycasts to only focus on the pickups instead of the other way around
    }
    private void Start()
    {
        blockInfoList = FindObjectsOfType<BlockInfo>();
        pickupInfoList = FindObjectsOfType<Interactable_Pickup>();

        playerStartPos = playerStartPos + new Vector3(0, -1 + Movement.Instance.heightOverBlock, 0);

        PlayAudio();
    }

    private void OnEnable()
    {
        Movement.Action_RespawnPlayer += ShowHiddenObjects;
        DataManager.Action_dataHasLoaded += SaveMapInfo;
    }

    private void OnDisable()
    {
        Movement.Action_RespawnPlayer -= ShowHiddenObjects;
        DataManager.Action_dataHasLoaded -= SaveMapInfo;
    }



    //--------------------


    public void SaveMapInfo()
    {
        SaveLoad_MapInfo.Instance.SaveGame();
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
