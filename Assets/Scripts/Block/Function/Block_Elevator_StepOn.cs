using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Elevator_StepOn : MonoBehaviour
{
    public bool isStandingOnBlock;


    //--------------------


    //private void Update()
    //{
    //    CheckIfPlayerIsOn();
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isStandingOnBlock = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isStandingOnBlock = false;
    }


    //--------------------


    void CheckIfPlayerIsOn()
    {
        StartCoroutine(CheckIfPlayerIsOn_Delay(0.2f));
    }
    IEnumerator CheckIfPlayerIsOn_Delay(float waitTime)
    {
        PlayerManager.Instance.PauseGame();

        yield return new WaitForSeconds(waitTime);

        if (Movement.Instance.blockStandingOn != gameObject && isStandingOnBlock)
        {
            isStandingOnBlock = false;
        }
        else if (Movement.Instance.blockStandingOn == gameObject && !isStandingOnBlock)
        {
            isStandingOnBlock = true;
        }


        PlayerManager.Instance.UnpauseGame();
    }
}