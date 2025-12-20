using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnlockDisplay : Singleton<UnlockDisplay>
{
    [SerializeField] GameObject unavailable_obj;
    [SerializeField] GameObject canUnlock_obj;
    [SerializeField] GameObject canEquip_obj;
    [SerializeField] GameObject isEquipped_obj;


    //--------------------


    public void SetDisplay_Unavailable(RegionName region, string level)
    {
        unavailable_obj.GetComponentInChildren<TextMeshProUGUI>().text = "Reach " + region.ToString() + "\r\nLv." + level + " to unlock";

        unavailable_obj.SetActive(true);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(false);
    }

    public void SetDisplay_CanUnlock()
    {
        unavailable_obj.SetActive(false);
        canUnlock_obj.SetActive(true);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(false);
    }

    public void SetDisplay_CanEquip()
    {
        unavailable_obj.SetActive(false);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(true);
        isEquipped_obj.SetActive(false);
    }

    public void SetDisplay_IsEquipped()
    {
        unavailable_obj.SetActive(false);
        canUnlock_obj.SetActive(false);
        canEquip_obj.SetActive(false);
        isEquipped_obj.SetActive(true);
    }
}
