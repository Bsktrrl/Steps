using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkinsManager : Singleton<SkinsManager>
{
    [Header("Shop")]
    [SerializeField] SkinShopManager SkinShopCostManager;

    public SkinsShopInfo skinShopInfo;


    //--------------------


    private void OnEnable()
    {
        DataPersistanceManager.Action_NewGame += ClearSkinsShopInfo;
    }
    private void OnDisable()
    {
        DataPersistanceManager.Action_NewGame -= ClearSkinsShopInfo;
    }


    //--------------------


    public void LoadData()
    {
        skinShopInfo = DataManager.Instance.skinsInfo_Store;
    }
    public void SaveData()
    {
        DataManager.Instance.skinsInfo_Store = skinShopInfo;

        DataPersistanceManager.instance.SaveGame();
    }


    //--------------------


    #region Shop

    public void UpdateAquiredSkins()
    {
        for (int i = 0; i < DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Count; i++)
        {
            if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Count <= 0) { return; }

            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region1_level1);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region1_level2);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region1_level3);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region1_level4);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region1_level5);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region1_level6);

            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region2_level1);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region2_level2);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region2_level3);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region2_level4);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region2_level5);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region2_level6);

            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region3_level1);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region3_level2);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region3_level3);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region3_level4);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region3_level5);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region3_level6);

            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region4_level1);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region4_level2);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region4_level3);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region4_level4);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region4_level5);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region4_level6);

            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region5_level1);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region5_level2);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region5_level3);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region5_level4);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region5_level5);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region5_level6);

            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region6_level1);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region6_level2);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region6_level3);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region6_level4);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region6_level5);
            //SearchSkinAquiredInfo(i, skinShopInfo.skin_Region6_level6);

            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region1_level1)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region1_level2)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region1_level3)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region1_level4)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region1_level5)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region1_level6)) { continue; }

            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region2_level1)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region2_level2)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region2_level3)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region2_level4)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region2_level5)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region2_level6)) { continue; }

            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region3_level1)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region3_level2)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region3_level3)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region3_level4)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region3_level5)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region3_level6)) { continue; }

            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region4_level1)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region4_level2)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region4_level3)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region4_level4)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region4_level5)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region4_level6)) { continue; }

            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region5_level1)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region5_level2)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region5_level3)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region5_level4)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region5_level5)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region5_level6)) { continue; }

            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region6_level1)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region6_level2)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region6_level3)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region6_level4)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region6_level5)) { continue; }
            if (SearchSkinAquiredInfo(i, skinShopInfo.skin_Region6_level6)) { continue; }
        }

        SaveData();
    }
    bool SearchSkinAquiredInfo(int index, SkinInfo skinInfo)
    {
        if (skinInfo.skin_Type == DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[index].skintype)
        {
            if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[index].levelSkin.isTaken)
            {
                skinInfo.skin_isInactive = false;
                skinInfo.skin_isAquired = true;

                return true;
            }
        }

        skinInfo.skin_isInactive = true;
        skinInfo.skin_isAquired = false;

        return false;
    }


    //--------------------


    void ClearSkinsShopInfo()
    {
        if (SkinShopCostManager.skinCostList.Count <= 0)
            skinShopInfo.currentSkinCost = 0;
        else
            skinShopInfo.currentSkinCost = SkinShopCostManager.skinCostList[0];

        ClearSkin(skinShopInfo.skin_Region1_level1);
        ClearSkin(skinShopInfo.skin_Region1_level2);
        ClearSkin(skinShopInfo.skin_Region1_level3);
        ClearSkin(skinShopInfo.skin_Region1_level4);
        ClearSkin(skinShopInfo.skin_Region1_level5);
        ClearSkin(skinShopInfo.skin_Region1_level6);

        ClearSkin(skinShopInfo.skin_Region2_level1);
        ClearSkin(skinShopInfo.skin_Region2_level2);
        ClearSkin(skinShopInfo.skin_Region2_level3);
        ClearSkin(skinShopInfo.skin_Region2_level4);
        ClearSkin(skinShopInfo.skin_Region2_level5);
        ClearSkin(skinShopInfo.skin_Region2_level6);

        ClearSkin(skinShopInfo.skin_Region3_level1);
        ClearSkin(skinShopInfo.skin_Region3_level2);
        ClearSkin(skinShopInfo.skin_Region3_level3);
        ClearSkin(skinShopInfo.skin_Region3_level4);
        ClearSkin(skinShopInfo.skin_Region3_level5);
        ClearSkin(skinShopInfo.skin_Region3_level6);

        ClearSkin(skinShopInfo.skin_Region4_level1);
        ClearSkin(skinShopInfo.skin_Region4_level2);
        ClearSkin(skinShopInfo.skin_Region4_level3);
        ClearSkin(skinShopInfo.skin_Region4_level4);
        ClearSkin(skinShopInfo.skin_Region4_level5);
        ClearSkin(skinShopInfo.skin_Region4_level6);

        ClearSkin(skinShopInfo.skin_Region5_level1);
        ClearSkin(skinShopInfo.skin_Region5_level2);
        ClearSkin(skinShopInfo.skin_Region5_level3);
        ClearSkin(skinShopInfo.skin_Region5_level4);
        ClearSkin(skinShopInfo.skin_Region5_level5);
        ClearSkin(skinShopInfo.skin_Region5_level6);

        ClearSkin(skinShopInfo.skin_Region6_level1);
        ClearSkin(skinShopInfo.skin_Region6_level2);
        ClearSkin(skinShopInfo.skin_Region6_level3);
        ClearSkin(skinShopInfo.skin_Region6_level4);
        ClearSkin(skinShopInfo.skin_Region6_level5);
        ClearSkin(skinShopInfo.skin_Region6_level6);

        SaveData();
    }
    void ClearSkin(SkinInfo skinInfo)
    {
        skinInfo.skin_isInactive = true;
        skinInfo.skin_isAquired = false;
        skinInfo.skin_isUnlocked = false;

        SkinShopCostManager.skinShopButton_Region1_Level1.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region1_Level2.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region1_Level3.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region1_Level4.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region1_Level5.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region1_Level6.GetComponent<SkinShopButtons>().ClearButtonInfo();

        SkinShopCostManager.skinShopButton_Region2_Level1.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region2_Level2.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region2_Level3.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region2_Level4.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region2_Level5.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region2_Level6.GetComponent<SkinShopButtons>().ClearButtonInfo();

        SkinShopCostManager.skinShopButton_Region3_Level1.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region3_Level2.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region3_Level3.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region3_Level4.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region3_Level5.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region3_Level6.GetComponent<SkinShopButtons>().ClearButtonInfo();

        SkinShopCostManager.skinShopButton_Region4_Level1.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region4_Level2.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region4_Level3.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region4_Level4.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region4_Level5.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region4_Level6.GetComponent<SkinShopButtons>().ClearButtonInfo();

        SkinShopCostManager.skinShopButton_Region5_Level1.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region5_Level2.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region5_Level3.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region5_Level4.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region5_Level5.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region5_Level6.GetComponent<SkinShopButtons>().ClearButtonInfo();

        SkinShopCostManager.skinShopButton_Region6_Level1.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region6_Level2.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region6_Level3.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region6_Level4.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region6_Level5.GetComponent<SkinShopButtons>().ClearButtonInfo();
        SkinShopCostManager.skinShopButton_Region6_Level6.GetComponent<SkinShopButtons>().ClearButtonInfo();
    }

    #endregion
}
