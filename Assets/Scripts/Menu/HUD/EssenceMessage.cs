using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class EssenceMessage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI essenceMessage;


    //--------------------


    private void OnEnable()
    {
        UpdateEssenceMessage();
    }


    //--------------------


    void UpdateEssenceMessage()
    {
        int essencecounter = 0;

        for (int i = 0; i < MapManager.Instance.mapInfo_ToSave.essenceList.Count; i++)
        {
            if (MapManager.Instance.mapInfo_ToSave.essenceList[i].isTaken)
            {
                essencecounter++;
            }
        }

        essenceMessage.text = 
            DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_Eccence_1 
            + " <color=#B593D5>" + essencecounter + "</color> "
            + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_Eccence_2
            + " <color=#B593D5>" + 10 + "</color> "
            + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_Eccence_3;
    }
}
