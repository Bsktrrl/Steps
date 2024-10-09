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


    //--------------------


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
            MainManager.Instance.canMove_Forward = false;
        }
        else if (direction == Vector3.back)
        {
            MainManager.Instance.canMove_Back = false;
        }
        else if (direction == Vector3.left)
        {
            MainManager.Instance.canMove_Left = false;
        }
        else if (direction == Vector3.right)
        {
            MainManager.Instance.canMove_Right = false;
        }
    }
    void ResetRaycastDirection_Horizontal(Vector3 direction)
    {
        if (direction == Vector3.forward)
        {
            MainManager.Instance.canMove_Forward = true;
        }
        else if (direction == Vector3.back)
        {
            MainManager.Instance.canMove_Back = true;
        }
        else if (direction == Vector3.left)
        {
            MainManager.Instance.canMove_Left = true;
        }
        else if (direction == Vector3.right)
        {
            MainManager.Instance.canMove_Right = true;
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
            MainManager.Instance.canMove_Forward = false;
        }
        else if (direction == Vector3.back && MainManager.Instance.canMove_Back)
        {
            MainManager.Instance.canMove_Back = false;
        }
        else if (direction == Vector3.left && MainManager.Instance.canMove_Left)
        {
            MainManager.Instance.canMove_Left = false;
        }
        else if (direction == Vector3.right && MainManager.Instance.canMove_Right)
        {
            MainManager.Instance.canMove_Right = false;
        }
    }
}
