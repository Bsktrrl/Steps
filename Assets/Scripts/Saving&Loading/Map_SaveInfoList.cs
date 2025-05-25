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

    public string mapName;
    public bool isCompleted;

    public List<CoinInfo> coinList = new List<CoinInfo>();
    public List<CollectableInfo> collectableList = new List<CollectableInfo>();
    public List<MaxStepInfo> maxStepList = new List<MaxStepInfo>();

    public SkinType skintype;

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
            if (obj.itemReceived == Items.Coin /*&& obj.itemReceived.amount > 0*/)
            {
                CoinInfo coinInfo = new CoinInfo();
                coinInfo.coinObj = obj.gameObject;
                coinInfo.pos = obj.gameObject.transform.position;
                coinInfo.isTaken = false;

                coinList.Add(coinInfo);
            }
            else if (obj.itemReceived == Items.Collectable /*&& obj.itemReceived.amount > 0*/)
            {
                CollectableInfo collectableInfo = new CollectableInfo();
                collectableInfo.collectableObj = obj.gameObject;
                collectableInfo.pos = obj.gameObject.transform.position;
                collectableInfo.isTaken = false;

                collectableList.Add(collectableInfo);
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
        for (int i = 0; i < mapSaveInfo.coinList.Count; i++)
        {
            if (mapSaveInfo.coinList[i].isTaken)
            {
                foreach (Interactable_Pickup pickup in pickUpList)
                {
                    if (pickup.itemReceived == Items.Coin)
                    {
                        if (mapSaveInfo.coinList[i].pos.x == pickup.gameObject.transform.position.x
                        && mapSaveInfo.coinList[i].pos.z == pickup.gameObject.transform.position.z)
                        {
                            pickup.gameObject.SetActive(false);

                            break;
                        }
                    }
                }
            }
        }

        //Collectable Pickups
        for (int i = 0; i < mapSaveInfo.collectableList.Count; i++)
        {
            if (mapSaveInfo.collectableList[i].isTaken)
            {
                foreach (Interactable_Pickup pickup in pickUpList)
                {
                    if (pickup.itemReceived == Items.Collectable)
                    {
                        if (mapSaveInfo.collectableList[i].pos.x == pickup.gameObject.transform.position.x
                        && mapSaveInfo.collectableList[i].pos.z == pickup.gameObject.transform.position.z)
                        {
                            pickup.gameObject.SetActive(false);

                            break;
                        }
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
public class CoinInfo
{
    public GameObject coinObj;
    public Vector3 pos;
    public bool isTaken;
}

[Serializable]
public class CollectableInfo
{
    public GameObject collectableObj;
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
