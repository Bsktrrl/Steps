using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Flameable : Singleton<Player_Flameable>
{
    public bool isFlameable;
    public int flameableStepCounter;

    Material original_Material;
    [SerializeField] Material flameable_Material;

    bool firstTimeCheck;


    //--------------------


    private void Start()
    {
        original_Material = gameObject.GetComponent<PlayerManager>().playerBody.transform.GetComponentInChildren<MeshRenderer>().material;
    }


    //--------------------


    private void OnEnable()
    {
        Player_BlockDetector.Action_isSwitchingBlocks += BecomeFlameable;
        Player_Movement.Action_StepTaken += BecomeFlameable;
        Player_Movement.Action_StepTaken += CheckFlameableCounter;
        PlayerStats.Action_RespawnPlayer += RemoveFlameable;
        Player_BlockDetector.Action_madeFirstRaycast += BecomeFlameable;
    }
    private void OnDisable()
    {
        Player_BlockDetector.Action_isSwitchingBlocks -= BecomeFlameable;
        Player_Movement.Action_StepTaken -= BecomeFlameable;
        Player_Movement.Action_StepTaken -= CheckFlameableCounter;
        PlayerStats.Action_RespawnPlayerEarly -= RemoveFlameable;
        Player_BlockDetector.Action_madeFirstRaycast -= BecomeFlameable;
    }


    //--------------------


    void BecomeFlameable()
    {
        if (PlayerManager.Instance.block_StandingOn_Current.block && !firstTimeCheck)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_Meltable>())
            {
                PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_Meltable>().isSteppedOn = true;

                firstTimeCheck = true;
            }
        }

        if (PlayerManager.Instance.block_Vertical_InFront.block)
        {
            if (PlayerManager.Instance.block_Vertical_InFront.block.GetComponent<Block_Lava>())
            {
                AddFlameable();
            }
        }
        
        if (PlayerManager.Instance.block_Vertical_InBack.block)
        {
            if (PlayerManager.Instance.block_Vertical_InBack.block.GetComponent<Block_Lava>())
            {
                AddFlameable();
            }
        }
        
        if (PlayerManager.Instance.block_Vertical_ToTheLeft.block)
        {
            if (PlayerManager.Instance.block_Vertical_ToTheLeft.block.GetComponent<Block_Lava>())
            {
                AddFlameable();
            }
        }
        
        if (PlayerManager.Instance.block_Vertical_ToTheRight.block)
        {
            if (PlayerManager.Instance.block_Vertical_ToTheRight.block.GetComponent<Block_Lava>())
            {
                AddFlameable();
            }
        }
    }

    void CheckFlameableCounter()
    {
        if (!isFlameable) { return; }

        flameableStepCounter += 1;

        //Remove Flameable after 5 steps
        if (flameableStepCounter > 5)
        {
            RemoveFlameable();
        }

        //Remove Flameable in water
        if (PlayerManager.Instance.block_StandingOn_Current.block)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_Water>())
            {
                RemoveFlameable();
            }
        }
    }
    void AddFlameable()
    {
        isFlameable = true;
        flameableStepCounter = 0;

        gameObject.GetComponent<PlayerManager>().playerBody.transform.GetComponentInChildren<MeshRenderer>().material = flameable_Material;
    }
    void RemoveFlameable()
    {
        isFlameable = false;
        flameableStepCounter = 0;

        firstTimeCheck = false;

        gameObject.GetComponent<PlayerManager>().playerBody.transform.GetComponentInChildren<MeshRenderer>().material = original_Material;
    }
}
