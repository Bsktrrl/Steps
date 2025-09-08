using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinWardrobeManager : Singleton<SkinWardrobeManager>
{
    [Header("Wardrobe Parent")]
    public GameObject wardrobeParent;

    [Header("Colors")]
    public Color inactive_Color;
    public Color bought_Color;
    public Color active_Color;

    [Header("ButtonSnap - Wardrobe")]
    public GameObject lastButtonSelected_Up_Wardrobe;
    public GameObject lastButtonSelected_Down_Wardrobe;
    public GameObject headerButtonSnap_Wardrobe;
    public GameObject backButtonSnap_Wardrobe;

    [Header("Player Model")]
    [SerializeField] GameObject playerObject;

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
        DataManager.Instance.playerStats_Store.itemsGot.essence = 12; //Remove this after testing of Skin Menu
    }


    //--------------------


    private void OnEnable()
    {
        SkinWardrobeButton.Action_SelectThisSkin += UpdatePlayerSkin;

        UpdateButtonStates();

        UpdateDefaultSkinState();

        UpdatePlayerSkin();
    }
    private void OnDisable()
    {
        SkinWardrobeButton.Action_SelectThisSkin -= UpdatePlayerSkin;
    }


    //--------------------


    public void UpdateDefaultSkinState()
    {
        if (DataManager.Instance.skinsInfo_Store.activeSkinType == SkinType.Default)
        {
            skinWardrobeButton_Default.GetComponent<SkinWardrobeButton>().isSelected = true;
            DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default.skin_isSelected = true;

            skinWardrobeButton_Default.GetComponent<SkinWardrobeButton>().WardrobeButton_isPressed();
        }
        else
        {
            skinWardrobeButton_Default.GetComponent<SkinWardrobeButton>().isSelected = false;
            DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default.skin_isSelected = false;
        }

        skinWardrobeButton_Default.GetComponent<SkinWardrobeButton>().UpdateButtonDisplay();
    }
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


    public void UpdatePlayerSkin()
    {
        HideAllSkins();

        switch (DataManager.Instance.skinsInfo_Store.activeSkinType)
        {
            case SkinType.None:
                if (skin_Default)
                    skin_Default.SetActive(true);
                break;

            case SkinType.Water_Grass:
                if (skin_Water_Grass)
                    skin_Water_Grass.SetActive(true);
                break;
            case SkinType.Water_Water:
                if (skin_Water_Water)
                    skin_Water_Water.SetActive(true);
                break;
            case SkinType.Water_Wood:
                if (skin_Water_Wood)
                    skin_Water_Wood.SetActive(true);
                break;
            case SkinType.Water_4:
                if (skin_Water_4)
                    skin_Water_4.SetActive(true);
                break;
            case SkinType.Water_5:
                if (skin_Water_5)
                    skin_Water_5.SetActive(true);
                break;
            case SkinType.Water_6:
                if (skin_Water_6)
                    skin_Water_6.SetActive(true);
                break;

            case SkinType.Cave_Stone:
                if (skin_Cave_Stone)
                    skin_Cave_Stone.SetActive(true);
                break;
            case SkinType.Cave_Stone_Brick:
                if (skin_Cave_Stone_Brick)
                    skin_Cave_Stone_Brick.SetActive(true);
                break;
            case SkinType.Cave_Lava:
                if (skin_Cave_Lava)
                    skin_Cave_Lava.SetActive(true);
                break;
            case SkinType.Cave_Rock:
                if (skin_Cave_Rock)
                    skin_Cave_Rock.SetActive(true);
                break;
            case SkinType.Cave_Brick_Brown:
                if (skin_Cave_Brick_Brown)
                    skin_Cave_Brick_Brown.SetActive(true);
                break;
            case SkinType.Cave_Brick_Black:
                if (skin_Cave_Brick_Black)
                    skin_Cave_Brick_Black.SetActive(true);
                break;

            case SkinType.Desert_Sand:
                if (skin_Desert_Sand)
                    skin_Desert_Sand.SetActive(true);
                break;
            case SkinType.Desert_Clay:
                if (skin_Desert_Clay)
                    skin_Desert_Clay.SetActive(true);
                break;
            case SkinType.Desert_Clay_Tiles:
                if (skin_Desert_Clay_Tiles)
                    skin_Desert_Clay_Tiles.SetActive(true);
                break;
            case SkinType.Desert_Sandstone:
                if (skin_Desert_Sandstone)
                    skin_Desert_Sandstone.SetActive(true);
                break;
            case SkinType.Desert_Sandstone_Swirl:
                if (skin_Desert_Sandstone_Swirl)
                    skin_Desert_Sandstone_Swirl.SetActive(true);
                break;
            case SkinType.Desert_Quicksand:
                if (skin_Desert_Quicksand)
                    skin_Desert_Quicksand.SetActive(true);
                break;

            case SkinType.Winter_Snow:
                if (skin_Winter_Snow)
                    skin_Winter_Snow.SetActive(true);
                break;
            case SkinType.Winter_Ice:
                if (skin_Winter_Ice)
                    skin_Winter_Ice.SetActive(true);
                break;
            case SkinType.Winter_ColdWood:
                if (skin_Winter_ColdWood)
                    skin_Winter_ColdWood.SetActive(true);
                break;
            case SkinType.Winter_FrozenGrass:
                if (skin_Winter_FrozenGrass)
                    skin_Winter_FrozenGrass.SetActive(true);
                break;
            case SkinType.Winter_CrackedIce:
                if (skin_Winter_CrackedIce)
                    skin_Winter_CrackedIce.SetActive(true);
                break;
            case SkinType.Winter_Crocked:
                if (skin_Winter_Crocked)
                    skin_Winter_Crocked.SetActive(true);
                break;

            case SkinType.Swamp_SwampWater:
                if (skin_Swamp_SwampWater)
                    skin_Swamp_SwampWater.SetActive(true);
                break;
            case SkinType.Swamp_Mud:
                if (skin_Swamp_Mud)
                    skin_Swamp_Mud.SetActive(true);
                break;
            case SkinType.Swamp_SwampGrass:
                if (skin_Swamp_SwampGrass)
                    skin_Swamp_SwampGrass.SetActive(true);
                break;
            case SkinType.Swamp_JungleWood:
                if (skin_Swamp_JungleWood)
                    skin_Swamp_JungleWood.SetActive(true);
                break;
            case SkinType.Swamp_SwampWood:
                if (skin_Swamp_SwampWood)
                    skin_Swamp_SwampWood.SetActive(true);
                break;
            case SkinType.Swamp_TempleBlock:
                if (skin_Swamp_TempleBlock)
                    skin_Swamp_TempleBlock.SetActive(true);
                break;

            case SkinType.Industrial_Metal:
                if (skin_Industrial_Metal)
                    skin_Industrial_Metal.SetActive(true);
                break;
            case SkinType.Industrial_Brass:
                if (skin_Industrial_Brass)
                    skin_Industrial_Brass.SetActive(true);
                break;
            case SkinType.Industrial_Gold:
                if (skin_Industrial_Gold)
                    skin_Industrial_Gold.SetActive(true);
                break;
            case SkinType.Industrial_Casing_Metal:
                if (skin_Industrial_Casing_Metal)
                    skin_Industrial_Casing_Metal.SetActive(true);
                break;
            case SkinType.Industria_Casingl_Brass:
                if (skin_Industria_Casingl_Brass)
                    skin_Industria_Casingl_Brass.SetActive(true);
                break;
            case SkinType.Industrial_Casing_Gold:
                if (skin_Industrial_Casing_Gold)
                    skin_Industrial_Casing_Gold.SetActive(true);
                break;

            case SkinType.Default:
                if (skin_Default)
                    skin_Default.SetActive(true);
                break;

            default:
                if (skin_Default)
                    skin_Default.SetActive(true);
                break;
        }
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
