using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : Singleton<Player_Stats>
{
    public static event Action Action_RespawnToSavePos;

    public static event Action updateCoins;
    public static event Action updateStepMax;

    public static event Action updateSwimsuit;
    public static event Action updateFlippers;
    public static event Action updateHikerGear;
    public static event Action updateLavaSuit;

    public Vector3 savePos;

    public Stats stats;
    public Collectables collectables;
    public Upgrades upgrades;


    //--------------------


    private void Start()
    {
        Player_Movement.Action_takeAStep += TakeAStep;

        savePos = transform.position;

        stats.steps_Max = 20;
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

    public void TakeAStep()
    {
        //Reduce available steps
        stats.steps_Current -= Player_Movement.Instance.currentMovementCost;

        //If steps is < 0
        if (stats.steps_Current < 0)
        {
            //Set available steps to max
            stats.steps_Current = stats.steps_Max;

            //Transfer the player to last savePos
            switch (Player_Camera.Instance.cameraState)
            {
                case CameraState.Forward:
                    gameObject.transform.SetPositionAndRotation(savePos, gameObject.transform.rotation);
                    MainManager.Instance.playerBody.transform.SetPositionAndRotation(savePos, Quaternion.Euler(Vector3.forward));
                    break;
                case CameraState.Backward:
                    gameObject.transform.SetPositionAndRotation(savePos, gameObject.transform.rotation);
                    MainManager.Instance.playerBody.transform.SetPositionAndRotation(savePos, Quaternion.Euler(Vector3.back));
                    break;
                case CameraState.Left:
                    gameObject.transform.SetPositionAndRotation(savePos, gameObject.transform.rotation);
                    MainManager.Instance.playerBody.transform.SetPositionAndRotation(savePos, Quaternion.Euler(Vector3.left));
                    break;
                case CameraState.Right:
                    gameObject.transform.SetPositionAndRotation(savePos, gameObject.transform.rotation);
                    MainManager.Instance.playerBody.transform.SetPositionAndRotation(savePos, Quaternion.Euler(Vector3.right));
                    break;

                default:
                    break;
            }

            //Reset all darkening tiles
            Action_RespawnToSavePos?.Invoke();
        }

        //Update the stepCounter UI
        UIManager.Instance.UpdateStepsUI();
    }


    void AddSwimSuit()
    {
        upgrades.SwimSuit = true;
        updateSwimsuit?.Invoke();
    }
    void AddFlippers()
    {
        upgrades.Flippers = true;
        updateFlippers?.Invoke();
    }
    void AddHikerGear()
    {
        upgrades.HikerGear = true;
        updateHikerGear?.Invoke();
    }
    void AddLavaSuit()
    {
        upgrades.LavaSuit = true;
        updateLavaSuit?.Invoke();
    }
}


//--------------------


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
public class Upgrades
{
    public bool SwimSuit;
    public bool Flippers;
    public bool HikerGear;
    public bool FenceSneak;

    public bool LavaSuit;
}