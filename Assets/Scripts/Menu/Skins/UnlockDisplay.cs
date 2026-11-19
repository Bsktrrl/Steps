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
    [SerializeField] GameObject unlockAllOtherSkins_obj;

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
        unlockAllOtherSkins_obj.SetActive(false);
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
        unlockAllOtherSkins_obj.SetActive(false);
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
        unlockAllOtherSkins_obj.SetActive(false);
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
        unlockAllOtherSkins_obj.SetActive(false);
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
        unlockAllOtherSkins_obj.SetActive(false);
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
        unlockAllOtherSkins_obj.SetActive(false);
        finishQuestline_obj.SetActive(false);

        _currentPulsingImageRt = isEquipped_obj.GetComponentInChildren<Image>().rectTransform;
    }

    public void SetDisplay_UnlockAllOtherSkins()
    {
        CopyPulseScaleTo(isEquipped_obj);

        unlockAllOtherSkins_obj.SetActive(true);
        isEquipped_obj.SetActive(false);
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
        var data = DataManager.Instance;
        int language = (int)data.settingData_StoreList.currentLanguage;
        var translations = data.game_TextDatabase_Store.gameText_LanguageList;
        var text = translations[language];

        if (region == RegionName.None && level == "2")
            return text.WardrobeMenu_Message_UnlockAllSkins;

        string levelName;
        if ((region == RegionName.None && level == "1") || region == RegionName.HUB)
        {
            levelName = text.name_Region_HUB;
        }
        else if (int.TryParse(level, out int number))
        {
            levelName = GetWardrobeLevelName(text, region, number);
            if (string.IsNullOrWhiteSpace(levelName))
                levelName = GetWardrobeLevelName(translations[(int)Languages.English], region, number);
        }
        else
        {
            return "";
        }

        if (string.IsNullOrWhiteSpace(levelName))
            return "";

        return $"{text.WardrobeMenu_Message_SkinUnavailable_1} {levelName} {text.WardrobeMenu_Message_SkinUnavailable_2}";
    }

    private static string GetWardrobeLevelName(Game_TextDatabase_Language text, RegionName region, int number)
    {
        return region switch
        {
            RegionName.Rivergreen => number switch
            {
                1 => text.name_Region_Water_Lv1,
                2 => text.name_Region_Water_Lv2,
                3 => text.name_Region_Water_Lv3,
                4 => text.name_Region_Water_Lv4,
                5 => text.name_Region_Water_Lv5,
                _ => null
            },
            RegionName.Sandlands => number switch
            {
                1 => text.name_Region_Sand_Lv1,
                2 => text.name_Region_Sand_Lv2,
                3 => text.name_Region_Sand_Lv3,
                4 => text.name_Region_Sand_Lv4,
                5 => text.name_Region_Sand_Lv5,
                _ => null
            },
            RegionName.Frostfields => number switch
            {
                1 => text.name_Region_Ice_Lv1,
                2 => text.name_Region_Ice_Lv2,
                3 => text.name_Region_Ice_Lv3,
                4 => text.name_Region_Ice_Lv4,
                5 => text.name_Region_Ice_Lv5,
                _ => null
            },
            RegionName.Firevein => number switch
            {
                1 => text.name_Region_Lava_Lv1,
                2 => text.name_Region_Lava_Lv2,
                3 => text.name_Region_Lava_Lv3,
                4 => text.name_Region_Lava_Lv4,
                5 => text.name_Region_Lava_Lv5,
                _ => null
            },
            RegionName.Witchmire => number switch
            {
                1 => text.name_Region_Swamp_Lv1,
                2 => text.name_Region_Swamp_Lv2,
                3 => text.name_Region_Swamp_Lv3,
                4 => text.name_Region_Swamp_Lv4,
                5 => text.name_Region_Swamp_Lv5,
                _ => null
            },
            RegionName.Metalworks => number switch
            {
                1 => text.name_Region_Metal_Lv1,
                2 => text.name_Region_Metal_Lv2,
                3 => text.name_Region_Metal_Lv3,
                4 => text.name_Region_Metal_Lv4,
                5 => text.name_Region_Metal_Lv5,
                _ => null
            },
            _ => null
        };
    }
}
