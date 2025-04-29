using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class EffectBlockInfo : MonoBehaviour
{
    float counter;
    float waitTime;

    EffectBlockManager effectBlockManager = new EffectBlockManager();

    [SerializeField] bool effectBlock_SpawnPoint_isAdded;
    [SerializeField] bool effectBlock_RefillSteps_isAdded;
    [SerializeField] bool effectBlock_Pusher_isAdded;
    [SerializeField] bool effectBlock_Teleporter_isAdded;
    [SerializeField] bool effectBlock_Moveable_isAdded;
    [SerializeField] bool effectBlock_MushroomCircle_isAdded;


    [SerializeField] List<GameObject> blockEffectHolding_List;


    //--------------------


    private void Awake()
    {
        effectBlockManager = FindObjectOfType<EffectBlockManager>();
    }
    private void Start()
    {
        SetWaitTime();
    }

    private void Update()
    {
        //Only trigger on Cubes and Slabs
        if (GetComponent<BlockInfo>().blockType != BlockType.Cube && GetComponent<BlockInfo>().blockType != BlockType.Slab) { return; }

        if (blockEffectHolding_List.Count > 1) { return; }

        if (effectBlock_SpawnPoint_isAdded) { return; }
        if (effectBlock_RefillSteps_isAdded) { return; }
        if (effectBlock_Pusher_isAdded) { return; }
        if (effectBlock_Teleporter_isAdded) { return; }
        if (effectBlock_MushroomCircle_isAdded) { return; }

        counter += Time.deltaTime;

        if (counter >= waitTime)
        {
            CheckForEffectBlockUpdate_SpawnPoint();
            CheckForEffectBlockUpdate_RefillSteps();
            CheckForEffectBlockUpdate_Pusher();
            CheckForEffectBlockUpdate_Teleporter();
            CheckForEffectBlockUpdate_Moveable();
            CheckForEffectBlockUpdate_MushroomCircle();

            counter = 0;
            SetWaitTime();
        }
    }

    private void OnEnable()
    {
        Block_Snow.Action_SnowSetup_End += AdjustPosition_Snow;
    }
    private void OnDisable()
    {
        Block_Snow.Action_SnowSetup_End -= AdjustPosition_Snow;
    }



    //--------------------


    void SetWaitTime()
    {
        waitTime = Random.Range(0, 1);
    }

    void CheckForEffectBlockUpdate_SpawnPoint()
    {
        if (!effectBlockManager.effectBlock_SpawnPoint_Prefab) { return; }
        if (!GetComponent<Block_SpawnPoint>()) { return; }
        if (effectBlock_SpawnPoint_isAdded) { return; }


        //----------


        effectBlock_SpawnPoint_isAdded = true;

        ChangeMovementCost(0);

        InstantiateEffectBlock(effectBlockManager.effectBlock_SpawnPoint_Prefab);

        AdjustPosition();
        ChangeColor();

        ChangeMovementCost(0);
    }
    void CheckForEffectBlockUpdate_RefillSteps()
    {
        if (!effectBlockManager.effectBlock_RefillSteps_Prefab) { return; }
        if (!GetComponent<Block_RefillSteps>()) { return; }
        if (effectBlock_RefillSteps_isAdded) { return; }


        //----------


        effectBlock_RefillSteps_isAdded = true;

        ChangeMovementCost(0);

        InstantiateEffectBlock(effectBlockManager.effectBlock_RefillSteps_Prefab);

        AdjustPosition();
        ChangeColor();

        ChangeMovementCost(0);
    }
    void CheckForEffectBlockUpdate_Pusher()
    {
        if (!effectBlockManager.effectBlock_Pusher_Prefab) { return; }
        if (!GetComponent<Block_Pusher>()) { return; }
        if (effectBlock_Pusher_isAdded) { return; }


        //----------


        effectBlock_Pusher_isAdded = true;

        ChangeMovementCost(0);

        InstantiateEffectBlock(effectBlockManager.effectBlock_Pusher_Prefab);

        AdjustPosition();
        ChangeColor();

        ChangeMovementCost(0);
    }
    void CheckForEffectBlockUpdate_Teleporter()
    {
        if (!effectBlockManager.effectBlock_Teleporter_Prefab) { return; }
        if (!GetComponent<Block_Teleport>()) { return; }
        if (effectBlock_Teleporter_isAdded) { return; }


        //----------


        effectBlock_Teleporter_isAdded = true;

        ChangeMovementCost(0);

        InstantiateEffectBlock(effectBlockManager.effectBlock_Teleporter_Prefab);

        AdjustPosition();
        ChangeColor();

        ChangeMovementCost(0);
    }
    void CheckForEffectBlockUpdate_Moveable()
    {
        if (!effectBlockManager.effectBlock_Moveable_Prefab) { return; }
        if (!GetComponent<Block_Moveable>()) { return; }
        if (effectBlock_Moveable_isAdded) { return; }


        //----------


        effectBlock_Moveable_isAdded = true;

        InstantiateEffectBlock(effectBlockManager.effectBlock_Moveable_Prefab);

        AdjustPosition();
        ChangeColor();
    }
    void CheckForEffectBlockUpdate_MushroomCircle()
    {
        if (!effectBlockManager.effectBlock_MushroomCircle_Prefab) { return; }
        if (!GetComponent<Block_MushroomCircle>()) { return; }
        if (effectBlock_MushroomCircle_isAdded) { return; }


        //----------


        effectBlock_MushroomCircle_isAdded = true;

        ChangeMovementCost(-1);

        InstantiateEffectBlock(effectBlockManager.effectBlock_MushroomCircle_Prefab);

        AdjustPosition();
        ChangeColor();

        ChangeMovementCost(-1);
    }

    void InstantiateEffectBlock(GameObject effectBlock)
    {
        //Add EffectBlock if there isn't any
        if (blockEffectHolding_List.Count <= 0)
        {
            blockEffectHolding_List.Add(Instantiate(effectBlock, transform));
        }

        //Check for multiple EffectBlocks
        if (blockEffectHolding_List.Count > 1)
        {
            for (int i = blockEffectHolding_List.Count - 1; i < 1; i--)
            {
                DestroyImmediate(blockEffectHolding_List[i]);

                blockEffectHolding_List.RemoveAt(i);
            }
        }
    }
    void ChangeMovementCost(int cost)
    {
        GetComponent<BlockInfo>().movementCost_Temp = cost;
        GetComponent<BlockInfo>().movementCost = cost;
    }
    void ChangeColor()
    {
        //Color colorTemp = new Color(GetComponent<BlockInfo>().stepCostText_Color.r - 0.25f, GetComponent<BlockInfo>().stepCostText_Color.g - 0.25f, GetComponent<BlockInfo>().stepCostText_Color.b - 0.25f, 1);

        //foreach (Transform child in transform)
        //{
        //    if (child.GetComponent<EffectBlock_Reference>())
        //    {
        //        foreach (Transform childchild in child)
        //        {
        //            childchild.GetComponentInChildren<Image>().color = colorTemp;
        //        }
        //    }
        //}
    }
    void AdjustPosition()
    {
        //foreach (Transform child in transform)
        //{
        //    if (child.GetComponent<EffectBlock_Reference>())
        //    {
        //        //float temp = GetComponent<BlockStepCostDisplay>().stepCostDisplay_Parent.transform.localPosition.y;

        //        child.GetComponent<RectTransform>().localPosition = new Vector3(0, (transform.localEulerAngles.y + 0.55f), 0);

        //        break;
        //    }
        //}
    }
    void AdjustPosition_Snow()
    {
        //foreach (Transform child in transform)
        //{
        //    if (child.GetComponent<EffectBlock_Reference>())
        //    {
        //        //float temp = GetComponent<BlockStepCostDisplay>().stepCostDisplay_Parent.transform.localPosition.y;

        //        child.GetComponent<RectTransform>().localPosition = new Vector3(0, (transform.localEulerAngles.y + 0.55f), 0);

        //        break;
        //    }
        //}
    }
}
