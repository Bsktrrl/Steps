using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public static event Action pickup_Coin_IsHit;
    public static event Action pickup_Step_IsHit;

    public PickupTypes pickupType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            switch (pickupType)
            {
                case PickupTypes.None:
                    break;

                case PickupTypes.Coin:
                    pickup_Coin_IsHit?.Invoke();
                    DestroyThisObject();
                    break;
                case PickupTypes.Steps:
                    pickup_Step_IsHit?.Invoke();
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
