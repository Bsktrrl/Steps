using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockDisplay : MonoBehaviour
{
    [SerializeField] GameObject unavailable_obj;
    [SerializeField] GameObject levelReached_obj;
    [SerializeField] GameObject canNotUnlock_obj;
    [SerializeField] GameObject canUnlock_obj;
    [SerializeField] GameObject canEquip_obj;
    [SerializeField] GameObject isEquipped_obj;

    [SerializeField] GameObject finishQuestline_obj;

    private RectTransform _currentPulsingImageRt;

    [SerializeField] TextMeshProUGUI selectedBlockName;


    //--------------------


    public void SetDisplay_Unavailable(RegionName region, string level)
    {
        unavailable_obj.GetComponentInChildren<TextMeshProUGUI>().text = UnavailableMessage(region, level);

        CopyPulseScaleTo(unavailable_obj);

        unavailable_obj.SetActive(true);
        levelReached_obj.SetActive(false);
        canNotUnlock_obj.SetActive(false);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(false);
        finishQuestline_obj.SetActive(false);

        _currentPulsingImageRt = unavailable_obj.GetComponentInChildren<Image>().rectTransform;
    }

    public void SetDisplay_LevelReached(RegionName region, string level)
    {
        levelReached_obj.GetComponentInChildren<TextMeshProUGUI>().text = UnavailableMessage(region, level);

        CopyPulseScaleTo(levelReached_obj);

        levelReached_obj.SetActive(true);
        unavailable_obj.SetActive(false);
        canNotUnlock_obj.SetActive(false);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(false);
        finishQuestline_obj.SetActive(false);

        _currentPulsingImageRt = levelReached_obj.GetComponentInChildren<Image>().rectTransform;
    }

    public void SetDisplay_CanNotUnlock()
    {
        CopyPulseScaleTo(canNotUnlock_obj);

        canNotUnlock_obj.SetActive(true);
        unavailable_obj.SetActive(false);
        levelReached_obj.SetActive(false);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(false);
        finishQuestline_obj.SetActive(false);

        _currentPulsingImageRt = canNotUnlock_obj.GetComponentInChildren<Image>().rectTransform;
    }

    public void SetDisplay_CanUnlock()
    {
        CopyPulseScaleTo(canUnlock_obj);

        canUnlock_obj.SetActive(true);
        unavailable_obj.SetActive(false);
        levelReached_obj.SetActive(false);
        canNotUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(false);
        finishQuestline_obj.SetActive(false);

        _currentPulsingImageRt = canUnlock_obj.GetComponentInChildren<Image>().rectTransform;
    }

    public void SetDisplay_CanEquip()
    {
        CopyPulseScaleTo(canEquip_obj);

        canEquip_obj.SetActive(true);
        unavailable_obj.SetActive(false);
        levelReached_obj.SetActive(false);
        canNotUnlock_obj.SetActive(false);
        canUnlock_obj.SetActive(false);
        isEquipped_obj.SetActive(false);
        finishQuestline_obj.SetActive(false);

        _currentPulsingImageRt = canEquip_obj.GetComponentInChildren<Image>().rectTransform;
    }

    public void SetDisplay_IsEquipped()
    {
        CopyPulseScaleTo(isEquipped_obj);

        isEquipped_obj.SetActive(true);
        unavailable_obj.SetActive(false);
        levelReached_obj.SetActive(false);
        canNotUnlock_obj.SetActive(false);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        finishQuestline_obj.SetActive(false);

        _currentPulsingImageRt = isEquipped_obj.GetComponentInChildren<Image>().rectTransform;
    }

    public void SetDisplay_FinishQuestline(HatType hatType)
    {
        string npcName = "";

        switch (hatType)
        {
            case HatType.None:
                break;

            case HatType.Floriel_Hat:
                npcName = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Water;
                break;
            case HatType.Archie_Hat:
                npcName = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Sand;
                break;
            case HatType.Aisa_Hat:
                npcName = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Ice;
                break;
            case HatType.Granith_Hat:
                npcName = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Lava;
                break;
            case HatType.Mossy_Hat:
                npcName = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Swamp;
                break;
            case HatType.Larry_Hat:
                npcName = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Metal;
                break;

            default:
                break;
        }

        finishQuestline_obj.GetComponentInChildren<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_Headgear_Unavailable + " " + npcName;

        CopyPulseScaleTo(finishQuestline_obj);

        finishQuestline_obj.SetActive(true);
        unavailable_obj.SetActive(false);
        levelReached_obj.SetActive(false);
        canNotUnlock_obj.SetActive(false);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(false);

        _currentPulsingImageRt = finishQuestline_obj.GetComponentInChildren<Image>().rectTransform;
    }

    void CopyPulseScaleTo(GameObject nextObj)
    {
        if (nextObj)
        {
            var nextRt = nextObj.GetComponentInChildren<Image>().rectTransform;

            if (_currentPulsingImageRt != null)
                nextRt.localScale = _currentPulsingImageRt.localScale;
            else
                nextRt.localScale = Vector3.one; // first time fallback
        }
    }

    public void SetSelectedBlockName(string name)
    {
        selectedBlockName.text = name;
    }


    //--------------------


    string UnavailableMessage(RegionName region, string level)
    {
        switch (region)
        {
            case RegionName.None:
                return "";

            case RegionName.Rivergreen:
                return
           DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinUnavailable_1 + " " +
           DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water + " Lv." + level +
           DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinUnavailable_2;
            case RegionName.Sandlands:
                return
           DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinUnavailable_1 + " " +
           DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand + " Lv." + level +
           DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinUnavailable_2;
            case RegionName.Frostfields:
                return
           DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinUnavailable_1 + " " +
           DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice + " Lv." + level +
           DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinUnavailable_2;
            case RegionName.Firevein:
                return
           DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinUnavailable_1 + " " +
           DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava + " Lv." + level +
           DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinUnavailable_2;
            case RegionName.Witchmire:
                return
           DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinUnavailable_1 + " " +
           DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp + " Lv." + level +
           DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinUnavailable_2;
            case RegionName.Metalworks:
                return
           DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinUnavailable_1 + " " +
           DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal + " Lv." + level +
           DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinUnavailable_2;

            default:
                return "";
        }
    }
}
