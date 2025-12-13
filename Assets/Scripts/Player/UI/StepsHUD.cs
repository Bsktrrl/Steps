using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepsHUD : Singleton<StepsHUD>
{
    [SerializeField] List<GameObject> stepsIconList = new List<GameObject>();


    //--------------------


    private void OnEnable()
    {
        Interactable_Pickup.Action_PickupGot += UpdateStepsDisplay;
        Movement.Action_RespawnPlayerLate += UpdateStepsDisplay;
        Movement.Action_StepTaken += UpdateStepsDisplay;
        DataManager.Action_dataHasLoaded += UpdateStepsDisplay;
        Block_Checkpoint.Action_SpawnPointEntered += UpdateStepsDisplay;
        Block_RefillSteps.Action_RefillStepsEntered += UpdateStepsDisplay;
        Block_MushroomCircle.Action_MushroomCircleEntered += UpdateStepsDisplay;
    }
    private void OnDisable()
    {
        Interactable_Pickup.Action_PickupGot -= UpdateStepsDisplay;
        Movement.Action_RespawnPlayerLate -= UpdateStepsDisplay;
        Movement.Action_StepTaken -= UpdateStepsDisplay;
        DataManager.Action_dataHasLoaded -= UpdateStepsDisplay;
        Block_Checkpoint.Action_SpawnPointEntered -= UpdateStepsDisplay;
        Block_RefillSteps.Action_RefillStepsEntered -= UpdateStepsDisplay;
        Block_MushroomCircle.Action_MushroomCircleEntered -= UpdateStepsDisplay;
    }


    //--------------------


    public void UpdateStepsDisplay()
    {
        for (int i = 0; i < 10; i++)
        {
            UpdateColors(i);
        }
    }

    void UpdateColors(int index)
    {
        if (index <= 6)
        {
            if (PlayerStats.Instance.stats.steps_Current >= index + 1)
                stepsIconList[index].GetComponent<Image>().color = StepsDisplay.Instance.normalColor_Active;
            else
                stepsIconList[index].GetComponent<Image>().color = StepsDisplay.Instance.normalColor_Used;
        }
        else
        {
            UpdateBonusStepsColors(7);
            UpdateBonusStepsColors(8);
            UpdateBonusStepsColors(9);
        }
    }
    void UpdateBonusStepsColors(int index)
    {
        if (PlayerStats.Instance.stats.steps_Max >= index + 1)
        {
            if (PlayerStats.Instance.stats.steps_Current >= index + 1)
                stepsIconList[index].GetComponent<Image>().color = StepsDisplay.Instance.bonusColor_Active;
            else
                stepsIconList[index].GetComponent<Image>().color = StepsDisplay.Instance.bonusColor_Used;
        }
        else
        {
            stepsIconList[index].GetComponent<Image>().color = StepsDisplay.Instance.bonusColor_Passive;
        }
    }
}
