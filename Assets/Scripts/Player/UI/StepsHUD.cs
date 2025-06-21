using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepsHUD : Singleton<StepsHUD>
{
    [SerializeField] List<GameObject> stepsIconList = new List<GameObject>();

    float iconTransparencyValue = 0.4f;

    //--------------------


    private void OnEnable()
    {
        Interactable_Pickup.Action_PickupGot += UpdateStepsDisplay;
        Movement.Action_RespawnPlayerLate += UpdateStepsDisplay;
        Movement.Action_StepTaken += UpdateStepsDisplay;
        DataManager.Action_dataHasLoaded += UpdateStepsDisplay;
        Block_SpawnPoint.Action_SpawnPointEntered += UpdateStepsDisplay;
        Block_RefillSteps.Action_RefillStepsEntered += UpdateStepsDisplay;
        Block_MushroomCircle.Action_MushroomCircleEntered += UpdateStepsDisplay;
    }
    private void OnDisable()
    {
        Interactable_Pickup.Action_PickupGot -= UpdateStepsDisplay;
        Movement.Action_RespawnPlayerLate -= UpdateStepsDisplay;
        Movement.Action_StepTaken -= UpdateStepsDisplay;
        DataManager.Action_dataHasLoaded -= UpdateStepsDisplay;
        Block_SpawnPoint.Action_SpawnPointEntered -= UpdateStepsDisplay;
        Block_RefillSteps.Action_RefillStepsEntered -= UpdateStepsDisplay;
        Block_MushroomCircle.Action_MushroomCircleEntered -= UpdateStepsDisplay;
    }


    //--------------------


    public void UpdateStepsDisplay()
    {
        if (PlayerStats.Instance.stats.steps_Current < 0) { return; }

        //Make non-tranparency based on amount of steps left
        for (int i = 0; i < PlayerStats.Instance.stats.steps_Current; i++)
        {
            ChangeTransparency(i, false);
        }

        //Make tranparency based on amount of steps used
        for (int i = PlayerStats.Instance.stats.steps_Current; i < stepsIconList.Count; i++)
        {
            ChangeTransparency(i, true);
        }

        //Check which icons to be visible
        SetVisibility();
    }

    void SetVisibility()
    {
        for (int i = 0; i < 7; i++)
        {
            stepsIconList[i].SetActive(true);
        }

        for (int i = 7; i < stepsIconList.Count; i++)
        {
            stepsIconList[i].SetActive(false);
        }

        if (PlayerStats.Instance.stats.steps_Max == 8)
        {
            stepsIconList[7].SetActive(true);
        }
        else if (PlayerStats.Instance.stats.steps_Max == 9)
        {
            stepsIconList[7].SetActive(true);
            stepsIconList[8].SetActive(true);
        }
        else if (PlayerStats.Instance.stats.steps_Max == 10)
        {
            stepsIconList[7].SetActive(true);
            stepsIconList[8].SetActive(true);
            stepsIconList[9].SetActive(true);
        }
    }

    void ChangeTransparency(int index, bool isUsed)
    {
        if (index < stepsIconList.Count)
        {
            Color originalColor = stepsIconList[index].GetComponent<Image>().color;

            if (isUsed)
                stepsIconList[index].GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, iconTransparencyValue);
            else
                stepsIconList[index].GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
        }
    }
}
