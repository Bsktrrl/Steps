using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad_PlayerStats : Singleton<SaveLoad_PlayerStats>
{
    public static event Action playerStats_hasLoaded;

    public void LoadData()
    {
        if (DataManager.Instance.playerStats_Store != null)
        {
            PlayerStats.Instance.stats = DataManager.Instance.playerStats_Store;
            PlayerStats.Instance.stats.ResetTempStats();
        }

        playerStats_hasLoaded?.Invoke();
    }
    public void SaveData()
    {
        DataManager.Instance.playerStats_Store = PlayerStats.Instance.stats;
        DataPersistanceManager.instance.SaveGame();
    }
}
