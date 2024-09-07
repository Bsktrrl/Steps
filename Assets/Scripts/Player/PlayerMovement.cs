using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{
    public static event Action takeAStep;
    public static event Action playerStopped;

    public Vector3 movementDirection;
    public MovementStates movementStates;

    public Vector3 moveToPos;
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
        if (MainManager.Instance.pauseGame) { return; }

        if (Input.GetKey(KeyCode.W))
        {
            if (PlayerStats.Instance.stats.steps_Current <= 0)
            {
                movementStates = MovementStates.Moving;
                PlayerStepCost.Instance.RefillSteps();

                return;
            }

            if (GetComponent<PlayerDetectorController>().platform_Vertical_Forward && Player_PlatformRequirementCheck.Instance.CheckPlatformRequirement(MovementDirection.Forward))
            {
                //Don't move into a Wall
                if (GetComponent<PlayerDetectorController>().platform_Horizontal_Forward)
                {
                    if (GetComponent<PlayerDetectorController>().platform_Horizontal_Forward.GetComponent<Platform>().platformType == PlatformType.Wall) { return; }
                }
                
                movementDirection = Vector3.forward;
                moveToPos = GetUpperCenterPositionOfPlatform(GetComponent<PlayerDetectorController>().platform_Vertical_Forward.gameObject.transform);
                movementStates = MovementStates.Moving;

                takeAStep?.Invoke();
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (PlayerStats.Instance.stats.steps_Current <= 0)
            {
                movementStates = MovementStates.Moving;
                PlayerStepCost.Instance.RefillSteps();

                return;
            }

            if (GetComponent<PlayerDetectorController>().platform_Vertical_Backward && Player_PlatformRequirementCheck.Instance.CheckPlatformRequirement(MovementDirection.Backward))
            {
                //Don't move into a Wall
                if (GetComponent<PlayerDetectorController>().platform_Horizontal_Backward)
                {
                    if (GetComponent<PlayerDetectorController>().platform_Horizontal_Backward.GetComponent<Platform>().platformType == PlatformType.Wall) { return; }
                }

                movementDirection = Vector3.back;
                moveToPos = GetUpperCenterPositionOfPlatform(GetComponent<PlayerDetectorController>().platform_Vertical_Backward.gameObject.transform);
                movementStates = MovementStates.Moving;

                takeAStep?.Invoke();
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (PlayerStats.Instance.stats.steps_Current <= 0)
            {
                movementStates = MovementStates.Moving;
                PlayerStepCost.Instance.RefillSteps();

                return;
            }

            if (GetComponent<PlayerDetectorController>().platform_Vertical_Right && Player_PlatformRequirementCheck.Instance.CheckPlatformRequirement(MovementDirection.Right))
            {
                //Don't move into a Wall
                if (GetComponent<PlayerDetectorController>().platform_Horizontal_Right)
                {
                    if (GetComponent<PlayerDetectorController>().platform_Horizontal_Right.GetComponent<Platform>().platformType == PlatformType.Wall) { return; }
                }

                movementDirection = Vector3.right;
                moveToPos = GetUpperCenterPositionOfPlatform(GetComponent<PlayerDetectorController>().platform_Vertical_Right.gameObject.transform);
                movementStates = MovementStates.Moving;

                takeAStep?.Invoke();
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (PlayerStats.Instance.stats.steps_Current <= 0)
            {
                movementStates = MovementStates.Moving;
                PlayerStepCost.Instance.RefillSteps();

                return;
            }

            if (GetComponent<PlayerDetectorController>().platform_Vertical_Left && Player_PlatformRequirementCheck.Instance.CheckPlatformRequirement(MovementDirection.Left))
            {
                //Don't move into a Wall
                if (GetComponent<PlayerDetectorController>().platform_Horizontal_Left)
                {
                    if (GetComponent<PlayerDetectorController>().platform_Horizontal_Left.GetComponent<Platform>().platformType == PlatformType.Wall) { return; }
                }

                movementDirection = Vector3.left;
                moveToPos = GetUpperCenterPositionOfPlatform(GetComponent<PlayerDetectorController>().platform_Vertical_Left.gameObject.transform);
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

        transform.SetPositionAndRotation(transform.position + movementDirection * PlayerStats.Instance.stats.movementSpeed * Time.deltaTime, Quaternion.identity);
    }
    void CheckPlayerStop()
    {
        if (movementStates == MovementStates.Still) { return; }

        if (Vector3.Distance(GetBottomCenterPositionOfPlayer(transform), moveToPos) < 0.1f)
        {
            movementStates = MovementStates.Still;

            transform.SetPositionAndRotation(new Vector3(moveToPos.x, moveToPos.y + 0.35f, moveToPos.z), Quaternion.identity);

            moveToPos = Vector3.zero;

            StartCoroutine(WaitToMoveAgain(0.01f));

            playerStopped?.Invoke();
        }
    }

    IEnumerator WaitToMoveAgain(float waitTime)
    {
        MainManager.Instance.pauseGame = true;

        yield return new WaitForSeconds(waitTime);

        MainManager.Instance.pauseGame = false;
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