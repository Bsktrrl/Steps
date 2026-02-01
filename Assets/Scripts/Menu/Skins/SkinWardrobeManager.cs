using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SkinWardrobeManager : Singleton<SkinWardrobeManager>
{
    [Header("Wardrobe Parent")]
    public GameObject wardrobeParent;
    public GameObject wardrobeMenuButton;

    [Header("Player Model")]
    [SerializeField] GameObject playerDisplayObject;

    [Header("Colors")]
    public Sprite sprite_Inactive;
    public Sprite sprite_Available;
    public Sprite sprite_Bought;
    public Sprite sprite_Selected;

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
    public string sprite_PauseMenu_default_Name;
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

    public SkinType selectedSkinType;
    public HatType selectedHatType;
    public SkinType equippedSkinType;


    //--------------------


    private void Start()
    {
        movement = FindObjectOfType<Movement>();
        selectedSkinType = SkinType.Default;
    }

    private void Update()
    {
        //Hat_HatUpdate();
    }

    private void OnEnable()
    {
        UpdateStartButton();
        UpdateEssenceDisplay();

        UpdatePlayerBodyDisplay();
        UpdatePlayerHatDisplay();

        if (skinWardrobeButton_Default && skinWardrobeButton_Default.GetComponent<UI_PulsatingMotion_WhenSelected>())
        {
            skinWardrobeButton_Default.GetComponent<UI_PulsatingMotion_WhenSelected>().EnableStartButton();
        }

        DataManager.Action_dataHasLoaded += UpdateStartButton;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= UpdateStartButton;
    }


    //--------------------


    void UpdateStartButton()
    {
        RemoveStartButton();

        switch (SkinsManager.Instance.skinInfo.activeSkinType)
        {
            case SkinType.None:
                SetStartButton(skinWardrobeButton_Default);
                break;

            case SkinType.Default:
                SetStartButton(skinWardrobeButton_Default);
                break;

            case SkinType.Rivergreen_Lv1:
                SetStartButton(skinWardrobeButton_Region1_Level1);
                break;
            case SkinType.Rivergreen_Lv2:
                SetStartButton(skinWardrobeButton_Region1_Level2);
                break;
            case SkinType.Rivergreen_Lv3:
                SetStartButton(skinWardrobeButton_Region1_Level3);
                break;
            case SkinType.Rivergreen_Lv4:
                SetStartButton(skinWardrobeButton_Region1_Level4);
                break;
            case SkinType.Rivergreen_Lv5:
                SetStartButton(skinWardrobeButton_Region1_Level5);
                break;

            case SkinType.Sandlands_Lv1:
                SetStartButton(skinWardrobeButton_Region2_Level1);
                break;
            case SkinType.Sandlands_Lv2:
                SetStartButton(skinWardrobeButton_Region2_Level2);
                break;
            case SkinType.Sandlands_Lv3:
                SetStartButton(skinWardrobeButton_Region2_Level3);
                break;
            case SkinType.Sandlands_Lv4:
                SetStartButton(skinWardrobeButton_Region2_Level4);
                break;
            case SkinType.Sandlands_Lv5:
                SetStartButton(skinWardrobeButton_Region2_Level5);
                break;

            case SkinType.Frostfield_Lv1:
                SetStartButton(skinWardrobeButton_Region3_Level1);
                break;
            case SkinType.Frostfield_Lv2:
                SetStartButton(skinWardrobeButton_Region3_Level2);
                break;
            case SkinType.Frostfield_Lv3:
                SetStartButton(skinWardrobeButton_Region3_Level3);
                break;
            case SkinType.Frostfield_Lv4:
                SetStartButton(skinWardrobeButton_Region3_Level4);
                break;
            case SkinType.Frostfield_Lv5:
                SetStartButton(skinWardrobeButton_Region3_Level5);
                break;

            case SkinType.Firevein_Lv1:
                SetStartButton(skinWardrobeButton_Region4_Level1);
                break;
            case SkinType.Firevein_Lv2:
                SetStartButton(skinWardrobeButton_Region4_Level2);
                break;
            case SkinType.Firevein_Lv3:
                SetStartButton(skinWardrobeButton_Region4_Level3);
                break;
            case SkinType.Firevein_Lv4:
                SetStartButton(skinWardrobeButton_Region4_Level4);
                break;
            case SkinType.Firevein_Lv5:
                SetStartButton(skinWardrobeButton_Region4_Level5);
                break;

            case SkinType.Witchmire_Lv1:
                SetStartButton(skinWardrobeButton_Region5_Level1);
                break;
            case SkinType.Witchmire_Lv2:
                SetStartButton(skinWardrobeButton_Region5_Level2);
                break;
            case SkinType.Witchmire_Lv3:
                SetStartButton(skinWardrobeButton_Region5_Level3);
                break;
            case SkinType.Witchmire_Lv4:
                SetStartButton(skinWardrobeButton_Region5_Level4);
                break;
            case SkinType.Witchmire_Lv5:
                SetStartButton(skinWardrobeButton_Region5_Level5);
                break;

            case SkinType.Metalworks_Lv1:
                SetStartButton(skinWardrobeButton_Region6_Level1);
                break;
            case SkinType.Metalworks_Lv2:
                SetStartButton(skinWardrobeButton_Region6_Level2);
                break;
            case SkinType.Metalworks_Lv3:
                SetStartButton(skinWardrobeButton_Region6_Level3);
                break;
            case SkinType.Metalworks_Lv4:
                SetStartButton(skinWardrobeButton_Region6_Level4);
                break;
            case SkinType.Metalworks_Lv5:
                SetStartButton(skinWardrobeButton_Region6_Level5);
                break;

            default:
                SetStartButton(skinWardrobeButton_Default);
                break;
        }
    }
    void RemoveStartButton()
    {
        skinWardrobeButton_Region1_Level1.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region1_Level2.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region1_Level3.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region1_Level4.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region1_Level5.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;

        skinWardrobeButton_Region2_Level1.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region2_Level2.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region2_Level3.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region2_Level4.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region2_Level5.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;

        skinWardrobeButton_Region3_Level1.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region3_Level2.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region3_Level3.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region3_Level4.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region3_Level5.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;

        skinWardrobeButton_Region4_Level1.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region4_Level2.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region4_Level3.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region4_Level4.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region4_Level5.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;

        skinWardrobeButton_Region5_Level1.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region5_Level2.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region5_Level3.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region5_Level4.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region5_Level5.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;

        skinWardrobeButton_Region6_Level1.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region6_Level2.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region6_Level3.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region6_Level4.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;
        skinWardrobeButton_Region6_Level5.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = false;

    }
    void SetStartButton(GameObject pulsatingScriptObj)
    {
        //ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(pulsatingScriptObj);
        wardrobeMenuButton.GetComponent<Button_ToPress>().uiElementToSelect = pulsatingScriptObj.GetComponent<Button>();
        pulsatingScriptObj.GetComponent<UI_PulsatingMotion_WhenSelected>().isStartButton = true;
    }


    //--------------------


    public void UpdatePlayerBodyDisplay()
    {
        //print("1. UpdatePlayerBodyDisplay: " + DataManager.Instance.skinsInfo_Store.activeSkinType);

        HideAllSkins();

        GameObject tempObj = GetTempSkinSelectedObject();

        if (tempObj != null)
            tempObj.SetActive(true);

        if (selectedSkinType != SkinType.Default /*&& selectedSkinType != SkinType.None*/ /*DataManager.Instance.skinsInfo_Store.activeSkinType != SkinType.Default && DataManager.Instance.skinsInfo_Store.activeSkinType != SkinType.None*/)
        {
            if (skin_Default)
                skin_Default.SetActive(false);
        }

        Hat_HatUpdate();
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
        HideAllSkins();
        HideAllHats();

        //Skin
        Skin_HatUpdate();

        //Hat
        Hat_HatUpdate();
    }
    void Skin_HatUpdate()
    {
        GameObject tempSkinObj = GetEquippedSkinSelectedObject();

        if (tempSkinObj != null)
        {
            tempSkinObj.SetActive(true);
        }
    }
    public void Hat_HatUpdate()
    {
        GameObject tempHatObj = GetHatSelectedObject();

        if (tempHatObj != null)
        {
            print("200. ShowHats");
            MoveHatObjectsToSelectedSkin();
            tempHatObj.SetActive(true);
        }
    }
    public void HideAllHats()
    {
        print("100. HideAllHats");
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

    public GameObject GetTempSkinSelectedObject()
    {
        switch (selectedSkinType)
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
    public GameObject GetEquippedSkinSelectedObject()
    {
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
    public GameObject GetTempHatSelectedObject()
    {
        switch (selectedHatType)
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
        if (DataManager.Instance.skinsInfo_Store.skinWardrobeInfo == null) return WardrobeSkinState.Hidden;

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

                    default:
                        return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default;
                }

            default:
                return DataManager.Instance.skinsInfo_Store.skinWardrobeInfo.skin_Default;
        }
    }
    public bool GetLevelSaveData(SkinType skinType)
    {
        if (DataManager.Instance.mapInfo_StoreList == null) return false;

        bool isLevel = false;

        for (int i = 0; i < DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List.Count; i++)
        {
            if (DataManager.Instance.mapInfo_StoreList.map_SaveInfo_List[i].skintype == skinType)
            {
                isLevel = true;
            }
        }

        if (isLevel)
            return true;
        else
            return false;
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

        selectedSkinType = skintype;

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
        if (DataManager.Instance.skinsInfo_Store.skinHatInfo == null) return WardrobeHatState.Hidden;

        switch (hatType)
        {
            case HatType.None:
                return WardrobeHatState.Hidden;

            case HatType.Floriel_Hat:
                return DataManager.Instance.skinsInfo_Store.skinHatInfo.hat_Region1;
            case HatType.Granith_Hat:
                return DataManager.Instance.skinsInfo_Store.skinHatInfo.hat_Region2;
            case HatType.Archie_Hat:
                return DataManager.Instance.skinsInfo_Store.skinHatInfo.hat_Region3;
            case HatType.Aisa_Hat:
                return DataManager.Instance.skinsInfo_Store.skinHatInfo.hat_Region4;
            case HatType.Mossy_Hat:
                return DataManager.Instance.skinsInfo_Store.skinHatInfo.hat_Region5;
            case HatType.Larry_Hat:
                return DataManager.Instance.skinsInfo_Store.skinHatInfo.hat_Region6;

            default:
                return WardrobeHatState.Hidden;
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
                DataManager.Instance.skinsInfo_Store.skinHatInfo.hat_Region1 = hatState;
                break;
            case HatType.Granith_Hat:
                DataManager.Instance.skinsInfo_Store.skinHatInfo.hat_Region2 = hatState;
                break;
            case HatType.Archie_Hat:
                DataManager.Instance.skinsInfo_Store.skinHatInfo.hat_Region3 = hatState;
                break;
            case HatType.Aisa_Hat:
                DataManager.Instance.skinsInfo_Store.skinHatInfo.hat_Region4 = hatState;
                break;
            case HatType.Mossy_Hat:
                DataManager.Instance.skinsInfo_Store.skinHatInfo.hat_Region5 = hatState;
                break;
            case HatType.Larry_Hat:
                DataManager.Instance.skinsInfo_Store.skinHatInfo.hat_Region6 = hatState;
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
        esseceCost.text = DataManager.Instance.playerStats_Store.itemsGot.essence_Current.ToString();
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


    [Header("Region 2")]
    public WardrobeSkinState skin_Region2_level1;
    public WardrobeSkinState skin_Region2_level2;
    public WardrobeSkinState skin_Region2_level3;
    public WardrobeSkinState skin_Region2_level4;
    public WardrobeSkinState skin_Region2_level5;

    [Header("Region 3")]
    public WardrobeSkinState skin_Region3_level1;
    public WardrobeSkinState skin_Region3_level2;
    public WardrobeSkinState skin_Region3_level3;
    public WardrobeSkinState skin_Region3_level4;
    public WardrobeSkinState skin_Region3_level5;

    [Header("Region 4")]
    public WardrobeSkinState skin_Region4_level1;
    public WardrobeSkinState skin_Region4_level2;
    public WardrobeSkinState skin_Region4_level3;
    public WardrobeSkinState skin_Region4_level4;
    public WardrobeSkinState skin_Region4_level5;

    [Header("Region 5")]
    public WardrobeSkinState skin_Region5_level1;
    public WardrobeSkinState skin_Region5_level2;
    public WardrobeSkinState skin_Region5_level3;
    public WardrobeSkinState skin_Region5_level4;
    public WardrobeSkinState skin_Region5_level5;

    [Header("Region 6")]
    public WardrobeSkinState skin_Region6_level1;
    public WardrobeSkinState skin_Region6_level2;
    public WardrobeSkinState skin_Region6_level3;
    public WardrobeSkinState skin_Region6_level4;
    public WardrobeSkinState skin_Region6_level5;

    [Header("Default")]
    public WardrobeSkinState skin_Default;
}

[Serializable]
public class SkinsHatInfo
{
    [Header("Hats")]
    public WardrobeHatState hat_Region1;
    public WardrobeHatState hat_Region2;
    public WardrobeHatState hat_Region3;
    public WardrobeHatState hat_Region4;
    public WardrobeHatState hat_Region5;
    public WardrobeHatState hat_Region6;

    [Header("Hats")]
    public bool hat_Region1_Version;
    public bool hat_Region2_Version;
    public bool hat_Region3_Version;
    public bool hat_Region4_Version;
    public bool hat_Region5_Version;
    public bool hat_Region6_Version;
}

public enum WardrobeSkinState
{
    Hidden,
    LevelIsVisited,
    Available,
    Bought,
    Selected
}
public enum WardrobeHatState
{
    Hidden,
    Available,
    Selected
}
