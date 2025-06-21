using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Water : MonoBehaviour
{
    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += UpdateMovementCostWithFlippers;
        Interactable_Pickup.Action_FlippersGot += UpdateMovementCostWithFlippers;
        Movement.Action_StepTaken += UpdateMovementCostWithFlippers;
    }

    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= UpdateMovementCostWithFlippers;
        Interactable_Pickup.Action_FlippersGot -= UpdateMovementCostWithFlippers;
        Movement.Action_StepTaken -= UpdateMovementCostWithFlippers;
    }


    //--------------------


    void UpdateMovementCostWithFlippers()
    {
        if (gameObject.GetComponent<BlockInfo>().movementCost == 0) { return; }

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
                            gameObject.GetComponent<BlockInfo>().movementCost = 0;
                            gameObject.GetComponent<BlockInfo>().movementCost_Temp = 0;
                            gameObject.GetComponent<BlockInfo>().movementSpeed = 4;
                        }
                    }
                }
            }
        }
    }
}
