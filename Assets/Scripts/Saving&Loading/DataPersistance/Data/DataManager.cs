using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class DataManager : Singleton<DataManager>, IDataPersistance
{
    #region General Variables
    public GameData gameData;

    public static Action dataIsSaving;
    public static Action datahasLoaded;
    #endregion


    //--------------------


    #region Variables
    //All stored Maps info
    [HideInInspector] public Map_SaveInfoList mapInfo_StoreList = new Map_SaveInfoList();

    //Player stored Stats info
    [HideInInspector] public Stats playerStats_Store = new Stats();
    #endregion


    //--------------------


    public void LoadData(GameData gameData)
    {
        //Get saved data from file to be loaded into the project
        #region

        this.mapInfo_StoreList = gameData.mapInfo_SaveList;
        this.playerStats_Store = gameData.playerStats_Save;

        print("Data has Loaded");
        #endregion

        StartCoroutine(LoadingDelay(0.5f));
    }
    IEnumerator LoadingDelay(float time)
    {
        yield return new WaitForSeconds(time);

        print("------------------------------");

        datahasLoaded?.Invoke();
    }

    public void SaveData(ref GameData gameData)
    {
        dataIsSaving?.Invoke();

        //Input what to save
        gameData.mapInfo_SaveList = this.mapInfo_StoreList;
        gameData.playerStats_Save = this.playerStats_Store;

        print("Data has Saved");
    }
}