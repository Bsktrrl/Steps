using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSelectName : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text_Can_Active;
    [SerializeField] TextMeshProUGUI text_Can_Passive;
    [SerializeField] TextMeshProUGUI text_Cannot_Active;
    [SerializeField] TextMeshProUGUI text_Cannot_Passive;


    //--------------------


    private void OnEnable()
    {
        SettingsManager.Action_SetNewLanguage += UpdateLevelNameDisplay;
    }


    //--------------------


    private void Start()
    {
        UpdateLevelNameDisplay();
    }


    //--------------------


    void UpdateLevelNameDisplay()
    {
        text_Can_Active.text = GetComponent<LevelInfo>().GetName();
        text_Cannot_Active.text = GetComponent<LevelInfo>().GetName();
        text_Can_Passive.text = GetComponent<LevelInfo>().GetName();
        text_Cannot_Passive.text = GetComponent<LevelInfo>().GetName();
    }
}
