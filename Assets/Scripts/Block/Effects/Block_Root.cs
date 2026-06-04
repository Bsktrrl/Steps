using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Block_Root : MonoBehaviour
{
    public static event Action Action_StandingOnRootBlock_Early;
    public static event Action Action_StandingOnRootBlock;

    Animator anim;

    [SerializeField] List<RootBlockLineInfo> RootFreeCostBlockList = new List<RootBlockLineInfo>();
    [SerializeField] List<GameObject> RootObjectList = new List<GameObject>();

    RaycastHit hit;
    bool finishedCheckingForBlocks;

    Vector3 tempOriginPos;
    Vector3 playerLookDir;

    [Header("Root Placement Tuning")]
    [SerializeField] float stairUpForwardOffset = 0.30f;
    [SerializeField] float stairDownForwardOffset = -0.20f;
    [SerializeField] float stairUpHeightOffset = 0.20f;
    [SerializeField] float stairSurfaceTilt = -45f;

    [SerializeField] bool isActive;
    [SerializeField] bool onTrigger;

    Coroutine rootAnimCoroutine;
    Coroutine delayedRootExitCheckCoroutine;
    Coroutine delayedRootActivationCoroutine;
    Coroutine confirmedStandingActivationCoroutine;

    bool playerIsInsideRootTrigger;
    bool rootActivationQueued;

    [Header("Live Root Continuation")]
    [SerializeField] bool checkForRootContinuation = true;
    [SerializeField] float continuationCheckInterval = 0.05f;
    [SerializeField] int maxContinuationSegmentsPerCheck = 8;

    float nextContinuationCheckTime;

    [Header("Empty Root Start")]
    [SerializeField] bool keepCheckingFromRootBlockWhenNoRoots = true;
    [SerializeField] bool keepCheckingFromRootBlockEvenWhenRootsExist = true;

    [Header("Activation Safety")]
    [SerializeField] private bool activateFromConfirmedStanding = true;
    [SerializeField] private bool allowTriggerFallback = true;

    private bool activatedForCurrentRootStand;
    private Vector3 queuedActivationDirection;

    private bool ignoreNextSelfReset;


    //--------------------


    private void Start()
    {
        anim = GetComponentInParent<Animator>();

        // Detect duplicates
        var duplicates = RootObjectList.GroupBy(r => r.GetInstanceID()).Where(g => g.Count() > 1).ToList();

        if (duplicates.Count > 0)
        {
            Debug.LogWarning($"{nameof(Block_Root)} on {name}: Duplicate root objects detected in RootObjectList.", this);
        }
    }

    private void Update()
    {
        if (Movement.Instance == null)
            return;

        if (Movement.Instance.isMovingOnLadder_Down ||
            Movement.Instance.isMovingOnLadder_Up ||
            Movement.Instance.isRespawning)
            return;

        UpdateLiveRootContinuation();
    }

    private void OnEnable()
    {
        Movement.Action_StepTaken_Early += CheckWhenToResetRootLine;
        Movement.Action_StepTaken_Late += CheckActivateFromConfirmedStanding;

        Movement.Action_RespawnPlayer += ResetOnRespawn;
        Action_StandingOnRootBlock_Early += ResetRootOnly;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken_Early -= CheckWhenToResetRootLine;
        Movement.Action_StepTaken_Late -= CheckActivateFromConfirmedStanding;

        Movement.Action_RespawnPlayer -= ResetOnRespawn;
        Action_StandingOnRootBlock_Early -= ResetRootOnly;

        StopRootCoroutines();
    }


    //--------------------


    private void OnTriggerEnter(Collider other)
    {
        if (!allowTriggerFallback)
            return;

        if (other.transform.gameObject.layer != 6) // 6 = PlayerLayer
            return;

        playerIsInsideRootTrigger = true;

        QueueRootActivation();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!allowTriggerFallback)
            return;

        if (other.transform.gameObject.layer != 6) // 6 = PlayerLayer
            return;

        playerIsInsideRootTrigger = true;

        QueueRootActivation();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.layer != 6) // 6 = PlayerLayer
            return;

        playerIsInsideRootTrigger = false;
    }


    //--------------------


    void StopRootCoroutines()
    {
        if (rootAnimCoroutine != null)
        {
            StopCoroutine(rootAnimCoroutine);
            rootAnimCoroutine = null;
        }

        if (delayedRootExitCheckCoroutine != null)
        {
            StopCoroutine(delayedRootExitCheckCoroutine);
            delayedRootExitCheckCoroutine = null;
        }

        if (delayedRootActivationCoroutine != null)
        {
            StopCoroutine(delayedRootActivationCoroutine);
            delayedRootActivationCoroutine = null;
        }

        if (confirmedStandingActivationCoroutine != null)
        {
            StopCoroutine(confirmedStandingActivationCoroutine);
            confirmedStandingActivationCoroutine = null;
        }
    }

    void QueueRootActivation()
    {
        if (Movement.Instance == null)
            return;

        if (Movement.Instance.isRespawning)
            return;

        GameObject rootSourceBlock = GetRootSourceBlock();

        if (rootSourceBlock == null)
            return;

        bool playerIsStandingOnRootBlock =
            Movement.Instance.blockStandingOn == rootSourceBlock;

        bool playerIsMovingOntoRootBlock =
            Movement.Instance.currentMoveTargetBlock == rootSourceBlock;

        // The trigger can overlap before Movement knows the player is targeting/standing on this block.
        // In that case, let the coroutine wait shortly instead of activating immediately.
        if (!playerIsStandingOnRootBlock && !playerIsMovingOntoRootBlock && !playerIsInsideRootTrigger)
            return;

        if (activatedForCurrentRootStand)
            return;

        if (rootActivationQueued)
            return;

        queuedActivationDirection = GetRootDirectionFromCurrentMovement();

        rootActivationQueued = true;

        if (delayedRootActivationCoroutine != null)
        {
            StopCoroutine(delayedRootActivationCoroutine);
            delayedRootActivationCoroutine = null;
        }

        delayedRootActivationCoroutine = StartCoroutine(ActivateRootWhenPlayerIsOnOrMovingOntoRootBlock());
    }

    IEnumerator ActivateRootWhenPlayerIsOnOrMovingOntoRootBlock()
    {
        GameObject rootSourceBlock = GetRootSourceBlock();

        if (rootSourceBlock == null)
        {
            ClearActivationQueue();
            yield break;
        }

        // Wait one frame so Movement.currentMoveTargetBlock has time to be assigned.
        yield return null;

        while (Movement.Instance != null)
        {
            if (Movement.Instance.isRespawning)
            {
                ClearActivationQueue();
                yield break;
            }

            if (activatedForCurrentRootStand)
            {
                ClearActivationQueue();
                yield break;
            }

            bool playerIsStandingOnRootBlock =
                Movement.Instance.blockStandingOn == rootSourceBlock;

            bool playerIsMovingOntoRootBlock =
                Movement.Instance.currentMoveTargetBlock == rootSourceBlock;

            if (playerIsStandingOnRootBlock || playerIsMovingOntoRootBlock)
            {
                Vector3 activationDirection = queuedActivationDirection;

                if (activationDirection.sqrMagnitude < 0.001f)
                    activationDirection = GetRootDirectionFromCurrentMovement();

                ClearActivationQueue();

                ActivateRootOnceForCurrentStand(activationDirection);
                yield break;
            }

            // If the player is no longer inside/near the trigger and is not targeting the RootBlock, stop waiting.
            if (!playerIsInsideRootTrigger)
            {
                ClearActivationQueue();
                yield break;
            }

            yield return null;
        }

        ClearActivationQueue();
    }

    void ClearActivationQueue()
    {
        rootActivationQueued = false;
        delayedRootActivationCoroutine = null;
        queuedActivationDirection = Vector3.zero;
    }

    void ActivateRootOnceForCurrentStand(Vector3 rootDirection)
    {
        if (Movement.Instance == null)
            return;

        if (Movement.Instance.isRespawning)
            return;

        if (activatedForCurrentRootStand)
            return;

        GameObject rootSourceBlock = GetRootSourceBlock();

        if (rootSourceBlock == null)
            return;

        bool playerIsStandingOnRootBlock =
            Movement.Instance.blockStandingOn == rootSourceBlock;

        bool playerIsMovingOntoRootBlock =
            Movement.Instance.currentMoveTargetBlock == rootSourceBlock;

        if (!playerIsStandingOnRootBlock && !playerIsMovingOntoRootBlock)
            return;

        // Important: lock this BEFORE invoking the global reset event.
        activatedForCurrentRootStand = true;

        // Prevent this root block from resetting itself from its own static event.
        ignoreNextSelfReset = true;
        Action_StandingOnRootBlock_Early?.Invoke();
        ignoreNextSelfReset = false;

        ActivateRoots(rootDirection);
    }

    void CheckActivateFromConfirmedStanding()
    {
        if (!activateFromConfirmedStanding)
            return;

        if (confirmedStandingActivationCoroutine != null)
        {
            StopCoroutine(confirmedStandingActivationCoroutine);
            confirmedStandingActivationCoroutine = null;
        }

        confirmedStandingActivationCoroutine = StartCoroutine(CheckActivateFromConfirmedStanding_Delayed());
    }

    IEnumerator CheckActivateFromConfirmedStanding_Delayed()
    {
        yield return null;

        confirmedStandingActivationCoroutine = null;

        if (Movement.Instance == null)
            yield break;

        if (Movement.Instance.isRespawning)
            yield break;

        GameObject rootSourceBlock = GetRootSourceBlock();

        if (rootSourceBlock == null)
            yield break;

        bool playerIsStandingOnThisRootBlock =
            Movement.Instance.blockStandingOn == rootSourceBlock;

        if (!playerIsStandingOnThisRootBlock)
        {
            activatedForCurrentRootStand = false;
            yield break;
        }

        QueueRootActivation();
    }

    GameObject GetRootSourceBlock()
    {
        return transform.parent != null ? transform.parent.gameObject : null;
    }

    Vector3 GetRootDirectionFromCurrentMovement()
    {
        GameObject rootSourceBlock = GetRootSourceBlock();

        if (rootSourceBlock == null || Movement.Instance == null)
            return Vector3.forward;

        // Best case: player is currently moving onto the RootBlock.
        // Use the block they are moving FROM.
        if (Movement.Instance.currentMoveTargetBlock == rootSourceBlock &&
            Movement.Instance.blockStandingOn != null &&
            Movement.Instance.blockStandingOn != rootSourceBlock)
        {
            Vector3 dir = rootSourceBlock.transform.position - Movement.Instance.blockStandingOn.transform.position;
            return SnapDirection(dir);
        }

        // After landing: use previous standing block.
        if (Movement.Instance.blockStandingOn == rootSourceBlock &&
            Movement.Instance.blockStandingOn_Previous != null &&
            Movement.Instance.blockStandingOn_Previous != rootSourceBlock)
        {
            Vector3 dir = rootSourceBlock.transform.position - Movement.Instance.blockStandingOn_Previous.transform.position;
            return SnapDirection(dir);
        }

        // Fallback.
        return SnapDirection(Movement.Instance.lookingDirection);
    }

    Vector3 SnapDirection(Vector3 dir)
    {
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            return Vector3.forward;

        dir.Normalize();

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
            return dir.x > 0f ? Vector3.right : Vector3.left;

        return dir.z > 0f ? Vector3.forward : Vector3.back;
    }


    //--------------------


    public void ActivateRoots()
    {
        ActivateRoots(Vector3.zero);
    }

    public void ActivateRoots(Vector3 forcedRootDirection)
    {
        if (Movement.Instance == null)
            return;

        HardResetRootsBeforeRebuild();

        if (forcedRootDirection.sqrMagnitude > 0.001f)
            playerLookDir = SnapDirection(forcedRootDirection);
        else
            playerLookDir = GetRootDirectionFromCurrentMovement();

        isActive = true;

        if (anim != null)
            anim.SetTrigger("Activate");

        MakeRootFreeCostList();

        Movement.Instance.UpdateBlocks();
        Movement.Instance.SetDarkenBlocks();

        Action_StandingOnRootBlock?.Invoke();
    }

    void HardResetRootsBeforeRebuild()
    {
        isActive = false;
        onTrigger = false;

        if (rootAnimCoroutine != null)
        {
            StopCoroutine(rootAnimCoroutine);
            rootAnimCoroutine = null;
        }

        SetBlockMovementCostToDefault();

        for (int i = 0; i < RootObjectList.Count; i++)
        {
            if (RootObjectList[i] == null) continue;

            RootObjectList[i].SetActive(false);
            RootObjectList[i].transform.position = Vector3.zero;
        }

        RootFreeCostBlockList.Clear();
        finishedCheckingForBlocks = false;

        if (Movement.Instance != null)
        {
            Movement.Instance.ResetDarkenBlocks_External();
            Movement.Instance.UpdateBlocks();
            Movement.Instance.SetDarkenBlocks();
        }
    }


    //--------------------


    void CheckWhenToResetRootLine()
    {
        if (!isActive)
            return;

        if (Movement.Instance == null)
            return;

        if (delayedRootExitCheckCoroutine != null)
        {
            StopCoroutine(delayedRootExitCheckCoroutine);
            delayedRootExitCheckCoroutine = null;
        }

        GameObject blockPlayerStartedOn = Movement.Instance.blockStandingOn;
        GameObject actualTargetBlock = Movement.Instance.currentMoveTargetBlock;

        bool startedFromRootArea =
            IsBlockRooted(blockPlayerStartedOn) ||
            IsRootSourceBlock(blockPlayerStartedOn);

        bool targetKeepsRoots =
            IsBlockRooted(actualTargetBlock) ||
            IsRootSourceBlock(actualTargetBlock);

        bool shouldDelayBecausePlayerMayBeFalling =
            ShouldDelayRootResetBecausePlayerMayBeFalling(blockPlayerStartedOn, actualTargetBlock);

        // If the player clearly leaves rooted/root-source area onto a non-rooted block, remove immediately.
        // But do NOT remove immediately during falling / vertical slope movement, because the target block
        // may not be valid yet or may have a changed Y value.
        if (startedFromRootArea && !targetKeepsRoots && !shouldDelayBecausePlayerMayBeFalling)
        {
            DestroyRootFreeCostList();
            return;
        }

        delayedRootExitCheckCoroutine = StartCoroutine(
            CheckWhenToResetRootLine_Delayed(
                blockPlayerStartedOn,
                actualTargetBlock,
                shouldDelayBecausePlayerMayBeFalling
            )
        );
    }
    bool ShouldDelayRootResetBecausePlayerMayBeFalling(GameObject blockPlayerStartedOn, GameObject actualTargetBlock)
    {
        if (Movement.Instance == null)
            return false;

        MovementStates movementState = Movement.Instance.GetMovementState();

        if (movementState == MovementStates.Falling)
            return true;

        if (Movement.Instance.isMoving || movementState == MovementStates.Moving)
        {
            // During slope/fall transitions the target may briefly be null or not finalized yet.
            if (actualTargetBlock == null)
                return true;

            if (blockPlayerStartedOn == null)
                return false;

            float yDifference = Mathf.Abs(
                actualTargetBlock.transform.position.y -
                blockPlayerStartedOn.transform.position.y
            );

            // Any vertical block change should use the delayed reset path instead of instantly
            // destroying the roots.
            return yDifference > 0.01f;
        }

        return false;
    }

    bool IsRootSourceBlock(GameObject block)
    {
        if (!block)
            return false;

        GameObject rootSourceBlock = GetRootSourceBlock();

        return rootSourceBlock != null && block == rootSourceBlock;
    }

    bool ShouldBlockKeepRoots(GameObject block)
    {
        return IsBlockRooted(block) || IsRootSourceBlock(block);
    }

    IEnumerator CheckWhenToResetRootLine_Delayed(GameObject blockPlayerStartedOn, GameObject actualTargetBlock, bool delayedBecausePlayerMayBeFalling)
    {
        yield return null;

        while (isActive && Movement.Instance != null && Movement.Instance.blockStandingOn == blockPlayerStartedOn)
        {
            yield return null;
        }

        while (isActive &&
               Movement.Instance != null &&
               (Movement.Instance.isMoving ||
                Movement.Instance.GetMovementState() == MovementStates.Moving ||
                Movement.Instance.GetMovementState() == MovementStates.Falling))
        {
            yield return null;
        }

        // Give slope/fall landing one extra frame so Movement.blockStandingOn and live root
        // continuation can settle before deciding whether roots should be destroyed.
        if (delayedBecausePlayerMayBeFalling)
        {
            yield return null;

            if (isActive && Movement.Instance != null)
            {
                if (RootFreeCostBlockList.Count > 0)
                    TryContinueRootLineFromOpenSegments();

                if (keepCheckingFromRootBlockEvenWhenRootsExist ||
                    (keepCheckingFromRootBlockWhenNoRoots && RootFreeCostBlockList.Count <= 0))
                {
                    TryStartRootLineFromRootBlock();
                }
            }
        }

        yield return null;

        delayedRootExitCheckCoroutine = null;

        if (!isActive)
            yield break;

        if (Movement.Instance == null)
            yield break;

        if (ShouldBlockKeepRoots(actualTargetBlock))
            yield break;

        if (ShouldBlockKeepRoots(Movement.Instance.blockStandingOn))
            yield break;

        DestroyRootFreeCostList();
    }

    bool IsBlockRooted(GameObject block)
    {
        if (!block) return false;

        for (int i = 0; i < RootFreeCostBlockList.Count; i++)
        {
            if (RootFreeCostBlockList[i].block == block)
                return true;
        }

        return false;
    }


    //--------------------


    #region BuildRootLine

    void MakeRootFreeCostList()
    {
        Vector3 lookDir_Temp = playerLookDir;

        if (lookDir_Temp.sqrMagnitude < 0.001f)
            lookDir_Temp = GetRootDirectionFromCurrentMovement();

        lookDir_Temp = SnapDirection(lookDir_Temp);

        if (Movement.Instance.isRespawning &&
            transform.parent.gameObject &&
            transform.parent.gameObject.GetComponent<Block_Checkpoint>())
        {
            switch (transform.parent.gameObject.GetComponent<Block_Checkpoint>().spawnDirection)
            {
                case MovementDirection.None:
                    lookDir_Temp = -Vector3.forward;
                    break;

                case MovementDirection.Forward:
                    lookDir_Temp = -Vector3.forward;
                    break;

                case MovementDirection.Backward:
                    lookDir_Temp = -Vector3.back;
                    break;

                case MovementDirection.Left:
                    lookDir_Temp = -Vector3.left;
                    break;

                case MovementDirection.Right:
                    lookDir_Temp = -Vector3.right;
                    break;

                default:
                    lookDir_Temp = -Vector3.forward;
                    break;
            }
        }

        // Keep playerLookDir as the original direction from the RootBlock.
        // The local lookDir_Temp will change whenever the path reaches a slope.
        playerLookDir = SnapDirection(lookDir_Temp);
        tempOriginPos = transform.parent.position;

        int safetyCounter = 0;
        int maxSafetyIterations = Mathf.Max(1, RootObjectList.Count + 8);

        while (!finishedCheckingForBlocks)
        {
            safetyCounter++;

            if (safetyCounter > maxSafetyIterations)
            {
                Debug.LogWarning(
                    $"{nameof(Block_Root)} on {name}: Root path calculation stopped by safety counter.",
                    this
                );

                finishedCheckingForBlocks = true;
                break;
            }

            bool blockIsFound = false;

            #region Check in line

            GameObject tempBlock_Adjacent =
                RaycastBlock(tempOriginPos + Vector3.up * 0.3f, lookDir_Temp, 1f);

            if (tempBlock_Adjacent)
            {
                GameObject tempBlock_Over =
                    RaycastBlock(
                        tempBlock_Adjacent.transform.position + Vector3.up * 0.3f,
                        Vector3.up,
                        1f
                    );

                if (!tempBlock_Over ||
                    (tempBlock_Over &&
                     tempBlock_Over.GetComponent<BlockInfo>() &&
                     tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Slab))
                {
                    BlockInfo adjacentInfo = tempBlock_Adjacent.GetComponent<BlockInfo>();

                    if (adjacentInfo != null &&
                        (adjacentInfo.blockType == BlockType.Stair ||
                         adjacentInfo.blockType == BlockType.Slope))
                    {
                        StairSlopeCorrection(ref tempBlock_Adjacent, lookDir_Temp);

                        if (TryAddRootBlockAndUpdateDirection(
                                tempBlock_Adjacent,
                                true,
                                ref lookDir_Temp))
                        {
                            blockIsFound = true;
                        }
                    }
                    else
                    {
                        if (TryAddRootBlockAndUpdateDirection(
                                tempBlock_Adjacent,
                                false,
                                ref lookDir_Temp))
                        {
                            blockIsFound = true;
                        }
                    }
                }
                else
                {
                    BlockInfo overInfo = tempBlock_Over.GetComponent<BlockInfo>();

                    if (overInfo != null &&
                        (overInfo.blockType == BlockType.Stair ||
                         overInfo.blockType == BlockType.Slope))
                    {
                        StairSlopeCorrection(ref tempBlock_Over, lookDir_Temp);

                        if (TryAddRootBlockAndUpdateDirection(
                                tempBlock_Over,
                                true,
                                ref lookDir_Temp))
                        {
                            blockIsFound = true;
                        }
                    }
                }
            }
            else
            {
                GameObject tempBlock_UnderEmpty =
                    RaycastBlock(
                        tempOriginPos + Vector3.up * 0.3f + lookDir_Temp,
                        Vector3.down,
                        1.25f
                    );

                GameObject tempBlock_OverEmpty =
                    RaycastBlock(
                        tempOriginPos + Vector3.up * 0.3f + lookDir_Temp,
                        Vector3.up,
                        1.25f
                    );

                if (tempBlock_UnderEmpty &&
                    tempBlock_UnderEmpty.GetComponent<BlockInfo>() &&
                    (tempBlock_UnderEmpty.GetComponent<BlockInfo>().blockType == BlockType.Stair ||
                     tempBlock_UnderEmpty.GetComponent<BlockInfo>().blockType == BlockType.Slope) &&
                    RootFreeCostBlockList.Count > 0 &&
                    (RootFreeCostBlockList[RootFreeCostBlockList.Count - 1].blockType == BlockType.Stair ||
                     RootFreeCostBlockList[RootFreeCostBlockList.Count - 1].blockType == BlockType.Slope))
                {
                    StairSlopeCorrection(ref tempBlock_UnderEmpty, lookDir_Temp);

                    if (TryAddRootBlockAndUpdateDirection(
                            tempBlock_UnderEmpty,
                            true,
                            ref lookDir_Temp))
                    {
                        blockIsFound = true;
                    }
                }
                else if (tempBlock_OverEmpty &&
                         tempBlock_OverEmpty.GetComponent<BlockInfo>() &&
                         (tempBlock_OverEmpty.GetComponent<BlockInfo>().blockType == BlockType.Stair ||
                          tempBlock_OverEmpty.GetComponent<BlockInfo>().blockType == BlockType.Slope))
                {
                    StairSlopeCorrection(ref tempBlock_OverEmpty, lookDir_Temp);

                    if (TryAddRootBlockAndUpdateDirection(
                            tempBlock_OverEmpty,
                            true,
                            ref lookDir_Temp))
                    {
                        blockIsFound = true;
                    }
                }
            }

            #endregion

            #region Check diagonal down after stair/slope

            if (!blockIsFound && RootFreeCostBlockList.Count > 0)
            {
                BlockType lastType =
                    RootFreeCostBlockList[RootFreeCostBlockList.Count - 1].blockType;

                if (lastType == BlockType.Stair || lastType == BlockType.Slope)
                {
                    Vector3 origin =
                        tempOriginPos + lookDir_Temp + Vector3.up * 2.0f;

                    GameObject diagDown =
                        RaycastBlock(origin, Vector3.down, 5.0f);

                    if (diagDown &&
                        diagDown.transform.position.y < tempOriginPos.y - 0.25f)
                    {
                        if (TryAddRootBlockAndUpdateDirection(
                                diagDown,
                                false,
                                ref lookDir_Temp))
                        {
                            blockIsFound = true;
                        }
                    }
                }
            }

            #endregion

            #region Check diagonal up after stair/slope

            if (!blockIsFound && RootFreeCostBlockList.Count > 0)
            {
                BlockType lastType =
                    RootFreeCostBlockList[RootFreeCostBlockList.Count - 1].blockType;

                if (lastType == BlockType.Stair || lastType == BlockType.Slope)
                {
                    Vector3 origin =
                        tempOriginPos + lookDir_Temp + Vector3.up * 0.1f;

                    GameObject diagUp =
                        RaycastBlock(origin, Vector3.up, 5.0f);

                    if (diagUp &&
                        diagUp.transform.position.y > tempOriginPos.y + 0.25f)
                    {
                        if (TryAddRootBlockAndUpdateDirection(
                                diagUp,
                                false,
                                ref lookDir_Temp))
                        {
                            blockIsFound = true;
                        }
                    }
                }
            }

            #endregion

            #region Check stairs/slopes

            if (!blockIsFound)
            {
                tempBlock_Adjacent =
                    RaycastBlock(tempOriginPos, lookDir_Temp, 1f);

                GameObject tempBlock_Over =
                    RaycastBlock(tempOriginPos + Vector3.up, lookDir_Temp, 1f);

                if (tempBlock_Adjacent &&
                    tempBlock_Adjacent.GetComponent<BlockInfo>() &&
                    (tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Stair ||
                     tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Slope))
                {
                    StairSlopeCorrection(ref tempBlock_Adjacent, lookDir_Temp);

                    if (TryAddRootBlockAndUpdateDirection(
                            tempBlock_Adjacent,
                            true,
                            ref lookDir_Temp))
                    {
                        blockIsFound = true;
                    }
                }

                if (!blockIsFound &&
                    tempBlock_Adjacent &&
                    tempBlock_Over &&
                    tempBlock_Over.GetComponent<BlockInfo>() &&
                    (tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Stair ||
                     tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Slope))
                {
                    StairSlopeCorrection(ref tempBlock_Over, lookDir_Temp);

                    if (TryAddRootBlockAndUpdateDirection(
                            tempBlock_Over,
                            true,
                            ref lookDir_Temp))
                    {
                        blockIsFound = true;
                    }
                }
            }

            #endregion

            #region Check Ladder

            if (!blockIsFound)
            {
                GameObject tempBlock_Ladder_Up =
                    RaycastLadder(tempOriginPos + Vector3.up, lookDir_Temp, 1.5f);

                GameObject tempBlock_Ladder_Down =
                    RaycastLadder(tempOriginPos, lookDir_Temp, 1f);

                if (tempBlock_Ladder_Up)
                {
                    Block_Ladder ladder = tempBlock_Ladder_Up.GetComponent<Block_Ladder>();

                    if (ladder &&
                        ladder.exitBlock_Up &&
                        ladder.exitBlock_Up.GetComponent<BlockInfo>())
                    {
                        if (TryAddRootBlockAndUpdateDirection(
                                ladder.exitBlock_Up,
                                false,
                                ref lookDir_Temp))
                        {
                            blockIsFound = true;
                        }
                    }
                }
                else if (tempBlock_Ladder_Down)
                {
                    Block_Ladder ladder = tempBlock_Ladder_Down.GetComponent<Block_Ladder>();

                    if (ladder &&
                        ladder.exitBlock_Down &&
                        ladder.exitBlock_Down.GetComponent<BlockInfo>())
                    {
                        if (TryAddRootBlockAndUpdateDirection(
                                ladder.exitBlock_Down,
                                false,
                                ref lookDir_Temp))
                        {
                            blockIsFound = true;
                        }
                    }
                }
            }

            #endregion

            #region Check after slope after falling

            if (!blockIsFound)
            {
                GameObject tempBlock_Falling = null;

                if (RootFreeCostBlockList.Count > 0 &&
                    RootFreeCostBlockList[RootFreeCostBlockList.Count - 1].blockType == BlockType.Slope)
                {
                    tempBlock_Falling =
                        RaycastBlock(tempOriginPos + lookDir_Temp, Vector3.down, 50f);
                }

                if (tempBlock_Falling)
                {
                    BlockInfo fallingInfo = tempBlock_Falling.GetComponent<BlockInfo>();

                    bool fallingBlockIsStairOrSlope =
                        fallingInfo != null &&
                        (fallingInfo.blockType == BlockType.Stair ||
                         fallingInfo.blockType == BlockType.Slope);

                    if (TryAddRootBlockAndUpdateDirection(
                            tempBlock_Falling,
                            fallingBlockIsStairOrSlope,
                            ref lookDir_Temp))
                    {
                        blockIsFound = true;
                    }
                }
            }

            #endregion

            if (!blockIsFound)
            {
                finishedCheckingForBlocks = true;
            }

            if (RootFreeCostBlockList.Count >= RootObjectList.Count)
            {
                finishedCheckingForBlocks = true;
            }
        }

        SetRootLineObjectsOrientation();
        MakeRootObjectsVisible();
        ChangeBlockMovementCost();
    }

    int SetupEntryInBlockList(GameObject tempBlock, bool stair, Vector3 incomingTravelDir)
    {
        if (tempBlock == null)
            return 0;

        incomingTravelDir = SnapDirection(incomingTravelDir);

        GameObject linkedTeleportBlock = null;
        bool shouldTeleportRoots =
            TryGetLinkedTeleportBlock(tempBlock, out linkedTeleportBlock);

        int addedCount = 0;

        // If this block is a teleporter with a valid linked teleporter,
        // this block should receive roots, but roots should NOT continue from it.
        bool blockShouldStopContinuation = shouldTeleportRoots;

        // The teleport effect should happen later, when the root object
        // is actually activated on this teleporter block.
        bool triggerTeleportEffectWhenRootActivates = shouldTeleportRoots;

        if (AddRootEntryToBlockList(
                tempBlock,
                stair,
                blockShouldStopContinuation,
                incomingTravelDir,
                triggerTeleportEffectWhenRootActivates))
        {
            addedCount++;
        }
        else
        {
            return addedCount;
        }

        tempOriginPos = tempBlock.transform.position;

        // This is normally the incoming direction, but becomes the slope's
        // facing direction when tempBlock is a slope.
        Vector3 directionAfterFirstBlock =
            GetLastRootTravelDirection(incomingTravelDir);

        // Teleporter redirect:
        // roots hit teleporter A, then appear on linked teleporter B,
        // and continuation continues from B.
        if (shouldTeleportRoots)
        {
            if (linkedTeleportBlock == null)
                return addedCount;

            if (RootFreeCostBlockList.Count >= RootObjectList.Count)
            {
                Debug.LogWarning(
                    $"{nameof(Block_Root)} on {name}: Not enough root objects in RootObjectList for linked teleporter root.",
                    this
                );

                finishedCheckingForBlocks = true;
                return addedCount;
            }

            if (!IsBlockAlreadyInRootLine(linkedTeleportBlock))
            {
                BlockInfo linkedInfo = linkedTeleportBlock.GetComponent<BlockInfo>();

                if (linkedInfo != null)
                {
                    bool linkedIsStairOrSlope =
                        linkedInfo.blockType == BlockType.Stair ||
                        linkedInfo.blockType == BlockType.Slope;

                    if (AddRootEntryToBlockList(
                            linkedTeleportBlock,
                            linkedIsStairOrSlope,
                            false,
                            directionAfterFirstBlock))
                    {
                        addedCount++;
                    }
                }
            }

            tempOriginPos = linkedTeleportBlock.transform.position;
        }

        return addedCount;
    }

    bool AddRootEntryToBlockList(GameObject tempBlock, bool stair, bool stopContinuationFromThisBlock, Vector3 incomingTravelDir, bool triggerTeleportEffectWhenRootActivates = false)
    {
        if (tempBlock == null)
            return false;

        if (RootFreeCostBlockList.Count >= RootObjectList.Count)
        {
            Debug.LogWarning(
                $"{nameof(Block_Root)} on {name}: Not enough root objects in RootObjectList.",
                this
            );

            finishedCheckingForBlocks = true;
            return false;
        }

        if (IsBlockAlreadyInRootLine(tempBlock))
        {
            finishedCheckingForBlocks = true;
            return false;
        }

        BlockInfo info = tempBlock.GetComponent<BlockInfo>();

        if (info == null)
            return false;

        RootBlockLineInfo rootBlockLineInfo = new RootBlockLineInfo();

        rootBlockLineInfo.block = tempBlock;
        rootBlockLineInfo.originalMovemetCost = info.movementCost_Temp_Base;
        rootBlockLineInfo.blockType = info.blockType;

        // facingDir now stores the direction that roots should continue
        // in after reaching this block.
        rootBlockLineInfo.facingDir =
            GetOutgoingRootDirection(tempBlock, incomingTravelDir);

        rootBlockLineInfo.stopContinuationFromThisBlock =
            stopContinuationFromThisBlock;

        rootBlockLineInfo.triggerTeleportEffectWhenRootActivates =
            triggerTeleportEffectWhenRootActivates;

        rootBlockLineInfo.teleportEffectHasTriggered = false;

        Block_Water water = rootBlockLineInfo.block.GetComponent<Block_Water>();

        if (water)
        {
            water.hasRoots = true;
        }

        RootFreeCostBlockList.Add(rootBlockLineInfo);

        return true;
    }
    void ActivateRootTeleportEffect(GameObject teleportBlock)
    {
        if (teleportBlock == null)
            return;

        Block_Teleport teleporter = teleportBlock.GetComponent<Block_Teleport>();

        if (teleporter == null)
            return;

        teleporter.ActivateRootTeleportEffect();
    }

    bool TryGetLinkedTeleportBlock(GameObject block, out GameObject linkedTeleportBlock)
    {
        linkedTeleportBlock = null;

        if (block == null)
            return false;

        Block_Teleport teleporter = block.GetComponent<Block_Teleport>();

        if (teleporter == null)
            return false;

        if (teleporter.newLandingSpot == null)
            return false;

        if (teleporter.newLandingSpot == block)
            return false;

        if (teleporter.newLandingSpot.GetComponent<BlockInfo>() == null)
            return false;

        if (IsBlockAlreadyInRootLine(teleporter.newLandingSpot))
        {
            finishedCheckingForBlocks = true;
            return false;
        }

        linkedTeleportBlock = teleporter.newLandingSpot;
        return true;
    }


    //-----


    GameObject RaycastBlock(Vector3 origin, Vector3 dir, float distance)
    {
        if (Physics.Raycast(origin, dir, out hit, distance, 1 << 15)) //15 = BlockLayer
        {
            if (hit.collider.transform.gameObject.GetComponent<BlockInfo>())
            {
                return hit.collider.transform.gameObject;
            }
        }

        return null;
    }

    GameObject RaycastLadder(Vector3 origin, Vector3 dir, float distance)
    {
        if (Physics.Raycast(origin, dir, out hit, distance, 1 << 20)) //20 = LadderLayer
        {
            Debug.DrawLine(origin, origin + dir, Color.cyan, 2);

            if (hit.collider.transform.parent.gameObject.GetComponent<Block_Ladder>())
            {
                return hit.collider.transform.parent.gameObject;
            }
        }

        return null;
    }

    void StairSlopeCorrection(ref GameObject obj, Vector3 travelDir)
    {
        if (obj == null)
            return;

        BlockInfo info = obj.GetComponent<BlockInfo>();

        if (info == null)
        {
            obj = null;
            return;
        }

        if (info.blockType != BlockType.Stair &&
            info.blockType != BlockType.Slope)
        {
            return;
        }

        Vector3 stairDir = SnapDirection(obj.transform.forward);
        travelDir = SnapDirection(travelDir);

        float dot = Vector3.Dot(stairDir, travelDir);
        float y = obj.transform.position.y;

        bool ok;
        const float eps = 0.05f;

        bool Approx(float a, float b) => Mathf.Abs(a - b) < eps;

        if (dot < 0f)
        {
            ok = Approx(tempOriginPos.y, y - 0.5f) ||
                 Approx(tempOriginPos.y, y - 1.0f) ||
                 Approx(tempOriginPos.y, y - 1.5f);
        }
        else
        {
            ok = Approx(tempOriginPos.y, y + 0.5f) ||
                 Approx(tempOriginPos.y, y + 1.0f) ||
                 Approx(tempOriginPos.y, y + 1.5f);
        }

        if (!ok)
        {
            Debug.LogWarning(
                $"REJECT stair/slope {obj.name}: originY={tempOriginPos.y} blockY={y} dot={dot}"
            );

            obj = null;
        }
    }

    void SetRootLineObjectsOrientation()
    {
        Vector3 fallbackDir = GetRootTravelDirection();

        for (int i = 0; i < RootFreeCostBlockList.Count; i++)
        {
            RootBlockLineInfo rootInfo = RootFreeCostBlockList[i];

            if (rootInfo == null)
                continue;

            GameObject block = rootInfo.block;
            BlockType type = rootInfo.blockType;

            if (block == null)
                continue;

            if (i >= RootObjectList.Count || RootObjectList[i] == null)
                continue;

            Vector3 segmentDir =
                GetRootDirectionForSegment(rootInfo, fallbackDir);

            fallbackDir = segmentDir;

            if (type == BlockType.Stair || type == BlockType.Slope)
            {
                bool isDownStep = false;

                if (i > 0 && RootFreeCostBlockList[i - 1].block != null)
                {
                    float prevY =
                        RootFreeCostBlockList[i - 1].block.transform.position.y;

                    float currY =
                        block.transform.position.y;

                    isDownStep = currY < prevY - 0.01f;
                }

                PlaceAndOrientRootOnStairOrSlope(
                    i,
                    segmentDir,
                    isDownStep
                );
            }
            else
            {
                RootObjectList[i].transform.SetPositionAndRotation(
                    block.transform.position,
                    Quaternion.LookRotation(segmentDir)
                );
            }
        }
    }

    void PlaceAndOrientRootOnStairOrSlope(int i, Vector3 travelDir, bool isDownStep)
    {
        var block = RootFreeCostBlockList[i].block;
        var root = RootObjectList[i];

        if (block == null || root == null)
            return;

        travelDir = SnapDirection(travelDir);

        Vector3 rootForward = isDownStep ? -travelDir : travelDir;
        float forwardOffset = isDownStep ? stairDownForwardOffset : stairUpForwardOffset;

        Vector3 pos = block.transform.position
                      + Vector3.up * stairUpHeightOffset
                      + travelDir * forwardOffset;

        root.transform.SetPositionAndRotation(pos, Quaternion.LookRotation(rootForward));
        root.transform.Rotate(stairSurfaceTilt, 0f, 0f, Space.Self);
    }

    void MakeRootObjectsVisible()
    {
        if (RootFreeCostBlockList.Count <= 0)
            return;

        if (rootAnimCoroutine != null)
        {
            StopCoroutine(rootAnimCoroutine);
            rootAnimCoroutine = null;
        }

        rootAnimCoroutine = StartCoroutine(AnimationDelay(0.04f));
    }

    IEnumerator AnimationDelay(float waitTime)
    {
        int rootsToAnimate = RootFreeCostBlockList.Count;

        for (int i = 0; i < rootsToAnimate; i++)
        {
            yield return new WaitForSeconds(waitTime);

            ActivateSingleRootObject(i);
        }

        rootAnimCoroutine = null;
    }

    void ChangeBlockMovementCost()
    {
        for (int i = 0; i < RootFreeCostBlockList.Count; i++)
        {
            if (RootFreeCostBlockList[i].block == null)
                continue;

            BlockInfo info = RootFreeCostBlockList[i].block.GetComponent<BlockInfo>();

            if (info == null)
                continue;

            info.SetTemporaryMovementCostOverride(0);
        }
    }

    #endregion


    //--------------------


    #region DestroyRootLine

    void DestroyRootFreeCostList()
    {
        isActive = false;

        rootActivationQueued = false;
        queuedActivationDirection = Vector3.zero;

        // Do NOT always reset activatedForCurrentRootStand here.
        // It should only reset when the player is no longer standing on this RootBlock.
        if (!IsPlayerStandingOnRootSourceBlock())
        {
            activatedForCurrentRootStand = false;
        }

        if (delayedRootActivationCoroutine != null)
        {
            StopCoroutine(delayedRootActivationCoroutine);
            delayedRootActivationCoroutine = null;
        }

        if (rootAnimCoroutine != null)
        {
            StopCoroutine(rootAnimCoroutine);
            rootAnimCoroutine = null;
        }

        if (Movement.Instance != null)
            playerLookDir = Movement.Instance.lookingDirection;

        ResetBlockLineObjects();
        SetBlockMovementCostToDefault();

        RootFreeCostBlockList.Clear();

        finishedCheckingForBlocks = false;
        onTrigger = false;

        if (Movement.Instance != null)
        {
            Movement.Instance.ResetDarkenBlocks_External();
            Movement.Instance.UpdateBlocks();
            Movement.Instance.SetDarkenBlocks();
        }
    }


    //-----


    void ResetBlockLineObjects()
    {
        for (int i = 0; i < RootObjectList.Count; i++)
        {
            if (RootObjectList[i] == null) continue;

            if (RootObjectList[i].activeInHierarchy)
            {
                Animator rootAnimator = RootObjectList[i].GetComponent<Animator>();

                if (rootAnimator != null)
                {
                    rootAnimator.ResetTrigger("Activate");
                    rootAnimator.SetTrigger("Deactivate");
                }

                RootObjectList[i].SetActive(false);
            }

            RootObjectList[i].transform.position = Vector3.zero;
        }
    }

    void SetBlockMovementCostToDefault()
    {
        for (int i = 0; i < RootFreeCostBlockList.Count; i++)
        {
            GameObject block = RootFreeCostBlockList[i].block;

            if (block == null)
                continue;

            if (block.GetComponent<Block_Water>())
            {
                block.GetComponent<Block_Water>().hasRoots = false;
            }

            BlockInfo info = block.GetComponent<BlockInfo>();

            if (info == null)
                continue;

            info.ClearTemporaryMovementCostOverride();
        }
    }

    #endregion


    #region LiveRootContinuation

    void UpdateLiveRootContinuation()
    {
        if (!checkForRootContinuation) return;
        if (!isActive) return;

        if (RootFreeCostBlockList.Count > 0)
        {
            SetRootLineObjectsOrientation();
        }

        if (Time.time < nextContinuationCheckTime) return;
        nextContinuationCheckTime = Time.time + continuationCheckInterval;

        if (RootFreeCostBlockList.Count > 0)
        {
            TryContinueRootLineFromOpenSegments();
        }

        // Let the RootBlock source keep checking while this root line is active,
        // even if the player is no longer standing on the RootBlock.
        if (keepCheckingFromRootBlockEvenWhenRootsExist ||
            (keepCheckingFromRootBlockWhenNoRoots && RootFreeCostBlockList.Count <= 0))
        {
            TryStartRootLineFromRootBlock();
        }
    }

    bool IsPlayerStandingOnRootSourceBlock()
    {
        if (Movement.Instance == null)
            return false;

        GameObject rootSourceBlock = GetRootSourceBlock();

        if (rootSourceBlock == null)
            return false;

        return Movement.Instance.blockStandingOn == rootSourceBlock;
    }

    void TryStartRootLineFromRootBlock()
    {
        if (RootObjectList.Count <= 0)
        {
            Debug.LogWarning(
                $"{nameof(Block_Root)} on {name}: No root objects assigned in RootObjectList.",
                this
            );

            return;
        }

        if (RootFreeCostBlockList.Count >= RootObjectList.Count)
            return;

        Vector3 lookDir_Temp = GetRootTravelDirection();

        GameObject rootParentBlock = GetRootSourceBlock();

        if (rootParentBlock == null)
            return;

        GameObject firstBlock =
            FindNextBlockFromRootBlock(rootParentBlock, lookDir_Temp);

        if (!firstBlock)
            return;

        if (IsBlockAlreadyInRootLine(firstBlock))
            return;

        BlockInfo firstInfo = firstBlock.GetComponent<BlockInfo>();

        if (!firstInfo)
            return;

        bool firstIsStairOrSlope =
            firstInfo.blockType == BlockType.Stair ||
            firstInfo.blockType == BlockType.Slope;

        int newRootIndex = RootFreeCostBlockList.Count;

        int addedRootCount =
            SetupEntryInBlockList(
                firstBlock,
                firstIsStairOrSlope,
                lookDir_Temp
            );

        if (addedRootCount <= 0)
            return;

        SetRootLineObjectsOrientation();

        for (int i = newRootIndex; i < newRootIndex + addedRootCount; i++)
        {
            ActivateSingleRootObject(i);
        }

        ChangeBlockMovementCost();

        if (Movement.Instance != null)
        {
            Movement.Instance.UpdateBlocks();
            Movement.Instance.SetDarkenBlocks();
        }
    }

    GameObject FindNextBlockFromRootBlock(GameObject rootBlock, Vector3 lookDir_Temp)
    {
        if (!rootBlock)
            return null;

        RootBlockLineInfo temporarySource = new RootBlockLineInfo();
        temporarySource.block = rootBlock;
        temporarySource.facingDir = SnapDirection(lookDir_Temp);

        BlockInfo rootInfo = rootBlock.GetComponent<BlockInfo>();

        if (rootInfo != null)
            temporarySource.blockType = rootInfo.blockType;
        else
            temporarySource.blockType = BlockType.Cube;

        return FindNextBlockForLiveContinuationFrom(
            temporarySource,
            lookDir_Temp
        );
    }

    void TryContinueRootLineFromOpenSegments()
    {
        if (RootFreeCostBlockList.Count <= 0)
            return;

        Vector3 fallbackDirection = GetRootTravelDirection();

        int addedCount = 0;
        int rootCountAtStart = RootFreeCostBlockList.Count;

        for (int i = 0; i < rootCountAtStart; i++)
        {
            if (addedCount >= maxContinuationSegmentsPerCheck)
                break;

            if (RootFreeCostBlockList.Count >= RootObjectList.Count)
            {
                Debug.LogWarning(
                    $"{nameof(Block_Root)} on {name}: Not enough root objects in RootObjectList to continue root line.",
                    this
                );

                break;
            }

            RootBlockLineInfo sourceRootInfo = RootFreeCostBlockList[i];

            if (sourceRootInfo == null || sourceRootInfo.block == null)
                continue;

            if (sourceRootInfo.stopContinuationFromThisBlock)
                continue;

            Vector3 sourceTravelDirection =
                GetRootDirectionForSegment(
                    sourceRootInfo,
                    fallbackDirection
                );

            if (HasRootSegmentAdjacentInDirection(
                    sourceRootInfo.block,
                    sourceTravelDirection))
            {
                continue;
            }

            GameObject nextBlock =
                FindNextBlockForLiveContinuationFrom(
                    sourceRootInfo,
                    sourceTravelDirection
                );

            if (!nextBlock)
                continue;

            if (IsBlockAlreadyInRootLine(nextBlock))
                continue;

            BlockInfo nextInfo = nextBlock.GetComponent<BlockInfo>();

            if (!nextInfo)
                continue;

            bool nextIsStairOrSlope =
                nextInfo.blockType == BlockType.Stair ||
                nextInfo.blockType == BlockType.Slope;

            int newRootIndex = RootFreeCostBlockList.Count;

            int addedRootCount =
                SetupEntryInBlockList(
                    nextBlock,
                    nextIsStairOrSlope,
                    sourceTravelDirection
                );

            if (addedRootCount <= 0)
                continue;

            SetRootLineObjectsOrientation();

            for (int rootIndex = newRootIndex;
                 rootIndex < newRootIndex + addedRootCount;
                 rootIndex++)
            {
                ActivateSingleRootObject(rootIndex);
            }

            ChangeBlockMovementCost();

            if (Movement.Instance != null)
            {
                Movement.Instance.UpdateBlocks();
                Movement.Instance.SetDarkenBlocks();
            }

            addedCount += addedRootCount;
        }
    }

    GameObject FindNextBlockForLiveContinuationFrom(RootBlockLineInfo sourceRootInfo, Vector3 lookDir_Temp)
    {
        if (sourceRootInfo == null || sourceRootInfo.block == null)
            return null;

        // Use the direction stored on this specific root segment.
        // For a slope, this will be the slope's facing direction.
        lookDir_Temp =
            GetRootDirectionForSegment(sourceRootInfo, lookDir_Temp);

        tempOriginPos = sourceRootInfo.block.transform.position;

        bool blockIsFound = false;
        GameObject foundBlock = null;

        #region Check in line

        GameObject tempBlock_Adjacent = RaycastBlock(tempOriginPos + (Vector3.up * 0.3f), lookDir_Temp, 1f);

        if (tempBlock_Adjacent)
        {
            GameObject tempBlock_Over = RaycastBlock(tempBlock_Adjacent.transform.position + (Vector3.up * 0.3f), Vector3.up, 1f);

            if (!tempBlock_Over || (tempBlock_Over && tempBlock_Over.GetComponent<BlockInfo>() && tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Slab))
            {
                if (tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Stair ||
                    tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                {
                    StairSlopeCorrectionLive(ref tempBlock_Adjacent, lookDir_Temp);

                    if (tempBlock_Adjacent)
                    {
                        foundBlock = tempBlock_Adjacent;
                        blockIsFound = true;
                    }
                }
                else
                {
                    foundBlock = tempBlock_Adjacent;
                    blockIsFound = true;
                }
            }
            else
            {
                if (tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Stair ||
                    tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                {
                    StairSlopeCorrectionLive(ref tempBlock_Over, lookDir_Temp);

                    if (tempBlock_Over)
                    {
                        foundBlock = tempBlock_Over;
                        blockIsFound = true;
                    }
                }
            }
        }
        else
        {
            GameObject tempBlock_UnderEmpty = RaycastBlock(tempOriginPos + (Vector3.up * 0.3f) + lookDir_Temp, Vector3.down, 1.25f);
            GameObject tempBlock_OverEmpty = RaycastBlock(tempOriginPos + (Vector3.up * 0.3f) + lookDir_Temp, Vector3.up, 1.25f);

            if (tempBlock_UnderEmpty &&
                (tempBlock_UnderEmpty.GetComponent<BlockInfo>().blockType == BlockType.Stair ||
                 tempBlock_UnderEmpty.GetComponent<BlockInfo>().blockType == BlockType.Slope) &&
                (sourceRootInfo.blockType == BlockType.Stair || sourceRootInfo.blockType == BlockType.Slope))
            {
                StairSlopeCorrectionLive(ref tempBlock_UnderEmpty, lookDir_Temp);

                if (tempBlock_UnderEmpty)
                {
                    foundBlock = tempBlock_UnderEmpty;
                    blockIsFound = true;
                }
            }
            else if (tempBlock_OverEmpty &&
                     (tempBlock_OverEmpty.GetComponent<BlockInfo>().blockType == BlockType.Stair ||
                      tempBlock_OverEmpty.GetComponent<BlockInfo>().blockType == BlockType.Slope))
            {
                StairSlopeCorrectionLive(ref tempBlock_OverEmpty, lookDir_Temp);

                if (tempBlock_OverEmpty)
                {
                    foundBlock = tempBlock_OverEmpty;
                    blockIsFound = true;
                }
            }
        }

        #endregion

        #region Check diagonal down after stair/slope

        if (!blockIsFound)
        {
            var sourceType = sourceRootInfo.blockType;

            if (sourceType == BlockType.Stair || sourceType == BlockType.Slope)
            {
                Vector3 origin = tempOriginPos + lookDir_Temp + Vector3.up * 2.0f;
                GameObject diagDown = RaycastBlock(origin, Vector3.down, 5.0f);

                if (diagDown && diagDown.transform.position.y < tempOriginPos.y - 0.25f)
                {
                    foundBlock = diagDown;
                    blockIsFound = true;
                }
            }
        }

        #endregion

        #region Check diagonal up after stair/slope

        if (!blockIsFound)
        {
            var sourceType = sourceRootInfo.blockType;

            if (sourceType == BlockType.Stair || sourceType == BlockType.Slope)
            {
                Vector3 origin = tempOriginPos + lookDir_Temp + Vector3.up * 0.1f;
                GameObject diagUp = RaycastBlock(origin, Vector3.up, 5.0f);

                if (diagUp && diagUp.transform.position.y > tempOriginPos.y + 0.25f)
                {
                    foundBlock = diagUp;
                    blockIsFound = true;
                }
            }
        }

        #endregion

        #region Check stairs/slopes

        if (!blockIsFound)
        {
            tempBlock_Adjacent = RaycastBlock(tempOriginPos, lookDir_Temp, 1f);
            GameObject tempBlock_Over = RaycastBlock(tempOriginPos + Vector3.up, lookDir_Temp, 1f);

            if (tempBlock_Adjacent &&
                (tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Stair ||
                 tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Slope))
            {
                StairSlopeCorrectionLive(ref tempBlock_Adjacent, lookDir_Temp);

                if (tempBlock_Adjacent)
                {
                    foundBlock = tempBlock_Adjacent;
                    blockIsFound = true;
                }
            }

            if (!blockIsFound &&
                tempBlock_Adjacent &&
                tempBlock_Over &&
                (tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Stair ||
                 tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Slope))
            {
                StairSlopeCorrectionLive(ref tempBlock_Over, lookDir_Temp);

                if (tempBlock_Over)
                {
                    foundBlock = tempBlock_Over;
                    blockIsFound = true;
                }
            }
        }

        #endregion

        #region Check Ladder

        if (!blockIsFound)
        {
            GameObject tempBlock_Ladder_Up = RaycastLadder(tempOriginPos + Vector3.up, lookDir_Temp, 1.5f);
            GameObject tempBlock_Ladder_Down = RaycastLadder(tempOriginPos, lookDir_Temp, 1f);

            if (tempBlock_Ladder_Up)
            {
                if (tempBlock_Ladder_Up.GetComponent<Block_Ladder>() &&
                    tempBlock_Ladder_Up.GetComponent<Block_Ladder>().exitBlock_Up &&
                    tempBlock_Ladder_Up.GetComponent<Block_Ladder>().exitBlock_Up.GetComponent<BlockInfo>())
                {
                    foundBlock = tempBlock_Ladder_Up.GetComponent<Block_Ladder>().exitBlock_Up;
                    blockIsFound = true;
                }
            }
            else if (tempBlock_Ladder_Down)
            {
                if (tempBlock_Ladder_Down.GetComponent<Block_Ladder>() &&
                    tempBlock_Ladder_Down.GetComponent<Block_Ladder>().exitBlock_Down &&
                    tempBlock_Ladder_Down.GetComponent<Block_Ladder>().exitBlock_Down.GetComponent<BlockInfo>())
                {
                    foundBlock = tempBlock_Ladder_Down.GetComponent<Block_Ladder>().exitBlock_Down;
                    blockIsFound = true;
                }
            }
        }

        #endregion

        #region Check after slope after falling

        if (!blockIsFound)
        {
            GameObject tempBlock_Falling = null;

            if (sourceRootInfo.blockType == BlockType.Slope)
            {
                tempBlock_Falling = RaycastBlock(tempOriginPos + lookDir_Temp, Vector3.down, 50f);
            }

            if (tempBlock_Falling)
            {
                foundBlock = tempBlock_Falling;
                blockIsFound = true;
            }
        }

        #endregion

        if (!blockIsFound)
            return null;

        if (!foundBlock)
            return null;

        if (!foundBlock.GetComponent<BlockInfo>())
            return null;

        return foundBlock;
    }

    Vector3 GetRootTravelDirection()
    {
        Vector3 lookDir_Temp = playerLookDir;

        if (lookDir_Temp.sqrMagnitude < 0.001f)
            lookDir_Temp = GetRootDirectionFromCurrentMovement();

        return SnapDirection(lookDir_Temp);
    }

    bool HasRootSegmentAdjacentInDirection(GameObject sourceBlock, Vector3 dir)
    {
        if (!sourceBlock) return false;

        Vector3 sourcePos = sourceBlock.transform.position;

        dir = SnapDirection(dir);

        for (int i = 0; i < RootFreeCostBlockList.Count; i++)
        {
            GameObject otherBlock = RootFreeCostBlockList[i].block;

            if (!otherBlock) continue;
            if (otherBlock == sourceBlock) continue;

            Vector3 offset = otherBlock.transform.position - sourcePos;
            Vector3 horizontalOffset = new Vector3(offset.x, 0f, offset.z);

            float forwardDistance = Vector3.Dot(horizontalOffset, dir);
            float sideDistance = Vector3.Distance(horizontalOffset, dir * forwardDistance);

            bool isInFront = forwardDistance > 0.75f && forwardDistance < 1.25f;
            bool isSameLine = sideDistance < 0.25f;
            bool isCloseEnoughVertically = Mathf.Abs(offset.y) <= 1.6f;

            if (isInFront && isSameLine && isCloseEnoughVertically)
            {
                return true;
            }
        }

        return false;
    }

    void ActivateSingleRootObject(int index)
    {
        if (index < 0) return;
        if (index >= RootObjectList.Count) return;
        if (RootObjectList[index] == null) return;

        if (RootObjectList[index].activeSelf)
            return;

        RootObjectList[index].SetActive(true);

        Animator rootAnimator = RootObjectList[index].GetComponent<Animator>();

        if (rootAnimator)
        {
            rootAnimator.ResetTrigger("Deactivate");
            rootAnimator.ResetTrigger("Activate");
            rootAnimator.SetTrigger("Activate");
        }

        ActivateTeleportEffectForRootSegment(index);
    }
    void ActivateTeleportEffectForRootSegment(int index)
    {
        if (index < 0) return;
        if (index >= RootFreeCostBlockList.Count) return;

        RootBlockLineInfo rootInfo = RootFreeCostBlockList[index];

        if (rootInfo == null)
            return;

        if (!rootInfo.triggerTeleportEffectWhenRootActivates)
            return;

        if (rootInfo.teleportEffectHasTriggered)
            return;

        rootInfo.teleportEffectHasTriggered = true;

        ActivateRootTeleportEffect(rootInfo.block);
    }

    bool IsBlockAlreadyInRootLine(GameObject block)
    {
        if (!block) return false;

        for (int i = 0; i < RootFreeCostBlockList.Count; i++)
        {
            if (RootFreeCostBlockList[i].block == block)
                return true;
        }

        return false;
    }
    bool ShouldStopRootPathBecauseBlockAlreadyHasRoots(GameObject block)
    {
        if (!block)
            return false;

        return IsBlockAlreadyInRootLine(block);
    }

    void StairSlopeCorrectionLive(ref GameObject obj, Vector3 travelDir)
    {
        if (!obj) return;

        var info = obj.GetComponent<BlockInfo>();

        if (!info)
        {
            obj = null;
            return;
        }

        if (info.blockType != BlockType.Stair && info.blockType != BlockType.Slope)
            return;

        Vector3 stairDir = obj.transform.forward.normalized;

        travelDir = SnapDirection(travelDir);

        float dot = Vector3.Dot(stairDir, travelDir);
        float y = obj.transform.position.y;

        bool ok;
        const float eps = 0.05f;

        bool Approx(float a, float b) => Mathf.Abs(a - b) < eps;

        if (dot < 0f)
        {
            ok = Approx(tempOriginPos.y, y - 0.5f) ||
                 Approx(tempOriginPos.y, y - 1.0f) ||
                 Approx(tempOriginPos.y, y - 1.5f);
        }
        else
        {
            ok = Approx(tempOriginPos.y, y + 0.5f) ||
                 Approx(tempOriginPos.y, y + 1.0f) ||
                 Approx(tempOriginPos.y, y + 1.5f);
        }

        if (!ok)
        {
            Debug.LogWarning($"REJECT live stair {obj.name}: originY={tempOriginPos.y} stairY={y} dot={dot}");
            obj = null;
        }
    }

    Vector3 GetOutgoingRootDirection(GameObject block, Vector3 incomingDirection)
    {
        Vector3 direction = SnapDirection(incomingDirection);

        if (block == null)
            return direction;

        BlockInfo info = block.GetComponent<BlockInfo>();

        // Only slopes redirect the root path.
        if (info != null && info.blockType == BlockType.Slope)
        {
            return SnapDirection(block.transform.forward);
        }

        return direction;
    }

    Vector3 GetRootDirectionForSegment(RootBlockLineInfo rootInfo, Vector3 fallbackDirection)
    {
        if (rootInfo != null && rootInfo.facingDir.sqrMagnitude > 0.001f)
        {
            return SnapDirection(rootInfo.facingDir);
        }

        return SnapDirection(fallbackDirection);
    }

    Vector3 GetLastRootTravelDirection(Vector3 fallbackDirection)
    {
        if (RootFreeCostBlockList.Count <= 0)
            return SnapDirection(fallbackDirection);

        RootBlockLineInfo lastRootInfo =
            RootFreeCostBlockList[RootFreeCostBlockList.Count - 1];

        return GetRootDirectionForSegment(lastRootInfo, fallbackDirection);
    }

    bool TryAddRootBlockAndUpdateDirection(
        GameObject block,
        bool stairOrSlope,
        ref Vector3 lookDir_Temp)
    {
        if (block == null)
            return false;

        int addedRootCount =
            SetupEntryInBlockList(block, stairOrSlope, lookDir_Temp);

        if (addedRootCount <= 0)
            return false;

        // If the added block, or a linked teleporter block, is a slope,
        // this becomes the slope's facing direction.
        lookDir_Temp = GetLastRootTravelDirection(lookDir_Temp);

        return true;
    }

    #endregion


    //--------------------


    void ResetRootOnly()
    {
        // This prevents the RootBlock that is currently activating
        // from destroying its own roots / activation state.
        if (ignoreNextSelfReset)
            return;

        DestroyRootFreeCostList();
    }

    void ResetOnRespawn()
    {
        rootActivationQueued = false;
        activatedForCurrentRootStand = false;
        queuedActivationDirection = Vector3.zero;
        playerIsInsideRootTrigger = false;

        DestroyRootFreeCostList();

        StartCoroutine(ActivateIfStandinOnAfterRespawn());
    }

    IEnumerator ActivateIfStandinOnAfterRespawn()
    {
        yield return null;

        if (Movement.Instance == null)
            yield break;

        GameObject rootSourceBlock = GetRootSourceBlock();

        if (rootSourceBlock == null)
            yield break;

        if (Movement.Instance.blockStandingOn == rootSourceBlock)
        {
            ActivateRootOnceForCurrentStand(GetRootDirectionFromCurrentMovement());
        }
    }
}

[Serializable]
public class RootBlockLineInfo
{
    public GameObject block;
    public int originalMovemetCost;

    public BlockType blockType;
    public Vector3 facingDir;

    public bool stopContinuationFromThisBlock;

    public bool triggerTeleportEffectWhenRootActivates;
    public bool teleportEffectHasTriggered;
}