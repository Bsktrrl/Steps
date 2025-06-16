using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Block_Elevator : MonoBehaviour
{
    [Header("Elevator Stats")]
    public float movementSpeed = 2;
    public float waitingTime = 1;

    [Header("Elevator Types")]
    public bool activate_Elevator;
    public bool stepOn_Elevator;

    [Header("Movement Path")]
    [SerializeField] List<MovementPath> movementPath;

    [SerializeField] bool isMoving = false;
    [SerializeField] bool waiting = true;
    [SerializeField] int pathSegmentCounter = 0;

    public bool elevatorIsActivated;

    private Vector3 lastPosition;
    private float accumulatedDistance = 0f;

    [SerializeField] bool hasCheckedForPlayer;
    bool playerIsOn;

    float UpdateBlocksCounter = 0;


    //--------------------


    private void Start()
    {
        if (gameObject.GetComponent<Block_Elevator_StepOn>())
            stepOn_Elevator = true;
        
        lastPosition = transform.position;

        CalculateMovementPath();
    }
    private void Update()
    {
        //Moving towards new endPos
        if (!waiting && isMoving)
        {
            if (stepOn_Elevator)
            {
                if (gameObject.GetComponent<Block_Elevator_StepOn>().isStandingOnBlock)
                {
                    ElevatorMovement(pathSegmentCounter);
                }
            }
            else if (activate_Elevator)
            {
                if (elevatorIsActivated)
                {
                    ElevatorMovement(pathSegmentCounter);
                }
            }
            else
            {
                ElevatorMovement(pathSegmentCounter);
            }
        }

        //Check distance before updating DarkenBlocks

        //CheckIfInRangeOfPlayer();
        //CheckIfDarkenBlock();
        UpdateBlocks();

        if (Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position) <= 2f)
        {
            UpdateBlocksCounter += Time.deltaTime;
            if (UpdateBlocksCounter > 0.05f)
            {
                UpdateBlocksCounter = 0;
                Movement.Instance.UpdateBlocks();
                Movement.Instance.SetDarkenBlocks();
            }
        }
    }
    private void OnEnable()
    {
        Movement.Action_RespawnPlayer += ResetBlock;
    }

    private void OnDisable()
    {
        Movement.Action_RespawnPlayer -= ResetBlock;
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
    }


    //--------------------


    void CheckIfInRangeOfPlayer()
    {
        //if (Movement.Instance.blockStandingOn == gameObject)
        //{
        //    print("1. CheckIfInRangeOfPlayer");
        //}

        float distance = (transform.position.y - PlayerManager.Instance.player.transform.position.y);
        //print("Abs: Elevator: " + transform.position.y + " | Player: " + PlayerManager.Instance.player.transform.position.y + " | Distance: " + distance);

        if (Movement.Instance.blockStandingOn != gameObject && (Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position) <= 1.4f) && (distance > -1f && distance < -0.9f) /*&& !hasCheckedForPlayer*/)
        {
            //print("2. CheckIfInRangeOfPlayer | Elevator: " + transform.position.y + " | Player: " + PlayerManager.Instance.player.transform.position.y + " | Distance: " + distance);
            Movement.Instance.UpdateBlocks();
            hasCheckedForPlayer = false;
        }
        else /*if (Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position) > 1.4f)*/
        {
            if (!hasCheckedForPlayer)
            {
                Movement.Instance.UpdateBlocks();
            }

            hasCheckedForPlayer = true;
        }

        //else
        //{
        //    print("3. CheckIfInRangeOfPlayer");
        //    Movement.Instance.UpdateBlocks();
        //    hasCheckedForPlayer = true;
        //}
    }
    void UpdateBlocks()
    {
        //Make a check when the player enters this block
        if (Movement.Instance.blockStandingOn == gameObject && !playerIsOn)
        {
            Movement.Instance.elevatorPos_Previous = transform.position;
            playerIsOn = true;
        }
        else if (Movement.Instance.blockStandingOn != gameObject)
        {
            playerIsOn = false;
        }

        //Update blocks underway
        if (Movement.Instance.blockStandingOn == gameObject && Movement.Instance.movementStates == MovementStates.Still)
        {
            PlayerManager.Instance.player.transform.position = transform.position + (Vector3.up * Movement.Instance.heightOverBlock);

            float delta = Vector3.Distance(transform.position, lastPosition);
            accumulatedDistance += delta;

            if (accumulatedDistance >= 0.1f /*Vector3.Distance(Movement.Instance.elevatorPos_Previous, transform.position) >= 1f*/)
            {
                accumulatedDistance = 0;
                Movement.Instance.elevatorPos_Previous = transform.position;
                Movement.Instance.UpdateBlocks();
                Movement.Instance.SetDarkenBlocks();
            }

            lastPosition = transform.position;
        }
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
                isMoving = false;

                StartCoroutine(BlockWaiting(waitingTime));
            }
        }
    }

    IEnumerator BlockWaiting(float waitTime)
    {
        waiting = true;

        yield return new WaitForSeconds(waitTime);

        waiting = false;
        isMoving = true;
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
                if (!gameObject.GetComponent<BlockInfo>().blockIsDark
                    && ((gameObject.transform.position.y < (PlayerManager.Instance.player.transform.position.y + 1) || PlayerStats.Instance.stats.abilitiesGot_Temporary.Descend || PlayerStats.Instance.stats.abilitiesGot_Permanent.Descend)
                    || (gameObject.transform.position.y < (PlayerManager.Instance.player.transform.position.y - 1) || PlayerStats.Instance.stats.abilitiesGot_Temporary.Ascend || PlayerStats.Instance.stats.abilitiesGot_Permanent.Ascend)))
                {
                    gameObject.GetComponent<BlockInfo>().SetDarkenColors();
                }
            }   
        }
        else
        {
            if (!hasCheckedForPlayer /*(Movement.Instance.blockStandingOn == gameObject && Movement.Instance.movementStates == MovementStates.Still) || Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position) >= 1.4f*/)
            {
                if (gameObject.GetComponent<BlockInfo>().blockIsDark)
                {
                    gameObject.GetComponent<BlockInfo>().blockIsDark = false;
                    gameObject.GetComponent<BlockInfo>().ResetDarkenColor();
                }
            }
            else
            {
                if (hasCheckedForPlayer /*&&
                    !gameObject.GetComponent<BlockInfo>().blockIsDark && Movement.Instance.movementStates == MovementStates.Still 
                    && ((gameObject.transform.position.y < (PlayerManager.Instance.player.transform.position.y + 1) || PlayerStats.Instance.stats.abilitiesGot_Temporary.Descend || PlayerStats.Instance.stats.abilitiesGot_Permanent.Descend)
                    || (gameObject.transform.position.y < (PlayerManager.Instance.player.transform.position.y - 1) || PlayerStats.Instance.stats.abilitiesGot_Temporary.Ascend || PlayerStats.Instance.stats.abilitiesGot_Permanent.Ascend))*/)
                {
                    gameObject.GetComponent<BlockInfo>().SetDarkenColors();
                }
            }
        }
    }


    //--------------------


    void ResetBlock()
    {
        isMoving = true;
        pathSegmentCounter = 0;

        StartCoroutine(BlockWaiting(waitingTime));
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

    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public Vector3 endPos;
}