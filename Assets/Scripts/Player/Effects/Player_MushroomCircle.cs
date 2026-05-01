using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_MushroomCircle : Singleton<Player_MushroomCircle>
{
    public static event Action Action_StartMushroomCircle;
    public static event Action Action_EndMushroomCircle;
    public static event Action Action_UpdateMushroomCircle;

    int freeStepCost_Max = 3;

    public int freeStepsCounter;

    public bool activeMushroomCircle;


    //--------------------


    private void OnEnable()
    {
        Movement.Action_StepTaken_Late += CheckForMushroomCircle;
        Movement.Action_RespawnPlayer += ResetMushroomCircle;
    }
    private void OnDisable()
    {
        Movement.Action_StepTaken_Late -= CheckForMushroomCircle;
        Movement.Action_RespawnPlayer -= ResetMushroomCircle;
    }


    //--------------------


    void CheckForMushroomCircle()
    {
        //Check if standing on a MushroomCircle
        if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>() && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.MushroomCircle)
        {
            freeStepsCounter = freeStepCost_Max;
            Action_StartMushroomCircle?.Invoke();
            activeMushroomCircle = true;
        }
        else
        {
            //Update FreeStep Counter
            if (freeStepsCounter > 1)
            {
                freeStepsCounter -= 1;
                Action_UpdateMushroomCircle?.Invoke();
            }
            else
            {
                ResetMushroomCircle();
            }
        }

    }


    //--------------------


    void ResetMushroomCircle()
    {
        freeStepsCounter = 0;
        Action_EndMushroomCircle?.Invoke();
        activeMushroomCircle = false;
    }
}
