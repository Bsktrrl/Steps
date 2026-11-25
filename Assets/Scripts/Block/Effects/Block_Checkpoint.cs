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
            ActivateCheckpoint(playLandingEffect: true);
            StartCoroutine(ResetSteps(0.01f));
        }
    }

    internal void ActivateDuringGrapple()
    {
        ActivateCheckpoint(playLandingEffect: false);
        // Subsequent blocks can spend steps immediately, so refill only once.
        Action_CheckPointEntered?.Invoke();
    }

    private void ActivateCheckpoint(bool playLandingEffect)
    {
        StepsHUD.Instance.stepCounter = PlayerStats.Instance.stats.steps_Current;
        MapManager.Instance.playerStartPos = transform.position + Vector3.up * Movement.Instance.heightOverBlock;
        MapManager.Instance.playerStartRot = spawnDirection;

        if (playLandingEffect)
            EffectManager.Instance.PerformCheckpointEffect();

        PlayerStats.Instance.RestoreMissingSteps();
    }

    IEnumerator ResetSteps(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        PlayerStats.Instance.RestoreMissingSteps();

        Action_CheckPointEntered?.Invoke();
    }
}
