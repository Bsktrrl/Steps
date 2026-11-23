using System;
using UnityEngine;

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
        if (!DataPersistanceManager.instance.NewGame())
            return;

        if (menuLevelInfo && menuLevelInfo.mapInfo_ToSave != null)
        {
            menuLevelInfo.mapInfo_ToSave.map_SaveInfo_List.Clear();
        }
        
        GetComponent<MainMenuButton>().SetPassive();
        continueButton.GetComponent<MainMenuButton>().SetActive();

        Action_PressNewGameButton?.Invoke();
    }
}
