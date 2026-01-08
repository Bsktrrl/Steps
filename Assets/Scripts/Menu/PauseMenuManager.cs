using System.Collections;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuManager : Singleton<PauseMenuManager>
{
    public static event Action Action_closePauseMenu;

    public bool isVisible;

    [Header("Parents")]
    public GameObject pauseMenu_Parent;

    public GameObject pauseMenu_MainMenu_Parent;
    public GameObject pauseMenu_Skins_Parent;
    public GameObject pauseMenu_Options_Parent;

    public GameObject pauseMenu_StartButton;

    [Header("LevelDisplay")]
    public GameObject levelDisplay_Parent;
    [SerializeField] TextMeshProUGUI levelName;
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

    [Header("Glueplant Sprites")]
    #region Sprites
    public Sprite glueplant_PauseMenu_Rivergreen;
    public Sprite glueplant_PauseMenu_Sandlands;
    public Sprite glueplant_PauseMenu_Frostfield;
    public Sprite glueplant_PauseMenu_Firevein;
    public Sprite glueplant_PauseMenu_Witchmire;
    public Sprite glueplant_PauseMenu_Metalworks;
    #endregion

    [Header("Skin Sprites")]
    #region Sprites
    public Sprite sprite_PauseMenu_Rivergreen_Lv1;
    public Sprite sprite_PauseMenu_Rivergreen_Lv2;
    public Sprite sprite_PauseMenu_Rivergreen_Lv3;
    public Sprite sprite_PauseMenu_Rivergreen_Lv4;
    public Sprite sprite_PauseMenu_Rivergreen_Lv5;

    public Sprite sprite_PauseMenu_Sandlands_Lv1;
    public Sprite sprite_PauseMenu_Sandlands_Lv2;
    public Sprite sprite_PauseMenu_Sandlands_Lv3;
    public Sprite sprite_PauseMenu_Sandlands_Lv4;
    public Sprite sprite_PauseMenu_Sandlands_Lv5;

    public Sprite sprite_PauseMenu_Frostfield_Lv1;
    public Sprite sprite_PauseMenu_Frostfield_Lv2;
    public Sprite sprite_PauseMenu_Frostfield_Lv3;
    public Sprite sprite_PauseMenu_Frostfield_Lv4;
    public Sprite sprite_PauseMenu_Frostfield_Lv5;

    public Sprite sprite_PauseMenu_Firevein_Lv1;
    public Sprite sprite_PauseMenu_Firevein_Lv2;
    public Sprite sprite_PauseMenu_Firevein_Lv3;
    public Sprite sprite_PauseMenu_Firevein_Lv4;
    public Sprite sprite_PauseMenu_Firevein_Lv5;

    public Sprite sprite_PauseMenu_Witchmire_Lv1;
    public Sprite sprite_PauseMenu_Witchmire_Lv2;
    public Sprite sprite_PauseMenu_Witchmire_Lv3;
    public Sprite sprite_PauseMenu_Witchmire_Lv4;
    public Sprite sprite_PauseMenu_Witchmire_Lv5;

    public Sprite sprite_PauseMenu_Metalworks_Lv1;
    public Sprite sprite_PauseMenu_Metalworks_Lv2;
    public Sprite sprite_PauseMenu_Metalworks_Lv3;
    public Sprite sprite_PauseMenu_Metalworks_Lv4;
    public Sprite sprite_PauseMenu_Metalworks_Lv5;
    #endregion


    //--------------------


    private void OnEnable()
    {
        Movement.Action_StepTaken += SetLevelInfo;
        Movement.Action_RespawnPlayer += SetLevelInfo;
        Map_SaveInfo.Action_SetupMap_hasLoaded += SetLevelInfo;
        SettingsManager.Action_SetNewLanguage += SetLevelInfo;
    }
    private void OnDisable()
    {
        Movement.Action_StepTaken -= SetLevelInfo;
        Movement.Action_RespawnPlayer -= SetLevelInfo;
        Map_SaveInfo.Action_SetupMap_hasLoaded -= SetLevelInfo;
        SettingsManager.Action_SetNewLanguage -= SetLevelInfo;
    }


    //--------------------


    public void OpenPauseMenu()
    {
        if (PlayerManager.Instance.npcInteraction) return;

        SetLevelInfo();

        pauseMenu_Parent.SetActive(true);
        pauseMenu_MainMenu_Parent.SetActive(true);

        //Set the first selected button for controller input
        EventSystem.current.SetSelectedGameObject(pauseMenu_StartButton);

        StartCoroutine(CheckPauseMenu());
    }

    public void ClosePauseMenu()
    {
        Action_closePauseMenu?.Invoke();

        ////Set the first selected button for controller input
        //EventSystem.current.SetSelectedGameObject(pauseMenu_StartButton);
        //print("8. pauseMenuManager = true");

        pauseMenu_Parent.SetActive(false);
        isVisible = false;
    }
    IEnumerator CheckPauseMenu()
    {
        ////Set the first selected button for controller input
        //EventSystem.current.SetSelectedGameObject(pauseMenu_StartButton);
        //print("9. pauseMenuManager = true");

        yield return null /*new WaitForSeconds(0.02f)*/;
        isVisible = true;
    }


    //--------------------


    void SetLevelInfo()
    {
        Map_SaveInfo mapInfo = SaveLoad_MapInfo.Instance.GetMapInfo(MapManager.Instance.mapInfo_ToSave.mapName);
        if (mapInfo == null ) { return; }

        //Set variables

        //Language
        switch (SettingsManager.Instance.settingsData.currentLanguage)
        {
            case Languages.Norwegian:
                levelName.text = mapInfo.mapNameDisplay.mapNameDisplay_norwegian;
                break;
            case Languages.English:
                levelName.text = mapInfo.mapNameDisplay.mapNameDisplay_english;
                break;
            case Languages.German:
                levelName.text = mapInfo.mapNameDisplay.mapNameDisplay_german;
                break;
            case Languages.Japanese:
                levelName.text = mapInfo.mapNameDisplay.mapNameDisplay_japanese;
                break;
            case Languages.Chinese:
                levelName.text = mapInfo.mapNameDisplay.mapNameDisplay_chinese;
                break;
            case Languages.Korean:
                levelName.text = mapInfo.mapNameDisplay.mapNameDisplay_korean;
                break;

            default:
                break;
        }

        //Skin
        skinImage.sprite = SelectSpriteForLevel(mapInfo.skintype);


        //-----


        //Changeable variables

        //Glueplant
        switch (mapInfo.skintype)
        {
            case SkinType.None:
                break;

            case SkinType.Default:
                break;

            case SkinType.Rivergreen_Lv1:
                glueplantImage.sprite = glueplant_PauseMenu_Rivergreen;
                break;
            case SkinType.Rivergreen_Lv2:
                glueplantImage.sprite = glueplant_PauseMenu_Rivergreen;
                break;
            case SkinType.Rivergreen_Lv3:
                glueplantImage.sprite = glueplant_PauseMenu_Rivergreen;
                break;
            case SkinType.Rivergreen_Lv4:
                glueplantImage.sprite = glueplant_PauseMenu_Rivergreen;
                break;
            case SkinType.Rivergreen_Lv5:
                glueplantImage.sprite = glueplant_PauseMenu_Rivergreen;
                break;

            case SkinType.Sandlands_Lv1:
                glueplantImage.sprite = glueplant_PauseMenu_Sandlands;
                break;
            case SkinType.Sandlands_Lv2:
                glueplantImage.sprite = glueplant_PauseMenu_Sandlands;
                break;
            case SkinType.Sandlands_Lv3:
                glueplantImage.sprite = glueplant_PauseMenu_Sandlands;
                break;
            case SkinType.Sandlands_Lv4:
                glueplantImage.sprite = glueplant_PauseMenu_Sandlands;
                break;
            case SkinType.Sandlands_Lv5:
                glueplantImage.sprite = glueplant_PauseMenu_Sandlands;
                break;

            case SkinType.Frostfield_Lv1:
                glueplantImage.sprite = glueplant_PauseMenu_Frostfield;
                break;
            case SkinType.Frostfield_Lv2:
                glueplantImage.sprite = glueplant_PauseMenu_Frostfield;
                break;
            case SkinType.Frostfield_Lv3:
                glueplantImage.sprite = glueplant_PauseMenu_Frostfield;
                break;
            case SkinType.Frostfield_Lv4:
                glueplantImage.sprite = glueplant_PauseMenu_Frostfield;
                break;
            case SkinType.Frostfield_Lv5:
                glueplantImage.sprite = glueplant_PauseMenu_Frostfield;
                break;

            case SkinType.Firevein_Lv1:
                glueplantImage.sprite = glueplant_PauseMenu_Firevein;
                break;
            case SkinType.Firevein_Lv2:
                glueplantImage.sprite = glueplant_PauseMenu_Firevein;
                break;
            case SkinType.Firevein_Lv3:
                glueplantImage.sprite = glueplant_PauseMenu_Firevein;
                break;
            case SkinType.Firevein_Lv4:
                glueplantImage.sprite = glueplant_PauseMenu_Firevein;
                break;
            case SkinType.Firevein_Lv5:
                glueplantImage.sprite = glueplant_PauseMenu_Firevein;
                break;

            case SkinType.Witchmire_Lv1:
                glueplantImage.sprite = glueplant_PauseMenu_Witchmire;
                break;
            case SkinType.Witchmire_Lv2:
                glueplantImage.sprite = glueplant_PauseMenu_Witchmire;
                break;
            case SkinType.Witchmire_Lv3:
                glueplantImage.sprite = glueplant_PauseMenu_Witchmire;
                break;
            case SkinType.Witchmire_Lv4:
                glueplantImage.sprite = glueplant_PauseMenu_Witchmire;
                break;
            case SkinType.Witchmire_Lv5:
                glueplantImage.sprite = glueplant_PauseMenu_Witchmire;
                break;

            case SkinType.Metalworks_Lv1:
                glueplantImage.sprite = glueplant_PauseMenu_Metalworks;
                break;
            case SkinType.Metalworks_Lv2:
                glueplantImage.sprite = glueplant_PauseMenu_Metalworks;
                break;
            case SkinType.Metalworks_Lv3:
                glueplantImage.sprite = glueplant_PauseMenu_Metalworks;
                break;
            case SkinType.Metalworks_Lv4:
                glueplantImage.sprite = glueplant_PauseMenu_Metalworks;
                break;
            case SkinType.Metalworks_Lv5:
                glueplantImage.sprite = glueplant_PauseMenu_Metalworks;
                break;

            default:
                break;
        }

        if (mapInfo.isCompleted)
            glueplant_Aquired.text = "1 / 1";
        else
            glueplant_Aquired.text = "0 / 1";

        //Essence aquired
        int essenceCounter = 0;
        for (int i = 0; i < mapInfo.essenceList.Count; i++)
        {
            if (mapInfo.essenceList[i].isTaken)
            {
                essenceCounter++;
            }
        }
        Essence_Aquired.text = essenceCounter + " / 10";

        //Skin aquired
        int skinCounter = 0;
        if (mapInfo.levelSkin.isTaken)
        {
            skinCounter++;
        }
        Skin_Aquired.text = skinCounter + " / 1";

        //StepMax aquired
        int stepsCounter = 0;
        for (int i = 0; i < mapInfo.maxStepList.Count; i++)
        {
            if (mapInfo.maxStepList[i].isTaken)
            {
                stepsCounter++;
            }
        }
        StepsMax_Aquired.text = stepsCounter + " / 3";

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

        if (mapInfo.abilitiesInLevel.SwimSuit)
            ability_Swimming.SetActive(true);
        if (mapInfo.abilitiesInLevel.SwiftSwim)
            ability_SwiftSwim.SetActive(true);
        if (mapInfo.abilitiesInLevel.Flippers)
            ability_Flippers.SetActive(true);

        if (mapInfo.abilitiesInLevel.Ascend)
            ability_Ascend.SetActive(true);
        if (mapInfo.abilitiesInLevel.Descend)
            ability_Descend.SetActive(true);

        if (mapInfo.abilitiesInLevel.Dash)
            ability_Dash.SetActive(true);
        if (mapInfo.abilitiesInLevel.Jumping)
            ability_Jumping.SetActive(true);
        if (mapInfo.abilitiesInLevel.GrapplingHook)
            ability_GrapplingHook.SetActive(true);
        if (mapInfo.abilitiesInLevel.CeilingGrab)
            ability_CeilingGrab.SetActive(true);
    }
    public Sprite SelectSpriteForLevel(SkinType skinType)
    {
        switch (skinType)
        {
            case SkinType.None:
                return null;

            case SkinType.Rivergreen_Lv1:
                return sprite_PauseMenu_Rivergreen_Lv1;
            case SkinType.Rivergreen_Lv2:
                return sprite_PauseMenu_Rivergreen_Lv2;
            case SkinType.Rivergreen_Lv3:
                return sprite_PauseMenu_Rivergreen_Lv3;
            case SkinType.Rivergreen_Lv4:
                return sprite_PauseMenu_Rivergreen_Lv4;
            case SkinType.Rivergreen_Lv5:
                return sprite_PauseMenu_Rivergreen_Lv5;

            case SkinType.Firevein_Lv1:
                return sprite_PauseMenu_Firevein_Lv1;
            case SkinType.Firevein_Lv2:
                return sprite_PauseMenu_Firevein_Lv2;
            case SkinType.Firevein_Lv3:
                return sprite_PauseMenu_Firevein_Lv3;
            case SkinType.Firevein_Lv4:
                return sprite_PauseMenu_Firevein_Lv4;
            case SkinType.Firevein_Lv5:
                return sprite_PauseMenu_Firevein_Lv5;

            case SkinType.Sandlands_Lv1:
                return sprite_PauseMenu_Sandlands_Lv1;
            case SkinType.Sandlands_Lv2:
                return sprite_PauseMenu_Sandlands_Lv2;
            case SkinType.Sandlands_Lv3:
                return sprite_PauseMenu_Sandlands_Lv3;
            case SkinType.Sandlands_Lv4:
                return sprite_PauseMenu_Sandlands_Lv4;
            case SkinType.Sandlands_Lv5:
                return sprite_PauseMenu_Sandlands_Lv5;

            case SkinType.Frostfield_Lv1:
                return sprite_PauseMenu_Frostfield_Lv1;
            case SkinType.Frostfield_Lv2:
                return sprite_PauseMenu_Frostfield_Lv2;
            case SkinType.Frostfield_Lv3:
                return sprite_PauseMenu_Frostfield_Lv3;
            case SkinType.Frostfield_Lv4:
                return sprite_PauseMenu_Frostfield_Lv4;
            case SkinType.Frostfield_Lv5:
                return sprite_PauseMenu_Frostfield_Lv5;

            case SkinType.Witchmire_Lv1:
                return sprite_PauseMenu_Witchmire_Lv1;
            case SkinType.Witchmire_Lv2:
                return sprite_PauseMenu_Witchmire_Lv2;
            case SkinType.Witchmire_Lv3:
                return sprite_PauseMenu_Witchmire_Lv3;
            case SkinType.Witchmire_Lv4:
                return sprite_PauseMenu_Witchmire_Lv4;
            case SkinType.Witchmire_Lv5:
                return sprite_PauseMenu_Witchmire_Lv5;

            case SkinType.Metalworks_Lv1:
                return sprite_PauseMenu_Metalworks_Lv1;
            case SkinType.Metalworks_Lv2:
                return sprite_PauseMenu_Metalworks_Lv2;
            case SkinType.Metalworks_Lv3:
                return sprite_PauseMenu_Metalworks_Lv3;
            case SkinType.Metalworks_Lv4:
                return sprite_PauseMenu_Metalworks_Lv4;
            case SkinType.Metalworks_Lv5:
                return sprite_PauseMenu_Metalworks_Lv5;

            default:
                return null;
        }
    }
}
