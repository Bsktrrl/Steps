using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu_ResetLevel : MonoBehaviour
{
    public void ResetLevel()
    {
        Scene scene = gameObject.scene;
        MapManager map = FindAnyObjectByType<MapManager>();
        if (!map || map.gameObject.scene != scene || !CanReload(scene))
            return;

        DataPersistanceManager persistence = DataPersistanceManager.instance;
        if (!persistence || !persistence.TryBeginSceneChange())
            return;

        if (TryGetComponent(out ButtonSound sound))
            sound.TryButtonPressSound();

        // The map stays active while the pause menu is transitioning.
        map.StartCoroutine(ReloadLevel(map, scene, persistence));
    }

    private static bool CanReload(Scene scene)
    {
        if (scene.buildIndex >= 0)
            return Application.CanStreamedLevelBeLoaded(scene.buildIndex);

#if UNITY_EDITOR
        return !string.IsNullOrEmpty(scene.path);
#else
        return false;
#endif
    }

    private static IEnumerator ReloadLevel(MapManager map, Scene scene, DataPersistanceManager persistence)
    {
        yield return map.StartFadeInBlackScreen();

        Player_Statistics_Manager.Active?.FinishLevelAttempt();
        yield return persistence.SaveBeforeSceneChange(FeedbackOutcome.Reset);

        EventSystem eventSystem = EventSystem.current;
        if (eventSystem)
        {
            eventSystem.SetSelectedGameObject(null);
            eventSystem.enabled = false;
        }

        GameObject player = MapManager.SpawnedPlayer;
        if (player)
        {
            FreeCam freeCam = player.GetComponentInChildren<FreeCam>();
            if (freeCam)
                freeCam.PrepareForLevelReload();
        }

#if UNITY_EDITOR
        if (scene.buildIndex < 0)
        {
            yield return UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(
                scene.path, new LoadSceneParameters(LoadSceneMode.Single));
            yield break;
        }
#endif
        yield return SceneManager.LoadSceneAsync(scene.buildIndex, LoadSceneMode.Single);
    }
}
