using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class LoadLevel : MonoBehaviour
{
    public static event Action Action_LevelIsComplete;

    [Header("Ready to add to the Game")]
    public bool readyToBePlayedAndDisplayed;

    [Header("Levels")]
    public regions regionToPlay;
    public string levelToPlay;

    [Header("Level Image")]
    public Sprite levelSprite;

    [Header("Unlock Requirement")]
    public List<GameObject> levelsToBeFinished;

    public bool canPlay;
    public bool isCompleted;

    [Header("In the Level")]
    public SkinType skinTypeInLevel;

    public AbilitiesGot abilitiesInLevel;


    MainMenuManager mainMenuManager;
    MenuLevelInfo menuLevelInfo;

    private bool _isLoading;


    //--------------------


    private void Start()
    {
        mainMenuManager = FindObjectOfType<MainMenuManager>();
        menuLevelInfo = FindObjectOfType<MenuLevelInfo>();
    }


    //--------------------


    public void LoadLevelScene()
    {
        if (_isLoading) return;

        if (!CheckIfCanBePlayed()) { return; }

        if (!string.IsNullOrEmpty(levelToPlay))
        {
            if (GetComponent<LevelInfo>())
                GetComponent<LevelInfo>().SaveNameDisplay();

            AnalyticsCalls.SelectLevel(levelToPlay, regionToPlay.ToString());

            StartCoroutine(LoadSceneCoroutine(levelToPlay));
        }
    }
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        if (_isLoading) yield break;
        _isLoading = true;

        if (mainMenuManager)
        {
            yield return mainMenuManager.FadeInBlackScreenCoroutine();
        }

        // Only do this if these singletons are guaranteed to exist here.
        if (RememberCurrentlySelectedUIElement.Instance != null && OverWorldManager.Instance != null)
        {
            RememberCurrentlySelectedUIElement.Instance.SaveSelectedUIElement(
                OverWorldManager.Instance.regionState,
                OverWorldManager.Instance.levelState
            );
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Optional: if you ever use allowSceneActivation elsewhere, set it explicitly
        // operation.allowSceneActivation = true;

        float nextLogTime = 0f;

        while (!operation.isDone)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (Time.unscaledTime >= nextLogTime)
            {
                nextLogTime = Time.unscaledTime + 0.25f; // 4 times/second max
                Debug.Log($"Loading progress: {operation.progress * 100f:0.0}%");
            }
#endif
            yield return null;
        }
    }


    //--------------------


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

                                            Action_LevelIsComplete?.Invoke();
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

    Rivergreen,
    Sandlands,
    Frostfield,
    Firevein,
    Witchmire,
    Metalworks
}