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
    List<GameObject> LOD_ObjectsList;
    Quaternion objectInitialRotation;

    RaycastHit hit;



    //--------------------


    private void Start()
    {
        GetLODObjects();

        CheckIfCanFall();

        //Get endPos
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, MapManager.Instance.pickup_LayerMask, MapManager.Instance.pickup_LayerMask))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                endPos = hit.transform.position;
            }
            else
            {
                endPos = gameObject.transform.position + (Vector3.down * 15);
            }
        }
        else
        {
            endPos = gameObject.transform.position + (Vector3.down * 15);
        }
    }
    private void Update()
    {
        if (!canFall) { return; }

        if (CheckIfReadyToFall())
        {
            gameObject.GetComponent<BlockInfo>().movementState = MovementStates.Falling;

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
        Player_CeilingGrab.Action_releaseCeiling += CheckIfStandingOn;
        Movement.Action_LandedFromFalling += CheckIfStandingOn;
        Movement.Action_RespawnPlayerEarly += ResetBlock;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken -= CheckIfStandingOn;
        Player_CeilingGrab.Action_releaseCeiling -= CheckIfStandingOn;
        Movement.Action_LandedFromFalling -= CheckIfStandingOn;
        Movement.Action_RespawnPlayerEarly -= ResetBlock;
    }


    //--------------------


    void GetLODObjects()
    {
        LOD_ObjectsList = new List<GameObject>();

        // Find all child GameObjects of the parent that have the LoadLevel script
        foreach (Transform child in gameObject.transform)
        {
            // Check if the child has the LoadLevel script
            if (child.GetComponent<MeshRenderer>() != null)
            {
                // Add the child GameObject to the list
                LOD_ObjectsList.Add(child.gameObject);
            }

            objectInitialRotation = /*child.*/transform.rotation;
        }
    }


    //--------------------


    void CheckIfCanFall()
    {
        //Check if the Block has another block under itself
        if (GetComponent<BlockInfo>().blockType == BlockType.Stair)
        {
            if (Physics.Raycast(gameObject.transform.position + (Vector3.up * 0.5f), Vector3.down, out hit, 1, MapManager.Instance.pickup_LayerMask))
            {
                if (hit.transform.gameObject.GetComponent<BlockInfo>())
                {
                    canFall = false;
                    return;
                }
            }
        }
        else
        {
            if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, 1, MapManager.Instance.pickup_LayerMask))
            {
                if (hit.transform.gameObject.GetComponent<BlockInfo>())
                {
                    if (hit.transform.gameObject.GetComponent<BlockInfo>().blockType == BlockType.Slab)
                    {
                        canFall = true;
                        return;
                    }
                    else
                    {
                        canFall = false;
                        return;
                    }
                }
            }
        }
        
        canFall = true;
    }


    //--------------------


    void CheckIfStandingOn()
    {
        if (!canFall) { return; }

        if (Movement.Instance.blockStandingOn == gameObject && !isSteppedOn && !Player_CeilingGrab.Instance.isCeilingGrabbing && Movement.Instance.GetMovementState() != MovementStates.Falling)
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
            isSteppedOn = false;

        //if (GetComponent<BoxCollider>())
        //    GetComponent<BoxCollider>().enabled = false;
        //else if (GetComponent<MeshCollider>())
        //    GetComponent<MeshCollider>().enabled = false;

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, endPos, PlayerManager.Instance.player.GetComponent<Movement>().fallSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, endPos) <= 0.03f)
        {
            gameObject.GetComponent<BlockInfo>().movementState = MovementStates.Still;

            HideBlock();

            isSteppedOn = false;
            waitCounter = 0;
            transform.position = gameObject.GetComponent<BlockInfo>().startPos;
        }
    }
    void FallingAlertAnimation()
    {
        if (resettingBlock)
            isSteppedOn = false;

        //When falling, straighten up the rotation from the shaking
        if (waitCounter >= waitTime_BeforeFalling)
        {
            for (int i = 0; i < LOD_ObjectsList.Count; i++)
            {
                LOD_ObjectsList[i].transform.SetPositionAndRotation(LOD_ObjectsList[i].transform.position, objectInitialRotation);
            }

            return;
        }

        //Shake the block
        if (LOD_ObjectsList.Count > 0)
        {
            for (int i = 0; i < LOD_ObjectsList.Count; i++)
            {
                float shakeValue = Mathf.Sin(Time.time * shakingSpeed) * shakingIntensity;
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

        gameObject.GetComponent<BlockInfo>().movementState = MovementStates.Still;

        waitCounter = 0;

        transform.position = gameObject.GetComponent<BlockInfo>().startPos;

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
    }
}
