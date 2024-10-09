using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewPlayerBlockDetector : MonoBehaviour
{
    [Header("DetectorSpots Horizontal")]
    [SerializeField] GameObject detectorSpot_Horizontal_Front;
    [SerializeField] GameObject detectorSpot_Horizontal_Back;
    [SerializeField] GameObject detectorSpot_Horizontal_Left;
    [SerializeField] GameObject detectorSpot_Horizontal_Right;

    [Header("DetectorSpots Vertical")]
    [SerializeField] GameObject detectorSpot_Vertical_Front;
    [SerializeField] GameObject detectorSpot_Vertical_Back;
    [SerializeField] GameObject detectorSpot_Vertical_Left;
    [SerializeField] GameObject detectorSpot_Vertical_Right;

    [Header("Raycast")]
    RaycastHit hit;
    float maxDistance = 0.5f;


    //--------------------


    private void Update()
    {
        RaycastSetup();
    }


    //--------------------


    void RaycastSetup()
    {
        //Check if something is in the way
        if (gameObject.GetComponent<PlayerCamera>().directionFacing == Vector3.forward)
        {
            //Check if something is in the way
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Front.transform.position, Vector3.forward);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Back.transform.position, Vector3.back);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Left.transform.position, Vector3.left);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Right.transform.position, Vector3.right);

            //Check if there is a block where the player can go
            PerformRaycast_Vertical(detectorSpot_Vertical_Front.transform.position, Vector3.forward);
            PerformRaycast_Vertical(detectorSpot_Vertical_Back.transform.position, Vector3.back);
            PerformRaycast_Vertical(detectorSpot_Vertical_Left.transform.position, Vector3.left);
            PerformRaycast_Vertical(detectorSpot_Vertical_Right.transform.position, Vector3.right);
        }
        else if (gameObject.GetComponent<PlayerCamera>().directionFacing == Vector3.back)
        {
            //Check if something is in the way
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Front.transform.position, Vector3.back);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Back.transform.position, Vector3.forward);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Left.transform.position, Vector3.right);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Right.transform.position, Vector3.left);

            //Check if there is a block where the player can go
            PerformRaycast_Vertical(detectorSpot_Vertical_Front.transform.position, Vector3.back);
            PerformRaycast_Vertical(detectorSpot_Vertical_Back.transform.position, Vector3.forward);
            PerformRaycast_Vertical(detectorSpot_Vertical_Left.transform.position, Vector3.right);
            PerformRaycast_Vertical(detectorSpot_Vertical_Right.transform.position, Vector3.left);
        }
        else if (gameObject.GetComponent<PlayerCamera>().directionFacing == Vector3.left)
        {
            //Check if something is in the way
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Front.transform.position, Vector3.left);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Back.transform.position, Vector3.right);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Left.transform.position, Vector3.back);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Right.transform.position, Vector3.forward);

            //Check if there is a block where the player can go
            PerformRaycast_Vertical(detectorSpot_Vertical_Front.transform.position, Vector3.left);
            PerformRaycast_Vertical(detectorSpot_Vertical_Back.transform.position, Vector3.right);
            PerformRaycast_Vertical(detectorSpot_Vertical_Left.transform.position, Vector3.back);
            PerformRaycast_Vertical(detectorSpot_Vertical_Right.transform.position, Vector3.forward);
        }
        else if (gameObject.GetComponent<PlayerCamera>().directionFacing == Vector3.right)
        {
            //Check if something is in the way
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Front.transform.position, Vector3.right);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Back.transform.position, Vector3.left);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Left.transform.position, Vector3.forward);
            PerformRaycast_Horizontal(detectorSpot_Horizontal_Right.transform.position, Vector3.back);

            //Check if there is a block where the player can go
            PerformRaycast_Vertical(detectorSpot_Vertical_Front.transform.position, Vector3.right);
            PerformRaycast_Vertical(detectorSpot_Vertical_Back.transform.position, Vector3.left);
            PerformRaycast_Vertical(detectorSpot_Vertical_Left.transform.position, Vector3.forward);
            PerformRaycast_Vertical(detectorSpot_Vertical_Right.transform.position, Vector3.back);
        }
    }
    void PerformRaycast_Horizontal(Vector3 rayPoint, Vector3 direction)
    {
        if (Physics.Raycast(rayPoint, direction, out hit, maxDistance, MainManager.Instance.Cube))
        {
            Debug.DrawRay(rayPoint, direction * hit.distance, Color.red);

            CheckRaycastDirection_Horizontal(direction);
        }
        else if (Physics.Raycast(rayPoint, direction, out hit, maxDistance, MainManager.Instance.Ladder))
        {
            Debug.DrawRay(rayPoint, direction * hit.distance, Color.blue);
            CheckRaycastDirection_Horizontal(direction);
        }

        else
        {
            Debug.DrawRay(rayPoint, direction * maxDistance, Color.green);

            ResetRaycastDirection_Horizontal(direction);
        }
    }
    void CheckRaycastDirection_Horizontal(Vector3 direction)
    {
        if (direction == Vector3.forward)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Forward = false;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Back = false;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Right = false;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Left = false;
                    break;

                default:
                    break;
            }
        }
        else if (direction == Vector3.back)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Back = false;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Forward = false;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Left = false;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Right = false;
                    break;

                default:
                    break;
            }
        }
        else if (direction == Vector3.left)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Left = false;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Right = false;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Forward = false;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Back = false;
                    break;

                default:
                    break;
            }
        }
        else if (direction == Vector3.right)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Right = false;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Left = false;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Back = false;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Forward = false;
                    break;

                default:
                    break;
            }
        }
    }
    void ResetRaycastDirection_Horizontal(Vector3 direction)
    {
        if (direction == Vector3.forward)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Forward = true;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Back = true;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Right = true;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Left = true;
                    break;

                default:
                    break;
            }
        }
        else if (direction == Vector3.back)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Back = true;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Forward = true;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Left = true;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Right = true;
                    break;

                default:
                    break;
            }
        }
        else if (direction == Vector3.left)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Left = true;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Right = true;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Forward = true;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Back = true;
                    break;

                default:
                    break;
            }
        }
        else if (direction == Vector3.right)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Right = true;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Left = true;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Back = true;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Forward = true;
                    break;

                default:
                    break;
            }
        }
    }

    void PerformRaycast_Vertical(Vector3 rayPoint, Vector3 direction)
    {
        if (Physics.Raycast(rayPoint, Vector3.down, out hit, maxDistance, MainManager.Instance.Cube))
        {
            Debug.DrawRay(rayPoint, Vector3.down * hit.distance, Color.green);
        }
        else if (Physics.Raycast(rayPoint, Vector3.down, out hit, maxDistance, MainManager.Instance.Ladder))
        {
            Debug.DrawRay(rayPoint, Vector3.down * hit.distance, Color.blue);
        }

        else
        {
            Debug.DrawRay(rayPoint, Vector3.down * maxDistance, Color.red);

            ResetRaycastDirection_Vertical(direction);
        }
    }
    void ResetRaycastDirection_Vertical(Vector3 direction)
    {
        if (direction == Vector3.forward && MainManager.Instance.canMove_Forward)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Forward = false;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Back = false;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Left = false;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Right = false;
                    break;

                default:
                    break;
            }
        }
        else if (direction == Vector3.back && MainManager.Instance.canMove_Back)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Back = false;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Forward = false;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Right = false;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Left = false;
                    break;

                default:
                    break;
            }
        }
        else if (direction == Vector3.left && MainManager.Instance.canMove_Left)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Left = false;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Right = false;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Forward = false;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Back = false;
                    break;

                default:
                    break;
            }
        }
        else if (direction == Vector3.right && MainManager.Instance.canMove_Right)
        {
            switch (gameObject.GetComponent<PlayerCamera>().cameraState)
            {
                case CameraState.Forward:
                    MainManager.Instance.canMove_Right = false;
                    break;
                case CameraState.Backward:
                    MainManager.Instance.canMove_Left = false;
                    break;
                case CameraState.Left:
                    MainManager.Instance.canMove_Back = false;
                    break;
                case CameraState.Right:
                    MainManager.Instance.canMove_Forward = false;
                    break;

                default:
                    break;
            }
        }
    }
}
