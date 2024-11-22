using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Player_SwiftSwim : Singleton<Player_SwiftSwim>
{
    public bool canSwiftSwim_Up;
    public bool canSwiftSwim_Down;

    public GameObject swiftSwim_Up_Obj;
    public GameObject swiftSwim_Down_Obj;

    public GameObject swiftSwimBlock_Target;

    public bool isSwiftSwimming_Up;
    public bool isSwiftSwimming_Down;

    RaycastHit hit;
    public Vector3 targetPos;

    bool canRun;


    //--------------------


    private void Update()
    {
        if (!canRun) { return; }

        ActivateSwiftSwimRaycast();

        if (isSwiftSwimming_Up || isSwiftSwimming_Down)
        {
            PerformSwiftSwimMovement();
        }
    }

    private void OnEnable()
    {
        SaveLoad_PlayerStats.playerStats_hasLoaded += StartRunningObject;
    }

    private void OnDisable()
    {
        SaveLoad_PlayerStats.playerStats_hasLoaded -= StartRunningObject;
    }
    void StartRunningObject()
    {
        canRun = true;
    }


    //--------------------


    void ActivateSwiftSwimRaycast()
    {
        if (isSwiftSwimming_Up) { return; }
        if (isSwiftSwimming_Down) { return; }

        canSwiftSwim_Up = RaycastSwiftSwim(Vector3.up);
        canSwiftSwim_Down = RaycastSwiftSwim(Vector3.down);
    }
    bool RaycastSwiftSwim(Vector3 dir)
    {
        if (gameObject.GetComponent<PlayerStats>())
        {
            if (gameObject.GetComponent<PlayerStats>().stats != null)
            {
                if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot != null || gameObject.GetComponent<PlayerStats>().stats.abilitiesTempGot != null)
                {
                    if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot.SwiftSwim || gameObject.GetComponent<PlayerStats>().stats.abilitiesTempGot.SwiftSwim)
                    {
                        if (Physics.Raycast(gameObject.transform.position + Vector3.down, dir, out hit, 1))
                        {
                            if (hit.transform.gameObject.GetComponent<Block_Water>())
                            {
                                //Debug.DrawRay(gameObject.transform.position, dir, Color.yellow, 1);

                                if (dir == Vector3.up)
                                {
                                    swiftSwim_Up_Obj = hit.transform.gameObject;
                                    swiftSwim_Up_Obj.GetComponent<BlockInfo>().DarkenColors();
                                }
                                else if (dir == Vector3.down)
                                {
                                    swiftSwim_Down_Obj = hit.transform.gameObject;
                                    swiftSwim_Down_Obj.GetComponent<BlockInfo>().DarkenColors();
                                }

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
        {
            if (swiftSwim_Up_Obj)
            {
                swiftSwim_Up_Obj.GetComponent<BlockInfo>().ResetColor();
                swiftSwim_Up_Obj = null;
            }
        }
        else if (dir == Vector3.down)
        {
            if (swiftSwim_Down_Obj)
            {
                swiftSwim_Down_Obj.GetComponent<BlockInfo>().ResetColor();
                swiftSwim_Down_Obj = null;
            }
        }
    }


    //--------------------


    public void SwiftSwim_Up()
    {
        if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot.SwiftSwim || gameObject.GetComponent<PlayerStats>().stats.abilitiesTempGot.SwiftSwim)
        {
            PlayerManager.Instance.pauseGame = true;
            PlayerManager.Instance.isTeleporting = true;
            Player_Movement.Instance.movementStates = MovementStates.Moving;
            isSwiftSwimming_Up = true;

            swiftSwimBlock_Target = swiftSwim_Up_Obj;
            targetPos = swiftSwimBlock_Target.transform.position /*+ Vector3.up*/ + (Vector3.up * Player_Movement.Instance.heightOverBlock);
        }
    }
    public void SwiftSwim_Down()
    {
        if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot.SwiftSwim || gameObject.GetComponent<PlayerStats>().stats.abilitiesTempGot.SwiftSwim)
        {
            PlayerManager.Instance.pauseGame = true;
            PlayerManager.Instance.isTeleporting = true;
            Player_Movement.Instance.movementStates = MovementStates.Moving;
            isSwiftSwimming_Down = true;

            swiftSwimBlock_Target = swiftSwim_Down_Obj;
            targetPos = swiftSwimBlock_Target.transform.position /*+ Vector3.down*/ + (Vector3.up * Player_Movement.Instance.heightOverBlock);
        }
    }
    void PerformSwiftSwimMovement()
    {
        //Move towards Target Block
        PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(transform.position, targetPos, 2 * Time.deltaTime);

        //Snap into place when close enough
        if (Vector3.Distance(transform.position, targetPos) <= 0.03f)
        {
            transform.position = targetPos;

            Player_Movement.Instance.movementStates = MovementStates.Still;
            PlayerManager.Instance.pauseGame = false;
            PlayerManager.Instance.isTeleporting = false;

            isSwiftSwimming_Up = false;
            isSwiftSwimming_Down = false;
            targetPos = Vector3.zero;

            Player_BlockDetector.Instance.PerformRaycast_Center_Vertical(Player_BlockDetector.Instance.detectorSpot_Vertical_Center, Vector3.down);

            if (PlayerManager.Instance.block_StandingOn_Current.block)
            {
                Player_Movement.Instance.currentMovementCost = PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementCost;
            }

            swiftSwimBlock_Target = null;
            
            Player_Movement.Instance.Action_StepTakenInvoke();
            Player_Movement.Instance.Action_ResetBlockColorInvoke();
        }
    }
}
