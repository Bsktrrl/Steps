using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkinWardrobeManager : Singleton<SkinWardrobeManager>
{
    [Header("Wardrobe Parent")]
    public GameObject wardrobeParent;

    [Header("Player Model")]
    [SerializeField] GameObject playerDisplayObject;

    [Header("Colors")]
    public Color inactive_Color;
    public Color available_Color;
    public Color bought_Color;
    public Color selected_Color;

    [Header("Skin")]
    public GameObject selectedSkin;
    public GameObject selectedHat;
    [SerializeField] TextMeshProUGUI esseceCost;
    public int skinCost = 5;

    #region Wardrobe Buttons
    [Header("Wardrobe - Buttons")]
    public GameObject skinWardrobeButton_Region1_Level1;
    public GameObject skinWardrobeButton_Region1_Level2;
    public GameObject skinWardrobeButton_Region1_Level3;
    public GameObject skinWardrobeButton_Region1_Level4;
    public GameObject skinWardrobeButton_Region1_Level5;

    public GameObject skinWardrobeButton_Region2_Level1;
    public GameObject skinWardrobeButton_Region2_Level2;
    public GameObject skinWardrobeButton_Region2_Level3;
    public GameObject skinWardrobeButton_Region2_Level4;
    public GameObject skinWardrobeButton_Region2_Level5;

    public GameObject skinWardrobeButton_Region3_Level1;
    public GameObject skinWardrobeButton_Region3_Level2;
    public GameObject skinWardrobeButton_Region3_Level3;
    public GameObject skinWardrobeButton_Region3_Level4;
    public GameObject skinWardrobeButton_Region3_Level5;

    public GameObject skinWardrobeButton_Region4_Level1;
    public GameObject skinWardrobeButton_Region4_Level2;
    public GameObject skinWardrobeButton_Region4_Level3;
    public GameObject skinWardrobeButton_Region4_Level4;
    public GameObject skinWardrobeButton_Region4_Level5;

    public GameObject skinWardrobeButton_Region5_Level1;
    public GameObject skinWardrobeButton_Region5_Level2;
    public GameObject skinWardrobeButton_Region5_Level3;
    public GameObject skinWardrobeButton_Region5_Level4;
    public GameObject skinWardrobeButton_Region5_Level5;

    public GameObject skinWardrobeButton_Region6_Level1;
    public GameObject skinWardrobeButton_Region6_Level2;
    public GameObject skinWardrobeButton_Region6_Level3;
    public GameObject skinWardrobeButton_Region6_Level4;
    public GameObject skinWardrobeButton_Region6_Level5;

    public GameObject skinWardrobeButton_Default;
    #endregion

    #region Wardrobe - PlayerHatObjects

    [Header("Wardrobe - PlayerHatObjects")]
    public GameObject hat_Parent;

    public GameObject hat_Floriel;
    public GameObject hat_Granith;
    public GameObject hat_Archie;
    public GameObject hat_Aisa;
    public GameObject hat_Mossy;
    public GameObject hat_Larry;

    #endregion

    #region PlayerSkinObjects

    [Header("Wardrobe - PlayerSkinObjects")]
    public GameObject skin_Default;

    public GameObject object_Rivergreen_Lv1;
    public GameObject object_Rivergreen_Lv2;
    public GameObject object_Rivergreen_Lv3;
    public GameObject object_Rivergreen_Lv4;
    public GameObject object_Rivergreen_Lv5;

    public GameObject object_Sandlands_Lv1;
    public GameObject object_Sandlands_Lv2;
    public GameObject object_Sandlands_Lv3;
    public GameObject object_Sandlands_Lv4;
    public GameObject object_Sandlands_Lv5;

    public GameObject object_Frostfield_Lv1;
    public GameObject object_Frostfield_Lv2;
    public GameObject object_Frostfield_Lv3;
    public GameObject object_Frostfield_Lv4;
    public GameObject object_Frostfield_Lv5;

    public GameObject object_Firevein_Lv1;
    public GameObject object_Firevein_Lv2;
    public GameObject object_Firevein_Lv3;
    public GameObject object_Firevein_Lv4;
    public GameObject object_Firevein_Lv5;

    public GameObject object_Witchmire_Lv1;
    public GameObject object_Witchmire_Lv2;
    public GameObject object_Witchmire_Lv3;
    public GameObject object_Witchmire_Lv4;
    public GameObject object_Witchmire_Lv5;

    public GameObject object_Metalworks_Lv1;
    public GameObject object_Metalworks_Lv2;
    public GameObject object_Metalworks_Lv3;
    public GameObject object_Metalworks_Lv4;
    public GameObject object_Metalworks_Lv5;

    #endregion

    Movement movement;


    //--------------------


    private void Start()
    {
        movement = FindObjectOfType<Movement>();

        //DataManager.Instance.playerStats_Store.itemsGot.essence_Max = 12; //Remove this after testing of Skin Menu
        //DataManager.Instance.playerStats_Store.itemsGot.essence_Current = 12; //Remove this after testing of Skin Menu
    }

    private void OnEnable()
    {
        //DataManager.Instance.playerStats_Store.itemsGot.essence_Max = 12; //Remove this after testing of Skin Menu
        //DataManager.Instance.playerStats_Store.itemsGot.essence_Current = 12; //Remove this after testing of Skin Menu
        
        UpdateEssenceDisplay();

        UpdatePlayerBodyDisplay();
        UpdatePlayerHatDisplay();
    }


    //--------------------


    public void UpdatePlayerBodyDisplay()
    {
        //print("1. UpdatePlayerBodyDisplay: " + DataManager.Instance.skinsInfo_Store.activeSkinType);

        HideAllSkins();

        GameObject tempObj = GetSkinSelectedObject();

        if (tempObj != null)
            tempObj.SetActive(true);

        if (DataManager.Instance.skinsInfo_Store.activeSkinType != SkinType.Default && DataManager.Instance.skinsInfo_Store.activeSkinType != SkinType.None)
        {
            if (skin_Default)
                skin_Default.SetActive(false);
        }
    }
    void HideAllSkins()
    {
        if (skin_Default)
            skin_Default.SetActive(false);

        if (object_Rivergreen_Lv1)
            object_Rivergreen_Lv1.SetActive(false);
        if (object_Rivergreen_Lv2)
            object_Rivergreen_Lv2.SetActive(false);
        if (object_Rivergreen_Lv3)
            object_Rivergreen_Lv3.SetActive(false);
        if (object_Rivergreen_Lv4)
            object_Rivergreen_Lv4.SetActive(false);
        if (object_Rivergreen_Lv5)
            object_Rivergreen_Lv5.SetActive(false);

        if (object_Firevein_Lv1)
            object_Firevein_Lv1.SetActive(false);
        if (object_Firevein_Lv2)
            object_Firevein_Lv2.SetActive(false);
        if (object_Firevein_Lv3)
            object_Firevein_Lv3.SetActive(false);
        if (object_Firevein_Lv4)
            object_Firevein_Lv4.SetActive(false);
        if (object_Firevein_Lv5)
            object_Firevein_Lv5.SetActive(false);

        if (object_Sandlands_Lv1)
            object_Sandlands_Lv1.SetActive(false);
        if (object_Sandlands_Lv2)
            object_Sandlands_Lv2.SetActive(false);
        if (object_Sandlands_Lv3)
            object_Sandlands_Lv3.SetActive(false);
        if (object_Sandlands_Lv4)
            object_Sandlands_Lv4.SetActive(false);
        if (object_Sandlands_Lv5)
            object_Sandlands_Lv5.SetActive(false);

        if (object_Frostfield_Lv1)
            object_Frostfield_Lv1.SetActive(false);
        if (object_Frostfield_Lv2)
            object_Frostfield_Lv2.SetActive(false);
        if (object_Frostfield_Lv3)
            object_Frostfield_Lv3.SetActive(false);
        if (object_Frostfield_Lv4)
            object_Frostfield_Lv4.SetActive(false);
        if (object_Frostfield_Lv5)
            object_Frostfield_Lv5.SetActive(false);

        if (object_Witchmire_Lv1)
            object_Witchmire_Lv1.SetActive(false);
        if (object_Witchmire_Lv2)
            object_Witchmire_Lv2.SetActive(false);
        if (object_Witchmire_Lv3)
            object_Witchmire_Lv3.SetActive(false);
        if (object_Witchmire_Lv4)
            object_Witchmire_Lv4.SetActive(false);
        if (object_Witchmire_Lv5)
            object_Witchmire_Lv5.SetActive(false);

        if (object_Metalworks_Lv1)
            object_Metalworks_Lv1.SetActive(false);
        if (object_Metalworks_Lv2)
            object_Metalworks_Lv2.SetActive(false);
        if (object_Metalworks_Lv3)
            object_Metalworks_Lv3.SetActive(false);
        if (object_Metalworks_Lv4)
            object_Metalworks_Lv4.SetActive(false);
        if (object_Metalworks_Lv5)
            object_Metalworks_Lv5.SetActive(false);

        if (skin_Default)
            skin_Default.SetActive(false);
    }
    public void UpdatePlayerHatDisplay()
    {
        HideAllHats();

        GameObject tempObj = GetHatSelectedObject();

        if (tempObj != null)
            tempObj.SetActive(true);
    }
    void HideAllHats()
    {
        if (hat_Floriel)
            hat_Floriel.SetActive(false);
        if (hat_Granith)
            hat_Granith.SetActive(false);
        if (hat_Archie)
            hat_Archie.SetActive(false);
        if (hat_Aisa)
            hat_Aisa.SetActive(false);
        if (hat_Mossy)
            hat_Mossy.SetActive(false);
        if (hat_Larry)
            hat_Larry.SetActive(false);
    }

    //--------------------


    public GameObject GetSkinButtonObject(int region, int level)
    {
        switch (region)
        {
            case 1:
                switch (level)
                {
                    case 1:
                        return skinWardrobeButton_Region1_Level1;
                    case 2:
                        return skinWardrobeButton_Region1_Level2;
                    case 3:
                        return skinWardrobeButton_Region1_Level3;
                    case 4:
                        return skinWardrobeButton_Region1_Level4;
                    case 5:
                        return skinWardrobeButton_Region1_Level5;

                    default:
                        return null;
                }
            case 2:
                switch (level)
                {
                    case 1:
                        return skinWardrobeButton_Region2_Level1;
                    case 2:
                        return skinWardrobeButton_Region2_Level2;
                    case 3:
                        return skinWardrobeButton_Region2_Level3;
                    case 4:
                        return skinWardrobeButton_Region2_Level4;
                    case 5:
                        return skinWardrobeButton_Region2_Level5;

                    default:
                        return null;
                }
            case 3:
                switch (level)
                {
                    case 1:
                        return skinWardrobeButton_Region3_Level1;
                    case 2:
                        return skinWardrobeButton_Region3_Level2;
                    case 3:
                        return skinWardrobeButton_Region3_Level3;
                    case 4:
                        return skinWardrobeButton_Region3_Level4;
                    case 5:
                        return skinWardrobeButton_Region3_Level5;

                    default:
                        return null;
                }
            case 4:
                switch (level)
                {
                    case 1:
                        return skinWardrobeButton_Region4_Level1;
                    case 2:
                        return skinWardrobeButton_Region4_Level2;
                    case 3:
                        return skinWardrobeButton_Region4_Level3;
                    case 4:
                        return skinWardrobeButton_Region4_Level4;
                    case 5:
                        return skinWardrobeButton_Region4_Level5;

                    default:
                        return null;
                }
            case 5:
                switch (level)
                {
                    case 1:
                        return skinWardrobeButton_Region5_Level1;
                    case 2:
                        return skinWardrobeButton_Region5_Level2;
                    case 3:
                        return skinWardrobeButton_Region5_Level3;
                    case 4:
                        return skinWardrobeButton_Region5_Level4;
                    case 5:
                        return skinWardrobeButton_Region5_Level5;

                    default:
                        return null;
                }
            case 6:
                switch (level)
                {
                    case 1:
                        return skinWardrobeButton_Region6_Level1;
                    case 2:
                        return skinWardrobeButton_Region6_Level2;
                    case 3:
                        return skinWardrobeButton_Region6_Level3;
                    case 4:
                        return skinWardrobeButton_Region6_Level4;
                    case 5:
                        return skinWardrobeButton_Region6_Level5;

                    default:
                        return null;
                }

            default:
                return skinWardrobeButton_Default;
        }
    }
    public GameObject GetHatObject(HatType hatType)
    {
        switch (hatType)
        {
            case HatType.None:
                return null;

            case HatType.Floriel_Hat:
                return hat_Floriel;
            case HatType.Granith_Hat:
                return hat_Granith;
            case HatType.Archie_Hat:
                return hat_Archie;
            case HatType.Aisa_Hat:
                return hat_Aisa;
            case HatType.Mossy_Hat:
                return hat_Mossy;
            case HatType.Larry_Hat:
                return hat_Larry;

            default:
                return null;
        }
    }

    public GameObject GetSkinSelectedObject()
    {
        //print("2. DataManager.Instance.skinsInfo_Store.activeSkinType: " + DataManager.Instance.skinsInfo_Store.activeSkinType);

        switch (DataManager.Instance.skinsInfo_Store.activeSkinType)
        {
            case SkinType.None:
                return skin_Default;

            case SkinType.Rivergreen_Lv1:
                return object_Rivergreen_Lv1;
            case SkinType.Rivergreen_Lv2:
                return object_Rivergreen_Lv2;
            case SkinType.Rivergreen_Lv3:
                return object_Rivergreen_Lv3;
            case SkinType.Rivergreen_Lv4:
                return object_Rivergreen_Lv4;
            case SkinType.Rivergreen_Lv5:
                return object_Rivergreen_Lv5;

            case SkinType.Firevein_Lv1:
                return object_Firevein_Lv1;
            case SkinType.Firevein_Lv2:
                return object_Firevein_Lv2;
            case SkinType.Firevein_Lv3:
                return object_Firevein_Lv3;
            case SkinType.Firevein_Lv4:
                return object_Firevein_Lv4;
            case SkinType.Firevein_Lv5:
                return object_Firevein_Lv5;

            case SkinType.Sandlands_Lv1:
                return object_Sandlands_Lv1;
            case SkinType.Sandlands_Lv2:
                return object_Sandlands_Lv2;
            case SkinType.Sandlands_Lv3:
                return object_Sandlands_Lv3;
            case SkinType.Sandlands_Lv4:
                return object_Sandlands_Lv4;
            case SkinType.Sandlands_Lv5:
                return object_Sandlands_Lv5;

            case SkinType.Frostfield_Lv1:
                return object_Frostfield_Lv1;
            case SkinType.Frostfield_Lv2:
                return object_Frostfield_Lv2;
            case SkinType.Frostfield_Lv3:
                return object_Frostfield_Lv3;
            case SkinType.Frostfield_Lv4:
                return object_Frostfield_Lv4;
            case SkinType.Frostfield_Lv5:
                return object_Frostfield_Lv5;

            case SkinType.Witchmire_Lv1:
                return object_Witchmire_Lv1;
            case SkinType.Witchmire_Lv2:
                return object_Witchmire_Lv2;
            case SkinType.Witchmire_Lv3:
                return object_Witchmire_Lv3;
            case SkinType.Witchmire_Lv4:
                return object_Witchmire_Lv4;
            case SkinType.Witchmire_Lv5:
                return object_Witchmire_Lv5;

            case SkinType.Metalworks_Lv1:
                return object_Metalworks_Lv1;
            case SkinType.Metalworks_Lv2:
                return object_Metalworks_Lv2;
            case SkinType.Metalworks_Lv3:
                return object_Metalworks_Lv3;
            case SkinType.Metalworks_Lv4:
                return object_Metalworks_Lv4;
            case SkinType.Metalworks_Lv5:
                return object_Metalworks_Lv5;

            case SkinType.Default:
                return skin_Default;

            default:
                return skin_Default;
        }
    }
    public GameObject GetHatSelectedObject()
    {
        switch (DataManager.Instance.skinsInfo_Store.activeHatType)
        {
            case HatType.None:
                return null;

            case HatType.Floriel_Hat:
                return hat_Floriel;
            case HatType.Granith_Hat:
                return hat_Granith;
            case HatType.Archie_Hat:
                return hat_Archie;
            case HatType.Aisa_Hat:
                return hat_Aisa;
            case HatType.Mossy_Hat:
                return hat_Mossy;
            case HatType.Larry_Hat:
                return hat_Larry;

            default:
                return null;
        }
    }

    public WardrobeSkinState GetSkinSaveData(int region, int level)
    {
        if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo == null) return WardrobeSkinState.Inactive;

        switch (region)
        {
            case 0:
                switch (level)
                {
                    case 0:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default;
                    default:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default;
                }
                
            case 1:
                switch (level)
                {
                    case 1:
                            return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level1;
                    case 2:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level2;
                    case 3:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level3;
                    case 4:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level4;
                    case 5:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level5;
                    case 6:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level6;

                    default:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default;
                }
            case 2:
                switch (level)
                {
                    case 1:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level1;
                    case 2:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level2;
                    case 3:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level3;
                    case 4:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level4;
                    case 5:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level5;
                    case 6:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level6;

                    default:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default;
                }
            case 3:
                switch (level)
                {
                    case 1:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level1;
                    case 2:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level2;
                    case 3:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level3;
                    case 4:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level4;
                    case 5:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level5;
                    case 6:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level6;

                    default:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default;
                }
            case 4:
                switch (level)
                {
                    case 1:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level1;
                    case 2:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level2;
                    case 3:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level3;
                    case 4:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level4;
                    case 5:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level5;
                    case 6:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level6;

                    default:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default;
                }
            case 5:
                switch (level)
                {
                    case 1:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level1;
                    case 2:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level2;
                    case 3:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level3;
                    case 4:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level4;
                    case 5:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level5;
                    case 6:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level6;

                    default:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default;
                }
            case 6:
                switch (level)
                {
                    case 1:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level1;
                    case 2:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level2;
                    case 3:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level3;
                    case 4:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level4;
                    case 5:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level5;
                    case 6:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level6;

                    default:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default;
                }

            default:
                return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default;
        }
    }
    public void SetSkinSaveData(int region, int level, WardrobeSkinState skinState)
    {
        if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo == null) return;

        switch (region)
        {
            case 0:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default = skinState;
                break;
            case 1:
                switch (level)
                {
                    case 1:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level1 = skinState;
                        break;
                    case 2:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level2 = skinState;
                        break;
                    case 3:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level3 = skinState;
                        break;
                    case 4:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level4 = skinState;
                        break;
                    case 5:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level5 = skinState;
                        break;
                    case 6:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level6 = skinState;
                        break;

                    default:
                        break;
                }
                break;
            case 2:
                switch (level)
                {
                    case 1:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level1 = skinState;
                        break;
                    case 2:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level2 = skinState;
                        break;
                    case 3:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level3 = skinState;
                        break;
                    case 4:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level4 = skinState;
                        break;
                    case 5:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level5 = skinState;
                        break;
                    case 6:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level6 = skinState;
                        break;

                    default:
                        break;
                }
                break;
            case 3:
                switch (level)
                {
                    case 1:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level1 = skinState;
                        break;
                    case 2:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level2 = skinState;
                        break;
                    case 3:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level3 = skinState;
                        break;
                    case 4:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level4 = skinState;
                        break;
                    case 5:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level5 = skinState;
                        break;
                    case 6:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level6 = skinState;
                        break;

                    default:
                        break;
                }
                break;
            case 4:
                switch (level)
                {
                    case 1:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level1 = skinState;
                        break;
                    case 2:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level2 = skinState;
                        break;
                    case 3:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level3 = skinState;
                        break;
                    case 4:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level4 = skinState;
                        break;
                    case 5:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level5 = skinState;
                        break;
                    case 6:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level6 = skinState;
                        break;

                    default:
                        break;
                }
                break;
            case 5:
                switch (level)
                {
                    case 1:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level1 = skinState;
                        break;
                    case 2:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level2 = skinState;
                        break;
                    case 3:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level3 = skinState;
                        break;
                    case 4:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level4 = skinState;
                        break;
                    case 5:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level5 = skinState;
                        break;
                    case 6:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level6 = skinState;
                        break;

                    default:
                        break;
                }
                break;
            case 6:
                switch (level)
                {
                    case 1:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level1 = skinState;
                        break;
                    case 2:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level2 = skinState;
                        break;
                    case 3:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level3 = skinState;
                        break;
                    case 4:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level4 = skinState;
                        break;
                    case 5:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level5 = skinState;
                        break;
                    case 6:
                        DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level6 = skinState;
                        break;

                    default:
                        break;
                }
                break;

            default:
                DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default = skinState;
                break;
        }

        SkinsManager.Instance.SaveData();
    }

    public void SetActiveSkinData(SkinType skintype)
    {
        DataManager.Instance.skinsInfo_Store.activeSkinType = skintype;
        SkinsManager.Instance.skinInfo.activeSkinType = skintype;

        SkinsManager.Instance.SaveData();
    }
    public void SetActiveHatData(HatType hatType)
    {
        DataManager.Instance.skinsInfo_Store.activeHatType = hatType;
        SkinsManager.Instance.skinInfo.activeHatType = hatType;

        SkinsManager.Instance.SaveData();
    }

    public WardrobeHatState GetHatSaveData(HatType hatType)
    {
        if (DataManager.Instance.skinsInfo_Store.skinHatInfo == null) return WardrobeHatState.Inactive;

        switch (hatType)
        {
            case HatType.None:
                return WardrobeHatState.Inactive;

            case HatType.Floriel_Hat:
                return DataManager.Instance.skinsInfo_Store.skinHatInfo.skin_Region1_Hat;
            case HatType.Granith_Hat:
                return DataManager.Instance.skinsInfo_Store.skinHatInfo.skin_Region2_Hat;
            case HatType.Archie_Hat:
                return DataManager.Instance.skinsInfo_Store.skinHatInfo.skin_Region3_Hat;
            case HatType.Aisa_Hat:
                return DataManager.Instance.skinsInfo_Store.skinHatInfo.skin_Region4_Hat;
            case HatType.Mossy_Hat:
                return DataManager.Instance.skinsInfo_Store.skinHatInfo.skin_Region5_Hat;
            case HatType.Larry_Hat:
                return DataManager.Instance.skinsInfo_Store.skinHatInfo.skin_Region6_Hat;

            default:
                return WardrobeHatState.Inactive;
        }
    }
    public void SetHatSaveData(HatType hatType, WardrobeHatState hatState)
    {
        if (DataManager.Instance.skinsInfo_Store.skinHatInfo == null) return;

        switch (hatType)
        {
            case HatType.None:
                break;

            case HatType.Floriel_Hat:
                DataManager.Instance.skinsInfo_Store.skinHatInfo.skin_Region1_Hat = hatState;
                break;
            case HatType.Granith_Hat:
                DataManager.Instance.skinsInfo_Store.skinHatInfo.skin_Region2_Hat = hatState;
                break;
            case HatType.Archie_Hat:
                DataManager.Instance.skinsInfo_Store.skinHatInfo.skin_Region3_Hat = hatState;
                break;
            case HatType.Aisa_Hat:
                DataManager.Instance.skinsInfo_Store.skinHatInfo.skin_Region4_Hat = hatState;
                break;
            case HatType.Mossy_Hat:
                DataManager.Instance.skinsInfo_Store.skinHatInfo.skin_Region5_Hat = hatState;
                break;
            case HatType.Larry_Hat:
                DataManager.Instance.skinsInfo_Store.skinHatInfo.skin_Region6_Hat = hatState;
                break;

            default:
                break;
        }

        SkinsManager.Instance.SaveData();
    }


    //--------------------


    public void MoveHatObjectsToSelectedSkin()
    {
        if (selectedSkin)
        {
            hat_Parent.transform.SetParent(selectedSkin.transform.Find("Armature_Player/Root"), true);
            hat_Parent.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }


    //--------------------


    public void UpdateEssenceDisplay()
    {
        esseceCost.text = DataManager.Instance.playerStats_Store.itemsGot.essence_Current + " / " + skinCost;
    }
}

[Serializable]
public class SkinsWardrobeInfo
{
    [Header("Region 1")]
    public WardrobeSkinState skin_Region1_level1;
    public WardrobeSkinState skin_Region1_level2;
    public WardrobeSkinState skin_Region1_level3;
    public WardrobeSkinState skin_Region1_level4;
    public WardrobeSkinState skin_Region1_level5;
    public WardrobeSkinState skin_Region1_level6;


    [Header("Region 2")]
    public WardrobeSkinState skin_Region2_level1;
    public WardrobeSkinState skin_Region2_level2;
    public WardrobeSkinState skin_Region2_level3;
    public WardrobeSkinState skin_Region2_level4;
    public WardrobeSkinState skin_Region2_level5;
    public WardrobeSkinState skin_Region2_level6;

    [Header("Region 3")]
    public WardrobeSkinState skin_Region3_level1;
    public WardrobeSkinState skin_Region3_level2;
    public WardrobeSkinState skin_Region3_level3;
    public WardrobeSkinState skin_Region3_level4;
    public WardrobeSkinState skin_Region3_level5;
    public WardrobeSkinState skin_Region3_level6;

    [Header("Region 4")]
    public WardrobeSkinState skin_Region4_level1;
    public WardrobeSkinState skin_Region4_level2;
    public WardrobeSkinState skin_Region4_level3;
    public WardrobeSkinState skin_Region4_level4;
    public WardrobeSkinState skin_Region4_level5;
    public WardrobeSkinState skin_Region4_level6;

    [Header("Region 5")]
    public WardrobeSkinState skin_Region5_level1;
    public WardrobeSkinState skin_Region5_level2;
    public WardrobeSkinState skin_Region5_level3;
    public WardrobeSkinState skin_Region5_level4;
    public WardrobeSkinState skin_Region5_level5;
    public WardrobeSkinState skin_Region5_level6;

    [Header("Region 6")]
    public WardrobeSkinState skin_Region6_level1;
    public WardrobeSkinState skin_Region6_level2;
    public WardrobeSkinState skin_Region6_level3;
    public WardrobeSkinState skin_Region6_level4;
    public WardrobeSkinState skin_Region6_level5;
    public WardrobeSkinState skin_Region6_level6;

    [Header("Default")]
    public WardrobeSkinState skin_Default;
}

[Serializable]
public class SkinsHatInfo
{
    [Header("Hats")]
    public WardrobeHatState skin_Region1_Hat;
    public WardrobeHatState skin_Region2_Hat;
    public WardrobeHatState skin_Region3_Hat;
    public WardrobeHatState skin_Region4_Hat;
    public WardrobeHatState skin_Region5_Hat;
    public WardrobeHatState skin_Region6_Hat;
}

public enum WardrobeSkinState
{
    Inactive,
    Available,
    Bought,
    Selected
}
public enum WardrobeHatState
{
    Inactive,
    Available,
    Selected
}
