using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAlpha : MonoBehaviour
{
    public Image[] theButtons;

    private void OnEnable()
    {
        for (int i = 0; i < theButtons.Length; i++)
        {
            theButtons[i].alphaHitTestMinimumThreshold = 1f;
        }
    }
}
