using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageText : MonoBehaviour
{
    [Header("Target Text")]
    [SerializeField] TextMeshProUGUI text;

    [Header("Languages")]
    [TextArea(1, 5)] [SerializeField] string norwegian;
    [TextArea(1, 5)] [SerializeField] string english;
    [TextArea(1, 5)] [SerializeField] string german;
    [TextArea(1, 5)] [SerializeField] string chinese;
    [TextArea(1, 5)] [SerializeField] string japanese;
    [TextArea(1, 5)] [SerializeField] string korean;


    //--------------------


    private void Start()
    {
        UpdateText();
    }
    private void OnEnable()
    {
        SettingsMenu.Action_SetNewLanguage += UpdateText;

        UpdateText();
    }
    private void OnDisable()
    {
        SettingsMenu.Action_SetNewLanguage -= UpdateText;
    }


    //--------------------


    public void UpdateText()
    {
        switch (DataManager.Instance.settingData_StoreList.currentLanguage)
        {
            case Languages.Norwegian:
                text.text = norwegian;
                break;
            case Languages.English:
                text.text = english;
                break;
            case Languages.German:
                text.text = german;
                break;
            case Languages.Japanese:
                text.text = japanese;
                break;
            case Languages.Chinese:
                text.text = chinese;
                break;
            case Languages.Korean:
                text.text = korean;
                break;

            default:
                text.text = norwegian;
                break;
        }
    }
}
