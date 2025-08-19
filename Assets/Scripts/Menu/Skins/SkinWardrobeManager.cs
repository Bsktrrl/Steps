using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinWardrobeManager : Singleton<SkinWardrobeManager>
{
    [Header("Colors")]
    public Color inactive_Color;
    public Color bought_Color;
    public Color active_Color;

    [Header("ButtonSnap - Wardrobe")]
    public GameObject lastButtonSelected_Up_Wardrobe;
    public GameObject lastButtonSelected_Down_Wardrobe;
    public GameObject headerButtonSnap_Wardrobe;
    public GameObject backButtonSnap_Wardrobe;

    #region ¨Wardrobe Buttons
    [Header("Wardrobe - Buttons")]
    public GameObject skinWardrobeButton_Region1_Level1;
    public GameObject skinWardrobeButton_Region1_Level2;
    public GameObject skinWardrobeButton_Region1_Level3;
    public GameObject skinWardrobeButton_Region1_Level4;
    public GameObject skinWardrobeButton_Region1_Level5;
    public GameObject skinWardrobeButton_Region1_Level6;

    public GameObject skinWardrobeButton_Region2_Level1;
    public GameObject skinWardrobeButton_Region2_Level2;
    public GameObject skinWardrobeButton_Region2_Level3;
    public GameObject skinWardrobeButton_Region2_Level4;
    public GameObject skinWardrobeButton_Region2_Level5;
    public GameObject skinWardrobeButton_Region2_Level6;

    public GameObject skinWardrobeButton_Region3_Level1;
    public GameObject skinWardrobeButton_Region3_Level2;
    public GameObject skinWardrobeButton_Region3_Level3;
    public GameObject skinWardrobeButton_Region3_Level4;
    public GameObject skinWardrobeButton_Region3_Level5;
    public GameObject skinWardrobeButton_Region3_Level6;

    public GameObject skinWardrobeButton_Region4_Level1;
    public GameObject skinWardrobeButton_Region4_Level2;
    public GameObject skinWardrobeButton_Region4_Level3;
    public GameObject skinWardrobeButton_Region4_Level4;
    public GameObject skinWardrobeButton_Region4_Level5;
    public GameObject skinWardrobeButton_Region4_Level6;

    public GameObject skinWardrobeButton_Region5_Level1;
    public GameObject skinWardrobeButton_Region5_Level2;
    public GameObject skinWardrobeButton_Region5_Level3;
    public GameObject skinWardrobeButton_Region5_Level4;
    public GameObject skinWardrobeButton_Region5_Level5;
    public GameObject skinWardrobeButton_Region5_Level6;

    public GameObject skinWardrobeButton_Region6_Level1;
    public GameObject skinWardrobeButton_Region6_Level2;
    public GameObject skinWardrobeButton_Region6_Level3;
    public GameObject skinWardrobeButton_Region6_Level4;
    public GameObject skinWardrobeButton_Region6_Level5;
    public GameObject skinWardrobeButton_Region6_Level6;
    #endregion


    //--------------------


    private void Start()
    {
        DataManager.Instance.playerStats_Store.itemsGot.essence = 12; //Remove this after testing of Skin Menu
    }
    private void OnEnable()
    {
        UpdateButtonStates();
    }


    //--------------------


    public void UpdateButtonStates()
    {
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region1_level1, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region1_level1, skinWardrobeButton_Region1_Level1);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region1_level2, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region1_level2, skinWardrobeButton_Region1_Level2);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region1_level3, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region1_level3, skinWardrobeButton_Region1_Level3);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region1_level4, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region1_level4, skinWardrobeButton_Region1_Level4);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region1_level5, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region1_level5, skinWardrobeButton_Region1_Level5);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region1_level6, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region1_level6, skinWardrobeButton_Region1_Level6);

        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region2_level1, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region2_level1, skinWardrobeButton_Region2_Level1);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region2_level2, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region2_level2, skinWardrobeButton_Region2_Level2);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region2_level3, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region2_level3, skinWardrobeButton_Region2_Level3);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region2_level4, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region2_level4, skinWardrobeButton_Region2_Level4);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region2_level5, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region2_level5, skinWardrobeButton_Region2_Level5);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region2_level6, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region2_level6, skinWardrobeButton_Region2_Level6);

        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region3_level1, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region3_level1, skinWardrobeButton_Region3_Level1);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region3_level2, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region3_level2, skinWardrobeButton_Region3_Level2);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region3_level3, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region3_level3, skinWardrobeButton_Region3_Level3);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region3_level4, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region3_level4, skinWardrobeButton_Region3_Level4);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region3_level5, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region3_level5, skinWardrobeButton_Region3_Level5);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region3_level6, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region3_level6, skinWardrobeButton_Region3_Level6);

        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region4_level1, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region4_level1, skinWardrobeButton_Region4_Level1);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region4_level2, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region4_level2, skinWardrobeButton_Region4_Level2);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region4_level3, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region4_level3, skinWardrobeButton_Region4_Level3);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region4_level4, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region4_level4, skinWardrobeButton_Region4_Level4);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region4_level5, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region4_level5, skinWardrobeButton_Region4_Level5);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region4_level6, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region4_level6, skinWardrobeButton_Region4_Level6);

        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region5_level1, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region5_level1, skinWardrobeButton_Region5_Level1);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region5_level2, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region5_level2, skinWardrobeButton_Region5_Level2);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region5_level3, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region5_level3, skinWardrobeButton_Region5_Level3);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region5_level4, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region5_level4, skinWardrobeButton_Region5_Level4);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region5_level5, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region5_level5, skinWardrobeButton_Region5_Level5);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region5_level6, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region5_level6, skinWardrobeButton_Region5_Level6);

        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region6_level1, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region6_level1, skinWardrobeButton_Region6_Level1);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region6_level2, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region6_level2, skinWardrobeButton_Region6_Level2);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region6_level3, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region6_level3, skinWardrobeButton_Region6_Level3);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region6_level4, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region6_level4, skinWardrobeButton_Region6_Level4);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region6_level5, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region6_level5, skinWardrobeButton_Region6_Level5);
        ChangeWardrobeButtonStates(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region6_level6, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region6_level6, skinWardrobeButton_Region6_Level6);

        SkinsManager.Instance.SaveData();
    }
    void ChangeWardrobeButtonStates(SkinShopObject skinShopObject, SkinWardrobeObject skinWardrobeObject, GameObject wardrobeButtonObject)
    {
        if (skinShopObject.skin_isBought)
        {
            skinWardrobeObject.skin_isInactive = false;
            skinWardrobeObject.skin_isBought = true;

            wardrobeButtonObject.GetComponent<SkinWardrobeButton>().isInactive = false;
            wardrobeButtonObject.GetComponent<SkinWardrobeButton>().isBought = true;
        }
        else if(skinShopObject.skin_isInactive || skinShopObject.skin_isAvailable)
        {
            skinWardrobeObject.skin_isInactive = true;
            skinWardrobeObject.skin_isBought = false;

            wardrobeButtonObject.GetComponent<SkinWardrobeButton>().isInactive = true;
            wardrobeButtonObject.GetComponent<SkinWardrobeButton>().isBought = false;
        }
        
        wardrobeButtonObject.GetComponent<SkinWardrobeButton>().UpdateButtonDisplay();
    }


    //--------------------


    public void UpdateSnapHeader(GameObject headerButtonReference)
    {
        Navigation nav = headerButtonSnap_Wardrobe.GetComponent<UnityEngine.UI.Button>().navigation;
        nav.selectOnDown = headerButtonReference.GetComponent<UnityEngine.UI.Button>();

        headerButtonSnap_Wardrobe.GetComponent<UnityEngine.UI.Button>().navigation = nav;

        lastButtonSelected_Up_Wardrobe = headerButtonReference;
    }
    public void UpdateSnapBack(GameObject backButtonReference)
    {
        Navigation nav = backButtonSnap_Wardrobe.GetComponent<UnityEngine.UI.Button>().navigation;
        nav.selectOnUp = backButtonReference.GetComponent<UnityEngine.UI.Button>();

        backButtonSnap_Wardrobe.GetComponent<UnityEngine.UI.Button>().navigation = nav;

        lastButtonSelected_Down_Wardrobe = backButtonReference;
    }
}

[Serializable]
public class SkinsWardrobeInfo
{
    [Header("Region 1")]
    public SkinWardrobeObject skin_Region1_level1;
    public SkinWardrobeObject skin_Region1_level2;
    public SkinWardrobeObject skin_Region1_level3;
    public SkinWardrobeObject skin_Region1_level4;
    public SkinWardrobeObject skin_Region1_level5;
    public SkinWardrobeObject skin_Region1_level6;

    public SkinHatObject skin_Region1_Hat;

    [Header("Region 2")]
    public SkinWardrobeObject skin_Region2_level1;
    public SkinWardrobeObject skin_Region2_level2;
    public SkinWardrobeObject skin_Region2_level3;
    public SkinWardrobeObject skin_Region2_level4;
    public SkinWardrobeObject skin_Region2_level5;
    public SkinWardrobeObject skin_Region2_level6;

    public SkinHatObject skin_Region2_Hat;

    [Header("Region 3")]
    public SkinWardrobeObject skin_Region3_level1;
    public SkinWardrobeObject skin_Region3_level2;
    public SkinWardrobeObject skin_Region3_level3;
    public SkinWardrobeObject skin_Region3_level4;
    public SkinWardrobeObject skin_Region3_level5;
    public SkinWardrobeObject skin_Region3_level6;

    public SkinHatObject skin_Region3_Hat;

    [Header("Region 4")]
    public SkinWardrobeObject skin_Region4_level1;
    public SkinWardrobeObject skin_Region4_level2;
    public SkinWardrobeObject skin_Region4_level3;
    public SkinWardrobeObject skin_Region4_level4;
    public SkinWardrobeObject skin_Region4_level5;
    public SkinWardrobeObject skin_Region4_level6;

    public SkinHatObject skin_Region4_Hat;

    [Header("Region 5")]
    public SkinWardrobeObject skin_Region5_level1;
    public SkinWardrobeObject skin_Region5_level2;
    public SkinWardrobeObject skin_Region5_level3;
    public SkinWardrobeObject skin_Region5_level4;
    public SkinWardrobeObject skin_Region5_level5;
    public SkinWardrobeObject skin_Region5_level6;

    public SkinHatObject skin_Region5_Hat;

    [Header("Region 6")]
    public SkinWardrobeObject skin_Region6_level1;
    public SkinWardrobeObject skin_Region6_level2;
    public SkinWardrobeObject skin_Region6_level3;
    public SkinWardrobeObject skin_Region6_level4;
    public SkinWardrobeObject skin_Region6_level5;
    public SkinWardrobeObject skin_Region6_level6;

    public SkinHatObject skin_Region6_Hat;

    [Header("Default")]
    public SkinWardrobeObject skin_Default;
}

[Serializable]
public class SkinWardrobeObject
{
    [Header("Skin Type")]
    public SkinType skin_Type;

    [Header("States")]
    public bool skin_isInactive;
    public bool skin_isBought;
    public bool skin_isSelected;

}
[Serializable]
public class SkinHatObject
{
    [Header("Skin Type")]
    public HatType hat_Type;

    [Header("States")]
    public bool skin_isInactive;
    public bool skin_isAquired;
    public bool skin_isSelected;
}
