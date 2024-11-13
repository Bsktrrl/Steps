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


    //--------------------


    private void Update()
    {
        if (RaycastForDescending())
        {
            playerCanDescend = true;
        }
        else
        {
            playerCanDescend = false;
        }

        PerformDescendMovement();
    }


    //--------------------


    bool RaycastForDescending()
    {
        if (gameObject.GetComponent<Player_Stats>().stats.abilities.Descend)
        {
            if (Physics.Raycast(transform.position + Vector3.down, Vector3.down, out hit, descendingDistance))
            {
                Debug.DrawRay(transform.position + Vector3.down, Vector3.down * descendingDistance, Color.yellow);

                if (hit.transform.GetComponent<BlockInfo>())
                {
                    if (!hit.transform.GetComponent<BlockInfo>().upper_Center)
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

                        descendingBlock_Current.GetComponent<BlockInfo>().DarkenColors();

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


    void PerformDescendMovement()
    {
        if (isDescending)
        {
            Vector3 targetPos = descendingBlock_Target.transform.position + (Vector3.up * Player_Movement.Instance.heightOverBlock);

            MainManager.Instance.player.transform.position = Vector3.MoveTowards(MainManager.Instance.player.transform.position, targetPos, descendingSpeed * Time.deltaTime);

            //Snap into place when close enough
            if (Vector3.Distance(MainManager.Instance.player.transform.position, targetPos) <= 0.03f)
            {
                MainManager.Instance.player.transform.position = targetPos;

                MainManager.Instance.pauseGame = false;
                gameObject.GetComponent<Player_Teleport>().isTeleporting = false;
                isDescending = false;

                Player_BlockDetector.Instance.PerformRaycast_Center_Vertical(Player_BlockDetector.Instance.detectorSpot_Vertical_Center, Vector3.down);

                if (MainManager.Instance.block_StandingOn_Current.block)
                {
                    Player_Movement.Instance.currentMovementCost = MainManager.Instance.block_StandingOn_Current.block.GetComponent<BlockInfo>().movementCost;
                }

                Player_Movement.Instance.Action_StepTakenInvoke();
                Player_Movement.Instance.Action_ResetBlockColorInvoke();
            }
        }
    }


    //--------------------


    public void Descend()
    {
        if (gameObject.GetComponent<Player_Stats>().stats.abilities.Descend)
        {
            MainManager.Instance.pauseGame = true;
            gameObject.GetComponent<Player_Teleport>().isTeleporting = true;
            isDescending = true;

            descendingBlock_Target = descendingBlock_Current;
        }
    }
}
