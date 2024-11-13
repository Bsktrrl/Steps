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
        Player_Movement.Action_StepTaken += StepsOnFallableBlock;

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


    void CalculateMovementPath()
    {
        endPos = gameObject.transform.position + (Vector3.down * distance);
    }


    //--------------------


    void StepsOnFallableBlock()
    {
        if (MainManager.Instance.block_StandingOn_Current.block == gameObject)
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

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, endPos, MainManager.Instance.player.GetComponent<Player_Movement>().fallSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, endPos) <= 0.03f)
        {
            isMoving = false;
            HideBlock();
            MainManager.Instance.player.GetComponent<Player_BlockDetector>().UpdateBlock_StandingOn();
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
}
