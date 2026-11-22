using System.Collections;
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
    private void Update()
    {
        CanPlayDisplay();
    }


    //--------------------


    void CanPlayDisplay()
    {
        bool isLocked = !CheckIfCanBePlayed();
        if (padlock.activeSelf != isLocked)
            padlock.SetActive(isLocked);
        regionIsLocked = isLocked;
    }

    public bool CheckIfCanBePlayed()
    {
        //print("1. Error: " + levelToPlay);
        if (PlayerStats.Instance.stats != null)
        {
            //print("2. Error: " + levelToPlay);
            if (PlayerStats.Instance.stats != null)
            {
                //print("3. Error: " + levelToPlay);
                if (PlayerStats.Instance.stats.itemsGot != null)
                {
                    //print("4. Error: " + levelToPlay);
                    int counter = 0;

                    if (menuLevelInfo && menuLevelInfo.mapInfo_ToSave != null)
                    {
                        //print("5. Error: " + levelToPlay);
                        if (menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List != null)
                        {
                            if (levelsToBeFinished.Count == 0)
                                return true;

                            foreach (Map_SaveInfo mapInfo in menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List)
                            {
                                if (!mapInfo.isCompleted)
                                    continue;

                                for (int i = 0; i < levelsToBeFinished.Count; i++)
                                {
                                    LoadLevel requiredLevel = levelsToBeFinished[i].GetComponent<LoadLevel>();
                                    if (mapInfo.mapName == requiredLevel.levelToPlay)
                                    {
                                        requiredLevel.isCompleted = true;
                                        counter++;
                                    }
                                }
                            }

                            if (counter >= levelsToBeFinished.Count)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }
}
