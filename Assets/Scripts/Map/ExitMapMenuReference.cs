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
        switch (levelRegion)
        {
            case regions.None:
                break;

            case regions.Rivergreen:
                DataManager.Instance.menuState_Store = MenuState.RegionMenu_Water;
                break;
            case regions.Sandlands:
                DataManager.Instance.menuState_Store = MenuState.RegionMenu_Sand;
                break;
            case regions.Frostfield:
                DataManager.Instance.menuState_Store = MenuState.RegionMenu_Ice;
                break;
            case regions.Firevein:
                DataManager.Instance.menuState_Store = MenuState.RegionMenu_Lava;
                break;
            case regions.Witchmire:
                DataManager.Instance.menuState_Store = MenuState.RegionMenu_Swamp;
                break;
            case regions.Metalworks:
                DataManager.Instance.menuState_Store = MenuState.RegionMenu_Metal;
                break;

            default:
                break;
        }

        DataPersistanceManager.instance.SaveGame();
    }
}
