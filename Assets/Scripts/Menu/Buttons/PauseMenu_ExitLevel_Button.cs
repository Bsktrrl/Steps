using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu_ExitLevel_Button : MonoBehaviour
{
    public void ExitLevelButton_isPressed()
    {
        Player_Movement.Instance.QuitLevel();
    }
}
