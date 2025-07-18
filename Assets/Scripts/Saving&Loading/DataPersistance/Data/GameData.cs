using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //MenuState
    /*[HideInInspector]*/ public MenuState menuState_Save = new MenuState();

    //Player saved Stats info
    /*[HideInInspector]*/ public Stats playerStats_Save = new Stats();

    //All saved Maps info
    /*[HideInInspector]*/ public Map_SaveInfoList mapInfo_SaveList = new Map_SaveInfoList();

    //Overworld States
    /*[HideInInspector]*/ public OverWorldStates overWorldStates_SaveList = new OverWorldStates();

    //Settings
    /*[HideInInspector]*/ public SettingData settingData_SaveList = new SettingData();

    /*[HideInInspector]*/ public MapNameDisplay mapNameDisplay_Save = new MapNameDisplay();


    //--------------------


    public GameData()
    {
        //Input Everything to clear up the file
        menuState_Save = MenuState.None;
        playerStats_Save = new Stats();
        mapInfo_SaveList = new Map_SaveInfoList();
        overWorldStates_SaveList = new OverWorldStates();
        //settingData_SaveList = new SettingData();
    }
}
