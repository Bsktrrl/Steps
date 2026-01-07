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
        if (!GetComponent<LevelSelectButton>().CheckButtonStatus()) return;

        OverWorldManager.Instance.ChangeStates(regionState, LevelState.First);

        MenuStates.Instance.SaveMenuState(MenuState.Overworld_Menu);

        switch (regionState)
        {
            case RegionState.None:
                break;

            case RegionState.Rivergreen:
                if (OverWorldManager.Instance.panelBackground) OverWorldManager.Instance.panelBackground.SetActive(true);
                if (OverWorldManager.Instance.levelPanel_Rivergreen) OverWorldManager.Instance.levelPanel_Rivergreen.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Rivergreen.transform.GetChild(0).gameObject);
                break;
            case RegionState.Sandlands:
                if (OverWorldManager.Instance.panelBackground) OverWorldManager.Instance.panelBackground.SetActive(true);
                if (OverWorldManager.Instance.levelPanel_Sandlands) OverWorldManager.Instance.levelPanel_Sandlands.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Sandlands.transform.GetChild(0).gameObject);
                break;
            case RegionState.Frostfields:
                if (OverWorldManager.Instance.panelBackground) OverWorldManager.Instance.panelBackground.SetActive(true);
                if(OverWorldManager.Instance.levelPanel_Frostfield) OverWorldManager.Instance.levelPanel_Frostfield.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Frostfield.transform.GetChild(0).gameObject);
                break;
            case RegionState.Firevein:
                if (OverWorldManager.Instance.panelBackground) OverWorldManager.Instance.panelBackground.SetActive(true);
                if(OverWorldManager.Instance.levelPanel_Firevein) OverWorldManager.Instance.levelPanel_Firevein.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Firevein.transform.GetChild(0).gameObject);
                break;
            case RegionState.Witchmire:
                if (OverWorldManager.Instance.panelBackground) OverWorldManager.Instance.panelBackground.SetActive(true);
                if (OverWorldManager.Instance.levelPanel_Witchmire) OverWorldManager.Instance.levelPanel_Witchmire.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Witchmire.transform.GetChild(0).gameObject);
                break;
            case RegionState.Metalworks:
                if (OverWorldManager.Instance.panelBackground) OverWorldManager.Instance.panelBackground.SetActive(true);
                if (OverWorldManager.Instance.levelPanel_Metalworks) OverWorldManager.Instance.levelPanel_Metalworks.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Metalworks.transform.GetChild(0).gameObject);
                break;

            default:
                break;
        }
    }
}
