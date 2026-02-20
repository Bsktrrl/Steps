using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TextData : MonoBehaviour
{
    [Header("Stats from Excel")]
    [SerializeField] TextAsset game_TextDatabase_Sheet;

    int startRow = 3;
    int columns = 4; //Size + 1

    [Header("Text Database")]
    public Game_TextDatabase game_TextDatabase = new Game_TextDatabase();


    //--------------------


    private void Start()
    {
        ReadExcelSheet();
    }


    //--------------------


    public void ReadExcelSheet()
    {
        for (int i = 0; i < columns - 1; i++)
        {
            Game_TextDatabase_Language gameText_LanguageList_Temp = new Game_TextDatabase_Language();
            game_TextDatabase.gameText_LanguageList.Add(gameText_LanguageList_Temp);
            print("game_TextDatabase.gameText_LanguageList.Size = " + game_TextDatabase.gameText_LanguageList.Count);
        }


        //-----


        //Separate Excel Sheet into a string[] by its ";"
        string[] excelData = game_TextDatabase_Sheet.text.Split(new string[] { ";", "\n" }, StringSplitOptions.None);

        // Calculate the size of the Excel table
        int excelTableSize = (excelData.Length / columns - 1) - 1;

        //Fill the new element with data
        for (int j = 0; j < columns - 1; j++)
        {
            //Region Names
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Water, 0);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Sand, 1);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Ice, 2);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Lava, 3);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Swamp, 4);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Metal, 5);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_6, 6);

            //Names of levels in Water Region
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Water_Lv1, 9);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Water_Lv2, 10);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Water_Lv3, 11);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Water_Lv4, 12);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Water_Lv5, 13);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Water_Lv6, 14);

            //Names of levels in Sand Region
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Sand_Lv1, 17);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Sand_Lv2, 18);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Sand_Lv3, 19);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Sand_Lv4, 20);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Sand_Lv5, 21);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Sand_Lv6, 22);

            //Names of levels in Ice Region
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Ice_Lv1, 25);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Ice_Lv2, 26);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Ice_Lv3, 27);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Ice_Lv4, 28);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Ice_Lv5, 29);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Ice_Lv6, 30);

            //Names of levels in Lava Region
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Lava_Lv1, 33);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Lava_Lv2, 34);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Lava_Lv3, 35);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Lava_Lv4, 36);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Lava_Lv5, 37);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Lava_Lv6, 38);

            //Names of levels in Swamp Region
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Swamp_Lv1, 41);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Swamp_Lv2, 42);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Swamp_Lv3, 43);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Swamp_Lv4, 44);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Swamp_Lv5, 45);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Swamp_Lv6, 46);

            //Names of levels in Metal Region
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Metal_Lv1, 49);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Metal_Lv2, 50);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Metal_Lv3, 51);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Metal_Lv4, 52);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Metal_Lv5, 53);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].name_Region_Metal_Lv6, 54);

            //Main Menu Buttons
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].mainMenu_Button_Continue, 57);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].mainMenu_Button_NewGame, 58);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].mainMenu_Button_Wardrobe, 59);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].mainMenu_Button_Options, 60);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].mainMenu_Button_Quit, 61);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].mainMenu_Button_6, 62);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].mainMenu_Button_7, 63);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].mainMenu_Button_8, 64);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].mainMenu_Button_9, 65);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].mainMenu_Button_10, 66);

            //Wardrobe Menu
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].WardrobeMenu_Header_Wardrobe, 69);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].WardrobeMenu_Message_SkinUnavailable_1, 70);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].WardrobeMenu_Message_SkinUnavailable_2, 71);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].WardrobeMenu_Message_SkinUnlock, 72);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].WardrobeMenu_Message_SkinEquip, 73);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].WardrobeMenu_Message_SkinEquipped, 74);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].WardrobeMenu_Message_Headgear_Unavailable, 75);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].WardrobeMenu_Message_Wardrobe_8, 76);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].WardrobeMenu_Message_Wardrobe_9, 77);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].WardrobeMenu_Message_Wardrobe_10, 78);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].WardrobeMenu_Message_Wardrobe_11, 79);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].WardrobeMenu_Message_Wardrobe_12, 80);

            //Settings
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Header_Settings, 83);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_Language, 84);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_TextSpeed, 85);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_StepDisplay, 86);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_SmoothCameraRotation, 87);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_RevertedCameraRotation, 88);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_SkipLevelIntro, 89);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_7, 90);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_8, 91);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_9, 92);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_10, 93);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_11, 94);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_12, 95);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_13, 96);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_14, 97);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_15, 98);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_16, 99);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_17, 100);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_18, 101);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_19, 102);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Settings_20, 103);

            //Controls
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Header_Controls, 104);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_1, 105);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_2, 106);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_3, 107);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_4, 108);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_5, 109);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_6, 110);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_7, 111);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_8, 112);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_9, 113);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_10, 114);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_11, 115);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_12, 116);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_13, 117);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_14, 118);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_15, 119);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_16, 120);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_17, 121);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_18, 122);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_19, 123);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_20, 124);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_21, 125);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_22, 126);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_23, 127);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_24, 128);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Controls_25, 129);

            //Video
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Header_Video, 130);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_1, 131);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_2, 132);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_3, 133);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_4, 134);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_5, 135);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_6, 136);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_7, 137);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_8, 138);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_9, 139);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_10, 140);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_11, 141);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_12, 142);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_13, 143);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_14, 144);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_15, 145);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_16, 146);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_17, 147);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_18, 148);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_19, 149);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Video_20, 150);

            //Audio
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Header_Audio, 151);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_1, 152);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_2, 153);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_3, 154);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_4, 155);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_5, 156);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_6, 157);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_7, 158);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_8, 159);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_9, 160);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_10, 161);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_11, 162);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_12, 163);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_13, 164);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_14, 165);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_15, 166);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_16, 167);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_17, 168);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_18, 169);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_19, 170);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Audio_20, 171);

            //Blank
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Header_Blank, 172);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_1, 173);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_2, 174);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_3, 175);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_4, 176);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_5, 177);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_6, 178);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_7, 179);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_8, 180);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_9, 181);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_10, 182);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_11, 183);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_12, 184);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_13, 185);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_14, 186);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_15, 187);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_16, 188);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_17, 189);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_18, 190);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_19, 191);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].options_Blank_20, 192);

            //Overworld Menu
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_1, 195);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_2, 196);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_3, 197);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_4, 198);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_5, 199);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_6, 200);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_7, 201);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_8, 202);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_9, 203);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_10, 204);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_11, 205);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_12, 206);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_13, 207);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_14, 208);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_15, 209);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_16, 210);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_17, 211);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_18, 212);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_19, 213);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].overworld_20, 214);

            //Pause Menu
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pauseMenu_Button_BackToGame, 217);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pauseMenu_Button_ExitLevel, 218);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pauseMenu_Button_3, 219);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pauseMenu_Button_4, 220);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pauseMenu_Button_5, 221);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pauseMenu_Button_6, 222);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pauseMenu_Button_7, 223);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pauseMenu_Button_8, 224);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pauseMenu_Button_9, 225);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pauseMenu_Button_10, 226);

            //Skin Names
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_Default, 229);
            
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_WaterRegion_Lv1, 230);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_WaterRegion_Lv2, 231);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_WaterRegion_Lv3, 232);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_WaterRegion_Lv4, 233);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_WaterRegion_Lv5, 234);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_WaterRegion_Lv6, 235);

            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_SandRegion_Lv1, 236);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_SandRegion_Lv2, 237);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_SandRegion_Lv3, 238);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_SandRegion_Lv4, 239);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_SandRegion_Lv5, 240);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_SandRegion_Lv6, 241);

            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_IceRegion_Lv1, 242);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_IceRegion_Lv2, 243);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_IceRegion_Lv3, 244);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_IceRegion_Lv4, 245);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_IceRegion_Lv5, 246);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_IceRegion_Lv6, 247);

            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_LavaRegion_Lv1, 248);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_LavaRegion_Lv2, 249);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_LavaRegion_Lv3, 250);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_LavaRegion_Lv4, 251);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_LavaRegion_Lv5, 252);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_LavaRegion_Lv6, 253);

            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_SwampRegion_Lv1, 254);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_SwampRegion_Lv2, 255);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_SwampRegion_Lv3, 256);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_SwampRegion_Lv4, 257);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_SwampRegion_Lv5, 258);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_SwampRegion_Lv6, 259);

            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_MetalRegion_Lv1, 260);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_MetalRegion_Lv2, 261);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_MetalRegion_Lv3, 262);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_MetalRegion_Lv4, 263);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_MetalRegion_Lv5, 264);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].skinName_MetalRegion_Lv6, 265);

            //Ability Names
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Name_Snorkel, 268);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Name_OxygenTank, 269);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Name_Flippers, 270);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Name_DrillHelmet, 271);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Name_DrillBoots, 272);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Name_HandDrill, 273);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Name_GrapplingHook, 274);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Name_SpringShoes, 275);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Name_ClimbingGloves, 276);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Name_10, 277);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Name_11, 278);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Name_12, 279);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Name_13, 280);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Name_14, 281);

            //Ability Messages
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Message_Snorkel, 284);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Message_OxygenTank, 285);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Message_Flippers, 286);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Message_DrillHelmet, 287);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Message_DrillBoots, 288);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Message_HandDrill, 289);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Message_GrapplingHook, 290);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Message_SpringShoes, 291);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Message_ClimbingGloves, 292);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Message_10, 293);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Message_11, 294);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Message_12, 295);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Message_13, 296);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].ability_Message_14, 297);

            //Pickup Messages
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pickup_Message_Eccence_1, 300);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pickup_Message_Eccence_2, 301);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pickup_Message_Eccence_3, 302);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pickup_Message_Footprint, 303);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pickup_Message_Skin_First, 304);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pickup_Message_Skin, 305);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pickup_Message_7, 306);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pickup_Message_8, 307);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pickup_Message_9, 308);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pickup_Message_10, 309);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pickup_Message_11, 310);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pickup_Message_12, 311);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pickup_Message_13, 312);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pickup_Message_14, 313);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].pickup_Message_15, 314);

            //Tutorial Messages
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].tutorial_Message_Movement, 317);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].tutorial_Message_CameraRotation, 318);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].tutorial_Message_Respawn, 319);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].tutorial_Message_FreeCam_1, 320);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].tutorial_Message_FreeCam_2, 321);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].tutorial_Message_Demo, 322);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].tutorial_Message_7, 323);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].tutorial_Message_8, 324);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].tutorial_Message_9, 325);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].tutorial_Message_10, 326);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].tutorial_Message_11, 327);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].tutorial_Message_12, 328);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].tutorial_Message_13, 329);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].tutorial_Message_14, 330);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].tutorial_Message_15, 331);

            //InterractableButton Message
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_Talk, 334);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_Interract, 335);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_FlippersUP, 336);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_FlippersDOWN, 337);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_DrillHelmet, 338);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_DrillShoes, 339);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_HandDrill, 340);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_GrapplingHook, 341);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_SpringShoes, 342);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_ClimbingGloves, 343);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_Respawn, 344);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_12, 345);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_13, 346);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_14, 347);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_15, 348);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_16, 349);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_17, 350);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_18, 351);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_19, 352);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_20, 353);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_21, 354);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_22, 355);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_23, 356);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_24, 357);
            SetTextData(excelData, j, ref game_TextDatabase.gameText_LanguageList[j].interractableButton_Message_25, 358);
        }

        //Remove elements that doesn't have a name
        //game_TextDatabase.gameText_LanguageList = game_TextDatabase.gameText_LanguageList.Where(obj => obj != null && !string.IsNullOrEmpty(obj.name_Region_Water)).ToList();

        //Input data into DataManager for easy access thoughout the game
        DataManager.Instance.game_TextDatabase_Store = game_TextDatabase;
    }


    void SetTextData(string[] excelData, int j, ref string languageVersion, int no)
    {
        if (excelData[(columns * no) + ((startRow - 1) * columns) + 1 + j] != "")
            languageVersion = excelData[(columns * no) + ((startRow - 1) * columns) + 1 + j].Trim();
        else
            languageVersion = "";
    }
}

[Serializable]
public class Game_TextDatabase
{
    public List<Game_TextDatabase_Language> gameText_LanguageList = new List<Game_TextDatabase_Language>();
}
[Serializable]
public class Game_TextDatabase_Language
{
    #region Region Names
    public string name_Region_Water;
    public string name_Region_Sand;
    public string name_Region_Ice;
    public string name_Region_Lava;
    public string name_Region_Swamp;
    public string name_Region_Metal;
    public string name_Region_6;
    #endregion

    //Level Names
    #region Names of levels in Water Region
    public string name_Region_Water_Lv1;
    public string name_Region_Water_Lv2;
    public string name_Region_Water_Lv3;
    public string name_Region_Water_Lv4;
    public string name_Region_Water_Lv5;
    public string name_Region_Water_Lv6;
    #endregion
    #region Names of levels in Sand Region
    public string name_Region_Sand_Lv1;
    public string name_Region_Sand_Lv2;
    public string name_Region_Sand_Lv3;
    public string name_Region_Sand_Lv4;
    public string name_Region_Sand_Lv5;
    public string name_Region_Sand_Lv6;
    #endregion
    #region Names of levels in Ice Region
    public string name_Region_Ice_Lv1;
    public string name_Region_Ice_Lv2;
    public string name_Region_Ice_Lv3;
    public string name_Region_Ice_Lv4;
    public string name_Region_Ice_Lv5;
    public string name_Region_Ice_Lv6;
    #endregion
    #region Names of levels in Lava Region
    public string name_Region_Lava_Lv1;
    public string name_Region_Lava_Lv2;
    public string name_Region_Lava_Lv3;
    public string name_Region_Lava_Lv4;
    public string name_Region_Lava_Lv5;
    public string name_Region_Lava_Lv6;
    #endregion
    #region Names of levels in Swamp Region
    public string name_Region_Swamp_Lv1;
    public string name_Region_Swamp_Lv2;
    public string name_Region_Swamp_Lv3;
    public string name_Region_Swamp_Lv4;
    public string name_Region_Swamp_Lv5;
    public string name_Region_Swamp_Lv6;
    #endregion
    #region Names of levels in Metal Region
    public string name_Region_Metal_Lv1;
    public string name_Region_Metal_Lv2;
    public string name_Region_Metal_Lv3;
    public string name_Region_Metal_Lv4;
    public string name_Region_Metal_Lv5;
    public string name_Region_Metal_Lv6;
    #endregion

    #region Main Menu Buttons
    public string mainMenu_Button_Continue;
    public string mainMenu_Button_NewGame;
    public string mainMenu_Button_Wardrobe;
    public string mainMenu_Button_Options;
    public string mainMenu_Button_Quit;
    public string mainMenu_Button_6;
    public string mainMenu_Button_7;
    public string mainMenu_Button_8;
    public string mainMenu_Button_9;
    public string mainMenu_Button_10;
    #endregion

    #region Wardrobe Menu
    public string WardrobeMenu_Header_Wardrobe;
    public string WardrobeMenu_Message_SkinUnavailable_1;
    public string WardrobeMenu_Message_SkinUnavailable_2;
    public string WardrobeMenu_Message_SkinUnlock;
    public string WardrobeMenu_Message_SkinEquip;
    public string WardrobeMenu_Message_SkinEquipped;
    public string WardrobeMenu_Message_Headgear_Unavailable;
    public string WardrobeMenu_Message_Wardrobe_8;
    public string WardrobeMenu_Message_Wardrobe_9;
    public string WardrobeMenu_Message_Wardrobe_10;
    public string WardrobeMenu_Message_Wardrobe_11;
    public string WardrobeMenu_Message_Wardrobe_12;
    #endregion

    //Options Menu
    #region Settings
    public string options_Header_Settings;
    public string options_Settings_Language;
    public string options_Settings_TextSpeed;
    public string options_Settings_StepDisplay;
    public string options_Settings_SmoothCameraRotation;
    public string options_Settings_RevertedCameraRotation;
    public string options_Settings_SkipLevelIntro;
    public string options_Settings_7;
    public string options_Settings_8;
    public string options_Settings_9;
    public string options_Settings_10;
    public string options_Settings_11;
    public string options_Settings_12;
    public string options_Settings_13;
    public string options_Settings_14;
    public string options_Settings_15;
    public string options_Settings_16;
    public string options_Settings_17;
    public string options_Settings_18;
    public string options_Settings_19;
    public string options_Settings_20;
    #endregion
    #region Controls
    public string options_Header_Controls;
    public string options_Controls_1;
    public string options_Controls_2;
    public string options_Controls_3;
    public string options_Controls_4;
    public string options_Controls_5;
    public string options_Controls_6;
    public string options_Controls_7;
    public string options_Controls_8;
    public string options_Controls_9;
    public string options_Controls_10;
    public string options_Controls_11;
    public string options_Controls_12;
    public string options_Controls_13;
    public string options_Controls_14;
    public string options_Controls_15;
    public string options_Controls_16;
    public string options_Controls_17;
    public string options_Controls_18;
    public string options_Controls_19;
    public string options_Controls_20;
    public string options_Controls_21;
    public string options_Controls_22;
    public string options_Controls_23;
    public string options_Controls_24;
    public string options_Controls_25;
    #endregion
    #region Video
    public string options_Header_Video;
    public string options_Video_1;
    public string options_Video_2;
    public string options_Video_3;
    public string options_Video_4;
    public string options_Video_5;
    public string options_Video_6;
    public string options_Video_7;
    public string options_Video_8;
    public string options_Video_9;
    public string options_Video_10;
    public string options_Video_11;
    public string options_Video_12;
    public string options_Video_13;
    public string options_Video_14;
    public string options_Video_15;
    public string options_Video_16;
    public string options_Video_17;
    public string options_Video_18;
    public string options_Video_19;
    public string options_Video_20;
    #endregion
    #region Audio
    public string options_Header_Audio;
    public string options_Audio_1;
    public string options_Audio_2;
    public string options_Audio_3;
    public string options_Audio_4;
    public string options_Audio_5;
    public string options_Audio_6;
    public string options_Audio_7;
    public string options_Audio_8;
    public string options_Audio_9;
    public string options_Audio_10;
    public string options_Audio_11;
    public string options_Audio_12;
    public string options_Audio_13;
    public string options_Audio_14;
    public string options_Audio_15;
    public string options_Audio_16;
    public string options_Audio_17;
    public string options_Audio_18;
    public string options_Audio_19;
    public string options_Audio_20;
    #endregion
    #region Blank
    public string options_Header_Blank;
    public string options_Blank_1;
    public string options_Blank_2;
    public string options_Blank_3;
    public string options_Blank_4;
    public string options_Blank_5;
    public string options_Blank_6;
    public string options_Blank_7;
    public string options_Blank_8;
    public string options_Blank_9;
    public string options_Blank_10;
    public string options_Blank_11;
    public string options_Blank_12;
    public string options_Blank_13;
    public string options_Blank_14;
    public string options_Blank_15;
    public string options_Blank_16;
    public string options_Blank_17;
    public string options_Blank_18;
    public string options_Blank_19;
    public string options_Blank_20;
    #endregion

    #region Overworld Menu
    public string overworld_1;
    public string overworld_2;
    public string overworld_3;
    public string overworld_4;
    public string overworld_5;
    public string overworld_6;
    public string overworld_7;
    public string overworld_8;
    public string overworld_9;
    public string overworld_10;
    public string overworld_11;
    public string overworld_12;
    public string overworld_13;
    public string overworld_14;
    public string overworld_15;
    public string overworld_16;
    public string overworld_17;
    public string overworld_18;
    public string overworld_19;
    public string overworld_20;
    #endregion

    #region Pause Menu
    public string pauseMenu_Button_BackToGame;
    public string pauseMenu_Button_ExitLevel;
    public string pauseMenu_Button_3;
    public string pauseMenu_Button_4;
    public string pauseMenu_Button_5;
    public string pauseMenu_Button_6;
    public string pauseMenu_Button_7;
    public string pauseMenu_Button_8;
    public string pauseMenu_Button_9;
    public string pauseMenu_Button_10;
    #endregion

    #region Skin Names
    public string skinName_Default;

    public string skinName_WaterRegion_Lv1;
    public string skinName_WaterRegion_Lv2;
    public string skinName_WaterRegion_Lv3;
    public string skinName_WaterRegion_Lv4;
    public string skinName_WaterRegion_Lv5;
    public string skinName_WaterRegion_Lv6;

    public string skinName_SandRegion_Lv1;
    public string skinName_SandRegion_Lv2;
    public string skinName_SandRegion_Lv3;
    public string skinName_SandRegion_Lv4;
    public string skinName_SandRegion_Lv5;
    public string skinName_SandRegion_Lv6;

    public string skinName_IceRegion_Lv1;
    public string skinName_IceRegion_Lv2;
    public string skinName_IceRegion_Lv3;
    public string skinName_IceRegion_Lv4;
    public string skinName_IceRegion_Lv5;
    public string skinName_IceRegion_Lv6;

    public string skinName_LavaRegion_Lv1;
    public string skinName_LavaRegion_Lv2;
    public string skinName_LavaRegion_Lv3;
    public string skinName_LavaRegion_Lv4;
    public string skinName_LavaRegion_Lv5;
    public string skinName_LavaRegion_Lv6;

    public string skinName_SwampRegion_Lv1;
    public string skinName_SwampRegion_Lv2;
    public string skinName_SwampRegion_Lv3;
    public string skinName_SwampRegion_Lv4;
    public string skinName_SwampRegion_Lv5;
    public string skinName_SwampRegion_Lv6;

    public string skinName_MetalRegion_Lv1;
    public string skinName_MetalRegion_Lv2;
    public string skinName_MetalRegion_Lv3;
    public string skinName_MetalRegion_Lv4;
    public string skinName_MetalRegion_Lv5;
    public string skinName_MetalRegion_Lv6;
    #endregion

    #region Ability Names
    public string ability_Name_Snorkel;
    public string ability_Name_OxygenTank;
    public string ability_Name_Flippers;
    public string ability_Name_DrillHelmet;
    public string ability_Name_DrillBoots;
    public string ability_Name_HandDrill;
    public string ability_Name_GrapplingHook;
    public string ability_Name_SpringShoes;
    public string ability_Name_ClimbingGloves;
    public string ability_Name_10;
    public string ability_Name_11;
    public string ability_Name_12;
    public string ability_Name_13;
    public string ability_Name_14;
    #endregion

    #region Ability Messages
    public string ability_Message_Snorkel;
    public string ability_Message_OxygenTank;
    public string ability_Message_Flippers;
    public string ability_Message_DrillHelmet;
    public string ability_Message_DrillBoots;
    public string ability_Message_HandDrill;
    public string ability_Message_GrapplingHook;
    public string ability_Message_SpringShoes;
    public string ability_Message_ClimbingGloves;
    public string ability_Message_10;
    public string ability_Message_11;
    public string ability_Message_12;
    public string ability_Message_13;
    public string ability_Message_14;
    #endregion

    #region Pickup Messages
    public string pickup_Message_Eccence_1;
    public string pickup_Message_Eccence_2;
    public string pickup_Message_Eccence_3;
    public string pickup_Message_Footprint;
    public string pickup_Message_Skin_First;
    public string pickup_Message_Skin;
    public string pickup_Message_7;
    public string pickup_Message_8;
    public string pickup_Message_9;
    public string pickup_Message_10;
    public string pickup_Message_11;
    public string pickup_Message_12;
    public string pickup_Message_13;
    public string pickup_Message_14;
    public string pickup_Message_15;
    #endregion

    #region Tutorial Messages
    public string tutorial_Message_Movement;
    public string tutorial_Message_CameraRotation;
    public string tutorial_Message_Respawn;
    public string tutorial_Message_FreeCam_1;
    public string tutorial_Message_FreeCam_2;
    public string tutorial_Message_Demo;
    public string tutorial_Message_7;
    public string tutorial_Message_8;
    public string tutorial_Message_9;
    public string tutorial_Message_10;
    public string tutorial_Message_11;
    public string tutorial_Message_12;
    public string tutorial_Message_13;
    public string tutorial_Message_14;
    public string tutorial_Message_15;
    #endregion

    #region InterractableButton Message
    public string interractableButton_Message_Talk;
    public string interractableButton_Message_Interract;
    public string interractableButton_Message_FlippersUP;
    public string interractableButton_Message_FlippersDOWN;
    public string interractableButton_Message_DrillHelmet;
    public string interractableButton_Message_DrillShoes;
    public string interractableButton_Message_HandDrill;
    public string interractableButton_Message_GrapplingHook;
    public string interractableButton_Message_SpringShoes;
    public string interractableButton_Message_ClimbingGloves;
    public string interractableButton_Message_Respawn;
    public string interractableButton_Message_12;
    public string interractableButton_Message_13;
    public string interractableButton_Message_14;
    public string interractableButton_Message_15;
    public string interractableButton_Message_16;
    public string interractableButton_Message_17;
    public string interractableButton_Message_18;
    public string interractableButton_Message_19;
    public string interractableButton_Message_20;
    public string interractableButton_Message_21;
    public string interractableButton_Message_22;
    public string interractableButton_Message_23;
    public string interractableButton_Message_24;
    public string interractableButton_Message_25;
    #endregion
}

public enum Text_Database_Enum
{
    #region All
    [InspectorName("None")] None,

    #region Region Names
    [InspectorName("Region Name / Water")] name_Region_Water,
    [InspectorName("Region Name / Sand")] name_Region_Sand,
    [InspectorName("Region Name / Ice")] name_Region_Ice,
    [InspectorName("Region Name / Lava")] name_Region_Lava,
    [InspectorName("Region Name / Swamp")] name_Region_Swamp,
    [InspectorName("Region Name / Metal")] name_Region_Metal,
    [InspectorName("Region Name / Region 6")] name_Region_6,
    #endregion

    //Level Names
    #region Names of levels in Water Region
    [InspectorName("Water Region Levels / Level 1")] name_Region_Water_Lv1,
    [InspectorName("Water Region Levels / Level 2")] name_Region_Water_Lv2,
    [InspectorName("Water Region Levels / Level 3")] name_Region_Water_Lv3,
    [InspectorName("Water Region Levels / Level 4")] name_Region_Water_Lv4,
    [InspectorName("Water Region Levels / Level 5")] name_Region_Water_Lv5,
    [InspectorName("Water Region Levels / Level 6")] name_Region_Water_Lv6,
    #endregion
    #region Names of levels in Sand Region
    [InspectorName("Sand Region Levels / Level 1")] name_Region_Sand_Lv1,
    [InspectorName("Sand Region Levels / Level 2")] name_Region_Sand_Lv2,
    [InspectorName("Sand Region Levels / Level 3")] name_Region_Sand_Lv3,
    [InspectorName("Sand Region Levels / Level 4")] name_Region_Sand_Lv4,
    [InspectorName("Sand Region Levels / Level 5")] name_Region_Sand_Lv5,
    [InspectorName("Sand Region Levels / Level 6")] name_Region_Sand_Lv6,
    #endregion
    #region Names of levels in Ice Region
    [InspectorName("Ice Region Levels / Level 1")] name_Region_Ice_Lv1,
    [InspectorName("Ice Region Levels / Level 2")] name_Region_Ice_Lv2,
    [InspectorName("Ice Region Levels / Level 3")] name_Region_Ice_Lv3,
    [InspectorName("Ice Region Levels / Level 4")] name_Region_Ice_Lv4,
    [InspectorName("Ice Region Levels / Level 5")] name_Region_Ice_Lv5,
    [InspectorName("Ice Region Levels / Level 6")] name_Region_Ice_Lv6,
    #endregion
    #region Names of levels in Lava Region
    [InspectorName("Lava Region Levels / Level 1")] name_Region_Lava_Lv1,
    [InspectorName("Lava Region Levels / Level 2")] name_Region_Lava_Lv2,
    [InspectorName("Lava Region Levels / Level 3")] name_Region_Lava_Lv3,
    [InspectorName("Lava Region Levels / Level 4")] name_Region_Lava_Lv4,
    [InspectorName("Lava Region Levels / Level 5")] name_Region_Lava_Lv5,
    [InspectorName("Lava Region Levels / Level 6")] name_Region_Lava_Lv6,
    #endregion
    #region Names of levels in Swamp Region
    [InspectorName("Swamp Region Levels / Level 1")] name_Region_Swamp_Lv1,
    [InspectorName("Swamp Region Levels / Level 2")] name_Region_Swamp_Lv2,
    [InspectorName("Swamp Region Levels / Level 3")] name_Region_Swamp_Lv3,
    [InspectorName("Swamp Region Levels / Level 4")] name_Region_Swamp_Lv4,
    [InspectorName("Swamp Region Levels / Level 5")] name_Region_Swamp_Lv5,
    [InspectorName("Swamp Region Levels / Level 6")] name_Region_Swamp_Lv6,
    #endregion
    #region Names of levels in Metal Region
    [InspectorName("Metal Region Levels / Level 1")] name_Region_Metal_Lv1,
    [InspectorName("Metal Region Levels / Level 2")] name_Region_Metal_Lv2,
    [InspectorName("Metal Region Levels / Level 3")] name_Region_Metal_Lv3,
    [InspectorName("Metal Region Levels / Level 4")] name_Region_Metal_Lv4,
    [InspectorName("Metal Region Levels / Level 5")] name_Region_Metal_Lv5,
    [InspectorName("Metal Region Levels / Level 6")] name_Region_Metal_Lv6,
    #endregion

    #region Main Menu Buttons
    [InspectorName("Main Menu Buttons / Continue")] mainMenu_Button_Continue,
    [InspectorName("Main Menu Buttons / NewGame")] mainMenu_Button_NewGame,
    [InspectorName("Main Menu Buttons / Wardrobe")] mainMenu_Button_Wardrobe,
    [InspectorName("Main Menu Buttons / Options")] mainMenu_Button_Options,
    [InspectorName("Main Menu Buttons / Quit")] mainMenu_Button_Quit,
    [InspectorName("Main Menu Buttons / 6")] mainMenu_Button_6,
    [InspectorName("Main Menu Buttons / 7")] mainMenu_Button_7,
    [InspectorName("Main Menu Buttons / 8")] mainMenu_Button_8,
    [InspectorName("Main Menu Buttons / 9")] mainMenu_Button_9,
    [InspectorName("Main Menu Buttons / 10")] mainMenu_Button_10,
    #endregion

    #region Wardrobe Menu
    [InspectorName("Wardrobe Menu / Header /Header")] WardrobeMenu_Header_Wardrobe,
    [InspectorName("Wardrobe Menu / Message / SkinUnavailable_1")] WardrobeMenu_Message_SkinUnavailable_1,
    [InspectorName("Wardrobe Menu / Message / SkinUnavailable_2")] WardrobeMenu_Message_SkinUnavailable_2,
    [InspectorName("Wardrobe Menu / Message / SkinUnlock")] WardrobeMenu_Message_SkinUnlock,
    [InspectorName("Wardrobe Menu / Message / SkinEquip")] WardrobeMenu_Message_SkinEquip,
    [InspectorName("Wardrobe Menu / Message / SkinEquipped")] WardrobeMenu_Message_SkinEquipped,
    [InspectorName("Wardrobe Menu / Message / Headgear_Unavailable")] WardrobeMenu_Message_Headgear_Unavailable,
    [InspectorName("Wardrobe Menu / Message / Wardrobe_8")] WardrobeMenu_Message_Wardrobe_8,
    [InspectorName("Wardrobe Menu / Message / Wardrobe_9")] WardrobeMenu_Message_Wardrobe_9,
    [InspectorName("Wardrobe Menu / Message / Wardrobe_10")] WardrobeMenu_Message_Wardrobe_10,
    [InspectorName("Wardrobe Menu / Message / Wardrobe_11")] WardrobeMenu_Message_Wardrobe_11,
    [InspectorName("Wardrobe Menu / Message / Wardrobe_12")] WardrobeMenu_Message_Wardrobe_12,
    #endregion

    //Options Menu
    #region Settings
    [InspectorName("Options Menu / Settings / Header / Header")] options_Header_Settings,
    [InspectorName("Options Menu / Settings / Language")] options_Settings_Language,
    [InspectorName("Options Menu / Settings / Text Speed")] options_Settings_TextSpeed,
    [InspectorName("Options Menu / Settings / Step Display")] options_Settings_StepDisplay,
    [InspectorName("Options Menu / Settings / Smooth Camera Rotation")] options_Settings_SmoothCameraRotation,
    [InspectorName("Options Menu / Settings / Reverted Camera Rotation")] options_Settings_RevertedCameraRotation,
    [InspectorName("Options Menu / Settings / Skip Level Intro")] options_Settings_SkipLevelIntro,
    [InspectorName("Options Menu / Settings / 7")] options_Settings_7,
    [InspectorName("Options Menu / Settings / 8")] options_Settings_8,
    [InspectorName("Options Menu / Settings / 9")] options_Settings_9,
    [InspectorName("Options Menu / Settings / 10")] options_Settings_10,
    [InspectorName("Options Menu / Settings / 11")] options_Settings_11,
    [InspectorName("Options Menu / Settings / 12")] options_Settings_12,
    [InspectorName("Options Menu / Settings / 13")] options_Settings_13,
    [InspectorName("Options Menu / Settings / 14")] options_Settings_14,
    [InspectorName("Options Menu / Settings / 15")] options_Settings_15,
    [InspectorName("Options Menu / Settings / 16")] options_Settings_16,
    [InspectorName("Options Menu / Settings / 17")] options_Settings_17,
    [InspectorName("Options Menu / Settings / 18")] options_Settings_18,
    [InspectorName("Options Menu / Settings / 19")] options_Settings_19,
    [InspectorName("Options Menu / Settings / 20")] options_Settings_20,
    #endregion
    #region Controls
    [InspectorName("Options Menu / Controls / Header / Header")] options_Header_Controls,
    [InspectorName("Options Menu / Controls / 1")] options_Controls_1,
    [InspectorName("Options Menu / Controls / 2")] options_Controls_2,
    [InspectorName("Options Menu / Controls / 3")] options_Controls_3,
    [InspectorName("Options Menu / Controls / 4")] options_Controls_4,
    [InspectorName("Options Menu / Controls / 5")] options_Controls_5,
    [InspectorName("Options Menu / Controls / 6")] options_Controls_6,
    [InspectorName("Options Menu / Controls / 7")] options_Controls_7,
    [InspectorName("Options Menu / Controls / 8")] options_Controls_8,
    [InspectorName("Options Menu / Controls / 9")] options_Controls_9,
    [InspectorName("Options Menu / Controls / 10")] options_Controls_10,
    [InspectorName("Options Menu / Controls / 11")] options_Controls_11,
    [InspectorName("Options Menu / Controls / 12")] options_Controls_12,
    [InspectorName("Options Menu / Controls / 13")] options_Controls_13,
    [InspectorName("Options Menu / Controls / 14")] options_Controls_14,
    [InspectorName("Options Menu / Controls / 15")] options_Controls_15,
    [InspectorName("Options Menu / Controls / 16")] options_Controls_16,
    [InspectorName("Options Menu / Controls / 17")] options_Controls_17,
    [InspectorName("Options Menu / Controls / 18")] options_Controls_18,
    [InspectorName("Options Menu / Controls / 19")] options_Controls_19,
    [InspectorName("Options Menu / Controls / 20")] options_Controls_20,
    [InspectorName("Options Menu / Controls / 21")] options_Controls_21,
    [InspectorName("Options Menu / Controls / 22")] options_Controls_22,
    [InspectorName("Options Menu / Controls / 23")] options_Controls_23,
    [InspectorName("Options Menu / Controls / 24")] options_Controls_24,
    [InspectorName("Options Menu / Controls / 25")] options_Controls_25,
    #endregion
    #region Video
    [InspectorName("Options Menu / Video / Header / Header")] options_Header_Video,
    [InspectorName("Options Menu / Video / 1")] options_Video_1,
    [InspectorName("Options Menu / Video / 2")] options_Video_2,
    [InspectorName("Options Menu / Video / 3")] options_Video_3,
    [InspectorName("Options Menu / Video / 4")] options_Video_4,
    [InspectorName("Options Menu / Video / 5")] options_Video_5,
    [InspectorName("Options Menu / Video / 6")] options_Video_6,
    [InspectorName("Options Menu / Video / 7")] options_Video_7,
    [InspectorName("Options Menu / Video / 8")] options_Video_8,
    [InspectorName("Options Menu / Video / 9")] options_Video_9,
    [InspectorName("Options Menu / Video / 10")] options_Video_10,
    [InspectorName("Options Menu / Video / 11")] options_Video_11,
    [InspectorName("Options Menu / Video / 12")] options_Video_12,
    [InspectorName("Options Menu / Video / 13")] options_Video_13,
    [InspectorName("Options Menu / Video / 14")] options_Video_14,
    [InspectorName("Options Menu / Video / 15")] options_Video_15,
    [InspectorName("Options Menu / Video / 16")] options_Video_16,
    [InspectorName("Options Menu / Video / 17")] options_Video_17,
    [InspectorName("Options Menu / Video / 18")] options_Video_18,
    [InspectorName("Options Menu / Video / 19")] options_Video_19,
    [InspectorName("Options Menu / Video / 20")] options_Video_20,
    #endregion
    #region Audio
    [InspectorName("Options Menu / Audio / Header / Header")] options_Header_Audio,
    [InspectorName("Options Menu / Audio / 1")] options_Audio_1,
    [InspectorName("Options Menu / Audio / 2")] options_Audio_2,
    [InspectorName("Options Menu / Audio / 3")] options_Audio_3,
    [InspectorName("Options Menu / Audio / 4")] options_Audio_4,
    [InspectorName("Options Menu / Audio / 5")] options_Audio_5,
    [InspectorName("Options Menu / Audio / 6")] options_Audio_6,
    [InspectorName("Options Menu / Audio / 7")] options_Audio_7,
    [InspectorName("Options Menu / Audio / 8")] options_Audio_8,
    [InspectorName("Options Menu / Audio / 9")] options_Audio_9,
    [InspectorName("Options Menu / Audio / 10")] options_Audio_10,
    [InspectorName("Options Menu / Audio / 11")] options_Audio_11,
    [InspectorName("Options Menu / Audio / 12")] options_Audio_12,
    [InspectorName("Options Menu / Audio / 13")] options_Audio_13,
    [InspectorName("Options Menu / Audio / 14")] options_Audio_14,
    [InspectorName("Options Menu / Audio / 15")] options_Audio_15,
    [InspectorName("Options Menu / Audio / 16")] options_Audio_16,
    [InspectorName("Options Menu / Audio / 17")] options_Audio_17,
    [InspectorName("Options Menu / Audio / 18")] options_Audio_18,
    [InspectorName("Options Menu / Audio / 19")] options_Audio_19,
    [InspectorName("Options Menu / Audio / 20")] options_Audio_20,
    #endregion
    #region Blank
    [InspectorName("Options Menu / Blank / Header / Header")] options_Header_Blank,
    [InspectorName("Options Menu / Blank / 1")] options_Blank_1,
    [InspectorName("Options Menu / Blank / 2")] options_Blank_2,
    [InspectorName("Options Menu / Blank / 3")] options_Blank_3,
    [InspectorName("Options Menu / Blank / 4")] options_Blank_4,
    [InspectorName("Options Menu / Blank / 5")] options_Blank_5,
    [InspectorName("Options Menu / Blank / 6")] options_Blank_6,
    [InspectorName("Options Menu / Blank / 7")] options_Blank_7,
    [InspectorName("Options Menu / Blank / 8")] options_Blank_8,
    [InspectorName("Options Menu / Blank / 9")] options_Blank_9,
    [InspectorName("Options Menu / Blank / 10")] options_Blank_10,
    [InspectorName("Options Menu / Blank / 11")] options_Blank_11,
    [InspectorName("Options Menu / Blank / 12")] options_Blank_12,
    [InspectorName("Options Menu / Blank / 13")] options_Blank_13,
    [InspectorName("Options Menu / Blank / 14")] options_Blank_14,
    [InspectorName("Options Menu / Blank / 15")] options_Blank_15,
    [InspectorName("Options Menu / Blank / 16")] options_Blank_16,
    [InspectorName("Options Menu / Blank / 17")] options_Blank_17,
    [InspectorName("Options Menu / Blank / 18")] options_Blank_18,
    [InspectorName("Options Menu / Blank / 19")] options_Blank_19,
    [InspectorName("Options Menu / Blank / 20")] options_Blank_20,
    #endregion

    #region Overworld Menu
    [InspectorName("Overworld Menu / overworld_1")] overworld_1,
    [InspectorName("Overworld Menu / overworld_2")] overworld_2,
    [InspectorName("Overworld Menu / overworld_3")] overworld_3,
    [InspectorName("Overworld Menu / overworld_4")] overworld_4,
    [InspectorName("Overworld Menu / overworld_5")] overworld_5,
    [InspectorName("Overworld Menu / overworld_6")] overworld_6,
    [InspectorName("Overworld Menu / overworld_7")] overworld_7,
    [InspectorName("Overworld Menu / overworld_8")] overworld_8,
    [InspectorName("Overworld Menu / overworld_9")] overworld_9,
    [InspectorName("Overworld Menu / overworld_10")] overworld_10,
    [InspectorName("Overworld Menu / overworld_11")] overworld_11,
    [InspectorName("Overworld Menu / overworld_12")] overworld_12,
    [InspectorName("Overworld Menu / overworld_13")] overworld_13,
    [InspectorName("Overworld Menu / overworld_14")] overworld_14,
    [InspectorName("Overworld Menu / overworld_15")] overworld_15,
    [InspectorName("Overworld Menu / overworld_16")] overworld_16,
    [InspectorName("Overworld Menu / overworld_17")] overworld_17,
    [InspectorName("Overworld Menu / overworld_18")] overworld_18,
    [InspectorName("Overworld Menu / overworld_19")] overworld_19,
    [InspectorName("Overworld Menu / overworld_10")] overworld_20,
    #endregion

    #region Pause Menu
    [InspectorName("Pause Menu / Back To Game")] pauseMenu_Button_BackToGame,
    [InspectorName("Pause Menu / Exit Level")] pauseMenu_Button_ExitLevel,
    [InspectorName("Pause Menu / Button 3")] pauseMenu_Button_3,
    [InspectorName("Pause Menu / Button 4")] pauseMenu_Button_4,
    [InspectorName("Pause Menu / Button 5")] pauseMenu_Button_5,
    [InspectorName("Pause Menu / Button 6")] pauseMenu_Button_6,
    [InspectorName("Pause Menu / Button 7")] pauseMenu_Button_7,
    [InspectorName("Pause Menu / Button 8")] pauseMenu_Button_8,
    [InspectorName("Pause Menu / Button 9")] pauseMenu_Button_9,
    [InspectorName("Pause Menu / Button 10")] pauseMenu_Button_10,
    #endregion

    #region Skin Names
    [InspectorName("Skin Names / Default / Default")] skinName_Default,

    [InspectorName("Skin Names / Water / WaterRegion_Lv1")] skinName_WaterRegion_Lv1,
    [InspectorName("Skin Names / Water / WaterRegion_Lv2")] skinName_WaterRegion_Lv2,
    [InspectorName("Skin Names / Water / WaterRegion_Lv3")] skinName_WaterRegion_Lv3,
    [InspectorName("Skin Names / Water / WaterRegion_Lv4")] skinName_WaterRegion_Lv4,
    [InspectorName("Skin Names / Water / WaterRegion_Lv5")] skinName_WaterRegion_Lv5,
    [InspectorName("Skin Names / Water / WaterRegion_Lv6")] skinName_WaterRegion_Lv6,

    [InspectorName("Skin Names / Sand / SandRegion_Lv1")] skinName_SandRegion_Lv1,
    [InspectorName("Skin Names / Sand / SandRegion_Lv2")] skinName_SandRegion_Lv2,
    [InspectorName("Skin Names / Sand / SandRegion_Lv3")] skinName_SandRegion_Lv3,
    [InspectorName("Skin Names / Sand / SandRegion_Lv4")] skinName_SandRegion_Lv4,
    [InspectorName("Skin Names / Sand / SandRegion_Lv5")] skinName_SandRegion_Lv5,
    [InspectorName("Skin Names / Sand / SandRegion_Lv6")] skinName_SandRegion_Lv6,

    [InspectorName("Skin Names / Ice / IceRegion_Lv1")] skinName_IceRegion_Lv1,
    [InspectorName("Skin Names / Ice / IceRegion_Lv2")] skinName_IceRegion_Lv2,
    [InspectorName("Skin Names / Ice / IceRegion_Lv3")] skinName_IceRegion_Lv3,
    [InspectorName("Skin Names / Ice / IceRegion_Lv4")] skinName_IceRegion_Lv4,
    [InspectorName("Skin Names / Ice / IceRegion_Lv5")] skinName_IceRegion_Lv5,
    [InspectorName("Skin Names / Ice / IceRegion_Lv6")] skinName_IceRegion_Lv6,

    [InspectorName("Skin Names / Lava / LavaRegion_Lv1")] skinName_LavaRegion_Lv1,
    [InspectorName("Skin Names / Lava / LavaRegion_Lv2")] skinName_LavaRegion_Lv2,
    [InspectorName("Skin Names / Lava / LavaRegion_Lv3")] skinName_LavaRegion_Lv3,
    [InspectorName("Skin Names / Lava / LavaRegion_Lv4")] skinName_LavaRegion_Lv4,
    [InspectorName("Skin Names / Lava / LavaRegion_Lv5")] skinName_LavaRegion_Lv5,
    [InspectorName("Skin Names / Lava / LavaRegion_Lv6")] skinName_LavaRegion_Lv6,

    [InspectorName("Skin Names / Swamp / SwampRegion_Lv1")] skinName_SwampRegion_Lv1,
    [InspectorName("Skin Names / Swamp / SwampRegion_Lv2")] skinName_SwampRegion_Lv2,
    [InspectorName("Skin Names / Swamp / SwampRegion_Lv3")] skinName_SwampRegion_Lv3,
    [InspectorName("Skin Names / Swamp / SwampRegion_Lv4")] skinName_SwampRegion_Lv4,
    [InspectorName("Skin Names / Swamp / SwampRegion_Lv5")] skinName_SwampRegion_Lv5,
    [InspectorName("Skin Names / Swamp / SwampRegion_Lv6")] skinName_SwampRegion_Lv6,

    [InspectorName("Skin Names / Metal / MetalRegion_Lv1")] skinName_MetalRegion_Lv1,
    [InspectorName("Skin Names / Metal / MetalRegion_Lv2")] skinName_MetalRegion_Lv2,
    [InspectorName("Skin Names / Metal / MetalRegion_Lv3")] skinName_MetalRegion_Lv3,
    [InspectorName("Skin Names / Metal / MetalRegion_Lv4")] skinName_MetalRegion_Lv4,
    [InspectorName("Skin Names / Metal / MetalRegion_Lv5")] skinName_MetalRegion_Lv5,
    [InspectorName("Skin Names / Metal / MetalRegion_Lv6")] skinName_MetalRegion_Lv6,
    #endregion

    #region Ability Names
    [InspectorName("Ability Names / Snorkel")] ability_Name_Snorkel,
    [InspectorName("Ability Names / Oxygen Tank")] ability_Name_OxygenTank,
    [InspectorName("Ability Names / Flippers")] ability_Name_Flippers,
    [InspectorName("Ability Names / Drill Helmet")] ability_Name_DrillHelmet,
    [InspectorName("Ability Names / Drill Boots")] ability_Name_DrillBoots,
    [InspectorName("Ability Names / Hand Drill")] ability_Name_HandDrill,
    [InspectorName("Ability Names / Grappling Hook")] ability_Name_GrapplingHook,
    [InspectorName("Ability Names / Spring Shoes")] ability_Name_SpringShoes,
    [InspectorName("Ability Names / Climbing Gloves")] ability_Name_ClimbingGloves,
    [InspectorName("Ability Names / 10")] ability_Name_10,
    [InspectorName("Ability Names / 11")] ability_Name_11,
    [InspectorName("Ability Names / 12")] ability_Name_12,
    [InspectorName("Ability Names / 13")] ability_Name_13,
    [InspectorName("Ability Names / 14")] ability_Name_14,
    #endregion

    #region Ability Messages
    [InspectorName("Ability Messages / Snorkel")] ability_Message_Snorkel,
    [InspectorName("Ability Messages / Oxygen Tank")] ability_Message_OxygenTank,
    [InspectorName("Ability Messages / Flippers")] ability_Message_Flippers,
    [InspectorName("Ability Messages / Drill Helmet")] ability_Message_DrillHelmet,
    [InspectorName("Ability Messages / Drill Boots")] ability_Message_DrillBoots,
    [InspectorName("Ability Messages / Hand Drill")] ability_Message_HandDrill,
    [InspectorName("Ability Messages / Grappling Hook")] ability_Message_GrapplingHook,
    [InspectorName("Ability Messages / Spring Shoes")] ability_Message_SpringShoes,
    [InspectorName("Ability Messages / Climbing Gloves")] ability_Message_ClimbingGloves,
    [InspectorName("Ability Messages / 10")] ability_Message_10,
    [InspectorName("Ability Messages / 11")] ability_Message_11,
    [InspectorName("Ability Messages / 12")] ability_Message_12,
    [InspectorName("Ability Messages / 13")] ability_Message_13,
    [InspectorName("Ability Messages / 14")] ability_Message_14,
    #endregion

    #region Pickup Messages
    [InspectorName("Pickup Messages / Essence 1")] pickup_Message_Eccence_1,
    [InspectorName("Pickup Messages / Essence 2")] pickup_Message_Eccence_2,
    [InspectorName("Pickup Messages / Essence 3")] pickup_Message_Eccence_3,
    [InspectorName("Pickup Messages / Footprint")] pickup_Message_Footprint,
    [InspectorName("Pickup Messages / Skin First")] pickup_Message_Skin_First,
    [InspectorName("Pickup Messages / Skin")] pickup_Message_Skin,
    [InspectorName("Pickup Messages / 7")] pickup_Message_7,
    [InspectorName("Pickup Messages / 8")] pickup_Message_8,
    [InspectorName("Pickup Messages / 9")] pickup_Message_9,
    [InspectorName("Pickup Messages / 10")] pickup_Message_10,
    [InspectorName("Pickup Messages / 11")] pickup_Message_11,
    [InspectorName("Pickup Messages / 12")] pickup_Message_12,
    [InspectorName("Pickup Messages / 13")] pickup_Message_13,
    [InspectorName("Pickup Messages / 14")] pickup_Message_14,
    [InspectorName("Pickup Messages / 15")] pickup_Message_15,
    #endregion

    #region Tutorial Messages
    [InspectorName("Tutorial Messages / Movement")] tutorial_Message_Movement,
    [InspectorName("Tutorial Messages / Camera Rotation")] tutorial_Message_CameraRotation,
    [InspectorName("Tutorial Messages / Respawn")] tutorial_Message_Respawn,
    [InspectorName("Tutorial Messages / FreeCam 1")] tutorial_Message_FreeCam_1,
    [InspectorName("Tutorial Messages / FreeCam 2")] tutorial_Message_FreeCam_2,
    [InspectorName("Tutorial Messages / Demo")] tutorial_Message_Demo,
    [InspectorName("Tutorial Messages / 7")] tutorial_Message_7,
    [InspectorName("Tutorial Messages / 8")] tutorial_Message_8,
    [InspectorName("Tutorial Messages / 9")] tutorial_Message_9,
    [InspectorName("Tutorial Messages / 10")] tutorial_Message_10,
    [InspectorName("Tutorial Messages / 11")] tutorial_Message_11,
    [InspectorName("Tutorial Messages / 12")] tutorial_Message_12,
    [InspectorName("Tutorial Messages / 13")] tutorial_Message_13,
    [InspectorName("Tutorial Messages / 14")] tutorial_Message_14,
    [InspectorName("Tutorial Messages / 15")] tutorial_Message_15,
    #endregion

    #region InterractableButton Message
    [InspectorName("InterractableButton Message / Talk")] interractableButton_Message_Talk,
    [InspectorName("InterractableButton Message / Interract")] interractableButton_Message_Interract,
    [InspectorName("InterractableButton Message / Flippers UP")] interractableButton_Message_FlippersUP,
    [InspectorName("InterractableButton Message / Flippers DOWN")] interractableButton_Message_FlippersDOWN,
    [InspectorName("InterractableButton Message / Drill Helmet")] interractableButton_Message_DrillHelmet,
    [InspectorName("InterractableButton Message / Drill Shoes")] interractableButton_Message_DrillShoes,
    [InspectorName("InterractableButton Message / Hand Drill")] interractableButton_Message_HandDrill,
    [InspectorName("InterractableButton Message / Grappling Hook")] interractableButton_Message_GrapplingHook,
    [InspectorName("InterractableButton Message / Spring Shoes")] interractableButton_Message_SpringShoes,
    [InspectorName("InterractableButton Message / Climbing Gloves")] interractableButton_Message_ClimbingGloves,
    [InspectorName("InterractableButton Message / Respawn")] interractableButton_Message_Respawn,
    [InspectorName("InterractableButton Message / 12")] interractableButton_Message_12,
    [InspectorName("InterractableButton Message / 13")] interractableButton_Message_13,
    [InspectorName("InterractableButton Message / 14")] interractableButton_Message_14,
    [InspectorName("InterractableButton Message / 15")] interractableButton_Message_15,
    [InspectorName("InterractableButton Message / 16")] interractableButton_Message_16,
    [InspectorName("InterractableButton Message / 17")] interractableButton_Message_17,
    [InspectorName("InterractableButton Message / 18")] interractableButton_Message_18,
    [InspectorName("InterractableButton Message / 19")] interractableButton_Message_19,
    [InspectorName("InterractableButton Message / 20")] interractableButton_Message_20,
    [InspectorName("InterractableButton Message / 21")] interractableButton_Message_21,
    [InspectorName("InterractableButton Message / 22")] interractableButton_Message_22,
    [InspectorName("InterractableButton Message / 23")] interractableButton_Message_23,
    [InspectorName("InterractableButton Message / 24")] interractableButton_Message_24,
    [InspectorName("InterractableButton Message / 25")] interractableButton_Message_25,
    #endregion

#endregion
}