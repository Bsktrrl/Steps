using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu_BackToGame_Button : MonoBehaviour
{
    public void BackToGameButton_isPressed()
    {
        PauseMenuManager.Instance.ClosePauseMenu();
        PlayerManager.Instance.pauseGame = false;
    }
}
