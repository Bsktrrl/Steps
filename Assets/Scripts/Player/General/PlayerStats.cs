using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : Singleton<PlayerStats>
{
    public Stats stats = new Stats();


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += RefillStepsToMax;
        DataManager.Action_dataHasLoaded += UpdateActiveAbilities;
    }

    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= RefillStepsToMax;
        DataManager.Action_dataHasLoaded -= UpdateActiveAbilities;
    }


    //--------------------


    public void RefillStepsToMax()
    {
        StepsHUD.Instance.stepCounter = stats.steps_Current;

        MapManager mapManagerIsActive = FindObjectOfType<MapManager>();
        if (!mapManagerIsActive) { return; }

        int counter = 0;

        //Add steps gotten from active level
        if (MapManager.Instance.mapInfo_ToSave != null)
        {
            if (MapManager.Instance.mapInfo_ToSave.maxStepList != null)
            {
                for (int i = 0; i < MapManager.Instance.mapInfo_ToSave.maxStepList.Count; i++)
                {
                    if (MapManager.Instance.mapInfo_ToSave.maxStepList[i].isTaken)
                    {
                        counter++;
                    }
                }
            }
        }

        stats.steps_Max = 7 + counter;
        stats.steps_Current = stats.steps_Max;
    }
    void ResetActiveAbilities()
    {
        if (PlayerManager.Instance.player)
        {
            if (PlayerManager.Instance.player.GetComponent<PlayerStats>())
            {
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Snorkel = false;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Flippers = false;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.OxygenTank = false;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.SpringShoes = false;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.GrapplingHook = false;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.ClimingGloves = false;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.HandDrill = false;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.DrillHelmet = false;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.DrillBoots = false;
            }
        }
    }
    void UpdateActiveAbilities()
    {
        ResetActiveAbilities();

        MapManager mapManagerIsActive = FindObjectOfType<MapManager>();
        if (!mapManagerIsActive) {  return; }
    }


    //--------------------


    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            Debug.Log($"Loading progress: {operation.progress * 100}%");
            yield return null;
        }
    }
}