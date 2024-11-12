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
        positionToGameObjectMap = new Dictionary<Vector3, GameObject>();

        //Populate the dictionary with all BlockInfo' positions
        foreach (BlockInfo obj in FindObjectsOfType<BlockInfo>())
        {
            if (!positionToGameObjectMap.ContainsKey(obj.gameObject.transform.position) && !obj.gameObject.GetComponent<Block_Elevator>())
            {
                positionToGameObjectMap.Add(obj.gameObject.transform.position, obj.gameObject);
            }
        }
    }

    public GameObject FindGameObjectAtPosition(Vector3 targetPosition)
    {
        // Fast lookup in the dictionary
        if (positionToGameObjectMap.TryGetValue(targetPosition, out GameObject foundObject))
        {
            return foundObject;
        }

        return null;
    }
}
