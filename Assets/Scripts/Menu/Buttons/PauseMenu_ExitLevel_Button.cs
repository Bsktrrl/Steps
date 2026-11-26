using System;
using System.Collections;
using UnityEngine;

public class PauseMenu_ExitLevel_Button : MonoBehaviour
{
    public static event Action Action_ExitLevel;

    MapManager mapManager;

    private void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
    }

    public void ExitLevelButton_isPressed()
    {
        GameObject playerObject = MapManager.SpawnedPlayer;
        if (!mapManager || !playerObject || !playerObject.TryGetComponent(out PlayerManager player) ||
            !player.TryBeginLevelExit())
            return;

        Action_ExitLevel?.Invoke();

        mapManager.StartCoroutine(ExitLevelCoroutine(player));
    }

    IEnumerator ExitLevelCoroutine(PlayerManager player)
    {
        yield return mapManager.StartFadeInBlackScreen();

        MapStatsGathered.Instance.ExitLevel();

        yield return new WaitForSecondsRealtime(1f);

        player.FinishLevelExit();

        yield return null;
    }
}
