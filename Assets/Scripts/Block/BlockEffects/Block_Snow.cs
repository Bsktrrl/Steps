using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

public class Block_Snow : MonoBehaviour
{
    [HideInInspector] public List<GameObject> LOD_ObjectList = new List<GameObject>();
    [SerializeField] float scale_Y_Value;


    //--------------------


    private void Start()
    {
        GetLOD();
        SetRandomBlockHeight();
        ChangeStepCounterPosition();
    }

    private void OnEnable()
    {
        Player_CeilingGrab.Action_grabCeiling += ChangeStepCounterPosition;
        Player_CeilingGrab.Action_releaseCeiling += ChangeStepCounterPosition;
    }
    private void OnDisable()
    {
        Player_CeilingGrab.Action_grabCeiling -= ChangeStepCounterPosition;
        Player_CeilingGrab.Action_releaseCeiling -= ChangeStepCounterPosition;
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

        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            //parentObject.transform.SetLocalPositionAndRotation(new Vector3(parentObject.transform.localPosition.x, (transform.position.y - (scale_Y_Value / 2)), parentObject.transform.localPosition.z), Quaternion.identity);
            parentObject.transform.SetLocalPositionAndRotation(new Vector3(parentObject.transform.localPosition.x, (- 1 - (scale_Y_Value - 1) - (scale_Y_Value / 10)), parentObject.transform.localPosition.z), Quaternion.identity);
        }
        else
        {
            parentObject.transform.SetLocalPositionAndRotation(new Vector3(parentObject.transform.localPosition.x, (scale_Y_Value - 1), parentObject.transform.localPosition.z), Quaternion.identity);
        }
    }
}
