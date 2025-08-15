using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SkinShopManager : Singleton<SkinShopManager>
{
    [Header("Skin Cost")]
    public TextMeshProUGUI skinShopCostText;
    public List<int> skinCostList = new List<int>();

    [Header("Colors")]
    public Color inactive_Color;
    public Color aquired_Color;
    public Color unlocked_Color;

    [Header("ButtonSnap")]
    public GameObject lastButtonSelected_Up;
    public GameObject lastButtonSelected_Down;
    public GameObject headerButtonSnap;
    public GameObject backButtonSnap;

    #region Shop Buttons
    [Header("Shop - Buttons")]
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
    #endregion


    //--------------------


    private void OnEnable()
    {
        UpdateSkinInfo();
        UpdateSkinButtonDisplay();

        SetSkinCostDisplay();

        SkinsManager.Instance.UpdateAquiredSkins();
    }


    //--------------------


    void UpdateSkinInfo()
    {
        SkinsManager.Instance.skinShopInfo.skin_Region1_level1.skin_Type = skinShopButton_Region1_Level1.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region1_level2.skin_Type = skinShopButton_Region1_Level2.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region1_level3.skin_Type = skinShopButton_Region1_Level3.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region1_level4.skin_Type = skinShopButton_Region1_Level4.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region1_level5.skin_Type = skinShopButton_Region1_Level5.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region1_level6.skin_Type = skinShopButton_Region1_Level6.GetComponent<SkinShopButtons>().skinType;

        SkinsManager.Instance.skinShopInfo.skin_Region2_level1.skin_Type = skinShopButton_Region2_Level1.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region2_level2.skin_Type = skinShopButton_Region2_Level2.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region2_level3.skin_Type = skinShopButton_Region2_Level3.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region2_level4.skin_Type = skinShopButton_Region2_Level4.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region2_level5.skin_Type = skinShopButton_Region2_Level5.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region2_level6.skin_Type = skinShopButton_Region2_Level6.GetComponent<SkinShopButtons>().skinType;

        SkinsManager.Instance.skinShopInfo.skin_Region3_level1.skin_Type = skinShopButton_Region3_Level1.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region3_level2.skin_Type = skinShopButton_Region3_Level2.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region3_level3.skin_Type = skinShopButton_Region3_Level3.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region3_level4.skin_Type = skinShopButton_Region3_Level4.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region3_level5.skin_Type = skinShopButton_Region3_Level5.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region3_level6.skin_Type = skinShopButton_Region3_Level6.GetComponent<SkinShopButtons>().skinType;

        SkinsManager.Instance.skinShopInfo.skin_Region4_level1.skin_Type = skinShopButton_Region4_Level1.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region4_level2.skin_Type = skinShopButton_Region4_Level2.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region4_level3.skin_Type = skinShopButton_Region4_Level3.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region4_level4.skin_Type = skinShopButton_Region4_Level4.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region4_level5.skin_Type = skinShopButton_Region4_Level5.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region4_level6.skin_Type = skinShopButton_Region4_Level6.GetComponent<SkinShopButtons>().skinType;

        SkinsManager.Instance.skinShopInfo.skin_Region5_level1.skin_Type = skinShopButton_Region5_Level1.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region5_level2.skin_Type = skinShopButton_Region5_Level2.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region5_level3.skin_Type = skinShopButton_Region5_Level3.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region5_level4.skin_Type = skinShopButton_Region5_Level4.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region5_level5.skin_Type = skinShopButton_Region5_Level5.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region5_level6.skin_Type = skinShopButton_Region5_Level6.GetComponent<SkinShopButtons>().skinType;

        SkinsManager.Instance.skinShopInfo.skin_Region6_level1.skin_Type = skinShopButton_Region6_Level1.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region6_level2.skin_Type = skinShopButton_Region6_Level2.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region6_level3.skin_Type = skinShopButton_Region6_Level3.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region6_level4.skin_Type = skinShopButton_Region6_Level4.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region6_level5.skin_Type = skinShopButton_Region6_Level5.GetComponent<SkinShopButtons>().skinType;
        SkinsManager.Instance.skinShopInfo.skin_Region6_level6.skin_Type = skinShopButton_Region6_Level6.GetComponent<SkinShopButtons>().skinType;
    }
    void UpdateSkinButtonDisplay()
    {
        skinShopButton_Region1_Level1.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region1_Level2.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region1_Level3.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region1_Level4.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region1_Level5.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region1_Level6.GetComponent<SkinShopButtons>().UpdateSkinDisplay();

        skinShopButton_Region2_Level1.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region2_Level2.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region2_Level3.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region2_Level4.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region2_Level5.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region2_Level6.GetComponent<SkinShopButtons>().UpdateSkinDisplay();

        skinShopButton_Region3_Level1.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region3_Level2.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region3_Level3.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region3_Level4.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region3_Level5.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region3_Level6.GetComponent<SkinShopButtons>().UpdateSkinDisplay();

        skinShopButton_Region4_Level1.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region4_Level2.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region4_Level3.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region4_Level4.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region4_Level5.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region4_Level6.GetComponent<SkinShopButtons>().UpdateSkinDisplay();

        skinShopButton_Region5_Level1.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region5_Level2.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region5_Level3.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region5_Level4.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region5_Level5.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region5_Level6.GetComponent<SkinShopButtons>().UpdateSkinDisplay();

        skinShopButton_Region6_Level1.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region6_Level2.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region6_Level3.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region6_Level4.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region6_Level5.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
        skinShopButton_Region6_Level6.GetComponent<SkinShopButtons>().UpdateSkinDisplay();
    }


    //--------------------


    public int GetSkinCost()
    {
        return SkinsManager.Instance.skinShopInfo.currentSkinCost;
    }
    public int GetEssenceAquired()
    {
        int tempEssenceAquired = 0;

        for (int i = 0; i < DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Count; i++)
        {
            for (int j = 0; j < DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].essenceList.Count; j++)
            {
                if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].essenceList[j].isTaken)
                {
                    tempEssenceAquired++;
                }
            }
        }

        return tempEssenceAquired;
    }
    public int GetEssenceUsed()
    {
        int essenceUsedCounter = 0;

        if (SkinsManager.Instance.skinShopInfo.skin_Region1_level1.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region1_level2.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region1_level3.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region1_level4.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region1_level5.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region1_level6.skin_isUnlocked)
            essenceUsedCounter++;

        if (SkinsManager.Instance.skinShopInfo.skin_Region2_level1.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region2_level2.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region2_level3.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region2_level4.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region2_level5.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region2_level6.skin_isUnlocked)
            essenceUsedCounter++;

        if (SkinsManager.Instance.skinShopInfo.skin_Region3_level1.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region3_level2.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region3_level3.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region3_level4.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region3_level5.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region3_level6.skin_isUnlocked)
            essenceUsedCounter++;

        if (SkinsManager.Instance.skinShopInfo.skin_Region4_level1.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region4_level2.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region4_level3.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region4_level4.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region4_level5.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region4_level6.skin_isUnlocked)
            essenceUsedCounter++;

        if (SkinsManager.Instance.skinShopInfo.skin_Region5_level1.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region5_level2.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region5_level3.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region5_level4.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region5_level5.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region2_level6.skin_isUnlocked)
            essenceUsedCounter++;

        if (SkinsManager.Instance.skinShopInfo.skin_Region6_level1.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region6_level2.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region6_level3.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region6_level4.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region6_level5.skin_isUnlocked)
            essenceUsedCounter++;
        if (SkinsManager.Instance.skinShopInfo.skin_Region6_level6.skin_isUnlocked)
            essenceUsedCounter++;

        return essenceUsedCounter;
    }


    //--------------------


    public void ChangeSkinCost()
    {
        //If the first time SkinCost is set
        if (SkinsManager.Instance.skinShopInfo.currentSkinCost <= 0 && skinCostList.Count > 0)
        {
            SkinsManager.Instance.skinShopInfo.currentSkinCost = skinCostList[0];
            SetSkinCostDisplay();

            return;
        }

        //Check for the next SkinCost to be displayed
        if (skinCostList.Count > 1)
        {
            for (int i = 0; i < skinCostList.Count - 1; i++)
            {
                if (SkinsManager.Instance.skinShopInfo.currentSkinCost == skinCostList[i])
                {
                    SkinsManager.Instance.skinShopInfo.currentSkinCost = skinCostList[i + 1];
                    SetSkinCostDisplay();
                    break;
                }
            }
        }
    }
    void SetSkinCostDisplay()
    {
        skinShopCostText.text = (GetEssenceAquired() - GetEssenceUsed()).ToString() + " / " + GetSkinCost().ToString();
    }


    //--------------------


    public void UpdateSnapHeader(GameObject headerButtonReference)
    {
        Navigation nav = headerButtonSnap.GetComponent<UnityEngine.UI.Button>().navigation;
        nav.selectOnDown = headerButtonReference.GetComponent<UnityEngine.UI.Button>();

        headerButtonSnap.GetComponent<UnityEngine.UI.Button>().navigation = nav;

        lastButtonSelected_Up = headerButtonReference;
    }
    public void UpdateSnapBack(GameObject backButtonReference)
    {
        Navigation nav = backButtonSnap.GetComponent<UnityEngine.UI.Button>().navigation;
        nav.selectOnUp = backButtonReference.GetComponent<UnityEngine.UI.Button>();

        backButtonSnap.GetComponent<UnityEngine.UI.Button>().navigation = nav;

        lastButtonSelected_Down = backButtonReference;
    }
}

[Serializable]
public class SkinsShopInfo
{
    [Header("Skin Cost")]
    public int currentSkinCost;


    //-----


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