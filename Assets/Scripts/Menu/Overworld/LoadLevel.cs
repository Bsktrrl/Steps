using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [Header("Levels")]
    public string levelToPlay;

    [Header("Level Image")]
    public Sprite levelSprite;

    [Header("Unlock Requirement")]
    public int coinsRequirement;
    public int collectableRequirement;
    public List<GameObject> levelsToBeFinished;

    [SerializeField] bool canPlay;
    public bool isCompleted;


    //--------------------


    public void LoadLevelScene()
    {
        if (!CheckIfCanBePlayed()) { return; }

        if (!string.IsNullOrEmpty(levelToPlay))
        {
            StartCoroutine(LoadSceneCoroutine(levelToPlay));
        }
    }
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            Debug.Log($"Loading progress: {operation.progress * 100}%");
            yield return null;
        }
    }


    //--------------------


    public bool CheckIfCanBePlayed()
    {
        if (coinsRequirement <= 0 && collectableRequirement <= 0 && levelsToBeFinished.Count <= 0)
        {
            return true;
        }

        //print("0. Error: " + levelToPlay);
        if (PlayerStats.Instance.stats != null)
        {
           //print("1. Error: " + levelToPlay);
            if (PlayerStats.Instance.stats != null)
            {
                //print("2. Error: " + levelToPlay);
                if (PlayerStats.Instance.stats.itemsGot != null)
                {
                    //print("3. Error: " + levelToPlay + " | " + PlayerStats.Instance.stats.itemsGot.coin + " >= " + coinsRequirement + " | " + PlayerStats.Instance.stats.itemsGot.collectable + " >= " + collectableRequirement);
                    if (PlayerStats.Instance.stats.itemsGot.coin >= coinsRequirement && PlayerStats.Instance.stats.itemsGot.collectable >= collectableRequirement)
                    {
                        //print("4. Error: " + levelToPlay);
                        int counter = 0;

                        if (MenuLevelInfo.Instance.mapInfo_ToSave != null)
                        {
                            //print("5. Error: " + levelToPlay);
                            if (MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List != null)
                            {
                                //print("6. Error: " + levelToPlay);
                                foreach (Map_SaveInfo mapInfo in MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List)
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
        }
        
        return false;
    }
}