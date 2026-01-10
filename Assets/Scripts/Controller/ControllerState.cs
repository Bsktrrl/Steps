using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XInput;

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
            SetFromGamepad(gamepad);
        }
        else if (device is Keyboard || device is Mouse)
        {
            SetInput(InputType.Keyboard);
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad)
        {
            // If a controller is removed, fall back to keyboard
            if (change == InputDeviceChange.Removed ||
                change == InputDeviceChange.Disconnected)
            {
                RefreshFromConnectedDevices();
            }
        }
    }


    // -------------------- Helpers --------------------


    private void RefreshFromConnectedDevices()
    {
        if (Gamepad.current != null)
            SetFromGamepad(Gamepad.current);
        else
            SetInput(InputType.Keyboard);
    }

    private void SetFromGamepad(Gamepad gamepad)
    {
        if (gamepad is XInputController)
            SetInput(InputType.Xbox);
        else if (gamepad is DualShockGamepad)
            SetInput(InputType.PlayStation);
        else
            SetInput(InputType.Xbox); // default fallback
    }

    private void SetInput(InputType type)
    {
        activeController = type;
    }
}

public enum InputType
{
    Keyboard,
    Xbox,
    PlayStation
}

