using System.Collections;
using UnityEngine;

public class Interactable_Movement : MonoBehaviour
{
    // Floating
    [Header("Float Behavior")]
    float amplitude; // How high and low the object will float
    float frequency; // How fast the object will float

    // Rotation
    [Header("Rotation")]
    float RotationSpeed;
    float Rotation;

    Vector3 startPos;
    Quaternion startRotation;


    //--------------------


    private void Start()
    {
        amplitude = 0.05f;
        frequency = 0.8f;
        RotationSpeed = 0.25f;
        Rotation = 0;

        // Use local coordinates so the pickup follows its parent's movement.
        startPos = transform.localPosition;
        startRotation = transform.localRotation;
    }

    private void Update()
    {
        transform.localPosition = SetPosition();
        transform.localRotation = startRotation * Quaternion.Euler(0, SetRotation_Y(), 0);
    }


    //--------------------


    Vector3 SetPosition()
    {
        float newPosY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

        return new Vector3(startPos.x, newPosY, startPos.z);
    }

    float SetRotation_Y()
    {
        Rotation -= RotationSpeed;

        return Rotation;
    }
}
