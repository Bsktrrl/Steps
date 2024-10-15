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
                    MainManager.Instance.player.GetComponent<Player_Stats>().stats.inventoryItems.coin += itemReceivedList[i].amount;
                    Player_Stats.Instance.UpdateCoins();
                    break;
                case Items.IncreaseMaxSteps:
                    MainManager.Instance.player.GetComponent<Player_Stats>().stats.steps_Max += itemReceivedList[i].amount;
                    Player_Stats.Instance.UpdateStepsMax();
                    break;

                default:
                    break;
            }

        }
    }
}

[Serializable]
public class ItemStats
{
    public Items item;
    public int amount;
}

public enum Items
{
    None,

    Coin,
    IncreaseMaxSteps
}