using System.Collections;
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
        MapManager.Action_EndIntroSequence += SetStepsDisplay_FromSceneStart;
        SettingsManager.Action_SetNewStepDisplay += SetStepsDisplay_FromSettings;
    }

    private void OnDisable()
    {
        MapManager.Action_EndIntroSequence -= SetStepsDisplay_FromSceneStart;
        SettingsManager.Action_SetNewStepDisplay -= SetStepsDisplay_FromSettings;
    }


    //--------------------


    void SetStepsDisplay_FromSceneStart()
    {
        SetStepsDisplayBase();

        if (ShouldShowFootprints())
        {
            // Scene start should fill one-by-one, like respawn.
            StepsHUD.Instance.UpdateStepsDisplay_Respawn();
        }
    }

    void SetStepsDisplay_FromSettings()
    {
        SetStepsDisplayBase();

        if (ShouldShowFootprints())
        {
            // Settings change should snap to current state.
            // No counting, no one-by-one refill.
            StepsHUD.Instance.RefreshAllFootprintsImmediate(false);
        }
    }

    void HideAllMenus()
    {
        stepDisplay_Steps.SetActive(false);
        stepDisplay_Number.SetActive(false);
        stepDisplay_NumbersSteps.SetActive(false);

        footsteps_Parent.SetActive(false);
    }

    private void RefreshFootprintsNowAndNextFrame()
    {
        if (StepsHUD.Instance == null)
            return;

        StepsHUD.Instance.RefreshAllFootprintsImmediate(false);
        StartCoroutine(RefreshFootprintsNextFrame());
    }

    private IEnumerator RefreshFootprintsNextFrame()
    {
        yield return null;

        if (StepsHUD.Instance != null)
            StepsHUD.Instance.RefreshAllFootprintsImmediate(false);
    }

    #region Helpers

    void SetStepsDisplayBase()
    {
        HideAllMenus();

        switch (DataManager.Instance.settingData_StoreList.currentStepDisplay)
        {
            case StepDisplay.Steps:
                stepDisplay_Steps.SetActive(true);
                footsteps_Parent.SetActive(true);
                break;

            case StepDisplay.Number:
                stepDisplay_Number.SetActive(true);
                break;

            case StepDisplay.NumberSteps:
                stepDisplay_NumbersSteps.SetActive(true);
                footsteps_Parent.SetActive(true);
                break;

            case StepDisplay.None:
                HideAllMenus();
                break;
        }
    }

    bool ShouldShowFootprints()
    {
        return DataManager.Instance.settingData_StoreList.currentStepDisplay == StepDisplay.Steps ||
               DataManager.Instance.settingData_StoreList.currentStepDisplay == StepDisplay.NumberSteps;
    }

    #endregion
}