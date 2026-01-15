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


    //--------------------


    private void OnEnable()
    {
        UpdateNumbersDisplay();

        if (!firstTimeRun_Check)
            FirstTimeUpdate();

        Action_Run_FirstTime += Update_FirstTime;

        Interactable_Pickup.Action_PickupGot += UpdateNumbersDisplay;
        Interactable_Pickup.Action_StepsUpPickupGot += UpdateNumbersDisplay;
        Movement.Action_StepTaken += UpdateNumbersDisplay;
        Block_MushroomCircle.Action_MushroomCircleEntered += UpdateNumbersDisplay;

        Movement.Action_RespawnPlayerLate += UpdateNumberDisplay_Respawn;
        Block_Checkpoint.Action_CheckPointEntered += UpdateNumberDisplay_Checkpoint;

        SettingsManager.Action_SetNewStepDisplay += UpdateNumberDisplaySpeed;

        Action_Run_FirstTime?.Invoke();
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

        SettingsManager.Action_SetNewStepDisplay -= UpdateNumberDisplaySpeed;
    }


    //--------------------


    void Update_FirstTime()
    {
        firstTimeRun_Check = true;
    }


    //--------------------


    void FirstTimeUpdate()
    {
        StartCoroutine(UpdateFootprintDelay_FirstTime(StepsHUD.Instance.StepsDisplay_CheckpointTime, StepsHUD.Instance.footprint_SpawnTime));
    }
    IEnumerator UpdateFootprintDelay_FirstTime(float startDelay, float waitTime)
    {
        yield return null;

        number_Current.sprite = StepsDisplay.Instance.number_0;

        yield return new WaitForSeconds(startDelay + extraTimeDelay);

        print("0.5. UpdateFootprintDelay | StepCounter: " + StepsHUD.Instance.stepCounter + " | PlayerStats.Instance.stats.steps_Max: " + (PlayerStats.Instance.stats.steps_Max));

        for (int i = 0; i < PlayerStats.Instance.stats.steps_Max; i++)
        {
            switch (GetCurrentNumberDisplay())
            {
                case 0:
                    number_Current.sprite = StepsDisplay.Instance.number_1;
                    break;
                case 1:
                    number_Current.sprite = StepsDisplay.Instance.number_2;
                    break;
                case 2:
                    number_Current.sprite = StepsDisplay.Instance.number_3;
                    break;
                case 3:
                    number_Current.sprite = StepsDisplay.Instance.number_4;
                    break;
                case 4:
                    number_Current.sprite = StepsDisplay.Instance.number_5;
                    break;
                case 5:
                    number_Current.sprite = StepsDisplay.Instance.number_6;
                    break;
                case 6:
                    number_Current.sprite = StepsDisplay.Instance.number_7;
                    break;
                case 7:
                    number_Current.sprite = StepsDisplay.Instance.number_8;
                    break;
                case 8:
                    number_Current.sprite = StepsDisplay.Instance.number_9;
                    break;
                case 9:
                    number_Current.sprite = StepsDisplay.Instance.number_10;
                    break;

                default:
                    break;
            }

            yield return new WaitForSeconds(waitTime);
        }

        UpdateNumbersDisplay();
    }


    //--------------------


    void UpdateNumberDisplaySpeed()
    {
        if (DataManager.Instance.settingData_StoreList.currentStepDisplay == StepDisplay.Number)
            numberSpawnTime = 0.15f;
        else
            numberSpawnTime = 0.1f;
    }


    //--------------------


    void UpdateNumbersDisplay()
    {
        if ((Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<Block_Checkpoint>()) && firstTime_Check) return;

        if (updateFootprintCoroutine != null)
        {
            StopCoroutine(updateFootprintCoroutine);
            updateFootprintCoroutine = null;
        }

        firstTime_Check = true;

        //Current
        switch (PlayerStats.Instance.stats.steps_Current)
        {
            case 0:
                number_Current.sprite = StepsDisplay.Instance.number_0;
                break;
            case 1:
                number_Current.sprite = StepsDisplay.Instance.number_1;
                break;
            case 2:
                number_Current.sprite = StepsDisplay.Instance.number_2;
                break;
            case 3:
                number_Current.sprite = StepsDisplay.Instance.number_3;
                break;
            case 4:
                number_Current.sprite = StepsDisplay.Instance.number_4;
                break;
            case 5:
                number_Current.sprite = StepsDisplay.Instance.number_5;
                break;
            case 6:
                number_Current.sprite = StepsDisplay.Instance.number_6;
                break;
            case 7:
                number_Current.sprite = StepsDisplay.Instance.number_7;
                break;
            case 8:
                number_Current.sprite = StepsDisplay.Instance.number_8;
                break;
            case 9:
                number_Current.sprite = StepsDisplay.Instance.number_9;
                break;
            case 10:
                number_Current.sprite = StepsDisplay.Instance.number_10;
                break;

            default:
                break;
        }

        //Max
        switch (PlayerStats.Instance.stats.steps_Max)
        {
            case 0:
                number_Max.sprite = StepsDisplay.Instance.number_0;
                break;
            case 1:
                number_Max.sprite = StepsDisplay.Instance.number_1;
                break;
            case 2:
                number_Max.sprite = StepsDisplay.Instance.number_2;
                break;
            case 3:
                number_Max.sprite = StepsDisplay.Instance.number_3;
                break;
            case 4:
                number_Max.sprite = StepsDisplay.Instance.number_4;
                break;
            case 5:
                number_Max.sprite = StepsDisplay.Instance.number_5;
                break;
            case 6:
                number_Max.sprite = StepsDisplay.Instance.number_6;
                break;
            case 7:
                number_Max.sprite = StepsDisplay.Instance.number_7;
                break;
            case 8:
                number_Max.sprite = StepsDisplay.Instance.number_8;
                break;
            case 9:
                number_Max.sprite = StepsDisplay.Instance.number_9;
                break;
            case 10:
                number_Max.sprite = StepsDisplay.Instance.number_10;
                break;

            default:
                break;
        }
    }


    //--------------------


    int GetCurrentNumberDisplay()
    {
        if (number_Current.sprite == StepsDisplay.Instance.number_0)
            return 0;
        else if (number_Current.sprite == StepsDisplay.Instance.number_1)
            return 1;
        else if (number_Current.sprite == StepsDisplay.Instance.number_2)
            return 2;
        else if (number_Current.sprite == StepsDisplay.Instance.number_3)
            return 3;
        else if (number_Current.sprite == StepsDisplay.Instance.number_4)
            return 4;
        else if (number_Current.sprite == StepsDisplay.Instance.number_5)
            return 5;
        else if (number_Current.sprite == StepsDisplay.Instance.number_6)
            return 6;
        else if (number_Current.sprite == StepsDisplay.Instance.number_7)
            return 7;
        else if (number_Current.sprite == StepsDisplay.Instance.number_8)
            return 8;
        else if (number_Current.sprite == StepsDisplay.Instance.number_9)
            return 9;
        else if (number_Current.sprite == StepsDisplay.Instance.number_10)
            return 10;
        else
            return 0;
    }


    //--------------------


    public void UpdateNumberDisplay_Respawn()
    {
        print("1. UpdateNumberDisplay_Checkpoint | StepCounter: " + StepsHUD.Instance.stepCounter + " | PlayerStats.Instance.stats.steps_Max: " + (PlayerStats.Instance.stats.steps_Max));

        updateFootprintCoroutine = StartCoroutine(UpdateFootprintDelay(StepsHUD.Instance.StepsDisplay_RespawnTime, numberSpawnTime));
        //StartCoroutine(UpdateFootprintDelay(StepsHUD.Instance.StepsDisplay_RespawnTime, numberSpawnTime));
    }
    public void UpdateNumberDisplay_Checkpoint()
    {
        print("2. UpdateNumberDisplay_Checkpoint | StepCounter: " + StepsHUD.Instance.stepCounter + " | PlayerStats.Instance.stats.steps_Max: " + (PlayerStats.Instance.stats.steps_Max));
        updateFootprintCoroutine = StartCoroutine(UpdateFootprintDelay(StepsHUD.Instance.StepsDisplay_CheckpointTime, numberSpawnTime));
        //StartCoroutine(UpdateFootprintDelay(StepsHUD.Instance.StepsDisplay_CheckpointTime, numberSpawnTime));
    }

    IEnumerator UpdateFootprintDelay(float startDelay, float waitTime)
    {
        yield return new WaitForSeconds(startDelay + extraTimeDelay);

        print("2.5. UpdateFootprintDelay | StepCounter: " + StepsHUD.Instance.stepCounter + " | PlayerStats.Instance.stats.steps_Max: " + (PlayerStats.Instance.stats.steps_Max));

        for (int i = StepsHUD.Instance.stepCounter; i < PlayerStats.Instance.stats.steps_Max; i++)
        {
            switch (GetCurrentNumberDisplay())
            {
                case 0:
                    number_Current.sprite = StepsDisplay.Instance.number_1;
                    break;
                case 1:
                    number_Current.sprite = StepsDisplay.Instance.number_2;
                    break;
                case 2:
                    number_Current.sprite = StepsDisplay.Instance.number_3;
                    break;
                case 3:
                    number_Current.sprite = StepsDisplay.Instance.number_4;
                    break;
                case 4:
                    number_Current.sprite = StepsDisplay.Instance.number_5;
                    break;
                case 5:
                    number_Current.sprite = StepsDisplay.Instance.number_6;
                    break;
                case 6:
                    number_Current.sprite = StepsDisplay.Instance.number_7;
                    break;
                case 7:
                    number_Current.sprite = StepsDisplay.Instance.number_8;
                    break;
                case 8:
                    number_Current.sprite = StepsDisplay.Instance.number_9;
                    break;
                case 9:
                    number_Current.sprite = StepsDisplay.Instance.number_10;
                    break;

                default:
                    break;
            }

            yield return new WaitForSeconds(waitTime);

            print("4. UpdateFootprintDelay | i = " + i);
        }

        UpdateNumbersDisplay();
    }
}
