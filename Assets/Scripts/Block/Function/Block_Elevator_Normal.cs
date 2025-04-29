using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Elevator_Normal : MonoBehaviour
{
    public elevatorDirection elevatorDirection;
    public int distance;
    public float movementSpeed = 1f;
    public float waitingTime = 60f;

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

        yield return new WaitForSeconds(waitingTime * Time.deltaTime);

        waiting = false;
    }


    //--------------------


    void ResetBlock()
    {
        moveToEndPos = true;

        StartCoroutine(BlockWaiting());
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

                    print("1. CheckIfDarkenBlock | IsDark: " + gameObject.GetComponent<BlockInfo>().blockIsDark);
                }
            }
            else
            {
                if (!gameObject.GetComponent<BlockInfo>().blockIsDark)
                {
                    print("2. CheckIfDarkenBlock | IsDark: " + gameObject.GetComponent<BlockInfo>().blockIsDark);

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