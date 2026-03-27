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
            

            ResetBlock();
        }
    }

    void DisolveBlock()
    {
        if (!isSteppedOn || Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        StartCoroutine(WaitBeforeDisolveBlock(0.145f));
    }
    IEnumerator WaitBeforeDisolveBlock(float waitTime)
    {
        IceBreakingEffect_Manager.Instance.BreakIce_Start();
        //Action_WalkedOffCrackedIce?.Invoke();

        yield return new WaitForSeconds(waitTime);

        originalBlock = Instantiate(newBlock, transform.position, Quaternion.identity);
        Player_BurnChanging.Instance.AddMeltedBlockToList(originalBlock);
        gameObject.SetActive(false);

        isSteppedOn = false;

        if (!Movement.Instance.isIceGliding)
        {
            Movement.Instance.UpdateAvailableMovementBlocks();
        }
    }

    public void ResetBlock()
    {
        isSteppedOn = false;
        gameObject.SetActive(true);

        if (originalBlock != null)
        {
            Destroy(originalBlock);
            originalBlock = null;
        }
    }
}
