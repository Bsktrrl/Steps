using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseMenuManager : Singleton<PauseMenuManager>
{
    public GameObject pauseMenu_Parent;

    public GameObject pauseMenu_MainMenu_Parent;
    public GameObject pauseMenu_Settings_Parent;
    public GameObject pauseMenu_Info_Parent;

    public GameObject pauseMenu_StartButton;

    //[SerializeField] TextMeshProUGUI glueplantAmount;


    //--------------------


    public void OpenPauseMenu()
    {
        pauseMenu_Parent.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        pauseMenu_Parent.SetActive(false);
    }
}
