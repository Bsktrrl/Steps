using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Elevator : MonoBehaviour
{
    public elevatorDirection elevatorDirection;
    public int distance;
    public float movementSpeed = 5f;
    public float waitingTime = 2f;

    Vector3 startPos;
    Vector3 endPos;

    bool moveToEndPos;
    bool waiting;


    //--------------------


    private void Start()
    {
        CalculateMovementPath();
    }
    private void Update()
    {
        if (!waiting)
        {
            //Moving towards endPos
            if (moveToEndPos)
            {
                ElevatorMovement_ToEndPos();
            }

            //Moving towards startPos
            else
            {
                ElevatorMovement_ToStartPos();
            }
        }
    }


    //--------------------


    void CalculateMovementPath()
    {
        startPos = transform.position;

        switch (elevatorDirection)
        {
            case elevatorDirection.None:
                endPos = startPos;
                break;
            case elevatorDirection.Up:
                endPos = startPos + (Vector3.up * distance);
                break;
            case elevatorDirection.Down:
                endPos = startPos + (Vector3.down * distance);
                break;
            case elevatorDirection.Left:
                endPos = startPos + (Vector3.left * distance);
                break;
            case elevatorDirection.Right:
                endPos = startPos + (Vector3.right * distance);
                break;

            default:
                endPos = startPos;
                break;
        }
    }


    //--------------------


    void ElevatorMovement_ToEndPos()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPos, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, endPos) <= 0.03f)
        {
            moveToEndPos = false;

            StartCoroutine(BlockWaiting());
        }
    }
    void ElevatorMovement_ToStartPos()
    {
        transform.position = Vector3.MoveTowards(transform.position, startPos, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, startPos) <= 0.03f)
        {
            moveToEndPos = true;

            StartCoroutine(BlockWaiting());
        }
    }

    IEnumerator BlockWaiting()
    {
        waiting = true;

        yield return new WaitForSeconds(waitingTime);

        waiting = false;
    }
}

public enum elevatorDirection
{
    None,

    Up,
    Down,
    Left,
    Right,
}