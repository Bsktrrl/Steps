using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanel : Singleton<LevelPanel>
{
    [SerializeField] Button panel_Level_1;
    [SerializeField] Button panel_Level_2;
    [SerializeField] Button panel_Level_3;
    [SerializeField] Button panel_Level_4;
    [SerializeField] Button panel_Level_5;
    [SerializeField] Button panel_Level_6;
    [SerializeField] Button panel_Level_Void;


    //--------------------


    public void ChangePanelBackgroundColor(Color color)
    {
        ColorBlock cb = new ColorBlock();
        cb.colorMultiplier = 1;
        cb.normalColor = Color.white;

        cb.selectedColor = color;
        cb.highlightedColor = color;
        cb.pressedColor = color;
        cb.disabledColor = color;

        panel_Level_1.colors = cb;
        panel_Level_2.colors = cb;
        panel_Level_3.colors = cb;
        panel_Level_4.colors = cb;
        panel_Level_5.colors = cb;
        panel_Level_6.colors = cb;
        panel_Level_Void.colors = cb;
    }
}
