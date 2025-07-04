using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Falling : MonoBehaviour
{
    [Header("Falling Parameters")]
    [SerializeField] int distance;
    [SerializeField] float waitTime_BeforeFalling = 0.75f;
    float waitCounter = 0;

    [Header("Checked If Stepped On")]
    bool isSteppedOn;
    [SerializeField] bool resettingBlock;

    [Header("Runtime Stats")]
    Vector3 endPos;

    [Header("FallingAnimation")]
    float shakingIntensity = 3;
    float shakingSpeed = 50;
    List<GameObject> LOD_ObjectsList;
    Quaternion objectInitialRotation;


    //--------------------


    private void Start()
    {
        CalculateMovementPath();

        GetLODObjects();
    }
    private void Update()
    {
        if (CheckIfReadyToFall() /*&& !Player_CeilingGrab.Instance.isCeilingGrabbing*/)
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
        Movement.Action_RespawnPlayer += ResetBlock;
    }

    private void OnDisable()
    {
        Movement.Action_RespawnPlayer -= ResetBlock;
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

            objectInitialRotation = child.transform.rotation;
        }
    }


    //--------------------


    void CalculateMovementPath()
    {
        endPos = gameObject.transform.position + (Vector3.down * distance);
    }


    //--------------------


    public void StepsOnFallableBlock()
    {
        isSteppedOn = true;
    }


    //--------------------


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

        //gameObject.transform.position = gameObject.transform.position + (Vector3.down * MainManager.Instance.player.GetComponent<Player_Movement>().fallSpeed * Time.deltaTime);

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, endPos, PlayerManager.Instance.player.GetComponent<Movement>().fallSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, endPos) <= 0.03f)
        {
            gameObject.GetComponent<BlockInfo>().movementState = MovementStates.Still;

            HideBlock();
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
                Vector3 currentRotation = transform.localEulerAngles;
                currentRotation.x = objectInitialRotation.x + shakeValue;
                
                LOD_ObjectsList[i].transform.localEulerAngles = currentRotation;
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

        waitCounter = 0;

        gameObject.GetComponent<BlockInfo>().movementState = MovementStates.Still;

        transform.position = gameObject.GetComponent<BlockInfo>().startPos;

        StartCoroutine(ResetBlockWaiting(0.1f));

        gameObject.SetActive(true);
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
