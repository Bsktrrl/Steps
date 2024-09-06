using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>
{
    public static event Action updateCoins;
    public static event Action updateStepMax;
    public static event Action updateSwimsuit;
    public static event Action updateFlippers;
    public static event Action updateHikerGear;
    public static event Action updateLavaSuit;

    public Vector3 startPos;

    public Stats stats;
    public Collectables collectables;
    public KeyItems keyItems;


    //--------------------


    private void Start()
    {
        startPos = transform.position;

        stats.steps_Max = 10;
        stats.steps_Current = stats.steps_Max;

        updateStepMax?.Invoke();
    }

    //Make sure the Subscriptions doesn't bug out
    private void OnEnable()
    {
        PickupObject.pickup_Coin_IsHit += AddCoin;
        PickupObject.pickup_Step_IsHit += AddMaxStep;

        PickupObject.pickup_KeyItem_SwimSuit_IsHit += AddSwimSuit;
        PickupObject.pickup_KeyItem_Flippers_IsHit += AddFlippers;
        PickupObject.pickup_KeyItem_HikerGear_IsHit += AddHikerGear;
        PickupObject.pickup_KeyItem_LavaSuit_IsHit += AddLavaSuit;
    }
    private void OnDisable()
    {
        PickupObject.pickup_Coin_IsHit -= AddCoin;
        PickupObject.pickup_Step_IsHit -= AddMaxStep;

        PickupObject.pickup_KeyItem_SwimSuit_IsHit -= AddSwimSuit;
        PickupObject.pickup_KeyItem_Flippers_IsHit -= AddFlippers;
        PickupObject.pickup_KeyItem_HikerGear_IsHit -= AddHikerGear;
        PickupObject.pickup_KeyItem_LavaSuit_IsHit -= AddLavaSuit;
    }


    //--------------------


    void AddCoin()
    {
        collectables.coin += 1;
        updateCoins?.Invoke();
    }
    void AddMaxStep()
    {
        stats.steps_Max += 1;
        stats.steps_Current = stats.steps_Max;

        updateStepMax?.Invoke();
    }



    void AddSwimSuit()
    {
        keyItems.SwimSuit = true;
        updateSwimsuit?.Invoke();
    }
    void AddFlippers()
    {
        keyItems.Flippers = true;
        updateFlippers?.Invoke();
    }
    void AddHikerGear()
    {
        keyItems.HikerGear = true;
        updateHikerGear?.Invoke();
    }
    void AddLavaSuit()
    {
        keyItems.LavaSuit = true;
        updateLavaSuit?.Invoke();
    }
}

[Serializable]
public class Stats
{
    [Header("Movement Speed")]
    public float movementSpeed = 5;

    [Header("Steps")]
    public int steps_Current;
    public int steps_Max;
}

[Serializable]
public class Collectables
{
    public int coin;
}

[Serializable]
public class KeyItems
{
    public bool SwimSuit;
    public bool Flippers;
    public bool HikerGear;

    public bool LavaSuit;
}