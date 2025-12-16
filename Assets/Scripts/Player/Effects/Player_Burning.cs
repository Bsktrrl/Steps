using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Burning : Singleton<Player_Burning>
{
    public bool isBurning;
    public int flameableStepCounter;

    [SerializeField] GameObject flameEffectObject;

    bool firstTimeCheck;

    RaycastHit hit;


    //--------------------


    private void OnEnable()
    {
        //Movement.Action_isSwitchingBlocks += BecomeFlameable;
        Movement.Action_StepTaken += BecomeFlameable;
        Movement.Action_StepTaken += CheckFlameableCounter;
        Movement.Action_RespawnPlayer += RemoveFlameable;
    }
    private void OnDisable()
    {
        //Movement.Action_isSwitchingBlocks -= BecomeFlameable;
        Movement.Action_StepTaken -= BecomeFlameable;
        Movement.Action_StepTaken -= CheckFlameableCounter;
        Movement.Action_RespawnPlayerEarly -= RemoveFlameable;
    }


    //--------------------


    void BecomeFlameable()
    {
        if (RaycastForLavaBlock(Vector3.forward) || RaycastForLavaBlock(Vector3.back) || RaycastForLavaBlock(Vector3.left) || RaycastForLavaBlock(Vector3.right))
        {
            AddFlameable();
        }
    }
    bool RaycastForLavaBlock(Vector3 dir)
    {
        if (Physics.Raycast(transform.position + dir, Vector3.down, out hit, 1.4f))
        {
            if ((hit.collider.transform.gameObject && hit.collider.transform.gameObject.GetComponent<BlockInfo>() && hit.collider.transform.gameObject.GetComponent<BlockInfo>().blockElement == BlockElement.Lava)
                || (hit.collider.transform.gameObject && hit.collider.transform.parent && hit.collider.transform.parent.gameObject.GetComponent<BlockInfo>() && hit.collider.transform.parent.gameObject.GetComponent<BlockInfo>().blockElement == BlockElement.Lava))
            {
                return true;
            }
        }

        return false;
    }


    void CheckFlameableCounter()
    {
        if (!isBurning) { return; }

        flameableStepCounter += 1;

        //Remove Flameable after 5 steps
        if (flameableStepCounter > 5)
        {
            RemoveFlameable();
        }

        //Remove Flameable in water
        if (Movement.Instance.blockStandingOn)
        {
            if (Movement.Instance.blockStandingOn.GetComponent<Block_Water>())
            {
                RemoveFlameable();
            }
        }
    }
    void AddFlameable()
    {
        StartCoroutine(DelayFlammable());
    }
    IEnumerator DelayFlammable()
    {
        yield return new WaitForEndOfFrame();

        isBurning = true;
        flameableStepCounter = 0;

        flameEffectObject.SetActive(true);
    }
    void RemoveFlameable()
    {
        if (isBurning)
        {
            isBurning = false;
            flameableStepCounter = 0;

            flameEffectObject.SetActive(false);

            firstTimeCheck = false;
        }
    }
}
