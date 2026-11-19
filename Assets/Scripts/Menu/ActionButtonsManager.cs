using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class ActionButtonsManager : Singleton<ActionButtonsManager>
{
    [Header("Event System")]
    public EventSystem eventSystem;

    [Header("Button Navigation")]
    public InputActionReference cancel_Button;

    [Header("Input System")]
    PlayerControls playerControls;
    MainMenuManager mainMenuManager;


    //--------------------


    private void Reset()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Start()
    {
        playerControls = new PlayerControls();
        mainMenuManager = FindObjectOfType<MainMenuManager>();

        if (eventSystem && eventSystem.TryGetComponent(out InputSystemUIInputModule inputModule))
        {
            InputAction submit = inputModule.submit?.action;
            if (submit != null)
                AddSpaceSubmitBinding(submit);
        }
    }

    private static void AddSpaceSubmitBinding(InputAction submit)
    {
        const string spacePath = "<Keyboard>/space";
        foreach (InputBinding binding in submit.bindings)
        {
            if (binding.path == spacePath)
                return;
        }

        submit.AddBinding(spacePath, groups: "Keyboard&Mouse");
    }

    //--------------------


    void OnMenu_Back()
    {
        //print("1. Back Button is Pressed");
    }
}
