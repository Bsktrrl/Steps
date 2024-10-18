using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Player_Stats : Singleton<Player_Stats>
{
    public static event Action Action_RespawnToSavePos;

    public static event Action updateCoins;
    public static event Action updateStepsMax;

    public static event Action updateFenceSneak;
    public static event Action updateSwimsuit;
    public static event Action updateFlippers;
    public static event Action updateHikerGear;
    public static event Action updateLavaSuit;

    public Vector3 savePos;

    public Stats stats;


    //--------------------


    private void Start()
    {
        Player_Movement.Action_StepTaken += TakeAStep;

        savePos = transform.position;

        stats.steps_Max = 200;
        stats.steps_Current = stats.steps_Max;

        updateCoins?.Invoke();
        updateStepsMax?.Invoke();
    }


    //--------------------


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
            switch (Cameras.Instance.cameraState)
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


    //--------------------


    //Items
    public void UpdateCoins()
    {
        updateCoins?.Invoke();
    }
    public void UpdateStepsMax()
    {
        updateStepsMax?.Invoke();
    }

    //Abilities
    public void UpdateFenceSneak()
    {
        updateFenceSneak?.Invoke();
    }
    public void UpdateSwimsuit()
    {
        updateSwimsuit?.Invoke();
    }
    public void UpdateFlippers()
    {
        updateFlippers?.Invoke();
    }
    public void UpdateHikerGear()
    {
        updateHikerGear?.Invoke();
    }
    public void UpdateLavaSuit()
    {
        updateLavaSuit?.Invoke();
    }
}


//--------------------


[Serializable]
public class Stats
{
    [Header("Steps")]
    public int steps_Max;
    public int steps_Current;

    [Header("Items")]
    public ItemsGot inventoryItems;

    [Header("Abilities")]
    public AbilitiesGot abilities;
}

[Serializable]
public class ItemsGot
{
    public int coin;
}

[Serializable]
public class AbilitiesGot
{
    public bool FenceSneak;
    public bool SwimSuit;
    public bool Flippers;
    public bool LavaSuit;

    public bool HikerGear;
}