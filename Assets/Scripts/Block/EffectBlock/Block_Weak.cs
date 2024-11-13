using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Weak : MonoBehaviour
{


    //--------------------


    private void Start()
    {
        GameObject obj = Instantiate(Player_WeakBlock.Instance.blockCrack, transform);

        obj.transform.localPosition = Vector3.zero;
    }


    //--------------------


    public void DestroyWeakBlock()
    {
        gameObject.GetComponent<BlockInfo>().DestroyBlockInfo();
        gameObject.GetComponent<BlockStepCostDisplay>().DestroyBlockStepCostDisplay();

        Destroy(gameObject);
    }
}
