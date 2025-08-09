using System;
using System.Collections;
using UnityEngine;

public class SkinsManager : Singleton<SkinsManager>
{
    public static event Action Action_SkinInfoFinishedSetup;

    public SkinsShopInfo skinsInfo;
    
    [Header("Buttons")]
    public GameObject skinShopButton_Region1_Level1;
    public GameObject skinShopButton_Region1_Level2;
    public GameObject skinShopButton_Region1_Level3;
    public GameObject skinShopButton_Region1_Level4;
    public GameObject skinShopButton_Region1_Level5;
    public GameObject skinShopButton_Region1_Level6;

    public GameObject skinShopButton_Region2_Level1;
    public GameObject skinShopButton_Region2_Level2;
    public GameObject skinShopButton_Region2_Level3;
    public GameObject skinShopButton_Region2_Level4;
    public GameObject skinShopButton_Region2_Level5;
    public GameObject skinShopButton_Region2_Level6;

    public GameObject skinShopButton_Region3_Level1;
    public GameObject skinShopButton_Region3_Level2;
    public GameObject skinShopButton_Region3_Level3;
    public GameObject skinShopButton_Region3_Level4;
    public GameObject skinShopButton_Region3_Level5;
    public GameObject skinShopButton_Region3_Level6;

    public GameObject skinShopButton_Region4_Level1;
    public GameObject skinShopButton_Region4_Level2;
    public GameObject skinShopButton_Region4_Level3;
    public GameObject skinShopButton_Region4_Level4;
    public GameObject skinShopButton_Region4_Level5;
    public GameObject skinShopButton_Region4_Level6;

    public GameObject skinShopButton_Region5_Level1;
    public GameObject skinShopButton_Region5_Level2;
    public GameObject skinShopButton_Region5_Level3;
    public GameObject skinShopButton_Region5_Level4;
    public GameObject skinShopButton_Region5_Level5;
    public GameObject skinShopButton_Region5_Level6;

    public GameObject skinShopButton_Region6_Level1;
    public GameObject skinShopButton_Region6_Level2;
    public GameObject skinShopButton_Region6_Level3;
    public GameObject skinShopButton_Region6_Level4;
    public GameObject skinShopButton_Region6_Level5;
    public GameObject skinShopButton_Region6_Level6;



    //--------------------


    private void OnEnable()
    {
        //DataManager.Action_dataHasLoaded += CheckIfSkinIsAquired;
        DataPersistanceManager.Action_NewGame += ClearSkinsShopInfo;
    }
    private void OnDisable()
    {
        //DataManager.Action_dataHasLoaded -= CheckIfSkinIsAquired;
        DataPersistanceManager.Action_NewGame -= ClearSkinsShopInfo;
    }


    //--------------------


    public void LoadData()
    {
        skinsInfo = DataManager.Instance.skinsInfo_Store;
        //CheckIfSkinIsAquired();
    }
    public void SaveData()
    {
        DataManager.Instance.skinsInfo_Store = skinsInfo;

        DataPersistanceManager.instance.SaveGame();
    }


    //--------------------


    void CheckIfSkinIsAquired()
    {
        for (int i = 0; i < DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Count; i++)
        {
            if (SearchSkinInfo(i, skinsInfo.skin_Region1_level1)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region1_level2)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region1_level3)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region1_level4)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region1_level5)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region1_level6)) { continue; }

            if (SearchSkinInfo(i, skinsInfo.skin_Region2_level1)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region2_level2)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region2_level3)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region2_level4)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region2_level5)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region2_level6)) { continue; }

            if (SearchSkinInfo(i, skinsInfo.skin_Region3_level1)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region3_level2)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region3_level3)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region3_level4)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region3_level5)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region3_level6)) { continue; }

            if (SearchSkinInfo(i, skinsInfo.skin_Region4_level1)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region4_level2)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region4_level3)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region4_level4)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region4_level5)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region4_level6)) { continue; }

            if (SearchSkinInfo(i, skinsInfo.skin_Region5_level1)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region5_level2)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region5_level3)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region5_level4)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region5_level5)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region5_level6)) { continue; }

            if (SearchSkinInfo(i, skinsInfo.skin_Region6_level1)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region6_level2)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region6_level3)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region6_level4)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region6_level5)) { continue; }
            if (SearchSkinInfo(i, skinsInfo.skin_Region6_level6)) { continue; }
        }

        SaveData();

        Action_SkinInfoFinishedSetup?.Invoke();
    }
    bool SearchSkinInfo(int index, SkinInfo skinInfo)
    {
        if (skinInfo.skin_Type == DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[index].skintype)
        {
            for (int i = 0; i < DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[index].skinsList.Count; i++)
            {
                if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[index].skinsList[i].isTaken)
                {
                    skinInfo.skin_isInactive = false;
                    skinInfo.skin_isAquired = true;

                    return true;
                }
            }
        }

        skinInfo.skin_isInactive = true;
        skinInfo.skin_isAquired = false;

        return false;
    }


    //--------------------


    void ClearSkinsShopInfo()
    {
        ClearSkin(skinsInfo.skin_Region1_level1);
        ClearSkin(skinsInfo.skin_Region1_level2);
        ClearSkin(skinsInfo.skin_Region1_level3);
        ClearSkin(skinsInfo.skin_Region1_level4);
        ClearSkin(skinsInfo.skin_Region1_level5);
        ClearSkin(skinsInfo.skin_Region1_level6);

        ClearSkin(skinsInfo.skin_Region2_level1);
        ClearSkin(skinsInfo.skin_Region2_level2);
        ClearSkin(skinsInfo.skin_Region2_level3);
        ClearSkin(skinsInfo.skin_Region2_level4);
        ClearSkin(skinsInfo.skin_Region2_level5);
        ClearSkin(skinsInfo.skin_Region2_level6);

        ClearSkin(skinsInfo.skin_Region3_level1);
        ClearSkin(skinsInfo.skin_Region3_level2);
        ClearSkin(skinsInfo.skin_Region3_level3);
        ClearSkin(skinsInfo.skin_Region3_level4);
        ClearSkin(skinsInfo.skin_Region3_level5);
        ClearSkin(skinsInfo.skin_Region3_level6);

        ClearSkin(skinsInfo.skin_Region4_level1);
        ClearSkin(skinsInfo.skin_Region4_level2);
        ClearSkin(skinsInfo.skin_Region4_level3);
        ClearSkin(skinsInfo.skin_Region4_level4);
        ClearSkin(skinsInfo.skin_Region4_level5);
        ClearSkin(skinsInfo.skin_Region4_level6);

        ClearSkin(skinsInfo.skin_Region5_level1);
        ClearSkin(skinsInfo.skin_Region5_level2);
        ClearSkin(skinsInfo.skin_Region5_level3);
        ClearSkin(skinsInfo.skin_Region5_level4);
        ClearSkin(skinsInfo.skin_Region5_level5);
        ClearSkin(skinsInfo.skin_Region5_level6);

        ClearSkin(skinsInfo.skin_Region6_level1);
        ClearSkin(skinsInfo.skin_Region6_level2);
        ClearSkin(skinsInfo.skin_Region6_level3);
        ClearSkin(skinsInfo.skin_Region6_level4);
        ClearSkin(skinsInfo.skin_Region6_level5);
        ClearSkin(skinsInfo.skin_Region6_level6);

        SaveData();
    }
    void ClearSkin(SkinInfo skinInfo)
    {
        skinInfo.skin_isInactive = true;
        skinInfo.skin_isAquired = false;
        skinInfo.skin_isUnlocked = false;

        skinShopButton_Region1_Level1.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region1_Level2.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region1_Level3.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region1_Level4.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region1_Level5.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region1_Level6.GetComponent<SkinShopButtons>().ClearButtonInfo();

        skinShopButton_Region2_Level1.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region2_Level2.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region2_Level3.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region2_Level4.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region2_Level5.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region2_Level6.GetComponent<SkinShopButtons>().ClearButtonInfo();

        skinShopButton_Region3_Level1.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region3_Level2.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region3_Level3.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region3_Level4.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region3_Level5.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region3_Level6.GetComponent<SkinShopButtons>().ClearButtonInfo();

        skinShopButton_Region4_Level1.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region4_Level2.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region4_Level3.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region4_Level4.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region4_Level5.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region4_Level6.GetComponent<SkinShopButtons>().ClearButtonInfo();

        skinShopButton_Region5_Level1.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region5_Level2.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region5_Level3.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region5_Level4.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region5_Level5.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region5_Level6.GetComponent<SkinShopButtons>().ClearButtonInfo();

        skinShopButton_Region6_Level1.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region6_Level2.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region6_Level3.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region6_Level4.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region6_Level5.GetComponent<SkinShopButtons>().ClearButtonInfo();
        skinShopButton_Region6_Level6.GetComponent<SkinShopButtons>().ClearButtonInfo();
    }
}

[Serializable]
public class SkinsShopInfo
{
    [Header("Region 1")]
    public SkinInfo skin_Region1_level1;
    public SkinInfo skin_Region1_level2;
    public SkinInfo skin_Region1_level3;
    public SkinInfo skin_Region1_level4;
    public SkinInfo skin_Region1_level5;
    public SkinInfo skin_Region1_level6;

    [Header("Region 2")]
    public SkinInfo skin_Region2_level1;
    public SkinInfo skin_Region2_level2;
    public SkinInfo skin_Region2_level3;
    public SkinInfo skin_Region2_level4;
    public SkinInfo skin_Region2_level5;
    public SkinInfo skin_Region2_level6;

    [Header("Region 3")]
    public SkinInfo skin_Region3_level1;
    public SkinInfo skin_Region3_level2;
    public SkinInfo skin_Region3_level3;
    public SkinInfo skin_Region3_level4;
    public SkinInfo skin_Region3_level5;
    public SkinInfo skin_Region3_level6;

    [Header("Region 4")]
    public SkinInfo skin_Region4_level1;
    public SkinInfo skin_Region4_level2;
    public SkinInfo skin_Region4_level3;
    public SkinInfo skin_Region4_level4;
    public SkinInfo skin_Region4_level5;
    public SkinInfo skin_Region4_level6;

    [Header("Region 5")]
    public SkinInfo skin_Region5_level1;
    public SkinInfo skin_Region5_level2;
    public SkinInfo skin_Region5_level3;
    public SkinInfo skin_Region5_level4;
    public SkinInfo skin_Region5_level5;
    public SkinInfo skin_Region5_level6;

    [Header("Region 6")]
    public SkinInfo skin_Region6_level1;
    public SkinInfo skin_Region6_level2;
    public SkinInfo skin_Region6_level3;
    public SkinInfo skin_Region6_level4;
    public SkinInfo skin_Region6_level5;
    public SkinInfo skin_Region6_level6;
}

[Serializable]
public class SkinInfo
{
    [Header("Skin Type")]
    public SkinType skin_Type;

    [Header("States")]
    public bool skin_isInactive;
    public bool skin_isAquired;
    public bool skin_isUnlocked;
}