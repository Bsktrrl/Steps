using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelInfoManager : Singleton<LevelInfoManager>
{
    private static readonly Color AcquiredAbilityColor = new Color32(181, 147, 213, 255);

    [Header("LevelDisplay")]
    [SerializeField] GameObject levelDisplay_Parent;
    [SerializeField] TextMeshProUGUI levelName;
    [SerializeField] Image levelImage;
    [SerializeField] Image glueplantImage;
    [SerializeField] Image skinImage;

    [SerializeField] TextMeshProUGUI glueplant_Aquired;
    [SerializeField] TextMeshProUGUI Essence_Aquired;
    [SerializeField] TextMeshProUGUI Skin_Aquired;
    [SerializeField] TextMeshProUGUI StepsMax_Aquired;

    [SerializeField] GameObject ability_Swimming;
    [SerializeField] GameObject ability_SwiftSwim;
    [SerializeField] GameObject ability_Flippers;
    [SerializeField] GameObject ability_Ascend;
    [SerializeField] GameObject ability_Descend;
    [SerializeField] GameObject ability_Dash;
    [SerializeField] GameObject ability_Jumping;
    [SerializeField] GameObject ability_GrapplingHook;
    [SerializeField] GameObject ability_CeilingGrab;

    [Header("LevelInfo Display")]
    [SerializeField] Image levelName_FillImage;
    [SerializeField] Image glueplant_FillImage;
    [SerializeField] Image essence_FillImage;
    [SerializeField] Image skin_FillImage;
    [SerializeField] Image footprint_FillImage;


    [Header("Glueplant Sprites")]
    #region Glueplant Sprites
    public Sprite glueplantSprite_HUB;
    public Sprite glueplantSprite_Rivergreen;
    public Sprite glueplantSprite_Sandlands;
    public Sprite glueplantSprite_Frostfield;
    public Sprite glueplantSprite_Firevein;
    public Sprite glueplantSprite_Witchmire;
    public Sprite glueplantSprite_Metalworks;
    #endregion

    [Header("Skin Sprites")]
    #region Skin Sprites
    public Sprite sprite_HUB_Quartz;
    public Sprite sprite_HUB_SourceBlock;

    public Sprite sprite_Rivergreen_Lv1;
    public Sprite sprite_Rivergreen_Lv2;
    public Sprite sprite_Rivergreen_Lv3;
    public Sprite sprite_Rivergreen_Lv4;
    public Sprite sprite_Rivergreen_Lv5;

    public Sprite sprite_Sandlands_Lv1;
    public Sprite sprite_Sandlands_Lv2;
    public Sprite sprite_Sandlands_Lv3;
    public Sprite sprite_Sandlands_Lv4;
    public Sprite sprite_Sandlands_Lv5;

    public Sprite sprite_Frostfield_Lv1;
    public Sprite sprite_Frostfield_Lv2;
    public Sprite sprite_Frostfield_Lv3;
    public Sprite sprite_Frostfield_Lv4;
    public Sprite sprite_Frostfield_Lv5;

    public Sprite sprite_Firevein_Lv1;
    public Sprite sprite_Firevein_Lv2;
    public Sprite sprite_Firevein_Lv3;
    public Sprite sprite_Firevein_Lv4;
    public Sprite sprite_Firevein_Lv5;

    public Sprite sprite_Witchmire_Lv1;
    public Sprite sprite_Witchmire_Lv2;
    public Sprite sprite_Witchmire_Lv3;
    public Sprite sprite_Witchmire_Lv4;
    public Sprite sprite_Witchmire_Lv5;

    public Sprite sprite_Metalworks_Lv1;
    public Sprite sprite_Metalworks_Lv2;
    public Sprite sprite_Metalworks_Lv3;
    public Sprite sprite_Metalworks_Lv4;
    public Sprite sprite_Metalworks_Lv5;
    #endregion


    MenuLevelInfo menuLevelInfo;


    //--------------------


    private void Start()
    {
        menuLevelInfo = FindAnyObjectByType<MenuLevelInfo>();
    }


    //--------------------


    private void OnEnable()
    {
        //SetupLevelDisplay();

        RememberCurrentlySelectedUIElement.Action_ChangedSelectedUIElement += SetupLevelDisplay;
    }
    private void OnDisable()
    {
        RememberCurrentlySelectedUIElement.Action_ChangedSelectedUIElement -= SetupLevelDisplay;
    }


    //--------------------


    public void SetupLevelDisplay()
    {
        UpdateProgressBars(-1, 0, 0, 0, 0);

        GameObject activeLevelObject = RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement;

        if (activeLevelObject.GetComponent<LoadLevel>())
        {
            bool foundLevel = false;
            AbilitiesGot collectedAbilities = null;

            //Get Level Image
            if (activeLevelObject.GetComponent<LoadLevel>().levelSprite)
                levelImage.sprite = activeLevelObject.GetComponent<LoadLevel>().levelSprite;
            else
                levelImage.sprite = null;

            //Level Name
            levelName.text = activeLevelObject.GetComponent<TMPro_TextDisplay>().GetText();

            //Find the correct mapInfo

            //Glueplant aquired
            glueplantImage.sprite = SelectGlueplantForLevel(activeLevelObject.GetComponent<LoadLevel>().regionToPlay, activeLevelObject.GetComponent<LoadLevel>().levelToPlay);


            if (menuLevelInfo && menuLevelInfo.mapInfo_ToSave != null && menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List.Count > 0)
            {
                for (int i = 0; i < menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List.Count; i++)
                {
                    if (menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List[i].mapName == activeLevelObject.GetComponent<LoadLevel>().levelToPlay)
                    {
                        Map_SaveInfo map = menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List[i];
                        StandStats stands = DataManager.Instance.glueplantStandStats_Store?.GetForLevel(map.mapName);
                        int glueplantPlacedCounter = stands?.CountTaken ?? (map.isCompleted ? 1 : 0);
                        int glueplantTotal = GetGlueplantTotal();
                        glueplant_Aquired.text = glueplantPlacedCounter + " / " + glueplantTotal;

                        //Essence aquired
                        float essenceCounter = 0;
                        for (int j = 0; j < menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List[i].essenceList.Count; j++)
                        {
                            if (menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List[i].essenceList[j].isTaken)
                            {
                                essenceCounter++;
                            }
                        }
                        Essence_Aquired.text = essenceCounter + " / 10";

                        //Skin aquired
                        int skinCounter = map.CountSkinsTaken();
                        Skin_Aquired.text = skinCounter + " / " + map.SkinPickupTotal;

                        //StepMax aquired
                        float stepsCounter = 0;
                        for (int j = 0; j < menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List[i].maxStepList.Count; j++)
                        {
                            if (menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List[i].maxStepList[j].isTaken)
                            {
                                stepsCounter++;
                            }
                        }
                        StepsMax_Aquired.text = stepsCounter + " / 3";

                        collectedAbilities = menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List[i].abilitiesGotInLevel;
                        foundLevel = true;

                        UpdateProgressBars(i, glueplantPlacedCounter, essenceCounter, skinCounter, stepsCounter);

                        break;
                    }
                }
            }

            //Get Skin Type to get in the Level
            skinImage.sprite = SelectSpriteForLevel(activeLevelObject.GetComponent<LoadLevel>().skinTypeInLevel);

            ShowLevelAbilities(activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel, collectedAbilities);

            if (!foundLevel)
            {
                SetupDefaultLevelDisplay();
            }

            if (activeLevelObject.GetComponent<LoadLevel>().readyToBePlayedAndDisplayed)
                levelDisplay_Parent.SetActive(true);
            else
                levelDisplay_Parent.SetActive(false);

            return;
        }

        levelDisplay_Parent.SetActive(false);
        return;
    }

    private void ShowLevelAbilities(AbilitiesGot available, AbilitiesGot collected)
    {
        var permanent = DataManager.Instance.playerStats_Store?.abilitiesGot_Permanent;

        SetIcon(ability_Swimming, Abilities.Snorkel);
        SetIcon(ability_SwiftSwim, Abilities.Flippers);
        SetIcon(ability_Flippers, Abilities.OxygenTank);
        SetIcon(ability_Ascend, Abilities.DrillHelmet);
        SetIcon(ability_Descend, Abilities.DrillBoots);
        SetIcon(ability_Dash, Abilities.HandDrill);
        SetIcon(ability_Jumping, Abilities.SpringShoes);
        SetIcon(ability_GrapplingHook, Abilities.GrapplingHook);
        SetIcon(ability_CeilingGrab, Abilities.ClimingGloves);

        void SetIcon(GameObject icon, Abilities ability)
        {
            bool isAvailable = available?.HasAbility(ability) == true;
            icon.SetActive(isAvailable);

            if (isAvailable && icon.TryGetComponent(out Image image))
            {
                bool acquired = collected?.HasAbility(ability) == true || permanent?.HasAbility(ability) == true;
                image.color = acquired ? AcquiredAbilityColor : Color.white;
            }
        }
    }

    void SetupDefaultLevelDisplay()
    {
        //Glueplant aquired
        glueplant_Aquired.text = "0 / " + GetGlueplantTotal();

        //Essence aquired
        Essence_Aquired.text = "0 / 10";

        //Skin aquired
        Skin_Aquired.text = "0 / 1";

        //StepMax aquired
        StepsMax_Aquired.text = "0 / 3";
    }

    public Sprite SelectSpriteForLevel(SkinType skinType)
    {
        switch (skinType)
        {
            case SkinType.None:
                return null;

            case SkinType.Quartz:
                return sprite_HUB_Quartz;
            case SkinType.SourceBlock:
                return sprite_HUB_SourceBlock;

            case SkinType.Rivergreen_Lv1:
                return sprite_Rivergreen_Lv1;
            case SkinType.Rivergreen_Lv2:
                return sprite_Rivergreen_Lv2;
            case SkinType.Rivergreen_Lv3:
                return sprite_Rivergreen_Lv3;
            case SkinType.Rivergreen_Lv4:
                return sprite_Rivergreen_Lv4;
            case SkinType.Rivergreen_Lv5:
                return sprite_Rivergreen_Lv5;

            case SkinType.Firevein_Lv1:
                return sprite_Firevein_Lv1;
            case SkinType.Firevein_Lv2:
                return sprite_Firevein_Lv2;
            case SkinType.Firevein_Lv3:
                return sprite_Firevein_Lv3;
            case SkinType.Firevein_Lv4:
                return sprite_Firevein_Lv4;
            case SkinType.Firevein_Lv5:
                return sprite_Firevein_Lv5;

            case SkinType.Sandlands_Lv1:
                return sprite_Sandlands_Lv1;
            case SkinType.Sandlands_Lv2:
                return sprite_Sandlands_Lv2;
            case SkinType.Sandlands_Lv3:
                return sprite_Sandlands_Lv3;
            case SkinType.Sandlands_Lv4:
                return sprite_Sandlands_Lv4;
            case SkinType.Sandlands_Lv5:
                return sprite_Sandlands_Lv5;

            case SkinType.Frostfield_Lv1:
                return sprite_Frostfield_Lv1;
            case SkinType.Frostfield_Lv2:
                return sprite_Frostfield_Lv2;
            case SkinType.Frostfield_Lv3:
                return sprite_Frostfield_Lv3;
            case SkinType.Frostfield_Lv4:
                return sprite_Frostfield_Lv4;
            case SkinType.Frostfield_Lv5:
                return sprite_Frostfield_Lv5;

            case SkinType.Witchmire_Lv1:
                return sprite_Witchmire_Lv1;
            case SkinType.Witchmire_Lv2:
                return sprite_Witchmire_Lv2;
            case SkinType.Witchmire_Lv3:
                return sprite_Witchmire_Lv3;
            case SkinType.Witchmire_Lv4:
                return sprite_Witchmire_Lv4;
            case SkinType.Witchmire_Lv5:
                return sprite_Witchmire_Lv5;

            case SkinType.Metalworks_Lv1:
                return sprite_Metalworks_Lv1;
            case SkinType.Metalworks_Lv2:
                return sprite_Metalworks_Lv2;
            case SkinType.Metalworks_Lv3:
                return sprite_Metalworks_Lv3;
            case SkinType.Metalworks_Lv4:
                return sprite_Metalworks_Lv4;
            case SkinType.Metalworks_Lv5:
                return sprite_Metalworks_Lv5;

            default:
                return null;
        }
    }

    public Sprite SelectGlueplantForLevel(regions region, string level)
    {
        //Glueplant aquired
        switch (region)
        {
            case regions.None:
                //print("0. Glueplant: None");
                return null;

            case regions.HUB:
                //print("1. Glueplant: Rivergreen");
                return glueplantSprite_HUB;

            case regions.Rivergreen:
                //print("1. Glueplant: Rivergreen");
                return glueplantSprite_Rivergreen;
            case regions.Sandlands:
                //print("2. Glueplant: Sandlands");
                return glueplantSprite_Sandlands;
            case regions.Frostfield:
                //print("3. Glueplant: Frostfield");
                return glueplantSprite_Frostfield;
            case regions.Firevein:
                //print("4. Glueplant: Firevein");
                return glueplantSprite_Firevein;
            case regions.Witchmire:
                //print("5. Glueplant: Witchmire");
                return glueplantSprite_Witchmire;
            case regions.Metalworks:
                switch (level)
                {
                    case "Metalworks_Lv_1":
                        return glueplantSprite_Rivergreen;
                    case "Metalworks_Lv_2":
                        return glueplantSprite_Sandlands;
                    case "Metalworks_Lv_3":
                        return glueplantSprite_Frostfield;
                    case "Metalworks_Lv_4":
                        return glueplantSprite_Firevein;
                    case "Metalworks_Lv_5":
                        return glueplantSprite_Witchmire;

                    default:
                        return null;
                }

            default:
                //print("00. Glueplant: None");
                return null;
        }
    }

    int GetGlueplantTotal() => OverWorldManager.Instance.regionState == RegionState.Metalworks ? StandStats.StandsPerLevel : 1;

    void UpdateProgressBars(int index, float glueplantCounter, float essenceCounter, float skinCounter, float stepsCounter)
    {
        int glueplantTotal = GetGlueplantTotal();
        int skinTotal = index < 0 ? 1 : menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List[index].SkinPickupTotal;
        SetProgressBar(glueplant_FillImage, glueplantCounter, glueplantTotal);
        SetProgressBar(essence_FillImage, essenceCounter, 10);
        SetProgressBar(skin_FillImage, skinCounter, skinTotal);
        SetProgressBar(footprint_FillImage, stepsCounter, 3);
        SetProgressBar(levelName_FillImage, glueplantCounter + essenceCounter + skinCounter + stepsCounter, 13 + glueplantTotal + skinTotal);
    }
    void SetProgressBar(Image fillImage, float current, float max)
    {
        fillImage.fillAmount = (current / max);
    }
}

public enum SkinType
{
    None,

    Default,

    Rivergreen_Lv1,
    Rivergreen_Lv2,
    Rivergreen_Lv3,
    Rivergreen_Lv4,
    Rivergreen_Lv5,

    Sandlands_Lv1,
    Sandlands_Lv2,
    Sandlands_Lv3,
    Sandlands_Lv4,
    Sandlands_Lv5,

    Frostfield_Lv1,
    Frostfield_Lv2,
    Frostfield_Lv3,
    Frostfield_Lv4,
    Frostfield_Lv5,

    Firevein_Lv1,
    Firevein_Lv2,
    Firevein_Lv3,
    Firevein_Lv4,
    Firevein_Lv5,

    Witchmire_Lv1,
    Witchmire_Lv2,
    Witchmire_Lv3,
    Witchmire_Lv4,
    Witchmire_Lv5,

    Metalworks_Lv1,
    Metalworks_Lv2,
    Metalworks_Lv3,
    Metalworks_Lv4,
    Metalworks_Lv5,

    Quartz,
    SourceBlock
}

public enum HatType
{
    None,

    Floriel_Hat,
    Granith_Hat,
    Archie_Hat,
    Aisa_Hat,
    Mossy_Hat,
    Larry_Hat
}

public enum RegionName
{
    None,

    Rivergreen,
    Sandlands,
    Frostfields,
    Firevein,
    Witchmire,
    Metalworks,
    HUB
}
