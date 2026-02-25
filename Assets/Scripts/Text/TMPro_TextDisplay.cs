using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.activeSceneChanged += HandleSceneChanged;
    }
    private void OnDestroy()
    {
        SettingsManager.Action_SetNewLanguage -= ShowText;
        SceneManager.activeSceneChanged -= HandleSceneChanged;
    }

    private void HandleSceneChanged(Scene oldScene, Scene newScene)
    {
        SettingsManager.Action_SetNewLanguage -= ShowText;
    }



    //--------------------


    void ShowText()
    {
        if (!GetComponent<TextMeshProUGUI>()) return;
        if (DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList.Count <= 0) return;

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
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_7;
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

            #region Finish Regions Messages
            case Text_Database_Enum.finishedRegion_Message_Water:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Water;
                break;
            case Text_Database_Enum.finishedRegion_Message_Sand:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Sand;
                break;
            case Text_Database_Enum.finishedRegion_Message_Ice:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Ice;
                break;
            case Text_Database_Enum.finishedRegion_Message_Lava:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Lava;
                break;
            case Text_Database_Enum.finishedRegion_Message_Swamp:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Swamp;
                break;
            case Text_Database_Enum.finishedRegion_Message_Metal:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Metal;
                break;
            case Text_Database_Enum.finishedRegion_Message_7:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_7;
                break;
            #endregion

            #region NPC Names
            case Text_Database_Enum.NPCName_Water:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Water;
                break;
            case Text_Database_Enum.NPCName_Sand:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Sand;
                break;
            case Text_Database_Enum.NPCName_Ice:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Ice;
                break;
            case Text_Database_Enum.NPCName_Lava:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Lava;
                break;
            case Text_Database_Enum.NPCName_Swamp:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Swamp;
                break;
            case Text_Database_Enum.NPCName_Metal:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Metal;
                break;
            case Text_Database_Enum.NPCName_7:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_7;
                break;
            case Text_Database_Enum.NPCName_Antagonist:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Antagonist;
                break;
            #endregion

            #region NPC Hat Names
            case Text_Database_Enum.NPCHat_Name_Water:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_Water;
                break;
            case Text_Database_Enum.NPCHat_Name_Sand:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_Sand;
                break;
            case Text_Database_Enum.NPCHat_Name_Ice:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_Ice;
                break;
            case Text_Database_Enum.NPCHat_Name_Lava:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_Lava;
                break;
            case Text_Database_Enum.NPCHat_Name_Swamp:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_Swamp;
                break;
            case Text_Database_Enum.NPCHat_Name_Metal:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_Metal;
                break;
            case Text_Database_Enum.NPCHat_Name_7:
                GetComponent<TextMeshProUGUI>().text = DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_7;
                break;
            #endregion

            default:
                GetComponent<TextMeshProUGUI>().text = "";
                break;
        }
    }
    public string GetText()
    {
        switch (textToDisplay)
        {
            //None
            case Text_Database_Enum.None:
                return "";

            #region Region Names
            case Text_Database_Enum.name_Region_Water:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water;
            case Text_Database_Enum.name_Region_Sand:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand;
            case Text_Database_Enum.name_Region_Ice:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice;
            case Text_Database_Enum.name_Region_Lava:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava;
            case Text_Database_Enum.name_Region_Swamp:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp;
            case Text_Database_Enum.name_Region_Metal:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal;
            case Text_Database_Enum.name_Region_6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_7;
            #endregion

            #region Names of levels in Water Region
            case Text_Database_Enum.name_Region_Water_Lv1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water_Lv1;
            case Text_Database_Enum.name_Region_Water_Lv2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water_Lv2;
            case Text_Database_Enum.name_Region_Water_Lv3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water_Lv3;
            case Text_Database_Enum.name_Region_Water_Lv4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water_Lv4;
            case Text_Database_Enum.name_Region_Water_Lv5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water_Lv5;
            case Text_Database_Enum.name_Region_Water_Lv6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water_Lv6;
            #endregion

            #region Names of levels in Sand Region
            case Text_Database_Enum.name_Region_Sand_Lv1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand_Lv1;
            case Text_Database_Enum.name_Region_Sand_Lv2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand_Lv2;
            case Text_Database_Enum.name_Region_Sand_Lv3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand_Lv3;
            case Text_Database_Enum.name_Region_Sand_Lv4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand_Lv4;
            case Text_Database_Enum.name_Region_Sand_Lv5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand_Lv5;
            case Text_Database_Enum.name_Region_Sand_Lv6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand_Lv6;
            #endregion

            #region Names of levels in Ice Region
            case Text_Database_Enum.name_Region_Ice_Lv1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice_Lv1;
            case Text_Database_Enum.name_Region_Ice_Lv2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice_Lv2;
            case Text_Database_Enum.name_Region_Ice_Lv3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice_Lv3;
            case Text_Database_Enum.name_Region_Ice_Lv4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice_Lv4;
            case Text_Database_Enum.name_Region_Ice_Lv5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice_Lv5;
            case Text_Database_Enum.name_Region_Ice_Lv6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice_Lv6;
            #endregion

            #region Names of levels in Lava Region
            case Text_Database_Enum.name_Region_Lava_Lv1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava_Lv1;
            case Text_Database_Enum.name_Region_Lava_Lv2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava_Lv2;
            case Text_Database_Enum.name_Region_Lava_Lv3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava_Lv3;
            case Text_Database_Enum.name_Region_Lava_Lv4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava_Lv4;
            case Text_Database_Enum.name_Region_Lava_Lv5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava_Lv5;
            case Text_Database_Enum.name_Region_Lava_Lv6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava_Lv6;
            #endregion

            #region Names of levels in Swamp Region
            case Text_Database_Enum.name_Region_Swamp_Lv1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp_Lv1;
            case Text_Database_Enum.name_Region_Swamp_Lv2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp_Lv2;
            case Text_Database_Enum.name_Region_Swamp_Lv3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp_Lv3;
            case Text_Database_Enum.name_Region_Swamp_Lv4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp_Lv4;
            case Text_Database_Enum.name_Region_Swamp_Lv5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp_Lv5;
            case Text_Database_Enum.name_Region_Swamp_Lv6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp_Lv6;
            #endregion

            #region Names of levels in Metal Region
            case Text_Database_Enum.name_Region_Metal_Lv1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal_Lv1;
            case Text_Database_Enum.name_Region_Metal_Lv2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal_Lv2;
            case Text_Database_Enum.name_Region_Metal_Lv3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal_Lv3;
            case Text_Database_Enum.name_Region_Metal_Lv4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal_Lv4;
            case Text_Database_Enum.name_Region_Metal_Lv5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal_Lv5;
            case Text_Database_Enum.name_Region_Metal_Lv6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal_Lv6;
            #endregion

            #region Main Menu Buttons
            case Text_Database_Enum.mainMenu_Button_Continue:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_Continue;
            case Text_Database_Enum.mainMenu_Button_NewGame:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_NewGame;
            case Text_Database_Enum.mainMenu_Button_Wardrobe:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_Wardrobe;
            case Text_Database_Enum.mainMenu_Button_Options:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_Options;
            case Text_Database_Enum.mainMenu_Button_Quit:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_Quit;
            case Text_Database_Enum.mainMenu_Button_6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_6;
            case Text_Database_Enum.mainMenu_Button_7:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_7;
            case Text_Database_Enum.mainMenu_Button_8:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_8;
            case Text_Database_Enum.mainMenu_Button_9:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_9;
            case Text_Database_Enum.mainMenu_Button_10:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].mainMenu_Button_10;
            #endregion

            #region Wardrobe Menu
            case Text_Database_Enum.WardrobeMenu_Header_Wardrobe:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Header_Wardrobe;
            case Text_Database_Enum.WardrobeMenu_Message_SkinUnavailable_1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinUnavailable_1;
            case Text_Database_Enum.WardrobeMenu_Message_SkinUnavailable_2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinUnavailable_2;
            case Text_Database_Enum.WardrobeMenu_Message_SkinUnlock:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinUnlock;
            case Text_Database_Enum.WardrobeMenu_Message_SkinEquip:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinEquip;
            case Text_Database_Enum.WardrobeMenu_Message_SkinEquipped:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_SkinEquipped;
            case Text_Database_Enum.WardrobeMenu_Message_Headgear_Unavailable:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_Headgear_Unavailable;
            case Text_Database_Enum.WardrobeMenu_Message_Wardrobe_8:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_Wardrobe_8;
            case Text_Database_Enum.WardrobeMenu_Message_Wardrobe_9:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_Wardrobe_9;
            case Text_Database_Enum.WardrobeMenu_Message_Wardrobe_10:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_Wardrobe_10;
            case Text_Database_Enum.WardrobeMenu_Message_Wardrobe_11:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_Wardrobe_11;
            case Text_Database_Enum.WardrobeMenu_Message_Wardrobe_12:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].WardrobeMenu_Message_Wardrobe_12;
            #endregion

            #region Settings
            case Text_Database_Enum.options_Header_Settings:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Header_Settings;
            case Text_Database_Enum.options_Settings_Language:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_Language;
            case Text_Database_Enum.options_Settings_TextSpeed:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_TextSpeed;
            case Text_Database_Enum.options_Settings_StepDisplay:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_StepDisplay;
            case Text_Database_Enum.options_Settings_SmoothCameraRotation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_SmoothCameraRotation;
            case Text_Database_Enum.options_Settings_RevertedCameraRotation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_RevertedCameraRotation;
            case Text_Database_Enum.options_Settings_SkipLevelIntro:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_SkipLevelIntro;
            case Text_Database_Enum.options_Settings_7:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_7;
            case Text_Database_Enum.options_Settings_8:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_8;
            case Text_Database_Enum.options_Settings_9:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_9;
            case Text_Database_Enum.options_Settings_10:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_10;
            case Text_Database_Enum.options_Settings_11:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_11;
            case Text_Database_Enum.options_Settings_12:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_12;
            case Text_Database_Enum.options_Settings_13:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_13;
            case Text_Database_Enum.options_Settings_14:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_14;
            case Text_Database_Enum.options_Settings_15:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_15;
            case Text_Database_Enum.options_Settings_16:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_16;
            case Text_Database_Enum.options_Settings_17:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_17;
            case Text_Database_Enum.options_Settings_18:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_18;
            case Text_Database_Enum.options_Settings_19:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_19;
            case Text_Database_Enum.options_Settings_20:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Settings_20;
            #endregion

            #region Controls
            case Text_Database_Enum.options_Header_Controls:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Header_Controls;
            case Text_Database_Enum.options_Controls_1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_1;
            case Text_Database_Enum.options_Controls_2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_2;
            case Text_Database_Enum.options_Controls_3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_3;
            case Text_Database_Enum.options_Controls_4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_4;
            case Text_Database_Enum.options_Controls_5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_5;
            case Text_Database_Enum.options_Controls_6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_6;
            case Text_Database_Enum.options_Controls_7:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_7;
            case Text_Database_Enum.options_Controls_8:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_8;
            case Text_Database_Enum.options_Controls_9:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_9;
            case Text_Database_Enum.options_Controls_10:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_10;
            case Text_Database_Enum.options_Controls_11:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_11;
            case Text_Database_Enum.options_Controls_12:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_12;
            case Text_Database_Enum.options_Controls_13:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_13;
            case Text_Database_Enum.options_Controls_14:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_14;
            case Text_Database_Enum.options_Controls_15:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_15;
            case Text_Database_Enum.options_Controls_16:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_16;
            case Text_Database_Enum.options_Controls_17:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_17;
            case Text_Database_Enum.options_Controls_18:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_18;
            case Text_Database_Enum.options_Controls_19:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_19;
            case Text_Database_Enum.options_Controls_20:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_20;
            case Text_Database_Enum.options_Controls_21:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_21;
            case Text_Database_Enum.options_Controls_22:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_22;
            case Text_Database_Enum.options_Controls_23:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_23;
            case Text_Database_Enum.options_Controls_24:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_24;
            case Text_Database_Enum.options_Controls_25:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Controls_25;
            #endregion

            #region Video
            case Text_Database_Enum.options_Header_Video:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Header_Video;
            case Text_Database_Enum.options_Video_1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_1;
            case Text_Database_Enum.options_Video_2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_2;
            case Text_Database_Enum.options_Video_3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_3;
            case Text_Database_Enum.options_Video_4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_4;
            case Text_Database_Enum.options_Video_5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_5;
            case Text_Database_Enum.options_Video_6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_6;
            case Text_Database_Enum.options_Video_7:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_7;
            case Text_Database_Enum.options_Video_8:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_8;
            case Text_Database_Enum.options_Video_9:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_9;
            case Text_Database_Enum.options_Video_10:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_10;
            case Text_Database_Enum.options_Video_11:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_11;
            case Text_Database_Enum.options_Video_12:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_12;
            case Text_Database_Enum.options_Video_13:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_13;
            case Text_Database_Enum.options_Video_14:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_14;
            case Text_Database_Enum.options_Video_15:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_15;
            case Text_Database_Enum.options_Video_16:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_16;
            case Text_Database_Enum.options_Video_17:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_17;
            case Text_Database_Enum.options_Video_18:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_18;
            case Text_Database_Enum.options_Video_19:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_19;
            case Text_Database_Enum.options_Video_20:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Video_20;
            #endregion

            #region Audio
            case Text_Database_Enum.options_Header_Audio:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Header_Audio;
            case Text_Database_Enum.options_Audio_1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_1;
            case Text_Database_Enum.options_Audio_2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_2;
            case Text_Database_Enum.options_Audio_3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_3;
            case Text_Database_Enum.options_Audio_4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_4;
            case Text_Database_Enum.options_Audio_5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_5;
            case Text_Database_Enum.options_Audio_6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_6;
            case Text_Database_Enum.options_Audio_7:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_7;
            case Text_Database_Enum.options_Audio_8:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_8;
            case Text_Database_Enum.options_Audio_9:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_9;
            case Text_Database_Enum.options_Audio_10:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_10;
            case Text_Database_Enum.options_Audio_11:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_11;
            case Text_Database_Enum.options_Audio_12:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_12;
            case Text_Database_Enum.options_Audio_13:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_13;
            case Text_Database_Enum.options_Audio_14:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_14;
            case Text_Database_Enum.options_Audio_15:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_15;
            case Text_Database_Enum.options_Audio_16:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_16;
            case Text_Database_Enum.options_Audio_17:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_17;
            case Text_Database_Enum.options_Audio_18:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_18;
            case Text_Database_Enum.options_Audio_19:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_19;
            case Text_Database_Enum.options_Audio_20:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Audio_20;
            #endregion

            #region Blank
            case Text_Database_Enum.options_Header_Blank:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Header_Blank;
            case Text_Database_Enum.options_Blank_1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_1;
            case Text_Database_Enum.options_Blank_2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_2;
            case Text_Database_Enum.options_Blank_3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_3;
            case Text_Database_Enum.options_Blank_4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_4;
            case Text_Database_Enum.options_Blank_5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_5;
            case Text_Database_Enum.options_Blank_6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_6;
            case Text_Database_Enum.options_Blank_7:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_7;
            case Text_Database_Enum.options_Blank_8:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_8;
            case Text_Database_Enum.options_Blank_9:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_9;
            case Text_Database_Enum.options_Blank_10:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_10;
            case Text_Database_Enum.options_Blank_11:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_11;
            case Text_Database_Enum.options_Blank_12:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_12;
            case Text_Database_Enum.options_Blank_13:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_13;
            case Text_Database_Enum.options_Blank_14:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_14;
            case Text_Database_Enum.options_Blank_15:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_15;
            case Text_Database_Enum.options_Blank_16:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_16;
            case Text_Database_Enum.options_Blank_17:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_17;
            case Text_Database_Enum.options_Blank_18:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_18;
            case Text_Database_Enum.options_Blank_19:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_19;
            case Text_Database_Enum.options_Blank_20:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].options_Blank_20;
            #endregion

            #region Overworld Menu
            case Text_Database_Enum.overworld_1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_1;
            case Text_Database_Enum.overworld_2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_2;
            case Text_Database_Enum.overworld_3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_3;
            case Text_Database_Enum.overworld_4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_4;
            case Text_Database_Enum.overworld_5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_5;
            case Text_Database_Enum.overworld_6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_6;
            case Text_Database_Enum.overworld_7:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_7;
            case Text_Database_Enum.overworld_8:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_8;
            case Text_Database_Enum.overworld_9:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_9;
            case Text_Database_Enum.overworld_10:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_10;
            case Text_Database_Enum.overworld_11:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_11;
            case Text_Database_Enum.overworld_12:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_12;
            case Text_Database_Enum.overworld_13:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_13;
            case Text_Database_Enum.overworld_14:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_14;
            case Text_Database_Enum.overworld_15:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_15;
            case Text_Database_Enum.overworld_16:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_16;
            case Text_Database_Enum.overworld_17:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_17;
            case Text_Database_Enum.overworld_18:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_18;
            case Text_Database_Enum.overworld_19:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_19;
            case Text_Database_Enum.overworld_20:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].overworld_20;
            #endregion

            #region Pause Menu
            case Text_Database_Enum.pauseMenu_Button_BackToGame:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_BackToGame;
            case Text_Database_Enum.pauseMenu_Button_ExitLevel:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_ExitLevel;
            case Text_Database_Enum.pauseMenu_Button_3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_3;
            case Text_Database_Enum.pauseMenu_Button_4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_4;
            case Text_Database_Enum.pauseMenu_Button_5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_5;
            case Text_Database_Enum.pauseMenu_Button_6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_6;
            case Text_Database_Enum.pauseMenu_Button_7:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_7;
            case Text_Database_Enum.pauseMenu_Button_8:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_8;
            case Text_Database_Enum.pauseMenu_Button_9:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_9;
            case Text_Database_Enum.pauseMenu_Button_10:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pauseMenu_Button_10;
            #endregion

            #region Skin Names
            case Text_Database_Enum.skinName_Default:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_Default;
            case Text_Database_Enum.skinName_WaterRegion_Lv1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_WaterRegion_Lv1;
            case Text_Database_Enum.skinName_WaterRegion_Lv2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_WaterRegion_Lv2;
            case Text_Database_Enum.skinName_WaterRegion_Lv3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_WaterRegion_Lv3;
            case Text_Database_Enum.skinName_WaterRegion_Lv4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_WaterRegion_Lv4;
            case Text_Database_Enum.skinName_WaterRegion_Lv5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_WaterRegion_Lv5;
            case Text_Database_Enum.skinName_WaterRegion_Lv6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_WaterRegion_Lv6;
            case Text_Database_Enum.skinName_SandRegion_Lv1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SandRegion_Lv1;
            case Text_Database_Enum.skinName_SandRegion_Lv2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SandRegion_Lv2;
            case Text_Database_Enum.skinName_SandRegion_Lv3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SandRegion_Lv3;
            case Text_Database_Enum.skinName_SandRegion_Lv4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SandRegion_Lv4;
            case Text_Database_Enum.skinName_SandRegion_Lv5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SandRegion_Lv5;
            case Text_Database_Enum.skinName_SandRegion_Lv6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SandRegion_Lv6;
            case Text_Database_Enum.skinName_IceRegion_Lv1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_IceRegion_Lv1;
            case Text_Database_Enum.skinName_IceRegion_Lv2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_IceRegion_Lv2;
            case Text_Database_Enum.skinName_IceRegion_Lv3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_IceRegion_Lv3;
            case Text_Database_Enum.skinName_IceRegion_Lv4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_IceRegion_Lv4;
            case Text_Database_Enum.skinName_IceRegion_Lv5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_IceRegion_Lv5;
            case Text_Database_Enum.skinName_IceRegion_Lv6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_IceRegion_Lv6;
            case Text_Database_Enum.skinName_LavaRegion_Lv1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_LavaRegion_Lv1;
            case Text_Database_Enum.skinName_LavaRegion_Lv2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_LavaRegion_Lv2;
            case Text_Database_Enum.skinName_LavaRegion_Lv3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_LavaRegion_Lv3;
            case Text_Database_Enum.skinName_LavaRegion_Lv4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_LavaRegion_Lv4;
            case Text_Database_Enum.skinName_LavaRegion_Lv5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_LavaRegion_Lv5;
            case Text_Database_Enum.skinName_LavaRegion_Lv6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_LavaRegion_Lv6;
            case Text_Database_Enum.skinName_SwampRegion_Lv1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SwampRegion_Lv1;
            case Text_Database_Enum.skinName_SwampRegion_Lv2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SwampRegion_Lv2;
            case Text_Database_Enum.skinName_SwampRegion_Lv3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SwampRegion_Lv3;
            case Text_Database_Enum.skinName_SwampRegion_Lv4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SwampRegion_Lv4;
            case Text_Database_Enum.skinName_SwampRegion_Lv5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SwampRegion_Lv5;
            case Text_Database_Enum.skinName_SwampRegion_Lv6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SwampRegion_Lv6;
            case Text_Database_Enum.skinName_MetalRegion_Lv1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_MetalRegion_Lv1;
            case Text_Database_Enum.skinName_MetalRegion_Lv2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_MetalRegion_Lv2;
            case Text_Database_Enum.skinName_MetalRegion_Lv3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_MetalRegion_Lv3;
            case Text_Database_Enum.skinName_MetalRegion_Lv4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_MetalRegion_Lv4;
            case Text_Database_Enum.skinName_MetalRegion_Lv5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_MetalRegion_Lv5;
            case Text_Database_Enum.skinName_MetalRegion_Lv6:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_MetalRegion_Lv6;
            #endregion

            #region Ability Names
            case Text_Database_Enum.ability_Name_Snorkel:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_Snorkel;
            case Text_Database_Enum.ability_Name_OxygenTank:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_OxygenTank;
            case Text_Database_Enum.ability_Name_Flippers:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_Flippers;
            case Text_Database_Enum.ability_Name_DrillHelmet:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_DrillHelmet;
            case Text_Database_Enum.ability_Name_DrillBoots:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_DrillBoots;
            case Text_Database_Enum.ability_Name_HandDrill:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_HandDrill;
            case Text_Database_Enum.ability_Name_GrapplingHook:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_GrapplingHook;
            case Text_Database_Enum.ability_Name_SpringShoes:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_SpringShoes;
            case Text_Database_Enum.ability_Name_ClimbingGloves:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_ClimbingGloves;
            case Text_Database_Enum.ability_Name_10:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_10;
            case Text_Database_Enum.ability_Name_11:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_11;
            case Text_Database_Enum.ability_Name_12:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_12;
            case Text_Database_Enum.ability_Name_13:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_13;
            case Text_Database_Enum.ability_Name_14:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Name_14;
            #endregion

            #region Ability Messages
            case Text_Database_Enum.ability_Message_Snorkel_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_Snorkel_Keyboard;
            case Text_Database_Enum.ability_Message_Snorkel_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_Snorkel_PlayStation;
            case Text_Database_Enum.ability_Message_Snorkel_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_Snorkel_xBox;
            case Text_Database_Enum.ability_Message_OxygenTank_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_OxygenTank_Keyboard;
            case Text_Database_Enum.ability_Message_OxygenTank_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_OxygenTank_PlayStation;
            case Text_Database_Enum.ability_Message_OxygenTank_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_OxygenTank_xBox;
            case Text_Database_Enum.ability_Message_Flippers_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_Flippers_Keyboard;
            case Text_Database_Enum.ability_Message_Flippers_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_Flippers_PlayStation;
            case Text_Database_Enum.ability_Message_Flippers_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_Flippers_xBox;
            case Text_Database_Enum.ability_Message_DrillHelmet_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_DrillHelmet_Keyboard;
            case Text_Database_Enum.ability_Message_DrillHelmet_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_DrillHelmet_PlayStation;
            case Text_Database_Enum.ability_Message_DrillHelmet_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_DrillHelmet_xBox;
            case Text_Database_Enum.ability_Message_DrillBoots_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_DrillBoots_Keyboard;
            case Text_Database_Enum.ability_Message_DrillBoots_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_DrillBoots_PlayStation;
            case Text_Database_Enum.ability_Message_DrillBoots_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_DrillBoots_xBox;
            case Text_Database_Enum.ability_Message_HandDrill_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_HandDrill_Keyboard;
            case Text_Database_Enum.ability_Message_HandDrill_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_HandDrill_PlayStation;
            case Text_Database_Enum.ability_Message_HandDrill_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_HandDrill_xBox;
            case Text_Database_Enum.ability_Message_GrapplingHook_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_GrapplingHook_Keyboard;
            case Text_Database_Enum.ability_Message_GrapplingHook_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_GrapplingHook_PlayStation;
            case Text_Database_Enum.ability_Message_GrapplingHook_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_GrapplingHook_xBox;
            case Text_Database_Enum.ability_Message_SpringShoes_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_SpringShoes_Keyboard;
            case Text_Database_Enum.ability_Message_SpringShoes_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_SpringShoes_PlayStation;
            case Text_Database_Enum.ability_Message_SpringShoes_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_SpringShoes_xBox;
            case Text_Database_Enum.ability_Message_ClimbingGloves_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_ClimbingGloves_Keyboard;
            case Text_Database_Enum.ability_Message_ClimbingGloves_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_ClimbingGloves_PlayStation;
            case Text_Database_Enum.ability_Message_ClimbingGloves_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_ClimbingGloves_xBox;
            case Text_Database_Enum.ability_Message_10_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_10_Keyboard;
            case Text_Database_Enum.ability_Message_10_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_10_PlayStation;
            case Text_Database_Enum.ability_Message_10_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_10_xBox;
            case Text_Database_Enum.ability_Message_11_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_11_Keyboard;
            case Text_Database_Enum.ability_Message_11_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_11_PlayStation;
            case Text_Database_Enum.ability_Message_11_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_11_xBox;
            case Text_Database_Enum.ability_Message_12_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_12_Keyboard;
            case Text_Database_Enum.ability_Message_12_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_12_PlayStation;
            case Text_Database_Enum.ability_Message_12_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_12_xBox;
            case Text_Database_Enum.ability_Message_13_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_13_Keyboard;
            case Text_Database_Enum.ability_Message_13_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_13_PlayStation;
            case Text_Database_Enum.ability_Message_13_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_13_xBox;
            case Text_Database_Enum.ability_Message_14_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_14_Keyboard;
            case Text_Database_Enum.ability_Message_14_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_14_PlayStation;
            case Text_Database_Enum.ability_Message_14_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].ability_Message_14_xBox;
            #endregion

            #region Pickup Messages
            case Text_Database_Enum.pickup_Message_Eccence_1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_Eccence_1;
            case Text_Database_Enum.pickup_Message_Eccence_2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_Eccence_2;
            case Text_Database_Enum.pickup_Message_Eccence_3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_Eccence_3;
            case Text_Database_Enum.pickup_Message_Footprint:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_Footprint;
            case Text_Database_Enum.pickup_Message_Skin_First:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_Skin_First;
            case Text_Database_Enum.pickup_Message_Skin:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_Skin;
            case Text_Database_Enum.pickup_Message_7:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_7;
            case Text_Database_Enum.pickup_Message_8:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_8;
            case Text_Database_Enum.pickup_Message_9:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_9;
            case Text_Database_Enum.pickup_Message_10:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_10;
            case Text_Database_Enum.pickup_Message_11:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_11;
            case Text_Database_Enum.pickup_Message_12:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_12;
            case Text_Database_Enum.pickup_Message_13:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_13;
            case Text_Database_Enum.pickup_Message_14:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_14;
            case Text_Database_Enum.pickup_Message_15:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].pickup_Message_15;
            #endregion

            #region Tutorial Messages
            case Text_Database_Enum.tutorial_Message_Movement_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Movement_Keyboard;
            case Text_Database_Enum.tutorial_Message_Movement_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Movement_PlayStation;
            case Text_Database_Enum.tutorial_Message_Movement_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Movement_xBox;
            case Text_Database_Enum.tutorial_Message_CameraRotation_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_CameraRotation_Keyboard;
            case Text_Database_Enum.tutorial_Message_CameraRotation_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_CameraRotation_PlayStation;
            case Text_Database_Enum.tutorial_Message_CameraRotation_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_CameraRotation_xBox;
            case Text_Database_Enum.tutorial_Message_Respawn_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Respawn_Keyboard;
            case Text_Database_Enum.tutorial_Message_Respawn_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Respawn_PlayStation;
            case Text_Database_Enum.tutorial_Message_Respawn_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Respawn_xBox;
            case Text_Database_Enum.tutorial_Message_FreeCam_1_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_FreeCam_1_Keyboard;
            case Text_Database_Enum.tutorial_Message_FreeCam_1_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_FreeCam_1_PlayStation;
            case Text_Database_Enum.tutorial_Message_FreeCam_1_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_FreeCam_1_xBox;
            case Text_Database_Enum.tutorial_Message_FreeCam_2_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_FreeCam_2_Keyboard;
            case Text_Database_Enum.tutorial_Message_FreeCam_2_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_FreeCam_2_PlayStation;
            case Text_Database_Enum.tutorial_Message_FreeCam_2_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_FreeCam_2_xBox;
            case Text_Database_Enum.tutorial_Message_Demo_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Demo_Keyboard;
            case Text_Database_Enum.tutorial_Message_Demo_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Demo_PlayStation;
            case Text_Database_Enum.tutorial_Message_Demo_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_Demo_xBox;
            case Text_Database_Enum.tutorial_Message_7_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_7_Keyboard;
            case Text_Database_Enum.tutorial_Message_7_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_7_PlayStation;
            case Text_Database_Enum.tutorial_Message_7_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_7_xBox;
            case Text_Database_Enum.tutorial_Message_8_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_8_Keyboard;
            case Text_Database_Enum.tutorial_Message_8_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_8_PlayStation;
            case Text_Database_Enum.tutorial_Message_8_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_8_xBox;
            case Text_Database_Enum.tutorial_Message_9_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_9_Keyboard;
            case Text_Database_Enum.tutorial_Message_9_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_9_PlayStation;
            case Text_Database_Enum.tutorial_Message_9_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_9_xBox;
            case Text_Database_Enum.tutorial_Message_10_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_10_Keyboard;
            case Text_Database_Enum.tutorial_Message_10_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_10_PlayStation;
            case Text_Database_Enum.tutorial_Message_10_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_10_xBox;

            case Text_Database_Enum.tutorial_Message_11_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_11_Keyboard;

            case Text_Database_Enum.tutorial_Message_11_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_11_PlayStation;

            case Text_Database_Enum.tutorial_Message_11_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_11_xBox;

            case Text_Database_Enum.tutorial_Message_12_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_12_Keyboard;

            case Text_Database_Enum.tutorial_Message_12_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_12_PlayStation;

            case Text_Database_Enum.tutorial_Message_12_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_12_xBox;

            case Text_Database_Enum.tutorial_Message_13_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_13_Keyboard;

            case Text_Database_Enum.tutorial_Message_13_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_13_PlayStation;

            case Text_Database_Enum.tutorial_Message_13_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_13_xBox;

            case Text_Database_Enum.tutorial_Message_14_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_14_Keyboard;

            case Text_Database_Enum.tutorial_Message_14_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_14_PlayStation;

            case Text_Database_Enum.tutorial_Message_14_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_14_xBox;

            case Text_Database_Enum.tutorial_Message_15_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_15_Keyboard;

            case Text_Database_Enum.tutorial_Message_15_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_15_PlayStation;

            case Text_Database_Enum.tutorial_Message_15_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].tutorial_Message_15_xBox;

            #endregion

            #region InterractableButton Message
            case Text_Database_Enum.interractableButton_Message_Talk_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Talk_Keyboard;

            case Text_Database_Enum.interractableButton_Message_Talk_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Talk_PlayStation;

            case Text_Database_Enum.interractableButton_Message_Talk_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Talk_xBox;

            case Text_Database_Enum.interractableButton_Message_Interract_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Interract_Keyboard;

            case Text_Database_Enum.interractableButton_Message_Interract_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Interract_PlayStation;

            case Text_Database_Enum.interractableButton_Message_Interract_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Interract_xBox;

            case Text_Database_Enum.interractableButton_Message_FlippersUP_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_FlippersUP_Keyboard;

            case Text_Database_Enum.interractableButton_Message_FlippersUP_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_FlippersUP_PlayStation;

            case Text_Database_Enum.interractableButton_Message_FlippersUP_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_FlippersUP_xBox;

            case Text_Database_Enum.interractableButton_Message_FlippersDOWN_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_FlippersDOWN_Keyboard;

            case Text_Database_Enum.interractableButton_Message_FlippersDOWN_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_FlippersDOWN_PlayStation;

            case Text_Database_Enum.interractableButton_Message_FlippersDOWN_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_FlippersDOWN_xBox;

            case Text_Database_Enum.interractableButton_Message_DrillHelmet_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_DrillHelmet_Keyboard;

            case Text_Database_Enum.interractableButton_Message_DrillHelmet_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_DrillHelmet_PlayStation;

            case Text_Database_Enum.interractableButton_Message_DrillHelmet_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_DrillHelmet_xBox;

            case Text_Database_Enum.interractableButton_Message_DrillShoes_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_DrillShoes_Keyboard;

            case Text_Database_Enum.interractableButton_Message_DrillShoes_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_DrillShoes_PlayStation;

            case Text_Database_Enum.interractableButton_Message_DrillShoes_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_DrillShoes_xBox;

            case Text_Database_Enum.interractableButton_Message_HandDrill_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_HandDrill_Keyboard;

            case Text_Database_Enum.interractableButton_Message_HandDrill_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_HandDrill_PlayStation;

            case Text_Database_Enum.interractableButton_Message_HandDrill_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_HandDrill_xBox;

            case Text_Database_Enum.interractableButton_Message_GrapplingHook_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_GrapplingHook_Keyboard;

            case Text_Database_Enum.interractableButton_Message_GrapplingHook_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_GrapplingHook_PlayStation;

            case Text_Database_Enum.interractableButton_Message_GrapplingHook_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_GrapplingHook_xBox;

            case Text_Database_Enum.interractableButton_Message_SpringShoes_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_SpringShoes_Keyboard;

            case Text_Database_Enum.interractableButton_Message_SpringShoes_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_SpringShoes_PlayStation;

            case Text_Database_Enum.interractableButton_Message_SpringShoes_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_SpringShoes_xBox;

            case Text_Database_Enum.interractableButton_Message_ClimbingGloves_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_ClimbingGloves_Keyboard;

            case Text_Database_Enum.interractableButton_Message_ClimbingGloves_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_ClimbingGloves_PlayStation;

            case Text_Database_Enum.interractableButton_Message_ClimbingGloves_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_ClimbingGloves_xBox;

            case Text_Database_Enum.interractableButton_Message_Respawn_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Respawn_Keyboard;

            case Text_Database_Enum.interractableButton_Message_Respawn_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Respawn_PlayStation;

            case Text_Database_Enum.interractableButton_Message_Respawn_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_Respawn_xBox;

            case Text_Database_Enum.interractableButton_Message_12_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_12_Keyboard;

            case Text_Database_Enum.interractableButton_Message_12_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_12_PlayStation;

            case Text_Database_Enum.interractableButton_Message_12_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_12_xBox;

            case Text_Database_Enum.interractableButton_Message_13_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_13_Keyboard;

            case Text_Database_Enum.interractableButton_Message_13_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_13_PlayStation;

            case Text_Database_Enum.interractableButton_Message_13_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_13_xBox;

            case Text_Database_Enum.interractableButton_Message_14_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_14_Keyboard;

            case Text_Database_Enum.interractableButton_Message_14_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_14_PlayStation;

            case Text_Database_Enum.interractableButton_Message_14_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_14_xBox;

            case Text_Database_Enum.interractableButton_Message_15_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_15_Keyboard;

            case Text_Database_Enum.interractableButton_Message_15_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_15_PlayStation;

            case Text_Database_Enum.interractableButton_Message_15_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_15_xBox;

            case Text_Database_Enum.interractableButton_Message_16_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_16_Keyboard;

            case Text_Database_Enum.interractableButton_Message_16_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_16_PlayStation;

            case Text_Database_Enum.interractableButton_Message_16_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_16_xBox;

            case Text_Database_Enum.interractableButton_Message_17_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_17_Keyboard;

            case Text_Database_Enum.interractableButton_Message_17_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_17_PlayStation;

            case Text_Database_Enum.interractableButton_Message_17_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_17_xBox;

            case Text_Database_Enum.interractableButton_Message_18_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_18_Keyboard;

            case Text_Database_Enum.interractableButton_Message_18_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_18_PlayStation;

            case Text_Database_Enum.interractableButton_Message_18_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_18_xBox;

            case Text_Database_Enum.interractableButton_Message_19_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_19_Keyboard;

            case Text_Database_Enum.interractableButton_Message_19_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_19_PlayStation;

            case Text_Database_Enum.interractableButton_Message_19_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_19_xBox;

            case Text_Database_Enum.interractableButton_Message_20_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_20_Keyboard;

            case Text_Database_Enum.interractableButton_Message_20_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_20_PlayStation;

            case Text_Database_Enum.interractableButton_Message_20_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_20_xBox;

            case Text_Database_Enum.interractableButton_Message_21_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_21_Keyboard;

            case Text_Database_Enum.interractableButton_Message_21_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_21_PlayStation;

            case Text_Database_Enum.interractableButton_Message_21_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_21_xBox;

            case Text_Database_Enum.interractableButton_Message_22_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_22_Keyboard;

            case Text_Database_Enum.interractableButton_Message_22_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_22_PlayStation;

            case Text_Database_Enum.interractableButton_Message_22_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_22_xBox;

            case Text_Database_Enum.interractableButton_Message_23_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_23_Keyboard;

            case Text_Database_Enum.interractableButton_Message_23_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_23_PlayStation;

            case Text_Database_Enum.interractableButton_Message_23_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_23_xBox;

            case Text_Database_Enum.interractableButton_Message_24_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_24_Keyboard;

            case Text_Database_Enum.interractableButton_Message_24_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_24_PlayStation;

            case Text_Database_Enum.interractableButton_Message_24_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_24_xBox;

            case Text_Database_Enum.interractableButton_Message_25_Keyboard:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_25_Keyboard;

            case Text_Database_Enum.interractableButton_Message_25_PlayStation:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_25_PlayStation;

            case Text_Database_Enum.interractableButton_Message_25_xBox:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].interractableButton_Message_25_xBox;

            #endregion

            #region Finish Regions Messages
            case Text_Database_Enum.finishedRegion_Message_Water:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Water;
            case Text_Database_Enum.finishedRegion_Message_Sand:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Sand;
            case Text_Database_Enum.finishedRegion_Message_Ice:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Ice;
            case Text_Database_Enum.finishedRegion_Message_Lava:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Lava;
            case Text_Database_Enum.finishedRegion_Message_Swamp:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Swamp;
            case Text_Database_Enum.finishedRegion_Message_Metal:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_Metal;
            case Text_Database_Enum.finishedRegion_Message_7:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].finishedRegion_Message_7;
            #endregion

            #region NPC Names
            case Text_Database_Enum.NPCName_Water:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Water;
            case Text_Database_Enum.NPCName_Sand:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Sand;
            case Text_Database_Enum.NPCName_Ice:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Ice;
            case Text_Database_Enum.NPCName_Lava:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Lava;
            case Text_Database_Enum.NPCName_Swamp:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Swamp;
            case Text_Database_Enum.NPCName_Metal:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Metal;
            case Text_Database_Enum.NPCName_7:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_7;
            case Text_Database_Enum.NPCName_Antagonist:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Antagonist;
            #endregion

            #region NPC Hat Names
            case Text_Database_Enum.NPCHat_Name_Water:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_Water;
            case Text_Database_Enum.NPCHat_Name_Sand:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_Sand;
            case Text_Database_Enum.NPCHat_Name_Ice:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_Ice;
            case Text_Database_Enum.NPCHat_Name_Lava:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_Lava;
            case Text_Database_Enum.NPCHat_Name_Swamp:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_Swamp;
            case Text_Database_Enum.NPCHat_Name_Metal:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_Metal;
            case Text_Database_Enum.NPCHat_Name_7:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_7;
            #endregion


            default:
                return "";
        }
    }
}