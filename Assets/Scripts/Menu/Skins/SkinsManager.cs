using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SkinsManager : Singleton<SkinsManager>
{
    [Header("Shop")]
    [SerializeField] SkinWardrobeManager SkinWardrobeCostManager;
    [SerializeField] SkinShopManager SkinShopCostManager;

    public SkinInfo skinInfo;

    [SerializeField] PauseMenuManager pauseMenuManager;

    //--------------------


    private void Update()
    {
        if (pauseMenuManager && SkinWardrobeCostManager)
        {
            if (SkinWardrobeCostManager.GetComponent<SkinWardrobeManager>().wardrobeParent.activeInHierarchy)
            {
                PauseMenuManager.Instance.levelDisplay_Parent.SetActive(false);
            }
            else
            {
                PauseMenuManager.Instance.levelDisplay_Parent.SetActive(true);
            }
        }
    }


    //--------------------


    private void OnEnable()
    {
        DataPersistanceManager.Action_NewGame += ClearSkinsWardrobeInfo;
        DataPersistanceManager.Action_NewGame += ClearSkinsShopInfo;
    }
    private void OnDisable()
    {
        DataPersistanceManager.Action_NewGame -= ClearSkinsWardrobeInfo;
        DataPersistanceManager.Action_NewGame -= ClearSkinsShopInfo;
    }


    //--------------------


    public void LoadData()
    {
        skinInfo = DataManager.Instance.skinsInfo_Store;
    }
    public void SaveData()
    {
        DataManager.Instance.skinsInfo_Store = skinInfo;

        DataPersistanceManager.instance.SaveGame();
    }


    //--------------------


    #region Wardrobe

    public void UpdateBoughtWardrobeSkins()
    {
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region1_level1, skinInfo.skinShopInfo.skin_Region1_level1);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region1_level2, skinInfo.skinShopInfo.skin_Region1_level2);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region1_level3, skinInfo.skinShopInfo.skin_Region1_level3);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region1_level4, skinInfo.skinShopInfo.skin_Region1_level4);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region1_level5, skinInfo.skinShopInfo.skin_Region1_level5);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region1_level6, skinInfo.skinShopInfo.skin_Region1_level6);

        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region2_level1, skinInfo.skinShopInfo.skin_Region2_level1);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region2_level2, skinInfo.skinShopInfo.skin_Region2_level2);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region2_level3, skinInfo.skinShopInfo.skin_Region2_level3);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region2_level4, skinInfo.skinShopInfo.skin_Region2_level4);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region2_level5, skinInfo.skinShopInfo.skin_Region2_level5);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region2_level6, skinInfo.skinShopInfo.skin_Region2_level6);

        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region3_level1, skinInfo.skinShopInfo.skin_Region3_level1);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region3_level2, skinInfo.skinShopInfo.skin_Region3_level2);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region3_level3, skinInfo.skinShopInfo.skin_Region3_level3);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region3_level4, skinInfo.skinShopInfo.skin_Region3_level4);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region3_level5, skinInfo.skinShopInfo.skin_Region3_level5);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region3_level6, skinInfo.skinShopInfo.skin_Region3_level6);

        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region4_level1, skinInfo.skinShopInfo.skin_Region4_level1);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region4_level2, skinInfo.skinShopInfo.skin_Region4_level2);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region4_level3, skinInfo.skinShopInfo.skin_Region4_level3);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region4_level4, skinInfo.skinShopInfo.skin_Region4_level4);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region4_level5, skinInfo.skinShopInfo.skin_Region4_level5);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region4_level6, skinInfo.skinShopInfo.skin_Region4_level6);

        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region5_level1, skinInfo.skinShopInfo.skin_Region5_level1);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region5_level2, skinInfo.skinShopInfo.skin_Region5_level2);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region5_level3, skinInfo.skinShopInfo.skin_Region5_level3);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region5_level4, skinInfo.skinShopInfo.skin_Region5_level4);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region5_level5, skinInfo.skinShopInfo.skin_Region5_level5);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region5_level6, skinInfo.skinShopInfo.skin_Region5_level6);

        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region6_level1, skinInfo.skinShopInfo.skin_Region6_level1);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region6_level2, skinInfo.skinShopInfo.skin_Region6_level2);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region6_level3, skinInfo.skinShopInfo.skin_Region6_level3);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region6_level4, skinInfo.skinShopInfo.skin_Region6_level4);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region6_level5, skinInfo.skinShopInfo.skin_Region6_level5);
        ChangeWardrobeSkinBoughtInfo(skinInfo.skinWardrobeInfo.skin_Region6_level6, skinInfo.skinShopInfo.skin_Region6_level6);

        SaveData();
    }
    void ChangeWardrobeSkinBoughtInfo(SkinWardrobeObject skinWardrobeInfo, SkinShopObject skinShopInfo)
    {
        if (skinShopInfo.skin_isBought)
        {
            skinWardrobeInfo.skin_isInactive = false;
            skinWardrobeInfo.skin_isBought = true;
        }
        else 
        {
            skinWardrobeInfo.skin_isInactive = true;
            skinWardrobeInfo.skin_isBought = false;
            skinWardrobeInfo.skin_isSelected = false;
        }
    }


    //-----


    void ClearSkinsWardrobeInfo()
    {
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region1_level1, SkinWardrobeCostManager.skinWardrobeButton_Region1_Level1.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region1_level2, SkinWardrobeCostManager.skinWardrobeButton_Region1_Level2.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region1_level3, SkinWardrobeCostManager.skinWardrobeButton_Region1_Level3.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region1_level4, SkinWardrobeCostManager.skinWardrobeButton_Region1_Level4.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region1_level5, SkinWardrobeCostManager.skinWardrobeButton_Region1_Level5.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region1_level6, SkinWardrobeCostManager.skinWardrobeButton_Region1_Level6.GetComponent<SkinWardrobeButton>());

        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region2_level1, SkinWardrobeCostManager.skinWardrobeButton_Region2_Level1.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region2_level2, SkinWardrobeCostManager.skinWardrobeButton_Region2_Level2.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region2_level3, SkinWardrobeCostManager.skinWardrobeButton_Region2_Level3.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region2_level4, SkinWardrobeCostManager.skinWardrobeButton_Region2_Level4.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region2_level5, SkinWardrobeCostManager.skinWardrobeButton_Region2_Level5.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region2_level6, SkinWardrobeCostManager.skinWardrobeButton_Region2_Level6.GetComponent<SkinWardrobeButton>());

        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region3_level1, SkinWardrobeCostManager.skinWardrobeButton_Region3_Level1.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region3_level2, SkinWardrobeCostManager.skinWardrobeButton_Region3_Level2.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region3_level3, SkinWardrobeCostManager.skinWardrobeButton_Region3_Level3.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region3_level4, SkinWardrobeCostManager.skinWardrobeButton_Region3_Level4.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region3_level5, SkinWardrobeCostManager.skinWardrobeButton_Region3_Level5.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region3_level6, SkinWardrobeCostManager.skinWardrobeButton_Region3_Level6.GetComponent<SkinWardrobeButton>());

        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region4_level1, SkinWardrobeCostManager.skinWardrobeButton_Region4_Level1.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region4_level2, SkinWardrobeCostManager.skinWardrobeButton_Region4_Level2.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region4_level3, SkinWardrobeCostManager.skinWardrobeButton_Region4_Level3.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region4_level4, SkinWardrobeCostManager.skinWardrobeButton_Region4_Level4.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region4_level5, SkinWardrobeCostManager.skinWardrobeButton_Region4_Level5.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region4_level6, SkinWardrobeCostManager.skinWardrobeButton_Region4_Level6.GetComponent<SkinWardrobeButton>());

        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region5_level1, SkinWardrobeCostManager.skinWardrobeButton_Region5_Level1.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region5_level2, SkinWardrobeCostManager.skinWardrobeButton_Region5_Level2.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region5_level3, SkinWardrobeCostManager.skinWardrobeButton_Region5_Level3.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region5_level4, SkinWardrobeCostManager.skinWardrobeButton_Region5_Level4.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region5_level5, SkinWardrobeCostManager.skinWardrobeButton_Region5_Level5.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region5_level6, SkinWardrobeCostManager.skinWardrobeButton_Region5_Level6.GetComponent<SkinWardrobeButton>());

        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region6_level1, SkinWardrobeCostManager.skinWardrobeButton_Region6_Level1.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region6_level2, SkinWardrobeCostManager.skinWardrobeButton_Region6_Level2.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region6_level3, SkinWardrobeCostManager.skinWardrobeButton_Region6_Level3.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region6_level4, SkinWardrobeCostManager.skinWardrobeButton_Region6_Level4.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region6_level5, SkinWardrobeCostManager.skinWardrobeButton_Region6_Level5.GetComponent<SkinWardrobeButton>());
        ClearSkinWardrobe(skinInfo.skinWardrobeInfo.skin_Region6_level6, SkinWardrobeCostManager.skinWardrobeButton_Region6_Level6.GetComponent<SkinWardrobeButton>());

        SaveData();
    }
    void ClearSkinWardrobe(SkinWardrobeObject skinWardrobeInfo, SkinWardrobeButton skinWardrobeButton)
    {
        skinWardrobeInfo.skin_isInactive = true;
        skinWardrobeInfo.skin_isBought = false;
        skinWardrobeInfo.skin_isSelected = false;

        skinWardrobeButton.ClearWardropbeButtonInfo();
    }

    #endregion

    #region Shop

    public void UpdateAvailableShopSkins()
    {
        for (int i = 0; i < DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Count; i++)
        {
            if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Count <= 0) { return; }

            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region1_level1)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region1_level2)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region1_level3)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region1_level4)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region1_level5)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region1_level6)) { continue; }

            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region2_level1)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region2_level2)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region2_level3)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region2_level4)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region2_level5)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region2_level6)) { continue; }

            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region3_level1)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region3_level2)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region3_level3)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region3_level4)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region3_level5)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region3_level6)) { continue; }

            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region4_level1)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region4_level2)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region4_level3)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region4_level4)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region4_level5)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region4_level6)) { continue; }

            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region5_level1)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region5_level2)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region5_level3)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region5_level4)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region5_level5)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region5_level6)) { continue; }

            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region6_level1)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region6_level2)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region6_level3)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region6_level4)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region6_level5)) { continue; }
            if (SearchShopSkinAvailableInfo(i, skinInfo.skinShopInfo.skin_Region6_level6)) { continue; }
        }

        SaveData();
    }
    bool SearchShopSkinAvailableInfo(int index, SkinShopObject skinInfo)
    {
        if (skinInfo.skin_Type == DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[index].skintype)
        {
            if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[index].levelSkin.isTaken)
            {
                skinInfo.skin_isInactive = false;
                skinInfo.skin_isAvailable = true;

                return true;
            }
        }

        skinInfo.skin_isInactive = true;
        skinInfo.skin_isAvailable = false;

        return false;
    }


    //-----


    void ClearSkinsShopInfo()
    {
        skinInfo.activeSkinType = SkinType.None;

        ClearSkinShop(skinInfo.skinShopInfo.skin_Region1_level1, SkinShopCostManager.skinShopButton_Region1_Level1.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region1_level2, SkinShopCostManager.skinShopButton_Region1_Level2.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region1_level3, SkinShopCostManager.skinShopButton_Region1_Level3.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region1_level4, SkinShopCostManager.skinShopButton_Region1_Level4.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region1_level5, SkinShopCostManager.skinShopButton_Region1_Level5.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region1_level6, SkinShopCostManager.skinShopButton_Region1_Level6.GetComponent<SkinShopButtons>());

        ClearSkinShop(skinInfo.skinShopInfo.skin_Region2_level1, SkinShopCostManager.skinShopButton_Region2_Level1.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region2_level2, SkinShopCostManager.skinShopButton_Region2_Level2.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region2_level3, SkinShopCostManager.skinShopButton_Region2_Level3.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region2_level4, SkinShopCostManager.skinShopButton_Region2_Level4.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region2_level5, SkinShopCostManager.skinShopButton_Region2_Level5.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region2_level6, SkinShopCostManager.skinShopButton_Region2_Level6.GetComponent<SkinShopButtons>());

        ClearSkinShop(skinInfo.skinShopInfo.skin_Region3_level1, SkinShopCostManager.skinShopButton_Region3_Level1.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region3_level2, SkinShopCostManager.skinShopButton_Region3_Level2.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region3_level3, SkinShopCostManager.skinShopButton_Region3_Level3.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region3_level4, SkinShopCostManager.skinShopButton_Region3_Level4.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region3_level5, SkinShopCostManager.skinShopButton_Region3_Level5.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region3_level6, SkinShopCostManager.skinShopButton_Region3_Level6.GetComponent<SkinShopButtons>());

        ClearSkinShop(skinInfo.skinShopInfo.skin_Region4_level1, SkinShopCostManager.skinShopButton_Region4_Level1.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region4_level2, SkinShopCostManager.skinShopButton_Region4_Level2.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region4_level3, SkinShopCostManager.skinShopButton_Region4_Level3.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region4_level4, SkinShopCostManager.skinShopButton_Region4_Level4.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region4_level5, SkinShopCostManager.skinShopButton_Region4_Level5.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region4_level6, SkinShopCostManager.skinShopButton_Region4_Level6.GetComponent<SkinShopButtons>());

        ClearSkinShop(skinInfo.skinShopInfo.skin_Region5_level1, SkinShopCostManager.skinShopButton_Region5_Level1.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region5_level2, SkinShopCostManager.skinShopButton_Region5_Level2.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region5_level3, SkinShopCostManager.skinShopButton_Region5_Level3.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region5_level4, SkinShopCostManager.skinShopButton_Region5_Level4.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region5_level5, SkinShopCostManager.skinShopButton_Region5_Level5.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region5_level6, SkinShopCostManager.skinShopButton_Region5_Level6.GetComponent<SkinShopButtons>());

        ClearSkinShop(skinInfo.skinShopInfo.skin_Region6_level1, SkinShopCostManager.skinShopButton_Region6_Level1.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region6_level2, SkinShopCostManager.skinShopButton_Region6_Level2.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region6_level3, SkinShopCostManager.skinShopButton_Region6_Level3.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region6_level4, SkinShopCostManager.skinShopButton_Region6_Level4.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region6_level5, SkinShopCostManager.skinShopButton_Region6_Level5.GetComponent<SkinShopButtons>());
        ClearSkinShop(skinInfo.skinShopInfo.skin_Region6_level6, SkinShopCostManager.skinShopButton_Region6_Level6.GetComponent<SkinShopButtons>());

        SaveData();
    }
    void ClearSkinShop(SkinShopObject skinShopInfo, SkinShopButtons skinShopButtons)
    {
        skinShopInfo.skin_isInactive = true;
        skinShopInfo.skin_isAvailable = false;
        skinShopInfo.skin_isBought = false;

        skinShopButtons.ClearShopButtonInfo();
    }

    #endregion
}

[Serializable]
public class SkinInfo
{
    public SkinType activeSkinType;

    public SkinsWardrobeInfo skinWardrobeInfo;
    public SkinsShopInfo skinShopInfo;
}
