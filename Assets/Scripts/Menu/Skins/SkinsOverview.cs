using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsOverview : Singleton<SkinsOverview>
{
    //[Header("Skin Names")]
    //public string skin_Default_Name;

    //public string skin_Rivergreen_Lv_1_Name;
    //public string skin_Rivergreen_Lv_2_Name;
    //public string skin_Rivergreen_Lv_3_Name;
    //public string skin_Rivergreen_Lv_4_Name;
    //public string skin_Rivergreen_Lv_5_Name;

    //public string skin_Sandlands_Lv_1_Name;
    //public string skin_Sandlands_Lv_2_Name;
    //public string skin_Sandlands_Lv_3_Name;
    //public string skin_Sandlands_Lv_4_Name;
    //public string skin_Sandlands_Lv_5_Name;

    //public string skin_Frostfield_Lv_1_Name;
    //public string skin_Frostfield_Lv_2_Name;
    //public string skin_Frostfield_Lv_3_Name;
    //public string skin_Frostfield_Lv_4_Name;
    //public string skin_Frostfield_Lv_5_Name;

    //public string skin_Firevein_Lv_1_Name;
    //public string skin_Firevein_Lv_2_Name;
    //public string skin_Firevein_Lv_3_Name;
    //public string skin_Firevein_Lv_4_Name;
    //public string skin_Firevein_Lv_5_Name;

    //public string skin_Witchmire_Lv_1_Name;
    //public string skin_Witchmire_Lv_2_Name;
    //public string skin_Witchmire_Lv_3_Name;
    //public string skin_Witchmire_Lv_4_Name;
    //public string skin_Witchmire_Lv_5_Name;

    //public string skin_Metalworks_Lv_1_Name;
    //public string skin_Metalworks_Lv_2_Name;
    //public string skin_Metalworks_Lv_3_Name;
    //public string skin_Metalworks_Lv_4_Name;
    //public string skin_Metalworks_Lv_5_Name;

    //[Header("Hat Names")]
    //public string hat_Floríel;
    //public string hat_Aisa;
    //public string hat_Archie;
    //public string hat_Granith;
    //public string hat_Mossy;
    //public string hat_Larry;


    //--------------------


    public string GetSkinName(SkinType skin)
    {
        switch (skin)
        {
            case SkinType.None:
                return null;

            case SkinType.Default:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_Default;

            case SkinType.Rivergreen_Lv1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_WaterRegion_Lv1 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water;
            case SkinType.Rivergreen_Lv2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_WaterRegion_Lv2 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water;
            case SkinType.Rivergreen_Lv3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_WaterRegion_Lv3 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water;
            case SkinType.Rivergreen_Lv4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_WaterRegion_Lv4 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water;
            case SkinType.Rivergreen_Lv5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_WaterRegion_Lv5 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Water;

            case SkinType.Sandlands_Lv1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SandRegion_Lv1 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand;
            case SkinType.Sandlands_Lv2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SandRegion_Lv2 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand;
            case SkinType.Sandlands_Lv3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SandRegion_Lv3 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand;
            case SkinType.Sandlands_Lv4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SandRegion_Lv4 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand;
            case SkinType.Sandlands_Lv5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SandRegion_Lv5 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Sand;

            case SkinType.Frostfield_Lv1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_IceRegion_Lv1 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice;
            case SkinType.Frostfield_Lv2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_IceRegion_Lv2 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice;
            case SkinType.Frostfield_Lv3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_IceRegion_Lv3 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice;
            case SkinType.Frostfield_Lv4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_IceRegion_Lv4 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice;
            case SkinType.Frostfield_Lv5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_IceRegion_Lv5 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Ice;

            case SkinType.Firevein_Lv1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_LavaRegion_Lv1 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava;
            case SkinType.Firevein_Lv2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_LavaRegion_Lv2 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava;
            case SkinType.Firevein_Lv3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_LavaRegion_Lv3 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava;
            case SkinType.Firevein_Lv4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_LavaRegion_Lv4 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava;
            case SkinType.Firevein_Lv5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_LavaRegion_Lv5 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Lava;

            case SkinType.Witchmire_Lv1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SwampRegion_Lv1 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp;
            case SkinType.Witchmire_Lv2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SwampRegion_Lv2 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp;
            case SkinType.Witchmire_Lv3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SwampRegion_Lv3 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp;
            case SkinType.Witchmire_Lv4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SwampRegion_Lv4 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp;
            case SkinType.Witchmire_Lv5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_SwampRegion_Lv5 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Swamp;

            case SkinType.Metalworks_Lv1:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_MetalRegion_Lv1 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal;
            case SkinType.Metalworks_Lv2:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_MetalRegion_Lv2 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal;
            case SkinType.Metalworks_Lv3:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_MetalRegion_Lv3 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal;
            case SkinType.Metalworks_Lv4:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_MetalRegion_Lv4 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal;
            case SkinType.Metalworks_Lv5:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].skinName_MetalRegion_Lv5 + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].name_Region_Metal;

            default:
                return null;
        }
    }

    public string GetHatName(HatType hat)
    {
        switch (hat)
        {
            case HatType.None:
                return "";

            case HatType.Floriel_Hat:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_Water + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Water;
            case HatType.Granith_Hat:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_Lava + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Lava;
            case HatType.Archie_Hat:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_Sand + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Sand;
            case HatType.Aisa_Hat:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_Ice + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Ice;
            case HatType.Mossy_Hat:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_Swamp + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Swamp;
            case HatType.Larry_Hat:
                return DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCHat_Name_Metal + " " + DataManager.Instance.game_TextDatabase_Store.gameText_LanguageList[(int)DataManager.Instance.settingData_StoreList.currentLanguage].NPCName_Metal;

            default:
                return "";
        }
    }
}
