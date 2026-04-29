using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_BurnTransforming : MonoBehaviour
{
    [Header("New Block When Burned")]
    [SerializeField] GameObject burningBlock;

    [Header("Other Parameters")]
    [SerializeField] GameObject burningBlock_InScene;
    public bool isSteppedOn;


    //--------------------


    private void OnEnable()
    {
        Movement.Action_isSwitchingBlocks += CheckIfSteppenOn;
        Movement.Action_StepTaken += BurnBlock;
        Movement.Action_RespawnPlayerEarly += ResetBlock;
    }

    private void OnDisable()
    {
        Movement.Action_isSwitchingBlocks -= CheckIfSteppenOn;
        Movement.Action_StepTaken -= BurnBlock;
        Movement.Action_RespawnPlayerEarly -= ResetBlock;
    }


    //--------------------


    void CheckIfSteppenOn()
    {
        if (Movement.Instance.blockStandingOn == gameObject)
            isSteppedOn = true;
        else
        {
            ResetBlock();
        }
    }

    void BurnBlock()
    {
        if (!Player_Burning.Instance.isBurning || Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (!isSteppedOn) { return; }

        StartCoroutine(WaitBeforeBurningBlock(0.005f));
    }

    IEnumerator WaitBeforeBurningBlock(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        burningBlock_InScene = Instantiate(burningBlock, transform.position, transform.rotation);
        Player_BurnChanging.Instance.AddBurnedBlockToList(burningBlock_InScene);
        gameObject.SetActive(false);

        isSteppedOn = false;
        Movement.Instance.UpdateAvailableMovementBlocks();
    }

    public void ResetBlock()
    {
        isSteppedOn = false;
        gameObject.SetActive(true);

        if (burningBlock_InScene != null)
        {
            Destroy(burningBlock_InScene);
            burningBlock_InScene = null;
        }
    }
}