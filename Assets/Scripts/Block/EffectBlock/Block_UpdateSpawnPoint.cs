using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_UpdateSpawnPoint : MonoBehaviour
{
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
        }
    }
}
