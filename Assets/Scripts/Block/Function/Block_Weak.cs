
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Block_Weak : MonoBehaviour
{
    public static event Action Action_WalkedOnCrackedIce;
    public static event Action Action_WalkedOffCrackedIce;

    [SerializeField] GameObject originalBlock;
    [SerializeField] GameObject newBlock;

    [SerializeField] GameObject IceCracked_Parent;
    [SerializeField] GameObject IceCracked_Parent_InScene;

    public bool isSteppedOn;

    private bool isDissolving;


    //--------------------


    private void OnEnable()
    {
        Movement.Action_isSwitchingBlocks += CheckIfSteppenOn;
        Movement.Action_StepTaken += DisolveBlock;
        Movement.Action_RespawnPlayerEarly += ResetBlock;
    }

    private void OnDisable()
    {
        Movement.Action_isSwitchingBlocks -= CheckIfSteppenOn;
        Movement.Action_StepTaken -= DisolveBlock;
        Movement.Action_RespawnPlayerEarly -= ResetBlock;
    }


    //--------------------


    void CheckIfSteppenOn()
    {
        if (Movement.Instance.blockStandingOn_Previous && Movement.Instance.blockStandingOn_Previous == gameObject)
        {
            isSteppedOn = true;
        }
        else
        {
            if (Movement.Instance.blockStandingOn && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockElement == BlockElement.Ice && Movement.Instance.blockStandingOn.GetComponent<Block_Weak>())
            {
                IceBreakingEffect_Manager.Instance.CrackIce_Start();
            }

            // Do not interrupt ice that is already breaking.
            if (!isDissolving)
            {
                ResetBlock();
            }
        }
    }

    void DisolveBlock()
    {
        if (!isSteppedOn || Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            return;
        }

        StartDissolving();
    }

    // Called when a movable block leaves this cracked ice.
    public void BreakFromMoveableBlock()
    {
        if (!isActiveAndEnabled)
            return;

        StartDissolving();
    }

    private void StartDissolving()
    {
        // Prevent multiple breaking effects and coroutines.
        if (isDissolving)
            return;

        isDissolving = true;

        StartCoroutine(WaitBeforeDisolveBlock(0.145f));
    }

    IEnumerator WaitBeforeDisolveBlock(float waitTime)
    {
        Vector3 breakPosition = transform.position;
        Quaternion breakRotation = transform.rotation;

        IceBreakingEffect_Manager.Instance.BreakIce_Start(breakPosition, breakRotation);

        yield return new WaitForSeconds(waitTime);

        if (originalBlock)
        {
            originalBlock = Instantiate(newBlock, breakPosition, Quaternion.identity);
            Player_BurnChanging.Instance.AddBurnedBlockToList(originalBlock);
        }

        gameObject.SetActive(false);

        isSteppedOn = false;
        isDissolving = false;

        if (!Movement.Instance.isIceGliding)
        {
            Movement.Instance.UpdateAvailableMovementBlocks();
        }
    }

    public void ResetBlock()
    {
        StopAllCoroutines();

        isSteppedOn = false;
        isDissolving = false;

        gameObject.SetActive(true);

        if (originalBlock != null)
        {
            Destroy(originalBlock);
            originalBlock = null;
        }
    }
}
