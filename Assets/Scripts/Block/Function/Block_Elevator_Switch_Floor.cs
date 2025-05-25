using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Elevator_Switch_Floor : MonoBehaviour
{
    [SerializeField] GameObject elevatorToActivate;

    public bool elevatorIsActivated;

    private List<SkinnedMeshRenderer> lodRenderers = new List<SkinnedMeshRenderer>();


    //--------------------


    private void Start()
    {
        foreach (var smr in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (smr.name.Contains("LOD"))
            {
                lodRenderers.Add(smr);
            }
        }
    }

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
        {
            ButtonAnimation_Off();
            ActivationSetup(other, false);
        }
        else
        {
            ButtonAnimation_On();
            ActivationSetup(other, true);
        }
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


    void ButtonAnimation_On()
    {
        foreach (var lod in lodRenderers)
        {
            lod.SetBlendShapeWeight(1, 100);
        }
    }
    void ButtonAnimation_Off()
    {
        foreach (var lod in lodRenderers)
        {
            lod.SetBlendShapeWeight(1, 0);
        }
    }


    //--------------------


    void ResetButton()
    {
        StopAllCoroutines();

        elevatorIsActivated = false;
        elevatorToActivate.GetComponent<Block_Elevator>().elevatorIsActivated = false;
    }
}
