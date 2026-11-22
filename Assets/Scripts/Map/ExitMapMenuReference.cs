using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMapMenuReference : MonoBehaviour
{
    [SerializeField] regions levelRegion;


    //--------------------


    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += SetMenuState;
    }
    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= SetMenuState;
    }


    //--------------------


    void SetMenuState()
    {
        MenuState returnMenu = GetMenuStateForRegion(levelRegion);
        if (returnMenu != MenuState.None)
            DataManager.Instance.menuState_Store = returnMenu;

        DataPersistanceManager.instance.SaveGame();
    }

    internal static MenuState GetMenuStateForRegion(regions region)
    {
        switch (region)
        {
            case regions.HUB: return MenuState.RegionMenu_HUB;
            case regions.Rivergreen: return MenuState.RegionMenu_Water;
            case regions.Sandlands: return MenuState.RegionMenu_Sand;
            case regions.Frostfield: return MenuState.RegionMenu_Ice;
            case regions.Firevein: return MenuState.RegionMenu_Lava;
            case regions.Witchmire: return MenuState.RegionMenu_Swamp;
            case regions.Metalworks: return MenuState.RegionMenu_Metal;
            default: return MenuState.None;
        }
    }
}
