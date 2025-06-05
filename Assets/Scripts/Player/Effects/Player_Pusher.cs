using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Pusher : Singleton<Player_Pusher>
{
    public bool playerIsPushed;
    Vector3 pushDirection;


    //--------------------


    private void Update()
    {
        CheckIfNotPushed();
    }
    private void OnEnable()
    {
        Movement.Action_StepTaken += CheckPush;
        Movement.Action_BodyRotated += CheckIfNotPushed;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken -= CheckPush;
        Movement.Action_BodyRotated -= CheckIfNotPushed;
    }


    //--------------------


    void CheckPush()
    {
        CheckIfPushed();
        CheckIfNotPushed();
    }
    void CheckIfPushed()
    {
        if (PlayerManager.Instance.block_StandingOn_Current.block)
        {
            if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<Block_Pusher>())
            {
                playerIsPushed = true;
                pushDirection = PlayerManager.Instance.lookingDirection;
            }
        }
    }

    void CheckIfNotPushed()
    {
        if (PlayerManager.Instance.lookingDirection != pushDirection)
        {
            playerIsPushed = false;
            pushDirection = Vector3.zero;
        }
    }
}
