using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsOverview : Singleton<SkinsOverview>
{
    [Header("Skin Names")]
    public string skin_Default_Name;

    public string skin_Rivergreen_Lv_1_Name;
    public string skin_Rivergreen_Lv_2_Name;
    public string skin_Rivergreen_Lv_3_Name;
    public string skin_Rivergreen_Lv_4_Name;
    public string skin_Rivergreen_Lv_5_Name;

    public string skin_Sandlands_Lv_1_Name;
    public string skin_Sandlands_Lv_2_Name;
    public string skin_Sandlands_Lv_3_Name;
    public string skin_Sandlands_Lv_4_Name;
    public string skin_Sandlands_Lv_5_Name;

    public string skin_Frostfield_Lv_1_Name;
    public string skin_Frostfield_Lv_2_Name;
    public string skin_Frostfield_Lv_3_Name;
    public string skin_Frostfield_Lv_4_Name;
    public string skin_Frostfield_Lv_5_Name;

    public string skin_Firevein_Lv_1_Name;
    public string skin_Firevein_Lv_2_Name;
    public string skin_Firevein_Lv_3_Name;
    public string skin_Firevein_Lv_4_Name;
    public string skin_Firevein_Lv_5_Name;

    public string skin_Witchmire_Lv_1_Name;
    public string skin_Witchmire_Lv_2_Name;
    public string skin_Witchmire_Lv_3_Name;
    public string skin_Witchmire_Lv_4_Name;
    public string skin_Witchmire_Lv_5_Name;

    public string skin_Metalworks_Lv_1_Name;
    public string skin_Metalworks_Lv_2_Name;
    public string skin_Metalworks_Lv_3_Name;
    public string skin_Metalworks_Lv_4_Name;
    public string skin_Metalworks_Lv_5_Name;

    [Header("Hat Names")]
    public string hat_Floríel;
    public string hat_Aisa;
    public string hat_Archie;
    public string hat_Granith;
    public string hat_Mossy;
    public string hat_Larry;


    //--------------------


    public string GetSkinName(SkinType skin)
    {
        switch (skin)
        {
            case SkinType.None:
                return null;

            case SkinType.Default:
                return skin_Default_Name;

            case SkinType.Rivergreen_Lv1:
                return skin_Rivergreen_Lv_1_Name;
            case SkinType.Rivergreen_Lv2:
                return skin_Rivergreen_Lv_2_Name;
            case SkinType.Rivergreen_Lv3:
                return skin_Rivergreen_Lv_3_Name;
            case SkinType.Rivergreen_Lv4:
                return skin_Rivergreen_Lv_4_Name;
            case SkinType.Rivergreen_Lv5:
                return skin_Rivergreen_Lv_5_Name;

            case SkinType.Sandlands_Lv1:
                return skin_Sandlands_Lv_1_Name;
            case SkinType.Sandlands_Lv2:
                return skin_Sandlands_Lv_2_Name;
            case SkinType.Sandlands_Lv3:
                return skin_Sandlands_Lv_3_Name;
            case SkinType.Sandlands_Lv4:
                return skin_Sandlands_Lv_4_Name;
            case SkinType.Sandlands_Lv5:
                return skin_Sandlands_Lv_5_Name;

            case SkinType.Frostfield_Lv1:
                return skin_Frostfield_Lv_1_Name;
            case SkinType.Frostfield_Lv2:
                return skin_Frostfield_Lv_2_Name;
            case SkinType.Frostfield_Lv3:
                return skin_Frostfield_Lv_3_Name;
            case SkinType.Frostfield_Lv4:
                return skin_Frostfield_Lv_4_Name;
            case SkinType.Frostfield_Lv5:
                return skin_Frostfield_Lv_5_Name;

            case SkinType.Firevein_Lv1:
                return skin_Firevein_Lv_1_Name;
            case SkinType.Firevein_Lv2:
                return skin_Firevein_Lv_2_Name;
            case SkinType.Firevein_Lv3:
                return skin_Firevein_Lv_3_Name;
            case SkinType.Firevein_Lv4:
                return skin_Firevein_Lv_4_Name;
            case SkinType.Firevein_Lv5:
                return skin_Firevein_Lv_5_Name;

            case SkinType.Witchmire_Lv1:
                return skin_Witchmire_Lv_1_Name;
            case SkinType.Witchmire_Lv2:
                return skin_Witchmire_Lv_2_Name;
            case SkinType.Witchmire_Lv3:
                return skin_Witchmire_Lv_3_Name;
            case SkinType.Witchmire_Lv4:
                return skin_Witchmire_Lv_4_Name;
            case SkinType.Witchmire_Lv5:
                return skin_Witchmire_Lv_5_Name;

            case SkinType.Metalworks_Lv1:
                return skin_Metalworks_Lv_1_Name;
            case SkinType.Metalworks_Lv2:
                return skin_Metalworks_Lv_2_Name;
            case SkinType.Metalworks_Lv3:
                return skin_Metalworks_Lv_3_Name;
            case SkinType.Metalworks_Lv4:
                return skin_Metalworks_Lv_4_Name;
            case SkinType.Metalworks_Lv5:
                return skin_Metalworks_Lv_5_Name;

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
                return hat_Floríel;
            case HatType.Granith_Hat:
                return hat_Granith;
            case HatType.Archie_Hat:
                return hat_Archie;
            case HatType.Aisa_Hat:
                return hat_Aisa;
            case HatType.Mossy_Hat:
                return hat_Mossy;
            case HatType.Larry_Hat:
                return hat_Larry;

            default:
                return "";
        }
    }
}
