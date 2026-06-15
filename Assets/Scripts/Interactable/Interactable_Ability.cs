using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Ability : MonoBehaviour
{
    [Header("Float Behavior")]
    [SerializeField] float amplitude = 0.05f;
    [SerializeField] float frequency = 0.8f;

    [Header("Rotation Behavior")]
    [SerializeField] GameObject frameObject;
    [SerializeField] GameObject iconObject;
    [SerializeField] float rotationSpeed = 0.25f;
    [SerializeField] float rotation;

    Vector3 startPos;
    float floatOffset;


    private void Start()
    {
        startPos = transform.position;

        // Random point in the sine wave curve
        floatOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        Vector3 position = SetPosition();

        rotation -= rotationSpeed;
        float yRotation = rotation;

        frameObject.transform.SetPositionAndRotation(
            position,
            Quaternion.Euler(0, yRotation, 0)
        );

        iconObject.transform.SetPositionAndRotation(
            position,
            Quaternion.Euler(0, -yRotation, 0)
        );
    }


    Vector3 SetPosition()
    {
        float newPosY = startPos.y + Mathf.Sin((Time.time * frequency) + floatOffset) * amplitude;

        return new Vector3(transform.position.x, newPosY, transform.position.z);
    }
}