using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTerrain : MonoBehaviour
{
    public float moveSpeed = 5f;        // Speed of movement
    public float rotationSpeed = 100f;   // Speed of rotation alignment with the terrain slope
    //public LayerMask terrainLayerMask;  // Layer mask to specify the terrain

    void Update()
    {
        // Handle cube movement
        MoveCube();

        // Align the cube to the terrain
        //MoveWithTheTerrain();
    }

    void MoveCube()
    {
        // Get player input for forward/backward and left/right movement
        float moveInput = Input.GetAxis("Vertical");
        float strafeInput = Input.GetAxis("Horizontal");

        // Create movement direction based on input
        Vector3 moveDirection = new Vector3(strafeInput, 0, moveInput).normalized;

        // If there is movement input
        if (moveDirection.magnitude > 0)
        {
            // Move the cube in the forward direction (relative to world space)
            transform.Translate((moveDirection + new Vector3(0, 0.3f, 0)) * moveSpeed * Time.deltaTime, Space.World);

            // Rotate the cube to face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = targetRotation;
        }
    }

    //void MoveWithTheTerrain()
    //{
    //    // Raycast downward from the cube to detect the terrain's surface
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 10f, terrainLayerMask))
    //    {
    //        // Adjust the cube's position to follow the terrain height
    //        Vector3 targetPosition = new Vector3(transform.position.x, hit.point.y + 0.3f, transform.position.z);
    //        transform.position = targetPosition;

    //        // Align the cube's up direction to the terrain's surface normal
    //        Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
    //        transform.rotation = slopeRotation;
    //    }
    //}
}
