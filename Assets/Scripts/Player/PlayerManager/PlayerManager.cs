using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    #region Variables

    [Header("Player Object")]
    public GameObject player;
    public GameObject playerBody;

    [Header("Data")]
    public GameObject dataManagerObject;

    [Header("Player Block Moving Towards")]
    [HideInInspector] public DetectedBlockInfo block_MovingTowards;

    [Header("Player Block Looking At")]
    public Vector3 lookingDirection;
    [HideInInspector] public GameObject block_LookingAt_Horizontal;
    [HideInInspector] public GameObject block_LookingAt_Vertical;

    [Header("Player Block Standing On Info")]
    [HideInInspector] public DetectedBlockInfo block_StandingOn_Current;
    [HideInInspector] public GameObject block_StandingOn_Previous;

    [Header("Player Block Horizontal")]
    [HideInInspector] public DetectedBlockInfo block_Horizontal_InFront;
    [HideInInspector] public DetectedBlockInfo block_Horizontal_InBack;
    [HideInInspector] public DetectedBlockInfo block_Horizontal_ToTheLeft;
    [HideInInspector] public DetectedBlockInfo block_Horizontal_ToTheRight;

    [Header("Player Block Vertical")]
    [HideInInspector] public DetectedBlockInfo block_Vertical_InFront;
    [HideInInspector] public DetectedBlockInfo block_Vertical_InBack;
    [HideInInspector] public DetectedBlockInfo block_Vertical_ToTheLeft;
    [HideInInspector] public DetectedBlockInfo block_Vertical_ToTheRight;

    [Header("Player Movement Restrictions")]
    public bool canMove_Forward;
    public bool canMove_Back;
    public bool canMove_Left;
    public bool canMove_Right;

    [Header("Game Paused")]
    public bool pauseGame;
    public bool isTransportingPlayer;

    [Header("Quicksand")]
    [HideInInspector] public int quicksandCounter;

    #endregion


    //--------------------


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
}

[Serializable]
public class DetectedBlockInfo
{
    public GameObject block;
    public Vector3 blockPosition;

    public BlockType blockType;
}
