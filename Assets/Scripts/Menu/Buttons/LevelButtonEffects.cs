using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonEffects : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Color color_CAN;
    [SerializeField] Color color_CANNOT;

    public bool canBePlayed;
    public int canBePlayerCount;


    //--------------------


    private void Update()
    {
        canBePlayerCount = 0;

        //If there isn't any requirements to play the level
        if (GetComponent<LoadLevel>().levelsToBeFinished.Count <= 0)
        {
            canBePlayerCount = 0;
        }

        //If there are requirements to play the level
        for (int i = 0; i < GetComponent<LoadLevel>().levelsToBeFinished.Count; i++)
        {
            for (int j = 0; j < MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List.Count; j++)
            {
                if (GetComponent<LoadLevel>().levelsToBeFinished[i].GetComponent<LoadLevel>().levelToPlay == MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List[j].mapName)
                {
                    if (MenuLevelInfo.Instance.mapInfo_ToSave.map_SaveInfo_List[j].isCompleted)
                    {
                        canBePlayerCount++;
                    }
                    else
                    {
                        image.color = color_CANNOT;
                    }
                }
                else
                {
                    image.color = color_CANNOT;
                }
            }
        }

        if (canBePlayerCount >= GetComponent<LoadLevel>().levelsToBeFinished.Count)
        {
            print("1000. Can play level: " + gameObject.name);
            canBePlayed = true;
            image.color = color_CAN;
        }
        else
        {
            print("2000. Cannot play level: " + gameObject.name);
            canBePlayed = false;
            image.color = color_CANNOT;
        }
    }
}
