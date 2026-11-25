using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistanceManager : Singleton<DataPersistanceManager>
{
    public static event Action Action_NewGame;

    [Header("File Storage Config")]
    public string fileName;

    public GameData gameData;
    List<IDataPersistance> dataPersistanceObjects;
    FileDataHandler dataHandler;
    DataManager dataManager;
    MapManager map;
    bool savingEnabled;
    bool canReplaceSave;
    bool savePending;
    bool isSaving;
    int settingsAdjustments;
    bool settingsSavePending;
    private static bool quitPrepared;
    private bool waitingForQuitReports;
    private double quitReportDeadline;

    public static DataPersistanceManager instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetQuitState()
    {
        quitPrepared = false;
    }

    //--------------------


    private void Awake()
    {
        if (instance != null)
        {
            print("Found more than 1 Data Persistence Manager in the scene");
        }

        instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataManager = FindAnyObjectByType<DataManager>();
        map = MapManager.Instance;
        this.dataPersistanceObjects = FindAllDataPersistenceObjects();

        LoadGame();
    }

    private void OnEnable()
    {
        Application.wantsToQuit += HandleQuitRequest;
    }

    private void OnDisable()
    {
        Application.wantsToQuit -= HandleQuitRequest;
        FinishQuitAfterReports();
    }

    private void LateUpdate()
    {
        if (waitingForQuitReports && Time.realtimeSinceStartupAsDouble >= quitReportDeadline)
            FinishQuitAfterReports();

        // Startup listeners finish populating the data, and a goal saves as part of its scene exit.
        if (quitPrepared || (dataManager && !dataManager.dataHasLoaded_Check) || (map && map.IsCompletingLevel))
            return;

        FlushPendingSave();
    }


    //--------------------


    public bool NewGame()
    {
        if (quitPrepared || !savingEnabled)
            return false;

        DataManager data = DataManager.Instance;
        GameData newData = data.CreateNewGameData(hasStartedNewGame: true);
        if (!WriteSave(newData))
            return false;

        savePending = false;
        gameData = newData;
        data.ApplyNewGameData(newData);
        Player_Statistics_Manager.Active?.ResetForNewGame(data);
        Action_NewGame?.Invoke();
        return true;
    }

    public void LoadGame()
    {
        savingEnabled = false;
        SaveLoadResult result = dataHandler.Load(out gameData);
        canReplaceSave = result == SaveLoadResult.Loaded;

        if (result != SaveLoadResult.Loaded)
        {
            // Keep the menu usable while writes are blocked after a failed read.
            DataManager data = DataManager.Instance;
            gameData = data.CreateNewGameData(hasStartedNewGame: false);
            data.ApplyNewGameData(gameData);
            Player_Statistics_Manager.Active?.ResetForNewGame(data);
        }

        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
            dataPersistanceObj.LoadData(gameData);

        savingEnabled = result != SaveLoadResult.Failed;
        if (result == SaveLoadResult.Missing)
            SaveGame();
    }

    public void SaveGame()
    {
        if (savingEnabled)
            savePending = true;
    }

    internal void BeginSettingsAdjustment() => settingsAdjustments++;

    internal void EndSettingsAdjustment()
    {
        if (settingsAdjustments == 0)
            return;

        settingsAdjustments--;
        if (settingsAdjustments == 0 && settingsSavePending)
            SaveGame();
    }

    internal void SaveSettings()
    {
        if (!savingEnabled)
            return;

        if (settingsAdjustments > 0)
            settingsSavePending = true;
        else
            SaveGame();
    }

    internal void SaveGameImmediately()
    {
        if (quitPrepared)
            return;

        SaveGame();
        FlushPendingSave();
    }

    private bool FlushPendingSave()
    {
        if (!savingEnabled || !savePending || isSaving)
            return false;

        savePending = false;
        isSaving = true;
        try
        {
            Player_Statistics_Manager.Active?.FlushStatistics();

            foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
                dataPersistanceObj.SaveData(ref gameData);

            return WriteSave(gameData);
        }
        finally
        {
            isSaving = false;
        }
    }

    private bool WriteSave(GameData data)
    {
        if (!dataHandler.Save(data, replaceExisting: canReplaceSave))
            return false;

        settingsSavePending = false;
        canReplaceSave = true;
        return true;
    }


    //--------------------


    private bool HandleQuitRequest()
    {
        if (Application.isEditor)
            return true;

        if (quitPrepared)
            return !waitingForQuitReports;

        var feedback = GetEnabledFeedback();
        bool saved = PrepareQuit(queueSessionReport: feedback != null);
        if (!saved || !feedback)
            return true;

        waitingForQuitReports = true;
        quitReportDeadline = Time.realtimeSinceStartupAsDouble + FeedbackForm.RequestTimeoutSeconds;
        StartCoroutine(SubmitReportsAndQuit(feedback, dataManager.playerStatsData_Store.pendingSessionReports));
        return false;
    }

    private void OnApplicationQuit()
    {
        if (!quitPrepared)
            PrepareQuit(queueSessionReport: GetEnabledFeedback() != null);
    }

    private static FeedbackForm GetEnabledFeedback()
    {
        var collection = FindAnyObjectByType<PlayerDataManager>();
        if (!collection || !collection.isActiveAndEnabled || !collection.collectData)
            return null;

        var feedback = FindAnyObjectByType<FeedbackForm>();
        return feedback && feedback.isActiveAndEnabled ? feedback : null;
    }

    private bool PrepareQuit(bool queueSessionReport)
    {
        if (quitPrepared || !savingEnabled || !dataManager)
            return false;

        quitPrepared = true;
        var statistics = Player_Statistics_Manager.Active;
        var levelStats = map ? FindAnyObjectByType<MapStatsGathered>() : null;
        if (levelStats)
        {
            if (map.IsCompletingLevel)
            {
                statistics?.RecordLevelCompletion();
                levelStats.CompleteLevel();
            }
            else
            {
                levelStats.ExitLevel();
            }
        }
        statistics?.FinishLevelAttempt();

        var session = FindAnyObjectByType<SessionStatsGathered>();
        if (session)
            session.StoreSessionStats();

        var storedStats = dataManager.playerStatsData_Store;
        var completedSession = storedStats.sessionStats ?? new Session_Stats();
        completedSession.session_No = Mathf.Max(1, completedSession.session_No);
        if (queueSessionReport)
        {
            storedStats.pendingSessionReports ??= new List<Session_Stats>();
            storedStats.pendingSessionReports.Add(completedSession);
        }

        storedStats.sessionStats = new Session_Stats { session_No = completedSession.session_No + 1 };
        dataManager.menuState_Store = MenuState.Main_Menu;
        SaveGame();
        return FlushPendingSave();
    }

    private IEnumerator SubmitReportsAndQuit(FeedbackForm feedback, List<Session_Stats> reports)
    {
        yield return null;

        try
        {
            while (feedback && reports.Count > 0 && Time.realtimeSinceStartupAsDouble < quitReportDeadline)
            {
                Session_Stats report = reports[0];
                bool delivered = false;
                yield return feedback.SubmitSessionReport(report, quitReportDeadline, success => delivered = success);
                if (!delivered)
                    break;

                reports.RemoveAt(0);
                // Persist the acknowledgement without adding the network wait to play time.
                if (!WriteSave(gameData))
                {
                    reports.Insert(0, report);
                    break;
                }
            }

            while (feedback && feedback.HasActiveRequests && Time.realtimeSinceStartupAsDouble < quitReportDeadline)
                yield return null;
        }
        finally
        {
            FinishQuitAfterReports();
        }
    }

    private void FinishQuitAfterReports()
    {
        if (!waitingForQuitReports)
            return;

        waitingForQuitReports = false;
        Application.Quit();
    }

    List<IDataPersistance> FindAllDataPersistenceObjects()
    {
        var persistenceObjects = new List<IDataPersistance>();
        foreach (MonoBehaviour behaviour in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (behaviour is IDataPersistance persistence)
                persistenceObjects.Add(persistence);
        }

        // Keep the previous callback order by sorting only the components that take part.
        if (persistenceObjects.Count > 1)
        {
            persistenceObjects.Sort((first, second) =>
                ((MonoBehaviour)first).GetInstanceID().CompareTo(((MonoBehaviour)second).GetInstanceID()));
        }

        return persistenceObjects;
    }
}
