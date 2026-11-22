using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : Singleton<PlayerStats>
{
    public static event Action Action_UpdateAbilityDisplay;

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
        MapManager map = FindAnyObjectByType<MapManager>();
        if (!map || map.gameObject.scene != gameObject.scene) return;

        StepsHUD.Instance.stepCounter = stats.steps_Current;

        int counter = 0;

        //Add steps gotten from active level
        if (map.mapInfo_ToSave?.maxStepList != null)
        {
            foreach (var footprint in map.mapInfo_ToSave.maxStepList)
            {
                if (footprint.isTaken)
                    counter++;
            }
        }

        stats.steps_Max = 7 + counter;
        stats.steps_Current = stats.steps_Max;
    }

    public void RestoreMissingSteps()
    {
        SetReplenishedSteps(stats.steps_Max);
    }

    public void SetReplenishedSteps(int newBalance)
    {
        int restored = Mathf.Max(0, newBalance - Mathf.Max(0, stats.steps_Current));
        stats.steps_Current = newBalance;
        Player_Statistics_Manager.Active?.RecordStepRefill(restored);
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
        // The main menu loads saved stats too, but has no gameplay map or player.
        MapManager map = FindAnyObjectByType<MapManager>();
        if (!map || map.gameObject.scene != gameObject.scene) return;

        ResetActiveAbilities();
        map.SetAbilities();
        Action_UpdateAbilityDisplay?.Invoke();
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
