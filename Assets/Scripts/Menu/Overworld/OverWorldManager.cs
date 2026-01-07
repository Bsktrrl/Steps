using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverWorldManager : Singleton<OverWorldManager>
{
    [Header("States")]
    public RegionState regionState;
    public LevelState levelState;

    [Header("Panel")]
    public GameObject panelBackground;
    public GameObject levelPanel_Rivergreen;
    public GameObject levelPanel_Sandlands;
    public GameObject levelPanel_Frostfield;
    public GameObject levelPanel_Firevein;
    public GameObject levelPanel_Witchmire;
    public GameObject levelPanel_Metalworks;

    [Header("Panel")]
    public GameObject levelPanel_Rivergreen_FirstLevel;
    public GameObject levelPanel_Sandlands_FirstLevel;
    public GameObject levelPanel_Frostfield_FirstLevel;
    public GameObject levelPanel_Firevein_FirstLevel;
    public GameObject levelPanel_Witchmire_FirstLevel;
    public GameObject levelPanel_Metalworks_FirstLevel;

    [Header("images")]
    public Image region_Rivergreen;
    public Image region_Sandlands;
    public Image region_Frostfield;
    public Image region_Firevein;
    public Image region_Witchmire;
    public Image region_Metalworks;

    #region RegionLevel Sprites
    [Header("RegionLevel Sprites")]
    [SerializeField] Sprite ice_Normal;
    [SerializeField] Sprite ice_Selected;
    [SerializeField] Sprite ice_1;
    [SerializeField] Sprite ice_2;
    [SerializeField] Sprite ice_3;
    [SerializeField] Sprite ice_4;
    [SerializeField] Sprite ice_5;
    [SerializeField] Sprite ice_6;
    [SerializeField] Sprite ice_Void;

    [SerializeField] Sprite stone_Normal;
    [SerializeField] Sprite stone_Selected;
    [SerializeField] Sprite stone_1;
    [SerializeField] Sprite stone_2;
    [SerializeField] Sprite stone_3;
    [SerializeField] Sprite stone_4;
    [SerializeField] Sprite stone_5;
    [SerializeField] Sprite stone_6;
    [SerializeField] Sprite stone_Void;

    [SerializeField] Sprite grass_Normal;
    [SerializeField] Sprite grass_Selected;
    [SerializeField] Sprite grass_1;
    [SerializeField] Sprite grass_2;
    [SerializeField] Sprite grass_3;
    [SerializeField] Sprite grass_4;
    [SerializeField] Sprite grass_5;
    [SerializeField] Sprite grass_6;
    [SerializeField] Sprite grass_Void;

    [SerializeField] Sprite desert_Normal;
    [SerializeField] Sprite desert_Selected;
    [SerializeField] Sprite desert_1;
    [SerializeField] Sprite desert_2;
    [SerializeField] Sprite desert_3;
    [SerializeField] Sprite desert_4;
    [SerializeField] Sprite desert_5;
    [SerializeField] Sprite desert_6;
    [SerializeField] Sprite desert_Void;

    [SerializeField] Sprite swamp_Normal;
    [SerializeField] Sprite swamp_Selected;
    [SerializeField] Sprite swamp_1;
    [SerializeField] Sprite swamp_2;
    [SerializeField] Sprite swamp_3;
    [SerializeField] Sprite swamp_4;
    [SerializeField] Sprite swamp_5;
    [SerializeField] Sprite swamp_6;
    [SerializeField] Sprite swamp_Void;

    [SerializeField] Sprite industrial_Normal;
    [SerializeField] Sprite industrial_Selected;
    [SerializeField] Sprite industrial_1;
    [SerializeField] Sprite industrial_2;
    [SerializeField] Sprite industrial_3;
    [SerializeField] Sprite industrial_4;
    [SerializeField] Sprite industrial_5;
    [SerializeField] Sprite industrial_6;
    [SerializeField] Sprite industrial_Void;
    #endregion


    //--------------------


    private void Start()
    {
        regionState = RegionState.None;
        levelState = LevelState.None;
    }

    private void OnEnable()
    {
        RememberCurrentlySelectedUIElement.Action_ChangedSelectedUIElement += CheckStates;
        DataManager.Action_dataHasLoaded += LoadUIElementState_IfExitsFromALevel;
    }
    private void OnDisable()
    {
        RememberCurrentlySelectedUIElement.Action_ChangedSelectedUIElement -= CheckStates;
        DataManager.Action_dataHasLoaded -= LoadUIElementState_IfExitsFromALevel;
    }


    //--------------------


    public void LoadUIElementState_IfExitsFromALevel()
    {
        if (DataManager.Instance.overWorldStates_StoreList.regionState != RegionState.None && DataManager.Instance.overWorldStates_StoreList.levelState != LevelState.None
            && MainMenuManager.Instance.overworldMenu_Parent.activeInHierarchy)
        {
            RememberCurrentlySelectedUIElement.Instance.overWorldStates = DataManager.Instance.overWorldStates_StoreList;

            //print("200. UIElementState - Load From Save - R: " + RememberCurrentlySelectedUIElement.Instance.overWorldStates.regionState + " | L: " + RememberCurrentlySelectedUIElement.Instance.overWorldStates.levelState);

            ChangeStates(RememberCurrentlySelectedUIElement.Instance.overWorldStates.regionState, RememberCurrentlySelectedUIElement.Instance.overWorldStates.levelState);

            switch (regionState)
            {
                case RegionState.None:
                    break;


                case RegionState.Rivergreen:
                    if (panelBackground) panelBackground.SetActive(true);
                    levelPanel_Rivergreen.SetActive(true);
                    SetSelected(levelPanel_Rivergreen);
                    break;

                case RegionState.Sandlands:
                    if (panelBackground) panelBackground.SetActive(true);
                    levelPanel_Sandlands.SetActive(true);
                    SetSelected(levelPanel_Sandlands);
                    break;

                case RegionState.Frostfields:
                    if(panelBackground) panelBackground.SetActive(true);
                    levelPanel_Frostfield.SetActive(true);
                    SetSelected(levelPanel_Frostfield);
                    break;
                
                case RegionState.Firevein:
                    if (panelBackground) panelBackground.SetActive(true);
                    levelPanel_Firevein.SetActive(true);
                    SetSelected(levelPanel_Firevein);
                    break;
                
                case RegionState.Witchmire:
                    if (panelBackground) panelBackground.SetActive(true);
                    levelPanel_Witchmire.SetActive(true);
                    SetSelected(levelPanel_Witchmire);
                    break;
                
                case RegionState.Metalworks:
                    if (panelBackground) panelBackground.SetActive(true);
                    levelPanel_Metalworks.SetActive(true);
                    SetSelected(levelPanel_Metalworks);
                    break;


                default:
                    break;
            }
        }
    }

    void SetSelected(GameObject obj)
    {
        switch (levelState)
        {
            case LevelState.None:
                break;

            case LevelState.First:
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(obj.transform.GetChild(0).gameObject);
                break;
            case LevelState.Second:
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(obj.transform.GetChild(1).gameObject);
                break;
            case LevelState.Third:
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(obj.transform.GetChild(2).gameObject);
                break;
            case LevelState.Forth:
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(obj.transform.GetChild(3).gameObject);
                break;
            case LevelState.Fifth:
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(obj.transform.GetChild(4).gameObject);
                break;
            case LevelState.edge:
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(obj.transform.GetChild(5).gameObject);
                break;

            default:
                break;
        }


    }


    //--------------------


    public void ChangeStates(RegionState _regionState, LevelState _levelState)
    {
        regionState = _regionState;
        levelState = _levelState;

        CheckStates();

        RememberCurrentlySelectedUIElement.Instance.SaveSelectedUIElement(regionState, levelState);
    }

    void CheckStates()
    {
        ResetSelectedRegions();

        switch (regionState)
        {
            case RegionState.Rivergreen:
                switch (levelState)
                {
                    case LevelState.None:
                        region_Rivergreen.sprite = grass_Selected;
                        break;
                    case LevelState.First:
                        region_Rivergreen.sprite = grass_1;
                        break;
                    case LevelState.Second:
                        region_Rivergreen.sprite = grass_2;
                        break;
                    case LevelState.Third:
                        region_Rivergreen.sprite = grass_3;
                        break;
                    case LevelState.Forth:
                        region_Rivergreen.sprite = grass_4;
                        break;
                    case LevelState.Fifth:
                        region_Rivergreen.sprite = grass_5;
                        break;
                    case LevelState.edge:
                        region_Rivergreen.sprite = grass_Void;
                        break;
                    default:
                        break;
                }
                break;


            case RegionState.Sandlands:
                switch (levelState)
                {
                    case LevelState.None:
                        region_Sandlands.sprite = desert_Selected;
                        break;
                    case LevelState.First:
                        region_Sandlands.sprite = desert_1;
                        break;
                    case LevelState.Second:
                        region_Sandlands.sprite = desert_2;
                        break;
                    case LevelState.Third:
                        region_Sandlands.sprite = desert_3;
                        break;
                    case LevelState.Forth:
                        region_Sandlands.sprite = desert_4;
                        break;
                    case LevelState.Fifth:
                        region_Sandlands.sprite = desert_5;
                        break;
                    case LevelState.edge:
                        region_Sandlands.sprite = desert_Void;
                        break;
                    default:
                        break;
                }
                break;

            case RegionState.Frostfields:
                switch (levelState)
                {
                    case LevelState.None:
                        region_Frostfield.sprite = ice_Selected;
                        break;
                    case LevelState.First:
                        region_Frostfield.sprite = ice_1;
                        break;
                    case LevelState.Second:
                        region_Frostfield.sprite = ice_2;
                        break;
                    case LevelState.Third:
                        region_Frostfield.sprite = ice_3;
                        break;
                    case LevelState.Forth:
                        region_Frostfield.sprite = ice_4;
                        break;
                    case LevelState.Fifth:
                        region_Frostfield.sprite = ice_5;
                        break;
                    case LevelState.edge:
                        region_Frostfield.sprite = ice_Void;
                        break;
                    default:
                        break;
                }
                break;

            case RegionState.Firevein:
                switch (levelState)
                {
                    case LevelState.None:
                        region_Firevein.sprite = stone_Selected;
                        break;
                    case LevelState.First:
                        region_Firevein.sprite = stone_1;
                        break;
                    case LevelState.Second:
                        region_Firevein.sprite = stone_2;
                        break;
                    case LevelState.Third:
                        region_Firevein.sprite = stone_3;
                        break;
                    case LevelState.Forth:
                        region_Firevein.sprite = stone_4;
                        break;
                    case LevelState.Fifth:
                        region_Firevein.sprite = stone_5;
                        break;
                    case LevelState.edge:
                        region_Firevein.sprite = stone_Void;
                        break;
                    default:
                        break;
                }
                break;

            case RegionState.Witchmire:
                switch (levelState)
                {
                    case LevelState.None:
                        region_Witchmire.sprite = swamp_Selected;
                        break;
                    case LevelState.First:
                        region_Witchmire.sprite = swamp_1;
                        break;
                    case LevelState.Second:
                        region_Witchmire.sprite = swamp_2;
                        break;
                    case LevelState.Third:
                        region_Witchmire.sprite = swamp_3;
                        break;
                    case LevelState.Forth:
                        region_Witchmire.sprite = swamp_4;
                        break;
                    case LevelState.Fifth:
                        region_Witchmire.sprite = swamp_5;
                        break;
                    case LevelState.edge:
                        region_Witchmire.sprite = swamp_Void;
                        break;
                    default:
                        break;
                }
                break;

            case RegionState.Metalworks:
                switch (levelState)
                {
                    case LevelState.None:
                        region_Metalworks.sprite = industrial_Selected;
                        break;
                    case LevelState.First:
                        region_Metalworks.sprite = industrial_1;
                        break;
                    case LevelState.Second:
                        region_Metalworks.sprite = industrial_2;
                        break;
                    case LevelState.Third:
                        region_Metalworks.sprite = industrial_3;
                        break;
                    case LevelState.Forth:
                        region_Metalworks.sprite = industrial_4;
                        break;
                    case LevelState.Fifth:
                        region_Metalworks.sprite = industrial_5;
                        break;
                    case LevelState.edge:
                        region_Metalworks.sprite = industrial_Void;
                        break;
                    default:
                        break;
                }
                break;


            default:
                break;
        }
    }

    void ResetSelectedRegions()
    {
        region_Frostfield.sprite = ice_Normal;
        region_Firevein.sprite = stone_Normal;
        region_Rivergreen.sprite = grass_Normal;
        region_Sandlands.sprite = desert_Normal;
        region_Witchmire.sprite = swamp_Normal;
        region_Metalworks.sprite = industrial_Normal;
    }
}

public enum RegionState
{
    None,

    Rivergreen,
    Sandlands,
    Frostfields,
    Firevein,
    Witchmire,
    Metalworks,
}
public enum LevelState
{
    None,

    First,
    Second,
    Third,
    Forth,
    Fifth,

    edge,
}