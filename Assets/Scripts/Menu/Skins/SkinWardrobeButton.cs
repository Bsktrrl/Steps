using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinWardrobeButton : MonoBehaviour
{
    public static event Action Action_SelectThisSkin;

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
        Action_SelectThisSkin += DeselectThisButton;

        UpdateButtonDisplay();
        UpdateIfDefaultSkin();
    }
    private void OnDisable()
    {
        Action_SelectThisSkin -= DeselectThisButton;
    }


    //--------------------


    public void WardrobeButton_isPressed()
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
                break;
            case WardrobeSkinState.Selected:
                break;

            default:
                break;
        }

        UpdateButtonDisplay();

        SkinWardrobeManager.Instance.UpdatePlayerBodyDisplay();

        SkinsManager.Instance.SaveData();
    }


    //--------------------


    public void UpdateButtonDisplay()
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


    //--------------------


    public void UpdateIfDefaultSkin()
    {
        if (SkinWardrobeManager.Instance.GetSkinSaveData(0, 0) == WardrobeSkinState.Inactive && hatType == HatType.None)
        {
            SkinWardrobeManager.Instance.SetSkinSaveData(0, 0, WardrobeSkinState.Selected);
        }
    }


    //--------------------


    void DeselectThisButton()
    {
        if (SkinWardrobeManager.Instance.GetSkinSaveData(region, level) == WardrobeSkinState.Selected)
        {
            SkinWardrobeManager.Instance.SetSkinSaveData(region, level, WardrobeSkinState.Bought);
            UpdateButtonDisplay();
        }
    }
}
