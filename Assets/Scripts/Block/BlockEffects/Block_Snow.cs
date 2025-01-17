using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Snow : MonoBehaviour
{
    [HideInInspector] public List<GameObject> LOD_ObjectList = new List<GameObject>();
    float scale_Y_Value;


    //--------------------


    private void Start()
    {
        GetLOD();
        SetRandomBlockHeight();
        ChangeStepCounterPosition();
    }


    //--------------------


    void GetLOD()
    {
        // Get all MeshFilters in the parent and its children
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        foreach (MeshFilter meshFilter in meshFilters)
        {
            LOD_ObjectList.Add(meshFilter.gameObject); // Add the GameObject to the list
        }
    }
    void SetRandomBlockHeight()
    {
        scale_Y_Value = Random.Range(1, 1.3f);

        for (int i = 0; i < LOD_ObjectList.Count; i++)
        {
            LOD_ObjectList[i].transform.localScale = new Vector3(1, scale_Y_Value, 1);
        }
    }
    void ChangeStepCounterPosition()
    {
        GameObject parentObject = GetComponent<BlockStepCostDisplay>().stepCostDisplay_Parent;

        parentObject.transform.SetLocalPositionAndRotation(new Vector3(parentObject.transform.localPosition.x, scale_Y_Value - 1, parentObject.transform.localPosition.z), Quaternion.identity);
    }
}
