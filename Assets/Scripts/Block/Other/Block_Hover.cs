using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Hover : MonoBehaviour
{
    //Floating
    [Header("Float Behavior")]
    [SerializeField] float amplitude; // How high and low the object will float
    [SerializeField] float frequency;   // How fast the object will float

    Vector3 startPos;


    //--------------------


    private void Start()
    {
        startPos = transform.position;
    }
    private void Update()
    {
        gameObject.transform.SetPositionAndRotation(SetPosition(), transform.rotation);
    }


    //--------------------


    Vector3 SetPosition()
    {
        float newPosY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

        return new Vector3(transform.position.x, newPosY, transform.position.z);
    }
}
