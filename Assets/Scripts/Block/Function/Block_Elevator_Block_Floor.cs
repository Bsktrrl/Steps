using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Elevator_Block_Floor : MonoBehaviour
{
    [SerializeField] GameObject elevatorToActivate;

    public bool isStandingOnBlock;


    //--------------------


    private void OnEnable()
    {
        PlayerStats.Action_RespawnPlayer += ResetButton;
    }
    private void OnDisable()
    {
        PlayerStats.Action_RespawnPlayer -= ResetButton;
    }


    //--------------------


    private void OnTriggerEnter(Collider other)
    {
        print("1. OnTriggerEnter");

        ActivationSetup(other, true);
    }
    private void OnTriggerExit(Collider other)
    {
        print("2. OnTriggerExit");

        ActivationSetup(other, false);
    }


    //--------------------


    void ActivationSetup(Collider other, bool activateState)
    {
        if (other.transform.gameObject.CompareTag("Player"))
        {
            if (activateState)
                StartCoroutine(DelayActivation(0.2f, activateState));
            else
                StartCoroutine(DelayActivation(0, activateState));
        }
    }
    IEnumerator DelayActivation(float waitTime, bool activateState)
    {
        isStandingOnBlock = activateState;

        yield return new WaitForSeconds(waitTime);

        elevatorToActivate.GetComponent<Block_Elevator>().elevatorIsActivated = activateState;
    }


    //--------------------


    void ResetButton()
    {
        StopAllCoroutines();

        isStandingOnBlock = false;
    }
}
