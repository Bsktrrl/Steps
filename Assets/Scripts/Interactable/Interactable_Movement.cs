using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Interactable_Movement : MonoBehaviour
{
    //Floating
    [Header("Float Behavior")]
    float amplitude; // How high and low the object will float
    float frequency;   // How fast the object will float
    
    //Rotation
    [Header("Rotation")]
    float RotationSpeed;
    float Rotation;

    Vector3 startPos;


    //--------------------


    private void Start()
    {
        amplitude = 0.05f;
        frequency = 0.8f;
        RotationSpeed = 0.25f;
        Rotation = 0;

        startPos = transform.position;
    }
    private void Update()
    {
        gameObject.transform.SetPositionAndRotation(SetPosition(), Quaternion.Euler(0, SetRotation_Y(), 0));
    }


    //--------------------


    Vector3 SetPosition()
    {
        float newPosY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

        return new Vector3(transform.position.x, newPosY, transform.position.z);
    }
    float SetRotation_Y()
    {
        Rotation -= RotationSpeed;

        return Rotation;
    }
}
