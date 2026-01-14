using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelInfoManager : Singleton<LevelInfoManager>
{
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
    public Sprite glueplantSprite_Rivergreen;
    public Sprite glueplantSprite_Sandlands;
    public Sprite glueplantSprite_Frostfield;
    public Sprite glueplantSprite_Firevein;
    public Sprite glueplantSprite_Witchmire;
    public Sprite glueplantSprite_Metalworks;
    #endregion

    [Header("Skin Sprites")]
    #region Skin Sprites
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
        UpdateProgressBars(-1, 0, 0, 0);

        GameObject activeLevelObject = RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement;

        if (activeLevelObject.GetComponent<LoadLevel>())
        {
            bool foundLevel = false;

            //Get Level Image
            if (activeLevelObject.GetComponent<LoadLevel>().levelSprite)
                levelImage.sprite = activeLevelObject.GetComponent<LoadLevel>().levelSprite;
            else
                levelImage.sprite = null;

            //Level Name
            levelName.text = activeLevelObject.GetComponent<LevelInfo>().GetName();

            //Find the correct mapInfo
            if (menuLevelInfo && menuLevelInfo.mapInfo_ToSave != null && menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List.Count > 0)
            {
                for (int i = 0; i < menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List.Count; i++)
                {
                    if (menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List[i].mapName == activeLevelObject.GetComponent<LoadLevel>().levelToPlay)
                    {
                        //Glueplant aquired
                        switch (OverWorldManager.Instance.regionState)
                        {
                            case RegionState.None:
                                break;

                            case RegionState.Rivergreen:
                                glueplantImage.sprite = glueplantSprite_Rivergreen;
                                break;
                            case RegionState.Sandlands:
                                glueplantImage.sprite = glueplantSprite_Sandlands;
                                break;
                            case RegionState.Frostfields:
                                glueplantImage.sprite = glueplantSprite_Frostfield;
                                break;
                            case RegionState.Firevein:
                                glueplantImage.sprite = glueplantSprite_Firevein;
                                break;
                            case RegionState.Witchmire:
                                glueplantImage.sprite = glueplantSprite_Witchmire;
                                break;
                            case RegionState.Metalworks:
                                glueplantImage.sprite = glueplantSprite_Metalworks;
                                break;

                            default:
                                break;
                        }

                        if (menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List[i].isCompleted)
                            glueplant_Aquired.text = "1 / 1";
                        else
                            glueplant_Aquired.text = "0 / 1";

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
                        float skinCounter = 0;
                        if (menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List[i].levelSkin.isTaken)
                        {
                            skinCounter++;
                        }
                        Skin_Aquired.text = skinCounter + " / 1";

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

                        foundLevel = true;

                        //Set Progress Bars
                        UpdateProgressBars(i, essenceCounter, skinCounter, stepsCounter);

                        break;
                    }
                }
            }

            //Get Skin Type to get in the Level
            skinImage.sprite = SelectSpriteForLevel(activeLevelObject.GetComponent<LoadLevel>().skinTypeInLevel);

            //Get Abilities to get in the Level
            ability_Swimming.SetActive(false);
            ability_SwiftSwim.SetActive(false);
            ability_Flippers.SetActive(false);

            ability_Ascend.SetActive(false);
            ability_Descend.SetActive(false);

            ability_Dash.SetActive(false);
            ability_Jumping.SetActive(false);
            ability_GrapplingHook.SetActive(false);
            ability_CeilingGrab.SetActive(false);

            if (activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel.Snorkel)
                ability_Swimming.SetActive(true);
            if (activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel.Flippers)
                ability_SwiftSwim.SetActive(true);
            if (activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel.OxygenTank)
                ability_Flippers.SetActive(true);

            if (activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel.DrillHelmet)
                ability_Ascend.SetActive(true);
            if (activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel.DrillBoots)
                ability_Descend.SetActive(true);

            if (activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel.HandDrill)
                ability_Dash.SetActive(true);
            if (activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel.SpringShoes)
                ability_Jumping.SetActive(true);
            if (activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel.GrapplingHook)
                ability_GrapplingHook.SetActive(true);
            if (activeLevelObject.GetComponent<LoadLevel>().abilitiesInLevel.ClimingGloves)
                ability_CeilingGrab.SetActive(true);

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

    void SetupDefaultLevelDisplay()
    {
        //Glueplant aquired
        glueplant_Aquired.text = "0 / 1";

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


    void UpdateProgressBars(int index, float essenceCounter, float skinCounter, float stepsCounter)
    {
        if (index < 0)
        {
            SetProgressBar(glueplant_FillImage, 0f, 1);
        }
        else
        {
            if (menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List[index].isCompleted)
                SetProgressBar(glueplant_FillImage, 1f, 1);
            else
                SetProgressBar(glueplant_FillImage, 0f, 1);
        }
        
        SetProgressBar(essence_FillImage, essenceCounter, 10);
        SetProgressBar(skin_FillImage, skinCounter, 1);
        SetProgressBar(footprint_FillImage, stepsCounter, 3);

        if (index < 0)
        {
            SetProgressBar(levelName_FillImage, 0f + essenceCounter + skinCounter + stepsCounter, 15);
        }
        else
        {
            if (menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List[index].isCompleted)
                SetProgressBar(levelName_FillImage, 1f + essenceCounter + skinCounter + stepsCounter, 15);
            else
                SetProgressBar(levelName_FillImage, 0f + essenceCounter + skinCounter + stepsCounter, 15);
        }
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
    Metalworks
}