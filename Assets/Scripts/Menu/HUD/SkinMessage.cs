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
            skinMessage.text =
                "<color=#B593D5>"
                + SkinsOverview.Instance.GetSkinName(MapManager.Instance.mapInfo_ToSave.skintype)
                + "</color> "
                + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_Skin;
        else
            skinMessage.text =
                "<color=#B593D5>" 
                + SkinsOverview.Instance.GetSkinName(MapManager.Instance.mapInfo_ToSave.skintype) 
                + "</color> "
                + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_Skin_First;

        DataManager.Instance.oneTimeRunData_Store.pickup_FirstSkin = true;
        DataPersistanceManager.Instance.SaveGame();
    }
}