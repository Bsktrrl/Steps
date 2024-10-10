using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : Singleton<MainManager>
{
    [Header("Player Object")]
    public GameObject player;

    [Header("Player Layer Masks")]
    public LayerMask playerLayerMask;
    public LayerMask player_SideForward_LayerMask;
    public LayerMask player_SideBack_LayerMask;
    public LayerMask player_SideLeft_LayerMask;
    public LayerMask player_SideRight_LayerMask;

    [Header("Cube Layer Mask")]
    public LayerMask Cube;
    public LayerMask Stair;
    public LayerMask Ladder;

    [Header("Player Movement Restrictions")]
    public bool canMove_Forward;
    public bool canMove_Back;
    public bool canMove_Left;
    public bool canMove_Right;

    [Header("Cameras")]
    public GameObject camera_Forward;
    public GameObject camera_Backward;
    public GameObject camera_Left;
    public GameObject camera_Right;

    [Header("Game Paused")]
    public bool pauseGame;
}
