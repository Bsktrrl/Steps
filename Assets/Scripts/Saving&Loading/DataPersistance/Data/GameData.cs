using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //NewGame
    /*[HideInInspector]*/public bool haveStartedNewGame_Save = new bool();

    //MenuState
    /*[HideInInspector]*/public MenuState menuState_Save = new MenuState();

    //Player saved Stats info
    /*[HideInInspector]*/ public Stats playerStats_Save = new Stats();

    //All saved Maps info
    /*[HideInInspector]*/ public Map_SaveInfoList mapInfo_SaveList = new Map_SaveInfoList();

    //Overworld States
    /*[HideInInspector]*/ public OverWorldStates overWorldStates_SaveList = new OverWorldStates();

    //Settings
    /*[HideInInspector]*/ public SettingData settingData_SaveList = new SettingData();

    //Map Display
    /*[HideInInspector]*/ public MapNameDisplay mapNameDisplay_Save = new MapNameDisplay();

    //NPC Data
    /*[HideInInspector]*/ public CharatersData charatersData_Save = new CharatersData();

    //Block Skins
    /*[HideInInspector]*/ public SkinInfo skinsInfo_Save = new SkinInfo();

    //Tutorial
    /*[HideInInspector]*/ public bool tutorial_Finished_Save = new bool();

    //Data to run once
    /*[HideInInspector]*/ public OneTimeRunData oneTimeRunData_Save = new OneTimeRunData();
    
    //Text Database
    /*[HideInInspector]*/ public Game_TextDatabase game_TextDatabase_Save = new Game_TextDatabase();

    //PlayerStatsData
    /*[HideInInspector]*/ public PlayerStatsData PlayerStatsData_Save = new PlayerStatsData();

    //PlayerStatsData
    /*[HideInInspector]*/ public GlueplantStandStats glueplantStandStats_Save = new GlueplantStandStats();

    //PlayerStatsData
    /*[HideInInspector]*/ public SwitchesData switchesData_Save = new SwitchesData();

    //PlayerStatsData
    /*[HideInInspector]*/ public PermanentAbilitiesReceived permanentAbilitiesReceived_Save = new PermanentAbilitiesReceived();

    //PlayerStatsData
    /*[HideInInspector]*/public PlayerStatistics playerStatistics_Save = new PlayerStatistics();


    //--------------------


    public GameData()
    {
        menuState_Save = MenuState.None;
    }
}
