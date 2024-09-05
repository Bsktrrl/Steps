
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public static event Action pickup_Coin_IsHit;
    public static event Action pickup_Step_IsHit;

    public static event Action pickup_KeyItem_SwimSuit_IsHit;
    public static event Action pickup_KeyItem_Flippers_IsHit;
    public static event Action pickup_KeyItem_HikerGear_IsHit;
    public static event Action pickup_KeyItem_LavaSuit_IsHit;

    public PickupItems item;
    public PickupKeyItems keyItem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            switch (item)
            {
                case PickupItems.None:
                    break;

                case PickupItems.Coin:
                    pickup_Coin_IsHit?.Invoke();
                    DestroyThisObject();
                    break;
                case PickupItems.Steps:
                    pickup_Step_IsHit?.Invoke();
                    DestroyThisObject();
                    break;

                default:
                    break;
            }

            switch (keyItem)
            {
                case PickupKeyItems.None:
                    break;

                case PickupKeyItems.SwimSuit:
                    pickup_KeyItem_SwimSuit_IsHit?.Invoke();
                    DestroyThisObject();
                    break;
                case PickupKeyItems.Flippers:
                    pickup_KeyItem_Flippers_IsHit?.Invoke();
                    DestroyThisObject();
                    break;
                case PickupKeyItems.HikerGear:
                    pickup_KeyItem_HikerGear_IsHit?.Invoke();
                    DestroyThisObject();
                    break;
                case PickupKeyItems.LavaSuit:
                    pickup_KeyItem_LavaSuit_IsHit?.Invoke();
                    DestroyThisObject();
                    break;

                default:
                    break;
            }
        }
    }

    void DestroyThisObject()
    {
        Destroy(gameObject);
    }
}
