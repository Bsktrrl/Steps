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

    [Header("Runtime Stats")]
    Vector3 endPos;
    bool isStandingOnBlock;
    bool isMoving;

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
        if (CheckIfReadyToFall())
        {
            Falling();
        }
    }


    //--------------------


    private void OnEnable()
    {
        Player_Movement.Action_StepTaken += StepsOnFallableBlock;
        PlayerStats.Action_RespawnPlayer += ResetBlock;
    }

    private void OnDisable()
    {
        Player_Movement.Action_StepTaken -= StepsOnFallableBlock;
        PlayerStats.Action_RespawnPlayer -= ResetBlock;
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


    void StepsOnFallableBlock()
    {
        if (PlayerManager.Instance.block_StandingOn_Current.block == gameObject)
        {
            isSteppedOn = true;
        }
    }


    //--------------------


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
        //gameObject.transform.position = gameObject.transform.position + (Vector3.down * MainManager.Instance.player.GetComponent<Player_Movement>().fallSpeed * Time.deltaTime);

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, endPos, PlayerManager.Instance.player.GetComponent<Player_Movement>().fallSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, endPos) <= 0.03f)
        {
            isMoving = false;
            HideBlock();
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
        waitCounter = 0;

        isSteppedOn = false;
        isStandingOnBlock = false;
        isMoving = false;

        print("isSteppedOn: " + isSteppedOn + " | isStandingOnBlock: " + isStandingOnBlock + " | isMoving: " + isMoving + " | waitCounter: " + waitCounter);

        waitCounter = 0;

        transform.position = gameObject.GetComponent<BlockInfo>().startPos;

        ShowBlock();
    }
}
