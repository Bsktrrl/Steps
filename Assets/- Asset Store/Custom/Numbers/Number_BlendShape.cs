using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number_BlendShape : MonoBehaviour
{
    SkinnedMeshRenderer SMR;
    void Start()
    {
        SMR = GetComponent<SkinnedMeshRenderer>();
        SMR.SetBlendShapeWeight(0, 100);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if(SMR.GetBlendShapeWeight(0) == 0)
            {
                SMR.SetBlendShapeWeight(0, 100);
            }
            else
            {
                SMR.SetBlendShapeWeight(0, 0);
            }
        }
    }
}
