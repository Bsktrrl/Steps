using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Water : MonoBehaviour
{
    private void OnEnable()
    {
        DataManager.datahasLoaded += UpdateMovementCostWithFlippers;
    }

    private void OnDisable()
    {
        DataManager.datahasLoaded -= UpdateMovementCostWithFlippers;
    }


    //--------------------


    void UpdateMovementCostWithFlippers()
    {
        if (PlayerManager.Instance.player.GetComponent<PlayerStats>())
        {
            if (PlayerManager.Instance.player.GetComponent<PlayerStats>().stats != null)
            {
                if (PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Permanent != null || PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary != null)
                {
                    if (PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Permanent.Flippers == true || PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Flippers == true)
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
        if (PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Permanent.Flippers || PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Flippers)
        {
            gameObject.GetComponent<BlockInfo>().movementCost = gameObject.GetComponent<BlockInfo>().movementCost - 1;
        }
    }
}
