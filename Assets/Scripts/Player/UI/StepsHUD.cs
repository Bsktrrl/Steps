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
        //Interactable_Pickup.Action_PickupGot += UpdateStepsDisplay_Walking;
        Movement.Action_StepTaken += UpdateStepsDisplay_Walking;
        //Block_MushroomCircle.Action_MushroomCircleEntered += UpdateStepsDisplay_Walking;

        Movement.Action_RespawnPlayerLate += UpdateStepsDisplay_Respawn;

        DataManager.Action_dataHasLoaded += UpdateStepsDisplay_Checkpoint;
        Block_Checkpoint.Action_CheckPointEntered += UpdateStepsDisplay_Checkpoint;
        //Block_RefillSteps.Action_RefillStepsEntered += UpdateStepsDisplay_Respawn;
    }
    private void OnDisable()
    {
        //Interactable_Pickup.Action_PickupGot -= UpdateStepsDisplay_Walking;
        Movement.Action_StepTaken -= UpdateStepsDisplay_Walking;
        //Block_MushroomCircle.Action_MushroomCircleEntered -= UpdateStepsDisplay_Walking;

        Movement.Action_RespawnPlayerLate -= UpdateStepsDisplay_Respawn;

        DataManager.Action_dataHasLoaded -= UpdateStepsDisplay_Checkpoint;
        Block_Checkpoint.Action_CheckPointEntered -= UpdateStepsDisplay_Checkpoint;
        //Block_RefillSteps.Action_RefillStepsEntered -= UpdateStepsDisplay_Respawn;
    }


    //--------------------


    public void UpdateStepsDisplay_Walking()
    {
        if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<EffectBlockInfo>() && !Movement.Instance.blockStandingOn.GetComponent<EffectBlockInfo>().effectBlock_SpawnPoint_isAdded)
        {
            for (int i = 0; i < 10; i++)
            {
                UpdateFootprints(i);
            }
        }
    }
    public void UpdateStepsDisplay_Respawn()
    {
        print("1. UpdateStepsDisplay_Respawn");
        StartCoroutine(UpdateFootprintDelay(0.5f, 0.15f));
    }
    public void UpdateStepsDisplay_Checkpoint()
    {
        print("2. UpdateStepsDisplay_Checkpoint");
        StartCoroutine(UpdateFootprintDelay(0.65f, 0.15f));
    }
    IEnumerator UpdateFootprintDelay(float startDelay, float waitTime)
    {
        yield return new WaitForSeconds(startDelay);

        for (int i = 0; i < 10; i++)
        {
            if (stepsIconList[i].GetComponent<Image>().sprite == StepsDisplay.Instance.normalFootstep_Active)
            {
                continue;
            }
            else
            {
                yield return new WaitForSeconds(waitTime);
                UpdateFootprints(i);
            }
        }
    }

    void UpdateFootprints(int index)
    {
        if (index <= 6)
        {
            if (PlayerStats.Instance.stats.steps_Current >= index + 1)
                stepsIconList[index].GetComponent<Image>().sprite = StepsDisplay.Instance.normalFootstep_Active;
            else
                stepsIconList[index].GetComponent<Image>().sprite = StepsDisplay.Instance.normalFootstep_Used;
        }
        else
        {
            UpdateBonusStepsColors(index);
            //UpdateBonusStepsColors(8);
            //UpdateBonusStepsColors(9);
        }
    }
    void UpdateBonusStepsColors(int index)
    {
        if (PlayerStats.Instance.stats.steps_Max >= index + 1)
        {
            if (PlayerStats.Instance.stats.steps_Current >= index + 1)
                stepsIconList[index].GetComponent<Image>().sprite = StepsDisplay.Instance.extraFootstep_Active;
            else
                stepsIconList[index].GetComponent<Image>().sprite = StepsDisplay.Instance.extraFootstep_Used;
        }
        else
        {
            stepsIconList[index].GetComponent<Image>().sprite = StepsDisplay.Instance.extraFootstep_Passive;
        }
    }
}
