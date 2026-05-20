using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem.Controls;

public class ControllerState : Singleton<ControllerState>
{
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
        if (device is Gamepad gamepad)
        {
            if (!GamepadHasRealInput(gamepad))
                return;

            RefreshFromConnectedDevices();
            return;
        }

        if (device is Keyboard keyboard)
        {
            if (!KeyboardHasRealInput(keyboard))
                return;

            SetInput(InputType.Keyboard);
            return;
        }

        if (device is Mouse mouse)
        {
            if (!MouseHasRealInput(mouse))
                return;

            SetInput(InputType.Keyboard);
            return;
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad)
        {
            if (change == InputDeviceChange.Added ||
                change == InputDeviceChange.Removed ||
                change == InputDeviceChange.Disconnected ||
                change == InputDeviceChange.Reconnected)
            {
                RefreshFromConnectedDevices();
            }
        }
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

        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (IsXboxGamepad(Gamepad.all[i]))
            {
                SetInput(InputType.Xbox);
                return;
            }
        }

        // Unknown gamepad fallback.
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

        Debug.Log("Active controller changed to: " + activeController);
    }

    private bool IsPlayStationGamepad(Gamepad gamepad)
    {
        if (gamepad == null)
            return false;

        string layout = gamepad.layout.ToLower();
        string name = gamepad.name.ToLower();
        string displayName = gamepad.displayName.ToLower();
        string description = gamepad.description.ToString().ToLower();

        if (gamepad is DualShockGamepad)
            return true;

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
    private bool GamepadHasRealInput(Gamepad gamepad)
    {
        if (gamepad == null)
            return false;

        if (gamepad.buttonSouth.isPressed ||
            gamepad.buttonNorth.isPressed ||
            gamepad.buttonEast.isPressed ||
            gamepad.buttonWest.isPressed ||
            gamepad.leftShoulder.isPressed ||
            gamepad.rightShoulder.isPressed ||
            gamepad.leftTrigger.ReadValue() > 0.2f ||
            gamepad.rightTrigger.ReadValue() > 0.2f ||
            gamepad.startButton.isPressed ||
            gamepad.selectButton.isPressed ||
            gamepad.dpad.ReadValue().sqrMagnitude > 0.1f ||
            gamepad.leftStick.ReadValue().sqrMagnitude > 0.25f ||
            gamepad.rightStick.ReadValue().sqrMagnitude > 0.25f)
        {
            return true;
        }

        return false;
    }
    private bool KeyboardHasRealInput(Keyboard keyboard)
    {
        if (keyboard == null)
            return false;

        foreach (KeyControl key in keyboard.allKeys)
        {
            if (key.isPressed)
                return true;
        }

        return false;
    }
    private bool MouseHasRealInput(Mouse mouse)
    {
        if (mouse == null)
            return false;

        return mouse.leftButton.isPressed ||
               mouse.rightButton.isPressed ||
               mouse.middleButton.isPressed ||
               mouse.forwardButton.isPressed ||
               mouse.backButton.isPressed ||
               mouse.scroll.ReadValue().sqrMagnitude > 0.01f;
    }
}

public enum InputType
{
    Keyboard,
    Xbox,
    PlayStation
}

