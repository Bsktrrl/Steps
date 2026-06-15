using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : Singleton<MapManager>
{
    public static event Action Action_StartIntroSequence;
    public static event Action Action_EndIntroSequence;

    public static event Action<GameObject> Action_SpawnedPlayerObject;
    public static event Action Action_PlayerObject_IsSpawned;
    public static GameObject SpawnedPlayer { get; private set; }

    [Header("Player")]
    [SerializeField] GameObject playerObject;
    GameObject playerObjectInScene;
    public Vector3 playerStartPos;
    public MovementDirection playerStartRot;

    [Header("LayerMask for Raycasting")]
    public LayerMask pickup_LayerMask;
    public LayerMask player_LayerMask;
    public LayerMask playerExclusive_LayerMask;

    [Header("MapInfo")]
    public LevelNames levelName;
    public Map_SaveInfo mapInfo_ToSave = new Map_SaveInfo();

    [Header("Player Object Parent")]
    [SerializeField] GameObject playerObject_Parent;

    [Header("Black Screen")]
    [SerializeField] GameObject canvas;
    public GameObject blackScreen;
    public float fadeDuration_In = 0.75f;
    public float fadeDuration_Out = 0.25f;

    [SerializeField] BlockInfo[] blockInfoList;
    Interactable_Pickup[] pickupInfoList;

    public bool introSequence;
    public bool introSequence_Finished;
    public bool haveIntroSequence = true;

    private Coroutine blackScreenFadeRoutine;


    //--------------------


    private void Awake()
    {
        if (blackScreen)
            blackScreen.SetActive(true);

        if (canvas)
            canvas.SetActive(true);

        SpawnPlayerObject();

        pickup_LayerMask = ~pickup_LayerMask; //Corrects the error that resulted in all raycasts to only focus on the pickups instead of the other way around

        if (haveIntroSequence)
        {
            introSequence = true;

            Action_StartIntroSequence?.Invoke();
        }
    }
    private void Start()
    {
        blockInfoList = FindObjectsOfType<BlockInfo>();
        pickupInfoList = FindObjectsOfType<Interactable_Pickup>();
    }

    private void OnEnable()
    {
        Movement.Action_RespawnPlayer += ShowHiddenObjects;

        DataManager.Action_dataHasLoaded += SaveMapInfo;
        DataManager.Action_dataHasLoaded += InputLevelNameDisplay;
        DataManager.Action_dataHasLoaded += FadeOutBlackScreen;
        DataManager.Action_dataHasLoaded += TurnOnOffIntroSequence;
    }

    private void OnDisable()
    {
        Movement.Action_RespawnPlayer -= ShowHiddenObjects;

        DataManager.Action_dataHasLoaded -= SaveMapInfo;
        DataManager.Action_dataHasLoaded -= InputLevelNameDisplay;
        DataManager.Action_dataHasLoaded -= FadeOutBlackScreen;
        DataManager.Action_dataHasLoaded -= TurnOnOffIntroSequence;
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
        var playerObjectInScene = Instantiate(playerObject);

        playerStartPos = playerStartPos + new Vector3(0, -1 + Movement.Instance.heightOverBlock, 0);

        playerObjectInScene.transform.position = playerStartPos;

        playerObjectInScene.transform.parent = playerObject_Parent.transform;

        SpawnedPlayer = playerObjectInScene;

        SetAbilities();

        Action_SpawnedPlayerObject?.Invoke(SpawnedPlayer);
        Action_PlayerObject_IsSpawned?.Invoke();
    }
    

    public void SetAbilities()
    {
        if (mapInfo_ToSave.abilitiesGotInLevel.Snorkel)
            PlayerStats.Instance.stats.abilitiesGot_Temporary.Snorkel = true;
        if (mapInfo_ToSave.abilitiesGotInLevel.Flippers)
            PlayerStats.Instance.stats.abilitiesGot_Temporary.Flippers = true;
        if (mapInfo_ToSave.abilitiesGotInLevel.OxygenTank)
            PlayerStats.Instance.stats.abilitiesGot_Temporary.OxygenTank = true;

        if (mapInfo_ToSave.abilitiesGotInLevel.DrillHelmet)
            PlayerStats.Instance.stats.abilitiesGot_Temporary.DrillHelmet = true;
        if (mapInfo_ToSave.abilitiesGotInLevel.DrillBoots)
            PlayerStats.Instance.stats.abilitiesGot_Temporary.DrillBoots = true;

        if (mapInfo_ToSave.abilitiesGotInLevel.SpringShoes)
            PlayerStats.Instance.stats.abilitiesGot_Temporary.SpringShoes = true;
        if (mapInfo_ToSave.abilitiesGotInLevel.HandDrill)
            PlayerStats.Instance.stats.abilitiesGot_Temporary.HandDrill = true;

        if (mapInfo_ToSave.abilitiesGotInLevel.GrapplingHook)
            PlayerStats.Instance.stats.abilitiesGot_Temporary.GrapplingHook = true;
        if (mapInfo_ToSave.abilitiesGotInLevel.ClimingGloves)
            PlayerStats.Instance.stats.abilitiesGot_Temporary.ClimingGloves = true;
    }

    public void ShowHiddenObjects()
    {
        foreach (BlockInfo block in blockInfoList)
        {
            if (block.gameObject)
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
    }


    //--------------------


    public void FadeOutBlackScreen()
    {
        StopBlackScreenFadeRoutine();
        blackScreenFadeRoutine = StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        if (blackScreen == null)
            yield break;

        Image blackScreenImage = blackScreen.GetComponent<Image>();
        Image blackScreenIconImage = blackScreen.GetComponent<LoadingIcon>().loadingIcon.GetComponent<Image>();

        if (blackScreenImage == null || blackScreenIconImage == null)
            yield break;

        blackScreen.SetActive(true);

        AudioSettingsManager.Instance.PrepareMasterVolumeFadeUp();

        yield return new WaitForSecondsRealtime(1f);

        Color color = blackScreenImage.color;
        float startAlpha = color.a;
        float duration = Mathf.Max(0.0001f, fadeDuration_In);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            float alpha = Mathf.Lerp(startAlpha, 0f, t);

            color.a = alpha;
            blackScreenImage.color = color;
            blackScreenIconImage.color = color;

            AudioSettingsManager.Instance.FadeMasterVolumeUp(t);

            yield return null;
        }

        color.a = 0f;
        blackScreenImage.color = color;
        blackScreenIconImage.color = color;

        blackScreen.SetActive(false);

        AudioSettingsManager.Instance.FinishMasterVolumeFadeUp();

        blackScreenFadeRoutine = null;
    }
    public void FadeInBlackScreen()
    {
        StopBlackScreenFadeRoutine();
        blackScreenFadeRoutine = StartCoroutine(FadeInBlackScreenCoroutine());
    }

    public IEnumerator FadeInBlackScreenCoroutine()
    {
        if (blackScreen == null)
            yield break;

        Image blackScreenImage = blackScreen.GetComponent<Image>();
        Image blackScreenIconImage = blackScreen.GetComponent<LoadingIcon>().loadingIcon.GetComponent<Image>();

        if (blackScreenImage == null || blackScreenIconImage == null)
            yield break;

        blackScreen.SetActive(true);

        AudioSettingsManager.Instance.PrepareMasterVolumeFadeDown();

        Color color = blackScreenImage.color;
        float startAlpha = color.a;
        float duration = Mathf.Max(0.0001f, fadeDuration_Out);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            float alpha = Mathf.Lerp(startAlpha, 1f, t);

            color.a = alpha;
            blackScreenImage.color = color;
            blackScreenIconImage.color = color;

            AudioSettingsManager.Instance.FadeMasterVolumeDown(t);

            yield return null;
        }

        color.a = 1f;
        blackScreenImage.color = color;
        blackScreenIconImage.color = color;

        AudioSettingsManager.Instance.FinishMasterVolumeFadeDown();

        blackScreenFadeRoutine = null;
    }
    private void StopBlackScreenFadeRoutine()
    {
        if (blackScreenFadeRoutine != null)
        {
            StopCoroutine(blackScreenFadeRoutine);
            blackScreenFadeRoutine = null;
        }
    }

    public void Action_EndIntroSequence_Invoke()
    {
        Action_EndIntroSequence?.Invoke();
    }


    //--------------------


    void TurnOnOffIntroSequence()
    {
        if (DataManager.Instance.settingData_StoreList.currentSkipIntro == SkipIntro.Yes)
        {
            haveIntroSequence = false;
        }
        else if (DataManager.Instance.settingData_StoreList.currentSkipIntro == SkipIntro.No)
        {
            haveIntroSequence = true;
        }
    }
}

[Serializable]
public class AudioTrack
{
    public AudioClip track;
    public float volume;
    public float pitch;
}

public enum LevelNames
{
    [InspectorName("None")] None,

    [InspectorName("Rivergreen / Level 1")] Rivergreen_Lv1,
    [InspectorName("Rivergreen / Level 2")] Rivergreen_Lv2,
    [InspectorName("Rivergreen / Level 3")] Rivergreen_Lv3,
    [InspectorName("Rivergreen / Level 4")] Rivergreen_Lv4,
    [InspectorName("Rivergreen / Level 5")] Rivergreen_Lv5,

    [InspectorName("Sandlands / Level 1")] Sandlands_Lv1,
    [InspectorName("Sandlands / Level 2")] Sandlands_Lv2,
    [InspectorName("Sandlands / Level 3")] Sandlands_Lv3,
    [InspectorName("Sandlands / Level 4")] Sandlands_Lv4,
    [InspectorName("Sandlands / Level 5")] Sandlands_Lv5,

    [InspectorName("Frostfield / Level 1")] Frostfield_Lv1,
    [InspectorName("Frostfield / Level 2")] Frostfield_Lv2,
    [InspectorName("Frostfield / Level 3")] Frostfield_Lv3,
    [InspectorName("Frostfield / Level 4")] Frostfield_Lv4,
    [InspectorName("Frostfield / Level 5")] Frostfield_Lv5,

    [InspectorName("Firevein / Level 1")] Firevein_Lv1,
    [InspectorName("Firevein / Level 2")] Firevein_Lv2,
    [InspectorName("Firevein / Level 3")] Firevein_Lv3,
    [InspectorName("Firevein / Level 4")] Firevein_Lv4,
    [InspectorName("Firevein / Level 5")] Firevein_Lv5,

    [InspectorName("Witchmire / Level 1")] Witchmire_Lv1,
    [InspectorName("Witchmire / Level 2")] Witchmire_Lv2,
    [InspectorName("Witchmire / Level 3")] Witchmire_Lv3,
    [InspectorName("Witchmire / Level 4")] Witchmire_Lv4,
    [InspectorName("Witchmire / Level 5")] Witchmire_Lv5,

    [InspectorName("Metalworks / Level 1")] Metalworks_Lv1,
    [InspectorName("Metalworks / Level 2")] Metalworks_Lv2,
    [InspectorName("Metalworks / Level 3")] Metalworks_Lv3,
    [InspectorName("Metalworks / Level 4")] Metalworks_Lv4,
    [InspectorName("Metalworks / Level 5")] Metalworks_Lv5,

    [InspectorName("HUB")] HUB,
}