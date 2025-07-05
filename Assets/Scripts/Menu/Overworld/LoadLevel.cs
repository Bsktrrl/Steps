using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class LoadLevel : MonoBehaviour
{
    [Header("Ready to add to the Game")]
    public bool readyToBePlayedAndDisplayed;

    [Header("Levels")]
    public regions regionToPlay;
    public string levelToPlay;

    [Header("Level Image")]
    public Sprite levelSprite;

    [Header("Unlock Requirement")]
    public List<GameObject> levelsToBeFinished;

    [SerializeField] bool canPlay;
    public bool isCompleted;

    [Header("In the Level")]
    public SkinType skinTypeInLevel;

    public AbilitiesGot abilitiesInLevel;


    //--------------------


    public void LoadLevelScene()
    {
        if (!CheckIfCanBePlayed()) { return; }

        if (!string.IsNullOrEmpty(levelToPlay))
        {
            RememberCurrentlySelectedUIElement.Instance.SaveSelectedUIElement(OverWorldManager.Instance.regionState, OverWorldManager.Instance.levelState);

            AnalyticsCalls.SelectLevel(levelToPlay, regionToPlay.ToString());

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
        //print("0. Error: " + levelToPlay);
        if (PlayerStats.Instance.stats != null)
        {
           //print("1. Error: " + levelToPlay);
            if (PlayerStats.Instance.stats != null)
            {
                //print("2. Error: " + levelToPlay);
                if (PlayerStats.Instance.stats.itemsGot != null)
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
        
        return false;
    }
}

public enum regions
{
    None,

    Water,
    Mountain,
    Desert,
    Winter,
    Swamp,
    Industrial
}