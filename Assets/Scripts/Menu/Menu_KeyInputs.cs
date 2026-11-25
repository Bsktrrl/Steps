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

    struct HorizontalHold
    {
        public MenuCategorySelected row;
        public float elapsed;
        public bool repeating;
    }

    HorizontalHold leftHold;
    HorizontalHold rightHold;


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
        EventSystem eventSystem = EventSystem.current;
        if (!eventSystem || !optionsManager)
            return;

        bool shiftLeft = shiftLeftAction?.WasPressedThisFrame() == true;
        if (shiftLeft || shiftRightAction?.WasPressedThisFrame() == true)
        {
            optionsManager.ShiftTab(eventSystem, shiftLeft);
            leftHold = default;
            rightHold = default;
            return;
        }

        GameObject selected = eventSystem.currentSelectedGameObject;
        HandleSettingsArrow(leftAction, selected, true, ref leftHold);
        HandleSettingsArrow(rightAction, selected, false, ref rightHold);
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


    void HandleSettingsArrow(InputAction action, GameObject selected, bool left, ref HorizontalHold hold)
    {
        if (action == null ||
            optionsManager.currentMenuCategorySelected is not (MenuCategories.Settings or MenuCategories.Audio) ||
            !optionsManager.TryGetSelectedOptionRow(selected, out MenuCategorySelected row))
        {
            hold = default;
            return;
        }

        if (action.WasPressedThisFrame())
        {
            hold = new HorizontalHold { row = row };
            PressSettingsArrow(row.settingState, left);
            return;
        }

        if (hold.row != row || !action.IsPressed())
        {
            hold = default;
            return;
        }

        hold.elapsed += Time.unscaledDeltaTime;
        if (hold.elapsed < (hold.repeating ? holdRepeatRate : holdDelay))
            return;

        hold.elapsed = 0f;
        hold.repeating = true;
        PressSettingsArrow(row.settingState, left);
    }

    public static void PressSettingsArrow(SettingState state, bool left)
    {
        SettingsManager.Instance.settingState = state;
        if (left)
            Action_MenuSettingsNavigationLeft_isPressed?.Invoke();
        else
            Action_MenuSettingsNavigationRight_isPressed?.Invoke();
    }

}
