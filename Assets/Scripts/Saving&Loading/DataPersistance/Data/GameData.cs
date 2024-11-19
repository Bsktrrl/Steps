using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //All saved Maps info
    [HideInInspector] public Map_SaveInfoList mapInfo_SaveList = new Map_SaveInfoList();

    //Player saved Stats info
    [HideInInspector] public Stats playerStats_Save = new Stats();

    //--------------------


    public GameData()
    {
        //Input All Lists to clear
        //playerStats_Save.ResetStats();
    }
}
