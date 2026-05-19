using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed_X;
    [SerializeField] float rotationSpeed_Y;
    [SerializeField] float rotationSpeed_Z;


    //--------------------


    private void Update()
    {
        RotateObject();
    }


    //--------------------


    void RotateObject()
    {
        Vector3 rotationAmount = new Vector3(rotationSpeed_X, rotationSpeed_Y, rotationSpeed_Z) * Time.deltaTime;

        transform.Rotate(rotationAmount, Space.Self);
    }
}
