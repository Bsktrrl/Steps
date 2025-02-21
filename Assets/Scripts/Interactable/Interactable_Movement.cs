using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Interactable_Movement : MonoBehaviour
{
    //Floating
    [Header("Float Behavior")]
    [SerializeField] float amplitude = 0.25f; // How high and low the object will float
    [SerializeField] float frequency = 1f;   // How fast the object will float
    
    //Rotation
    [Header("Rotation")]
    [SerializeField] float RotationSpeed = 0.25f;
    float Rotation;

    Vector3 startPos;


    //--------------------


    private void Start()
    {
        startPos = transform.position;
    }
    private void Update()
    {
        gameObject.transform.SetLocalPositionAndRotation(SetPosition(), Quaternion.Euler(0, SetRotation_Y(), 0));
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
