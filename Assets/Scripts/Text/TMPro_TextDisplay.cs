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

            //Region Names
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

            //Names of levels in Water Region
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

            //Names of levels in Sand Region
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

            //Names of levels in Ice Region
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

            //Names of levels in Lava Region
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

            //Names of levels in Swamp Region
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

            //Names of levels in Metal Region
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

            //Main Menu Buttons
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

            //Wardrobe Menu
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

            //Settings
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

            //Controls
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

            //Video
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

            //Audio
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

            //Blank
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

            //Overworld Menu
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

            //Pause Menu
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

            //Skin Names
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

            //Ability Names
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

            //Ability Messages
            case Text_Database_Enum.ability_Message_Snorkel:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_Snorkel;
                break;
            case Text_Database_Enum.ability_Message_OxygenTank:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_OxygenTank;
                break;
            case Text_Database_Enum.ability_Message_Flippers:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_Flippers;
                break;
            case Text_Database_Enum.ability_Message_DrillHelmet:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_DrillHelmet;
                break;
            case Text_Database_Enum.ability_Message_DrillBoots:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_DrillBoots;
                break;
            case Text_Database_Enum.ability_Message_HandDrill:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_HandDrill;
                break;
            case Text_Database_Enum.ability_Message_GrapplingHook:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_GrapplingHook;
                break;
            case Text_Database_Enum.ability_Message_SpringShoes:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_SpringShoes;
                break;
            case Text_Database_Enum.ability_Message_ClimbingGloves:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_ClimbingGloves;
                break;
            case Text_Database_Enum.ability_Message_10:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_10;
                break;
            case Text_Database_Enum.ability_Message_11:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_11;
                break;
            case Text_Database_Enum.ability_Message_12:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_12;
                break;
            case Text_Database_Enum.ability_Message_13:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_13;
                break;
            case Text_Database_Enum.ability_Message_14:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_14;
                break;

            //Pickup Messages
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

            //Tutorial Messages
            case Text_Database_Enum.tutorial_Message_Movement:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Movement;
                break;
            case Text_Database_Enum.tutorial_Message_CameraRotation:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_CameraRotation;
                break;
            case Text_Database_Enum.tutorial_Message_Respawn:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Respawn;
                break;
            case Text_Database_Enum.tutorial_Message_FreeCam_1:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_FreeCam_1;
                break;
            case Text_Database_Enum.tutorial_Message_FreeCam_2:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_FreeCam_2;
                break;
            case Text_Database_Enum.tutorial_Message_Demo:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Demo;
                break;
            case Text_Database_Enum.tutorial_Message_7:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_7;
                break;
            case Text_Database_Enum.tutorial_Message_8:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_8;
                break;
            case Text_Database_Enum.tutorial_Message_9:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_9;
                break;
            case Text_Database_Enum.tutorial_Message_10:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_10;
                break;
            case Text_Database_Enum.tutorial_Message_11:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_11;
                break;
            case Text_Database_Enum.tutorial_Message_12:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_12;
                break;
            case Text_Database_Enum.tutorial_Message_13:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_13;
                break;
            case Text_Database_Enum.tutorial_Message_14:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_14;
                break;
            case Text_Database_Enum.tutorial_Message_15:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_15;
                break;

            //InterractableButton Message
            case Text_Database_Enum.interractableButton_Message_Talk:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Talk;
                break;
            case Text_Database_Enum.interractableButton_Message_Interract:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Interract;
                break;
            case Text_Database_Enum.interractableButton_Message_FlippersUP:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_FlippersUP;
                break;
            case Text_Database_Enum.interractableButton_Message_FlippersDOWN:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_FlippersDOWN;
                break;
            case Text_Database_Enum.interractableButton_Message_DrillHelmet:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_DrillHelmet;
                break;
            case Text_Database_Enum.interractableButton_Message_DrillShoes:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_DrillShoes;
                break;
            case Text_Database_Enum.interractableButton_Message_HandDrill:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_HandDrill;
                break;
            case Text_Database_Enum.interractableButton_Message_GrapplingHook:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_GrapplingHook;
                break;
            case Text_Database_Enum.interractableButton_Message_SpringShoes:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_SpringShoes;
                break;
            case Text_Database_Enum.interractableButton_Message_ClimbingGloves:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_ClimbingGloves;
                break;
            case Text_Database_Enum.interractableButton_Message_Respawn:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Respawn;
                break;
            case Text_Database_Enum.interractableButton_Message_12:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_12;
                break;
            case Text_Database_Enum.interractableButton_Message_13:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_13;
                break;
            case Text_Database_Enum.interractableButton_Message_14:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_14;
                break;
            case Text_Database_Enum.interractableButton_Message_15:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_15;
                break;
            case Text_Database_Enum.interractableButton_Message_16:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_16;
                break;
            case Text_Database_Enum.interractableButton_Message_17:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_17;
                break;
            case Text_Database_Enum.interractableButton_Message_18:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_18;
                break;
            case Text_Database_Enum.interractableButton_Message_19:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_19;
                break;
            case Text_Database_Enum.interractableButton_Message_20:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_20;
                break;
            case Text_Database_Enum.interractableButton_Message_21:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_21;
                break;
            case Text_Database_Enum.interractableButton_Message_22:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_22;
                break;
            case Text_Database_Enum.interractableButton_Message_23:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_23;
                break;
            case Text_Database_Enum.interractableButton_Message_24:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_24;
                break;
            case Text_Database_Enum.interractableButton_Message_25:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_25;
                break;

            default:
                GetComponent<TextMeshProUGUI>().text = "";
                break;
        }
    }
}