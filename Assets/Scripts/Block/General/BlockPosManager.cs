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
            if (!positionToGameObjectMap.ContainsKey(obj.gameObject.transform.position) && !obj.gameObject.GetComponent<Block_Elevator_Normal>() && !obj.gameObject.GetComponent<Block_Elevator_StepOn>())
            {
                positionToGameObjectMap.Add(obj.gameObject.transform.position, obj.gameObject);
            }
        }
    }

    public GameObject FindGameObjectAtPosition(Vector3 targetPosition, GameObject searchingObj)
    {
        //print("2. Search for Objects");

        // Fast lookup in the dictionary
        if (positionToGameObjectMap.TryGetValue(targetPosition, out GameObject foundObject1))
        {
            //if (foundObject1.GetComponent<BlockInfo>().blockType == BlockType.HalfSlab)
            //{
            //    //return foundObject6;
            //    print("HalfSlab");
            //    return null;
            //}
            //else
            //{
            //    return foundObject1;
            //}

            return foundObject1;
        }

        else if (positionToGameObjectMap.TryGetValue(targetPosition + (Vector3.down * 0.5f), out GameObject foundObject2) && foundObject2.GetComponent<BlockInfo>().blockType == BlockType.Stair)
        {
            return foundObject2;
        }
        else if (positionToGameObjectMap.TryGetValue(targetPosition + (Vector3.down * 0.5f), out GameObject foundObject3) && foundObject3.GetComponent<BlockInfo>().blockType == BlockType.Slope)
        {
            return foundObject3;
        }

        else if (positionToGameObjectMap.TryGetValue(targetPosition + (Vector3.up * 0.5f), out GameObject foundObject4) && searchingObj.GetComponent<BlockInfo>().blockType == BlockType.Stair)
        {
            return foundObject4;
        }
        else if (positionToGameObjectMap.TryGetValue(targetPosition + (Vector3.up * 0.5f), out GameObject foundObject5) && searchingObj.GetComponent<BlockInfo>().blockType == BlockType.Slope)
        {
            return foundObject5;
        }

        else
        {
            //print("Did NOT Find" + targetPosition + " | Name: " + searchingObj + " | Pos: " + searchingObj.transform.position + " | LocalPos: " + searchingObj.transform.localPosition);

            if (targetPosition == new Vector3(-5, 0, -13))
            {
                //print("Did NOT Find" + targetPosition);
            }
        }

        return null;
    }
}
