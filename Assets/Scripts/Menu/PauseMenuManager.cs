using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuManager : Singleton<PauseMenuManager>
{
    [Header("Parents")]
    public GameObject pauseMenu_Parent;

    public GameObject pauseMenu_MainMenu_Parent;
    public GameObject pauseMenu_Skins_Parent;
    public GameObject pauseMenu_Options_Parent;

    public GameObject pauseMenu_StartButton;

    [Header("LevelDisplay")]
    [SerializeField] GameObject levelDisplay_Parent;
    [SerializeField] TextMeshProUGUI levelName;
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


    [Header("Sprites")]
    #region Sprites
    public Sprite Water_Grass;
    public Sprite Water_Water;
    public Sprite Water_Wood;
    public Sprite Water_4;
    public Sprite Water_5;
    public Sprite Water_6;

    public Sprite Cave_Stone;
    public Sprite Cave_Stone_Brick;
    public Sprite Cave_Lava;
    public Sprite Cave_Rock;
    public Sprite Cave_Brick_Brown;
    public Sprite Cave_Brick_Black;

    public Sprite Desert_Sand;
    public Sprite Desert_Clay;
    public Sprite Desert_Clay_Tiles;
    public Sprite Desert_Sandstone;
    public Sprite Desert_Sandstone_Swirl;
    public Sprite Desert_Quicksand;

    public Sprite Winter_Snow;
    public Sprite Winter_Ice;
    public Sprite Winter_ColdWood;
    public Sprite Winter_FrozenGrass;
    public Sprite Winter_CrackedIce;
    public Sprite Winter_Crocked;

    public Sprite Swamp_SwampWater;
    public Sprite Swamp_Mud;
    public Sprite Swamp_SwampGrass;
    public Sprite Swamp_JungleWood;
    public Sprite Swamp_SwampWood;
    public Sprite Swamp_TempleBlock;

    public Sprite Industrial_Metal;
    public Sprite Industrial_Brass;
    public Sprite Industrial_Gold;
    public Sprite Industrial_Casing_Metal;
    public Sprite Industria_Casingl_Brass;
    public Sprite Industrial_Casing_Gold;
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

        //Set the first selected button for controller input
        EventSystem.current.SetSelectedGameObject(pauseMenu_StartButton);

        pauseMenu_Parent.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        pauseMenu_Parent.SetActive(false);
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
        if (mapInfo.isCompleted)
            glueplant_Aquired.text = "1 / 1";
        else
            glueplant_Aquired.text = "0 / 1";

        //Essence aquired
        int essenceCounter = 0;
        for (int i = 0; i < mapInfo.coinList.Count; i++)
        {
            if (mapInfo.coinList[i].isTaken)
            {
                essenceCounter++;
            }
        }
        Essence_Aquired.text = essenceCounter + " / 10";

        //Skin aquired
        int skinCounter = 0;
        for (int i = 0; i < mapInfo.skinsList.Count; i++)
        {
            if (mapInfo.skinsList[i].isTaken)
            {
                skinCounter++;
            }
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

            case SkinType.Water_Grass:
                return Water_Grass;
            case SkinType.Water_Water:
                return Water_Water;
            case SkinType.Water_Wood:
                return Water_Wood;
            case SkinType.Water_4:
                return Water_4;
            case SkinType.Water_5:
                return Water_5;
            case SkinType.Water_6:
                return Water_6;

            case SkinType.Cave_Stone:
                return Cave_Stone;
            case SkinType.Cave_Stone_Brick:
                return Cave_Stone_Brick;
            case SkinType.Cave_Lava:
                return Cave_Lava;
            case SkinType.Cave_Rock:
                return Cave_Rock;
            case SkinType.Cave_Brick_Brown:
                return Cave_Brick_Brown;
            case SkinType.Cave_Brick_Black:
                return Cave_Brick_Black;

            case SkinType.Desert_Sand:
                return Desert_Sand;
            case SkinType.Desert_Clay:
                return Desert_Clay;
            case SkinType.Desert_Clay_Tiles:
                return Desert_Clay_Tiles;
            case SkinType.Desert_Sandstone:
                return Desert_Sandstone;
            case SkinType.Desert_Sandstone_Swirl:
                return Desert_Sandstone_Swirl;
            case SkinType.Desert_Quicksand:
                return Desert_Quicksand;

            case SkinType.Winter_Snow:
                return Winter_Snow;
            case SkinType.Winter_Ice:
                return Winter_Ice;
            case SkinType.Winter_ColdWood:
                return Winter_ColdWood;
            case SkinType.Winter_FrozenGrass:
                return Winter_FrozenGrass;
            case SkinType.Winter_CrackedIce:
                return Winter_CrackedIce;
            case SkinType.Winter_Crocked:
                return Winter_Crocked;

            case SkinType.Swamp_SwampWater:
                return Swamp_SwampWater;
            case SkinType.Swamp_Mud:
                return Swamp_Mud;
            case SkinType.Swamp_SwampGrass:
                return Swamp_SwampGrass;
            case SkinType.Swamp_JungleWood:
                return Swamp_JungleWood;
            case SkinType.Swamp_SwampWood:
                return Swamp_SwampWood;
            case SkinType.Swamp_TempleBlock:
                return Swamp_TempleBlock;

            case SkinType.Industrial_Metal:
                return Industrial_Metal;
            case SkinType.Industrial_Brass:
                return Industrial_Brass;
            case SkinType.Industrial_Gold:
                return Industrial_Gold;
            case SkinType.Industrial_Casing_Metal:
                return Industrial_Casing_Metal;
            case SkinType.Industria_Casingl_Brass:
                return Industria_Casingl_Brass;
            case SkinType.Industrial_Casing_Gold:
                return Industrial_Casing_Gold;

            default:
                return null;
        }
    }
}
