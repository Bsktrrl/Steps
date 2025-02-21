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

                case Items.Coin:
                    PlayerManager.Instance.player.GetComponent<PlayerStats>().stats.itemsGot.coin += itemReceivedList[i].amount;
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