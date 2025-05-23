using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class NewGameButton : MonoBehaviour
{
    public void NewGameButton_isPressed()
    {
        string path = Path.Combine(Application.persistentDataPath, DataPersistanceManager.instance.fileName);

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted");
        }

        DataPersistanceManager.instance.NewGame();
    }
}
