using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Block_Root : MonoBehaviour
{
    public static event Action Action_StandingOnRootBlock;

    Animator anim;

    [SerializeField] List<RootBlockLineInfo> RootFreeCostBlockList = new List<RootBlockLineInfo>();
    [SerializeField] List<GameObject> RootObjectList = new List<GameObject>();

    RaycastHit hit;
    bool finishedCheckingForBlocks;

    Vector3 tempOriginPos;
    Vector3 playerLookDir;


    //--------------------


    private void Start()
    {
        anim = GetComponentInParent<Animator>();
    }
    private void Update()
    {
        if (Movement.Instance.isMovingOnLadder_Down || Movement.Instance.isMovingOnLadder_Up)
        {
            return;
        }

        CheckWhenToResetRootLine();
    }

    private void OnEnable()
    {
        Movement.Action_RespawnPlayer += ResetOnRespawn;
    }
    private void OnDisable()
    {
        Movement.Action_RespawnPlayer -= ResetOnRespawn;
    }


    //--------------------


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.layer == 6) //6 = PlayerLayer
        {
            DestroyRootFreeCostList();

            anim.SetTrigger("Activate");

            MakeRootFreeCostList();
            Action_StandingOnRootBlock?.Invoke();
        }
    }


    //--------------------


    void CheckWhenToResetRootLine()
    {
        if (playerLookDir != Movement.Instance.lookingDirection && playerLookDir != -Movement.Instance.lookingDirection)
        {
            DestroyRootFreeCostList();
        }

        bool standingOnCheck = false;
        for (int i = 0; i < RootFreeCostBlockList.Count; i++)
        {
            if (Movement.Instance.blockStandingOn == RootFreeCostBlockList[i].block)
            {
                standingOnCheck = true;
                break;
            }
        }

        if (playerLookDir != Movement.Instance.lookingDirection && !standingOnCheck)
        {
            DestroyRootFreeCostList();
        }
    }


    //--------------------


    #region BuildRootLine
    void MakeRootFreeCostList()
    {
        playerLookDir = Movement.Instance.lookingDirection;
        tempOriginPos = gameObject.transform.parent.position;

        //Make list of blocks that must cost 0 and get rootLines on them
        while (!finishedCheckingForBlocks)
        {
            bool blockIsFound = false;

            #region Check in line
            GameObject tempBlock_Adjacent = new GameObject();
            tempBlock_Adjacent = RaycastBlock(tempOriginPos, playerLookDir, 1f);

            //If there IS a block adjacent
            if (tempBlock_Adjacent)
            {
                //is there a block over adjacent?
                GameObject tempBlock_Over = RaycastBlock(tempBlock_Adjacent.transform.position, Vector3.up, 1f);

                //If there is NOT a block over adjacent
                if (!tempBlock_Over) 
                {
                    if (tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                    {
                        StairSlopeCorrection(ref tempBlock_Adjacent);

                        if (tempBlock_Adjacent)
                        {
                            SetupEntryInBlockList(tempBlock_Adjacent, true);
                            blockIsFound = true;
                        }
                        else
                        {
                            blockIsFound = false;
                        }
                    }
                    else
                    {
                        SetupEntryInBlockList(tempBlock_Adjacent, false);
                        blockIsFound = true;
                    }
                }

                //If there IS a block over adjacent
                else
                {
                    if (tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                    {
                        StairSlopeCorrection(ref tempBlock_Over);

                        if (tempBlock_Over)
                        {
                            SetupEntryInBlockList(tempBlock_Over, true);

                            blockIsFound = true;
                        }
                        else
                        {
                            blockIsFound = false;
                        }
                    }
                    else
                    {
                        blockIsFound = false;
                    }
                }
            }

            //If there is NOT a block adjacent?
            else
            {
                GameObject tempBlock_UnderEmpty = RaycastBlock(tempOriginPos + playerLookDir, Vector3.down, 1.25f);
                GameObject tempBlock_OverEmpty = RaycastBlock(tempOriginPos + playerLookDir, Vector3.up, 1.25f);

                if (tempBlock_UnderEmpty && (tempBlock_UnderEmpty.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_UnderEmpty.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                    && RootFreeCostBlockList.Count > 0 && (RootFreeCostBlockList[RootFreeCostBlockList.Count - 1].blockType == BlockType.Stair || RootFreeCostBlockList[RootFreeCostBlockList.Count - 1].blockType == BlockType.Slope))
                {
                    StairSlopeCorrection(ref tempBlock_UnderEmpty);

                    if (tempBlock_UnderEmpty)
                    {
                        SetupEntryInBlockList(tempBlock_UnderEmpty, true);
                        blockIsFound = true;
                    }
                    else
                    {
                        blockIsFound = false;
                    }
                }
                else if (tempBlock_OverEmpty && (tempBlock_OverEmpty.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_OverEmpty.GetComponent<BlockInfo>().blockType == BlockType.Slope))
                {
                    StairSlopeCorrection(ref tempBlock_OverEmpty);

                    if (tempBlock_OverEmpty)
                    {
                        SetupEntryInBlockList(tempBlock_OverEmpty, true);
                        blockIsFound = true;
                    }
                    else
                    {
                        blockIsFound = false;
                    }
                }
                else
                {
                    blockIsFound = false;
                }
            }
            #endregion


            #region Check stairs/slopes
            if (!blockIsFound)
            {
                tempBlock_Adjacent = RaycastBlock(tempOriginPos, playerLookDir, 1f);
                GameObject tempBlock_Over = RaycastBlock(tempOriginPos + Vector3.up, playerLookDir, 1f);

                //Is there a stair/slope adjacent?
                if (tempBlock_Adjacent && (tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Slope))
                {
                    //Check orientation of stair/slope

                    StairSlopeCorrection(ref tempBlock_Adjacent);

                    if (tempBlock_Adjacent)
                    {
                        SetupEntryInBlockList(tempBlock_Adjacent, true);
                        blockIsFound = true;
                    }
                    else
                    {
                        blockIsFound = false;
                    }
                }
                else
                {
                    blockIsFound = false;
                }

                //is there a stair/slope over adjacent?
                if (!blockIsFound && tempBlock_Adjacent && (tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Slope))
                {
                    //Check orientation of stair/slope

                    StairSlopeCorrection(ref tempBlock_Over);

                    if (tempBlock_Over)
                    {
                        SetupEntryInBlockList(tempBlock_Over, true);
                        blockIsFound = true;
                    }
                    else
                    {
                        blockIsFound = false;
                    }
                }
                else
                {
                    blockIsFound = false;
                }
            }
            #endregion


            #region Check Ladder
            if (!blockIsFound)
            {
                //print("1. Ladder: tempOriginPos: " + tempOriginPos);
                GameObject tempBlock_Ladder_Up = new GameObject();
                tempBlock_Ladder_Up = RaycastLadder(tempOriginPos + Vector3.up, playerLookDir, 1.5f);

                GameObject tempBlock_Ladder_Down = new GameObject();
                tempBlock_Ladder_Down = RaycastLadder(tempOriginPos, playerLookDir, 1f);

                //Ladder Up
                if (tempBlock_Ladder_Up)
                {
                    //print("2. Ladder");
                    SetupEntryInBlockList(tempBlock_Ladder_Up.GetComponent<Block_Ladder>().exitBlock_Up, false);

                    blockIsFound = true;
                }

                //Ladder Down
                else if (tempBlock_Ladder_Down)
                {
                    //print("3. Ladder");
                    SetupEntryInBlockList(tempBlock_Ladder_Down.GetComponent<Block_Ladder>().exitBlock_Down, false);

                    blockIsFound = true;
                }

                else
                {
                    blockIsFound = false;
                }
            }
            #endregion


            #region Check after slope after falling

            if (!blockIsFound)
            {
                GameObject tempBlock_Falling = null;

                if (RootFreeCostBlockList.Count > 0 && RootFreeCostBlockList[RootFreeCostBlockList.Count - 1].blockType == BlockType.Slope)
                {
                    tempBlock_Falling = RaycastBlock(tempOriginPos + playerLookDir, Vector3.down, 50f);
                }
                else
                {
                    tempBlock_Falling = null;
                }

                //Is there a block under when falling?
                if (tempBlock_Falling)
                {
                    //Check orientation of stair/slope
                    if (tempBlock_Falling.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_Falling.GetComponent<BlockInfo>().blockType == BlockType.Slope)
                    {
                        //StairSlopeCorrection(ref tempBlock_Falling);

                        if (tempBlock_Falling)
                        {
                            SetupEntryInBlockList(tempBlock_Falling, true);
                            blockIsFound = true;
                        }
                        else
                        {
                            blockIsFound = false;
                        }
                    }
                    else
                    {
                        SetupEntryInBlockList(tempBlock_Falling, false);
                        blockIsFound = true;
                    }
                }
                else
                {
                    blockIsFound = false;
                }
            }
            #endregion


            if (!blockIsFound)
                finishedCheckingForBlocks = true;
        }
        
        //Change position of each rootLine to their new blocks, and rotate them to correct orientation
        SetRootLineObjectsOrientation();

        //Make RootLines visible and add animation to them, not all at once, but in order, with some delay
        MakeRootObjectsVisible();

        //Change the cost of blocks in the list to 0 (save their original costs so they can be reset)
        ChangeBlockMovementCost();
    }
    void SetupEntryInBlockList(GameObject tempBlock, bool stair)
    {
        RootBlockLineInfo rootBlockLineInfo = new RootBlockLineInfo();
        rootBlockLineInfo.block = tempBlock;
        rootBlockLineInfo.originalMovemetCost = tempBlock.GetComponent<BlockInfo>().movementCost;
        rootBlockLineInfo.blockType = tempBlock.GetComponent<BlockInfo>().blockType;
        rootBlockLineInfo.facingDir = tempBlock.transform.localPosition;

        RootFreeCostBlockList.Add(rootBlockLineInfo);
        tempOriginPos = RootFreeCostBlockList[RootFreeCostBlockList.Count - 1].block.transform.position;
    }


    //-----


    GameObject RaycastBlock(Vector3 origin, Vector3 dir, float distance)
    {
        if (Physics.Raycast(origin, dir, out hit, distance, 1 << 15)) //15 = BlockLayer
        {
            if (hit.collider.transform.gameObject.GetComponent<BlockInfo>())
            {
                return hit.collider.transform.gameObject;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
    GameObject RaycastLadder(Vector3 origin, Vector3 dir, float distance)
    {
        if (Physics.Raycast(origin, dir, out hit, distance, 1 << 20)) //20 = LadderLayer
        {
            Debug.DrawLine(origin, origin + dir, Color.cyan, 2);

            if (hit.collider.transform.parent.gameObject.GetComponent<Block_Ladder>())
            {
                return hit.collider.transform.parent.gameObject;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
    void StairSlopeCorrection(ref GameObject obj)
    {
        BlockInfo info = obj.GetComponent<BlockInfo>();

        if (info.blockType == BlockType.Stair || info.blockType == BlockType.Slope)
        {
            // direction the object is facing
            Vector3 objectDir = obj.transform.forward;

            // direction the player is looking (must be a forward vector!)
            Vector3 playerDir = Movement.Instance.lookingDirection.normalized;

            // dot product
            float dot = Vector3.Dot(objectDir, playerDir);


            //Stair/Slope is facing opposite of player - Over
            if (dot < 0f)
            {
                if (tempOriginPos.y != (obj.transform.position.y - 0.5f) && tempOriginPos.y != (obj.transform.position.y - 1f))
                {
                    obj = null;
                }
            }

            //Stair/Slope is facing the same direction as player - Under
            else
            {
                if (tempOriginPos.y != (obj.transform.position.y + 0.5f) && tempOriginPos.y != (obj.transform.position.y + 1f))
                {
                    obj = null;
                }
            }
        }
    }
    void SetRootLineObjectsOrientation()
    {
        for (int i = 0; i < RootFreeCostBlockList.Count; i++)
        {
            if (RootFreeCostBlockList[i].blockType == BlockType.Stair || RootFreeCostBlockList[i].blockType == BlockType.Slope)
            {
                if (RootFreeCostBlockList[i].block.transform.forward == Vector3.forward || RootFreeCostBlockList[i].block.transform.forward == Vector3.back)
                {
                    RootObjectList[i].transform.SetPositionAndRotation(
                    new Vector3(
                        RootFreeCostBlockList[i].block.transform.position.x,
                        RootFreeCostBlockList[i].block.transform.position.y + 0.2f,
                        RootFreeCostBlockList[i].block.transform.position.z + 0.3f
                    ),
                Quaternion.LookRotation(playerLookDir));

                    RootObjectList[i].transform.localRotation = Quaternion.Euler(new Vector3(RootObjectList[i].transform.localRotation.x - 45f, RootObjectList[i].transform.localRotation.y, RootObjectList[i].transform.localRotation.z));
                }
                else if (RootFreeCostBlockList[i].block.transform.forward == Vector3.left || RootFreeCostBlockList[i].block.transform.forward == Vector3.right)
                {
                    RootObjectList[i].transform.SetPositionAndRotation(
                    new Vector3(
                        RootFreeCostBlockList[i].block.transform.position.x + 0.3f,
                        RootFreeCostBlockList[i].block.transform.position.y + 0.2f,
                        RootFreeCostBlockList[i].block.transform.position.z
                        ),
                Quaternion.LookRotation(playerLookDir));

                    RootObjectList[i].transform.localRotation = Quaternion.Euler(new Vector3(RootObjectList[i].transform.localRotation.x - 45f, RootObjectList[i].transform.localRotation.y + 90, RootObjectList[i].transform.localRotation.z));
                }
            }
            else
            {
                RootObjectList[i].transform.SetPositionAndRotation(
                new Vector3(
                    RootFreeCostBlockList[i].block.transform.position.x,
                    RootFreeCostBlockList[i].block.transform.position.y,
                    RootFreeCostBlockList[i].block.transform.position.z
                    ),
                Quaternion.LookRotation(playerLookDir));
            }
        }
    }
    void MakeRootObjectsVisible()
    {
        StartCoroutine(AnimationDelay(0.04f));
    }
    IEnumerator AnimationDelay(float waitTime)
    {
        for (int i = 0; i < RootFreeCostBlockList.Count; i++)
        {
            yield return new WaitForSeconds(waitTime);

            RootObjectList[i].SetActive(true);
            RootObjectList[i].GetComponent<Animator>().SetTrigger("Activate");
        }
    }
    void ChangeBlockMovementCost()
    {
        for (int i = 0; i < RootFreeCostBlockList.Count; i++)
        {
            RootFreeCostBlockList[i].block.GetComponent<BlockInfo>().movementCost_Temp = 0;
            RootFreeCostBlockList[i].block.GetComponent<BlockInfo>().movementCost = 0;
        }
    }
    #endregion


    //--------------------


    #region DestroyRootLine
    void DestroyRootFreeCostList()
    {
        playerLookDir = Movement.Instance.lookingDirection;

        //Play Destroy-Root Animation
        DestroyRootAnimation();

        //Put Root Animation Object back in the pool, and set their positions to Vector.Zero
        ResetBlockLineObjects();

        //Reset the cost of all blocks in the list back to their normal cost
        SetBlockMovementCostToDefault();

        //Empty the list
        RootFreeCostBlockList.Clear();

        finishedCheckingForBlocks = false;
    }


    //-----


    void DestroyRootAnimation()
    {

    }
    void ResetBlockLineObjects()
    {
        for (int i = 0; i < RootObjectList.Count; i++)
        {
            if (RootObjectList[i].activeInHierarchy)
            {
                RootObjectList[i].GetComponent<Animator>().SetTrigger("Deactivate");
                RootObjectList[i].SetActive(false);
                RootObjectList[i].transform.position = Vector3.zero;
            }
        }
    }
    void SetBlockMovementCostToDefault()
    {
        for (int i = 0; i < RootFreeCostBlockList.Count; i++)
        {
            RootFreeCostBlockList[i].block.GetComponent<BlockInfo>().movementCost_Temp = RootFreeCostBlockList[i].originalMovemetCost;
            RootFreeCostBlockList[i].block.GetComponent<BlockInfo>().movementCost = RootFreeCostBlockList[i].originalMovemetCost;
        }
    }
    #endregion


    //--------------------


    void ResetOnRespawn()
    {
        DestroyRootFreeCostList();
    }
}

[Serializable]
public class RootBlockLineInfo
{
    public GameObject block;
    public int originalMovemetCost;

    public BlockType blockType;
    public Vector3 facingDir;
}