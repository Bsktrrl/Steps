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
        unavailable_obj.GetComponentInChildren<TextMeshProUGUI>().text = "Find skin in " + region.ToString() + "\r\nLv." + level + " to unlock";

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

    public void SetDisplay_LevelReached()
    {
        levelReached_obj.GetComponentInChildren<TextMeshProUGUI>().text = "Find the skin in level\r\nto unlock";

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
                npcName = "Floríel";
                break;
            case HatType.Granith_Hat:
                npcName = "Granith";
                break;
            case HatType.Archie_Hat:
                npcName = "Archie";
                break;
            case HatType.Aisa_Hat:
                npcName = "Aisa";
                break;
            case HatType.Mossy_Hat:
                npcName = "Mossy";
                break;
            case HatType.Larry_Hat:
                npcName = "Larry";
                break;

            default:
                break;
        }

        finishQuestline_obj.GetComponentInChildren<TextMeshProUGUI>().text = "Finish the questline of " + npcName;

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

    public void SetSelectedBlockName(string name, RegionName region)
    {
        switch (region)
        {
            case RegionName.None:
                    selectedBlockName.text = name;
                break;

            case RegionName.Rivergreen:
                selectedBlockName.text = name + "\n of the Rivergreen";
                break;
            case RegionName.Sandlands:
                selectedBlockName.text = name + "\n of the Sandlands";
                break;
            case RegionName.Frostfields:
                selectedBlockName.text = name + "\n of the Frostfields";
                break;
            case RegionName.Firevein:
                selectedBlockName.text = name + "\n of the Firevein Mountain";
                break;
            case RegionName.Witchmire:
                selectedBlockName.text = name + "\n of the Witchmire";
                break;
            case RegionName.Metalworks:
                selectedBlockName.text = name + "\n of the Metalworks";
                break;

            default:
                selectedBlockName.text = name;
                break;
        }
        
    }
}
