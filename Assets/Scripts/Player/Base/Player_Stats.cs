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
        if (MainManager.Instance.block_StandingOn_Current.block)
        {
            if (MainManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>())
            {
                stats.steps_Current -= MainManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().GetMovementCost();
            }
        }

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
        stats.abilities.FenceSneak = true;
    }
    public void UpdateSwimsuit()
    {
        stats.abilities.SwimSuit = true;
    }
    public void UpdateSwiftSwim()
    {
        stats.abilities.SwiftSwim = true;
    }
    public void UpdateFlippers()
    {
        stats.abilities.Flippers = true;

        //Find all gameObjects in the scene containing a Block_Water component
        Block_Water[] waterBlocks = FindObjectsOfType<Block_Water>();

        //Update the movementCost of all blocks
        foreach (Block_Water block in waterBlocks)
        {
            block.UpdateFastSwimmingMovementCost();
        }
    }
    public void UpdateLavaSuit()
    {
        stats.abilities.LavaSuit = true;
    }
    public void UpdateLavaSwiftSwim()
    {
        stats.abilities.LavaSwiftSwim = true;
    }
    public void UpdateHikerGear()
    {
        stats.abilities.HikerGear = true;
    }
    public void UpdateIceSpikes()
    {
        stats.abilities.IceSpikes = true;
    }
    public void UpdateGrapplingHook()
    {
        stats.abilities.GrapplingHook = true;
    }
    public void UpdateHammer()
    {
        stats.abilities.Hammer = true;
    }
    public void UpdateClimbingGear()
    {
        stats.abilities.ClimbingGear = true;
    }
    public void UpdateDash()
    {
        stats.abilities.Dash = true;
    }
    public void UpdateAscend()
    {
        stats.abilities.Ascend = true;
    }
    public void UpdateDescend()
    {
        stats.abilities.Descend = true;
    }
    public void UpdateControlStick()
    {
        stats.abilities.ControlStick = true;
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
    public bool SwiftSwim;
    public bool Flippers;
    public bool LavaSuit;
    public bool LavaSwiftSwim;

    public bool HikerGear; //Moving up Slopes

    public bool IceSpikes;
    public bool GrapplingHook;
    public bool Hammer;
    public bool ClimbingGear;
    public bool Dash;
    public bool Ascend;
    public bool Descend;
    public bool ControlStick;
}