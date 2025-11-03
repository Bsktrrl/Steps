using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : Singleton<MapManager>
{
    [Header("Player")]
    [SerializeField] GameObject playerObject;
    GameObject playerObjectInScene;
    public Vector3 playerStartPos;
    public MovementDirection playerStartRot;

    [Header("Stats")]
    public float timeUsedInLevel = 0;
    public int stepCount = 0;
    public int respawnCount = 0;
    public int abilitiesPickedUp = 0;
    public int cameraRotated = 0;

    public int swimCounter = 0;
    public int swiftSwimCounter = 0;
    public int jumpCounter = 0;
    public int dashCounter = 0;
    public int ascendCounter = 0;
    public int descendCounter = 0;
    public int grapplingHookCounter = 0;
    public int ceilingGrabCounter = 0;

    [Header("LayerMask for Raycasting")]
    public LayerMask pickup_LayerMask;
    public LayerMask player_LayerMask;

    [Header("Sound")]
    public List<AudioTrack> mapAudioList;
    [SerializeField] List<AudioSource> mapAudioSourceList;

    [Header("MapInfo")]
    public Map_SaveInfo mapInfo_ToSave = new Map_SaveInfo();

    [Header("Player Object Parent")]
    [SerializeField] GameObject playerObject_Parent;

    [Header("Black Screen")]
    [SerializeField] GameObject blackScreen;
    float fadeDuration_In = 0.75f;
    float fadeDuration_Out = 0.25f;

    BlockInfo[] blockInfoList;
    Interactable_Pickup[] pickupInfoList;


    //--------------------


    private void Awake()
    {
        blackScreen.SetActive(true);

        SpawnPlayerObject();

        pickup_LayerMask = ~pickup_LayerMask; //Corrects the error that resulted in all raycasts to only focus on the pickups instead of the other way around
    }
    private void Start()
    {
        blockInfoList = FindObjectsOfType<BlockInfo>();
        pickupInfoList = FindObjectsOfType<Interactable_Pickup>();

        //Movement.Instance.RotatePlayerBody(180);

        PlayAudio();
    }
    private void Update()
    {
        timeUsedInLevel += Time.deltaTime;
    }

    private void OnEnable()
    {
        Movement.Action_RespawnPlayer += ShowHiddenObjects;
        Movement.Action_RespawnPlayer += UpdateRespawnCount;

        Movement.Action_StepTaken += UpdateStepCount;
        Interactable_Pickup.Action_AbilityPickupGot += UpdateAbilitiesPickedUp;

        DataManager.Action_dataHasLoaded += SaveMapInfo;
        DataManager.Action_dataHasLoaded += InputLevelNameDisplay;
        DataManager.Action_dataHasLoaded += FadeOutBlackScreen;
    }

    private void OnDisable()
    {
        Movement.Action_RespawnPlayer -= ShowHiddenObjects;
        Movement.Action_RespawnPlayer -= UpdateRespawnCount;

        Movement.Action_StepTaken -= UpdateStepCount;
        Interactable_Pickup.Action_AbilityPickupGot -= UpdateAbilitiesPickedUp;

        DataManager.Action_dataHasLoaded -= SaveMapInfo;
        DataManager.Action_dataHasLoaded -= InputLevelNameDisplay;
        DataManager.Action_dataHasLoaded -= FadeOutBlackScreen;
    }



    //--------------------


    public void SaveMapInfo()
    {
        SaveLoad_MapInfo.Instance.SaveGame();
    }
    void InputLevelNameDisplay()
    {
        mapInfo_ToSave.mapNameDisplay.mapNameDisplay_norwegian = DataManager.Instance.mapNameDisplay_Store.mapNameDisplay_norwegian;
        mapInfo_ToSave.mapNameDisplay.mapNameDisplay_english = DataManager.Instance.mapNameDisplay_Store.mapNameDisplay_english;
        mapInfo_ToSave.mapNameDisplay.mapNameDisplay_german = DataManager.Instance.mapNameDisplay_Store.mapNameDisplay_german;
        mapInfo_ToSave.mapNameDisplay.mapNameDisplay_chinese = DataManager.Instance.mapNameDisplay_Store.mapNameDisplay_chinese;
        mapInfo_ToSave.mapNameDisplay.mapNameDisplay_japanese = DataManager.Instance.mapNameDisplay_Store.mapNameDisplay_japanese;
        mapInfo_ToSave.mapNameDisplay.mapNameDisplay_korean = DataManager.Instance.mapNameDisplay_Store.mapNameDisplay_korean;
    }


    //--------------------


    void SpawnPlayerObject()
    {
        playerObjectInScene = Instantiate(playerObject);

        playerStartPos = playerStartPos + new Vector3(0, -1 + Movement.Instance.heightOverBlock, 0);
        playerObjectInScene.transform.position = playerStartPos;

        playerObjectInScene.transform.parent = playerObject_Parent.transform;
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


    void UpdateStepCount()
    {
        stepCount++;
    }
    void UpdateRespawnCount()
    {
        respawnCount++;
    }
    void UpdateAbilitiesPickedUp()
    {
        abilitiesPickedUp++;
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


    //--------------------


    public void FadeOutBlackScreen()
    {
        StartCoroutine(FadeOutCoroutine());
    }
    private IEnumerator FadeOutCoroutine()
    {
        Image blackScreenImage = blackScreen.GetComponent<Image>();

        Color color = blackScreenImage.color;
        float startAlpha = color.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration_In)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration_In);
            color.a = alpha;
            blackScreenImage.color = color;
            yield return null;
        }

        // Ensure it's fully transparent at the end
        color.a = 0f;
        blackScreenImage.color = color;
        blackScreen.SetActive(false);
    }
    public void FadeInBlackScreen()
    {
        StartCoroutine(FadeInBlackScreenCoroutine());
    }
    public IEnumerator FadeInBlackScreenCoroutine()
    {
        blackScreen.SetActive(true);

        Image blackScreenImage = blackScreen.GetComponent<Image>();

        Color color = blackScreenImage.color;
        float startAlpha = color.a; // should be 0 if transparent
        float elapsed = 0f;

        while (elapsed < fadeDuration_Out)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 1f, elapsed / fadeDuration_Out); // fade to opaque
            color.a = alpha;
            blackScreenImage.color = color;
            yield return null;
        }

        // Ensure it's fully opaque at the end
        color.a = 1f;
        blackScreenImage.color = color;
    }
}

[Serializable]
public class AudioTrack
{
    public AudioClip track;
    public float volume;
}
