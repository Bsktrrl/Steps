using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_SandFalling : MonoBehaviour
{
    [Header("Falling Parameters")]
    float waitTime_BeforeFalling = 0.75f;
    float waitCounter;

    [Header("Checked If Stepped On")]
    [SerializeField] bool isSteppedOn;
    [SerializeField] bool canFall;
    [SerializeField] bool resettingBlock;

    // Separates the warning stage from a block that is already falling.
    bool hasStartedFalling;

    // While part of a carried pillar, its own warning and falling logic pause.
    private bool carriedByMoveablePillar;

    // A moveable block can trigger the same warning as the player.
    // Keep a separate reference only while the warning/fall is active.
    Block_Moveable waitingMoveableBlock;
    Block_Moveable carriedMoveableBlock;

    [Header("Runtime Stats")]
    Vector3 endPos;

    [Header("FallingAnimation")]
    float shakingIntensity = 3;
    float shakingSpeed = 50;
    [SerializeField] List<GameObject> LOD_ObjectsList;
    Quaternion objectInitialRotation;

    [Header("FallingSandObject")]
    [SerializeField] GameObject sandBlockAsset;

    RaycastHit hit;


    //--------------------


    private void Start()
    {
        //GetLODObjects();

        // The warning meshes return to this rotation when a fall is cancelled.
        objectInitialRotation = transform.rotation;

        CheckIfCanFall();

        // Get the lowest fallback position if the block never lands.
        endPos = transform.position + (Vector3.down * 15);
    }

    private void Update()
    {
        if (!canFall)
            return;

        Movement movement = Movement.Instance;

        // Cancel the warning ONLY on the block the player is descending from.
        // Other falling blocks must keep their own warning/falling behavior.
        // currentMoveStartBlock is fixed at the start of the movement, unlike
        // blockStandingOn, which can change while the player descends.
        if (!hasStartedFalling && movement != null && movement.isDescending &&
            movement.currentMoveStartBlock == gameObject)
        {
            if (isSteppedOn)
                CancelFallingAttempt();

            return;
        }

        // An already-falling block also must not continue into the player.
        // Only interrupt a fall when the player is actually below it in the
        // same grid column (or actively descending through that column).
        if (hasStartedFalling &&
            (IsPlayerBelowBlock() ||
             (movement != null && movement.isDescending && IsPlayerInSameColumn())))
        {
            CancelFallingAttempt(resetFallingPosition: true);
            return;
        }

        if (!hasStartedFalling && CheckIfReadyToFall())
        {
            // The space underneath may have changed DURING the warning.
            // Do not use the canFall result calculated in Start().
            if (IsPlayerBelowBlock() || HasBlockingBlockBelow())
            {
                CancelFallingAttempt();
                return;
            }

            hasStartedFalling = true;

            // Carry the moveable block only if it is STILL directly above
            // when the warning ends. It may have been pushed away meanwhile.
            if (waitingMoveableBlock != null &&
                IsMoveableDirectlyAbove(waitingMoveableBlock) &&
                waitingMoveableBlock.BeginFallingWithSandBlock())
            {
                carriedMoveableBlock = waitingMoveableBlock;
            }
            waitingMoveableBlock = null;

            GetComponent<BlockInfo>().movementState = MovementStates.Falling;

            if (gameObject == movement.blockStandingOn)
            {
                movement.StartFallingWithBlock();
            }
        }

        if (hasStartedFalling)
            Falling();
    }


    //--------------------


    private void OnEnable()
    {
        Movement.Action_StepTaken += CheckIfStandingOn;
        Player_CeilingGrab.Action_isCeilingGrabbing_Finished += CheckIfStandingOn;
        Movement.Action_LandedFromFalling += CheckIfStandingOn;
        Movement.Action_RespawnPlayerEarly += ResetBlock;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken -= CheckIfStandingOn;
        Player_CeilingGrab.Action_isCeilingGrabbing_Finished -= CheckIfStandingOn;
        Movement.Action_LandedFromFalling -= CheckIfStandingOn;
        Movement.Action_RespawnPlayerEarly -= ResetBlock;

        // Also release the moveable if this block is disabled externally.
        ReleaseCarriedMoveableBlock();
        waitingMoveableBlock = null;
    }


    //--------------------


    void GetLODObjects()
    {
        LOD_ObjectsList = new List<GameObject>();

        foreach (Transform child in transform)
        {
            if (child.GetComponent<MeshRenderer>() != null)
            {
                LOD_ObjectsList.Add(child.gameObject);
            }

            objectInitialRotation = transform.rotation;
        }
    }


    //--------------------


    void CheckIfCanFall()
    {
        if (carriedByMoveablePillar)
        {
            canFall = false;
            SetFallingSandEnabled(false);
            return;
        }

        // Check if the block has another block above itself.
        if (Physics.Raycast(
                transform.position,
                Vector3.up,
                out hit,
                1,
                MapManager.Instance.pickup_LayerMask))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                canFall = false;
                SetFallingSandEnabled(false);
                return;
            }
        }

        // Check if the block has another block under itself.
        if (GetComponent<BlockInfo>().blockType == BlockType.Stair)
        {
            if (Physics.Raycast(
                    transform.position + (Vector3.up * 0.5f),
                    Vector3.down,
                    out hit,
                    1,
                    MapManager.Instance.pickup_LayerMask))
            {
                if (hit.transform.gameObject.GetComponent<BlockInfo>())
                {
                    canFall = false;
                    SetFallingSandEnabled(false);
                    return;
                }
            }
        }
        else
        {
            if (Physics.Raycast(
                    transform.position,
                    Vector3.down,
                    out hit,
                    1,
                    MapManager.Instance.pickup_LayerMask))
            {
                BlockInfo blockBelow =
                    hit.transform.gameObject.GetComponent<BlockInfo>();

                if (blockBelow != null)
                {
                    if (blockBelow.blockType == BlockType.Slab)
                    {
                        canFall = true;
                        SetFallingSandEnabled(true);
                        return;
                    }

                    canFall = false;
                    SetFallingSandEnabled(false);
                    return;
                }
            }
        }

        canFall = true;
        SetFallingSandEnabled(true);
    }

    void SetFallingSandEnabled(bool enabled)
    {
        if (sandBlockAsset == null)
            return;

        FallingSandScript fallingSand =
            sandBlockAsset.GetComponent<FallingSandScript>();

        if (fallingSand != null)
        {
            fallingSand.enabled = enabled;
        }
    }


    //--------------------
    // Moveable blocks triggering / riding falling sand
    //--------------------

    // Called by Block_Moveable for carried columns and detached columns.
    // Do not change the original canFall conditions permanently: reevaluate
    // them at the new position when this block is released or has landed.
    public void SetCarriedByMoveablePillar(bool carried)
    {
        if (carriedByMoveablePillar == carried)
            return;

        carriedByMoveablePillar = carried;
        if (carried)
        {
            if (isSteppedOn && !hasStartedFalling)
                CancelFallingAttempt();
            canFall = false;
            SetFallingSandEnabled(false);
        }
        else if (!resettingBlock)
        {
            CheckIfCanFall();
        }
    }

    // Called ONLY after a moveable block has completed a push or landing.
    public void TriggerFromMoveableBlock(Block_Moveable moveable)
    {
        if (!canFall || resettingBlock || hasStartedFalling || isSteppedOn ||
            moveable == null || !IsMoveableDirectlyAbove(moveable))
            return;

        // Keep the previous safeguard for the player's Descend ability.
        if (Movement.Instance != null && Movement.Instance.isDescending)
            return;

        waitingMoveableBlock = moveable;
        isSteppedOn = true; // Reuse the existing 0.75-second shake and timer.
    }

    bool IsMoveableDirectlyAbove(Block_Moveable moveable)
    {
        if (moveable == null || !moveable.isActiveAndEnabled)
            return false;

        Vector3 difference = moveable.transform.position - transform.position;
        const float tolerance = 0.15f;
        return Mathf.Abs(difference.x) < tolerance &&
               Mathf.Abs(difference.z) < tolerance &&
               Mathf.Abs(difference.y - 1f) < tolerance;
    }

    void MoveCarriedMoveableBlock(Vector3 offset)
    {
        if (carriedMoveableBlock != null)
            carriedMoveableBlock.FollowFallingSandBlock(offset);
    }

    void ReleaseCarriedMoveableBlock()
    {
        if (carriedMoveableBlock != null)
            carriedMoveableBlock.EndFallingWithSandBlock();

        carriedMoveableBlock = null;
    }


    //--------------------
    // Safety checks performed immediately before the falling animation starts.
    //--------------------

    bool HasBlockingBlockBelow()
    {
        // Match the existing CheckIfCanFall raycast and Slab exception.
        bool isStair = GetComponent<BlockInfo>().blockType == BlockType.Stair;
        Vector3 origin = transform.position +
                         (isStair ? Vector3.up * 0.5f : Vector3.zero);

        if (!Physics.Raycast(origin, Vector3.down, out RaycastHit belowHit,
                1f, MapManager.Instance.pickup_LayerMask))
            return false;

        BlockInfo blockBelow = belowHit.transform.GetComponent<BlockInfo>();
        if (blockBelow == null)
            return false;

        // Slabs were already allowed in the original falling rule.
        return isStair || blockBelow.blockType != BlockType.Slab;
    }

    bool IsPlayerInSameColumn()
    {
        if (PlayerManager.Instance == null ||
            PlayerManager.Instance.player == null)
            return false;

        Vector3 playerPos = PlayerManager.Instance.player.transform.position;
        const float gridTolerance = 0.45f;

        return Mathf.Abs(playerPos.x - transform.position.x) < gridTolerance &&
               Mathf.Abs(playerPos.z - transform.position.z) < gridTolerance;
    }

    bool IsPlayerBelowBlock()
    {
        if (!IsPlayerInSameColumn())
            return false;

        float playerY = PlayerManager.Instance.player.transform.position.y;
        float distanceBelow = transform.position.y - playerY;

        // The player's root is approximately one unit above its support.
        // Include the possible Descend landing space, but not a player who
        // is standing ON TOP of this falling block.
        float checkDepth = Movement.Instance != null
            ? Movement.Instance.ascendDescend_Distance + 1f
            : 3f;

        return distanceBelow >= -0.1f && distanceBelow <= checkDepth;
    }

    void RestoreWarningRotation()
    {
        for (int i = 0; i < LOD_ObjectsList.Count; i++)
        {
            if (LOD_ObjectsList[i] != null)
            {
                LOD_ObjectsList[i].transform.SetPositionAndRotation(
                    LOD_ObjectsList[i].transform.position,
                    objectInitialRotation);
            }
        }
    }

    void CancelFallingAttempt(bool resetFallingPosition = false)
    {
        // If the block was already in mid-air, return it to its starting
        // position instead of leaving it suspended in the player's space.
        Vector3 oldPosition = transform.position;
        if (resetFallingPosition)
            transform.position = GetComponent<BlockInfo>().startPos;

        MoveCarriedMoveableBlock(transform.position - oldPosition);
        ReleaseCarriedMoveableBlock();
        waitingMoveableBlock = null;

        hasStartedFalling = false;
        isSteppedOn = false;
        waitCounter = 0f;
        GetComponent<BlockInfo>().movementState = MovementStates.Still;
        RestoreWarningRotation();

        // Keep canFall unchanged: a later step onto the block is allowed to
        // trigger the warning again, followed by another fresh safety check.
    }


    //--------------------


    void CheckIfStandingOn()
    {
        if (!canFall)
            return;

        // Ignore the stale ceiling block while the player is rotating
        // out of CeilingGrab.
        if (Player_CeilingGrab.Instance.isCeilingRotation)
            return;

        if (Movement.Instance.blockStandingOn == gameObject &&
            !isSteppedOn &&
            !Player_CeilingGrab.Instance.isCeilingGrabbing &&
            Movement.Instance.GetMovementState() != MovementStates.Falling)
        {
            waitingMoveableBlock = null; // The player is now the trigger.
            isSteppedOn = true;
        }
    }

    bool CheckIfReadyToFall()
    {
        if (resettingBlock)
            isSteppedOn = false;

        if (isSteppedOn && !resettingBlock)
        {
            FallingAlertAnimation();

            if (waitCounter < waitTime_BeforeFalling)
                waitCounter += Time.deltaTime;

            if (waitCounter >= waitTime_BeforeFalling)
            {
                return true;
            }
        }

        return false;
    }

    void Falling()
    {
        if (resettingBlock)
        {
            isSteppedOn = false;
            return;
        }

        float fallDistance =
            PlayerManager.Instance.player.GetComponent<Movement>().fallSpeed *
            Time.deltaTime;

        // Stop on the first block encountered underneath instead of
        // continuing through it.
        Vector3 oldPosition = transform.position;
        if (TryLandOnBlockBelow(fallDistance))
        {
            MoveCarriedMoveableBlock(transform.position - oldPosition);
            LandBlock();
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            endPos,
            fallDistance);
        MoveCarriedMoveableBlock(transform.position - oldPosition);

        // The block still disappears if it falls out of the playable area
        // without finding another block.
        if (Vector3.Distance(transform.position, endPos) <= 0.03f)
        {
            GetComponent<BlockInfo>().movementState = MovementStates.Still;

            ReleaseCarriedMoveableBlock();
            waitingMoveableBlock = null;
            HideBlock();

            hasStartedFalling = false;
            isSteppedOn = false;
            waitCounter = 0;
            transform.position = GetComponent<BlockInfo>().startPos;
        }
    }

    bool TryLandOnBlockBelow(float fallDistance)
    {
        Collider fallingCollider = GetComponent<Collider>();

        if (fallingCollider == null)
            return false;

        Bounds fallingBounds = fallingCollider.bounds;
        float skinWidth = 0.03f;

        // Cast from the centre far enough to include the lower half of this
        // block and the distance it will travel during this frame.
        float rayDistance =
            fallingBounds.extents.y + fallDistance + skinWidth;

        RaycastHit[] hits = Physics.RaycastAll(
            fallingBounds.center,
            Vector3.down,
            rayDistance,
            MapManager.Instance.pickup_LayerMask,
            QueryTriggerInteraction.Ignore);

        RaycastHit closestHit = new RaycastHit();
        bool blockFound = false;
        float closestDistance = float.MaxValue;

        foreach (RaycastHit currentHit in hits)
        {
            BlockInfo hitBlock =
                currentHit.collider.GetComponentInParent<BlockInfo>();

            if (hitBlock == null)
                continue;

            // Ignore this falling block and any of its own child colliders.
            if (hitBlock.gameObject == gameObject ||
                currentHit.collider.transform.IsChildOf(transform))
            {
                continue;
            }

            if (currentHit.distance < closestDistance)
            {
                closestDistance = currentHit.distance;
                closestHit = currentHit;
                blockFound = true;
            }
        }

        if (!blockFound)
            return false;

        // Place the bottom of this block directly on the top surface
        // of the block it landed on.
        float landingAdjustment =
            closestHit.point.y - fallingCollider.bounds.min.y;

        transform.position += Vector3.up * landingAdjustment;

        return true;
    }

    void LandBlock()
    {
        GetComponent<BlockInfo>().movementState = MovementStates.Still;

        hasStartedFalling = false;
        isSteppedOn = false;
        canFall = false;
        waitCounter = 0;

        SetFallingSandEnabled(false);
        ReleaseCarriedMoveableBlock();

        // Remove any remaining rotation from the warning shake.
        RestoreWarningRotation();
    }

    void FallingAlertAnimation()
    {
        if (resettingBlock)
            isSteppedOn = false;

        // When falling, straighten the rotation after the shaking.
        if (waitCounter >= waitTime_BeforeFalling)
        {
            RestoreWarningRotation();
            return;
        }

        // Shake the block.
        if (LOD_ObjectsList.Count > 0)
        {
            for (int i = 0; i < LOD_ObjectsList.Count; i++)
            {
                float shakeValue =
                    Mathf.Sin(Time.time * shakingSpeed) * shakingIntensity;

                Vector3 currentRotation = transform.eulerAngles;
                currentRotation.x = objectInitialRotation.x + shakeValue;

                LOD_ObjectsList[i].transform.eulerAngles = currentRotation;
            }
        }
    }


    //--------------------


    void HideBlock()
    {
        gameObject.SetActive(false);
    }

    public void ShowBlock()
    {
        gameObject.SetActive(true);
    }


    //--------------------


    public void ResetBlock()
    {
        resettingBlock = true;
        carriedByMoveablePillar = false;

        hasStartedFalling = false;
        isSteppedOn = false;
        waitingMoveableBlock = null;
        ReleaseCarriedMoveableBlock();
        RestoreWarningRotation();
        StopAllCoroutines();

        if (GetComponent<BoxCollider>())
            GetComponent<BoxCollider>().enabled = true;
        else if (GetComponent<MeshCollider>())
            GetComponent<MeshCollider>().enabled = true;

        GetComponent<BlockInfo>().movementState = MovementStates.Still;

        waitCounter = 0;
        transform.position = GetComponent<BlockInfo>().startPos;

        StartCoroutine(ResetBlockWaiting(0.1f));

        ShowBlock();
    }

    IEnumerator ResetBlockWaiting(float waitTime)
    {
        isSteppedOn = false;

        yield return new WaitForSeconds(waitTime);

        if (GetComponent<BoxCollider>())
            GetComponent<BoxCollider>().enabled = true;
        else if (GetComponent<MeshCollider>())
            GetComponent<MeshCollider>().enabled = true;

        resettingBlock = false;

        // Re-evaluate whether the block can fall from its original position.
        CheckIfCanFall();
    }
}