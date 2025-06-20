using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Weak : MonoBehaviour
{
    [SerializeField] GameObject originalBlock;
    [SerializeField] GameObject newBlock;

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
        if (Movement.Instance.blockStandingOn_Previous == gameObject)
            isSteppedOn = true;
        else
        {
            ResetBlock();
        }
    }

    void DisolveBlock()
    {
        if (!isSteppedOn || Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        StartCoroutine(WaitBeforeDisolveBlock(0.005f));
    }
    IEnumerator WaitBeforeDisolveBlock(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        originalBlock = Instantiate(newBlock, transform.position, Quaternion.identity);
        Player_BurnChanging.Instance.AddMeltedBlockToList(originalBlock);
        gameObject.SetActive(false);

        isSteppedOn = false;

        Movement.Instance.UpdateAvailableMovementBlocks();
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
