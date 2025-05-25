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
        Player_BlockDetector.Action_isSwitchingBlocks += CheckIfSteppenOn;
        Player_Movement.Action_StepTaken += DisolveBlock;
        PlayerStats.Action_RespawnPlayerEarly += ResetBlock;
    }

    private void OnDisable()
    {
        Player_BlockDetector.Action_isSwitchingBlocks -= CheckIfSteppenOn;
        Player_Movement.Action_StepTaken -= DisolveBlock;
        PlayerStats.Action_RespawnPlayerEarly -= ResetBlock;
    }


    //--------------------


    void CheckIfSteppenOn()
    {
        if (PlayerManager.Instance.block_StandingOn_Previous == gameObject)
            isSteppedOn = true;
        else
        {
            ResetBlock();
        }
    }

    void DisolveBlock()
    {
        if (!isSteppedOn) { return; }

        StartCoroutine(WaitBeforeDisolveBlock(0.005f));
    }
    IEnumerator WaitBeforeDisolveBlock(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        originalBlock = Instantiate(newBlock, transform.position, Quaternion.identity);
        Player_BurnChanging.Instance.AddMeltedBlockToList(originalBlock);
        gameObject.SetActive(false);

        isSteppedOn = false;
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
