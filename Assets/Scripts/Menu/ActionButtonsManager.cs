using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class ActionButtonsManager : Singleton<ActionButtonsManager>
{
    internal static event System.Action<Button> MouseHoverUpdated;

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
    private const float CursorIdleSeconds = 3f;
    private Vector2 lastMousePosition;
    private float lastMouseActivity;


    //--------------------


    private void Reset()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Start()
    {
        playerControls = new PlayerControls();
        mainMenuManager = FindObjectOfType<MainMenuManager>();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = mainMenuManager && Mouse.current != null;
        lastMousePosition = Mouse.current?.position.ReadValue() ?? Vector2.zero;
        lastMouseActivity = Time.unscaledTime;

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

    private void LateUpdate()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null || !Application.isFocused) return;

        Vector2 position = mouse.position.ReadValue();
        bool moved = position != lastMousePosition || mouse.delta.ReadValue() != Vector2.zero;
        lastMousePosition = position;
        bool clicked = mouse.leftButton.wasPressedThisFrame || mouse.rightButton.wasPressedThisFrame ||
                       mouse.middleButton.wasPressedThisFrame;

        if (moved || clicked || mouse.scroll.ReadValue() != Vector2.zero)
        {
            lastMouseActivity = Time.unscaledTime;
            Cursor.visible = true;
        }
        else if (Time.unscaledTime - lastMouseActivity >= CursorIdleSeconds)
        {
            Cursor.visible = false;
        }

        bool released = mouse.leftButton.wasReleasedThisFrame;
        if ((!moved && !clicked && !released) ||
            !uiInputModule || !uiInputModule.isActiveAndEnabled || eventSystem.currentInputModule != uiInputModule)
            return;

        RaycastResult hit = uiInputModule.GetLastRaycastResult(mouse.deviceId);
        Button target = hit.module is GraphicRaycaster && hit.gameObject
            ? hit.gameObject.GetComponentInParent<Button>()
            : null;
        if (target && target.TryGetComponent(out OptionSelectAnimation arrow))
            target = arrow.SelectionButton;

        // A stationary mouse must not take focus back from keyboard/controller navigation.
        if (moved && !clicked && !mouse.leftButton.isPressed && !released &&
            CanSelectWithMouse(target) && eventSystem.currentSelectedGameObject != target.gameObject)
            eventSystem.SetSelectedGameObject(target.gameObject);

        MouseHoverUpdated?.Invoke(target);
    }

    internal static bool CanSelectWithMouse(Button button)
    {
        // Settings rows are already keyboard destinations. Their child arrows perform the edit.
        return button && button.IsActive() &&
            (button.IsInteractable() || (!button.interactable && button.GetComponent<MenuCategorySelected>()));
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
