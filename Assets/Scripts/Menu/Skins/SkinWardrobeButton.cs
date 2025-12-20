using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkinWardrobeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    public static event Action Action_SelectThisSkin;
    public static event Action Action_SelectThisHat;

    public static event Action Action_BuySkin;

    [Header("General")]
    public SkinType skinType;
    public HatType hatType;
    [SerializeField] RegionName region;
    [SerializeField] int level;

    Player_Animations Player_Animations;
    SkinWardrobeManager skinWardrobeManager;

    [SerializeField] Image backgroundImage;
    [SerializeField] Image skinImage;
    [SerializeField] TextMeshProUGUI UnknownText;

    UnlockDisplay unlockDisplay;


    //--------------------


    private void Awake()
    {
        unlockDisplay = FindObjectOfType<UnlockDisplay>();
    }
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
        Action_BuySkin += UpdateIfSkinIsAvailable;
        Action_BuySkin += SetActive;

        UpdateIfSkinIsAvailable();

        CheckIfSkinIsFound();

        UpdateSkinButtonDisplay();
        UpdateHatButtonDisplay();

        UpdateIfDefaultSkin();
    }
    private void OnDisable()
    {
        Action_SelectThisSkin -= DeselectThisSkinButton;
        Action_SelectThisHat -= DeselectThisHatButton;
        Action_BuySkin -= UpdateIfSkinIsAvailable;
        Action_BuySkin -= SetActive;
    }


    //--------------------

    void CheckIfSkinIsFound()
    {
        if (SkinWardrobeManager.Instance.GetLevelSaveData(skinType))
        {
            if (SkinWardrobeManager.Instance.GetSkinSaveData(GetRegionNumber(region), level) == WardrobeSkinState.LevelIsVisited
                || SkinWardrobeManager.Instance.GetSkinSaveData(GetRegionNumber(region), level) == WardrobeSkinState.Available
                || SkinWardrobeManager.Instance.GetSkinSaveData(GetRegionNumber(region), level) == WardrobeSkinState.Bought
                || SkinWardrobeManager.Instance.GetSkinSaveData(GetRegionNumber(region), level) == WardrobeSkinState.Selected)
            {

            }
            else
            {
                SkinWardrobeManager.Instance.SetSkinSaveData(GetRegionNumber(region), level, WardrobeSkinState.LevelIsVisited);
            }
        }
    }
    public void WardrobeButton_isPressed()
    {
        //If it's a block
        if (hatType == HatType.None)
        {
            WardrobeSkinState tempState = skinWardrobeManager.GetSkinSaveData(GetRegionNumber(region), level);

            switch (tempState)
            {
                case WardrobeSkinState.Hidden:
                    //If inactive, stay inactive
                    break;
                case WardrobeSkinState.LevelIsVisited:
                    //Nothing is happening if only the level is visited, but player don't have enough essence
                    break;
                case WardrobeSkinState.Available:
                    //Check condition to see if button is bought
                    if (PlayerStats.Instance.stats.itemsGot.essence_Current >= skinWardrobeManager.skinCost)
                    {
                        PlayerStats.Instance.stats.itemsGot.essence_Current -= skinWardrobeManager.skinCost;
                        skinWardrobeManager.UpdateEssenceDisplay();

                        skinWardrobeManager.SetSkinSaveData(GetRegionNumber(region), level, WardrobeSkinState.Bought);

                        Action_BuySkin?.Invoke();
                    }
                    break;
                case WardrobeSkinState.Bought:
                    //Check condition to see if button is selected
                    Action_SelectThisSkin?.Invoke();

                    skinWardrobeManager.SetActiveSkinData(skinType);

                    skinWardrobeManager.SetSkinSaveData(GetRegionNumber(region), level, WardrobeSkinState.Selected);

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
                case WardrobeHatState.Hidden:
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

        SetActive();

        SkinsManager.Instance.SaveData();
    }


    //--------------------


    public void UpdateSkinButtonDisplay()
    {
        skinImage.gameObject.SetActive(true);
        UnknownText.gameObject.SetActive(false);

        //Based on SaveData
        if (skinWardrobeManager && hatType == HatType.None)
        {
            WardrobeSkinState tempState = skinWardrobeManager.GetSkinSaveData(GetRegionNumber(region), level);

            switch (tempState)
            {
                case WardrobeSkinState.Hidden:
                    backgroundImage.sprite = SkinWardrobeManager.Instance.sprite_Inactive;
                    break;
                case WardrobeSkinState.LevelIsVisited:
                    backgroundImage.sprite = SkinWardrobeManager.Instance.sprite_Inactive;
                    break;
                case WardrobeSkinState.Available:
                    backgroundImage.sprite = SkinWardrobeManager.Instance.sprite_Available;
                    break;
                case WardrobeSkinState.Bought:
                    backgroundImage.sprite = SkinWardrobeManager.Instance.sprite_Bought;
                    break;
                case WardrobeSkinState.Selected:
                    backgroundImage.sprite = SkinWardrobeManager.Instance.sprite_Selected;
                    break;

                default:
                    break;
            }
        }

        //Based on Level Discovered
        if (!skinWardrobeManager.GetLevelSaveData(skinType) && skinType != SkinType.Default && hatType == HatType.None)
        {
            backgroundImage.sprite = SkinWardrobeManager.Instance.sprite_Inactive;
            skinImage.gameObject.SetActive(false);
            UnknownText.gameObject.SetActive(true);
        }
    }
    public void UpdateHatButtonDisplay()
    {
        if (hatType != HatType.None && skinWardrobeManager)
        {
            WardrobeHatState tempState = skinWardrobeManager.GetHatSaveData(hatType);

            switch (tempState)
            {
                case WardrobeHatState.Hidden:
                    backgroundImage.sprite = SkinWardrobeManager.Instance.sprite_Inactive;
                    break;
                case WardrobeHatState.Available:
                    backgroundImage.sprite = SkinWardrobeManager.Instance.sprite_Available;
                    break;
                case WardrobeHatState.Selected:
                    backgroundImage.sprite = SkinWardrobeManager.Instance.sprite_Selected;
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
                backgroundImage.sprite = SkinWardrobeManager.Instance.sprite_Selected;
            }
            else
            {
                backgroundImage.sprite = SkinWardrobeManager.Instance.sprite_Bought;
            }
        }
    }
    public void UpdateIfSkinIsAvailable()
    {
        if (SkinWardrobeManager.Instance.GetSkinSaveData(GetRegionNumber(region), level) == WardrobeSkinState.LevelIsVisited && PlayerStats.Instance.stats.itemsGot.essence_Current >= 10)
        {
            //SkinWardrobeManager.Instance.SetSkinSaveData(GetRegionNumber(region), level, WardrobeSkinState.Available);
        }
        else if (SkinWardrobeManager.Instance.GetSkinSaveData(GetRegionNumber(region), level) == WardrobeSkinState.Available && PlayerStats.Instance.stats.itemsGot.essence_Current < 10)
        {
            //SkinWardrobeManager.Instance.SetSkinSaveData(GetRegionNumber(region), level, WardrobeSkinState.LevelIsVisited);
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
        if (skinWardrobeManager.GetSkinSaveData(GetRegionNumber(region), level) == WardrobeSkinState.Selected)
        {
            skinWardrobeManager.SetSkinSaveData(GetRegionNumber(region), level, WardrobeSkinState.Bought);
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


    //--------------------


    int GetRegionNumber(RegionName regionName)
    {
        switch (regionName)
        {
            case RegionName.None:
                return 0;

            case RegionName.Rivergreen:
                return 1;
            case RegionName.Sandlands:
                return 2;
            case RegionName.Frostfields:
                return 3;
            case RegionName.Firevein:
                return 4;
            case RegionName.Witchmire:
                return 5;
            case RegionName.Metalworks:
                return 6;

            default:
                return 0;
        }
    }


    //--------------------

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Highlighted
        SetActive();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // Normal (mouse left)
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // Pressed
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // Released (back to Highlighted if still hovered)
        SetActive();
    }
    public void OnSelect(BaseEventData eventData)
    {
        // Selected (keyboard/controller)
        SetActive();
    }
    public void OnDeselect(BaseEventData eventData)
    {

    }

    void SetActive()
    {
        WardrobeSkinState tempState = skinWardrobeManager.GetSkinSaveData(GetRegionNumber(region), level);

        switch (tempState)
        {
            case WardrobeSkinState.Hidden:
                unlockDisplay.SetDisplay_Unavailable(region, level.ToString());
                break;
            case WardrobeSkinState.LevelIsVisited:
                unlockDisplay.SetDisplay_LevelReached();
                break;
            case WardrobeSkinState.Available:
                if (PlayerStats.Instance.stats.itemsGot.essence_Current >= 10)
                    unlockDisplay.SetDisplay_CanUnlock();
                else
                    unlockDisplay.SetDisplay_CanNotUnlock();
                break;
            case WardrobeSkinState.Bought:
                unlockDisplay.SetDisplay_CanEquip();
                break;
            case WardrobeSkinState.Selected:
                unlockDisplay.SetDisplay_IsEquipped();
                break;

            default:
                unlockDisplay.SetDisplay_Unavailable(region, level.ToString());
                break;
        }
    }
}

