using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad_PlayerStats : Singleton<SaveLoad_PlayerStats>
{
    public void LoadData()
    {
        if (DataManager.Instance.playerStats_Store != null)
        {
            PlayerStats.Instance.stats = DataManager.Instance.playerStats_Store;
        }
    }
    public void SaveData()
    {
        DataManager.Instance.playerStats_Store = PlayerStats.Instance.stats;
        DataPersistanceManager.instance.SaveGame();
    }
}
