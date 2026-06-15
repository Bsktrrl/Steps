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
        if (CheckIfCanBePlayed())
        {
            padlock.SetActive(false);
            regionIsLocked = false;
        }
        else
        {
            padlock.SetActive(true);
            regionIsLocked = true;
        }
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
                            //print("6. Error: " + levelToPlay);
                            foreach (Map_SaveInfo mapInfo in menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List)
                            {
                                //print("7. Error: " + levelToPlay);
                                for (int i = 0; i < levelsToBeFinished.Count; i++)
                                {
                                    if (mapInfo.mapName == levelsToBeFinished[i].GetComponent<LoadLevel>().levelToPlay)
                                    {
                                        if (mapInfo.isCompleted)
                                        {
                                            levelsToBeFinished[i].GetComponent<LoadLevel>().isCompleted = true;
                                            counter++;
                                        }
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
