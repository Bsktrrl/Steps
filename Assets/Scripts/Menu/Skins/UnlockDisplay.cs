using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnlockDisplay : MonoBehaviour
{
    [SerializeField] GameObject unavailable_obj;
    [SerializeField] GameObject levelReached_obj;
    [SerializeField] GameObject canNotUnlock_obj;
    [SerializeField] GameObject canUnlock_obj;
    [SerializeField] GameObject canEquip_obj;
    [SerializeField] GameObject isEquipped_obj;

    [SerializeField] GameObject finishQuestline_obj;


    //--------------------


    public void SetDisplay_Unavailable(RegionName region, string level)
    {
        unavailable_obj.GetComponentInChildren<TextMeshProUGUI>().text = "Find skin in " + region.ToString() + "\r\nLv." + level + " to unlock";

        unavailable_obj.SetActive(true);
        levelReached_obj.SetActive(false);
        canNotUnlock_obj.SetActive(false);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(false);
        finishQuestline_obj.SetActive(false);
    }

    public void SetDisplay_LevelReached()
    {
        levelReached_obj.GetComponentInChildren<TextMeshProUGUI>().text = "Find the skin\r\nto unlock";

        unavailable_obj.SetActive(false);
        levelReached_obj.SetActive(true);
        canNotUnlock_obj.SetActive(false);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(false);
        finishQuestline_obj.SetActive(false);
    }

    public void SetDisplay_CanNotUnlock()
    {
        unavailable_obj.SetActive(false);
        levelReached_obj.SetActive(false);
        canNotUnlock_obj.SetActive(true);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(false);
        finishQuestline_obj.SetActive(false);
    }

    public void SetDisplay_CanUnlock()
    {
        unavailable_obj.SetActive(false);
        levelReached_obj.SetActive(false);
        canNotUnlock_obj.SetActive(false);
        canUnlock_obj.SetActive(true);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(false);
        finishQuestline_obj.SetActive(false);
    }

    public void SetDisplay_CanEquip()
    {
        unavailable_obj.SetActive(false);
        levelReached_obj.SetActive(false);
        canNotUnlock_obj.SetActive(false);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(true);
        isEquipped_obj.SetActive(false);
        finishQuestline_obj.SetActive(false);
    }

    public void SetDisplay_IsEquipped()
    {
        unavailable_obj.SetActive(false);
        levelReached_obj.SetActive(false);
        canNotUnlock_obj.SetActive(false);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(true);
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

        unavailable_obj.SetActive(false);
        levelReached_obj.SetActive(false);
        canNotUnlock_obj.SetActive(false);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(false);
        finishQuestline_obj.SetActive(true);
    }
}
