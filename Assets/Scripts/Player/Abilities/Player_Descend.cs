using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Descend : Singleton<Player_Descend>
{
    [Header("Descending")]
    public bool playerCanDescend;
    public GameObject descendingBlock_Previous;
    public GameObject descendingBlock_Current;
    public GameObject descendingBlock_Target;
    public float descendingDistance = 4;
    public float descendingSpeed = 20;

    public bool isDescending;

    RaycastHit hit;

    bool canRun;
    bool descendStepCorrection;


    //--------------------


    private void Update()
    {
        if (!canRun) { return; }

        if (RaycastForDescending())
            playerCanDescend = true;
        else
            playerCanDescend = false;

        PerformDescendMovement();
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


    bool RaycastForDescending()
    {
        if (gameObject.GetComponent<PlayerStats>())
        {
            if (gameObject.GetComponent<PlayerStats>().stats != null)
            {
                if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot != null || gameObject.GetComponent<PlayerStats>().stats.abilitiesTempGot != null)
                {
                    if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot.Descend || gameObject.GetComponent<PlayerStats>().stats.abilitiesTempGot.Descend)
                    {
                        if (Physics.Raycast(transform.position + Vector3.down, Vector3.down, out hit, descendingDistance))
                        {
                            Debug.DrawRay(transform.position + Vector3.down, Vector3.down * descendingDistance, Color.yellow);

                            if (hit.transform.GetComponent<BlockInfo>())
                            {
                                //If Descending block is a WaterBlock
                                if (hit.transform.GetComponent<Block_Water>() && (PlayerStats.Instance.stats.abilitiesGot.SwimSuit || PlayerStats.Instance.stats.abilitiesGot.Flippers || PlayerStats.Instance.stats.abilitiesGot.SwiftSwim || PlayerStats.Instance.stats.abilitiesTempGot.SwimSuit || PlayerStats.Instance.stats.abilitiesTempGot.Flippers || PlayerStats.Instance.stats.abilitiesTempGot.SwiftSwim))
                                {
                                    if (hit.transform.GetComponent<BlockInfo>().upper_Center)
                                    {
                                        if (hit.transform.GetComponent<BlockInfo>().upper_Center.GetComponent<Block_Slab>())
                                        {
                                            //print("1. Descending - WaterBlock with Slab over");
                                            DescendingIsAllowed();
                                            return true;
                                        }
                                        else
                                        {
                                            //print("2. Descending - WaterBlock with a Block over");
                                            DescendingIsNOTAllowed();
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        //print("3. Descending - WaterBlock with Empty over");
                                        DescendingIsAllowed();
                                        return true;
                                    }
                                }

                                //If Descending block is a Slab
                                else if (hit.transform.GetComponent<Block_Slab>())
                                {
                                    if (hit.transform.GetComponent<BlockInfo>().upper_Center)
                                    {
                                        if (hit.transform.GetComponent<BlockInfo>().upper_Center.GetComponent<Block_Slab>())
                                        {
                                            //print("4. Descending - Slab with Slab over");
                                            DescendingIsAllowed();
                                            return true;
                                        }
                                        else
                                        {
                                            //print("5. Descending - Slab with a Block over");
                                            DescendingIsNOTAllowed();
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        //print("6. Descending - Slab with Empty over");
                                        DescendingIsAllowed();
                                        return true;
                                    }
                                }

                                //If space over the blockHit is empty 
                                else if (!hit.transform.GetComponent<BlockInfo>().upper_Center)
                                {
                                    //If Descending block is a WaterBlock
                                    if (hit.transform.GetComponent<Block_Water>())
                                    {
                                        //print("7. Descending - Empty over but Water is Hit");
                                        DescendingIsNOTAllowed();
                                        return false;
                                    }
                                    else
                                    {
                                        //print("8. Descending - Block with Empty over");
                                        DescendingIsAllowed();
                                        return true;
                                    }
                                }

                                //If space over the blockHit has a Block 
                                else if (hit.transform.GetComponent<BlockInfo>().upper_Center)
                                {
                                    //If space over the blockHit has a Slab 
                                    if (hit.transform.GetComponent<BlockInfo>().upper_Center.GetComponent<Block_Slab>())
                                    {
                                        //If blockHit is Water
                                        if (hit.transform.GetComponent<Block_Water>())
                                        {
                                            //Can Swim
                                            if (PlayerStats.Instance.stats.abilitiesGot.SwimSuit || PlayerStats.Instance.stats.abilitiesGot.Flippers || PlayerStats.Instance.stats.abilitiesGot.SwiftSwim || PlayerStats.Instance.stats.abilitiesTempGot.SwimSuit || PlayerStats.Instance.stats.abilitiesTempGot.Flippers || PlayerStats.Instance.stats.abilitiesTempGot.SwiftSwim)
                                            {
                                                //print("9. Descending - WaterBlock with Slab over - CanSwim");
                                                DescendingIsAllowed();
                                                return true;
                                            }
                                            else
                                            {
                                                //print("10. Descending - WaterBlock with Slab over - CanNOTSwim");
                                                DescendingIsNOTAllowed();
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            //print("11. Descending - Block with Slab over");
                                            DescendingIsAllowed();
                                            return true;
                                        }
                                    }
                                    else
                                    {
                                        //print("12. Descending - Block with Block over");
                                        DescendingIsNOTAllowed();
                                        return false;
                                    }
                                }

                                //If not allowed to Descend
                                else
                                {
                                    DescendingIsNOTAllowed();
                                    return false;
                                }

                                //if ((hit.transform.GetComponent<BlockInfo>().upper_Center == null && hit.transform.position != gameObject.transform.position + (Vector3.down * gameObject.GetComponent<Player_Movement>().heightOverBlock) + Vector3.down)
                                //    || hit.transform.GetComponent<BlockInfo>().upper_Center.GetComponent<Block_Slab>()
                                //    || (hit.transform.GetComponent<BlockInfo>().upper_Center.GetComponent<Block_Water>() && hit.transform.position != gameObject.transform.position + (Vector3.down * gameObject.GetComponent<Player_Movement>().heightOverBlock) + Vector3.down && PlayerStats.Instance.stats.abilitiesGot.SwimSuit))
                                //{
                                //    DescendingIsAllowed();
                                //    return true;
                                //}
                            }
                        }

                        
                    }
                }
            }
        }

        //Don't Descend
        DescendingIsNOTAllowed();
        return false;
    }
    void DescendingIsAllowed()
    {
        descendingBlock_Previous = descendingBlock_Current;
        descendingBlock_Current = hit.transform.gameObject;

        if (descendingBlock_Current != descendingBlock_Previous)
        {
            if (descendingBlock_Previous)
            {
                if (descendingBlock_Previous.GetComponent<BlockInfo>())
                {
                    descendingBlock_Previous.GetComponent<BlockInfo>().ResetColor();
                }
            }
        }

        if (Player_SwiftSwim.Instance.swiftSwim_Down_Obj)
        {
            if (Player_SwiftSwim.Instance.swiftSwim_Down_Obj.GetComponent<Block_Water>())
            {

            }
            else
            {
                descendingBlock_Current.GetComponent<BlockInfo>().DarkenColors();
            }
        }
        else
        {
            descendingBlock_Current.GetComponent<BlockInfo>().DarkenColors();
        }
    }
    void DescendingIsNOTAllowed()
    {
        if (descendingBlock_Current)
        {
            if (descendingBlock_Current.GetComponent<BlockInfo>())
            {
                descendingBlock_Current.GetComponent<BlockInfo>().ResetColor();
                descendingBlock_Current = null;
            }
        }
        if (descendingBlock_Previous)
        {
            if (descendingBlock_Previous.GetComponent<BlockInfo>())
            {
                descendingBlock_Previous.GetComponent<BlockInfo>().ResetColor();
                descendingBlock_Previous = null;
            }
        }
    }


    //--------------------


    public void Descend()
    {
        if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot.Descend || gameObject.GetComponent<PlayerStats>().stats.abilitiesTempGot.Descend)
        {
            if (PlayerStats.Instance.stats.steps_Current <= 0)
            {
                PlayerStats.Instance.RespawnPlayer();
            }
            else
            {
                Player_Movement.Instance.movementStates = MovementStates.Moving;
                PlayerManager.Instance.pauseGame = true;
                PlayerManager.Instance.isTeleporting = true;
                isDescending = true;

                descendingBlock_Target = descendingBlock_Current;
            }
        }
    }

    void PerformDescendMovement()
    {
        if (isDescending)
        {
            Vector3 targetPos = descendingBlock_Target.transform.position + (Vector3.up * Player_Movement.Instance.heightOverBlock);

            PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, targetPos, descendingSpeed * Time.deltaTime);

            //if (PlayerManager.Instance.block_StandingOn_Current != null && !descendStepCorrection)
            //{
            //    if (PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>())
            //    {
            //        PlayerStats.Instance.stats.steps_Current += PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementCost;
            //        descendStepCorrection = true;
            //    }
            //}

            //Snap into place when close enough
            if (Vector3.Distance(PlayerManager.Instance.player.transform.position, targetPos) <= 0.03f)
            {
                PlayerManager.Instance.player.transform.position = targetPos;

                Player_Movement.Instance.movementStates = MovementStates.Still;
                PlayerManager.Instance.pauseGame = false;
                PlayerManager.Instance.isTeleporting = false;

                Player_BlockDetector.Instance.PerformRaycast_Center_Vertical(Player_BlockDetector.Instance.detectorSpot_Vertical_Center, Vector3.down);

                //if (descendingBlock_Target.GetComponent<BlockInfo>() /*PlayerManager.Instance.block_StandingOn_Current.block*/)
                //{
                //    Player_Movement.Instance.currentMovementCost = descendingBlock_Target.GetComponent<BlockInfo>().movementCost /*PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementCost*/;
                //}

                descendStepCorrection = false;

                Player_Movement.Instance.Action_StepTakenInvoke();
                Player_Movement.Instance.Action_ResetBlockColorInvoke();
                isDescending = false;
            }
        }
    }
}
