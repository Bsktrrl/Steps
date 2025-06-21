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
        if (Movement.Instance.blockStandingOn)
        {
            if (Movement.Instance.blockStandingOn.GetComponent<Block_Pusher>())
            {
                playerIsPushed = true;
                pushDirection = Movement.Instance.lookingDirection;
            }
        }
    }

    void CheckIfNotPushed()
    {
        if (Movement.Instance.lookingDirection != pushDirection)
        {
            playerIsPushed = false;
            pushDirection = Vector3.zero;
        }
    }
}
