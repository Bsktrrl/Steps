using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinWardrobeButton : MonoBehaviour
{
    public static event Action Action_SelectThisSkin;

    [Header("General")]
    public HatType hatType;
    public SkinType skinType;
    [SerializeField] int region;
    [SerializeField] int level;

    [Header("Status")]
    public bool isInactive;
    public bool isBought;
    public bool isSelected;

    [Header("Components")]
    [SerializeField] Image frame;


    //--------------------


    private void OnEnable()
    {
        UpdateButtonDisplay();
        IfDefaultSkinButton();

        UpdateButton();

        Action_SelectThisSkin += UpdateButton;

    }
    private void OnDisable()
    {
        Action_SelectThisSkin -= UpdateButton;
    }


    //--------------------


    public void IfDefaultSkinButton()
    {
        if (skinType == SkinType.Default && region <= 0 && level <= 0)
        {
            isInactive = false;
            isBought = true;

            if (DataManager.Instance.skinsInfo_Store.activeSkinType == SkinType.None)
            {
                WardrobeButton_isPressed();
            }
            else
            {
                isSelected = false;
            }
        }
    }


    //--------------------


    public void WardrobeButton_isPressed()
    {
        if (isBought && !isSelected)
        {
            SkinsManager.Instance.skinInfo.activeSkinType = skinType;
            SkinsManager.Instance.SaveData();

            Action_SelectThisSkin?.Invoke(); //For updating all other buttons when one is selected
        }
    }
    void UpdateButton()
    {
        if (SkinsManager.Instance.skinInfo.activeSkinType == skinType)
        {
            isSelected = true;
        }
        else
        {
            isSelected = false;
        }

        UpdateButtonDisplay();
    }


    //--------------------


    public void UpdateButtonDisplay()
    {
        switch (region)
        {
            case 1:
                switch (level)
                {
                    case 1:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level1);
                        break;
                    case 2:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level2);
                        break;
                    case 3:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level3);
                        break;
                    case 4:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level4);
                        break;
                    case 5:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level5);
                        break;
                    case 6:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level6);
                        break;

                    default:
                        break;
                }
                break;
            case 2:
                switch (level)
                {
                    case 1:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level1);
                        break;
                    case 2:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level2);
                        break;
                    case 3:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level3);
                        break;
                    case 4:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level4);
                        break;
                    case 5:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level5);
                        break;
                    case 6:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level6);
                        break;

                    default:
                        break;
                }
                break;
            case 3:
                switch (level)
                {
                    case 1:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level1);
                        break;
                    case 2:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level2);
                        break;
                    case 3:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level3);
                        break;
                    case 4:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level4);
                        break;
                    case 5:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level5);
                        break;
                    case 6:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level6);
                        break;

                    default:
                        break;
                }
                break;
            case 4:
                switch (level)
                {
                    case 1:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level1);
                        break;
                    case 2:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level2);
                        break;
                    case 3:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level3);
                        break;
                    case 4:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level4);
                        break;
                    case 5:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level5);
                        break;
                    case 6:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level6);
                        break;

                    default:
                        break;
                }
                break;
            case 5:
                switch (level)
                {
                    case 1:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level1);
                        break;
                    case 2:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level2);
                        break;
                    case 3:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level3);
                        break;
                    case 4:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level4);
                        break;
                    case 5:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level5);
                        break;
                    case 6:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level6);
                        break;

                    default:
                        break;
                }
                break;
            case 6:
                switch (level)
                {
                    case 1:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level1);
                        break;
                    case 2:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level2);
                        break;
                    case 3:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level3);
                        break;
                    case 4:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level4);
                        break;
                    case 5:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level5);
                        break;
                    case 6:
                        CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level6);
                        break;

                    default:
                        break;
                }
                break;

            default:
                CheckState(DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default);
                break;
        }

        CheckFrameColor();
    }
    void CheckState(SkinWardrobeObject skinWardrobeObject)
    {
        if (isSelected)
            skinWardrobeObject.skin_isSelected = true;
        else
            skinWardrobeObject.skin_isSelected = false;

        SkinsManager.Instance.SaveData();


        //-----


        if (skinWardrobeObject.skin_isInactive)
            isInactive = true;
        else
            isInactive = false;

        if (skinWardrobeObject.skin_isBought)
            isBought = true;
        else
            isBought = false;

        if (skinWardrobeObject.skin_isSelected)
            isSelected = true;
        else
            isSelected = false;
    }
    void CheckFrameColor()
    {
        if (isSelected)
        {
            frame.color = SkinWardrobeManager.Instance.active_Color;
        }
        else if (isBought)
        {
            frame.color = SkinWardrobeManager.Instance.bought_Color;
        }
        else if (isInactive)
        {
            frame.color = SkinWardrobeManager.Instance.inactive_Color;
        }
    }


    //--------------------


    public void ClearWardropbeButtonInfo()
    {
        isInactive = true;
        isBought = false;
        isSelected = false;
    }
}
