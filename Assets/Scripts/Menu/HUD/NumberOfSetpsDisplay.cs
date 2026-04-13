using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NumberOfSetpsDisplay : MonoBehaviour
{
    public static event Action Action_Run_FirstTime;

    [SerializeField] Image number_Current;
    [SerializeField] Image number_Max;

    [SerializeField] float numberSpawnTime = 0.15f;
    [SerializeField] float extraTimeDelay = 0.2f;

    bool firstTimeRun_Check;
    bool firstTime_Check;

    private Coroutine updateFootprintCoroutine;

    private bool hasInitializedThisScene;


    //--------------------


    private void OnEnable()
    {
        UpdateNumberDisplaySpeed();

        Action_Run_FirstTime += Update_FirstTime;

        Interactable_Pickup.Action_PickupGot += UpdateNumbersDisplay;
        Interactable_Pickup.Action_StepsUpPickupGot += UpdateNumbersDisplay;
        Movement.Action_StepTaken += UpdateNumbersDisplay;
        Block_MushroomCircle.Action_MushroomCircleEntered += UpdateNumbersDisplay;

        Movement.Action_RespawnPlayerLate += UpdateNumberDisplay_Respawn;
        Block_Checkpoint.Action_CheckPointEntered += UpdateNumberDisplay_Checkpoint;

        SettingsManager.Action_SetNewStepDisplay += HandleStepDisplayChanged;
        SettingsManager.Action_SetNewStepDisplay += UpdateNumberDisplaySpeed;

        // If this number display is being enabled because of a settings change,
        // it should snap to the real current value immediately.
        SetNumbersInstant(PlayerStats.Instance.stats.steps_Current, PlayerStats.Instance.stats.steps_Max);

        // Only run the startup animation once per scene, not every time the object is re-enabled.
        if (!hasInitializedThisScene)
        {
            hasInitializedThisScene = true;

            if (!firstTimeRun_Check)
            {
                FirstTimeUpdate();
                Action_Run_FirstTime?.Invoke();
            }
        }
    }

    private void OnDisable()
    {
        Action_Run_FirstTime -= Update_FirstTime;

        Interactable_Pickup.Action_PickupGot -= UpdateNumbersDisplay;
        Interactable_Pickup.Action_StepsUpPickupGot -= UpdateNumbersDisplay;
        Movement.Action_StepTaken -= UpdateNumbersDisplay;
        Block_MushroomCircle.Action_MushroomCircleEntered -= UpdateNumbersDisplay;

        Movement.Action_RespawnPlayerLate -= UpdateNumberDisplay_Respawn;
        Block_Checkpoint.Action_CheckPointEntered -= UpdateNumberDisplay_Checkpoint;

        SettingsManager.Action_SetNewStepDisplay -= HandleStepDisplayChanged;
        SettingsManager.Action_SetNewStepDisplay -= UpdateNumberDisplaySpeed;

        StopRunningNumberCoroutine();
    }


    //--------------------


    void Update_FirstTime()
    {
        firstTimeRun_Check = true;
    }


    //--------------------


    void FirstTimeUpdate()
    {
        StopRunningNumberCoroutine();
        updateFootprintCoroutine = StartCoroutine(UpdateFootprintDelay_FirstTime(StepsHUD.Instance.StepsDisplay_CheckpointTime, StepsHUD.Instance.footprint_SpawnTime));
    }

    IEnumerator UpdateFootprintDelay_FirstTime(float startDelay, float waitTime)
    {
        yield return null;

        SetNumbersInstant(0, PlayerStats.Instance.stats.steps_Max);

        yield return new WaitForSeconds(startDelay /*+ extraTimeDelay*/);

        int max = PlayerStats.Instance.stats.steps_Max;

        for (int value = 1; value <= max; value++)
        {
            SetCurrentNumber(value);
            yield return new WaitForSeconds(waitTime);
        }

        SetNumbersInstant(PlayerStats.Instance.stats.steps_Current, PlayerStats.Instance.stats.steps_Max);
        updateFootprintCoroutine = null;
    }


    //--------------------


    void UpdateNumberDisplaySpeed()
    {
        if (DataManager.Instance.settingData_StoreList.currentStepDisplay == StepDisplay.Number)
            numberSpawnTime = 0.15f;
        else
            numberSpawnTime = 0.1f;
    }

    void HandleStepDisplayChanged()
    {
        StopRunningNumberCoroutine();
        SetNumbersInstant(PlayerStats.Instance.stats.steps_Current, PlayerStats.Instance.stats.steps_Max);
    }


    //--------------------


    void UpdateNumbersDisplay()
    {
        if (PlayerStats.Instance == null || PlayerStats.Instance.stats == null || StepsDisplay.Instance == null)
            return;

        // Do not let normal walking updates override the delayed checkpoint update.
        if (IsStandingOnCheckpoint() && firstTime_Check)
            return;

        StopRunningNumberCoroutine();

        firstTime_Check = true;

        SetNumbersInstant(PlayerStats.Instance.stats.steps_Current, PlayerStats.Instance.stats.steps_Max);
    }


    //--------------------


    int GetCurrentNumberDisplay()
    {
        if (number_Current.sprite == StepsDisplay.Instance.number_0) return 0;
        if (number_Current.sprite == StepsDisplay.Instance.number_1) return 1;
        if (number_Current.sprite == StepsDisplay.Instance.number_2) return 2;
        if (number_Current.sprite == StepsDisplay.Instance.number_3) return 3;
        if (number_Current.sprite == StepsDisplay.Instance.number_4) return 4;
        if (number_Current.sprite == StepsDisplay.Instance.number_5) return 5;
        if (number_Current.sprite == StepsDisplay.Instance.number_6) return 6;
        if (number_Current.sprite == StepsDisplay.Instance.number_7) return 7;
        if (number_Current.sprite == StepsDisplay.Instance.number_8) return 8;
        if (number_Current.sprite == StepsDisplay.Instance.number_9) return 9;
        if (number_Current.sprite == StepsDisplay.Instance.number_10) return 10;

        return 0;
    }


    //--------------------


    public void UpdateNumberDisplay_Respawn()
    {
        StopRunningNumberCoroutine();
        updateFootprintCoroutine = StartCoroutine(UpdateNumberDelay_Respawn(StepsHUD.Instance.StepsDisplay_RespawnTime, numberSpawnTime));
    }

    public void UpdateNumberDisplay_Checkpoint()
    {
        StopRunningNumberCoroutine();
        updateFootprintCoroutine = StartCoroutine(UpdateNumberDelay_Checkpoint(StepsHUD.Instance.StepsDisplay_CheckpointTime));
    }

    IEnumerator UpdateNumberDelay_Respawn(float startDelay, float waitTime)
    {
        yield return new WaitForSeconds(startDelay /*+ extraTimeDelay*/);

        int startValue = Mathf.Clamp(StepsHUD.Instance.stepCounter, 0, PlayerStats.Instance.stats.steps_Max);
        int maxValue = PlayerStats.Instance.stats.steps_Max;

        SetNumbersInstant(startValue, maxValue);

        for (int value = startValue + 1; value <= maxValue; value++)
        {
            SetCurrentNumber(value);
            yield return new WaitForSeconds(waitTime);
        }

        SetNumbersInstant(PlayerStats.Instance.stats.steps_Current, PlayerStats.Instance.stats.steps_Max);
        updateFootprintCoroutine = null;
    }

    IEnumerator UpdateNumberDelay_Checkpoint(float startDelay)
    {
        yield return new WaitForSeconds(startDelay /*+ extraTimeDelay*/);

        // Checkpoint should jump directly to max, not count upward.
        SetNumbersInstant(PlayerStats.Instance.stats.steps_Max, PlayerStats.Instance.stats.steps_Max);

        updateFootprintCoroutine = null;
    }


    //--------------------


    private void StopRunningNumberCoroutine()
    {
        if (updateFootprintCoroutine != null)
        {
            StopCoroutine(updateFootprintCoroutine);
            updateFootprintCoroutine = null;
        }
    }

    private bool IsStandingOnCheckpoint()
    {
        return Movement.Instance != null &&
               Movement.Instance.blockStandingOn != null &&
               Movement.Instance.blockStandingOn.GetComponent<Block_Checkpoint>() != null;
    }

    private void SetNumbersInstant(int current, int max)
    {
        SetCurrentNumber(current);
        SetMaxNumber(max);
    }

    private void SetCurrentNumber(int value)
    {
        number_Current.sprite = GetNumberSprite(Mathf.Clamp(value, 0, 10));
    }

    private void SetMaxNumber(int value)
    {
        number_Max.sprite = GetNumberSprite(Mathf.Clamp(value, 0, 10));
    }

    private Sprite GetNumberSprite(int value)
    {
        switch (value)
        {
            case 0: return StepsDisplay.Instance.number_0;
            case 1: return StepsDisplay.Instance.number_1;
            case 2: return StepsDisplay.Instance.number_2;
            case 3: return StepsDisplay.Instance.number_3;
            case 4: return StepsDisplay.Instance.number_4;
            case 5: return StepsDisplay.Instance.number_5;
            case 6: return StepsDisplay.Instance.number_6;
            case 7: return StepsDisplay.Instance.number_7;
            case 8: return StepsDisplay.Instance.number_8;
            case 9: return StepsDisplay.Instance.number_9;
            case 10: return StepsDisplay.Instance.number_10;
            default: return StepsDisplay.Instance.number_0;
        }
    }
}