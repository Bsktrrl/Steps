using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : Singleton<PlayerStats>
{
    public static event Action Action_RespawnToSavePos;
    public static event Action Action_RespawnPlayerEarly;
    public static event Action Action_RespawnPlayer;
    public static event Action Action_RespawnPlayerLate;

    public static event Action updateCoins;
    public static event Action updateCollectable;
    public static event Action updateStepsMax;

    public Vector3 savePos;

    public Stats stats = new Stats();


    //--------------------


    private void Start()
    {
        savePos = transform.position;
    }


    //--------------------


    private void OnEnable()
    {
        Player_Movement.Action_StepTaken += TakeAStep;
        DataManager.Action_dataHasLoaded += RefillStepsToMax;
        DataManager.Action_dataHasLoaded += UpdateActiveAbilities;
    }

    private void OnDisable()
    {
        Player_Movement.Action_StepTaken -= TakeAStep;
        DataManager.Action_dataHasLoaded -= RefillStepsToMax;
        DataManager.Action_dataHasLoaded -= UpdateActiveAbilities;
    }


    //--------------------


    void RefillStepsToMax()
    {
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
    void UpdateActiveAbilities()
    {
        MapManager mapManagerIsActive = FindObjectOfType<MapManager>();
        if (!mapManagerIsActive) {  return; }

        //Based on what's picked up in the level, assign active abilities to the player
        if (MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.FenceSneak)
            stats.abilitiesGot_Temporary.FenceSneak = true;
        else
            stats.abilitiesGot_Temporary.FenceSneak = false;

        if (MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.SwimSuit)
            stats.abilitiesGot_Temporary.SwimSuit = true;
        else
            stats.abilitiesGot_Temporary.SwimSuit = false;

        if (MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.SwiftSwim)
            stats.abilitiesGot_Temporary.SwiftSwim = true;
        else
            stats.abilitiesGot_Temporary.SwiftSwim = false;

        if (MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Flippers)
            stats.abilitiesGot_Temporary.Flippers = true;
        else
            stats.abilitiesGot_Temporary.Flippers = false;

        if (MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Flameable)
            stats.abilitiesGot_Temporary.Flameable = true;
        else
            stats.abilitiesGot_Temporary.Flameable = false;

        if (MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.LavaSwiftSwim)
            stats.abilitiesGot_Temporary.LavaSwiftSwim = true;
        else
            stats.abilitiesGot_Temporary.LavaSwiftSwim = false;

        if (MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.HikerGear)
            stats.abilitiesGot_Temporary.HikerGear = true;
        else
            stats.abilitiesGot_Temporary.HikerGear = false;

        if (MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.IceSpikes)
            stats.abilitiesGot_Temporary.IceSpikes = true;
        else
            stats.abilitiesGot_Temporary.IceSpikes = false;

        if (MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.GrapplingHook)
            stats.abilitiesGot_Temporary.GrapplingHook = true;
        else
            stats.abilitiesGot_Temporary.GrapplingHook = false;

        if (MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Hammer)
            stats.abilitiesGot_Temporary.Hammer = true;
        else
            stats.abilitiesGot_Temporary.Hammer = false;

        if (MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.ClimbingGear)
            stats.abilitiesGot_Temporary.ClimbingGear = true;
        else
            stats.abilitiesGot_Temporary.ClimbingGear = false;

        if (MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Dash)
            stats.abilitiesGot_Temporary.Dash = true;
        else
            stats.abilitiesGot_Temporary.Dash = false;

        if (MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Ascend)
            stats.abilitiesGot_Temporary.Ascend = true;
        else
            stats.abilitiesGot_Temporary.Ascend = false;

        if (MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.Descend)
            stats.abilitiesGot_Temporary.Descend = true;
        else
            stats.abilitiesGot_Temporary.Descend = false;

        if (MapManager.Instance.mapInfo_ToSave.abilitiesGotInLevel.ControlStick)
            stats.abilitiesGot_Temporary.ControlStick = true;
        else
            stats.abilitiesGot_Temporary.ControlStick = false;

        Player_BlockDetector.Instance.Action_MadeFirstRaycast_Invoke();
    }


    //--------------------


    public void TakeAStep()
    {
        //Reduce available steps
        if (PlayerManager.Instance.block_StandingOn_Current.block)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>() && !PlayerManager.Instance.isTransportingPlayer)
            {
                stats.steps_Current -= PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().GetMovementCost();
            }
        }

        Player_Movement.Instance.Action_StepCostTakenInvoke();

        //If steps is < 0
        if (stats.steps_Current < 0)
        {
            stats.steps_Current = 0;
            RespawnPlayer();
        }

        //Update the stepCounter UI
        UIManager.Instance.UpdateUI();
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
        //Set Pause parameters
        PlayerManager.Instance.pauseGame = true;
        PlayerManager.Instance.isTransportingPlayer = true;
        Player_Movement.Instance.movementStates = MovementStates.Moving;

        RespawnPlayerEarly_Action();

        yield return new WaitForSeconds(waitTime);

        //Move player
        transform.position = MapManager.Instance.playerStartPos;
        transform.SetPositionAndRotation(MapManager.Instance.playerStartPos, Quaternion.identity);
        Player_Movement.Instance.SetPlayerBodyRotation(0);
        RespawnPlayer_Action();

        yield return new WaitForSeconds(waitTime);

        //Rest Block colors
        Player_Movement.Instance.Action_ResetBlockColorInvoke();

        //Refill Steps to max + stepPickups gotten
        RefillStepsToMax();

        //Update active abilities acording to the MapInfo
        UpdateActiveAbilities();

        //Update the UI
        UIManager.Instance.UpdateUI();
        Player_Movement.Instance.movementStates = MovementStates.Still;

        //stats.ResetTempStats(); //Reset all tempAbilities to not be active

        yield return new WaitForSeconds(waitTime * 25);

        RespawnPlayerLate_Action();

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
    public void RespawnPlayerEarly_Action()
    {
        Action_RespawnPlayerEarly?.Invoke();
    }
    public void RespawnPlayer_Action()
    {
        Action_RespawnPlayer?.Invoke();
    }
    public void RespawnPlayerLate_Action()
    {
        Action_RespawnPlayerLate?.Invoke();
    }
    public void RespawnToSavePos_Action()
    {
        Action_RespawnToSavePos?.Invoke();
    }
}