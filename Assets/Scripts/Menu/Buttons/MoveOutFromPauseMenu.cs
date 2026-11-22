using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveOutFromPauseMenu : MonoBehaviour
{
    public static event Action Action_MoveOutFromPauseMenu;

    [Header("Animator - Closing Animation")]
    [SerializeField] List<Animator> closingPauseMenuAnimatorList = new List<Animator>();
    float closingMenuDelay = 0.2f;

    bool firstTimeEnter;
    PauseMenuManager pauseMenu;
    ActionButtonsManager buttons;


    //--------------------


    void Update()
    {
        if (firstTimeEnter)
        {
            firstTimeEnter = false;
            return;
        }

        EventSystem eventSystem = EventSystem.current;
        if (!eventSystem || !eventSystem.isActiveAndEnabled || eventSystem.currentSelectedGameObject != gameObject)
            return;

        if (!pauseMenu)
            pauseMenu = FindAnyObjectByType<PauseMenuManager>();
        if (!IsPauseMenuOpen())
            return;

        if (!buttons)
            buttons = FindAnyObjectByType<ActionButtonsManager>();
        if (!buttons || !buttons.cancel_Button || buttons.cancel_Button.action?.WasPressedThisFrame() != true)
            return;

        StartCoroutine(CloseMenuDelay(closingMenuDelay));
    }

    private void OnEnable()
    {
        firstTimeEnter = true;
    }


    //--------------------


    IEnumerator CloseMenuDelay(float waitTime)
    {
        for (int i = 0; i < closingPauseMenuAnimatorList.Count; i++)
        {
            if (closingPauseMenuAnimatorList[i])
                closingPauseMenuAnimatorList[i].SetTrigger("Close");
        }

        Action_MoveOutFromPauseMenu?.Invoke();

        yield return new WaitForSeconds(waitTime);

        EventSystem eventSystem = EventSystem.current;
        if (!eventSystem || !eventSystem.isActiveAndEnabled || !IsPauseMenuOpen() || !pauseMenu.isVisible)
            yield break;

        PlayerManager player = FindAnyObjectByType<PlayerManager>();
        if (player && player.pauseGame)
            pauseMenu.ClosePauseMenu();
    }

    bool IsPauseMenuOpen() =>
        pauseMenu && pauseMenu.isActive && pauseMenu.pauseMenu_Parent && pauseMenu.pauseMenu_Parent.activeInHierarchy;
}
