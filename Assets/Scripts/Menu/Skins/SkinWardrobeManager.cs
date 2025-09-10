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

    Movement movement;


    //--------------------


    private void Start()
    {
        movement = FindObjectOfType<Movement>();

        DataManager.Instance.playerStats_Store.itemsGot.essence_Max = 12; //Remove this after testing of Skin Menu
        DataManager.Instance.playerStats_Store.itemsGot.essence_Current = 12; //Remove this after testing of Skin Menu
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
        print("1. UpdatePlayerBodyDisplay: " + DataManager.Instance.skinsInfo_Store.activeSkinType);

        HideAllSkins();

        GameObject tempObj = GetSkinSelectedObject();

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
        print("2. DataManager.Instance.skinsInfo_Store.activeSkinType: " + DataManager.Instance.skinsInfo_Store.activeSkinType);

        switch (DataManager.Instance.skinsInfo_Store.activeSkinType)
        {
            case SkinType.None:
                return skin_Default;

            case SkinType.Water_Grass:
                return skin_Water_Grass;
            case SkinType.Water_Water:
                return skin_Water_Water;
            case SkinType.Water_Wood:
                return skin_Water_Wood;
            case SkinType.Water_4:
                return skin_Water_4;
            case SkinType.Water_5:
                return skin_Water_5;
            case SkinType.Water_6:
                return skin_Water_6;

            case SkinType.Cave_Stone:
                return skin_Cave_Stone;
            case SkinType.Cave_Stone_Brick:
                return skin_Cave_Stone_Brick;
            case SkinType.Cave_Lava:
                return skin_Cave_Lava;
            case SkinType.Cave_Rock:
                return skin_Cave_Rock;
            case SkinType.Cave_Brick_Brown:
                return skin_Cave_Brick_Brown;
            case SkinType.Cave_Brick_Black:
                return skin_Cave_Brick_Black;

            case SkinType.Desert_Sand:
                return skin_Desert_Sand;
            case SkinType.Desert_Clay:
                return skin_Desert_Clay;
            case SkinType.Desert_Clay_Tiles:
                return skin_Desert_Clay_Tiles;
            case SkinType.Desert_Sandstone:
                return skin_Desert_Sandstone;
            case SkinType.Desert_Sandstone_Swirl:
                return skin_Desert_Sandstone_Swirl;
            case SkinType.Desert_Quicksand:
                return skin_Desert_Quicksand;

            case SkinType.Winter_Snow:
                return skin_Winter_Snow;
            case SkinType.Winter_Ice:
                return skin_Winter_Ice;
            case SkinType.Winter_ColdWood:
                return skin_Winter_ColdWood;
            case SkinType.Winter_FrozenGrass:
                return skin_Winter_FrozenGrass;
            case SkinType.Winter_CrackedIce:
                return skin_Winter_CrackedIce;
            case SkinType.Winter_Crocked:
                return skin_Winter_Crocked;

            case SkinType.Swamp_SwampWater:
                return skin_Swamp_SwampWater;
            case SkinType.Swamp_Mud:
                return skin_Swamp_Mud;
            case SkinType.Swamp_SwampGrass:
                return skin_Swamp_SwampGrass;
            case SkinType.Swamp_JungleWood:
                return skin_Swamp_JungleWood;
            case SkinType.Swamp_SwampWood:
                return skin_Swamp_SwampWood;
            case SkinType.Swamp_TempleBlock:
                return skin_Swamp_TempleBlock;

            case SkinType.Industrial_Metal:
                return skin_Industrial_Metal;
            case SkinType.Industrial_Brass:
                return skin_Industrial_Brass;
            case SkinType.Industrial_Gold:
                return skin_Industrial_Gold;
            case SkinType.Industrial_Casing_Metal:
                return skin_Industrial_Casing_Metal;
            case SkinType.Industria_Casingl_Brass:
                return skin_Industria_Casingl_Brass;
            case SkinType.Industrial_Casing_Gold:
                return skin_Industrial_Casing_Gold;

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
