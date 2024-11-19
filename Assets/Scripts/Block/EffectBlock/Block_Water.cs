using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Water : MonoBehaviour
{
    private void OnEnable()
    {
        SaveLoad_PlayerStats.playerStats_hasLoaded += UpdateMovementCostWithFlippers;
    }

    private void OnDisable()
    {
        SaveLoad_PlayerStats.playerStats_hasLoaded -= UpdateMovementCostWithFlippers;
    }


    //--------------------


    void UpdateMovementCostWithFlippers()
    {
        if (PlayerManager.Instance.player.GetComponent<PlayerStats>())
        {
            if (PlayerManager.Instance.player.GetComponent<PlayerStats>().stats != null)
            {
                if (PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot != null)
                {
                    if (PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.Flippers == true)
                    {
                        if (gameObject.GetComponent<BlockInfo>())
                        {
                            gameObject.GetComponent<BlockInfo>().movementCost -= 1;
                        }
                    }
                }
            }
        }
    }


    //--------------------


    public void UpdateFastSwimmingMovementCost()
    {
        if (PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot.Flippers)
        {
            gameObject.GetComponent<BlockInfo>().movementCost = gameObject.GetComponent<BlockInfo>().movementCost - 1;
        }
    }
}
