using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

public class Block_Snow : MonoBehaviour
{
    public static event Action Action_SnowSetup_End;

    [HideInInspector] public List<GameObject> LOD_ObjectList = new List<GameObject>();
    [SerializeField] float scale_Y_Value;
    [SerializeField] float localInitialPos_Y;


    //--------------------


    private void Start()
    {
        GetLOD();

        SetLocalInitialPos_Y();
        SetRandomBlockHeight();

        ChangeStepCounterPosition();

        Action_SnowSetup_End?.Invoke();
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
    void SetLocalInitialPos_Y()
    {
        localInitialPos_Y = GetComponent<BlockStepCostDisplay>().stepCostDisplay_Parent.transform.localPosition.y;
    }
    void SetRandomBlockHeight()
    {
        if (GetComponent<BlockInfo>().blockType == BlockType.Slab)
            scale_Y_Value = UnityEngine.Random.Range(0.3f, 0.6f);
        else
            scale_Y_Value = UnityEngine.Random.Range(1, 1.3f);

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
            parentObject.transform.SetLocalPositionAndRotation(new Vector3(parentObject.transform.localPosition.x, (- 1 - (scale_Y_Value - 1) - (scale_Y_Value / 10)), parentObject.transform.localPosition.z), Quaternion.identity);
        }
        else
        {
            if (GetComponent<BlockInfo>().blockType == BlockType.Slab)
                parentObject.transform.SetLocalPositionAndRotation(new Vector3(parentObject.transform.localPosition.x, (scale_Y_Value / 2) - (localInitialPos_Y * 2), parentObject.transform.localPosition.z), Quaternion.identity);
            else
                parentObject.transform.SetLocalPositionAndRotation(new Vector3(parentObject.transform.localPosition.x, (scale_Y_Value - 1) /*- (localInitialPos_Y * 2)*/, parentObject.transform.localPosition.z), Quaternion.identity);
        }
    }
}
