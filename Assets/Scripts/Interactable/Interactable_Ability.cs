using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Ability : MonoBehaviour
{
    [Header("Float Behavior")]
    [SerializeField] float amplitude = 0.05f; // How high and low the object will float
    [SerializeField] float frequency = 0.8f;   // How fast the object will float

    [Header("Rotation Behavior")]
    [SerializeField] GameObject frameObject;
    [SerializeField] GameObject iconObject;
    [SerializeField] float rotationSpeed = 0.25f;
    [SerializeField] float rotation;

    Vector3 startPos;


    //--------------------


    private void Start()
    {
        startPos = transform.position;
    }
    private void Update()
    {
        frameObject.transform.SetPositionAndRotation(SetPosition(), Quaternion.Euler(0, SetRotation_Y(), 0));
        iconObject.transform.SetPositionAndRotation(SetPosition(), Quaternion.Euler(0, -SetRotation_Y(), 0));
    }


    //--------------------


    Vector3 SetPosition()
    {
        float newPosY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

        return new Vector3(transform.position.x, newPosY, transform.position.z);
    }
    float SetRotation_Y()
    {
        rotation -= rotationSpeed;

        return rotation;
    }
}
