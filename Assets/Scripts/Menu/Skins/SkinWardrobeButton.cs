using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


    //--------------------


    private void OnEnable()
    {
        Action_SelectThisSkin += DeselectThisSkinButton;
        Action_SelectThisHat += DeselectThisHatButton;

        UpdateWardrobeButtonDisplay();
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

            SkinsManager.Instance.skinInfo.activeSkinType = skinType;

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

                    SkinWardrobeManager.Instance.SetSkinSaveData(region, level, WardrobeSkinState.Selected);

                    SkinWardrobeManager.Instance.selectedSkin = SkinWardrobeManager.Instance.GetSkinSelectedObject();
                    SkinWardrobeManager.Instance.MoveHatObjectsToSelectedSkin();
                    break;
                case WardrobeSkinState.Selected:
                    DeselectThisSkinButton();

                    SkinWardrobeManager.Instance.SetSkinSaveData(0, 0, WardrobeSkinState.Selected);

                    SkinWardrobeManager.Instance.selectedSkin = SkinWardrobeManager.Instance.GetSkinSelectedObject();
                    SkinWardrobeManager.Instance.MoveHatObjectsToSelectedSkin();
                    break;

                default:
                    break;
            }

            UpdateWardrobeButtonDisplay();

            SkinWardrobeManager.Instance.UpdatePlayerBodyDisplay();
        }

        //If it's a hat
        else
        {
            WardrobeHatState tempHat = SkinWardrobeManager.Instance.GetHatSaveData(hatType);

            SkinsManager.Instance.skinInfo.activeHatType = hatType;

            switch (tempHat)
            {
                case WardrobeHatState.Inactive:
                    //If inactive, stay inactive
                    break;
                case WardrobeHatState.Available:
                    //Check condition to see if button is selected
                    Action_SelectThisHat?.Invoke();

                    SkinWardrobeManager.Instance.SetHatSaveData(hatType, WardrobeHatState.Selected);

                    SkinWardrobeManager.Instance.MoveHatObjectsToSelectedSkin();
                    break;
                case WardrobeHatState.Selected:
                    DeselectThisHatButton();

                    SkinWardrobeManager.Instance.SetHatSaveData(hatType, WardrobeHatState.Selected);
                    break;

                default:
                    break;
            }

            UpdateHatButtonDisplay();

            SkinWardrobeManager.Instance.UpdatePlayerHatDisplay();
        }

        SkinsManager.Instance.SaveData();
    }


    //--------------------


    public void UpdateWardrobeButtonDisplay()
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
    public void UpdateHatButtonDisplay()
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


    //--------------------


    public void UpdateIfDefaultSkin()
    {
        WardrobeSkinState skinState = SkinWardrobeManager.Instance.GetSkinSaveData(0, 0);

        if (skinType == SkinType.Default && skinState == WardrobeSkinState.Selected)
        {
            WardrobeButton_isPressed();

            UpdateWardrobeButtonDisplay();
        }
    }


    //--------------------


    void DeselectThisSkinButton()
    {
        if (SkinWardrobeManager.Instance.GetSkinSaveData(region, level) == WardrobeSkinState.Selected)
        {
            SkinWardrobeManager.Instance.SetSkinSaveData(region, level, WardrobeSkinState.Bought);
            UpdateWardrobeButtonDisplay();
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
