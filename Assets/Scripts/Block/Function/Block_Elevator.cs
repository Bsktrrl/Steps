using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Block_Elevator : MonoBehaviour
{
    [Header("Elevator Stats")]
    public float movementSpeed = 1;
    public float waitingTime = 1;

    [Header("Movement Path")]
    [SerializeField] List<MovementPath> movementPath;

    [SerializeField] bool moveToPos;
    [SerializeField] bool waiting = true;
    [SerializeField] int pathSegmentCounter = 0;


    //--------------------


    private void Start()
    {
        CalculateMovementPath();
    }
    private void Update()
    {
        //Moving towards new endPos
        if (!waiting && moveToPos)
        {
            ElevatorMovement(pathSegmentCounter);
        }

        CheckIfDarkenBlock();
    }
    private void OnEnable()
    {
        PlayerStats.Action_RespawnPlayer += ResetBlock;
    }

    private void OnDisable()
    {
        PlayerStats.Action_RespawnPlayer -= ResetBlock;
    }


    //--------------------


    void CalculateMovementPath()
    {
        //Set Movement path to be used by the elevator
        for (int i = 0; i < movementPath.Count; i++)
        {
            //Set new startPos
            if (i <= 0)
            {
                movementPath[i].startPos = transform.position;
            }
            else
            {
                movementPath[i].startPos = movementPath[i - 1].endPos;
            }

            //Set new endPos
            switch (movementPath[i].elevatorDirection)
            {
                case elevatorDirection.None:
                    movementPath[i].endPos = movementPath[i].startPos;
                    break;
                case elevatorDirection.Up:
                    movementPath[i].endPos = movementPath[i].startPos + (Vector3.up * movementPath[i].distance);
                    break;
                case elevatorDirection.Down:
                    movementPath[i].endPos = movementPath[i].startPos + (Vector3.down * movementPath[i].distance);
                    break;
                case elevatorDirection.forward:
                    movementPath[i].endPos = movementPath[i].startPos + (Vector3.forward * movementPath[i].distance);
                    break;
                case elevatorDirection.backward:
                    movementPath[i].endPos = movementPath[i].startPos + (Vector3.back * movementPath[i].distance);
                    break;
                case elevatorDirection.Left:
                    movementPath[i].endPos = movementPath[i].startPos + (Vector3.left * movementPath[i].distance);
                    break;
                case elevatorDirection.Right:
                    movementPath[i].endPos = movementPath[i].startPos + (Vector3.right * movementPath[i].distance);
                    break;

                default:
                    movementPath[i].endPos = movementPath[i].startPos;
                    break;
            }
        }

        waiting = false;
        moveToPos = true;
    }


    //--------------------


    void ElevatorMovement(int index)
    {
        transform.position = Vector3.MoveTowards(transform.position, movementPath[index].endPos, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movementPath[index].endPos) <= 0.03f)
        {
            pathSegmentCounter++;
            if (pathSegmentCounter > movementPath.Count - 1)
                pathSegmentCounter = 0;

            if (movementPath[index].waitAfterMoving)
            {
                moveToPos = false;

                StartCoroutine(BlockWaiting(waitingTime));
            }
        }
    }

    IEnumerator BlockWaiting(float waitTime)
    {
        waiting = true;

        yield return new WaitForSeconds(waitTime);

        waiting = false;
        moveToPos = true;
    }


    //--------------------


    void ResetBlock()
    {
        moveToPos = true;

        StartCoroutine(BlockWaiting(waitingTime));
    }


    //--------------------


    void CheckIfDarkenBlock()
    {
        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            if (Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position) >= 1.75f)
            {
                if (gameObject.GetComponent<BlockInfo>().blockIsDark)
                {
                    gameObject.GetComponent<BlockInfo>().ResetDarkenColor();
                }
            }
            else
            {
                if (!gameObject.GetComponent<BlockInfo>().blockIsDark)
                {
                    gameObject.GetComponent<BlockInfo>().SetDarkenColors();
                }
            }   
        }
        else
        {
            if (Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position) >= 1.4f)
            {
                if (gameObject.GetComponent<BlockInfo>().blockIsDark)
                {
                    //print("1. CheckIfDarkenBlock | Distance: " + Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position));

                    gameObject.GetComponent<BlockInfo>().blockIsDark = false;

                    gameObject.GetComponent<BlockInfo>().ResetDarkenColor();
                }
            }
            else
            {
                if (!gameObject.GetComponent<BlockInfo>().blockIsDark)
                {
                    //print("3. CheckIfDarkenBlock | Distance: " + Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position));

                    gameObject.GetComponent<BlockInfo>().SetDarkenColors();
                }
            }
        }
    }
}

public enum elevatorDirection
{
    None,

    Up,
    Down,
    forward,
    backward,
    Left,
    Right,
}

[Serializable]
public class MovementPath
{
    public elevatorDirection elevatorDirection;
    public int distance;
    public bool waitAfterMoving;

    public Vector3 startPos;
    public Vector3 endPos;
}