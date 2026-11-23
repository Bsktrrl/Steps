using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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
    public List<PickupSaveInfo> essenceList = new List<PickupSaveInfo>();
    public List<PickupSaveInfo> skinList = new List<PickupSaveInfo>();
    public List<PickupSaveInfo> maxStepList = new List<PickupSaveInfo>();
    public List<PickupSaveInfo> abilityPickupList = new List<PickupSaveInfo>();

    [Header("Skin available")]
    public SkinType skintype;
    public SkinType skintype_2;

    [Header("Abilities available")]
    public AbilitiesGot abilitiesInLevel;
    public AbilitiesGot abilitiesGotInLevel = new AbilitiesGot();


    //--------------------


    //Setup the Map's info
    public void SetupMap()
    {
        SetMapName();
        CorrectingMapObjects();

        Action_SetupMap_hasLoaded?.Invoke();
    }
    void SetMapName()
    {
        mapName = SceneManager.GetActiveScene().name;
    }

    public void CorrectingMapObjects()
    {
        essenceList ??= new List<PickupSaveInfo>();
        skinList ??= new List<PickupSaveInfo>();
        maxStepList ??= new List<PickupSaveInfo>();
        abilityPickupList ??= new List<PickupSaveInfo>();

        var pickupsByNumber = new Dictionary<(Items item, int number), Interactable_Pickup>();
        var duplicateNumbers = new HashSet<(Items item, int number)>();
        Scene scene = SceneManager.GetActiveScene();
        // Some pickups start inactive and can be enabled later in the level.
        foreach (Interactable_Pickup pickup in UnityEngine.Object.FindObjectsOfType<Interactable_Pickup>(true))
        {
            if (pickup.gameObject.scene != scene || !pickup.IsNumberedPickup)
                continue;

            Items item = pickup.itemReceived;
            string kind = item == Items.None ? "Ability" : item.ToString();
            int number = pickup.TryGetComponent(out PickupData pickupData) ? pickupData.GetPickupNumber(item) : 0;
            if (number <= 0)
            {
                Debug.LogWarning($"{kind} pickups need a positive pickup number.", pickup);
                continue;
            }

            var key = (item, number);
            if (!pickupsByNumber.TryAdd(key, pickup) && duplicateNumbers.Add(key))
                Debug.LogWarning($"{kind} pickup number {key.Item2} is used more than once in {mapName}. Assign a unique number to each pickup of this kind.", pickup);
        }

        foreach (var pair in pickupsByNumber)
        {
            if (duplicateNumbers.Contains(pair.Key))
                continue;

            var (item, number) = pair.Key;
            List<PickupSaveInfo> entries = GetPickups(item);
            PickupSaveInfo entry = entries.Find(saved => saved != null && saved.pickupNo == number);
            if (entry == null)
            {
                entry = new PickupSaveInfo { pickupNo = number };
                entries.Add(entry);
            }

            if (entry.isTaken)
                pair.Value.gameObject.SetActive(false);
        }

        registeredPickups = pickupsByNumber;
        foreach (var key in duplicateNumbers)
            registeredPickups.Remove(key);
    }

    [NonSerialized] Dictionary<(Items item, int number), Interactable_Pickup> registeredPickups;

    List<PickupSaveInfo> GetPickups(Items item)
    {
        return item switch
        {
            Items.Essence => essenceList,
            Items.Footprint => maxStepList,
            Items.Skin => skinList,
            _ => abilityPickupList
        };
    }

    internal bool TryMarkPickupTaken(Interactable_Pickup pickup, int number)
    {
        var key = (pickup.itemReceived, number);
        if (registeredPickups == null || !registeredPickups.TryGetValue(key, out Interactable_Pickup registered) || registered != pickup)
            return false;

        PickupSaveInfo entry = GetPickups(key.itemReceived).Find(saved => saved != null && saved.pickupNo == number);
        if (entry == null || entry.isTaken)
            return false;

        entry.isTaken = true;
        return true;
    }

    public int SkinPickupTotal => Mathf.Max(1, skinList?.Count ?? 0);

    public int CountSkinsTaken()
    {
        int count = 0;
        if (skinList != null)
        {
            foreach (PickupSaveInfo skin in skinList)
            {
                if (skin?.isTaken == true)
                    count++;
            }
        }
        return count;
    }
}

[Serializable]
public class PickupSaveInfo
{
    [FormerlySerializedAs("essenceNo")] public int pickupNo;
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
