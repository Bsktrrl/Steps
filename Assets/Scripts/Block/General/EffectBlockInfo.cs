using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class EffectBlockInfo : MonoBehaviour
{
    float counter;
    float waitTime;

    EffectBlockManager effectBlockManager;

    [Header("Is Added")]
    [SerializeField] bool effectBlock_SpawnPoint_isAdded;
    [SerializeField] bool effectBlock_RefillSteps_isAdded;
    [SerializeField] bool effectBlock_Pusher_isAdded;
    public bool effectBlock_Teleporter_isAdded;
    [SerializeField] bool effectBlock_Moveable_isAdded;
    [SerializeField] bool effectBlock_MushroomCircle_isAdded;

    [Header("Child List")]
    [SerializeField] List<GameObject> blockEffectHolding_List;


    //--------------------


    private void Awake()
    {
        effectBlockManager = FindObjectOfType<EffectBlockManager>();

        CheckEffectBlockInChildRecursively(transform);
    }
    private void Start()
    {
        SetWaitTime();
    }

    private void Update()
    {
        //Only trigger on Cubes and Slabs
        if (GetComponent<BlockInfo>().blockType != BlockType.Cube && GetComponent<BlockInfo>().blockType != BlockType.Slab) { return; }

        if (HasAnyEffectBlockChild()) { return; }

        if (effectBlock_SpawnPoint_isAdded) { return; }
        if (effectBlock_RefillSteps_isAdded) { return; }
        if (effectBlock_Pusher_isAdded) { return; }
        if (effectBlock_Teleporter_isAdded) { return; }
        if (effectBlock_Moveable_isAdded) { return; }
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


    void CheckEffectBlockInChildRecursively(Transform parent)
    {
        if (GetComponent<Block_Checkpoint>())
        {
            effectBlock_SpawnPoint_isAdded = true;
            CheckForEffectBlockUpdate_SpawnPoint();
        }
        else if (GetComponent<Block_RefillSteps>())
        {
            effectBlock_RefillSteps_isAdded = true;
            CheckForEffectBlockUpdate_RefillSteps();
        }
        else if (GetComponent<Block_Pusher>())
        {
            effectBlock_Pusher_isAdded = true;
            CheckForEffectBlockUpdate_Pusher();
        }
        else if (GetComponent<Block_Teleport>())
        {
            effectBlock_Teleporter_isAdded = true;
            CheckForEffectBlockUpdate_Teleporter();
        }
        else if (GetComponent<Block_Moveable>())
        {
            effectBlock_Moveable_isAdded = true;
            CheckForEffectBlockUpdate_Moveable();
        }
        else if (GetComponent<Block_MushroomCircle>())
        {
            effectBlock_MushroomCircle_isAdded = true;
            CheckForEffectBlockUpdate_MushroomCircle();
        }
    }


    //--------------------


    void SetWaitTime()
    {
        waitTime = Random.Range(0, 1);
    }

    void CheckForEffectBlockUpdate_SpawnPoint()
    {
        if (!GetComponent<Block_Checkpoint>()) { return; }

        ChangeMovementCost(0);

        if (!effectBlockManager.effectBlock_SpawnPoint_Prefab) { return; }
        if (effectBlock_SpawnPoint_isAdded) { return; }


        //----------


        effectBlock_SpawnPoint_isAdded = true;

        InstantiateEffectBlock(effectBlockManager.effectBlock_SpawnPoint_Prefab);

        AdjustPosition();
        ChangeColor();

        ChangeMovementCost(0);
    }
    void CheckForEffectBlockUpdate_RefillSteps()
    {
        if (!GetComponent<Block_RefillSteps>()) { return; }

        ChangeMovementCost(0);

        if (!effectBlockManager.effectBlock_RefillSteps_Prefab) { return; }
        if (effectBlock_RefillSteps_isAdded) { return; }


        //----------


        effectBlock_RefillSteps_isAdded = true;

        InstantiateEffectBlock(effectBlockManager.effectBlock_RefillSteps_Prefab);

        AdjustPosition();
        ChangeColor();

        ChangeMovementCost(0);
    }
    void CheckForEffectBlockUpdate_Pusher()
    {
        if (!GetComponent<Block_Pusher>()) { return; }

        ChangeMovementCost(0);

        if (!effectBlockManager.effectBlock_Pusher_Prefab) { return; }
        if (effectBlock_Pusher_isAdded) { return; }


        //----------


        effectBlock_Pusher_isAdded = true;

        InstantiateEffectBlock(effectBlockManager.effectBlock_Pusher_Prefab);

        AdjustPosition();
        ChangeColor();

        ChangeMovementCost(0);
    }
    void CheckForEffectBlockUpdate_Teleporter()
    {
        if (!GetComponent<Block_Teleport>()) { return; }

        ChangeMovementCost(0);

        if (!effectBlockManager.effectBlock_Teleporter_Prefab) { return; }
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
        if (!GetComponent<Block_MushroomCircle>()) { return; }

        ChangeMovementCost(-1);

        if (!effectBlockManager.effectBlock_MushroomCircle_Prefab) { return; }
        if (effectBlock_MushroomCircle_isAdded) { return; }


        //----------


        effectBlock_MushroomCircle_isAdded = true;

        InstantiateEffectBlock(effectBlockManager.effectBlock_MushroomCircle_Prefab);

        AdjustPosition();
        ChangeColor();

        ChangeMovementCost(-1);
    }

    void InstantiateEffectBlock(GameObject effectBlock)
    {
        if (HasAnyEffectBlockChild()) return;

        GameObject instance = Instantiate(effectBlock, transform);
        blockEffectHolding_List.Add(instance);

        // Special case for teleporter
        var teleport = GetComponent<Block_Teleport>();
        if (teleport)
        {
            teleport.SetupTeleporter();
        }

        // Optional: if you're worried about duplicates later
        RemoveDuplicateEffectBlocks();
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

    bool HasAnyEffectBlockChild()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<EffectBlock_Reference>()) return true;
        }
        return false;
    }
    void RemoveDuplicateEffectBlocks()
    {
        var seen = new HashSet<GameObject>();
        for (int i = blockEffectHolding_List.Count - 1; i >= 0; i--)
        {
            var obj = blockEffectHolding_List[i];
            if (!obj || seen.Contains(obj))
            {
                if (obj) DestroyImmediate(obj);
                blockEffectHolding_List.RemoveAt(i);
            }
            else
            {
                seen.Add(obj);
            }
        }
    }
}
