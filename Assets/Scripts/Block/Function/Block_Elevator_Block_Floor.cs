using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Elevator_Block_Floor : MonoBehaviour
{
    [SerializeField] GameObject elevatorToActivate;


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
        ActivationSetup(other, true);
    }
    private void OnTriggerExit(Collider other)
    {
        ActivationSetup(other, false);
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
        yield return new WaitForSeconds(waitTime);

        elevatorToActivate.GetComponent<Block_Elevator>().elevatorIsActivated = activationState;
    }


    //--------------------


    void ResetButton()
    {
        StopAllCoroutines();
    }
}
