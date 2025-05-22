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
            Falling();
        }
    }


    //--------------------


    private void OnEnable()
    {
        Player_Movement.Action_StepTaken += CheckIfStandingOn;
        Player_CeilingGrab.Action_releaseCeiling += CheckIfStandingOn;
        Player_Movement.Action_LandedFromFalling += CheckIfStandingOn;
        PlayerStats.Action_RespawnPlayerEarly += ResetBlock;

        Player_Movement.Action_LandedFromFalling += CheckIfStandingOn;
    }

    private void OnDisable()
    {
        Player_Movement.Action_StepTaken -= CheckIfStandingOn;
        Player_CeilingGrab.Action_releaseCeiling -= CheckIfStandingOn;
        Player_Movement.Action_LandedFromFalling -= CheckIfStandingOn;
        PlayerStats.Action_RespawnPlayerEarly -= ResetBlock;

        Player_Movement.Action_LandedFromFalling -= CheckIfStandingOn;
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

        if (PlayerManager.Instance.block_StandingOn_Current.block == gameObject && !isSteppedOn && !Player_CeilingGrab.Instance.isCeilingGrabbing && Player_Movement.Instance.movementStates != MovementStates.Falling)
        {
            isSteppedOn = true;
        }
    }

    bool CheckIfReadyToFall()
    {
        if (isSteppedOn)
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
        if (GetComponent<BoxCollider>())
            GetComponent<BoxCollider>().enabled = false;
        else if (GetComponent<MeshCollider>())
            GetComponent<MeshCollider>().enabled = false;

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, endPos, PlayerManager.Instance.player.GetComponent<Player_Movement>().fallSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, endPos) <= 0.03f)
        {
            gameObject.SetActive(false);
            isSteppedOn = false;
            waitCounter = 0;
            transform.position = gameObject.GetComponent<BlockInfo>().startPos;
            PlayerManager.Instance.player.GetComponent<Player_BlockDetector>().Update_BlockStandingOn();
        }
    }
    void FallingAlertAnimation()
    {
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


    public void ResetBlock()
    {
        if (GetComponent<BoxCollider>())
            GetComponent<BoxCollider>().enabled = true;
        else if (GetComponent<MeshCollider>())
            GetComponent<MeshCollider>().enabled = true;

        waitCounter = 0;

        isSteppedOn = false;

        waitCounter = 0;

        transform.position = gameObject.GetComponent<BlockInfo>().startPos;

        gameObject.SetActive(true);
    }
}
