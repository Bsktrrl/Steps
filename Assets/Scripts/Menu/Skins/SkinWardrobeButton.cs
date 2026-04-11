using System;
using System.Collections;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkinWardrobeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    public static event Action Action_ButtonIsPressed;

    public static event Action Action_SelectSkin;
    public static event Action Action_SelectHat;

    public static event Action Action_BuySkin;

    public static event Action Action_SkinIsSelected;

    [Header("General")]
    public SkinType skinType;
    public HatType hatType;
    [SerializeField] RegionName region;
    [SerializeField] int level;

    [SerializeField] Image backgroundImage;
    [SerializeField] Image skinImage;
    [SerializeField] TextMeshProUGUI UnknownText;

    [Header("Script Parents")]
    [SerializeField] Player_Animations Player_Animations;
    [SerializeField] SkinWardrobeManager skinWardrobeManager;
    [SerializeField] UnlockDisplay unlockDisplay;



    //--------------------


    private void Awake()
    {
        //unlockDisplay = FindObjectOfType<UnlockDisplay>();
    }
    private void Start()
    {
        //Player_Animations = FindAnyObjectByType<Player_Animations>();
    }
    private void Update()
    {
        UpdateDefaultSkinButtonDisplay();
    }
    private void OnEnable()
    {
        StartCoroutine(IsHighlighted_Delay());

        // Try resolve if not wired (prefab-instantiated case)
        if (!skinWardrobeManager)
            skinWardrobeManager = SkinWardrobeManager.Instance ?? FindObjectOfType<SkinWardrobeManager>(true);

        if (!unlockDisplay)
            unlockDisplay = FindObjectOfType<UnlockDisplay>(true);

        if (!skinWardrobeManager || !unlockDisplay)
        {
            Debug.LogError($"[{name}] Missing refs. skinWardrobeManager={skinWardrobeManager}, unlockDisplay={unlockDisplay}");
            // DON'T disable permanently; just skip init this frame.
            return;
        }

        // subscribe + init
        Action_SelectSkin += DeselectThisSkinButton;
        Action_SelectHat += DeselectThisHatButton;
        Action_BuySkin += UpdateIfSkinIsAvailable;
        Action_BuySkin += UpdateButtonDisplay;

        UpdateIfSkinIsAvailable();
        CheckIfSkinIsFound();
        UpdateSkinButtonDisplay();
        UpdateHatButtonDisplay();
        UpdateIfDefaultSkin();
    }

    private void OnDisable()
    {
        Action_SelectSkin -= DeselectThisSkinButton;
        Action_SelectHat -= DeselectThisHatButton;
        Action_BuySkin -= UpdateIfSkinIsAvailable;
        Action_BuySkin -= UpdateButtonDisplay;
    }

    IEnumerator IsHighlighted_Delay()
    {
        yield return new WaitForEndOfFrame();

        if (skinType == SkinType.Default && gameObject.GetComponent<ButtonSound>())
            gameObject.GetComponent<ButtonSound>().isHighlighted = true;
    }

    //--------------------

    void CheckIfSkinIsFound()
    {
        if (skinWardrobeManager.GetLevelSaveData(skinType))
        {
            if (skinWardrobeManager.GetSkinSaveData(GetRegionNumber(region), level) == WardrobeSkinState.LevelIsVisited
                || skinWardrobeManager.GetSkinSaveData(GetRegionNumber(region), level) == WardrobeSkinState.Available
                || skinWardrobeManager.GetSkinSaveData(GetRegionNumber(region), level) == WardrobeSkinState.Bought
                || skinWardrobeManager.GetSkinSaveData(GetRegionNumber(region), level) == WardrobeSkinState.Selected)
            {

            }
            else
            {
                skinWardrobeManager.SetSkinSaveData(GetRegionNumber(region), level, WardrobeSkinState.LevelIsVisited);
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
                    WardrobeSounds(ButtonSoundStates.ButtonCannot);
                    break;
                case WardrobeSkinState.LevelIsVisited:
                    //Nothing is happening if only the level is visited, but player don't have enough essence
                    WardrobeSounds(ButtonSoundStates.ButtonCannot);
                    break;
                case WardrobeSkinState.Available:
                    //Check condition to see if button is bought
                    if (PlayerStats.Instance.stats.itemsGot.essence_Current >= skinWardrobeManager.skinCost)
                    {
                        PlayerStats.Instance.stats.itemsGot.essence_Current -= skinWardrobeManager.skinCost;
                        skinWardrobeManager.UpdateEssenceDisplay();

                        skinWardrobeManager.SetSkinSaveData(GetRegionNumber(region), level, WardrobeSkinState.Bought);

                        WardrobeSounds(ButtonSoundStates.ButtonBuy);

                        Action_BuySkin?.Invoke();
                    }
                    else
                    {
                        WardrobeSounds(ButtonSoundStates.ButtonCannot);
                    }
                    break;
                case WardrobeSkinState.Bought:
                    //Check condition to see if button is selected
                    WardrobeSounds(ButtonSoundStates.ButtonEquip_On);

                    Action_SelectSkin?.Invoke();

                    skinWardrobeManager.SetActiveSkinData(skinType);

                    skinWardrobeManager.SetSkinSaveData(GetRegionNumber(region), level, WardrobeSkinState.Selected);

                    skinWardrobeManager.selectedSkin = skinWardrobeManager.GetSkinSelectedObject();
                    skinWardrobeManager.MoveHatObjectsToSelectedSkin();
                    break;
                case WardrobeSkinState.Selected:
                    DeselectThisSkinButton();

                    WardrobeSounds(ButtonSoundStates.ButtonEquip_Off);

                    skinWardrobeManager.SetActiveSkinData(SkinType.Default);

                    skinWardrobeManager.SetSkinSaveData(0, 0, WardrobeSkinState.Selected);

                    skinWardrobeManager.selectedSkin = skinWardrobeManager.GetSkinSelectedObject();
                    skinWardrobeManager.MoveHatObjectsToSelectedSkin();

                    if (skinWardrobeManager.skinWardrobeButton_Default && skinWardrobeManager.skinWardrobeButton_Default.GetComponent<SkinWardrobeButton>())
                        skinWardrobeManager.skinWardrobeButton_Default.GetComponent<SkinWardrobeButton>().UpdateButtonDisplay();
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
                    WardrobeSounds(ButtonSoundStates.ButtonCannot);
                    break;
                case WardrobeHatState.Available:
                    //Check condition to see if button is selected

                    WardrobeSounds(ButtonSoundStates.ButtonEquip_On);

                    Action_SelectHat?.Invoke();

                    skinWardrobeManager.SetActiveHatData(hatType);

                    skinWardrobeManager.SetHatSaveData(hatType, WardrobeHatState.Selected);

                    skinWardrobeManager.selectedHat = skinWardrobeManager.GetHatSelectedObject();
                    //skinWardrobeManager.MoveHatObjectsToSelectedSkin();
                    break;
                case WardrobeHatState.Selected:
                    DeselectThisHatButton();

                    WardrobeSounds(ButtonSoundStates.ButtonEquip_Off);

                    skinWardrobeManager.SetActiveHatData(HatType.None);

                    skinWardrobeManager.SetHatSaveData(hatType, WardrobeHatState.Available);

                    skinWardrobeManager.selectedHat = skinWardrobeManager.GetHatSelectedObject();
                    //skinWardrobeManager.MoveHatObjectsToSelectedSkin();
                    break;

                default:
                    break;
            }

            UpdateHatButtonDisplay();
            Player_Body.Instance.UpdatePlayerHats();

            skinWardrobeManager.UpdatePlayerHatDisplay();
        }

        UpdateButtonDisplay();
        skinWardrobeManager.Hat_HatUpdate();

        SkinsManager.Instance.SaveData();

        Action_SkinIsSelected?.Invoke();
        print("----. Action_SkinIsSelected");
    }


    //--------------------


    public void UpdateSkinButtonDisplay()
    {
        if (!skinWardrobeManager) return;
        if (!skinImage || !UnknownText || !backgroundImage) return;

        skinImage.gameObject.SetActive(true);
        UnknownText.gameObject.SetActive(false);

        WardrobeSkinState tempState = skinWardrobeManager.GetSkinSaveData(GetRegionNumber(region), level);

        //Based on SaveData
        //Skins
        if (skinWardrobeManager && hatType == HatType.None)
        {
            switch (tempState)
            {
                case WardrobeSkinState.Hidden:
                    backgroundImage.sprite = skinWardrobeManager.sprite_Inactive;
                    break;
                case WardrobeSkinState.LevelIsVisited:
                    backgroundImage.sprite = skinWardrobeManager.sprite_Inactive;
                    break;
                case WardrobeSkinState.Available:
                    backgroundImage.sprite = skinWardrobeManager.sprite_Available;
                    break;
                case WardrobeSkinState.Bought:
                    backgroundImage.sprite = skinWardrobeManager.sprite_Bought;
                    break;
                case WardrobeSkinState.Selected:
                    backgroundImage.sprite = skinWardrobeManager.sprite_Selected;
                    break;

                default:
                    break;
            }
        }

        //Based on Level Discovered
        if (/*!skinWardrobeManager.GetLevelSaveData(skinType) &&*/ skinType != SkinType.Default && hatType == HatType.None && (tempState == WardrobeSkinState.Hidden || tempState == WardrobeSkinState.LevelIsVisited))
        {
            backgroundImage.sprite = skinWardrobeManager.sprite_Inactive;
            skinImage.gameObject.SetActive(false);
            UnknownText.gameObject.SetActive(true);
        }
    }
    public void UpdateHatButtonDisplay()
    {
        //if (!skinWardrobeManager) return;
        //if (!skinImage || !UnknownText || !backgroundImage) return;

        if (hatType != HatType.None && skinWardrobeManager)
        {
            skinImage.gameObject.SetActive(true);
            UnknownText.gameObject.SetActive(false);

            WardrobeHatState tempState = skinWardrobeManager.GetHatSaveData(hatType);

            switch (tempState)
            {
                case WardrobeHatState.Hidden:
                    backgroundImage.sprite = skinWardrobeManager.sprite_Inactive;
                    break;
                case WardrobeHatState.Available:
                    backgroundImage.sprite = skinWardrobeManager.sprite_Bought;
                    break;
                case WardrobeHatState.Selected:
                    backgroundImage.sprite = skinWardrobeManager.sprite_Selected;
                    break;

                default:
                    backgroundImage.sprite = skinWardrobeManager.sprite_Inactive;
                    break;
            }

            //Based on Hat Discovered
            if (tempState == WardrobeHatState.Hidden)
            {
                backgroundImage.sprite = skinWardrobeManager.sprite_Inactive;
                skinImage.gameObject.SetActive(false);
                UnknownText.gameObject.SetActive(true);
            }
        }
    }

    public void UpdateDefaultSkinButtonDisplay()
    {
        if (skinType == SkinType.Default && skinWardrobeManager)
        {
            if (DataManager.Instance.skinsInfo_Store.activeSkinType == SkinType.Default)
            {
                backgroundImage.sprite = skinWardrobeManager.sprite_Selected;
            }
            else
            {
                backgroundImage.sprite = skinWardrobeManager.sprite_Bought;
            }
        }
    }
    public void UpdateIfSkinIsAvailable()
    {
        if (skinWardrobeManager.GetSkinSaveData(GetRegionNumber(region), level) == WardrobeSkinState.LevelIsVisited && PlayerStats.Instance.stats.itemsGot.essence_Current >= 10)
        {
            //skinWardrobeManager.SetSkinSaveData(GetRegionNumber(region), level, WardrobeSkinState.Available);
        }
        else if (skinWardrobeManager.GetSkinSaveData(GetRegionNumber(region), level) == WardrobeSkinState.Available && PlayerStats.Instance.stats.itemsGot.essence_Current < 10)
        {
            //skinWardrobeManager.SetSkinSaveData(GetRegionNumber(region), level, WardrobeSkinState.LevelIsVisited);
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


    #region When Button is Selected
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Highlighted
        UpdateButtonDisplay();
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
        UpdateButtonDisplay();
    }
    public void OnSelect(BaseEventData eventData)
    {
        // Selected (keyboard/controller)
        UpdateButtonDisplay();
    }
    public void OnDeselect(BaseEventData eventData)
    {

    }
    #endregion


    void UpdateButtonDisplay()
    {
        if (!skinWardrobeManager) return;
        if (!skinImage || !UnknownText || !backgroundImage) return;

        //If it's a Skin
        if (hatType == HatType.None)
        {
            WardrobeSkinState tempState = skinWardrobeManager.GetSkinSaveData(GetRegionNumber(region), level);

            switch (tempState)
            {
                case WardrobeSkinState.Hidden:
                    unlockDisplay.SetDisplay_Unavailable(region, level.ToString());
                    unlockDisplay.SetSelectedBlockName("");

                    skinWardrobeManager.selectedSkinType = SkinType.None;
                    skinWardrobeManager.selectedSkin = skinWardrobeManager.GetTempSkinSelectedObject();
                    skinWardrobeManager.UpdatePlayerBodyDisplay();
                    skinWardrobeManager.HideAllHats();
                    break;
                case WardrobeSkinState.LevelIsVisited:
                    unlockDisplay.SetDisplay_LevelReached(region, level.ToString());
                    unlockDisplay.SetSelectedBlockName("");

                    skinWardrobeManager.selectedSkinType = SkinType.None;
                    skinWardrobeManager.selectedSkin = skinWardrobeManager.GetTempSkinSelectedObject();
                    skinWardrobeManager.UpdatePlayerBodyDisplay();
                    skinWardrobeManager.HideAllHats();
                    break;
                case WardrobeSkinState.Available:
                    if (PlayerStats.Instance.stats.itemsGot.essence_Current >= 10)
                        unlockDisplay.SetDisplay_CanUnlock();
                    else
                        unlockDisplay.SetDisplay_CanNotUnlock();

                    unlockDisplay.SetSelectedBlockName(SkinsOverview.Instance.GetSkinName(skinType));

                    skinWardrobeManager.selectedSkinType = skinType;
                    skinWardrobeManager.selectedSkin = skinWardrobeManager.GetTempSkinSelectedObject();
                    skinWardrobeManager.UpdatePlayerBodyDisplay();
                    //skinWardrobeManager.Hat_HatUpdate();
                    break;
                case WardrobeSkinState.Bought:
                    unlockDisplay.SetDisplay_CanEquip();
                    unlockDisplay.SetSelectedBlockName(SkinsOverview.Instance.GetSkinName(skinType));

                    skinWardrobeManager.selectedSkinType = skinType;
                    skinWardrobeManager.selectedSkin = skinWardrobeManager.GetTempSkinSelectedObject();
                    skinWardrobeManager.UpdatePlayerBodyDisplay();
                    //skinWardrobeManager.Hat_HatUpdate();
                    break;
                case WardrobeSkinState.Selected:
                    unlockDisplay.SetDisplay_IsEquipped();
                    unlockDisplay.SetSelectedBlockName(SkinsOverview.Instance.GetSkinName(skinType));

                    skinWardrobeManager.selectedSkinType = skinType;
                    skinWardrobeManager.selectedSkin = skinWardrobeManager.GetTempSkinSelectedObject();
                    skinWardrobeManager.UpdatePlayerBodyDisplay();
                    //skinWardrobeManager.Hat_HatUpdate();
                    break;

                default:
                    unlockDisplay.SetDisplay_Unavailable(region, level.ToString());
                    unlockDisplay.SetSelectedBlockName("");

                    skinWardrobeManager.selectedSkinType = SkinType.None;
                    skinWardrobeManager.selectedSkin = skinWardrobeManager.GetTempSkinSelectedObject();
                    skinWardrobeManager.UpdatePlayerBodyDisplay();
                    skinWardrobeManager.HideAllHats();
                    break;
            }
        }

        //If it's a Hat
        else
        {
            WardrobeHatState tempState = skinWardrobeManager.GetHatSaveData(hatType);

            switch (tempState)
            {
                case WardrobeHatState.Hidden:
                    unlockDisplay.SetDisplay_FinishQuestline(hatType);
                    unlockDisplay.SetSelectedBlockName("");

                    skinWardrobeManager.selectedHat = skinWardrobeManager.GetHatSelectedObject();
                    break;
                case WardrobeHatState.Available:
                    unlockDisplay.SetDisplay_CanEquip();
                    unlockDisplay.SetSelectedBlockName(SkinsOverview.Instance.GetHatName(hatType));

                    UpdateHatButtonDisplay();
                    skinWardrobeManager.selectedHatType = hatType;
                    skinWardrobeManager.selectedHat = skinWardrobeManager.GetTempHatSelectedObject();
                    skinWardrobeManager.selectedSkin = skinWardrobeManager.GetEquippedSkinSelectedObject();

                    skinWardrobeManager.UpdatePlayerBodyDisplay();
                    skinWardrobeManager.UpdatePlayerHatDisplay();
                    break;
                case WardrobeHatState.Selected:
                    unlockDisplay.SetDisplay_IsEquipped();
                    unlockDisplay.SetSelectedBlockName(SkinsOverview.Instance.GetHatName(hatType));

                    UpdateHatButtonDisplay();
                    skinWardrobeManager.selectedHatType = hatType;
                    skinWardrobeManager.selectedHat = skinWardrobeManager.GetTempHatSelectedObject();
                    skinWardrobeManager.selectedSkin = skinWardrobeManager.GetEquippedSkinSelectedObject();

                    skinWardrobeManager.UpdatePlayerBodyDisplay();
                    skinWardrobeManager.UpdatePlayerHatDisplay();
                    break;

                default:
                    unlockDisplay.SetDisplay_FinishQuestline(hatType);
                    unlockDisplay.SetSelectedBlockName("");

                    skinWardrobeManager.selectedHat = skinWardrobeManager.GetHatSelectedObject();
                    break;
            }
        }
    }


    //--------------------


    void WardrobeSounds(ButtonSoundStates buttonSoundStates)
    {
        if (gameObject.GetComponent<ButtonSound>() && gameObject.GetComponent<ButtonSound>().isHighlighted)
        {
            gameObject.GetComponent<ButtonSound>().buttonPress_Sound = buttonSoundStates;
            gameObject.GetComponent<ButtonSound>().TryButtonPressSound();
            //Action_ButtonIsPressed?.Invoke();
        }
    }
}

