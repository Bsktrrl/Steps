using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Elevator_Switch_Floor_Multiple : MonoBehaviour
{
    [SerializeField] GameObject elevatorToActivate;

    [SerializeField] List<GameObject> linkedSwitches = new List<GameObject>();

    public bool elevatorIsActivated;

    [HideInInspector] public List<SkinnedMeshRenderer> lodRenderers = new List<SkinnedMeshRenderer>();


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
        if (elevatorIsActivated)
        {
            for (int i = 0; i < linkedSwitches.Count; i++)
            {
                if (linkedSwitches[i].GetComponent<Block_Elevator_Switch_Floor_Multiple>())
                {
                    linkedSwitches[i].GetComponent<Block_Elevator_Switch_Floor_Multiple>().ButtonAnimation_On();
                }
            }

            ActivationSetup(other, false);
        }
        else
        {
            for (int i = 0; i < linkedSwitches.Count; i++)
            {
                if (linkedSwitches[i].GetComponent<Block_Elevator_Switch_Floor_Multiple>())
                {
                    linkedSwitches[i].GetComponent<Block_Elevator_Switch_Floor_Multiple>().ButtonAnimation_Off();
                }
            }

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
        for (int i = 0; i < linkedSwitches.Count; i++)
        {
            if (linkedSwitches[i].GetComponent<Block_Elevator_Switch_Floor_Multiple>())
            {
                linkedSwitches[i].GetComponent<Block_Elevator_Switch_Floor_Multiple>().elevatorIsActivated = activationState;
            }
        }

        elevatorIsActivated = activationState;

        yield return new WaitForSeconds(waitTime);

        elevatorToActivate.GetComponent<Block_Elevator>().elevatorIsActivated = activationState;
    }


    //--------------------


    public void ButtonAnimation_On()
    {
        foreach (var lod in lodRenderers)
        {
            lod.SetBlendShapeWeight(0, 100);
        }
    }
    public void ButtonAnimation_Off()
    {
        foreach (var lod in lodRenderers)
        {
            lod.SetBlendShapeWeight(0, 0);
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
