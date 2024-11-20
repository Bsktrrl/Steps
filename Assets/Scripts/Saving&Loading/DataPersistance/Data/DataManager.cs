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
    //MenuState
    [HideInInspector] public MenuState menuState_Store = new MenuState();

    //Player stored Stats info
    [HideInInspector] public Stats playerStats_Store = new Stats();

    //All stored Maps info
    [HideInInspector] public Map_SaveInfoList mapInfo_StoreList = new Map_SaveInfoList();
    #endregion


    //--------------------


    public void LoadData(GameData gameData)
    {
        //Get saved data from file to be loaded into the project
        #region

        this.menuState_Store = gameData.menuState_Save;
        this.playerStats_Store = gameData.playerStats_Save;
        this.mapInfo_StoreList = gameData.mapInfo_SaveList;

        print("Data has Loaded");
        #endregion

        StartCoroutine(LoadingDelay(0.01f));
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
         gameData.menuState_Save = this.menuState_Store;
        gameData.mapInfo_SaveList = this.mapInfo_StoreList;
        gameData.playerStats_Save = this.playerStats_Store;

        print("Data has Saved");
    }
}