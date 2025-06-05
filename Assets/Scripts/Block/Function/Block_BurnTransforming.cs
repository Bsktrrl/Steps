using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_BurnTransforming : MonoBehaviour
{
    [SerializeField] GameObject meltingBlock;
    [SerializeField] GameObject meltingBlock_InScene;
    public bool isSteppedOn;


    //--------------------


    private void OnEnable()
    {
        Movement.Action_isSwitchingBlocks += CheckIfSteppenOn;
        Movement.Action_StepTaken += MeltBlock;
        PlayerStats.Action_RespawnPlayerEarly += ResetBlock;
    }

    private void OnDisable()
    {
        Movement.Action_isSwitchingBlocks -= CheckIfSteppenOn;
        Movement.Action_StepTaken -= MeltBlock;
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

    void MeltBlock()
    {
        if (!Player_Burning.Instance.isBurning) { return; }

        if (!isSteppedOn) { return; }

        StartCoroutine(WaitBeforeMeltingBlock(0.005f));
    }
    IEnumerator WaitBeforeMeltingBlock(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        meltingBlock_InScene = Instantiate(meltingBlock, transform.position, Quaternion.identity);
        Player_BurnChanging.Instance.AddMeltedBlockToList(meltingBlock_InScene);
        gameObject.SetActive(false);

        isSteppedOn = false;
    }

    public void ResetBlock()
    {
        isSteppedOn = false;
        gameObject.SetActive(true);

        if (meltingBlock_InScene != null)
        {
            Destroy(meltingBlock_InScene);
            meltingBlock_InScene = null;
        }
    }
}
