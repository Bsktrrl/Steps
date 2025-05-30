using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_SpawnPoint : MonoBehaviour
{
    public static event Action Action_SpawnPointEntered;

    private void OnEnable()
    {
        Player_Movement.Action_StepTaken += UpdateSpawnPos;
    }

    private void OnDisable()
    {
        Player_Movement.Action_StepTaken -= UpdateSpawnPos;
    }


    //--------------------


    void UpdateSpawnPos()
    {
        if (PlayerManager.Instance.block_StandingOn_Current.block == gameObject)
        {
            MapManager.Instance.playerStartPos = PlayerManager.Instance.block_StandingOn_Current.block.transform.position + (Vector3.up * Player_Movement.Instance.heightOverBlock);

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
