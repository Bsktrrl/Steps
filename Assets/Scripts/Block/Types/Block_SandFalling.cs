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

        CheckIfCanFall();

        // Get the lowest fallback position if the block never lands.
        endPos = transform.position + (Vector3.down * 15);
    }

    private void Update()
    {
        if (!canFall)
            return;

        if (CheckIfReadyToFall())
        {
            GetComponent<BlockInfo>().movementState = MovementStates.Falling;

            if (gameObject == Movement.Instance.blockStandingOn)
            {
                Movement.Instance.StartFallingWithBlock();
            }

            Falling();
        }
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


    void CheckIfStandingOn()
    {
        if (!canFall)
            return;

        if (Movement.Instance.blockStandingOn == gameObject &&
            !isSteppedOn &&
            !Player_CeilingGrab.Instance.isCeilingGrabbing &&
            Movement.Instance.GetMovementState() != MovementStates.Falling)
        {
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
        if (TryLandOnBlockBelow(fallDistance))
        {
            LandBlock();
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            endPos,
            fallDistance);

        // The block still disappears if it falls out of the playable area
        // without finding another block.
        if (Vector3.Distance(transform.position, endPos) <= 0.03f)
        {
            GetComponent<BlockInfo>().movementState = MovementStates.Still;

            HideBlock();

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

        isSteppedOn = false;
        canFall = false;
        waitCounter = 0;

        SetFallingSandEnabled(false);

        // Remove any remaining rotation from the warning shake.
        for (int i = 0; i < LOD_ObjectsList.Count; i++)
        {
            LOD_ObjectsList[i].transform.SetPositionAndRotation(
                LOD_ObjectsList[i].transform.position,
                objectInitialRotation);
        }
    }

    void FallingAlertAnimation()
    {
        if (resettingBlock)
            isSteppedOn = false;

        // When falling, straighten the rotation after the shaking.
        if (waitCounter >= waitTime_BeforeFalling)
        {
            for (int i = 0; i < LOD_ObjectsList.Count; i++)
            {
                LOD_ObjectsList[i].transform.SetPositionAndRotation(
                    LOD_ObjectsList[i].transform.position,
                    objectInitialRotation);
            }

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

        isSteppedOn = false;
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