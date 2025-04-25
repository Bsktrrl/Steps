using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ascend : Singleton<Player_Ascend>
{
    [Header("Ascending")]
    [SerializeField] Vector3 ascendStartPos;

    public bool playerCanAscend;
    [HideInInspector] public GameObject ascendingBlock_Previous;
    [HideInInspector] public GameObject ascendingBlock_Current;
    [HideInInspector] public GameObject ascendingBlock_Target;
    public float ascendingDistance = 2;
    public float ascendingSpeed = 15;

    public bool isAscending;
    float ascendDuration = 0.1f;

    RaycastHit hit;

    bool canRun;


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += StartRunningObject;
        Player_Movement.Action_StepTaken += PrepareForRaycastForAscending;
    }

    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= StartRunningObject;
        Player_Movement.Action_StepTaken -= PrepareForRaycastForAscending;
    }
    void StartRunningObject()
    {
        canRun = true;
    }


    //--------------------


    public void RunAscend()
    {
        if (!canRun) { return; }
        if (!gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Permanent.Ascend && !gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Ascend) { return; }
        if (Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }
        if (Player_Movement.Instance.movementStates == MovementStates.Moving) { return; }
        if (PlayerManager.Instance.pauseGame) { return; }
        if (PlayerManager.Instance.isTransportingPlayer) { return; }

        if (RaycastForAscending())
        {
            if (PlayerStats.Instance.stats.steps_Current <= 0 && ascendingBlock_Target.GetComponent<BlockInfo>().movementCost > 0)
            {
                PlayerStats.Instance.RespawnPlayer();
            }
            else
            {
                StartCoroutine(Ascend());
            }
        }
    }

    void PrepareForRaycastForAscending()
    {
        RaycastForAscending();
    }
    bool RaycastForAscending()
    {
        //Don't Ascend
        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            AscendingIsNOTAllowed();
            return false;
        }

        if (gameObject.GetComponent<PlayerStats>())
        {
            if (gameObject.GetComponent<PlayerStats>().stats != null)
            {
                if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Permanent != null || gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary != null)
                {
                    if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Permanent.Ascend || gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Ascend)
                    {
                        if (Physics.Raycast(transform.position, Vector3.up, out hit, ascendingDistance))
                        {
                            Debug.DrawRay(transform.position, Vector3.up * ascendingDistance, Color.cyan);

                            if (hit.transform.GetComponent<BlockInfo>())
                            {
                                //If Ascending block is a WaterBlock
                                if (hit.transform.GetComponent<Block_Water>() && (PlayerStats.Instance.stats.abilitiesGot_Permanent.SwimSuit || PlayerStats.Instance.stats.abilitiesGot_Permanent.Flippers || PlayerStats.Instance.stats.abilitiesGot_Permanent.SwiftSwim || PlayerStats.Instance.stats.abilitiesGot_Temporary.SwimSuit || PlayerStats.Instance.stats.abilitiesGot_Temporary.Flippers || PlayerStats.Instance.stats.abilitiesGot_Temporary.SwiftSwim))
                                {
                                    //print("1. Ascending - WaterBlock");
                                    AscendingIsAllowed();
                                    return true;
                                }

                                //If Ascending block is a Slab and it's empty over it
                                else if (hit.transform.GetComponent<BlockInfo>().blockType == BlockType.Slab && !hit.transform.GetComponent<BlockInfo>().upper_Center)
                                {
                                    //print("2. Ascending - Slab and noting over");
                                    AscendingIsAllowed();
                                    return true;
                                }

                                //If the space over the blockHit is a Slab
                                else if (hit.transform.GetComponent<BlockInfo>().upper_Center != null)
                                {
                                    if (hit.transform.GetComponent<BlockInfo>().upper_Center.GetComponent<BlockInfo>().blockType == BlockType.Slab)
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
                    ascendingBlock_Previous.GetComponent<BlockInfo>().ResetDarkenColor();
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
                ascendingBlock_Current.GetComponent<BlockInfo>().SetDarkenColors();
            }
        }
        else
        {
            ascendingBlock_Current.GetComponent<BlockInfo>().SetDarkenColors();
        }

        ascendingBlock_Target = ascendingBlock_Current;
    }
    void AscendingIsNOTAllowed()
    {
        if (ascendingBlock_Current)
        {
            if (ascendingBlock_Current.GetComponent<BlockInfo>())
            {
                ascendingBlock_Current.GetComponent<BlockInfo>().ResetDarkenColor();
                ascendingBlock_Current = null;
            }
        }
        if (ascendingBlock_Previous)
        {
            if (ascendingBlock_Previous.GetComponent<BlockInfo>())
            {
                ascendingBlock_Previous.GetComponent<BlockInfo>().ResetDarkenColor();
                ascendingBlock_Previous = null;
            }
        }
    }


    //--------------------

    private IEnumerator Ascend()
    {
        isAscending = true;

        ascendStartPos = gameObject.transform.position;

        PlayerManager.Instance.pauseGame = true;
        PlayerManager.Instance.isTransportingPlayer = true;
        Player_Movement.Instance.movementStates = MovementStates.Moving;

        Vector3 startPosition = PlayerManager.Instance.block_StandingOn_Current.block.transform.position + (Vector3.up * Player_Movement.Instance.heightOverBlock);
        Vector3 endPosition;
        if (ascendingBlock_Target)
            endPosition = ascendingBlock_Target.transform.position + (Vector3.up * (Player_Movement.Instance.heightOverBlock));
        else
            endPosition = ascendStartPos;

        float elapsedTime = 0f;
        float targetDistance = Vector3.Distance(gameObject.transform.position, endPosition);

        while (elapsedTime < (ascendDuration * targetDistance))
        {
            elapsedTime += Time.deltaTime;

            // Calculate the progress (0 to 1) of the jump
            float progress = elapsedTime / (ascendDuration * targetDistance);

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

        isAscending = false;
    }
}
