using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class SkinShopButtons : MonoBehaviour
{
    public static event Action Action_BoughtSkin;

    [Header("General")]
    public SkinType skinType;
    [SerializeField] int region;
    [SerializeField] int level;

    [Header("Status")]
    [SerializeField] bool isInactive;
    [SerializeField] bool isAvailable;
    [SerializeField] bool isBought;

    [Header("Components")]
    [SerializeField] Image skinImage;
    [SerializeField] Image frame;
    [SerializeField] GameObject inactiveImage;


    //--------------------


    private void OnEnable()
    {
        UpdateSkinDisplay();

        Action_BoughtSkin += UpdateSkinDisplay;
    }
    private void OnDisable()
    {
        Action_BoughtSkin -= UpdateSkinDisplay;
    }


    //--------------------


    public void UpdateSkinDisplay()
    {
        SkinShopObject tempSkinInfo = GetThisSkinShopInfo();

        IfInactive_Display(tempSkinInfo);
        IfAquired_Display(tempSkinInfo);
        IfBought_Display(tempSkinInfo);

        SaveThisSkinShopInfo();
    }

    void IfInactive_Display(SkinShopObject skinInfo)
    {
        if (skinInfo.skin_isInactive)
        {
            isInactive = true;

            skinInfo.skin_isInactive = true;

            frame.color = SkinShopManager.Instance.inactive_Color;
            inactiveImage.SetActive(true);
        }
        else
        {
            isInactive = false;

            skinInfo.skin_isInactive = false;

            inactiveImage.SetActive(false);
        }
    }
    void IfAquired_Display(SkinShopObject skinInfo)
    {
        LevelSkinsInfo skinData = GetSkinData();

        if (skinData != null && skinData.isTaken && isBought)
        {
            frame.color = SkinShopManager.Instance.unlocked_Color;
            isBought = true;

            skinInfo.skin_isBought = true;
        }
        else if (skinData != null && skinData.isTaken && !isBought)
        {
            isAvailable = true;
            isInactive = false;

            skinInfo.skin_isAvailable = true;
            skinInfo.skin_isInactive = false;

            UpdateIfInactive(false);
            inactiveImage.SetActive(false);

            if (skinInfo.skin_isAvailable && (SkinShopManager.Instance.GetSkinCost() <= SkinShopManager.Instance.GetEssenceAquired()))
            {
                frame.color = SkinShopManager.Instance.aquired_Color;
            }
            else
            {
                frame.color = SkinShopManager.Instance.inactive_Color;
            }
        }
        else
        {
            isAvailable = false;
            isInactive = true;

            skinInfo.skin_isAvailable = false;
            skinInfo.skin_isInactive = true;

            UpdateIfInactive(true);

            inactiveImage.SetActive(true);
        }
    }
    void IfBought_Display(SkinShopObject skinInfo)
    {
        if (skinInfo.skin_isBought)
        {
            isBought = true;
            frame.color = SkinShopManager.Instance.unlocked_Color;
        }
        else
        {
            isBought = false;
        }
    }


    //--------------------


    void UpdateIfInactive(bool state)
    {
        isInactive = state;

        if (state)
        {
            frame.color = SkinShopManager.Instance.inactive_Color;
            inactiveImage.SetActive(true);
        }
        else
        {
            inactiveImage.SetActive(false);
        }
    }
    void UpdateIfAquired(bool state)
    {
        isAvailable = state;

        if (state)
        {
            frame.color = SkinShopManager.Instance.aquired_Color;
            inactiveImage.SetActive(false);
        }
        else
        {
            inactiveImage.SetActive(true);
        }
    }
    void UpdateIfUnlocked(bool state)
    {
        isAvailable = !state;
        isBought = state;

        frame.color = SkinShopManager.Instance.unlocked_Color;

        if (state)
            inactiveImage.SetActive(false);

        SkinShopManager.Instance.SetSkinCostDisplay();
    }


    //--------------------


    SkinShopObject GetThisSkinShopInfo()
    {
        switch (region)
        {
            case 1:
                switch (level)
                {
                    case 1:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region1_level1;
                    case 2:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region1_level2;
                    case 3:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region1_level3;
                    case 4:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region1_level4;
                    case 5:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region1_level5;
                    case 6:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region1_level6;

                    default:
                        break;
                }
                break;
            case 2:
                switch (level)
                {
                    case 1:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region2_level1;
                    case 2:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region2_level2;
                    case 3:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region2_level3;
                    case 4:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region2_level4;
                    case 5:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region2_level5;
                    case 6:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region2_level6;

                    default:
                        break;
                }
                break;
            case 3:
                switch (level)
                {
                    case 1:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region3_level1;
                    case 2:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region3_level2;
                    case 3:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region3_level3;
                    case 4:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region3_level4;
                    case 5:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region3_level5;
                    case 6:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region3_level6;

                    default:
                        break;
                }
                break;
            case 4:
                switch (level)
                {
                    case 1:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region4_level1;
                    case 2:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region4_level2;
                    case 3:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region4_level3;
                    case 4:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region4_level4;
                    case 5:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region4_level5;
                    case 6:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region4_level6;

                    default:
                        break;
                }
                break;
            case 5:
                switch (level)
                {
                    case 1:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region5_level1;
                    case 2:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region5_level2;
                    case 3:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region5_level3;
                    case 4:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region5_level4;
                    case 5:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region5_level5;
                    case 6:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region5_level6;

                    default:
                        break;
                }
                break;
            case 6:
                switch (level)
                {
                    case 1:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region6_level1;
                    case 2:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region6_level2;
                    case 3:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region6_level3;
                    case 4:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region6_level4;
                    case 5:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region6_level5;
                    case 6:
                        return SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region6_level6;

                    default:
                        break;
                }
                break;

            default:
                break;
        }

        return null;
    }
    void SaveThisSkinShopInfo()
    {
        switch (region)
        {
            case 1:
                switch (level)
                {
                    case 1:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region1_level1, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region1_level1);
                        break;
                    case 2:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region1_level2, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region1_level2);
                        break;
                    case 3:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region1_level3, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region1_level3);
                        break;
                    case 4:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region1_level4, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region1_level4);
                        break;
                    case 5:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region1_level5, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region1_level5);
                        break;
                    case 6:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region1_level6, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region1_level6);
                        break;

                    default:
                        break;
                }
                break;
            case 2:
                switch (level)
                {
                    case 1:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region2_level1, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region2_level1);
                        break;
                    case 2:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region2_level2, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region2_level2);
                        break;
                    case 3:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region2_level3, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region2_level3);
                        break;
                    case 4:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region2_level4, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region2_level4);
                        break;
                    case 5:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region2_level5, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region2_level5);
                        break;
                    case 6:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region2_level6, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region2_level6);
                        break;

                    default:
                        break;
                }
                break;
            case 3:
                switch (level)
                {
                    case 1:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region3_level1, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region3_level1);
                        break;
                    case 2:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region3_level2, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region3_level2);
                        break;
                    case 3:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region3_level3, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region3_level3);
                        break;
                    case 4:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region3_level4, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region3_level4);
                        break;
                    case 5:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region3_level5, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region3_level5);
                        break;
                    case 6:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region3_level6, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region3_level6);
                        break;

                    default:
                        break;
                }
                break;
            case 4:
                switch (level)
                {
                    case 1:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region4_level1, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region4_level1);
                        break;
                    case 2:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region4_level2, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region4_level2);
                        break;
                    case 3:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region4_level3, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region4_level3);
                        break;
                    case 4:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region4_level4, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region4_level4);
                        break;
                    case 5:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region4_level5, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region4_level5);
                        break;
                    case 6:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region4_level6, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region4_level6);
                        break;

                    default:
                        break;
                }
                break;
            case 5:
                switch (level)
                {
                    case 1:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region5_level1, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region5_level1);
                        break;
                    case 2:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region5_level2, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region5_level2);
                        break;
                    case 3:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region5_level3, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region5_level3);
                        break;
                    case 4:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region5_level4, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region5_level4);
                        break;
                    case 5:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region5_level5, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region5_level5);
                        break;
                    case 6:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region5_level6, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region5_level6);
                        break;

                    default:
                        break;
                }
                break;
            case 6:
                switch (level)
                {
                    case 1:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region6_level1, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region6_level1);
                        break;
                    case 2:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region6_level2, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region6_level2);
                        break;
                    case 3:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region6_level3, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region6_level3);
                        break;
                    case 4:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region6_level4, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region6_level4);
                        break;
                    case 5:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region6_level5, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region6_level5);
                        break;
                    case 6:
                        SaveSkinInfo(SkinsManager.Instance.skinInfo.skinShopInfo.skin_Region6_level6, SkinsManager.Instance.skinInfo.skinWardrobeInfo.skin_Region6_level6);
                        break;

                    default:
                        break;
                }
                break;

            default:
                break;
        }

        SkinsManager.Instance.SaveData();
    }
    void SaveSkinInfo(SkinShopObject skinShopObject, SkinWardrobeObject skinWardrobeObject)
    {
        skinShopObject.skin_isInactive = isInactive;
        skinShopObject.skin_isAvailable = isAvailable;
        skinShopObject.skin_isBought = isBought;

        if (skinShopObject.skin_isInactive || skinShopObject.skin_isAvailable)
        {
            skinWardrobeObject.skin_isInactive = true;
            skinWardrobeObject.skin_isBought = false;
            skinWardrobeObject.skin_isSelected = false;
        }
        if (skinShopObject.skin_isBought)
        {
            skinWardrobeObject.skin_isInactive = false;
            skinWardrobeObject.skin_isBought = true;
        }

        SkinsManager.Instance.SaveData();
    }

    LevelSkinsInfo GetSkinData()
    {
        for (int i = 0; i < DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Count; i++)
        {
            if (skinType == DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].skintype)
            {
                return DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].levelSkin;
            }
        }

        return null;
    }


    //--------------------


    public void BlockButton_isPressed()
    {
        LevelSkinsInfo skinData = GetSkinData();

        //If a skin HAS been taken in a level, but has NOT been Unlocked from the Shop yet - UNLOCK IT, if you have the required essence
        if (skinData != null && skinData.isTaken && !isBought && (SkinShopManager.Instance.GetSkinCost() <= SkinShopManager.Instance.GetEssenceAquired()))
        {
            UpdateIfUnlocked(true);
            UpdateIfInactive(false);

            SkinWardrobeManager.Instance.UpdateButtonStates();
            SkinShopManager.Instance.SetSkinCostDisplay();

            //SkinShopManager.Instance.ChangeSkinCost();

            Action_BoughtSkin?.Invoke();
        }
    }


    //--------------------


    public void ClearShopButtonInfo()
    {
        isInactive = true;
        isAvailable = false;
        isBought = false;
    }
}
