using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Pusher : MonoBehaviour
{
    public bool playerIsPushed;
    [SerializeField] Vector3 pushDirection;


    //--------------------


    private void OnEnable()
    {
        Player_Movement.Action_StepTaken += Push;
        Player_Movement.Action_BodyRotated += CheckIfNotPushed;
    }

    private void OnDisable()
    {
        Player_Movement.Action_StepTaken -= Push;
        Player_Movement.Action_BodyRotated -= CheckIfNotPushed;
    }


    //--------------------


    void Push()
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
