using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_OffsetScale : MonoBehaviour
{
    public static event Action ActionBlockOffset_End;

    [SerializeField] GameObject numberDisplayObject;

    [HideInInspector] public List<GameObject> LOD_ObjectList = new List<GameObject>();
    [SerializeField] float scale_Y_Value;
    [SerializeField] float localInitialPos_Y;


    //--------------------


    private void Start()
    {
        GetLOD();

        ChangeStepCounterPosition();

        CorrectChilderenForSnow();

        ActionBlockOffset_End?.Invoke();
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
    public void ChangeStepCounterPosition()
    {
        float childScale_Y = transform.GetChild(0).transform.localScale.y;

        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            numberDisplayObject.transform.localPosition = new Vector3(0, -Mathf.Abs((1 - childScale_Y) / 2), 0);
            numberDisplayObject.GetComponent<NumberDisplay>().localStartHeight = numberDisplayObject.transform.localPosition.y;
        }
        else
        {
            if (GetComponent<BlockInfo>().blockType == BlockType.Slab)
            {
                numberDisplayObject.transform.localPosition = new Vector3(0, Mathf.Abs(((1 - childScale_Y) / 2) / 10), 0);
                numberDisplayObject.GetComponent<NumberDisplay>().localStartHeight = numberDisplayObject.transform.localPosition.y;
            }
            else
            {
                numberDisplayObject.transform.localPosition = new Vector3(0, Mathf.Abs((1 - childScale_Y) / 2), 0);
                numberDisplayObject.GetComponent<NumberDisplay>().localStartHeight = numberDisplayObject.transform.localPosition.y;
            }
        }
    }

    void CorrectChilderenForSnow()
    {
        if (GetComponentInChildren<EffectVisualMarker>())
        {
            GetComponentInChildren<EffectVisualMarker>().gameObject.transform.localPosition += new Vector3(0, (scale_Y_Value - 1) / 2, 0);
        }
    }
}
