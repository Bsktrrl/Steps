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

    bool effectBlock_SpawnPoint_isAdded;
    bool effectBlock_RefillSteps_isAdded;
    bool effectBlock_Pusher_isAdded;
    bool effectBlock_Teleporter_isAdded;


    //--------------------


    private void Awake()
    {
        effectBlockManager = FindObjectOfType<EffectBlockManager>();
    }
    private void Start()
    {
        SetWaitTime();

        //GetComponent<BlockInfo>().movementCost_Temp = 0;
        //GetComponent<BlockInfo>().movementCost = 0;
    }

    private void Update()
    {
        //Only trigger on Cubes and Slabs
        if (GetComponent<BlockInfo>().blockType != BlockType.Cube && GetComponent<BlockInfo>().blockType != BlockType.Slab) { return; }
        if (effectBlock_SpawnPoint_isAdded) { return; }

        counter += Time.deltaTime;

        if (counter >= waitTime)
        {
            CheckForEffectBlockUpdate_SpawnPoint();
            CheckForEffectBlockUpdate_RefillSteps();
            CheckForEffectBlockUpdate_Pusher();
            CheckForEffectBlockUpdate_Teleporter();

            counter = 0;
            SetWaitTime();
        }
    }


    //--------------------


    void SetWaitTime()
    {
        waitTime = Random.Range(0, 1);
    }

    void CheckForEffectBlockUpdate_SpawnPoint()
    {
        if (GetComponent<Block_UpdateSpawnPoint>() && !effectBlock_SpawnPoint_isAdded)
        {
            effectBlock_SpawnPoint_isAdded = true;

            foreach (Transform child in transform)
            {
                if (child.GetComponent<EffectBlock_Reference>())
                {
                    return;
                }
            }

            GameObject effectBlock_SpawnPoint_Canvas = Instantiate(effectBlockManager.effectBlock_SpawnPoint_Canvas, transform);

            ChangeColor(ref effectBlock_SpawnPoint_Canvas);

            GetComponent<BlockInfo>().movementCost_Temp = 0;
            GetComponent<BlockInfo>().movementCost = 0;
        }
    }
    void CheckForEffectBlockUpdate_RefillSteps()
    {
        if (GetComponent<Block_RefillSteps>() && !effectBlock_RefillSteps_isAdded)
        {
            effectBlock_RefillSteps_isAdded = true;

            foreach (Transform child in transform)
            {
                if (child.GetComponent<EffectBlock_Reference>())
                {
                    return;
                }
            }

            GameObject effectBlock_RefillSteps_Canvas = Instantiate(effectBlockManager.effectBlock_RefillSteps_Canvas, transform);

            ChangeColor(ref effectBlock_RefillSteps_Canvas);

            GetComponent<BlockInfo>().movementCost_Temp = 0;
            GetComponent<BlockInfo>().movementCost = 0;
        }
    }
    void CheckForEffectBlockUpdate_Pusher()
    {
        if (GetComponent<Block_Pusher>() && !effectBlock_Pusher_isAdded)
        {
            effectBlock_Pusher_isAdded = true;

            foreach (Transform child in transform)
            {
                if (child.GetComponent<EffectBlock_Reference>())
                {
                    return;
                }
            }

            GameObject effectBlock_Pusher_Canvas = Instantiate(effectBlockManager.effectBlock_Pusher_Canvas, transform);

            ChangeColor(ref effectBlock_Pusher_Canvas);
        }
    }
    void CheckForEffectBlockUpdate_Teleporter()
    {
        if (GetComponent<Block_Teleport>() && !effectBlock_Teleporter_isAdded)
        {
            effectBlock_Teleporter_isAdded = true;

            foreach (Transform child in transform)
            {
                if (child.GetComponent<EffectBlock_Reference>())
                {
                    return;
                }
            }

            GameObject effectBlock_Teleporter_Canvas = Instantiate(effectBlockManager.effectBlock_Teleporter_Canvas, transform);

            ChangeColor(ref effectBlock_Teleporter_Canvas);
        }
    }

    void ChangeColor(ref GameObject canvas)
    {
        switch (GetComponent<BlockInfo>().blockElement)
        {
            case BlockElement.None:
                break;

            case BlockElement.Grass:
                canvas.GetComponentInChildren<Image>().color = effectBlockManager.color_Grass;
                break;
            case BlockElement.Wood:
                canvas.GetComponentInChildren<Image>().color = effectBlockManager.color_Wood;
                break;
            case BlockElement.Sand:
                canvas.GetComponentInChildren<Image>().color = effectBlockManager.color_Sand;
                break;
            case BlockElement.Sandstone:
                canvas.GetComponentInChildren<Image>().color = effectBlockManager.color_SandBlock;
                break;

            default:
                break;
        }
    }
}
