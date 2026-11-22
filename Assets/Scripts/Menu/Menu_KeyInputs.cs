using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Menu_KeyInputs : Singleton<Menu_KeyInputs>
{
    public static event Action Action_MenuNavigationUp_isPressed;
    public static event Action Action_MenuNavigationDown_isPressed;
    public static event Action Action_MenuNavigationLeft_isPressed;
    public static event Action Action_MenuNavigationRight_isPressed;

    public static event Action Action_MenuSettingsNavigationUp_isPressed;
    public static event Action Action_MenuSettingsNavigationDown_isPressed;
    public static event Action Action_MenuSettingsNavigationLeft_isPressed;
    public static event Action Action_MenuSettingsNavigationRight_isPressed;

    [Header("Input System")]
    public PlayerControls playerControls;

    [Header("Hold Settings")]
    [SerializeField] float holdDelay = 0.2f;
    [SerializeField] float holdRepeatRate = 0.1f;

    MenuManager optionsManager;
    PlayerInput playerInput;

    InputAction leftAction;
    InputAction rightAction;
    InputAction shiftLeftAction;
    InputAction shiftRightAction;

    MenuCategorySelected shiftLeftRow;
    MenuCategorySelected shiftRightRow;

    bool leftWasPressed;
    bool rightWasPressed;

    float leftHoldTimer;
    float rightHoldTimer;

    bool leftHoldStarted;
    bool rightHoldStarted;


    //--------------------


    private void Start()
    {
        playerControls = new PlayerControls();

        optionsManager = FindObjectOfType<MenuManager>();

        playerInput = GetComponent<PlayerInput>();

        if (playerInput != null && playerInput.actions != null)
        {
            leftAction = playerInput.actions.FindAction("MenuNavigation_Left");
            rightAction = playerInput.actions.FindAction("MenuNavigation_Right");
            shiftLeftAction = playerInput.actions.FindAction("OptionsMenuShift_Left");
            shiftRightAction = playerInput.actions.FindAction("OptionsMenuShift_Right");
        }
    }

    private void Update()
    {
        HandleLeftInput();
        HandleRightInput();
    }


    //--------------------


    void OnMenuNavigation_Up()
    {
        // Existing up logic if needed later.
    }

    void OnMenuNavigation_Down()
    {
        // Existing down logic if needed later.
    }

    void OnMenuNavigation_Left()
    {
        // Do nothing here.
        // Left input is handled safely in Update() instead.
    }

    void OnMenuNavigation_Right()
    {
        // Do nothing here.
        // Right input is handled safely in Update() instead.
    }


    //--------------------
    // LEFT / RIGHT HOLD INPUT
    //--------------------


    void HandleLeftInput()
    {
        bool shiftPressedThisFrame = HandleMenuShift(shiftLeftAction, true, ref shiftLeftRow);
        bool leftIsPressed = (leftAction != null && leftAction.IsPressed()) ||
                             (shiftLeftRow && shiftLeftAction.IsPressed());
        bool leftPressedThisFrame = (leftAction != null && leftAction.WasPressedThisFrame()) ||
                                    shiftPressedThisFrame;

        if (leftIsPressed || leftPressedThisFrame)
        {
            if (!leftWasPressed || leftPressedThisFrame)
            {
                leftWasPressed = leftIsPressed;
                leftHoldTimer = 0f;
                leftHoldStarted = false;

                InvokeMenuNavigationLeft();
            }
            else
            {
                HandleLeftHold();
            }
        }
        else
        {
            ResetLeftHold();
        }
    }

    void HandleRightInput()
    {
        bool shiftPressedThisFrame = HandleMenuShift(shiftRightAction, false, ref shiftRightRow);
        bool rightIsPressed = (rightAction != null && rightAction.IsPressed()) ||
                              (shiftRightRow && shiftRightAction.IsPressed());
        bool rightPressedThisFrame = (rightAction != null && rightAction.WasPressedThisFrame()) ||
                                     shiftPressedThisFrame;

        if (rightIsPressed || rightPressedThisFrame)
        {
            if (!rightWasPressed || rightPressedThisFrame)
            {
                rightWasPressed = rightIsPressed;
                rightHoldTimer = 0f;
                rightHoldStarted = false;

                InvokeMenuNavigationRight();
            }
            else
            {
                HandleRightHold();
            }
        }
        else
        {
            ResetRightHold();
        }
    }


    //--------------------

    bool HandleMenuShift(InputAction action, bool left, ref MenuCategorySelected heldRow)
    {
        var eventSystem = EventSystem.current;
        if (action == null || !eventSystem || !optionsManager ||
            (!action.IsPressed() && !action.WasPressedThisFrame()))
        {
            heldRow = null;
            return false;
        }

        GameObject selected = eventSystem.currentSelectedGameObject;
        if (!action.WasPressedThisFrame())
        {
            if (heldRow && heldRow.gameObject != selected)
                heldRow = null;
            return false;
        }

        heldRow = null;
        GameObject panel = optionsManager.currentMenuCategorySelected switch
        {
            MenuCategories.Settings => optionsManager.settingsMenuParent,
            MenuCategories.Audio => optionsManager.audioMenuParent,
            _ => null
        };
        if (panel && selected && selected.activeInHierarchy && selected.transform.IsChildOf(panel.transform) &&
            selected.TryGetComponent(out heldRow))
            return true;

        optionsManager.ShiftTab(eventSystem, left);
        return false;
    }

    //--------------------


    void HandleLeftHold()
    {
        leftHoldTimer += Time.unscaledDeltaTime;

        if (!leftHoldStarted)
        {
            if (leftHoldTimer >= holdDelay)
            {
                leftHoldStarted = true;
                leftHoldTimer = 0f;

                InvokeMenuNavigationLeft();
            }
        }
        else
        {
            if (leftHoldTimer >= holdRepeatRate)
            {
                leftHoldTimer = 0f;

                InvokeMenuNavigationLeft();
            }
        }
    }

    void HandleRightHold()
    {
        rightHoldTimer += Time.unscaledDeltaTime;

        if (!rightHoldStarted)
        {
            if (rightHoldTimer >= holdDelay)
            {
                rightHoldStarted = true;
                rightHoldTimer = 0f;

                InvokeMenuNavigationRight();
            }
        }
        else
        {
            if (rightHoldTimer >= holdRepeatRate)
            {
                rightHoldTimer = 0f;

                InvokeMenuNavigationRight();
            }
        }
    }


    //--------------------


    void ResetLeftHold()
    {
        leftWasPressed = false;
        leftHoldTimer = 0f;
        leftHoldStarted = false;
    }

    void ResetRightHold()
    {
        rightWasPressed = false;
        rightHoldTimer = 0f;
        rightHoldStarted = false;
    }


    //--------------------


    void InvokeMenuNavigationLeft()
    {
        if (shiftLeftRow)
        {
            PressSettingsArrow(shiftLeftRow.settingState, true);
        }
        else if (IsSettingsCategorySelected())
        {
            Action_MenuSettingsNavigationLeft_isPressed?.Invoke();
        }
    }

    void InvokeMenuNavigationRight()
    {
        if (shiftRightRow)
        {
            PressSettingsArrow(shiftRightRow.settingState, false);
        }
        else if (IsSettingsCategorySelected())
        {
            Action_MenuSettingsNavigationRight_isPressed?.Invoke();
        }
    }

    public static void PressSettingsArrow(SettingState state, bool left)
    {
        SettingsManager.Instance.settingState = state;
        if (left)
            Action_MenuSettingsNavigationLeft_isPressed?.Invoke();
        else
            Action_MenuSettingsNavigationRight_isPressed?.Invoke();
    }

    bool IsSettingsCategorySelected()
    {
        return optionsManager &&
        (
            optionsManager.currentMenuCategorySelected == MenuCategories.Settings ||
            optionsManager.currentMenuCategorySelected == MenuCategories.Controls ||
            optionsManager.currentMenuCategorySelected == MenuCategories.Video ||
            optionsManager.currentMenuCategorySelected == MenuCategories.Audio
        );
    }
}
