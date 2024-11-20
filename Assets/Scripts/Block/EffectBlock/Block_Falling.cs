using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Falling : MonoBehaviour
{
    public int distance;
    float waitCounter;
    public float waitTime = 0.5f;

    public bool isSteppedOn;

    public Vector3 endPos;
    public bool isStandingOnBlock;
    public bool isMoving;


    //--------------------


    private void Start()
    {
        CalculateMovementPath();
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
            if (waitCounter < waitTime)
                waitCounter += Time.deltaTime;

            if (waitCounter >= waitTime)
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
