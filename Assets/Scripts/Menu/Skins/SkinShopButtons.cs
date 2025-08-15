using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class SkinShopButtons : MonoBehaviour
{
    [Header("General")]
    public SkinType skinType;
    [SerializeField] int region;
    [SerializeField] int level;

    [Header("Status")]
    [SerializeField] bool isInactive;
    [SerializeField] bool isAquired;
    [SerializeField] bool isUnlocked;

    [Header("Components")]
    [SerializeField] Image skinImage;
    [SerializeField] Image frame;
    [SerializeField] GameObject inactiveImage;


    //--------------------


    private void OnEnable()
    {
        UpdateSkinDisplay();
    }


    //--------------------


    public void UpdateSkinDisplay()
    {
        SkinInfo tempSkinInfo = GetThisSkinInfo();

        IfInactive_Display(tempSkinInfo);
        IfAquired_Display(tempSkinInfo);
        IfUnlocked_Display(tempSkinInfo);
    }

    void IfInactive_Display(SkinInfo skinInfo)
    {
        if (skinInfo.skin_isInactive)
        {
            isInactive = true;

            frame.color = SkinShopManager.Instance.inactive_Color;
            inactiveImage.SetActive(true);
        }
        else
        {
            isInactive = false;

            inactiveImage.SetActive(false);
        }
    }
    void IfAquired_Display(SkinInfo skinInfo)
    {
        if (skinInfo.skin_isAquired)
        {
            isAquired = true;
            isInactive = false;

            frame.color = SkinShopManager.Instance.aquired_Color;

            UpdateIfInactive(false);
        }
        else
        {
            isAquired = false;
            isInactive = true;
            UpdateIfInactive(true);

            inactiveImage.SetActive(true);
        }
    }
    void IfUnlocked_Display(SkinInfo skinInfo)
    {
        if (skinInfo.skin_isUnlocked)
        {
            isUnlocked = true;
            frame.color = SkinShopManager.Instance.unlocked_Color;
        }
        else
        {
            isUnlocked = false;
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
        isAquired = state;

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
        isUnlocked = state;

        frame.color = SkinShopManager.Instance.unlocked_Color;

        if (state)
            inactiveImage.SetActive(false);
    }


    //--------------------


    SkinInfo GetThisSkinInfo()
    {
        switch (region)
        {
            case 1:
                switch (level)
                {
                    case 1:
                        return SkinsManager.Instance.skinShopInfo.skin_Region1_level1;
                    case 2:
                        return SkinsManager.Instance.skinShopInfo.skin_Region1_level2;
                    case 3:
                        return SkinsManager.Instance.skinShopInfo.skin_Region1_level3;
                    case 4:
                        return SkinsManager.Instance.skinShopInfo.skin_Region1_level4;
                    case 5:
                        return SkinsManager.Instance.skinShopInfo.skin_Region1_level5;
                    case 6:
                        return SkinsManager.Instance.skinShopInfo.skin_Region1_level6;

                    default:
                        break;
                }
                break;
            case 2:
                switch (level)
                {
                    case 1:
                        return SkinsManager.Instance.skinShopInfo.skin_Region2_level1;
                    case 2:
                        return SkinsManager.Instance.skinShopInfo.skin_Region2_level2;
                    case 3:
                        return SkinsManager.Instance.skinShopInfo.skin_Region2_level3;
                    case 4:
                        return SkinsManager.Instance.skinShopInfo.skin_Region2_level4;
                    case 5:
                        return SkinsManager.Instance.skinShopInfo.skin_Region2_level5;
                    case 6:
                        return SkinsManager.Instance.skinShopInfo.skin_Region2_level6;

                    default:
                        break;
                }
                break;
            case 3:
                switch (level)
                {
                    case 1:
                        return SkinsManager.Instance.skinShopInfo.skin_Region3_level1;
                    case 2:
                        return SkinsManager.Instance.skinShopInfo.skin_Region3_level2;
                    case 3:
                        return SkinsManager.Instance.skinShopInfo.skin_Region3_level3;
                    case 4:
                        return SkinsManager.Instance.skinShopInfo.skin_Region3_level4;
                    case 5:
                        return SkinsManager.Instance.skinShopInfo.skin_Region3_level5;
                    case 6:
                        return SkinsManager.Instance.skinShopInfo.skin_Region3_level6;

                    default:
                        break;
                }
                break;
            case 4:
                switch (level)
                {
                    case 1:
                        return SkinsManager.Instance.skinShopInfo.skin_Region4_level1;
                    case 2:
                        return SkinsManager.Instance.skinShopInfo.skin_Region4_level2;
                    case 3:
                        return SkinsManager.Instance.skinShopInfo.skin_Region4_level3;
                    case 4:
                        return SkinsManager.Instance.skinShopInfo.skin_Region4_level4;
                    case 5:
                        return SkinsManager.Instance.skinShopInfo.skin_Region4_level5;
                    case 6:
                        return SkinsManager.Instance.skinShopInfo.skin_Region4_level6;

                    default:
                        break;
                }
                break;
            case 5:
                switch (level)
                {
                    case 1:
                        return SkinsManager.Instance.skinShopInfo.skin_Region5_level1;
                    case 2:
                        return SkinsManager.Instance.skinShopInfo.skin_Region5_level2;
                    case 3:
                        return SkinsManager.Instance.skinShopInfo.skin_Region5_level3;
                    case 4:
                        return SkinsManager.Instance.skinShopInfo.skin_Region5_level4;
                    case 5:
                        return SkinsManager.Instance.skinShopInfo.skin_Region5_level5;
                    case 6:
                        return SkinsManager.Instance.skinShopInfo.skin_Region5_level6;

                    default:
                        break;
                }
                break;
            case 6:
                switch (level)
                {
                    case 1:
                        return SkinsManager.Instance.skinShopInfo.skin_Region6_level1;
                    case 2:
                        return SkinsManager.Instance.skinShopInfo.skin_Region6_level2;
                    case 3:
                        return SkinsManager.Instance.skinShopInfo.skin_Region6_level3;
                    case 4:
                        return SkinsManager.Instance.skinShopInfo.skin_Region6_level4;
                    case 5:
                        return SkinsManager.Instance.skinShopInfo.skin_Region6_level5;
                    case 6:
                        return SkinsManager.Instance.skinShopInfo.skin_Region6_level6;

                    default:
                        break;
                }
                break;

            default:
                break;
        }

        return null;
    }
    void SaveThisSkinInfo()
    {
        switch (region)
        {
            case 1:
                switch (level)
                {
                    case 1:
                        SkinsManager.Instance.skinShopInfo.skin_Region1_level1.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region1_level1.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region1_level1.skin_isUnlocked = isUnlocked;
                        break;
                    case 2:
                        SkinsManager.Instance.skinShopInfo.skin_Region1_level2.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region1_level2.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region1_level2.skin_isUnlocked = isUnlocked;
                        break;
                    case 3:
                        SkinsManager.Instance.skinShopInfo.skin_Region1_level3.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region1_level3.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region1_level3.skin_isUnlocked = isUnlocked;
                        break;
                    case 4:
                        SkinsManager.Instance.skinShopInfo.skin_Region1_level4.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region1_level4.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region1_level4.skin_isUnlocked = isUnlocked;
                        break;
                    case 5:
                        SkinsManager.Instance.skinShopInfo.skin_Region1_level5.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region1_level5.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region1_level5.skin_isUnlocked = isUnlocked;
                        break;
                    case 6:
                        SkinsManager.Instance.skinShopInfo.skin_Region1_level6.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region1_level6.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region1_level6.skin_isUnlocked = isUnlocked;
                        break;

                    default:
                        break;
                }
                break;
            case 2:
                switch (level)
                {
                    case 1:
                        SkinsManager.Instance.skinShopInfo.skin_Region2_level1.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region2_level1.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region2_level1.skin_isUnlocked = isUnlocked;
                        break;
                    case 2:
                        SkinsManager.Instance.skinShopInfo.skin_Region2_level2.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region2_level2.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region2_level2.skin_isUnlocked = isUnlocked;
                        break;
                    case 3:
                        SkinsManager.Instance.skinShopInfo.skin_Region2_level3.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region2_level3.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region2_level3.skin_isUnlocked = isUnlocked;
                        break;
                    case 4:
                        SkinsManager.Instance.skinShopInfo.skin_Region2_level4.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region2_level4.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region2_level4.skin_isUnlocked = isUnlocked;
                        break;
                    case 5:
                        SkinsManager.Instance.skinShopInfo.skin_Region2_level5.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region2_level5.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region2_level5.skin_isUnlocked = isUnlocked;
                        break;
                    case 6:
                        SkinsManager.Instance.skinShopInfo.skin_Region2_level6.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region2_level6.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region2_level6.skin_isUnlocked = isUnlocked;
                        break;

                    default:
                        break;
                }
                break;
            case 3:
                switch (level)
                {
                    case 1:
                        SkinsManager.Instance.skinShopInfo.skin_Region3_level1.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region3_level1.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region3_level1.skin_isUnlocked = isUnlocked;
                        break;
                    case 2:
                        SkinsManager.Instance.skinShopInfo.skin_Region3_level2.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region3_level2.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region3_level2.skin_isUnlocked = isUnlocked;
                        break;
                    case 3:
                        SkinsManager.Instance.skinShopInfo.skin_Region3_level3.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region3_level3.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region3_level3.skin_isUnlocked = isUnlocked;
                        break;
                    case 4:
                        SkinsManager.Instance.skinShopInfo.skin_Region3_level4.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region3_level4.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region3_level4.skin_isUnlocked = isUnlocked;
                        break;
                    case 5:
                        SkinsManager.Instance.skinShopInfo.skin_Region3_level5.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region3_level5.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region3_level5.skin_isUnlocked = isUnlocked;
                        break;
                    case 6:
                        SkinsManager.Instance.skinShopInfo.skin_Region3_level6.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region3_level6.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region3_level6.skin_isUnlocked = isUnlocked;
                        break;

                    default:
                        break;
                }
                break;
            case 4:
                switch (level)
                {
                    case 1:
                        SkinsManager.Instance.skinShopInfo.skin_Region4_level1.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region4_level1.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region4_level1.skin_isUnlocked = isUnlocked;
                        break;
                    case 2:
                        SkinsManager.Instance.skinShopInfo.skin_Region4_level2.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region4_level2.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region4_level2.skin_isUnlocked = isUnlocked;
                        break;
                    case 3:
                        SkinsManager.Instance.skinShopInfo.skin_Region4_level3.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region4_level3.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region4_level3.skin_isUnlocked = isUnlocked;
                        break;
                    case 4:
                        SkinsManager.Instance.skinShopInfo.skin_Region4_level4.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region4_level4.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region4_level4.skin_isUnlocked = isUnlocked;
                        break;
                    case 5:
                        SkinsManager.Instance.skinShopInfo.skin_Region4_level5.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region4_level5.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region4_level5.skin_isUnlocked = isUnlocked;
                        break;
                    case 6:
                        SkinsManager.Instance.skinShopInfo.skin_Region4_level6.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region4_level6.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region4_level6.skin_isUnlocked = isUnlocked;
                        break;

                    default:
                        break;
                }
                break;
            case 5:
                switch (level)
                {
                    case 1:
                        SkinsManager.Instance.skinShopInfo.skin_Region5_level1.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region5_level1.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region5_level1.skin_isUnlocked = isUnlocked;
                        break;
                    case 2:
                        SkinsManager.Instance.skinShopInfo.skin_Region5_level2.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region5_level2.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region5_level2.skin_isUnlocked = isUnlocked;
                        break;
                    case 3:
                        SkinsManager.Instance.skinShopInfo.skin_Region5_level3.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region5_level3.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region5_level3.skin_isUnlocked = isUnlocked;
                        break;
                    case 4:
                        SkinsManager.Instance.skinShopInfo.skin_Region5_level4.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region5_level4.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region5_level4.skin_isUnlocked = isUnlocked;
                        break;
                    case 5:
                        SkinsManager.Instance.skinShopInfo.skin_Region5_level5.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region5_level5.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region5_level5.skin_isUnlocked = isUnlocked;
                        break;
                    case 6:
                        SkinsManager.Instance.skinShopInfo.skin_Region5_level6.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region5_level6.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region5_level6.skin_isUnlocked = isUnlocked;
                        break;

                    default:
                        break;
                }
                break;
            case 6:
                switch (level)
                {
                    case 1:
                        SkinsManager.Instance.skinShopInfo.skin_Region6_level1.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region6_level1.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region6_level1.skin_isUnlocked = isUnlocked;
                        break;
                    case 2:
                        SkinsManager.Instance.skinShopInfo.skin_Region6_level2.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region6_level2.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region6_level2.skin_isUnlocked = isUnlocked;
                        break;
                    case 3:
                        SkinsManager.Instance.skinShopInfo.skin_Region6_level3.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region6_level3.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region6_level3.skin_isUnlocked = isUnlocked;
                        break;
                    case 4:
                        SkinsManager.Instance.skinShopInfo.skin_Region6_level4.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region6_level4.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region6_level4.skin_isUnlocked = isUnlocked;
                        break;
                    case 5:
                        SkinsManager.Instance.skinShopInfo.skin_Region6_level5.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region6_level5.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region6_level5.skin_isUnlocked = isUnlocked;
                        break;
                    case 6:
                        SkinsManager.Instance.skinShopInfo.skin_Region6_level6.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinShopInfo.skin_Region6_level6.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinShopInfo.skin_Region6_level6.skin_isUnlocked = isUnlocked;
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

        UpdateSkinDisplay();

        //If a skin HAS been taken in a level, but has NOT been Unlocked from the Shop yet - UNLOCK IT, if you have the required essence
        /*else*/ if (GetSkinData().isTaken && !isUnlocked && (SkinShopManager.Instance.GetSkinCost() <= SkinShopManager.Instance.GetEssenceAquired()))
        {
            UpdateIfUnlocked(true);
            UpdateIfInactive(false);

            SkinShopManager.Instance.ChangeSkinCost();
        }

        SaveThisSkinInfo();
    }


    //--------------------


    public void ClearButtonInfo()
    {
        isInactive = true;
        isAquired = false;
        isUnlocked = false;
    }
}
