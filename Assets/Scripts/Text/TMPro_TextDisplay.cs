using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPro_TextDisplay : MonoBehaviour
{
    [SerializeField] Text_Database_Enum textToDisplay;


    //--------------------


    private void Start()
    {
        ShowText();
    }
    private void OnEnable()
    {
        SettingsManager.Action_SetNewLanguage += ShowText;
    }
    private void OnDisable()
    {
        SettingsManager.Action_SetNewLanguage += ShowText;
    }


    //--------------------


    void ShowText()
    {
        switch (textToDisplay)
        {
            //None
            case Text_Database_Enum.None:
                GetComponent<TextMeshProUGUI>().text = "";
                break;

            #region Region Names
            case Text_Database_Enum.name_Region_Water:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water;
                break;
            case Text_Database_Enum.name_Region_Sand:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand;
                break;
            case Text_Database_Enum.name_Region_Ice:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice;
                break;
            case Text_Database_Enum.name_Region_Lava:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava;
                break;
            case Text_Database_Enum.name_Region_Swamp:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp;
                break;
            case Text_Database_Enum.name_Region_Metal:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal;
                break;
            case Text_Database_Enum.name_Region_6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_6;
                break;
            #endregion

            #region Names of levels in Water Region
            case Text_Database_Enum.name_Region_Water_Lv1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water_Lv1;
                break;
            case Text_Database_Enum.name_Region_Water_Lv2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water_Lv2;
                break;
            case Text_Database_Enum.name_Region_Water_Lv3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water_Lv3;
                break;
            case Text_Database_Enum.name_Region_Water_Lv4:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water_Lv4;
                break;
            case Text_Database_Enum.name_Region_Water_Lv5:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water_Lv5;
                break;
            case Text_Database_Enum.name_Region_Water_Lv6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water_Lv6;
                break;
            #endregion

            #region Names of levels in Sand Region
            case Text_Database_Enum.name_Region_Sand_Lv1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand_Lv1;
                break;
            case Text_Database_Enum.name_Region_Sand_Lv2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand_Lv2;
                break;
            case Text_Database_Enum.name_Region_Sand_Lv3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand_Lv3;
                break;
            case Text_Database_Enum.name_Region_Sand_Lv4:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand_Lv4;
                break;
            case Text_Database_Enum.name_Region_Sand_Lv5:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand_Lv5;
                break;
            case Text_Database_Enum.name_Region_Sand_Lv6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand_Lv6;
                break;
            #endregion

            #region Names of levels in Ice Region
            case Text_Database_Enum.name_Region_Ice_Lv1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice_Lv1;
                break;
            case Text_Database_Enum.name_Region_Ice_Lv2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice_Lv2;
                break;
            case Text_Database_Enum.name_Region_Ice_Lv3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice_Lv3;
                break;
            case Text_Database_Enum.name_Region_Ice_Lv4:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice_Lv4;
                break;
            case Text_Database_Enum.name_Region_Ice_Lv5:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice_Lv5;
                break;
            case Text_Database_Enum.name_Region_Ice_Lv6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice_Lv6;
                break;
            #endregion

            #region Names of levels in Lava Region
            case Text_Database_Enum.name_Region_Lava_Lv1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava_Lv1;
                break;
            case Text_Database_Enum.name_Region_Lava_Lv2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava_Lv2;
                break;
            case Text_Database_Enum.name_Region_Lava_Lv3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava_Lv3;
                break;
            case Text_Database_Enum.name_Region_Lava_Lv4:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava_Lv4;
                break;
            case Text_Database_Enum.name_Region_Lava_Lv5:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava_Lv5;
                break;
            case Text_Database_Enum.name_Region_Lava_Lv6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava_Lv6;
                break;
            #endregion

            #region Names of levels in Swamp Region
            case Text_Database_Enum.name_Region_Swamp_Lv1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp_Lv1;
                break;
            case Text_Database_Enum.name_Region_Swamp_Lv2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp_Lv2;
                break;
            case Text_Database_Enum.name_Region_Swamp_Lv3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp_Lv3;
                break;
            case Text_Database_Enum.name_Region_Swamp_Lv4:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp_Lv4;
                break;
            case Text_Database_Enum.name_Region_Swamp_Lv5:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp_Lv5;
                break;
            case Text_Database_Enum.name_Region_Swamp_Lv6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp_Lv6;
                break;
            #endregion

            #region Names of levels in Metal Region
            case Text_Database_Enum.name_Region_Metal_Lv1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal_Lv1;
                break;
            case Text_Database_Enum.name_Region_Metal_Lv2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal_Lv2;
                break;
            case Text_Database_Enum.name_Region_Metal_Lv3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal_Lv3;
                break;
            case Text_Database_Enum.name_Region_Metal_Lv4:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal_Lv4;
                break;
            case Text_Database_Enum.name_Region_Metal_Lv5:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal_Lv5;
                break;
            case Text_Database_Enum.name_Region_Metal_Lv6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal_Lv6;
                break;
            #endregion

            #region Main Menu Buttons
            case Text_Database_Enum.mainMenu_Button_Continue:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_Continue;
                break;
            case Text_Database_Enum.mainMenu_Button_NewGame:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_NewGame;
                break;
            case Text_Database_Enum.mainMenu_Button_Wardrobe:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_Wardrobe;
                break;
            case Text_Database_Enum.mainMenu_Button_Options:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_Options;
                break;
            case Text_Database_Enum.mainMenu_Button_Quit:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_Quit;
                break;
            case Text_Database_Enum.mainMenu_Button_6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_6;
                break;
            case Text_Database_Enum.mainMenu_Button_7:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_7;
                break;
            case Text_Database_Enum.mainMenu_Button_8:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_8;
                break;
            case Text_Database_Enum.mainMenu_Button_9:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_9;
                break;
            case Text_Database_Enum.mainMenu_Button_10:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_10;
                break;
            #endregion

            #region Wardrobe Menu
            case Text_Database_Enum.WardrobeMenu_Header_Wardrobe:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Header_Wardrobe;
                break;
            case Text_Database_Enum.WardrobeMenu_Message_SkinUnavailable_1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinUnavailable_1;
                break;
            case Text_Database_Enum.WardrobeMenu_Message_SkinUnavailable_2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinUnavailable_2;
                break;
            case Text_Database_Enum.WardrobeMenu_Message_SkinUnlock:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinUnlock;
                break;
            case Text_Database_Enum.WardrobeMenu_Message_SkinEquip:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinEquip;
                break;
            case Text_Database_Enum.WardrobeMenu_Message_SkinEquipped:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinEquipped;
                break;
            case Text_Database_Enum.WardrobeMenu_Message_Headgear_Unavailable:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_Headgear_Unavailable;
                break;
            case Text_Database_Enum.WardrobeMenu_Message_Wardrobe_8:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_Wardrobe_8;
                break;
            case Text_Database_Enum.WardrobeMenu_Message_Wardrobe_9:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_Wardrobe_9;
                break;
            case Text_Database_Enum.WardrobeMenu_Message_Wardrobe_10:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_Wardrobe_10;
                break;
            case Text_Database_Enum.WardrobeMenu_Message_Wardrobe_11:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_Wardrobe_11;
                break;
            case Text_Database_Enum.WardrobeMenu_Message_Wardrobe_12:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_Wardrobe_12;
                break;
            #endregion

            #region Settings
            case Text_Database_Enum.options_Header_Settings:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Header_Settings;
                break;
            case Text_Database_Enum.options_Settings_Language:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_Language;
                break;
            case Text_Database_Enum.options_Settings_TextSpeed:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_TextSpeed;
                break;
            case Text_Database_Enum.options_Settings_StepDisplay:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_StepDisplay;
                break;
            case Text_Database_Enum.options_Settings_SmoothCameraRotation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_SmoothCameraRotation;
                break;
            case Text_Database_Enum.options_Settings_RevertedCameraRotation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_RevertedCameraRotation;
                break;
            case Text_Database_Enum.options_Settings_SkipLevelIntro:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_SkipLevelIntro;
                break;
            case Text_Database_Enum.options_Settings_7:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_7;
                break;
            case Text_Database_Enum.options_Settings_8:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_8;
                break;
            case Text_Database_Enum.options_Settings_9:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_9;
                break;
            case Text_Database_Enum.options_Settings_10:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_10;
                break;
            case Text_Database_Enum.options_Settings_11:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_11;
                break;
            case Text_Database_Enum.options_Settings_12:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_12;
                break;
            case Text_Database_Enum.options_Settings_13:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_13;
                break;
            case Text_Database_Enum.options_Settings_14:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_14;
                break;
            case Text_Database_Enum.options_Settings_15:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_15;
                break;
            case Text_Database_Enum.options_Settings_16:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_16;
                break;
            case Text_Database_Enum.options_Settings_17:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_17;
                break;
            case Text_Database_Enum.options_Settings_18:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_18;
                break;
            case Text_Database_Enum.options_Settings_19:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_19;
                break;
            case Text_Database_Enum.options_Settings_20:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_20;
                break;
            #endregion

            #region Controls
            case Text_Database_Enum.options_Header_Controls:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Header_Controls;
                break;
            case Text_Database_Enum.options_Controls_1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_1;
                break;
            case Text_Database_Enum.options_Controls_2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_2;
                break;
            case Text_Database_Enum.options_Controls_3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_3;
                break;
            case Text_Database_Enum.options_Controls_4:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_4;
                break;
            case Text_Database_Enum.options_Controls_5:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_5;
                break;
            case Text_Database_Enum.options_Controls_6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_6;
                break;
            case Text_Database_Enum.options_Controls_7:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_7;
                break;
            case Text_Database_Enum.options_Controls_8:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_8;
                break;
            case Text_Database_Enum.options_Controls_9:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_9;
                break;
            case Text_Database_Enum.options_Controls_10:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_10;
                break;
            case Text_Database_Enum.options_Controls_11:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_11;
                break;
            case Text_Database_Enum.options_Controls_12:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_12;
                break;
            case Text_Database_Enum.options_Controls_13:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_13;
                break;
            case Text_Database_Enum.options_Controls_14:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_14;
                break;
            case Text_Database_Enum.options_Controls_15:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_15;
                break;
            case Text_Database_Enum.options_Controls_16:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_16;
                break;
            case Text_Database_Enum.options_Controls_17:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_17;
                break;
            case Text_Database_Enum.options_Controls_18:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_18;
                break;
            case Text_Database_Enum.options_Controls_19:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_19;
                break;
            case Text_Database_Enum.options_Controls_20:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_20;
                break;
            case Text_Database_Enum.options_Controls_21:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_21;
                break;
            case Text_Database_Enum.options_Controls_22:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_22;
                break;
            case Text_Database_Enum.options_Controls_23:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_23;
                break;
            case Text_Database_Enum.options_Controls_24:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_24;
                break;
            case Text_Database_Enum.options_Controls_25:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_25;
                break;
            #endregion

            #region Video
            case Text_Database_Enum.options_Header_Video:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Header_Video;
                break;
            case Text_Database_Enum.options_Video_1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_1;
                break;
            case Text_Database_Enum.options_Video_2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_2;
                break;
            case Text_Database_Enum.options_Video_3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_3;
                break;
            case Text_Database_Enum.options_Video_4:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_4;
                break;
            case Text_Database_Enum.options_Video_5:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_5;
                break;
            case Text_Database_Enum.options_Video_6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_6;
                break;
            case Text_Database_Enum.options_Video_7:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_7;
                break;
            case Text_Database_Enum.options_Video_8:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_8;
                break;
            case Text_Database_Enum.options_Video_9:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_9;
                break;
            case Text_Database_Enum.options_Video_10:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_10;
                break;
            case Text_Database_Enum.options_Video_11:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_11;
                break;
            case Text_Database_Enum.options_Video_12:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_12;
                break;
            case Text_Database_Enum.options_Video_13:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_13;
                break;
            case Text_Database_Enum.options_Video_14:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_14;
                break;
            case Text_Database_Enum.options_Video_15:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_15;
                break;
            case Text_Database_Enum.options_Video_16:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_16;
                break;
            case Text_Database_Enum.options_Video_17:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_17;
                break;
            case Text_Database_Enum.options_Video_18:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_18;
                break;
            case Text_Database_Enum.options_Video_19:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_19;
                break;
            case Text_Database_Enum.options_Video_20:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_20;
                break;
            #endregion

            #region Audio
            case Text_Database_Enum.options_Header_Audio:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Header_Audio;
                break;
            case Text_Database_Enum.options_Audio_1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_1;
                break;
            case Text_Database_Enum.options_Audio_2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_2;
                break;
            case Text_Database_Enum.options_Audio_3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_3;
                break;
            case Text_Database_Enum.options_Audio_4:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_4;
                break;
            case Text_Database_Enum.options_Audio_5:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_5;
                break;
            case Text_Database_Enum.options_Audio_6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_6;
                break;
            case Text_Database_Enum.options_Audio_7:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_7;
                break;
            case Text_Database_Enum.options_Audio_8:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_8;
                break;
            case Text_Database_Enum.options_Audio_9:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_9;
                break;
            case Text_Database_Enum.options_Audio_10:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_10;
                break;
            case Text_Database_Enum.options_Audio_11:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_11;
                break;
            case Text_Database_Enum.options_Audio_12:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_12;
                break;
            case Text_Database_Enum.options_Audio_13:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_13;
                break;
            case Text_Database_Enum.options_Audio_14:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_14;
                break;
            case Text_Database_Enum.options_Audio_15:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_15;
                break;
            case Text_Database_Enum.options_Audio_16:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_16;
                break;
            case Text_Database_Enum.options_Audio_17:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_17;
                break;
            case Text_Database_Enum.options_Audio_18:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_18;
                break;
            case Text_Database_Enum.options_Audio_19:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_19;
                break;
            case Text_Database_Enum.options_Audio_20:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_20;
                break;
            #endregion

            #region Blank
            case Text_Database_Enum.options_Header_Blank:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Header_Blank;
                break;
            case Text_Database_Enum.options_Blank_1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_1;
                break;
            case Text_Database_Enum.options_Blank_2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_2;
                break;
            case Text_Database_Enum.options_Blank_3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_3;
                break;
            case Text_Database_Enum.options_Blank_4:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_4;
                break;
            case Text_Database_Enum.options_Blank_5:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_5;
                break;
            case Text_Database_Enum.options_Blank_6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_6;
                break;
            case Text_Database_Enum.options_Blank_7:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_7;
                break;
            case Text_Database_Enum.options_Blank_8:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_8;
                break;
            case Text_Database_Enum.options_Blank_9:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_9;
                break;
            case Text_Database_Enum.options_Blank_10:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_10;
                break;
            case Text_Database_Enum.options_Blank_11:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_11;
                break;
            case Text_Database_Enum.options_Blank_12:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_12;
                break;
            case Text_Database_Enum.options_Blank_13:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_13;
                break;
            case Text_Database_Enum.options_Blank_14:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_14;
                break;
            case Text_Database_Enum.options_Blank_15:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_15;
                break;
            case Text_Database_Enum.options_Blank_16:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_16;
                break;
            case Text_Database_Enum.options_Blank_17:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_17;
                break;
            case Text_Database_Enum.options_Blank_18:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_18;
                break;
            case Text_Database_Enum.options_Blank_19:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_19;
                break;
            case Text_Database_Enum.options_Blank_20:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_20;
                break;
            #endregion

            #region Overworld Menu
            case Text_Database_Enum.overworld_1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_1;
                break;
            case Text_Database_Enum.overworld_2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_2;
                break;
            case Text_Database_Enum.overworld_3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_3;
                break;
            case Text_Database_Enum.overworld_4:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_4;
                break;
            case Text_Database_Enum.overworld_5:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_5;
                break;
            case Text_Database_Enum.overworld_6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_6;
                break;
            case Text_Database_Enum.overworld_7:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_7;
                break;
            case Text_Database_Enum.overworld_8:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_8;
                break;
            case Text_Database_Enum.overworld_9:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_9;
                break;
            case Text_Database_Enum.overworld_10:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_10;
                break;
            case Text_Database_Enum.overworld_11:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_11;
                break;
            case Text_Database_Enum.overworld_12:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_12;
                break;
            case Text_Database_Enum.overworld_13:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_13;
                break;
            case Text_Database_Enum.overworld_14:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_14;
                break;
            case Text_Database_Enum.overworld_15:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_15;
                break;
            case Text_Database_Enum.overworld_16:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_16;
                break;
            case Text_Database_Enum.overworld_17:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_17;
                break;
            case Text_Database_Enum.overworld_18:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_18;
                break;
            case Text_Database_Enum.overworld_19:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_19;
                break;
            case Text_Database_Enum.overworld_20:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_20;
                break;
            #endregion

            #region Pause Menu
            case Text_Database_Enum.pauseMenu_Button_BackToGame:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_BackToGame;
                break;
            case Text_Database_Enum.pauseMenu_Button_ExitLevel:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_ExitLevel;
                break;
            case Text_Database_Enum.pauseMenu_Button_3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_3;
                break;
            case Text_Database_Enum.pauseMenu_Button_4:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_4;
                break;
            case Text_Database_Enum.pauseMenu_Button_5:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_5;
                break;
            case Text_Database_Enum.pauseMenu_Button_6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_6;
                break;
            case Text_Database_Enum.pauseMenu_Button_7:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_7;
                break;
            case Text_Database_Enum.pauseMenu_Button_8:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_8;
                break;
            case Text_Database_Enum.pauseMenu_Button_9:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_9;
                break;
            case Text_Database_Enum.pauseMenu_Button_10:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_10;
                break;
            #endregion

            #region Skin Names
            case Text_Database_Enum.skinName_Default:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_Default;
                break;
            case Text_Database_Enum.skinName_WaterRegion_Lv1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_WaterRegion_Lv1;
                break;
            case Text_Database_Enum.skinName_WaterRegion_Lv2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_WaterRegion_Lv2;
                break;
            case Text_Database_Enum.skinName_WaterRegion_Lv3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_WaterRegion_Lv3;
                break;
            case Text_Database_Enum.skinName_WaterRegion_Lv4:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_WaterRegion_Lv4;
                break;
            case Text_Database_Enum.skinName_WaterRegion_Lv5:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_WaterRegion_Lv5;
                break;
            case Text_Database_Enum.skinName_WaterRegion_Lv6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_WaterRegion_Lv6;
                break;
            case Text_Database_Enum.skinName_SandRegion_Lv1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SandRegion_Lv1;
                break;
            case Text_Database_Enum.skinName_SandRegion_Lv2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SandRegion_Lv2;
                break;
            case Text_Database_Enum.skinName_SandRegion_Lv3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SandRegion_Lv3;
                break;
            case Text_Database_Enum.skinName_SandRegion_Lv4:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SandRegion_Lv4;
                break;
            case Text_Database_Enum.skinName_SandRegion_Lv5:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SandRegion_Lv5;
                break;
            case Text_Database_Enum.skinName_SandRegion_Lv6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SandRegion_Lv6;
                break;
            case Text_Database_Enum.skinName_IceRegion_Lv1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_IceRegion_Lv1;
                break;
            case Text_Database_Enum.skinName_IceRegion_Lv2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_IceRegion_Lv2;
                break;
            case Text_Database_Enum.skinName_IceRegion_Lv3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_IceRegion_Lv3;
                break;
            case Text_Database_Enum.skinName_IceRegion_Lv4:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_IceRegion_Lv4;
                break;
            case Text_Database_Enum.skinName_IceRegion_Lv5:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_IceRegion_Lv5;
                break;
            case Text_Database_Enum.skinName_IceRegion_Lv6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_IceRegion_Lv6;
                break;
            case Text_Database_Enum.skinName_LavaRegion_Lv1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_LavaRegion_Lv1;
                break;
            case Text_Database_Enum.skinName_LavaRegion_Lv2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_LavaRegion_Lv2;
                break;
            case Text_Database_Enum.skinName_LavaRegion_Lv3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_LavaRegion_Lv3;
                break;
            case Text_Database_Enum.skinName_LavaRegion_Lv4:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_LavaRegion_Lv4;
                break;
            case Text_Database_Enum.skinName_LavaRegion_Lv5:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_LavaRegion_Lv5;
                break;
            case Text_Database_Enum.skinName_LavaRegion_Lv6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_LavaRegion_Lv6;
                break;
            case Text_Database_Enum.skinName_SwampRegion_Lv1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SwampRegion_Lv1;
                break;
            case Text_Database_Enum.skinName_SwampRegion_Lv2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SwampRegion_Lv2;
                break;
            case Text_Database_Enum.skinName_SwampRegion_Lv3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SwampRegion_Lv3;
                break;
            case Text_Database_Enum.skinName_SwampRegion_Lv4:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SwampRegion_Lv4;
                break;
            case Text_Database_Enum.skinName_SwampRegion_Lv5:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SwampRegion_Lv5;
                break;
            case Text_Database_Enum.skinName_SwampRegion_Lv6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SwampRegion_Lv6;
                break;
            case Text_Database_Enum.skinName_MetalRegion_Lv1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_MetalRegion_Lv1;
                break;
            case Text_Database_Enum.skinName_MetalRegion_Lv2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_MetalRegion_Lv2;
                break;
            case Text_Database_Enum.skinName_MetalRegion_Lv3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_MetalRegion_Lv3;
                break;
            case Text_Database_Enum.skinName_MetalRegion_Lv4:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_MetalRegion_Lv4;
                break;
            case Text_Database_Enum.skinName_MetalRegion_Lv5:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_MetalRegion_Lv5;
                break;
            case Text_Database_Enum.skinName_MetalRegion_Lv6:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_MetalRegion_Lv6;
                break;
            #endregion

            #region Ability Names
            case Text_Database_Enum.ability_Name_Snorkel:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_Snorkel;
                break;
            case Text_Database_Enum.ability_Name_OxygenTank:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_OxygenTank;
                break;
            case Text_Database_Enum.ability_Name_Flippers:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_Flippers;
                break;
            case Text_Database_Enum.ability_Name_DrillHelmet:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_DrillHelmet;
                break;
            case Text_Database_Enum.ability_Name_DrillBoots:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_DrillBoots;
                break;
            case Text_Database_Enum.ability_Name_HandDrill:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_HandDrill;
                break;
            case Text_Database_Enum.ability_Name_GrapplingHook:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_GrapplingHook;
                break;
            case Text_Database_Enum.ability_Name_SpringShoes:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_SpringShoes;
                break;
            case Text_Database_Enum.ability_Name_ClimbingGloves:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_ClimbingGloves;
                break;
            case Text_Database_Enum.ability_Name_10:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_10;
                break;
            case Text_Database_Enum.ability_Name_11:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_11;
                break;
            case Text_Database_Enum.ability_Name_12:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_12;
                break;
            case Text_Database_Enum.ability_Name_13:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_13;
                break;
            case Text_Database_Enum.ability_Name_14:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_14;
                break;
            #endregion

            #region Ability Messages
            case Text_Database_Enum.ability_Message_Snorkel_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_Snorkel_Keyboard;
                break;
            case Text_Database_Enum.ability_Message_Snorkel_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_Snorkel_PlayStation;
                break;
            case Text_Database_Enum.ability_Message_Snorkel_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_Snorkel_xBox;
                break;
            case Text_Database_Enum.ability_Message_OxygenTank_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_OxygenTank_Keyboard;
                break;
            case Text_Database_Enum.ability_Message_OxygenTank_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_OxygenTank_PlayStation;
                break;
            case Text_Database_Enum.ability_Message_OxygenTank_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_OxygenTank_xBox;
                break;
            case Text_Database_Enum.ability_Message_Flippers_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_Flippers_Keyboard;
                break;
            case Text_Database_Enum.ability_Message_Flippers_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_Flippers_PlayStation;
                break;
            case Text_Database_Enum.ability_Message_Flippers_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_Flippers_xBox;
                break;
            case Text_Database_Enum.ability_Message_DrillHelmet_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_DrillHelmet_Keyboard;
                break;
            case Text_Database_Enum.ability_Message_DrillHelmet_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_DrillHelmet_PlayStation;
                break;
            case Text_Database_Enum.ability_Message_DrillHelmet_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_DrillHelmet_xBox;
                break;
            case Text_Database_Enum.ability_Message_DrillBoots_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_DrillBoots_Keyboard;
                break;
            case Text_Database_Enum.ability_Message_DrillBoots_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_DrillBoots_PlayStation;
                break;
            case Text_Database_Enum.ability_Message_DrillBoots_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_DrillBoots_xBox;
                break;
            case Text_Database_Enum.ability_Message_HandDrill_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_HandDrill_Keyboard;
                break;
            case Text_Database_Enum.ability_Message_HandDrill_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_HandDrill_PlayStation;
                break;
            case Text_Database_Enum.ability_Message_HandDrill_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_HandDrill_xBox;
                break;
            case Text_Database_Enum.ability_Message_GrapplingHook_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_GrapplingHook_Keyboard;
                break;
            case Text_Database_Enum.ability_Message_GrapplingHook_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_GrapplingHook_PlayStation;
                break;
            case Text_Database_Enum.ability_Message_GrapplingHook_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_GrapplingHook_xBox;
                break;
            case Text_Database_Enum.ability_Message_SpringShoes_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_SpringShoes_Keyboard;
                break;
            case Text_Database_Enum.ability_Message_SpringShoes_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_SpringShoes_PlayStation;
                break;
            case Text_Database_Enum.ability_Message_SpringShoes_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_SpringShoes_xBox;
                break;
            case Text_Database_Enum.ability_Message_ClimbingGloves_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_ClimbingGloves_Keyboard;
                break;
            case Text_Database_Enum.ability_Message_ClimbingGloves_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_ClimbingGloves_PlayStation;
                break;
            case Text_Database_Enum.ability_Message_ClimbingGloves_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_ClimbingGloves_xBox;
                break;
            case Text_Database_Enum.ability_Message_10_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_10_Keyboard;
                break;
            case Text_Database_Enum.ability_Message_10_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_10_PlayStation;
                break;
            case Text_Database_Enum.ability_Message_10_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_10_xBox;
                break;
            case Text_Database_Enum.ability_Message_11_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_11_Keyboard;
                break;
            case Text_Database_Enum.ability_Message_11_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_11_PlayStation;
                break;
            case Text_Database_Enum.ability_Message_11_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_11_xBox;
                break;
            case Text_Database_Enum.ability_Message_12_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_12_Keyboard;
                break;
            case Text_Database_Enum.ability_Message_12_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_12_PlayStation;
                break;
            case Text_Database_Enum.ability_Message_12_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_12_xBox;
                break;
            case Text_Database_Enum.ability_Message_13_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_13_Keyboard;
                break;
            case Text_Database_Enum.ability_Message_13_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_13_PlayStation;
                break;
            case Text_Database_Enum.ability_Message_13_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_13_xBox;
                break;
            case Text_Database_Enum.ability_Message_14_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_14_Keyboard;
                break;
            case Text_Database_Enum.ability_Message_14_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_14_PlayStation;
                break;
            case Text_Database_Enum.ability_Message_14_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_14_xBox;
                break;
            #endregion

            #region Pickup Messages
            case Text_Database_Enum.pickup_Message_Eccence_1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_Eccence_1;
                break;
            case Text_Database_Enum.pickup_Message_Eccence_2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_Eccence_2;
                break;
            case Text_Database_Enum.pickup_Message_Eccence_3:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_Eccence_3;
                break;
            case Text_Database_Enum.pickup_Message_Footprint:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_Footprint;
                break;
            case Text_Database_Enum.pickup_Message_Skin_First:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_Skin_First;
                break;
            case Text_Database_Enum.pickup_Message_Skin:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_Skin;
                break;
            case Text_Database_Enum.pickup_Message_7:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_7;
                break;
            case Text_Database_Enum.pickup_Message_8:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_8;
                break;
            case Text_Database_Enum.pickup_Message_9:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_9;
                break;
            case Text_Database_Enum.pickup_Message_10:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_10;
                break;
            case Text_Database_Enum.pickup_Message_11:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_11;
                break;
            case Text_Database_Enum.pickup_Message_12:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_12;
                break;
            case Text_Database_Enum.pickup_Message_13:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_13;
                break;
            case Text_Database_Enum.pickup_Message_14:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_14;
                break;
            case Text_Database_Enum.pickup_Message_15:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_15;
                break;
            #endregion

            #region Tutorial Messages
            case Text_Database_Enum.tutorial_Message_Movement_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Movement_Keyboard;
                break;
            case Text_Database_Enum.tutorial_Message_Movement_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Movement_PlayStation;
                break;
            case Text_Database_Enum.tutorial_Message_Movement_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Movement_xBox;
                break;
            case Text_Database_Enum.tutorial_Message_CameraRotation_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_CameraRotation_Keyboard;
                break;
            case Text_Database_Enum.tutorial_Message_CameraRotation_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_CameraRotation_PlayStation;
                break;
            case Text_Database_Enum.tutorial_Message_CameraRotation_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_CameraRotation_xBox;
                break;
            case Text_Database_Enum.tutorial_Message_Respawn_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Respawn_Keyboard;
                break;
            case Text_Database_Enum.tutorial_Message_Respawn_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Respawn_PlayStation;
                break;
            case Text_Database_Enum.tutorial_Message_Respawn_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Respawn_xBox;
                break;
            case Text_Database_Enum.tutorial_Message_FreeCam_1_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_FreeCam_1_Keyboard;
                break;
            case Text_Database_Enum.tutorial_Message_FreeCam_1_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_FreeCam_1_PlayStation;
                break;
            case Text_Database_Enum.tutorial_Message_FreeCam_1_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_FreeCam_1_xBox;
                break;
            case Text_Database_Enum.tutorial_Message_FreeCam_2_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_FreeCam_2_Keyboard;
                break;
            case Text_Database_Enum.tutorial_Message_FreeCam_2_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_FreeCam_2_PlayStation;
                break;
            case Text_Database_Enum.tutorial_Message_FreeCam_2_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_FreeCam_2_xBox;
                break;
            case Text_Database_Enum.tutorial_Message_Demo_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Demo_Keyboard;
                break;
            case Text_Database_Enum.tutorial_Message_Demo_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Demo_PlayStation;
                break;
            case Text_Database_Enum.tutorial_Message_Demo_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Demo_xBox;
                break;
            case Text_Database_Enum.tutorial_Message_7_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_7_Keyboard;
                break;
            case Text_Database_Enum.tutorial_Message_7_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_7_PlayStation;
                break;
            case Text_Database_Enum.tutorial_Message_7_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_7_xBox;
                break;
            case Text_Database_Enum.tutorial_Message_8_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_8_Keyboard;
                break;
            case Text_Database_Enum.tutorial_Message_8_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_8_PlayStation;
                break;
            case Text_Database_Enum.tutorial_Message_8_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_8_xBox;
                break;
            case Text_Database_Enum.tutorial_Message_9_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_9_Keyboard;
                break;
            case Text_Database_Enum.tutorial_Message_9_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_9_PlayStation;
                break;
            case Text_Database_Enum.tutorial_Message_9_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_9_xBox;
                break;
            case Text_Database_Enum.tutorial_Message_10_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_10_Keyboard;
                break;
            case Text_Database_Enum.tutorial_Message_10_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_10_PlayStation;
                break;
            case Text_Database_Enum.tutorial_Message_10_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_10_xBox;
                break;
            case Text_Database_Enum.tutorial_Message_11_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_11_Keyboard;
                break;
            case Text_Database_Enum.tutorial_Message_11_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_11_PlayStation;
                break;
            case Text_Database_Enum.tutorial_Message_11_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_11_xBox;
                break;
            case Text_Database_Enum.tutorial_Message_12_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_12_Keyboard;
                break;
            case Text_Database_Enum.tutorial_Message_12_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_12_PlayStation;
                break;
            case Text_Database_Enum.tutorial_Message_12_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_12_xBox;
                break;
            case Text_Database_Enum.tutorial_Message_13_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_13_Keyboard;
                break;
            case Text_Database_Enum.tutorial_Message_13_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_13_PlayStation;
                break;
            case Text_Database_Enum.tutorial_Message_13_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_13_xBox;
                break;
            case Text_Database_Enum.tutorial_Message_14_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_14_Keyboard;
                break;
            case Text_Database_Enum.tutorial_Message_14_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_14_PlayStation;
                break;
            case Text_Database_Enum.tutorial_Message_14_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_14_xBox;
                break;
            case Text_Database_Enum.tutorial_Message_15_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_15_Keyboard;
                break;
            case Text_Database_Enum.tutorial_Message_15_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_15_PlayStation;
                break;
            case Text_Database_Enum.tutorial_Message_15_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_15_xBox;
                break;
            #endregion

            #region InterractableButton Message
            case Text_Database_Enum.interractableButton_Message_Talk_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Talk_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_Talk_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Talk_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_Talk_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Talk_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_Interract_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Interract_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_Interract_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Interract_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_Interract_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Interract_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_FlippersUP_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_FlippersUP_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_FlippersUP_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_FlippersUP_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_FlippersUP_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_FlippersUP_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_FlippersDOWN_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_FlippersDOWN_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_FlippersDOWN_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_FlippersDOWN_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_FlippersDOWN_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_FlippersDOWN_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_DrillHelmet_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_DrillHelmet_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_DrillHelmet_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_DrillHelmet_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_DrillHelmet_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_DrillHelmet_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_DrillShoes_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_DrillShoes_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_DrillShoes_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_DrillShoes_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_DrillShoes_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_DrillShoes_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_HandDrill_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_HandDrill_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_HandDrill_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_HandDrill_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_HandDrill_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_HandDrill_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_GrapplingHook_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_GrapplingHook_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_GrapplingHook_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_GrapplingHook_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_GrapplingHook_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_GrapplingHook_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_SpringShoes_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_SpringShoes_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_SpringShoes_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_SpringShoes_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_SpringShoes_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_SpringShoes_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_ClimbingGloves_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_ClimbingGloves_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_ClimbingGloves_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_ClimbingGloves_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_ClimbingGloves_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_ClimbingGloves_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_Respawn_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Respawn_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_Respawn_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Respawn_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_Respawn_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Respawn_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_12_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_12_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_12_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_12_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_12_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_12_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_13_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_13_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_13_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_13_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_13_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_13_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_14_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_14_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_14_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_14_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_14_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_14_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_15_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_15_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_15_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_15_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_15_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_15_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_16_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_16_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_16_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_16_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_16_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_16_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_17_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_17_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_17_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_17_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_17_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_17_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_18_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_18_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_18_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_18_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_18_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_18_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_19_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_19_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_19_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_19_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_19_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_19_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_20_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_20_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_20_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_20_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_20_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_20_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_21_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_21_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_21_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_21_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_21_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_21_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_22_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_22_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_22_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_22_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_22_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_22_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_23_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_23_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_23_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_23_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_23_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_23_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_24_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_24_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_24_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_24_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_24_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_24_xBox;
                break;
            case Text_Database_Enum.interractableButton_Message_25_Keyboard:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_25_Keyboard;
                break;
            case Text_Database_Enum.interractableButton_Message_25_PlayStation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_25_PlayStation;
                break;
            case Text_Database_Enum.interractableButton_Message_25_xBox:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_25_xBox;
                break;
            #endregion

            default:
                GetComponent<TextMeshProUGUI>().text = "";
                break;
        }
    }
}