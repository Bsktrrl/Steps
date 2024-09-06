using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static event Action takeAStep;
    public static event Action finishMovement;

    [Header("States")]
    public MovementState movementState;
    public MovementDirection movementDirection;

    [Header("GameObjects")]
    [SerializeField] GameObject detectorCenter;
    [SerializeField] GameObject detectorFront;
    [SerializeField] GameObject detectorBack;
    [SerializeField] GameObject detectorLeft;
    [SerializeField] GameObject detectorRight;

    [Header("Variables")]
    public Vector3 moveToPos;
    public LayerMask platformMask;

    private void Start()
    {
        PlayerReset.playerReset += Raycasting;

        movementState = MovementState.Still;
        movementDirection = MovementDirection.None;
    }
    void Update()
    {
        if (!MainManager.Instance.gamePaused)
        {
            KeyInputs();

            //If standing on a Platform
            if (MainManager.Instance.playerStats.platformObject_StandingOn_Current)
            {
                if (MainManager.Instance.playerStats.platformObject_StandingOn_Current.GetComponent<Platform>().platformType == PlatformTypes.Grass)
                {
                    GrassMovement();
                }
                else if (MainManager.Instance.playerStats.platformObject_StandingOn_Current.GetComponent<Platform>().platformType == PlatformTypes.Ice)
                {
                    IceMovement();
                }
                else if (MainManager.Instance.playerStats.platformObject_StandingOn_Current.GetComponent<Platform>().platformType == PlatformTypes.Sand)
                {
                    SandMovement();
                }
                else if (MainManager.Instance.playerStats.platformObject_StandingOn_Current.GetComponent<Platform>().platformType == PlatformTypes.Hill)
                {
                    HillMovement();
                }
                else if (MainManager.Instance.playerStats.platformObject_StandingOn_Current.GetComponent<Platform>().platformType == PlatformTypes.Mountain)
                {
                    MountainMovement();
                }
                else if (MainManager.Instance.playerStats.platformObject_StandingOn_Current.GetComponent<Platform>().platformType == PlatformTypes.Water)
                {
                    WaterMovement();
                }
                else if (MainManager.Instance.playerStats.platformObject_StandingOn_Current.GetComponent<Platform>().platformType == PlatformTypes.Lava)
                {
                    LavaMovement();
                }
            }

            //If not standing on a Platform, move back to the previous platform
            else
            {
                if (MainManager.Instance.playerStats.platformObject_StandingOn_Previous)
                {
                    gameObject.transform.position = MainManager.Instance.playerStats.platformObject_StandingOn_Previous.transform.position;

                    Raycasting();
                }
                
                movementState = MovementState.Still;
                movementDirection = MovementDirection.None;

                finishMovement?.Invoke();
            }

            MoveWithTheTerrain();
        }
    }

    void KeyInputs()
    {
        //Set movement direction based on Key Input
        if (movementState == MovementState.Still)
        {
            Raycasting();

            if (Input.GetKey(KeyCode.W))
            {
                if (MainManager.Instance.playerStats.platformObject_Forward)
                {
                    MainManager.Instance.playerStats.stepsToUse -= MainManager.Instance.playerStats.platformObject_Forward.GetComponent<Platform>().stepsCost;

                    if (MainManager.Instance.playerStats.platformObject_StandingOn_Current)
                    {
                        moveToPos = MainManager.Instance.playerStats.platformObject_StandingOn_Current.transform.position + new Vector3(0, 0, 1);
                    }

                    movementDirection = MovementDirection.Forward;
                    movementState = MovementState.Moving;

                    takeAStep?.Invoke();
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (MainManager.Instance.playerStats.platformObject_Backward)
                {
                    MainManager.Instance.playerStats.stepsToUse -= MainManager.Instance.playerStats.platformObject_Backward.GetComponent<Platform>().stepsCost;

                    if (MainManager.Instance.playerStats.platformObject_StandingOn_Current)
                    {
                        moveToPos = MainManager.Instance.playerStats.platformObject_StandingOn_Current.transform.position + new Vector3(0, 0, -1);
                    }

                    movementDirection = MovementDirection.Backward;
                    movementState = MovementState.Moving;

                    takeAStep?.Invoke();
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (MainManager.Instance.playerStats.platformObject_Right)
                {
                    MainManager.Instance.playerStats.stepsToUse -= MainManager.Instance.playerStats.platformObject_Right.GetComponent<Platform>().stepsCost;

                    if (MainManager.Instance.playerStats.platformObject_StandingOn_Current)
                    {
                        moveToPos = MainManager.Instance.playerStats.platformObject_StandingOn_Current.transform.position + new Vector3(1, 0, 0);
                    }

                    movementDirection = MovementDirection.Right;
                    movementState = MovementState.Moving;

                    takeAStep?.Invoke();
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                if (MainManager.Instance.playerStats.platformObject_Left)
                {
                    MainManager.Instance.playerStats.stepsToUse -= MainManager.Instance.playerStats.platformObject_Left.GetComponent<Platform>().stepsCost;

                    if (MainManager.Instance.playerStats.platformObject_StandingOn_Current)
                    {
                        moveToPos = MainManager.Instance.playerStats.platformObject_StandingOn_Current.transform.position + new Vector3(-1, 0, 0);
                    }

                    movementDirection = MovementDirection.Left;
                    movementState = MovementState.Moving;

                    takeAStep?.Invoke();
                }
            }
        }
    }

    void GrassMovement()
    {
        //When Moving
        if (movementState == MovementState.Moving)
        {
            Vector3 PlayerWorldPos_Current = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);

            Raycasting();

            if (Vector3.Distance(PlayerWorldPos_Current, moveToPos) <= 0.05f)
            {
                gameObject.transform.position = new Vector3(moveToPos.x, 0.3f, moveToPos.z);

                Raycasting();

                movementState = MovementState.Still;
                movementDirection = MovementDirection.None;

                finishMovement?.Invoke();
            }

            //Move Player
            if (movementState == MovementState.Moving)
            {
                switch (movementDirection)
                {
                    case MovementDirection.None:
                        break;

                    case MovementDirection.Forward:
                        gameObject.transform.position += Vector3.forward * MainManager.Instance.playerStats.movementSpeed * Time.deltaTime;
                        break;
                    case MovementDirection.Backward:
                        gameObject.transform.position += Vector3.back * MainManager.Instance.playerStats.movementSpeed * Time.deltaTime;
                        break;
                    case MovementDirection.Left:
                        gameObject.transform.position += Vector3.left * MainManager.Instance.playerStats.movementSpeed * Time.deltaTime;
                        break;
                    case MovementDirection.Right:
                        gameObject.transform.position += Vector3.right * MainManager.Instance.playerStats.movementSpeed * Time.deltaTime;
                        break;

                    default:
                        break;
                }

                Raycasting();
            }
        }
    }
    void IceMovement()
    {
        //When Moving
        if (movementState == MovementState.Moving)
        {
            Vector3 PlayerAltPos = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);

            if (Vector3.Distance(PlayerAltPos, moveToPos) <= 0.1f)
            {
                gameObject.transform.position = new Vector3(moveToPos.x, 0.3f, moveToPos.z);

                if (MainManager.Instance.playerStats.platformObject_StandingOn_Current.GetComponent<Platform>().platformType == PlatformTypes.Ice)
                {
                    Raycasting();

                    if (movementDirection == MovementDirection.Forward && MainManager.Instance.playerStats.platformObject_Forward)
                    {
                        moveToPos = MainManager.Instance.playerStats.platformObject_StandingOn_Current.transform.position + new Vector3(0, 0, 1);
                    }
                    else if (movementDirection == MovementDirection.Backward && MainManager.Instance.playerStats.platformObject_Backward)
                    {
                        moveToPos = MainManager.Instance.playerStats.platformObject_StandingOn_Current.transform.position + new Vector3(0, 0, -1);
                    }
                    else if (movementDirection == MovementDirection.Right && MainManager.Instance.playerStats.platformObject_Right)
                    {
                        moveToPos = MainManager.Instance.playerStats.platformObject_StandingOn_Current.transform.position + new Vector3(1, 0, 0);
                    }
                    else if (movementDirection == MovementDirection.Left && MainManager.Instance.playerStats.platformObject_Left)
                    {
                        moveToPos = MainManager.Instance.playerStats.platformObject_StandingOn_Current.transform.position + new Vector3(-1, 0, 0);
                    }
                    else
                    {
                        Raycasting();

                        movementState = MovementState.Still;
                        movementDirection = MovementDirection.None;
                    }
                }
                else
                {
                    Raycasting();

                    movementState = MovementState.Still;
                    movementDirection = MovementDirection.None;
                }

                finishMovement?.Invoke();
            }

            if (movementState == MovementState.Moving)
            {
                //Move Player
                switch (movementDirection)
                {
                    case MovementDirection.None:
                        break;

                    case MovementDirection.Forward:
                        gameObject.transform.position += Vector3.forward * MainManager.Instance.playerStats.movementSpeed * Time.deltaTime;
                        break;
                    case MovementDirection.Backward:
                        gameObject.transform.position += Vector3.back * MainManager.Instance.playerStats.movementSpeed * Time.deltaTime;
                        break;
                    case MovementDirection.Left:
                        gameObject.transform.position += Vector3.left * MainManager.Instance.playerStats.movementSpeed * Time.deltaTime;
                        break;
                    case MovementDirection.Right:
                        gameObject.transform.position += Vector3.right * MainManager.Instance.playerStats.movementSpeed * Time.deltaTime;
                        break;

                    default:
                        break;
                }

                Raycasting();
            }
        }
    }
    void SandMovement()
    {
        GrassMovement();
    }
    void HillMovement()
    {
        GrassMovement();
    }
    void MountainMovement()
    {
        GrassMovement();
    }
    void WaterMovement()
    {
        GrassMovement();
    }
    void LavaMovement()
    {
        GrassMovement();
    }

    void Raycasting()
    {
        detectorCenter.GetComponent<Detector>().PerformRaycast_Center();
        detectorFront.GetComponent<Detector>().PerformRaycast();
        detectorBack.GetComponent<Detector>().PerformRaycast();
        detectorRight.GetComponent<Detector>().PerformRaycast();
        detectorLeft.GetComponent<Detector>().PerformRaycast();
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
