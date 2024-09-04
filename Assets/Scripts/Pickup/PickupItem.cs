using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            MainManager.Instance.collectables.coin += 1;

            DestroyThisObject();
        }
    }

    void DestroyThisObject()
    {
        Destroy(gameObject);
    }
}
