using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Water : MonoBehaviour
{
    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += UpdateMovementCostWithOxygenTank;
        Interactable_Pickup.Action_OxygenTankGot += UpdateMovementCostWithOxygenTank;
        Movement.Action_StepTaken += UpdateMovementCostWithOxygenTank;
    }

    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= UpdateMovementCostWithOxygenTank;
        Interactable_Pickup.Action_OxygenTankGot -= UpdateMovementCostWithOxygenTank;
        Movement.Action_StepTaken -= UpdateMovementCostWithOxygenTank;
    }


    //--------------------


    void UpdateMovementCostWithOxygenTank()
    {
        if (gameObject.GetComponent<BlockInfo>().movementCost == 0) { return; }

        if (PlayerManager.Instance.player.GetComponent<PlayerStats>())
        {
            if (PlayerManager.Instance.player.GetComponent<PlayerStats>().stats != null)
            {
                if (PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Permanent != null || PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary != null)
                {
                    if (PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Permanent.OxygenTank == true || PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.OxygenTank == true)
                    {
                        if (gameObject.GetComponent<BlockInfo>())
                        {
                            gameObject.GetComponent<BlockInfo>().movementCost = 0;
                            gameObject.GetComponent<BlockInfo>().movementCost_Temp = 0;
                            gameObject.GetComponent<BlockInfo>().movementSpeed = 4;
                        }
                    }
                    else
                    {
                        if (gameObject.GetComponent<BlockInfo>())
                        {
                            gameObject.GetComponent<BlockInfo>().movementCost = 2;
                            gameObject.GetComponent<BlockInfo>().movementCost_Temp = 2;
                            gameObject.GetComponent<BlockInfo>().movementSpeed = 2;
                        }
                    }
                }
            }
        }
    }
}
