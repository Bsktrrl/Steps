using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchOptions : MonoBehaviour
{
    [SerializeField] List<GameObject> collidersList = new List<GameObject>();


    //--------------------


    private void OnEnable()
    {
        Interactable_Pickup.Action_JumpingGot += SetColliders;
        DataManager.Action_dataHasLoaded += SetColliders;
        Movement.Action_RespawnPlayer += SetColliders;
    }
    private void OnDisable()
    {
        Interactable_Pickup.Action_JumpingGot -= SetColliders;
        DataManager.Action_dataHasLoaded -= SetColliders;
        Movement.Action_RespawnPlayer += SetColliders;
    }


    //--------------------


    void SetColliders()
    {
        if (DataManager.Instance.playerStats_Store.abilitiesGot_Temporary.SpringShoes || DataManager.Instance.playerStats_Store.abilitiesGot_Permanent.SpringShoes)
        {
            for (int i = 0; i < collidersList.Count; i++)
            {
                collidersList[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < collidersList.Count; i++)
            {
                collidersList[i].SetActive(true);
            }
        }
    }
}
