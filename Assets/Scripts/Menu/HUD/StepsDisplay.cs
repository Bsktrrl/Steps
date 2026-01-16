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

    [Header("Footsteps Sprites")]
    public Sprite normalFootstep_Active;
    public Sprite normalFootstep_Used;

    public Sprite extraFootstep_Passive;
    public Sprite extraFootstep_Active;
    public Sprite extraFootstep_Used;

    [Header("Numbers Sprites")]
    public Sprite number_0;
    public Sprite number_1;
    public Sprite number_2;
    public Sprite number_3;
    public Sprite number_4;
    public Sprite number_5;
    public Sprite number_6;
    public Sprite number_7;
    public Sprite number_8;
    public Sprite number_9;
    public Sprite number_10;


    //--------------------


    private void OnEnable()
    {
        if (MapManager.Instance.haveIntroSequence)
            MapManager.Action_EndIntroSequence += SetStepsDisplay;
        else
            DataManager.Action_dataHasLoaded += SetStepsDisplay;

            SettingsManager.Action_SetNewStepDisplay += SetStepsDisplay;
    }
    private void OnDisable()
    {
        if (MapManager.Instance.haveIntroSequence)
            MapManager.Action_EndIntroSequence -= SetStepsDisplay;
        else
            DataManager.Action_dataHasLoaded -= SetStepsDisplay;

        SettingsManager.Action_SetNewStepDisplay -= SetStepsDisplay;
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

        if (MapManager.Instance.haveIntroSequence)
        {
            //Movement.Instance.RespawnPlayer();
            //PlayerStats.Instance.RefillStepsToMax();
            StepsHUD.Instance.UpdateStepsDisplay_Respawn();
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
