using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_SpawnPoint : MonoBehaviour
{
    public static event Action Action_SpawnPointEntered;

    private void OnEnable()
    {
        Movement.Action_StepTaken += UpdateSpawnPos;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken -= UpdateSpawnPos;
    }


    //--------------------


    void UpdateSpawnPos()
    {
        if (Movement.Instance.blockStandingOn == gameObject)
        {
            MapManager.Instance.playerStartPos = Movement.Instance.blockStandingOn.transform.position + (Vector3.up * Movement.Instance.heightOverBlock);

            PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Current = PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Max;
            StartCoroutine(ResetSteps(0.01f));
        }
        IEnumerator ResetSteps(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Current = PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Max;

            Action_SpawnPointEntered?.Invoke();
        }
    }
}
