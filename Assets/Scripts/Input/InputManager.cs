﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : IGameService
{
    private InputControls input;

    public InputAction P_LeftClick { get; private set; }
    public InputAction P_RightClick { get; private set; }
    public InputAction P_MouseDelta { get; private set; }
    public InputAction P_MousePosition { get; private set; }

    public InputManager()
    {
        input = new InputControls();
        input.Player.Enable();

        P_LeftClick = input.Player.LeftClick;
        P_RightClick = input.Player.RightClick;
        P_MouseDelta = input.Player.MouseDelta;
        P_MousePosition = input.Player.MousePosition;
    }

    public void ToggleInput(bool toggle)
    {
        if (toggle)
            input.Enable();
        else
            input.Disable();
    }
}
