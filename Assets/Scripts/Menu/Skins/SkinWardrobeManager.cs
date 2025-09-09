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

    [Header("SkinCost")]
    [SerializeField] TextMeshProUGUI esseceCost;
    public int skinCost = 1;

    #region Wardrobe Buttons
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

    public GameObject skinWardrobeButton_Default;
    #endregion

    #region Wardrobe - PlayerHatObjects

    [Header("Wardrobe - PlayerHatObjects")]
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

    public GameObject skin_Water_Grass;
    public GameObject skin_Water_Water;
    public GameObject skin_Water_Wood;
    public GameObject skin_Water_4;
    public GameObject skin_Water_5;
    public GameObject skin_Water_6;

    public GameObject skin_Cave_Stone;
    public GameObject skin_Cave_Stone_Brick;
    public GameObject skin_Cave_Lava;
    public GameObject skin_Cave_Rock;
    public GameObject skin_Cave_Brick_Brown;
    public GameObject skin_Cave_Brick_Black;

    public GameObject skin_Desert_Sand;
    public GameObject skin_Desert_Clay;
    public GameObject skin_Desert_Clay_Tiles;
    public GameObject skin_Desert_Sandstone;
    public GameObject skin_Desert_Sandstone_Swirl;
    public GameObject skin_Desert_Quicksand;

    public GameObject skin_Winter_Snow;
    public GameObject skin_Winter_Ice;
    public GameObject skin_Winter_ColdWood;
    public GameObject skin_Winter_FrozenGrass;
    public GameObject skin_Winter_CrackedIce;
    public GameObject skin_Winter_Crocked;

    public GameObject skin_Swamp_SwampWater;
    public GameObject skin_Swamp_Mud;
    public GameObject skin_Swamp_SwampGrass;
    public GameObject skin_Swamp_JungleWood;
    public GameObject skin_Swamp_SwampWood;
    public GameObject skin_Swamp_TempleBlock;

    public GameObject skin_Industrial_Metal;
    public GameObject skin_Industrial_Brass;
    public GameObject skin_Industrial_Gold;
    public GameObject skin_Industrial_Casing_Metal;
    public GameObject skin_Industria_Casingl_Brass;
    public GameObject skin_Industrial_Casing_Gold;

    #endregion

    //--------------------


    private void Start()
    {
        DataManager.Instance.playerStats_Store.itemsGot.essence_Max = 12; //Remove this after testing of Skin Menu
        DataManager.Instance.playerStats_Store.itemsGot.essence_Current = 12; //Remove this after testing of Skin Menu
    }

    private void OnEnable()
    {
        DataManager.Instance.playerStats_Store.itemsGot.essence_Max = 12; //Remove this after testing of Skin Menu
        DataManager.Instance.playerStats_Store.itemsGot.essence_Current = 12; //Remove this after testing of Skin Menu
        
        UpdateEssenceDisplay();

        UpdatePlayerBodyDisplay();
    }


    //--------------------


    public void UpdatePlayerBodyDisplay()
    {
        HideAllSkins();

        GameObject tempObj = GetSkinSelectedData();

        if (tempObj != null)
            tempObj.SetActive(true);
    }
    void HideAllSkins()
    {
        if (skin_Default)
            skin_Default.SetActive(false);

        if (skin_Water_Grass)
            skin_Water_Grass.SetActive(false);
        if (skin_Water_Water)
            skin_Water_Water.SetActive(false);
        if (skin_Water_Wood)
            skin_Water_Wood.SetActive(false);
        if (skin_Water_4)
            skin_Water_4.SetActive(false);
        if (skin_Water_5)
            skin_Water_5.SetActive(false);
        if (skin_Water_6)
            skin_Water_6.SetActive(false);

        if (skin_Cave_Stone)
            skin_Cave_Stone.SetActive(false);
        if (skin_Cave_Stone_Brick)
            skin_Cave_Stone_Brick.SetActive(false);
        if (skin_Cave_Lava)
            skin_Cave_Lava.SetActive(false);
        if (skin_Cave_Rock)
            skin_Cave_Rock.SetActive(false);
        if (skin_Cave_Brick_Brown)
            skin_Cave_Brick_Brown.SetActive(false);
        if (skin_Cave_Brick_Black)
            skin_Cave_Brick_Black.SetActive(false);

        if (skin_Desert_Sand)
            skin_Desert_Sand.SetActive(false);
        if (skin_Desert_Clay)
            skin_Desert_Clay.SetActive(false);
        if (skin_Desert_Clay_Tiles)
            skin_Desert_Clay_Tiles.SetActive(false);
        if (skin_Desert_Sandstone)
            skin_Desert_Sandstone.SetActive(false);
        if (skin_Desert_Sandstone_Swirl)
            skin_Desert_Sandstone_Swirl.SetActive(false);
        if (skin_Desert_Quicksand)
            skin_Desert_Quicksand.SetActive(false);

        if (skin_Winter_Snow)
            skin_Winter_Snow.SetActive(false);
        if (skin_Winter_Ice)
            skin_Winter_Ice.SetActive(false);
        if (skin_Winter_ColdWood)
            skin_Winter_ColdWood.SetActive(false);
        if (skin_Winter_FrozenGrass)
            skin_Winter_FrozenGrass.SetActive(false);
        if (skin_Winter_CrackedIce)
            skin_Winter_CrackedIce.SetActive(false);
        if (skin_Winter_Crocked)
            skin_Winter_Crocked.SetActive(false);

        if (skin_Swamp_SwampWater)
            skin_Swamp_SwampWater.SetActive(false);
        if (skin_Swamp_Mud)
            skin_Swamp_Mud.SetActive(false);
        if (skin_Swamp_SwampGrass)
            skin_Swamp_SwampGrass.SetActive(false);
        if (skin_Swamp_JungleWood)
            skin_Swamp_JungleWood.SetActive(false);
        if (skin_Swamp_SwampWood)
            skin_Swamp_SwampWood.SetActive(false);
        if (skin_Swamp_TempleBlock)
            skin_Swamp_TempleBlock.SetActive(false);

        if (skin_Industrial_Metal)
            skin_Industrial_Metal.SetActive(false);
        if (skin_Industrial_Brass)
            skin_Industrial_Brass.SetActive(false);
        if (skin_Industrial_Gold)
            skin_Industrial_Gold.SetActive(false);
        if (skin_Industrial_Casing_Metal)
            skin_Industrial_Casing_Metal.SetActive(false);
        if (skin_Industria_Casingl_Brass)
            skin_Industria_Casingl_Brass.SetActive(false);
        if (skin_Industrial_Casing_Gold)
            skin_Industrial_Casing_Gold.SetActive(false);
    }


    //--------------------


    public GameObject GetSkinObject(int region, int level)
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
                    case 6:
                        return skinWardrobeButton_Region1_Level6;

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
                    case 6:
                        return skinWardrobeButton_Region2_Level6;

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
                    case 6:
                        return skinWardrobeButton_Region3_Level6;

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
                    case 6:
                        return skinWardrobeButton_Region4_Level6;

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
                    case 6:
                        return skinWardrobeButton_Region5_Level6;

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
                    case 6:
                        return skinWardrobeButton_Region6_Level6;

                    default:
                        return null;
                }

            default:
                return skinWardrobeButton_Default;
        }
    }

    public WardrobeSkinState GetSkinSaveData(int region, int level)
    {
        if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo == null) return WardrobeSkinState.Inactive;

        switch (region)
        {
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
                        return WardrobeSkinState.Inactive;
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
                        return WardrobeSkinState.Inactive;
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
                        return WardrobeSkinState.Inactive;
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
                        return WardrobeSkinState.Inactive;
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
                        return WardrobeSkinState.Inactive;
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
                        return WardrobeSkinState.Inactive;
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


    //--------------------


    public void UpdateEssenceDisplay()
    {
        esseceCost.text = DataManager.Instance.playerStats_Store.itemsGot.essence_Current + " / " + skinCost;
    }


    public GameObject GetSkinSelectedData()
    {
        if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo == null)
            return skin_Default;

        if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default == WardrobeSkinState.Selected)
            return skin_Default;

        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level1 == WardrobeSkinState.Selected)
            return skin_Water_Grass;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level2 == WardrobeSkinState.Selected)
            return skin_Water_Water;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level3 == WardrobeSkinState.Selected)
            return skin_Water_Wood;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level4 == WardrobeSkinState.Selected)
            return skin_Water_4;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level5 == WardrobeSkinState.Selected)
            return skin_Water_5;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region1_level6 == WardrobeSkinState.Selected)
            return skin_Water_6;
    
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level1 == WardrobeSkinState.Selected)
            return skin_Cave_Stone;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level2 == WardrobeSkinState.Selected)
            return skin_Cave_Stone_Brick;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level3 == WardrobeSkinState.Selected)
            return skin_Cave_Lava;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level4 == WardrobeSkinState.Selected)
            return skin_Cave_Rock;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level5 == WardrobeSkinState.Selected)
            return skin_Cave_Brick_Brown;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region2_level6 == WardrobeSkinState.Selected)
            return skin_Cave_Brick_Black;

        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level1 == WardrobeSkinState.Selected)
            return skin_Desert_Sand;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level2 == WardrobeSkinState.Selected)
            return skin_Desert_Clay;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level3 == WardrobeSkinState.Selected)
            return skin_Desert_Clay_Tiles;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level4 == WardrobeSkinState.Selected)
            return skin_Desert_Sandstone;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level5 == WardrobeSkinState.Selected)
            return skin_Desert_Sandstone_Swirl;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region3_level6 == WardrobeSkinState.Selected)
            return skin_Desert_Quicksand;

        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level1 == WardrobeSkinState.Selected)
            return skin_Winter_Snow;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level2 == WardrobeSkinState.Selected)
            return skin_Winter_Ice;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level3 == WardrobeSkinState.Selected)
            return skin_Winter_ColdWood;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level4 == WardrobeSkinState.Selected)
            return skin_Winter_FrozenGrass;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level5 == WardrobeSkinState.Selected)
            return skin_Winter_CrackedIce;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region4_level6 == WardrobeSkinState.Selected)
            return skin_Winter_Crocked;

        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level1 == WardrobeSkinState.Selected)
            return skin_Swamp_SwampWater;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level2 == WardrobeSkinState.Selected)
            return skin_Swamp_Mud;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level3 == WardrobeSkinState.Selected)
            return skin_Swamp_SwampGrass;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level4 == WardrobeSkinState.Selected)
            return skin_Swamp_JungleWood;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level5 == WardrobeSkinState.Selected)
            return skin_Swamp_SwampWood;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region5_level6 == WardrobeSkinState.Selected)
            return skin_Swamp_TempleBlock;

        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level1 == WardrobeSkinState.Selected)
            return skin_Industrial_Metal;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level2 == WardrobeSkinState.Selected)
            return skin_Industrial_Brass;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level3 == WardrobeSkinState.Selected)
            return skin_Industrial_Gold;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level4 == WardrobeSkinState.Selected)
            return skin_Industrial_Casing_Metal;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level5 == WardrobeSkinState.Selected)
            return skin_Industria_Casingl_Brass;
        else if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Region6_level6 == WardrobeSkinState.Selected)
            return skin_Industrial_Casing_Gold;

        else
            return skin_Default;
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
