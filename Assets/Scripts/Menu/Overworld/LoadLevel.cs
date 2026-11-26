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

    //--------------------

    private void Awake()
    {
        mainMenuManager = FindObjectOfType<MainMenuManager>();
        menuLevelInfo = FindObjectOfType<MenuLevelInfo>();
    }


    //--------------------


    public void LoadLevelScene()
    {
        if (DataPersistanceManager.GameplayInputBlocked)
            return;

        ResolveReferences();

        if (!CheckIfCanBePlayed()) return;
        if (string.IsNullOrEmpty(levelToPlay)) return;

        DataPersistanceManager persistence = DataPersistanceManager.instance;
        if (!persistence || !Application.CanStreamedLevelBeLoaded(levelToPlay) || !persistence.TryBeginSceneChange())
            return;

        persistence.StartCoroutine(LoadSceneCoroutine(levelToPlay, persistence));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, DataPersistanceManager persistence)
    {
        if (mainMenuManager != null)
        {
            yield return mainMenuManager.PlayFadeInAndWait(0.92f);
        }

        if (TryGetComponent(out LevelInfo levelInfo))
            levelInfo.StoreNameDisplay();

        SessionStatsGathered.Instance.StoreSessionStats();

        if (RememberCurrentlySelectedUIElement.Instance != null && OverWorldManager.Instance != null)
        {
            OverWorldManager overworld = OverWorldManager.Instance;
            LevelState enteredLevel = TryGetComponent(out LevelButton levelButton) ? levelButton.State : overworld.levelState;
            RememberCurrentlySelectedUIElement.Instance.SaveSelectedUIElement(
                overworld.regionState,
                enteredLevel
            );
        }

        yield return persistence.SaveBeforeSceneChange();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
            yield return null;
    }


    //--------------------


    private void EnsureMenuReferences()
    {
        if (mainMenuManager == null)
            mainMenuManager = FindObjectOfType<MainMenuManager>(true);

        if (menuLevelInfo == null)
            menuLevelInfo = FindObjectOfType<MenuLevelInfo>(true);
    }
    private void ResolveReferences()
    {
        if (mainMenuManager == null)
            mainMenuManager = FindObjectOfType<MainMenuManager>(true);

        if (menuLevelInfo == null)
            menuLevelInfo = FindObjectOfType<MenuLevelInfo>(true);
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
                                    LoadLevel requiredLevel = levelsToBeFinished[i].GetComponent<LoadLevel>();
                                    if (mapInfo.mapName == requiredLevel.levelToPlay)
                                    {
                                        if (mapInfo.isCompleted)
                                        {
                                            requiredLevel.isCompleted = true;
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
    Metalworks,
    HUB
}
