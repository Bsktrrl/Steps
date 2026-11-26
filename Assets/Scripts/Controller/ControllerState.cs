using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem.Controls;

public class ControllerState : Singleton<ControllerState>
{
    internal static event Action<InputDevice> MenuInputUsed;
    internal static event Action<InputType> ActiveInputChanged;
    internal static bool HasConnectedController => Gamepad.all.Count > 0 || Joystick.all.Count > 0;

    public InputType activeController { get; private set; } = InputType.Keyboard;


    //--------------------


    private void OnEnable()
    {
        InputSystem.onEvent += OnInputEvent;
        InputSystem.onDeviceChange += OnDeviceChange;

        RefreshFromConnectedDevices();
    }

    private void OnDisable()
    {
        InputSystem.onEvent -= OnInputEvent;
        InputSystem.onDeviceChange -= OnDeviceChange;
    }


    // -------------------- Input detection --------------------


    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if (eventPtr.type != StateEvent.Type && eventPtr.type != DeltaStateEvent.Type)
            return;

        if (device is Gamepad gamepad)
        {
            if (!GamepadHasRealInput(eventPtr, gamepad))
                return;

            RefreshFromConnectedDevices();
            MenuInputUsed?.Invoke(gamepad);
            return;
        }

        if (device is Joystick joystick)
        {
            if (HasNewButtonPress(eventPtr, joystick) || HasDirectionInput(eventPtr, joystick.stick, 0.0625f))
                MenuInputUsed?.Invoke(joystick);
            return;
        }

        if (device is Keyboard keyboard)
        {
            if (!HasNewButtonPress(eventPtr, keyboard))
                return;

            SetInput(InputType.Keyboard);
            MenuInputUsed?.Invoke(keyboard);
            return;
        }

        if (device is Mouse mouse)
        {
            if (mouse.delta.ReadValueFromEvent(eventPtr) != Vector2.zero ||
                (mouse.position.ReadValueFromEvent(eventPtr, out Vector2 position) &&
                 position != mouse.position.ReadValue()))
                MenuInputUsed?.Invoke(mouse);

            if (HasNewButtonPress(eventPtr, mouse) || mouse.scroll.ReadValueFromEvent(eventPtr).sqrMagnitude > 0.01f)
                SetInput(InputType.Keyboard);
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        bool connected = change is InputDeviceChange.Added or InputDeviceChange.Reconnected;
        bool disconnected = change is InputDeviceChange.Removed or InputDeviceChange.Disconnected;
        if (device is Gamepad && (connected || disconnected))
            RefreshFromConnectedDevices();

        if (connected && (device is Gamepad || device is Joystick))
            MenuInputUsed?.Invoke(device);
    }


    // -------------------- Helpers --------------------


    private void RefreshFromConnectedDevices()
    {
        if (Gamepad.all.Count <= 0)
        {
            SetInput(InputType.Keyboard);
            return;
        }

        // If Unity sees any PlayStation controller, prefer PlayStation.
        // This prevents flickering if the same physical controller also appears as XInput.
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (IsPlayStationGamepad(Gamepad.all[i]))
            {
                SetInput(InputType.PlayStation);
                return;
            }
        }

        // Every other connected gamepad uses the Xbox display.
        SetInput(InputType.Xbox);
    }

    private void SetFromGamepad(Gamepad gamepad)
    {
        if (gamepad == null)
        {
            SetInput(InputType.Keyboard);
            return;
        }

        if (IsPlayStationGamepad(gamepad))
        {
            SetInput(InputType.PlayStation);
            return;
        }

        if (IsXboxGamepad(gamepad))
        {
            SetInput(InputType.Xbox);
            return;
        }

        SetInput(InputType.Xbox);
    }

    private void SetInput(InputType type)
    {
        if (activeController == type)
            return;

        activeController = type;
        ActiveInputChanged?.Invoke(type);

        Debug.Log("Active controller changed to: " + activeController);
    }

    private bool IsPlayStationGamepad(Gamepad gamepad)
    {
        if (gamepad == null)
            return false;

        if (gamepad is DualShockGamepad)
            return true;

        string layout = gamepad.layout.ToLower();
        string name = gamepad.name.ToLower();
        string displayName = gamepad.displayName.ToLower();
        string description = gamepad.description.ToString().ToLower();

        return layout.Contains("dualshock") ||
               layout.Contains("dualsense") ||
               layout.Contains("playstation") ||
               name.Contains("dualshock") ||
               name.Contains("dualsense") ||
               name.Contains("playstation") ||
               name.Contains("ps4") ||
               name.Contains("ps5") ||
               displayName.Contains("dualshock") ||
               displayName.Contains("dualsense") ||
               displayName.Contains("playstation") ||
               displayName.Contains("ps4") ||
               displayName.Contains("ps5") ||
               description.Contains("dualshock") ||
               description.Contains("dualsense") ||
               description.Contains("playstation") ||
               description.Contains("sony") ||
               description.Contains("ps4") ||
               description.Contains("ps5");
    }
    private bool IsXboxGamepad(Gamepad gamepad)
    {
        if (gamepad == null)
            return false;

        string layout = gamepad.layout.ToLower();
        string name = gamepad.name.ToLower();
        string displayName = gamepad.displayName.ToLower();
        string description = gamepad.description.ToString().ToLower();

        if (gamepad is XInputController)
            return true;

        return layout.Contains("xinput") ||
               layout.Contains("xbox") ||
               name.Contains("xinput") ||
               name.Contains("xbox") ||
               displayName.Contains("xinput") ||
               displayName.Contains("xbox") ||
               description.Contains("xinput") ||
               description.Contains("xbox");
    }
    private static bool GamepadHasRealInput(InputEventPtr eventPtr, Gamepad gamepad)
    {
        return HasNewButtonPress(eventPtr, gamepad) ||
               (gamepad.leftTrigger.ReadValue() <= 0.2f && gamepad.leftTrigger.ReadValueFromEvent(eventPtr) > 0.2f) ||
               (gamepad.rightTrigger.ReadValue() <= 0.2f && gamepad.rightTrigger.ReadValueFromEvent(eventPtr) > 0.2f) ||
               HasDirectionInput(eventPtr, gamepad.dpad, 0.1f) ||
               HasDirectionInput(eventPtr, gamepad.leftStick, 0.0625f) ||
               HasDirectionInput(eventPtr, gamepad.rightStick, 0.0625f);
    }

    private static bool HasNewButtonPress(InputEventPtr eventPtr, InputDevice device)
    {
        foreach (InputControl control in eventPtr.EnumerateChangedControls(device))
            if (control is ButtonControl button && !button.isPressed &&
                button.ReadValueFromEvent(eventPtr) >= button.pressPointOrDefault)
                return true;

        return false;
    }

    private static bool HasDirectionInput(InputEventPtr eventPtr, Vector2Control control, float threshold)
    {
        if (!control.ReadValueFromEvent(eventPtr, out Vector2 value) || value.sqrMagnitude <= threshold)
            return false;

        Vector2 previous = control.ReadValue();
        return previous.sqrMagnitude <= threshold || (value - previous).sqrMagnitude > 0.01f;
    }
}

public enum InputType
{
    Keyboard,
    Xbox,
    PlayStation
}

