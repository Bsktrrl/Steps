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
            //Is there a block adjacent?
            GameObject tempBlock_Adjacent = new GameObject();
            tempBlock_Adjacent = RaycastBlock(tempOriginPos, Movement.Instance.lookingDirection, 1f);

            //if (RootFreeCostBlockList.Count() > 0 && (RootFreeCostBlockList[RootFreeCostBlockList.Count - 1].blockType == BlockType.Stair || RootFreeCostBlockList[RootFreeCostBlockList.Count - 1].blockType == BlockType.Slope))
            //{
            //    tempBlock_Adjacent = RaycastBlock(tempOriginPos + Vector3.down, Movement.Instance.lookingDirection, 1f);
            //}
            //else
            //{
            //    tempBlock_Adjacent = RaycastBlock(tempOriginPos, Movement.Instance.lookingDirection, 1f);
            //}

            if (tempBlock_Adjacent)
            {
                //is there a block over adjacent?
                GameObject tempBlock_Over = RaycastBlock(tempBlock_Adjacent.transform.position, Vector3.up, 1f);

                if (!tempBlock_Over) 
                {
                    SetupEntryInBlockList(tempBlock_Adjacent, false);

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
            #endregion


            #region Check stairs/slopes
            if (!blockIsFound)
            {
                tempBlock_Adjacent = RaycastBlock(tempOriginPos, Movement.Instance.lookingDirection, 1f);
                GameObject tempBlock_Over = RaycastBlock(tempOriginPos + Vector3.up, Movement.Instance.lookingDirection, 1f);

                //Is there a stair/slope adjacent?
                if (tempBlock_Adjacent && (tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_Adjacent.GetComponent<BlockInfo>().blockType == BlockType.Slope))
                {
                    //Check orientation of stair/slope

                    SetupEntryInBlockList(tempBlock_Adjacent, true);

                    blockIsFound = true;
                }
                else
                {
                    blockIsFound = false;
                }

                //is there a stair/slope over adjacent?
                if (!blockIsFound && tempBlock_Adjacent && (tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Stair || tempBlock_Over.GetComponent<BlockInfo>().blockType == BlockType.Slope))
                {
                    //Check orientation of stair/slope

                    SetupEntryInBlockList(tempBlock_Over, true);

                    blockIsFound = true;
                }
                else
                {
                    blockIsFound = false;
                }
            }
            #endregion


            #region Check after slope if landing on free, after falling

            if (!blockIsFound)
            {

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
    void SetRootLineObjectsOrientation()
    {
        for (int i = 0; i < RootFreeCostBlockList.Count; i++)
        {
            if (RootFreeCostBlockList[i].blockType == BlockType.Stair || RootFreeCostBlockList[i].blockType == BlockType.Slope)
            {
                if (RootFreeCostBlockList[i].block.transform.forward == Vector3.forward || RootFreeCostBlockList[i].block.transform.forward == Vector3.back)
                {
                    print("10. Stair/Slope 0, 180, -180");
                    RootObjectList[i].transform.SetPositionAndRotation(
                    new Vector3(
                        RootFreeCostBlockList[i].block.transform.position.x,
                        RootFreeCostBlockList[i].block.transform.position.y + 0.2f,
                        RootFreeCostBlockList[i].block.transform.position.z + 0.3f
                    ),
                Quaternion.LookRotation(Movement.Instance.lookingDirection));

                    RootObjectList[i].transform.localRotation = Quaternion.Euler(new Vector3(RootObjectList[i].transform.localRotation.x - 45f, RootObjectList[i].transform.localRotation.y, RootObjectList[i].transform.localRotation.z));
                }
                else if (RootFreeCostBlockList[i].block.transform.forward == Vector3.left || RootFreeCostBlockList[i].block.transform.forward == Vector3.right)
                {
                    print("20. Stair/Slope 90, -90, 270, -270");
                    RootObjectList[i].transform.SetPositionAndRotation(
                    new Vector3(
                        RootFreeCostBlockList[i].block.transform.position.x + 0.3f,
                        RootFreeCostBlockList[i].block.transform.position.y + 0.2f,
                        RootFreeCostBlockList[i].block.transform.position.z
                        ),
                Quaternion.LookRotation(Movement.Instance.lookingDirection));

                    RootObjectList[i].transform.localRotation = Quaternion.Euler(new Vector3(RootObjectList[i].transform.localRotation.x - 45f, RootObjectList[i].transform.localRotation.y + 90, RootObjectList[i].transform.localRotation.z));
                }
                else
                {
                    print("30. Stair/Slope - Else");
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
                Quaternion.LookRotation(Movement.Instance.lookingDirection));
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