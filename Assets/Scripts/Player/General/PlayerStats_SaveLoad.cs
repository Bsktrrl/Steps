using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats_SaveLoad : Singleton<PlayerStats_SaveLoad>
{
    public void LoadGame()
    {
        PlayerStats.Instance.stats = DataManager.Instance.playerStats_Store;

        print("PlayerStats is Loaded");
    }
    public void SaveGame()
    {
        DataManager.Instance.playerStats_Store = PlayerStats.Instance.stats;
        DataPersistanceManager.instance.SaveGame();

        print("PlayerStats is Saved");
    }
}
