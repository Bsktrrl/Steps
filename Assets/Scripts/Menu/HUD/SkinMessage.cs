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
        skinMessage.text = "<color=#B593D5>" + SkinsOverview.Instance.GetSkinName(MapManager.Instance.mapInfo_ToSave.skintype) + "</color> was found";
    }
}