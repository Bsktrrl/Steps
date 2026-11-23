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

    // Remember a HUB visit across the return to the main menu.
    private static bool returnFromHUB;

    public static bool ConsumeHUBReturn()
    {
        bool result = returnFromHUB;
        returnFromHUB = false;
        return result;
    }

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

    public bool introSequence;
    public bool introSequence_Finished;

    // The map intro and the first HUB spawn/dialogue are separate sequences.
    public bool IntroInputLocked => introSequence || !introSequence_Finished;

    public bool GameplayInputLocked => IntroInputLocked ||
        (levelName == LevelNames.HUB && HUBTutorial.Instance != null &&
         HUBTutorial.Instance.GameplayInputLocked);

    private Coroutine blackScreenFadeRoutine;


    //--------------------


    private void Awake()
    {
        returnFromHUB = levelName == LevelNames.HUB;

        if (blackScreen)
            blackScreen.SetActive(true);

        if (canvas)
            canvas.SetActive(true);

        // Lock input before instantiating the player and enabling its controls.
        introSequence = false;
        introSequence_Finished = false;

        SpawnPlayerObject();

        pickup_LayerMask = ~pickup_LayerMask; //Corrects the error that resulted in all raycasts to only focus on the pickups instead of the other way around
    }
    private void Start()
    {
        blockInfoList = FindObjectsOfType<BlockInfo>();

        PlayerManager.Instance.PauseGame();
    }

    private void OnEnable()
    {
        Movement.Action_RespawnPlayer += ShowHiddenObjects;

        DataManager.Action_dataHasLoaded += InputLevelNameDisplay;
        DataManager.Action_dataHasLoaded += SaveMapInfo;
        DataManager.Action_dataHasLoaded += FadeOutBlackScreen;
    }

    private void OnDisable()
    {
        Movement.Action_RespawnPlayer -= ShowHiddenObjects;

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
        DataManager data = DataManager.Instance;
        string sceneName = gameObject.scene.name;
        MapNameDisplay menuName = data.mapNameDisplay_Store;
        if (menuName?.sceneName != sceneName)
            menuName = null;

        MapNameDisplay display = mapInfo_ToSave.mapNameDisplay ??= new MapNameDisplay();
        display.mapNameDisplay_norwegian = ResolveName(Languages.Norwegian, menuName?.mapNameDisplay_norwegian, display.mapNameDisplay_norwegian);
        display.mapNameDisplay_english = ResolveName(Languages.English, menuName?.mapNameDisplay_english, display.mapNameDisplay_english);
        display.mapNameDisplay_german = ResolveName(Languages.German, menuName?.mapNameDisplay_german, display.mapNameDisplay_german);
        display.mapNameDisplay_chinese = ResolveName(Languages.Chinese, menuName?.mapNameDisplay_chinese, display.mapNameDisplay_chinese);
        display.mapNameDisplay_japanese = ResolveName(Languages.Japanese, menuName?.mapNameDisplay_japanese, display.mapNameDisplay_japanese);
        display.mapNameDisplay_korean = ResolveName(Languages.Korean, menuName?.mapNameDisplay_korean, display.mapNameDisplay_korean);

        string ResolveName(Languages language, string fromMenu, string fromSave)
        {
            if (!string.IsNullOrWhiteSpace(fromMenu))
                return fromMenu;

            string localized = data.game_TextDatabase_Store?.GetLevelName(levelName, language);
            if (!string.IsNullOrWhiteSpace(localized))
                return localized;

            return string.IsNullOrWhiteSpace(fromSave) ? sceneName : fromSave;
        }
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
        if (!SpawnedPlayer || SpawnedPlayer.scene != gameObject.scene ||
            !SpawnedPlayer.TryGetComponent(out PlayerStats playerStats) || playerStats.stats == null || mapInfo_ToSave == null)
            return;

        AbilitiesGot levelAbilities = mapInfo_ToSave.abilitiesGotInLevel ??= new AbilitiesGot();
        GivePlayerTempAbilities startupGrants = GetComponentInChildren<GivePlayerTempAbilities>();
        if (startupGrants && startupGrants.isActiveAndEnabled)
            startupGrants.ApplyTo(levelAbilities);

        // Keep the live stats separate from the map record and drop grants from the previous level.
        playerStats.stats.abilitiesGot_Temporary = levelAbilities.Copy();
    }

    public void ShowHiddenObjects()
    {
        foreach (BlockInfo block in blockInfoList)
        {
            GameObject blockObject = block.gameObject;
            if (blockObject)
            {
                if (!blockObject.activeInHierarchy)
                {
                    blockObject.SetActive(true);
                }

                if (blockObject.TryGetComponent(out Block_Falling fallingBlock))
                {
                    fallingBlock.ResetBlock();
                }
            }
        }
    }


    //--------------------


    public void FadeOutBlackScreen()
    {
        if (introSequence_Finished || blackScreenFadeRoutine != null)
            return;

        blackScreenFadeRoutine = StartCoroutine(RunLevelIntro());
    }

    private IEnumerator RunLevelIntro()
    {
        // Startup listeners can finish loading stats and UI before the camera moves.
        yield return null;

        GlueplantCamera introCamera = FindAnyObjectByType<GlueplantCamera>();
        if (introCamera && introCamera.gameObject.scene != gameObject.scene)
            introCamera = null;

        bool playIntro = DataManager.Instance.settingData_StoreList?.currentSkipIntro == SkipIntro.No;
        introSequence = playIntro && introCamera && introCamera.TryStartTravel(SpawnedPlayer);
        if (introSequence)
            Action_StartIntroSequence?.Invoke();
        else if (introCamera)
            introCamera.UsePlayerView();

        yield return FadeOutCoroutine();

        while (introCamera && introCamera.IsTravelingThisIntro)
            yield return null;

        blackScreenFadeRoutine = null;
        introSequence = false;
        introSequence_Finished = true;
        var holeShader = FindAnyObjectByType<HoleShaderOnOffScript>();
        if (holeShader)
            holeShader.HoleShader_On();
        Action_EndIntroSequence?.Invoke();
    }

    private IEnumerator FadeOutCoroutine()
    {
        if (blackScreen == null)
            yield break;

        Image blackScreenImage = blackScreen.GetComponent<Image>();
        if (!blackScreenImage)
        {
            blackScreen.SetActive(false);
            yield break;
        }

        LoadingIcon icon = blackScreen.GetComponent<LoadingIcon>();
        Image blackScreenIconImage = icon && icon.loadingIcon ? icon.loadingIcon.GetComponent<Image>() : null;

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
            if (blackScreenIconImage)
                blackScreenIconImage.color = color;

            AudioSettingsManager.Instance.FadeMasterVolumeUp(t);

            yield return null;
        }

        color.a = 0f;
        blackScreenImage.color = color;
        if (blackScreenIconImage)
            blackScreenIconImage.color = color;

        blackScreen.SetActive(false);

        AudioSettingsManager.Instance.FinishMasterVolumeFadeUp();
    }

    public void FadeInBlackScreen()
    {
        StartFadeInBlackScreen();
    }

    internal Coroutine StartFadeInBlackScreen()
    {
        StopBlackScreenFadeRoutine();
        blackScreenFadeRoutine = StartCoroutine(FadeInBlackScreenCoroutine());
        return blackScreenFadeRoutine;
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
