using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Pusher : Singleton<Player_Pusher>
{
    public bool playerIsPushed;
    [SerializeField] Vector3 pushDirection_Old;
    [SerializeField] Vector3 pushDirection_New;
    [SerializeField] string pushDirectionDescription;

    public GameObject BlockToPushInto;
    RaycastHit hit;


    //--------------------


    private void Update()
    {
        CheckIfNotPushed();
    }
    private void OnEnable()
    {
        Movement.Action_StepTaken_Early += CheckPush;
        Movement.Action_StepTaken += CheckPush;
        Movement.Action_BodyRotated += CheckIfNotPushed;
        Movement.Action_LandedFromFalling += CheckPush;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken_Early -= CheckPush;
        Movement.Action_StepTaken -= CheckPush;
        Movement.Action_BodyRotated -= CheckIfNotPushed;
        Movement.Action_LandedFromFalling -= CheckPush;
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
            if (Movement.Instance.blockStandingOn.GetComponent<Block_Pusher>() || playerIsPushed)
            {
                playerIsPushed = true;

                pushDirection_Old = pushDirection_New;
                pushDirection_New = Movement.Instance.lookingDirection;

                RaycastPushDirectionBlock();
                DisplayPushDirection(pushDirection_New, pushDirectionDescription);
            }
        }
    }

    void CheckIfNotPushed()
    {
        if (!Movement.Instance.blockStandingOn.GetComponent<Block_Pusher>() && pushDirection_Old != pushDirection_New)
        {
            playerIsPushed = false;
            pushDirection_New = Vector3.zero;
            RaycastPushDirectionBlock();
            DisplayPushDirection(pushDirection_New, pushDirectionDescription);
        }
    }

    public void DisplayPushDirection(Vector3 dir, string describer)
    {
        if (dir == Vector3.zero)
            describer = "Zero";
        else if (dir == Vector3.forward)
            describer = "forward";
        else if (dir == Vector3.back)
            describer = "back";
        else if (dir == Vector3.left)
            describer = "left";
        else if (dir == Vector3.right)
            describer = "right";
    }
    void RaycastPushDirectionBlock()
    {
        if (Physics.Raycast(gameObject.transform.position + pushDirection_New, Vector3.down, out hit, 1))
        {
            if (hit.transform.gameObject == Movement.Instance.moveToBlock_Forward.targetBlock
                || hit.transform.gameObject == Movement.Instance.moveToBlock_Back.targetBlock
                || hit.transform.gameObject == Movement.Instance.moveToBlock_Left.targetBlock
                || hit.transform.gameObject == Movement.Instance.moveToBlock_Right.targetBlock)
            {
                BlockToPushInto = hit.transform.gameObject;
            }
        }
    }
}
