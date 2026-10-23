
using System;
using System.Collections.Generic;
using UnityEngine;

public class Block_Moveable : MonoBehaviour
{
    public static event Action Action_ObjectHasBeenPushed;

    private MovementDirection movementDirection = MovementDirection.None;

    public bool canMove;
    [SerializeField] private bool isMoving;
    [SerializeField] private bool isIceGliding;

    private bool isFalling;
    private bool isDespawned;
    private float fallSpeed;

    // Separate from normal pushing: the falling sand block carries this
    // moveable block until it lands, cancels or disappears.
    private bool isBeingCarriedByFallingSand;

    // A slope exit is a distinct diagonal, grid-aligned movement.
    private bool isSlopeGliding;
    private Vector3 slopeGlideTarget;

    private Vector3 startPos;
    private Vector3 savePos;

    private BlockInfo blockInfo;

    private const float GRID_SIZE = 1f;
    private const float SNAP_DISTANCE = 0.03f;
    private const float DOWN_RAY_START_OFFSET = 0.25f;
    private const float DOWN_RAY_DISTANCE = 1f;
    private const float FORWARD_CHECK_DISTANCE = 1f;
    private const float DESPAWN_Y = -5f;

    private Renderer[] blockRenderers;
    private Collider[] blockColliders;
    private bool[] rendererStates;
    private bool[] colliderStates;

    // Only the colliders of non-supporting liquids covered by this block
    // are temporarily disabled. Their original states are restored on exit.
    private readonly List<Collider> coveredLiquidColliders = new List<Collider>();

    // Keep the logical 1x1x1 block on the grid. Only its visual children
    // are lowered into quicksand, similarly to the player's body height.
    private readonly List<Transform> quicksandVisualChildren = new List<Transform>();
    private readonly List<Vector3> quicksandVisualStartPositions = new List<Vector3>();
    private int quicksandSinkStage;
    private Vector3Int? lastQuicksandCell;

    // A moveable block carries the uninterrupted vertical column directly
    // above it. The blocks remain separate GameObjects and retain their own
    // original positions for the game's existing respawn system.
    private readonly List<BlockInfo> carriedPillar = new List<BlockInfo>();

    // A blocked height splits the pillar. The detached upper part remains in
    // its original column and starts falling AFTER the carrier clears that cell.
    private sealed class DetachedPillar
    {
        public readonly List<BlockInfo> blocks = new List<BlockInfo>();
        public readonly Dictionary<Renderer, bool> hiddenRenderers =
            new Dictionary<Renderer, bool>();
        public readonly Dictionary<Collider, bool> hiddenColliders =
            new Dictionary<Collider, bool>();
        public bool readyToFall;
        public bool hiddenBelowLevel;
        public float speed;
    }
    private readonly List<DetachedPillar> detachedPillars = new List<DetachedPillar>();
    private Block_Moveable pillarCarrier;
    private readonly Dictionary<Renderer, bool> hiddenPillarRenderers =
        new Dictionary<Renderer, bool>();
    private readonly Dictionary<Collider, bool> hiddenPillarColliders =
        new Dictionary<Collider, bool>();

    public static int movingBlockCount = 0;
    public static bool AnyBlockMoving => movingBlockCount > 0;


    //--------------------


    private void Start()
    {
        startPos = transform.position;
        savePos = transform.position;
        blockInfo = GetComponent<BlockInfo>();

        CacheQuicksandVisualChildren();
        DisableCoveredLiquidColliders(savePos);
        RefreshMovementState();
    }

    private void Update()
    {
        // Detached columns fall independently, even if this moveable block
        // has stopped pushing or is continuing along an ice path.
        UpdateDetachedPillars();

        if (!isMoving) return;
        if (isBeingCarriedByFallingSand) return;

        if (isFalling)
        {
            PerformFalling();
            return;
        }

        if (isSlopeGliding)
        {
            PerformSlopeGlide();
            return;
        }

        // Preserve the original ice-glide speed. Ordinary pushes become
        // 15% slower per quicksand stage, relative to the original speed.
        // This never writes to BlockInfo.movementSpeed, so reductions cannot stack.
        float speed = isIceGliding
            ? 10f
            : blockInfo.movementSpeed * GetQuicksandMovementSpeedMultiplier();
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

        PlayerManager.Action_UpdatedLookingAtHorizontal += DisplayButtonMessage;
    }

    private void OnDisable()
    {
        RestoreCoveredLiquidColliders();
        RestoreHiddenPillar();
        ReleasePillar(refreshMovement: false);
        RestoreDetachedPillars();
        pillarCarrier = null;

        if (isBeingCarriedByFallingSand)
        {
            isBeingCarriedByFallingSand = false;
            if (isMoving)
                movingBlockCount = Mathf.Max(0, movingBlockCount - 1);
            isMoving = false;
        }

        Movement.Action_StepTaken -= RefreshMovementState;
        Movement.Action_BodyRotated -= RefreshMovementState;
        Movement.Action_RespawnPlayer -= ResetBlockPos;
        Movement.Action_LandedFromFalling -= RefreshMovementState;

        Player_KeyInputs.Action_InteractButton_isPressed -= ActivateBlockMovement;

        PlayerManager.Action_UpdatedLookingAtHorizontal -= DisplayButtonMessage;
    }


    //--------------------


    private void RefreshMovementState()
    {
        if (isMoving || isDespawned || pillarCarrier != null) return;

        movementDirection = GetPushDirectionFromPlayer();
        canMove = movementDirection != MovementDirection.None && CanMoveOneStep();

        Movement.Instance.UpdateAvailableMovementBlocks();

        DisplayButtonMessage();
    }

    private MovementDirection GetPushDirectionFromPlayer()
    {
        Vector3 lookDir = Movement.Instance.lookDir;

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
        if (PlayerManager.Instance == null ||
            PlayerManager.Instance.player == null)
            return false;

        Vector3 expectedPlayerPos =
            transform.position + directionToPlayer * GRID_SIZE;

        Vector3 actualPlayerPos =
            PlayerManager.Instance.player.transform.position;

        const float horizontalTolerance = 0.15f;
        const float verticalTolerance = 0.25f;

        return
            Mathf.Abs(actualPlayerPos.x - expectedPlayerPos.x)
                <= horizontalTolerance &&
            Mathf.Abs(actualPlayerPos.z - expectedPlayerPos.z)
                <= horizontalTolerance &&
            Mathf.Abs(actualPlayerPos.y - expectedPlayerPos.y)
                <= verticalTolerance;
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

        if (HasBlockingObjectInFront(moveDir))
            return false;

        // The pillar must be available to carry; obstacles at upper heights
        // will split it instead of preventing the lower block's push.
        if (!CanMovePillar(moveDir))
            return false;

        // A downward slope may only be entered when its lower exit is free.
        Vector3 nextPos = savePos + moveDir;
        if (IsBlockedDownhillSlope(nextPos, moveDir))
            return false;

        // Support is no longer required for a horizontal push.
        // If the destination has no support, the block will fall
        // after completing its horizontal movement.

        return true;
    }

    // Use the same fence collider/layer detection as Movement's raycast.
    // Unlike a BlockInfo obstruction, a fence may be a child collider
    // between two grid cells, so checking only the destination misses it.
    private bool HasFenceOnPath(Vector3 origin, Vector3 displacement)
    {
        float distance = displacement.magnitude;
        if (distance <= 0.0001f)
            return false;

        RaycastHit[] hits = Physics.RaycastAll(
            origin, displacement / distance, distance,
            MapManager.Instance.pickup_LayerMask);

        foreach (RaycastHit fenceHit in hits)
        {
            if (fenceHit.collider != null &&
                fenceHit.collider.GetComponentInParent<Fence>() != null)
                return true;
        }

        return false;
    }

    private bool HasBlockingObjectInFront(Vector3 moveDir)
    {
        // The normal first-hit check below only recognizes BlockInfo.
        // Fences have their own component and must also block a push.
        if (HasFenceOnPath(transform.position, moveDir * FORWARD_CHECK_DISTANCE))
            return true;

        if (Physics.Raycast(transform.position, moveDir, out RaycastHit hit, FORWARD_CHECK_DISTANCE))
        {
            // Preserve the existing first-hit behavior except where the
            // first hit is one of the four pass-through liquids.
            if (!IsPassThroughLiquid(hit.collider.GetComponentInParent<BlockInfo>()))
            {
                bool blocked = hit.transform != null &&
                               hit.transform.GetComponent<BlockInfo>() != null;
                Debug.DrawRay(transform.position, moveDir * FORWARD_CHECK_DISTANCE,
                    blocked ? Color.red : Color.green, 0.5f);
                return blocked;
            }

            RaycastHit[] hits = Physics.RaycastAll(
                transform.position, moveDir, FORWARD_CHECK_DISTANCE);
            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit candidate in hits)
            {
                if (candidate.collider == null ||
                    candidate.transform == transform ||
                    candidate.transform.IsChildOf(transform))
                    continue;

                if (IsPassThroughLiquid(candidate.collider.GetComponentInParent<BlockInfo>()))
                    continue;

                bool blocked = candidate.transform.GetComponent<BlockInfo>() != null;
                Debug.DrawRay(transform.position, moveDir * FORWARD_CHECK_DISTANCE,
                    blocked ? Color.red : Color.green, 0.5f);
                return blocked;
            }
        }

        Debug.DrawRay(transform.position, moveDir * FORWARD_CHECK_DISTANCE, Color.green, 0.5f);
        return false;
    }

    //--------------------
    // Blocks stacked vertically on a moveable block
    //--------------------

    private bool TryGetPillarAtRest(List<BlockInfo> pillar)
    {
        pillar.Clear();
        Vector3Int baseCell = Vector3Int.RoundToInt(savePos);

        // A pillar consists ONLY of directly adjacent blocks, with no gaps.
        // Checking small cell centres avoids picking up neighbouring columns.
        for (int height = 1; height <= 100; height++)
        {
            Vector3Int cell = baseCell + Vector3Int.up * height;
            Collider[] hits = Physics.OverlapBox(
                (Vector3)cell, Vector3.one * 0.24f,
                Quaternion.identity, Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Ignore);

            BlockInfo found = null;
            foreach (Collider hit in hits)
            {
                if (hit == null) continue;
                BlockInfo info = hit.GetComponentInParent<BlockInfo>();
                if (info == null || info == blockInfo || info.GridPosition != cell)
                    continue;

                // If liquid and solid share a cell, carry the solid block.
                if (found == null ||
                    (IsPassThroughLiquid(found) && !IsPassThroughLiquid(info)))
                    found = info;
            }

            if (found == null)
                break;

            Block_Moveable otherMoveable = found.GetComponent<Block_Moveable>();
            if (otherMoveable != null &&
                (otherMoveable.isMoving || otherMoveable.isDespawned ||
                 otherMoveable.pillarCarrier != null))
                return false;

            pillar.Add(found);
        }

        return true;
    }

    // Find the FIRST (lowest) carried height whose NEXT cell contains a
    // solid block. Everything at and above that height must be left behind.
    private int FindLowestBlockedPillarIndex(
        List<BlockInfo> pillar, Vector3 movementDelta)
    {
        for (int index = 0; index < pillar.Count; index++)
        {
            BlockInfo carried = pillar[index];
            if (carried == null) continue;

            // Fences can sit between cells instead of inside the target
            // cell. Split at the first carried height that crosses one.
            if (HasFenceOnPath(carried.transform.position, movementDelta))
                return index;

            Vector3Int destination = Vector3Int.RoundToInt(
                carried.transform.position + movementDelta);

            Collider[] hits = Physics.OverlapBox(
                (Vector3)destination, Vector3.one * 0.24f,
                Quaternion.identity, Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Ignore);

            foreach (Collider hit in hits)
            {
                if (hit == null) continue;
                BlockInfo other = hit.GetComponentInParent<BlockInfo>();
                if (other == null || other == blockInfo ||
                    pillar.Contains(other) || IsPassThroughLiquid(other))
                    continue;

                if (other.GridPosition == destination)
                    return index;
            }
        }
        return -1;
    }

    private bool CanMovePillar(Vector3 moveDelta)
    {
        // Upper collisions now split the pillar, never block its base.
        if (carriedPillar.Count > 0)
            return true;

        List<BlockInfo> prospectivePillar = new List<BlockInfo>();
        return TryGetPillarAtRest(prospectivePillar);
    }

    private void DetachBlockedPillar(Vector3 movementDelta)
    {
        if (carriedPillar.Count == 0)
            return;

        Physics.SyncTransforms();
        int firstBlocked = FindLowestBlockedPillarIndex(
            carriedPillar, movementDelta);
        if (firstBlocked < 0)
            return;

        DetachedPillar detached = new DetachedPillar();
        for (int i = firstBlocked; i < carriedPillar.Count; i++)
        {
            BlockInfo member = carriedPillar[i];
            if (member != null)
                detached.blocks.Add(member);
            // Retain pillarCarrier and SandFalling suspension until the
            // detached column lands. It is no longer in the carried list.
        }
        carriedPillar.RemoveRange(firstBlocked,
            carriedPillar.Count - firstBlocked);
        detachedPillars.Add(detached);
    }

    // Arm all newly detached groups only when the base has COMPLETED the
    // horizontal/diagonal tile, preventing them from landing on it mid-push.
    private void ArmDetachedPillars()
    {
        foreach (DetachedPillar group in detachedPillars)
            group.readyToFall = true;
    }

    // The player occupies a grid cell above the block they stand on.
    // A detached pillar can keep descending until its lowest block reaches
    // the NEXT cell above the player. This is a temporary stop, not a landing:
    // the same group resumes falling when the player leaves its column.
    private bool TryGetPlayerStopHeight(Vector3 bottomPosition, out float stopY)
    {
        stopY = float.NegativeInfinity;
        if (PlayerManager.Instance == null ||
            PlayerManager.Instance.player == null)
            return false;

        Vector3 playerPosition = PlayerManager.Instance.player.transform.position;

        // Include the player while moving INTO the cell, not just when their
        // root has reached its centre. Adjacent full grid cells are unaffected.
        const float horizontalSafety = 0.95f;
        if (Mathf.Abs(playerPosition.x - bottomPosition.x) >= horizontalSafety ||
            Mathf.Abs(playerPosition.z - bottomPosition.z) >= horizontalSafety)
            return false;

        // Do not treat the player standing ON the detached block as beneath it.
        if (playerPosition.y >= bottomPosition.y - 0.15f)
            return false;

        // Movement positions the player's root approximately 0.95 above the
        // standing block. Round to its logical cell, then leave one full grid
        // space for the falling block immediately above the player.
        stopY = Mathf.Round(playerPosition.y / GRID_SIZE) * GRID_SIZE + GRID_SIZE;
        return true;
    }

    private void UpdateDetachedPillars()
    {
        // These blocks move independently of the carrier. Player movement
        // options must therefore be rebuilt as their colliders move, including
        // during a fall, not only after a push or final landing.
        bool refreshPlayerMovement = false;

        for (int i = detachedPillars.Count - 1; i >= 0; i--)
        {
            DetachedPillar group = detachedPillars[i];
            if (!group.readyToFall || group.hiddenBelowLevel)
                continue;
            if (group.blocks.Count == 0 || group.blocks[0] == null)
            {
                FinishDetachedPillar(group);
                detachedPillars.RemoveAt(i);
                refreshPlayerMovement = true;
                continue;
            }

            BlockInfo bottom = group.blocks[0];
            Collider bottomCollider = bottom.GetComponent<Collider>();
            if (bottomCollider == null)
                bottomCollider = bottom.GetComponentInChildren<Collider>();
            if (bottomCollider == null)
            {
                FinishDetachedPillar(group);
                detachedPillars.RemoveAt(i);
                refreshPlayerMovement = true;
                continue;
            }

            Physics.SyncTransforms();
            group.speed += Mathf.Abs(Physics.gravity.y) * Time.deltaTime;
            float distance = group.speed * Time.deltaTime;
            Vector3 basePosition = bottom.transform.position;
            Bounds bounds = bottomCollider.bounds;
            float landingY = float.NegativeInfinity;
            float castDistance = bounds.extents.y + distance + SNAP_DISTANCE;

            RaycastHit[] hits = Physics.RaycastAll(
                bounds.center, Vector3.down, castDistance,
                Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            foreach (RaycastHit contact in hits)
            {
                if (contact.collider == null) continue;
                BlockInfo support = contact.collider.GetComponentInParent<BlockInfo>();
                if (support == null || group.blocks.Contains(support) ||
                    IsPassThroughLiquid(support))
                    continue;

                // This retains the bottom collider's actual offset from
                // its root instead of assuming a particular mesh pivot.
                float candidateY = basePosition.y +
                    contact.point.y - bounds.min.y;
                if (candidateY > basePosition.y + SNAP_DISTANCE ||
                    candidateY < basePosition.y - distance - SNAP_DISTANCE)
                    continue;
                if (candidateY > landingY)
                    landingY = candidateY;
            }

            float targetY = landingY > float.NegativeInfinity
                ? landingY : basePosition.y - distance;

            // A player is a TEMPORARY support. Never complete the detached
            // group's fall on the player; keep the group active so it can drop
            // the remaining distance after the player moves away.
            bool stoppedByPlayer = false;
            if (TryGetPlayerStopHeight(basePosition, out float playerStopY) &&
                playerStopY >= targetY - SNAP_DISTANCE &&
                (landingY == float.NegativeInfinity ||
                 landingY < playerStopY - SNAP_DISTANCE))
            {
                targetY = playerStopY;
                stoppedByPlayer = true;
                group.speed = 0f;
            }

            Vector3 delta = Vector3.up * (targetY - basePosition.y);
            if (delta.sqrMagnitude > 0.0000001f)
            {
                foreach (BlockInfo member in group.blocks)
                    if (member != null) member.transform.position += delta;
                Physics.SyncTransforms();
                refreshPlayerMovement = true;
            }

            if (stoppedByPlayer)
                continue;

            if (landingY > float.NegativeInfinity)
            {
                FinishDetachedPillar(group);
                detachedPillars.RemoveAt(i);
                refreshPlayerMovement = true;
            }
            else if (bottom.transform.position.y < DESPAWN_Y)
            {
                // Preserve the same Y=-5 removal behavior without disabling
                // the GameObjects (they must remain available for respawn).
                HideDetachedPillar(group);
                refreshPlayerMovement = true;
            }
        }

        if (refreshPlayerMovement && Movement.Instance != null)
            Movement.Instance.RefreshAfterMoveableBlockChange();
    }

    private void FinishDetachedPillar(DetachedPillar group)
    {
        Physics.SyncTransforms();
        foreach (BlockInfo member in group.blocks)
        {
            if (member == null) continue;
            Block_SandFalling sand = member.GetComponent<Block_SandFalling>();
            if (sand != null) sand.SetCarriedByMoveablePillar(false);

            Block_Moveable moveable = member.GetComponent<Block_Moveable>();
            if (moveable == null || moveable.pillarCarrier != this) continue;
            moveable.pillarCarrier = null;
            moveable.savePos = member.transform.position;
            if (moveable.isActiveAndEnabled && !moveable.isDespawned)
            {
                moveable.UpdateRestingSurfaceEffects();
                moveable.RefreshMovementState();
            }
        }
    }

    private void HideDetachedPillar(DetachedPillar group)
    {
        group.hiddenBelowLevel = true;
        foreach (BlockInfo member in group.blocks)
        {
            if (member == null) continue;
            foreach (Renderer renderer in member.GetComponentsInChildren<Renderer>(true))
            {
                group.hiddenRenderers[renderer] = renderer.enabled;
                renderer.enabled = false;
            }
            foreach (Collider collider in member.GetComponentsInChildren<Collider>(true))
            {
                group.hiddenColliders[collider] = collider.enabled;
                collider.enabled = false;
            }
        }
    }

    private void RestoreDetachedPillars()
    {
        foreach (DetachedPillar group in detachedPillars)
        {
            foreach (KeyValuePair<Renderer, bool> entry in group.hiddenRenderers)
                if (entry.Key != null) entry.Key.enabled = entry.Value;
            foreach (KeyValuePair<Collider, bool> entry in group.hiddenColliders)
                if (entry.Key != null) entry.Key.enabled = entry.Value;
            FinishDetachedPillar(group);
        }
        detachedPillars.Clear();
    }

    private bool BeginCarryingPillar()
    {
        if (carriedPillar.Count > 0)
            return true;

        List<BlockInfo> prospectivePillar = new List<BlockInfo>();
        if (!TryGetPillarAtRest(prospectivePillar))
            return false;

        foreach (BlockInfo carried in prospectivePillar)
        {
            if (carried == null) continue;
            Block_Moveable otherMoveable = carried.GetComponent<Block_Moveable>();
            if (otherMoveable != null)
            {
                otherMoveable.pillarCarrier = this;
                otherMoveable.canMove = false;
                otherMoveable.RestoreCoveredLiquidColliders();
            }
            Block_SandFalling sand = carried.GetComponent<Block_SandFalling>();
            if (sand != null)
                sand.SetCarriedByMoveablePillar(true);

            carriedPillar.Add(carried);
        }

        return true;
    }

    private void MovePillar(Vector3 positionDelta)
    {
        if (positionDelta == Vector3.zero || carriedPillar.Count == 0)
            return;

        foreach (BlockInfo carried in carriedPillar)
        {
            if (carried == null) continue;
            carried.transform.position += positionDelta;

            Block_Moveable otherMoveable = carried.GetComponent<Block_Moveable>();
            if (otherMoveable != null && otherMoveable.pillarCarrier == this)
                otherMoveable.savePos = carried.transform.position;
        }

        // Make the new positions visible to physics queries in this frame.
        Physics.SyncTransforms();
    }

    private void ReleasePillar(bool refreshMovement = true)
    {
        if (carriedPillar.Count == 0)
            return;

        Physics.SyncTransforms();
        foreach (BlockInfo carried in carriedPillar)
        {
            if (carried == null) continue;
            Block_SandFalling sand = carried.GetComponent<Block_SandFalling>();
            if (sand != null)
                sand.SetCarriedByMoveablePillar(false);

            Block_Moveable otherMoveable = carried.GetComponent<Block_Moveable>();
            if (otherMoveable == null || otherMoveable.pillarCarrier != this)
                continue;

            otherMoveable.pillarCarrier = null;
            otherMoveable.savePos = otherMoveable.transform.position;
            if (otherMoveable.isActiveAndEnabled && !isDespawned)
            {
                otherMoveable.UpdateRestingSurfaceEffects();
                if (refreshMovement)
                    otherMoveable.RefreshMovementState();
            }
        }

        carriedPillar.Clear();
    }

    private void HidePillarForDespawn()
    {
        foreach (BlockInfo carried in carriedPillar)
        {
            if (carried == null) continue;
            foreach (Renderer renderer in carried.GetComponentsInChildren<Renderer>(true))
            {
                if (renderer == null) continue;
                hiddenPillarRenderers[renderer] = renderer.enabled;
                renderer.enabled = false;
            }
            foreach (Collider collider in carried.GetComponentsInChildren<Collider>(true))
            {
                if (collider == null) continue;
                hiddenPillarColliders[collider] = collider.enabled;
                collider.enabled = false;
            }
        }
    }

    private void RestoreHiddenPillar()
    {
        foreach (KeyValuePair<Renderer, bool> entry in hiddenPillarRenderers)
            if (entry.Key != null) entry.Key.enabled = entry.Value;
        foreach (KeyValuePair<Collider, bool> entry in hiddenPillarColliders)
            if (entry.Key != null) entry.Key.enabled = entry.Value;

        hiddenPillarRenderers.Clear();
        hiddenPillarColliders.Clear();
    }

    // Water, lava, swamp water and mud have no physical support for a
    // moveable block. Quicksand deliberately does NOT belong to this group.
    private static bool IsPassThroughLiquid(BlockInfo info)
    {
        return info != null &&
               (info.GetComponent<Block_Water>() != null ||
                info.GetComponent<Block_Lava>() != null ||
                info.GetComponent<Block_SwampWater>() != null ||
                info.GetComponent<Block_Mud>() != null);
    }

    private BlockInfo GetSupportingBlockBelow(Vector3 worldPos)
    {
        Vector3 rayOrigin = worldPos + Vector3.up * DOWN_RAY_START_OFFSET;
        if (!Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit firstHit,
                DOWN_RAY_DISTANCE))
            return null;

        // In ordinary situations, retain the exact first-hit rule.
        // Only look behind a collision if that collision was liquid.
        if (!IsPassThroughLiquid(firstHit.collider.GetComponentInParent<BlockInfo>()))
            return GetValidSupport(firstHit);

        RaycastHit[] hits = Physics.RaycastAll(
            rayOrigin, Vector3.down, DOWN_RAY_DISTANCE);
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == null ||
                hit.transform == transform ||
                hit.transform.IsChildOf(transform))
                continue;

            if (IsPassThroughLiquid(hit.collider.GetComponentInParent<BlockInfo>()))
                continue;

            return GetValidSupport(hit);
        }

        return null;
    }

    private static BlockInfo GetValidSupport(RaycastHit hit)
    {
        BlockInfo support = hit.transform.GetComponent<BlockInfo>();
        if (support == null || support.blockType == BlockType.Stair ||
            support.blockType == BlockType.Slope)
            return null;

        return support;
    }

    private bool HasSupportBelow(Vector3 worldPos)
    {
        bool supported = GetSupportingBlockBelow(worldPos) != null;
        Vector3 rayOrigin = worldPos + Vector3.up * DOWN_RAY_START_OFFSET;
        Debug.DrawRay(rayOrigin, Vector3.down * DOWN_RAY_DISTANCE,
            supported ? Color.green : Color.red, 0.5f);
        return supported;
    }

    //--------------------
    // Liquid overlap and quicksand visual stages
    //--------------------

    private void RestoreCoveredLiquidColliders()
    {
        foreach (Collider collider in coveredLiquidColliders)
        {
            if (collider != null)
                collider.enabled = true;
        }

        coveredLiquidColliders.Clear();
    }

    private void DisableCoveredLiquidColliders(Vector3 gridPos)
    {
        RestoreCoveredLiquidColliders();
        Physics.SyncTransforms();

        // Only cover liquid blocks occupying the SAME grid cell. Liquids
        // below the movable block keep their normal player interaction.
        Vector3Int cell = Vector3Int.RoundToInt(gridPos);
        Collider[] nearby = Physics.OverlapBox(
            (Vector3)cell, Vector3.one * 0.35f,
            Quaternion.identity, ~0,
            QueryTriggerInteraction.Collide);

        HashSet<BlockInfo> processed = new HashSet<BlockInfo>();
        foreach (Collider overlap in nearby)
        {
            if (overlap == null)
                continue;

            BlockInfo liquid = overlap.GetComponentInParent<BlockInfo>();
            if (liquid == null || liquid == blockInfo ||
                liquid.GridPosition != cell ||
                !IsPassThroughLiquid(liquid) || !processed.Add(liquid))
                continue;

            foreach (Collider liquidCollider in liquid.GetComponentsInChildren<Collider>(true))
            {
                if (liquidCollider != null && liquidCollider.enabled)
                {
                    coveredLiquidColliders.Add(liquidCollider);
                    liquidCollider.enabled = false;
                }
            }
        }
    }

    private void CacheQuicksandVisualChildren()
    {
        quicksandVisualChildren.Clear();
        quicksandVisualStartPositions.Clear();

        // The existing BlockInfo expects its mesh renderers on children.
        // Keep root position, grid checks and root colliders unchanged.
        foreach (Transform child in transform)
        {
            if (child.GetComponentInChildren<Renderer>(true) == null)
                continue;

            quicksandVisualChildren.Add(child);
            quicksandVisualStartPositions.Add(child.localPosition);
        }
    }

    private void SetQuicksandVisualStage(int stage)
    {
        quicksandSinkStage = Mathf.Clamp(stage, 0, 5);

        // Player_BodyHeight uses -0.15 normally and -0.30/-0.42/-0.54/
        // -0.66/-0.78 for quicksand. These are the corresponding deltas.
        float sinkOffset = quicksandSinkStage == 0 ? 0f :
                           -0.15f - 0.12f * (quicksandSinkStage - 1);

        for (int i = 0; i < quicksandVisualChildren.Count; i++)
        {
            if (quicksandVisualChildren[i] != null)
                quicksandVisualChildren[i].localPosition =
                    quicksandVisualStartPositions[i] + Vector3.up * sinkOffset;
        }
    }

    private void RefreshQuicksandStageAtRest()
    {
        BlockInfo support = GetSupportingBlockBelow(savePos);
        if (support == null || support.GetComponent<Block_Quicksand>() == null)
        {
            lastQuicksandCell = null;
            SetQuicksandVisualStage(0);
            return;
        }

        Vector3Int currentCell = Vector3Int.RoundToInt(savePos);
        if (!lastQuicksandCell.HasValue || lastQuicksandCell.Value != currentCell)
        {
            lastQuicksandCell = currentCell;
            SetQuicksandVisualStage(quicksandSinkStage + 1);
        }
    }

    private void ResetQuicksandStage()
    {
        lastQuicksandCell = null;
        SetQuicksandVisualStage(0);
    }

    private float GetQuicksandMovementSpeedMultiplier()
    {
        // Stage 0 = 100%, stage 1 = 90%, ..., stage 5 = 50%.
        // quicksandSinkStage belongs to this block, independent of the player.
        return 1f - 0.14f * Mathf.Clamp(quicksandSinkStage, 0, 5);
    }

    private void UpdateRestingSurfaceEffects()
    {
        DisableCoveredLiquidColliders(savePos);
        RefreshQuicksandStageAtRest();
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
        if (isMoving || isDespawned || pillarCarrier != null) return;

        // Do not let a push relocate a block during an already-started Dash.
        if (Movement.Instance != null && Movement.Instance.isDashing) return;

        RefreshMovementState();

        if (movementDirection == MovementDirection.None) return;
        if (!canMove) return;

        Vector3 pushDir = GetMoveVector();
        if (pushDir == Vector3.zero) return;

        Vector3 facingTowardBlock = -pushDir;

        if (Movement.Instance.JustTurnedToward(facingTowardBlock))
            return;

        if (Movement.Instance.IsInTurnAbilityDelay(facingTowardBlock))
            return;

        if (!BeginCarryingPillar()) return;
        DetachBlockedPillar(pushDir);

        // The starting tile may already be cracked ice. Begin its crack
        // effect too, so the very first tile receives both stages.
        CrackWeakIceBelow(savePos);

        isMoving = true;
        isIceGliding = false;
        movingBlockCount++;

        // Hide invalid dash previews immediately while this block is in motion.
        Movement.Instance.RefreshAfterMoveableBlockChange();
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

        // At the beginning of EACH grid step (including ice-glide steps),
        // separate any upper blocks that would run into a solid wall.
        if (Vector3.Distance(transform.position, savePos) <= SNAP_DISTANCE)
            DetachBlockedPillar(moveDir);

        Vector3 positionBeforeMove = transform.position;
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            movementSpeed * Time.deltaTime
        );
        MovePillar(transform.position - positionBeforeMove);

        if (Vector3.Distance(transform.position, targetPos) <= SNAP_DISTANCE)
        {
            // Remember where the block was before moving.
            Vector3 previousPos = savePos;

            Vector3 positionBeforeSnap = transform.position;
            transform.position = targetPos;
            MovePillar(transform.position - positionBeforeSnap);
            savePos = targetPos;
            ArmDetachedPillars();
            RestoreCoveredLiquidColliders();

            // Break the cracked ice just left, then play a fresh crack
            // effect on the tile the block has now reached.
            BreakWeakIceBelow(previousPos);
            CrackWeakIceBelow(savePos);

            // Slopes are deliberately excluded from HasSupportBelow.
            // Recognize the slope at this grid cell BEFORE starting a fall.
            if (TryStartSlopeGlide(savePos, moveDir))
            {
                // The block will glide diagonally down the slope.
            }
            else if (!HasSupportBelow(savePos))
            {
                StartFalling();
            }
            else if (ShouldContinueIceGlide())
            {
                isMoving = true;
                isIceGliding = true;
            }
            else
            {
                StopMovement();
                RefreshMovementState();
            }

            Action_ObjectHasBeenPushed?.Invoke();
        }
    }

    private void BreakWeakIceBelow(Vector3 previousPos)
    {
        Vector3 rayOrigin =
            previousPos + Vector3.up * DOWN_RAY_START_OFFSET;

        RaycastHit[] hits = Physics.RaycastAll(
            rayOrigin,
            Vector3.down,
            DOWN_RAY_DISTANCE
        );

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == null)
                continue;

            Block_Weak weakBlock =
                hit.collider.GetComponentInParent<Block_Weak>();

            if (weakBlock != null)
            {
                weakBlock.BreakFromMoveableBlock();
                return;
            }
        }
    }



    private void CrackWeakIceBelow(Vector3 worldPos)
    {
        Vector3 rayOrigin =
            worldPos + Vector3.up * DOWN_RAY_START_OFFSET;

        RaycastHit[] hits = Physics.RaycastAll(
            rayOrigin,
            Vector3.down,
            DOWN_RAY_DISTANCE
        );

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == null)
                continue;

            Block_Weak weakBlock =
                hit.collider.GetComponentInParent<Block_Weak>();

            if (weakBlock != null)
            {
                weakBlock.CrackFromMoveableBlock();
                return;
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

        // Cannot continue through another block.
        if (HasBlockingObjectInFront(moveDir))
            return false;
        if (!CanMovePillar(moveDir))
            return false;

        // An ice glide may enter a downhill slope only when its exit is clear.
        if (IsBlockedDownhillSlope(nextPos, moveDir))
            return false;

        // Allow gliding off an edge.
        // The block will move one horizontal grid space
        // and then begin falling.
        if (!HasSupportBelow(nextPos))
            return true;

        // Otherwise, preserve the original ice-glide rule:
        // the next supported tile must also be ice.
        if (!IsIceBelow(nextPos))
            return false;

        return true;
    }


    //--------------------
    // Slopes
    //--------------------

    private static Vector3 GetSlopeDownhill(Vector3 forward)
    {
        // Slopes are rotated in 90-degree increments in this 1x1x1 grid.
        return new Vector3(
            Mathf.Round(forward.x),
            0f,
            Mathf.Round(forward.z)
        );
    }

    private BlockInfo FindBlockAtGridPosition(Vector3 worldPos)
    {
        Vector3Int expected = Vector3Int.RoundToInt(worldPos);
        Collider[] hits = Physics.OverlapBox(
            (Vector3)expected,
            Vector3.one * 0.25f,
            Quaternion.identity,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore
        );

        foreach (Collider hit in hits)
        {
            if (hit == null || hit.transform.IsChildOf(transform))
                continue;

            BlockInfo info = hit.GetComponentInParent<BlockInfo>();
            if (info != null && info.GridPosition == expected &&
                !IsPassThroughLiquid(info))
                return info;
        }

        return null;
    }

    private bool TryGetDownhillSlope(Vector3 blockPosition, Vector3 moveDir, out BlockInfo slope)
    {
        // The block is one grid cell ABOVE the slope it is crossing.
        slope = FindBlockAtGridPosition(blockPosition + Vector3.down);
        return slope != null &&
               slope.blockType == BlockType.Slope &&
               GetSlopeDownhill(slope.transform.forward) == moveDir;
    }

    private bool IsBlockedDownhillSlope(Vector3 blockPosition, Vector3 moveDir)
    {
        if (!TryGetDownhillSlope(blockPosition, moveDir, out BlockInfo slope))
            return false;

        // The downhill finish is one space forward AND one space lower.
        Vector3 exitPosition = blockPosition + moveDir + Vector3.down;
        return HasFenceOnPath(blockPosition, exitPosition - blockPosition) ||
               FindBlockAtGridPosition(exitPosition) != null ||
               !CanMovePillar(exitPosition - savePos);
    }

    private bool TryStartSlopeGlide(Vector3 blockPosition, Vector3 moveDir)
    {
        if (!TryGetDownhillSlope(blockPosition, moveDir, out BlockInfo slope))
            return false;

        Vector3 exitPosition = blockPosition + moveDir + Vector3.down;
        if (HasFenceOnPath(blockPosition, exitPosition - blockPosition) ||
            FindBlockAtGridPosition(exitPosition) != null ||
            !CanMovePillar(exitPosition - savePos))
        {
            // This is also checked before entering. Do not fall into an
            // occupied exit if another block arrived during the push.
            StopMovement();
            return true;
        }

        DetachBlockedPillar(exitPosition - savePos);
        slopeGlideTarget = exitPosition;
        isSlopeGliding = true;
        isIceGliding = false;
        return true;
    }

    private void PerformSlopeGlide()
    {
        Vector3 positionBeforeMove = transform.position;
        transform.position = Vector3.MoveTowards(
            transform.position,
            slopeGlideTarget,
            blockInfo.movementSpeed * Time.deltaTime
        );
        MovePillar(transform.position - positionBeforeMove);

        if (Vector3.Distance(transform.position, slopeGlideTarget) > SNAP_DISTANCE)
            return;

        Vector3 positionBeforeSnap = transform.position;
        transform.position = slopeGlideTarget;
        MovePillar(transform.position - positionBeforeSnap);
        savePos = slopeGlideTarget;
        ArmDetachedPillars();
        RestoreCoveredLiquidColliders();
        isSlopeGliding = false;

        // A second downhill slope should continue the same diagonal glide.
        // Otherwise reuse the existing falling/ice/standing rules.
        if (TryStartSlopeGlide(savePos, GetMoveVector()))
        {
            // A connected downhill slope has taken over movement.
        }
        else if (!HasSupportBelow(savePos))
        {
            StartFalling();
        }
        else if (ShouldContinueIceGlide())
        {
            isIceGliding = true;
        }
        else
        {
            StopMovement();
        }

        Action_ObjectHasBeenPushed?.Invoke();
    }


    //--------------------
    // Riding a falling sand block
    //--------------------

    // Called by Block_SandFalling once its warning has completed.
    public bool BeginFallingWithSandBlock()
    {
        if (isMoving || isDespawned || pillarCarrier != null)
            return false;
        if (!BeginCarryingPillar())
            return false;

        isBeingCarriedByFallingSand = true;
        isMoving = true;
        movingBlockCount++;
        canMove = false;
        Movement.Instance.RefreshAfterMoveableBlockChange();
        return true;
    }

    public void FollowFallingSandBlock(Vector3 positionDelta)
    {
        if (!isBeingCarriedByFallingSand || isDespawned)
            return;

        transform.position += positionDelta;
        MovePillar(positionDelta);
        savePos = transform.position;

        if (transform.position.y < DESPAWN_Y)
        {
            isBeingCarriedByFallingSand = false;
            DespawnBlock();
        }
    }

    public void EndFallingWithSandBlock()
    {
        if (!isBeingCarriedByFallingSand)
            return;

        isBeingCarriedByFallingSand = false;
        // Ending a carried fall must not restart the sand warning.
        StopMovement(notifyFallingSupport: false);
    }


    //--------------------
    // Falling
    //--------------------


    private void StartFalling()
    {
        RestoreCoveredLiquidColliders();
        ResetQuicksandStage();
        isFalling = true;
        isIceGliding = false;
        fallSpeed = 0f;
    }

    private void PerformFalling()
    {
        // Accelerate downward using Unity gravity.
        fallSpeed += Mathf.Abs(Physics.gravity.y) * Time.deltaTime;

        float fallDistance = fallSpeed * Time.deltaTime;

        Vector3 currentPos = transform.position;
        float nextY = currentPos.y - fallDistance;

        // Check for blocks that the falling block could land on
        // during this frame.
        //
        // The additional half-grid distance accounts for the
        // bottom of the moving block reaching the supporting block.
        RaycastHit[] hits = Physics.RaycastAll(
            currentPos,
            Vector3.down,
            fallDistance + GRID_SIZE * 0.5f + SNAP_DISTANCE,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore
        );

        bool foundLanding = false;
        float landingY = float.NegativeInfinity;

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform == null)
                continue;

            // Ignore this block and its child colliders.
            if (hit.transform == transform ||
                hit.transform.IsChildOf(transform))
                continue;

            // Only land on another block.
            BlockInfo supportBlock =
                hit.collider.GetComponentInParent<BlockInfo>();

            if (supportBlock == null || IsPassThroughLiquid(supportBlock))
                continue;

            // The movable block occupies one vertical grid unit.
            float possibleLandingY =
                hit.point.y + GRID_SIZE * 0.5f;

            // The supporting surface must be reached during
            // this frame and must not be below the despawn level.
            if (possibleLandingY > currentPos.y + SNAP_DISTANCE)
                continue;

            if (possibleLandingY < nextY - SNAP_DISTANCE)
                continue;

            if (possibleLandingY < DESPAWN_Y)
                continue;

            // Choose the highest valid landing surface.
            if (possibleLandingY > landingY)
            {
                landingY = possibleLandingY;
                foundLanding = true;
            }
        }

        if (foundLanding)
        {
            transform.position = new Vector3(
                currentPos.x,
                landingY,
                currentPos.z
            );
            MovePillar(transform.position - currentPos);

            savePos = transform.position;

            LandFromFalling();
            return;
        }

        // Continue falling vertically.
        transform.position = new Vector3(
            currentPos.x,
            nextY,
            currentPos.z
        );
        MovePillar(transform.position - currentPos);

        // Remove the block if it falls below Y = -5.
        if (transform.position.y < DESPAWN_Y)
        {
            DespawnBlock();
        }
    }

    private void LandFromFalling()
    {
        isFalling = false;
        fallSpeed = 0f;

        // Continue moving in the original direction if
        // the block landed on ice and the glide can continue.
        if (ShouldContinueIceGlide())
        {
            isMoving = true;
            isIceGliding = true;
        }
        else
        {
            // Landing on a normal block, or reaching the end
            // of a valid ice path, stops movement.
            StopMovement();
        }
    }


    //--------------------
    // Despawning
    //--------------------


    private void DespawnBlock()
    {
        if (isDespawned)
            return;

        isDespawned = true;
        HidePillarForDespawn();
        RestoreCoveredLiquidColliders();
        ResetQuicksandStage();
        isFalling = false;
        fallSpeed = 0f;

        // Keep the GameObject active so that its existing
        // player-respawn event can restore the block.
        // Hide all renderers and disable all colliders instead.
        blockRenderers = GetComponentsInChildren<Renderer>(true);
        blockColliders = GetComponentsInChildren<Collider>(true);

        rendererStates = new bool[blockRenderers.Length];
        colliderStates = new bool[blockColliders.Length];

        for (int i = 0; i < blockRenderers.Length; i++)
        {
            rendererStates[i] = blockRenderers[i].enabled;
            blockRenderers[i].enabled = false;
        }

        for (int i = 0; i < blockColliders.Length; i++)
        {
            colliderStates[i] = blockColliders[i].enabled;
            blockColliders[i].enabled = false;
        }

        StopMovement();

        Movement.Instance.UpdateAvailableMovementBlocks();
        DisplayButtonMessage();
    }

    private void RestoreDespawnedBlock()
    {
        if (!isDespawned)
            return;

        // Restore each renderer and collider to the enabled
        // state it had before the block despawned.
        if (blockRenderers != null)
        {
            for (int i = 0; i < blockRenderers.Length; i++)
            {
                if (blockRenderers[i] != null)
                    blockRenderers[i].enabled = rendererStates[i];
            }
        }

        if (blockColliders != null)
        {
            for (int i = 0; i < blockColliders.Length; i++)
            {
                if (blockColliders[i] != null)
                    blockColliders[i].enabled = colliderStates[i];
            }
        }

        isDespawned = false;
    }


    //--------------------


    private void StopMovement(bool notifyFallingSupport = true)
    {
        if (isMoving)
        {
            movingBlockCount = Mathf.Max(0, movingBlockCount - 1);
        }

        isMoving = false;
        isIceGliding = false;
        isSlopeGliding = false;
        isFalling = false;
        fallSpeed = 0f;
        canMove = false;
        ReleasePillar();

        if (!isDespawned)
        {
            UpdateRestingSurfaceEffects();

            // Let an unblocked sand tile run its existing warning/fall cycle.
            // Only a completed push or landing triggers this callback.
            if (notifyFallingSupport)
            {
                BlockInfo support = GetSupportingBlockBelow(savePos);
                if (support != null)
                {
                    Block_SandFalling sand = support.GetComponent<Block_SandFalling>();
                    if (sand != null)
                        sand.TriggerFromMoveableBlock(this);
                }
            }
        }

        RefreshMovementState();
        Movement.Instance.RefreshAfterMoveableBlockChange();
    }

    private void ResetBlockPos()
    {
        RestoreHiddenPillar();
        ReleasePillar(refreshMovement: false);
        RestoreDetachedPillars();
        pillarCarrier = null;

        if (isMoving)
        {
            movingBlockCount = Mathf.Max(0, movingBlockCount - 1);
        }

        isMoving = false;
        isBeingCarriedByFallingSand = false;
        isIceGliding = false;
        isSlopeGliding = false;
        isFalling = false;
        fallSpeed = 0f;

        RestoreCoveredLiquidColliders();
        ResetQuicksandStage();
        transform.position = startPos;
        savePos = startPos;

        // Restore the block if it previously fell below Y = -5.
        RestoreDespawnedBlock();

        canMove = false;
        RefreshMovementState();
        Movement.Instance.RefreshAfterMoveableBlockChange();
    }


    //--------------------


    void DisplayButtonMessage()
    {
        if (PlayerManager.Instance.block_LookingAt_Horizontal && PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<Block_Moveable>() && PlayerManager.Instance.block_LookingAt_Horizontal.GetComponent<Block_Moveable>().canMove)
        {
            ButtonMessageManager.Instance.SetButtonMessage(ButtonMessageManager.Instance.buttonMessages.buttonMessage_Push);
            ButtonMessageManager.Instance.ShowButtonMessage(ButtonMessageManager.Instance.buttonMessages.buttonMessage_Push);
        }
        else
        {
            ButtonMessageManager.Instance.HideButtonMessage(ButtonMessageManager.Instance.buttonMessages.buttonMessage_Push);
        }
    }
}
