using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Descend : Singleton<Player_Descend>
{
    [Header("Descending")]
    [SerializeField] Vector3 descendStartPos;

    public bool playerCanDescend;
    [HideInInspector] public GameObject descendingBlock_Previous;
    [HideInInspector] public GameObject descendingBlock_Current;
    [HideInInspector] public GameObject descendingBlock_Target;
    public float descendingDistance = 3;
    public float descendingSpeed = 20;

    public bool isDescending;
    float descendDuration = 0.1f;

    RaycastHit hit;

    bool canRun;


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += StartRunningObject;
        Player_Movement.Action_StepTaken += PrepareForRaycastForDecending;
    }

    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= StartRunningObject;
        Player_Movement.Action_StepTaken -= PrepareForRaycastForDecending;
    }
    void StartRunningObject()
    {
        canRun = true;
    }


    //--------------------


    public void RunDescend()
    {
        if (!canRun) { return; }

        if (!gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Permanent.Descend && !gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Descend) { return; }

        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (Player_Movement.Instance.movementStates == MovementStates.Moving) { return; }
        if (PlayerManager.Instance.pauseGame) { return; }
        if (PlayerManager.Instance.isTransportingPlayer) { return; }

        if (RaycastForDescending())
        {
            if (PlayerStats.Instance.stats.steps_Current <= 0 && descendingBlock_Target.GetComponent<BlockInfo>().movementCost > 0)
            {
                PlayerStats.Instance.RespawnPlayer();
            }
            else
            {
                StartCoroutine(Descend());
            }
        }
    }
    void PrepareForRaycastForDecending()
    {
        RaycastForDescending();
    }
    bool RaycastForDescending()
    {
        if (gameObject.GetComponent<PlayerStats>())
        {
            if (gameObject.GetComponent<PlayerStats>().stats != null)
            {
                if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Permanent != null || gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary != null)
                {
                    if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Permanent.Descend || gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Descend)
                    {
                        if (Physics.Raycast(transform.position + Vector3.down, Vector3.down, out hit, descendingDistance))
                        {
                            Debug.DrawRay(transform.position + Vector3.down, Vector3.down * descendingDistance, Color.yellow);

                            if (hit.transform.GetComponent<BlockInfo>())
                            {
                                //If Descending block is a WaterBlock
                                if (hit.transform.GetComponent<Block_Water>() && (PlayerStats.Instance.stats.abilitiesGot_Permanent.SwimSuit || PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers || PlayerStats.Instance.stats.abilitiesGot_Permanent.SwiftSwim || PlayerStats.Instance.stats.abilitiesGot_Temporary.SwimSuit || PlayerStats.Instance.stats.abilitiesGot_Temporary.Flippers || PlayerStats.Instance.stats.abilitiesGot_Temporary.SwiftSwim))
                                {
                                    if (hit.transform.GetComponent<BlockInfo>().upper_Center)
                                    {
                                        if (hit.transform.GetComponent<BlockInfo>().upper_Center.GetComponent<BlockInfo>().blockType == BlockType.Slab)
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
                                else if (hit.transform.GetComponent<BlockInfo>().blockType == BlockType.Slab)
                                {
                                    if (hit.transform.GetComponent<BlockInfo>().upper_Center)
                                    {
                                        if (hit.transform.GetComponent<BlockInfo>().upper_Center.GetComponent<BlockInfo>().blockType == BlockType.Slab)
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
                                    if (hit.transform.GetComponent<BlockInfo>().upper_Center.GetComponent<BlockInfo>().blockType == BlockType.Slab)
                                    {
                                        //If blockHit is Water
                                        if (hit.transform.GetComponent<Block_Water>())
                                        {
                                            //Can Swim
                                            if (PlayerStats.Instance.stats.abilitiesGot_Permanent.SwimSuit || PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers || PlayerStats.Instance.stats.abilitiesGot_Permanent.SwiftSwim || PlayerStats.Instance.stats.abilitiesGot_Temporary.SwimSuit || PlayerStats.Instance.stats.abilitiesGot_Temporary.Flippers || PlayerStats.Instance.stats.abilitiesGot_Temporary.SwiftSwim)
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
                                //    || hit.transform.GetComponent<BlockInfo>().upper_Center.GetComponent<BlockInfo>().blockType == BlockType.Slab
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
                    descendingBlock_Previous.GetComponent<BlockInfo>().ResetDarkenColor();
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

        descendingBlock_Target = descendingBlock_Current;
    }
    void DescendingIsNOTAllowed()
    {
        if (descendingBlock_Current)
        {
            if (descendingBlock_Current.GetComponent<BlockInfo>())
            {
                descendingBlock_Current.GetComponent<BlockInfo>().ResetDarkenColor();
                descendingBlock_Current = null;
            }
        }
        if (descendingBlock_Previous)
        {
            if (descendingBlock_Previous.GetComponent<BlockInfo>())
            {
                descendingBlock_Previous.GetComponent<BlockInfo>().ResetDarkenColor();
                descendingBlock_Previous = null;
            }
        }
    }


    //--------------------


    IEnumerator Descend()
    {
        isDescending = true;

        descendStartPos = gameObject.transform.position;

        PlayerManager.Instance.pauseGame = true;
        PlayerManager.Instance.isTransportingPlayer = true;
        Player_Movement.Instance.movementStates = MovementStates.Moving;

        Vector3 startPosition = PlayerManager.Instance.block_StandingOn_Current.block.transform.position + (Vector3.up * Player_Movement.Instance.heightOverBlock);
        Vector3 endPosition;
        if (descendingBlock_Target)
            endPosition = descendingBlock_Target.transform.position + (Vector3.up * (Player_Movement.Instance.heightOverBlock));
        else
            endPosition = descendStartPos;

        float elapsedTime = 0f;
        float targetDistance = Vector3.Distance(gameObject.transform.position, endPosition);


        while (elapsedTime < (descendDuration * targetDistance))
        {
            elapsedTime += Time.deltaTime;

            // Calculate the progress (0 to 1) of the jump
            float progress = elapsedTime / (descendDuration * targetDistance);

            // Interpolate the forward position
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, progress);

            // Update the player's position
            transform.position = currentPosition;

            yield return null;
        }

        // Ensure the player lands exactly at the end position
        transform.position = endPosition;

        Player_BlockDetector.Instance.RaycastSetup();

        Player_Movement.Instance.movementStates = MovementStates.Still;
        PlayerManager.Instance.pauseGame = false;
        PlayerManager.Instance.isTransportingPlayer = false;

        Player_BlockDetector.Instance.PerformRaycast_Center_Vertical(Player_BlockDetector.Instance.detectorSpot_Vertical_Center, Vector3.down);

        Player_BlockDetector.Instance.Update_BlockStandingOn();

        Player_Movement.Instance.Action_ResetBlockColorInvoke();
        Player_Movement.Instance.Action_StepTaken_Invoke();

        isDescending = false;
    }
}
