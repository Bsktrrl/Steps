using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ascend : Singleton<Player_Ascend>
{
    [Header("Ascending")]
    public bool playerCanAscend;
    public GameObject ascendingBlock_Previous;
    public GameObject ascendingBlock_Current;
    public GameObject ascendingBlock_Target;
    public float ascendingDistance = 3;
    public float ascendingSpeed = 15;

    public bool isAscending;

    RaycastHit hit;

    bool canRun;


    //--------------------


    private void Update()
    {
        if (!canRun) { return; }

        if (RaycastForAscending())
            playerCanAscend = true;
        else
            playerCanAscend = false;

        PerformAscendMovement();
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


    bool RaycastForAscending()
    {
        if (gameObject.GetComponent<PlayerStats>())
        {
            if (gameObject.GetComponent<PlayerStats>().stats != null)
            {
                if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot != null)
                {
                    if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot.Ascend)
                    {
                        if (Physics.Raycast(transform.position, Vector3.up, out hit, ascendingDistance))
                        {
                            Debug.DrawRay(transform.position, Vector3.up * ascendingDistance, Color.cyan);

                            if (hit.transform.GetComponent<BlockInfo>())
                            {
                                //If Ascending block is a WaterBlock
                                if (hit.transform.GetComponent<Block_Water>() && (PlayerStats.Instance.stats.abilitiesGot.SwimSuit || PlayerStats.Instance.stats.abilitiesGot.Flippers || PlayerStats.Instance.stats.abilitiesGot.SwiftSwim))
                                {
                                    //print("1. Ascending - WaterBlock");
                                    AscendingIsAllowed();
                                    return true;
                                }

                                //If Ascending block is a Slab and it's empty over it
                                else if (hit.transform.GetComponent<Block_Slab>() && !hit.transform.GetComponent<BlockInfo>().upper_Center)
                                {
                                    //print("2. Ascending - Slab and noting over");
                                    AscendingIsAllowed();
                                    return true;
                                }

                                //If the space over the blockHit is a Slab
                                else if (hit.transform.GetComponent<BlockInfo>().upper_Center != null)
                                {
                                    if (hit.transform.GetComponent<BlockInfo>().upper_Center.GetComponent<Block_Slab>())
                                    {
                                        //print("3. Ascending - Block over is a Slab");
                                        AscendingIsAllowed();
                                        return true;
                                    }
                                    else
                                    {
                                        //print("4. Ascending - Block over is NOT a Slab");
                                        AscendingIsNOTAllowed();
                                        return false;
                                    }
                                }

                                //If available space over the blockHit is empty
                                else if (!hit.transform.GetComponent<BlockInfo>().upper_Center)
                                {
                                    //If Ascending block is a WaterBlock
                                    if (hit.transform.GetComponent<Block_Water>())
                                    {
                                        // print("5. Ascending - Empty but Water is Hit");
                                        AscendingIsNOTAllowed();
                                        return false;
                                    }
                                    else
                                    {
                                        //print("6. Ascending - Empty");
                                        AscendingIsAllowed();
                                        return true;
                                    }
                                }

                                //If not allowed to Ascend
                                else
                                {
                                    AscendingIsNOTAllowed();
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
        }

        //Don't Ascend
        AscendingIsNOTAllowed();
        return false;
    }
    void AscendingIsAllowed()
    {
        ascendingBlock_Previous = ascendingBlock_Current;
        ascendingBlock_Current = hit.transform.gameObject;

        if (ascendingBlock_Current != ascendingBlock_Previous)
        {
            if (ascendingBlock_Previous)
            {
                if (ascendingBlock_Previous.GetComponent<BlockInfo>())
                {
                    ascendingBlock_Previous.GetComponent<BlockInfo>().ResetColor();
                }
            }
        }

        if (Player_SwiftSwim.Instance.swiftSwim_Up_Obj)
        {
            if (Player_SwiftSwim.Instance.swiftSwim_Up_Obj.GetComponent<Block_Water>())
            {

            }
            else
            {
                ascendingBlock_Current.GetComponent<BlockInfo>().DarkenColors();
            }
        }
        else
        {
            ascendingBlock_Current.GetComponent<BlockInfo>().DarkenColors();
        }
    }
    void AscendingIsNOTAllowed()
    {
        if (ascendingBlock_Current)
        {
            if (ascendingBlock_Current.GetComponent<BlockInfo>())
            {
                ascendingBlock_Current.GetComponent<BlockInfo>().ResetColor();
                ascendingBlock_Current = null;
            }
        }
        if (ascendingBlock_Previous)
        {
            if (ascendingBlock_Previous.GetComponent<BlockInfo>())
            {
                ascendingBlock_Previous.GetComponent<BlockInfo>().ResetColor();
                ascendingBlock_Previous = null;
            }
        }
    }


    //--------------------


    public void Ascend()
    {
        if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot.Ascend)
        {
            if (PlayerStats.Instance.stats.steps_Current <= 0)
            {
                PlayerStats.Instance.RespawnPlayer();
            }
            else
            {
                PlayerManager.Instance.pauseGame = true;
                PlayerManager.Instance.isTeleporting = true;
                isAscending = true;
                Player_Movement.Instance.movementStates = MovementStates.Moving;

                ascendingBlock_Target = ascendingBlock_Current;
            }
        }
    }
    void PerformAscendMovement()
    {
        if (isAscending)
        {
            Vector3 targetPos = ascendingBlock_Target.transform.position + (Vector3.up * Player_Movement.Instance.heightOverBlock);

            PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, targetPos, ascendingSpeed * Time.deltaTime);

            //Snap into place when close enough
            if (Vector3.Distance(PlayerManager.Instance.player.transform.position, targetPos) <= 0.03f)
            {
                PlayerManager.Instance.player.transform.position = targetPos;

                Player_Movement.Instance.movementStates = MovementStates.Still;
                PlayerManager.Instance.pauseGame = false;
                PlayerManager.Instance.isTeleporting = false;

                Player_BlockDetector.Instance.PerformRaycast_Center_Vertical(Player_BlockDetector.Instance.detectorSpot_Vertical_Center, Vector3.down);

                //if (PlayerManager.Instance.block_StandingOn_Current.block)
                //{
                //    Player_Movement.Instance.currentMovementCost = PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementCost;
                //}

                Player_Movement.Instance.Action_StepTakenInvoke();
                Player_Movement.Instance.Action_ResetBlockColorInvoke();
                isAscending = false;
            }
        }
    }
}
