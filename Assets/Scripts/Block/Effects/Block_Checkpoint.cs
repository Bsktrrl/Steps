using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Checkpoint : MonoBehaviour
{
    public static event Action Action_CheckPointEntered;

    public MovementDirection spawnDirection;


    //--------------------


    private void OnEnable()
    {
        Movement.Action_StepTaken += UpdateSpawnPos;
        //DataManager.Action_dataHasLoaded += UpdateSpawnPos;
        Movement.Action_LandedFromFalling += UpdateSpawnPos;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken -= UpdateSpawnPos;
        //DataManager.Action_dataHasLoaded -= UpdateSpawnPos;
        Movement.Action_LandedFromFalling -= UpdateSpawnPos;
    }


    //--------------------


    void UpdateSpawnPos()
    {
        if (Movement.Instance.blockStandingOn == gameObject)
        {
            MapManager.Instance.playerStartPos = Movement.Instance.blockStandingOn.transform.position + (Vector3.up * Movement.Instance.heightOverBlock);
            MapManager.Instance.playerStartRot = spawnDirection;

            EffectManager.Instance.PerformCheckpointEffect();

            PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Current = PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Max;
            StartCoroutine(ResetSteps(0.01f));
        }
    }

    IEnumerator ResetSteps(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Current = PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Max;

        Action_CheckPointEntered?.Invoke();
    }
}
