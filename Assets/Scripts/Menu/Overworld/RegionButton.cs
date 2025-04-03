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
        OverWorldManager.Instance.ChangeStates(regionState, LevelState.First);

        switch (regionState)
        {
            case RegionState.None:
                break;

            case RegionState.Ice:
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Ice.transform.GetChild(0).gameObject);
                OverWorldManager.Instance.levelPanel_Ice.SetActive(true);
                break;
            case RegionState.Stone:
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Stone.transform.GetChild(0).gameObject);
                OverWorldManager.Instance.levelPanel_Stone.SetActive(true);
                break;
            case RegionState.Grass:
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Grass.transform.GetChild(0).gameObject);
                OverWorldManager.Instance.levelPanel_Grass.SetActive(true);
                break;
            case RegionState.Desert:
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Desert.transform.GetChild(0).gameObject);
                OverWorldManager.Instance.levelPanel_Desert.SetActive(true);
                break;
            case RegionState.Swamp:
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Swamp.transform.GetChild(0).gameObject);
                OverWorldManager.Instance.levelPanel_Swamp.SetActive(true);
                break;
            case RegionState.Industrial:
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Industrial.transform.GetChild(0).gameObject);
                OverWorldManager.Instance.levelPanel_Industrial.SetActive(true);
                break;

            default:
                break;
        }
    }
}
