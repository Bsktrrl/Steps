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
    [SerializeField] float stairDownForwardOffset = -0.20f; // = 0.30 - 0.50
    [SerializeField] float stairUpHeightOffset = 0.20f;
    [SerializeField] float stairSurfaceTilt = -45f;

    [SerializeField] bool isActive;
    [SerializeField] bool onTrigger;

    Coroutine rootAnimCoroutine;
    Coroutine delayedRootExitCheckCoroutine;

    [Header("Live Root Continuation")]
    [SerializeField] bool checkForRootContinuation = true;
    [SerializeField] float continuationCheckInterval = 0.05f;
    [SerializeField] int maxContinuationSegmentsPerCheck = 8;

    float nextContinuationCheckTime;


    //--------------------


    private void Start()
    {
        anim = GetComponentInParent<Animator>();

        // Detect duplicates
        var duplicates = RootObjectList.GroupBy(r => r.GetInstanceID()).Where(g => g.Count() > 1).ToList();
    }

    private void Update()
    {
        if (Movement.Instance.isMovingOnLadder_Down || Movement.Instance.isMovingOnLadder_Up || Movement.Instance.isRespawning) return;

        //CheckWhenToResetRootLine();

        UpdateLiveRootContinuation();
    }

    private void OnEnable()
    {
        // Reset before Movement recalculates visuals on step complete.
        Movement.Action_StepTaken_Early += CheckWhenToResetRootLine;

        Movement.Action_RespawnPlayer += ResetOnRespawn;
        Action_StandingOnRootBlock_Early += ResetRootOnly;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken_Early -= CheckWhenToResetRootLine;

        Movement.Action_RespawnPlayer -= ResetOnRespawn;
        Action_StandingOnRootBlock_Early -= ResetRootOnly;

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
    }


    //--------------------


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.layer != 6) // 6 = PlayerLayer
            return;

        SetupActivateRoot();
    }

    void SetupActivateRoot()
    {
        if (Movement.Instance.isRespawning) return;

        Action_StandingOnRootBlock_Early?.Invoke();

        ActivateRoots();
    }


    //--------------------


    public void ActivateRoots()
    {
        HardResetRootsBeforeRebuild();

        playerLookDir = Movement.Instance.lookingDirection;

        isActive = true;

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

        // Restore previous rooted blocks first.
        SetBlockMovementCostToDefault();

        // Hide pooled root visuals immediately.
        for (int i = 0; i < RootObjectList.Count; i++)
        {
            if (RootObjectList[i] == null) continue;

            RootObjectList[i].SetActive(false);
            RootObjectList[i].transform.position = Vector3.zero;
        }

        RootFreeCostBlockList.Clear();
        finishedCheckingForBlocks = false;

        // Rebuild movement visuals from a fully clean state.
        Movement.Instance.ResetDarkenBlocks_External();
        Movement.Instance.UpdateBlocks();
        Movement.Instance.SetDarkenBlocks();
    }


    //--------------------


    void CheckWhenToResetRootLine()
    {
        if (delayedRootExitCheckCoroutine != null)
        {
            StopCoroutine(delayedRootExitCheckCoroutine);
            delayedRootExitCheckCoroutine = null;
        }

        GameObject blockPlayerStartedOn = Movement.Instance.blockStandingOn;

        delayedRootExitCheckCoroutine = StartCoroutine(CheckWhenToResetRootLine_Delayed(blockPlayerStartedOn));
    }

    IEnumerator CheckWhenToResetRootLine_Delayed(GameObject blockPlayerStartedOn)
    {
        // Wait at least one frame.
        yield return null;

        // Wait until Movement.blockStandingOn has actually updated.
        // This is the important part.
        while (isActive && Movement.Instance.blockStandingOn == blockPlayerStartedOn)
        {
            yield return null;
        }

        delayedRootExitCheckCoroutine = null;

        if (!isActive)
            yield break;

        bool standingOnRootSegment = false;

        for (int i = 0; i < RootFreeCostBlockList.Count; i++)
        {
            if (RootFreeCostBlockList[i].block == null)
                continue;

            if (Movement.Instance.blockStandingOn == RootFreeCostBlockList[i].block)
            {
                standingOnRootSegment = true;
                break;
            }
        }

        // Keep roots alive while standing on any root segment.
        if (standingOnRootSegment)
        {
            yield break;
        }

        // Keep roots alive while standing on the RootBlock itself.
        if (Movement.Instance.blockStandingOn == transform.parent.gameObject)
        {
            yield break;
        }

        // Player has moved from a rooted block/root block onto a non-rooted block.
        DestroyRootFreeCostList();
    }


    //--------------------


    #region BuildRootLine
    void MakeRootFreeCostList()
    {
        Vector3 lookDir_Temp = playerLookDir;

        if (Movement.Instance.isRespawning && transform.parent.gameObject && transform.parent.gameObject.GetComponent<Block_Checkpoint>())
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
        else
        {
            lookDir_Temp = Movement.Instance.lookingDirection;
        }

        tempOriginPos = transform.parent.position;

        //Make list of blocks that must cost 0 and get rootLines on them
        while (!finishedCheckingForBlocks)
        {
            bool blockIsFound = false;

            #region Check in line
            GameObject tempBlock_Adjacent = new GameObject();
            tempBlock_Adjacent = RaycastBlock(tempOriginPos + (Vector3.up * 0.3f), lookDir_Temp, 1f);

            //If there IS a block adjacent
            if (tempBlock_Adjacent)
            {
                //is there a block over adjacent?
                GameObject tempBlock_Over = RaycastBlock(tempBlock_Adjacent.transform.position + (Vector3.up * 0.3f), Vector3.up, 1f);

                //If there is NOT a block over adjacent
                if (!tempBlock_Over || (tempBlock_Over && tempBlock_Over.GetComponent<BlockInfo>() && tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Slab))
                {
                    if (tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                    {
                        StairSlopeCorrection(ref tempBlock_Adjacent);

                        if (tempBlock_Adjacent)
                        {
                            SetupEntryInBlockList(tempBlock_Adjacent, true);
                            blockIsFound = true;
                        }
                        else
                        {
                            blockIsFound = false;
                        }
                    }
                    else
                    {
                        SetupEntryInBlockList(tempBlock_Adjacent, false);
                        blockIsFound = true;
                    }
                }

                //If there IS a block over adjacent
                else
                {
                    if (tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                    {
                        StairSlopeCorrection(ref tempBlock_Over);

                        if (tempBlock_Over)
                        {
                            SetupEntryInBlockList(tempBlock_Over, true);

                            blockIsFound = true;
                        }
                        else
                        {
                            blockIsFound = false;
                        }
                    }
                    else
                    {
                        blockIsFound = false;
                    }
                }
            }

            //If there is NOT a block adjacent?
            else
            {
                GameObject tempBlock_UnderEmpty = RaycastBlock(tempOriginPos + (Vector3.up * 0.3f) + lookDir_Temp, Vector3.down, 1.25f);
                GameObject tempBlock_OverEmpty = RaycastBlock(tempOriginPos + (Vector3.up * 0.3f) + lookDir_Temp, Vector3.up, 1.25f);

                if (tempBlock_UnderEmpty && (tempBlock_UnderEmpty.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_UnderEmpty.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                    && RootFreeCostBlockList.Count > 0 && (RootFreeCostBlockList[RootFreeCostBlockList.Count - 1].blockType == BlockType.Stair || RootFreeCostBlockList[RootFreeCostBlockList.Count - 1].blockType == BlockType.Slope))
                {
                    StairSlopeCorrection(ref tempBlock_UnderEmpty);

                    if (tempBlock_UnderEmpty)
                    {
                        SetupEntryInBlockList(tempBlock_UnderEmpty, true);
                        blockIsFound = true;
                    }
                    else
                    {
                        blockIsFound = false;
                    }
                }
                else if (tempBlock_OverEmpty && (tempBlock_OverEmpty.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_OverEmpty.GetComponent<BlockInfo>().blockType == BlockType.Slope))
                {
                    StairSlopeCorrection(ref tempBlock_OverEmpty);

                    if (tempBlock_OverEmpty)
                    {
                        SetupEntryInBlockList(tempBlock_OverEmpty, true);
                        blockIsFound = true;
                    }
                    else
                    {
                        blockIsFound = false;
                    }
                }
                else
                {
                    blockIsFound = false;
                }
            }
            #endregion

            #region Check diagonal down after stair/slope (step off down-stair)
            if (!blockIsFound && RootFreeCostBlockList.Count > 0)
            {
                var lastType = RootFreeCostBlockList[RootFreeCostBlockList.Count - 1].blockType;

                if (lastType == BlockType.Stair || lastType == BlockType.Slope)
                {
                    Vector3 origin = tempOriginPos + lookDir_Temp + Vector3.up * 2.0f;

                    GameObject diagDown = RaycastBlock(origin, Vector3.down, 5.0f);

                    if (diagDown)
                    {
                        if (diagDown.transform.position.y < tempOriginPos.y - 0.25f)
                        {
                            SetupEntryInBlockList(diagDown, false);
                            blockIsFound = true;
                        }
                    }
                }
            }
            #endregion

            #region Check diagonal up after stair/slope (step off up-stair)
            if (!blockIsFound && RootFreeCostBlockList.Count > 0)
            {
                var lastType = RootFreeCostBlockList[RootFreeCostBlockList.Count - 1].blockType;

                if (lastType == BlockType.Stair || lastType == BlockType.Slope)
                {
                    Vector3 origin = tempOriginPos + lookDir_Temp + Vector3.up * 0.1f;

                    GameObject diagUp = RaycastBlock(origin, Vector3.up, 5.0f);

                    if (diagUp)
                    {
                        if (diagUp.transform.position.y > tempOriginPos.y + 0.25f)
                        {
                            SetupEntryInBlockList(diagUp, false);
                            blockIsFound = true;
                        }
                    }
                }
            }
            #endregion

            #region Check stairs/slopes
            if (!blockIsFound)
            {
                tempBlock_Adjacent = RaycastBlock(tempOriginPos, lookDir_Temp, 1f);
                GameObject tempBlock_Over = RaycastBlock(tempOriginPos + Vector3.up, lookDir_Temp, 1f);

                //Is there a stair/slope adjacent?
                if (tempBlock_Adjacent && (tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Slope))
                {
                    StairSlopeCorrection(ref tempBlock_Adjacent);

                    if (tempBlock_Adjacent)
                    {
                        SetupEntryInBlockList(tempBlock_Adjacent, true);
                        blockIsFound = true;
                    }
                    else
                    {
                        blockIsFound = false;
                    }
                }
                else
                {
                    blockIsFound = false;
                }

                //is there a stair/slope over adjacent?
                if (!blockIsFound && tempBlock_Adjacent && tempBlock_Over && (tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Slope))
                {
                    StairSlopeCorrection(ref tempBlock_Over);

                    if (tempBlock_Over)
                    {
                        SetupEntryInBlockList(tempBlock_Over, true);
                        blockIsFound = true;
                    }
                    else
                    {
                        blockIsFound = false;
                    }
                }
                else
                {
                    blockIsFound = false;
                }
            }
            #endregion

            #region Check Ladder
            if (!blockIsFound)
            {
                GameObject tempBlock_Ladder_Up = new GameObject();
                tempBlock_Ladder_Up = RaycastLadder(tempOriginPos + Vector3.up, lookDir_Temp, 1.5f);

                GameObject tempBlock_Ladder_Down = new GameObject();
                tempBlock_Ladder_Down = RaycastLadder(tempOriginPos, lookDir_Temp, 1f);

                //Ladder Up
                if (tempBlock_Ladder_Up)
                {
                    if (tempBlock_Ladder_Up && tempBlock_Ladder_Up.GetComponent<Block_Ladder>() && tempBlock_Ladder_Up.GetComponent<Block_Ladder>().exitBlock_Up && tempBlock_Ladder_Up.GetComponent<Block_Ladder>().exitBlock_Up.GetComponent<BlockInfo>())
                    {
                        SetupEntryInBlockList(tempBlock_Ladder_Up.GetComponent<Block_Ladder>().exitBlock_Up, false);
                        blockIsFound = true;

                        print("1.2. Ladder IS Detected");
                    }
                    else
                    {
                        print("1.2. Ladder is NOT Detected");
                        blockIsFound = false;
                    }
                }

                //Ladder Down
                else if (tempBlock_Ladder_Down)
                {
                    if (tempBlock_Ladder_Down && tempBlock_Ladder_Down.GetComponent<Block_Ladder>() && tempBlock_Ladder_Down.GetComponent<Block_Ladder>().exitBlock_Down && tempBlock_Ladder_Down.GetComponent<Block_Ladder>().exitBlock_Down.GetComponent<BlockInfo>())
                    {
                        SetupEntryInBlockList(tempBlock_Ladder_Down.GetComponent<Block_Ladder>().exitBlock_Down, false);
                        blockIsFound = true;

                        print("2.1. Ladder IS Detected");
                    }
                    else
                    {
                        print("2.2. Ladder is NOT Detected");
                        blockIsFound = false;
                    }
                }

                else
                {
                    blockIsFound = false;
                }
            }
            #endregion

            #region Check after slope after falling

            if (!blockIsFound)
            {
                GameObject tempBlock_Falling = null;

                if (RootFreeCostBlockList.Count > 0 && RootFreeCostBlockList[RootFreeCostBlockList.Count - 1].blockType == BlockType.Slope)
                {
                    tempBlock_Falling = RaycastBlock(tempOriginPos + lookDir_Temp, Vector3.down, 50f);
                }
                else
                {
                    tempBlock_Falling = null;
                }

                //Is there a block under when falling?
                if (tempBlock_Falling)
                {
                    //Check orientation of stair/slope
                    if (tempBlock_Falling.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_Falling.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                    {
                        if (tempBlock_Falling)
                        {
                            SetupEntryInBlockList(tempBlock_Falling, true);
                            blockIsFound = true;
                        }
                        else
                        {
                            blockIsFound = false;
                        }
                    }
                    else
                    {
                        SetupEntryInBlockList(tempBlock_Falling, false);
                        blockIsFound = true;
                    }
                }
                else
                {
                    blockIsFound = false;
                }
            }
            #endregion


            if (!blockIsFound)
            {
                finishedCheckingForBlocks = true;
            }
        }

        //Change position of each rootLine to their new blocks, and rotate them to correct orientation
        SetRootLineObjectsOrientation();

        for (int i = 0; i < RootFreeCostBlockList.Count; i++)
        {
            var b = RootFreeCostBlockList[i].block;
            var r = RootObjectList[i];
        }

        //Make RootLines visible and add animation to them, not all at once, but in order, with some delay
        MakeRootObjectsVisible();

        //Change the cost of blocks in the list to 0 (save their original costs so they can be reset)
        ChangeBlockMovementCost();
    }

    void SetupEntryInBlockList(GameObject tempBlock, bool stair)
    {
        RootBlockLineInfo rootBlockLineInfo = new RootBlockLineInfo();
        rootBlockLineInfo.block = tempBlock;
        rootBlockLineInfo.originalMovemetCost = tempBlock.GetComponent<BlockInfo>().movementCost_Temp_Base;
        rootBlockLineInfo.blockType = tempBlock.GetComponent<BlockInfo>().blockType;
        rootBlockLineInfo.facingDir = tempBlock.transform.localPosition;

        if (rootBlockLineInfo.block && rootBlockLineInfo.block.GetComponent<Block_Water>())
            rootBlockLineInfo.block.GetComponent<Block_Water>().hasRoots = true;

        RootFreeCostBlockList.Add(rootBlockLineInfo);
        tempOriginPos = RootFreeCostBlockList[RootFreeCostBlockList.Count - 1].block.transform.position;
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
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
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
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    void StairSlopeCorrection(ref GameObject obj)
    {
        var info = obj.GetComponent<BlockInfo>();
        if (info.blockType != BlockType.Stair && info.blockType != BlockType.Slope) return;

        Vector3 stairDir = obj.transform.forward.normalized;
        Vector3 travelDir = Movement.Instance.lookingDirection.normalized;

        //Correction if respawning on a RootBlockCheckpoint
        if (Movement.Instance.isRespawning && transform.parent.gameObject && transform.parent.gameObject.GetComponent<Block_Checkpoint>())
        {
            switch (transform.parent.gameObject.GetComponent<Block_Checkpoint>().spawnDirection)
            {
                case MovementDirection.None:
                    travelDir = -Vector3.forward;
                    break;

                case MovementDirection.Forward:
                    travelDir = -Vector3.forward;
                    break;
                case MovementDirection.Backward:
                    travelDir = -Vector3.back;
                    break;
                case MovementDirection.Left:
                    travelDir = -Vector3.left;
                    break;
                case MovementDirection.Right:
                    travelDir = -Vector3.right;
                    break;

                default:
                    travelDir = -Vector3.forward;
                    break;
            }
        }

        float dot = Vector3.Dot(stairDir, travelDir);

        // Expected origin Y relative to this stair depends on whether we're entering from high side or low side.
        float y = obj.transform.position.y;

        bool ok;
        const float eps = 0.05f;

        bool Approx(float a, float b) => Mathf.Abs(a - b) < eps;

        if (dot < 0f)
        {
            // entering from the "high" side (travel against stairDir)
            ok = Approx(tempOriginPos.y, y - 0.5f) || Approx(tempOriginPos.y, y - 1.0f) || Approx(tempOriginPos.y, y - 1.5f);
        }
        else
        {
            // entering from the "low" side (travel with stairDir)
            ok = Approx(tempOriginPos.y, y + 0.5f) || Approx(tempOriginPos.y, y + 1.0f) || Approx(tempOriginPos.y, y + 1.5f);
        }

        if (!ok)
        {
            Debug.LogWarning($"REJECT stair {obj.name}: originY={tempOriginPos.y} stairY={y} dot={dot}");
            obj = null;
        }
    }

    void SetRootLineObjectsOrientation()
    {
        Vector3 dir = playerLookDir;

        for (int i = 0; i < RootFreeCostBlockList.Count; i++)
        {
            var block = RootFreeCostBlockList[i].block;
            var type = RootFreeCostBlockList[i].blockType;

            if (type == BlockType.Stair || type == BlockType.Slope)
            {
                bool isDownStep = false;
                if (i > 0)
                {
                    float prevY = RootFreeCostBlockList[i - 1].block.transform.position.y;
                    float currY = block.transform.position.y;
                    isDownStep = currY < prevY - 0.01f;
                }

                PlaceAndOrientRootOnStairOrSlope(i, dir, isDownStep);
            }
            else
            {
                RootObjectList[i].transform.SetPositionAndRotation(
                    block.transform.position,
                    Quaternion.LookRotation(dir)
                );
            }
        }
    }

    void PlaceAndOrientRootOnStairOrSlope(int i, Vector3 travelDir, bool isDownStep)
    {
        var block = RootFreeCostBlockList[i].block;
        var root = RootObjectList[i];

        // Ensure travelDir is horizontal + normalized
        travelDir.y = 0f;
        if (travelDir.sqrMagnitude > 0.0001f) travelDir.Normalize();

        // Flip facing for down-steps
        Vector3 rootForward = isDownStep ? -travelDir : travelDir;

        // Use different forward offset for down vs up
        float forwardOffset = isDownStep ? stairDownForwardOffset : stairUpForwardOffset;

        // Position
        Vector3 pos = block.transform.position
                      + Vector3.up * stairUpHeightOffset
                      + travelDir * forwardOffset;

        root.transform.SetPositionAndRotation(pos, Quaternion.LookRotation(rootForward));

        // Tilt onto the surface
        root.transform.Rotate(stairSurfaceTilt, 0f, 0f, Space.Self);
    }

    void MakeRootObjectsVisible()
    {
        if (rootAnimCoroutine != null)
        {
            StopCoroutine(rootAnimCoroutine);
            rootAnimCoroutine = null;
        }

        rootAnimCoroutine = StartCoroutine(AnimationDelay(0.04f));
    }

    IEnumerator AnimationDelay(float waitTime)
    {
        for (int i = 0; i < RootFreeCostBlockList.Count; i++)
        {
            yield return new WaitForSeconds(waitTime);

            if (i < RootObjectList.Count && RootObjectList[i] != null)
            {
                RootObjectList[i].SetActive(true);
                RootObjectList[i].GetComponent<Animator>().SetTrigger("Activate");
            }
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

        if (rootAnimCoroutine != null)
        {
            StopCoroutine(rootAnimCoroutine);
            rootAnimCoroutine = null;
        }

        playerLookDir = Movement.Instance.lookingDirection;

        ResetBlockLineObjects();
        SetBlockMovementCostToDefault();

        RootFreeCostBlockList.Clear();

        finishedCheckingForBlocks = false;

        onTrigger = false;

        Movement.Instance.ResetDarkenBlocks_External();
        Movement.Instance.UpdateBlocks();
        Movement.Instance.SetDarkenBlocks();
    }


    //-----


    void ResetBlockLineObjects()
    {
        for (int i = 0; i < RootObjectList.Count; i++)
        {
            if (RootObjectList[i] == null) continue;

            if (RootObjectList[i].activeInHierarchy)
            {
                RootObjectList[i].GetComponent<Animator>().SetTrigger("Deactivate");
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
        if (RootFreeCostBlockList.Count <= 0) return;

        // Keep existing root visuals attached to their blocks.
        // This is important when rooted blocks move.
        SetRootLineObjectsOrientation();

        if (Time.time < nextContinuationCheckTime) return;
        nextContinuationCheckTime = Time.time + continuationCheckInterval;

        TryContinueRootLineFromOpenSegments();
    }

    void TryContinueRootLineFromOpenSegments()
    {
        if (RootFreeCostBlockList.Count <= 0) return;

        Vector3 lookDir_Temp = GetRootTravelDirection();

        int addedCount = 0;

        // Snapshot count so newly added roots do not all check in the same pass.
        // They can continue expanding on the next interval.
        int rootCountAtStart = RootFreeCostBlockList.Count;

        for (int i = 0; i < rootCountAtStart; i++)
        {
            if (addedCount >= maxContinuationSegmentsPerCheck)
                break;

            if (RootFreeCostBlockList.Count >= RootObjectList.Count)
            {
                Debug.LogWarning($"{nameof(Block_Root)} on {name}: Not enough root objects in RootObjectList to continue root line.");
                break;
            }

            RootBlockLineInfo sourceRootInfo = RootFreeCostBlockList[i];

            if (sourceRootInfo == null || sourceRootInfo.block == null)
                continue;

            // Optimization:
            // Only root segments with no rooted block in front are allowed to search.
            if (HasRootSegmentAdjacentInDirection(sourceRootInfo.block, lookDir_Temp))
                continue;

            GameObject nextBlock = FindNextBlockForLiveContinuationFrom(sourceRootInfo, lookDir_Temp);

            if (!nextBlock)
                continue;

            if (IsBlockAlreadyInRootLine(nextBlock))
                continue;

            BlockInfo nextInfo = nextBlock.GetComponent<BlockInfo>();

            if (!nextInfo)
                continue;

            bool nextIsStairOrSlope = nextInfo.blockType == BlockType.Stair || nextInfo.blockType == BlockType.Slope;

            int newRootIndex = RootFreeCostBlockList.Count;

            SetupEntryInBlockList(nextBlock, nextIsStairOrSlope);

            // Re-place all roots so moving blocks and newly added roots are correct.
            SetRootLineObjectsOrientation();

            // Show only the newly added visual.
            ActivateSingleRootObject(newRootIndex);

            // Apply 0 movement cost to the new block.
            ChangeBlockMovementCost();

            // Refresh movement visuals.
            Movement.Instance.UpdateBlocks();
            Movement.Instance.SetDarkenBlocks();

            addedCount++;
        }
    }

    GameObject FindNextBlockForLiveContinuationFrom(RootBlockLineInfo sourceRootInfo, Vector3 lookDir_Temp)
    {
        if (sourceRootInfo == null || sourceRootInfo.block == null)
            return null;

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
                if (tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Slope)
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
                if (tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Slope)
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
                (tempBlock_UnderEmpty.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_UnderEmpty.GetComponent<BlockInfo>().blockType == BlockType.Slope) &&
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
                     (tempBlock_OverEmpty.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_OverEmpty.GetComponent<BlockInfo>().blockType == BlockType.Slope))
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

                if (diagDown)
                {
                    if (diagDown.transform.position.y < tempOriginPos.y - 0.25f)
                    {
                        foundBlock = diagDown;
                        blockIsFound = true;
                    }
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

                if (diagUp)
                {
                    if (diagUp.transform.position.y > tempOriginPos.y + 0.25f)
                    {
                        foundBlock = diagUp;
                        blockIsFound = true;
                    }
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
                (tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Slope))
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
                (tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Slope))
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
            lookDir_Temp = Movement.Instance.lookingDirection;

        lookDir_Temp.y = 0f;

        if (lookDir_Temp.sqrMagnitude > 0.001f)
            lookDir_Temp.Normalize();

        return lookDir_Temp;
    }

    bool HasRootSegmentAdjacentInDirection(GameObject sourceBlock, Vector3 dir)
    {
        if (!sourceBlock) return false;

        Vector3 sourcePos = sourceBlock.transform.position;

        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            return false;

        dir.Normalize();

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

            // Allows stairs/slopes to count as connected even if Y differs.
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

        RootObjectList[index].SetActive(true);

        Animator rootAnimator = RootObjectList[index].GetComponent<Animator>();

        if (rootAnimator)
        {
            rootAnimator.SetTrigger("Activate");
        }
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

    void StairSlopeCorrectionLive(ref GameObject obj, Vector3 travelDir)
    {
        if (!obj) return;

        var info = obj.GetComponent<BlockInfo>();

        if (!info)
        {
            obj = null;
            return;
        }

        if (info.blockType != BlockType.Stair && info.blockType != BlockType.Slope) return;

        Vector3 stairDir = obj.transform.forward.normalized;

        travelDir.y = 0f;

        if (travelDir.sqrMagnitude > 0.0001f)
            travelDir.Normalize();

        float dot = Vector3.Dot(stairDir, travelDir);

        float y = obj.transform.position.y;

        bool ok;
        const float eps = 0.05f;

        bool Approx(float a, float b) => Mathf.Abs(a - b) < eps;

        if (dot < 0f)
        {
            ok = Approx(tempOriginPos.y, y - 0.5f) || Approx(tempOriginPos.y, y - 1.0f) || Approx(tempOriginPos.y, y - 1.5f);
        }
        else
        {
            ok = Approx(tempOriginPos.y, y + 0.5f) || Approx(tempOriginPos.y, y + 1.0f) || Approx(tempOriginPos.y, y + 1.5f);
        }

        if (!ok)
        {
            Debug.LogWarning($"REJECT live stair {obj.name}: originY={tempOriginPos.y} stairY={y} dot={dot}");
            obj = null;
        }
    }

    #endregion


    //--------------------


    void ResetRootOnly()
    {
        DestroyRootFreeCostList();
    }

    void ResetOnRespawn()
    {
        DestroyRootFreeCostList();

        //Activate if standing on it after respawn
        StartCoroutine(ActivateIfStandinOnAfterRespawn());
    }

    IEnumerator ActivateIfStandinOnAfterRespawn()
    {
        yield return null;

        if (Movement.Instance.blockStandingOn == transform.parent.gameObject)
        {
            ActivateRoots();
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
}