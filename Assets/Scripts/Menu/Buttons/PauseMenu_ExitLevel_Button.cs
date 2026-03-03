using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu_ExitLevel_Button : MonoBehaviour
{
    MapManager mapManager;
    MapStatsGathered mapStatsGathered;


    //--------------------


    private void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        mapStatsGathered = FindObjectOfType<MapStatsGathered>();
    }

    public void ExitLevelButton_isPressed()
    {
        StartCoroutine(ExitLevelCoroutine());
    }

    IEnumerator ExitLevelCoroutine()
    {
        yield return mapManager.FadeInBlackScreenCoroutine();

        MapStatsGathered.Instance.ExitLevel();

        yield return new WaitForEndOfFrame();

        PlayerManager.Instance.QuitLevel();

        yield return null;
    }
}
