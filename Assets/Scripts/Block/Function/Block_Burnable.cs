using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Burnable : MonoBehaviour
{
    private void OnEnable()
    {
        Movement.Action_StepTaken += CheckForPlayer;
        PlayerStats.Action_RespawnPlayer += ResetBlock;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken -= CheckForPlayer;
        PlayerStats.Action_RespawnPlayer -= ResetBlock;
    }


    //--------------------


    void CheckForPlayer()
    {
        if (!Player_Burning.Instance.isBurning) { return; }


        if (Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position) <= 1.1f && Player_Burning.Instance.flameableStepCounter <= 5)
        {
            DestroyWeakBlock();
        }
    }

    public void DestroyWeakBlock()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }
    public void ResetBlock()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
    }
}
