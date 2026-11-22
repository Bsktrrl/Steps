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
    public GameObject levelPanel_HUB;
    public GameObject levelPanel_Rivergreen;
    public GameObject levelPanel_Sandlands;
    public GameObject levelPanel_Frostfield;
    public GameObject levelPanel_Firevein;
    public GameObject levelPanel_Witchmire;
    public GameObject levelPanel_Metalworks;

    [Header("Panel")]
    public GameObject levelPanel_HUB_FirstLevel;
    public GameObject levelPanel_Rivergreen_FirstLevel;
    public GameObject levelPanel_Sandlands_FirstLevel;
    public GameObject levelPanel_Frostfield_FirstLevel;
    public GameObject levelPanel_Firevein_FirstLevel;
    public GameObject levelPanel_Witchmire_FirstLevel;
    public GameObject levelPanel_Metalworks_FirstLevel;

    [Header("Region Select")]
    [SerializeField] GameObject regionSelect_FirstButton;

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
    }
    private void OnDisable()
    {
        RememberCurrentlySelectedUIElement.Action_ChangedSelectedUIElement -= CheckStates;
    }


    //--------------------


    public void LoadUIElementState_IfExitsFromALevel()
    {
        // DataManager also calls this before Action_dataHasLoaded. At that point
        // MainMenuManager has not opened the saved menu yet.
        if (!DataManager.Instance.dataHasLoaded_Check)
            return;

        RestoreMenuSelectionAfterLoad(false);
    }

    public void RestoreMenuSelectionAfterLoad(bool returningFromHUB, string exitedLevelScene = null, bool selectNextLevel = false)
    {
        MainMenuManager menu = MainMenuManager.Instance;
        if (menu == null || !menu.overworldMenu_Parent.activeInHierarchy)
            return;

        OverWorldStates saved = DataManager.Instance.overWorldStates_StoreList;
        RememberCurrentlySelectedUIElement.Instance.overWorldStates = saved;

        // Never activate a level panel while the region selection menu is open.
        if (menu.regionSelectMenu.activeInHierarchy)
        {
            if (panelBackground) panelBackground.SetActive(false);
            levelState = LevelState.None;
            regionState = returningFromHUB || saved == null ? RegionState.None : saved.regionState;
            CheckStates();

            SetRestoredSelection(GetRegionSelection(menu.regionSelectMenu, returningFromHUB));
            return;
        }

        if (!menu.levelSelectMenu.activeInHierarchy)
            return;

        GameObject panel = GetLevelPanel(menu.menuState, out RegionState panelRegion);
        if (!panel) return;

        regionState = panelRegion;
        levelState = saved != null && saved.regionState == regionState ? saved.levelState : LevelState.None;
        CheckStates();

        if (panelBackground) panelBackground.SetActive(true);
        panel.SetActive(true);
        SetSelected(panel, exitedLevelScene, selectNextLevel);
    }

    GameObject GetLevelPanel(MenuState menuState, out RegionState region)
    {
        switch (menuState)
        {
            case MenuState.RegionMenu_HUB: region = RegionState.HUB; return levelPanel_HUB;
            case MenuState.RegionMenu_Water: region = RegionState.Rivergreen; return levelPanel_Rivergreen;
            case MenuState.RegionMenu_Sand: region = RegionState.Sandlands; return levelPanel_Sandlands;
            case MenuState.RegionMenu_Ice: region = RegionState.Frostfields; return levelPanel_Frostfield;
            case MenuState.RegionMenu_Lava: region = RegionState.Firevein; return levelPanel_Firevein;
            case MenuState.RegionMenu_Swamp: region = RegionState.Witchmire; return levelPanel_Witchmire;
            case MenuState.RegionMenu_Metal: region = RegionState.Metalworks; return levelPanel_Metalworks;
            default: region = RegionState.None; return null;
        }
    }

    void SetSelected(GameObject panel, string exitedLevelScene, bool selectNextLevel)
    {
        // The menu also contains Back and layout objects. The LevelButton stores the level order.
        LevelButton[] levels = panel.GetComponentsInChildren<LevelButton>(true);
        LevelButton selected = string.IsNullOrEmpty(exitedLevelScene) ? null :
            System.Array.Find(levels, level => level.TryGetComponent(out LoadLevel loader) &&
                                                loader.levelToPlay == exitedLevelScene);

        if (selected && selectNextLevel && selected.State >= LevelState.First)
        {
            LevelButton next = FindSelectableLevel(levels, panel, selected.State + 1);
            if (next)
                selected = next;
        }

        if (!selected || !IsSelectableInMenu(selected.gameObject, panel))
            selected = FindSelectableLevel(levels, panel, levelState);

        if (!selected)
            selected = FindSelectableLevel(levels, panel, LevelState.First);

        if (!selected)
            selected = System.Array.Find(levels, level => IsSelectableInMenu(level.gameObject, panel));

        SetRestoredSelection(selected ? selected.gameObject : null);
    }

    LevelButton FindSelectableLevel(LevelButton[] levels, GameObject panel, LevelState state)
    {
        if (state == LevelState.None)
            return null;

        return System.Array.Find(levels, level => level.State == state && IsSelectableInMenu(level.gameObject, panel));
    }

    bool IsSelectableInMenu(GameObject obj, GameObject menu)
    {
        if (!obj || !menu || !obj.activeInHierarchy || !obj.transform.IsChildOf(menu.transform))
            return false;

        Selectable selectable = obj.GetComponent<Selectable>();
        return selectable && selectable.isActiveAndEnabled && selectable.IsInteractable();
    }

    GameObject GetRegionSelection(GameObject menu, bool returningFromHUB)
    {
        if (!returningFromHUB)
        {
            GameObject selected = ActionButtonsManager.Instance.eventSystem.currentSelectedGameObject;
            if (IsSelectableRegionButton(selected, menu))
                return selected;
        }

        if (IsSelectableRegionButton(regionSelect_FirstButton, menu))
            return regionSelect_FirstButton;

        foreach (Button_ToPress button in menu.GetComponentsInChildren<Button_ToPress>())
        {
            if (IsSelectableRegionButton(button.gameObject, menu))
                return button.gameObject;
        }

        return null;
    }

    bool IsSelectableRegionButton(GameObject obj, GameObject menu)
    {
        return IsSelectableInMenu(obj, menu) &&
               obj.TryGetComponent(out Button_ToPress button) &&
               GetLevelPanel(button.newMenuState, out _) != null;
    }

    void SetRestoredSelection(GameObject selected)
    {
        var eventSystem = ActionButtonsManager.Instance.eventSystem;
        if (eventSystem.currentSelectedGameObject != selected)
            eventSystem.SetSelectedGameObject(selected);

        var remembered = RememberCurrentlySelectedUIElement.Instance;
        remembered.currentSelectedUIElement = selected;

        if (!selected) return;

        if (selected.TryGetComponent(out LevelButton levelButton))
        {
            if (levelState != levelButton.State)
                ChangeStates(regionState, levelButton.State);
        }
        else if (selected.TryGetComponent(out RegionButton regionButton))
        {
            ChangeStates(regionButton.regionState, LevelState.None);
        }

        remembered.Action_ChangedSelectedUIElement_Invoke();
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
            case RegionState.HUB:
                switch (levelState)
                {
                    case LevelState.None:
                        region_Rivergreen.sprite = grass_Selected;
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
    HUB,
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
