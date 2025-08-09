using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class SkinShopButtons : MonoBehaviour
{
    [Header("General")]
    [SerializeField] SkinType skinType;
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

    [Header("SkinInfo")]
    SkinInfo skinInfo = new SkinInfo();


    //--------------------


    private void Start()
    {
        skinInfo = GetThisSkinInfo();
    }

    private void OnEnable()
    {
        CheckBlockState();

        //SkinsManager.Action_SkinInfoFinishedSetup += CheckBlockState;
    }
    private void OnDisable()
    {
        //SkinsManager.Action_SkinInfoFinishedSetup -= CheckBlockState;
    }


    //--------------------


    void CheckBlockState()
    {
        CheckIfInactive();
        CheckIfAquired();
        CheckIfUnlocked();

        //SaveThisSkinInfo();
    }

    void CheckIfInactive()
    {
        if (GetThisSkinInfo().skin_isInactive)
        {
            isInactive = true;
            inactiveImage.SetActive(true);
        }
        else
        {
            isInactive = false;
            inactiveImage.SetActive(false);
        }
    }
    void CheckIfAquired()
    {
        if (GetThisSkinInfo().skin_isAquired)
        {
            isAquired = true;
            isInactive = false;
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
    void CheckIfUnlocked()
    {
        if (GetThisSkinInfo().skin_isUnlocked)
        {
            isUnlocked = true;
            frame.color = Color.yellow;
        }
        else
        {
            isUnlocked = false;
            frame.color = Color.white;
        }
    }


    //--------------------


    void UpdateBlockState()
    {
        UpdateIfInactive(isInactive);
        UpdateIfAquired(isAquired);
        UpdateIfUnlocked(isUnlocked);
    }

    void UpdateIfInactive(bool state)
    {
        isInactive = state;

        if (state)
            inactiveImage.SetActive(true);
        else
            inactiveImage.SetActive(false);
    }
    void UpdateIfAquired(bool state)
    {
        isAquired = state;

        if (state)
            inactiveImage.SetActive(false);
        else
            inactiveImage.SetActive(true);
    }
    void UpdateIfUnlocked(bool state)
    {
        isUnlocked = state;

        frame.color = Color.yellow;

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
                        return SkinsManager.Instance.skinsInfo.skin_Region1_level1;
                    case 2:
                        return SkinsManager.Instance.skinsInfo.skin_Region1_level2;
                    case 3:
                        return SkinsManager.Instance.skinsInfo.skin_Region1_level3;
                    case 4:
                        return SkinsManager.Instance.skinsInfo.skin_Region1_level4;
                    case 5:
                        return SkinsManager.Instance.skinsInfo.skin_Region1_level5;
                    case 6:
                        return SkinsManager.Instance.skinsInfo.skin_Region1_level6;

                    default:
                        break;
                }
                break;
            case 2:
                switch (level)
                {
                    case 1:
                        return SkinsManager.Instance.skinsInfo.skin_Region2_level1;
                    case 2:
                        return SkinsManager.Instance.skinsInfo.skin_Region2_level2;
                    case 3:
                        return SkinsManager.Instance.skinsInfo.skin_Region2_level3;
                    case 4:
                        return SkinsManager.Instance.skinsInfo.skin_Region2_level4;
                    case 5:
                        return SkinsManager.Instance.skinsInfo.skin_Region2_level5;
                    case 6:
                        return SkinsManager.Instance.skinsInfo.skin_Region2_level6;

                    default:
                        break;
                }
                break;
            case 3:
                switch (level)
                {
                    case 1:
                        return SkinsManager.Instance.skinsInfo.skin_Region3_level1;
                    case 2:
                        return SkinsManager.Instance.skinsInfo.skin_Region3_level2;
                    case 3:
                        return SkinsManager.Instance.skinsInfo.skin_Region3_level3;
                    case 4:
                        return SkinsManager.Instance.skinsInfo.skin_Region3_level4;
                    case 5:
                        return SkinsManager.Instance.skinsInfo.skin_Region3_level5;
                    case 6:
                        return SkinsManager.Instance.skinsInfo.skin_Region3_level6;

                    default:
                        break;
                }
                break;
            case 4:
                switch (level)
                {
                    case 1:
                        return SkinsManager.Instance.skinsInfo.skin_Region4_level1;
                    case 2:
                        return SkinsManager.Instance.skinsInfo.skin_Region4_level2;
                    case 3:
                        return SkinsManager.Instance.skinsInfo.skin_Region4_level3;
                    case 4:
                        return SkinsManager.Instance.skinsInfo.skin_Region4_level4;
                    case 5:
                        return SkinsManager.Instance.skinsInfo.skin_Region4_level5;
                    case 6:
                        return SkinsManager.Instance.skinsInfo.skin_Region4_level6;

                    default:
                        break;
                }
                break;
            case 5:
                switch (level)
                {
                    case 1:
                        return SkinsManager.Instance.skinsInfo.skin_Region5_level1;
                    case 2:
                        return SkinsManager.Instance.skinsInfo.skin_Region5_level2;
                    case 3:
                        return SkinsManager.Instance.skinsInfo.skin_Region5_level3;
                    case 4:
                        return SkinsManager.Instance.skinsInfo.skin_Region5_level4;
                    case 5:
                        return SkinsManager.Instance.skinsInfo.skin_Region5_level5;
                    case 6:
                        return SkinsManager.Instance.skinsInfo.skin_Region5_level6;

                    default:
                        break;
                }
                break;
            case 6:
                switch (level)
                {
                    case 1:
                        return SkinsManager.Instance.skinsInfo.skin_Region6_level1;
                    case 2:
                        return SkinsManager.Instance.skinsInfo.skin_Region6_level2;
                    case 3:
                        return SkinsManager.Instance.skinsInfo.skin_Region6_level3;
                    case 4:
                        return SkinsManager.Instance.skinsInfo.skin_Region6_level4;
                    case 5:
                        return SkinsManager.Instance.skinsInfo.skin_Region6_level5;
                    case 6:
                        return SkinsManager.Instance.skinsInfo.skin_Region6_level6;

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
                        SkinsManager.Instance.skinsInfo.skin_Region1_level1.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region1_level1.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region1_level1.skin_isUnlocked = isUnlocked;
                        break;
                    case 2:
                        SkinsManager.Instance.skinsInfo.skin_Region1_level2.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region1_level2.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region1_level2.skin_isUnlocked = isUnlocked;
                        break;
                    case 3:
                        SkinsManager.Instance.skinsInfo.skin_Region1_level3.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region1_level3.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region1_level3.skin_isUnlocked = isUnlocked;
                        break;
                    case 4:
                        SkinsManager.Instance.skinsInfo.skin_Region1_level4.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region1_level4.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region1_level4.skin_isUnlocked = isUnlocked;
                        break;
                    case 5:
                        SkinsManager.Instance.skinsInfo.skin_Region1_level5.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region1_level5.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region1_level5.skin_isUnlocked = isUnlocked;
                        break;
                    case 6:
                        SkinsManager.Instance.skinsInfo.skin_Region1_level6.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region1_level6.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region1_level6.skin_isUnlocked = isUnlocked;
                        break;

                    default:
                        break;
                }
                break;
            case 2:
                switch (level)
                {
                    case 1:
                        SkinsManager.Instance.skinsInfo.skin_Region2_level1.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region2_level1.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region2_level1.skin_isUnlocked = isUnlocked;
                        break;
                    case 2:
                        SkinsManager.Instance.skinsInfo.skin_Region2_level2.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region2_level2.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region2_level2.skin_isUnlocked = isUnlocked;
                        break;
                    case 3:
                        SkinsManager.Instance.skinsInfo.skin_Region2_level3.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region2_level3.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region2_level3.skin_isUnlocked = isUnlocked;
                        break;
                    case 4:
                        SkinsManager.Instance.skinsInfo.skin_Region2_level4.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region2_level4.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region2_level4.skin_isUnlocked = isUnlocked;
                        break;
                    case 5:
                        SkinsManager.Instance.skinsInfo.skin_Region2_level5.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region2_level5.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region2_level5.skin_isUnlocked = isUnlocked;
                        break;
                    case 6:
                        SkinsManager.Instance.skinsInfo.skin_Region2_level6.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region2_level6.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region2_level6.skin_isUnlocked = isUnlocked;
                        break;

                    default:
                        break;
                }
                break;
            case 3:
                switch (level)
                {
                    case 1:
                        SkinsManager.Instance.skinsInfo.skin_Region3_level1.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region3_level1.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region3_level1.skin_isUnlocked = isUnlocked;
                        break;
                    case 2:
                        SkinsManager.Instance.skinsInfo.skin_Region3_level2.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region3_level2.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region3_level2.skin_isUnlocked = isUnlocked;
                        break;
                    case 3:
                        SkinsManager.Instance.skinsInfo.skin_Region3_level3.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region3_level3.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region3_level3.skin_isUnlocked = isUnlocked;
                        break;
                    case 4:
                        SkinsManager.Instance.skinsInfo.skin_Region3_level4.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region3_level4.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region3_level4.skin_isUnlocked = isUnlocked;
                        break;
                    case 5:
                        SkinsManager.Instance.skinsInfo.skin_Region3_level5.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region3_level5.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region3_level5.skin_isUnlocked = isUnlocked;
                        break;
                    case 6:
                        SkinsManager.Instance.skinsInfo.skin_Region3_level6.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region3_level6.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region3_level6.skin_isUnlocked = isUnlocked;
                        break;

                    default:
                        break;
                }
                break;
            case 4:
                switch (level)
                {
                    case 1:
                        SkinsManager.Instance.skinsInfo.skin_Region4_level1.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region4_level1.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region4_level1.skin_isUnlocked = isUnlocked;
                        break;
                    case 2:
                        SkinsManager.Instance.skinsInfo.skin_Region4_level2.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region4_level2.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region4_level2.skin_isUnlocked = isUnlocked;
                        break;
                    case 3:
                        SkinsManager.Instance.skinsInfo.skin_Region4_level3.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region4_level3.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region4_level3.skin_isUnlocked = isUnlocked;
                        break;
                    case 4:
                        SkinsManager.Instance.skinsInfo.skin_Region4_level4.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region4_level4.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region4_level4.skin_isUnlocked = isUnlocked;
                        break;
                    case 5:
                        SkinsManager.Instance.skinsInfo.skin_Region4_level5.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region4_level5.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region4_level5.skin_isUnlocked = isUnlocked;
                        break;
                    case 6:
                        SkinsManager.Instance.skinsInfo.skin_Region4_level6.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region4_level6.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region4_level6.skin_isUnlocked = isUnlocked;
                        break;

                    default:
                        break;
                }
                break;
            case 5:
                switch (level)
                {
                    case 1:
                        SkinsManager.Instance.skinsInfo.skin_Region5_level1.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region5_level1.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region5_level1.skin_isUnlocked = isUnlocked;
                        break;
                    case 2:
                        SkinsManager.Instance.skinsInfo.skin_Region5_level2.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region5_level2.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region5_level2.skin_isUnlocked = isUnlocked;
                        break;
                    case 3:
                        SkinsManager.Instance.skinsInfo.skin_Region5_level3.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region5_level3.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region5_level3.skin_isUnlocked = isUnlocked;
                        break;
                    case 4:
                        SkinsManager.Instance.skinsInfo.skin_Region5_level4.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region5_level4.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region5_level4.skin_isUnlocked = isUnlocked;
                        break;
                    case 5:
                        SkinsManager.Instance.skinsInfo.skin_Region5_level5.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region5_level5.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region5_level5.skin_isUnlocked = isUnlocked;
                        break;
                    case 6:
                        SkinsManager.Instance.skinsInfo.skin_Region5_level6.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region5_level6.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region5_level6.skin_isUnlocked = isUnlocked;
                        break;

                    default:
                        break;
                }
                break;
            case 6:
                switch (level)
                {
                    case 1:
                        SkinsManager.Instance.skinsInfo.skin_Region6_level1.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region6_level1.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region6_level1.skin_isUnlocked = isUnlocked;
                        break;
                    case 2:
                        SkinsManager.Instance.skinsInfo.skin_Region6_level2.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region6_level2.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region6_level2.skin_isUnlocked = isUnlocked;
                        break;
                    case 3:
                        SkinsManager.Instance.skinsInfo.skin_Region6_level3.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region6_level3.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region6_level3.skin_isUnlocked = isUnlocked;
                        break;
                    case 4:
                        SkinsManager.Instance.skinsInfo.skin_Region6_level4.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region6_level4.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region6_level4.skin_isUnlocked = isUnlocked;
                        break;
                    case 5:
                        SkinsManager.Instance.skinsInfo.skin_Region6_level5.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region6_level5.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region6_level5.skin_isUnlocked = isUnlocked;
                        break;
                    case 6:
                        SkinsManager.Instance.skinsInfo.skin_Region6_level6.skin_isInactive = isInactive;
                        SkinsManager.Instance.skinsInfo.skin_Region6_level6.skin_isAquired = isAquired;
                        SkinsManager.Instance.skinsInfo.skin_Region6_level6.skin_isUnlocked = isUnlocked;
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


    //--------------------


    public void BlockButton_isPressed()
    {
        //Insert payment requirements

        CheckBlockState();

        if (!isAquired) //Change to "UpdateIfInactive(false)"
        {
            UpdateIfAquired(true);
            UpdateIfInactive(false);
        }
        else if (isAquired && !isUnlocked)
        {
            UpdateIfUnlocked(true);
            UpdateIfInactive(false);
        }
        else //Remove after testing
        {
            UpdateIfInactive(true);
        }

        SaveThisSkinInfo();

        SkinsManager.Instance.SaveData();
    }

    public void ClearButtonInfo()
    {
        isInactive = true;
        isAquired = false;
        isUnlocked = false;
    }
}
