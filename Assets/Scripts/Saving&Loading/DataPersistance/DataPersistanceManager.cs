using System;
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
    bool savingEnabled;
    bool canReplaceSave;

    public static DataPersistanceManager instance { get; private set; }


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
        this.dataPersistanceObjects = FindAllDataPersistenceObjects();

        LoadGame();
    }


    //--------------------


    public bool NewGame()
    {
        if (!savingEnabled)
            return false;

        DataManager data = DataManager.Instance;
        GameData newData = data.CreateNewGameData(hasStartedNewGame: true);
        if (!WriteSave(newData))
            return false;

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
            WriteSave(gameData);
    }

    public void SaveGame()
    {
        if (!savingEnabled)
            return;

        Player_Statistics_Manager.Active?.FlushStatistics();

        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
            dataPersistanceObj.SaveData(ref gameData);

        WriteSave(gameData);
    }

    private bool WriteSave(GameData data)
    {
        if (!dataHandler.Save(data, replaceExisting: canReplaceSave))
            return false;

        canReplaceSave = true;
        return true;
    }


    //--------------------


    private void OnApplicationQuit()
    {
        if (!savingEnabled)
            return;

        Player_Statistics_Manager.Active?.FinishLevelAttempt();

        //Make it so that the playerStatsData are reset on next game played
        if (FindObjectOfType<MapStatsGathered>())
            MapStatsGathered.Instance.ExitLevel();

        //PlayerStats Data
        if (FindObjectOfType<FeedbackForm>())
            FeedbackForm.Instance.SubmitFeedback_Session();
        if (FindObjectOfType<SessionStatsGathered>())
            SessionStatsGathered.Instance.ResetSessionStats();

        DataManager.Instance.playerStatsData_Store.sessionStats.session_No += 1;

        //Save the variables
        DataManager.Instance.menuState_Store = MenuState.Main_Menu;
        SaveGame();
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
