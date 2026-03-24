using UnityEngine;

public class Block_Moveable : MonoBehaviour
{
    private MovementDirection movementDirection = MovementDirection.None;

    public bool canMove;
    [SerializeField] private bool isMoving;
    [SerializeField] private bool isIceGliding;

    private Vector3 startPos;
    private Vector3 savePos;

    private BlockInfo blockInfo;

    private const float GRID_SIZE = 1f;
    private const float SNAP_DISTANCE = 0.03f;
    private const float DOWN_RAY_START_OFFSET = 0.25f;
    private const float DOWN_RAY_DISTANCE = 1f;
    private const float FORWARD_CHECK_DISTANCE = 1f;


    //--------------------


    private void Start()
    {
        startPos = transform.position;
        savePos = transform.position;
        blockInfo = GetComponent<BlockInfo>();

        RefreshMovementState();
    }

    private void Update()
    {
        if (!isMoving) return;

        float speed = isIceGliding ? 10f : blockInfo.movementSpeed;
        PerformMovement(speed);
    }


    //--------------------


    private void OnEnable()
    {
        Movement.Action_StepTaken += RefreshMovementState;
        Movement.Action_BodyRotated += RefreshMovementState;
        Movement.Action_RespawnPlayer += ResetBlockPos;
        Movement.Action_LandedFromFalling += RefreshMovementState;

        Player_KeyInputs.Action_InteractButton_isPressed += ActivateBlockMovement;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken -= RefreshMovementState;
        Movement.Action_BodyRotated -= RefreshMovementState;
        Movement.Action_RespawnPlayer -= ResetBlockPos;
        Movement.Action_LandedFromFalling -= RefreshMovementState;

        Player_KeyInputs.Action_InteractButton_isPressed -= ActivateBlockMovement;
    }


    //--------------------


    private void RefreshMovementState()
    {
        if (isMoving) return;

        movementDirection = GetPushDirectionFromPlayer();
        canMove = movementDirection != MovementDirection.None && CanMoveOneStep();

        Movement.Instance.UpdateAvailableMovementBlocks();
    }

    private MovementDirection GetPushDirectionFromPlayer()
    {
        Vector3 lookDir = Movement.Instance.lookingDirection;

        if (lookDir == Vector3.forward && IsPlayerAdjacent(-lookDir))
            return MovementDirection.Backward;

        if (lookDir == Vector3.back && IsPlayerAdjacent(-lookDir))
            return MovementDirection.Forward;

        if (lookDir == Vector3.left && IsPlayerAdjacent(-lookDir))
            return MovementDirection.Right;

        if (lookDir == Vector3.right && IsPlayerAdjacent(-lookDir))
            return MovementDirection.Left;

        return MovementDirection.None;
    }

    private bool IsPlayerAdjacent(Vector3 directionToPlayer)
    {
        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, GRID_SIZE, MapManager.Instance.playerExclusive_LayerMask))
        {
            return hit.collider != null && hit.collider.gameObject == PlayerManager.Instance.player;
        }

        return false;
    }

    private Vector3 GetMoveVector()
    {
        switch (movementDirection)
        {
            case MovementDirection.Forward:
                return Vector3.back;
            case MovementDirection.Backward:
                return Vector3.forward;
            case MovementDirection.Left:
                return Vector3.right;
            case MovementDirection.Right:
                return Vector3.left;
            default:
                return Vector3.zero;
        }
    }

    private bool CanMoveOneStep()
    {
        Vector3 moveDir = GetMoveVector();
        if (moveDir == Vector3.zero) return false;

        Vector3 targetPos = savePos + moveDir;

        if (HasBlockingObjectInFront(moveDir))
            return false;

        if (!HasSupportBelow(targetPos))
            return false;

        return true;
    }

    private bool HasBlockingObjectInFront(Vector3 moveDir)
    {
        if (Physics.Raycast(transform.position, moveDir, out RaycastHit hit, FORWARD_CHECK_DISTANCE))
        {
            if (hit.transform != null && hit.transform.GetComponent<BlockInfo>() != null)
            {
                Debug.DrawRay(transform.position, moveDir * FORWARD_CHECK_DISTANCE, Color.red, 0.5f);
                return true;
            }
        }

        Debug.DrawRay(transform.position, moveDir * FORWARD_CHECK_DISTANCE, Color.green, 0.5f);
        return false;
    }

    private bool HasSupportBelow(Vector3 worldPos)
    {
        Vector3 rayOrigin = worldPos + Vector3.up * DOWN_RAY_START_OFFSET;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, DOWN_RAY_DISTANCE))
        {
            if (hit.transform != null && hit.transform.GetComponent<BlockInfo>() != null && hit.transform.GetComponent<BlockInfo>().blockType != BlockType.Stair && hit.transform.GetComponent<BlockInfo>().blockType != BlockType.Slope)
            {
                Debug.DrawRay(rayOrigin, Vector3.down * DOWN_RAY_DISTANCE, Color.green, 0.5f);
                return true;
            }
        }

        Debug.DrawRay(rayOrigin, Vector3.down * DOWN_RAY_DISTANCE, Color.red, 0.5f);
        return false;
    }

    private bool IsIceBelow(Vector3 worldPos)
    {
        Vector3 rayOrigin = worldPos + Vector3.up * DOWN_RAY_START_OFFSET;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, DOWN_RAY_DISTANCE, MapManager.Instance.pickup_LayerMask))
        {
            return hit.transform != null && hit.transform.GetComponent<Block_IceGlide>() != null;
        }

        return false;
    }

    private void ActivateBlockMovement()
    {
        if (isMoving) return;
        if (movementDirection == MovementDirection.None) return;

        RefreshMovementState();
        if (!canMove) return;

        isMoving = true;
        isIceGliding = false;
    }

    private void PerformMovement(float movementSpeed)
    {
        Vector3 moveDir = GetMoveVector();
        if (moveDir == Vector3.zero)
        {
            StopMovement();
            return;
        }

        Vector3 targetPos = savePos + moveDir;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            movementSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) <= SNAP_DISTANCE)
        {
            transform.position = targetPos;
            savePos = targetPos;

            if (ShouldContinueIceGlide())
            {
                isMoving = true;
                isIceGliding = true;
            }
            else
            {
                StopMovement();
                RefreshMovementState();
            }
        }
    }

    private bool ShouldContinueIceGlide()
    {
        Vector3 moveDir = GetMoveVector();
        if (moveDir == Vector3.zero)
            return false;

        // Must currently be standing on ice.
        if (!IsIceBelow(savePos))
            return false;

        Vector3 nextPos = savePos + moveDir;

        // Next step must be valid.
        if (HasBlockingObjectInFront(moveDir))
            return false;

        if (!HasSupportBelow(nextPos))
            return false;

        // Only continue gliding if the NEXT tile is also ice.
        if (!IsIceBelow(nextPos))
            return false;

        return true;
    }

    private void StopMovement()
    {
        isMoving = false;
        isIceGliding = false;
        canMove = false;

        Movement.Instance.UpdateAvailableMovementBlocks();
    }

    private void ResetBlockPos()
    {
        transform.position = startPos;
        savePos = startPos;

        StopMovement();
        RefreshMovementState();
    }
}