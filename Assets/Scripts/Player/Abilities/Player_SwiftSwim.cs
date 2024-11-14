using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Player_SwiftSwim : MonoBehaviour
{
    public bool canSwiftSwim_Up;
    public bool canSwiftSwim_Down;

    public GameObject swiftSwim_Up_Obj;
    public GameObject swiftSwim_Down_Obj;

    public GameObject swiftSwimBlock_Target;

    bool isSwiftSwimming_Up;
    bool isSwiftSwimming_Down;

    RaycastHit hit;
    Vector3 targetPos;


    //--------------------


    private void Start()
    {
        //Player_Movement.Action_StepTaken += ActivateSwiftSwimRaycast;
    }
    private void Update()
    {
        ActivateSwiftSwimRaycast();

        PerformSwiftSwimMovement();
    }


    //--------------------


    void ActivateSwiftSwimRaycast()
    {
        if (isSwiftSwimming_Up) { return; }

        canSwiftSwim_Up = RaycastSwiftSwim(Vector3.up);
        canSwiftSwim_Down = RaycastSwiftSwim(Vector3.down);
    }
    bool RaycastSwiftSwim(Vector3 dir)
    {
        if (gameObject.GetComponent<Player_Stats>().stats.abilities.SwiftSwim)
        {
            if (Physics.Raycast(gameObject.transform.position + Vector3.down, dir * 2, out hit, 1))
            {
                if (hit.transform.gameObject.GetComponent<Block_Water>())
                {
                    Debug.DrawRay(gameObject.transform.position, dir, Color.yellow);

                    if (dir == Vector3.up)
                        swiftSwim_Up_Obj = hit.transform.gameObject;
                    else if (dir == Vector3.down)
                        swiftSwim_Down_Obj = hit.transform.gameObject;

                    return true;
                }
                else
                {
                    ResetObj(dir);
                    return false;
                }
            }
            else
            {
                ResetObj(dir);
                return false;
            }
        }
        else
        {
            ResetObj(dir);
            return false;
        }
    }
    void ResetObj(Vector3 dir)
    {
        if (dir == Vector3.up)
            swiftSwim_Up_Obj = null;
        else if (dir == Vector3.down)
            swiftSwim_Down_Obj = null;
    }


    //--------------------


    public void SwiftSwim_Up()
    {
        if (gameObject.GetComponent<Player_Stats>().stats.abilities.SwiftSwim)
        {
            MainManager.Instance.pauseGame = true;
            MainManager.Instance.isTeleporting = true;
            isSwiftSwimming_Up = true;
            Player_Movement.Instance.movementStates = MovementStates.Moving;

            swiftSwimBlock_Target = swiftSwim_Up_Obj;
            targetPos = swiftSwimBlock_Target.transform.position + Vector3.up + (Vector3.up * Player_Movement.Instance.heightOverBlock);
        }
    }
    public void SwiftSwim_Down()
    {
        if (gameObject.GetComponent<Player_Stats>().stats.abilities.SwiftSwim)
        {
            MainManager.Instance.pauseGame = true;
            MainManager.Instance.isTeleporting = true;
            isSwiftSwimming_Down = true;
            Player_Movement.Instance.movementStates = MovementStates.Moving;

            swiftSwimBlock_Target = swiftSwim_Down_Obj;
            targetPos = swiftSwimBlock_Target.transform.position + Vector3.down + (Vector3.up * Player_Movement.Instance.heightOverBlock);
        }
    }
    void PerformSwiftSwimMovement()
    {
        if (!isSwiftSwimming_Up && !isSwiftSwimming_Down) { return; }

        //Move towards Target Block
        MainManager.Instance.player.transform.position = Vector3.MoveTowards(MainManager.Instance.player.transform.position, targetPos, swiftSwimBlock_Target.GetComponent<BlockInfo>().movementSpeed * Time.deltaTime);

        //Snap into place when close enough
        if (Vector3.Distance(MainManager.Instance.player.transform.position, targetPos) <= 0.03f)
        {
            MainManager.Instance.player.transform.position = targetPos;

            Player_Movement.Instance.movementStates = MovementStates.Still;
            MainManager.Instance.pauseGame = false;
            MainManager.Instance.isTeleporting = false;
            isSwiftSwimming_Up = false;
            isSwiftSwimming_Down = false;

            Player_BlockDetector.Instance.PerformRaycast_Center_Vertical(Player_BlockDetector.Instance.detectorSpot_Vertical_Center, Vector3.down);

            if (MainManager.Instance.block_StandingOn_Current.block)
            {
                Player_Movement.Instance.currentMovementCost = MainManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementCost;
            }

            swiftSwimBlock_Target = null;

            Player_Movement.Instance.Action_StepTakenInvoke();
            Player_Movement.Instance.Action_ResetBlockColorInvoke();
        }
    }
}
