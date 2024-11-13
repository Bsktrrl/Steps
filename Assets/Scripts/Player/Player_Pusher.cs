using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Pusher : MonoBehaviour
{
    public bool playerIsPushed;
    [SerializeField] Vector3 pushDirection;


    //--------------------

    private void Start()
    {
        Player_Movement.Action_StepTaken += Push;
    }


    //--------------------


    void Push()
    {
        CheckIfPushed();
        CheckIfNotPushed();
    }
    void CheckIfPushed()
    {
        if (MainManager.Instance.block_StandingOn_Current.block)
        {
            if (MainManager.Instance.block_StandingOn_Current.block.GetComponent<Block_Pusher>())
            {
                playerIsPushed = true;
                pushDirection = MainManager.Instance.lookingDirection;
            }
        }
    }

    void CheckIfNotPushed()
    {
        if (MainManager.Instance.lookingDirection != pushDirection)
        {
            playerIsPushed = false;
            pushDirection = Vector3.zero;
        }
    }
}
