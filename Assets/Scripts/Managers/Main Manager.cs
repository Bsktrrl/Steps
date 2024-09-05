using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : Singleton<MainManager>
{
    public static event Action updateUI;
    public static event Action noMoreSteps;

    public Vector3 startPos;

    public PlayerStats playerStats;
    public Collectables collectables;

    public bool gamePaused;


    //--------------------


    private void Awake()
    {
        playerStats.stepsMax = 10;
        RefillSteps();

        startPos = new Vector3 (0, 0.3f, 0);
    }


    //--------------------


    //Make sure the Subscriptions doesn't bug out
    private void OnEnable()
    {
        PickupItem.pickup_Coin_IsHit += AddCoin;
        PlayerMovement.takeAStep += TakeAStep;
        PickupItem.pickup_Step_IsHit += AddMaxStep;
    }
    private void OnDisable()
    {
        PickupItem.pickup_Coin_IsHit -= AddCoin;
        PlayerMovement.takeAStep -= TakeAStep;
        PickupItem.pickup_Step_IsHit -= AddMaxStep;
    }


    //--------------------


    void AddCoin()
    {
        collectables.coin += 1;
        updateUI?.Invoke();
    }
    void AddMaxStep()
    {
        playerStats.stepsMax += 1;

        RefillSteps();
        updateUI?.Invoke();
    }
    void TakeAStep()
    {
        if (playerStats.stepsToUse < 0)
        {
            noMoreSteps?.Invoke();

            playerStats.stepsToUse = playerStats.stepsMax;
        }

        updateUI?.Invoke();
    }
    void RefillSteps()
    {
        playerStats.stepsToUse = playerStats.stepsMax;
    }
}


[Serializable]
public class PlayerStats
{
    [Header("Platform Standing On")]
    public GameObject platformObject_StandingOn_Previous;
    public GameObject platformObject_StandingOn_Current;

    [Header("Platform Detected")]
    public GameObject platformObject_Forward;
    public GameObject platformObject_Backward;
    public GameObject platformObject_Right;
    public GameObject platformObject_Left;

    [Header("Movement Speed")]
    public float movementSpeed = 3f;

    [Header("Steps")]
    public int stepsMax;
    public int stepsToUse;
}

[Serializable]
public class Collectables
{
    public int coin;
}