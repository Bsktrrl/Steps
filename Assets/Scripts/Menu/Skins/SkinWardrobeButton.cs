using System;
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

    Player_Animations Player_Animations;
    SkinWardrobeManager skinWardrobeManager;


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
        skinWardrobeManager = FindAnyObjectByType<SkinWardrobeManager>();

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
            WardrobeSkinState tempState = skinWardrobeManager.GetSkinSaveData(region, level);

            switch (tempState)
            {
                case WardrobeSkinState.Inactive:
                    //If inactive, stay inactive
                    break;
                case WardrobeSkinState.Available:
                    //Check condition to see if button is bought
                    if (PlayerStats.Instance.stats.itemsGot.essence_Current >= skinWardrobeManager.skinCost)
                    {
                        PlayerStats.Instance.stats.itemsGot.essence_Current -= skinWardrobeManager.skinCost;
                        skinWardrobeManager.UpdateEssenceDisplay();

                        skinWardrobeManager.SetSkinSaveData(region, level, WardrobeSkinState.Bought);
                    }
                    break;
                case WardrobeSkinState.Bought:
                    //Check condition to see if button is selected
                    Action_SelectThisSkin?.Invoke();

                    skinWardrobeManager.SetActiveSkinData(skinType);

                    skinWardrobeManager.SetSkinSaveData(region, level, WardrobeSkinState.Selected);

                    skinWardrobeManager.selectedSkin = skinWardrobeManager.GetSkinSelectedObject();
                    skinWardrobeManager.MoveHatObjectsToSelectedSkin();
                    break;
                case WardrobeSkinState.Selected:
                    DeselectThisSkinButton();

                    skinWardrobeManager.SetActiveSkinData(SkinType.Default);

                    skinWardrobeManager.SetSkinSaveData(0, 0, WardrobeSkinState.Selected);

                    skinWardrobeManager.selectedSkin = skinWardrobeManager.GetSkinSelectedObject();
                    skinWardrobeManager.MoveHatObjectsToSelectedSkin();
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


            skinWardrobeManager.UpdatePlayerBodyDisplay();
        }

        //If it's a hat
        else
        {
            WardrobeHatState tempHat = skinWardrobeManager.GetHatSaveData(hatType);

            switch (tempHat)
            {
                case WardrobeHatState.Inactive:
                    //If inactive, stay inactive
                    break;
                case WardrobeHatState.Available:
                    //Check condition to see if button is selected
                    Action_SelectThisHat?.Invoke();

                    skinWardrobeManager.SetActiveHatData(hatType);

                    skinWardrobeManager.SetHatSaveData(hatType, WardrobeHatState.Selected);

                    skinWardrobeManager.selectedHat = skinWardrobeManager.GetHatSelectedObject();
                    //SkinWardrobeManager.Instance.MoveHatObjectsToSelectedSkin();
                    break;
                case WardrobeHatState.Selected:
                    DeselectThisHatButton();

                    skinWardrobeManager.SetActiveHatData(HatType.None);

                    skinWardrobeManager.SetHatSaveData(hatType, WardrobeHatState.Available);

                    skinWardrobeManager.selectedHat = skinWardrobeManager.GetHatSelectedObject();
                    //SkinWardrobeManager.Instance.MoveHatObjectsToSelectedSkin();
                    break;

                default:
                    break;
            }

            UpdateHatButtonDisplay();
            Player_Body.Instance.UpdatePlayerHats();

            skinWardrobeManager.UpdatePlayerHatDisplay();
        }

        SkinsManager.Instance.SaveData();
    }


    //--------------------


    public void UpdateSkinButtonDisplay()
    {
        if (hatType == HatType.None && skinWardrobeManager)
        {
            WardrobeSkinState tempState = skinWardrobeManager.GetSkinSaveData(region, level);

            switch (tempState)
            {
                case WardrobeSkinState.Inactive:

                    break;
                case WardrobeSkinState.Available:

                    break;
                case WardrobeSkinState.Bought:

                    break;
                case WardrobeSkinState.Selected:

                    break;

                default:
                    break;
            }
        }
    }
    public void UpdateHatButtonDisplay()
    {
        if (hatType != HatType.None && skinWardrobeManager)
        {
            WardrobeHatState tempState = skinWardrobeManager.GetHatSaveData(hatType);

            switch (tempState)
            {
                case WardrobeHatState.Inactive:

                    break;
                case WardrobeHatState.Available:

                    break;
                case WardrobeHatState.Selected:

                    break;

                default:
                    break;
            }
        }
    }

    public void UpdateDefaultSkinButtonDisplay()
    {
        if (skinType == SkinType.Default && skinWardrobeManager)
        {
            if (DataManager.Instance.skinsInfo_Store.activeSkinType == SkinType.Default)
            {

            }
            else
            {

            }
        }
    }


    //--------------------


    public void UpdateIfDefaultSkin()
    {
        if (skinWardrobeManager)
        {
            WardrobeSkinState skinState = skinWardrobeManager.GetSkinSaveData(0, 0);

            if (skinType == SkinType.Default && skinState == WardrobeSkinState.Selected)
            {
                WardrobeButton_isPressed();

                UpdateSkinButtonDisplay();
            }
        }
    }


    //--------------------


    void DeselectThisSkinButton()
    {
        if (skinWardrobeManager.GetSkinSaveData(region, level) == WardrobeSkinState.Selected)
        {
            skinWardrobeManager.SetSkinSaveData(region, level, WardrobeSkinState.Bought);
            UpdateSkinButtonDisplay();
        }
    }
    void DeselectThisHatButton()
    {
        if (skinWardrobeManager.GetHatSaveData(hatType) == WardrobeHatState.Selected)
        {
            skinWardrobeManager.SetHatSaveData(hatType, WardrobeHatState.Available);
            UpdateHatButtonDisplay();
        }
    }
}
