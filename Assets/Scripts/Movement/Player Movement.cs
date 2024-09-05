using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static event Action takeAStep;

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

    private void Start()
    {
        movementState = MovementState.Still;
        movementDirection = MovementDirection.None;
    }
    void Update()
    {
        if (!MainManager.Instance.gamePaused)
        {
            KeyInputs();

            if (MainManager.Instance.playerStats.platformType_StandingOn == PlatformTypes.Grass)
            {
                GrassMovement();
            }
            else if (MainManager.Instance.playerStats.platformType_StandingOn == PlatformTypes.Ice)
            {
                IceMovement();
            }
        }
        
    }

    void KeyInputs()
    {
        //Set movement direction based on Key Input
        if (movementState == MovementState.Still)
        {
            if (Input.GetKey(KeyCode.W) && detectorFront.GetComponent<Detector>().canMoveFurther)
            {
                MainManager.Instance.playerStats.stepsLeft -= MainManager.Instance.playerStats.platformObject_StandingOn.GetComponent<Platform>().stepsCost;

                moveToPos = MainManager.Instance.playerStats.platformObject_StandingOn.transform.position + new Vector3(0, 0, 1);

                movementDirection = MovementDirection.Forward;
                movementState = MovementState.Moving;

                takeAStep?.Invoke();
            }
            if (Input.GetKey(KeyCode.S) && detectorBack.GetComponent<Detector>().canMoveFurther)
            {
                MainManager.Instance.playerStats.stepsLeft -= MainManager.Instance.playerStats.platformObject_StandingOn.GetComponent<Platform>().stepsCost;

                moveToPos = MainManager.Instance.playerStats.platformObject_StandingOn.transform.position + new Vector3(0, 0, -1);

                movementDirection = MovementDirection.Backward;
                movementState = MovementState.Moving;

                takeAStep?.Invoke();
            }
            if (Input.GetKey(KeyCode.D) && detectorRight.GetComponent<Detector>().canMoveFurther)
            {
                MainManager.Instance.playerStats.stepsLeft -= MainManager.Instance.playerStats.platformObject_StandingOn.GetComponent<Platform>().stepsCost;

                moveToPos = MainManager.Instance.playerStats.platformObject_StandingOn.transform.position + new Vector3(1, 0, 0);

                movementDirection = MovementDirection.Right;
                movementState = MovementState.Moving;

                takeAStep?.Invoke();
            }
            if (Input.GetKey(KeyCode.A) && detectorLeft.GetComponent<Detector>().canMoveFurther)
            {
                MainManager.Instance.playerStats.stepsLeft -= MainManager.Instance.playerStats.platformObject_StandingOn.GetComponent<Platform>().stepsCost;

                moveToPos = MainManager.Instance.playerStats.platformObject_StandingOn.transform.position + new Vector3(-1, 0, 0);

                movementDirection = MovementDirection.Left;
                movementState = MovementState.Moving;

                takeAStep?.Invoke();
            }
        }
    }

    void GrassMovement()
    {
        //When Moving
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

            Vector3 PlayerAltPos = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);

            print("1. Moving | MovePos: " + moveToPos + " | PlayerPos: " + PlayerAltPos + " | Distance: " + Vector3.Distance(PlayerAltPos, moveToPos));

            if (Vector3.Distance(PlayerAltPos, moveToPos) <= 0.05f)
            {
                print("1. Reached Destination");

                gameObject.transform.position = new Vector3(moveToPos.x, 0.3f, moveToPos.z);

                movementState = MovementState.Still;
            }
        }
    }
    void IceMovement()
    {
        //When Moving
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

            Vector3 PlayerAltPos = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);

            print("2. Moving | MovePos: " + moveToPos + " | PlayerPos: " + PlayerAltPos + " | Distance: " + Vector3.Distance(PlayerAltPos, moveToPos));

            if (Vector3.Distance(PlayerAltPos, moveToPos) <= 0.05f)
            {
                gameObject.transform.position = new Vector3(moveToPos.x, 0.3f, moveToPos.z);

                print("3. Reached Destination | Moving | MovePos: " + moveToPos + " | PlayerPos: " + gameObject.transform.position + " | Distance: " + Vector3.Distance(PlayerAltPos, moveToPos));

                if (MainManager.Instance.playerStats.platformType_StandingOn == PlatformTypes.Ice)
                {
                    detectorFront.GetComponent<Detector>().PerformRaycast();
                    detectorBack.GetComponent<Detector>().PerformRaycast();
                    detectorRight.GetComponent<Detector>().PerformRaycast();
                    detectorLeft.GetComponent<Detector>().PerformRaycast();

                    print("3. PlatformTypes.Ice");
                    if (movementDirection == MovementDirection.Forward && detectorFront.GetComponent<Detector>().canMoveFurther)
                    {
                        print("3. detectorForward.GetComponent<Detector>().canMoveFurther");
                        moveToPos = MainManager.Instance.playerStats.platformObject_StandingOn.transform.position + new Vector3(0, 0, 1);
                    }
                    else if (movementDirection == MovementDirection.Backward && detectorBack.GetComponent<Detector>().canMoveFurther)
                    {
                        print("3. detectorBackward.GetComponent<Detector>().canMoveFurther");
                        moveToPos = MainManager.Instance.playerStats.platformObject_StandingOn.transform.position + new Vector3(0, 0, -1);
                    }
                    else if (movementDirection == MovementDirection.Right && detectorRight.GetComponent<Detector>().canMoveFurther)
                    {
                        print("3. detectorRight.GetComponent<Detector>().canMoveFurther");
                        moveToPos = MainManager.Instance.playerStats.platformObject_StandingOn.transform.position + new Vector3(1, 0, 0);
                    }
                    else if (movementDirection == MovementDirection.Left && detectorLeft.GetComponent<Detector>().canMoveFurther)
                    {
                        print("3. detectorLeft.GetComponent<Detector>().canMoveFurther");
                        moveToPos = MainManager.Instance.playerStats.platformObject_StandingOn.transform.position + new Vector3(-1, 0, 0);
                    }
                    else
                    {
                        print("4. MovementState.Still");
                        movementState = MovementState.Still;
                    }
                }
                else
                {
                    print("5. MovementState.Still");
                    movementState = MovementState.Still;
                }
            }
        }
    }
}
