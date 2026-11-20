using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistanceManager : Singleton<DataPersistanceManager>
{
    public static event Action Action_NewGame;

    [Header("File Storage Config")]
    public string fileName;

    public GameData gameData;
    List<IDataPersistance> dataPersistanceObjects;
    FileDataHandler dataHandler;

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


    public void NewGame()
    {
        GameData oldData = gameData ?? new GameData();

        this.gameData = new GameData(); // create fresh data

        // Clear the DataManager's stores explicitly
        DataManager.Instance.Load_NewGame_Data(oldData, this.gameData);
        Player_Statistics_Manager.Active?.ResetForNewGame();

        //Persist through newGame
        this.gameData.settingData_SaveList = DataManager.Instance.settingData_StoreList;

        // Save the new clean data
        SaveGame(updateStatistics: false);

        Action_NewGame?.Invoke();
    }
    public void LoadGame()
    {
        //Load any saved data from a file using the "data handler"
        this.gameData = dataHandler.Load();

        //If no data can be loaded, initialize to a new data
        if (this.gameData == null)
        {
            print("No data was found. Initializing data to defaults");
            NewGame();
        }

        //Push the loaded data to all other scripts that need it
        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.LoadData(gameData);
        }
    }
    public void SaveGame()
    {
        SaveGame(updateStatistics: true);
    }

    private void SaveGame(bool updateStatistics)
    {
        if (updateStatistics)
            Player_Statistics_Manager.Active?.FlushStatistics();

        //Pass the data to other scripts so thay can update it
        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.SaveData(ref gameData);
        }

        //Save that data to a file using the "data handler"
        dataHandler.Save(gameData);
    }


    //--------------------


    private void OnApplicationQuit()
    {
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
