using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScript : MonoBehaviour
{
    [SerializeField] private int switchType;
    [SerializeField] private SkinnedMeshRenderer LOD0;
    [SerializeField] private SkinnedMeshRenderer LOD1;
    [SerializeField] private SkinnedMeshRenderer LOD2;

    void Start()
    {
        
    }

    void Update()
    {
        //Press P to activate switches
        //Switch type 0 - regular button
        if (switchType == 0)
        {
            if (Input.GetKey(KeyCode.P))
            {
                LOD0.SetBlendShapeWeight(0, 100);
                LOD1.SetBlendShapeWeight(0, 100);
                LOD2.SetBlendShapeWeight(0, 100);
            }
            else
            {
                LOD0.SetBlendShapeWeight(0, 0);
                LOD1.SetBlendShapeWeight(0, 0);
                LOD2.SetBlendShapeWeight(0, 0);
            }
        }
        //Switch type 1 - button with switch
        else if (switchType == 1)
        {
            if (Input.GetKey(KeyCode.P))
            {
                LOD0.SetBlendShapeWeight(0, 100);
                LOD1.SetBlendShapeWeight(0, 100);
                LOD2.SetBlendShapeWeight(0, 100);
            }
            else
            {
                LOD0.SetBlendShapeWeight(0, 0);
                LOD1.SetBlendShapeWeight(0, 0);
                LOD2.SetBlendShapeWeight(0, 0);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                if (LOD0.GetBlendShapeWeight(1) == 100)
                {
                    LOD0.SetBlendShapeWeight(1, 0);
                    LOD1.SetBlendShapeWeight(1, 0);
                    LOD2.SetBlendShapeWeight(1, 0);
                }
                else
                {
                    LOD0.SetBlendShapeWeight(1, 100);
                    LOD1.SetBlendShapeWeight(1, 100);
                    LOD2.SetBlendShapeWeight(1, 100);
                }
            }
        }
        //Switch type 2 - two connected buttons
        else if (switchType == 2)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (LOD0.GetBlendShapeWeight(0) == 100)
                {
                    LOD0.SetBlendShapeWeight(0, 0);
                    LOD1.SetBlendShapeWeight(0, 0);
                    LOD2.SetBlendShapeWeight(0, 0);
                }
                else
                {
                    LOD0.SetBlendShapeWeight(0, 100);
                    LOD1.SetBlendShapeWeight(0, 100);
                    LOD2.SetBlendShapeWeight(0, 100);
                }
            }
        }
        //Switch type 3 - lever on wall
        else if (switchType == 3)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (LOD0.GetBlendShapeWeight(0) == 100)
                {
                    LOD0.SetBlendShapeWeight(0, 0);
                    LOD1.SetBlendShapeWeight(0, 0);
                    LOD2.SetBlendShapeWeight(0, 0);
                }
                else
                {
                    LOD0.SetBlendShapeWeight(0, 100);
                    LOD1.SetBlendShapeWeight(0, 100);
                    LOD2.SetBlendShapeWeight(0, 100);
                }
            }
        }
    }
}
