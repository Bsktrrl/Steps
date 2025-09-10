using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Body : MonoBehaviour
{
    [Header("selectedSkinBlock")]
    public GameObject selectedSkinBlock;

    #region PlayerHatObjects

    [Header("PlayerHatObjects")]
    public GameObject hat_Parent;

    public GameObject hat_Floriel;
    public GameObject hat_Granith;
    public GameObject hat_Archie;
    public GameObject hat_Aisa;
    public GameObject hat_Mossy;
    public GameObject hat_Larry;

    #endregion

    #region PlayerSkinObjects

    [Header("PlayerSkinObjects")]
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


    private void Update()
    {
        UpdatePlayerSkin();
    }

    private void OnEnable()
    {
        SkinWardrobeButton.Action_SelectThisSkin += UpdatePlayerSkin;
        DataManager.Action_dataHasLoaded += UpdatePlayerSkin;

        SkinWardrobeButton.Action_SelectThisHat += UpdatePlayerHats;
        DataManager.Action_dataHasLoaded += UpdatePlayerHats;

        UpdatePlayerSkin();
        UpdatePlayerHats();
    }
    private void OnDisable()
    {
        SkinWardrobeButton.Action_SelectThisSkin -= UpdatePlayerSkin;
        DataManager.Action_dataHasLoaded -= UpdatePlayerSkin;

        SkinWardrobeButton.Action_SelectThisHat -= UpdatePlayerHats;
        DataManager.Action_dataHasLoaded -= UpdatePlayerHats;
    }


    //--------------------


    public void UpdatePlayerSkin()
    {
        HideAllSkins();

        switch (DataManager.Instance.skinsInfo_Store.activeSkinType)
        {
            case SkinType.None:
                if (skin_Default)
                {
                    selectedSkinBlock = skin_Default;
                    skin_Default.SetActive(true);
                }
                break;

            case SkinType.Water_Grass:
                if (skin_Water_Grass)
                {
                    selectedSkinBlock = skin_Water_Grass;
                    skin_Water_Grass.SetActive(true);
                }
                break;
            case SkinType.Water_Water:
                if (skin_Water_Water)
                {
                    selectedSkinBlock = skin_Water_Water;
                    skin_Water_Water.SetActive(true);
                }
                break;
            case SkinType.Water_Wood:
                if (skin_Water_Wood)
                {
                    selectedSkinBlock = skin_Water_Wood;
                    skin_Water_Wood.SetActive(true);
                }
                break;
            case SkinType.Water_4:
                if (skin_Water_4)
                {
                    selectedSkinBlock = skin_Water_4;
                    skin_Water_4.SetActive(true);
                }
                break;
            case SkinType.Water_5:
                if (skin_Water_5)
                {
                    selectedSkinBlock = skin_Water_5;
                    skin_Water_5.SetActive(true);
                }
                break;
            case SkinType.Water_6:
                if (skin_Water_6)
                {
                    selectedSkinBlock = skin_Water_6;
                    skin_Water_6.SetActive(true);
                }
                break;

            case SkinType.Cave_Stone:
                if (skin_Cave_Stone)
                {
                    selectedSkinBlock = skin_Cave_Stone;
                    skin_Cave_Stone.SetActive(true);
                }
                break;
            case SkinType.Cave_Stone_Brick:
                if (skin_Cave_Stone_Brick)
                {
                    selectedSkinBlock = skin_Cave_Stone_Brick;
                    skin_Cave_Stone_Brick.SetActive(true);
                }
                break;
            case SkinType.Cave_Lava:
                if (skin_Cave_Lava)
                {
                    selectedSkinBlock = skin_Cave_Lava;
                    skin_Cave_Lava.SetActive(true);
                }
                break;
            case SkinType.Cave_Rock:
                if (skin_Cave_Rock)
                {
                    selectedSkinBlock = skin_Cave_Rock;
                    skin_Cave_Rock.SetActive(true);
                }
                break;
            case SkinType.Cave_Brick_Brown:
                if (skin_Cave_Brick_Brown)
                {
                    selectedSkinBlock = skin_Cave_Brick_Brown;
                    skin_Cave_Brick_Brown.SetActive(true);
                }
                break;
            case SkinType.Cave_Brick_Black:
                if (skin_Cave_Brick_Black)
                {
                    selectedSkinBlock = skin_Cave_Brick_Black;
                    skin_Cave_Brick_Black.SetActive(true);
                }
                break;

            case SkinType.Desert_Sand:
                if (skin_Desert_Sand)
                {
                    selectedSkinBlock = skin_Desert_Sand;
                    skin_Desert_Sand.SetActive(true);
                }
                break;
            case SkinType.Desert_Clay:
                if (skin_Desert_Clay)
                {
                    selectedSkinBlock = skin_Desert_Clay;
                    skin_Desert_Clay.SetActive(true);
                }
                break;
            case SkinType.Desert_Clay_Tiles:
                if (skin_Desert_Clay_Tiles)
                {
                    selectedSkinBlock = skin_Desert_Clay_Tiles;
                    skin_Desert_Clay_Tiles.SetActive(true);
                }
                break;
            case SkinType.Desert_Sandstone:
                if (skin_Desert_Sandstone)
                {
                    selectedSkinBlock = skin_Desert_Sandstone;
                    skin_Desert_Sandstone.SetActive(true);
                }
                break;
            case SkinType.Desert_Sandstone_Swirl:
                if (skin_Desert_Sandstone_Swirl)
                {
                    selectedSkinBlock = skin_Desert_Sandstone_Swirl;
                    skin_Desert_Sandstone_Swirl.SetActive(true);
                }
                break;
            case SkinType.Desert_Quicksand:
                if (skin_Desert_Quicksand)
                {
                    selectedSkinBlock = skin_Desert_Quicksand;
                    skin_Desert_Quicksand.SetActive(true);
                }
                break;

            case SkinType.Winter_Snow:
                if (skin_Winter_Snow)
                {
                    selectedSkinBlock = skin_Winter_Snow;
                    skin_Winter_Snow.SetActive(true);
                }
                break;
            case SkinType.Winter_Ice:
                if (skin_Winter_Ice)
                {
                    selectedSkinBlock = skin_Winter_Ice;
                    skin_Winter_Ice.SetActive(true);
                }
                break;
            case SkinType.Winter_ColdWood:
                if (skin_Winter_ColdWood)
                {
                    selectedSkinBlock = skin_Winter_ColdWood;
                    skin_Winter_ColdWood.SetActive(true);
                }
                break;
            case SkinType.Winter_FrozenGrass:
                if (skin_Winter_FrozenGrass)
                {
                    selectedSkinBlock = skin_Winter_FrozenGrass;
                    skin_Winter_FrozenGrass.SetActive(true);
                }
                break;
            case SkinType.Winter_CrackedIce:
                if (skin_Winter_CrackedIce)
                {
                    selectedSkinBlock = skin_Winter_CrackedIce;
                    skin_Winter_CrackedIce.SetActive(true);
                }
                break;
            case SkinType.Winter_Crocked:
                if (skin_Winter_Crocked)
                {
                    selectedSkinBlock = skin_Winter_Crocked;
                    skin_Winter_Crocked.SetActive(true);
                }
                break;

            case SkinType.Swamp_SwampWater:
                if (skin_Swamp_SwampWater)
                {
                    selectedSkinBlock = skin_Swamp_SwampWater;
                    skin_Swamp_SwampWater.SetActive(true);
                }
                break;
            case SkinType.Swamp_Mud:
                if (skin_Swamp_Mud)
                {
                    selectedSkinBlock = skin_Swamp_Mud;
                    skin_Swamp_Mud.SetActive(true);
                }
                break;
            case SkinType.Swamp_SwampGrass:
                if (skin_Swamp_SwampGrass)
                {
                    selectedSkinBlock = skin_Swamp_SwampGrass;
                    skin_Swamp_SwampGrass.SetActive(true);
                }
                break;
            case SkinType.Swamp_JungleWood:
                if (skin_Swamp_JungleWood)
                {
                    selectedSkinBlock = skin_Swamp_JungleWood;
                    skin_Swamp_JungleWood.SetActive(true);
                }
                break;
            case SkinType.Swamp_SwampWood:
                if (skin_Swamp_SwampWood)
                {
                    selectedSkinBlock = skin_Swamp_SwampWood;
                    skin_Swamp_SwampWood.SetActive(true);
                }
                break;
            case SkinType.Swamp_TempleBlock:
                if (skin_Swamp_TempleBlock)
                {
                    selectedSkinBlock = skin_Swamp_TempleBlock;
                    skin_Swamp_TempleBlock.SetActive(true);
                }
                break;

            case SkinType.Industrial_Metal:
                if (skin_Industrial_Metal)
                {
                    selectedSkinBlock = skin_Industrial_Metal;
                    skin_Industrial_Metal.SetActive(true);
                }
                break;
            case SkinType.Industrial_Brass:
                if (skin_Industrial_Brass)
                {
                    selectedSkinBlock = skin_Industrial_Brass;
                    skin_Industrial_Brass.SetActive(true);
                }
                break;
            case SkinType.Industrial_Gold:
                if (skin_Industrial_Gold)
                {
                    selectedSkinBlock = skin_Industrial_Gold;
                    skin_Industrial_Gold.SetActive(true);
                }
                break;
            case SkinType.Industrial_Casing_Metal:
                if (skin_Industrial_Casing_Metal)
                {
                    selectedSkinBlock = skin_Industrial_Casing_Metal;
                    skin_Industrial_Casing_Metal.SetActive(true);
                }
                break;
            case SkinType.Industria_Casingl_Brass:
                if (skin_Industria_Casingl_Brass)
                {
                    selectedSkinBlock = skin_Industria_Casingl_Brass;
                    skin_Industria_Casingl_Brass.SetActive(true);
                }
                break;
            case SkinType.Industrial_Casing_Gold:
                if (skin_Industrial_Casing_Gold)
                {
                    selectedSkinBlock = skin_Industrial_Casing_Gold;
                    skin_Industrial_Casing_Gold.SetActive(true);
                }
                break;

            case SkinType.Default:
                if (skin_Default)
                {
                    selectedSkinBlock = skin_Default;
                    skin_Default.SetActive(true);
                }
                break;

            default:
                if (skin_Default)
                {
                    selectedSkinBlock = skin_Default;
                    skin_Default.SetActive(true);
                }
                break;
        }

        UpdatePlayerHats();
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

    public void UpdatePlayerHats()
    {
        HideAllHats();

        MoveHatObjectsToSelectedSkin();

        switch (DataManager.Instance.skinsInfo_Store.activeHatType)
        {
            case HatType.None:
                break;

            case HatType.Floriel_Hat:
                if (hat_Floriel)
                    hat_Floriel.SetActive(true);
                break;
            case HatType.Granith_Hat:
                if (hat_Granith)
                    hat_Granith.SetActive(true);
                break;
            case HatType.Archie_Hat:
                if (hat_Archie)
                    hat_Archie.SetActive(true);
                break;
            case HatType.Aisa_Hat:
                if (hat_Aisa)
                    hat_Aisa.SetActive(true);
                break;
            case HatType.Mossy_Hat:
                if (hat_Mossy)
                    hat_Mossy.SetActive(true);
                break;
            case HatType.Larry_Hat:
                if (hat_Larry)
                    hat_Larry.SetActive(true);
                break;

            default:
                break;
        }
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

    public void MoveHatObjectsToSelectedSkin()
    {
        if (selectedSkinBlock)
        {
            hat_Parent.transform.SetParent(selectedSkinBlock.transform.Find("Armature_Player/Root"), true);
        }
    }
}
