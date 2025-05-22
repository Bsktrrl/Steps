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

    bool canRun;

    float rayLength = 2f;


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += StartRunningObject;
        DataManager.Action_dataHasLoaded += PrepareForRaycastForAscending;
        Player_Movement.Action_StepTaken += PrepareForRaycastForAscending;
        PlayerStats.Action_RespawnPlayerLate += PrepareForRaycastForAscending;
    }

    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= StartRunningObject;
        DataManager.Action_dataHasLoaded -= PrepareForRaycastForAscending;
        Player_Movement.Action_StepTaken -= PrepareForRaycastForAscending;
        PlayerStats.Action_RespawnPlayerLate -= PrepareForRaycastForAscending;
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
                        Vector3 origin = transform.position + (Vector3.up * 0.1f);

                        // 1. Check for a cube directly above the player
                        if (Physics.Raycast(origin, Vector3.up, out RaycastHit hit1, rayLength, MapManager.Instance.pickup_LayerMask))
                        {
                            float verticalDistance = Mathf.Abs(hit1.point.y) - Mathf.Abs(transform.position.y);

                            print("verticalDistance: " + verticalDistance + " | RayLength: " + rayLength);

                            if (verticalDistance > rayLength)
                            {
                                // Block too high to ascend to
                                AscendingIsNOTAllowed();
                                return false;
                            }

                            if (hit1.collider.GetComponent<BlockInfo>() == null)
                            {
                                print("1. Ascend");

                                AscendingIsNOTAllowed();
                                return false; // Hit something that's not a block
                            }

                            Vector3 secondRayOrigin = hit1.collider.transform.position + (Vector3.up * 0.3f);
                            if (Physics.Raycast(secondRayOrigin, Vector3.up, out RaycastHit hit2, 1, MapManager.Instance.pickup_LayerMask))
                            {
                                BlockInfo secondBlock = hit2.collider.GetComponent<BlockInfo>();
                                if (secondBlock != null)
                                {
                                    // v CASE 1: First block is Water and Player can Swim — allow regardless of second block
                                    if (hit1.collider.GetComponent<BlockInfo>().blockElement == BlockElement.Water && PlayerHasSwimAbility())
                                    {
                                        AscendingIsAllowed(hit1.transform.gameObject);
                                        return true;
                                    }

                                    // v CASE 2: Second block is Water and Player can Swim
                                    if (secondBlock.blockElement == BlockElement.Water && PlayerHasSwimAbility())
                                    {
                                        AscendingIsAllowed(hit1.transform.gameObject);
                                        return true;
                                    }

                                    // v CASE 3: Second block is a slab — allow
                                    if (secondBlock.blockType == BlockType.Slab)
                                    {
                                        AscendingIsAllowed(hit1.transform.gameObject);
                                        return true;
                                    }

                                    // x CASE 4: Solid second block above — block
                                    AscendingIsNOTAllowed();
                                    return false;
                                }
                            }

                            print("4. Ascend");

                            //Check water compatability
                            if (hit1.collider.GetComponent<BlockInfo>().blockElement == BlockElement.Water)
                            {
                                if (PlayerHasSwimAbility())
                                {
                                    AscendingIsAllowed(hit1.transform.gameObject);
                                    return true;
                                }
                                else
                                {
                                    AscendingIsNOTAllowed();
                                    return false;
                                }
                            }
                            
                            // First block exists, second does not — can ascend
                            AscendingIsAllowed(hit1.transform.gameObject);
                            return true;
                        }

                        // No block above the player — cannot ascend
                        AscendingIsNOTAllowed();
                        return false;
                    }
                }
            }
        }

        //Don't Ascend
        AscendingIsNOTAllowed();
        return false;
    }
    bool PlayerHasSwimAbility()
    {
        var stats = PlayerStats.Instance.stats;
        return stats.abilitiesGot_Permanent.SwimSuit ||
               stats.abilitiesGot_Permanent.Flippers ||
               stats.abilitiesGot_Permanent.SwiftSwim ||
               stats.abilitiesGot_Temporary.SwimSuit ||
               stats.abilitiesGot_Temporary.Flippers ||
               stats.abilitiesGot_Temporary.SwiftSwim;
    }

    void AscendingIsAllowed(GameObject hitObject)
    {
        ascendingBlock_Previous = ascendingBlock_Current;
        ascendingBlock_Current = hitObject;

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
