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
                OverWorldManager.Instance.levelPanel_Ice.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Ice.transform.GetChild(0).gameObject);
                break;
            case RegionState.Stone:
                OverWorldManager.Instance.levelPanel_Stone.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Stone.transform.GetChild(0).gameObject);
                break;
            case RegionState.Grass:
                OverWorldManager.Instance.levelPanel_Grass.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Grass.transform.GetChild(0).gameObject);
                break;
            case RegionState.Desert:
                OverWorldManager.Instance.levelPanel_Desert.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Desert.transform.GetChild(0).gameObject);
                break;
            case RegionState.Swamp:
                OverWorldManager.Instance.levelPanel_Swamp.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Swamp.transform.GetChild(0).gameObject);
                break;
            case RegionState.Industrial:
                OverWorldManager.Instance.levelPanel_Industrial.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Industrial.transform.GetChild(0).gameObject);
                break;

            default:
                break;
        }
    }
}
