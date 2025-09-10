using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_GetItem : MonoBehaviour 
{
    public List<ItemStats> itemReceivedList;



    //--------------------


    public void GetItems()
    {
        for (int i = 0; i < itemReceivedList.Count; i++)
        {
            switch (itemReceivedList[i].item)
            {
                case Items.None:
                    break;

                case Items.Essence:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.itemsGot.essence_Max += itemReceivedList[i].amount;
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.itemsGot.essence_Current += itemReceivedList[i].amount;
                    break;
                case Items.IncreaseMaxSteps:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.steps_Max += itemReceivedList[i].amount;
                    break;

                default:
                    break;
            }

        }
    }
}