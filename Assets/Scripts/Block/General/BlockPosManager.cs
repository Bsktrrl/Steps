using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPosManager : Singleton<BlockPosManager>
{
    private Dictionary<Vector3, GameObject> positionToGameObjectMap;


    //--------------------


    void Awake()
    {
        BuildDictionary();
    }


    //--------------------


    void BuildDictionary()
    {
        BlockInfo[] blocks = FindObjectsOfType<BlockInfo>();
        positionToGameObjectMap = new Dictionary<Vector3, GameObject>(blocks.Length);

        //Populate the dictionary with all BlockInfo' positions
        foreach (BlockInfo obj in blocks)
        {
            Vector3 position = obj.transform.position;
            if (!positionToGameObjectMap.ContainsKey(position) &&
                !obj.TryGetComponent<Block_Elevator>(out _) &&
                !obj.TryGetComponent<Block_Elevator_StepOn>(out _))
            {
                positionToGameObjectMap.Add(position, obj.gameObject);
            }
        }
    }

    public GameObject FindGameObjectAtPosition(Vector3 targetPosition, GameObject searchingObj)
    {
        //print("2. Search for Objects");

        // Prefer the exact position before considering half-height offsets.
        if (positionToGameObjectMap.TryGetValue(targetPosition, out GameObject foundObject1))
            return foundObject1;

        if (positionToGameObjectMap.TryGetValue(targetPosition + (Vector3.down * 0.5f), out GameObject lowerBlock))
        {
            BlockType blockType = lowerBlock.GetComponent<BlockInfo>().blockType;
            if (blockType == BlockType.Stair || blockType == BlockType.Slope)
                return lowerBlock;
        }

        if (positionToGameObjectMap.TryGetValue(targetPosition + (Vector3.up * 0.5f), out GameObject upperBlock))
        {
            BlockType blockType = searchingObj.GetComponent<BlockInfo>().blockType;
            if (blockType == BlockType.Stair || blockType == BlockType.Slope)
                return upperBlock;
        }

        return null;
    }
}
