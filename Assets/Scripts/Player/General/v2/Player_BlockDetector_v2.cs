using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player_BlockDetector_v2 : Singleton<Player_BlockDetector_v2>
{
    public static event Action Action_BlockRaycast_Start;
    public static event Action Action_BlockRaycast_End;

    [Header("Important Blocks")]
    public GameObject block_StandingOn_Current;

    RaycastHit hit;


    //--------------------


    private void Update()
    {
        block_StandingOn_Current = Raycast_BlockStandingOn();
    }


    //--------------------


    GameObject Raycast_BlockStandingOn()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                return hit.transform.gameObject;
            }
        }

        return null;
    }
    
    public bool Raycast_IfDirectionIsAllowedToMove(Vector3 position, Vector3 direction, float length)
    {
        //Raycast wants to hit nothing in direction
        if (RaycastBlock(position, direction, length))
            return false;

        //Raycast wants to hit a block down in direction
        if (RaycastBlock(transform.position + direction, Vector3.down, 1))
            return true;
        else
            return false;
    }
    bool RaycastBlock(Vector3 position, Vector3 direction, float length)
    {
        if (Physics.Raycast(position, direction, out hit, length))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                return true;
            }
        }

        return false;
    }

    public GameObject Raycast_MoveToBlock(Vector3 position, Vector3 direction, float length)
    {
        if (Physics.Raycast(position, direction, out hit, length))
        {
            if (hit.transform.gameObject.GetComponent<BlockInfo>())
            {
                return hit.transform.gameObject;
            }
        }

        return null;
    }
}
