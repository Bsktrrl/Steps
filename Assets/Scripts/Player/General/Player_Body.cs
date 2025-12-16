using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Body : Singleton<Player_Body>
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
    public GameObject skinObject_Default;

    public GameObject skinObject_Rivergreen_Lv1;
    public GameObject skinObject_Rivergreen_Lv2;
    public GameObject skinObject_Rivergreen_Lv3;
    public GameObject skinObject_Rivergreen_Lv4;
    public GameObject skinObject_Rivergreen_Lv5;

    public GameObject skinObject_Sandlands_Lv1;
    public GameObject skinObject_Sandlands_Lv2;
    public GameObject skinObject_Sandlands_Lv3;
    public GameObject skinObject_Sandlands_Lv4;
    public GameObject skinObject_Sandlands_Lv5;

    public GameObject skinObject_Frostfield_Lv1;
    public GameObject skinObject_Frostfield_Lv2;
    public GameObject skinObject_Frostfield_Lv3;
    public GameObject skinObject_Frostfield_Lv4;
    public GameObject skinObject_Frostfield_Lv5;

    public GameObject skinObject_Firevein_Lv1;
    public GameObject skinObject_Firevein_Lv2;
    public GameObject skinObject_Firevein_Lv3;
    public GameObject skinObject_Firevein_Lv4;
    public GameObject skinObject_Firevein_Lv5;

    public GameObject skinObject_Witchmire_Lv1;
    public GameObject skinObject_Witchmire_Lv2;
    public GameObject skinObject_Witchmire_Lv3;
    public GameObject skinObject_Witchmire_Lv4;
    public GameObject skinObject_Witchmire_Lv5;

    public GameObject skinObject_Metalworks_Lv1;
    public GameObject skinObject_Metalworks_Lv2;
    public GameObject skinObject_Metalworks_Lv3;
    public GameObject skinObject_Metalworks_Lv4;
    public GameObject skinObject_Metalworks_Lv5;

    #endregion


    //--------------------


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
                if (skinObject_Default)
                {
                    selectedSkinBlock = skinObject_Default;
                    skinObject_Default.SetActive(true);
                }
                break;

            case SkinType.Rivergreen_Lv1:
                if (skinObject_Rivergreen_Lv1)
                {
                    selectedSkinBlock = skinObject_Rivergreen_Lv1;
                    skinObject_Rivergreen_Lv1.SetActive(true);
                }
                break;
            case SkinType.Rivergreen_Lv2:
                if (skinObject_Rivergreen_Lv2)
                {
                    selectedSkinBlock = skinObject_Rivergreen_Lv2;
                    skinObject_Rivergreen_Lv2.SetActive(true);
                }
                break;
            case SkinType.Rivergreen_Lv3:
                if (skinObject_Rivergreen_Lv3)
                {
                    selectedSkinBlock = skinObject_Rivergreen_Lv3;
                    skinObject_Rivergreen_Lv3.SetActive(true);
                }
                break;
            case SkinType.Rivergreen_Lv4:
                if (skinObject_Rivergreen_Lv4)
                {
                    selectedSkinBlock = skinObject_Rivergreen_Lv4;
                    skinObject_Rivergreen_Lv4.SetActive(true);
                }
                break;
            case SkinType.Rivergreen_Lv5:
                if (skinObject_Rivergreen_Lv5)
                {
                    selectedSkinBlock = skinObject_Rivergreen_Lv5;
                    skinObject_Rivergreen_Lv5.SetActive(true);
                }
                break;

            case SkinType.Firevein_Lv1:
                if (skinObject_Firevein_Lv1)
                {
                    selectedSkinBlock = skinObject_Firevein_Lv1;
                    skinObject_Firevein_Lv1.SetActive(true);
                }
                break;
            case SkinType.Firevein_Lv2:
                if (skinObject_Firevein_Lv2)
                {
                    selectedSkinBlock = skinObject_Firevein_Lv2;
                    skinObject_Firevein_Lv2.SetActive(true);
                }
                break;
            case SkinType.Firevein_Lv3:
                if (skinObject_Firevein_Lv3)
                {
                    selectedSkinBlock = skinObject_Firevein_Lv3;
                    skinObject_Firevein_Lv3.SetActive(true);
                }
                break;
            case SkinType.Firevein_Lv4:
                if (skinObject_Firevein_Lv4)
                {
                    selectedSkinBlock = skinObject_Firevein_Lv4;
                    skinObject_Firevein_Lv4.SetActive(true);
                }
                break;
            case SkinType.Firevein_Lv5:
                if (skinObject_Firevein_Lv5)
                {
                    selectedSkinBlock = skinObject_Firevein_Lv5;
                    skinObject_Firevein_Lv5.SetActive(true);
                }
                break;

            case SkinType.Sandlands_Lv1:
                if (skinObject_Sandlands_Lv1)
                {
                    selectedSkinBlock = skinObject_Sandlands_Lv1;
                    skinObject_Sandlands_Lv1.SetActive(true);
                }
                break;
            case SkinType.Sandlands_Lv2:
                if (skinObject_Sandlands_Lv2)
                {
                    selectedSkinBlock = skinObject_Sandlands_Lv2;
                    skinObject_Sandlands_Lv2.SetActive(true);
                }
                break;
            case SkinType.Sandlands_Lv3:
                if (skinObject_Sandlands_Lv3)
                {
                    selectedSkinBlock = skinObject_Sandlands_Lv3;
                    skinObject_Sandlands_Lv3.SetActive(true);
                }
                break;
            case SkinType.Sandlands_Lv4:
                if (skinObject_Sandlands_Lv4)
                {
                    selectedSkinBlock = skinObject_Sandlands_Lv4;
                    skinObject_Sandlands_Lv4.SetActive(true);
                }
                break;
            case SkinType.Sandlands_Lv5:
                if (skinObject_Sandlands_Lv5)
                {
                    selectedSkinBlock = skinObject_Sandlands_Lv5;
                    skinObject_Sandlands_Lv5.SetActive(true);
                }
                break;

            case SkinType.Frostfield_Lv1:
                if (skinObject_Frostfield_Lv1)
                {
                    selectedSkinBlock = skinObject_Frostfield_Lv1;
                    skinObject_Frostfield_Lv1.SetActive(true);
                }
                break;
            case SkinType.Frostfield_Lv2:
                if (skinObject_Frostfield_Lv2)
                {
                    selectedSkinBlock = skinObject_Frostfield_Lv2;
                    skinObject_Frostfield_Lv2.SetActive(true);
                }
                break;
            case SkinType.Frostfield_Lv3:
                if (skinObject_Frostfield_Lv3)
                {
                    selectedSkinBlock = skinObject_Frostfield_Lv3;
                    skinObject_Frostfield_Lv3.SetActive(true);
                }
                break;
            case SkinType.Frostfield_Lv4:
                if (skinObject_Frostfield_Lv4)
                {
                    selectedSkinBlock = skinObject_Frostfield_Lv4;
                    skinObject_Frostfield_Lv4.SetActive(true);
                }
                break;
            case SkinType.Frostfield_Lv5:
                if (skinObject_Frostfield_Lv5)
                {
                    selectedSkinBlock = skinObject_Frostfield_Lv5;
                    skinObject_Frostfield_Lv5.SetActive(true);
                }
                break;

            case SkinType.Witchmire_Lv1:
                if (skinObject_Witchmire_Lv1)
                {
                    selectedSkinBlock = skinObject_Witchmire_Lv1;
                    skinObject_Witchmire_Lv1.SetActive(true);
                }
                break;
            case SkinType.Witchmire_Lv2:
                if (skinObject_Witchmire_Lv2)
                {
                    selectedSkinBlock = skinObject_Witchmire_Lv2;
                    skinObject_Witchmire_Lv2.SetActive(true);
                }
                break;
            case SkinType.Witchmire_Lv3:
                if (skinObject_Witchmire_Lv3)
                {
                    selectedSkinBlock = skinObject_Witchmire_Lv3;
                    skinObject_Witchmire_Lv3.SetActive(true);
                }
                break;
            case SkinType.Witchmire_Lv4:
                if (skinObject_Witchmire_Lv4)
                {
                    selectedSkinBlock = skinObject_Witchmire_Lv4;
                    skinObject_Witchmire_Lv4.SetActive(true);
                }
                break;
            case SkinType.Witchmire_Lv5:
                if (skinObject_Witchmire_Lv5)
                {
                    selectedSkinBlock = skinObject_Witchmire_Lv5;
                    skinObject_Witchmire_Lv5.SetActive(true);
                }
                break;

            case SkinType.Metalworks_Lv1:
                if (skinObject_Metalworks_Lv1)
                {
                    selectedSkinBlock = skinObject_Metalworks_Lv1;
                    skinObject_Metalworks_Lv1.SetActive(true);
                }
                break;
            case SkinType.Metalworks_Lv2:
                if (skinObject_Metalworks_Lv2)
                {
                    selectedSkinBlock = skinObject_Metalworks_Lv2;
                    skinObject_Metalworks_Lv2.SetActive(true);
                }
                break;
            case SkinType.Metalworks_Lv3:
                if (skinObject_Metalworks_Lv3)
                {
                    selectedSkinBlock = skinObject_Metalworks_Lv3;
                    skinObject_Metalworks_Lv3.SetActive(true);
                }
                break;
            case SkinType.Metalworks_Lv4:
                if (skinObject_Metalworks_Lv4)
                {
                    selectedSkinBlock = skinObject_Metalworks_Lv4;
                    skinObject_Metalworks_Lv4.SetActive(true);
                }
                break;
            case SkinType.Metalworks_Lv5:
                if (skinObject_Metalworks_Lv5)
                {
                    selectedSkinBlock = skinObject_Metalworks_Lv5;
                    skinObject_Metalworks_Lv5.SetActive(true);
                }
                break;

            case SkinType.Default:
                if (skinObject_Default)
                {
                    selectedSkinBlock = skinObject_Default;
                    skinObject_Default.SetActive(true);
                }
                break;

            default:
                if (skinObject_Default)
                {
                    selectedSkinBlock = skinObject_Default;
                    skinObject_Default.SetActive(true);
                }
                break;
        }

        UpdatePlayerHats();
    }
    void HideAllSkins()
    {
        if (skinObject_Default)
            skinObject_Default.SetActive(false);

        if (skinObject_Rivergreen_Lv1)
            skinObject_Rivergreen_Lv1.SetActive(false);
        if (skinObject_Rivergreen_Lv2)
            skinObject_Rivergreen_Lv2.SetActive(false);
        if (skinObject_Rivergreen_Lv3)
            skinObject_Rivergreen_Lv3.SetActive(false);
        if (skinObject_Rivergreen_Lv4)
            skinObject_Rivergreen_Lv4.SetActive(false);
        if (skinObject_Rivergreen_Lv5)
            skinObject_Rivergreen_Lv5.SetActive(false);

        if (skinObject_Firevein_Lv1)
            skinObject_Firevein_Lv1.SetActive(false);
        if (skinObject_Firevein_Lv2)
            skinObject_Firevein_Lv2.SetActive(false);
        if (skinObject_Firevein_Lv3)
            skinObject_Firevein_Lv3.SetActive(false);
        if (skinObject_Firevein_Lv4)
            skinObject_Firevein_Lv4.SetActive(false);
        if (skinObject_Firevein_Lv5)
            skinObject_Firevein_Lv5.SetActive(false);

        if (skinObject_Sandlands_Lv1)
            skinObject_Sandlands_Lv1.SetActive(false);
        if (skinObject_Sandlands_Lv2)
            skinObject_Sandlands_Lv2.SetActive(false);
        if (skinObject_Sandlands_Lv3)
            skinObject_Sandlands_Lv3.SetActive(false);
        if (skinObject_Sandlands_Lv4)
            skinObject_Sandlands_Lv4.SetActive(false);
        if (skinObject_Sandlands_Lv5)
            skinObject_Sandlands_Lv5.SetActive(false);

        if (skinObject_Frostfield_Lv1)
            skinObject_Frostfield_Lv1.SetActive(false);
        if (skinObject_Frostfield_Lv2)
            skinObject_Frostfield_Lv2.SetActive(false);
        if (skinObject_Frostfield_Lv3)
            skinObject_Frostfield_Lv3.SetActive(false);
        if (skinObject_Frostfield_Lv4)
            skinObject_Frostfield_Lv4.SetActive(false);
        if (skinObject_Frostfield_Lv5)
            skinObject_Frostfield_Lv5.SetActive(false);

        if (skinObject_Witchmire_Lv1)
            skinObject_Witchmire_Lv1.SetActive(false);
        if (skinObject_Witchmire_Lv2)
            skinObject_Witchmire_Lv2.SetActive(false);
        if (skinObject_Witchmire_Lv3)
            skinObject_Witchmire_Lv3.SetActive(false);
        if (skinObject_Witchmire_Lv4)
            skinObject_Witchmire_Lv4.SetActive(false);
        if (skinObject_Witchmire_Lv5)
            skinObject_Witchmire_Lv5.SetActive(false);

        if (skinObject_Metalworks_Lv1)
            skinObject_Metalworks_Lv1.SetActive(false);
        if (skinObject_Metalworks_Lv2)
            skinObject_Metalworks_Lv2.SetActive(false);
        if (skinObject_Metalworks_Lv3)
            skinObject_Metalworks_Lv3.SetActive(false);
        if (skinObject_Metalworks_Lv4)
            skinObject_Metalworks_Lv4.SetActive(false);
        if (skinObject_Metalworks_Lv5)
            skinObject_Metalworks_Lv5.SetActive(false);
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
