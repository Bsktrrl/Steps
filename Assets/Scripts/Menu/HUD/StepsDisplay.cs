using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StepsDisplay : MonoBehaviour
{
    [Header("Parents")]
    [SerializeField] GameObject stepDisplay_Icons;
    [SerializeField] GameObject stepDisplay_Number;
    [SerializeField] GameObject stepDisplay_NumberIcons;

    [Header("TextObjects")]
    [SerializeField] TextMeshProUGUI stepDisplay_Number_Text;
    [SerializeField] TextMeshProUGUI stepDisplay_NumberIcons_Text;


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += SetStepsDisplay;
        SettingsManager.Action_SetNewStepDisplay += SetStepsDisplay;

        Interactable_Pickup.Action_PickupGot += ChangeStepText;
        Movement.Action_RespawnPlayerLate += ChangeStepText;
        Movement.Action_StepTaken += ChangeStepText;
        DataManager.Action_dataHasLoaded += ChangeStepText;
        Block_SpawnPoint.Action_SpawnPointEntered += ChangeStepText;
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
        Block_SpawnPoint.Action_SpawnPointEntered -= ChangeStepText;
        Block_RefillSteps.Action_RefillStepsEntered -= ChangeStepText;
        Block_MushroomCircle.Action_MushroomCircleEntered -= ChangeStepText;
    }


    //--------------------


    void SetStepsDisplay()
    {
        HideAllMenus();

        print("20. CurrentStepsDisplay: " + DataManager.Instance.settingData_StoreList.currentStepDisplay.ToString());

        switch (DataManager.Instance.settingData_StoreList.currentStepDisplay)
        {
            case StepDisplay.Icon:
                stepDisplay_Icons.SetActive(true);
                StepsHUD.Instance.UpdateStepsDisplay();
                break;
            case StepDisplay.Number:
                stepDisplay_Number.SetActive(true);
                break;
            case StepDisplay.NumberIcon:
                stepDisplay_NumberIcons.SetActive(true);
                stepDisplay_Icons.SetActive(true);
                StepsHUD.Instance.UpdateStepsDisplay();
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
