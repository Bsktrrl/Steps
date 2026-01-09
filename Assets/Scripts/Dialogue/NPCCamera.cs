using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCamera : MonoBehaviour
{
    [Header("Camera Direction")]
    [SerializeField] CameraDirection cameraDirection;

    [Header("Camera States")]
    [SerializeField] CameraStates cameraState_FrontRight;
    [SerializeField] CameraStates cameraState_FrontLeft;
    [SerializeField] CameraStates cameraState_BackRight;
    [SerializeField] CameraStates cameraState_BackLeft;


    //--------------------


    private void Start()
    {
        //cameraState_FrontRight.position = new Vector3(2.47f, 0.16f, 2.58f);
        //cameraState_FrontRight.rotation = new Vector3(8f, -133f, 0);

        //cameraState_FrontLeft.position = new Vector3(-2.47f, 0.16f, 2.58f);
        //cameraState_FrontLeft.rotation = new Vector3(5f, 127f, 0);

        //cameraState_BackRight.position = new Vector3(2.47f, 0.16f, -2.58f);
        //cameraState_BackRight.rotation = new Vector3(8f, -45f, 0);

        //cameraState_BackLeft.position = new Vector3(-2.47f, 0.16f, -2.58f);
        //cameraState_BackLeft.rotation = new Vector3(8f, 35f, 0);

        //SetCameraDirection();
    }


    //--------------------


    public void SetCameraDirection()
    {
        switch (cameraDirection)
        {
            case CameraDirection.Front_Right:
                gameObject.transform.SetLocalPositionAndRotation(/*gameObject.transform.parent.position +*/ cameraState_FrontRight.position, Quaternion.Euler(/*gameObject.transform.parent.rotation.eulerAngles -*/ cameraState_FrontRight.rotation));
                break;
            case CameraDirection.Front_Left:
                gameObject.transform.SetLocalPositionAndRotation(/*gameObject.transform.parent.position +*/ cameraState_FrontLeft.position, Quaternion.Euler(/*gameObject.transform.parent.rotation.eulerAngles -*/ cameraState_FrontLeft.rotation));
                break;
            case CameraDirection.Back_Right:
                gameObject.transform.SetLocalPositionAndRotation(/*gameObject.transform.parent.position +*/ cameraState_BackRight.position, Quaternion.Euler(/*gameObject.transform.parent.rotation.eulerAngles -*/ cameraState_BackRight.rotation));
                break;
            case CameraDirection.Back_Left:
                gameObject.transform.SetLocalPositionAndRotation(/*gameObject.transform.parent.position +*/ cameraState_BackLeft.position, Quaternion.Euler(/*gameObject.transform.parent.rotation.eulerAngles -*/ cameraState_BackLeft.rotation));
                break;

            default:
                break;
        }
    }
}
[Serializable]
public class CameraStates
{
    public Vector3 position;
    public Vector3 rotation;
}

public enum CameraDirection
{
    Front_Right,
    Front_Left,
    Back_Right,
    Back_Left
}