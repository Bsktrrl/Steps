using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionButton : MonoBehaviour
{
    public RegionState regionState;


    //--------------------


    public void Button_isPressed()
    {
        if (!GetComponent<LevelButtonEffects>().canBePlayed) return;

        OverWorldManager.Instance.ChangeStates(regionState, LevelState.First);

        MenuStates.Instance.SaveMenuState(MenuState.Overworld_Menu);

        switch (regionState)
        {
            case RegionState.None:
                break;

            case RegionState.Ice:
                if (OverWorldManager.Instance.panelBackground) OverWorldManager.Instance.panelBackground.SetActive(true);
                if(OverWorldManager.Instance.levelPanel_Ice) OverWorldManager.Instance.levelPanel_Ice.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Ice.transform.GetChild(0).gameObject);
                break;
            case RegionState.Stone:
                if (OverWorldManager.Instance.panelBackground) OverWorldManager.Instance.panelBackground.SetActive(true);
                if(OverWorldManager.Instance.levelPanel_Stone) OverWorldManager.Instance.levelPanel_Stone.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Stone.transform.GetChild(0).gameObject);
                break;
            case RegionState.Grass:
                if(OverWorldManager.Instance.panelBackground) OverWorldManager.Instance.panelBackground.SetActive(true);
                if (OverWorldManager.Instance.levelPanel_Grass) OverWorldManager.Instance.levelPanel_Grass.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Grass.transform.GetChild(0).gameObject);
                break;
            case RegionState.Desert:
                if (OverWorldManager.Instance.panelBackground) OverWorldManager.Instance.panelBackground.SetActive(true);
                if (OverWorldManager.Instance.levelPanel_Desert) OverWorldManager.Instance.levelPanel_Desert.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Desert.transform.GetChild(0).gameObject);
                break;
            case RegionState.Swamp:
                if (OverWorldManager.Instance.panelBackground) OverWorldManager.Instance.panelBackground.SetActive(true);
                if (OverWorldManager.Instance.levelPanel_Swamp) OverWorldManager.Instance.levelPanel_Swamp.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Swamp.transform.GetChild(0).gameObject);
                break;
            case RegionState.Industrial:
                if (OverWorldManager.Instance.panelBackground) OverWorldManager.Instance.panelBackground.SetActive(true);
                if (OverWorldManager.Instance.levelPanel_Industrial) OverWorldManager.Instance.levelPanel_Industrial.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Industrial.transform.GetChild(0).gameObject);
                break;

            default:
                break;
        }
    }
}
