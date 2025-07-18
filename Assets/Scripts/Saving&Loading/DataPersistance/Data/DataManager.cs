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

    public static Action Action_dataIsSaving;
    public static Action Action_dataHasLoaded;
    #endregion


    //--------------------


    #region Variables
    //MenuState
    /*[HideInInspector]*/ public MenuState menuState_Store = new MenuState();

    //Player stored Stats info
    /*[HideInInspector]*/ public Stats playerStats_Store = new Stats();

    //All stored Maps info
    /*[HideInInspector]*/ public Map_SaveInfoList mapInfo_StoreList = new Map_SaveInfoList();

    //Overworld States
    /*[HideInInspector]*/ public OverWorldStates overWorldStates_StoreList = new OverWorldStates();

    //Settings
    /*[HideInInspector]*/ public SettingData settingData_StoreList = new SettingData();

    /*[HideInInspector]*/ public MapNameDisplay mapNameDisplay_Store = new MapNameDisplay();
    #endregion


    //--------------------


    public void LoadData(GameData gameData)
    {
        //Get saved data from file to be loaded into the project
        #region

        this.menuState_Store = gameData.menuState_Save;
        this.playerStats_Store = gameData.playerStats_Save;
        this.mapInfo_StoreList = gameData.mapInfo_SaveList;
        this.overWorldStates_StoreList = gameData.overWorldStates_SaveList;
        this.settingData_StoreList = gameData.settingData_SaveList;
        this.mapNameDisplay_Store = gameData.mapNameDisplay_Save;

        #endregion

        //Load the saved data into the project
        #region

        SaveLoad_MapInfo tempMapInfo = FindObjectOfType<SaveLoad_MapInfo>();
        if (tempMapInfo)
        {
            SaveLoad_MapInfo.Instance.LoadData();
            print("1. MapInfo has Loaded");
        }

        SaveLoad_PlayerStats tempPlayerStat = FindObjectOfType<SaveLoad_PlayerStats>();
        if (tempPlayerStat)
        {
            SaveLoad_PlayerStats.Instance.LoadData();
            print("2. PlayerStat has Loaded");
        }

        OverWorldManager tempOverworld = FindObjectOfType<OverWorldManager>();
        if (tempOverworld)
        {
            OverWorldManager.Instance.LoadUIElementState_IfExitsFromALevel();
            print("3. UIElementState has Loaded");
        }

        SettingsMenu tempSettingData = FindObjectOfType<SettingsMenu>();
        if (tempOverworld)
        {
            SettingsMenu.Instance.LoadData();
            print("4. SettingData has Loaded");
        }

        #endregion

        StartCoroutine(LoadingDelay(0.01f));
    }
    IEnumerator LoadingDelay(float time)
    {
        yield return new WaitForSeconds(time);

        print("------------------------------");

        Action_dataHasLoaded?.Invoke();
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
    }


    //--------------------


    public void Load_NewGame_Data(GameData oldData, GameData newData)
    {
        //Files to delete upon newGame
        this.menuState_Store = newData.menuState_Save;
        this.playerStats_Store = newData.playerStats_Save;
        this.mapInfo_StoreList = newData.mapInfo_SaveList;
        this.overWorldStates_StoreList = newData.overWorldStates_SaveList;
        this.settingData_StoreList = newData.settingData_SaveList;
        this.mapNameDisplay_Store = newData.mapNameDisplay_Save;

        //Persist through newGame
        this.settingData_StoreList = oldData.settingData_SaveList;
    }
}