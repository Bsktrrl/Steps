using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Water : MonoBehaviour
{
    private void Start()
    {
        UpdateFastSwimmingMovementCost();
    }


    public void UpdateFastSwimmingMovementCost()
    {
        if (MainManager.Instance.player.GetComponent<Player_Stats>().stats.abilities.Flippers)
        {
            gameObject.GetComponent<BlockInfo>().movementCost = gameObject.GetComponent<BlockInfo>().movementCost - 1;
        }
    }
}
