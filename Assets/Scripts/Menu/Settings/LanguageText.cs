using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageText : MonoBehaviour
{
    [Header("Target Text")]
    TextMeshProUGUI textComponent;

    [Header("Languages")]
    [TextArea(1, 20)] [SerializeField] string norwegian;
    [TextArea(1, 20)] [SerializeField] string english;
    [TextArea(1, 20)] [SerializeField] string german;
    [TextArea(1, 20)] [SerializeField] string chinese;
    [TextArea(1, 20)] [SerializeField] string japanese;
    [TextArea(1, 20)] [SerializeField] string korean;


    //--------------------


    private void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();

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
        if (textComponent)
        {
            switch (DataManager.Instance.settingData_StoreList.currentLanguage)
            {
                case Languages.Norwegian:
                    textComponent.text = norwegian;
                    break;
                case Languages.English:
                    textComponent.text = english;
                    break;
                case Languages.German:
                    textComponent.text = german;
                    break;
                case Languages.Japanese:
                    textComponent.text = japanese;
                    break;
                case Languages.Chinese:
                    textComponent.text = chinese;
                    break;
                case Languages.Korean:
                    textComponent.text = korean;
                    break;

                default:
                    textComponent.text = norwegian;
                    break;
            }
        }
    }
}
