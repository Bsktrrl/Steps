using UnityEngine;

public class AbilityInfoButton_ImageDisplay : MonoBehaviour
{
    [SerializeField] GameObject passive_Temporary;
    [SerializeField] GameObject passive_Permanent;

    [SerializeField] GameObject active_Temporary;
    [SerializeField] GameObject active_Permanent;


    //--------------------


    private void OnEnable()
    {
        UpdateImage();
    }


    //--------------------


    void UpdateImage()
    {
        HideAllImages();

        Abilities ability = GetComponent<AbilityVersions>().ability;
        var stats = PlayerStats.Instance.stats;

        if (stats?.abilitiesGot_Permanent?.HasAbility(ability) == true)
            ShowImage(passive_Permanent, active_Permanent);
        else if (stats?.abilitiesGot_Temporary?.HasAbility(ability) == true)
            ShowImage(passive_Temporary, active_Temporary);
    }


    //--------------------


    void HideAllImages()
    {
        passive_Temporary.SetActive(false);
        passive_Permanent.SetActive(false);

        active_Temporary.SetActive(false);
        active_Permanent.SetActive(false);
    }
    void ShowImage(GameObject obj_Passive, GameObject obj_Active)
    {
        obj_Passive.SetActive(true);
        obj_Active.SetActive(true);
    }
}
