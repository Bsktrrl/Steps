using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Descend : MonoBehaviour
{
    [Header("Descending")]
    public bool playerCanDescend;
    public GameObject descendingBlock_Previous;
    public GameObject descendingBlock_Current;
    public GameObject descendingBlock_Target;
    public float descendingDistance = 4;
    public float descendingSpeed = 20;

    bool isDescending;

    RaycastHit hit;

    bool canRun;


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
        DataManager.datahasLoaded += StartRunningObject;
    }

    private void OnDisable()
    {
        DataManager.datahasLoaded -= StartRunningObject;
    }
    void StartRunningObject()
    {
        canRun = true;
    }


    //--------------------


    bool RaycastForDescending()
    {
        if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot.Descend)
        {
            if (Physics.Raycast(transform.position + Vector3.down, Vector3.down, out hit, descendingDistance))
            {
                Debug.DrawRay(transform.position + Vector3.down, Vector3.down * descendingDistance, Color.yellow);

                if (hit.transform.GetComponent<BlockInfo>())
                {
                    if (hit.transform.GetComponent<BlockInfo>().upper_Center == null && hit.transform.position != gameObject.transform.position + (Vector3.down * gameObject.GetComponent<Player_Movement>().heightOverBlock) + Vector3.down)
                    {
                        //print("HitPos: " + hit.transform.position + " | PlayerPos: " + (gameObject.transform.position + (Vector3.down * gameObject.GetComponent<Player_Movement>().heightOverBlock) + Vector3.down));
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


                        return true;
                    }
                }
            }

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

        return false;
    }


    //--------------------


    public void Descend()
    {
        if (gameObject.GetComponent<PlayerStats>().stats.abilitiesGot.Descend)
        {
            Player_Movement.Instance.movementStates = MovementStates.Moving;
            PlayerManager.Instance.pauseGame = true;
            PlayerManager.Instance.isTeleporting = true;
            isDescending = true;

            descendingBlock_Target = descendingBlock_Current;
        }
    }

    void PerformDescendMovement()
    {
        if (isDescending)
        {
            Vector3 targetPos = descendingBlock_Target.transform.position + (Vector3.up * Player_Movement.Instance.heightOverBlock);

            PlayerManager.Instance.player.transform.position = Vector3.MoveTowards(PlayerManager.Instance.player.transform.position, targetPos, descendingSpeed * Time.deltaTime);

            //Snap into place when close enough
            if (Vector3.Distance(PlayerManager.Instance.player.transform.position, targetPos) <= 0.03f)
            {
                PlayerManager.Instance.player.transform.position = targetPos;

                PlayerManager.Instance.pauseGame = false;
                PlayerManager.Instance.isTeleporting = false;
                isDescending = false;
                Player_Movement.Instance.movementStates = MovementStates.Still;

                Player_BlockDetector.Instance.PerformRaycast_Center_Vertical(Player_BlockDetector.Instance.detectorSpot_Vertical_Center, Vector3.down);

                if (PlayerManager.Instance.block_StandingOn_Current.block)
                {
                    Player_Movement.Instance.currentMovementCost = PlayerManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementCost;
                }

                Player_Movement.Instance.Action_StepTakenInvoke();
                Player_Movement.Instance.Action_ResetBlockColorInvoke();
            }
        }
    }
}
