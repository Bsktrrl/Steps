using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Weak : MonoBehaviour
{
    private void Start()
    {
        GameObject obj = Instantiate(Player_Hammer.Instance.blockCrack, transform);

        obj.transform.localPosition = Vector3.zero;
    }
    private void OnEnable()
    {
        PlayerStats.Action_RespawnPlayer += ResetBlock;
    }

    private void OnDisable()
    {
        PlayerStats.Action_RespawnPlayer -= ResetBlock;
    }


    //--------------------


    public void DestroyWeakBlock()
    {
        gameObject.SetActive(false);
        //gameObject.GetComponent<BlockStepCostDisplay>().DestroyBlockStepCostDisplay();

        //Destroy(gameObject);
    }
    public void ResetBlock()
    {
        gameObject.SetActive(true);
    }
}
