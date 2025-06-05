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
        Movement.Action_StepTaken += TakeAStep;
        //DataManager.Action_dataHasLoaded += RespawnPlayer;
        DataManager.Action_dataHasLoaded += RefillStepsToMax;
        DataManager.Action_dataHasLoaded += UpdateActiveAbilities;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken -= TakeAStep;
        //DataManager.Action_dataHasLoaded -= RespawnPlayer;
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
    void ResetActiveAbilities()
    {
        if (PlayerManager.Instance.player)
        {
            if (PlayerManager.Instance.player.GetComponent<PlayerStats>())
            {
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.SwimSuit = false;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.SwiftSwim = false;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Flippers = false;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Jumping = false;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.GrapplingHook = false;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.CeilingGrab = false;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Dash = false;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Ascend = false;
                PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Descend = false;
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


    public void TakeAStep()
    {
        //Reduce available steps
        if (PlayerManager.Instance.block_StandingOn_Current.block)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>() /*&& !PlayerManager.Instance.isTransportingPlayer*/ && !Player_Pusher.Instance.playerIsPushed)
            {
                stats.steps_Current -= PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementCost;
            }
        }

        //If steps is < 0
        if (stats.steps_Current < 0)
        {
            stats.steps_Current = 0;
            RespawnPlayer();
        }
    }
    public void RespawnPlayer()
    {
        print("Respawn Player");

        StartCoroutine(ResetplayerPos(0.01f));
    }

    IEnumerator ResetplayerPos(float waitTime)
    {
        //Set Pause parameters
        //PlayerManager.Instance.isTransportingPlayer = true;
        Movement.Instance.SetMovementState(MovementStates.Moving);

        RespawnPlayerEarly_Action();

        yield return new WaitForSeconds(waitTime);

        //Move player
        transform.position = MapManager.Instance.playerStartPos;
        transform.SetPositionAndRotation(MapManager.Instance.playerStartPos, Quaternion.identity);

        RespawnPlayer_Action();

        //Reset for CeilingAbility
        Player_CeilingGrab.Instance.ResetCeilingGrab();

        Player_DarkenBlock.Instance.block_hasBeenDarkened = false;

        yield return new WaitForSeconds(waitTime);

        //Refill Steps to max + stepPickups gotten
        RefillStepsToMax();

        //Update active abilities according to the MapInfo
        //UpdateActiveAbilities();

        Movement.Instance.SetMovementState(MovementStates.Still);

        CameraController.Instance.ResetCameraRotation();
        Movement.Instance.RotatePlayerBody(0);

        //Player_DarkenBlock.Instance.SetStartingDarkenBlock();

        //yield return new WaitForSeconds(waitTime * 25);

        yield return new WaitForSeconds(waitTime * 30);

        RespawnPlayerLate_Action();

        //PlayerManager.Instance.isTransportingPlayer = false;
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