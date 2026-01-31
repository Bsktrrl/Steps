using System;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEditor;

public class NewGameButton : MonoBehaviour
{
    public static event Action Action_PressNewGameButton;

    [SerializeField] GameObject continueButton;

    MenuLevelInfo menuLevelInfo;


    //--------------------


    private void Start()
    {
        menuLevelInfo = FindAnyObjectByType<MenuLevelInfo>();
    }


    //--------------------


    public void NewGameButton_isPressed()
    {
        string path = Path.Combine(Application.persistentDataPath, DataPersistanceManager.instance.fileName);

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted");
        }

        DataPersistanceManager.instance.NewGame();

        if (menuLevelInfo && menuLevelInfo.mapInfo_ToSave != null)
        {
            menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List.Clear();
        }
        
        GetComponent<MainMenuButton>().SetPassive();
        continueButton.GetComponent<MainMenuButton>().SetActive();

        Action_PressNewGameButton?.Invoke();
    }
}
