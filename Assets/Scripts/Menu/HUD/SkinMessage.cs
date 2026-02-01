using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinMessage : MonoBehaviour
{
    [SerializeField] Image skinImage;
    [SerializeField] TextMeshProUGUI skinMessage;


    //--------------------


    private void OnEnable()
    {
        UpdateSkinMessage();
    }


    //--------------------


    void UpdateSkinMessage()
    {
        skinImage.sprite = PauseMenuManager.Instance.SelectSpriteForLevel(MapManager.Instance.mapInfo_ToSave.skintype);

        if (DataManager.Instance.oneTimeRunData_Store.pickup_FirstSkin)
            skinMessage.text = "<color=#B593D5>" + SkinsOverview.Instance.GetSkinName(MapManager.Instance.mapInfo_ToSave.skintype) + "</color> was found";
        else
            skinMessage.text = "<color=#B593D5>" + SkinsOverview.Instance.GetSkinName(MapManager.Instance.mapInfo_ToSave.skintype) + "</color> was found. You may equip it in the Wardrobe Menu";

        DataManager.Instance.oneTimeRunData_Store.pickup_FirstSkin = true;
        DataPersistanceManager.Instance.SaveGame();
    }
}