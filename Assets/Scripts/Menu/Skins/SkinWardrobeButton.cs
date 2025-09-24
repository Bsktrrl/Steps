using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SkinWardrobeButton : MonoBehaviour
{
    public static event Action Action_SelectThisSkin;
    public static event Action Action_SelectThisHat;

    [Header("General")]
    public SkinType skinType;
    public HatType hatType;
    [SerializeField] int region;
    [SerializeField] int level;

    [Header("Components")]
    [SerializeField] Image frame;
    [SerializeField] GameObject overlay;

    Player_Animations Player_Animations;


    //--------------------


    private void Start()
    {
        Player_Animations = FindAnyObjectByType<Player_Animations>();
    }
    private void Update()
    {
        UpdateDefaultSkinButtonDisplay();
    }
    private void OnEnable()
    {
        Action_SelectThisSkin += DeselectThisSkinButton;
        Action_SelectThisHat += DeselectThisHatButton;

        UpdateSkinButtonDisplay();
        UpdateHatButtonDisplay();

        UpdateIfDefaultSkin();
    }
    private void OnDisable()
    {
        Action_SelectThisSkin -= DeselectThisSkinButton;
        Action_SelectThisHat -= DeselectThisHatButton;
    }


    //--------------------


    public void WardrobeButton_isPressed()
    {
        //If it's a block
        if (hatType == HatType.None)
        {
            WardrobeSkinState tempState = SkinWardrobeManager.Instance.GetSkinSaveData(region, level);

            switch (tempState)
            {
                case WardrobeSkinState.Inactive:
                    //If inactive, stay inactive
                    break;
                case WardrobeSkinState.Available:
                    //Check condition to see if button is bought
                    if (PlayerStats.Instance.stats.itemsGot.essence_Current >= SkinWardrobeManager.Instance.skinCost)
                    {
                        PlayerStats.Instance.stats.itemsGot.essence_Current -= SkinWardrobeManager.Instance.skinCost;
                        SkinWardrobeManager.Instance.UpdateEssenceDisplay();

                        SkinWardrobeManager.Instance.SetSkinSaveData(region, level, WardrobeSkinState.Bought);
                    }
                    break;
                case WardrobeSkinState.Bought:
                    //Check condition to see if button is selected
                    Action_SelectThisSkin?.Invoke();

                    SkinWardrobeManager.Instance.SetActiveSkinData(skinType);

                    SkinWardrobeManager.Instance.SetSkinSaveData(region, level, WardrobeSkinState.Selected);

                    SkinWardrobeManager.Instance.selectedSkin = SkinWardrobeManager.Instance.GetSkinSelectedObject();
                    SkinWardrobeManager.Instance.MoveHatObjectsToSelectedSkin();
                    break;
                case WardrobeSkinState.Selected:
                    DeselectThisSkinButton();

                    SkinWardrobeManager.Instance.SetActiveSkinData(SkinType.Default);

                    SkinWardrobeManager.Instance.SetSkinSaveData(0, 0, WardrobeSkinState.Selected);

                    SkinWardrobeManager.Instance.selectedSkin = SkinWardrobeManager.Instance.GetSkinSelectedObject();
                    SkinWardrobeManager.Instance.MoveHatObjectsToSelectedSkin();
                    break;

                default:
                    break;
            }

            UpdateSkinButtonDisplay();

            Player_Body.Instance.UpdatePlayerSkin();

            if (Player_Animations)
            {
                Player_Animations.Instance.UpdateAnimator();
            }
            

            SkinWardrobeManager.Instance.UpdatePlayerBodyDisplay();
        }

        //If it's a hat
        else
        {
            WardrobeHatState tempHat = SkinWardrobeManager.Instance.GetHatSaveData(hatType);

            switch (tempHat)
            {
                case WardrobeHatState.Inactive:
                    //If inactive, stay inactive
                    break;
                case WardrobeHatState.Available:
                    //Check condition to see if button is selected
                    Action_SelectThisHat?.Invoke();

                    SkinWardrobeManager.Instance.SetActiveHatData(hatType);

                    SkinWardrobeManager.Instance.SetHatSaveData(hatType, WardrobeHatState.Selected);

                    SkinWardrobeManager.Instance.selectedHat = SkinWardrobeManager.Instance.GetHatSelectedObject();
                    //SkinWardrobeManager.Instance.MoveHatObjectsToSelectedSkin();
                    break;
                case WardrobeHatState.Selected:
                    DeselectThisHatButton();

                    SkinWardrobeManager.Instance.SetActiveHatData(HatType.None);

                    SkinWardrobeManager.Instance.SetHatSaveData(hatType, WardrobeHatState.Available);

                    SkinWardrobeManager.Instance.selectedHat = SkinWardrobeManager.Instance.GetHatSelectedObject();
                    //SkinWardrobeManager.Instance.MoveHatObjectsToSelectedSkin();
                    break;

                default:
                    break;
            }

            UpdateHatButtonDisplay();
            Player_Body.Instance.UpdatePlayerHats();

            SkinWardrobeManager.Instance.UpdatePlayerHatDisplay();
        }

        SkinsManager.Instance.SaveData();
    }


    //--------------------


    public void UpdateSkinButtonDisplay()
    {
        if (hatType == HatType.None)
        {
            WardrobeSkinState tempState = SkinWardrobeManager.Instance.GetSkinSaveData(region, level);
            switch (tempState)
            {
                case WardrobeSkinState.Inactive:
                    overlay.SetActive(true);
                    frame.color = SkinWardrobeManager.Instance.inactive_Color;
                    break;
                case WardrobeSkinState.Available:
                    overlay.SetActive(false);
                    frame.color = SkinWardrobeManager.Instance.available_Color;
                    break;
                case WardrobeSkinState.Bought:
                    overlay.SetActive(false);
                    frame.color = SkinWardrobeManager.Instance.bought_Color;
                    break;
                case WardrobeSkinState.Selected:
                    overlay.SetActive(false);
                    frame.color = SkinWardrobeManager.Instance.selected_Color;
                    break;

                default:
                    break;
            }
        }
    }
    public void UpdateDefaultSkinButtonDisplay()
    {
        if (skinType == SkinType.Default)
        {
            if (DataManager.Instance.skinsInfo_Store.activeSkinType == SkinType.Default)
            {
                overlay.SetActive(false);
                frame.color = SkinWardrobeManager.Instance.selected_Color;
            }
            else
            {
                overlay.SetActive(false);
                frame.color = SkinWardrobeManager.Instance.bought_Color;
            }
        }
    }

    public void UpdateHatButtonDisplay()
    {
        if (hatType != HatType.None)
        {
            WardrobeHatState tempState = SkinWardrobeManager.Instance.GetHatSaveData(hatType);

            switch (tempState)
            {
                case WardrobeHatState.Inactive:
                    overlay.SetActive(true);
                    frame.color = SkinWardrobeManager.Instance.inactive_Color;
                    break;
                case WardrobeHatState.Available:
                    overlay.SetActive(false);
                    frame.color = SkinWardrobeManager.Instance.bought_Color;
                    break;
                case WardrobeHatState.Selected:
                    overlay.SetActive(false);
                    frame.color = SkinWardrobeManager.Instance.selected_Color;
                    break;

                default:
                    break;
            }
        }
    }


    //--------------------


    public void UpdateIfDefaultSkin()
    {
        WardrobeSkinState skinState = SkinWardrobeManager.Instance.GetSkinSaveData(0, 0);

        if (skinType == SkinType.Default && skinState == WardrobeSkinState.Selected)
        {
            WardrobeButton_isPressed();

            UpdateSkinButtonDisplay();
        }
    }


    //--------------------


    void DeselectThisSkinButton()
    {
        if (SkinWardrobeManager.Instance.GetSkinSaveData(region, level) == WardrobeSkinState.Selected)
        {
            SkinWardrobeManager.Instance.SetSkinSaveData(region, level, WardrobeSkinState.Bought);
            UpdateSkinButtonDisplay();
        }
    }
    void DeselectThisHatButton()
    {
        if (SkinWardrobeManager.Instance.GetHatSaveData(hatType) == WardrobeHatState.Selected)
        {
            SkinWardrobeManager.Instance.SetHatSaveData(hatType, WardrobeHatState.Available);
            UpdateHatButtonDisplay();
        }
    }
}
