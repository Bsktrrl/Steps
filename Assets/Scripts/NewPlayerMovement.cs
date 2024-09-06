using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5;

    [SerializeField] Vector3 movementDirection;
    [SerializeField] float platformRotationDegree;
    [SerializeField] MovementStates movementStates;

    [SerializeField] Vector3 moveToPos;
    public LayerMask platformMask;


    //--------------------


    private void Update()
    {
        KeyInputs();

        CheckPlayerStop();

        MovePlayer();

        AlignToSlope();

        MoveWithTheTerrain();
    }


    //--------------------


    void KeyInputs()
    {
        if (movementStates == MovementStates.Moving) { return; }

        if (Input.GetKey(KeyCode.W))
        {
            movementDirection = Vector3.forward;
            platformRotationDegree = GetComponent<PlayerController>().detector_Vertical_Forward.gameObject.transform.rotation.x;
            moveToPos = GetComponent<PlayerController>().detector_Vertical_Forward.gameObject.transform.position;
            movementStates = MovementStates.Moving;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementDirection = Vector3.back;
            platformRotationDegree = GetComponent<PlayerController>().detector_Vertical_Backward.gameObject.transform.rotation.x;
            moveToPos = GetComponent<PlayerController>().detector_Vertical_Backward.gameObject.transform.position;
            movementStates = MovementStates.Moving;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementDirection = Vector3.right;
            platformRotationDegree = GetComponent<PlayerController>().detector_Vertical_Right.gameObject.transform.rotation.x;
            moveToPos = GetComponent<PlayerController>().detector_Vertical_Right.gameObject.transform.position;
            movementStates = MovementStates.Moving;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementDirection = Vector3.left;
            platformRotationDegree = GetComponent<PlayerController>().detector_Vertical_Left.gameObject.transform.rotation.x;
            moveToPos = GetComponent<PlayerController>().detector_Vertical_Left.gameObject.transform.position;
            movementStates = MovementStates.Moving;
        }
    }
    void MovePlayer()
    {
        if (movementStates == MovementStates.Still) { return; }

        transform.SetPositionAndRotation(transform.position + movementDirection * movementSpeed * Time.deltaTime, Quaternion.Euler(platformRotationDegree, 0, 0));
    }
    void AlignToSlope()
    {
        if (movementStates == MovementStates.Still) { return; }

        if (movementDirection == Vector3.forward)
        {
            // Align the cube's up vector to the slope normal
            Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, GetComponent<PlayerController>().detector_Vertical_Forward.GetComponent<NewDetector>().detectedPlatformNormal) * transform.rotation;
            
            // Smoothly rotate the cube to align with the slope
            transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation, 10 * Time.deltaTime);
        }
    }
    void CheckPlayerStop()
    {
        if (Vector3.Distance(transform.position, moveToPos) < 0.05f)
        {
            movementStates = MovementStates.Still;

            moveToPos = Vector3.zero;
        }
    }

    void MoveWithTheTerrain()
    {
        // Raycast downward from the cube to detect the terrain's surface
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 10f, platformMask))
        {
            // Adjust the cube's position to follow the terrain height
            Vector3 targetPosition = new Vector3(transform.position.x, hit.point.y + 0.3f, transform.position.z);
            transform.position = targetPosition;

            // Align the cube's up direction to the terrain's surface normal
            Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = slopeRotation;
        }
    }
}

public enum MovementStates
{
    Still,
    Moving
}