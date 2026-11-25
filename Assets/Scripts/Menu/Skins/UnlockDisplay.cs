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
    [SerializeField] TextMeshProUGUI canUnlockCost;
    [SerializeField] TextMeshProUGUI cannotUnlockCost;
    [SerializeField] GameObject canEquip_obj;
    [SerializeField] GameObject isEquipped_obj;
    [SerializeField] GameObject unlockAllOtherSkins_obj;

    [SerializeField] GameObject finishQuestline_obj;

    private RectTransform _currentPulsingImageRt;

    [SerializeField] TextMeshProUGUI selectedBlockName;


    //--------------------


    public void SetDisplay_Unavailable(RegionName region, string level)
    {
        unavailable_obj.GetComponentInChildren<TextMeshProUGUI>(true).text = UnavailableMessage(region, level);

        CopyPulseScaleTo(unavailable_obj);

        unavailable_obj.SetActive(true);
        levelReached_obj.SetActive(false);
        canNotUnlock_obj.SetActive(false);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(false);
        unlockAllOtherSkins_obj.SetActive(false);
        finishQuestline_obj.SetActive(false);
    }

    public void SetDisplay_LevelReached(RegionName region, string level)
    {
        levelReached_obj.GetComponentInChildren<TextMeshProUGUI>(true).text = UnavailableMessage(region, level);

        CopyPulseScaleTo(levelReached_obj);

        levelReached_obj.SetActive(true);
        unavailable_obj.SetActive(false);
        canNotUnlock_obj.SetActive(false);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(false);
        unlockAllOtherSkins_obj.SetActive(false);
        finishQuestline_obj.SetActive(false);
    }

    public void SetDisplay_CanNotUnlock(int cost)
    {
        cannotUnlockCost.SetText("{0}", cost);
        CopyPulseScaleTo(canNotUnlock_obj);

        canNotUnlock_obj.SetActive(true);
        unavailable_obj.SetActive(false);
        levelReached_obj.SetActive(false);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(false);
        unlockAllOtherSkins_obj.SetActive(false);
        finishQuestline_obj.SetActive(false);
    }

    public void SetDisplay_CanUnlock(int cost)
    {
        canUnlockCost.SetText("{0}", cost);
        CopyPulseScaleTo(canUnlock_obj);

        canUnlock_obj.SetActive(true);
        unavailable_obj.SetActive(false);
        levelReached_obj.SetActive(false);
        canNotUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(false);
        unlockAllOtherSkins_obj.SetActive(false);
        finishQuestline_obj.SetActive(false);
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
    }

    public void SetDisplay_UnlockAllOtherSkins()
    {
        CopyPulseScaleTo(unlockAllOtherSkins_obj);

        unlockAllOtherSkins_obj.SetActive(true);
        isEquipped_obj.SetActive(false);
        unavailable_obj.SetActive(false);
        levelReached_obj.SetActive(false);
        canNotUnlock_obj.SetActive(false);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        finishQuestline_obj.SetActive(false);
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

        finishQuestline_obj.GetComponentInChildren<TextMeshProUGUI>(true).text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_Headgear_Unavailable + " " + npcName;

        CopyPulseScaleTo(finishQuestline_obj);

        finishQuestline_obj.SetActive(true);
        unavailable_obj.SetActive(false);
        levelReached_obj.SetActive(false);
        canNotUnlock_obj.SetActive(false);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(false);
        unlockAllOtherSkins_obj.SetActive(false);
    }

    void CopyPulseScaleTo(GameObject nextObj)
    {
        Image image = nextObj.GetComponentInChildren<Image>(true);
        if (!image)
            return;

        RectTransform nextRt = image.rectTransform;
        nextRt.localScale = _currentPulsingImageRt ? _currentPulsingImageRt.localScale : Vector3.one;
        _currentPulsingImageRt = nextRt;
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
            levelName = text.GetLevelName(region, number);
            if (string.IsNullOrWhiteSpace(levelName))
                levelName = translations[(int)Languages.English].GetLevelName(region, number);
        }
        else
        {
            return "";
        }

        if (string.IsNullOrWhiteSpace(levelName))
            return "";

        return $"{text.WardrobeMenu_Message_SkinUnavailable_1} {levelName} {text.WardrobeMenu_Message_SkinUnavailable_2}";
    }
}
