using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionButton : MonoBehaviour
{
    [SerializeField] RegionState regionState;
    [SerializeField] Color selectColor;

    public void Button_isPressed()
    {
        OverWorldManager.Instance.levelPanel.SetActive(true);
        OverWorldManager.Instance.ChangeStates(regionState, LevelState.First);
        LevelPanel.Instance.ChangePanelBackgroundColor(selectColor);
    }
}
