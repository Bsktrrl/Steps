using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public static event Action Action_QuitGame;

    public void QuitGameFunction()
    {
        Action_QuitGame?.Invoke();
        Application.Quit();
    }
}
