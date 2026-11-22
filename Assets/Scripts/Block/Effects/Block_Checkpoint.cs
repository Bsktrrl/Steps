using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Block_Checkpoint : MonoBehaviour
{
    public static event Action Action_CheckPointEntered;

    public MovementDirection spawnDirection;


    //--------------------


    private void OnEnable()
    {
        Movement.Action_StepTaken += UpdateSpawnPos;
        Movement.Action_LandedFromFalling += UpdateSpawnPos;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken -= UpdateSpawnPos;
        Movement.Action_LandedFromFalling -= UpdateSpawnPos;
    }


    //--------------------


    void UpdateSpawnPos()
    {
        if (Movement.Instance.blockStandingOn == gameObject)
        {
            StepsHUD.Instance.stepCounter = PlayerStats.Instance.stats.steps_Current;

            MapManager.Instance.playerStartPos = Movement.Instance.blockStandingOn.transform.position + (Vector3.up * Movement.Instance.heightOverBlock);
            MapManager.Instance.playerStartRot = spawnDirection;

            EffectManager.Instance.PerformCheckpointEffect();

            PlayerStats.Instance.RestoreMissingSteps();
            StartCoroutine(ResetSteps(0.01f));
        }
    }

    IEnumerator ResetSteps(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        PlayerStats.Instance.RestoreMissingSteps();

        Action_CheckPointEntered?.Invoke();
    }
}
