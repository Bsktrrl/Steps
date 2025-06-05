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

    bool canRun;

    float rayLength = 2;


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += StartRunningObject;
        DataManager.Action_dataHasLoaded += PrepareForRaycastForDecending;
        Movement.Action_StepTaken += PrepareForRaycastForDecending;
        PlayerStats.Action_RespawnPlayerLate += PrepareForRaycastForDecending;
    }

    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= StartRunningObject;
        DataManager.Action_dataHasLoaded -= PrepareForRaycastForDecending;
        Movement.Action_StepTaken -= PrepareForRaycastForDecending;
        PlayerStats.Action_RespawnPlayerLate -= PrepareForRaycastForDecending;
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

        if (Movement.Instance.GetMovementState() == MovementStates.Moving) { return; }
        if (PlayerManager.Instance.pauseGame) { return; }
        //if (PlayerManager.Instance.isTransportingPlayer) { return; }

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
        //Don't Descend
        if (Player_CeilingGrab.Instance.isCeilingGrabbing)
        {
            DescendingIsNOTAllowed();
            return false;
        }

        if (gameObject.GetComponent<PlayerStats>())
        {
            if (gameObject.GetComponent<PlayerStats>().stats != null)
            {
                if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Permanent != null || gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary != null)
                {
                    if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Permanent.Descend || gameObject.GetComponent<PlayerStats>().stats.abilitiesGot_Temporary.Descend)
                    {
                        Vector3 origin = transform.position + (Vector3.down * 1f); // Start from block below player

                        print("1. Descend");

                        // 1. Check for a block directly below the player's current block
                        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit1, 0.5f, MapManager.Instance.pickup_LayerMask))
                        {
                            print("2. Descend");

                            BlockInfo blockBelow = hit1.collider.GetComponent<BlockInfo>();

                            if (blockBelow != null)
                            {
                                // Block directly below — allow only if it's water AND player can swim
                                if (blockBelow.blockElement == BlockElement.Water)
                                {
                                    if (PlayerHasSwimAbility())
                                    {
                                        print("3. Descend");

                                        DescendingIsAllowed(hit1.transform.gameObject);
                                        return true;
                                    }
                                    else
                                    {
                                        print("3.5 Descend");

                                        // Solid or non-swimmable water block — block descend
                                        DescendingIsNOTAllowed();
                                        return false;
                                    }
                                }
                                //Allow if it's a Slab
                                else if (blockBelow.blockType == BlockType.Slab)
                                {
                                    print("3.6 Descend");

                                    DescendingIsAllowed(hit1.transform.gameObject);
                                    return true;
                                }

                                //Alow if standing on a slab
                                else if (PlayerManager.Instance.block_StandingOn_Current.blockType == BlockType.Slab)
                                {
                                    DescendingIsAllowed(hit1.transform.gameObject);
                                    return true;
                                }

                                    print("4. Descend");

                                DescendingIsNOTAllowed();
                                return false;
                            }
                        }

                        // 2. No adjacent block — check further down
                        Vector3 secondRayOrigin = origin + (Vector3.down * 1f); // Start one unit lower (i.e. 2 units below player)

                        print("5. Descend");

                        if (Physics.Raycast(secondRayOrigin, Vector3.down, out RaycastHit hit2, rayLength, MapManager.Instance.pickup_LayerMask))
                        {
                            float verticalDistance = hit2.point.y - transform.position.y;

                            if (verticalDistance > (rayLength + 1))
                            {
                                // Block too high to ascend to
                                DescendingIsNOTAllowed();
                                return false;
                            }

                            print("6. Descend");

                            BlockInfo lowerBlock = hit2.collider.GetComponent<BlockInfo>();

                            if (lowerBlock != null)
                            {
                                if (lowerBlock.blockElement == BlockElement.Water)
                                {
                                    if (PlayerHasSwimAbility())
                                    {
                                        print("7. Descend");

                                        DescendingIsAllowed(hit2.transform.gameObject);
                                        return true;
                                    }
                                    else
                                    {
                                        print("8. Descend");

                                        DescendingIsNOTAllowed();
                                        return false;
                                    }
                                }

                                print("9. Descend");

                                DescendingIsAllowed(hit2.transform.gameObject);
                                return true;
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

    void DescendingIsAllowed(GameObject hitObject)
    {
        descendingBlock_Previous = descendingBlock_Current;
        descendingBlock_Current = hitObject;

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
                descendingBlock_Current.GetComponent<BlockInfo>().SetDarkenColors();
            }
        }
        else
        {
            descendingBlock_Current.GetComponent<BlockInfo>().SetDarkenColors();
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
        //PlayerManager.Instance.isTransportingPlayer = true;
        Movement.Instance.SetMovementState(MovementStates.Moving);

        Vector3 startPosition = PlayerManager.Instance.block_StandingOn_Current.block.transform.position + (Vector3.up * Movement.Instance.heightOverBlock);
        Vector3 endPosition;
        if (descendingBlock_Target)
            endPosition = descendingBlock_Target.transform.position + (Vector3.up * (Movement.Instance.heightOverBlock));
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

        Movement.Instance.SetMovementState(MovementStates.Still);
        PlayerManager.Instance.pauseGame = false;

        Movement.Instance.Action_StepTaken_Invoke();

        isDescending = false;
    }
}
