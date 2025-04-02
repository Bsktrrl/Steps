using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverWorldManager : Singleton<OverWorldManager>
{
    [Header("Panel")]
    public GameObject levelPanel;

    [Header("images")]
    public Image region_Ice;
    public Image region_Stone;
    public Image region_Grass;
    public Image region_Desert;
    public Image region_Swamp;
    public Image region_Industrial;

    [Header("States")]
    public RegionState regionState;
    public LevelState levelState;

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


    //--------------------


    private void Start()
    {
        regionState = RegionState.None;
        levelState = LevelState.None;
    }

    private void OnEnable()
    {
        RememberCurrentlySelectedUIElement.Action_ChangedSelectedUIElement += CheckStates;
    }
    private void OnDisable()
    {
        RememberCurrentlySelectedUIElement.Action_ChangedSelectedUIElement -= CheckStates;
    }


    //--------------------


    public void ChangeStates(RegionState _regionState, LevelState _levelState)
    {
        regionState = _regionState;
        levelState = _levelState;

        CheckStates();
    }

    void CheckStates()
    {
        ResetSelectedRegions();

        switch (regionState)
        {
            case RegionState.Ice:
                switch (levelState)
                {
                    case LevelState.None:
                        region_Ice.sprite = ice_Selected;
                        break;
                    case LevelState.First:
                        region_Ice.sprite = ice_1;
                        break;
                    case LevelState.Second:
                        region_Ice.sprite = ice_2;
                        break;
                    case LevelState.Third:
                        region_Ice.sprite = ice_3;
                        break;
                    case LevelState.Forth:
                        region_Ice.sprite = ice_4;
                        break;
                    case LevelState.Fifth:
                        region_Ice.sprite = ice_5;
                        break;
                    case LevelState.Sixth:
                        region_Ice.sprite = ice_6;
                        break;
                    case LevelState.edge:
                        region_Ice.sprite = ice_Void;
                        break;
                    default:
                        break;
                }
                break;
            case RegionState.Stone:
                switch (levelState)
                {
                    case LevelState.None:
                        region_Stone.sprite = stone_Selected;
                        break;
                    case LevelState.First:
                        region_Stone.sprite = stone_1;
                        break;
                    case LevelState.Second:
                        region_Stone.sprite = stone_2;
                        break;
                    case LevelState.Third:
                        region_Stone.sprite = stone_3;
                        break;
                    case LevelState.Forth:
                        region_Stone.sprite = stone_4;
                        break;
                    case LevelState.Fifth:
                        region_Stone.sprite = stone_5;
                        break;
                    case LevelState.Sixth:
                        region_Stone.sprite = stone_6;
                        break;
                    case LevelState.edge:
                        region_Stone.sprite = stone_Void;
                        break;
                    default:
                        break;
                }
                break;
            case RegionState.Grass:
                switch (levelState)
                {
                    case LevelState.None:
                        region_Grass.sprite = grass_Selected;
                        break;
                    case LevelState.First:
                        region_Grass.sprite = grass_1;
                        break;
                    case LevelState.Second:
                        region_Grass.sprite = grass_2;
                        break;
                    case LevelState.Third:
                        region_Grass.sprite = grass_3;
                        break;
                    case LevelState.Forth:
                        region_Grass.sprite = grass_4;
                        break;
                    case LevelState.Fifth:
                        region_Grass.sprite = grass_5;
                        break;
                    case LevelState.Sixth:
                        region_Grass.sprite = grass_6;
                        break;
                    case LevelState.edge:
                        region_Grass.sprite = grass_Void;
                        break;
                    default:
                        break;
                }
                break;
            case RegionState.Desert:
                switch (levelState)
                {
                    case LevelState.None:
                        region_Desert.sprite = desert_Selected;
                        break;
                    case LevelState.First:
                        region_Desert.sprite = desert_1;
                        break;
                    case LevelState.Second:
                        region_Desert.sprite = desert_2;
                        break;
                    case LevelState.Third:
                        region_Desert.sprite = desert_3;
                        break;
                    case LevelState.Forth:
                        region_Desert.sprite = desert_4;
                        break;
                    case LevelState.Fifth:
                        region_Desert.sprite = desert_5;
                        break;
                    case LevelState.Sixth:
                        region_Desert.sprite = desert_6;
                        break;
                    case LevelState.edge:
                        region_Desert.sprite = desert_Void;
                        break;
                    default:
                        break;
                }
                break;
            case RegionState.Swamp:
                switch (levelState)
                {
                    case LevelState.None:
                        region_Swamp.sprite = swamp_Selected;
                        break;
                    case LevelState.First:
                        region_Swamp.sprite = swamp_1;
                        break;
                    case LevelState.Second:
                        region_Swamp.sprite = swamp_2;
                        break;
                    case LevelState.Third:
                        region_Swamp.sprite = swamp_3;
                        break;
                    case LevelState.Forth:
                        region_Swamp.sprite = swamp_4;
                        break;
                    case LevelState.Fifth:
                        region_Swamp.sprite = swamp_5;
                        break;
                    case LevelState.Sixth:
                        region_Swamp.sprite = swamp_6;
                        break;
                    case LevelState.edge:
                        region_Swamp.sprite = swamp_Void;
                        break;
                    default:
                        break;
                }
                break;
            case RegionState.Industrial:
                switch (levelState)
                {
                    case LevelState.None:
                        region_Industrial.sprite = industrial_Selected;
                        break;
                    case LevelState.First:
                        region_Industrial.sprite = industrial_1;
                        break;
                    case LevelState.Second:
                        region_Industrial.sprite = industrial_2;
                        break;
                    case LevelState.Third:
                        region_Industrial.sprite = industrial_3;
                        break;
                    case LevelState.Forth:
                        region_Industrial.sprite = industrial_4;
                        break;
                    case LevelState.Fifth:
                        region_Industrial.sprite = industrial_5;
                        break;
                    case LevelState.Sixth:
                        region_Industrial.sprite = industrial_6;
                        break;
                    case LevelState.edge:
                        region_Industrial.sprite = industrial_Void;
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
        region_Ice.sprite = ice_Normal;
        region_Stone.sprite = stone_Normal;
        region_Grass.sprite = grass_Normal;
        region_Desert.sprite = desert_Normal;
        region_Swamp.sprite = swamp_Normal;
        region_Industrial.sprite = industrial_Normal;
    }
}

public enum RegionState
{
    None,

    Ice,
    Stone,
    Grass,
    Desert,
    Swamp,
    Industrial,
}
public enum LevelState
{
    None,

    First,
    Second,
    Third,
    Forth,
    Fifth,
    Sixth,

    edge,
}