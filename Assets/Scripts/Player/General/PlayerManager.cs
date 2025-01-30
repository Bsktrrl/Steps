using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerManager : Singleton<PlayerManager>
{
    #region Variables

    [Header("Player Object")]
    public GameObject player;
    public GameObject playerBody;

    [Header("Data")]
    public GameObject dataManagerObject;

    [Header("Player Block Moving Towards")]
    public DetectedBlockInfo block_MovingTowards;

    [Header("Player Block Looking At")]
    public Vector3 lookingDirection;
    public GameObject block_LookingAt_Horizontal;
    public GameObject block_LookingAt_Vertical;

    [Header("Player Block Standing On Info")]
    public DetectedBlockInfo block_StandingOn_Current;
    public GameObject block_StandingOn_Previous;

    [Header("Player Block Horizontal")]
    public DetectedBlockInfo block_Horizontal_InFront;
    public DetectedBlockInfo block_Horizontal_InBack;
    public DetectedBlockInfo block_Horizontal_ToTheLeft;
    public DetectedBlockInfo block_Horizontal_ToTheRight;

    [Header("Player Block Vertical")]
    public DetectedBlockInfo block_Vertical_InFront;
    public DetectedBlockInfo block_Vertical_InBack;
    public DetectedBlockInfo block_Vertical_ToTheLeft;
    public DetectedBlockInfo block_Vertical_ToTheRight;

    [Header("Player Movement Restrictions")]
    public bool canMove_Forward;
    public bool canMove_Back;
    public bool canMove_Left;
    public bool canMove_Right;

    [Header("Game Paused")]
    public bool pauseGame;
    public bool isTransportingPlayer;

    #endregion


    //--------------------

    private void Update()
    {
        RespawnPlayerIfToLow();
    }

    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += LoadPlayerStats;
        Player_Movement.Action_StepTaken += StepsOnFallableBlock;
        Player_Movement.Action_StepTaken += MakeStepSound;
    }

    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= LoadPlayerStats;
        Player_Movement.Action_StepTaken -= StepsOnFallableBlock;
        Player_Movement.Action_StepTaken -= MakeStepSound;
    }


    //--------------------


    void LoadPlayerStats()
    {
        SaveLoad_PlayerStats.Instance.LoadData();
    }
    public void SavePlayerStats()
    {
        SaveLoad_PlayerStats.Instance.SaveData();
    }


    //--------------------


    void MakeStepSound()
    {
        if (block_StandingOn_Current.block)
        {
            if (block_StandingOn_Current.block.GetComponent<BlockInfo>())
            {
                block_StandingOn_Current.block.GetComponent<BlockInfo>().MakeStepSound();
            }
        }
    }
    void StepsOnFallableBlock()
    {
        if (block_StandingOn_Current.block)
        {
            if (block_StandingOn_Current.block.GetComponent<Block_Falling>())
            {
                block_StandingOn_Current.block.GetComponent<Block_Falling>().StepsOnFallableBlock();
            }
        }
    }


    public bool PreventButtonsOfTrigger()
    {
        if (Player_Movement.Instance.movementStates == MovementStates.Moving) { return false; }

        if (Player_Movement.Instance.isSlopeGliding) { return false; }
        if (Player_Movement.Instance.isIceGliding) { return false; }

        if (pauseGame) { return false; }
        if (isTransportingPlayer) { return false; }
        if (Cameras_v2.Instance.isRotating) { return false; }
        if (Player_Interact.Instance.isInteracting) { return false; }
        if (Player_GraplingHook.Instance.isGrapplingHooking) { return false; }
        if (Player_Dash.Instance.isDashing) { return false; }
        if (Player_Ascend.Instance.isAscending) { return false; }
        if (Player_Descend.Instance.isDescending) { return false; }
        if (Player_Jumping.Instance.isJumping) { return false; }
        if (Player_SwiftSwim.Instance.isSwiftSwimming_Down) { return false; }
        if (Player_SwiftSwim.Instance.isSwiftSwimming_Up) { return false; }
        if (!Player_CeilingGrab.Instance.isCeilingRotation) { return false; }

        if (Player_Movement.Instance.ladderMovement_Up) { return false; }
        if (Player_Movement.Instance.ladderMovement_Down) { return false; }
        if (Player_Movement.Instance.ladderMovement_Top) { return false; }
        if (Player_Movement.Instance.ladderMovement_Top_ToBlock) { return false; }
        if (Player_Movement.Instance.ladderMovement_Down_ToBlockFromTop) { return false; }
        if (Player_Movement.Instance.ladderMovement_Down_ToBottom) { return false; }

        return true;
    }



    //--------------------


    void RespawnPlayerIfToLow()
    {
        if (transform.position.y <= -5)
        {
            PlayerStats.Instance.RespawnPlayer();
        }
    }
}

[Serializable]
public class DetectedBlockInfo
{
    public GameObject block;
    public Vector3 blockPosition;

    public BlockType blockType;
}
