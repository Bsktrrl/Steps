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

    [Header("KeyPresses")]
    public bool forward_isPressed;
    public bool back_isPressed;
    public bool left_isPressed;
    public bool right_isPressed;

    public bool cameraX_isPressed;
    public bool cameraY_isPressed;

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
        RespawnPlayerIfToLowInMapHeight();
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

        return true;
    }



    //--------------------


    void RespawnPlayerIfToLowInMapHeight()
    {
        if (transform.position.y <= -5)
        {
            PlayerStats.Instance.RespawnPlayer();
        }
    }



    //--------------------


    void OnForward_Down()
    {
        forward_isPressed = true;
    }
    void OnForward_Up()
    {
        forward_isPressed = false;
    }
    void OnBackward_Down()
    {
        back_isPressed = true;
    }
    void OnBackward_Up()
    {
        back_isPressed = false;
    }
    void OnLeft_Down()
    {
        left_isPressed = true;
    }
    void OnLeft_Up()
    {
        left_isPressed = false;
    }
    void OnRight_Down()
    {
        right_isPressed = true;
    }
    void OnRight_Up()
    {
        right_isPressed = false;
    }

    void OnCameraRotateX()
    {
        Cameras_v2.Instance.RotateCameraX();
    }
    void OnCameraRotateY()
    {
        Cameras_v2.Instance.RotateCameraY();
    }
    void OnAbilityUp()
    {
        Player_Movement.Instance.Key_SwiftSwimUp();
        Player_Ascend.Instance.RunAscend();
    }
    void OnAbilityDown()
    {
        Player_Movement.Instance.Key_SwiftSwimDown();
        Player_Interact.Instance.InteractWithObject();
        Player_Descend.Instance.RunDescend();
        Player_Movement.Instance.Action_PressMoveBlockButtonInvoke();
    }
    void OnAbilityLeft()
    {
        Player_CeilingGrab.Instance.CeilingGrab();
    }
    void OnAbilityRight_DownPress()
    {
        Player_GraplingHook.Instance.StartGrappling();
    }
    void OnAbilityRight_RelesePress()
    {
        Player_GraplingHook.Instance.StopGrappling();
    }
    void OnRespawn()
    {
        Player_Movement.Instance.Key_Respawn();
    }
    void OnQuit()
    {
        Player_Movement.Instance.Key_Quit();
    }
}

[Serializable]
public class DetectedBlockInfo
{
    public GameObject block;
    public Vector3 blockPosition;

    public BlockType blockType;
}
