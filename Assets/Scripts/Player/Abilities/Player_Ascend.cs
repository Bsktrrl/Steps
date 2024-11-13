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

    bool isAscending;

    RaycastHit hit;


    //--------------------


    private void Update()
    {
        if (RaycastForAscending())
        {
            playerCanAscend = true;
        }
        else
        {
            playerCanAscend = false;
        }

        PerformAscendMovement();
    }


    //--------------------


    bool RaycastForAscending()
    {
        if (gameObject.GetComponent<Player_Stats>().stats.abilities.Ascend)
        {
            if (Physics.Raycast(transform.position, Vector3.up, out hit, ascendingDistance))
            {
                Debug.DrawRay(transform.position, Vector3.up * ascendingDistance, Color.cyan);

                if (hit.transform.GetComponent<BlockInfo>())
                {
                    if (!hit.transform.GetComponent<BlockInfo>().upper_Center)
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

                        ascendingBlock_Current.GetComponent<BlockInfo>().DarkenColors();

                        return true;
                    }
                }
            }

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
        
        return false;
    }


    //--------------------


    void PerformAscendMovement()
    {
        if (isAscending)
        {
            Vector3 targetPos = ascendingBlock_Target.transform.position + (Vector3.up * Player_Movement.Instance.heightOverBlock);

            MainManager.Instance.player.transform.position = Vector3.MoveTowards(MainManager.Instance.player.transform.position, targetPos, ascendingSpeed * Time.deltaTime);

            //Snap into place when close enough
            if (Vector3.Distance(MainManager.Instance.player.transform.position, targetPos) <= 0.03f)
            {
                MainManager.Instance.player.transform.position = targetPos;

                MainManager.Instance.pauseGame = false;
                MainManager.Instance.isTeleporting = false;
                isAscending = false;

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


    public void Ascend()
    {
        if (gameObject.GetComponent<Player_Stats>().stats.abilities.Ascend)
        {
            MainManager.Instance.pauseGame = true;
            MainManager.Instance.isTeleporting = true;
            isAscending = true;

            ascendingBlock_Target = ascendingBlock_Current;
        }
    }
}
