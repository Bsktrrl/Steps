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
    private InputSystemUIInputModule uiInputModule;
    private InputActionAsset navigationActions;
    private InputActionReference originalMove;
    private InputActionReference menuMove;
    private int rightStickBindingIndex;


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

            uiInputModule = inputModule;
            InstallMenuNavigation();
            SetWardrobePreviewActive(FindObjectOfType<SkinWardrobeManager>() != null);
        }
    }

    private void OnEnable()
    {
        SkinWardrobeManager.PreviewVisibilityChanged += SetWardrobePreviewActive;
    }

    private void OnDisable()
    {
        SkinWardrobeManager.PreviewVisibilityChanged -= SetWardrobePreviewActive;
    }

    private void OnDestroy()
    {
        if (uiInputModule && uiInputModule.move == menuMove)
            uiInputModule.move = originalMove;
        if (menuMove) Destroy(menuMove);
        if (navigationActions) Destroy(navigationActions);
    }

    private void InstallMenuNavigation()
    {
        navigationActions = ScriptableObject.CreateInstance<InputActionAsset>();
        InputActionMap ui = navigationActions.AddActionMap("Menu Navigation");
        InputAction move = ui.AddAction("Move", InputActionType.Value, expectedControlLayout: "Vector2");
        move.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w").With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s").With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a").With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d").With("Right", "<Keyboard>/rightArrow");
        move.AddBinding("<Gamepad>/dpad");
        move.AddBinding("<Gamepad>/leftStick", processors: "stickDeadzone(min=0.25)");
        move.AddBinding("<Joystick>/stick", processors: "stickDeadzone(min=0.25)");
        rightStickBindingIndex = move.bindings.Count;
        move.AddBinding("<Gamepad>/rightStick", processors: "stickDeadzone(min=0.25)");

        originalMove = uiInputModule.move;
        menuMove = InputActionReference.Create(move);
        uiInputModule.move = menuMove;
    }

    private void SetWardrobePreviewActive(bool active)
    {
        if (!menuMove) return;

        // An empty override disables this binding while the stick controls the preview.
        if (active)
            menuMove.action.ApplyBindingOverride(rightStickBindingIndex, string.Empty);
        else
            menuMove.action.RemoveBindingOverride(rightStickBindingIndex);
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
