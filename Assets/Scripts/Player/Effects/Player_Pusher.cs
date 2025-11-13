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
        Movement.Action_StepTaken_Late += CheckPush;
        Movement.Action_BodyRotated += CheckIfNotPushed;
        Movement.Action_LandedFromFalling += CheckPush;
        Movement.Action_StepTaken_Late += NullifyBlockToPushInto;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken_Early -= CheckPush;
        Movement.Action_StepTaken -= CheckPush;
        Movement.Action_StepTaken_Late -= CheckPush;
        Movement.Action_BodyRotated -= CheckIfNotPushed;
        Movement.Action_LandedFromFalling -= CheckPush;
        Movement.Action_StepTaken_Late -= NullifyBlockToPushInto;
    }


    //--------------------


    public void CheckPush()
    {
        CheckIfPushed();
        CheckIfNotPushed();
    }
    void CheckIfPushed()
    {
        if (Movement.Instance.blockStandingOn)
        {
            if (Movement.Instance.blockStandingOn == BlockToPushInto && playerIsPushed)
            {
                CheckIfNotPushed();
            }
            else if (Movement.Instance.blockStandingOn.GetComponent<Block_Pusher>() || playerIsPushed)
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
        if (Movement.Instance.blockStandingOn && !Movement.Instance.blockStandingOn.GetComponent<Block_Pusher>() && pushDirection_Old != pushDirection_New)
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
    public void RaycastPushDirectionBlock()
    {
        float racastUpOffset = 0.5f;


        if (Physics.Raycast(gameObject.transform.position + (Vector3.up * racastUpOffset), pushDirection_New, out hit, 1 + racastUpOffset))
        {
            Debug.DrawRay(gameObject.transform.position + (Vector3.up * racastUpOffset), pushDirection_New * (1 + racastUpOffset), Color.green, 1f);

            if (hit.transform.gameObject == Movement.Instance.moveToBlock_Forward.targetBlock
                || hit.transform.gameObject == Movement.Instance.moveToBlock_Back.targetBlock
                || hit.transform.gameObject == Movement.Instance.moveToBlock_Left.targetBlock
                || hit.transform.gameObject == Movement.Instance.moveToBlock_Right.targetBlock)
            {
                BlockToPushInto = hit.transform.gameObject;
                return;
            }
        }
        else
        {
            Debug.DrawRay(gameObject.transform.position + (Vector3.up * racastUpOffset), pushDirection_New * (1 + racastUpOffset), Color.red, 1f);
        }

        if (Physics.Raycast(gameObject.transform.position + (Vector3.up * racastUpOffset) + pushDirection_New, Vector3.down, out hit, 1 + racastUpOffset))
        {
            Debug.DrawRay(gameObject.transform.position + (Vector3.up * racastUpOffset) + pushDirection_New, Vector3.down, Color.yellow, 1f);

            if (hit.transform.gameObject == Movement.Instance.moveToBlock_Forward.targetBlock
                || hit.transform.gameObject == Movement.Instance.moveToBlock_Back.targetBlock
                || hit.transform.gameObject == Movement.Instance.moveToBlock_Left.targetBlock
                || hit.transform.gameObject == Movement.Instance.moveToBlock_Right.targetBlock)
            {
                BlockToPushInto = hit.transform.gameObject;
            }
        }
        else
        {
            Debug.DrawRay(gameObject.transform.position + (Vector3.up * racastUpOffset) + pushDirection_New, Vector3.down, Color.magenta, 1f);
        }
    }
    void NullifyBlockToPushInto()
    {
        BlockToPushInto = null;
    }
}
