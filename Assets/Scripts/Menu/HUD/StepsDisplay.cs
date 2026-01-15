using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StepsDisplay : Singleton<StepsDisplay>
{
    [Header("Parents")]
    [SerializeField] GameObject footsteps_Parent;

    [SerializeField] GameObject stepDisplay_Steps;
    [SerializeField] GameObject stepDisplay_Number;
    [SerializeField] GameObject stepDisplay_NumbersSteps;

    [Header("Sprites")]
    public Sprite normalFootstep_Active;
    public Sprite normalFootstep_Used;

    public Sprite extraFootstep_Passive;
    public Sprite extraFootstep_Active;
    public Sprite extraFootstep_Used;



    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += SetStepsDisplay;
        SettingsManager.Action_SetNewStepDisplay += SetStepsDisplay;

        //Interactable_Pickup.Action_PickupGot += ChangeStepText;
        //Movement.Action_RespawnPlayerLate += ChangeStepText;
        //Movement.Action_StepTaken += ChangeStepText;
        //DataManager.Action_dataHasLoaded += ChangeStepText;
        //Block_Checkpoint.Action_CheckPointEntered += ChangeStepText;
        //Block_RefillSteps.Action_RefillStepsEntered += ChangeStepText;
        //Block_MushroomCircle.Action_MushroomCircleEntered += ChangeStepText;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= SetStepsDisplay;
        SettingsManager.Action_SetNewStepDisplay -= SetStepsDisplay;

        //Interactable_Pickup.Action_PickupGot -= ChangeStepText;
        //Movement.Action_RespawnPlayerLate -= ChangeStepText;
        //Movement.Action_StepTaken -= ChangeStepText;
        //DataManager.Action_dataHasLoaded -= ChangeStepText;
        //Block_Checkpoint.Action_CheckPointEntered -= ChangeStepText;
        //Block_RefillSteps.Action_RefillStepsEntered -= ChangeStepText;
        //Block_MushroomCircle.Action_MushroomCircleEntered -= ChangeStepText;
    }


    //--------------------


    void SetStepsDisplay()
    {
        HideAllMenus();

        switch (DataManager.Instance.settingData_StoreList.currentStepDisplay)
        {
            case StepDisplay.Steps:
                stepDisplay_Steps.SetActive(true);
                footsteps_Parent.SetActive(true);
                StepsHUD.Instance.UpdateStepsDisplay_Walking();
                break;
            case StepDisplay.Number:
                stepDisplay_Number.SetActive(true);
                break;
            case StepDisplay.NumberSteps:
                stepDisplay_NumbersSteps.SetActive(true);
                footsteps_Parent.SetActive(true);
                StepsHUD.Instance.UpdateStepsDisplay_Walking();
                break;
            case StepDisplay.None:
                HideAllMenus();
                break;

            default:
                break;
        }
    }

    void HideAllMenus()
    {
        stepDisplay_Steps.SetActive(false);
        stepDisplay_Number.SetActive(false);
        stepDisplay_NumbersSteps.SetActive(false);

        footsteps_Parent.SetActive(false);
    }
}
