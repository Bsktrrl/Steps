using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [Header("Levels")]
    public string levelToPlay;


    //--------------------


    public void LoadSceneAsync()
    {
        if (!string.IsNullOrEmpty(levelToPlay))
        {
            StartCoroutine(LoadSceneCoroutine(levelToPlay));
        }
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            Debug.Log($"Loading progress: {operation.progress * 100}%");
            yield return null;
        }
    }
}
