using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Elevator_Switch_Floor : MonoBehaviour
{
    [SerializeField] GameObject elevatorToActivate;

    public bool elevatorIsActivated;


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

        if (elevatorIsActivated)
            ActivationSetup(other, false);
        else
            ActivationSetup(other, true);
    }


    //--------------------


    void ActivationSetup(Collider other, bool activationState)
    {
        if (other.transform.gameObject.CompareTag("Player"))
        {
            if (activationState)
                StartCoroutine(DelayActivation(0.2f, activationState));
            else
                StartCoroutine(DelayActivation(0, activationState));
        }
    }
    IEnumerator DelayActivation(float waitTime, bool activationState)
    {
        elevatorIsActivated = activationState;

        yield return new WaitForSeconds(waitTime);

        elevatorToActivate.GetComponent<Block_Elevator>().elevatorIsActivated = activationState;
    }


    //--------------------


    void ResetButton()
    {
        StopAllCoroutines();

        elevatorIsActivated = false;
        elevatorToActivate.GetComponent<Block_Elevator>().elevatorIsActivated = false;
    }
}
