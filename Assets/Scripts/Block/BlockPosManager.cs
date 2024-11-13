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

    public GameObject FindGameObjectAtPosition(Vector3 targetPosition)
    {
        //print("2. Search for Objects");

        // Fast lookup in the dictionary
        if (positionToGameObjectMap.TryGetValue(targetPosition, out GameObject foundObject))
        {
            //print("3. Find Object");
            return foundObject;
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
