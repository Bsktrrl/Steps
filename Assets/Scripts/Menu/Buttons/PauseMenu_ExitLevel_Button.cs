using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu_ExitLevel_Button : MonoBehaviour
{
    MapManager mapManager;

    private void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
    }

    public void ExitLevelButton_isPressed()
    {
        //Update analyticsData
        AnalyticsCalls.OnLevel(mapManager.timeUsedInLevel, mapManager.stepCount, mapManager.respawnCount, mapManager.abilitiesPickedUp, mapManager.cameraRotated, mapManager.swimCounter, mapManager.swiftSwimCounter, mapManager.jumpCounter, mapManager.dashCounter, mapManager.ascendCounter, mapManager.descendCounter, mapManager.grapplingHookCounter, mapManager.ceilingGrabCounter);
        AnalyticsCalls.OnLevelExit(mapManager.timeUsedInLevel, mapManager.stepCount, mapManager.respawnCount, mapManager.abilitiesPickedUp, mapManager.cameraRotated, mapManager.swimCounter, mapManager.swiftSwimCounter, mapManager.jumpCounter, mapManager.dashCounter, mapManager.ascendCounter, mapManager.descendCounter, mapManager.grapplingHookCounter, mapManager.ceilingGrabCounter);

        PlayerManager.Instance.QuitLevel();
    }
}
