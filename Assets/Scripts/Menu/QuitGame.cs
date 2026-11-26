using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public static event Action Action_QuitGame;

    public void QuitGameFunction()
    {
        if (DataPersistanceManager.GameplayInputBlocked)
            return;

        Action_QuitGame?.Invoke();
        Application.Quit();
    }
}
