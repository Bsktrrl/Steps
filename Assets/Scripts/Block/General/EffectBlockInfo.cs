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
        if (effectBlock_SpawnPoint_isAdded) { return; }
        if (effectBlock_RefillSteps_isAdded) { return; }
        if (effectBlock_Pusher_isAdded) { return; }
        if (effectBlock_Teleporter_isAdded) { return; }

        counter += Time.deltaTime;

        if (counter >= waitTime)
        {
            CheckForEffectBlockUpdate_SpawnPoint();
            CheckForEffectBlockUpdate_RefillSteps();
            CheckForEffectBlockUpdate_Pusher();
            CheckForEffectBlockUpdate_Teleporter();
            CheckForEffectBlockUpdate_Moveable();

            counter = 0;
            SetWaitTime();
        }
    }

    private void OnEnable()
    {
        Block_Snow.Action_SnowSetup_End += AdjustPosition;
    }
    private void OnDisable()
    {
        Block_Snow.Action_SnowSetup_End -= AdjustPosition;
    }



    //--------------------


    void SetWaitTime()
    {
        waitTime = Random.Range(0, 1);
    }

    void CheckForEffectBlockUpdate_SpawnPoint()
    {
        if (GetComponent<Block_SpawnPoint>() && !effectBlock_SpawnPoint_isAdded)
        {
            effectBlock_SpawnPoint_isAdded = true;

            GetComponent<BlockInfo>().movementCost_Temp = 0;
            GetComponent<BlockInfo>().movementCost = 0;

            foreach (Transform child in transform)
            {
                if (child.GetComponent<EffectBlock_Reference>())
                {
                    return;
                }
            }

            GameObject effectBlock_SpawnPoint_Canvas = Instantiate(effectBlockManager.effectBlock_SpawnPoint_Canvas, transform);

            AdjustPosition();
            ChangeColor();

            GetComponent<BlockInfo>().movementCost_Temp = 0;
            GetComponent<BlockInfo>().movementCost = 0;
        }
    }
    void CheckForEffectBlockUpdate_RefillSteps()
    {
        if (GetComponent<Block_RefillSteps>() && !effectBlock_RefillSteps_isAdded)
        {
            effectBlock_RefillSteps_isAdded = true;

            GetComponent<BlockInfo>().movementCost_Temp = 0;
            GetComponent<BlockInfo>().movementCost = 0;

            foreach (Transform child in transform)
            {
                if (child.GetComponent<EffectBlock_Reference>())
                {
                    return;
                }
            }

            GameObject effectBlock_RefillSteps_Canvas = Instantiate(effectBlockManager.effectBlock_RefillSteps_Canvas, transform);

            AdjustPosition();
            ChangeColor();

            GetComponent<BlockInfo>().movementCost_Temp = 0;
            GetComponent<BlockInfo>().movementCost = 0;
        }
    }
    void CheckForEffectBlockUpdate_Pusher()
    {
        if (GetComponent<Block_Pusher>() && !effectBlock_Pusher_isAdded)
        {
            effectBlock_Pusher_isAdded = true;

            GetComponent<BlockInfo>().movementCost_Temp = 0;
            GetComponent<BlockInfo>().movementCost = 0;

            foreach (Transform child in transform)
            {
                if (child.GetComponent<EffectBlock_Reference>())
                {
                    return;
                }
            }

            GameObject effectBlock_Pusher_Canvas = Instantiate(effectBlockManager.effectBlock_Pusher_Canvas, transform);

            AdjustPosition();
            ChangeColor();
        }
    }
    void CheckForEffectBlockUpdate_Teleporter()
    {
        if (GetComponent<Block_Teleport>() && !effectBlock_Teleporter_isAdded)
        {
            effectBlock_Teleporter_isAdded = true;

            GetComponent<BlockInfo>().movementCost_Temp = 0;
            GetComponent<BlockInfo>().movementCost = 0;

            foreach (Transform child in transform)
            {
                if (child.GetComponent<EffectBlock_Reference>())
                {
                    return;
                }
            }

            GameObject effectBlock_Teleporter_Canvas = Instantiate(effectBlockManager.effectBlock_Teleporter_Canvas, transform);

            AdjustPosition();
            ChangeColor();
        }
    }
    void CheckForEffectBlockUpdate_Moveable()
    {
        if (GetComponent<Block_Moveable>() && !effectBlock_Moveable_isAdded)
        {
            effectBlock_Moveable_isAdded = true;

            foreach (Transform child in transform)
            {
                if (child.GetComponent<EffectBlock_Reference>())
                {
                    return;
                }
            }

            GameObject effectBlock_Moveable_Canvas = Instantiate(effectBlockManager.effectBlock_Moveable_Canvas, transform);

            AdjustPosition();
            ChangeColor();
        }
    }

    void ChangeColor()
    {
        Color colorTemp = new Color(GetComponent<BlockInfo>().stepCostText_Color.r - 0.25f, GetComponent<BlockInfo>().stepCostText_Color.g - 0.25f, GetComponent<BlockInfo>().stepCostText_Color.b - 0.25f, 1);

        foreach (Transform child in transform)
        {
            if (child.GetComponent<EffectBlock_Reference>())
            {
                foreach (Transform childchild in child)
                {
                    childchild.GetComponentInChildren<Image>().color = colorTemp;
                }
            }
        }
    }
    void AdjustPosition()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<EffectBlock_Reference>())
            {
                float temp = GetComponent<BlockStepCostDisplay>().stepCostDisplay_Parent.transform.localPosition.y;

                child.GetComponent<RectTransform>().localPosition = new Vector3(0, (temp + 0.55f), 0);

                break;
            }
        }
    }
}
