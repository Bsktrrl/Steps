using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DataPersistanceManager : Singleton<DataPersistanceManager>
{
    public static event Action Action_NewGame;
    private const float AutomaticSaveRetryDelay = 5f;

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
    bool loadFailed;
    float nextAutomaticSaveTime;
    int settingsAdjustments;
    bool settingsSavePending;
    private SaveRecoveryDialog saveDialog;
    private bool sceneChangePending;
    private bool quitPrepared;
    private bool quitSaveSucceeded;
    private bool waitingForQuitReports;
    private double quitReportDeadline;

    public static DataPersistanceManager instance { get; private set; }
    internal static bool SaveDialogOpen => instance && instance.saveDialog && instance.saveDialog.isActiveAndEnabled;
    internal static bool GameplayInputBlocked =>
        instance && (instance.sceneChangePending || instance.quitPrepared || SaveDialogOpen);

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
        if (saveDialog)
            saveDialog.Dismiss();
        FinishQuitAfterReports();
    }

    private void OnDestroy()
    {
        if (saveDialog)
            Destroy(saveDialog.gameObject);
        if (instance == this)
            instance = null;
    }

    private void LateUpdate()
    {
        if (waitingForQuitReports && Time.realtimeSinceStartupAsDouble >= quitReportDeadline)
            FinishQuitAfterReports();

        // Startup listeners finish populating the data, and a goal saves as part of its scene exit.
        if (quitPrepared || sceneChangePending || SaveDialogOpen ||
            Time.unscaledTime < nextAutomaticSaveTime ||
            (dataManager && !dataManager.dataHasLoaded_Check) || (map && map.IsCompletingLevel))
            return;

        FlushPendingSave();
    }


    //--------------------


    internal void RequestNewGame(Action onSaved)
    {
        if (!savingEnabled || quitPrepared || sceneChangePending || SaveDialogOpen)
            return;

        bool TryStartNewGame()
        {
            if (!TrySaveNewGame())
                return false;

            if (saveDialog)
                saveDialog.Dismiss();
            onSaved();
            return true;
        }

        if (!TryStartNewGame())
        {
            GetSaveDialog().Show("Could not start a new game",
                "Check that the save folder is writable and has free space. Retry, or cancel to return to the menu.",
                () => TryStartNewGame(), "Cancel", () => saveDialog.Dismiss(restoreSelection: true));
        }
    }

    private bool TrySaveNewGame()
    {
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

    private void LoadGame()
    {
        savingEnabled = false;
        SaveLoadResult result = dataHandler.Load(out GameData loadedData);
        // Once a read fails, a missing file on retry may mean the storage is still unavailable.
        if (result == SaveLoadResult.Failed || (loadFailed && result == SaveLoadResult.Missing))
        {
            loadFailed = true;
            GetSaveDialog().Show("Could not read your save",
                "Check that the save file is accessible or restore a working copy, then choose Retry.",
                LoadGame, "Quit", Application.isEditor ? null : () => Application.Quit());
            return;
        }

        loadFailed = false;
        if (saveDialog)
            saveDialog.Dismiss();

        gameData = loadedData;
        canReplaceSave = result == SaveLoadResult.Loaded;

        if (result == SaveLoadResult.Missing)
        {
            DataManager data = DataManager.Instance;
            gameData = data.CreateNewGameData(hasStartedNewGame: false);
            data.ApplyNewGameData(gameData);
            Player_Statistics_Manager.Active?.ResetForNewGame(data);
        }

        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
            dataPersistanceObj.LoadData(gameData);

        savingEnabled = true;
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

    internal bool TryBeginSceneChange()
    {
        if (!savingEnabled || sceneChangePending || quitPrepared || SaveDialogOpen)
            return false;

        sceneChangePending = true;
        EventSystem system = EventSystem.current;
        if (system)
            system.SetSelectedGameObject(null);
        return true;
    }

    // The caller owns the transition from the start of its fade until scene unload.
    internal IEnumerator SaveBeforeSceneChange()
    {
        SaveGame();
        if (FlushPendingSave())
            yield break;

        bool retry = false;
        ShowWriteFailure(() => retry = true);
        do
        {
            while (!retry)
                yield return null;

            retry = false;
        } while (!FlushPendingSave());

        saveDialog.Dismiss();
    }

    private bool FlushPendingSave()
    {
        if (!savingEnabled || !savePending || isSaving)
            return false;

        savePending = false;
        isSaving = true;
        bool saved = false;
        try
        {
            Player_Statistics_Manager.Active?.FlushStatistics();

            foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
                dataPersistanceObj.SaveData(ref gameData);

            saved = WriteSave(gameData);
            return saved;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Could not prepare the save.\n" + e, this);
            return false;
        }
        finally
        {
            if (!saved)
            {
                savePending = true;
                nextAutomaticSaveTime = Time.unscaledTime + AutomaticSaveRetryDelay;
            }

            isSaving = false;
        }
    }

    private bool WriteSave(GameData data)
    {
        if (!dataHandler.Save(data, replaceExisting: canReplaceSave))
            return false;

        settingsSavePending = false;
        canReplaceSave = true;
        nextAutomaticSaveTime = 0f;
        return true;
    }

    private SaveRecoveryDialog GetSaveDialog()
    {
        if (!saveDialog)
            saveDialog = SaveRecoveryDialog.Create(gameObject.scene);

        return saveDialog;
    }

    private void ShowWriteFailure(Action retry)
    {
        GetSaveDialog().Show("Could not save your progress",
            "Your progress is still in memory. Check that the save folder is writable and has free space, then choose Retry.",
            retry);
    }

    private bool HandleQuitRequest()
    {
        // An unreadable save can be left untouched by closing the game.
        if (Application.isEditor || instance != this || !savingEnabled || !dataManager)
            return true;

        if (quitSaveSucceeded)
            return !waitingForQuitReports;

        if (quitPrepared || sceneChangePending || SaveDialogOpen)
            return false;

        var feedback = GetEnabledFeedback();
        PrepareQuit(queueSessionReport: feedback != null);
        if (TrySaveForQuit(feedback))
            return !waitingForQuitReports;

        ShowWriteFailure(() =>
        {
            if (!TrySaveForQuit(feedback))
                return;

            saveDialog.Dismiss();
            if (!waitingForQuitReports)
                Application.Quit();
        });
        return false;
    }

    private bool TrySaveForQuit(FeedbackForm feedback)
    {
        if (!FlushPendingSave())
            return false;

        quitSaveSucceeded = true;
        if (!feedback)
            return true;

        waitingForQuitReports = true;
        quitReportDeadline = Time.realtimeSinceStartupAsDouble + FeedbackForm.RequestTimeoutSeconds;
        StartCoroutine(SubmitReportsAndQuit(feedback, dataManager.playerStatsData_Store.pendingSessionReports));
        return true;
    }

    private void OnApplicationQuit()
    {
        if (instance != this || !savingEnabled || !dataManager || quitSaveSucceeded)
            return;

        if (!quitPrepared)
            PrepareQuit(queueSessionReport: GetEnabledFeedback() != null);

        FlushPendingSave();
    }

    private static FeedbackForm GetEnabledFeedback()
    {
        var collection = FindAnyObjectByType<PlayerDataManager>();
        if (!collection || !collection.isActiveAndEnabled || !collection.collectData)
            return null;

        var feedback = FindAnyObjectByType<FeedbackForm>();
        return feedback && feedback.isActiveAndEnabled ? feedback : null;
    }

    private void PrepareQuit(bool queueSessionReport)
    {
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
