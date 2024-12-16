using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Ladder : MonoBehaviour
{
    [SerializeField] GameObject raycastPoint;

    public GameObject ladder_Over;
    public GameObject ladder_Under;

    RaycastHit hit;


    //--------------------


    private void Start()
    {
        castRay(Vector3.up, ref ladder_Over);
        castRay(Vector3.down, ref ladder_Under);
    }


    //--------------------


    void castRay(Vector3 dir, ref GameObject ladder)
    {
        if (Physics.Raycast(raycastPoint.transform.position, dir, out hit, 1))
        {
            if (hit.transform.gameObject)
            {
                if (hit.transform.gameObject.GetComponent<Block_Ladder>())
                {
                    ladder = hit.transform.gameObject;
                }
            }
        }
    }


    //--------------------


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerManager.Instance.player)
        {
            //print("1. Player has entered Ladder");

            if (ladder_Over)
            {
                ladder_Over.GetComponent<BlockStepCostDisplay>().ShowDisplay();
                ladder_Over.GetComponent<BlockInfo>().DarkenColors();
                Player_Movement.Instance.isOnLadder = true;
            }

            if (ladder_Under)
            {
                ladder_Under.GetComponent<BlockStepCostDisplay>().ShowDisplay();
                ladder_Under.GetComponent<BlockInfo>().DarkenColors();
                Player_Movement.Instance.isOnLadder = true;
            }

            if (!Player_Movement.Instance.ladderMovement_Top && !Player_Movement.Instance.ladderMovement_Down_ToBlock && Player_Movement.Instance.isOnLadder)
            {
                if (!ladder_Over && !Player_Movement.Instance.ladderSteppedOn
                    && !Player_Movement.Instance.ladderMovement_Top_ToBlock && !Player_Movement.Instance.ladderMovement_Up)
                {
                    Player_Movement.Instance.ladderToApproach_Current = gameObject;
                    Player_Movement.Instance.ladderMovement_Down_ToBlock = true;
                }

                Player_Movement.Instance.ladderSteppedOn = gameObject;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerManager.Instance.player)
        {
            //print("2. Player has exited Ladder");

            if (ladder_Over)
            {
                ladder_Over.GetComponent<BlockStepCostDisplay>().HideDisplay();
                ladder_Over.GetComponent<BlockInfo>().ResetColor();

                if (!Player_Movement.Instance.ladderMovement_Up && !Player_Movement.Instance.ladderMovement_Top && !Player_Movement.Instance.ladderMovement_Top_ToBlock
                    && !Player_Movement.Instance.ladderMovement_Down)
                {
                    Player_Movement.Instance.isOnLadder = false;
                    Player_Movement.Instance.ladderSteppedOn = null;
                }
            }
            if (ladder_Under)
            {
                ladder_Under.GetComponent<BlockStepCostDisplay>().HideDisplay();
                ladder_Under.GetComponent<BlockInfo>().ResetColor();

                if (!Player_Movement.Instance.ladderMovement_Up && !Player_Movement.Instance.ladderMovement_Top && !Player_Movement.Instance.ladderMovement_Top_ToBlock
                    && !Player_Movement.Instance.ladderMovement_Down)
                {
                    Player_Movement.Instance.isOnLadder = false;
                    Player_Movement.Instance.ladderSteppedOn = null;
                }
            }
        }
    }
}