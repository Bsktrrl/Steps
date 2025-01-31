using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerManager : Singleton<PlayerManager>
{
    #region Variables

    [Header("Input System")]
    PlayerControls playerControls; 

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


    private void Start()
    {
        playerControls = new PlayerControls();

        //Change Cursor State
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
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



    //--------------------


    void OnForward()
    {
        Player_Movement.Instance.Key_MoveForward();
        Player_Dash.Instance.Dash_Forward();
        Player_Jumping.Instance.Jump_Forward();
        print("OnForward");
    }
    void OnBackward()
    {
        Player_Movement.Instance.Key_MoveBackward();
        Player_Dash.Instance.Dash_Backward();
        Player_Jumping.Instance.Jump_Backward();
        print("OnBackward");
    }
    void OnLeft()
    {
        Player_Movement.Instance.Key_MoveLeft();
        Player_Dash.Instance.Dash_Left();
        Player_Jumping.Instance.Jump_Left();
        print("OnLeft");
    }
    void OnRight()
    {
        Player_Movement.Instance.Key_MoveRight();
        Player_Dash.Instance.Dash_Right();
        Player_Jumping.Instance.Jump_Right();
        print("OnRight");
    }
    void OnCameraRotateX()
    {
        Cameras_v2.Instance.RotateCameraX();
        print("OnCameraRotateX");
    }
    void OnCameraRotateY()
    {
        Cameras_v2.Instance.RotateCameraY();
        print("OnCameraRotateY");
    }
    void OnAbilityUp()
    {
        Player_Movement.Instance.Key_SwiftSwimUp();
        Player_Ascend.Instance.RunAscend();
        print("OnAbilityUp");
    }
    void OnAbilityDown()
    {
        Player_Movement.Instance.Key_SwiftSwimDown();
        Player_Interact.Instance.InteractWithObject();
        Player_Descend.Instance.RunDescend();
        Player_Movement.Instance.Action_PressMoveBlockButtonInvoke();
        print("OnAbilityDown");
    }
    void OnAbilityLeft()
    {
        Player_CeilingGrab.Instance.CeilingGrab();
        print("OnAbilityLeft");
    }
    void OnAbilityRight_DownPress()
    {
        Player_GraplingHook.Instance.StartGrappling();
        print("OnAbilityRight_DownPress");
    }
    void OnAbilityRight_RelesePress()
    {
        Player_GraplingHook.Instance.StopGrappling();
        print("OnAbilityRight_RelesePress");
    }
    void OnRespawn()
    {
        Player_Movement.Instance.Key_Respawn();
        print("OnRespawn");
    }
    void OnQuit()
    {
        Player_Movement.Instance.Key_Quit();
        print("OnQuit");
    }
}

[Serializable]
public class DetectedBlockInfo
{
    public GameObject block;
    public Vector3 blockPosition;

    public BlockType blockType;
}
