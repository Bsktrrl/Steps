using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu_ExitLevel_Button : MonoBehaviour
{
    public static event Action Action_ExitLevel;

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
        Action_ExitLevel?.Invoke();

        StartCoroutine(ExitLevelCoroutine());
    }

    IEnumerator ExitLevelCoroutine()
    {
        yield return mapManager.FadeInBlackScreenCoroutine(MapManager.Instance.blackScreen.GetComponent<Image>());
        yield return mapManager.FadeInBlackScreenCoroutine(MapManager.Instance.blackScreen.GetComponent<LoadingIcon>().loadingIcon.GetComponent<Image>());

        MapStatsGathered.Instance.ExitLevel();

        yield return new WaitForSeconds(1f);

        PlayerManager.Instance.QuitLevel();

        yield return null;
    }
}
