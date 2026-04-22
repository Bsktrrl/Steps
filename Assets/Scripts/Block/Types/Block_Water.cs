using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Water : MonoBehaviour
{
    public bool hasRoots;

    float movementSpeed_Normal = 2.75f;
    float movementSpeed_Flippers = 6;


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += UpdateMovementCostWithSwimming;
        Interactable_Pickup.Action_SnorkelGot += UpdateMovementCostWithSwimming;
        Interactable_Pickup.Action_FlippersGot += UpdateMovementCostWithSwimming;
        Movement.Action_StepTaken += UpdateMovementCostWithSwimming;
    }

    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= UpdateMovementCostWithSwimming;
        Interactable_Pickup.Action_SnorkelGot -= UpdateMovementCostWithSwimming;
        Interactable_Pickup.Action_FlippersGot -= UpdateMovementCostWithSwimming;
        Movement.Action_StepTaken -= UpdateMovementCostWithSwimming;
    }


    //--------------------


    void UpdateMovementCostWithSwimming()
    {
        //if (gameObject.GetComponent<BlockInfo>().movementCost == 0) { return; }

        if (GetComponent<Block_Checkpoint>()) return;

        if (hasRoots)
        {
            if (gameObject && gameObject.GetComponent<BlockInfo>()
                && (PlayerStats.Instance.stats.abilitiesGot_Temporary.Flippers || PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers))
            {
                gameObject.GetComponent<BlockInfo>().movementCost = 0;
                gameObject.GetComponent<BlockInfo>().movementCost_Temp = 0;
                gameObject.GetComponent<BlockInfo>().movementSpeed = movementSpeed_Flippers;
            }
            else if (gameObject && gameObject.GetComponent<BlockInfo>())
            {
                gameObject.GetComponent<BlockInfo>().movementCost = 0;
                gameObject.GetComponent<BlockInfo>().movementCost_Temp = 0;
                gameObject.GetComponent<BlockInfo>().movementSpeed = movementSpeed_Normal;
            }
        }
        else
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
                                gameObject.GetComponent<BlockInfo>().movementCost = 0;
                                gameObject.GetComponent<BlockInfo>().movementCost_Temp = 0;
                                gameObject.GetComponent<BlockInfo>().movementSpeed = movementSpeed_Flippers;
                            }
                        }
                        else
                        {
                            if (gameObject.GetComponent<BlockInfo>())
                            {
                                gameObject.GetComponent<BlockInfo>().movementCost = 2;
                                gameObject.GetComponent<BlockInfo>().movementCost_Temp = 2;
                                gameObject.GetComponent<BlockInfo>().movementSpeed = movementSpeed_Normal;
                            }
                        }
                    }
                }
            }
        }
    }
}
