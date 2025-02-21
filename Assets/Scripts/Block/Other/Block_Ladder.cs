using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Block_Ladder : MonoBehaviour
{
    [SerializeField] GameObject raycastPoint;

    public GameObject lastLadderPart_Up;
    public GameObject lastLadderPart_Down;

    public GameObject exitBlock_Up;
    public GameObject exitBlock_Down;

    RaycastHit hit;


    //--------------------


    private void Start()
    {
        FindTopLadderPart();
        FindBottomLadderPart();

        if (transform.rotation.y == 0)
        {
            FindExitBlock_Up(Player_Movement.Instance.DirectionCalculator(Vector3.forward));
            FindExitBlock_Down();
        }
        else if (transform.rotation.y == 180)
        {
            FindExitBlock_Up(Player_Movement.Instance.DirectionCalculator(Vector3.back));
            FindExitBlock_Down();
        }
        else if (transform.rotation.y == -90)
        {
            FindExitBlock_Up(Player_Movement.Instance.DirectionCalculator(Vector3.left));
            FindExitBlock_Down();
        }
        else if (transform.rotation.y == 90)
        {
            FindExitBlock_Up(Player_Movement.Instance.DirectionCalculator(Vector3.right));
            FindExitBlock_Down();
        }

    }


    //--------------------


    void FindTopLadderPart()
    {
        if (!lastLadderPart_Up)
        {
            bool findLastLadder = false;

            lastLadderPart_Up = raycastPoint;
            GameObject tempObject = null;

            //Find the top ladderPart in the ladder
            while (!findLastLadder)
            {
                if (Physics.Raycast(lastLadderPart_Up.transform.position, Vector3.up, out hit, 1))
                {
                    if (hit.transform.gameObject.GetComponent<Block_Ladder>())
                    {
                        lastLadderPart_Up = hit.transform.gameObject.GetComponent<Block_Ladder>().raycastPoint;
                        tempObject = hit.transform.gameObject;
                    }
                    else
                    {
                        if (tempObject)
                            lastLadderPart_Up = tempObject;
                        else
                            lastLadderPart_Up = gameObject;

                        findLastLadder = true;
                    }
                }
                else
                {
                    if (tempObject)
                        lastLadderPart_Up = tempObject;
                    else
                        lastLadderPart_Up = gameObject;

                    findLastLadder = true;
                }
            }
        }
    }
    void FindBottomLadderPart()
    {
        if (!lastLadderPart_Down)
        {
            bool findLastLadder = false;

            lastLadderPart_Down = raycastPoint;
            GameObject tempObject = null;

            //Find the top ladderPart in the ladder
            while (!findLastLadder)
            {
                if (Physics.Raycast(lastLadderPart_Down.transform.position, Vector3.down, out hit, 1))
                {
                    if (hit.transform.gameObject.GetComponent<Block_Ladder>())
                    {
                        lastLadderPart_Down = hit.transform.gameObject.GetComponent<Block_Ladder>().raycastPoint;
                        tempObject = hit.transform.gameObject;
                    }
                    else
                    {
                        if (tempObject)
                            lastLadderPart_Down = tempObject;
                        else
                            lastLadderPart_Down = gameObject;

                        findLastLadder = true;
                    }
                }
                else
                {
                    if (tempObject)
                        lastLadderPart_Down = tempObject;
                    else
                        lastLadderPart_Down = gameObject;

                    findLastLadder = true;
                }
            }
        }
    }

    void FindExitBlock_Up(Vector3 dir)
    {
        //Find the exitBlock to the ladder
        if (Physics.Raycast(lastLadderPart_Up.transform.position + Vector3.up + (dir * 0.5f), Vector3.down, out hit, 1))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                exitBlock_Up = hit.transform.gameObject;
                return;
            }
        }

        exitBlock_Up = null;
    }
    void FindExitBlock_Down()
    {
        //Find the exitBlock to the ladder
        if (Physics.Raycast(lastLadderPart_Down.transform.position, Vector3.down, out hit, 1))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                exitBlock_Down = hit.transform.gameObject;
                return;
            }
        }

        exitBlock_Down = null;
    }


    //--------------------


    public void DarkenExitBlock_Up(Vector3 dir)
    {
        if (!lastLadderPart_Up) { return; }

        //Find the exitBlock to the ladder
        if (Physics.Raycast(lastLadderPart_Up.transform.position + Vector3.up + (dir * 0.5f), Vector3.down, out hit, 1))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                hit.transform.gameObject.GetComponent<BlockInfo>().DarkenColors();
            }
        }
    }
    public void DarkenExitBlock_Down()
    {
        if (!lastLadderPart_Down) { return; }

        //Find the exitBlock to the ladder
        if (Physics.Raycast(lastLadderPart_Down.transform.position, Vector3.down, out hit, 1))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                hit.transform.gameObject.GetComponent<BlockInfo>().DarkenColors();
            }
        }
    }
}
