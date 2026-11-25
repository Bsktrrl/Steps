using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class DataManager : Singleton<DataManager>, IDataPersistance
{
    #region General Variables
    public GameData gameData;

    public static Action Action_dataIsSaving;
    public static Action Action_dataHasLoaded;

    public bool dataHasLoaded_Check;
    #endregion


    //--------------------


    #region Variables

    //Data to run once
    /*[HideInInspector]*/public OneTimeRunData oneTimeRunData_Store = new OneTimeRunData();

    //NewGame
    /*[HideInInspector]*/
    public bool haveStartedNewGame_Store = new bool();

    //MenuState
    /*[HideInInspector]*/public MenuState menuState_Store = new MenuState();

    //Player stored Stats info
    /*[HideInInspector]*/ public Stats playerStats_Store = new Stats();

    //All stored Maps info
    /*[HideInInspector]*/ public Map_SaveInfoList mapInfo_StoreList = new Map_SaveInfoList();

    //Overworld States
    /*[HideInInspector]*/ public OverWorldStates overWorldStates_StoreList = new OverWorldStates();

    //Settings
    /*[HideInInspector]*/ public SettingData settingData_StoreList = new SettingData();

    //Map Display
    /*[HideInInspector]*/ public MapNameDisplay mapNameDisplay_Store = new MapNameDisplay();

    //NPC Data
    /*[HideInInspector]*/ public CharatersData charatersData_Store = new CharatersData();

    //Block Skins
    /*[HideInInspector]*/ public SkinInfo skinsInfo_Store = new SkinInfo();

    //Text Database
    /*[HideInInspector]*/public Game_TextDatabase game_TextDatabase_Store = new Game_TextDatabase();
    [SerializeField] TextAsset localizationSource;
    bool localizationFromAsset;

    //PlayerStatsData
    /*[HideInInspector]*/ public PlayerStatsData playerStatsData_Store = new PlayerStatsData();

    //PlayerStatsData
    /*[HideInInspector]*/ public GlueplantStandStats glueplantStandStats_Store = new GlueplantStandStats();

    //PlayerStatsData
    /*[HideInInspector]*/ public SwitchesData switchesData_Store = new SwitchesData();

    //PlayerStatsData
    /*[HideInInspector]*/ public PermanentAbilitiesReceived permanentAbilitiesReceived_Store = new PermanentAbilitiesReceived();

    //PlayerStatsData
    /*[HideInInspector]*/ public PlayerStatistics playerStatistics_Store = new PlayerStatistics();


    #endregion


    //--------------------


    public void LoadData(GameData gameData)
    {
        GetSavedDataFromFile(gameData);
        var textData = FindAnyObjectByType<TextData>();
        bool hasSceneText = textData && textData.gameObject.scene == gameObject.scene && textData.HasSource;
        localizationFromAsset = hasSceneText || localizationSource;
        if (hasSceneText)
            textData.ReadExcelSheet();
        else if (localizationSource)
            game_TextDatabase_Store = TextData.ReadDatabase(localizationSource);

        Player_Statistics_Manager.Active?.BeginTracking(this);
        LoadDataIntoProject(gameData);

        StartCoroutine(LoadingDelay(0.01f));
    }
    IEnumerator LoadingDelay(float time)
    {
        yield return new WaitForSeconds(time);

        print("------------------------------");

        dataHasLoaded_Check = true;
        Action_dataHasLoaded?.Invoke();
    }


    //--------------------


    void GetSavedDataFromFile(GameData gameData)
    {
        this.haveStartedNewGame_Store = gameData.haveStartedNewGame_Save;

        this.menuState_Store = gameData.menuState_Save;
        this.playerStats_Store = gameData.playerStats_Save;
        this.mapInfo_StoreList = gameData.mapInfo_SaveList;
        this.overWorldStates_StoreList = gameData.overWorldStates_SaveList;
        this.settingData_StoreList = gameData.settingData_SaveList;
        this.mapNameDisplay_Store = gameData.mapNameDisplay_Save;
        this.charatersData_Store = gameData.charatersData_Save;
        this.skinsInfo_Store = gameData.skinsInfo_Save;
        // Older saves can still provide text in a scene without a packaged source.
        if ((game_TextDatabase_Store?.gameText_LanguageList?.Count ?? 0) == 0)
            this.game_TextDatabase_Store = gameData.game_TextDatabase_Save ?? new Game_TextDatabase();

        this.playerStatsData_Store = gameData.PlayerStatsData_Save ?? new PlayerStatsData();

        this.oneTimeRunData_Store = gameData.oneTimeRunData_Save;

        this.glueplantStandStats_Store = gameData.glueplantStandStats_Save ?? new GlueplantStandStats();
        // Earlier saves can have all four stands filled without the level completion flag.
        if (mapInfo_StoreList?.map_SaveInfo_List != null)
        {
            foreach (Map_SaveInfo savedMap in mapInfo_StoreList.map_SaveInfo_List)
            {
                if (savedMap != null && glueplantStandStats_Store.GetForLevel(savedMap.mapName)?.CountTaken == StandStats.StandsPerLevel)
                    savedMap.isCompleted = true;
            }
        }
        this.switchesData_Store = gameData.switchesData_Save;

        this.permanentAbilitiesReceived_Store = gameData.permanentAbilitiesReceived_Save;

        this.playerStatistics_Store = gameData.playerStatistics_Save ?? new PlayerStatistics();
    }

    void LoadDataIntoProject(GameData gameData)
    {
        SaveLoad_MapInfo tempMapInfo = FindAnyObjectByType<SaveLoad_MapInfo>();
        if (tempMapInfo)
        {
            SaveLoad_MapInfo.Instance.LoadData();
            print("1. MapInfo has Loaded");
        }

        SaveLoad_PlayerStats tempPlayerStat = FindAnyObjectByType<SaveLoad_PlayerStats>();
        if (tempPlayerStat)
        {
            SaveLoad_PlayerStats.Instance.LoadData();
            print("2. PlayerStat has Loaded");
        }

        // Restore the level's abilities before the startup notifications read player stats.
        MapManager map = MapManager.Instance;
        if (map && map.gameObject.scene == SceneManager.GetActiveScene())
            map.SetAbilities();

        OverWorldManager tempOverworld = FindAnyObjectByType<OverWorldManager>();
        if (tempOverworld)
        {
            OverWorldManager.Instance.LoadUIElementState_IfExitsFromALevel();
            print("3. UIElementState has Loaded");
        }

        if (tempOverworld)
        {
            SettingsManager.Instance.LoadData();
            print("4. SettingData has Loaded");
        }

        NPCManager tempNPCManager = FindAnyObjectByType<NPCManager>();
        if (tempNPCManager)
        {
            NPCManager.Instance.LoadData();
            print("5. NPCManager has Loaded");
        }

        SkinsManager tempSkinsManager = FindAnyObjectByType<SkinsManager>();
        if (tempSkinsManager)
        {
            SkinsManager.Instance.LoadData();
            print("6. SkinsManager has Loaded");
        }
    }

    public void SaveData(ref GameData gameData)
    {
        Action_dataIsSaving?.Invoke();

        //Input what variables to save upon saving
        gameData.menuState_Save = this.menuState_Store;
        gameData.mapInfo_SaveList = this.mapInfo_StoreList;
        gameData.playerStats_Save = this.playerStats_Store;
        gameData.overWorldStates_SaveList = this.overWorldStates_StoreList;
        gameData.mapNameDisplay_Save = this.mapNameDisplay_Store;
        gameData.charatersData_Save = this.charatersData_Store;
        gameData.settingData_SaveList = this.settingData_StoreList;
        gameData.skinsInfo_Save = this.skinsInfo_Store;
        gameData.game_TextDatabase_Save = localizationFromAsset ? null : game_TextDatabase_Store;

        gameData.PlayerStatsData_Save = this.playerStatsData_Store;

        gameData.oneTimeRunData_Save = this.oneTimeRunData_Store;

        gameData.haveStartedNewGame_Save = this.haveStartedNewGame_Store;

        gameData.glueplantStandStats_Save = this.glueplantStandStats_Store;
        gameData.switchesData_Save = this.switchesData_Store;

        gameData.permanentAbilitiesReceived_Save = this.permanentAbilitiesReceived_Store;

        gameData.playerStatistics_Save = this.playerStatistics_Store;
    }

    internal GameData CreateNewGameData(bool hasStartedNewGame)
    {
        var newData = new GameData
        {
            haveStartedNewGame_Save = hasStartedNewGame,
            game_TextDatabase_Save = localizationFromAsset ? null : game_TextDatabase_Store
        };

        if (hasStartedNewGame)
        {
            // Copy settings before resetting the intro so a failed write leaves live settings intact.
            if (settingData_StoreList != null)
            {
                var settings = JsonUtility.FromJson<SettingData>(JsonUtility.ToJson(settingData_StoreList));
                settings.currentSkipIntro = newData.settingData_SaveList.currentSkipIntro;
                newData.settingData_SaveList = settings;
            }

            newData.oneTimeRunData_Save.startVolum_Values = oneTimeRunData_Store?.startVolum_Values == true;
        }

        newData.PlayerStatsData_Save.sessionStats.session_No = 1;
        newData.playerStatistics_Save.initialized = true;
        newData.playerStatistics_Save.sessions = 1;
        newData.skinsInfo_Save.skinWardrobeInfo = new SkinsWardrobeInfo { skin_Default = WardrobeSkinState.Selected };
        newData.skinsInfo_Save.skinHatInfo = new SkinsHatInfo();
        return newData;
    }

    internal void ApplyNewGameData(GameData newData)
    {
        GetSavedDataFromFile(newData);
        PlayerStats playerStats = FindAnyObjectByType<PlayerStats>();
        if (playerStats)
            playerStats.stats = playerStats_Store;
    }
}

[Serializable]
public class OneTimeRunData
{
    //English Start Language
    /*[HideInInspector]*/ public bool startLanguage_English = new bool();

    //Volum Start Values
    /*[HideInInspector]*/ public bool startVolum_Values = new bool();

    //Force FirstIntroSequence
    /*[HideInInspector]*/ public bool forceIntroSequenceInFirstLevel = new bool();

    //Tutorial
    /*[HideInInspector]*/ public bool tutorial_PlayerSpawned = new bool();
    /*[HideInInspector]*/ public bool tutorial_Finished = new bool();

    //Footprint
    /*[HideInInspector]*/public bool pickup_FirstFootprint = new bool();

    //Skin
    /*[HideInInspector]*/ public bool pickup_FirstSkin = new bool();
    
    //Skin
    /*[HideInInspector]*/ public bool glueplantHUB_PickedUp = new bool();

    //Skin
    /*[HideInInspector]*/public List<TutorialDataParts> tutorialSegmenets = new List<TutorialDataParts>();
}

