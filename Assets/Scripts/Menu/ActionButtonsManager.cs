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
    PauseMenuManager pauseMenuManager;
    private InputSystemUIInputModule uiInputModule;
    private InputActionAsset navigationActions;
    private InputActionReference originalMove;
    private InputActionReference menuMove;
    private int rightStickBindingIndex;
    private Vector2 lastMousePosition;
    private static bool menuCursorRequested;
    private int controllerInputFrame = -1;


    //--------------------


    private void Reset()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetCursorInput()
    {
        menuCursorRequested = false;
    }

    private void Start()
    {
        playerControls = new PlayerControls();
        mainMenuManager = FindObjectOfType<MainMenuManager>();
        pauseMenuManager = FindAnyObjectByType<PauseMenuManager>();

        if (!mainMenuManager)
            ResetCursorInput();

        Cursor.lockState = CursorLockMode.None;
        lastMousePosition = Mouse.current?.position.ReadValue() ?? Vector2.zero;
        UpdateCursorVisibility(Mouse.current);

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
        bool controllerNavigating = controllerInputFrame == Time.frameCount || HasHeldControllerNavigation();
        if (controllerNavigating)
            menuCursorRequested = false;

        bool cursorVisible = UpdateCursorVisibility(mouse);
        if (mouse == null || !Application.isFocused) return;

        Vector2 position = mouse.position.ReadValue();
        bool moved = position != lastMousePosition || mouse.delta.ReadValue() != Vector2.zero;
        lastMousePosition = position;
        // Consume the pointer position even when controller input wins this frame.
        if (controllerNavigating || !cursorVisible ||
            (DataPersistanceManager.GameplayInputBlocked && !DataPersistanceManager.SaveDialogOpen))
            return;

        bool clicked = mouse.leftButton.wasPressedThisFrame || mouse.rightButton.wasPressedThisFrame ||
                       mouse.middleButton.wasPressedThisFrame;

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

    private bool IsMenuOpen => DataPersistanceManager.SaveDialogOpen || mainMenuManager ||
        (pauseMenuManager && pauseMenuManager.isActive);

    private bool HasHeldControllerNavigation()
    {
        if (!IsMenuOpen || !eventSystem || !uiInputModule || !uiInputModule.isActiveAndEnabled ||
            eventSystem.currentInputModule != uiInputModule || !menuMove)
            return false;

        // The UI module can repeat navigation during a hold without a fresh input event.
        InputAction move = menuMove.action;
        return move?.activeControl?.device is Gamepad or Joystick && move.ReadValue<Vector2>() != Vector2.zero;
    }

    private bool UpdateCursorVisibility(Mouse mouse)
    {
        bool visible = mouse != null && Application.isFocused && IsMenuOpen &&
            (!ControllerState.HasConnectedController || menuCursorRequested);
        if (Cursor.visible != visible)
            Cursor.visible = visible;
        return visible;
    }

    private void OnMenuInputUsed(InputDevice device)
    {
        if (device is Gamepad || device is Joystick)
        {
            controllerInputFrame = Time.frameCount;
            menuCursorRequested = false;
        }
        else if (controllerInputFrame != Time.frameCount && (device is Keyboard || (device is Mouse && IsMenuOpen)))
            menuCursorRequested = true;
    }

    internal static bool CanSelectWithMouse(Button button)
    {
        // Settings rows are already keyboard destinations. Their child arrows perform the edit.
        return button && button.IsActive() &&
            (button.IsInteractable() || (!button.interactable && button.GetComponent<MenuCategorySelected>()));
    }

    private void OnEnable()
    {
        ControllerState.MenuInputUsed += OnMenuInputUsed;
        SkinWardrobeManager.PreviewVisibilityChanged += SetWardrobePreviewActive;
    }

    private void OnDisable()
    {
        ControllerState.MenuInputUsed -= OnMenuInputUsed;
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
            .With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d");
        move.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow").With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow").With("Right", "<Keyboard>/rightArrow");
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
