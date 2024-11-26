using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : Singleton<PlayerStats>
{
    public static event Action Action_RespawnToSavePos;
    public static event Action Action_RespawnPlayer;

    public static event Action updateCoins;
    public static event Action updateCollectable;
    public static event Action updateStepsMax;

    public Vector3 savePos;

    public Stats stats;


    //--------------------


    private void Start()
    {
        savePos = transform.position;
    }


    //--------------------


    private void OnEnable()
    {
        Player_Movement.Action_StepTaken += TakeAStep;
        SaveLoad_PlayerStats.playerStats_hasLoaded += RefillStepsToMax;
    }

    private void OnDisable()
    {
        Player_Movement.Action_StepTaken -= TakeAStep;
        SaveLoad_PlayerStats.playerStats_hasLoaded -= RefillStepsToMax;
    }


    //--------------------


    void RefillStepsToMax()
    {
        stats.steps_Current = stats.steps_Max;
    }
    public void RefillStepsToMax(int corrections)
    {
        stats.steps_Current = stats.steps_Max + corrections;
    }


    //--------------------


    public void TakeAStep()
    {
        //Reduce available steps
        if (PlayerManager.Instance.block_StandingOn_Current.block)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>())
            {
                stats.steps_Current -= PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().GetMovementCost();
            }
        }

        //If steps is < 0
        if (stats.steps_Current < 0)
        {
            RespawnPlayer();
        }
        else
        {
            //Update the stepCounter UI
            UIManager.Instance.UpdateUI();
        }
    }
    public void RespawnPlayer()
    {
        print("Respawn Player");

        StartCoroutine(ResetplayerPos(0.01f));

        //if (!string.IsNullOrEmpty(SceneManager.GetActiveScene().name))
        //{
        //    StartCoroutine(LoadSceneCoroutine(SceneManager.GetActiveScene().name));
        //}
    }

    IEnumerator ResetplayerPos(float waitTime)
    {
        PlayerManager.Instance.pauseGame = true;
        PlayerManager.Instance.isTransportingPlayer = true;
        Player_Movement.Instance.movementStates = MovementStates.Moving;

        stats.steps_Current = stats.steps_Max;

        yield return new WaitForSeconds(waitTime);

        transform.position = MapManager.Instance.playerStartPos;

        Action_RespawnPlayer?.Invoke();
        Player_Movement.Instance.SetPlayerBodyRotation(0);

        yield return new WaitForSeconds(waitTime);

        Player_Movement.Instance.Action_ResetBlockColorInvoke();

        Player_Movement.Instance.movementStates = MovementStates.Still;
        stats.steps_Current = stats.steps_Max;
        UIManager.Instance.UpdateUI();

        stats.ResetTempStats();

        yield return new WaitForSeconds(waitTime * 25);

        PlayerManager.Instance.pauseGame = false;
        PlayerManager.Instance.isTransportingPlayer = false;
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

    public void RespawnToSavePos_Action()
    {
        Action_RespawnToSavePos?.Invoke();
    }
}