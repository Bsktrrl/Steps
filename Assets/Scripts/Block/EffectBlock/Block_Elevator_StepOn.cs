using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Elevator_StepOn : MonoBehaviour
{
    public elevatorDirection elevatorDirection;
    public int distance;
    public float movementSpeed = 5f;

    Vector3 startPos;
    Vector3 endPos;

    bool moveToEndPos = true;

    public bool isStandingOnBlock;
    public bool isMoving;


    //--------------------


    private void Start()
    {
        CalculateMovementPath();
    }
    private void Update()
    {
        CheckIfPlayerIsOn();

        if (isMoving)
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


    void CheckIfPlayerIsOn()
    {
        if (MainManager.Instance.block_StandingOn_Current.block != gameObject && isStandingOnBlock)
        {
            isStandingOnBlock = false;
        }
        else if (MainManager.Instance.block_StandingOn_Current.block == gameObject && !isStandingOnBlock)
        {
            isStandingOnBlock = true;
            isMoving = true;
        }
    }
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
            case elevatorDirection.forward:
                endPos = startPos + (Vector3.forward * distance);
                break;
            case elevatorDirection.backward:
                endPos = startPos + (Vector3.back * distance);
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
            isMoving = false;
        }
    }
    void ElevatorMovement_ToStartPos()
    {
        transform.position = Vector3.MoveTowards(transform.position, startPos, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, startPos) <= 0.03f)
        {
            isMoving = false;
            moveToEndPos = true;
        }
    }
}