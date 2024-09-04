using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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
    [SerializeField] float movementSpeed = 0.1f;

    private void Start()
    {
        movementState = MovementState.Still;
        movementDirection = MovementDirection.None;
    }
    void Update()
    {
        //If Movement State has changed
        if (detectorFront.GetComponent<Detector>().stateHasChanged
            || detectorBack.GetComponent<Detector>().stateHasChanged
            || detectorLeft.GetComponent<Detector>().stateHasChanged
            || detectorRight.GetComponent<Detector>().stateHasChanged)
        {
            movementState = MovementState.Still;
            movementDirection = MovementDirection.None;

            detectorFront.GetComponent<Detector>().stateHasChanged = false;
            detectorBack.GetComponent<Detector>().stateHasChanged = false;
            detectorLeft.GetComponent<Detector>().stateHasChanged = false;
            detectorRight.GetComponent<Detector>().stateHasChanged = false;
        }

        //Set movement direction based on Key Input
        if (movementState == MovementState.Still)
        {
            if (Input.GetKeyDown(KeyCode.W) && detectorFront.GetComponent<Detector>().canMoveFurther)
            {
                movementDirection = MovementDirection.Forward;
                movementState = MovementState.Moving;
            }
            if (Input.GetKeyDown(KeyCode.S) && detectorBack.GetComponent<Detector>().canMoveFurther)
            {
                movementDirection = MovementDirection.Backward;
                movementState = MovementState.Moving;
            }
            if(Input.GetKeyDown(KeyCode.D) && detectorRight.GetComponent<Detector>().canMoveFurther)
            {
                movementDirection = MovementDirection.Right;
                movementState = MovementState.Moving;
            }
            if(Input.GetKeyDown(KeyCode.A) && detectorLeft.GetComponent<Detector>().canMoveFurther)
            {
                movementDirection = MovementDirection.Left;
                movementState = MovementState.Moving;
            }
        }

        //When Moving
        if (movementState == MovementState.Moving)
        {
            switch (movementDirection)
            {
                case MovementDirection.None:
                    break;

                case MovementDirection.Forward:
                    gameObject.transform.position += new Vector3(0, 0, movementSpeed);
                    break;
                case MovementDirection.Backward:
                    gameObject.transform.position += new Vector3(0, 0, -movementSpeed);
                    break;
                case MovementDirection.Left:
                    gameObject.transform.position += new Vector3(-movementSpeed, 0, 0);
                    break;
                case MovementDirection.Right:
                    gameObject.transform.position += new Vector3(movementSpeed, 0, 0);
                    break;

                default:
                    break;
            }
        }
    }
}
