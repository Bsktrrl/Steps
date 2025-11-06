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
        SetupLadder();
    }


    //--------------------


    public void SetupLadder()
    {
        print("SetupLadder");
        FindTopLadderPart();
        FindBottomLadderPart();

        float yRot = Mathf.Round(transform.eulerAngles.y);

        if (yRot == 0 || yRot == 360)
        {
            FindExitBlock_Up(Vector3.forward);
            FindExitBlock_Down();
        }
        else if (yRot == 180)
        {
            FindExitBlock_Up(Vector3.back);
            FindExitBlock_Down();
        }
        else if (yRot == 270)
        {
            FindExitBlock_Up(Vector3.left);
            FindExitBlock_Down();
        }
        else if (yRot == 90)
        {
            FindExitBlock_Up(Vector3.right);
            FindExitBlock_Down();
        }
    }


    //--------------------


    void FindTopLadderPart()
    {
        if (lastLadderPart_Up != null) return;

        GameObject current = raycastPoint;
        GameObject result = current.transform.parent.gameObject; //default if no ladder found above
        GameObject hitObject = null;

        while (true)
        {
            //Try raycasting upward from current
            if (PerformMovementRaycast(current.transform.position, Vector3.up, 1, out hitObject) == RaycastHitObjects.Ladder)
            {
                Block_Ladder ladder = hitObject.GetComponent<Block_Ladder>();
                if (ladder != null)
                {
                    current = ladder.raycastPoint;

                    //Update the current top candidate
                    result = current.transform.parent.gameObject; 
                    continue;
                }
            }

            //No more ladder above
            break;
        }

        lastLadderPart_Up = result;
    }

    void FindBottomLadderPart()
    {
        if (lastLadderPart_Down != null) return;

        GameObject current = raycastPoint;
        GameObject result = current.transform.parent.gameObject; //default if no ladder found above
        GameObject hitObject = null;

        while (true)
        {
            //Try raycasting upward from current
            if (PerformMovementRaycast(current.transform.position, Vector3.down, 1, out hitObject) == RaycastHitObjects.Ladder)
            {
                Block_Ladder ladder = hitObject.GetComponent<Block_Ladder>();
                if (ladder != null)
                {
                    current = ladder.raycastPoint;

                    //Update the current top candidate
                    result = current.transform.parent.gameObject;
                    continue;
                }
            }

            //No more ladder above
            break;
        }

        lastLadderPart_Down = result;
    }

    void FindExitBlock_Up(Vector3 dir)
    {
        GameObject outObject1 = null;

        //Find the exitBlock to the ladder
        if (PerformMovementRaycast(lastLadderPart_Up.transform.position + Vector3.up + (dir * 0.5f), Vector3.down, 1, out outObject1) == RaycastHitObjects.BlockInfo)
        {
            exitBlock_Up = outObject1;
            return;
        }

        exitBlock_Up = null;
    }
    void FindExitBlock_Down()
    {
        GameObject outObject1 = null;

        //Find the exitBlock to the ladder
        if (PerformMovementRaycast(lastLadderPart_Down.transform.position, Vector3.down, 1, out outObject1) == RaycastHitObjects.BlockInfo)
        {
            exitBlock_Down = outObject1;
            return;
        }

        exitBlock_Down = null;
    }


    //--------------------


    public void DarkenExitBlock_Up(Vector3 dir)
    {
        if (!lastLadderPart_Up) { return; }

        if (exitBlock_Up)
        {
            if (exitBlock_Up.GetComponent<BlockInfo>())
            {
                exitBlock_Up.GetComponent<BlockInfo>().SetDarkenColors();
            }
        }
    }
    public void DarkenExitBlock_Down()
    {
        if (!lastLadderPart_Down) { return; }

        if (exitBlock_Down)
        {
            if (exitBlock_Down.GetComponent<BlockInfo>())
            {
                exitBlock_Down.GetComponent<BlockInfo>().SetDarkenColors();
            }
        }
    }


    public RaycastHitObjects PerformMovementRaycast(Vector3 objPos, Vector3 dir, float distance, out GameObject obj)
    {
        int combinedMask = MapManager.Instance.pickup_LayerMask/* | MapManager.Instance.player_LayerMask*/;

        if (Physics.Raycast(objPos, dir, out hit, distance, combinedMask))
        {
            if (hit.transform.GetComponent<BlockInfo>())
            {
                obj = hit.transform.gameObject;

                return RaycastHitObjects.BlockInfo;
            }
            else if (hit.transform.GetComponentInParent<Block_Ladder>())
            {
                obj = hit.transform.parent.gameObject;

                return RaycastHitObjects.Ladder;
            }
            else
            {
                obj = hit.transform.gameObject;

                return RaycastHitObjects.Other;
            }
        }

        obj = null;
        return RaycastHitObjects.None;
    }
}
