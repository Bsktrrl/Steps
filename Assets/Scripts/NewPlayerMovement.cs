using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    public static event Action takeAStep;
    public static event Action playerStopped;

    public float movementSpeed = 5;

    [SerializeField] Vector3 movementDirection;
    //[SerializeField] float platformRotationDegree;
    [SerializeField] MovementStates movementStates;

    [SerializeField] Vector3 moveToPos;
    public LayerMask platformMask;


    //--------------------


    private void Update()
    {
        KeyInputs();

        CheckPlayerStop();

        MovePlayer();

        MoveWithTheTerrain();
    }


    //--------------------


    void KeyInputs()
    {
        if (movementStates == MovementStates.Moving) { return; }

        if (Input.GetKey(KeyCode.W))
        {
            if (GetComponent<PlayerController>().platform_Vertical_Forward)
            {
                //Don't move into a Wall
                if (GetComponent<PlayerController>().platform_Horizontal_Forward)
                {
                    if (GetComponent<PlayerController>().platform_Horizontal_Forward.GetComponent<NewPlatform>().platformType == PlatformType.Wall) { return; }
                }
                
                movementDirection = Vector3.forward;
                moveToPos = GetUpperCenterPositionOfPlatform(GetComponent<PlayerController>().platform_Vertical_Forward.gameObject.transform);
                movementStates = MovementStates.Moving;

                takeAStep?.Invoke();
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (GetComponent<PlayerController>().platform_Vertical_Backward)
            {
                //Don't move into a Wall
                if (GetComponent<PlayerController>().platform_Horizontal_Backward)
                {
                    if (GetComponent<PlayerController>().platform_Horizontal_Backward.GetComponent<NewPlatform>().platformType == PlatformType.Wall) { return; }
                }

                movementDirection = Vector3.back;
                moveToPos = GetUpperCenterPositionOfPlatform(GetComponent<PlayerController>().platform_Vertical_Backward.gameObject.transform);
                movementStates = MovementStates.Moving;

                takeAStep?.Invoke();
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (GetComponent<PlayerController>().platform_Vertical_Right)
            {
                //Don't move into a Wall
                if (GetComponent<PlayerController>().platform_Horizontal_Right)
                {
                    if (GetComponent<PlayerController>().platform_Horizontal_Right.GetComponent<NewPlatform>().platformType == PlatformType.Wall) { return; }
                }

                movementDirection = Vector3.right;
                moveToPos = GetUpperCenterPositionOfPlatform(GetComponent<PlayerController>().platform_Vertical_Right.gameObject.transform);
                movementStates = MovementStates.Moving;

                takeAStep?.Invoke();
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (GetComponent<PlayerController>().platform_Vertical_Left)
            {
                //Don't move into a Wall
                if (GetComponent<PlayerController>().platform_Horizontal_Left)
                {
                    if (GetComponent<PlayerController>().platform_Horizontal_Left.GetComponent<NewPlatform>().platformType == PlatformType.Wall) { return; }
                }

                movementDirection = Vector3.left;
                moveToPos = GetUpperCenterPositionOfPlatform(GetComponent<PlayerController>().platform_Vertical_Left.gameObject.transform);
                movementStates = MovementStates.Moving;

                takeAStep?.Invoke();
            }
        }
    }

    Vector3 GetUpperCenterPositionOfPlatform(Transform platformTransform)
    {
        // Get the center of the platform
        Vector3 platformCenter = platformTransform.position;

        // Get the height of the platform
        float platformHeight = platformTransform.localScale.y;

        // Calculate the upper center position by adding half the height to the Y-axis
        Vector3 upperCenter = platformCenter + platformTransform.up * (platformHeight / 2);

        return new Vector3(upperCenter.x, upperCenter.y - 0.05f, upperCenter.z);
    }
    Vector3 GetBottomCenterPositionOfPlayer(Transform playerTransform)
    {
        // Get the center of the platform
        Vector3 playerCenter = playerTransform.position;

        // Get the height of the platform
        float playerHeight = playerTransform.localScale.y;

        // Calculate the upper center position by adding half the height to the Y-axis
        Vector3 bottomCenter = playerCenter + playerTransform.up * (playerHeight / 2);


        return new Vector3(bottomCenter.x, bottomCenter.y - 0.6f, bottomCenter.z);
    }

    void MovePlayer()
    {
        if (movementStates == MovementStates.Still) { return; }

        transform.SetPositionAndRotation(transform.position + movementDirection * movementSpeed * Time.deltaTime, Quaternion.identity /*Quaternion.Euler(platformRotationDegree, 0, 0)*/);
    }
    void CheckPlayerStop()
    {
        if (movementStates == MovementStates.Still) { return; }

        if (Vector3.Distance(GetBottomCenterPositionOfPlayer(transform), moveToPos) < 0.1f)
        {
            movementStates = MovementStates.Still;

            transform.SetPositionAndRotation(new Vector3(moveToPos.x, moveToPos.y + 0.35f, moveToPos.z), Quaternion.identity);

            moveToPos = Vector3.zero;

            playerStopped?.Invoke();
        }
    }

    void MoveWithTheTerrain()
    {
        // Raycast downward from the cube to detect the terrain's surface
        RaycastHit hit;
        if (Physics.Raycast(transform.position /*+ Vector3.up*/, Vector3.down, out hit, 10f, platformMask))
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