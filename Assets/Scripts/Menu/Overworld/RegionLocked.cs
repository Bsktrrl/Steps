using System.Collections.Generic;
using UnityEngine;

public class RegionLocked : MonoBehaviour
{
    [SerializeField] GameObject padlock;


    [Header("Unlock Requirement")]
    public List<GameObject> levelsToBeFinished;


    MenuLevelInfo menuLevelInfo;

    public bool regionIsLocked;


    //--------------------


    private void Awake()
    {
        menuLevelInfo = FindObjectOfType<MenuLevelInfo>();
    }
    private void OnEnable()
    {
        MenuLevelInfo.menuLevelInfo_hasLoaded += RefreshLock;
        NewGameButton.Action_PressNewGameButton += RefreshLock;
        RefreshLock();
    }

    private void OnDisable()
    {
        MenuLevelInfo.menuLevelInfo_hasLoaded -= RefreshLock;
        NewGameButton.Action_PressNewGameButton -= RefreshLock;
    }


    //--------------------


    void RefreshLock()
    {
        regionIsLocked = !CheckIfCanBePlayed();
        if (padlock && padlock.activeSelf != regionIsLocked)
            padlock.SetActive(regionIsLocked);
    }

    public bool CheckIfCanBePlayed()
    {
        var savedMaps = menuLevelInfo ? menuLevelInfo.mapInfo_ToSave?.map_SaveInfo_List : null;
        if (savedMaps == null || levelsToBeFinished == null || PlayerStats.Instance.stats?.itemsGot == null)
            return false;

        int completed = 0;
        foreach (GameObject level in levelsToBeFinished)
        {
            if (!level || !level.TryGetComponent(out LoadLevel requiredLevel))
                continue;

            foreach (Map_SaveInfo mapInfo in savedMaps)
            {
                if (mapInfo == null || !mapInfo.isCompleted || mapInfo.mapName != requiredLevel.levelToPlay)
                    continue;

                requiredLevel.isCompleted = true;
                completed++;
            }
        }

        return completed >= levelsToBeFinished.Count;
    }
}
