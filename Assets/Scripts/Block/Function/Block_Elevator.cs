using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Elevator : MonoBehaviour
{
    [Header("Elevator Stats")]
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float waitingTime = 1f;

    [Header("Elevator Types")]
    [SerializeField] private bool activate_Elevator;
    [SerializeField] private bool stepOn_Elevator;

    [Header("Movement Path")]
    [SerializeField] private List<MovementPath> movementPath = new List<MovementPath>();

    [Header("Runtime")]
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool waiting = true;
    [SerializeField] private int pathSegmentCounter = 0;

    public bool elevatorIsActivated;

    [SerializeField] private bool hasCheckedForPlayer;
    private bool playerIsOn;

    private float updateBlocksCounter = 0f;
    private float accumulatedDistance = 0f;

    private Vector3 lastPosition;
    private Vector3 lastElevatorPosition;

    private MovingMachineScript movingMachineScript;
    private Block_Elevator_StepOn stepOnScript;
    private Block_Ladder ladderChild;

    private Transform playerTransform;

    [SerializeField] private float stepOnDelay = 0.25f;
    private float stepOnTimer = 0f;


    //--------------------


    private void Start()
    {
        stepOnScript = GetComponent<Block_Elevator_StepOn>();
        if (stepOnScript != null)
            stepOn_Elevator = true;

        movingMachineScript = GetComponent<MovingMachineScript>();
        ladderChild = GetComponentInChildren<Block_Ladder>();

        if (PlayerManager.Instance != null && PlayerManager.Instance.player != null)
            playerTransform = PlayerManager.Instance.player.transform;

        lastPosition = transform.position;
        lastElevatorPosition = transform.position;

        if (movementPath == null || movementPath.Count == 0)
        {
            Debug.LogError("Block_Elevator has no movement path assigned.", this);
            enabled = false;
            return;
        }

        CalculateMovementPath();
    }

    private void Update()
    {
        if (movementPath == null || movementPath.Count == 0)
            return;

        if (playerTransform == null && PlayerManager.Instance != null && PlayerManager.Instance.player != null)
            playerTransform = PlayerManager.Instance.player.transform;

        HandleElevatorMovement();
        UpdateBlocks();

        if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) <= 2f)
        {
            updateBlocksCounter += Time.deltaTime;
            if (updateBlocksCounter >= 0.05f)
            {
                updateBlocksCounter = 0f;

                if (Movement.Instance != null)
                {
                    Movement.Instance.UpdateBlocks();
                    Movement.Instance.SetDarkenBlocks();
                }

                if (Player_CeilingGrab.Instance != null)
                    Player_CeilingGrab.Instance.RaycastCeiling();
            }
        }
    }

    private void LateUpdate()
    {
        lastElevatorPosition = transform.position;
    }


    //--------------------


    private void OnEnable()
    {
        Movement.Action_RespawnPlayer += ResetBlock;
    }

    private void OnDisable()
    {
        Movement.Action_RespawnPlayer -= ResetBlock;
    }


    //--------------------


    void HandleElevatorMovement()
    {
        if (waiting || !isMoving)
        {
            StopAnimation();
            return;
        }

        bool shouldMove = false;

        if (stepOn_Elevator)
        {
            bool playerStandingOnThisBlock =
                Movement.Instance != null &&
                Movement.Instance.blockStandingOn == gameObject;

            if (playerStandingOnThisBlock)
                stepOnTimer += Time.deltaTime;
            else
                stepOnTimer = 0f;

            shouldMove = stepOnTimer >= stepOnDelay;
        }
        else if (activate_Elevator)
        {
            shouldMove = elevatorIsActivated;
        }
        else
        {
            shouldMove = true;
        }

        if (!shouldMove)
        {
            StopAnimation();
            return;
        }

        Vector3 positionBefore = transform.position;

        ElevatorMovement(pathSegmentCounter);

        bool actuallyMoved = (transform.position - positionBefore).sqrMagnitude > 0.000001f;

        if (actuallyMoved)
            StartAnimation();
        else
            StopAnimation();
    }

    void CalculateMovementPath()
    {
        for (int i = 0; i < movementPath.Count; i++)
        {
            movementPath[i].startPos = (i == 0) ? transform.position : movementPath[i - 1].endPos;

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

    void UpdateBlocks()
    {
        if (Movement.Instance == null || playerTransform == null)
            return;

        bool playerStandingOnThisBlock = Movement.Instance.blockStandingOn == gameObject;

        if (playerStandingOnThisBlock && !playerIsOn)
        {
            Movement.Instance.elevatorPos_Previous = transform.position;
            playerIsOn = true;
        }
        else if (!playerStandingOnThisBlock)
        {
            playerIsOn = false;
        }

        if (playerStandingOnThisBlock &&
            Movement.Instance.movementStates == MovementStates.Still &&
            Player_CeilingGrab.Instance != null &&
            !Player_CeilingGrab.Instance.isCeilingRotation_OFF)
        {
            Vector3 elevatorDelta = transform.position - lastElevatorPosition;

            if (elevatorDelta != Vector3.zero)
            {
                playerTransform.position += elevatorDelta;
            }

            float delta = elevatorDelta.magnitude;
            accumulatedDistance += delta;

            if (accumulatedDistance >= 0.1f)
            {
                accumulatedDistance = 0f;
                Movement.Instance.elevatorPos_Previous = transform.position;
                Movement.Instance.UpdateBlocks();
                Movement.Instance.SetDarkenBlocks();
            }

            lastPosition = transform.position;
        }
    }

    void ElevatorMovement(int index)
    {
        if (movementPath == null || movementPath.Count == 0)
            return;

        if (index < 0 || index >= movementPath.Count)
        {
            Debug.LogError($"Invalid path index {index}. movementPath count: {movementPath.Count}", this);
            return;
        }

        Vector3 target = movementPath[index].endPos;

        if ((transform.position - target).sqrMagnitude > 0.000001f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                movementSpeed * Time.deltaTime
            );
        }

        if (Vector3.Distance(transform.position, target) <= 0.03f)
        {
            pathSegmentCounter++;

            if (pathSegmentCounter >= movementPath.Count)
            {
                pathSegmentCounter = 0;

                if (ladderChild != null)
                    ladderChild.SetupLadder();
            }

            if (movementPath[index].waitAfterMoving)
            {
                isMoving = false;
                StopAnimation();
                StartCoroutine(BlockWaiting(waitingTime));
            }
        }
    }

    IEnumerator BlockWaiting(float waitTime)
    {
        waiting = true;
        StopAnimation();

        yield return new WaitForSeconds(waitTime);

        waiting = false;
        isMoving = true;
    }

    void StartAnimation()
    {
        if (movingMachineScript != null && !gameObject.GetComponent<Block_Elevator_StepOn>())
            movingMachineScript.StartMovement();
    }

    void StopAnimation()
    {
        if (movingMachineScript != null && !gameObject.GetComponent<Block_Elevator_StepOn>())
            movingMachineScript.StopMovement();
    }

    void CheckIfInRangeOfPlayer()
    {
        if (Movement.Instance == null || playerTransform == null)
            return;

        float distance = transform.position.y - playerTransform.position.y;

        if (Movement.Instance.blockStandingOn != gameObject &&
            Vector3.Distance(transform.position, playerTransform.position) <= 1.4f &&
            distance > -1f && distance < -0.9f)
        {
            Movement.Instance.UpdateBlocks();
            hasCheckedForPlayer = false;
        }
        else
        {
            if (!hasCheckedForPlayer)
            {
                Movement.Instance.UpdateBlocks();
            }

            hasCheckedForPlayer = true;
        }
    }

    void CheckIfDarkenBlock()
    {
        if (Player_CeilingGrab.Instance == null || playerTransform == null)
            return;

        BlockInfo blockInfo = GetComponent<BlockInfo>();
        if (blockInfo == null)
            return;

        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) >= 1.75f)
            {
                if (blockInfo.blockIsDark)
                {
                    blockInfo.ResetDarkenColor();
                }
            }
            else
            {
                if (!blockInfo.blockIsDark &&
                    ((transform.position.y < (playerTransform.position.y + 1) ||
                      PlayerStats.Instance.stats.abilitiesGot_Temporary.DrillBoots ||
                      PlayerStats.Instance.stats.abilitiesGot_Permanent.DrillBoots) ||
                     (transform.position.y < (playerTransform.position.y - 1) ||
                      PlayerStats.Instance.stats.abilitiesGot_Temporary.DrillHelmet ||
                      PlayerStats.Instance.stats.abilitiesGot_Permanent.DrillHelmet)))
                {
                    blockInfo.SetDarkenColors();
                }
            }
        }
        else
        {
            if (!hasCheckedForPlayer)
            {
                if (blockInfo.blockIsDark)
                {
                    blockInfo.blockIsDark = false;
                    blockInfo.ResetDarkenColor();
                }
            }
            else
            {
                if (hasCheckedForPlayer)
                {
                    blockInfo.SetDarkenColors();
                }
            }
        }
    }

    void ResetBlock()
    {
        StopAllCoroutines();

        isMoving = true;
        waiting = true;
        pathSegmentCounter = 0;
        stepOnTimer = 0f;

        transform.position = movementPath != null && movementPath.Count > 0
            ? movementPath[0].startPos
            : transform.position;

        lastPosition = transform.position;
        lastElevatorPosition = transform.position;
        accumulatedDistance = 0f;
        playerIsOn = false;

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