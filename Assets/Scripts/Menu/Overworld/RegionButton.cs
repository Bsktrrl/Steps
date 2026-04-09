using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionButton : MonoBehaviour
{
    public static event Action Action_ButtonIsPressed;

    public RegionState regionState;

    float changeStatesDuration = 0.3f;


    //--------------------


    public void Button_isPressed()
    {
        if (!GetComponent<LevelSelectButton>().CheckButtonStatus())
        {
            if (gameObject.GetComponent<ButtonSound>())
                gameObject.GetComponent<ButtonSound>().buttonPress_Sound = ButtonSoundStates.ButtonCannot;
            else
                gameObject.GetComponent<ButtonSound>().buttonPress_Sound = ButtonSoundStates.ButtonPress;

            Action_ButtonIsPressed?.Invoke();

            return;
        }
        else
            gameObject.GetComponent<ButtonSound>().buttonPress_Sound = ButtonSoundStates.ButtonPress;

        Action_ButtonIsPressed?.Invoke();
        StartCoroutine(Button_isPressed_Delay(changeStatesDuration));
    }
    IEnumerator Button_isPressed_Delay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        MenuStates.Instance.SaveMenuState(MenuState.Overworld_Menu);

        OverWorldManager.Instance.ChangeStates(regionState, LevelState.First);

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
                if (OverWorldManager.Instance.levelPanel_Frostfield) OverWorldManager.Instance.levelPanel_Frostfield.SetActive(true);
                ActionButtonsManager.Instance.eventSystem.SetSelectedGameObject(OverWorldManager.Instance.levelPanel_Frostfield.transform.GetChild(0).gameObject);
                break;
            case RegionState.Firevein:
                if (OverWorldManager.Instance.panelBackground) OverWorldManager.Instance.panelBackground.SetActive(true);
                if (OverWorldManager.Instance.levelPanel_Firevein) OverWorldManager.Instance.levelPanel_Firevein.SetActive(true);
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
