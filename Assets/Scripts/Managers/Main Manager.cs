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
        playerStats.stepsMax = 5;
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
        if (playerStats.stepsLeft < 0)
        {
            noMoreSteps?.Invoke();

            playerStats.stepsLeft = playerStats.stepsMax;
        }

        updateUI?.Invoke();
    }
    void RefillSteps()
    {
        playerStats.stepsLeft = playerStats.stepsMax;
    }
}


[Serializable]
public class PlayerStats
{
    public PlatformTypes platformType_StandingOn;
    public GameObject platformObject_StandingOn;

    public float movementSpeed = 3f;

    public int stepsMax;
    public int stepsLeft;
}

[Serializable]
public class Collectables
{
    public int coin;
}