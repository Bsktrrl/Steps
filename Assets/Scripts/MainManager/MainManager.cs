using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : Singleton<MainManager>
{
    [Header("Player Object")]
    public GameObject player;
    public GameObject playerBody;

    [Header("Player Block Moving Towards")]
    public DetectedBlockInfo block_MovingTowards;

    [Header("Player Block Standing On Info")]
    public DetectedBlockInfo block_StandingOn;

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
}

[Serializable]
public class DetectedBlockInfo
{
    public GameObject block;
    public Vector3 blockPosition;

    public BlockElement blockElement;
    public BlockType blockType;
}
