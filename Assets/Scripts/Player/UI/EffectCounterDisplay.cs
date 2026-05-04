using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EffectCounterDisplay : MonoBehaviour
{
    [SerializeField] GameObject ui_Parent_FlameCounter;
    [SerializeField] GameObject ui_Parent_MushroomCircleCounter;
    [SerializeField] GameObject ui_Parent_ExtraCounter;


    //--------------------


    private void Start()
    {
        EndFlameCounter();
        EndMushroomCircleCounter();
    }


    //--------------------


    private void OnEnable()
    {
        Player_Burning.Action_PlayerStartedBurning += StartFlameCounter;
        Player_Burning.Action_PlayerEndBurning += EndFlameCounter;
        Player_Burning.Action_UpdateFlamCounter += ChangeFlameCounter;

        Player_MushroomCircle.Action_StartMushroomCircle += StartMushroomCircleCounter;
        Player_MushroomCircle.Action_EndMushroomCircle += EndMushroomCircleCounter;
        Player_MushroomCircle.Action_UpdateMushroomCircle += ChangeMushroomCircleCounter;
    }
    private void OnDisable()
    {
        Player_Burning.Action_PlayerStartedBurning -= StartFlameCounter;
        Player_Burning.Action_PlayerEndBurning -= EndFlameCounter;
        Player_Burning.Action_UpdateFlamCounter -= ChangeFlameCounter;

        Player_MushroomCircle.Action_StartMushroomCircle -= StartMushroomCircleCounter;
        Player_MushroomCircle.Action_EndMushroomCircle -= EndMushroomCircleCounter;
        Player_MushroomCircle.Action_UpdateMushroomCircle -= ChangeMushroomCircleCounter;
    }


    //--------------------


    void StartFlameCounter()
    {
        ChangeFlameCounter();
        ui_Parent_FlameCounter.SetActive(true);
    }
    void EndFlameCounter()
    {
        ui_Parent_FlameCounter.SetActive(false);
    }
    void ChangeFlameCounter()
    {
        int counterTemp = 0;

        switch (Player_Burning.Instance.flameableStepCounter)
        {
            case 0:
                counterTemp = 5;
                break;
            case 1:
                counterTemp = 4;
                break;
            case 2:
                counterTemp = 3;
                break;
            case 3:
                counterTemp = 2;
                break;
            case 4:
                counterTemp = 1;
                break;
            case 5:
                counterTemp = 0;
                break;

            default:
                counterTemp = 100;
                break;
        }

        ChangeCounter(ui_Parent_FlameCounter.GetComponentInChildren<TextMeshProUGUI>(), counterTemp);
    }


    //-----


    void StartMushroomCircleCounter()
    {
        ChangeMushroomCircleCounter();
        ui_Parent_MushroomCircleCounter.SetActive(true);
    }
    void EndMushroomCircleCounter()
    {
        ui_Parent_MushroomCircleCounter.SetActive(false);
    }
    void ChangeMushroomCircleCounter()
    {
        ChangeCounter(ui_Parent_MushroomCircleCounter.GetComponentInChildren<TextMeshProUGUI>(), Player_MushroomCircle.Instance.freeStepsCounter);
    }


    //-----


    void ChangeCounter(TextMeshProUGUI _text, int counter)
    {
        _text.text = counter.ToString();
        print("ChangeCounter to: " + counter);
    }
}
