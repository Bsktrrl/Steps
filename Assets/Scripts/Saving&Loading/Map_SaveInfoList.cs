using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class Map_SaveInfoList
{
    public List<Map_SaveInfo> map_SaveInfo_List = new List<Map_SaveInfo>();

    public Map_SaveInfoList()
    {
        if (map_SaveInfo_List == null)
            map_SaveInfo_List = new List<Map_SaveInfo>();
    }

}

[Serializable]
public class Map_SaveInfo
{
    public static event Action Action_SetupMap_hasLoaded;

    [Header("Map Name")]
    public string mapName;
    public MapNameDisplay mapNameDisplay;
    
    [Header("Map is Completed")]
    public bool isCompleted;

    [Header("Collectables")]
    public List<EssenceInfo> essenceList = new List<EssenceInfo>();
    public LevelSkinsInfo levelSkin = new LevelSkinsInfo();
    public List<MaxStepInfo> maxStepList = new List<MaxStepInfo>();

    [Header("Skin available")]
    public SkinType skintype;

    [Header("Abilities available")]
    public AbilitiesGot abilitiesInLevel;
    //public AbilitiesGot abilitiesGotInLevel;


    //--------------------


    //Setup the Map's info
    public void SetupMap()
    {
        SetMapName();
        AddInteractableInfo();

        Action_SetupMap_hasLoaded?.Invoke();
    }
    void SetMapName()
    {
        mapName = SceneManager.GetActiveScene().name;
    }
    void AddInteractableInfo()
    {
        //Find all objects in the scene with a Interactable_GetItem script attached
        Interactable_Pickup[] objectsWithScript = UnityEngine.Object.FindObjectsOfType<Interactable_Pickup>();

        //Add all coins to the list
        foreach (Interactable_Pickup obj in objectsWithScript)
        {
            if (obj.itemReceived == Items.Essence /*&& obj.itemReceived.amount > 0*/)
            {
                EssenceInfo essenceInfo = new EssenceInfo();
                essenceInfo.essenceObj = obj.gameObject;
                essenceInfo.pos = obj.gameObject.transform.position;
                essenceInfo.isTaken = false;

                essenceList.Add(essenceInfo);
            }
            else if (obj.itemReceived == Items.Skin /*&& obj.itemReceived.amount > 0*/)
            {
                LevelSkinsInfo skinInfo = new LevelSkinsInfo();
                skinInfo.skinObj = obj.gameObject;
                skinInfo.pos = obj.gameObject.transform.position;
                skinInfo.isTaken = false;

                levelSkin = skinInfo;
            }
            else if (obj.itemReceived == Items.IncreaseMaxSteps /*&& obj.itemReceived.amount > 0*/)
            {
                MaxStepInfo maxStepInfo = new MaxStepInfo();
                maxStepInfo.maxStepObj = obj.gameObject;
                maxStepInfo.pos = obj.gameObject.transform.position;
                maxStepInfo.isTaken = false;

                maxStepList.Add(maxStepInfo);
            }
        }
    }


    //-----


    public void CorrectingMapObjects()
    {
        //Find all objects in the scene with a Interactable_GetItem script attached
        Map_SaveInfo mapSaveInfo = MapManager.Instance.mapInfo_ToSave;
        Interactable_Pickup[] pickUpList = UnityEngine.Object.FindObjectsOfType<Interactable_Pickup>();

        //Coin Pickups
        for (int i = 0; i < mapSaveInfo.essenceList.Count; i++)
        {
            if (mapSaveInfo.essenceList[i].isTaken)
            {
                foreach (Interactable_Pickup pickup in pickUpList)
                {
                    if (pickup.itemReceived == Items.Essence)
                    {
                        if (mapSaveInfo.essenceList[i].pos.x == pickup.gameObject.transform.position.x
                        && mapSaveInfo.essenceList[i].pos.z == pickup.gameObject.transform.position.z)
                        {
                            pickup.gameObject.SetActive(false);

                            break;
                        }
                    }
                }
            }
        }

        //Skins Pickups
        if (mapSaveInfo.levelSkin.isTaken)
        {
            foreach (Interactable_Pickup pickup in pickUpList)
            {
                if (pickup.itemReceived == Items.Skin)
                {
                    if (mapSaveInfo.levelSkin.pos.x == pickup.gameObject.transform.position.x
                    && mapSaveInfo.levelSkin.pos.z == pickup.gameObject.transform.position.z)
                    {
                        pickup.gameObject.SetActive(false);

                        break;
                    }
                }
            }
        }

        //MaxSteps Pickups
        for (int i = 0; i < mapSaveInfo.maxStepList.Count; i++)
        {
            if (mapSaveInfo.maxStepList[i].isTaken)
            {
                foreach (Interactable_Pickup pickup in pickUpList)
                {
                    if (pickup.itemReceived == Items.IncreaseMaxSteps)
                    {
                        if (mapSaveInfo.maxStepList[i].pos.x == pickup.gameObject.transform.position.x
                        && mapSaveInfo.maxStepList[i].pos.z == pickup.gameObject.transform.position.z)
                        {
                            pickup.gameObject.SetActive(false);

                            break;
                        }
                    }
                }
            }
        }
    }
}

[Serializable]
public class EssenceInfo
{
    public GameObject essenceObj;
    public Vector3 pos;
    public bool isTaken;
}

[Serializable]
public class LevelSkinsInfo
{
    public GameObject skinObj;
    public Vector3 pos;
    public bool isTaken;
}

[Serializable]
public class MaxStepInfo
{
    public GameObject maxStepObj;
    public Vector3 pos;
    public bool isTaken;
}

[Serializable]
public class MapNameDisplay
{
    public string mapNameDisplay_norwegian;
    public string mapNameDisplay_english;
    public string mapNameDisplay_german;
    public string mapNameDisplay_chinese;
    public string mapNameDisplay_japanese;
    public string mapNameDisplay_korean;
}