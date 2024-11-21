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
        SaveLoad_PlayerStats.playerStats_hasLoaded += CorrectionsAfterLoadingStats;
    }

    private void OnDisable()
    {
        Player_Movement.Action_StepTaken -= TakeAStep;
        SaveLoad_PlayerStats.playerStats_hasLoaded -= CorrectionsAfterLoadingStats;
    }


    //--------------------


    void CorrectionsAfterLoadingStats()
    {
        stats.steps_Current = stats.steps_Max;
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
        PlayerManager.Instance.isTeleporting = true;
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

        yield return new WaitForSeconds(waitTime * 25);

        PlayerManager.Instance.pauseGame = false;
        PlayerManager.Instance.isTeleporting = false;
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


    //--------------------
    //Abilities
    #region
    public void UpdateFenceSneak()
    {
        stats.abilitiesGot.FenceSneak = true;
    }
    public void UpdateSwimsuit()
    {
        stats.abilitiesGot.SwimSuit = true;
    }
    public void UpdateSwiftSwim()
    {
        stats.abilitiesGot.SwiftSwim = true;
    }
    public void UpdateFlippers()
    {
        stats.abilitiesGot.Flippers = true;

        //Find all gameObjects in the scene containing a Block_Water component
        Block_Water[] waterBlocks = FindObjectsOfType<Block_Water>();

        //Update the movementCost of all blocks
        foreach (Block_Water block in waterBlocks)
        {
            block.UpdateFastSwimmingMovementCost();
        }
    }
    public void UpdateLavaSuit()
    {
        stats.abilitiesGot.LavaSuit = true;
    }
    public void UpdateLavaSwiftSwim()
    {
        stats.abilitiesGot.LavaSwiftSwim = true;
    }
    public void UpdateHikerGear()
    {
        stats.abilitiesGot.HikerGear = true;
    }
    public void UpdateIceSpikes()
    {
        stats.abilitiesGot.IceSpikes = true;
    }
    public void UpdateGrapplingHook()
    {
        stats.abilitiesGot.GrapplingHook = true;
    }
    public void UpdateHammer()
    {
        stats.abilitiesGot.Hammer = true;
    }
    public void UpdateClimbingGear()
    {
        stats.abilitiesGot.ClimbingGear = true;
    }
    public void UpdateDash()
    {
        stats.abilitiesGot.Dash = true;
    }
    public void UpdateAscend()
    {
        stats.abilitiesGot.Ascend = true;
    }
    public void UpdateDescend()
    {
        stats.abilitiesGot.Descend = true;
    }
    public void UpdateControlStick()
    {
        stats.abilitiesGot.ControlStick = true;
    }
    #endregion
}