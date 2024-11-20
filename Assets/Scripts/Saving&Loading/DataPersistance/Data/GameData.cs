using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //MenuState
    [HideInInspector] public MenuState menuState_Save = new MenuState();

    //Player saved Stats info
    [HideInInspector] public Stats playerStats_Save = new Stats();

    //All saved Maps info
    [HideInInspector] public Map_SaveInfoList mapInfo_SaveList = new Map_SaveInfoList();


    //--------------------


    public GameData()
    {
        //Input All Lists to clear
        //playerStats_Save.ResetStats();
    }
}
