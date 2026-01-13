using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StepsDisplay : Singleton<StepsDisplay>
{
    [Header("Parents")]
    [SerializeField] GameObject stepDisplay_Icons;
    [SerializeField] GameObject stepDisplay_Number;
    [SerializeField] GameObject stepDisplay_NumberIcons;

    [Header("TextObjects")]
    [SerializeField] TextMeshProUGUI stepDisplay_Number_Text;
    [SerializeField] TextMeshProUGUI stepDisplay_NumberIcons_Text;

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

        Interactable_Pickup.Action_PickupGot += ChangeStepText;
        Movement.Action_RespawnPlayerLate += ChangeStepText;
        Movement.Action_StepTaken += ChangeStepText;
        DataManager.Action_dataHasLoaded += ChangeStepText;
        Block_Checkpoint.Action_CheckPointEntered += ChangeStepText;
        Block_RefillSteps.Action_RefillStepsEntered += ChangeStepText;
        Block_MushroomCircle.Action_MushroomCircleEntered += ChangeStepText;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= SetStepsDisplay;
        SettingsManager.Action_SetNewStepDisplay -= SetStepsDisplay;

        Interactable_Pickup.Action_PickupGot -= ChangeStepText;
        Movement.Action_RespawnPlayerLate -= ChangeStepText;
        Movement.Action_StepTaken -= ChangeStepText;
        DataManager.Action_dataHasLoaded -= ChangeStepText;
        Block_Checkpoint.Action_CheckPointEntered -= ChangeStepText;
        Block_RefillSteps.Action_RefillStepsEntered -= ChangeStepText;
        Block_MushroomCircle.Action_MushroomCircleEntered -= ChangeStepText;
    }


    //--------------------


    void SetStepsDisplay()
    {
        HideAllMenus();

        //print("20. CurrentStepsDisplay: " + DataManager.Instance.settingData_StoreList.currentStepDisplay.ToString());

        switch (DataManager.Instance.settingData_StoreList.currentStepDisplay)
        {
            case StepDisplay.Steps:
                stepDisplay_Icons.SetActive(true);
                StepsHUD.Instance.UpdateStepsDisplay_Walking();
                break;
            case StepDisplay.Number:
                stepDisplay_Number.SetActive(true);
                break;
            case StepDisplay.NumberSteps:
                stepDisplay_NumberIcons.SetActive(true);
                stepDisplay_Icons.SetActive(true);
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
        stepDisplay_Icons.SetActive(false);
        stepDisplay_Number.SetActive(false);
        stepDisplay_NumberIcons.SetActive(false);
    }


    //--------------------


    void ChangeStepText()
    {
        stepDisplay_Number_Text.text = PlayerStats.Instance.stats.steps_Current + "/" + PlayerStats.Instance.stats.steps_Max;
        stepDisplay_NumberIcons_Text.text = PlayerStats.Instance.stats.steps_Current + "/" + PlayerStats.Instance.stats.steps_Max;
    }
}
